using AspNetCoreGeneratedDocument;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace skipper_group_new.Controllers
{
    public class InvestorController : Controller
    {
        private readonly IInvestor _Investor;
        private readonly clsMainMenuList _menuService;

        public InvestorController(clsMainMenuList menuService, IInvestor investor)
        {
            _Investor = investor;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region category
        [HttpGet]
        [Route("backoffice/investor/category")]
        public async Task<IActionResult> category()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.ButtonName = "Save";
            return View("~/Views/backoffice/investor/category.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/viewcategory")]
        public async Task<IActionResult> viewcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _Investor.GetCategory();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/investor/viewcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/investor/addcategory")]
        public async Task<IActionResult> addcategory(clsCategory c, IFormFile UploadAPDFFile, IFormFile BannerImgFile, IFormFile HomeImageFile)
        {
            try
            {
                var keysToRemove = ModelState.Keys.Where(k => k != nameof(c.Category)).ToList();
                foreach (var key in keysToRemove)
                {
                    ModelState.Remove(key);
                }
                if (c.PcatId == 0)
                {
                    c.Status = true;
                    c.DisplayOrder = 0;
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ButtonName = c.PcatId > 0 ? "Update" : "Save";
                    return View("~/Views/backoffice/investor/category.cshtml", c);
                }

                await _Investor.AddCategory(c);

                HttpContext.Session.SetString(
                    "Message",
                    c.PcatId > 0
                        ? "Category updated successfully."
                        : "Category added successfully."
                );

                return RedirectToAction("viewcategory", "investor");
            }
            catch
            {
                TempData["ErrorMessage"] = "❌ Something went wrong.";
                return RedirectToAction("viewcategory", "investor");
            }
        }

        [HttpGet]
        [Route("backoffice/investor/GetCategoryByID/{id}")]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                clsCategory obj = new clsCategory();

                if (id > 0)
                {
                    var cat = await _Investor.EditCategory(id);
                    if (cat != null)
                    {
                        obj.Category = cat.Category;
                        obj.Status = cat.Status;
                        obj.Detail = WebUtility.HtmlDecode(cat.Detail);
                        obj.PcatId = cat.PcatId;
                        obj.ShortDetail = WebUtility.HtmlDecode(cat.ShortDetail);
                        obj.DisplayOrder = cat.DisplayOrder;
                        obj.Banner = cat.Banner;
                        obj.PageMeta = cat.PageMeta;
                        obj.PageTitle = cat.PageTitle;
                        obj.PageMetaDesc = cat.PageMetaDesc;
                        obj.Canonical = cat.Canonical;
                        obj.PageScript = cat.PageScript;
                        obj.UploadAPDF = cat.UploadAPDF;

                        ViewBag.ButtonName = "Update";
                        return View("~/Views/backoffice/investor/category.cshtml", obj);
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", " Category not found.");
                        return RedirectToAction("viewcategory", "investor");
                    }
                }
                else
                {

                    HttpContext.Session.SetString("Message", " Invalid Category ID.");
                    return RedirectToAction("viewcategory", "investor");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching the category details.";
                return RedirectToAction("viewcategory", "investor");
            }
        }

        [HttpGet]
        [Route("backoffice/investor/DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedCat = await _Investor.DeleteCategory(id);
                    if (deletedCat > 0)
                    {
                        HttpContext.Session.SetString("Message", " Category deleted successfully.");
                        TempData["Title"] = "Category";
                    }
                }
                else
                {
                    HttpContext.Session.SetString("Message", " Category id is necessary.");

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewcategory", "investor");
        }

        [HttpPost]
        [Route("backoffice/investor/ExportCategoryToExcel")]
        public async Task<IActionResult> ExportCategoryToExcel()
        {
            var catdtl = await _Investor.GetCategoryTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Categories");
                worksheet.Cell(1, 1).Value = "Category";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Date";

                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.Category;
                    worksheet.Cell(row, 2).Value = c.PageTitle;
                    worksheet.Cell(row, 3).Value = c.trdate;
                    worksheet.Cell(row, 3).Style.DateFormat.Format = "yyyy-MM-dd";

                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"CategoryList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet]
        [Route("backoffice/investor/changecatstatus/{id}")]
        public async Task<IActionResult> changecatstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _Investor.ChangeCatStatus(id);
                    if (chngstatus > 0)
                    {
                        HttpContext.Session.SetString("Message", " Status changed successfully.");
                        TempData["Title"] = "Product";
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", " Status changed successfully.");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewcategory", "investor");
        }
        #endregion

        #region Sub-Category
        [HttpGet]
        [Route("backoffice/investor/subcategory")]
        public async Task<IActionResult> subcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var cat = await _Investor.GetCategoryTblData();
            var filterresult = cat.AsEnumerable()
                 .Where(row => row.Status == true)
                 .ToList();
            if (filterresult != null && filterresult.Count > 0)
            {

                var categoryList = filterresult.AsEnumerable()

                    .Select(row => new SelectListItem
                    {
                        Value = Convert.ToString(row.PcatId),  // or your key column
                        Text = row.Category  // or your name column
                    })
                    .ToList();


                ViewBag.Category = new SelectList(categoryList, "Value", "Text");
            }
            else
            {
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            return View("~/Views/backoffice/investor/subcategory.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/viewsubcategory")]
        public async Task<IActionResult> viewsubcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _Investor.GetSubCategoryTblData();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/investor/viewsubcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/investor/addsubcategory")]
        public async Task<IActionResult> addsubcategory(SubCategory c, IFormFile UploadAIFile, IFormFile BannerImgFile)
        {
            try
            {
                if (UploadAIFile != null && UploadAIFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(UploadAIFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadAIFile.CopyToAsync(stream);
                    }

                    c.UploadAImage = "/uploads/ProductImages/" + uniqueName;
                }
                if (BannerImgFile != null && BannerImgFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(BannerImgFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BannerImgFile.CopyToAsync(stream);
                    }

                    c.Banner = "/uploads/ProductImages/" + uniqueName;
                }
                if (c.PSubCatId > 0)
                {
                    c.Mode = "2";
                }
                else
                {
                    c.Mode = "1";
                    c.Status = true;
                    c.ShowOnHome = false;
                    c.DisplayOrder = 0;
                }
                if (c != null)
                {
                    int x = await _Investor.AddSubCategory(c);
                    if (x > 0)
                    {
                        if (c.PSubCatId > 0)
                        {
                            HttpContext.Session.SetString("Message", " Sub-Category updated successfully.");
                            TempData["Title"] = "Sub-Category";
                            return RedirectToAction("viewsubcategory", "investor");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", " Sub-Category added successfully.");

                            TempData["Title"] = "Sub-Category";
                            return RedirectToAction("viewsubcategory", "investor");
                        }

                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return RedirectToAction("viewsubcategory", "investor");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("viewsubcategory", "investor");
            }
            return RedirectToAction("viewsubcategory", "investor");
        }

        [HttpGet]
        [Route("backoffice/investor/GetSubCategoryByID/{id}")]
        public async Task<IActionResult> GetSubCategoryByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var cat = await _Investor.EditSubCategory(id);
                    if (cat != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        var cat1 = await _Investor.GetCategoryTblData();
                        var filterresult = cat1.AsEnumerable()
                             .Where(row => row.Status == true)
                             .ToList();
                        if (filterresult != null && filterresult.Count > 0)
                        {

                            var categoryList = filterresult.AsEnumerable()

                                .Select(row => new SelectListItem
                                {
                                    Value = Convert.ToString(row.PcatId),  // or your key column
                                    Text = row.Category  // or your name column
                                })
                                .ToList();


                            ViewBag.Category = new SelectList(categoryList, "Value", "Text");
                        }
                        else
                        {
                            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                        }
                        return View("~/Views/backoffice/investor/subcategory.cshtml", cat);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Category not found.";
                        return RedirectToAction("viewsubcategory", "investor");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Category ID.";
                    return RedirectToAction("viewsubcategory", "investor");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching the category details.";
                return RedirectToAction("viewsubcategory", "investor");
            }
        }

        [HttpGet]
        [Route("backoffice/investor/DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedCat = await _Investor.DeleteSubCategory(id);
                    if (deletedCat < 0)
                    {
                        HttpContext.Session.SetString("Message", " Sub-Category deleted successfully.");
                        TempData["Title"] = "Sub-Category";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Sub-Category id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewsubcategory", "investor");
        }

        [HttpPost]
        [Route("backoffice/investor/ExportSubCategoryToExcel")]
        public async Task<IActionResult> ExportSubCategoryToExcel()
        {
            var catdtl = await _Investor.GetSubCategoryTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("SubCategories");
                worksheet.Cell(1, 1).Value = "Sub Category";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.Category;
                    worksheet.Cell(row, 2).Value = c.PageTitle;
                    worksheet.Cell(row, 3).Value = c.trdate;
                    worksheet.Cell(row, 3).Style.DateFormat.Format = "yyyy-MM-dd";
                    worksheet.Cell(row, 4).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"SubCategoryList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet]
        [Route("backoffice/investor/changesubcatstatus/{id}")]
        public async Task<IActionResult> changesubcatstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _Investor.ChangeSubCatStatus(id);
                    if (chngstatus > 0)
                    {
                        HttpContext.Session.SetString("Message", " Status changed successfully.");

                        TempData["Title"] = "Product";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to change the status.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Id is necessary.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewsubcategory", "investor");
        }
        #endregion

        [HttpGet]
        [Route("backoffice/investor/yearcategory")]
        public async Task<IActionResult> yearcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var cat = await _Investor.BindYearCategory();
            ViewBag.YearCategory = cat;
            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/investor/yearcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/investor/yearcategory")]
        public async Task<IActionResult> yearcategory(clsInvestor obj)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            clsInvestor objcls = new clsInvestor();
            if (obj.yearcategory != null)
            {
                objcls.Id = obj.Id;
                objcls.yearcategory = obj.yearcategory;
                objcls.displayorder = obj.displayorder;
                objcls.status = obj.status;
                if (obj.Id > 0)
                {
                    objcls.mode = "2";
                }
                else
                {
                    objcls.mode = "1";
                }
                objcls.uname = HttpContext.Session.GetString("UserName");
                int x = _Investor.AddYearCategory(objcls);
                {
                    if (obj.Id > 0)
                    {
                        HttpContext.Session.SetString("Message", " Year Category updated successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", " Year Category Added successfully.");
                    }
                    return RedirectToAction("yearcategory", "investor");
                }
            }

            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/investor/yearcategory.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {

            var objcls = new clsInvestor();
            ViewBag.Menus = _menuService.GetMenu();


            //Get list of banner types
            var yeartype = await _Investor.BindYearCategory();
            var filterresults = from DataRow dr in yeartype.Rows
                                where Convert.ToInt32(dr["ycatid"]) == id
                                select dr;
            if (filterresults.Any())
            {
                var row = filterresults.First(); // Get the first matching row

                objcls.Id = Convert.ToInt32(row["ycatid"]);
                objcls.yearcategory = Convert.ToString(row["category"]);
                objcls.status = Convert.ToBoolean(row["status"]);
                objcls.displayorder = Convert.ToString(row["displayorder"]);
                ViewBag.CreateUpdate = "Update";
            }

            return View("~/Views/Backoffice/investor/yearcategory.cshtml", objcls);
        }

        [HttpGet]
        [Route("backoffice/investor/yearstatus/{id}")]
        public async Task<IActionResult> yearstatus(int id)
        {

            clsInvestor obj = new clsInvestor();
            var x = await _Investor.BindYearCategory();
            var filterresults = from DataRow dr in x.Rows
                                where Convert.ToInt32(dr["ycatid"]) == id
                                select dr;
            if (filterresults.Any())
            {
                var row = filterresults.First();
                obj.status = Convert.ToBoolean(row["status"]) == true ? true : false; // Toggle status

                string status = obj.status ? "True" : "False";

                int x1 = _Investor.UpdateYearCategoryStatus(status, id);
                if (x1 > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                    return RedirectToAction("yearcategory", "investor");
                }
            }

            return RedirectToAction("yearcategory", "investor");
        }
        [HttpGet]
        [Route("backoffice/investor/YearDelete/{id}")]
        public async Task<IActionResult> YearDelete(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = _Investor.DeleteYearCategory(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Year Category deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete album. Please try again.");
            }


            return RedirectToAction("yearcategory", "investor");
        }

        [HttpGet]
        [Route("backoffice/investor/addinvestor")]
        public async Task<IActionResult> addinvestor()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            clsInvestor obj = new clsInvestor();
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/investor/addinvestor.cshtml", obj);
        }
        [HttpPost]
        [Route("backoffice/investor/saveinvestor")]
        public async Task<IActionResult> saveinvestor(clsInvestor cls, IFormFile fileuploader, IFormFile fileuploader2)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.CreateUpdate = "Save";
            clsInvestor objcls = new clsInvestor();
            if (!string.IsNullOrEmpty(cls.category) && !string.IsNullOrEmpty(cls.subcategory) && !string.IsNullOrEmpty(cls.Name))
            {
                objcls.Id = cls.Id;
                objcls.category = cls.category;
                objcls.subcategory = cls.subcategory;
                objcls.Name = cls.Name;
                objcls.ShortDetail = cls.ShortDetail;
                objcls.Description = cls.Description;
                objcls.vediourl = cls.vediourl;
                objcls.thirdpartyurl = cls.thirdpartyurl;
                objcls.investordate = cls.investordate;
                objcls.doctype = cls.doctype;
                objcls.newexpiredate = cls.newexpiredate;
                objcls.Quarterly = cls.Quarterly;
                objcls.rewriteurl = cls.rewriteurl;
                objcls.uploadfile = cls.uploadfile;
                objcls.uploadimage = cls.uploadimage;
                if (objcls.Id > 0)
                {
                    objcls.mode = "2";
                    objcls.status = cls.status;
                }
                else
                {
                    objcls.mode = "1";
                    objcls.status = true;
                }
                objcls.showonhome = cls.showonhome;

                if (fileuploader != null && fileuploader.Length > 0)
                {
                    var fileName = Path.GetFileName(fileuploader.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/ProductsImage", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fileuploader.CopyTo(stream);
                    }

                    objcls.uploadimage = fileName;
                }
                else
                {
                    objcls.uploadimage = cls.uploadimage ?? string.Empty;
                }
                if (fileuploader2 != null && fileuploader2.Length > 0)
                {
                    var fileName = Path.GetFileName(fileuploader2.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/prospectus", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fileuploader2.CopyTo(stream);
                    }

                    objcls.uploadfile = fileName;
                }
                else
                {
                    objcls.uploadfile = cls.uploadfile ?? string.Empty;
                }
                objcls.yearcategory = cls.yearcategory;
                objcls.displayorder = cls.displayorder;
                objcls.uname = HttpContext.Session.GetString("UserName");
                int x = _Investor.AddInvestor(objcls);
                {
                    if (objcls.Id > 0)
                    {
                        HttpContext.Session.SetString("Message", " Investor updated successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", " Investor Added successfully.");
                    }
                    return RedirectToAction("viewinvestor", "investor");
                }
            }
            else
            {
                HttpContext.Session.SetString("Message", " Please select  category, sub category and investor Name.");
                return View("~/Views/backoffice/investor/addinvestor.cshtml", objcls);
            }



            //return View("~/Views/backoffice/investor/addinvestor.cshtml");
        }
        [HttpGet]
        [Route("backoffice/investor/investoredit/{id}")]
        public async Task<IActionResult> investoredit(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = await _Investor.BindInvestor();

            var filterresults = from DataRow dr in x.Rows
                                where Convert.ToInt32(dr["productid"]) == Convert.ToInt32(id)
                                select dr;
            clsInvestor objcls = new clsInvestor();
            if (filterresults.Any())
            {
                objcls.Id = Convert.ToInt32(filterresults.First()["productid"]);
                objcls.category = Convert.ToString(filterresults.First()["pcatid"]);
                objcls.subcategory = Convert.ToString(filterresults.First()["psubcatid"]);
                objcls.Quarterly = Convert.ToString(filterresults.First()["yqid"]);
                objcls.Name = Convert.ToString(filterresults.First()["productname"]);
                objcls.ShortDetail = WebUtility.HtmlDecode(Convert.ToString(filterresults.First()["shortdetail"]));

                objcls.vediourl = Convert.ToString(filterresults.First()["vedioname"]);
                objcls.thirdpartyurl = Convert.ToString(filterresults.First()["purl"]);
                objcls.investordate = filterresults.First().Field<DateTime?>("investordate");

                objcls.doctype = Convert.ToString(filterresults.First()["modelno"]);
                objcls.newexpiredate = filterresults.First().IsNull("expiraydate") ? (DateTime?)null : Convert.ToDateTime(filterresults.First()["expiraydate"]);
                objcls.yearcategory = Convert.ToString(filterresults.First()["ycatid"]);
                objcls.rewriteurl = Convert.ToString(filterresults.First()["rewrite_url"]);
                objcls.showongroup = Convert.ToBoolean(filterresults.First()["showongroup"]);
                objcls.uploadfile = Convert.ToString(filterresults.First()["prospectus"]);
                objcls.uploadimage = Convert.ToString(filterresults.First()["uploadaimage"]);
                objcls.status = Convert.ToBoolean(filterresults.First()["status"]);
                objcls.displayorder = Convert.ToString(filterresults.First()["displayorder"]);
                ViewBag.CreateUpdate = "Update";
                return View("~/Views/backoffice/investor/addinvestor.cshtml", objcls);
            }
            return View("~/Views/backoffice/investor/viewinvestor.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/viewinvestor")]
        public async Task<IActionResult> viewinvestor()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            DataTable x = await _Investor.BindInvestor();

            ViewBag.Bindinvestor = x.AsEnumerable()
        .OrderBy(r => r["productid"] == DBNull.Value ? int.MaxValue : Convert.ToInt32(r["productid"]))
        .CopyToDataTable();

            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/investor/viewinvestor.cshtml");
        }
        [HttpGet]
        [Route("backoffice/investor/investorstatus/{id}")]
        public async Task<IActionResult> investorstatus(int id)
        {

            clsInvestor obj = new clsInvestor();
            var x = await _Investor.BindInvestor();
            var filterresults = from DataRow dr in x.Rows
                                where Convert.ToInt32(dr["productid"]) == id
                                select dr;
            if (filterresults.Any())
            {
                var row = filterresults.First();
                obj.status = Convert.ToBoolean(row["status"]) == true ? true : false; // Toggle status

                string status = obj.status ? "True" : "False";

                int x1 = _Investor.UpdateInvestorStatus(status, id);
                if (x1 > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                    return RedirectToAction("viewinvestor", "investor");
                }
            }

            return RedirectToAction("viewinvestor", "investor");
        }
        [HttpGet]
        [Route("backoffice/investor/investorDelete/{id}")]
        public async Task<IActionResult> investorDelete(int id)
        {
            HttpContext.Session.Remove("Message");
            var result = _Investor.DeleteINvestor(id);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Investor deleted successfully.");
            }
            else
            {
                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Failed to delete album. Please try again.");
            }


            return RedirectToAction("viewinvestor", "investor");
        }
        [HttpGet]
        [Route("backoffice/investor/investorshowonhome/{id}")]
        public async Task<IActionResult> investorshowonhome(int id)
        {

            clsInvestor obj = new clsInvestor();
            var x = await _Investor.BindInvestor();
            var filterresults = from DataRow dr in x.Rows
                                where Convert.ToInt32(dr["productid"]) == id
                                select dr;
            if (filterresults.Any())
            {
                var row = filterresults.First();
                obj.status = Convert.ToBoolean(row["showongroup"]) == true ? true : false; // Toggle status

                string status = obj.status ? "True" : "False";

                int x1 = _Investor.UpdateInvestorShowonHome(status, id);
                if (x1 > 0)
                {
                    HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                    return RedirectToAction("viewinvestor", "investor");
                }
            }

            return RedirectToAction("viewinvestor", "investor");
        }
        [HttpGet]
        [Route("investor/downloadfile/{id}")]
        public IActionResult DownloadFile(string id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", "prospectus", id);


            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "application/octet-stream";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, id);
        }
        [HttpGet("investor/getyearcategory")]
        public async Task<JsonResult> getyearcategory()
        {
            DataTable dt = await _Investor.BindYearCategory();

            var list = dt.AsEnumerable()
               .Where(row => row.Field<bool>("status") == true)
                         .Select(row => new
                         {
                             Value = row["ycatid"],
                             Text = row["category"]
                         })
                         .ToList();



            return Json(list);
        }

        [HttpGet]
        [Route("Investor/GetCategory")]
        public async Task<JsonResult> GetCategory()
        {
            DataTable dt1 = await _Investor.GetCategory();

            var list1 = dt1.AsEnumerable()
                .Where(row =>
                    dt1.Columns.Contains("status") &&
                    row["status"] != DBNull.Value &&
                    Convert.ToBoolean(row["status"]) == true
                )
                .Select(row => new
                {
                    value = row["pcatid"]?.ToString(),
                    text = row["category"]?.ToString()
                })
                .ToList();

            return Json(list1);
        }


        [HttpGet("investor/getsubcategory")]
        public async Task<JsonResult> getsubcategory(int categoryId)
        {
            DataTable dt = await _Investor.GetSubCategory();
            var filterresults = from DataRow dr in dt.Rows
                                where Convert.ToInt32(dr["pcatid"]) == categoryId && Convert.ToBoolean(dr["status"]) == true
                                select dr;

            var list = filterresults
            .Select(x => new
            {
                Value = x["psubcatid"].ToString(),
                Text = x["category"].ToString()
            })
            .ToList();

            return Json(list);


        }
    }
}
