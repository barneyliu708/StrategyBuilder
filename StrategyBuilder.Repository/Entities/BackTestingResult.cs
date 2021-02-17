using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class BackTestingResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ExecutedOn { get; set; }

        [Required]
        public DateTime StartFrom { get; set; }

        [Required]
        public DateTime EndTo { get; set; }

        [Required]
        public string ReportFileUri { get; set; }
    }

}
