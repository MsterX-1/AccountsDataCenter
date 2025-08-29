using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string SellerName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; } //navigation property
    }
}
