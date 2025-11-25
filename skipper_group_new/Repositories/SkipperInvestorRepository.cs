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
    }
}
