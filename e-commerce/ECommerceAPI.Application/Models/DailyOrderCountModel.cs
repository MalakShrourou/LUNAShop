using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class DailyOrderCountModel
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
    }

}
