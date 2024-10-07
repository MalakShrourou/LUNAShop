using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> Login(string email);
        Task DefaultAddress(int addressId, int customerId);
        Task ChangeActiveStatus(int customerId);
        Task<bool> ChangePassword(int id, string password);
        Task<Customer> FindByVerificationCodeAsync(string code);

    }
}
