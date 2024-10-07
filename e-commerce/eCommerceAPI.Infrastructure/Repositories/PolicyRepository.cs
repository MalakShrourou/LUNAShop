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
    public class PolicyRepository : IRepository<Policy>
    {
        private readonly eCommerceContext _context;
        public PolicyRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Policy entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Policies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Policy policy = await GetById(id);
            if (policy == null)
                throw new KeyNotFoundException($"Policy with ID {id} not found.");

            policy.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Policy>> GetAll()
        {
            return await _context.Policies
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Policy> GetById(int id)
        {
            return await _context.Policies
                .Where(a => !a.IsDeleted)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Policy entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Policy existingPolicy = await GetById(entity.Id);
            if (existingPolicy == null)
                throw new KeyNotFoundException($"Policy with ID {entity.Id} not found.");

            _context.Policies.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
