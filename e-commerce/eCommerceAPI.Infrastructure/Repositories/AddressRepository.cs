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
    public class AddressRepository : IRepository<Address>
    {
        private readonly eCommerceContext _context;
        public AddressRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Addresses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Address address = await GetById(id);
            if (address == null)
                throw new KeyNotFoundException($"Address with ID {id} not found.");

            address.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Address>> GetAll()
        {
            return await _context.Addresses
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Address> GetById(int id)
        {
            return await _context.Addresses
                .Where(a => !a.IsDeleted)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Address existingAddress = await GetById(entity.Id);
            if (existingAddress == null)
                throw new KeyNotFoundException($"Address with ID {entity.Id} not found.");

            _context.Addresses.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
