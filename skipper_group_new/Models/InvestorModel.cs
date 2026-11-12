using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class InvSubCategoryModel
    {
        public int psubcatid { get; set; }
        public string subcategory { get; set; }
        public string subcatrewriteurl { get; set; }
        public int displayorder { get; set; }
    }
    public class InvestorModel
    {
        public int pcatid { get; set; }
        public string category { get; set; }
        public string rewriteurl { get; set; }
        public int displayorder { get; set; }
        public List<InvSubCategoryModel> subcategory { get; set; }
    }
    public class InvestorViewModel
    {
        public EnquiryModel Enquiry { get; set; } = new EnquiryModel();

        public IEnumerable<ReportModel> Reports { get; set; }
            = new List<ReportModel>();
    }

    public class ReportModel
    {
        public int pcatid { get; set; }
        public int psubcatid { get; set; }
        public int productid { get; set; }
        public string prospectus { get; set; }
        public string uploadaimage { get; set; }
        public string purl { get; set; }
        public string title { get; set; }
        public string productname { get; set; }
        public string yearcategory { get; set; }
        public string shortDetail { get; set; }
        public DateTime trdate { get; set; }

        public string FName { get; set; }
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address")]
        public string? EmailId { get; set; }
        public string? Subject { get; set; }
        public string? FMessage { get; set; }
    }

    public class InvestorQueryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Captcha is required")]
        public string CaptchaCode { get; set; }
    }

}
