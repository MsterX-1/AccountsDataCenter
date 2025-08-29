using Application.DTOs.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.UserDto
{
    public class GetUserWithAccountsDto
    {
        [JsonPropertyName("User ID")]
        public string Id { get; set; }
        [JsonPropertyName("User Name")]
        public string UserName { get; set; }

        [JsonPropertyName("Accounts")]
        public List<GetAccountDto> Accounts { get; set; }
    }
}
