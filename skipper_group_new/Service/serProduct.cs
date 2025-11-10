using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;
using System.Web;

namespace skipper_group_new.Service
{
    public class serProduct : IProducts
    {
        private readonly IProductRepository _repository;

        public serProduct(IProductRepository repository)
        {
            _repository = repository;
        }

        #region Product
        public Task<List<ProductDtl>> GetProductTblData()
        {
            return _repository.GetProdDtl();
        }

        public Task<int> AddProduct(clsProduct product)
        {
            return _repository.AddProduct(product);
        }

        public Task<clsProduct> EditProduct(int id)
        {
            return _repository.GetProductById(id);
        }

        public Task<int> DeleteProduct(int id)
        {
            return _repository.DeleteProduct(id);
        }
        public Task<int> ChangeStatus(int id)
        {
            return _repository.ChangeStatus(id);
        }
        #endregion

        #region Category
        public Task<List<CategoryDtl>> GetCategoryTblData()
        {
            return _repository.GetCatDtl();
        }

        public Task<int> AddCategory(clsCategory category)
        {
            return _repository.AddCategory(category);
        }

        public Task<clsCategory> EditCategory(int id)
        {
            return _repository.GetCategoryById(id);
        }

        public Task<int> DeleteCategory(int id)
        {
            return _repository.DeleteCategory(id);
        }

        public Task<int> ChangeCatStatus(int id)
        {
            return _repository.ChangeCatStatus(id);
        }
        #endregion

        #region sub-Category
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
        #endregion

        public Task<int> AddProductType(clsCategory obj)
        {
            return _repository.AddProductType(obj);
        }


        public Task<List<CategoryDtl>> GetProductTypeyTblData()
        {
            return _repository.GetProductTypeTblData();
        }
        public int UpdateStatus(string status, int id)
        {
            return _repository.UpdateStatus(status, id);
        }
        public int DeleteRecords(int id)
        {
            return _repository.DeleteRecords(id);
        }
        public Task<List<clsCategory>> GetProductTypeList()
        {
            return _repository.GetProductTypeList();
        }
        public Task<DataTable> GetVehicleTyreList()
        {
            return _repository.GetVehicleTyreList();
        }
        public int VehicleUpdateStatus(string status, int id)
        {
            return _repository.VehicleUpdateStatus(status, id);
        }
        public int VehicleDeleteRecords(int id)
        {
            return _repository.VehicleDeleteRecords(id);
        }


        Task<int> IProducts.AddVehicleType(SubCategory obj)
        {
            return _repository.AddVehicleType(obj);
        }
        public Task<DataTable> GetBrandList()
        {
            return _repository.GetBrandList();
        }
        public int BrandUpdateStatus(string status, int id)
        {
            return _repository.BrandUpdateStatus(status, id);
        }
        public int BrandDeleteRecords(int id)
        {
            return _repository.BrandDeleteRecords(id);
        }
        public Task<DataTable> GetModelList()
        {
            return _repository.GetModelList();
        }
        public int ModelUpdateStatus(string status, int id)
        {
            return _repository.ModelUpdateStatus(status, id);
        }
        public int ModelDeleteRecords(int id)
        {
            return _repository.ModelDeleteRecords(id);
        }
        Task<int> IProducts.AddModel(clsProduct obj)
        {
            return _repository.AddModel(obj);
        }
        Task<int> IProducts.AddBrand(clsProduct obj)
        {
            return _repository.AddBrand(obj);
        }
        public Task<DataTable> GetTyreTypeList()
        {
            return _repository.GetTyreTypeList();
        }
        public int TyreTypeUpdateStatus(string status, int id)
        {
            return _repository.TyreTypeUpdateStatus(status, id);
        }
        public int TyretypeDeleteRecords(int id)
        {
            return _repository.TyretypeDeleteRecords(id);
        }
        Task<int> IProducts.AddTYre_Type(clsProduct obj)
        {
            return _repository.AddTYre_Type(obj);
        }
        public Task<DataTable> GetPositionList()
        {
            return _repository.GetPositionList();
        }
        Task<int> IProducts.AddPosition(clsProduct obj)
        {
            return _repository.AddPosition(obj);
        }
        public int PositionDeleteRecords(int id)
        {
            return _repository.PositionDeleteRecords(id);
        }
        public int PositionUpdateStatus(string status, int id)
        {
            return _repository.PositionUpdateStatus(status, id);
        }
        public Task<DataTable> GetDesignTypeList()
        {
            return _repository.GetDesignTypeList();
        }
        public int DesignTypeDeleteRecords(int id)
        {
            return _repository.DesignTypeDeleteRecords(id);
        }
        public int DesignTypeUpdateStatus(string status, int id)
        {
            return _repository.DesignTypeUpdateStatus(status, id);
        }
        Task<int> IProducts.AdddesignType(clsProduct obj)
        {
            return _repository.AdddesignType(obj);
        }
        public Task<DataTable> GetDealerList()
        {
            return _repository.GetDealerList();
        }
        public int DealerDeleteRecords(int id)
        {
            return _repository.DealerDeleteRecords(id);
        }
        public int DealerUpdateStatus(string status, int id)
        {
            return _repository.DealerUpdateStatus(status, id);
        }
        Task<int> IProducts.AddDealer(clsProduct obj)
        {
            return _repository.AddDealer(obj);
        }

