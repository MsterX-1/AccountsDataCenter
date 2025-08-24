using Application.DTOs.AccountDto;
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
    public class AccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository accountRepo, IMapper mapper, IUserRepository userRepo)
        {
            _accountRepo = accountRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }
        public async Task<IEnumerable<GetAccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepo.GetAllAsync();
            if (accounts == null || !accounts.Any())
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<GetAccountDto>>(accounts);
            }
        }
        public async Task<GetAccountDto> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<GetAccountDto>(account);
            }
        }
        public async Task<IEnumerable<GetAccountDto>> GetAccountsByUserIdAsync(int userId)
        {
            var accounts = await _accountRepo.GetAccountsByUserIdAsync(userId);
            if (accounts == null || !accounts.Any())
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<GetAccountDto>>(accounts);
            }
        }
        public async Task<IEnumerable<GetAccountDto>> GetAccountsBySellerNameAsync(string sellerName)
        {
            var accounts = await _accountRepo.GetAccountsBySellerName(sellerName);
            if (accounts == null || !accounts.Any())
            {
                return null;
            }
            else
            {
                return _mapper.Map<IEnumerable<GetAccountDto>>(accounts);
            }
        }
        public async Task AddAccountAsync(CreateAccountDto createAccountDto)
        {
            var account = _mapper.Map<Account>(createAccountDto);
            await _accountRepo.AddAsync(account);
        }
        public async Task UpdateAccountAsync(int id, UpdateAccountDto updateAccountDto)
        {
            // Validate account exists
            var existingAccount = await _accountRepo.GetByIdAsync(id);
            if (existingAccount == null)
                throw new KeyNotFoundException($"No account found with ID: {id}");

            if (updateAccountDto.UserId != null) // If UserId is being updated, validate the new UserId exists
            {
                var user = await _userRepo.GetByIdAsync((int)updateAccountDto.UserId);
                if (user == null)
                    throw new ArgumentException($"Invalid User ID: {updateAccountDto.UserId}");
            }

            _mapper.Map(updateAccountDto, existingAccount);

            await _accountRepo.UpdateAsync(existingAccount);

        }
    }
}
