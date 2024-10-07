using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eCommerceAPI.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        public string FNameAR { get; set; }
        public string LNameAR { get; set; }
        public string FNameEN { get; set; }
        public string LNameEN { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }       
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
