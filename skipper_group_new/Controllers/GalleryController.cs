using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.Data;
using System.Reflection;

namespace skipper_group_new.Controllers
{
    public class GalleryController : Controller
    {
        private readonly clsMainMenuList _menuService;
        private readonly IBackofficePage _backofficeService;

        public GalleryController(IBackofficePage backofficeService, clsMainMenuList menuService)
        {
            _backofficeService = backofficeService;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("backoffice/gallery/addalbum")]
        public async Task<IActionResult> AddAlbum()
        {

            var objAlbumType = new clsGallery();

            // Await the async method to get actual DataTable
            DataTable content = await _backofficeService.GetAlbumTypeList();

            if (content != null && content.Rows.Count > 0)
            {
                objAlbumType.selectalbumtype = content.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["typeid"]?.ToString() ?? string.Empty,
                        Text = row["typename"]?.ToString() ?? string.Empty
                    })
                    .ToList();
            }
            else
            {
                objAlbumType.selectalbumtype = new List<SelectListItem>();
            }


            ViewBag.Menus = _menuService.GetMenu();
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/gallery/addalbum.cshtml", objAlbumType);
        }
        [HttpPost]
        [Route("backoffice/gallery/addalbum")]
        public async Task<IActionResult> addAlbum(clsGallery objgallery, IFormFile file_Uploader)
        {
            HttpContext.Session.Remove("Message");
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();
            ModelState.Remove("pagetitle");
            ModelState.Remove("metakeywords");
            ModelState.Remove("metadesc");
            ModelState.Remove("canonical");
            ModelState.Remove("searchname");
            ModelState.Remove("startdate");
            ModelState.Remove("enddate");
            ModelState.Remove("uploadlargeimage");
            ModelState.Remove("bannerimage");
            ModelState.Remove("largeimage");
            ModelState.Remove("uploadbanner");
            ModelState.Remove("selectalbumtype");
            ModelState.Remove("shortdetail");
            ModelState.Remove("albumdesc");
            ModelState.Remove("mode");
            ModelState.Remove("uname");
            ModelState.Remove("status");
            ModelState.Remove("URL");
            ModelState.Remove("displayorder");

            if (ModelState.IsValid)
            {
                try
                {
                    objgallery.title = objgallery.title;
                    objgallery.albumtype = objgallery.albumtype;
                    objgallery.shortdetail = objgallery.shortdetail;
                    if (objgallery.id == 0)
                    {
                        objgallery.mode = "1";
                    }
                    else
                    {
                        objgallery.mode = "2";
                        objgallery.id = objgallery.id;
                    }
                    if (file_Uploader != null && file_Uploader.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader.CopyTo(stream);
                        }

                        objgallery.uploadbanner = fileName;
                    }
                    objgallery.eventsdate = objgallery.eventsdate ?? DateTime.Now;
                    objgallery.displayorder = objgallery.displayorder ?? "0";
                    objgallery.bannerimage = objgallery.bannerimage ?? string.Empty;
                    objgallery.uname = HttpContext.Session.GetString("UserName");
                    objgallery.status = true;
                    objgallery.pagetitle = objgallery.pagetitle ?? string.Empty;
                    objgallery.metakeywords = objgallery.metakeywords ?? string.Empty;
                    objgallery.metadesc = objgallery.metadesc ?? string.Empty;
                    objgallery.albumdesc = objgallery.shortdetail ?? string.Empty;
                    objgallery.canonical = objgallery.canonical ?? string.Empty;
                    int result = _backofficeService.AddAlbum(objgallery);
                    if (result > 0)
                    {
                        if (objgallery.id == 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Album Added successfully.");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Album Updated successfully.");
                        }

                        TempData["Title"] = "Gallery Album";
                        return RedirectToAction("AddAlbum", "Gallery");
                    }
                    else
                    {
                        ViewBag.Error = "Failed to add album. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Please fill in all required fields.");
                return RedirectToAction("AddAlbum", "Gallery");
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/gallery/addalbum.cshtml", objAlbumType);
        }
        [HttpGet]
        [Route("backoffice/gallery/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {

            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();

            DataTable content = await _backofficeService.GetAlbumTypeList();

            if (content != null && content.Rows.Count > 0)
            {
                objAlbumType.selectalbumtype = content.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["typeid"]?.ToString() ?? string.Empty,
                        Text = row["typename"]?.ToString() ?? string.Empty
                    })
                    .ToList();
            }
            else
            {
                objAlbumType.selectalbumtype = new List<SelectListItem>();
            }

            //Get list of banner types
            var bannerTypes = await _backofficeService.GetAlbumListByID(id);
            if (bannerTypes != null && bannerTypes.Rows.Count > 0)
            {
                objAlbumType.id = Convert.ToInt32(bannerTypes.Rows[0]["albumid"]);
                objAlbumType.title = Convert.ToString(bannerTypes.Rows[0]["albumtitle"]);
                objAlbumType.albumtype = Convert.ToInt16(bannerTypes.Rows[0]["typeid"]);
                objAlbumType.shortdetail = Convert.ToString(bannerTypes.Rows[0]["albumdesc"]);
                //objAlbumType.eventsdate = Convert.ToDateTime(bannerTypes.Rows[0]["albumdate"]);
                objAlbumType.displayorder = Convert.ToString(bannerTypes.Rows[0]["displayorder"]);
                objAlbumType.uploadbanner = Convert.ToString(bannerTypes.Rows[0]["uploadaimage"]);
                objAlbumType.bannerimage = "/uploads/smallimages/" + Convert.ToString(bannerTypes.Rows[0]["uploadaimage"]);
                objAlbumType.uploadbanner = Convert.ToString(bannerTypes.Rows[0]["uploadaimage"]);
                objAlbumType.pagetitle = Convert.ToString(bannerTypes.Rows[0]["pagetitle"]);
                objAlbumType.metakeywords = Convert.ToString(bannerTypes.Rows[0]["pagemeta"]);
                objAlbumType.metadesc = Convert.ToString(bannerTypes.Rows[0]["pagemetadesc"]);
                //objAlbumType.pagescript = Convert.ToString(bannerTypes.Rows[0]["pagescript"]);
                objAlbumType.canonical = Convert.ToString(bannerTypes.Rows[0]["canonical"]);
                objAlbumType.status = Convert.ToBoolean(bannerTypes.Rows[0]["status"]) ? true : false;
                objAlbumType.mode = "2"; // Set mode to update
                objAlbumType.uname = HttpContext.Session.GetString("UserName");

                ViewBag.CreateUpdate = "Update";
            }

            return View("~/Views/Backoffice/gallery/addalbum.cshtml", objAlbumType);
        }
        [HttpGet]
        [Route("backoffice/gallery/viewalbum")]
        public async Task<IActionResult> viewalbum()
        {

            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();

            //Get list of banner types
            var bannerTypes = _backofficeService.GetAlbumList();
            ViewBag.medialist = bannerTypes.Result;

            return View("~/Views/Backoffice/gallery/viewalbum.cshtml", objAlbumType);
        }
        [HttpGet]
        [Route("backoffice/gallery/PageStatus/{id}")]
        public async Task<IActionResult> PageStatus(int id)
        {
            HttpContext.Session.Remove("Message");
            clsGallery objalbum = new clsGallery();
            var x = _backofficeService.GetAlbumListByID(id);
            if (x.Result != null && x.Result.Rows.Count > 0)
            {
                objalbum.status = Convert.ToBoolean(x.Result.Rows[0]["status"]) == true ? true : false; // Toggle status

                string status = objalbum.status ? "False" : "True"; // If true, set to 0 (inactive), if false, set to 1 (active)

                int x1 = _backofficeService.UpdateAlbumStatus(status, id);
                if (x1 > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                    return RedirectToAction("viewalbum", "Gallery");
                }
            }

            return RedirectToAction("viewalbum", "Gallery");
        }
        [HttpGet]
        [Route("backoffice/gallery/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = _backofficeService.DeleteAlbum(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Album deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete album. Please try again.");
            }


            return RedirectToAction("viewalbum", "Gallery");
        }
        [HttpGet]
        [Route("backoffice/gallery/addvedio")]
        public async Task<IActionResult> Addvedio()
        {
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();

            var content = await _backofficeService.GetAlbumTypeListByID();
            if (content != null)
            {
                ViewBag.AlbumTypeList = content.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row["albumid"].ToString(),    // column name
                    Text = row["albumtitle"].ToString()   // column name
                }).ToList();

                objAlbumType.selectalbumtype = ViewBag.AlbumTypeList;

            }
            else
            {
                objAlbumType.selectalbumtype = new List<SelectListItem>();
            }
            ViewBag.CreateUpdate = "Save";
            //objAlbumType.albumtype=

            return View("~/Views/backoffice/gallery/addvedio.cshtml", objAlbumType);

        }
        [HttpPost]
        [Route("backoffice/gallery/addvedio")]
        public async Task<IActionResult> addvedio(clsGallery cls, IFormFile file_Uploader)
        {
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();


            objAlbumType.title = cls.title;
            objAlbumType.displayorder = cls.displayorder ?? "0";
            objAlbumType.URL = cls.URL;
            objAlbumType.uname = HttpContext.Session.GetString("UserName");
            if (cls.id > 0)
            {
                objAlbumType.mode = "2";
                objAlbumType.id = cls.id;
            }
            else
            {
                objAlbumType.mode = "1";
            }
            if (file_Uploader != null && file_Uploader.Length > 0)
            {
                var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                var filePath = Path.Combine("wwwroot/uploads/vedio", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file_Uploader.CopyTo(stream);
                }

                objAlbumType.uploadbanner = fileName;
            }
            else
            {
                objAlbumType.uploadbanner = cls.uploadbanner;
            }
            objAlbumType.status = cls.status;
            int x = await _backofficeService.AddVedio(objAlbumType);
            if (x > 0)
            {
                if (cls.id > 0)
                {
                    HttpContext.Session.SetString("Message", "Vedio Update successfully.");
                    return RedirectToAction("viewvedio", "Gallery");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Vedio Saved successfully.");
                    return RedirectToAction("addvedio", "Gallery");
                }

            }


            ViewBag.CreateUpdate = "Save";
            //objAlbumType.albumtype=

            return View("~/Views/backoffice/gallery/addvedio.cshtml", objAlbumType);

        }
        [HttpGet]
        [Route("backoffice/gallery/viewvedio")]
        public async Task<IActionResult> viewvedio()
        {
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();

            var content = await _backofficeService.BindVedioList();

            if (content != null)
            {
                ViewBag.VedioList = content;
            }

            return View("~/Views/backoffice/gallery/viewvedio.cshtml", objAlbumType);
        }
        [HttpGet]
        [Route("backoffice/gallery/VedioPageStatus/{id}")]
        public async Task<IActionResult> VedioPageStatus(int id)
        {
            HttpContext.Session.Remove("Message");
            clsGallery objalbum = new clsGallery();
            var x = _backofficeService.BindVedioList();
            if (x != null)
            {
                x.Result.DefaultView.RowFilter = "vedioid=" + id;

                DataTable filteredTable = x.Result.DefaultView.ToTable();

                if (filteredTable.Rows.Count > 0)
                {
                    objalbum.status = Convert.ToBoolean(filteredTable.Rows[0]["status"]);
                    string status = objalbum.status ? "False" : "True";

                    int x1 = _backofficeService.UpdateVedioStatus(status, id);
                    if (x1 > 0)
                    {
                        HttpContext.Session.SetString("Message", "Status updated successfully!");
                        HttpContext.Session.SetString("MessageType", "success");

                        return RedirectToAction("viewvedio", "Gallery");
                    }
                }
            }
            return RedirectToAction("viewvedio", "Gallery");
        }
        [HttpGet]
        [Route("backoffice/gallery/VedioDelete/{id}")]
        public async Task<IActionResult> VedioDelete(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = _backofficeService.DeleteVedio(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Album deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete album. Please try again.");
            }


            return RedirectToAction("viewvedio", "Gallery");
        }

        [HttpGet]
        [Route("backoffice/gallery/vedioedit/{id}")]
        public async Task<IActionResult> vedioedit(int id)
        {

            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();

            
            var x = await _backofficeService.BindVedioList();
            var bannerTypes = x.AsEnumerable().Where(r => r.Field<int>("vedioid") == id).CopyToDataTable();
            if (bannerTypes != null && bannerTypes.Rows.Count > 0)
            {
                objAlbumType.id = Convert.ToInt32(bannerTypes.Rows[0]["vedioid"]);
                objAlbumType.title = Convert.ToString(bannerTypes.Rows[0]["vediotitle"]);                
                objAlbumType.displayorder = Convert.ToString(bannerTypes.Rows[0]["displayorder"]);
                objAlbumType.URL = Convert.ToString(bannerTypes.Rows[0]["uploadvedio"]);
                objAlbumType.bannerimage =  Convert.ToString(bannerTypes.Rows[0]["thumbnailimage"]);
                objAlbumType.uploadbanner = Convert.ToString(bannerTypes.Rows[0]["thumbnailimage"]);              
                objAlbumType.status = Convert.ToBoolean(bannerTypes.Rows[0]["status"]) ? true : false;
                objAlbumType.mode = "2"; // Set mode to update
                objAlbumType.uname = HttpContext.Session.GetString("UserName");

                ViewBag.CreateUpdate = "Update";
            }

            return View("~/Views/Backoffice/gallery/addvedio.cshtml", objAlbumType);
        }
        //Rakesh Chauhan 11/12/2025
        [HttpGet]
        [Route("backoffice/gallery/addphotogallery")]
        public async Task<IActionResult> PhotoGallary()
        {
            var objAlbumType = new clsGallery();
            DataTable content = await _backofficeService.GetAlbumList();

            if (content != null && content.Rows.Count > 0)
            {
                objAlbumType.selectalbumtype = content.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Albumid"]?.ToString() ?? string.Empty,
                        Text = row["Albumtitle"]?.ToString() ?? string.Empty
                    })
                    .ToList();
            }
            else
            {
                objAlbumType.selectalbumtype = new List<SelectListItem>();
            }
            ViewBag.Menus = _menuService.GetMenu();
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/gallery/addphotogallery.cshtml", objAlbumType);
        }

        [HttpPost]
        [Route("backoffice/gallery/addPhoto")]
        public async Task<IActionResult> addPhoto(clsGallery objgallery, IFormFile? file_Uploader)
        {
            HttpContext.Session.Remove("Message");
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();
            ModelState.Remove("eventsdate");
            ModelState.Remove("pagetitle");
            ModelState.Remove("metakeywords");
            ModelState.Remove("metadesc");
            ModelState.Remove("canonical");
            ModelState.Remove("shortdetail");
            ModelState.Remove("bannerimage");
            ModelState.Remove("largeimage");
            ModelState.Remove("URL");
            ModelState.Remove("albumdesc");
            ModelState.Remove("searchname");
            ModelState.Remove("startdate");
            ModelState.Remove("enddate");
            ModelState.Remove("tagline");
            ModelState.Remove("selectalbumtype");
            ModelState.Remove("uploadlargeimage");

            if (ModelState.IsValid)
            {
                try
                {
                    objgallery.albumtype = objgallery.albumtype;
                    objgallery.title = objgallery.title;
                    objgallery.displayorder = objgallery.displayorder ?? "0";
                    objgallery.uname = HttpContext.Session.GetString("UserName");
                    objgallery.status = true;
                    if (objgallery.id == 0)
                    {
                        objgallery.mode = "1";
                    }
                    else
                    {
                        objgallery.mode = "2";
                        objgallery.id = objgallery.id;
                    }
                    if (file_Uploader != null && file_Uploader.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader.FileName);
                        var filePath = Path.Combine("wwwroot/uploads/LargeImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader.CopyTo(stream);
                        }

                        objgallery.uploadbanner = fileName;
                    }
                    else
                    {
                        objgallery.uploadbanner = objgallery.uploadbanner;
                    }
                    int result = await _backofficeService.AddAlbumPhoto(objgallery);
                    if (result > 0)
                    {
                        if (objgallery.id == 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Photo gallary Added successfully.");
                            TempData["Title"] = "Gallery Photo";
                            return RedirectToAction("PhotoGallary", "Gallery");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Photo gallary Updated successfully.");
                            TempData["Title"] = "Gallery Photo";
                            return RedirectToAction("viewphotogallery", "Gallery");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Failed to add photo. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Please fill in all required fields.");
                return RedirectToAction("PhotoGallary", "Gallery");
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/gallery/addphotogallery.cshtml", objAlbumType);
        }

        [HttpGet]
        [Route("backoffice/gallery/editPhotoGallary/{id}")]
        public async Task<IActionResult> editPhotoGallary(int id)
        {

            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();
            int mode = 5;
            var x = await _backofficeService.BindPhotoGallaryList(mode);
            var content = await _backofficeService.GetAlbumList();
            var bannerTypes = x.AsEnumerable().Where(r => r.Field<int>("photoid") == id).CopyToDataTable();
            if (bannerTypes != null && bannerTypes.Rows.Count > 0)
            {
                objAlbumType.id = Convert.ToInt32(bannerTypes.Rows[0]["photoid"]);
                objAlbumType.title = Convert.ToString(bannerTypes.Rows[0]["phototitle"]);
                objAlbumType.displayorder = Convert.ToString(bannerTypes.Rows[0]["displayorder"]);
                objAlbumType.uploadbanner = Convert.ToString(bannerTypes.Rows[0]["uploadphoto"]);
                objAlbumType.mode = "2";
                objAlbumType.uname = HttpContext.Session.GetString("UserName");
                var selectedAlbumType = Convert.ToInt32(bannerTypes.Rows[0]["albumid"]);
                objAlbumType.albumtype = selectedAlbumType;
                objAlbumType.selectalbumtype = content.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["Albumid"].ToString(),
                        Text = row["Albumtitle"].ToString(),
                        Selected = (row["Albumid"].ToString() == selectedAlbumType.ToString())
                    })
                    .ToList();
                ViewBag.CreateUpdate = "Update";
            }
            return View("~/Views/backoffice/gallery/addphotogallery.cshtml", objAlbumType);
        }

        [HttpGet]
        [Route("backoffice/gallery/viewphotogallery")]
        public async Task<IActionResult> viewphotogallery()
        {
            var objAlbumType = new clsGallery();
            ViewBag.Menus = _menuService.GetMenu();
            int mode = 6;
            var content = await _backofficeService.BindPhotoGallaryList(mode);
            if (content != null)
            {
                ViewBag.GalPhotoList = content;
            }
            return View("~/Views/backoffice/gallery/viewphotogallery.cshtml", objAlbumType);
        }

        [HttpGet]
        [Route("/backoffice/gallery/deletePhotoGallary/{id}")]
        public async Task<IActionResult> deletephotogallery(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = await _backofficeService.DeletePhotoGallary(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Photo gallery deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete photo gallery. Please try again.");
            }
            return RedirectToAction("viewphotogallery", "Gallery");
        }

        [HttpGet]
        [Route("/backoffice/gallery/chngStatus/{id}")]
        public async Task<IActionResult> changestatus(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = await _backofficeService.changestatus(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status changed successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to change status. Please try again.");
            }
            return RedirectToAction("viewphotogallery", "Gallery");
        }

    }
}
