using DocumentFormat.OpenXml.Office2010.Excel;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EngineeringController(ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService, IHttpContextAccessor httpContextAccessor)
     : base(homePageService, menuService, httpContextAccessor)
        {
            _homePageService = homePageService;

            _validationRules = configuration.GetSection("UrlValidationRules:Rules")
             .Get<List<UrlValidationRule>>() ?? new();
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("/{productname}/{productid:int}")]
        public async Task<IActionResult> engineering(string productname, string productid)
        {
            await LoadTableSeoDataAsync("product_master", "productid", Convert.ToInt32(productid));
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
            //  About Product  capabilities(landing Page)
            var i1 = await _homePageService.GetProductCapabilities();
            var filter1 = $" productid='{productid.ToString()}'";
            DataRow[] res1 = i1.Select(filter1);
            if (res1.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)res1).CopyToDataTable<DataRow>();
                obj.Capabilities = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["details"]));

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
                if (productid != "3")
                {
                    // Product Category List (Landing Page)
                    var pcat = await _homePageService.GetProductCategoryList();
                    DataRow[] pcatdata = pcat.Select($"status=1 and productid={productid}");
                    ViewBag.ProductCatList = pcatdata.CopyToDataTable();

                    // Featured Projects List (Landing Page)
                    var proj = await _homePageService.GetProjectsList();


                    //DataRow[] projdata = proj.Select($"status=1 and ntypeid={productid}").OrderBy(r => r.Field<int>("displayorder")).ToArray();

                    //var firstOne = projdata.Take(1).CopyToDataTable();
                    //var firstTwo = projdata.Skip(1).Take(2).CopyToDataTable();

                    DataRow[] projdata = proj
    .Select($"status=1 and ntypeid={productid}")
    .OrderBy(r => r.Field<int>("displayorder"))
    .ToArray();

                    DataTable firstOne = proj.Clone();
                    DataTable firstTwo = proj.Clone();

                    if (projdata.Length > 0)
                    {
                        firstOne = projdata.Take(1).CopyToDataTable();
                    }

                    if (projdata.Length > 1)
                    {
                        firstTwo = projdata.Skip(1).Take(2).CopyToDataTable();
                    }


                    ViewBag.FeaturedProjList1 = firstOne;
                    ViewBag.FeaturedProjList2 = firstTwo;
                }
                else
                {
                    var sub = await _homePageService.GetProductSubCategoryList();
                    DataRow[] pcatdata = sub.Select($"status=1");
                    ViewBag.ProductSubCatList = pcatdata.CopyToDataTable();

                    obj.id = productid;
                }
                obj.collageid = productid;


                return View("engineering", obj);
            }



            return View(obj);
        }
        public IActionResult GetProjectList()
        {
            var x = _homePageService.GetCategoryList();
            ViewBag.ProjectTypes = x.Result.Select("status=1").OrderBy(r => Convert.ToInt32(r["displayorder"])).Select(r => new clsHomeModel
            {
                id = Convert.ToString(r["pcatid"]),
                Name = Convert.ToString(r["category"])
            })
    .ToList();


            var list = _homePageService.GetProjectsList();
            var filterlist = list.Result.Select("status=1").OrderBy(r => Convert.ToInt32(r["displayorder"]));
            ViewBag.ProjectsList = filterlist.CopyToDataTable();
            return View();


        }

        [HttpGet]
        public async Task<IActionResult> projlist(int id)
        {
            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
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

            GetProjectList();

            return View("/Views/engineering/projlist.cshtml", obj);
        }

        public async Task<IActionResult> LoadMenu(string productid)
        {
            var x1 = await _homePageService.GetProductList();
            DataRow[] data;
            data = x1.Select("status=1");

            ViewBag.ProductList = data.Length > 0
                ? data.CopyToDataTable()
                : x1.Clone();

            ViewBag.CurrentProductId = productid;

            // Menu List
            var menuData = await _homePageService.GetMenuList();
            DataRow[] menuresult = menuData.Select($"pagestatus=1 and linkposition like '%Header,External%' and collageid='{productid.ToString()}'");
            if (menuresult.Length > 0)
            {
                ViewBag.MenuList = menuresult.CopyToDataTable();
            }
            else
            {
                ViewBag.MenuList = menuData.Clone();
            }


            DataRow[] menuresult1 = menuData.Select($"pagestatus=1 and linkposition like '%Hamburger%' and collageid='{productid.ToString()}'");
            if (menuresult1.Length > 0)
            {
                ViewBag.SubHamburger = menuresult1.CopyToDataTable();
            }
            else
            {
                ViewBag.SubHamburger = menuresult1.Clone();
            }

            return View();
        }

    }
}
