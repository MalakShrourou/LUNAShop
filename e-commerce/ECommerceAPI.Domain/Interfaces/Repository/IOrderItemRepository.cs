using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IList<OrderItem>> GetOrderItemsByOrderId(int orderId);
        Task<IList<OrderItem>> GetOrderItemsCategory();
        Task<IList<OrderItem>> GetTrendingProducts(DateTime fromDate);
        Task<IList<OrderItem>> GetProductSales(int productId, DateTime fromDate);
        Task<IList<OrderItem>> GetCustomerItems(int customerId);
    }
}
