using System.Runtime.CompilerServices;
using MailKit.Net.Smtp;
using MimeKit;
using SubscriptionService.Models;

namespace SubscriptionService.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public EmailService()
        {
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["Email:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Email:SmtpServer"], int.Parse(_config["Email:Port"]), true);
            await smtp.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async void CreateMessage(EmailContentModel EmailModel, List<SubscriptionModel> subs)
        {
            string temp;
            if (EmailModel.IsProject == true)
            {
                temp = $"New Project: {EmailModel.Title}";
                EmailModel.Title = temp;
            }
            else if (EmailModel.IsPhoto == true)
            {
                temp = $"New Photo: {EmailModel.Title}";
                EmailModel.Title = temp;
            }
            else if (EmailModel.IsVideo == true)
            {
                temp = $"New Video: {EmailModel.Title}";
                EmailModel.Title = temp;
            }

            string message = $"<h2>{EmailModel.Title}</h2><p>{EmailModel.Description}</p>";

            foreach (var sub in subs)
            {
                await SendEmailAsync(sub.Email, EmailModel.Title, message);
            }
        }
    }
}
