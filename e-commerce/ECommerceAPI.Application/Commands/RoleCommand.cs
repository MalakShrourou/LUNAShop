using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class RoleCommand
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public IList<PermissionCommand> Permissions { get; set; } 

    }
}
