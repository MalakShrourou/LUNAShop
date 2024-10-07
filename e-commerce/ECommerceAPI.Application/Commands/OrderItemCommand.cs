using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class OrderItemCommand
    {
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}
