using Dapper;
using skipper_group_new.Models;
using System.Data.SqlClient;
using System.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using skipper_group_new.Models;
using skipper_group_new.Repositories;

namespace skipper_group_new.Repositories
{
    public interface IManagementRepo
    {
        Task<List<TeamTypeDtl>> GetTeamTblData();
        Task<int> AddTeamType(clsTeamType team);
        Task<clsTeamType> EditTeamType(int id);
        Task<int> DeleteTeamType(int id);

        Task<List<ManagementDtl>> GetTeamManagementData();
        Task<int> AddTeam(Management m);
        Task<Management> EditTeam(int id);
        Task<int> DeleteTeam(int id);
        Task<Dictionary<int, string>> GetTeamDropdown();
        Task<Dictionary<int, string>> GetSubTeamDropdown();
        Task<int> ChangeStatus(int id);
        Task<int> ChangeManStatus(int id);

        Task<List<EnquiryModel>> GetEnquiry();
        Task<List<ProductEnquiryModel>> GetProductEnquiry();

        Task<int> AddEditJob(PostJobModel m);
        Task<List<PostJobModel>> GetJobList();
        Task<DataTable> GetProductSolutionList();
        Task<PostJobModel> GetJobPostById(int jobID);
        Task<int> Delete(int jobID);
        Task<List<Applicationview>> GetApplicantsDetail();

      
        Task<int> DeleteStore(int storeID);
        Task<int> ChangeDelerStatus(int storeId);
        Task<List<clsReview>> GetReviewData();
        int UpdateReviewStatus(string status, int id);
        int AddReview(clsReview obj);
        Task<int> SaveApplication(JobApplicationModel model);
        Task<Application> GetApplicantsDetailByID(int App_id);
    }

    public class ManagementRepository : IManagementRepo
    {
        private readonly string _connectionString;

        public ManagementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<TeamTypeDtl>> GetTeamTblData()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ttypeid", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 7);

                var result = await db.QueryAsync<TeamTypeDtl>(
                    "teamtypeSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
        public async Task<clsTeamType> EditTeamType(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ttypeid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 6);
                var result = await db.QueryFirstOrDefaultAsync<clsTeamType>(
                    "teamtypeSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }
        public async Task<int> ChangeStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ttypeid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);
                var affectedRows = await db.ExecuteAsync(
                    "teamtypeSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return affectedRows;
            }
        }
        public async Task<int> AddTeamType(clsTeamType team)
        {
            if (team == null) return 0;

            using var conn = new SqlConnection(_connectionString);
            int mode = team.TTypeId > 0 ? 2 : 1;

            conn.Open();
            var parameters = MapToTeamtypeParameters(team, mode);
            await conn.ExecuteAsync("teamtypeSP", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@ttypeid");
        }
        public async Task<int> DeleteTeamType(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();

                var t = new clsTeamType { TTypeId = id };
                int mode = 3;
                var parameters = MapToTeamtypeParameters(t, mode);
                await conn.ExecuteAsync("teamtypeSP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@ttypeid");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteBlog failed: {ex.Message}");
                return -1;
            }
        }
        private DynamicParameters MapToTeamtypeParameters(clsTeamType b, int mode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ttypeid", b.TTypeId, DbType.Int32, ParameterDirection.InputOutput);
            parameters.Add("@ttype", b.TType);
            parameters.Add("@status", b.Status);
            parameters.Add("@displayorder", b.DisplayOrder);
            parameters.Add("@shortdesc", b.ShortDesc);
            parameters.Add("@collageid", b.CollageId);
            parameters.Add("@uname", b.UName);
            parameters.Add("@Mode", mode);
            return parameters;
        }

        public async Task<List<ManagementDtl>> GetTeamManagementData()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@teamid", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@mode", 7);

                var result = await db.QueryAsync<ManagementDtl>(
                    "ourteamSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
        }
        public async Task<Management> EditTeam(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@teamid", id, DbType.Int32, ParameterDirection.InputOutput);
                parameters.Add("@mode", 8);

                return await db.QueryFirstOrDefaultAsync<Management>(
                    "ourteamSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public async Task<int> ChangeManStatus(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@teamid", id);
                parameters.Add("@mode", 9);

                return await db.ExecuteAsync(
                    "ourteamSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public async Task<Dictionary<int, string>> GetTeamDropdown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@teamid", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@mode", 10);

                var result = await db.QueryAsync<TeamTypeDtl>(
                    "ourteamSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.TTypeId, x => x.TType);
            }
        }
        public async Task<Dictionary<int, string>> GetSubTeamDropdown()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@teamid", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@mode", 11);

