using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task ChangeStatus(int id, Status status);
        Task<IList<Order>> GetCustomerOrders(int customerId);
        Task<IList<Order>> GetSalesByDeliveryDate(DateTime startDate, DateTime endDate);
        Task<IList<Order>> GetOrderStatus();
    }
}
