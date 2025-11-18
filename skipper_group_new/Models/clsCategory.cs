

namespace skipper_group_new.Models
{
    public class clsCategory
    {
        public int PcatId { get; set; }

       
        public string Category { get; set; }

        public string shortname { get; set; }


        public string Detail { get; set; }

        public string ShortDetail { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? ShowOnHome { get; set; }

        public bool? Status { get; set; }

        public string Banner { get; set; }

        public string UploadAPDF { get; set; }
        
        public string? PageTitle { get; set; }

        public string? PageMeta { get; set; }

        public string? PageMetaDesc { get; set; }

        public string? RewriteUrl { get; set; }
        public string? productid { get; set; }

        public string? Canonical { get; set; }

        public bool?  NoIndexFollow { get; set; }

        public string? PageScript { get; set; }

        public string HomeImage { get; set; }

        public string HomeDesc { get; set; }

        public string Uname { get; set; }

        public int? Mode { get; set; }
    }

    public class CategoryDtl
    {
        public int PcatId { get; set; }
        public string Category { get; set; }
        public string PageTitle { get; set; }
        public string trdate { get; set; }
        public bool Status { get; set; }
    }
}
