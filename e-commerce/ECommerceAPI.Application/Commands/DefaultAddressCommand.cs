using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class DefaultAddressCommand
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
    }
}
