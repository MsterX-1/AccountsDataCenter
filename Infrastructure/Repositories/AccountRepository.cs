using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccountRepository:GenericRepository<Account>,IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Account>> GetAccountsBySellerName(string sellerName)
        {
            return await _db.Accounts.Where(a => a.SellerName.ToLower() == sellerName.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId)
        {
            return await _db.Accounts.Where(a => a.UserId == userId).ToListAsync();
            
        }
    }
}
