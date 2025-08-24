using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDto
{
    public class UpdateAccountDto
    {
        public string? SellerName { get; set; }=null;  
        public decimal? Price { get; set; }= null;    // nullable
        public int? UserId { get; set; } = null;      // nullable
    }
}
