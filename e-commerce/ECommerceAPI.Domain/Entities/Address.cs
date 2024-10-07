using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Address : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string BuildingNo { get; set; }
    }
}
