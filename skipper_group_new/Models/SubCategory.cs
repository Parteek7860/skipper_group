using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class SubCategory
    {
        public int PSubCatId { get; set; }

        [Required]
        [Display(Name = "Main Category")]
        public int? PCatId { get; set; }

        [Required]
        [Display(Name = "Subcategory Name")]
        public string Category { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Long Description")]
        public string Detail { get; set; }
        public string homedesc { get; set; }
        public string homedesc2 { get; set; }
        public string tagline { get; set; }

        [Display(Name = "Short Detail")]
        public string ShortDetail { get; set; }

        [Display(Name = "Banner Image")]
        public string Banner { get; set; }

        [Display(Name = "Upload Image")]
        public string UploadAImage { get; set; }

        [Display(Name = "Page Title")]
        public string PageTitle { get; set; }

        [Display(Name = "Page Meta")]
        public string PageMeta { get; set; }

        [Display(Name = "Meta Description")]
        public string PageMetaDesc { get; set; }

        [Display(Name = "Rewrite URL")]
        public string RewriteUrl { get; set; }

        [Display(Name = "Canonical")]
        public string Canonical { get; set; }

        [Display(Name = "No Index/Follow")]
        public bool NoIndexFollow { get; set; }

        [Display(Name = "Page Script")]
        public string PageScript { get; set; }

        [Display(Name = "Show on Sanden")]
        public bool ShowOnSanden { get; set; }

        [Display(Name = "Show on Home")]
        public bool ShowOnHome { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        public string Uname { get; set; }
        public string Mode { get; set; }
        public string SmartTyre { get; set; }

        public string Others { get; set; }
    }

    public class SubCatDtl
    {
        public int PSubCatId { get; set; }
        public int product_type { get; set; }
        public string Category { get; set; }
        public string PageTitle { get; set; }
        public string trdate { get; set; }
        public bool Status { get; set; }
        public string pcatid { get; set; }
        public string categoryname { get; set; }
    }
}
