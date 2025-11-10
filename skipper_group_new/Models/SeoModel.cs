using DocumentFormat.OpenXml.Office2013.Excel;

namespace skipper_group_new.Models
{
    public class SeoModel
    {
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalUrl { get; set; }
        public string Robots { get; set; }
        public int PageId { get; set; }
    }
}
