using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDto
{
    public class AuthDto
    {
        public string Messege { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public string Token { get; set; }
        public DateTime JWTExpiresOn { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
