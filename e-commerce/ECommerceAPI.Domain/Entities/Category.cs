using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Category :BaseEntity
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string DescriptionAR { get; set; }
        public string DescriptionEN { get; set; }
        public string ImageFilePath { get; set; }
        public bool IsActive { get; set; }
        public IList<Product> Products { get; set; }
    }
}
