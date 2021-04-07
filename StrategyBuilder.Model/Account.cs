using Newtonsoft.Json;
using StrategyBuilder.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Model
{
    public class Account
    {
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime Date { get; set; }
        public decimal Cash { get; set; }
        public decimal AccountValue { get; set; }
        public decimal Benchmark { get; set; }
    }
}
