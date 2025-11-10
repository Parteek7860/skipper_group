using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsMediatype
    {
        [Required(ErrorMessage = " Fill Media Type")]
        public string mediatype { get; set; }
        public List<SelectListItem> selectmediatype { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage = " Fill display order")]
        public string displayorder { get; set; }
        public string uname { get; set; }
        public string mode { get; set; }
        public int doctype { get; set; }
        public bool status { get; set; }
        public string searchname { get; set; }

        public string eventsttile { get; set; }
        public string startdate { get; set; }

        public string enddate { get; set; }
        [Required(ErrorMessage = " Fill Media Title")]
        public string eventstitle { get; set; }
        public string tagline { get; set; }
        public string colorcode { get; set; }
        public DateTime eventsdate { get; set; }

        public string shortdetail { get; set; }
        public string detail { get; set; }
        public string bannerimage { get; set; }
        public string uploadbanner { get; set; }
        public string pagetitle { get; set; }
        public string metakeywords { get; set; }
        public string metadesc { get; set; }
        public string pagescript { get; set; }
        public string canonical { get; set; }

        public string Largeimage { get; set; }
        public string uploadlargeimage { get; set; }
        public string url { get; set; }
        public bool showonhome { get; set; }
    }
}
