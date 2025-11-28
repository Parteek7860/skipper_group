
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using skipper_group_new.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace skipper_group_new.Repositories
{
    public class SkipperHomeRepository : ISkipperHomeRepository
    {
        private readonly string _connectionString;

        public SkipperHomeRepository(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
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
                using (SqlCommand cmd = new SqlCommand("BindPageMasterSP", conn))
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
                cmd.Parameters.AddWithValue("@App_Address", objML_contact.address);
                cmd.Parameters.AddWithValue("@MaritalStatus", objML_contact.zipcode);
                cmd.Parameters.AddWithValue("@state", objML_contact.state);

                cmd.Parameters.AddWithValue("@uname", "user");
                cmd.Parameters.AddWithValue("@mode", 1);
                cmd.Parameters.Add("@App_id", SqlDbType.Int, 0, "@App_id").Direction = ParameterDirection.Output;
                conn.Open();
                result = cmd.ExecuteNonQuery();
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
    }
}
