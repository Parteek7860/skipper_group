using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;

namespace skipper_group_new.Repositories
{
    public class SkipperInvestorRepository : ISkipperInvestorRepo
    {
        // RAKESH CHAUHAN - 19/11/2025
        private readonly string _connectionString;

        public SkipperInvestorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> GetCategoryItem()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("InvestorDTL", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", 1);
            cmd.Parameters.AddWithValue("@ID", DBNull.Value);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            var catTable = new DataTable();
            catTable.Load(reader);

            return catTable;
        }

        public async Task<DataTable> GetSubCategoryItem(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("InvestorDTL", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", 2);
            cmd.Parameters.AddWithValue("@ID", id);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            var subTable = new DataTable();
            subTable.Load(reader);

            return subTable;
        }

        public async Task<DataTable> GetReports(int pcatid, int psubcatid)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("InvestorDTL", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", 3);
            cmd.Parameters.AddWithValue("@ID", 0);
            cmd.Parameters.AddWithValue("@pcatid", pcatid);
            cmd.Parameters.AddWithValue("@psubcatid", psubcatid);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            var subTable = new DataTable();
            subTable.Load(reader);

            return subTable;
        }

        public async Task<DataTable> GetCategoryDetail(int pcatid)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("InvestorDTL", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", 4);
            cmd.Parameters.AddWithValue("@pcatid", pcatid);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            var catDetailTable = new DataTable();
            catDetailTable.Load(reader);
            return catDetailTable;
        }

        public async Task<int> SaveQueryData(InvestorQueryModel m)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("investorenquirysp", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@eid", m.Id);
                    cmd.Parameters.AddWithValue("@fname", m.Name);
                    cmd.Parameters.AddWithValue("@emailid", m.Email);
                    cmd.Parameters.AddWithValue("@Mobile", m.Mobile);
                    cmd.Parameters.AddWithValue("@City", "");
                    cmd.Parameters.AddWithValue("@Address", m.Address);
                    cmd.Parameters.AddWithValue("@FMessage",m.Comment);
                    cmd.Parameters.AddWithValue("@Subject", "");
                    cmd.Parameters.AddWithValue("@category", "");
                    cmd.Parameters.AddWithValue("@state", "");
                    cmd.Parameters.AddWithValue("@organizationname", "");
                    cmd.Parameters.AddWithValue("@country", "");
                    cmd.Parameters.AddWithValue("@Uname", "");
                    cmd.Parameters.AddWithValue("@Mode",1);                    
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
