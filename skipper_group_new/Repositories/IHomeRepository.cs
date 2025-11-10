using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface IHomeRepository
    {
        Task<string> GetPageDescription(int id);

        Task<string> GetSmallDescription(int id);
        Task<List<clsmainmenu>> GetMainMenu();
        Task<DataTable> GetCMSDetail(int id);

        Task<List<clsHomeBanner>> GetHomeBanner();

        int SaveContactDetails(clsContact objML_contact);

        Task<List<clsHomeModel>> NewsEventsList();
        Task<List<clsHomeModel>> ProductList(bool s);
        Task<List<clsHomeModel>> Products();
        //Task<DataTable> GetPageList();
    }
}
