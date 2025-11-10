using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsReview
    {
        public int Id { get; set; }
        public string username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string emailid { get; set; }
        public string reviewdesc { get; set; }
        public int Rating { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string mobileno { get; set; }
        public string title { get; set; }
        public bool status { get; set; }
        public string mode { get; set; }    
    }
}
