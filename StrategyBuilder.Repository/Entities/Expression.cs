using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class Expression
    {
        [Key]
        public int Id { get; set; }

        [StringLength(1000)]
        public string Content { get; set; }
    }
}
