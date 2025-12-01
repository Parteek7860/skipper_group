
using DocumentFormat.OpenXml.Bibliography;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace skipper_group_new.Controllers
{
    public class BaseController : Controller
    {

        private readonly ISkipperHome _homePageService;
        protected readonly MenuDataService _menuService;
        public SeoModel PageSeo { get; private set; } = new SeoModel();

        public BaseController(ISkipperHome homePageService, MenuDataService menuService)
        {
            _homePageService = homePageService;
            _menuService = menuService;
        }
        public List<clsHomeModel> TopMenuList => _menuService.TopMenuList;
        public List<clsHomeModel> MainMenuList => _menuService.MainMenuList;
        public List<clsHomeModel> HamBurgerList => _menuService.HamBurgerList;
        public List<clsHomeModel> productlist => _menuService.productlist;
        public List<SeoModel> SeoList => _menuService.SeoList;


        public List<clsHomeModel> InvestorList => _menuService.InvestorList;


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            base.OnActionExecuting(context);


            _menuService.productlist = await GetProductlist();
            _menuService.InvestorList = await GetInvestorList();
            _menuService.MainMenuList = await LoadMainMenu();
            _menuService.HamBurgerList = await LoadHamburgerMenus();


            await next();

        }
        protected async Task LoadCMSDataAsync(int pageId)
        {
            var dt = await _homePageService.GetCMSData();
            if (dt == null || dt.Rows.Count == 0)
                return;

            var row = dt.AsEnumerable()
                .FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == pageId);

            var parentId = Convert.ToInt32(row["parentid"]);

            var parent = dt.AsEnumerable()
                ?.FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == parentId);

            string parentName = parent?["linkname"]?.ToString() ?? "";


            if (row != null)
            {
                var seo = new clsHomeModel
                {
                    Name = row["linkname"]?.ToString() ?? "Metro Tyres",
                    uploadimage = row["uploadbanner"]?.ToString() ?? "",
                    SmallDescription = row["smalldesc"]?.ToString() ?? "",
                    cmscontent = row["pagedescription"]?.ToString() ?? "",
                    tagline = row["tagline"]?.ToString() ?? "",
                    parentname = parentName,
                    id = row["parentid"]?.ToString() ?? ""



                };


                _menuService.GetCMSData = new List<clsHomeModel> { seo };
            }
        }
        protected async Task LoadSeoDataAsync(int pageId)
        {
            var dt = await _homePageService.GetCMSData();
            if (dt == null || dt.Rows.Count == 0)
                return;

            var row = dt.AsEnumerable()
                .FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == pageId);

            if (row != null)
            {
                var seo = new SeoModel
                {
                    Title = row["pagetitle"]?.ToString() ?? "Metro Tyres",
                    MetaDescription = row["pagemetadesc"]?.ToString() ?? "",
                    MetaKeywords = row["pagemeta"]?.ToString() ?? "",
                    Robots = row["no_indexfollow"]?.ToString() ?? "index,follow",
                    CanonicalUrl = row["rewriteurl"]?.ToString() ?? ""
                };

                // Store in both ViewData and Service property (optional cache)
                ViewData["PageSeo"] = seo;
                _menuService.SeoList = new List<SeoModel> { seo };
            }
        }

        protected async Task LoadTableSeoDataAsync(string tablename, string column_name, int pageId)
        {
            var dt = await _homePageService.GetDynamicTableSEO(tablename);
            if (dt == null || dt.Rows.Count == 0)
                return;

            var row = dt.AsEnumerable()
                .FirstOrDefault(r => r.Field<bool>("status") && r.Field<int>(column_name) == pageId);

            if (row != null)
            {
                var seo = new SeoModel
                {
                    Title = row["pagetitle"]?.ToString() ?? "Metro Tyres",
                    MetaDescription = row["pagemetadesc"]?.ToString() ?? "",
                    MetaKeywords = row["pagemeta"]?.ToString() ?? "",
                    Robots = row["no_indexfollow"]?.ToString() ?? "index,follow",
                    CanonicalUrl = row["rewriteurl"]?.ToString() ?? ""
                };

                // Store in both ViewData and Service property (optional cache)
                ViewData["PageSeo"] = seo;
                _menuService.SeoList = new List<SeoModel> { seo };
            }
        }
        protected async Task<IActionResult> LoadInnerMenuAsync(int id)
        {
            if (id <= 0)
                return null;

            var dtTask = _homePageService.GetHamburgerMenuList();
            if (dtTask == null || dtTask.Result.Rows.Count == 0)
                return null;

            var dt = dtTask.Result;

            // 🔹 Find current page row
            DataRow currentRow = dt.AsEnumerable()
                .FirstOrDefault(r => r["pageid"].ToString() == id.ToString());

            if (currentRow == null)
                return null;

            string parentId = currentRow["ParentId"].ToString();
            DataRow[] filterResults;

            if (parentId != "0")
                filterResults = dt.Select($"pagestatus=1 and parentid='{parentId}'");
            else
                filterResults = dt.Select($"pagestatus=1 and parentid='{id}'");

            if (filterResults.Length == 0)
                return null;

            var list = filterResults.CopyToDataTable().AsEnumerable()
                .Select(r => new clsHomeModel
                {
                    linkname = r["linkname"].ToString(),
                    rewriteurl = r["rewriteurl"].ToString(),
                    pageid = r["pageid"].ToString()
                })
                .ToList();

            // ✅ Make available to the view
            ViewBag.InnerMenu = list;
            if (list.Count > 0 && parentId == "0")
            {
                string firstPageId = list.First().pageid;
                string firstRewriteUrl = list.First().rewriteurl;

                ViewBag.CurrentId = firstPageId;
                ViewBag.CurrentPageid = firstPageId;

                // 🔹 Redirect to that rewrite URL
                return RedirectToAction("Page", "MetroHome");

            }
            else
            {
                ViewBag.CurrentId = id;
            }
            return null;
        }

        private async Task SetSeoDataAsync(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            string currentPath = request.Path.Value.Trim('/').ToLower();

            // 🔹 Default path for home page
            if (string.IsNullOrEmpty(currentPath))
                currentPath = "home";

            currentPath = currentPath.Split('?')[0];

            string[] ignoredFolders = { "css", "js", "images", "uploads", "fonts", "lib", "content", "assets" };
            string[] staticExtensions = { ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico", ".webp", ".woff", ".woff2", ".ttf", ".map", ".json", ".mp4", ".pdf" };
            string[] ignoredRoutes = { "backoffice", "admin", "api", "swagger" };

            // 🔹 Skip static or admin routes
            if (ignoredFolders.Any(f => currentPath.StartsWith(f + "/", StringComparison.OrdinalIgnoreCase)) ||
                staticExtensions.Any(ext => currentPath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) ||
                ignoredRoutes.Any(r => currentPath.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            // 🔹 Load CMS data once
            DataTable cmsData = await _homePageService.GetCMSData();
            if (cmsData == null || cmsData.Rows.Count == 0)
                return;

            DataRow? row = null;

            // 🔹 Case 1: Home page (default pageid = 1)
            if (currentPath == "home" || currentPath == "home/index" || currentPath == "/")
            {
                row = cmsData.AsEnumerable()
                    .Where(r => r.Field<bool>("pagestatus") == true && r.Field<int>("pageid") == 1)
                    .FirstOrDefault();
            }

            // 🔹 Case 2: Match rewrite URL
            if (row == null)
            {
                row = cmsData.AsEnumerable()
                    .Where(r =>
                        r.Field<bool>("pagestatus") == true &&
                        (r.Field<string>("rewriteurl") ?? "").Trim('/').ToLower() == currentPath)
                    .FirstOrDefault();
            }

            // 🔹 Case 3: Assign SEO data
            if (row != null)
            {
                PageSeo = new SeoModel
                {
                    Title = row["pagetitle"]?.ToString() ?? "Metro Tyres",
                    MetaDescription = row["pagemetadesc"]?.ToString() ?? "",
                    MetaKeywords = row["pagemeta"]?.ToString() ?? "",
                    Robots = row["no_indexfollow"]?.ToString() ?? "index,follow",
                    CanonicalUrl = $"{request.Scheme}://{request.Host}{request.Path}"
                };
            }
            else
            {
                // 🔹 Default fallback SEO (if nothing matches)
                PageSeo = new SeoModel
                {
                    Title = "Metro Tyres",
                    MetaDescription = "Best quality tyres in India by Metro Tyres.",
                    MetaKeywords = "tyres, metro tyres, india",
                    Robots = "index,follow",
                    CanonicalUrl = $"{request.Scheme}://{request.Host}{request.Path}"
                };
            }

            // 🔹 Safely assign to ViewData (optional, if you still want backward compatibility)
            if (context.Controller is Controller controller)
            {
                controller.ViewData["PageSeo"] = PageSeo;
            }
        }


        //private async Task<List<clsHomeModel>> GetTopMenu()
        //{
        //    var dt = await _homePageService.GetHamburgerMenuList();
        //    if (dt == null || dt.Rows.Count == 0)
        //        return new List<clsHomeModel>();

        //    return dt.Select("pagestatus=1 and linkposition like '%Top-Links%'")
        //        .CopyToDataTable().AsEnumerable()
        //         .OrderBy(r => Convert.ToInt32(r["displayorder"]))
        //        .Select(r => new clsHomeModel
        //        {
        //            linkname = r["linkname"].ToString(),
        //            rewriteurl = r["rewriteurl"].ToString(),
        //            pageid = r["pageid"].ToString(),
        //            pageurl_Id= r["rewriteid"].ToString()
        //        }).ToList();
        //}


        public async Task<string> GetParentID(int pageId)
        {
            var dt = await _homePageService.GetCMSData();



            var row = dt.AsEnumerable()
                .FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == pageId);

            var parentId = Convert.ToInt32(row["parentid"]);

            var parent = dt.AsEnumerable()
                ?.FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == parentId);


            return parentId.ToString();
        }
        public async Task<string> GetParentName(int pageId)
        {
            var dt = await _homePageService.GetCMSData();



            var row = dt.AsEnumerable()
                .FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == pageId);

            var parentId = Convert.ToInt32(row["parentid"]);

            var parent = dt.AsEnumerable()
                ?.FirstOrDefault(r => r.Field<bool>("pagestatus") && r.Field<int>("pageid") == parentId);
            string parentName = parent?["linkname"]?.ToString() ?? "";

            return parentName.ToString();
        }
        private async Task<List<clsHomeModel>> GetInvestorList()
        {
            var dt = await _homePageService.GetInvestorList();
            var dt1 = await _homePageService.GetInvestorSubCategoryList();
            if (dt == null || dt.Rows.Count == 0)
                return (new List<clsHomeModel>());

            var investormenu = dt.Select("status=1")
                .CopyToDataTable().AsEnumerable()
                 .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                .Select(dr => new clsHomeModel
                {
                    Name = dr["category"].ToString(),
                    rewriteurl = dr["rewriteurl"].ToString(),
                    pageid = dr["pcatid"].ToString(),
                    SubMenus = dt1.AsEnumerable()
                        .Where(sub => sub["pcatid"].ToString() == dr["pcatid"].ToString() && Convert.ToInt32(sub["status"]) == 1)
                         .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                        .Select(sub => new SubhomeModel
                        {
                            linkname = sub["category"].ToString(),
                            rewriteurl = sub["rewriteurl"].ToString(),
                            ParentId = sub["pcatid"].ToString(),
                            pageid = sub["psubcatid"].ToString()
                        }).Take(1).ToList()
                }).ToList();
            return investormenu;
        }

        private async Task<List<clsHomeModel>> GetProductlist()
        {
            var dt = await _homePageService.GetProductList();
            var dt1 = await _homePageService.GetProductCategoryList();
            var dt2 = await _homePageService.GetProductSubCategoryList();
            if (dt == null || dt.Rows.Count == 0)
                return (new List<clsHomeModel>());

            var products = dt.Select("status=1")
                .CopyToDataTable().AsEnumerable()
                 .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                .Select(dr => new clsHomeModel
                {
                    Name = dr["productname"].ToString(),
                    rewriteurl = dr["rewrite_url"].ToString(),
                    SmallDescription = dr["productshortdescp"].ToString(),
                    pageid = dr["productid"].ToString(),

                    SubMenus = dt1.AsEnumerable()
                        .Where(sub => sub["productid"].ToString() == dr["productid"].ToString() && Convert.ToInt32(sub["status"]) == 1)
                         .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                        .Select(sub => new SubhomeModel
                        {
                            linkname = sub["category"].ToString(),
                            rewriteurl = sub["rewriteurl"].ToString(),
                            smalldesc = sub["shortdetail"].ToString(),
                            ParentId = sub["productid"].ToString(),
                            pageid = sub["pcatid"].ToString(),
                            // SECOND LEVEL SUBMENUS
                            SubMenus2 = dt2.AsEnumerable()
                     .Where(sub2 => sub2["pcatid"].ToString() == sub["pcatid"].ToString()
                                    && Convert.ToInt32(sub2["status"]) == 1)
                     .OrderBy(sub2 => Convert.ToInt32(sub2["displayorder"]))
                     .Select(sub2 => new SubhomeModel2
                     {
                         linkname = sub2["category"].ToString(),
                         rewriteurl = sub2["rewriteurl"].ToString(),
                         ParentId = sub2["pcatid"].ToString(),
                         smalldesc = sub2["shortdetail"].ToString(),
                         pageid = sub2["psubcatid"].ToString()
                     }).ToList()
                        }).ToList(),


                }).ToList();
            return products;
        }

        private async Task<List<clsHomeModel>> LoadMainMenu()
        {
            var dt = await _homePageService.GetMenuList();



            if (dt == null || dt.Rows.Count == 0)
                return (new List<clsHomeModel>());

            var result = dt.Select("pagestatus=1 AND collageid=0 AND linkposition LIKE '%header%'")
     .CopyToDataTable()
     .AsEnumerable()
     .OrderBy(r => Convert.ToInt32(r["displayorder"]))
     .Select(dr => new clsHomeModel
     {
         Name = dr["linkname"].ToString(),
         rewriteurl = dr["rewriteurl"].ToString(),
         pageid = dr["pageid"].ToString(),
         collageid = dr["collageid"].ToString(),
         // FIRST LEVEL SUB MENUS
         SubMenus = dt.AsEnumerable()
             .Where(sub => sub["ParentId"].ToString() == dr["pageid"].ToString()
                           && Convert.ToInt32(sub["pagestatus"]) == 1)
             .OrderBy(sub => Convert.ToInt32(sub["displayorder"]))
             .Select(sub => new SubhomeModel
             {
                 linkname = sub["linkname"].ToString(),
                 rewriteurl = sub["rewriteurl"].ToString(),
                 ParentId = sub["ParentId"].ToString(),
                 smalldesc = sub["smalldesc"].ToString(),
                 pageid = sub["pageid"].ToString(),


                 // SECOND LEVEL SUBMENUS
                 SubMenus2 = dt.AsEnumerable()
                     .Where(sub2 => sub2["ParentId"].ToString() == sub["pageid"].ToString()
                                    && Convert.ToInt32(sub2["pagestatus"]) == 1)
                     .OrderBy(sub2 => Convert.ToInt32(sub2["displayorder"]))
                     .Select(sub2 => new SubhomeModel2
                     {
                         linkname = sub2["linkname"].ToString(),
                         rewriteurl = sub2["rewriteurl"].ToString(),
                         ParentId = sub2["ParentId"].ToString(),
                         smalldesc = sub2["smalldesc"].ToString(),
                         pageid = sub2["pageid"].ToString()
                     }).ToList()
             }).ToList()
     }).ToList();



            return result;
        }

        private async Task<List<clsHomeModel>> LoadHamburgerMenus()
        {
            var dt = await _homePageService.GetHamburgerMenuList();
            if (dt == null || dt.Rows.Count == 0)
                return (new List<clsHomeModel>());

            var leftMenu = dt.Select("pagestatus=1 and collageid=0 AND linkposition like '%Hamburger%'")
                .CopyToDataTable().AsEnumerable()
                 .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                .Select(dr => new clsHomeModel
                {
                    Name = dr["linkname"].ToString(),
                    rewriteurl = dr["rewriteurl"].ToString(),
                    pageid = dr["pageid"].ToString(),
                    SubMenus = dt.AsEnumerable()
                        .Where(sub => sub["ParentId"].ToString() == dr["pageid"].ToString() && Convert.ToInt32(sub["pagestatus"]) == 1)
                         .OrderBy(r => Convert.ToInt32(r["displayorder"]))
                        .Select(sub => new SubhomeModel
                        {
                            linkname = sub["linkname"].ToString(),
                            rewriteurl = sub["rewriteurl"].ToString(),
                            ParentId = sub["ParentId"].ToString(),
                            pageid = sub["pageid"].ToString()
                        }).ToList()
                }).ToList();




            return (leftMenu);
        }




    }
}

