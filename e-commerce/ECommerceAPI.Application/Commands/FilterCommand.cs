using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class FilterCommand
    {
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public int? BrandId { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
        public string? UserLang { get; set; }
    }
}
