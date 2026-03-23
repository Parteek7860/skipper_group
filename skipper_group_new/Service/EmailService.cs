using Microsoft.Extensions.Options;
using skipper_group_new.Models;
using System.Net;
using System.Net.Mail;

namespace skipper_group_new.Service
{
    public class EmailService
    {
        private readonly SmtpSettings _smtp;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtp = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool IsUser = false)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtp.FromEmail, _smtp.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);
            //if (IsUser == false)
            //{
            //    message.CC.Add(_smtp.CcEmail);
            //}


            var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtp.UserName, "^k1s87#$90B@"),
                EnableSsl = _smtp.EnableSsl
            };

            await client.SendMailAsync(message);
        }
    }
}