                var result = await db.QueryAsync<SubteamDtl>(
                    "ourteamSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToDictionary(x => x.TSubTypeId, x => x.Subtype);
            }
        }
        public async Task<int> AddTeam(Management m)
        {
            if (m == null) return 0;
            if (m.UName == null)
            {
                m.UName = " ";
            }
            using var conn = new SqlConnection(_connectionString);
            int mode = m.Teamid > 0 ? 2 : 1;

            conn.Open();
            var parameters = MapToTeamParameters(m, mode);
            await conn.ExecuteAsync("ourteamSP", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@teamid");
        }
        public async Task<int> DeleteTeam(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                conn.Open();

                var t = new Management { Teamid = id };
                int mode = 3;
                var parameters = MapToTeamParameters(t, mode);
                await conn.ExecuteAsync("ourteamSP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@teamid");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteSubTeam failed: {ex.Message}");
                return -1;
            }
        }
        private DynamicParameters MapToTeamParameters(Management b, int mode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@teamid", b.Teamid);
            parameters.Add("@ttypeid", b.TTypeId);
            parameters.Add("@name", b.Name);
            parameters.Add("@designation", b.Designation);
            parameters.Add("@qualification", b.Qualification);
            parameters.Add("@nationality", b.Nationality);
            parameters.Add("@headquarter", b.Headquarter);
            parameters.Add("@experience", b.Experience);
            parameters.Add("@industries", b.Industries);
            parameters.Add("@detaildesc", b.DetailDesc);
            parameters.Add("@shortdesc", b.ShortDesc);

            parameters.Add("@Uploadphoto", b.UploadPhoto);
            parameters.Add("@Uploadphoto1", b.UploadPhoto1);

            parameters.Add("@displayorder", b.DisplayOrder);
            parameters.Add("@showonhome", b.ShowOnHome);
            parameters.Add("@status", b.Status);

            parameters.Add("@PageTitle", b.PageTitle);
            parameters.Add("@pagemeta", b.PageMeta);
            parameters.Add("@pagemetadesc", b.PageMetaDesc);
            parameters.Add("@canonical", b.Canonical);
            parameters.Add("@no_indexfollow", b.NoIndexFollow);

            parameters.Add("@showtop", b.ShowTop);
            parameters.Add("@other_schema", b.OtherSchema);

            parameters.Add("@tsubtypeid", b.TSubTypeId);
            parameters.Add("@collageid", b.CollageId);
            parameters.Add("@dean", b.Dean);
            parameters.Add("@principal", b.Principal);
            parameters.Add("@director", b.Director);
            parameters.Add("@expyear", b.ExpYear);

            parameters.Add("@uname", b.UName);
            parameters.Add("@mode", mode);
            return parameters;
        }
        public async Task<List<EnquiryModel>> GetEnquiry()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eid", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 2);
                var result = await db.QueryAsync<EnquiryModel>(
                    "enquirysp",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<List<ProductEnquiryModel>> GetProductEnquiry()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@eid", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 3);
                var result = await db.QueryAsync<ProductEnquiryModel>(
                    "enquirysp",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<int> AddEditJob(PostJobModel m)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PostedJobsSP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter jobIdParam = new SqlParameter("@JobId", SqlDbType.Int);
                if (m.Mode == 1)
                {
                    jobIdParam.Direction = ParameterDirection.Output;
                }
                else
                {
                    jobIdParam.Direction = ParameterDirection.Input;
                    jobIdParam.Value = m.Jobid;
                }
                cmd.Parameters.Add(jobIdParam);

                AddParameters(cmd, m);

                conn.Open();
                cmd.ExecuteNonQuery();

                return m.Mode == 1 ? (int)jobIdParam.Value : m.Jobid;
            }
        }
        private void AddParameters(SqlCommand cmd, PostJobModel job)
        {
            cmd.Parameters.AddWithValue("@JobCode", job.JobCode);
            cmd.Parameters.AddWithValue("@jobcategory", job.jobcategory);
            cmd.Parameters.AddWithValue("@designation", job.designation);
            cmd.Parameters.AddWithValue("@JobTitle", job.JobTitle);
            cmd.Parameters.AddWithValue("@Qualification", job.Qualification ?? "");
            cmd.Parameters.AddWithValue("@Min_Expyear", job.Min_Expyear);
            cmd.Parameters.AddWithValue("@Min_Expmonth", job.Min_Expmonth);
            cmd.Parameters.AddWithValue("@Max_Expyear", job.Max_Expyear);
            cmd.Parameters.AddWithValue("@Max_Expmonth", job.Max_Expmonth);
            cmd.Parameters.AddWithValue("@salary", job.salary ?? "");
            cmd.Parameters.AddWithValue("@Skills", job.Skills ?? "");
            cmd.Parameters.AddWithValue("@JobOpening_date", job.JobOpening_date);
            cmd.Parameters.AddWithValue("@JobClosing_Date", job.JobClosing_date);
            cmd.Parameters.AddWithValue("@Location", job.Location ?? "");
            cmd.Parameters.AddWithValue("@JobDesc", job.JobDesc ?? "");
            cmd.Parameters.AddWithValue("@Ageyear", job.Ageyear);
            cmd.Parameters.AddWithValue("@Agemonth", job.Agemonth);
            cmd.Parameters.AddWithValue("@Status", job.Status);
            cmd.Parameters.AddWithValue("@displayorder", job.displayorder);
            cmd.Parameters.AddWithValue("@company", job.company ?? "");
            cmd.Parameters.AddWithValue("@department", job.department ?? "");
            cmd.Parameters.AddWithValue("@shortdesc", job.shortdesc ?? "");
            cmd.Parameters.AddWithValue("@emailid", job.emailid ?? "");
            cmd.Parameters.AddWithValue("@rewriteurl", job.rewriteurl ?? "");
          //  cmd.Parameters.AddWithValue("@noofvacancies", job.NoOfVacancies ?? "");
            cmd.Parameters.AddWithValue("@emptypeid", job.EmpTypeId ?? "");
            cmd.Parameters.AddWithValue("@Uname", job.Uname ?? "");
            cmd.Parameters.AddWithValue("@Mode", job.Mode ?? 0);
        }
        public async Task<List<PostJobModel>> GetJobList()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 5);
                var result = await db.QueryAsync<PostJobModel>(
                    "PostedJobsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<DataTable> GetProductSolutionList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProductSolutionSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }
        public async Task<PostJobModel> GetJobPostById(int jobID)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@JobId", jobID, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 6, DbType.Int32, ParameterDirection.Input);

                var result = await db.QueryAsync<PostJobModel>(
                    "PostedJobsSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
        }
        public async Task<int> Delete(int jobID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@JobId", jobID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Mode", 3, DbType.Int32, ParameterDirection.Input);
                    var affectedRows = await db.ExecuteAsync(
                        "PostedJobsSP",
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
        public async Task<List<Applicationview>> GetApplicantsDetail()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@App_id", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters.Add("@Mode", 4);
                var result = await db.QueryAsync<Applicationview>(
                    "PostedApplicationSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
       


        public async Task<int> DeleteStore(int storeID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@storeid", storeID, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@Mode", 3, DbType.Int32, ParameterDirection.Input);
                    var affectedRows = await db.ExecuteAsync(
                        "storesSP",
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
        public async Task<int> ChangeDelerStatus(int storeId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@storeid", storeId, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 10, DbType.Int32, ParameterDirection.Input);

                var rows = await db.ExecuteAsync(
                    "storesSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return rows;
            }
        }
        public async Task<List<clsReview>> GetReviewData()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                var result = await db.QueryAsync<clsReview>(
                    "BindUserReviewSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public int UpdateReviewStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusUserReviewSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "True" ? 1 : 0);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int AddReview(clsReview obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("user_reviewSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", obj.Id);
                    cmd.Parameters.AddWithValue("@title", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@username", obj.username ?? string.Empty);
                    cmd.Parameters.AddWithValue("@rating", obj.Rating);
                    cmd.Parameters.AddWithValue("@desc", obj.reviewdesc);
                    cmd.Parameters.AddWithValue("@mobileno", obj.mobileno);
                    cmd.Parameters.AddWithValue("@emailid", obj.emailid);
                    cmd.Parameters.AddWithValue("@userid", "");

                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<int> SaveApplication(JobApplicationModel model)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            int mode = model.App_id > 0 ? 2 : 1;

            var parameters = new DynamicParameters();
            parameters.Add("@Mode", mode);
            parameters.Add("@App_Id", model.App_id, direction: ParameterDirection.InputOutput);
            parameters.Add("@JobId", model.Jobid);
            parameters.Add("@JobTitle", model.JobTitle);
            parameters.Add("@Designation", model.Designation);
            parameters.Add("@FName", model.FName);
            parameters.Add("@LName", model.LName);
            parameters.Add("@App_DOB", model.App_DOB);
            parameters.Add("@Gender", model.gender);
            parameters.Add("@MaritalStatus", model.MaritalStatus);
            parameters.Add("@Father_HusbandName", model.Father_HusbandName);
            parameters.Add("@App_Address", model.App_Address);
            parameters.Add("@Telephone", model.Telephone);
            parameters.Add("@Mobile", model.Mobile);
            parameters.Add("@App_Email", model.App_Email);
            parameters.Add("@City", model.City);
            parameters.Add("@State", model.State);
            parameters.Add("@App_Qualification", model.App_Qualification);
            parameters.Add("@App_Expyear", model.App_Expyear);
            parameters.Add("@App_Expmonth", model.App_Expmonth);
            parameters.Add("@App_Skills", model.App_Skills);
            parameters.Add("@AttachCV", model.ResumePath);
            parameters.Add("@FunArea", model.funarea);
            parameters.Add("@CIndustry", model.cindustry);
            parameters.Add("@PLocation", model.plocation);
            parameters.Add("@CEmployer", model.cemployer);
            parameters.Add("@CSalary", model.csalary);
            parameters.Add("@AreaOfIntrst", model.Areaofintrst);
            parameters.Add("@Country", model.Country);
            parameters.Add("@UName", model.UName);

            await conn.ExecuteAsync("PostedApplicationSP", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@App_Id");
        }

        public async Task<Application> GetApplicantsDetailByID(int App_id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@App_id", App_id, DbType.Int32, ParameterDirection.Input);
                parameters.Add("@Mode", 5);

                var result = await db.QueryFirstOrDefaultAsync<Application>(
                    "PostedApplicationSP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

    }
}
