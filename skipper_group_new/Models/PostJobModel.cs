using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class PostJobModel
    {
        public int Jobid { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string jobcategory { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string designation { get; set; }
        [Required(ErrorMessage = "Job Code is required")]
        public string JobCode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Qualification { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Min_Expyear { get; set; }
        public int Min_Expmonth { get; set; }
        public int Max_Expyear { get; set; }
        public int Max_Expmonth { get; set; }


        [Required(ErrorMessage = "Salary is required")]
        public string salary { get; set; }

        [Required(ErrorMessage = "Skills are required")]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string emailid { get; set; }

        [Required(ErrorMessage = "Job Opening Date is required")]
        [DataType(DataType.Date)]
        public DateTime JobOpening_date { get; set; }

        [Required(ErrorMessage = "Job Closing Date is required")]
        [DataType(DataType.Date)]
        public DateTime JobClosing_date { get; set; }
        public string JobDesc { get; set; }
        public int Ageyear { get; set; }
        public int Agemonth { get; set; }
        public bool Status { get; set; }
        public int displayorder { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string shortdesc { get; set; }
        public string rewriteurl { get; set; }
        public string NoOfVacancies { get; set; }
        public string EmpTypeId { get; set; }
        public string Uname { get; set; }
        public DateTime? trDate { get; set; }
        public int? Mode { get; set; }
    }
}
