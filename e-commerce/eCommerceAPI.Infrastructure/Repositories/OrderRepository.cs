using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Enums;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly eCommerceContext _context;
        private readonly ICustomerRepository _customerRepository;
        public OrderRepository(eCommerceContext context, ICustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

        public async Task ChangeStatus(int id, Status status)
        {
            var order = await GetById(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.Status = status;
            if (status == Status.Delivered)
                order.DeliveryDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(Order entity)
        {
            await _context.Orders.AddAsync(entity);

            IList<OrderItem> orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == null && oi.CustomerId == entity.CustomerId)
                .ToListAsync();
            _context.OrderItems.RemoveRange(orderItems);

            Customer customer = await _customerRepository.GetById(entity.CustomerId);
            customer.LastOrder = DateTime.Now;

            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            var order = await GetById(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude( oi => oi.Product)
                .ThenInclude(p=> p.ProductImages)
                .Where(o => !o.IsDeleted)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task Update(Order entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetCustomerOrders(int customerId)
        {
            Customer? customer = await _context.Customers
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<IList<Order>> GetSalesByDeliveryDate(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.DeliveryDate.HasValue // Filter orders with non-null delivery dates
                    && o.DeliveryDate.Value.Date >= startDate
                    && o.DeliveryDate.Value.Date <= endDate)
                .OrderBy(o => o.DeliveryDate)
                .ToListAsync();
        }

        public async Task<IList<Order>> GetOrderStatus() // Only 'New' and 'Confirmed' statuses
        {
            return await _context.Orders
                .Where(o => o.Status == Status.New || o.Status == Status.Confirmed)
                .ToListAsync();
        }
    }
}
