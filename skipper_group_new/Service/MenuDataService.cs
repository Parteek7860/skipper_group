using skipper_group_new.Interface;
using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Service
{
    public class MenuDataService
    {
        public List<clsHomeModel> TopMenuList { get; set; } = new();
        public List<clsHomeModel> MainMenuList { get; set; } = new();
        public List<clsHomeModel> HamBurgerList { get; set; } = new();
        public List<clsHomeModel> RightHamBurgerList { get; set; } = new();

        public List<SeoModel> SeoList { get; set; } = new();

        public List<clsHomeModel> GetCMSData { get; set; } = new();
        public List<clsHomeModel> MobileMenu { get; set; } = new();
        public List<clsProduct> FooterList { get; set; } = new();
    }
}
