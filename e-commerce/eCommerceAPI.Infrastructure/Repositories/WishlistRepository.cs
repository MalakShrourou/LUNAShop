using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly eCommerceContext _context;

        public WishlistRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Wishlist entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Wishlists.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Wishlist wishlist = await GetById(id);
            if (wishlist == null)
                throw new KeyNotFoundException($"Wishlist with ID {id} not found.");

            wishlist.IsDeleted = true;
            _context.Wishlists.Update(wishlist); 
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Wishlist>> GetAll()
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(w => !w.IsDeleted)
                .ToListAsync();
        }

        public async Task<Wishlist> GetById(int id)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(w => !w.IsDeleted)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task Update(Wishlist entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Wishlist existingWishlist = await GetById(entity.Id); 
            if (existingWishlist == null)
                throw new KeyNotFoundException($"Wishlist with ID {entity.Id} not found.");

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Wishlist>> GetCustomerWishlists(int customerId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(w => w.CustomerId == customerId && !w.IsDeleted)
                .ToListAsync();
        }
    }
}
