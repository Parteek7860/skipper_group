using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsCMS
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " Fill Page Name")]
        public string pagename { get; set; }
        [Required(ErrorMessage = " Fill link Name")]
        public string linkname { get; set; }
        public string pageurl { get; set; }
        public int parentid { get; set; }

        public List<SelectListItem> selectparent { get; set; }
        public int doctype { get; set; }
        public string tagline1 { get; set; }
        public string tagline2 { get; set; }
        public string pageposition { get; set; }

        public IList<string> outputparm { get; set; }
        public IList<SelectListItem> linkposition { get; set; }
        public string megamenu { get; set; }
        public string smalldesc { get; set; }
        public string pagedesc { get; set; }
        public string uploadbanner { get; set; }
        public int displayorder { get; set; }
        public string pagetitle { get; set; }
        public string metakeywords { get; set; }
        public string metadesc { get; set; }
        public string pagescript { get; set; }
        public string canonical { get; set; }

        public string uname { get; set; }
        public int mode { get; set; }
        public bool isselect { get; set; }
        public string bannerimage { get; set; }
        public int Level { get; set; }
        public bool PageStatus { get; set; }
        public string rewriteurl { get; set; }

        public string controllername { get; set; }

        public string actionname { get; set; }
    }
}
