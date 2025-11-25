using DocumentFormat.OpenXml.Wordprocessing;

namespace skipper_group_new.Models
{
    public class clsHomeModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string parentname { get; set; } = "";
        public string Description { get; set; } = "";
        public string SmallDescription { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string eventsdate { get; set; } = "";
        public string productid { get; set; } = "";

        public string? Name { get; set; }
        public string tagline { get; set; }

        public string? linkname { get; set; }
        public string collageid { get; set; }

        public string? rewriteurl { get; set; }

        public string? pageid { get; set; }

        public string? uploadimage { get; set; }
        public string pageurl_Id { get; set; }
        public string? cmscontent { get; set; }
        public string? shortdesc { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string pagedesc2 { get; set; }
        public string pagedesc3 { get; set; }
        public string? id { get; set; }
        public int storeid { get; set; }
        public string phoneno { get; set; }
        public string emailid { get; set; }
        public string emailid2 { get; set; }
        public string address { get; set; }
        public string mobileno { get; set; }
        public string cityname { get; set; }

     
        public List<SubhomeModel> SubMenus { get; set; } = new List<SubhomeModel>();

        public List<SubhomeModel> SubMenus2 { get; set; } = new List<SubhomeModel>();
        public List<SearchItem> SearchResults { get; set; } = new();
        public string megamenu { get; set; }
        public int TotalCount { get; set; }
    }

}
