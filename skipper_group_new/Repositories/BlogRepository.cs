using Dapper;
using skipper_group_new.Models;
using System.Data.SqlClient;
using System.Data;

namespace skipper_group_new.Repositories
{
    public class BlogRepository : IBlogRepo
    {
        private readonly string _connectionString;

        public BlogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<BlogDtl>> GetBlogDtl()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@blogId", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);

                var blogs = await db.QueryAsync<BlogDtl>(
                    "blogsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return blogs.ToList();
            }
        }
        public async Task<clsBlog> GetBlogById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@blogId", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);

                return await db.QueryFirstOrDefaultAsync<clsBlog>(
                    "blogsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public async Task<int> ChangeBlogStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@blogId", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 7);

                return await db.ExecuteAsync(
                    "blogsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public async Task<List<Blogcatdtl>> GetBlogCatList()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@blogId", 0, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 8);

                var blogCats = await db.QueryAsync<Blogcatdtl>(
                    "blogsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return blogCats.ToList();
            }
        }
        public async Task<int> AddBlog(clsBlog blog)
        {
            if (blog == null) return 0;

            using var conn = new SqlConnection(_connectionString);
            int mode = blog.BlogId > 0 ? 2 : 1;

            conn.Open();
            var parameters = MapToBlogParameters(blog, mode);
            await conn.ExecuteAsync("blogsSP", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@blogId");
        }
        public async Task<int> DeleteBlog(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();

                var blog = new clsBlog { BlogId = id };
                int mode = 3;
                var parameters = MapToBlogParameters(blog, mode);
                await conn.ExecuteAsync("blogsSP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@blogId");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteBlog failed: {ex.Message}");
                return -1;
            }
        }
        private DynamicParameters MapToBlogParameters(clsBlog b, int mode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@blogId", b.BlogId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@blogtitle", b.BlogTitle);
            parameters.Add("@topicId", b.TopicId);
            parameters.Add("@AutId", b.AutId);
            parameters.Add("@CatId", b.CatId);
            parameters.Add("@tagid", b.TagId);
            parameters.Add("@companyname", b.CompanyName);
            parameters.Add("@BlogImage", b.BlogImage);
            parameters.Add("@LargeImage", b.LargeImage);
            parameters.Add("@smalldesc", b.SmallDesc);
            parameters.Add("@longdesc", b.LongDesc);
            parameters.Add("@blogdate", b.BlogDate ?? DateTime.Now);
            parameters.Add("@urllink", b.UrlLink);
            parameters.Add("@displayorder", b.DisplayOrder);
            parameters.Add("@rewriteurl", b.RewriteUrl);
            parameters.Add("@PageTitle", b.PageTitle);
            parameters.Add("@PageMeta", b.PageMeta);
            parameters.Add("@PageMetaDesc", b.PageMetaDesc);
            parameters.Add("@status", b.Status);
            parameters.Add("@canonical", b.Canonical);
            parameters.Add("@no_indexfollow", b.NoIndexFollow);
            parameters.Add("@uname", b.Uname);
            parameters.Add("@pagescript", b.PageScript);
            parameters.Add("@Mode", mode);
            return parameters;
        }



        public async Task<int> AddBlogCat(clsBlogCategory blog)
        {
            if (blog == null) return 0;

            using var conn = new SqlConnection(_connectionString);
            int mode = blog.BcatId > 0 ? 2 : 1;

            conn.Open();
            var parameters = MapToBlogCatParameters(blog, mode);
            await conn.ExecuteAsync("blogcategorySP", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@bcatid");
        }
        public async Task<int> ChangeBlogCatStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@bcatid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);

                var affectedRows = await db.ExecuteAsync(
                    "blogcategorySP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return affectedRows;
            }
        }

        public async Task<clsBlogCategory> EditBlogCat(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@bcatid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);

                var result = await db.QueryFirstOrDefaultAsync<clsBlogCategory>(
                    "blogcategorySP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public async Task<int> DeleteBlogCat(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();

                var blog = new clsBlogCategory { BcatId = id };
                int mode = 3;
                var parameters = MapToBlogCatParameters(blog, mode);
                await conn.ExecuteAsync("blogcategorySP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@bcatid");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteBlog failed: {ex.Message}");
                return -1;
            }
        }
        private DynamicParameters MapToBlogCatParameters(clsBlogCategory b, int mode)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@bcatid", b.BcatId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@bcattitle", b.BcatTitle);
            parameters.Add("@status", b.Status);
            parameters.Add("@displayorder", b.DisplayOrder);
            parameters.Add("@uname", b.Uname);
            parameters.Add("@Mode", mode);
            return parameters;
        }
    }
}
