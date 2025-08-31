using Application.DTOs.AuthDto;
using Application.DTOs.ItemDto;
using Application.DTOs.RoleDto;
using Application.DTOs.UserDto;
using Application.Extentions.UserExtentions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }



        public async Task<AuthDto> RegisterAsync(SignUpDto newUser)
        {
            if (await _userManager.FindByNameAsync(newUser.UserName) != null)
                return new AuthDto { Messege = "User Name is already taken" };

            if (await _userManager.FindByEmailAsync(newUser.Email) != null)
                return new AuthDto { Messege = "Email is already registered" };

            var user = newUser.ConvertToUser();
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDto { Messege = $"User registration failed: {errors}" };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthDto
            {
                Messege = "User registered successfully",
                IsAuthenticated = true,
                UserName = user.UserName,
                FristName = user.FristName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                JWTExpiresOn = jwtSecurityToken.ValidTo
            };
        }

        // When Login
        public async Task<AuthDto> GetTokenAsync(LoginDto loginDto)
        {
            var authDto = new AuthDto();
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == loginDto.userName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.password))
                return new AuthDto { Messege = "UserName or Password is Incorrect !" };

            var jwtSecurityToken = await CreateJwtToken(user);

            var rolesList = await _userManager.GetRolesAsync(user);
            authDto.IsAuthenticated = true;
            authDto.UserName = user.UserName;
            authDto.Email = user.Email;
            authDto.Roles = rolesList.ToList();
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDto.JWTExpiresOn = jwtSecurityToken.ValidTo;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }
            return authDto;
        }



        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JWT:DurationInMinutes"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<string> AddRoleAsync(AddRoleDto addRoleDto)
        {
            var user = await _userManager.FindByIdAsync(addRoleDto.UserId);
            if (user == null || !await _roleManager.RoleExistsAsync(addRoleDto.Role))
                return "Invalid User ID or Role";

            if (await _userManager.IsInRoleAsync(user, addRoleDto.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, addRoleDto.Role);

            return result.Succeeded ? string.Empty : "Failed to add role to user";
        }

        public async Task<AuthDto> RefreshTokenAsync(string Token)
        {
            var authDto = new AuthDto();
            //get refresh token owner
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == Token));

            if (user == null)
            {
                authDto.Messege = "Invalid Token";
                return authDto;
            }
            //get refresh token
            var refreshToken = user.RefreshTokens.Single(t => t.Token == Token);
            if (!refreshToken.IsActive)
            {
                authDto.Messege = "Inactive Token";
                return authDto;
            }
            //revoke current refresh token
            refreshToken.Revoked = DateTime.UtcNow;

            //generate new refresh token and save to db
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            //generate new jwt
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            return new AuthDto
            {
                IsAuthenticated = true,
                UserName = user.UserName,
                Email = user.Email,
                Roles = rolesList.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                JWTExpiresOn = jwtSecurityToken.ValidTo,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires
            };
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var authDto = new AuthDto();
            //get refresh token owner
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            //get refresh token
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;

            //revoke current refresh token
            refreshToken.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow
                };
            
        }
    }
}
