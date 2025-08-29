using Application.DTOs.ItemDto;
using Application.DTOs.UserDto;
using Application.Extentions.UserExtentions;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepo, UserManager<User> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        #region By _UserRepo

        public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();

            if (users == null || !users.Any())
                throw new Exception("No users found.");

            return users.ConvertToGetDtoList();
        }

        public async Task<IEnumerable<GetUserWithAccountsDto>> GetAllUsersWithAccounts()
        {
            var usersWithAccounts = await _userRepo.GetUsersWithAccountsAsync();

            if (usersWithAccounts == null || !usersWithAccounts.Any())
                throw new Exception("No users found.");

            return usersWithAccounts.ConvertToGetDtoWithAccountsList();
        }

        #endregion

        #region By UserManager

        public async Task<GetUserDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new Exception("User not found.");

            return user.ConvertToGetDto();
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<GetUserDto> GetUserByUserNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
                throw new Exception("User not found.");

            return user.ConvertToGetDto();
        }

        #endregion
    }
}
