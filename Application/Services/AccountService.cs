using Application.DTOs.AccountDto;
using Application.Extentions.AccountExtentions;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly UserManager<User> _userManager;
        public AccountService(IAccountRepository accountRepo,UserManager<User> userManager)
        {
            _accountRepo = accountRepo;
            _userManager = userManager;
        }
        public async Task<IEnumerable<GetAccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepo.GetAllAsync();
            if(accounts == null || !accounts.Any())
                throw new Exception("No accounts found.");

            return accounts.ConvertToGetDtoList();
        }
        public async Task<GetAccountDto> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null)
                throw new Exception("No accounts found.");

            return account.ConvertToGetDto();
        }
        public async Task<IEnumerable<GetAccountDto>> GetAccountsByUserIdAsync(string userId)
        {
            var accounts = await _accountRepo.GetAccountsByUserIdAsync(userId);
            if (accounts == null || !accounts.Any())
                throw new Exception("No accounts found.");

            return accounts.ConvertToGetDtoList();
        }
        public async Task<IEnumerable<GetAccountDto>> GetAccountsBySellerNameAsync(string sellerName)
        {
            var accounts = await _accountRepo.GetAccountsBySellerName(sellerName);
            if (accounts == null || !accounts.Any())
                throw new Exception("No accounts found.");

            return accounts.ConvertToGetDtoList();
        }
        public async Task AddAccountAsync(CreateAccountDto createAccountDto)
        {
            var user = await _userManager.FindByIdAsync(createAccountDto.UserId);
            if (user == null)
                throw new ArgumentException("Invalid User ID !");

            var newAccount = createAccountDto.ConvertToAccount();
            await _accountRepo.AddAsync(newAccount);
        }
        public async Task UpdateAccountAsync(int accountId, UpdateAccountDto updateAccountDto)
        {
            // Validate account exists
            var existingAccount = await _accountRepo.GetByIdAsync(accountId);
            if (existingAccount == null)
                throw new KeyNotFoundException("No account found.");

            // Validate new user if provided
            if (!string.IsNullOrEmpty(updateAccountDto.UserId))
            {
                var user = await _userManager.FindByIdAsync(updateAccountDto.UserId);
                if (user == null)
                    throw new ArgumentException("Invalid User ID !");
                existingAccount.UserId = updateAccountDto.UserId;
            }
            // Update fields if provided
            if (!string.IsNullOrEmpty(updateAccountDto.SellerName))
                existingAccount.SellerName = updateAccountDto.SellerName;

            if (updateAccountDto.Price.HasValue)
                existingAccount.Price = updateAccountDto.Price.Value;

            await _accountRepo.UpdateAsync(existingAccount);
        }
        public async Task DeleteAccountAsync(int id)
        {
            var existingAccount = await _accountRepo.GetByIdAsync(id);
            if (existingAccount == null)
                throw new KeyNotFoundException("No account found !");
            await _accountRepo.DeleteAsync(id);
        }

    }
}
