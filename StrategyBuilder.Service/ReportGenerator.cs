using MongoDB.Bson;
using Newtonsoft.Json;
using StrategyBuilder.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StrategyBuilder.Service
{
    public class ReportGenerator
    {
        public static string GenerateReport(string strategyName, 
                                            string description, 
                                            string symbol,
                                            string[] eventNames,
                                            int eventdatefrom, 
                                            int eventdateto, 
                                            NegativeIndexArray<decimal> meanResult, 
                                            DateTime executedOn,
                                            DateTime executeFrom,
                                            DateTime executeTo)
        {
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
                yAxis.Add(meanResult[i]);
            }

            string outputfilename = $"{strategyName}_{symbol}_{executeFrom:yyyy-MM-dd}_{executeTo:yyyy-MM-dd}_{Guid.NewGuid()}.pdf";
            object arg = new
            {
                filename = outputfilename,
                strategyname = strategyName,
                strategydescription = description,
                symbol = symbol,
                eventNames = eventNames,
                executedon = executedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                executefrom = executeFrom.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                executeto = executeTo.ToString("yyyy-MM-dd HH:mm:ss.fff"),
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

            return "http://127.0.0.1:8887/" + outputfilename;
        }
    }
}
