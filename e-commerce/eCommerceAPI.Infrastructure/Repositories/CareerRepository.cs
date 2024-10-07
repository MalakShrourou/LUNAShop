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
    public class CareerRepository : IRepository<Career>
    {
        private readonly eCommerceContext _context;

        public CareerRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Career entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Careers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Career career = await GetById(id);
            if (career == null)
                throw new KeyNotFoundException($"Career with ID {id} not found.");

            career.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Career>> GetAll()
        {
            return await _context.Careers
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Career> GetById(int id)
        {
            return await _context.Careers
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Career entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Career with ID {entity.Id} not found.");

            _context.Careers.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

