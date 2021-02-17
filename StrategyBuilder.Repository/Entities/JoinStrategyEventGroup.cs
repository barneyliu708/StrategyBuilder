using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class JoinStrategyEventGroup
    {
        public int EventGroupId { get; set; }
        public EventGroup EventGroup { get; set; }
        public int StrategyId { get; set; }
        public Strategy Strategy { get; set; }
    }
}
