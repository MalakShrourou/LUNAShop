using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class ReviewCommand
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Rate { get; set; }
        public string? TextReview { get; set; }
    }
}
