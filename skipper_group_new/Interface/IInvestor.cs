using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Interface
{
    public interface IInvestor
    {
        Task<DataTable> GetCategory();
        Task<List<CategoryDtl>> GetCategoryTblData();
        Task<int> AddCategory(clsCategory category);
        Task<clsCategory> EditCategory(int id);

        Task<int> DeleteCategory(int id);
        Task<int> ChangeCatStatus(int id);

        Task<List<SubCatDtl>> GetSubCategoryTblData();
        Task<int> AddSubCategory(SubCategory category);
        Task<SubCategory> EditSubCategory(int id);
        Task<int> DeleteSubCategory(int id);
        Task<Dictionary<int, string>> GetCatDropdown();
        Task<List<SubCatDtl>> GetSubCategoriesByCategoryId(int catID);
        Task<int> ChangeSubCatStatus(int id);

        Task<DataTable> BindYearCategory();
        int UpdateYearCategoryStatus(string status, int id);
        int DeleteYearCategory(int id); 
        int AddYearCategory(clsInvestor obj);
    }
}
