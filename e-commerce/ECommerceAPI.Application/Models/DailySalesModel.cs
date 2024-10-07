using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class DailySalesModel
    {
        public DateTime Date { get; set; }
        public double TotalSales { get; set; }
    }
}
