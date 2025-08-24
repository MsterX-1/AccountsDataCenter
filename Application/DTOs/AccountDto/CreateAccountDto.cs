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
        [MaxLength(50)]
        public required string SellerName { get; set; }
        [JsonPropertyName("Price")]
        public required decimal Price { get; set; }
        [JsonPropertyName("User Id")]
        public required int UserId { get; set; }
    }
}
