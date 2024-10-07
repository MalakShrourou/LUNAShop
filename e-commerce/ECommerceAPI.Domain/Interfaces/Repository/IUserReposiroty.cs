using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Login(string email);
        Task<bool> ChangePassword(int id, string password);
        Task<IList<User>> GetCustomerServiceTeam();
    }
}
