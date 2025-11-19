using skipper_group_new.Models;
using System.Data;
using System.Threading.Tasks;
namespace skipper_group_new.Repositories
{
    public interface ISkipperHomeRepository
    {
        Task<DataTable> GetMenuList();
      

        Task<DataTable> GetSubMenuList();

        Task<DataTable> GetHamburgerMenuList();

        Task<DataTable> GetCMSData();

        Task<DataTable> GetSeoFriendlyUrls();

        Task<DataTable> GetProjectsList();
    }
}
