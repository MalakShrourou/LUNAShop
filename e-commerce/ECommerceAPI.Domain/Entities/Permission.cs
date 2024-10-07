﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class Permission 
    {
        public int Id { get; set; }
        public string Action {  get; set; }
        public IList<Role> Roles { get; set; }
    }
}
