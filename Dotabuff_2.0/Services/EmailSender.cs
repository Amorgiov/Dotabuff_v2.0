using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace Dotabuff_2._0.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Временная заглушка для отправки email (ничего не делает)
            return Task.CompletedTask;
        }
    }
}
