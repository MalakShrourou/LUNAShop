using eCommerceAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public double TotalPrice { get; set; } 
        public Status Status { get; set; } = Status.New;
        public DateTime? DeliveryDate { get; set; }
        public string? Notes { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int? Rate { get; set; }
        public string? Feedback { get; set; }

    }
}
