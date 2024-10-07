using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class ArticleModel : BaseModel
    {
        public string TitleEN { get; set; }
        public string ContentEN { get; set; }
        public string TitleAR { get; set; }
        public string ContentAR { get; set; }
    }
}
