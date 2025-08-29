using Application.DTOs.AuthDto;
using Application.DTOs.ItemDto;
using Application.DTOs.RoleDto;
using Application.DTOs.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthService
{
    public interface IAuthService
    {
        // Sign Up
        Task<AuthDto> RegisterAsync(SignUpDto signUpDto);
        //Login
        Task<AuthDto> GetTokenAsync(LoginDto loginDto);
        //Add Role
        Task<string> AddRoleAsync(AddRoleDto addRoleDto);
    }
}
