using Dapper;
using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;

namespace skipper_group_new.Repositories
{
    public class InvestorRepository : IInvestorRepository
    {
        private readonly string _connectionString;

        public InvestorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
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
        public async Task<DataTable> GetCategory()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindInvestorProductCateSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
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

        public async Task<clsCategory> EditCategory(int categoryID)
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
        public async Task<DataTable> GetSubCategory()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindSubCategoryInvestorSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
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
        public async Task<DataTable> BindYearCategory()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindYearCategorySP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int UpdateYearCategoryStatus(string status, int id)
        {
            int result = 0;
            string query = "UpdStatusYearCategorySP";
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
        public int DeleteYearCategory(int id)
        {
            int result = 0;
            string query = "DeleteYearCategorySP";
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
        public int AddYearCategory(clsInvestor obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("yearcategorySP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ycatid", obj.Id);
                    cmd.Parameters.AddWithValue("@category", obj.yearcategory ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> BindInvestor()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindInvestorListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public int UpdateInvestorStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusInvestorSP";
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
        public int DeleteINvestor(int id)
        {
            int result = 0;
            string query = "DeleteInvestorSP";
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
        public int UpdateInvestorShowonHome(string status, int id)
        {
            int result = 0;
            string query = "UpdShowonHomeInvestorSP";
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
        public int AddInvestor(clsInvestor obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ProductsSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productid", obj.Id);
                    cmd.Parameters.AddWithValue("@pcatid", obj.category);
                    cmd.Parameters.AddWithValue("@psubcatid", obj.subcategory);
                    cmd.Parameters.AddWithValue("@ycatid", obj.yearcategory ?? string.Empty);
                    cmd.Parameters.AddWithValue("@yqid", obj.Quarterly ?? string.Empty);
                    cmd.Parameters.AddWithValue("@productname", obj.Name);
                    cmd.Parameters.AddWithValue("@shortdetail", obj.ShortDetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@productdetail", obj.Description ?? string.Empty);
                    cmd.Parameters.AddWithValue("@rewrite_url", obj.rewriteurl ?? string.Empty);
                 
                    cmd.Parameters.AddWithValue("@showonhome", obj.showonhome);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@prospectus", obj.uploadfile ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uploadaimage", obj.uploadimage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageTitle", obj.PageTitle ?? string.Empty);
                    cmd.Parameters.AddWithValue("@PageMetaDesc", obj.MetaDescription ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Pagemeta", obj.MetaKeywords ?? string.Empty);
                    cmd.Parameters.AddWithValue("@investordate", obj.investordate);
                    cmd.Parameters.AddWithValue("@modelno", obj.doctype ?? string.Empty);
                    cmd.Parameters.AddWithValue("@expiraydate", obj.newexpiredate);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                  //  cmd.Parameters.AddWithValue("@purl", obj.vediourl ?? string.Empty);
                    cmd.Parameters.AddWithValue("@purl", obj.thirdpartyurl ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Isfamilyproduct", "" ?? string.Empty);
                    cmd.Parameters.AddWithValue("@producttitle", obj.Name ?? string.Empty);


                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
