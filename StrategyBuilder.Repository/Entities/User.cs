using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrategyBuilder.Repository.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string EncryptedPassword { get; set; }
        
        [StringLength(1000)]
        public string SecretKey { get; set; }

        public ICollection<EventGroup> EventGroups { get; set; }
    }
}
