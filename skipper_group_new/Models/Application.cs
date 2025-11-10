namespace skipper_group_new.Models
{
    public class Application
    {
        public int App_id { get; set; }                 
        public int? Jobid { get; set; }                 
        public string JobTitle { get; set; }            
        public string Designation { get; set; }         
        public string FName { get; set; }               
        public string LName { get; set; }               
        public DateTime? App_DOB { get; set; }          
        public string Gender { get; set; }              
        public string MaritalStatus { get; set; }       
        public string Father_HusbandName { get; set; }  
        public string App_Address { get; set; }         
        public string Telephone { get; set; }           
        public string Mobile { get; set; }              
        public string App_Email { get; set; }           
        public string City { get; set; }                
        public string State { get; set; }               
        public string App_Qualification { get; set; }   
        public int? App_Expyear { get; set; }           
        public int? App_Expmonth { get; set; }          
        public string App_Yearmonth { get; set; }       
        public string App_Skills { get; set; }          
        public string AttachCV { get; set; }            
        public string Funarea { get; set; }             
        public string Cindustry { get; set; }           
        public string Plocation { get; set; }           
        public string Cemployer { get; set; }           
        public string Csalary { get; set; }             
        public string CurrIndustries { get; set; }      
        public string FuntionalArea { get; set; }       
        public string Areaofintrst { get; set; }        
        public string PrefLocation { get; set; }        
        public int? Countryid { get; set; }             
        public string Uname { get; set; }               
        public DateTime Trdate { get; set; }            
        public string Country { get; set; }            
        public string Empdetails { get; set; }  
        

    }
    public class Applicationview
    {
        public int App_id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string App_Email { get; set; }
        public string App_Qualification { get; set; }
        public int? App_Expyear { get; set; }
        public int? App_Expmonth { get; set; }
        public DateTime Trdate { get; set; }
    }
}

