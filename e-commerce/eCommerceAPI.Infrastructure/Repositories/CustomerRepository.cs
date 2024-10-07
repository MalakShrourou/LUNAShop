using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly eCommerceContext _context;
        public CustomerRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Customer customer = await GetById(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            customer.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Customer>> GetAll()
        {
            return await _context.Customers
                .Include(c => c.Addresses.Where(r => !r.IsDeleted))
                .Include(c => c.Orders.Where(o => !o.IsDeleted))
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers
                .Include(c => c.Addresses.Where(r => !r.IsDeleted))
                .Include(c => c.Orders.Where(o => !o.IsDeleted))
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> Login(string email)
        {
            Customer customer =  await _context.Customers
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .Where(u => !u.IsDeleted)
                .FirstAsync(u => u.Email == email);

            if (customer.IsVerified == false)
                throw new Exception("Please verify your email to sign in");

            if (!customer.IsActive)
                throw new Exception("Customer account is deactivated");

            customer.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task Update(Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Customer with ID {entity.Id} not found.");

            _context.Customers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DefaultAddress(int addressId, int customerId)
        {
            Customer? customer = await GetById(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found");

            customer.DefaultAddress = addressId;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeActiveStatus(int customerId)
        {
            Customer customer = await GetById(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found");

            customer.IsActive = !customer.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangePassword(int id, string password)
        {
            Customer customer = await GetById(id);
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Password = password;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Customer> FindByVerificationCodeAsync(string code)
        {
            Customer? customer = await _context.Customers.SingleOrDefaultAsync(u => u.VerificationCode == code);
            customer.IsVerified = true;
            customer.VerificationCode = null;
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}