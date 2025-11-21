using skipper_group_new.Interface;

using skipper_group_new.Models;
using skipper_group_new.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;
using System.Threading.Tasks;

namespace skipper_group_new.Service
{
    public class clsSkipperHome : ISkipperHome
    {
        private readonly ISkipperHomeRepository _repository;

        public clsSkipperHome(ISkipperHomeRepository repository) => this._repository = repository;

        public Task<DataTable> GetMenuList() => this._repository.GetMenuList();

        public Task<DataTable> GetSubMenuList() => this._repository.GetSubMenuList();

        public Task<DataTable> GetHamburgerMenuList() => this._repository.GetHamburgerMenuList();

        public Task<DataTable> GetCMSData() => this._repository.GetCMSData();

        public Task<DataTable> GetSeoFriendlyUrls() => this._repository.GetSeoFriendlyUrls();

        public Task<DataTable> GetProjectsList() => this._repository.GetProjectsList();
        public Task<DataTable> GetCarrer() => this._repository.GetCarrer();
        public int SaveEnquiryDetails(EnquiryModel model) => this._repository.SaveEnquiryDetails(model);
        public int SaveContactEnquiry(EnquiryModel model) => this._repository.SaveContactEnquiry(model);
        public Task<DataTable> GetInvestorList() => this._repository.GetInvestorList();
        public Task<DataTable> GetProductList() => this._repository.GetProductList();
        public Task<DataTable> GetProductCategoryList() => this._repository.GetProductCategoryList();
        public Task<DataTable> GetProductSubCategoryList() => this._repository.GetProductSubCategoryList();
    }
}
