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
            //string url = productname + "/" + productid;
            //if (!string.IsNullOrEmpty(url))
            //{
            //    Task<DataTable> pageid = this._homePageService.GetCMSData();
            //    if (pageid != null)
            //    {
            //        DataRow[] data = pageid.Result.Select($"pagestatus=1 and rewriteurl='{url.ToString()}'");
            //        if (data.Length > 0)
            //        {
            //            DataTable dt = ((IEnumerable<DataRow>)data).CopyToDataTable<DataRow>();
            //            productid = Convert.ToString(dt.Rows[0]["pageid"]);
            //        }

            //    }
            //}


            //if (!string.IsNullOrEmpty(id))
            //{
            //    await LoadCMSDataAsync(Convert.ToInt32(id));
            //    Task<DataTable> x = this._homePageService.GetCMSData();
            //    if (x != null)
            //    {
            //        DataRow[] results = x.Result.Select($"pagestatus=1 and pageid='{productid.ToString()}'");
            //        if (results.Length == 0)
            //        {
            //            return (IActionResult)this.RedirectToAction("Index", "SkipperHome");
            //        }
            //        else
            //        {
            //            DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
            //            obj.cmscontent = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
            //            obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));
            //            obj.Name = Convert.ToString(dt.Rows[0]["pagename"]);
            //            obj.uploadimage = Convert.ToString(dt.Rows[0]["uploadbanner"]);
            //            obj.pageid = Convert.ToString(dt.Rows[0]["pageid"]);
            //            obj.parentname = "";

            //            return View("~/Views/engineering/engineering-cms.cshtml", obj);
            //        }

            //    }
            //}
            //else
            {
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
                    var x1 = await _homePageService.GetProductList();
                    DataRow[] data = x1.Select($"status=1");
                    ViewBag.ProductList = data.CopyToDataTable();
                    ViewBag.CurrentProductId = productid;

                    // Menu List
                    var menuData = await _homePageService.GetMenuList();
                    DataRow[] menuresult = menuData.Select($"pagestatus=1 and linkposition like '%Header,External%' and collageid='{productid.ToString()}'");
                    ViewBag.MenuList = menuresult.CopyToDataTable();

                    return View("engineering", obj);
                }
            }

            return View();
        }




    }
}
