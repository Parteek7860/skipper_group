using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;

namespace skipper_group_new.Controllers
{
    public class downloadController : Controller
    {
        private readonly IBlog _blog;
        private readonly clsMainMenuList _menuService;

        public downloadController(clsMainMenuList menuService, IBlog blog)
        {
            _blog = blog;
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
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Imagepath/imagefilepath.cshtml", new clsDownload());
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
