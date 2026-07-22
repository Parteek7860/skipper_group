
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using skipper_group_new.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using university.Repositories;

namespace skipper_group_new.Repositories
{
    public class SkipperHomeRepository : ISkipperHomeRepository
    {
        private readonly string _connectionString;

        Enc_Decyption enc = new Enc_Decyption();
        public SkipperHomeRepository(IDbConnectionProvider provider)
        {
            _connectionString = provider.ConnectionString;
        }
        public async Task<DataTable> GetMenuList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindPageMasterSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetSubMenuList()
        {
            DataTable subMenuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindVehicleTyreList", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        subMenuList = table;
                    }
                }
            }
            return subMenuList;
        }

        public async Task<DataTable> GetHamburgerMenuList()
        {
            DataTable hamburgerMenuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindPageMasterSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        hamburgerMenuList = table;
                    }
                }
            }
            return hamburgerMenuList;
        }

        public async Task<DataTable> GetSeoFriendlyUrls()
        {
            DataTable seoFriendlyUrls;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindSEOPageMasterSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        seoFriendlyUrls = table;
                    }
                }
            }
            return seoFriendlyUrls;
        }

        public async Task<DataTable> GetCMSData()
        {
            DataTable cmsData;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindPageMasterSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        cmsData = table;
                    }
                }
            }
            return cmsData;
        }
        public async Task<DataTable> GetProjectsList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProjectListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetMapGalleryProjectList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetMapGalleryProjectListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetCarrer()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCareerSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public int SaveEnquiryDetails(EnquiryModel objML_contact)
        {
            int result = 0;
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                SqlCommand cmd = new SqlCommand("PostedApplicationSP", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@jobid", objML_contact.Eid);
                cmd.Parameters.AddWithValue("@fName", objML_contact.FName);
                cmd.Parameters.AddWithValue("@lname", objML_contact.lastname);
                cmd.Parameters.AddWithValue("@App_Email", objML_contact.EmailId);
                cmd.Parameters.AddWithValue("@mobile", objML_contact.phone);
                cmd.Parameters.AddWithValue("@City", objML_contact.city);
                cmd.Parameters.AddWithValue("@country", objML_contact.country);
                cmd.Parameters.AddWithValue("@App_Address", objML_contact.address);
                cmd.Parameters.AddWithValue("@MaritalStatus", objML_contact.zipcode);
                cmd.Parameters.AddWithValue("@state", objML_contact.state);
                cmd.Parameters.AddWithValue("@jobtitle", objML_contact.jobname);
                cmd.Parameters.AddWithValue("@AttachCV", objML_contact.uploadfile);
                cmd.Parameters.AddWithValue("@uploadfile", objML_contact.uploadfile);
                cmd.Parameters.AddWithValue("@uname", "user");
                cmd.Parameters.AddWithValue("@mode", 1);
                //cmd.Parameters.Add("@App_id", SqlDbType.Int, 0, "@App_id").Direction = ParameterDirection.Output;

                SqlParameter outputId = new SqlParameter("@App_id", SqlDbType.Int);
                outputId.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputId);

                conn.Open();
                result = cmd.ExecuteNonQuery();
                result = Convert.ToInt32(outputId.Value);
            }
            return result;

        }
        public int SaveContactEnquiry(EnquiryModel objML_contact)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                SqlCommand cmd = new SqlCommand("enquirysp", conn);
                cmd.CommandType = CommandType.StoredProcedure;
              

                cmd.Parameters.AddWithValue("@fName", objML_contact.FName);
                cmd.Parameters.AddWithValue("@organizationname", objML_contact.company);
                cmd.Parameters.AddWithValue("@emailid", objML_contact.EmailId);
                cmd.Parameters.AddWithValue("@mobile", objML_contact.phone);
                cmd.Parameters.AddWithValue("@City", objML_contact.country);
                cmd.Parameters.AddWithValue("@Address", objML_contact.address);
                cmd.Parameters.AddWithValue("@fmessage", objML_contact.country);
                cmd.Parameters.AddWithValue("@division", objML_contact.OrganizationName);
                cmd.Parameters.AddWithValue("@corporate_group", objML_contact.corp_grup);

                cmd.Parameters.AddWithValue("@uname", "user");
                cmd.Parameters.AddWithValue("@mode", 1);
                cmd.Parameters.Add("@eid", SqlDbType.Int, 0, "@eid").Direction = ParameterDirection.Output;
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            return result;

        }
        public async Task<DataTable> GetInvestorList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindInvestorProductCateSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetInvestorSubCategoryList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindInvestorProductSubCateSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetProductList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProductSolutionSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetProductCategoryList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProductCategorySP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetCategoryList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindCategoryListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetProductSubCategoryList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProductSubCategorySP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetNewsEvents()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindEventsListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetBannerList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetHomeBannerSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetBannerPopupList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPopupBannerListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetAboutProduct()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAboutProductSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetProductCapabilities()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetProductCapabilitiesSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetDynamicTableSEO(string table_name)
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSeoSectionSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tablename", table_name);
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetLeadershipList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetOurTeamListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetBlogList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBlogListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetProductSolutionList()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BindProductSolutionSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
        public async Task<DataTable> GetSeoFriendlyStaticRedirectionUrls()
        {
            DataTable seoFriendlyUrls;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSeoFriendlyStaticRedirectionUrlsSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        seoFriendlyUrls = table;
                    }
                }
            }
            return seoFriendlyUrls;
        }

        public async Task<List<clsSearchModel>> GetsearchList(string q)
        {
            var items = new List<clsSearchModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("GlobalSearchSP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Query", SqlDbType.NVarChar, 200).Value = q;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            items.Add(new clsSearchModel
                            {
                                PageId = reader["PageId"] != DBNull.Value
                                    ? Convert.ToInt32(reader["PageId"])
                                    : 0,

                                Title = reader["Title"]?.ToString(),

                                ShortDesc = reader["ShortDesc"] != DBNull.Value
                                    ? reader["ShortDesc"].ToString()
                                    : null,

                                LongDesc = reader["LongDesc"] != DBNull.Value
                                    ? reader["LongDesc"].ToString()
                                    : null,

                                PageUrl = reader["PageUrl"] != DBNull.Value
                                    ? reader["PageUrl"].ToString()
                                    : null,

                                RewriteUrl = reader["RewriteUrl"] != DBNull.Value
                                    ? reader["RewriteUrl"].ToString()
                                    : null,

                                RewriteID = reader["RewriteID"] != DBNull.Value
                                    ? Convert.ToInt32(reader["RewriteID"])
                                    : 0
                            });
                        }
                    }
                }
            }

            return items;
        }
        public async Task<DataTable> GetLicenseExpire()
        {
            DataTable menuList;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetLicenseExpireListSP", conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataTable table = new DataTable();
                        await conn.OpenAsync();
                        da.Fill(table);
                        menuList = table;
                    }
                }
            }
            return menuList;
        }
    }
}
