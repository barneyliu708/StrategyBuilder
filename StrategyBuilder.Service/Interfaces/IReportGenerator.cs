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
                                            string symbol,
                                            string[] eventNames,
                                            int eventdatefrom,
                                            int eventdateto,
                                            NegativeIndexArray<decimal> meanResult,
                                            DateTime executedOn,
                                            DateTime executeFrom,
                                            DateTime executeTo);
    }
}
