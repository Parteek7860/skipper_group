using System.Data;
using skipper_group_new.Repositories;
using System.Data.SqlClient;
using skipper_group_new.Models;
using System.Net;

namespace skipper_group_new.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly string _connectionString;

        public HomeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> GetPageDescription(int id)
        {
            string pagedesc = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select pageid,linkname,pagetitle,rewriteurl,pageurl,pagedescription from pagemaster where pagestatus=1 and pageid='" + id + "' order by  displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pagedesc = Convert.ToString(reader["pagedescription"]);
                }
            }

            return pagedesc;
        }

        public async Task<string> GetSmallDescription(int id)
        {
            string pagedesc = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select pageid,linkname,pagetitle,rewriteurl,pageurl,pagedescription,smalldesc from pagemaster where pagestatus=1 and pageid='" + id + "' order by  displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pagedesc = Convert.ToString(reader["smalldesc"]);
                }
            }

            return pagedesc;
        }
        public async Task<DataTable> GetCMSDetail(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT pageid, linkname, pagetitle, rewriteurl, pageurl, pagedescription,tagline 
                         FROM pagemaster 
                         WHERE pagestatus = 1 AND pageid = @pageid 
                         ORDER BY displayorder";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pageid", id);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        dt.Load(reader);
                    } // reader is closed here automatically
                } // cmd disposed here
            } // conn closed here

            return dt;
        }
        public async Task<List<clsmainmenu>> GetMainMenu()
        {
            var menulist = new List<clsmainmenu>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select pageid,linkname,pagetitle,rewriteurl,pageurl, LEFT(pageurl, CHARINDEX('.aspx', pageurl) - 1) AS Url_name from pagemaster where pagestatus=1 and linkposition like '%Header%' order by  displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    menulist.Add(new clsmainmenu
                    {
                        pageid = Convert.ToString(reader["pageid"]),
                        pageurl = Convert.ToString(reader["pageurl"]),
                        Url_name = Convert.ToString(reader["Url_name"]),
                        linkname = reader["linkname"].ToString()
                    });
                }
            }

            return menulist;
        }

        public async Task<List<clsHomeBanner>> GetHomeBanner()
        {
            var bnanerlist = new List<clsHomeBanner>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select * from homebanner where status=1 order by displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    bnanerlist.Add(new clsHomeBanner
                    {
                        Title = Convert.ToString(reader["title"]),
                        ImageUrl = Convert.ToString(reader["bannerimage"]),
                        Description = WebUtility.HtmlDecode(Convert.ToString(reader["tagline1"])),
                    });
                }
            }

            return bnanerlist;
        }

        public int SaveContactDetails(clsContact objML_contact)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
              
                SqlCommand cmd = new SqlCommand("enquirysp", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fName", objML_contact.title);
                cmd.Parameters.AddWithValue("@emailid", objML_contact.Email);
                cmd.Parameters.AddWithValue("@mobile", objML_contact.Phone);
                cmd.Parameters.AddWithValue("@fMessage", objML_contact.Message);
                cmd.Parameters.AddWithValue("@organizationname", objML_contact.Company);
                cmd.Parameters.AddWithValue("@state", objML_contact.country);
                cmd.Parameters.AddWithValue("@uname", "user");
                cmd.Parameters.AddWithValue("@mode", 1);
                cmd.Parameters.Add("@eid", SqlDbType.Int, 0, "@eid").Direction = ParameterDirection.Output;
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            return result;

        }
        public async Task<List<clsHomeModel>> NewsEventsList()
        {
            var newslist = new List<clsHomeModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select top 3 * from events where status=1 order by eventsdate desc,displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    newslist.Add(new clsHomeModel
                    {
                        Title = Convert.ToString(reader["eventstitle"]),
                        ImageUrl = Convert.ToString(reader["uploadevents"]),
                        eventsdate = Convert.ToString(reader["eventsdate"]),
                        Description = WebUtility.HtmlDecode(Convert.ToString(reader["shortdesc"])),
                    });
                }
            }

            return newslist;
        }

        public async Task<List<clsHomeModel>> ProductList(bool s)
        {
            var categorylist = new List<clsHomeModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select * from Productcate where status=1 ";
                if(s==true)
                {
                    query += " and showonhome=1";
                }
                query += " order by displayorder";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    categorylist.Add(new clsHomeModel
                    {
                        Title = Convert.ToString(reader["category"]),
                        ImageUrl = Convert.ToString(reader["banner"]),
                        Id = Convert.ToInt16(reader["pcatid"]),
                        Description = WebUtility.HtmlDecode(Convert.ToString(reader["shortdetail"])),
                    });
                }
            }

            return categorylist;
        }
        public async Task<List<clsHomeModel>> Products()
        {
            var list = new List<clsHomeModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "select productid,p.pcatid,productname,UploadAImage,p.shortdetail,homedesc from products p inner join productcate cate on cate.pcatid=p.pcatid where p.status=1 and cate.status=1 order by p.displayorder ";
                
                
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    list.Add(new clsHomeModel
                    {
                        Title = Convert.ToString(reader["productname"]),
                        ImageUrl = Convert.ToString(reader["UploadAImage"]),
                        Id = Convert.ToInt16(reader["productid"]),
                        Description = WebUtility.HtmlDecode(Convert.ToString(reader["shortdetail"])),
                    });
                }
            }

            return list;
        }
    }
}
