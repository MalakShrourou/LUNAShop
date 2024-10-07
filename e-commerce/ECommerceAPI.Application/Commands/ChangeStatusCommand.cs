using eCommerceAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Commands
{
    public class ChangeStatusCommand
    {
        public int Id { get; set; }
        public Status Status { get; set; }
    }
}
