using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsBlog
    {
        public int BlogId { get; set; }
        [Required(ErrorMessage = "Field is required.")]
        public string BlogTitle { get; set; }
        public int? TopicId { get; set; }
        public int? AutId { get; set; }
        public int? CatId { get; set; }
        public int? TagId { get; set; }
        public string CompanyName { get; set; }
        public string BlogImage { get; set; }
        public string LargeImage { get; set; }
        public string SmallDesc { get; set; }
        public string LongDesc { get; set; }
        public DateTime? BlogDate { get; set; }
        public string UrlLink { get; set; }
        public int? DisplayOrder { get; set; }
        public string RewriteUrl { get; set; }
        public string PageTitle { get; set; }
        public string PageMeta { get; set; }
        public string PageMetaDesc { get; set; }
        public bool Status { get; set; }
        public string Canonical { get; set; }
        public bool NoIndexFollow { get; set; }
        public string Uname { get; set; }
        public string PageScript { get; set; }
        public int Mode { get; set; }
    }

    public class BlogDtl
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogImage { get; set; }
        public string SmallDesc { get; set; }
        public DateTime? BlogDate { get; set; }
        public bool Status { get; set; }
    }
}
