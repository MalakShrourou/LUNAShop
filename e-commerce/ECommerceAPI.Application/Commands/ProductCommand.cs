using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class ProductCommand
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int UserId { get; set; } // modified user
        public bool Availability { get; set; }
        public IList<ProductImageCommand> ProductImages { get; set; } = new List<ProductImageCommand>();
        public int ImageId { get; set; } // ID of Default Image
    }
}
