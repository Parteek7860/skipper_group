using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;
using university.Repositories;

namespace skipper_group_new.Repositories
{
    public class AudioRepository : IAudioRepo
    {
        private readonly string _connectionString;

        public AudioRepository(IDbConnectionProvider provider)
        {
            _connectionString = provider.ConnectionString;
        }
        public async Task<int> AddAudioVideo(AudioVideoModel model)
        {
            int audioId = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AudioVideo_Action", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AudioId", model.AudioId);
                cmd.Parameters.AddWithValue("@AudioTitle", model.AudioTitle);
                cmd.Parameters.AddWithValue("@AudioPath", model.AudioPath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DisplayOrder", model.displayOrder ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Mode", model.Mode); 
                await conn.OpenAsync();
                object result = await cmd.ExecuteScalarAsync();
                if (result != null) audioId = Convert.ToInt32(result);
            }
            return audioId;
        }

        public async Task<DataTable> GetAudioVideoList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AudioVideo_Action", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", 5);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }

        public async Task<int> DeleteAudioVideo(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AudioVideo_Action", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AudioId", id);
                cmd.Parameters.AddWithValue("@Mode", 3);

                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ChangeStatus(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_AudioVideo_Action", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AudioId", id);
                cmd.Parameters.AddWithValue("@Mode", 4);

                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
