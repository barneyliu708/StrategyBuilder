using StrategyBuilder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IReportGenerator
    {
        public string GenerateReport(string strategyName,
                                            string description,
                                            string[] symbolList,
                                            string[] eventNames,
                                            int eventdatefrom,
                                            int eventdateto,
                                            Dictionary<string, NegativeIndexArray<decimal>> meanResult,
                                            List<Account> accountPerformance,
                                            DateTime executedOn,
                                            DateTime executeFrom,
                                            DateTime executeTo);
    }
}
