using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string SellerName { get; set; }
        public required decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } //navigation property
    }
}
