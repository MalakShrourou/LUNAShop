using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Request : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
