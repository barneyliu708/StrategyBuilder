using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class Strategy
    {
        public Strategy()
        {
            EventGroups = new List<EventGroup>();
            BackTestingResults = new List<BackTestingResult>();
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

        public ICollection<EventGroup> EventGroups { get; set; }
        public ICollection<BackTestingResult> BackTestingResults { get; set; }
    }
}
