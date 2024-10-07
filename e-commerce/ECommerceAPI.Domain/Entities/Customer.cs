using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class Customer : BaseEntity
    {
        public string FNameAR { get; set; }
        public string LNameAR { get; set; }
        public string FNameEN { get; set; }
        public string LNameEN { get; set; }
        public string Email { get; set;}
        public string Password { get; set;}
        public string Phone { get; set;}
        public DateTime? LastOrder {  get; set;}
        public DateTime LastLogin {  get; set;}
        public bool IsActive { get; set;}
        public IList<Order>? Orders { get; set;}
        public IList<Address>? Addresses { get; set;}
        public int? DefaultAddress {  get; set; } // ID of Default Address
        public IList<Review>? Reviews { get; set;}
        public int RoleId { get; set;}
        public Role Role { get; set;}
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
