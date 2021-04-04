using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Model
{
    public class BackTestingRequest
    {
        public string from { get; set; }
        public string to { get; set; }
        public string[] SimbolList { get; set; }
        public int StrategyId { get; set; }
    }
}
