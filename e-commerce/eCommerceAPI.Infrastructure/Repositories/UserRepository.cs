using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly eCommerceContext _context;
        public UserRepository(eCommerceContext context)
        {
            _context = context;
        }
        
        public async Task<int> Create(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            User user = await GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Login(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .Where(u => !u.IsDeleted)
                .FirstAsync(u => u.Email == email);
        }

        public async Task Update(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"User with ID {entity.Id} not found.");

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangePassword(int id, string password)
        {
            User user = await GetById(id);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Password = password;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<User>> GetCustomerServiceTeam()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted && u.Role.NameEN == "Customer Service")
                .ToListAsync();
        }
    }
}
