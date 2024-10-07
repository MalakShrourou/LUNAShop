﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class BrandCommand
    {
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string ImageBase64 { get; set; }
        public bool IsActive { get; set; }
    }
}
