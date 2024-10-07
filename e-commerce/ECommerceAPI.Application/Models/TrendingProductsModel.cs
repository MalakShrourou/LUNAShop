using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class TrendingProductsModel
    {
        public int ProductId { get; set; }
        public string ProductNameEN { get; set; }
        public string ProductNameAR { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
