using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FristName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public ICollection<Account> Accounts { get; set; }//navigation property
        public ICollection<RefreshToken> RefreshTokens { get; set; }//navigation property

    }
}
