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

            return Ok(new
            {
                result.UserName,
                result.Email,
                result.Roles,
                result.Token,
                result.ExpiresOn
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
    }
}
