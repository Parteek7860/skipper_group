
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsbanner
    {
        public int id { get; set; }
        public string d_id { get; set; }
        [Required]
        public string name { get; set; }
        public string bannertype { get; set; }

        public List<SelectListItem> bannertypeselect { get; set; }
        public string devicetype1 { get; set; }

        public List<SelectListItem> devicetype { get; set; }
        public string shortdesc { get; set; }


        public string uploadimage { get; set; }

        public string uploadbanner { get; set; }

        [Required]
        public string displayorder { get; set; }

        public string status { get; set; }

        public string bannerlogo { get; set; }

        public string uname { get; set; }

        public string mode { get; set; }
        public string url { get; set; }
        public IFormFile ImageFile { get; set; }


        public string startdate { get; set; }

        public string enddate { get; set; }

        public string btype { get; set; }

        public int bannertypeid { get; set; }
        public string message { get; set; }
        public int doctype { get; set; }

        public string searchname { get; set; }

        public string projectname { get; set; }

        public List<SelectListItem> selectproject { get; set; }
    }
}
