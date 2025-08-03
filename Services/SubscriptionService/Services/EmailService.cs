using System.Runtime.CompilerServices;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.SignalR;
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

        public async Task CreateMessage(EmailContentModel EmailModel, List<SubscriptionModel> subs)
        {
            if (EmailModel.IsProject)
                EmailModel.Title = $"New Project: {EmailModel.Title}";
            else if (EmailModel.IsPhoto)
                EmailModel.Title = $"New Photo: {EmailModel.Title}";
            else if (EmailModel.IsVideo)
                EmailModel.Title = $"New Video: {EmailModel.Title}";

            foreach (var sub in subs)
            {
                if(sub.name == null)
                {
                    sub.name = sub.Email;
                }
                string personalizedMessage = $@"
            <div style='font-family:Segoe UI, Roboto, sans-serif; color:#333; line-height:1.6; padding:20px;'>
                <p style='font-size:14px; color:#666;'>Hello <strong>{sub.name}</strong> (User ID: <em>{sub.Id}</em>),</p>

                <h2 style='color:#2C3E50; border-bottom:2px solid #eee; padding-bottom:5px;'>{EmailModel.Title}</h2>

                <p style='font-size:16px;'>
                    <strong>Description:</strong><br />
                    <em>{EmailModel.Description}</em>
                </p>

                <p style='margin-top:30px; font-size:14px; color:#555; text-align:center;'>
    If you wish to unsubscribe from future updates, you may do so by clicking the button below:
</p>

<div style='text-align:center; margin-top:15px;'>
    <a href='https://yourdomain.com/project/'
       style='padding:10px 24px; background-color:#ffffff; color:#2980B9; border:1px solid #2980B9;
              border-radius:4px; text-decoration:none; font-size:14px; font-weight:500; display:inline-block;'>
        Unsubscribe
    </a>
</div>
                <hr style='margin:30px 0; border:none; border-top:1px solid #ccc;' />

                <p style='font-size:12px; color:#999;'>
                    &copy; {DateTime.Now.Year} Mufu's Portfolio.. All rights reserved.
                </p>
            </div>";

                await SendEmailAsync(sub.Email, EmailModel.Title, personalizedMessage);
            }
        }

    }
}
