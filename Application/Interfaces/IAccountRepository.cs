using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public  interface IAccountRepository: IGenericRepository<Account>
    {
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId);
        Task<IEnumerable<Account>> GetAccountsBySellerName(string sellerName);
    }
}
