using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsInvestor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string uname { get; set; }
        public string yearcategory { get; set; }
        public string displayorder { get; set; }
        public bool status { get; set; }
        public string mode { get; set; }
        public bool showonhome { get; set; }
        public string rewriteurl { get; set; }
        public bool showongroup { get; set; }
        public string ShortDetail { get; set; }
        public string uploadfile { get; set; }
        public string uploadimage { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }

        public string category { get; set; }
       
        public string vediourl { get; set; }
        public string thirdpartyurl { get; set; }
        public string subcategory { get; set; }
       
        public string Quarterly { get; set; }


        [DataType(DataType.Date)]
        public DateTime? investordate { get; set; }
        public string doctype { get; set; }
        [DataType(DataType.Date)]
        public DateTime? newexpiredate { get; set; }


    }
}
