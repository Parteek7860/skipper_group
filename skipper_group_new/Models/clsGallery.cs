using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsGallery
    {
        public int id { get; set; }
        public string title { get; set; }
        
        public string uname { get; set; }
        public string mode { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? eventsdate { get; set; }
        public int albumtype { get; set; }
        public List<SelectListItem> selectalbumtype { get; set; }
        public string pagetitle { get; set; }
        public string metakeywords { get; set; }
        public string metadesc { get; set; }
        
        public string canonical { get; set; }
        public string shortdetail { get; set; }
        public string bannerimage { get; set; }
        public string uploadbanner { get; set; }
        public string Largeimage { get; set; }
        public string uploadlargeimage { get; set; }
        public string displayorder { get; set; }
        public bool status { get; set; }
        public string URL { get; set; }
        public string albumdesc { get; set; }

        public string searchname { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string tagline { get; set; }
        //public int photoid { get; set; }
        //public bool isselect { get; set; }
    }
}
