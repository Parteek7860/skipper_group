
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace skipper_group_new.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IManagement _management;
        private readonly clsMainMenuList _menuService;

        public ReviewController(clsMainMenuList menuService, IManagement management)
        {
            _management = management;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("/backoffice/review/addreview")]
        public IActionResult addreview()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Review/addreview.cshtml");
        }
        [HttpPost]
        [Route("/backoffice/review/addreview")]
        public IActionResult addreview(clsReview cls)
        {
            clsReview obj = new clsReview();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            obj.Id = cls.Id;
            obj.username = cls.username;
            obj.emailid = cls.emailid;
            obj.mobileno = cls.mobileno;
            obj.status = cls.status;
            obj.Rating = cls.Rating;
            obj.title = cls.title;
            obj.reviewdesc = cls.reviewdesc;
            if (cls.Id > 0)
            {
                obj.mode = "2";
            }
            else
            {
                obj.mode = "1";
                obj.status = true;
            }
            var x = _management.AddReview(obj);
            if (x > 0)
            {
                if (cls.Id > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Review update successfully.");
                    return RedirectToAction("viewreview", "review");
                }
                else
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Review saved successfully.");
                    return RedirectToAction("addreview", "review");
                }
            }
          
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Review/addreview.cshtml");
        }
        [HttpGet]
        [Route("/backoffice/review/viewreview")]
        public IActionResult viewreview()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = _management.GetReviewData();
            ViewBag.ReviewData = x.Result;

            return View("~/Views/backoffice/Review/viewreview.cshtml");
        }

        [HttpPost]
        [Route("/backoffice/review/ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = await _management.GetReviewData();

            var review = x.FirstOrDefault(r => r.Id == id);

            if (review != null)
            {
                // Toggle the status
                review.status = !review.status;
                string status = review.status ? "True" : "False";

                var y = _management.UpdateReviewStatus(status, id);
                if (y > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");
                    return RedirectToAction("viewreview", "review");
                }

            }

            return View("~/Views/backoffice/Review/viewreview.cshtml");

        }
        [HttpPost]
        [Route("/backoffice/review/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = _management.GetReviewData();


            return RedirectToAction("viewreview", "review");
        }
        [HttpGet]
        [Route("/backoffice/review/Reviewedit/{id}")]
        public async Task<IActionResult> Reviewedit(int id)
        {
            clsReview obj = new clsReview();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = await _management.GetReviewData();

            var review = x.FirstOrDefault(r => r.Id == id);

            if (review != null)
            {
                // Toggle the status
                obj.status = !review.status;
                obj.Rating = review.Rating;
                obj.reviewdesc = review.reviewdesc;
                obj.Id = review.Id;
                obj.username = review.username;
                obj.emailid = review.emailid;
                obj.mobileno = review.mobileno;
                ViewBag.CreateUpdate = "Update";
                return View("~/Views/Backoffice/review/addreview.cshtml", obj);



            }


            return View("~/Views/Backoffice/review/addreview.cshtml", obj);
        }
    }
}
