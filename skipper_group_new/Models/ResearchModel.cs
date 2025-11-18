using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace skipper_group_new.Models
{
    public class ResearchModel
    {
        public int ResearchId { get; set; }

        // Basic Data
        [Required(ErrorMessage ="This field is required.")]
        public int NTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string CatId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string ResearchTitle { get; set; }       
        public string? Tagline { get; set; }
        public string? ShortDesc { get; set; }
        public string? ResearchDesc { get; set; }
        public string? Location { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Types { get; set; }

        // Dates
        public DateTime? ResearchSDate { get; set; }
        public DateTime? ResearchEDate { get; set; }

        // Media
        public string? UploadEvents { get; set; }
        public string? UploadFile { get; set; }
        public string? LargeImage { get; set; }
        public string? HomeImage { get; set; }
        public string? VeryLargeImage { get; set; }
        public string? YoutubeUrl { get; set; }

        // Display & Status
        public bool? Status { get; set; }
        public bool? ShowOnHome { get; set; }
        public bool? Archive { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? ShowOnSchool { get; set; }
        public bool? ShowOnGroup { get; set; }

        // SEO
        public string? PageTitle { get; set; }
        public string? PageMeta { get; set; }
        public string? PageMetaDesc { get; set; }
        public string? RewriteUrl { get; set; }
        public string? Canonical { get; set; }
        public bool? NoIndexFollow { get; set; }

        // Other
        public int? LCID { get; set; }
        public string? OtherSchema { get; set; }       

        // Audit
        public string? UName { get; set; }
        public string? ColorCode { get; set; }
        public DateTime? TRDate { get; set; }
        public int? Mode { get; set; }
        //dropdown
        public List<SelectListItem> ?selectProduct { get; set; }
        public List<SelectListItem> ?selectCategory { get; set; }
    }
}
