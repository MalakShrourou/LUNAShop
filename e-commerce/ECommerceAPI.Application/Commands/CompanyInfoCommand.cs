using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class CompanyInfoCommand
    {
        public string Name { get; set; }
        public string ValuesEN { get; set; }
        public string ValuesAR { get; set; }
        public string OverviewEN { get; set; }
        public string OverviewAR { get; set; }
        public string InvolvementEN { get; set; }
        public string InvolvementAR { get; set; }
        public string Logo { get; set; } // logo ImageBase64
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressAR { get; set; }
        public string AddressEN { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string X { get; set; } //twitter
    }
}
