using System.Data;
using skipper_group_new.Models;

namespace skipper_group_new.Repositories
{
    public interface IHomePageRepository
    {
        Task<DataTable> GetSignInDetails(string username, string password);
        Task<DataTable> GetMenuList();

        Task<DataTable> GetFormList(string moduleid);
        Task<DataTable> SearchFormList(string text);
        int CreateBannerType(clsBannerType objbannertype);

        int CreateMedia(clsMediatype obj);

        int AddTestimonials(clsTestimonial obj);

        int DeleteBannerType(int id);

        int UpdateBannerType(string status,int id);

        Task<DataTable> GetBannerTypeList();

        Task<DataTable> GetBannerList();
        Task<DataTable> GetBannerTypeListByID(int id);
        int DeleteBanner(int id);
        Task<DataTable> GetBannerListByID(int id);
        Task<DataTable> GetEventTypeList();
        Task<DataTable> GetEventList();
        Task<DataTable> GetEnquiryList();
        int DeleteMediaSection(int id);
        int UpdateEventsStatus(string status, int id);
        int UpdateEventsStatusShowHome(string status, int id);
        Task<DataTable> GetPageList();
        Task<DataTable> GetTestimonialsList();
        Task<int> ChangeUserPasswordAsync(ChangePasswordModel model);

        int CreateHomeBanner(clsbanner obj);

        int UpdateBannerStatus(string status, int id);
        int UpdateTestimonilsStatus(bool status, int id);
        int DeleteTestimonils(int id);
    }
}
