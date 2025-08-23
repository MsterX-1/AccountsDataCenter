using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDto
{
    public class GetAccountDto
    {
        [JsonPropertyName("Account ID")]
        public int Id { get; set; }
        [JsonPropertyName("Seller Name")]
        public string SellerName { get; set; }
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
        [JsonPropertyName("Created At")]
        public DateTime? CreatedAt { get; set; }
    }
}
