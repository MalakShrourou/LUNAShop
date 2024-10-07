using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class AddressCommand
    {
        public int CustomerId { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string BuildingNo { get; set; }
    }
}
