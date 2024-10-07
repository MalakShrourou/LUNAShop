using eCommerceAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class OrderModel : BaseModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public double TotalPrice { get; set; }
        public Status Status { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public int? Rate { get; set; }
        public string? Feedback { get; set; }
        public IList<OrderItemModel> OrderItems { get; set; }
        public int AddressId { get; set; }
        public AddressModel Address { get; set; }
    }
}
