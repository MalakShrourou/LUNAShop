using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Models
{
    public class AuthenticationModel
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
