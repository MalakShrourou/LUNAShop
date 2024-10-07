using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class RequestCommand
    {
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
