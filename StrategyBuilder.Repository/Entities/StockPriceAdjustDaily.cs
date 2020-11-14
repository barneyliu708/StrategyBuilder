using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class StockPriceAdjustDaily
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Closed { get; set; }
        public int Volume { get; set; }
    }
}
