using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Service;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace skipper_group_new.Controllers
{
    public class EngineeringController : BaseController
    {
        private readonly List<UrlValidationRule> _validationRules;
        private readonly ISkipperHome _homePageService;

        public EngineeringController(ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService)
     : base(homePageService, menuService)
        {
            _homePageService = homePageService;

            _validationRules = configuration.GetSection("UrlValidationRules:Rules")
             .Get<List<UrlValidationRule>>() ?? new();
        }
        [HttpGet]
        [Route("/{productname}/{productid:int}")]
        public async Task<IActionResult> engineering(string productname, string productid)
        {
            clsHomeModel obj = new clsHomeModel();
            await LoadMenu(productid);
            //  About Product (landing Page)
            var i = await _homePageService.GetAboutProduct();
            var filter = $" productid='{productid.ToString()}'";
            DataRow[] res = i.Select(filter);
            if (res.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)res).CopyToDataTable<DataRow>();
                obj.Description = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["details"]));
                
            }
            if (string.IsNullOrEmpty(productid) || !int.TryParse(productid, out int projId))
            {
                return RedirectToAction("engineering", "Engineering");
            }
            else
            {
                var x = await this._homePageService.GetBannerList();
                DataRow[] results = x.Select($"status=1 and collageid='{productid.ToString()}'");
                if (results.Length > 0)

                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.uploadimage = Convert.ToString(dt.Rows[0]["bannerimage"]);
                    obj.shortdesc = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["tagline1"]));

                }
                // Product Master List


                return View("engineering", obj);
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> projlist(int id)
        {

            await LoadSeoDataAsync(id);
            //await LoadCMSDataAsync(id);
            clsHomeModel obj = new clsHomeModel();
            var x = await this._homePageService.GetCMSData();
            DataRow[] results = x.Select($"pagestatus=1 and pageid='{id.ToString()}'");
            if (results.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                obj.id = Convert.ToString(dt.Rows[0]["collageid"]);
                obj.uploadimage = Convert.ToString(dt.Rows[0]["uploadbanner"]);
                obj.Name = Convert.ToString(dt.Rows[0]["linkname"]);
                obj.tagline = Convert.ToString(dt.Rows[0]["tagline"]);

            }
            await LoadMenu(obj.id);

            // Projects Master List
            var x1 = await _homePageService.GetProjectsList();
            DataRow[] data = x1.Select($"status=1 and ntypeid={obj.id}");
            ViewBag.ProjList = data.CopyToDataTable();

            return View("/Views/engineering/projlist.cshtml", obj);
        }

        public async Task<IActionResult> LoadMenu(string productid)
        {
            var x1 = await _homePageService.GetProductList();
            DataRow[] data = x1.Select($"status=1");
            ViewBag.ProductList = data.CopyToDataTable();
            ViewBag.CurrentProductId = productid;

            // Menu List
            var menuData = await _homePageService.GetMenuList();
            DataRow[] menuresult = menuData.Select($"pagestatus=1 and linkposition like '%Header,External%' and collageid='{productid.ToString()}'");
            ViewBag.MenuList = menuresult.CopyToDataTable();

            DataRow[] menuresult1 = menuData.Select($"pagestatus=1 and linkposition like '%Hamburger%' and collageid='{productid.ToString()}'");
            ViewBag.SubHamburger = menuresult1.CopyToDataTable();

            return View();
        }

    }
}
