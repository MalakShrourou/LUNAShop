using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class WishlistModel : BaseModel
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public FilteredProductModel Product { get; set; }
        public int Quantity { get; set; }
    }
}
