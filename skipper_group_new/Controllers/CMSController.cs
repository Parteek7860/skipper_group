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
        [Route("backoffice/cms/addpages/{name?}/{pageid?}")]
        public IActionResult addPages(string name, int pageid)
        {
            var obj = new clsCMS();

            var menuList = _menuService.GetMenu(pageid);
            ViewBag.Menus = menuList;

            if (pageid > 0)
            {
                var content = _backofficeService.GetPageList().Result;
                var pageList = BuildHierarchy(content, 0, 0, pageid);
                obj.selectparent = pageList.AsEnumerable()
            .Select(row => new SelectListItem
            {
                Value = row.Id.ToString(),
                Text = WebUtility.HtmlDecode(row.DisplayName).ToString()
            }).ToList();
            }
            else
            {
                var content = _backofficeService.BindPageList().Result;
                var pageList = BuildHierarchy(content, 0, 0, pageid);
                obj.selectparent = pageList.AsEnumerable()
             .Where(x => x.collageid == 0)
            .Select(row => new SelectListItem
            {
                Value = row.Id.ToString(),
                Text = WebUtility.HtmlDecode(row.DisplayName).ToString()

            }).ToList();
            }


            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger", Value = "Hamburger" });
            names.Add(new SelectListItem { Text = "External", Value = "External" });
            names.Add(new SelectListItem { Text = "Sub-Footer", Value = "Sub-Footer" });
            names.Add(new SelectListItem { Text = "MobileFooter", Value = "MobileFooter" });
            obj.linkposition = names;
            ViewBag.CreateUpdate = "Save";

            if (pageid > 0)
            {
                ViewBag._Type = "micro";
            }

            return View("~/Views/backoffice/cms/addpages.cshtml", obj);
        }
        [HttpPost]
        [Route("backoffice/cms/addpages")]
        [Route("backoffice/cms/addpages/{name}/{pageid:int}")]
        public IActionResult addpages(clsCMS cls, IFormCollection frm, IFormFile file_Uploader, int? pageid)
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu(cls.collageid);


            if (cls.collageid != null)
            {
                obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
            .Where(row => row.Field<int>("collageid") == pageid)
            .Select(row => new SelectListItem
            {
                Value = row.Field<int>("pageid").ToString(),
                Text = row.Field<string>("linkname")
            }).ToList();
            }
            else
            {
                var content = _backofficeService.BindPageList().Result;
                var pageList = BuildHierarchy(content, 0, 0, cls.collageid);
                obj.selectparent = pageList.AsEnumerable()
             .Where(x => x.collageid == 0)
            .Select(row => new SelectListItem
            {
                Value = row.Id.ToString(),
                Text = WebUtility.HtmlDecode(row.DisplayName).ToString()

            }).ToList();
            }

            //obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
            //    .Select(row => new SelectListItem
            //    {
            //        Value = row.Field<int>("pageid").ToString(),
            //        Text = row.Field<string>("linkname")
            //    }).ToList();

            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger", Value = "Hamburger" });
            names.Add(new SelectListItem { Text = "External", Value = "External" });
            names.Add(new SelectListItem { Text = "Sub-Footer", Value = "Sub-Footer" });
            names.Add(new SelectListItem { Text = "MobileFooter", Value = "MobileFooter" });
            obj.linkposition = names;
            ViewBag.CreateUpdate = "Save";

            if (string.IsNullOrEmpty(cls.linkname))
            {
                ViewBag.Message = "Page Name is required.";
                if (cls.collageid != null)
                {
                    return RedirectToAction("addpages", "CMS", new { name = "micro", pageid = pageid });
                }
                else
                {
                    return View("~/Views/backoffice/cms/addpages.cshtml", obj);
                }

            }
            if (cls.Id > 0)
            {
                obj.Id = cls.Id;
                obj.mode = 2;
            }
            else
            {
                obj.mode = 1;
                obj.PageStatus = true;
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
            if (string.IsNullOrEmpty(cls.controllername))
            {
                obj.controllername = "SkipperHome";
            }
            else
            {
                obj.controllername = cls.controllername;
            }
            if (string.IsNullOrEmpty(cls.actionname))
            {
                obj.actionname = "cms";
            }
            else
            {
                obj.actionname = cls.actionname;
            }

            obj.displayorder = cls.displayorder;
            obj.pagedesc2 = cls.pagedesc2;
            obj.pagedesc3 = cls.pagedesc3;
            obj.mobilemegamenu = cls.mobilemegamenu;
            if (file_Uploader != null && file_Uploader.Length > 0)
            {
                var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                var filePath = Path.Combine("wwwroot/uploads/banner", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file_Uploader.CopyTo(stream);
                }

                obj.uploadbanner = fileName;
            }
            else
            {
                obj.uploadbanner = cls.uploadbanner;
            }
            obj.collageid = cls.collageid;
            int x = _backofficeService.AddCMS(obj);
            if (x > 0)
            {
                if (cls.Id > 0)
                {
                    HttpContext.Session.SetString("Message", "Page Update successfully.");
                    if (cls.collageid != null)
                    {
                        return RedirectToAction("viewpages", "CMS", new { name = "micro", pageid = obj.collageid });
                    }
                    else
                    {
                        return RedirectToAction("viewpages", "cms");
                    }

                }
                else
                {
                    HttpContext.Session.SetString("Message", "Page Save successfully.");
                    if (cls.collageid != null)
                    {
                        return RedirectToAction("addpages", "CMS", new { name = "micro", pageid = obj.collageid });
                    }
                    else
                    {
                        return RedirectToAction("addpages", "cms");
                    }

                }
            }
            else
            {
                ViewBag.Message = "Error while saving record.";
            }
            return View("~/Views/backoffice/cms/addpages.cshtml", obj);
        }
        [HttpGet]
        [Route("backoffice/cms/viewpages/{name?}/{pageid?}")]
        public IActionResult viewpages(string name, int pageid)
        {
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu(pageid);
            var content = _backofficeService.BindPageList().Result;
            if (Convert.ToInt16(pageid) > 0)
            {
                var rows = content.AsEnumerable()
                      .Where(r => r.Field<int>("collageid") == pageid);

                content = rows.Any() ? rows.CopyToDataTable() : content.Clone();
            }
            var pageList = BuildHierarchy(content, 0, 0, pageid);
            if (pageid > 0)
            {
                ViewBag._Type = "micro";
            }
            return View("~/Views/backoffice/cms/viewpages.cshtml", pageList);
        }
        private List<clsCMS> BuildHierarchy(DataTable table, int parentId, int level, int collageid)
        {
            var list = new List<clsCMS>();
            var rows = table.Select("parentid=" + parentId + " and collageid=" + collageid, "displayorder ASC");

            foreach (var row in rows)
            {
                string prefix = level == 0 ? "" : string.Concat(Enumerable.Repeat("&nbsp;&nbsp;&nbsp;", level)) + "› ";


                var page = new clsCMS
                {
                    Id = Convert.ToInt32(row["Pageid"]),
                    pagename = Convert.ToString(row["pagename"]) ?? string.Empty,
                    linkname = Convert.ToString(row["linkname"]) ?? string.Empty,
                    pageposition = Convert.ToString(row["linkposition"]) ?? string.Empty,
                    displayorder = Convert.ToInt32(row["displayorder"]),
                    PageStatus = row["pagestatus"] != DBNull.Value ? Convert.ToBoolean(row["pagestatus"]) : false,
                    Level = level,
                    DisplayName = prefix + " " + Convert.ToString(row["linkname"])
                };

                list.Add(page);

                // Add child pages recursively
                list.AddRange(BuildHierarchy(table, page.Id, level + 1, collageid));
            }

            return list;
        }

        [HttpGet]
        [Route("backoffice/cms/editstatus/{id:int}")]
        public IActionResult editstatus(int id)
        {
            HttpContext.Session.Remove("Message");
            var obj = new clsCMS();
            ViewBag.Menus = _menuService.GetMenu();

            string pageid = HttpContext.Session.GetString("microid");

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

                    if (pageid != null)
                    {
                        return RedirectToAction("viewpages", "CMS", new { name = "micro", pageid = pageid });
                    }
                    else
                    {
                        return RedirectToAction("viewpages", "CMS");
                    }


                }
            }

            return View("~/Views/backoffice/cms/viewpages.cshtml");
        }
        [HttpGet]
        [Route("backoffice/cms/edit/{id}")]
        [Route("backoffice/cms/edit/{name}/{pageid}/{id}")]
        public IActionResult edit(int id, int? pageid)
        {
            var obj = new clsCMS();

            ViewBag.Menus = _menuService.GetMenu(pageid);


            obj.selectparent = _backofficeService.GetPageList().Result.AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<int>("pageid").ToString(),
                    Text = row.Field<string>("linkname")
                }).ToList();
            List<SelectListItem> names = new List<SelectListItem>();
            names.Add(new SelectListItem { Text = "Header", Value = "Header" });
            names.Add(new SelectListItem { Text = "Hamburger", Value = "Hamburger" });
            names.Add(new SelectListItem { Text = "External", Value = "External" });
            names.Add(new SelectListItem { Text = "Sub-Footer", Value = "Sub-Footer" });
            names.Add(new SelectListItem { Text = "MobileFooter", Value = "MobileFooter" });
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
                obj.uploadbanner = dt.Rows[0]["uploadbanner"].ToString();
                obj.pagedesc2 = WebUtility.HtmlDecode(dt.Rows[0]["pagedescription1"].ToString());
                obj.pagedesc3 = WebUtility.HtmlDecode(dt.Rows[0]["pagedescription2"].ToString());
                obj.mobilemegamenu = WebUtility.HtmlDecode(dt.Rows[0]["mobilemegamenu"].ToString());
                obj.collageid = Convert.ToInt32(dt.Rows[0]["collageid"].ToString());

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


    }
}
