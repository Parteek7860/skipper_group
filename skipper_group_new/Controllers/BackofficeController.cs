using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class BackofficeController : Controller
    {
        private readonly IHomePage _homePageService;
        private readonly clsMainMenuList _menuService;
        public int parentcode = 0;
        public BackofficeController(IHomePage homePageService, clsMainMenuList menuService)
        {
            _homePageService = homePageService;
            _menuService = menuService;
        }

        [Route("backoffice")]
        public IActionResult Index()
        {
            return View("~/Views/Backoffice/index.cshtml");
        }

        [HttpGet]
        [Route("backoffice/signin")]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [Route("backoffice/dashboard")]
        public async Task<IActionResult> dashboard(clsLogin objlogin)
        {
            if (objlogin.UserName == null || objlogin.Password == null)
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }
            var detail = await _homePageService.GetSignInDetails(objlogin.UserName, objlogin.Password);
            if (detail != null && detail.Rows.Count > 0)
            {
                HttpContext.Session.SetString("UserName", objlogin.UserName);
                HttpContext.Session.SetString("UserId", detail.Rows[0]["UserId"].ToString());
                HttpContext.Session.SetString("ClientName", "Skipper Group");
                BindMenuList();
                BindListofdashBoard();
                return View("/Views/Backoffice/DashBoard.cshtml");
            }
            else
            {
                ModelState.AddModelError("Message", "Invalid username or password.");
                ViewBag.Message = "Invalid username or password.";
                return View("index", objlogin);
            }
            return View();
        }


        [HttpGet]
        [Route("backoffice/dashboard")]
        public async Task<IActionResult> dashboard()
        {
            BindListofdashBoard();
            await BindMenuList();
            return View("/Views/Backoffice/DashBoard.cshtml");
        }
        [HttpGet]
        [Route("backoffice/dashboard/{id}")]
        public async Task<IActionResult> dashboard(int id)
        {   
            BindListofdashBoard();
            await BindMenuList();
            return View("/Views/Backoffice/DashBoard.cshtml");
        }
        [HttpGet]
        [Route("backoffice/dashboard/{name}/{pageid:int}")]
        public async Task<IActionResult> dashboard(string name, string pageid)
        {
            
            BindListofdashBoard();
            HttpContext.Session.SetString("microid",pageid);
            HttpContext.Session.SetString("micro", name);
           
            parentcode = 1;
           
            ViewBag._type = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            await BindMenuList();
            return View("/Views/Backoffice/DashBoard.cshtml");
        }

        [HttpGet]
        [Route("backoffice/Signout")]
        public IActionResult signout()
        {
            ViewBag.UserName = null;
            ViewBag.UserId = null;
            ViewBag.ClientName = null;
            HttpContext.Session.Clear();
            return View("/Views/Backoffice/Index.cshtml");
        }
        public async Task<IActionResult> BindMenuList()
        {
            var menuList = await _homePageService.GetMenuList();
            if (parentcode > 0)
            {
                var rows = menuList.AsEnumerable()
                                   .Where(r => r.Field<int>("pareentcode") == parentcode);

                menuList = rows.Any() ? rows.CopyToDataTable() : menuList.Clone();
            }
            else
            {
                var rows = menuList.AsEnumerable()
                                   .Where(r => r.Field<int>("moduleid") != 45);

                menuList = rows.Any() ? rows.CopyToDataTable() : menuList.Clone();
            }
                var menus = new List<clsmainmenu>();
            var menusform = new List<clsmainmenu>();

            if (menuList != null && menuList.Rows.Count > 0)
            {
                foreach (DataRow row in menuList.Rows)
                {
                    var menu = new clsmainmenu
                    {
                        Title = row["moduleid"].ToString(),
                        linkname = row["modulename"].ToString(),
                        Forms = new List<clsmainmenu>()
                    };

                    var formlist = _homePageService.GetFormList(row["moduleid"].ToString());
                    if (formlist.Result != null && formlist.Result.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in formlist.Result.Rows)
                        {
                            menu.Forms.Add(new clsmainmenu
                            {
                                Moduleid = row1["formid"].ToString(),
                                modulename = row1["formcaption"].ToString(),
                                Url_name = "/backoffice/" + row1["formname"].ToString().Replace(".aspx", "")
                            });
                        }
                    }
                    menus.Add(menu);
                }
            }
            ViewBag.Menus = menus;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult BindListofdashBoard()
        {
            var content = _homePageService.GetEventList();
            ViewData["media"] = content.Result.Rows.Count;

            content = _homePageService.GetBannerList();
            ViewData["banner"] = content.Result.Rows.Count;

            content = _homePageService.GetPageList();
            ViewData["totalpages"] = content?.Result.Rows.Count ?? 0;

            content = _homePageService.GetEnquiryList();
            var enquiryTable = content?.Result;
            ViewData["enquiry"] = content?.Result.Rows.Count ?? 0;

            var topEnquiries = enquiryTable?.AsEnumerable()
            .Select(row => new DashboardModel
            {
                EID = row.Field<int>("eid"),
                FName = row.Field<string>("FName"),
                EmailId = row.Field<string>("EmailId"),
                Mobile = row.Field<string>("Mobile"),
                FMessage = row.Field<string>("FMessage"),
                Subject = row.Field<string>("Subject"),
                TrDate = Convert.ToString(row.Field<DateTime>("TrDate")),
                Status = row.Field<bool>("Status") ? "Active" : "Inactive"
            })
            .Take(5)
            .ToList();
            ViewBag.TopEnquiry = topEnquiries;
            return View();
        }

        [HttpGet]
        [Route("backoffice/users/changepass")]
        public IActionResult ChangePassword()
        {
            return View("/Views/backoffice/users/changepass.cshtml");
        }

        [HttpPost]
        [Route("backoffice/users/ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordModel model)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Message"] = "Session expired or invalid user.";
                return RedirectToAction("Index", "Backoffice");
            }

            model.UserId = userId;
            var result = await _homePageService.ChangeUserPasswordAsync(model);
            TempData["Message"] = result == 1 ? "Password changed successfully." : "Failed to change password.";
            return RedirectToAction("Index", "Backoffice");
        }
        [HttpGet]
        [Route("backoffice/testimonials/addtestimonials")]
        public IActionResult addtestimonials()
        {
            ViewBag.Menus = _menuService.GetMenu();
            return View("/Views/backoffice/testimonials/addtestimonials.cshtml");
        }
        [HttpPost]
        [Route("backoffice/testimonials/addtestimonials")]
        public IActionResult addtestimonials(clsTestimonial obj, IFormFile file_Uploader)
        {
            ViewBag.Menus = _menuService.GetMenu();
            clsTestimonial objcls = new clsTestimonial();

            objcls.testimonialid = obj.testimonialid;
            objcls.placed = obj.placed;
            objcls.designation = obj.designation;
            objcls.Name = obj.Name;
            objcls.Description = obj.Description;
            objcls.detail = obj.detail;
            objcls.displayorder = obj.displayorder ?? "0";
            objcls.status = obj.status;
            objcls.uname = HttpContext.Session.GetString("UserName");
            if (obj.testimonialid != null && obj.testimonialid != "" && Convert.ToInt32(obj.testimonialid) > 0)
            {
                objcls.mode = "2";
            }
            else
            {
                objcls.mode = "1";
                objcls.status = true;
            }
            if (file_Uploader != null && file_Uploader.Length > 0)
            {
                var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                var filePath = Path.Combine("wwwroot/uploads/TestimonialImage", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file_Uploader.CopyTo(stream);
                }

                objcls.Image = fileName;
            }
            else
            {
                objcls.Image = obj.Image ?? string.Empty;
            }
            objcls.pagetitle = obj.pagetitle ?? string.Empty;
            objcls.metadesc = obj.metadesc ?? string.Empty;
            objcls.metakeywords = obj.metakeywords ?? string.Empty;
            objcls.canonical = obj.canonical ?? string.Empty;
            objcls.uploadvedio = obj.uploadvedio ?? string.Empty;
            int x = _homePageService.AddTestimonials(objcls);
            if (x > 0)
            {
                if (Convert.ToInt32(obj.testimonialid) > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Testimonials updated successfully.");
                    return RedirectToAction("viewtestimonials", "Backoffice");
                }
                else
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Testimonials added successfully.");
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to save Testimonials. Please try again.");
            }
            return View("/Views/backoffice/testimonials/addtestimonials.cshtml");
        }
        [HttpGet]
        [Route("backoffice/testimonials/viewtestimonials")]
        public async Task<IActionResult> viewtestimonials()
        {
            ViewBag.Menus = _menuService.GetMenu();

            var dt = await _homePageService.GetTestimonialsList();

            ViewBag.Testimonials = dt;


            return View("~/Views/backoffice/testimonials/viewtestimonials.cshtml");
        }
        [HttpGet]
        [Route("backoffice/testimonials/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {

            var obj = new clsTestimonial();
            ViewBag.Menus = _menuService.GetMenu();

            var x = await _homePageService.GetTestimonialsList();
            var bannerTypes = x.AsEnumerable().Where(r => r.Field<int>("testimonialid") == id).CopyToDataTable();
            if (bannerTypes != null && bannerTypes.Rows.Count > 0)
            {
                obj.testimonialid = Convert.ToString(bannerTypes.Rows[0]["testimonialid"]);
                obj.Name = Convert.ToString(bannerTypes.Rows[0]["testimonialname"]);
                obj.designation = (Convert.ToString(bannerTypes.Rows[0]["desg"]));
                obj.placed = (Convert.ToString(bannerTypes.Rows[0]["placed"]));
                obj.displayorder = Convert.ToString(bannerTypes.Rows[0]["displayorder"]);
                obj.Image = Convert.ToString(bannerTypes.Rows[0]["uploadphoto"]);
                obj.detail = WebUtility.HtmlDecode(Convert.ToString(bannerTypes.Rows[0]["detaildesc"]));
                obj.Description = WebUtility.HtmlDecode(Convert.ToString(bannerTypes.Rows[0]["testimonialdesc"]));
                obj.status = Convert.ToBoolean(bannerTypes.Rows[0]["status"]) ? true : false;
                obj.mode = "2"; // Set mode to update
                obj.uname = HttpContext.Session.GetString("UserName");

                ViewBag.CreateUpdate = "Update";
            }

            return View("~/Views/Backoffice/testimonials/addtestimonials.cshtml", obj);
        }
        [HttpGet]
        [Route("backoffice/testimonials/tStatus/{id}")]
        public async Task<IActionResult> tStatus(int id)
        {

            clsTestimonial objalbum = new clsTestimonial();
            var x = _homePageService.GetTestimonialsList();
            if (x != null)
            {
                var filterresults = x.Result.AsEnumerable().Where(r => r.Field<int>("testimonialid") == id);
                if (filterresults.Any())
                {
                    objalbum.status = filterresults.All(r => r.Field<bool>("status")) ? true : false;
                    // string status = objalbum.status ? "false" : "true";

                    int x1 = _homePageService.UpdateTestimonilsStatus(objalbum.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                        return RedirectToAction("viewtestimonials", "Backoffice");
                    }
                }
            }


            return RedirectToAction("viewtestimonials", "Backoffice");
        }
        [HttpGet]
        [Route("backoffice/testimonials/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = _homePageService.DeleteTestimonils(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Testimonials deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete Tvc. Please try again.");
            }


            return RedirectToAction("viewtestimonials", "Backoffice");
        }
        [HttpGet]
        [Route("autosearch")]
        public async Task<IActionResult> AutoSearch(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Content("");

            var dt = await _homePageService.SearchFormList(term);

            var html = "<ul style='list-style:none; margin:0; padding:5px;'>";
            foreach (DataRow row in dt.Rows)
            {
                var formId = row["FormId"];
                var formCaption = row["FormCaption"];
                var url = "/backoffice/" + row["formname"].ToString().Replace(".aspx", "");

                html += $@"
            <li style='padding:5px; border-bottom:1px solid #eee;'>
                <a href='{url}' class='suggestion-link' 
                   style='text-decoration:none; color:#333; display:block;'>
                    {formCaption}
                </a>
            </li>";
            }
            html += "</ul>";

            return Content(html, "text/html");
        }


    }

}
