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

        public async Task Execute(DateTime from, DateTime to, string symbol, int strategyId, int eventdatefrom = -5, int eventdateto = 5)
        {
            Dictionary<DateTime, StockPriceAdjustDaily> stockprices = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, symbol);
            Strategy strategy = _strategyService.GetStrategiesByStrategyId(strategyId);
            // IEnumerable<Event> events = strategy.EventGroups.SelectMany(x => x.Events);
            IEnumerable<Event> events = strategy.JoinStrategyEventGroups.Select(j => j.EventGroup).SelectMany(x => x.Events).Distinct();
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
                        previous = stockprices.GetPreviousAvailableDate(current);
                        curResult[i] = stockprices[previous].Closed;

                        current = previous;
                    }

                    // forward lookup
                    current = stockprices.GetNearestAvailableDate(e.Occurrence);
                    DateTime next;
                    for (int i = 1; i <= eventdateto; i++)
                    {
                        next = stockprices.GetNextAvailableDate(current);
                        curResult[i] = stockprices[next].Closed;

                        current = next;
                    }

                    // add the result
                    results.Add(curResult);
                }
            }

            // normalize the result
            List<NegativeIndexArray<decimal>> normalizedresults = new List<NegativeIndexArray<decimal>>();
            foreach(var r in results)
            {
                var nr = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
                for (int i = eventdatefrom; i <= eventdateto; i++)
                {
                    nr[i] = (r[i] - r[0]) * 100 / r[0];
                }
                normalizedresults.Add(nr);
            }

            // calculate the mean result
            NegativeIndexArray<decimal> mean = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
            for (int i = eventdatefrom; i <= eventdateto; i++)
            {
                mean[i] = normalizedresults.Sum(x => x[i]) / normalizedresults.Count;
            }

            // generate report
            string reportUri = _reportGenerator.GenerateReport(strategy.Name, 
                                                              strategy.Description, 
                                                              symbol,
                                                              strategy.JoinStrategyEventGroups.Select(j => j.EventGroup.Name).ToArray(),
                                                              eventdatefrom, 
                                                              eventdateto, 
                                                              mean, 
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
    }
}
