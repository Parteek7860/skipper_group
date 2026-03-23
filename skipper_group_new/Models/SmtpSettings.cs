namespace skipper_group_new.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string ToEmail { get; set; } = string.Empty;
        public string CcEmail { get; set; } = string.Empty;
    }
}
