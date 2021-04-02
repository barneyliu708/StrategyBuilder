using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class Indicator
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Key { get; set; }

        [StringLength(100)]
        public string Text { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
