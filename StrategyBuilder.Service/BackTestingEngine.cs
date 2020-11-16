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

        public BackTestingEngine(DbContext dbContext, IStockDataRepo stockDataRepo, IStrategyService strategyService) : base(dbContext) 
        {
            _stockDataRepo = stockDataRepo;
            _strategyService = strategyService;
        } 

        public async Task Execute(DateTime from, DateTime to, string symbol, int strategyId, int eventdatefrom = -5, int eventdateto = 5)
        {
            Dictionary<DateTime, StockPriceAdjustDaily> stockprices = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, symbol);
            Strategy strategy = _strategyService.GetStrategiesByStrategyId(strategyId);
            IEnumerable<Event> events = strategy.EventGroups.SelectMany(x => x.Events);

            List<NegativeIndexArray<decimal>> results = new List<NegativeIndexArray<decimal>>();
            foreach (Event e in events)
            {
                if (stockprices.ContainsKey(e.Occurrence))
                {
                    var curResult = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
                    curResult[0] += stockprices[e.Occurrence].Closed;

                    // backward lookup
                    DateTime current = e.Occurrence;
                    DateTime previous;
                    for (int i = -1; i >= eventdatefrom; i--)
                    {
                        previous = stockprices.GetPreviousAvailableDate(current);
                        curResult[i] += stockprices[previous].Closed;

                        current = previous;
                    }

                    // forward lookup
                    current = e.Occurrence;
                    DateTime next;
                    for (int i = 1; i <= eventdateto; i++)
                    {
                        next = stockprices.GetNextAvailableDate(current);
                        curResult[i] += stockprices[next].Closed;

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
                    nr[i] = (r[i] - r[0]) / r[0];
                }
                normalizedresults.Add(nr);
            }

            // calculate the mean result
            NegativeIndexArray<decimal> mean = new NegativeIndexArray<decimal>(Math.Max(-1 * eventdatefrom, eventdateto));
            for (int i = eventdatefrom; i <= eventdateto; i++)
            {
                mean[i] = normalizedresults.Sum(x => x[i]) / normalizedresults.Count;
            }



            ///////////////////////
            // generate report
            //////////////////////
            
            // full path to .py file
            string pyScriptPath = @"C:\Users\barne\OneDrive\Documents\CPT\HU\Semester 5\GRAD 695\Project\ReportGeneratorScript.py";
            // convert input arguments to JSON string
            List<int> xAxis = new List<int>();
            List<decimal> yAxis = new List<decimal>();
            for (int i = eventdatefrom; i <= eventdateto; i++)
            {
                xAxis.Add(i);
                yAxis.Add(mean[i]);
            }

            string outputfilename = $"{strategy.Name}_{DateTime.Now.ToString("yyyy_MM_dd")}_{Guid.NewGuid()}.pdf";
            object arg = new
            {
                filename = outputfilename,
                x = xAxis.ToArray(),
                y = yAxis.ToArray()
            };
            string jsonStr = JsonConvert.SerializeObject(arg);
            BsonDocument argsBson = BsonDocument.Parse(JsonConvert.SerializeObject(arg));

            bool saveInputFile = true;

            string argsFile = string.Format("{0}\\{1}.txt", Path.GetDirectoryName(pyScriptPath), Guid.NewGuid());

            string outputString = null;
            // create new process start info 
            ProcessStartInfo prcStartInfo = new ProcessStartInfo
            {
                // full path of the Python interpreter 'python.exe'
                FileName = @"C:\Users\barne\anaconda3\python.exe", // string.Format(@"""{0}""", "python.exe"),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };

            try
            {
                // write input arguments to .txt file 
                using (StreamWriter sw = new StreamWriter(argsFile))
                {
                    sw.WriteLine(argsBson);
                    prcStartInfo.Arguments = string.Format("{0} {1}", string.Format(@"""{0}""", pyScriptPath), string.Format(@"""{0}""", argsFile));
                }
                // start process
                using (Process process = Process.Start(prcStartInfo))
                {
                    // read standard output JSON string
                    using (StreamReader myStreamReader = process.StandardOutput)
                    {
                        outputString = myStreamReader.ReadLine();
                        process.WaitForExit();
                    }
                }
            }
            finally
            {
                // delete/save temporary .txt file 
                if (!saveInputFile)
                {
                    File.Delete(argsFile);
                }
            }
            Console.WriteLine(outputString);

            try
            {
                using(var db = new StrategyBuilderContext())
                {
                    var newReport = new BackTestingResult()
                    {
                        ExecutedOn = DateTime.Now,
                        ExecutedBy = strategy.CreatedBy,
                        Strategy = strategy,
                        ReportFileUri = "C:\\Users\\barne\\GitHubRepos\\StrategyBuilder.BFF\\StrategyBuilder.Api\\" + outputfilename,
                        StartFrom = from,
                        EndTo = to
                    };
                    //db.BackTestingResults.Add(newReport);
                    //var result = db.BackTestingResults.Where(x => x.Strategy.Id == strategyId).ToList();
                    //result.Add(newReport);
                    db.Entry(newReport).State = EntityState.Added;
                    //var s = db.Strategies.FirstOrDefault(s => s.Id == strategyId);
                    //s.BackTestingResults.Add(newReport);

                    //db.Update(s);
                    _dbContext.SaveChanges();
                }
                //save the result to database
                //strategy.BackTestingResults.Add(
                //    new BackTestingResult()
                //    {
                //        ExecutedOn = DateTime.Now,
                //    //ExecutedBy = new User() { Id = strategy.CreatedBy.Id },
                //    //Strategy = strategy,
                //    ReportFileUri = "C:\\Users\\barne\\GitHubRepos\\StrategyBuilder.BFF\\StrategyBuilder.Api\\" + outputfilename,
                //        StartFrom = from,
                //        EndTo = to
                //    });
                //_dbContext.Update(strategy);
                ////var backtestingResults = _dbContext.Set<BackTestingResult>().Where(r => r.Strategy.Id == strategyId).ToList();
                ////_dbContext.Add(new BackTestingResult()
                ////{
                ////    ExecutedOn = DateTime.Now,
                ////    ExecutedBy = strategy.CreatedBy,
                ////    Strategy = strategy,
                ////    ReportFileUri = "C:\\Users\\barne\\GitHubRepos\\StrategyBuilder.BFF\\StrategyBuilder.Api\\" + outputfilename,
                ////    StartFrom = from,
                ////    EndTo = to
                ////});
                //_dbContext.SaveChanges();
            } 
            catch (Exception ex)
            {

            }
            
        }

        //private NegativeIndexArray CalculateEventsPerformance(IEnumerable<Event> events, Dictionary<DateTime, StockPriceAdjustDaily> stockprices)
        //{
        //    var result = new NegativeIndexArray<decimal>()
        //}
    }
}
