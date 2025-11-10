using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;

namespace skipper_group_new.Controllers
{
    public class OthersController : Controller
    {
        private readonly IManagement _management;
        private readonly clsMainMenuList _menuService;

        public OthersController(clsMainMenuList menuService, IManagement management)
        {
            _management = management;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("/backoffice/others/viewenquiry")]
        public async Task<IActionResult> viewenquiry()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var enquiry = await _management.GetEnquiry();
            var sortedList = enquiry.OrderByDescending(x => x.trdate).ToList();
            ViewBag.Enquiry = sortedList;
            return View("~/Views/backoffice/others/viewenquiry.cshtml");
        }

        [HttpGet]
        [Route("/backoffice/others/viewproductenquiry")]
        public async Task<IActionResult> viewproductenquiry()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var pEnquery = await _management.GetProductEnquiry();
            ViewBag.ProductEnquiry = pEnquery;
            return View("~/Views/backoffice/others/viewproductenquiry.cshtml");
        }


    }
}
