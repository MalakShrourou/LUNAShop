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
    public class ProductRepository : IProductRepository
    {
        private readonly eCommerceContext _context;
        public ProductRepository(eCommerceContext context) 
        {
            _context = context; 
        }

        public async Task<int> Create(Product entity)
        {
            int id = entity.ImageId;
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            IList<int> ints = new List<int>();
            foreach (ProductImage item in entity.ProductImages)
            {
                ints.Add(item.Id);                
            }
            entity.ImageId = ints[id];

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Product product = await GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.User)
                    .ThenInclude(u => u.Role)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.Category)
                .Include(p => p.Brand) 
                .Include(p => p.Reviews!)
                    .ThenInclude(r => r.Customer)
                .Where(p => !p.IsDeleted)
                .FirstAsync(p => p.Id == id);
        }

        public async Task Update(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Product existingProduct = await GetById(entity.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {entity.Id} not found.");

            _context.Products.Update(entity);
            await _context.SaveChangesAsync();

            if (entity.ProductImages != null && entity.ProductImages.Any())
            {
                IList<int> imageIds = entity.ProductImages.Select(img => img.Id).ToList();
                if (entity.ImageId >= 0 && entity.ImageId < imageIds.Count)
                {
                    entity.ImageId = imageIds[entity.ImageId];
                    await _context.SaveChangesAsync();  
                }
            }
        }

        public async Task<IList<Product>> Top10InCategory(int productId)
        {
            Product product = await GetById(productId);
            if(product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return await _context.Products
                .Where(p => !p.IsDeleted && p.CategoryId == product.CategoryId)
                .Include(p=> p.ProductImages)
                .Include(p=>p.Reviews)
                .OrderByDescending(p => p.Reviews.Average(r => r.Rate))
                .Take(10)
                .ToListAsync();
        }

        public async Task<IList<Product>> GetPopularProducts()
        {
            return await _context.Products
                .Where( p=> !p.IsDeleted)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews)
                .OrderByDescending(p => p.Reviews.Average(r => r.Rate))
                .Take(10)
                .ToListAsync();
        }

        public async Task<(IList<Product>, int)> SearchProducts(string query, int? categoryId, string? sortBy,
            int? brandId, double? minPrice, double? maxPrice, int pageNumber, int pageSize, string userLang)
        {
            IList<Product> products = await _context.Products
                .Include(p => p.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            products = products.Where(p => p.NameEN.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.DescriptionEN.Contains(query, StringComparison.OrdinalIgnoreCase) 
                || p.NameAR.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.DescriptionAR.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.Category.NameEN.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.Category.NameAR.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.Brand.NameEN.Contains(query, StringComparison.OrdinalIgnoreCase)
                || p.Brand.NameAR.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (brandId != null)
                    products = products.Where(p => p.BrandId == brandId).ToList();
            if (categoryId != null)
                products = products.Where(p => p.CategoryId == categoryId).ToList();
            
            if (minPrice != null)
                products = products.Where(p => p.Price >= minPrice).ToList();
            if (maxPrice != null)
                products = products.Where(p => p.Price <= maxPrice).ToList();
            
            products = sortBy switch
            {
                "priceAsc" => products.OrderBy(p => p.Price).ToList(),
                "priceDesc" => products.OrderByDescending(p => p.Price).ToList(),
                "nameAsc" => userLang == "en" ? products.OrderBy(p => p.NameEN).ToList() :
                products.OrderBy(p => p.NameAR).ToList(),
                "nameDesc" => userLang == "en" ? products.OrderByDescending(p => p.NameEN).ToList() :
                products.OrderByDescending(p => p.NameAR).ToList(),
                _ => products.OrderBy(p => p.Id).ToList()
            };
            
            int Count = products.Count();
            products = products.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return (products, Count);
        }
    }
}
