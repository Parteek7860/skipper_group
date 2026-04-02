using Microsoft.Extensions.Options;
using skipper_group_new.Models;
using System.Net;
using System.Net.Mail;

namespace skipper_group_new.Service
{
    public class EmailService
    {
        private readonly SmtpSettings _smtp;
        private readonly IWebHostEnvironment _env;


        public EmailService(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            _smtp = smtpSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string attachment = "")
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtp.FromEmail, _smtp.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);
            if (_smtp.IsAdminCcEmail)
            {
                message.CC.Add(_smtp.CcEmail);
            }
            if (!string.IsNullOrEmpty(attachment))
            {
                var filePath = Path.Combine(_env.WebRootPath, "uploads", "files", attachment);

                if (File.Exists(filePath))
                {
                    var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    var attach = new Attachment(stream, Path.GetFileName(filePath));
                    message.Attachments.Add(attach);
                }

            }
            var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtp.UserName, "^k1s87#$90B@"),
                EnableSsl = _smtp.EnableSsl,
                

            };
            if (_smtp.IsMail == true)
            {
                await client.SendMailAsync(message);
            }

        }
    }
}
