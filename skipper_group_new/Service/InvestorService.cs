using DocumentFormat.OpenXml.InkML;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;
using System.Web;

namespace skipper_group_new.Service
{
    public class InvestorService : IInvestor
    {
        private readonly IInvestorRepository _repository;

        public InvestorService(IInvestorRepository repository)
        {
            _repository = repository;
        }
        public Task<DataTable> GetCategory()
        {
            return _repository.GetCategory();
        }
        public Task<int> AddCategory(clsCategory category)
        {
            return _repository.AddCategory(category);
        }
        public Task<clsCategory> EditCategory(int id)
        {
            return _repository.EditCategory(id);
        }
        public Task<int> DeleteCategory(int id)
        {
            return _repository.DeleteCategory(id);
        }
        public Task<List<CategoryDtl>> GetCategoryTblData()
        {
            return _repository.GetCatDtl();
        }
        public Task<int> ChangeCatStatus(int id)
        {
            return _repository.ChangeCatStatus(id);
        }
        public Task<List<SubCatDtl>> GetSubCategoryTblData()
        {
            return _repository.GetSubCatDtl();
        }

        public Task<int> AddSubCategory(SubCategory category)
        {
            return _repository.AddSubCategory(category);
        }

        public Task<SubCategory> EditSubCategory(int id)
        {
            return _repository.GetSubCategoryById(id);
        }

        public Task<int> DeleteSubCategory(int id)
        {
            return _repository.DeleteSubCategory(id);
        }
        public Task<Dictionary<int, string>> GetCatDropdown()
        {
            return _repository.GetCategoryDrop();
        }
        public Task<List<SubCatDtl>> GetSubCategoriesByCategoryId(int catID)
        {
            return _repository.GetSubCategoriesByCategoryId(catID);
        }
        public Task<int> ChangeSubCatStatus(int id)
        {
            return _repository.ChangeSubCatStatus(id);
        }
        public Task<DataTable> BindYearCategory()
        {
            return _repository.BindYearCategory();
        }
        public int UpdateYearCategoryStatus(string status, int id)
        {
            return _repository.UpdateYearCategoryStatus(status, id);
        }
        public int DeleteYearCategory(int id)
        {
            return _repository.DeleteYearCategory(id);
        }
        public int AddYearCategory(clsInvestor obj)
        {
            return _repository.AddYearCategory(obj);
        }
    }
}
