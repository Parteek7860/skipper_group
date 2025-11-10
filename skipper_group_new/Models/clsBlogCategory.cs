namespace skipper_group_new.Models
{
    public class clsBlogCategory
    {        
        public int BcatId { get; set; }              
        public string BcatTitle { get; set; }      
        public bool Status { get; set; }  
        public int DisplayOrder { get; set; } 
        public string Uname { get; set; }   
    }

    public class Blogcatdtl
    {
        public int BcatId { get; set; }
        public string BcatTitle { get; set; }
        public DateTime? TrDate { get; set; }
        public bool Status { get; set; }
    }
}
