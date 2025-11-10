using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsProduct
    {
        public int ProductId { get; set; }
        public int PcatId { get; set; }
        public int? PsubcatId { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "This Field is required.")]
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public string ShortDetail { get; set; }
        public string ProductDetail { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string RewriteUrl { get; set; }
        public string PageTitle { get; set; }
        public string PageMeta { get; set; }
        public string PageMetaDesc { get; set; }
        public string Prospectus { get; set; }
        public string UploadAImage { get; set; }
        public string Status { get; set; }
        public string Purl { get; set; }
        public bool IsFamilyProduct { get; set; }
        public bool ShowOnGroup { get; set; }
        public bool ShowOnHome { get; set; }
        public string LargeImage { get; set; }
        public string Canonical { get; set; }
        public bool NoIndexFollow { get; set; }
        public string PageScript { get; set; }
        public string LongDesc { get; set; }
        public DateTime? InvestorDate { get; set; }
        public int? YcatId { get; set; }
        public string Specification { get; set; }
        public string BannerImg { get; set; }
        public string Uname { get; set; }
        public decimal Mode { get; set; }

        public string title { get; set; }
        public string branch { get; set; }
        public string city { get; set; }
        public string telephone { get; set; }
        public string postal_code { get; set; }
        public string street { get; set; }
        public string email { get; set; }
        public int id { get; set; }
    }

    public class ProductDtl
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public string trdate { get; set; }
        public bool Status { get; set; }
    }

    public class ProductTypeModel
    {
        public string PID { get; set; }
        public string Title { get; set; }
        public string RewriteUrl { get; set; }
        public string Image { get; set; }
        public int ProductTypeId { get; set; }
    }
}
