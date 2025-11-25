using skipper_group_new.Models;
using System.Data;
using System.Threading.Tasks;

namespace skipper_group_new.Interface
{
    public interface ISkipperHome
    {
        Task<DataTable> GetMenuList();

        Task<DataTable> GetProjectsList();

        Task<DataTable> GetSubMenuList();

        Task<DataTable> GetHamburgerMenuList();

        Task<DataTable> GetCMSData();

        Task<DataTable> GetSeoFriendlyUrls();
        Task<DataTable> GetCarrer();
        int SaveEnquiryDetails(EnquiryModel model);
        int SaveContactEnquiry(EnquiryModel model);

        Task<DataTable> GetInvestorList();
        Task<DataTable> GetProductList();
        Task<DataTable> GetProductCategoryList();
        Task<DataTable> GetProductSubCategoryList();
        Task<DataTable> GetNewsEvents();
        Task<DataTable> GetBannerList();
    }
}
