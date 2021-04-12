using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Newtonsoft.Json;
using StrategyBuilder.Model;
using StrategyBuilder.Model.Extensions;
using StrategyBuilder.Repository;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Service
{
    public class BackTestingEngine : BaseService, IBackTestingEngine
    {
        private IStockDataRepo _stockDataRepo;
        private IStrategyService _strategyService;
        private IReportGenerator _reportGenerator;

        public BackTestingEngine(DbContext dbContext, IStockDataRepo stockDataRepo, IStrategyService strategyService, IReportGenerator reportGenerator) : base(dbContext) 
        {
            _stockDataRepo = stockDataRepo;
            _strategyService = strategyService;
            _reportGenerator = reportGenerator;
        }

        public void DeleteBackTestingResult(int resultId)
        {
            var report = _dbContext.Set<BackTestingResult>().FirstOrDefault(x => x.Id == resultId);
            if (report != null)
            {
                _dbContext.Remove(report);
                _dbContext.SaveChanges();
            }
        }

        public async Task Execute(DateTime from, DateTime to, string[] symbolList, int strategyId)
        {
            Strategy strategy = _strategyService.GetStrategiesByStrategyId(strategyId);
            IEnumerable<Event> events = strategy.JoinStrategyEventGroups.Select(j => j.EventGroup).SelectMany(x => x.Events).Distinct();

            int eventdatefrom = -5;
            int eventdateto = 5;
            Dictionary<string, Dictionary<DateTime, StockPriceAdjustDaily>> stockprices = new Dictionary<string, Dictionary<DateTime, StockPriceAdjustDaily>>();
            Dictionary<string, NegativeIndexArray<decimal>> meanresults = new Dictionary<string, NegativeIndexArray<decimal>>();
            foreach (var symbol in symbolList)
            {
                Dictionary<DateTime, StockPriceAdjustDaily> prices = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, symbol);
                stockprices.Add(symbol, prices);

                NegativeIndexArray<decimal> meanresult = await ExecuteEventPriceImpact(from, to, events, stockprices[symbol], eventdatefrom, eventdateto);
                meanresults.Add(symbol, meanresult);
            }

            Dictionary<DateTime, StockPriceAdjustDaily> sp500prices = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, "IVV");

            // Execute porfolio
            decimal cash = 100000; // initial account balance 100,000
            decimal accountValue = cash; 
            List<TradeRecord> tradeRecords = new List<TradeRecord>();
            Dictionary<string, int> portfolio = symbolList.ToDictionary(x => x, x => 0);
            List<Account> accountPerformance = new List<Account>();

            // get trade history
            foreach(var join in strategy.JoinStrategyEventGroups)
            {
                foreach(var evt in join.EventGroup.Events)
                {
                    foreach(var sybl in symbolList)
                    {
                        decimal cashAvailable = accountValue * join.Percentage * 0.01m / symbolList.Length;
                        var newTrade = new TradeRecord();
                        newTrade.Date = stockprices[sybl].GetNextAvailableDate(evt.Occurrence.AddDays(join.AfterDays));
                        newTrade.Symbol = sybl;
                        newTrade.Action = join.Action == "buy" ? 1 : -1;
                        newTrade.PurchasePrice = stockprices[sybl][stockprices[sybl].GetNextAvailableDate(newTrade.Date)].Closed;
                        newTrade.Quantity = decimal.ToInt32(cashAvailable / newTrade.PurchasePrice);

                        tradeRecords.Add(newTrade);
                    }
                }
            }
            tradeRecords = tradeRecords.OrderBy(x => x.Date).ToList();
            Dictionary<DateTime, List<TradeRecord>> tradeHistory = new Dictionary<DateTime, List<TradeRecord>>();
            foreach(var trade in tradeRecords)
            {
                if (!tradeHistory.ContainsKey(trade.Date))
                {
                    tradeHistory.Add(trade.Date, new List<TradeRecord>());
                }
                tradeHistory[trade.Date].Add(trade);
            }

            // get portfolio value trend
            DateTime current = from;
            int benchmarkquant = decimal.ToInt32(cash / sp500prices[sp500prices.GetNextAvailableDate(current)].Closed);
            decimal remaincash = cash - benchmarkquant * sp500prices[sp500prices.GetNextAvailableDate(current)].Closed;
            while (current <= to)
            {
                var newPerformance = new Account();
                if (tradeHistory.ContainsKey(current))
                {
                    List<TradeRecord> trades = tradeHistory[current];
                    foreach(var trade in trades)
                    {
                        if (cash >= trade.Action * trade.PurchasePrice * trade.Quantity)
                        {
                            cash = cash - trade.Action * trade.PurchasePrice * trade.Quantity;
                            portfolio[trade.Symbol] += trade.Action * trade.Quantity;
                        }
                    }
                }
                accountValue = cash + portfolio.Sum(x => x.Value * stockprices[x.Key][stockprices[x.Key].GetPreviousAvailableDate(current)].Closed);
                newPerformance.Date = current;
                newPerformance.Cash = cash;
                newPerformance.AccountValue = accountValue;
                newPerformance.Benchmark = remaincash + benchmarkquant * sp500prices[sp500prices.GetPreviousAvailableDate(current)].Closed;
                accountPerformance.Add(newPerformance);

                current = current.AddDays(1);
            }


            // generate report
            string reportUri = _reportGenerator.GenerateReport(strategy.Name, 
                                                              strategy.Description,
                                                              symbolList,
                                                              strategy.JoinStrategyEventGroups.Select(j => j.EventGroup.Name).ToArray(),
                                                              eventdatefrom, 
                                                              eventdateto,
                                                              meanresults, 
                                                              accountPerformance,
                                                              DateTime.Now, 
                                                              from,
                                                              to);
            
            // save report uri to database
            try
            {
                _strategyService.InsertIntoBackTestingResult(DateTime.Now, from, to, reportUri, strategy.CreatedBy.Id, strategyId);
            } 
            catch (Exception ex)
            {
                //suppress
            }
            
        }

        private async Task<NegativeIndexArray<decimal>> ExecuteEventPriceImpact(
            DateTime from, 
            DateTime to,
            IEnumerable<Event> events,
            Dictionary<DateTime, StockPriceAdjustDaily> stockprices, 
            int eventdatefrom, 
            int eventdateto)
        {
            List<NegativeIndexArray<decimal>> results = new List<NegativeIndexArray<decimal>>();
            foreach (Event e in events)
            {
                if (e.Occurrence.AddDays(eventdatefrom) >= from && e.Occurrence.AddDays(eventdateto) <= to)
                {
                    var curResult = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
                    DateTime current = stockprices.GetNearestAvailableDate(e.Occurrence);
                    curResult[0] = stockprices[current].Closed;

                    // backward lookup
                    DateTime previous;
                    for (int i = -1; i >= eventdatefrom; i--)
                    {
                        previous = stockprices.GetPreviousAvailableDate(current.AddDays(-1));
                        curResult[i] = stockprices[previous].Closed;

                        current = previous;
                    }

                    // forward lookup
                    current = stockprices.GetNearestAvailableDate(e.Occurrence);
                    DateTime next;
                    for (int i = 1; i <= eventdateto; i++)
                    {
                        next = stockprices.GetNextAvailableDate(current.AddDays(1));
                        curResult[i] = stockprices[next].Closed;

                        current = next;
                    }

                    // add the result
                    results.Add(curResult);
                }
            }

            // normalize the result
            List<NegativeIndexArray<decimal>> normalizedresults = new List<NegativeIndexArray<decimal>>();
            foreach (var r in results)
            {
                var nr = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
                for (int i = eventdatefrom; i <= eventdateto; i++)
                {
                    nr[i] = (r[i] - r[0]) * 100 / r[0];
                }
                normalizedresults.Add(nr);
            }

            // calculate the mean result
            NegativeIndexArray<decimal> meanresults = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
            for (int i = eventdatefrom; i <= eventdateto; i++)
            {
                meanresults[i] = normalizedresults.Sum(x => x[i]) / normalizedresults.Count;
            }

            return meanresults;
        }


    }

    public class TradeRecord
    {
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public int Action { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
