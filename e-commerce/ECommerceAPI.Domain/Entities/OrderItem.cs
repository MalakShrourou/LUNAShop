using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int? OrderId { get; set; }
        public Order Order {  get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int Quantity { get; set; }
        public double NetPrice => (Product.Price + (Product.Price * (Product.Tax/100))) * Quantity;
    }
}
