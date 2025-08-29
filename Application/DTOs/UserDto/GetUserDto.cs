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
        public string Id { get; set; }
        [JsonPropertyName("User Name")]
        public string UserName { get; set; }
        [JsonPropertyName("First Name")]
        public string FirstName { get; set; }
        [JsonPropertyName("Last Name")]
        public string LastName { get; set; }
        [JsonPropertyName("Email")]
        public string Email { get; set; }
    }
}
