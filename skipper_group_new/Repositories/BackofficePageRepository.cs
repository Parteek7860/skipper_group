using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;

namespace skipper_group_new.Repositories
{
    public class BackofficePageRepository : IBackofficePageRepository
    {
        private readonly string _connectionString;

        public BackofficePageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DataTable> GetAlbumTypeList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select typeid,typename from albumtype where status=1 order by displayorder ";

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
        public async Task<DataTable> GetAlbumList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select a.* from album a  order by a.displayorder ";

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
        public int AddAlbum(clsGallery objgallery)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AlbumSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@albumid", objgallery.id);
                    cmd.Parameters.AddWithValue("@typeid", objgallery.albumtype);
                    cmd.Parameters.AddWithValue("@status", objgallery.status);
                    cmd.Parameters.AddWithValue("@albumtitle", objgallery.title);
                    cmd.Parameters.AddWithValue("@albumdate", objgallery.eventsdate);
                    cmd.Parameters.AddWithValue("@displayorder", objgallery.displayorder);
                    cmd.Parameters.AddWithValue("@uploadaimage", objgallery.uploadbanner);
                    cmd.Parameters.AddWithValue("@imagebanner", objgallery.uploadlargeimage);
                    cmd.Parameters.AddWithValue("@pagetitle", objgallery.pagetitle);
                    cmd.Parameters.AddWithValue("@albumdesc", objgallery.albumdesc);
                    cmd.Parameters.AddWithValue("@parentid", "0");
                    cmd.Parameters.AddWithValue("@picasacode", "0");
                    cmd.Parameters.AddWithValue("@PageMeta", objgallery.metadesc);
                    cmd.Parameters.AddWithValue("@PageMetadesc", objgallery.metakeywords);
                    cmd.Parameters.AddWithValue("@canonical", objgallery.canonical);
                    cmd.Parameters.AddWithValue("@showonmainsite", objgallery.metakeywords);
                    cmd.Parameters.AddWithValue("@showonmicrosite", objgallery.metakeywords);
                    cmd.Parameters.AddWithValue("@uname", objgallery.uname);
                    cmd.Parameters.AddWithValue("@mode", objgallery.mode);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int DeleteAlbum(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"delete from album where albumid=@albumid ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@albumid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public async Task<DataTable> GetAlbumListByID(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // string query = @"select a.*,t.typename from album a inner join albumtype t on t.typeid=a.typeid  where albumid=@albumid ";
                string query = @"select a.* from album a  where albumid=@albumid ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@albumid", id);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int UpdateAlbumStatus(string status, int id)
        {
            int result = 0;
            string query = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                if (status == "True")
                {
                    query = @"update album set status=1 where albumid=@albumid ";
                }
                else
                {
                    query = @"update album set status=0 where albumid=@albumid ";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@albumid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int UpdateVedioStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusVedioSP";
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
        public int DeleteVedio(int id)
        {
            int result = 0;
            string query = "DeleteVedioSP";
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
        public async Task<DataTable> GetAlbumTypeListByID()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                //string query = @"select a.*,t.typename from album a inner join albumtype t on t.typeid=a.typeid where a.typeid=2 order by a.displayorder ";
                string query = @"select a.* from album a order by a.displayorder ";

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

        public async Task<DataTable> BindVedioList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select v.* from vedio v  order by v.displayorder";

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
        public async Task<DataTable> GetPageList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select 0 Pageid, 'Main Page' as linkname union all select Pageid, case when Parentid<>0 then '>>'+pagename else pagename end as linkname from pagemaster";

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
        public async Task<DataTable> BindPageList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select * from pagemaster";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    await conn.OpenAsync();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }

        public int AddCMS(clsCMS cls)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PageMasterSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PageId", cls.Id); 
                    cmd.Parameters.AddWithValue("@PageName", cls.pagename);
                    cmd.Parameters.AddWithValue("@linkposition", cls.pageposition);
                    cmd.Parameters.AddWithValue("@linkname", cls.linkname);
                    cmd.Parameters.AddWithValue("@PageTitle", cls.pagetitle);
                    cmd.Parameters.AddWithValue("@PageMeta", cls.metakeywords);
                    cmd.Parameters.AddWithValue("@PageMetaDesc", cls.metadesc);
                    cmd.Parameters.AddWithValue("@megamenu", cls.megamenu);
                    cmd.Parameters.AddWithValue("@PageStatus", true); 
                    cmd.Parameters.AddWithValue("@parentid", cls.parentid);
                    cmd.Parameters.AddWithValue("@pageurl", cls.pageurl ?? "");
                    cmd.Parameters.AddWithValue("@rewriteid", DBNull.Value); 
                    cmd.Parameters.AddWithValue("@rewriteurl", cls.rewriteurl); 
                    cmd.Parameters.AddWithValue("@UploadBanner", cls.uploadbanner ?? "");
                    cmd.Parameters.AddWithValue("@displayorder", cls.displayorder);
                    cmd.Parameters.AddWithValue("@quicklinks", false); 
                    cmd.Parameters.AddWithValue("@smalldesc", cls.smalldesc);
                    cmd.Parameters.AddWithValue("@restricted", false); 
                    cmd.Parameters.AddWithValue("@target", DBNull.Value);
                    cmd.Parameters.AddWithValue("@tagline", cls.tagline1 ?? "");
                    cmd.Parameters.AddWithValue("@collageid", 0);
                    cmd.Parameters.AddWithValue("@canonical", cls.canonical ?? "");
                    cmd.Parameters.AddWithValue("@no_indexfollow", false);
                    cmd.Parameters.AddWithValue("@other_schema", DBNull.Value); 
                    cmd.Parameters.AddWithValue("@dynamicurlvalue", DBNull.Value); 
                    cmd.Parameters.AddWithValue("@dynamicurlrewrte", DBNull.Value); 
                    cmd.Parameters.AddWithValue("@PageDescription1", cls.pagedesc2);
                    cmd.Parameters.AddWithValue("@PageDescription2", cls.pagedesc3);
                    cmd.Parameters.AddWithValue("@PageDescription", cls.pagedesc ?? "");
                    cmd.Parameters.AddWithValue("@controller", cls.controllername);
                    cmd.Parameters.AddWithValue("@action", cls.actionname);
                    cmd.Parameters.AddWithValue("@Uname", cls.uname ?? "System");
                    cmd.Parameters.AddWithValue("@Mode", cls.mode);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<DataTable> GetPageListByID(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"select * from pagemaster where pageid=@pageid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    await conn.OpenAsync();
                    cmd.Parameters.AddWithValue("@pageid", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public int DeleteRecords(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"delete from pagemaster where pageid=@pageid ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pageid", id);

                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int CMSUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusCMSSP";
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
        public async Task<DataTable> GetFAQList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindFAQListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public async Task<int> AddFAQ(clsfaq obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("faqdetailsSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cid", obj.Id);
                    cmd.Parameters.AddWithValue("@question", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@description", obj.description ?? string.Empty);
                    cmd.Parameters.AddWithValue("@faqid", "0");
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        public int FAQDeleteRecords(int id)
        {
            int result = 0;
            string query = "DeleteFAQSP";
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
        public int FAQUpdateStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusFAQSP";
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
        public async Task<DataTable> GetImageFilePathList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindImagePathFileSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
        public async Task<DataTable> BindVedioTVCList()
        {

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BindTvcListSP", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var table = new DataTable();
                await conn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }        
        public int DeleteTvc(int id)
        {
            int result = 0;
            string query = "DeleteTvcSP";
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
        public int UpdateTvcStatus(string status, int id)
        {
            int result = 0;
            string query = "UpStatusTvcSP";
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
        public async Task<int> AddTvc(clsGallery obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("tvcsSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vedioid", obj.id);
                    cmd.Parameters.AddWithValue("@vediotitle", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uploadvedio", obj.URL ?? string.Empty);
                    cmd.Parameters.AddWithValue("@thumbnailimage", obj.uploadbanner ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@tagline", obj.tagline ?? string.Empty);

                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }

        public async Task<int> AddVedio(clsGallery obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("vedioSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@vedioid", obj.id);
                    cmd.Parameters.AddWithValue("@vediotitle", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@uploadvedio", obj.URL ?? string.Empty);
                    cmd.Parameters.AddWithValue("@thumbnailimage", obj.uploadbanner ?? string.Empty);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@status", obj.status);
                    cmd.Parameters.AddWithValue("@tagline", obj.tagline ?? string.Empty);
                    cmd.Parameters.AddWithValue("@showhome", 0);
                    cmd.Parameters.AddWithValue("@uname", obj.uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
        //Rakesh 12/11/2025
        public async Task<int> AddAlbumPhoto(clsGallery obj)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AlbumPhotoSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@photoid", obj.id);
                    cmd.Parameters.AddWithValue("@AlbumId", obj.albumtype);
                    cmd.Parameters.AddWithValue("@photoTitle", obj.title ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Uploadphoto", obj.uploadbanner ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Status", obj.status);
                    cmd.Parameters.AddWithValue("@displayorder", obj.displayorder);
                    cmd.Parameters.AddWithValue("@largeimage", obj.uploadlargeimage ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Uname", obj.uname);
                    cmd.Parameters.AddWithValue("@Mode", obj.mode);
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        public async Task<DataTable> BindPhotoGallaryList(int mode)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("AlbumPhotoSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@photoid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                };
                cmd.Parameters.Add(photoIdParam);
                cmd.Parameters.AddWithValue("@AlbumId", 0);
                cmd.Parameters.AddWithValue("@photoTitle", "");
                cmd.Parameters.AddWithValue("@Uploadphoto", "");
                cmd.Parameters.AddWithValue("@Status", true);
                cmd.Parameters.AddWithValue("@displayorder", 0);
                cmd.Parameters.AddWithValue("@largeimage", "");
                cmd.Parameters.AddWithValue("@Uname", "");
                cmd.Parameters.AddWithValue("@Mode", mode);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<int> DeletePhotoGallary(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("AlbumPhotoSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@photoid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = id
                };
                cmd.Parameters.Add(photoIdParam);
                cmd.Parameters.AddWithValue("@AlbumId", 0);
                cmd.Parameters.AddWithValue("@photoTitle", "");
                cmd.Parameters.AddWithValue("@Uploadphoto", "");
                cmd.Parameters.AddWithValue("@Status", true);
                cmd.Parameters.AddWithValue("@displayorder", 0);
                cmd.Parameters.AddWithValue("@largeimage", "");
                cmd.Parameters.AddWithValue("@Uname", "");
                cmd.Parameters.AddWithValue("@Mode", 3);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                result = (photoIdParam.Value != DBNull.Value) ? 1 : 0;
            }
            return result;
        }

        public async Task<int> changestatus(int id)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("AlbumPhotoSP", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter photoIdParam = new SqlParameter("@photoid", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = id
                };
                cmd.Parameters.Add(photoIdParam);
                cmd.Parameters.AddWithValue("@AlbumId", 0);
                cmd.Parameters.AddWithValue("@photoTitle", "");
                cmd.Parameters.AddWithValue("@Uploadphoto", "");
                cmd.Parameters.AddWithValue("@Status", true);
                cmd.Parameters.AddWithValue("@displayorder", 0);
                cmd.Parameters.AddWithValue("@largeimage", "");
                cmd.Parameters.AddWithValue("@Uname", "");
                cmd.Parameters.AddWithValue("@Mode", 7);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                result = (photoIdParam.Value != DBNull.Value) ? 1 : 0;
            }
            return result;
        }
    }
}
