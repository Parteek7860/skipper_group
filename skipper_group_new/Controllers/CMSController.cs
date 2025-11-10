using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class CMSController : Controller
    {
        private readonly clsMainMenuList _menuService;
        private readonly IBackofficePage _backofficeService;

        public CMSController(IBackofficePage backofficeService, clsMainMenuList menuService)
        {
            _backofficeService = backofficeService;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("backoffice/cms/addpages")]
        public IActionResult addPages()
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();

            obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<int>("pageid").ToString(),
                    Text = row.Field<string>("linkname")
                }).ToList();

            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Top-Links", Value = "Top-Links" });
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger menu", Value = "Hamburger menu" });
            names.Add(new SelectListItem { Text = "Left Side Menu", Value = "Left Side Menu" });
            names.Add(new SelectListItem { Text = "Right Side Menu", Value = "Right Side Menu" });
            names.Add(new SelectListItem { Text = "Mobile", Value = "Mobile" });
            obj.linkposition = names;
            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/cms/addpages.cshtml", obj);
        }
        [HttpPost]
        [Route("backoffice/cms/addpages")]
        public IActionResult addpages(clsCMS cls, IFormCollection frm)
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();

            obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<int>("pageid").ToString(),
                    Text = row.Field<string>("linkname")
                }).ToList();

            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Top-Links", Value = "Top-Links" });
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger menu", Value = "Hamburger menu" });
            names.Add(new SelectListItem { Text = "Left Side Menu", Value = "Left Side Menu" });
            names.Add(new SelectListItem { Text = "Right Side Menu", Value = "Right Side Menu" });
            names.Add(new SelectListItem { Text = "Mobile", Value = "Mobile" });
            obj.linkposition = names;
            ViewBag.CreateUpdate = "Save";

            if (string.IsNullOrEmpty(cls.linkname))
            {
                ViewBag.Message = "Page Name is required.";
                return View("~/Views/backoffice/cms/addpages.cshtml", obj);
            }
            if (cls.Id > 0)
            {
                obj.Id = cls.Id;
                obj.mode = 2;
            }
            else
            {
                obj.mode = 1;
            }
            obj.pagename = cls.pagename;
            obj.linkname = cls.linkname;
            obj.pageurl = cls.pageurl;
            obj.parentid = cls.parentid;
            obj.smalldesc = cls.smalldesc;
            obj.tagline1 = cls.tagline1;
            obj.pageposition = frm["skill"];
            obj.smalldesc = cls.smalldesc;
            obj.pagedesc = cls.pagedesc;
            obj.pagetitle = cls.pagetitle;
            obj.metakeywords = cls.metakeywords;
            obj.megamenu = cls.megamenu;
            obj.metadesc = cls.metadesc;
            obj.canonical = cls.canonical;
            obj.rewriteurl = cls.rewriteurl;
            obj.controllername=cls.controllername;
            obj.actionname=cls.actionname;
            obj.displayorder= cls.displayorder;
            int x = _backofficeService.AddCMS(obj);
            if (x > 0)
            {
                if (cls.Id > 0)
                {
                    HttpContext.Session.SetString("Message", "Page Update successfully.");
                    return RedirectToAction("viewpages", "cms");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Page Save successfully.");
                    return RedirectToAction("addpagesk", "Gallery");
                }
            }
            else
            {
                ViewBag.Message = "Error while saving record.";
            }
            return View("~/Views/backoffice/cms/addpages.cshtml", obj);
        }
        [HttpGet]
        [Route("backoffice/cms/viewpages")]
        public IActionResult viewpages()
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();
            var content = _backofficeService.BindPageList().Result;
            var pageList = BuildHierarchy(content, 0, 0);

            return View("~/Views/backoffice/cms/viewpages.cshtml", pageList);
        }
        private List<clsCMS> BuildHierarchy(DataTable table, int parentId, int level)
        {
            var list = new List<clsCMS>();
            var rows = table.Select("ParentId=" + parentId, "displayorder ASC");

            foreach (var row in rows)
            {
                var page = new clsCMS
                {
                    Id = Convert.ToInt32(row["pageid"]),
                    pagename = Convert.ToString(row["pagename"]),
                    linkname = Convert.ToString(row["linkname"]),
                    pageposition = Convert.ToString(row["linkposition"]),
                    displayorder = Convert.ToInt32(row["displayorder"]),
                    PageStatus = Convert.ToBoolean(row["pagestatus"]),
                    Level = level
                };

                list.Add(page);

                // Add child pages recursively
                list.AddRange(BuildHierarchy(table, page.Id, level + 1));
            }

            return list;
        }
        [HttpGet]
        [Route("backoffice/cms/editstatus/{id}")]
        public IActionResult editstatus(int id)
        {
            HttpContext.Session.Remove("Message");
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();

            var cms = _backofficeService.BindPageList();

            var filterResults = cms.Exception == null
       ? cms.Result.AsEnumerable().Where(r => r.Field<int>("pageid") == id)
       : Enumerable.Empty<DataRow>();

            var row = filterResults.FirstOrDefault();
            if (row != null)
            {
                obj.pagename = Convert.ToInt32(row["pageStatus"]) == 1 ? "True" : "False";
                int x1 = _backofficeService.CMSUpdateStatus(obj.pagename, id);
                if (x1 > 0)
                {
                    HttpContext.Session.SetString(
                        "Message",
                        (HttpContext.Session.GetString("Message") ?? "") + "Status Update successfully."
                    );
                    return RedirectToAction("viewpages", "CMS");
                }
            }

            return View("~/Views/backoffice/cms/viewpages.cshtml");
        }
        [HttpGet]
        [Route("backoffice/cms/edit/{id}")]
        public IActionResult edit(int id)
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();
            obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<int>("pageid").ToString(),
                    Text = row.Field<string>("linkname")
                }).ToList();
            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Top-Links", Value = "Top-Links" });
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger menu", Value = "Hamburger menu" });
            names.Add(new SelectListItem { Text = "Left Side Menu", Value = "Left Side Menu" });
            names.Add(new SelectListItem { Text = "Right Side Menu", Value = "Right Side Menu" });
            names.Add(new SelectListItem { Text = "Mobile", Value = "Mobile" });
            obj.linkposition = names;
            ViewBag.UpdateStatus = "Update";
            var dt = _backofficeService.GetPageListByID(id).Result;
            if (dt != null && dt.Rows.Count > 0)
            {
                obj.Id = Convert.ToInt32(dt.Rows[0]["pageid"]);
                obj.pagename = dt.Rows[0]["pagename"].ToString();
                obj.linkname = dt.Rows[0]["linkname"].ToString();
                obj.pageurl = dt.Rows[0]["pageurl"].ToString();
                obj.parentid = Convert.ToInt32(dt.Rows[0]["parentid"]);
                obj.pageposition = dt.Rows[0]["linkposition"].ToString();
                obj.outputparm = (dt.Rows[0]["linkposition"]).ToString().Split(',').ToList();
                obj.megamenu = dt.Rows[0]["megamenu"].ToString();
                obj.smalldesc = WebUtility.HtmlDecode(dt.Rows[0]["smalldesc"].ToString());
                obj.pagedesc = WebUtility.HtmlDecode(dt.Rows[0]["pagedescription"].ToString());
                obj.tagline1 = dt.Rows[0]["tagline"].ToString();
                obj.displayorder = Convert.ToInt32(dt.Rows[0]["displayorder"].ToString());
                obj.pagetitle = dt.Rows[0]["pagetitle"].ToString();
                obj.metakeywords = dt.Rows[0]["pagemeta"].ToString();
                obj.metadesc = dt.Rows[0]["pagemetadesc"].ToString();
                obj.canonical = dt.Rows[0]["canonical"].ToString();
                obj.rewriteurl = dt.Rows[0]["rewriteurl"].ToString();
                obj.controllername = dt.Rows[0]["controller"].ToString();
                obj.actionname = dt.Rows[0]["action"].ToString();

                foreach (SelectListItem item in obj.linkposition)
                {
                    foreach (string str in obj.outputparm)
                    {
                        if (item.Text == str)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            return View("~/Views/backoffice/cms/addpages.cshtml", obj);
        }

        [HttpGet]
        [Route("backoffice/cms/delete/{id}")]
        public IActionResult delete(int id)
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();

            var content = _backofficeService.DeleteRecords(id);
            if (content != null)
            {
                TempData["Title"] = "Data Delete Successful";
            }
            return View("~/Views/backoffice/cms/viewpages.cshtml");
        }

        // [HttpPost]
        // [Route("backoffice/Products/ExportProductToExcel")]
        // public async Task<IActionResult> ExportProductToExcel()
        // {
        //var catdtl = await _products.GetProductTblData();
        //using (var workbook = new ClosedXML.Excel.XLWorkbook())
        //{
        //    var worksheet = workbook.Worksheets.Add("Products");
        //    worksheet.Cell(1, 1).Value = "Product";
        //    worksheet.Cell(1, 2).Value = "Title";
        //    worksheet.Cell(1, 3).Value = "Model Number";
        //    worksheet.Cell(1, 4).Value = "Date";
        //    worksheet.Cell(1, 5).Value = "Status";
        //    int row = 2;
        //    foreach (var c in catdtl)
        //    {
        //        worksheet.Cell(row, 1).Value = c.ProductName;
        //        worksheet.Cell(row, 2).Value = c.ProductTitle;
        //        worksheet.Cell(row, 3).Value = c.ModelNo;
        //        worksheet.Cell(row, 4).Style.DateFormat.Format = "yyyy-MM-dd";
        //        worksheet.Cell(row, 5).Value = c.Status ? "Active" : "Inactive";
        //        row++;
        //    }

        //    using (var stream = new MemoryStream())
        //    {
        //        workbook.SaveAs(stream);
        //        stream.Position = 0;
        //        string fileName = $"ProductList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //    }
        //}
        //}
    }
}
