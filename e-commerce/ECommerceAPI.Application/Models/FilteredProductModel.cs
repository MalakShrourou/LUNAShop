using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class FilteredProductModel
    {
        public int Id { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }
        public double Price { get; set; }
        public double Tax {  get; set; }
        public string ImageBase64 { get; set; }
    }
}
