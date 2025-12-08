using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;

namespace skipper_group_new.Repositories
{
    public class HomsPageRepository : IHomePageRepository
    {
        private readonly string _connectionString;
        Enc_Decyption enc = new Enc_Decyption();

        public HomsPageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> GetSignInDetails(string UserName, string Password)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SelectLoginSP";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", UserName);
                    cmd.Parameters.AddWithValue("@Password", enc.AES_Encrypt(Password, "@9899848281"));
                    await conn.OpenAsync();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public async Task<DataTable> GetMenuList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetMenuModuleListSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public async Task<DataTable> GetFormList(string moduleid)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFormListSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@moduleid", moduleid);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public async Task<DataTable> SearchFormList(string text)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SearchMenuListSP";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@text", text);
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public int CreateBannerType(clsBannerType objbannertype)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("homebannertypeSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@btypeid", objbannertype.btypeid);
                    cmd.Parameters.AddWithValue("@btype", objbannertype.btype);
                    cmd.Parameters.AddWithValue("@status", objbannertype.status);
                    cmd.Parameters.AddWithValue("@mobilestatus", objbannertype.mobilestatus);
                    cmd.Parameters.AddWithValue("@collageid", objbannertype.collageid);
                    cmd.Parameters.AddWithValue("@displayorder", objbannertype.displayorder);
                    cmd.Parameters.AddWithValue("@uname", objbannertype.uname);
                    cmd.Parameters.AddWithValue("@mode", objbannertype.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<DataTable> GetBannerTypeList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBannerTypeListSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
        public async Task<DataTable> GetBannerTypeListByID(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBannerTypeListByIDSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@btypeid", id);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }
        public int DeleteBannerType(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteBannerTypeSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@btypeid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int UpdateBannerType(string status, int id)
        {
            int result = 0;
            string query = "UpStatusHomeBannertypeSP";
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
        public async Task<DataTable> GetBannerList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBannerListSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int DeleteBanner(int id)
        {
            int result = 0;
            string query = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteBannerSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetBannerListByID(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBannerListByIDSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bid", id);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }
        public async Task<DataTable> GetEventTypeList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select * from newstype where 1=1 ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public async Task<DataTable> GetEventList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"BindEventsListSP ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int DeleteMediaSection(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteMediaSectionSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@eventsid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int UpdateEventsStatus(string status, int id)
        {
            int result = 0;
            string query = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (status == "True")
                {
                    query = "UpdateEventsStatusTrueSP";
                }
                else
                {
                    query = "UpdateEventsStatusfalseSP";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@eventsid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int UpdateEventsStatusShowHome(string status, int id)
        {
            int result = 0;
            string query = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (status == "True")
                {
                    query = "UpdateEventsShowonhometrueSP";
                }
                else
                {
                    query = "UpdateEventsShowonhomefalseSP";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@eventsid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetPageList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select * from pagemaster ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }

        public async Task<DataTable> GetEnquiryList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("GetEnquirySP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public async Task<int> ChangeUserPasswordAsync(ChangePasswordModel model)
        {
            if (model == null || model.NewPassword != model.ConfirmPassword)
                return 0;

            bool isCorrect = await CheckCurrentPasswordAsync(model.UserId, model.CurrentPassword);
            if (!isCorrect)
                return 0;

            string hashedPassword = CryptoUtils.HashPassword(model.NewPassword);

            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE bousermaster SET UserPassword = @Password WHERE UserId = @UserId";
                await conn.OpenAsync();
                int affectedRows = await conn.ExecuteAsync(query, new { UserId = model.UserId, Password = hashedPassword });
                return affectedRows > 0 ? 1 : 0;
            }
        }

        public async Task<bool> CheckCurrentPasswordAsync(string userId, string currentPassword)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT UserPassword FROM bousermaster WHERE UserId = @UserId";
                await conn.OpenAsync();
                string storedHashedPassword = await conn.ExecuteScalarAsync<string>(query, new { UserId = userId });

                if (string.IsNullOrWhiteSpace(storedHashedPassword))
                    return false;

                return CryptoUtils.VerifyPassword(currentPassword, storedHashedPassword);
            }
        }
        public int CreateMedia(clsMediatype obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EventsSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@eventsid", obj.id);
                    cmd.Parameters.AddWithValue("@eventstitle", obj.eventstitle);
                    cmd.Parameters.AddWithValue("@ntypeid", obj.mediatype);
                    cmd.Parameters.AddWithValue("@location", obj.tagline ?? string.Empty);
                    cmd.Parameters.AddWithValue("@shortdesc", obj.shortdetail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@EventsDesc", obj.detail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@UploadEvents", obj.uploadbanner);
                    cmd.Parameters.AddWithValue("@largeimage", obj.uploadlargeimage);
                    cmd.Parameters.AddWithValue("@youtube_url", obj.url ?? string.Empty);
                    cmd.Parameters.AddWithValue("@showonhome", obj.showonhome);
                    cmd.Parameters.AddWithValue("@eventsdate", obj.eventsdate);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.pagetitle);
                    cmd.Parameters.AddWithValue("@eventedate", obj.eventsdate);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.metakeywords);
                    cmd.Parameters.AddWithValue("@PageMetaDesc", obj.metadesc);
                    cmd.Parameters.AddWithValue("@rewriteurl", "");
                    cmd.Parameters.AddWithValue("@uploadfile", "");
                    cmd.Parameters.AddWithValue("@colorcode", "");
                    // cmd.Parameters.AddWithValue("@pagescript", obj.pagescript ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int CreateHomeBanner(clsbanner obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("HomeBannerSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@bid", obj.id);
                    cmd.Parameters.AddWithValue("@btypeid", obj.bannertype);
                    cmd.Parameters.AddWithValue("@title", obj.name);
                    cmd.Parameters.AddWithValue("@devicetype", obj.devicetype1);
                    cmd.Parameters.AddWithValue("@tagline1", obj.tagline1);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@bannerimage", obj.uploadbanner);
                    cmd.Parameters.AddWithValue("@blogo", "");
                    cmd.Parameters.AddWithValue("@startdate", obj.startdate);
                    cmd.Parameters.AddWithValue("@enddate", obj.enddate);
                    cmd.Parameters.AddWithValue("@collageid", obj.collageid);
                    cmd.Parameters.AddWithValue("@url", obj.url ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@tagline2", "");
                    cmd.Parameters.AddWithValue("@mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int UpdateBannerStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusBannerSP";
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
        public async Task<DataTable> GetTestimonialsList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"BindTestimonialsSP";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int UpdateTestimonilsStatus(bool status, int id)
        {
            int result = 0;
            string query = "UpdStatusTestimonialsSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == true ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DeleteTestimonils(int id)
        {
            int result = 0;
            string query = "DeleteTestimonialsSP";
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
        public int AddTestimonials(clsTestimonial obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TestimonialsSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@testimonialid", obj.testimonialid);
                    cmd.Parameters.AddWithValue("@Testimonialname", obj.Name);
                    cmd.Parameters.AddWithValue("@Tesid", 0);
                    cmd.Parameters.AddWithValue("@testimonialdesc", obj.Description ?? string.Empty);
                    cmd.Parameters.AddWithValue("@detaildesc", obj.detail ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Uploadphoto", obj.Image);
                    cmd.Parameters.AddWithValue("@Placed", obj.placed);

                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@pagetitle", obj.pagetitle);
                    cmd.Parameters.AddWithValue("@desg", obj.designation);
                    cmd.Parameters.AddWithValue("@PageMeta", obj.metakeywords);
                    cmd.Parameters.AddWithValue("@PageMetaDesc", obj.metadesc);
                    cmd.Parameters.AddWithValue("@showonhome", "0");
                    cmd.Parameters.AddWithValue("@showright", "0");
                    cmd.Parameters.AddWithValue("@uploadvedio", obj.uploadvedio);
                    cmd.Parameters.AddWithValue("@canonical", obj.canonical);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int UpdateEventsTypeStatus(string status, int id)
        {
            int result = 0;
            string query = "UpdStatuseventstypeSP";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;   // Important!

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Parameters.AddWithValue("@status", status == "true" ? 0 : 1);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DeleteEventsTypeSection(int id)
        {
            int result = 0;
            string query = "DeleteEeventstypeSP";
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
        public int CreateMediaType(clsMediatype obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("newstypeSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ntypeid", obj.id);
                    cmd.Parameters.AddWithValue("@ntype", obj.mediatype);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetImagePath()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"BindImageFileListSP";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int CreateFilePathImage(clsDownload obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ImagefileuploadSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fileid", obj.id ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uploadfile", obj.FilePath);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@filetitle", obj.Filetitle);
                    cmd.Parameters.AddWithValue("@status", "1");
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@mode", "1");
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
