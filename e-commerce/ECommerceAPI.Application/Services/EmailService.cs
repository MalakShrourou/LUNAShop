using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    
    private string BuildOrderConfirmationEmailBody(Order order)
    {
        return $@"
            <h2>Thank you for your order!</h2>
            <p>Order Number: {order.Id}</p>
            <p>Order Total: {order.TotalPrice:C}</p>
            <h3>Order Summary</h3>
            <ul>
                {string.Join("", order.OrderItems.Select(item => $"<li>{item.Product.NameEN} - {item.Quantity} - {item.NetPrice:C}</li>"))}
            </ul>
            <h3>Shipping Address</h3>
            <p>{order.Address.StreetName}, {order.Address.BuildingNo}, {order.Address.City}</p>";
    }
    
    public Task SendOrderConfirmationEmail(Order order)
    {
        try
        {
            string subject = $"Order Confirmation - {order.Id}";
            string body = BuildOrderConfirmationEmailBody(order);

            SmtpClient client = new SmtpClient("smtp.office365.com", 465)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mshrourou@outlook.com", "Shrourou#4")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mshrourou@outlook.com",
                to: order.Customer.Email,
                subject,
                body
                ));
        }
        catch (SmtpFailedRecipientException ex)
        {
            Console.WriteLine($"Failed to deliver message to {ex.FailedRecipient}: {ex.Message}");
            throw;
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP error: {ex.Message}. Check if the SMTP server requires secure authentication.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

    public Task SendVerificationEmailAsync(string email, string verificationLink)
    {
        try
        {
            string subject = "Email Verification";
            string message = $"Please verify your email by clicking the link: <a href='{verificationLink}'>Verify Email</a>";

            SmtpClient client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mshrourou@outlook.com", "Shrourou#4")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mshrourou@outlook.com",
                to: email,
                subject,
                message
                ));
        }
        catch (SmtpFailedRecipientException ex)
        {
            Console.WriteLine($"Failed to deliver message to {ex.FailedRecipient}: {ex.Message}");
            throw;
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP error: {ex.Message}. Check if the SMTP server requires secure authentication.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }

}
