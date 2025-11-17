using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface IInvestorRepository
    {
        Task<DataTable> GetCategory();
        Task<List<CategoryDtl>> GetCatDtl();
        Task<int> AddCategory(clsCategory category);
        Task<clsCategory> EditCategory(int id);
        Task<int> DeleteCategory(int id);
        Task<int> ChangeCatStatus(int id);

        Task<List<SubCatDtl>> GetSubCatDtl();
        Task<DataTable> GetSubCategory();
        Task<int> AddSubCategory(SubCategory category);
        Task<SubCategory> GetSubCategoryById(int id);
        Task<int> DeleteSubCategory(int id);
        Task<Dictionary<int, string>> GetCategoryDrop();
        Task<List<SubCatDtl>> GetSubCategoriesByCategoryId(int catID);
        Task<int> ChangeSubCatStatus(int id);
        Task<DataTable> BindYearCategory();
        int UpdateYearCategoryStatus(string status, int id);
        int DeleteYearCategory(int id);
        int AddYearCategory(clsInvestor obj);
        Task<DataTable> BindInvestor();
        int UpdateInvestorStatus(string status, int id);
        int DeleteINvestor(int id);
        int UpdateInvestorShowonHome(string status, int id);
        int AddInvestor(clsInvestor obj);
    }
}
