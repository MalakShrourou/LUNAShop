using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceAPI.Domain.Entities;

namespace eCommerceAPI.Domain.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationLink);
        Task SendOrderConfirmationEmail(Order order);

    }
}
