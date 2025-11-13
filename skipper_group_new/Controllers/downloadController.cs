using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;

namespace skipper_group_new.Controllers
{
    public class downloadController : Controller
    {
        private readonly IHomePage _homepage;
        private readonly clsMainMenuList _menuService;

        public downloadController(clsMainMenuList menuService, IHomePage homepage)
        {
            _homepage = homepage;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/backoffice/Imagepath/imagefilepath")]
        public async Task<IActionResult> imagefilepath()
        {
            var mennu = _menuService.GetMenu();
            ViewBag.Menus = mennu;

            // view list
            var list = await _homepage.GetImagePath();
            ViewBag.List = list;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Imagepath/imagefilepath.cshtml");
        }
        [HttpPost]
        [Route("/backoffice/Imagepath/imagefilepath")]
        public async Task<IActionResult> imagefilepath(clsDownload cls, IFormFile fileuploader)
        {
            var mennu = _menuService.GetMenu();
            ViewBag.Menus = mennu;

            clsDownload objcls = new clsDownload();

            objcls.id = cls.id;
            objcls.Filetitle = cls.Filetitle;
            objcls.FilePath = cls.FilePath;
            objcls.displayorder = cls.displayorder ?? string.Empty;
            objcls.uname = HttpContext.Session.GetString("username") ?? "admin";

            if (fileuploader != null && fileuploader.Length > 0)
            {
                var fileName = Path.GetFileName(fileuploader.FileName); // captures name
                var filePath = Path.Combine("wwwroot/uploads/moduleimg", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileuploader.CopyTo(stream);
                }

                objcls.Filetitle = "/Uploads/moduleimg/" + fileName;
                objcls.FilePath = fileName;
            }
            int x = _homepage.CreateFilePathImage(objcls);
            if (x > 0)
            {
                if (cls.id != "0")
                {
                    HttpContext.Session.SetString("Message", "File Update successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "File Created successfully.");
                }
            }
            var list = await _homepage.GetImagePath();
            ViewBag.List = list;
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Imagepath/imagefilepath.cshtml", objcls);
        }
        [HttpGet]
        [Route("download/downloadfile/{id}")]
        public IActionResult DownloadFile(string id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", "moduleimg", id);


            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "application/octet-stream";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, id);
        }

        [HttpGet]
        [Route("/backoffice/download/uploadfile")]
        public IActionResult uploadfile()
        {
            var mennu = _menuService.GetMenu();
            ViewBag.Menus = mennu;
            return View("~/Views/backoffice/download/uploadfile.cshtml");
        }
        [HttpGet]
        [Route("/backoffice/download/viewuploadfile")]
        public IActionResult viewuploadfile()
        {
            var mennu = _menuService.GetMenu();
            ViewBag.Menus = mennu;
            return View("~/Views/backoffice/download/viewuploadfile.cshtml");
        }
    }
}
