using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class RolePermissionsCommand
    {
        public int[] PermissionIds { get; set; }
        public int RoleId { get; set; }
    }
}
