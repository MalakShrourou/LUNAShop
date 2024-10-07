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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly eCommerceContext _context;
        public OrderItemRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(OrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            OrderItem? orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.ProductId == entity.ProductId && oi.OrderId == entity.OrderId && !oi.IsDeleted);

            if (orderItem == null)
                await _context.OrderItems.AddAsync(entity);
            else
                orderItem.Quantity = entity.Quantity;

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            OrderItem orderItem = await GetById(id);
            if (orderItem == null)
                throw new KeyNotFoundException($"Order item with ID {id} not found.");
            orderItem.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<OrderItem>> GetAll()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(oi => !oi.IsDeleted)
                .ToListAsync();
        }

        public async Task<OrderItem> GetById(int id)
        {
            return await _context.OrderItems
                      .Include(oi => oi.Product)
                      .ThenInclude(p => p.ProductImages)
                      .Where(oi => !oi.IsDeleted)
                      .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        public async Task<IList<OrderItem>> GetCustomerItems(int customerId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(oi=>oi.CustomerId == customerId && !oi.IsDeleted && oi.OrderId == null)
                .ToListAsync();
        }

        public async Task<IList<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
        }

        public async Task<IList<OrderItem>> GetOrderItemsCategory()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Where(oi => !oi.IsDeleted)
                .ToListAsync();
        }

        public async Task<IList<OrderItem>> GetProductSales(int productId, DateTime fromDate)
        {
            IList<OrderItem> orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi=>oi.ProductId == productId 
                    && !oi.IsDeleted 
                    && oi.Order.CreatedOn >= fromDate)
                .ToListAsync();
            return orderItems;
        }

        public async Task<IList<OrderItem>> GetTrendingProducts(DateTime fromDate)
        {
            IList<OrderItem> orderItems = await GetAll();
            orderItems.Where(oi => oi.Order.CreatedOn >= fromDate);
            return orderItems;
        }

        public async Task Update(OrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            _context.OrderItems.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
