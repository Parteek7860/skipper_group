using skipper_group_new.mainclass;
namespace skipper_group_new.mainclass
{
    public static class CaptchaHelper
    {
        public static string GenerateCaptcha()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%&*";
            var random = new Random();
            var captcha = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());            
            return captcha;
        }
    }
}
