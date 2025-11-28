using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface IProductRepository
    {
        #region Product
        Task<List<ProductDtl>> GetProdDtl();
        Task<int> AddProduct(clsProduct product);
        Task<clsProduct> GetProductById(int id);
        Task<int> DeleteProduct(int id);
        Task<int> ChangeStatus(int id);
        #endregion

        #region Category
        Task<List<CategoryDtl>> GetCatDtl();
      
        Task<int> AddCategory(clsCategory category);
        Task<clsCategory> GetCategoryById(int id);
        Task<int> DeleteCategory(int id);
        Task<int> ChangeCatStatus(int id);
        #endregion

        #region Sub-Category
        Task<List<SubCatDtl>> GetSubCatDtl();
        Task<int> AddSubCategory(SubCategory category);
        Task<SubCategory> GetSubCategoryById(int id);
        Task<int> DeleteSubCategory(int id);
        Task<Dictionary<int, string>> GetCategoryDrop();
        Task<List<SubCatDtl>> GetSubCategoriesByCategoryId(int catID);
        Task<int> ChangeSubCatStatus(int id);
        #endregion
        int AddProductCategory(clsCategory category);
        int CategoryUpdateStatus(string status, int id);
        int CategoryDeleteRecords( int id);
        Task<DataTable> BindProductCategory();
        Task<DataTable> BindProductSubCategory();
        int AddSubProductCategory(SubCategory category);
        int SubCategoryUpdateStatus(string status, int id);
        int SubCategoryDeleteRecords(int id);
        Task<DataTable> BindProductSolution();
        Task<int> AddProductSolution(clsCategory obj);

        Task<List<CategoryDtl>> GetProductTypeTblData();
        int UpdateStatus(string status, int id);

        int DeleteRecords(int id);
        Task<List<clsCategory>> GetProductTypeList();

        Task<DataTable> GetVehicleTyreList();

        int VehicleUpdateStatus(string status, int id);
        int VehicleDeleteRecords(int id);
        Task<int> AddVehicleType(SubCategory obj);

        Task<DataTable> GetBrandList();
        int BrandUpdateStatus(string status, int id);
        int BrandDeleteRecords(int id);
        Task<DataTable> GetModelList();
        int ModelUpdateStatus(string status, int id);
        int ModelDeleteRecords(int id);
        Task<int> AddModel(clsProduct obj);

        Task<int> AddBrand(clsProduct obj);

        Task<DataTable> GetTyreTypeList();
        int TyreTypeUpdateStatus(string status, int id);
        int TyretypeDeleteRecords(int id);
        Task<int> AddTYre_Type(clsProduct obj);
        Task<DataTable> GetPositionList();
        Task<int> AddPosition(clsProduct obj);
        int PositionDeleteRecords(int id);
        int PositionUpdateStatus(string status, int id);
        Task<DataTable> GetDesignTypeList();
        int DesignTypeDeleteRecords(int id);
        int DesignTypeUpdateStatus(string status, int id);
        Task<int> AdddesignType(clsProduct obj);

        Task<DataTable> GetDealerList();
        int DealerDeleteRecords(int id);
        int DealerUpdateStatus(string status, int id);
        Task<int> AddDealer(clsProduct obj);

        Task<Dictionary<int, string>> GetProductTypeDropDown();
        Task<Dictionary<int, string>> GetVehicleTypeDropDown(int vid);
        Task<Dictionary<int, string>> GetTyreTypeDropDown();
        Task<Dictionary<int, string>> GetPositionDropDown();
        Task<Dictionary<int, string>> GetDesignTypeDropDown();
        Task<int> AddEditProductTyre(ProductTyre t);
        Task<int> ChangeTyreStatus(int id);
        Task<List<PTyreView>> GetProductTyreList();
        Task<ProductTyre> GetProductTyrebyID(int id);
        Task<int> DeletetyrebyID(int id);
        Task<List<BrandListModel>> GetProductSize();
        Task<List<MapingModel>> GetMapingItem(int id);
        Task<List<BrandModel>> GetBrands(int brandId);
        Task<List<UTyrePhoto>> GetTyreImage(int id);
        Task<int> UploadTyreImage(UTyrePhoto m);
        Task<int> UploadMultipleTyreImage(UTyrePhoto m);
        Task<int> DeleteTyreImage(int photoid);
        Task<int> UpdateProductSize(MappingDetail mapping);

        Task<DataTable> GetAboutProduct();
        Task<DataTable> GetProductCapabilities();
        Task<int> AddAboutProducts(clsCategory obj);

        Task<int> AddProductsCapabilities(clsCategory obj);
    }
}
