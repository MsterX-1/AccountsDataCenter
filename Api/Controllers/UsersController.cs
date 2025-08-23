using Application.DTOs.ItemDto;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        #region Get Methods
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userDto = await _userService.GetAllUsersAsync();
            if(userDto == null)
            {
                return NotFound("No user found.");
            }
            return Ok(userDto);
        }
        [HttpGet("WithAccounts")]
        public async Task<IActionResult> GetAllUsersWithAccounts()
        {
            var userWithAccDto = await _userService.GetAllUsersWithAccounts();
            if(userWithAccDto == null)
            {
                return NotFound("No user found.");
            }
            return Ok(userWithAccDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if(userDto == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(userDto);
        }
        [HttpGet("ByUserName/{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var userDto = await _userService.GetUserByUserNameAsync(userName);
            if(userDto == null)
            {
                return NotFound($"User with User Name {userName} not found.");
            }
            return Ok(userDto);
        }
        #endregion

        #region Post Methods
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userService.AddUserAsync(userDto);
            var createdUser = await _userService.GetUserByUserNameAsync(userDto.UserName);
            return Ok(createdUser);
        }
        #endregion

        #region Put Methods
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _userService.GetUserByIdAsync(id);
            if(existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            await _userService.UpdateUserAsync(id, userDto);
            var updatedUser = await _userService.GetUserByIdAsync(id);
            return Ok(updatedUser);
        }
        #endregion

        #region Delete Methods
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if(existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            await _userService.DeleteUserAsync(id);
            return Ok("Deleted Successfully");
        }
        #endregion
    }
}
