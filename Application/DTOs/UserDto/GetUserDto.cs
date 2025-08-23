using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.ItemDto
{
    public class GetUserDto
    {
        [JsonPropertyName("User ID")]
        public int Id { get; set; }
        [JsonPropertyName("User Name")]
        public string UserName { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
    }
}
