using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace skipper_group_new.Models
{
    public class JobApplicationModel
    {
       
        public int App_id { get; set; }        
        public int Jobid { get; set; }

        [Required(ErrorMessage = "Job Title is required")]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "First Name should contain only letters")]
        [StringLength(50)]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Last Name should contain only letters")]
        [StringLength(50)]
        public string LName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime App_DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string gender { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Father/Husband Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name should contain only letters")]
        public string Father_HusbandName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string App_Address { get; set; }

        [RegularExpression(@"^[0-9]{6,12}$", ErrorMessage = "Enter a valid Telephone number")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string App_Email { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        public string App_Qualification { get; set; }

        [Range(0, 50, ErrorMessage = "Experience years must be between 0 and 50")]
        public int? App_Expyear { get; set; }

        [Range(0, 11, ErrorMessage = "Experience months must be between 0 and 11")]
        public int? App_Expmonth { get; set; }

        [StringLength(200, ErrorMessage = "Skills cannot exceed 200 characters")]
        public string App_Skills { get; set; }

        [Required(ErrorMessage = "Please upload your CV")]
        public IFormFile AttachCV { get; set; }

        public string? ResumePath { get; set; }

        public string cemployer { get; set; }
        
        public string csalary { get; set; }

        public string funarea { get; set; }
        public string cindustry { get; set; }
        public string Areaofintrst { get; set; }
        public string plocation { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Captcha is required")]
        public string CaptchaCode { get; set; }        
        public string Designation { get; set; }

        public string? CaptchaDisplay { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TrDate { get; set; }

        public string? UName { get; set; }
        public string? Name { get; set; }
    }
}