        public Task<Dictionary<int, string>> GetProductTypeDropDown()
        {
            return _repository.GetProductTypeDropDown();
        }

        public Task<Dictionary<int, string>> GetVehicleTypeDropDown(int Vid)
        {
            return _repository.GetVehicleTypeDropDown(Vid);
        }

        public Task<Dictionary<int, string>> GetTyreTypeDropDown()
        {
            return _repository.GetTyreTypeDropDown();
        }

        public Task<Dictionary<int, string>> GetPositionDropDown()
        {
            return _repository.GetPositionDropDown();
        }

        public Task<Dictionary<int, string>> GetDesignTypeDropDown()
        {
            return _repository.GetDesignTypeDropDown();
        }

        public async Task<int> AddEditProductTyre(ProductTyre t)
        {
            return await _repository.AddEditProductTyre(t);
        }

        public async Task<int> ChangeTyreStatus(int id)
        {
            return await _repository.ChangeTyreStatus(id);
        }
        public async Task<List<PTyreView>> GetProductTyreList()
        {
            return await _repository.GetProductTyreList();
        }

        public async Task<ProductTyre> GetProductTyrebyID(int id)
        {
            var res = await _repository.GetProductTyrebyID(id);
            if (res == null)
                return null;

            var tyre = new ProductTyre
            {
                ProductTyreId = res.ProductTyreId,
                ProductTyreTitle = res.ProductTyreTitle,
                ShortDesc = HttpUtility.HtmlDecode(res.ShortDesc),
                DetailDesc = HttpUtility.HtmlDecode(res.DetailDesc),
                Tagline = res.Tagline,
                Review = HttpUtility.HtmlDecode(res.Review),
                ProductTypeId = res.ProductTypeId,
                VehicleTypeId = res.VehicleTypeId,
                TyreTypeId = res.TyreTypeId,
                PostingId = res.PostingId,
                UploadImage = res.UploadImage,
                UploadFile = res.UploadFile,
                RewriteUrl = res.RewriteUrl,
                RewriteUrlSec = res.RewriteUrlSec,
                DisplayOrder = res.DisplayOrder,
                PageTitle = res.PageTitle,
                PageMeta = res.PageMeta,
                PageMetaDesc = res.PageMetaDesc,
                Status = res.Status,
                showOnHome = res.showOnHome,
                UName = res.UName,
                trdate = res.trdate,
                ThumbnailImage = res.ThumbnailImage,
                PageScript = res.PageScript,
                canonical = res.canonical,
                NoIndexFollow = res.NoIndexFollow,
                Features = HttpUtility.HtmlDecode(res.Features),
                DesignType = res.DesignType,
                SizeTitle = res.SizeTitle,
                DetailImage = res.DetailImage,
                Mode = res.Mode
            };

            return tyre;
        }


        public async Task<int> DeleteTyreByID(int id)
        {
            return await _repository.DeletetyrebyID(id);
        }

        public async Task<int> DeleteTyreImage(int photoid)
        {
            return await _repository.DeleteTyreImage(photoid);
        }
        public async Task<List<ProductBrands>> GetProductSize(int id)
        {
            var list = new List<ProductBrands>();

            var brands = await _repository.GetProductSize();
            var mapingItems = await _repository.GetMapingItem(id);

            foreach (var brand in brands)
            {
                var models = await _repository.GetBrands(brand.brandID);

                if (models != null && models.Any())
                {
                    foreach (var model in models)
                    {
                        var mapping = mapingItems.FirstOrDefault(m =>m.modelid == model.modelID && m.brandid == brand.brandID);
                        model.ProjId = id;               
                        model.BrandId = brand.brandID;   
                        model.BlogoId = mapping != null ? mapping.blogoid : 0;
                        model.Status = mapping != null;
                    }

                    list.Add(new ProductBrands
                    {
                        brandID = brand.brandID,
                        brandTitle = brand.brandTitle,
                        Model = models
                    });
                }
            }

            return list;
        }

        public async Task<List<UTyrePhoto>> GetTyreImage(int id)
        {
            return await _repository.GetTyreImage(id);
        }
        public async Task<int> UploadTyreImage(UTyrePhoto m)
        {
            return await _repository.UploadTyreImage(m);
        }
        public async Task<int> UploadMultipleTyreImage(UTyrePhoto m)
        {
            return await _repository.UploadMultipleTyreImage(m);
        }

        public async Task<int> UpdateProductSize(MappingDetail mapping)
        {
            return await _repository.UpdateProductSize(mapping);
        }
    }
}
