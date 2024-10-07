using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public double Price {  get; set; }
        public double Tax { get; set;}
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int UserId { get; set; } // modified user
        public User User { get; set; }  
        public IList<Review>? Reviews { get; set; }
        public bool Availability { get; set; }
        public IList<ProductImage> ProductImages { get; set; }
        public int ImageId { get; set; } // ID of Default Image
        public IList<Wishlist>? Wishlists { get; set; }
        public IList<OrderItem>? OrderItems { get; set; }
    }
}
