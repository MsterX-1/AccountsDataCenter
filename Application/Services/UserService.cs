using Application.DTOs.ItemDto;
using Application.DTOs.UserDto;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            if(users == null || !users.Any())
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<GetUserDto>>(users);
            }
        }
        public async Task<IEnumerable<GetUserWithAccountsDto>> GetAllUsersWithAccounts()
        {
            var usersWithAccounts = await _userRepo.GetUsersWithAccountsAsync();
            if (usersWithAccounts == null || !usersWithAccounts.Any())
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<GetUserWithAccountsDto>>(usersWithAccounts);
            }
        }
        public async Task<GetUserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if(user == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<GetUserDto>(user);
            }
        }
        public async Task AddUserAsync(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepo.AddAsync(user);
        }
        public async Task UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser != null)
            {
                _mapper.Map(userDto, existingUser);
                await _userRepo.UpdateAsync(existingUser);
            }
        }
        public async Task DeleteUserAsync(int id)
        {
            if (await _userRepo.GetByIdAsync(id) != null)
            {
                await _userRepo.DeleteAsync(id);
            }
        }
        public async Task<GetUserDto> GetUserByUserNameAsync(string name)
        {
            var user = await _userRepo.GetUserByUserNameAsync(name);
            if(user == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<GetUserDto>(user);
            }
        }

    }
}
