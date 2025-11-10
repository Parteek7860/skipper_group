using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class MediaController : Controller
    {
        private readonly IHomePage _homePageService;
        private readonly clsMainMenuList _menuService;
        public MediaController(IHomePage homePageService, clsMainMenuList menuService)
        {
            _homePageService = homePageService;
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("backoffice/media/addeventstype")]
        public IActionResult addeventstype()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.CreateUpdate = "Save";

            // Get List of events types if needed
            var eventTypes = _homePageService.GetEventTypeList();
            if (eventTypes.Result != null && eventTypes.Result.Rows.Count > 0)
            {
                ViewBag.EventTypes = eventTypes.Result;
            }
            else
            {
                ViewBag.EventTypes = null;
            }

            return View("~/Views/backoffice/media/addeventstype.cshtml");
        }
        [HttpGet]
        [Route("backoffice/media/viewmedia_section")]
        public IActionResult viewmedia_section()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            // Get List of events types if needed
            var medialist = _homePageService.GetEventList();
            if (medialist.Result != null && medialist.Result.Rows.Count > 0)
            {
                ViewBag.medialist = medialist.Result;
            }
            else
            {
                ViewBag.medialist = null;
            }
            return View("~/Views/backoffice/media/viewmedia_section.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Media/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            clsMediatype obj1 = new clsMediatype();
            try
            {
                int x = _homePageService.DeleteMediaSection(id);
                if (x > 0)
                {
                    ViewBag.Success = "Events Delete successfully.";
                    TempData["Title"] = "Media Section";
                    return RedirectToAction("viewmedia_section", "Media");
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Backoffice/Media/viewmedia_section.cshtml", obj1);
        }
        [HttpGet]
        [Route("backoffice/Media/UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                HttpContext.Session.Remove("Message");
                clsBannerType objbannertype = new clsBannerType();
                // Get the Media list by ID
                var x = _homePageService.GetEventList();
                var filteredRows = x.Result.AsEnumerable().Where(row => Convert.ToInt32(row["eventsid"]) == id).ToList();

                if (filteredRows.Count > 0)
                {
                    objbannertype.status = Convert.ToString(Convert.ToInt32(filteredRows[0]["status"])) == "1" ? "True" : "False"; // Toggle status
                    int x1 = _homePageService.UpdateEventsStatus(objbannertype.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");
                        TempData["Title"] = "Media Section";
                        return RedirectToAction("viewmedia_section", "Media");
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("viewmedia_section", "Media");
        }
        [HttpGet]
        [Route("backoffice/Media/UpdateStatusShowHome/{id}")]
        public async Task<IActionResult> UpdateStatusShowHome(int id)
        {
            try
            {
                HttpContext.Session.Remove("Message");
                clsBannerType objbannertype = new clsBannerType();
                // Get the Media list by ID
                var x = _homePageService.GetEventList();
                var filteredRows = x.Result.AsEnumerable().Where(row => Convert.ToInt32(row["eventsid"]) == id).ToList();

                if (filteredRows.Count > 0)
                {
                    objbannertype.status = Convert.ToString(Convert.ToInt32(filteredRows[0]["showonhome"])) == "1" ? "True" : "False"; // Toggle status
                    int x1 = _homePageService.UpdateEventsStatusShowHome(objbannertype.status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");
                        TempData["Title"] = "Media Section";
                        return RedirectToAction("viewmedia_section", "Media");
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("viewmedia_section", "Media");
        }

        [HttpGet]
        [Route("backoffice/Media/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {
            try
            {
                HttpContext.Session.Remove("Message");
                clsMediatype objbannertype = new clsMediatype();
                var mediatypeList = _homePageService.GetEventTypeList();
                if (mediatypeList.Result != null && mediatypeList.Result.Rows.Count > 0)
                {
                    objbannertype.selectmediatype = mediatypeList.Result.AsEnumerable().Where(row => row["status"].ToString() == "True").Select(row => new SelectListItem
                    {
                        Value = row["ntypeid"].ToString(),
                        Text = row["ntype"].ToString()
                    }).ToList();
                }
                else
                {
                    objbannertype.selectmediatype = new List<SelectListItem>();
                }
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                // Get the Media list by ID
                var x = _homePageService.GetEventList();
                var filteredRows = x.Result.AsEnumerable().Where(row => Convert.ToInt32(row["eventsid"]) == id).ToList();

                if (filteredRows.Count > 0)
                {
                    objbannertype.status = Convert.ToBoolean(Convert.ToInt32(filteredRows[0]["status"]));
                    objbannertype.eventstitle = Convert.ToString(filteredRows[0]["eventstitle"]);
                    objbannertype.mediatype = Convert.ToString(filteredRows[0]["ntypeid"]);
                    objbannertype.eventsdate = Convert.ToDateTime(filteredRows[0]["eventsdate"]);
                    //objbannertype.tagline = Convert.ToString(filteredRows[0]["tagline"]);
                    objbannertype.shortdetail = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["shortdesc"]));
                    objbannertype.detail = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["eventsdesc"]));
                    objbannertype.displayorder = Convert.ToString(filteredRows[0]["displayorder"]);
                    objbannertype.url = Convert.ToString(filteredRows[0]["youtube_url"]);
                    objbannertype.pagetitle = Convert.ToString(filteredRows[0]["pagetitle"]);
                    objbannertype.metakeywords = Convert.ToString(filteredRows[0]["pagemeta"]);
                    objbannertype.metadesc = Convert.ToString(filteredRows[0]["pagemetadesc"]);
                    objbannertype.colorcode = Convert.ToString(filteredRows[0]["colorcode"]);

                    objbannertype.bannerimage =  Convert.ToString(filteredRows[0]["uploadevents"]);
                    objbannertype.uploadbanner = Convert.ToString(filteredRows[0]["uploadevents"]);

                    objbannertype.Largeimage = Convert.ToString(filteredRows[0]["largeimage"]);
                    objbannertype.uploadlargeimage = Convert.ToString(filteredRows[0]["largeimage"]);

                    objbannertype.id = Convert.ToInt16(filteredRows[0]["eventsid"]);

                    ViewBag.CreateUpdate = "Update";
                    return View("~/Views/backoffice/media/media_section.cshtml", objbannertype);
                }

            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + ex.Message.ToString());
            }
            return RedirectToAction("viewmedia_section", "Media");
        }
        [HttpGet]
        [Route("backoffice/media/media_section")]
        public IActionResult media_section()
        {
            //HttpContext.Session.Remove("Message");
            clsMediatype objMediatype = new clsMediatype();
            try
            {
                var mediatypeList = _homePageService.GetEventTypeList();
                if (mediatypeList.Result != null && mediatypeList.Result.Rows.Count > 0)
                {
                    objMediatype.selectmediatype = mediatypeList.Result.AsEnumerable().Where(row => row["status"].ToString() == "True").Select(row => new SelectListItem
                    {
                        Value = row["ntypeid"].ToString(),
                        Text = row["ntype"].ToString()
                    }).ToList();
                }
                else
                {
                    objMediatype.selectmediatype = new List<SelectListItem>();
                }


                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                ViewBag.CreateUpdate = "Save";
                objMediatype.eventsdate = Convert.ToDateTime(DateTime.Now);

                return View("~/Views/backoffice/media/media_section.cshtml", objMediatype);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/media/media_section.cshtml");
            }
            return View("~/Views/backoffice/media/media_section.cshtml", objMediatype);
        }
        [HttpPost]
        [Route("backoffice/Media/media_section")]
        public async Task<IActionResult> media_section(clsMediatype obj, IFormFile file_Uploader, IFormFile file_Uploader2)
        {
            HttpContext.Session.Remove("Message");
            clsMediatype objMedia = new clsMediatype();
            try
            {
                if (!string.IsNullOrEmpty(obj.eventstitle))
                {
                    objMedia.mediatype = obj.mediatype;
                    objMedia.eventstitle = obj.eventstitle;
                    objMedia.eventsdate = obj.eventsdate;
                    objMedia.tagline = obj.tagline;
                    objMedia.shortdetail = obj.shortdetail ?? string.Empty;
                    objMedia.detail = obj.detail ?? string.Empty;
                    objMedia.displayorder = obj.displayorder;
                    objMedia.url = obj.url;
                    objMedia.pagetitle = obj.pagetitle ?? string.Empty;
                    objMedia.metakeywords = obj.metakeywords ?? string.Empty;
                    objMedia.metadesc = obj.metadesc ?? string.Empty;
                    objMedia.canonical = obj.canonical ?? string.Empty;
                    objMedia.status = obj.status;
                    objMedia.uname = HttpContext.Session.GetString("UserName");
                    objMedia.colorcode = obj.colorcode ?? string.Empty;
                    if (file_Uploader != null && file_Uploader.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader.CopyTo(stream);
                        }

                        objMedia.uploadbanner = fileName;
                    }
                    else
                    {
                        objMedia.uploadbanner = obj.uploadbanner ?? string.Empty;
                    }
                    if (file_Uploader2 != null && file_Uploader2.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader2.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/LargeImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader2.CopyTo(stream);
                        }

                        objMedia.Largeimage = fileName;
                    }
                    else
                    {
                        objMedia.uploadlargeimage = obj.uploadlargeimage ?? string.Empty;
                    }
                    objMedia.id = obj.id;
                    if (obj.id == 0)
                    {
                        objMedia.mode = "1";
                        int x = _homePageService.CreateMedia(objMedia);
                        if (x > 0)
                        {
                            
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Media Section Save successfully.");
                            
                            return RedirectToAction("viewmedia_section", "Media");
                        }
                    }
                    else
                    {
                        // Update logic here if needed
                        objMedia.mode = "2";
                        int x = _homePageService.CreateMedia(objMedia);
                        if (x > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Media Section Update successfully.");
                            
                            return RedirectToAction("viewmedia_section", "Media");
                        }
                    }
                }
                else
                {
                    var mediatypeList = _homePageService.GetEventTypeList();
                    if (mediatypeList.Result != null && mediatypeList.Result.Rows.Count > 0)
                    {
                        objMedia.selectmediatype = mediatypeList.Result.AsEnumerable().Where(row => row["status"].ToString() == "True").Select(row => new SelectListItem
                        {
                            Value = row["ntypeid"].ToString(),
                            Text = row["ntype"].ToString()
                        }).ToList();
                    }
                    else
                    {
                        objMedia.selectmediatype = new List<SelectListItem>();
                    }
                    ViewBag.CreateUpdate = "Update";

                    var menuList = _menuService.GetMenu();
                    ViewBag.Menus = menuList;

                    return View("~/Views/backoffice/media/media_section.cshtml", objMedia);
                }

                return View("~/Views/backoffice/media/media_section.cshtml", objMedia);
            }
            catch (Exception ex)
            {
                
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + ex.Message.ToString());
            }
            return View("~/Views/backoffice/media/media_section.cshtml", objMedia);
        }


    }
}

