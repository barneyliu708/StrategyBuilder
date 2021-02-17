using System;
using System.ComponentModel.DataAnnotations;

namespace StrategyBuilder.Repository.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public DateTime Occurrence { get; set; }
    }
}
