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
    // el class el haye3mmel impllementation lel interface el mo5tasa bel item wel hgat elly mo5tasa bel item bas
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<User> GetUserByUserNameAsync(string name)
        {
            return await _db.Users.FirstOrDefaultAsync(i => i.UserName == name);
        }

        public async Task<IEnumerable<User>> GetUsersWithAccountsAsync()
        {
            return await _db.Users.Include(u => u.Accounts).ToListAsync();
        }
    }
}
