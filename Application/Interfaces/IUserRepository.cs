using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        // interface feh kol 7aga fel generic repository w kaman feh 7agat mo5tasa bel item bas
        Task<User>GetUserByUserNameAsync(string name);
        Task<IEnumerable<User>> GetUsersWithAccountsAsync();
    }
}
