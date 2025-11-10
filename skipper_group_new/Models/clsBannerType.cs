using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace skipper_group_new.Models
{
    public class clsBannerType
    {
        public int btypeid { get; set; }

        public string btype { get; set; }
        public string status { get; set; }
        public string mobilestatus { get; set; }
        public string collageid { get; set; }

        public List<SelectListItem> bannertypeselect { get; set; }     


        [Required]
        public string displayorder { get; set; }

        public string uname { get; set; }
     
        public string mode { get; set; }


    }
}
