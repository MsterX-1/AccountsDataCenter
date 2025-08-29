using Application.DTOs.AccountDto;
using Application.DTOs.ItemDto;
using Application.DTOs.UserDto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extentions.UserExtentions
{
    public static class UserExtention
    {
        //  Extension methods for User-related functionalities can be added here.
        public static IEnumerable<GetUserDto> ConvertToGetDtoList(this IEnumerable<User> users)
        {
            return users.Select(user => new GetUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FristName,
                LastName = user.LastName,
                Email = user.Email
            }).ToList();

        }
        public static GetUserDto ConvertToGetDto(this User user)
        {
            return new GetUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FristName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
        public static IEnumerable<GetUserWithAccountsDto> ConvertToGetDtoWithAccountsList(this IEnumerable<User> users)
        {
            return users.Select(user => new GetUserWithAccountsDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Accounts = user.Accounts != null ? user.Accounts.Select(account => new GetAccountDto
                {
                    Id = account.Id,
                    SellerName = account.SellerName,
                    Price = account.Price,
                    CreatedAt = account.CreatedAt
                }).ToList() : new List<GetAccountDto>()
            }).ToList();
        }
        public static User ConvertToUser(this SignUpDto dto)
        {
            return new User
            {
                UserName = dto.UserName,
                FristName = dto.FristName,
                LastName = dto.LastName,
                Email = dto.Email
            };
        }
    }
}
