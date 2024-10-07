using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class ProductModel : BaseModel
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int? UserId { get; set; } // modified user
        public UserModel User { get; set; }
        public IList<ReviewModel>? Reviews { get; set; }
        public bool Availability { get; set; }
        public IList<ProductImageModel> ProductImages { get; set; }
        public int ImageId { get; set; } // ID of Default Image
    }
}
