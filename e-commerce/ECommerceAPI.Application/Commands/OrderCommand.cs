using eCommerceAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class OrderCommand
    {
        public int CustomerId { get; set; }
        public string? Notes { get; set; }
        public int AddressId { get; set; }
        public Status Status { get; set; }
        public int? Rate {  get; set; }
        public string? Feedback { get; set; }
        public IList<OrderItemCommand> OrderItems { get; set; }
    }
}
