using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Services.EmailService
{
    public class DummyEmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log or ignore for now
            Console.WriteLine($"Email to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
