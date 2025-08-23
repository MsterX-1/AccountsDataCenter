using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public required string UserName { get; set; }
        [MaxLength(50)]
        public required string Password { get; set; }
        public ICollection<Account> Accounts { get; set; }//navigation property

    }
}
