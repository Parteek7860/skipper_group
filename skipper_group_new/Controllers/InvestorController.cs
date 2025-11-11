using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;

namespace skipper_group_new.Controllers
{
    public class InvestorController : Controller
    {
        private readonly IProducts _products;
        private readonly clsMainMenuList _menuService;

        public InvestorController(clsMainMenuList menuService, IProducts products)
        {
            _products = products;
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
            return View("~/Views/backoffice/investor/category.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/viewcategory")]
        public async Task<IActionResult> viewcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _products.GetCategoryTblData();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/investor/viewcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/investor/AddCategory")]
        public async Task<IActionResult> AddCategory(clsCategory c, IFormFile UploadAPDFFile, IFormFile BannerImgFile, IFormFile HomeImageFile)
        {
            try
            {
                if (UploadAPDFFile != null && UploadAPDFFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(UploadAPDFFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadAPDFFile.CopyToAsync(stream);
                    }

                    c.UploadAPDF = "/uploads/ProductImages/" + uniqueName;
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
                if (HomeImageFile != null && HomeImageFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(HomeImageFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HomeImageFile.CopyToAsync(stream);
                    }

                    c.HomeImage = "/uploads/ProductImages/" + uniqueName;
                }
                if (c != null)
                {
                    int x = await _products.AddCategory(c);
                    if (x > 0)
                    {
                        if (c.PcatId > 0)
                        {
                            HttpContext.Session.SetString("Message", " Category updated successfully.");
                            
                            return RedirectToAction("viewcategory", "investor");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", " Category Add successfully.");
                            return RedirectToAction("viewcategory", "investor");
                        }
                    }
                }
                else
                {
                    HttpContext.Session.SetString("Message", " Please correct the error and try again");
                    
                    return RedirectToAction("viewcategory", "investor");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Something went wrong.";
                return RedirectToAction("viewcategory", "investor");
            }
            return RedirectToAction("viewcategory", "investor");
        }

        [HttpGet]
        [Route("backoffice/investor/GetCategoryByID/{id}")]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var cat = await _products.EditCategory(id);
                    if (cat != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        return View("~/Views/backoffice/investor/category.cshtml", cat);
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
                    var deletedCat = await _products.DeleteCategory(id);
                    if (deletedCat > 0)
                    {
                        HttpContext.Session.SetString("Message", " Category deleted successfully.");
                        TempData["Title"] = "Category";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the category.";
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
            var catdtl = await _products.GetCategoryTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Categories");
                worksheet.Cell(1, 1).Value = "Category";
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
                    string fileName = $"CategoryList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/investor/ChangeCatStatus/{id}")]
        public async Task<IActionResult> ChangeCatStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _products.ChangeCatStatus(id);
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
            var cat = await _products.GetCatDropdown();
            ViewBag.Category = new SelectList(cat, "Key", "Value");
            return View("~/Views/backoffice/investor/subcategory.cshtml");
        }

        [HttpGet]
        [Route("backoffice/investor/viewsubcategory")]
        public async Task<IActionResult> viewsubcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _products.GetSubCategoryTblData();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/investor/viewsubcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/investor/AddSubCategory")]
        public async Task<IActionResult> AddSubCategory(SubCategory c, IFormFile UploadAIFile, IFormFile BannerImgFile)
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
                if (c != null)
                {
                    int x = await _products.AddSubCategory(c);
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
                    var cat = await _products.EditSubCategory(id);
                    if (cat != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        var catd = await _products.GetCatDropdown();
                        ViewBag.Category = new SelectList(catd, "Key", "Value", cat.PCatId);
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
                    var deletedCat = await _products.DeleteSubCategory(id);
                    if (deletedCat > 0)
                    {
                        HttpContext.Session.SetString("Message", " Sub-Category deleted successfully.");
                        
                        TempData["Title"] = "Sub-Category";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the sub-category.";
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
            var catdtl = await _products.GetSubCategoryTblData();
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

        [HttpPost]
        [Route("backoffice/investor/ChangeSubCatStatus/{id}")]
        public async Task<IActionResult> ChangeSubCatStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _products.ChangeSubCatStatus(id);
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
    }
}
