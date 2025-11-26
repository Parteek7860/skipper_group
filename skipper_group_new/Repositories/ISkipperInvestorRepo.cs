using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface ISkipperInvestorRepo
    {
        // RAKESH CHAUHAN - 19/11/2025
        Task<DataTable> GetCategoryItem();
        Task<DataTable> GetSubCategoryItem(int id);
        Task<DataTable> GetReports(int pcatid, int psubcatid);
        Task<DataTable> GetCategoryDetail(int pcatid);
        Task<int> SaveQueryData(InvestorQueryModel model);
    }
}
