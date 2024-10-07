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
    public class BrandRepository: IBrandRepository
    {
        private readonly eCommerceContext _context;
        public BrandRepository(eCommerceContext context)
        {
            _context = context;
        }
        public async Task<int> Create(Brand entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Brands.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Brand brand = await GetById(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            brand.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Brand>> GetAll()
        {
            return await _context.Brands
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }
        
        public async Task<IList<Brand>> GetActive()
        {
            return await _context.Brands
                .Where(b => !b.IsDeleted && b.IsActive)
                .ToListAsync();
        }

        public async Task<Brand> GetById(int id)
        {
            return await _context.Brands
                .Where(b => !b.IsDeleted)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Update(Brand entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if(GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Brand with ID {entity.Id} not found.");

            _context.Brands.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<(IList<Product>, int)> GetProductsByBrand(int? categoryId, string? sortBy, int brandId,
                    double? minPrice, double? maxPrice, int pageNumber, int pageSize, string userLang)
        {        
            IList<Product> products = await _context.Products
               .Include(p => p.ProductImages)
               .Include(p => p.Brand)
               .Where(p => p.BrandId == brandId && !p.IsDeleted)
               .ToListAsync();

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

        public async Task ChangeActiveStatus(int brandId)
        {
            Brand brand = await GetById(brandId);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {brandId} not found");

            brand.IsActive = !brand.IsActive;
            await _context.SaveChangesAsync();
        }
    }
}
