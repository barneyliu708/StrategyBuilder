using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StrategyBuilder.Repository.Interfaces;

namespace StrategyBuilder.Service
{
    public class IndicatorService : BaseService, IIndicatorService
    {
        private IStockDataRepo _stockDataRepo;

        public IndicatorService(DbContext dbContext, IStockDataRepo stockDataRepo) : base(dbContext)
        {
            _stockDataRepo = stockDataRepo;
        }

        public IEnumerable<Indicator> GetAllIndicators()
        {
            return _dbContext.Set<Indicator>().ToList();
        }

        public async Task<IEnumerable<Event>> GetEventsFromExpression(DateTime from, DateTime to, string expression)
        {
            expression = "{Symbol:GOOG};{Comparator:Less};{Indicator:SMA}";

            Dictionary<DateTime, decimal> dataDict = new Dictionary<DateTime, decimal>();
            DateTime curDate = from;
            while (curDate <= to)
            {
                dataDict[curDate] = 0;
                curDate = curDate.AddDays(1);
            }

            DateTime[] dateRange = dataDict.Keys.ToArray();

            string pattern = @"{([a-zA-Z]*):([a-zA-Z]*)}";

            string symbol = string.Empty;
            string comparator = string.Empty;
            var indicators = expression.Split(';').Select(exp =>
            {
                Match m = Regex.Match(exp, pattern, RegexOptions.IgnoreCase);
                string type = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                if (type == "Symbol")
                {
                    symbol = value;
                }

                if (type == "Comparator")
                {
                    comparator = value;
                }
                return new 
                {
                    Type = m.Groups[1].Value,
                    Value = m.Groups[2].Value
                };
            }).ToArray();

            string curOperator = string.Empty;
            foreach (var indicator in indicators)
            {
                if (indicator.Type == "Symbol")
                {
                    var pricesDict = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, indicator.Value);
                    foreach (var date in dateRange)
                    {
                        if (!pricesDict.ContainsKey(date))
                        {
                            dataDict.Remove(date);
                        }
                        else
                        {
                            switch (curOperator)
                            {
                                case "Multiplication":
                                    dataDict[date] *= pricesDict[date].Closed;
                                    break;
                                case "Division":
                                    dataDict[date] /= pricesDict[date].Closed;
                                    break;
                                case "Plus":
                                    dataDict[date] += pricesDict[date].Closed;
                                    break;
                                case "Minus":
                                    dataDict[date] -= pricesDict[date].Closed;
                                    break;
                                default:
                                    dataDict[date] = pricesDict[date].Closed;
                                    break;
                            }
                        }
                    }
                }
                else if (indicator.Type == "Indicator" && !string.IsNullOrWhiteSpace(symbol))
                {
                    var idxDict = await _stockDataRepo.GetStockTechIndicator(from, to, symbol, indicator.Value);
                    foreach (var date in dateRange)
                    {
                        if (!idxDict.ContainsKey(date))
                        {
                            dataDict.Remove(date);
                        }
                        else
                        {
                            switch (curOperator)
                            {
                                case "Multiplication":
                                    dataDict[date] *= idxDict[date].Closed;
                                    break;
                                case "Division":
                                    dataDict[date] /= idxDict[date].Closed;
                                    break;
                                case "Plus":
                                    dataDict[date] += idxDict[date].Closed;
                                    break;
                                case "Minus":
                                    dataDict[date] -= idxDict[date].Closed;
                                    break;
                                default:
                                    dataDict[date] = idxDict[date].Closed;
                                    break;
                            }
                        }
                    }
                }
                else if (indicator.Type == "Operator")
                {
                    curOperator = indicator.Value;
                }
                else if (indicator.Type == "Comparator")
                {
                    curOperator = "Minus";
                }
            }

            List<Event> results = new List<Event>();
            dateRange = dataDict.Keys.ToArray().OrderBy(x => x).ToArray();
            for (int cur = 1; cur < dateRange.Length; cur++)
            {
                DateTime yesterday = dateRange[cur - 1];
                DateTime today = dateRange[cur];
                if (comparator == "Less" && dataDict[today] < 0 && dataDict[yesterday] > 0)
                {
                    results.Add(new Event() { Occurrence = today });
                }

                if (comparator == "Greater" && dataDict[today] > 0 && dataDict[yesterday] < 0)
                {
                    results.Add(new Event() { Occurrence = today });
                }
            }

            return results;
        }
    }
}
