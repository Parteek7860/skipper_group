using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsContact
    {
        public int Id { get; set; }
        public string title { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string Phone { get; set; }        
        public string Message { get; set; }
        public string country { get; set; }
        public string Company { get; set; }
        public string city { get; set; }

        [BindProperty]
        public string CaptchaInput { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a type")]
        [RegularExpression("^(?!0$).*", ErrorMessage = "Please select a valid type1111")]
        public string typename { get; set; }

        public string CaptchaCode { get; set; }
        public string cmscontent { get; set; }
        public string HiddenCaptchaCode { get; set; }
        public string Name { get; set; }

    }
}
