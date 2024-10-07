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
    public class ArticleRepository : IRepository<Article>
    {
        private readonly eCommerceContext _context;

        public ArticleRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Article entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _context.Articles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Article article = await GetById(id);
            if (article == null)
                throw new KeyNotFoundException($"Article with ID {id} not found.");

            article.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Article>> GetAll()
        {
            return await _context.Articles
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Article> GetById(int id)
        {
            return await _context.Articles
                .Where(a => !a.IsDeleted)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Article entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Article article = await GetById(entity.Id);
            if (article == null)
                throw new KeyNotFoundException($"Article with ID {entity.Id} not found.");

            _context.Articles.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
