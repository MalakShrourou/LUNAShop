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
    public class RequestRepository : IRepository<Request>
    {
        private readonly eCommerceContext _context;
        public RequestRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Request entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Requests.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Request request = await GetById(id);
            if (request == null)
                throw new KeyNotFoundException($"Request with ID {id} not found.");

            request.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Request>> GetAll()
        {
            return await _context.Requests
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Request> GetById(int id)
        {
            return await _context.Requests
                .Where(a => !a.IsDeleted)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Request entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Request existingRequest = await GetById(entity.Id);
            if (existingRequest == null)
                throw new KeyNotFoundException($"Request with ID {entity.Id} not found.");

            _context.Requests.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
