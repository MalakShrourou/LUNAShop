using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class PolicyModel :BaseModel
    {
        public string TitleEN { get; set; }
        public string FileBase64EN { get; set; }
        public string TitleAR { get; set; }
        public string FileBase64AR { get; set; }
    }
}
