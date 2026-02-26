using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace skipper_group_new.Controllers
{
    public class HomeBannerController : Controller
    {
        private readonly IHomePage _homePageService;
        private readonly clsMainMenuList _menuService;
        clsBannerType clsBannerType = new clsBannerType();
        clsbanner clsBanner = new clsbanner();

        public HomeBannerController(IHomePage homePageService, clsMainMenuList menuService)
        {
            _homePageService = homePageService;
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("backoffice/homebanner/addhomebannertype")]
        public async Task<IActionResult> addhomebannertype()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            await BindStaticdata();
            var bannerTypes = await _homePageService.GetBannerTypeList();
            DataRow[] rows = bannerTypes.Select("collageid = 0");


            DataTable filteredTable = rows.Length > 0
                ? rows.CopyToDataTable()
                : bannerTypes.Clone();   // returns empty table with same structure

            ViewBag.BannerTypes = filteredTable;


            ViewBag.SuccessCreate = "Save";
            ViewBag.Title = "Home Banner Type";
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }
        [HttpGet]
        [Route("backoffice/homebanner/addhomebannertype/{id:int}")]
        [Route("backoffice/homebanner/addhomebannertype/{name}/{pageid:int}/{id:int}")]

        public async Task<IActionResult> addhomebannertype(int id, int pageid)
        {
            //var pageid = HttpContext.Session.GetString("microid");

            var menuList = _menuService.GetMenu(pageid);
            ViewBag.Menus = menuList;

            await BindStaticdata();

            //Get list of banner types with ID
            var bannerTypes = _homePageService.GetBannerTypeListByID(id);
            if (bannerTypes.Result != null || bannerTypes.Result.Rows.Count > 0)
            {
                clsBannerType.btypeid = Convert.ToInt32(bannerTypes.Result.Rows[0]["btypeid"]);
                clsBannerType.btype = Convert.ToString(bannerTypes.Result.Rows[0]["btype"]);
                clsBannerType.displayorder = Convert.ToString(bannerTypes.Result.Rows[0]["displayorder"]);
                clsBannerType.status = Convert.ToString(bannerTypes.Result.Rows[0]["status"]);
                clsBannerType.collageid = Convert.ToString(bannerTypes.Result.Rows[0]["collageid"]);
            }
            ViewBag.SuccessCreate = "Update";
            ViewBag.Title = "Home Banner Type";

            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }
        [HttpPost]
        [Route("backoffice/homebanner/save")]
        [Route("backoffice/homebanner/save/{name}/{pageid:int}")]
        public async Task<IActionResult> save(clsBannerType bannertype, int pageid)
        {
            try
            {

                var bannerTypes = await _homePageService.GetBannerTypeList();
                DataRow[] rows = bannerTypes.Select("btype = '" + bannertype.btype + "'");
                if (rows != null)
                {
                    if (rows.Length > 0 && bannertype.btypeid == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Banner Type already exists. Please enter a different Banner Type.");
                        return RedirectToAction("addhomebannertype", "HomeBanner");
                    }
                }

                clsBannerType objbannertype = new clsBannerType();
                ModelState.Remove("bannertypeselect");
                ModelState.Remove("collageid");
                ModelState.Remove("status");
                ModelState.Remove("mobilestatus");
                ModelState.Remove("uname");
                ModelState.Remove("mode");

                //await BindStaticdata();
                if (ModelState.IsValid)
                {
                    objbannertype.btypeid = bannertype.btypeid;
                    objbannertype.btype = bannertype.btype;
                    objbannertype.displayorder = bannertype.displayorder;
                    objbannertype.uname = Convert.ToString(HttpContext.Session.GetString("UserName") ?? "System");
                    if (bannertype.btypeid > 0)
                    {
                        objbannertype.mode = "2";
                        objbannertype.status = bannertype.status;
                    }
                    else
                    {
                        objbannertype.mode = "1";
                        objbannertype.status = "1";
                    }


                    objbannertype.mobilestatus = "1";
                    if (Convert.ToInt32(pageid) > 0)
                    {
                        objbannertype.collageid = pageid.ToString();
                    }
                    else
                    {
                        objbannertype.collageid = "0";
                    }
                    int x = _homePageService.CreateBannerType(objbannertype);
                    if (x > 0)
                    {
                        if (objbannertype.btypeid > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Banner Type updated successfully.");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Banner Type Added successfully.");
                        }
                        if (objbannertype.collageid != "0")
                        {
                            return RedirectToAction("addhomebannertype", "HomeBanner", new { name = "micro", pageid = objbannertype.collageid });

                        }
                        else
                        {
                            return RedirectToAction("addhomebannertype", "HomeBanner");
                        }
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Please correct the errors and try again.";
                    return RedirectToAction("addhomebannertype", "HomeBanner");
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", bannertype);
            }
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }
        [HttpGet]
        [Route("backoffice/homebanner/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                clsBannerType objbannertype = new clsBannerType();
                int x = _homePageService.DeleteBannerType(id);
                if (x > 0)
                {
                    ViewBag.Success = "Banner type Delete successfully.";
                    TempData["Title"] = "Home Banner Type";
                    return RedirectToAction("addhomebannertype", "HomeBanner");
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }
        [HttpGet]
        [Route("backoffice/homebanner/UpdateStatus/{id}")]
        [Route("backoffice/homebanner/UpdateStatus/{name}/{pageid}/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, int? pageid)
        {
            try
            {
                clsBannerType objbannertype = new clsBannerType();
                var x = _homePageService.GetBannerTypeListByID(id);
                if (x.Result != null && x.Result.Rows.Count > 0)
                {
                    objbannertype.status = Convert.ToString(x.Result.Rows[0]["status"]) == "True" ? "True" : "False"; // Toggle status

                    int x1 = _homePageService.UpdateBannerType(objbannertype.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "  Status update successfully.");

                        return RedirectToAction("addhomebannertype", "HomeBanner");
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }

        [HttpGet]
        [Route("backoffice/homebanner/updatemobilestatus/{id}")]

        public async Task<IActionResult> updatemobilestatus(int id, int? pageid)
        {
            try
            {
                clsBannerType objbannertype = new clsBannerType();
                var x = _homePageService.GetBannerTypeListByID(id);
                if (x.Result != null && x.Result.Rows.Count > 0)
                {
                    objbannertype.status = Convert.ToString(x.Result.Rows[0]["mobilestatus"]) == "True" ? "True" : "False"; // Toggle status

                    int x1 = _homePageService.UpdateMobileBannerTypeStatus(objbannertype.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "  Status update successfully.");

                        return RedirectToAction("addhomebannertype", "HomeBanner");
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }
        public async Task<IActionResult> BindStaticdata()
        {
            clsBannerType = new clsBannerType();
            clsBannerType.bannertypeselect = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Banner", Value = "Banner" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Video", Value = "Video" }
            };
            return View(clsBannerType);
        }

        // Home Banner
        [HttpGet]
        [Route("backoffice/homebanner/addhomebanner")]
        public async Task<IActionResult> addhomebanner()
        {
            var pageid1 = HttpContext.Request.RouteValues["pageid"]?.ToString();
            var menuList = _menuService.GetMenu(Convert.ToInt16(pageid1));
            ViewBag.Menus = menuList;

            await BindDevicedata();


            ViewBag.CreateUpdate = "Save";
            ViewBag.Title = "Home Banner";
            return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
        }
        [HttpPost]
        [Route("backoffice/homebanner/addhomebanner")]
        [Route("backoffice/homebanner/addhomebanner/{name}/{pageid:int}")]
        public async Task<IActionResult> addhomebanner(clsbanner obj, IFormFile file_Uploader, IFormFile file_Uploader2, int pageid)
        {
            var pageid1 = Convert.ToString(pageid);
            var menuList = _menuService.GetMenu(Convert.ToInt16(pageid1));
            ViewBag.Menus = menuList;

            await BindDevicedata();
            clsbanner objbanner = new clsbanner();
            if (!string.IsNullOrEmpty(obj.name))
            {
                objbanner.name = obj.name;
                objbanner.bannertype = obj.bannertype;
                objbanner.devicetype1 = obj.devicetype1;
                objbanner.shortdesc = obj.shortdesc;
                objbanner.displayorder = obj.displayorder;
                objbanner.status = obj.status;
                if (!string.IsNullOrEmpty(Convert.ToString(pageid1)))
                {
                    objbanner.collageid = pageid1;
                }
                else
                {
                    objbanner.collageid = obj.collageid;
                }

                objbanner.uname = Convert.ToString(HttpContext.Session.GetString("UserName"));
                if (obj.id > 0)
                {
                    objbanner.mode = "2";
                    objbanner.id = obj.id;
                    //objbanner.collageid = "0";
                }
                else
                {
                    objbanner.mode = "1";
                    objbanner.status = "1";
                }
                if (file_Uploader != null && file_Uploader.Length > 0)
                {
                    var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/banner", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file_Uploader.CopyTo(stream);
                    }

                    objbanner.uploadbanner = fileName;
                }
                else
                {
                    objbanner.uploadbanner = obj.uploadbanner ?? string.Empty;
                }
                if (file_Uploader2 != null && file_Uploader2.Length > 0)
                {
                    var fileName = Path.GetFileName(file_Uploader2.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/banner", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file_Uploader2.CopyTo(stream);
                    }

                    objbanner.bannerlogo = fileName;
                }
                else
                {
                    objbanner.bannerlogo = obj.bannerlogo ?? string.Empty;
                }
                objbanner.name = obj.name;
                objbanner.tagline1 = obj.tagline1;
                objbanner.url = obj.url;
                objbanner.startdate = obj.startdate;
                objbanner.enddate = obj.enddate;


                int x = _homePageService.CreateHomeBaner(objbanner);

                if (x > 0)
                {
                    var bannerTypes = _homePageService.GetBannerList();
                    var filterresult = bannerTypes.Result.Select("collageid=0").OrderByDescending(r => r["bid"]);
                    DataTable dt = filterresult.CopyToDataTable();
                    ViewBag.bannerlist = dt;
                    if (obj.id > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Update successfully.");


                        if (Convert.ToInt32(obj.collageid) > 0)
                        {
                            return RedirectToAction("viewhomebanner", "HomeBanner", new { name = "micro", pageid = pageid });
                        }
                        else
                        {
                            return View("~/Views/backoffice/HomeBanner/viewhomebanner.cshtml", objbanner);
                        }

                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Save successfully.");
                        if (pageid > 0)
                        {
                            return RedirectToAction("addhomebanner", "HomeBanner", new { name = "micro", pageid = pageid });
                        }
                        else
                        {
                            return View("~/Views/backoffice/HomeBanner/viewhomebanner.cshtml", objbanner);
                        }

                    }

                }

            }


            ViewBag.CreateUpdate = "Save";
            ViewBag.Title = "Home Banner";
            return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
        }
        public async Task<IActionResult> BindDevicedata()
        {
            clsBanner = new clsbanner();
            clsBanner.devicetype = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Desktop", Value = "Desktop" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Mobile", Value = "Mobile" }
            };
            clsBanner.bannertypeselect = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Banner", Value = "1" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Video", Value = "5" }

            };
            return View(clsBannerType);
        }
        // View Home banner
        [HttpGet]
        [Route("backoffice/homebanner/ViewHomeBanner")]
        [Route("backoffice/homebanner/ViewHomeBanner/micro/{pageid?}")]
        public async Task<IActionResult> ViewHomeBanner(int? pageid)
        {

            var menuList = _menuService.GetMenu(pageid);
            ViewBag.Menus = menuList;

            //Get list of banner types
            var bannerTypes = _homePageService.GetBannerList();
            var filterresult = bannerTypes.Result
      .Select("collageid=0")
      .OrderByDescending(r => r["bid"]);

            DataTable dt = null;

            if (filterresult.Any())
            {
                dt = filterresult.CopyToDataTable();
            }
            else
            {
                dt = bannerTypes.Result.Clone();
            }

            ViewBag.bannerlist = dt;

            return View("~/Views/Backoffice/homebanner/ViewHomeBanner.cshtml", clsBanner);
        }
        [HttpGet]
        [Route("backoffice/homebanner/DeleteBanner/{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                int x = _homePageService.DeleteBanner(id);
                if (x > 0)
                {
                    ViewBag.Success = "Banner Delete successfully.";
                    TempData["Title"] = "Home Banner";
                    return RedirectToAction("ViewHomeBanner", "HomeBanner");
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
            }
            return RedirectToAction("ViewHomeBanner", "HomeBanner");

        }
        [HttpGet]
        [Route("backoffice/homebanner/editBanner/{id}")]
        [Route("backoffice/homebanner/editBanner/{name}/{pageid:int}/{id:int}")]
        public async Task<IActionResult> editBanner(int id, int pageid)
        {
            try
            {
                await BindDevicedata();
                var menuList = _menuService.GetMenu(pageid);
                ViewBag.Menus = menuList;

                var bannerlist = _homePageService.GetBannerListByID(id);
                if (bannerlist.Result != null && bannerlist.Result.Rows.Count > 0)
                {
                    clsBanner.id = Convert.ToInt32(bannerlist.Result.Rows[0]["bid"]);
                    clsBanner.name = Convert.ToString(bannerlist.Result.Rows[0]["title"]);
                    clsBanner.shortdesc = WebUtility.HtmlDecode(Convert.ToString(bannerlist.Result.Rows[0]["tagline1"]));
                    clsBanner.bannertype = Convert.ToString(bannerlist.Result.Rows[0]["btypeid"]);
                    clsBanner.displayorder = Convert.ToString(bannerlist.Result.Rows[0]["displayorder"]);
                    clsBanner.uploadimage = Convert.ToString(bannerlist.Result.Rows[0]["bannerimage"]);
                    clsBanner.uploadbanner = Convert.ToString(bannerlist.Result.Rows[0]["bannerimage"]);
                    clsBanner.bannerlogo = Convert.ToString(bannerlist.Result.Rows[0]["mobileimage"]);
                    clsBanner.status = Convert.ToString(bannerlist.Result.Rows[0]["status"]);
                    clsBanner.devicetype1 = Convert.ToString(bannerlist.Result.Rows[0]["devicetype"]);
                    clsBanner.url = Convert.ToString(bannerlist.Result.Rows[0]["url"]);
                    clsBanner.collageid = Convert.ToString(pageid);
                    clsBanner.tagline1 = WebUtility.HtmlDecode(Convert.ToString(bannerlist.Result.Rows[0]["tagline1"]));
                    clsBanner.startdate = Convert.ToString(bannerlist.Result.Rows[0]["bannerstartdate"]) != "" ? Convert.ToDateTime(bannerlist.Result.Rows[0]["bannerstartdate"]).ToString("yyyy-MM-dd") : "";
                    clsBanner.enddate = Convert.ToString(bannerlist.Result.Rows[0]["bannerenddate"]) != "" ? Convert.ToDateTime(bannerlist.Result.Rows[0]["bannerenddate"]).ToString("yyyy-MM-dd") : "";

                    ViewBag.CreateUpdate = "Update";
                }
            }
            catch (Exception ex)
            {
                //ViewBag.ErrorMessage = "An error occurred while creating the banner type: " + ex.Message;
                return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
            }
            return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);

        }

        [HttpGet]
        [Route("Backoffice/HomeBanner/statusbanner/{id}")]
        public async Task<IActionResult> statusbanner(int id)
        {
            try
            {
                await BindDevicedata();
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                clsbanner obj = new clsbanner();
                var bannerlist = _homePageService.GetBannerListByID(id);
                // With this code block:
                var filteredRows = bannerlist.Result.Select($"bid = {id}");


                if (filteredRows.Length > 0)
                {
                    obj.status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                    int x1 = _homePageService.UpdateBannerStatus(obj.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                        return RedirectToAction("viewhomebanner", "HomeBanner");
                    }
                }
            }
            catch (Exception ex)
            {
                //ViewBag.ErrorMessage = "An error occurred while creating the banner type: " + ex.Message;
                return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
            }
            return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);

        }

        #region Micro Site Home Banner Module
        [HttpGet]
        [Route("backoffice/homebanner/addhomebannertype/micro/{pageid:int}")]
        public async Task<IActionResult> addhomebannertype(string pageid, string name)
        {
            var routeId = pageid;
            var menuList = _menuService.GetMenu(Convert.ToInt16(pageid));

            ViewBag.Menus = menuList;

            await BindStaticdata();
            var bannerTypes = await _homePageService.GetBannerTypeList();
            DataRow[] rows = bannerTypes.Select("collageid ='" + pageid + "'");


            DataTable filteredTable = rows.Length > 0
                ? rows.CopyToDataTable()
                : bannerTypes.Clone();   // returns empty table with same structure

            ViewBag.BannerTypes = filteredTable;

            ViewBag._type = "micro";
            ViewBag.SuccessCreate = "Save";
            ViewBag.Title = "Home Banner Type";
            return View("~/Views/Backoffice/homebanner/addhomebannertype.cshtml", clsBannerType);
        }

        [HttpGet]
        [Route("backoffice/homebanner/addhomebanner/micro/{pageid:int}")]
        public async Task<IActionResult> addhomebanner(int pageid, string name)
        {

            var routeId = pageid;
            var menuList = _menuService.GetMenu(pageid);

            ViewBag.Menus = menuList;

            await BindDevicedata();

            ViewBag._type = "micro";
            ViewBag.CreateUpdate = "Save";
            ViewBag.Title = "Home Banner";
            return View("~/Views/Backoffice/homebanner/addhomebanner.cshtml", clsBanner);
        }

        [HttpGet]
        [Route("backoffice/homebanner/ViewHomeBanner/micro/{pageid:int}")]
        public async Task<IActionResult> ViewHomeBanner(int pageid, string name)
        {

            var routeId = pageid;
            var menuList = _menuService.GetMenu(pageid);

            ViewBag.Menus = menuList;
            ViewBag._type = "micro";
            //Get list of banner types
            var bannerTypes = _homePageService.GetBannerList();
            var filterresult = bannerTypes.Result.Select($"collageid = '{pageid}'").OrderByDescending(r => r["bid"]);
            DataTable dt;

            if (filterresult.Any())
            {
                dt = filterresult.CopyToDataTable();
            }
            else
            {
                // Create an empty table with same structure
                dt = bannerTypes.Result.Clone();
            }

            ViewBag.bannerlist = dt;
            if (pageid > 0)
            {
                ViewBag._Type = "micro";
            }

            return View("~/Views/Backoffice/homebanner/ViewHomeBanner.cshtml", clsBanner);
        }
        #endregion

        #region POPUP BANNER
        [HttpGet]
        [Route("backoffice/homebanner/addpopupbanner")]
        public async Task<IActionResult> AddPopupBanner(int pageid = 0)
        {
            var menuList = _menuService.GetMenu(pageid);
            ViewBag.Menus = menuList;
            var b = new clsbanner();
            b.bannertypeselect = new List<SelectListItem>
            {
                new SelectListItem { Text = "Banner", Value = "Banner" }
            };
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/Backoffice/homebanner/addpopupbanner.cshtml", b);
        }

        [HttpPost]
        [Route("backoffice/homebanner/addpopupbanner")]
        public IActionResult AddEditPopupBanner(clsbanner model, IFormFile PopupBannerFile)
        {
            var requiredFields = new[] { "bannertype", "name", "startdate" };

            foreach (var key in ModelState.Keys.ToList())
            {
                if (!requiredFields.Contains(key))
                {
                    ModelState.Remove(key);
                }
            }

            if (!ModelState.IsValid)
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                ViewBag.CreateUpdate = "Save";

                model.bannertypeselect = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Banner", Value = "Banner" }
                };

                return View("~/Views/Backoffice/homebanner/addpopupbanner.cshtml", model);
            }


            var objbanner = new clsbanner
            {
                id = model.id,
                status = model.status ?? "1",
                bannertype = model.bannertype,
                name = model.name,
                startdate = model.startdate,
                enddate = model.enddate,
                url = model.url,
                displayorder = model.displayorder,
                uname = Convert.ToString(HttpContext.Session.GetString("UserName")) ?? "Syatem",
                bannerlogo = model.bannerlogo
            };
            if (model.id > 0)
            {
                objbanner.mode = "2";
            }
            else
            {
                objbanner.mode = "1";
            }

            if (PopupBannerFile != null && PopupBannerFile.Length > 0)
            {
                var fileName = Path.GetFileName(PopupBannerFile.FileName);
                var filePath = Path.Combine("wwwroot/uploads/banner", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    PopupBannerFile.CopyTo(stream);
                }

                objbanner.bannerlogo = fileName;
            }

            int x = _homePageService.AddEditPopupBanner(objbanner);

            if (x > 0)
            {
                HttpContext.Session.SetString("Message",
                    (HttpContext.Session.GetString("Message") ?? "") +
                    (model.id > 0 ? " Updated successfully." : " Saved successfully.")
                );

                return RedirectToAction("ViewPopupBanner");
            }

            return RedirectToAction("ViewPopupBanner");
        }

        [HttpGet]
        [Route("backoffice/homebanner/getpopupdatabyid/{id}")]
        public async Task<IActionResult> GetPopupData(int? id)
        {
            if (id == null || id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Banner ID.";
                return RedirectToAction("ViewPopupBanner");
            }

            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;

                var data = await _homePageService.GetPopupData();

                if (data == null || data.Rows.Count == 0)
                {
                    TempData["ErrorMessage"] = "No popup banner data found.";
                    return RedirectToAction("ViewPopupBanner");
                }

                var row = data.AsEnumerable()
                              .FirstOrDefault(x => x.Field<int>("bid") == id);

                if (row == null)
                {
                    TempData["ErrorMessage"] = "Popup banner not found.";
                    return RedirectToAction("ViewPopupBanner");
                }

                var obj = new clsbanner
                {
                    id = row.Field<int>("bid"),
                    bannertype = row.Field<string>("btype"),
                    name = row.Field<string>("Title"),
                    shortdesc = row.Field<string>("tagline1"),
                    bannerlogo = row.Field<string>("bannerimage"),
                    displayorder = row.Field<int>("displayorder").ToString(),
                    status = row.Field<bool>("status") ? "1" : "0",
                    url = row.Field<string>("url"),
                    collageid = row.Field<int?>("collageid")?.ToString(),
                    startdate = row.Field<DateTime?>("popupstartdate") != null ? row.Field<DateTime>("popupstartdate").ToString("yyyy-MM-dd") : "",
                    enddate = row.Field<DateTime?>("popupenddate") != null ? row.Field<DateTime>("popupenddate").ToString("yyyy-MM-dd") : ""
                };

                obj.bannertypeselect = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Banner", Value = "Banner" }
                };

                ViewBag.CreateUpdate = "Update";

                return View("~/Views/Backoffice/homebanner/addpopupbanner.cshtml", obj);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching popup banner.";
                return RedirectToAction("ViewPopupBanner");
            }
        }


        [HttpGet]
        [Route("backoffice/homebanner/deletepopup/{id}")]
        public IActionResult deletepopup(int id)
        {
            if (id > 0)
            {
                var deleted = _homePageService.DeletePopup(id);

                if (deleted > 0)
                    HttpContext.Session.SetString("Message", "Deleted successfully.");
                else
                    TempData["ErrorMessage"] = "Failed to delete.";
            }

            return RedirectToAction("ViewPopupBanner");
        }


        [HttpGet]
        [Route("backoffice/homebanner/changestatus/{id}")]
        public IActionResult changestatus(int id)
        {
            if (id > 0)
            {
                var updated = _homePageService.ChangeStatus(id);

                if (updated > 0)
                    HttpContext.Session.SetString("Message", "Status updated successfully.");
                else
                    TempData["ErrorMessage"] = "Failed to update status.";
            }

            return RedirectToAction("ViewPopupBanner");
        }

        [HttpGet]
        [Route("backoffice/homebanner/viewpopupnbanner")]
        public async Task<IActionResult> ViewPopupBanner()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var bannerTypes = await _homePageService.GetPopupData();
            return View("~/Views/Backoffice/homebanner/viewpopupbanner.cshtml", bannerTypes);
        }
        #endregion POPUP BANNER
    }
}
