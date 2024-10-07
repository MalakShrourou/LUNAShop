using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageFilePath { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
