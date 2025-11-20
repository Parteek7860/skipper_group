using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace skipper_group_new.Models
{
    public class EnquiryModel
    {
        public int Eid { get; set; }
        public string OrganizationName { get; set; }
        public string FName { get; set; }
        public string lastname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
    ErrorMessage = "Please enter a valid email address")]
        public string EmailId { get; set; }
        public string Subject { get; set; }
        public string FMessage { get; set; }        
        public DateTime trdate { get; set; }

        public string zipcode { get; set; }
        public string address { get; set; }
        public string company {  get; set; }
        public string city { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string phone { get; set; }
        public string country { get; set; }
        public String state { get; set; }
        public string capacha { get; set; }
        [BindProperty]
        public string CaptchaInput { get; set; }
        public string CaptchaCode { get; set; }
        public string uploadfile { get; set; }

    }

    public class ProductEnquiryModel
    {
        public int Eid { get; set; }

        public string? FName { get; set; }
                     
        public string? EmailId { get; set; }
                     
        public string? OrganizationName { get; set; }
                     
        public string?  ProductName { get; set; }
                     
        public string? FMessage { get; set; }

        public DateTime TrDate { get; set; }


    }
}
