using skipper_group_new.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace skipper_group_new.Repositories
{
    public class ProductRepo : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region Product
        public async Task<List<ProductDtl>> GetProdDtl()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@productid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);

                var result = await db.QueryAsync<ProductDtl>(
                    "ProductsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<int> AddProduct(clsProduct product)
        {
            using var conn = new SqlConnection(_connectionString);
            if (product != null)
            {
                int mode = product.ProductId > 0 ? 2 : 1;
                conn.Open();
                var parameters = MapToProductParameters(product, mode);
                await conn.ExecuteAsync("ProductsSP", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@productid");
            }
            return 0;
        }

        public async Task<clsProduct> GetProductById(int productId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string sql = "GetProductById";
                return await conn.QueryFirstOrDefaultAsync<clsProduct>(
                    sql,
                    new { productid = productId },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteProduct(int productId)
        {
            var product = new clsProduct();
            product.ProductId = productId;
            try
            {
                using var conn = new SqlConnection(_connectionString);
                if (product != null)
                {
                    int mode = 3;
                    conn.Open();
                    var parameters = MapToProductParameters(product, mode);
                    await conn.ExecuteAsync("ProductsSP", parameters, commandType: CommandType.StoredProcedure);
                    return parameters.Get<int>("@productid");
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<int> ChangeStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@productid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 7);

                var rows = await db.ExecuteAsync(
                    "ProductsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rows;
            }
        }

        private DynamicParameters MapToProductParameters(clsProduct product, int mode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@productid", product.ProductId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@pcatid", product.PcatId);
            parameters.Add("@psubcatid", product.PsubcatId);
            parameters.Add("@productname", product.ProductName);
            parameters.Add("@producttitle", product.ProductTitle);
            parameters.Add("@modelno", product.ModelNo);
            parameters.Add("@shortdetail", product.ShortDetail);
            parameters.Add("@productdetail", product.ProductDetail);
            parameters.Add("@displayorder", product.DisplayOrder);
            parameters.Add("@rewrite_url", product.RewriteUrl);
            parameters.Add("@PageTitle", product.PageTitle);
            parameters.Add("@PageMeta", product.PageMeta);
            parameters.Add("@PageMetaDesc", product.PageMetaDesc);
            parameters.Add("@prospectus", product.Prospectus);
            parameters.Add("@UploadAImage", product.UploadAImage);
            parameters.Add("@status", product.Status);
            parameters.Add("@purl", product.Purl);
            parameters.Add("@Isfamilyproduct", product.IsFamilyProduct);
            parameters.Add("@showongroup", product.ShowOnGroup);
            parameters.Add("@showonhome", product.ShowOnHome);
            parameters.Add("@largeimage", product.LargeImage);
            parameters.Add("@canonical", product.Canonical);
            parameters.Add("@no_indexfollow", product.NoIndexFollow);
            parameters.Add("@pagescript", product.PageScript);
            parameters.Add("@longdesc", product.LongDesc);
            parameters.Add("@investordate", product.InvestorDate);
            parameters.Add("@ycatid", product.YcatId);
            parameters.Add("@specification", product.Specification);
            parameters.Add("@bannerimg", product.BannerImg);
            parameters.Add("@uname", product.Uname);
            parameters.Add("@Mode", mode);
            return parameters;
        }
        #endregion

        #region Category
        public async Task<List<CategoryDtl>> GetCatDtl()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@pcatid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);

                var result = await db.QueryAsync<CategoryDtl>(
                    "productcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<int> ChangeCatStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@pcatid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);

                var rows = await db.ExecuteAsync(
                    "productcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rows;
            }
        }

        public async Task<int> AddCategory(clsCategory category)
        {
            using var conn = new SqlConnection(_connectionString);
            if (category != null)
            {
                int mode = category.PcatId > 0 ? 2 : 1;
                conn.Open();
                var parameters = MapToCategoryParameters(category, mode);
                await conn.ExecuteAsync("productcateSP", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@pcatid");
            }
            return 0;
        }

        public async Task<clsCategory> GetCategoryById(int categoryID)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string sql = "GetCategoryById";
                return await conn.QueryFirstOrDefaultAsync<clsCategory>(
                    sql,
                    new { pcatid = categoryID },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteCategory(int catid)
        {
            var category = new clsCategory();
            category.PcatId = catid;
            try
            {
                using var conn = new SqlConnection(_connectionString);
                if (category != null)
                {
                    int mode = 3;
                    conn.Open();
                    var parameters = MapToCategoryParameters(category, mode);
                    await conn.ExecuteAsync("productcateSP", parameters, commandType: CommandType.StoredProcedure);
                    return parameters.Get<int>("@pcatid");
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private DynamicParameters MapToCategoryParameters(clsCategory cat, int mode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@pcatid", cat.PcatId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@category", cat.Category);
            parameters.Add("@detail", cat.Detail);
            parameters.Add("@shortdetail", cat.ShortDetail);
            parameters.Add("@displayorder", cat.DisplayOrder);
            parameters.Add("@showonhome", cat.ShowOnHome);
            parameters.Add("@Status", cat.Status);
            parameters.Add("@banner", cat.Banner);
            parameters.Add("@UploadAPDF", cat.UploadAPDF);
            parameters.Add("@PageTitle", cat.PageTitle);
            parameters.Add("@PageMeta", cat.PageMeta);
            parameters.Add("@PageMetaDesc", cat.PageMetaDesc);
            parameters.Add("@rewriteurl", cat.RewriteUrl);
            parameters.Add("@canonical", cat.Canonical);
            parameters.Add("@no_indexfollow", cat.NoIndexFollow);
            parameters.Add("@pagescript", cat.PageScript);
            parameters.Add("@homeimage", cat.HomeImage);
            parameters.Add("@homedesc", cat.HomeDesc);
            parameters.Add("@Uname", cat.Uname);
            parameters.Add("@Mode", mode);
            return parameters;
        }
        #endregion

        #region Sub-Category
        public async Task<Dictionary<int, string>> GetCategoryDrop()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@psubcatid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 9);

                var result = await db.QueryAsync<SubCatDtl>(
                    "productsubcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToDictionary(x => x.PSubCatId, x => x.Category);
            }
        }

        public async Task<int> ChangeSubCatStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@psubcatid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 7);

                return await db.ExecuteAsync(
                    "productsubcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<List<SubCatDtl>> GetSubCategoriesByCategoryId(int categoryId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@pcatid", categoryId);
                parameters.Add("@psubcatid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);

                var result = await db.QueryAsync<SubCatDtl>(
                    "productsubcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<List<SubCatDtl>> GetSubCatDtl()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@psubcatid", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);

                var result = await db.QueryAsync<SubCatDtl>(
                    "productsubcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<SubCategory> GetSubCategoryById(int categoryID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@psubcatid", categoryID, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 8);

                return await db.QueryFirstOrDefaultAsync<SubCategory>(
                    "productsubcateSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }


        public async Task<int> AddSubCategory(SubCategory sub)
        {
            using var conn = new SqlConnection(_connectionString);
            if (sub != null)
            {
                int mode = sub.PSubCatId > 0 ? 2 : 1;
                conn.Open();
                var parameters = MapSubCategoryParameters(sub, mode);
                await conn.ExecuteAsync("productsubcateSP", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@psubcatid");
            }
            return 0;
        }

        public async Task<int> DeleteSubCategory(int catid)
        {
            var category = new SubCategory();
            category.PSubCatId = catid;
            try
            {
                using var conn = new SqlConnection(_connectionString);
                if (category != null)
                {
                    int mode = 3;
                    conn.Open();
                    var parameters = MapSubCategoryParameters(category, mode);
                    await conn.ExecuteAsync("productsubcateSP", parameters, commandType: CommandType.StoredProcedure);
                    return parameters.Get<int>("@pcatid");
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private DynamicParameters MapSubCategoryParameters(SubCategory cat, int mode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@psubcatid", cat.PSubCatId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@pcatid", cat.PCatId);
            parameters.Add("@category", cat.Category);
            parameters.Add("@detail", cat.Detail);
            parameters.Add("@shortdetail", cat.ShortDetail);
            parameters.Add("@displayorder", cat.DisplayOrder);
            parameters.Add("@showonhome", cat.ShowOnHome);
            parameters.Add("@Status", cat.Status);
            parameters.Add("@banner", cat.Banner);
            parameters.Add("@UploadAImage", cat.UploadAImage);
            parameters.Add("@PageTitle", cat.PageTitle);
            parameters.Add("@PageMeta", cat.PageMeta);
            parameters.Add("@PageMetaDesc", cat.PageMetaDesc);
            parameters.Add("@rewriteurl", cat.RewriteUrl);
            parameters.Add("@canonical", cat.Canonical);
            parameters.Add("@no_indexfollow", cat.NoIndexFollow);
            parameters.Add("@pagescript", cat.PageScript);
            parameters.Add("@showonsanden", cat.ShowOnSanden);
            parameters.Add("@position", cat.Position ?? "");
            parameters.Add("@Uname", cat.Uname ?? "system");
            parameters.Add("@Mode", mode);
            return parameters;
        }
        #endregion

        public async Task<int> AddProductType(clsCategory obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("product_typeSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@product_typeid", obj.PcatId);
                    cmd.Parameters.AddWithValue("@product_typetitle", obj.Category);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail);
                    cmd.Parameters.AddWithValue("@description", obj.Detail);
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAPDF);
                    cmd.Parameters.AddWithValue("@banner", obj.Banner);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@status", obj.Status);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc);
                    cmd.Parameters.AddWithValue("@rewriteurl", "");
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@tag1", "");
                    cmd.Parameters.AddWithValue("@tag2", "");
                    cmd.Parameters.AddWithValue("@showonhome", "0");
                    cmd.Parameters.AddWithValue("@showonmenu", "0");
                    cmd.Parameters.AddWithValue("@canonical", obj.Canonical);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<List<CategoryDtl>> GetProductTypeTblData()
        {
            var result = new List<CategoryDtl>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("BindproducttypeSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 👉 If you need to pass parameters:
                // cmd.Parameters.AddWithValue("@Status", 1);

                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new CategoryDtl
                        {
                            PcatId = reader["product_typeid"] != DBNull.Value ? (int)reader["product_typeid"] : 0,
                            Category = reader["product_typetitle"] != DBNull.Value ? reader["product_typetitle"].ToString() : string.Empty,
                            PageTitle = reader["PageTitle"] != DBNull.Value ? reader["PageTitle"].ToString() : string.Empty,
                            trdate = reader["trdate"] != DBNull.Value ? reader["trdate"].ToString() : string.Empty,
                            Status = reader["Status"] != DBNull.Value ? Convert.ToBoolean(reader["Status"]) : false
                        };

                        result.Add(item);
                    }
                }
            }

            return result;
        }
        public int UpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusproducttypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteproducttypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public Task<List<clsCategory>> GetProductTypeList()
        {
            var result = new List<clsCategory>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("BindproducttypeSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();  // <-- synchronous open

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new clsCategory
                        {
                            PcatId = reader["product_typeid"] != DBNull.Value ? (int)reader["product_typeid"] : 0,
                            Category = reader["product_typetitle"]?.ToString() ?? string.Empty,
                            PageTitle = reader["PageTitle"]?.ToString() ?? string.Empty,
                            PageMeta = reader["PageMeta"]?.ToString() ?? string.Empty,
                            PageMetaDesc = reader["PageMetaDesc"]?.ToString() ?? string.Empty,
                            ShortDetail = reader["short_desc"]?.ToString() ?? string.Empty,
                            Detail = reader["description"]?.ToString() ?? string.Empty,
                            DisplayOrder = reader["displayorder"] != DBNull.Value ? (int)reader["displayorder"] : 0,
                            Banner = reader["banner"]?.ToString() ?? string.Empty,
                            UploadAPDF = reader["uploadimage"]?.ToString() ?? string.Empty,
                            Canonical = reader["canonical"]?.ToString() ?? string.Empty,
                            Uname = reader["uname"]?.ToString() ?? string.Empty,
                            Status = reader["Status"] != DBNull.Value && Convert.ToBoolean(reader["Status"])
                        };
                        result.Add(item);
                    }
                }
            }

            return Task.FromResult(result);

        }
        public async Task<DataTable> GetVehicleTyreList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindVehicleTyreList", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int VehicleUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusVehicletypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int VehicleDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteVehicletypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<int> AddVehicleType(SubCategory obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("vehicle_typeSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@product_typeid", obj.PCatId);
                    cmd.Parameters.AddWithValue("@vehicle_typeid", obj.PSubCatId);
                    cmd.Parameters.AddWithValue("@vehicle_typetitle", obj.Category);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail);
                    cmd.Parameters.AddWithValue("@detail_desc", obj.Detail);
                    cmd.Parameters.AddWithValue("@smarttyres", obj.SmartTyre);
                    cmd.Parameters.AddWithValue("@Others", obj.Others);
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage);
                    cmd.Parameters.AddWithValue("@banner", obj.Banner);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@status", obj.Status);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@tag1", "");
                    cmd.Parameters.AddWithValue("@tag2", "");
                    cmd.Parameters.AddWithValue("@showonhome", "0");
                    cmd.Parameters.AddWithValue("@showonmenu", "0");
                    cmd.Parameters.AddWithValue("@uploadfile", "");
                    cmd.Parameters.AddWithValue("@canonical", obj.Canonical);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetBrandList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindBrandListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int BrandUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusBrandSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int BrandDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteBrandSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<int> AddBrand(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("brandsp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@brandid", obj.id);
                    cmd.Parameters.AddWithValue("@brandtitle", obj.title);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetModelList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindModelListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int ModelUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusModelSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int ModelDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteModelSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<int> AddModel(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("modelsp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@modelid", obj.id);
                    cmd.Parameters.AddWithValue("@modeltitle", obj.title);
                    cmd.Parameters.AddWithValue("@brandid", obj.PcatId);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@uploadfile", "");
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetTyreTypeList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindTypretypeListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }

        public int TyreTypeUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusTypreTypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int TyretypeDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteTyreTypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<int> AddTYre_Type(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("tyre_typesp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tyre_typeid", obj.id);
                    cmd.Parameters.AddWithValue("@tyre_typetitle", obj.title);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetPositionList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindPositionSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public async Task<int> AddPosition(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("postingsp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@postingid", obj.id);
                    cmd.Parameters.AddWithValue("@postingtitle", obj.title);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@pagescript", obj.PageScript ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int PositionDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeletePositionSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int PositionUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusPositionSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetDesignTypeList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BinddesignTypeSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int DesignTypeDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeletedesignTypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DesignTypeUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusDesignTypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<int> AdddesignType(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("designtypesp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@designtypeid", obj.id);
                    cmd.Parameters.AddWithValue("@designtypetitle", obj.title);
                    cmd.Parameters.AddWithValue("@short_desc", obj.ShortDetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@rewriteurl", obj.RewriteUrl);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@rewriteurl_sec", "");
                    cmd.Parameters.AddWithValue("@uploadimage", obj.UploadAImage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.PageMeta ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pagemetadesc", obj.PageMetaDesc ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetDealerList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindDealerSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int DealerDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteDealerSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DealerUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusDealerSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<int> AddDealer(clsProduct obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dealer_store_listSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dsid", obj.id);
                    cmd.Parameters.AddWithValue("@branch", obj.branch);
                    cmd.Parameters.AddWithValue("@name", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.DisplayOrder);
                    cmd.Parameters.AddWithValue("@city", obj.city);
                    cmd.Parameters.AddWithValue("@status", obj.Status);
                    cmd.Parameters.AddWithValue("@street", obj.street ?? string.Empty);
                    cmd.Parameters.AddWithValue("@postal_code", obj.postal_code ?? string.Empty);
                    cmd.Parameters.AddWithValue("@telephone", obj.telephone ?? string.Empty);
                    cmd.Parameters.AddWithValue("@email", obj.email ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.Uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<Dictionary<int, string>> GetProductTypeDropDown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 5);

                var result = await db.QueryAsync<ProductDrop>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.product_typeid, x => x.product_typetitle);
            }
        }

        public async Task<Dictionary<int, string>> GetVehicleTypeDropDown(int VID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@VID", VID);
                parameters.Add("@Mode", 6);

                var result = await db.QueryAsync<VehicleDrop>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.vehicle_typeid, x => x.vehicle_typetitle);
            }
        }

        public async Task<Dictionary<int, string>> GetTyreTypeDropDown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 7);

                var result = await db.QueryAsync<TyreDrop>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.tyre_typeid, x => x.tyre_typetitle);
            }
        }

        public async Task<Dictionary<int, string>> GetPositionDropDown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 8);

                var result = await db.QueryAsync<Position>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.postingid, x => x.postingtitle);
            }
        }

        public async Task<Dictionary<int, string>> GetDesignTypeDropDown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 9);

                var result = await db.QueryAsync<Design>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.designtypeid, x => x.designtypetitle);
            }
        }

        public async Task<int> AddEditProductTyre(ProductTyre t)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("product_tyreSP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter("@product_tyreid", SqlDbType.Int);
                if (t.Mode == 1)
                {
                    parm.Direction = ParameterDirection.Output;
                }
                else
                {
                    parm.Direction = ParameterDirection.Input;
                    parm.Value = t.ProductTyreId;
                }
                cmd.Parameters.Add(parm);
                AddParameters(cmd, t);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return t.Mode == 1 ? (int)parm.Value : t.ProductTyreId;
            }
        }

        public async Task<int> ChangeTyreStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 14);

                var rows = await db.ExecuteAsync(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rows;
            }
        }

        private void AddParameters(SqlCommand cmd, ProductTyre product)
        {
            cmd.Parameters.AddWithValue("@product_tyretitle", product.ProductTyreTitle ?? "");
            cmd.Parameters.AddWithValue("@rewriteurl", product.RewriteUrl ?? "");
            cmd.Parameters.AddWithValue("@rewriteurl_sec", product.RewriteUrlSec ?? "");
            cmd.Parameters.AddWithValue("@displayorder", product.DisplayOrder ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@product_typeid", product.ProductTypeId);
            cmd.Parameters.AddWithValue("@vehicle_typeid", product.VehicleTypeId);
            cmd.Parameters.AddWithValue("@tyre_typeid", product.TyreTypeId);
            cmd.Parameters.AddWithValue("@postingid", product.PostingId);
            cmd.Parameters.AddWithValue("@uploadfile", product.UploadFile ?? "");
            cmd.Parameters.AddWithValue("@thumbnail_image", product.ThumbnailImage ?? "");
            cmd.Parameters.AddWithValue("@short_desc", product.ShortDesc ?? "");
            cmd.Parameters.AddWithValue("@detail_desc", product.DetailDesc ?? "");
            cmd.Parameters.AddWithValue("@tagline", product.Tagline ?? "");
            cmd.Parameters.AddWithValue("@review", product.Review ?? "");
            cmd.Parameters.AddWithValue("@uploadimage", product.UploadImage ?? "");
            cmd.Parameters.AddWithValue("@PageTitle", product.PageTitle ?? "");
            cmd.Parameters.AddWithValue("@pagemeta", product.PageMeta ?? "");
            cmd.Parameters.AddWithValue("@pagemetadesc", product.PageMetaDesc ?? "");
            cmd.Parameters.AddWithValue("@status", product.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@showonhome", product.showOnHome ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@no_indexfollow", product.NoIndexFollow);
            cmd.Parameters.AddWithValue("@pagescript", product.PageScript ?? "");
            cmd.Parameters.AddWithValue("@features", product.Features ?? "");
            cmd.Parameters.AddWithValue("@designtype", product.DesignType ?? "");
            cmd.Parameters.AddWithValue("@sizetitle", product.SizeTitle ?? "");
            cmd.Parameters.AddWithValue("@detail_image", product.DetailImage ?? "");
            cmd.Parameters.AddWithValue("@uname", product.UName ?? "");
            cmd.Parameters.AddWithValue("@VID", 0);
            cmd.Parameters.AddWithValue("@Mode", product.Mode);
        }

        public async Task<List<PTyreView>> GetProductTyreList()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 10);
                var result = await db.QueryAsync<PTyreView>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<ProductTyre> GetProductTyrebyID(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 11, DbType.Int32, ParameterDirection.Input);

                var result = await db.QueryAsync<ProductTyre>(
                    "product_tyreSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
        }

        public async Task<int> DeletetyrebyID(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@product_tyreid", id, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Mode", 3, DbType.Int32, ParameterDirection.Input);
                    var affectedRows = await db.ExecuteAsync(
                        "product_tyreSP",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteJob failed: {ex.Message}");
                return -1;
            }
        }

        public async Task<List<BrandListModel>> GetProductSize()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 12);
                var groupedBrands = await db.QueryAsync<BrandListModel>("product_tyreSP", parameters, commandType: CommandType.StoredProcedure);
                return groupedBrands.ToList();
            }
        }

        public async Task<List<BrandModel>> GetBrands(int brandId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@product_tyreid", 0, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@brandid", brandId, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 13);
                var groupedBrands = await db.QueryAsync<BrandModel>("product_tyreSP", parameters, commandType: CommandType.StoredProcedure);
                return groupedBrands.ToList();
            }
        }

        public async Task<List<MapingModel>> GetMapingItem(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Projid", id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@blogid", id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 4);
                var result = await db.QueryAsync<MapingModel>(
                    "product_tyreImageProcedure",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }

        public async Task<List<UTyrePhoto>> GetTyreImage(int id)
        {
            var tPhoto = new List<UTyrePhoto>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@sizeid", id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 1);

                tPhoto = (await db.QueryAsync<UTyrePhoto>(
                    "product_tyreImageProcedure",
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                return tPhoto;
            }
        }

        public async Task<int> UploadTyreImage(UTyrePhoto m)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("productPhotoSP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = new SqlParameter("@photoid", SqlDbType.Int);
                if (m.Mode == 1)
                {
                    parm.Direction = ParameterDirection.Output;
                }
                else
                {
                    parm.Direction = ParameterDirection.Input;
                    parm.Value = m.PhotoID;
                }
                cmd.Parameters.Add(parm);
                AddPhotoParameters(cmd, m);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return m.Mode == 1 ? (int)parm.Value : m.PhotoID;
            }
        }

        private void AddPhotoParameters(SqlCommand cmd, UTyrePhoto m)
        {
            cmd.Parameters.AddWithValue("@product_tyreid", m.ProductTyreID);
            cmd.Parameters.AddWithValue("@photoTitle", m.PhotoTitle ?? "");
            cmd.Parameters.AddWithValue("@Uploadphoto", m.UploadPhoto ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", m.Status);
            cmd.Parameters.AddWithValue("@displayorder", m.DisplayOrder);
            cmd.Parameters.AddWithValue("@largeimage", m.LargeImage ?? "");
            cmd.Parameters.AddWithValue("@sizeid", m.SizeID);
            cmd.Parameters.AddWithValue("@Uname", m.Uname ?? "");
            cmd.Parameters.AddWithValue("@Mode", m.Mode);
        }

        public async Task<int> UploadMultipleTyreImage(UTyrePhoto m)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("product_tyreImageProcedure", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                m.Mode = 2;
                cmd.Parameters.AddWithValue("@photoid", m.PhotoID);
                cmd.Parameters.AddWithValue("@phototitle", m.PhotoTitle ?? string.Empty);
                cmd.Parameters.AddWithValue("@displayorder", m.DisplayOrder);
                cmd.Parameters.AddWithValue("@sizeid", m.SizeID);
                cmd.Parameters.AddWithValue("@Mode", m.Mode);

                await conn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                return rowsAffected;
            }
        }
        public async Task<int> DeleteTyreImage(int photoid)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@photoid", photoid, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Mode", 3, DbType.Int32, ParameterDirection.Input);
                    var affectedRows = await db.ExecuteAsync(
                        "product_tyreImageProcedure",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteJob failed: {ex.Message}");
                return -1;
            }
        }

        public async Task<int> UpdateProductSize(MappingDetail mapping)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("mapproject_brandSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter mmodelParam = new SqlParameter("@mmodelid", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = mapping.MModelId
                    };
                    cmd.Parameters.Add(mmodelParam);
                    cmd.Parameters.AddWithValue("@Projid", mapping.ProjId);
                    cmd.Parameters.AddWithValue("@brandid", mapping.BrandId);
                    cmd.Parameters.AddWithValue("@modelid", mapping.ModelId);
                    cmd.Parameters.AddWithValue("@blogoid", mapping.ProjId);
                    cmd.Parameters.AddWithValue("@uname", mapping.UName ?? "admin");
                    cmd.Parameters.AddWithValue("@Mode", mapping.DesignTypeId ?? 1);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (int)cmd.Parameters["@mmodelid"].Value;
                }
            }
        }

    }

}
