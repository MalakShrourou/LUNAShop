using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class ReviewModel : BaseModel
    {
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public int ProductId { get; set; }
        public int Rate { get; set; }
        public string? TextReview { get; set; }
    }
}
