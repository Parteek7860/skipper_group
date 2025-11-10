using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;
using System.Reflection;

namespace skipper_group_new.Service
{
    public class clsHomePage : IHomePage
    {
        private readonly IHomePageRepository _repository;

        public clsHomePage(IHomePageRepository repository)
        {
            _repository = repository;
        }
        public Task<DataTable> GetSignInDetails(string UserName, string Password)
        {
            return _repository.GetSignInDetails(UserName, Password);
        }
        public Task<DataTable> GetMenuList()
        {
            return _repository.GetMenuList();
        }
        public Task<DataTable> GetFormList(string Moduleid)
        {
            return _repository.GetFormList(Moduleid);
        }
        public Task<DataTable> SearchFormList(string text)
        {
            return _repository.SearchFormList(text);
        }

        public int CreateBannerType(clsBannerType objbannertype)
        {
            return _repository.CreateBannerType(objbannertype);
        }

        public Task<DataTable> GetBannerTypeList()
        {
            return _repository.GetBannerTypeList();
        }
        public Task<DataTable> GetBannerTypeListByID(int id)
        {
            return _repository.GetBannerTypeListByID(id);
        }
        public int DeleteBannerType(int id)
        {
            return _repository.DeleteBannerType(id);
        }

        public int UpdateBannerType(string status, int id)
        {
            return _repository.UpdateBannerType(status, id);
        }
        public Task<DataTable> GetBannerList()
        {
            return _repository.GetBannerList();
        }
        public int DeleteBanner(int id)
        {
            return _repository.DeleteBanner(id);
        }
        public Task<DataTable> GetBannerListByID(int id)
        {
            return _repository.GetBannerListByID(id);
        }
        public Task<DataTable> GetEventTypeList()
        {
            return _repository.GetEventTypeList();
        }
        public Task<DataTable> GetEventList()
        {
            return _repository.GetEventList();
        }
        public int DeleteMediaSection(int id)
        {
            return _repository.DeleteMediaSection(id);
        }
        public int UpdateEventsStatus(string status, int id)
        {
            return _repository.UpdateEventsStatus(status, id);
        }
        public int UpdateEventsStatusShowHome(string status, int id)
        {
            return _repository.UpdateEventsStatusShowHome(status, id);
        }
        public Task<DataTable> GetPageList()
        {
            return _repository.GetPageList();
        }
        public Task<DataTable> GetEnquiryList()
        {
            return _repository.GetEnquiryList();
        }
        public Task<int> ChangeUserPasswordAsync(ChangePasswordModel model)
        {
            return _repository.ChangeUserPasswordAsync(model);
        }
        public int CreateMedia(clsMediatype obj)
        {
            return _repository.CreateMedia(obj);
        }
        public int CreateHomeBaner(clsbanner obj)
        {
            return _repository.CreateHomeBanner(obj);
        }

        public int UpdateBannerStatus(string status, int id)
        {
            return _repository.UpdateBannerStatus(status, id);
        }
        public Task<DataTable> GetTestimonialsList()
        {
            return _repository.GetTestimonialsList();
        }
        public int UpdateTestimonilsStatus(bool status, int id)
        {
            return _repository.UpdateTestimonilsStatus(status, id);
        }
        public int DeleteTestimonils(int id)
        {
            return _repository.DeleteTestimonils(id);
        }
        public int AddTestimonials(clsTestimonial obj)
        {
            return _repository.AddTestimonials(obj);
        }
    }
}
