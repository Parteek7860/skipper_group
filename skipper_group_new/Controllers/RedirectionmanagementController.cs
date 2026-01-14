using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace skipper_group_new.Controllers
{
    public class RedirectionmanagementController : Controller
    {
        private readonly IBackofficePage _homePageService;
        private readonly clsMainMenuList _menuService;


        public RedirectionmanagementController(IBackofficePage homePageService, clsMainMenuList menuService)
        {
            _homePageService = homePageService;
            _menuService = menuService;

        }
        [HttpGet]
        [Route("/backoffice/redirectionmanagement/redirection")]
        public async Task<IActionResult> redirection()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var redirectionList = await _homePageService.GetRedirectionList();
            var orderedRows = redirectionList.AsEnumerable()
                                 .OrderByDescending(r => r.Field<int>("id"));
            ViewBag.RedirectionList = orderedRows.CopyToDataTable();

            ViewBag.SaveUpdate = "Save";

            return View("/Views/backoffice/redirectionmanagement/redirection.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/backoffice/redirectionmanagement/redirection")]
        public async Task<IActionResult> redirection(clsRedirection obj)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.SaveUpdate = "Save";

            if (!string.IsNullOrEmpty(obj.OldUrl) && !string.IsNullOrEmpty(obj.NewUrl))
            {
                clsRedirection cls = new clsRedirection();
                cls.NewUrl = obj.NewUrl;
                cls.OldUrl = obj.OldUrl;
                cls.status = true;
                cls.redirect_type = "301";
                var result = _homePageService.AddRedirection(cls);
                if(result > 0)
                {
                    HttpContext.Session.SetString("Message", "Redirection added successfully.");
                }
                return RedirectToAction("redirection");
            }
            else
            {
                return View("/Views/backoffice/redirectionmanagement/redirection.cshtml", obj);
            }


            return View("/Views/backoffice/redirectionmanagement/redirection.cshtml");
        }

        [HttpGet]
        [Route("/backoffice/redirectionmanagement/datastatus/{id}")]
        public async Task<IActionResult> datastatus(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            DataTable x = await _homePageService.GetRedirectionList();
            var filtered = x.AsEnumerable().Where(r => r.Field<int>("id") == id).FirstOrDefault();

            if (filtered != null)
            {
                bool currentStatus = filtered.Field<bool>("status");
                string newStatus = currentStatus == true ? "True" : "False";
                var result = _homePageService.UpdateRedirectionStatus(newStatus, id);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Redirection status successfully.");
                }

                return RedirectToAction("redirection");
            }


            ViewBag.SaveUpdate = "Save";

            return View("/Views/backoffice/redirectionmanagement/redirection.cshtml");
        }

        [HttpGet]
        [Route("/backoffice/redirectionmanagement/datadelete/{id}")]
        public async Task<IActionResult> datadelete(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            DataTable x = await _homePageService.GetRedirectionList();
            var filtered = x.AsEnumerable().Where(r => r.Field<int>("id") == id).FirstOrDefault();

            if (filtered != null)
            {

                var result = _homePageService.DeleteRedirectionRecords(id);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Redirection delete successfully.");
                }

                return RedirectToAction("redirection");
            }


            ViewBag.SaveUpdate = "Save";

            return View("/Views/backoffice/redirectionmanagement/redirection.cshtml");
        }

    }
}
