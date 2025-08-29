using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDto
{
    public class CreateAccountDto
    {
        [JsonPropertyName("Seller Name")]
        [Required,MaxLength(100)]
        public string SellerName { get; set; }
        [JsonPropertyName("Price")]
        [Required]
        public decimal Price { get; set; }
        [JsonPropertyName("User Id")]
        [Required]
        public string UserId { get; set; }
    }
}
