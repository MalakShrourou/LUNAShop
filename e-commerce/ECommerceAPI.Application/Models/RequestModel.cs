using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class RequestModel : BaseModel
    {
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
