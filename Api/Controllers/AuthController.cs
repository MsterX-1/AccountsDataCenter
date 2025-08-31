using Application.DTOs.AuthDto;
using Application.DTOs.ItemDto;
using Application.DTOs.RoleDto;
using Application.DTOs.UserDto;
using Application.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }


        // Implement account-related actions here (e.g., registration, login)
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm] SignUpDto newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(newUser);

            if (!result.IsAuthenticated)
                return BadRequest(result.Messege);

            return Ok(result);

        }

        [HttpPost("LoginToGetToken")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(dto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Messege);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new
            {
                result.UserName,
                result.Email,
                result.Roles,
                result.Token,
                result.JWTExpiresOn,
                result.RefreshTokenExpiration
            });

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(dto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(dto);

        }
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is requred from body or cookie");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is Invalid");

            return Ok($"Token: {token} Has been Revoked");

        }
        private void setRefreshTokenInCookie(string refreshToken, DateTime refreshTokenExpiration)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiration.ToLocalTime(),// to make it same time regon
                Secure = true, // Set to true in production for HTTPS
                SameSite = SameSiteMode.Strict // Adjust based on your requirements
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
