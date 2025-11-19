
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
    }
}
