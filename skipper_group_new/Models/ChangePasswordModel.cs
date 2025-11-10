namespace skipper_group_new.Models
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; } 
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
