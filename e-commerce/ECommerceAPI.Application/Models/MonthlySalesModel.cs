using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class MonthlySalesModel
    {
        public DateTime YearMonth { get; set; } // YYYY-MM
        public double TotalSales { get; set; }
    }
}
