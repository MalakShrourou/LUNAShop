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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly eCommerceContext _context;
        public CategoryRepository(eCommerceContext context)
        {
            _context = context;
        }
        
        public async Task<int> Create(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Category category = await GetById(id);
            if(category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IList<Category>> GetActive()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted && c.IsActive)
                .ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Category with ID {entity.Id} not found.");

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(IList<Product>, int)> GetProductsByGategory(int categoryId, string? sortBy, int? brandId,
            double? minPrice, double? maxPrice, int pageNumber, int pageSize, string userLang)
        {
            IList<Product> products = await _context.Products
                .Include( p=> p.ProductImages)
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync();

            if (brandId != null)
                products = products.Where(p => p.BrandId == brandId).ToList();

            if(minPrice != null)
                products = products.Where(p => p.Price >= minPrice).ToList();
            if(maxPrice != null)
                products = products.Where(p => p.Price <= maxPrice).ToList();

            products = sortBy switch
            {
                "priceAsc" => products.OrderBy(p => p.Price).ToList(),
                "priceDesc" => products.OrderByDescending(p => p.Price).ToList(),
                "nameAsc" => userLang == "en" ? products.OrderBy(p => p.NameEN).ToList():
                    products.OrderBy(p => p.NameAR).ToList(),
                "nameDesc" => userLang == "en" ? products.OrderByDescending(p => p.NameEN).ToList():
                    products.OrderByDescending(p => p.NameAR).ToList(),
                _ => products.OrderBy(p => p.Id).ToList()
            };

            int Count = products.Count();
            products = products.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return (products, Count);
        }

        public async Task ChangeActiveStatus(int categoryId)
        {
            Category category = await GetById(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            category.IsActive = !category.IsActive;
            await _context.SaveChangesAsync();
        }
    }
}
