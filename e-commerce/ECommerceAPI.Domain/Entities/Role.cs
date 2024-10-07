using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IList<Permission> Permissions { get; set; }
        public IList<User>? Users { get; set; }
    }
}
