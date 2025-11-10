namespace skipper_group_new.Models
{  
    public class Management
    {     
        // Team Details
        public int Teamid { get; set; }
        public int TTypeId { get; set; }
        public string Name { get; set; } 
        public string Designation { get; set; }
        public string Qualification { get; set; } 
        public string Nationality { get; set; }  
        public string Headquarter { get; set; }     

        public string Experience { get; set; }  
        public string Industries { get; set; }   

            
        public string DetailDesc { get; set; }  
        public string ShortDesc { get; set; }   

           
        public string UploadPhoto { get; set; } 
        public string UploadPhoto1 { get; set; }

            
        public int DisplayOrder { get; set; }  
        public bool ShowOnHome { get; set; }  
        public bool Status { get; set; }   
        public bool ShowTop { get; set; }  

        // SEO and Meta
        public string PageTitle { get; set; }
        public string PageMeta { get; set; } 
        public string PageMetaDesc { get; set; } 
        public string Canonical { get; set; }  
        public bool NoIndexFollow { get; set; }  

        public string OtherSchema { get; set; }  
        public int TSubTypeId { get; set; }   

        public int CollageId { get; set; }  
        public string Dean { get; set; }   
        public string Principal { get; set; }  
        public string Director { get; set; }  
        public string ExpYear { get; set; }   

        public string UName { get; set; } 
    }
  
    public class ManagementDtl
    {
        public int Teamid { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public class SubteamDtl
    {
        public int TSubTypeId {  get; set; }
        public string Subtype {  get; set; }
    }
}
