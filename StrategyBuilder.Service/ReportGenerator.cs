using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Newtonsoft.Json;
using StrategyBuilder.Model;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StrategyBuilder.Service
{
    public class ReportGenerator: IReportGenerator
    {
        private IConfiguration _config;
        private string _pyScriptPath;

        public ReportGenerator(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// generate report and return the string of report pdf file location
        /// </summary>
        /// <param name="strategyName"></param>
        /// <param name="description"></param>
        /// <param name="symbol"></param>
        /// <param name="eventNames"></param>
        /// <param name="eventdatefrom"></param>
        /// <param name="eventdateto"></param>
        /// <param name="meanResult"></param>
        /// <param name="executedOn"></param>
        /// <param name="executeFrom"></param>
        /// <param name="executeTo"></param>
        /// <returns></returns>
        public string GenerateReport(string strategyName, 
                                            string description, 
                                            string[] symbolList,
                                            string[] eventNames,
                                            int eventdatefrom, 
                                            int eventdateto, 
                                            NegativeIndexArray<decimal> meanResult, 
                                            DateTime executedOn,
                                            DateTime executeFrom,
                                            DateTime executeTo)
        {
            // full path to .py file
            _pyScriptPath = _config["ReportScriptLocation"];

            // convert input arguments to JSON string
            List<int> xAxis = new List<int>();
            List<decimal> yAxis = new List<decimal>();
            for (int i = eventdatefrom; i <= eventdateto; i++)
            {
                xAxis.Add(i);
                yAxis.Add(meanResult[i]);
            }

            string outputfileroot = _config["ReportResultPhysicalLocation"];
            string outputfilename = $"{strategyName}_{string.Join("+", symbolList)}_{executeFrom:yyyy-MM-dd}_{executeTo:yyyy-MM-dd}_{Guid.NewGuid()}.pdf";
            object arg = new
            {
                filename = Path.Combine(outputfileroot, outputfilename),
                strategyname = strategyName,
                strategydescription = description,
                symbolList = symbolList,
                eventNames = eventNames,
                executedon = executedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                executefrom = executeFrom.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                executeto = executeTo.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                x = xAxis.ToArray(),
                y = yAxis.ToArray()
            };
            string jsonStr = JsonConvert.SerializeObject(arg);
            BsonDocument argsBson = BsonDocument.Parse(JsonConvert.SerializeObject(arg));

            bool saveInputFile = false;

            string argsFile = string.Format("{0}\\{1}.txt", Path.GetDirectoryName(_pyScriptPath), Guid.NewGuid());

            string outputString = null;
            // create new process start info 
            ProcessStartInfo prcStartInfo = new ProcessStartInfo
            {
                // full path of the Python interpreter 'python.exe'
                FileName = _config["PythonInterpreterLocation"], // string.Format(@"""{0}""", "python.exe"),
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
                    prcStartInfo.Arguments = string.Format("{0} {1}", string.Format(@"""{0}""", _pyScriptPath), string.Format(@"""{0}""", argsFile));
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
                    //File.Delete(argsFile);
                }
            }
            Console.WriteLine(outputString);
            string webserver = _config["ReportResultWebServer"];
            if (string.IsNullOrWhiteSpace(webserver))
            {
                webserver = "http://127.0.0.1:8887/";
            }
            return Path.Combine(webserver, outputfilename);
        }
    }
}
