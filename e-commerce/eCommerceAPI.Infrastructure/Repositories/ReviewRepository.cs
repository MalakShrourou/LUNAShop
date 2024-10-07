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
    public class ReviewRepository : IRepository<Review>
    {
        private readonly eCommerceContext _context;
        public ReviewRepository(eCommerceContext context)
        {
            _context = context;
        }
        public async Task<int> Create(Review entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Review review = await GetById(id);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {id} not found.");

            review.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Review>> GetAll()
        {
           return await _context.Reviews
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Review> GetById(int id)
        {
            return await _context.Reviews
                .Where(r => !r.IsDeleted)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Update(Review entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Review with ID {entity.Id} not found.");

            _context.Reviews.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
