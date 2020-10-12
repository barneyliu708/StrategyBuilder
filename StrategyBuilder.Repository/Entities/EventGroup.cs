using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class EventGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
