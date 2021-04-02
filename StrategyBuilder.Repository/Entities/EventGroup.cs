using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrategyBuilder.Repository.Entities
{
    public class EventGroup
    {
        public EventGroup()
        {
            JoinStrategyEventGroups = new List<JoinStrategyEventGroup>();
            Events = new List<Event>();
            Expressions = new List<Expression>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public User CreatedBy { get; set; }

        public ICollection<JoinStrategyEventGroup> JoinStrategyEventGroups { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Expression> Expressions { get; set; }
    }
}
