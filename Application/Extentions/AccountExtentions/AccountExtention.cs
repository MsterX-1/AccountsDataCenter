using Application.DTOs.AccountDto;
using Application.DTOs.ItemDto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extentions.AccountExtentions
{
    public static class AccountExtention
    {
        public static IEnumerable<GetAccountDto> ConvertToGetDtoList(this IEnumerable<Account> accounts)
        {
            return accounts.Select(accounts => new GetAccountDto
            {
                Id = accounts.Id,
                SellerName = accounts.SellerName,
                Price = accounts.Price,
                CreatedAt = accounts.CreatedAt
            }).ToList();

        }
        public static GetAccountDto ConvertToGetDto(this Account account)
        {
            return new GetAccountDto
            {
                Id = account.Id,
                SellerName = account.SellerName,
                Price = account.Price,
                CreatedAt = account.CreatedAt
            };
        }
        public static Account ConvertToAccount(this CreateAccountDto dto)
        {
            return new Account
            {
                SellerName = dto.SellerName,
                Price = dto.Price,
                UserId = dto.UserId
            };
        }
    }
}
