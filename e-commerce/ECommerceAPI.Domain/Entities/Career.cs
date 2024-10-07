using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Career : BaseEntity
    {
        public string TitleEN { get; set; }
        public string TitleAR { get; set; }
        public string DescriptionEN { get; set; }
        public string DescriptionAR { get; set; }
    }
}
