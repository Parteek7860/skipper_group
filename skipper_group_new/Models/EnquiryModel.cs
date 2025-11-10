namespace skipper_group_new.Models
{
    public class EnquiryModel
    {
        public int Eid { get; set; }
        public string OrganizationName { get; set; }
        public string FName { get; set; }
        public string EmailId { get; set; }
        public string Subject { get; set; }
        public string FMessage { get; set; }        
        public DateTime trdate { get; set; }


    }

    public class ProductEnquiryModel
    {
        public int Eid { get; set; }

        public string? FName { get; set; }
                     
        public string? EmailId { get; set; }
                     
        public string? OrganizationName { get; set; }
                     
        public string?  ProductName { get; set; }
                     
        public string? FMessage { get; set; }

        public DateTime TrDate { get; set; }


    }
}
