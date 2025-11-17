using Dapper;
using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;

namespace skipper_group_new.Repositories
{
    //Rakesh Chauhan - 12/06/2024 - Backoffice Project Repository Created
    public class BackofficeProjectRepository: IBackofficeProjectRepository
    {
        private readonly string _connectionString;

        public BackofficeProjectRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        private DynamicParameters MapToResearchParameters(ResearchModel research)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@researchid", research.ResearchId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@ntypeid", research.NTypeId ?? 0);
            parameters.Add("@catid", research.CatId ?? "");
            parameters.Add("@researchTitle", research.ResearchTitle ?? "");
            parameters.Add("@tagline", research.Tagline ?? "");
            parameters.Add("@location", research.Location ?? "");
            parameters.Add("@types", research.Types ?? "");
            parameters.Add("@researchsdate", research.ResearchSDate.HasValue ? research.ResearchSDate.Value : null);
            parameters.Add("@shortdesc", research.ShortDesc ?? "");
            parameters.Add("@researchDesc", research.ResearchDesc ?? "");
            parameters.Add("@UploadEvents", research.UploadEvents ?? "");
            parameters.Add("@largeimage", research.LargeImage ?? "");
            parameters.Add("@homeimage", research.HomeImage ?? "");
            parameters.Add("@verylargeimage", research.VeryLargeImage ?? "");
            parameters.Add("@DisplayOrder", research.DisplayOrder??0);
            parameters.Add("@PageTitle", research.PageTitle ?? "");
            parameters.Add("@PageMeta", research.PageMeta ?? "");
            parameters.Add("@PageMetaDesc", research.PageMetaDesc ?? ""); 
            parameters.Add("@other_schema", research.OtherSchema ?? "");
            parameters.Add("@canonical", research.Canonical ?? "");
            parameters.Add("@no_indexfollow", research.NoIndexFollow??true);
            parameters.Add("@uname", research.UName ?? "");
            parameters.Add("@status", research.Status ?? true);
            parameters.Add("@Mode", research.Mode);
            parameters.Add("@showonhome", research.ShowOnHome ?? true);
            parameters.Add("@showonschool", research.ShowOnSchool ?? true);
            parameters.Add("@archive", research.Archive ?? true);
            parameters.Add("@researchedate", research.ResearchEDate.HasValue ? research.ResearchEDate.Value : null);
            parameters.Add("@rewriteurl", research.RewriteUrl ?? "");
            parameters.Add("@youtube_url", research.YoutubeUrl??"");
            parameters.Add("@uploadfile", research.UploadFile ?? "");
            parameters.Add("@colorcode", research.ColorCode ?? "");
            parameters.Add("@showongroup", research.ShowOnGroup ?? true);
            parameters.Add("@lcid", research.LCID??0);
            return parameters;
        }


        public async Task<DataTable> GetProduct()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("researchSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@researchid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                };
                cmd.Parameters.Add(photoIdParam);
                cmd.Parameters.AddWithValue("@Mode", 5);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<DataTable> GetCategory()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("researchSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@researchid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                };
                cmd.Parameters.Add(photoIdParam);
                cmd.Parameters.AddWithValue("@Mode", 6);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<int> AddUpdateProject(ResearchModel research)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var parameters = MapToResearchParameters(research);
                    await conn.OpenAsync();

                    await conn.ExecuteAsync(
                        "researchSP",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    var newId = parameters.Get<int>("@researchid");
                    return newId;
                }
            }
            catch (SqlException sqlEx)
            {              
                throw new Exception("SQL Error in AddUpdateProject: " + sqlEx.Message, sqlEx);
            }
            catch (InvalidOperationException invEx)
            {               
                throw new Exception("Invalid Operation in AddUpdateProject: " + invEx.Message, invEx);
            }
            catch (Exception ex)
            {                
                throw new Exception("General Error in AddUpdateProject: " + ex.Message, ex);
            }
        }


        public async Task<DataTable> GetProjectData()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("researchSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@researchid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                };
                cmd.Parameters.Add(photoIdParam);                
                cmd.Parameters.AddWithValue("@Mode", 7);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }


        public async Task<int> ExecuteProjectAction(int researchId, int mode)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@researchid", researchId, DbType.Int32, ParameterDirection.InputOutput);
                    parameters.Add("@Mode", mode);

                    await conn.OpenAsync();

                    await conn.ExecuteAsync(
                        "researchSP",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return parameters.Get<int>("@researchid");
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("SQL Error in ExecuteProjectAction: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("General Error in ExecuteProjectAction: " + ex.Message, ex);
            }
        }


    }
}
