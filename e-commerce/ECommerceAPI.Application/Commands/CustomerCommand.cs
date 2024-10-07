﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class CustomerCommand
    {
        public string FNameAR { get; set; }
        public string LNameAR { get; set; }
        public string FNameEN { get; set; }
        public string LNameEN { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Phone { get; set; }
        public int? DefaultAddress { get; set; } // ID of Default Address
    }
}
