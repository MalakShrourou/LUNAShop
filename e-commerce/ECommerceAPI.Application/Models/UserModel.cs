using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class UserModel : BaseModel
    {
        public string FNameAR { get; set; }
        public string LNameAR { get; set; }
        public string FNameEN { get; set; }
        public string LNameEN { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public Role Role { get; set; }
    }
}
