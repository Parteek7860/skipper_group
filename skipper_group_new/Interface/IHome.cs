using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Interface
{
    public interface IHome
    {
        Task<List<clsmainmenu>> GetMainManu();
        Task<string> GetPageDescription(int id);

        Task<string> GetSmallDescription(int id);

        Task<DataTable> GetCMSDetail(int id);
        Task<List<clsHomeBanner>> GetHomeBanner();
        int SaveContactDetails(clsContact objML_contact);

        Task<List<clsHomeModel>> NewsEventsList();

        Task<List<clsHomeModel>> ProductList(bool s);

        Task<List<clsHomeModel>> Products();


    }
}
