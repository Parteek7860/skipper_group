using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;
using System.Net;
using System.Reflection;

namespace skipper_group_new.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProducts _products;
        private readonly clsMainMenuList _menuService;

        public ProductsController(clsMainMenuList menuService, IProducts products)
        {
            _products = products;
            _menuService = menuService;
        }

        #region Product
        [HttpGet]
        [Route("backoffice/Products/product")]
        public async Task<IActionResult> product()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var cat = await _products.GetCatDropdown();
            ViewBag.CategoryList = new SelectList(cat, "Key", "Value");
            ViewBag.SubcategoryList = new SelectList(new List<SelectListItem>());
            return View("~/Views/backoffice/Products/product.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetSubcategoriesByCategoryId(int categoryId)
        {
            var subcategories = await _products.GetSubCategoriesByCategoryId(categoryId);

            var result = subcategories.Select(sc => new
            {
                Value = sc.PSubCatId,
                Text = sc.Category
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        [Route("backoffice/Products/viewproduct")]
        public async Task<IActionResult> viewproduct()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var prodDtl = await _products.GetProductTblData();
            ViewBag.ProdDtl = prodDtl;
            return View("~/Views/backoffice/Products/viewproduct.cshtml");
        }

        [HttpPost]
        [Route("backoffice/Products/AddProduct")]
        public async Task<IActionResult> AddProduct(clsProduct product, IFormFile UploadAImageFile, IFormFile BannerImgFile, IFormFile UploadLargeImageFile)
        {
            try
            {
                if (UploadAImageFile != null && UploadAImageFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(UploadAImageFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadAImageFile.CopyToAsync(stream);
                    }

                    product.UploadAImage = "/uploads/ProductImages/" + uniqueName;
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

                    product.BannerImg = "/uploads/ProductImages/" + uniqueName;
                }
                if (UploadLargeImageFile != null && UploadLargeImageFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(UploadLargeImageFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadLargeImageFile.CopyToAsync(stream);
                    }

                    product.LargeImage = "/uploads/ProductImages/" + uniqueName;
                }

                if (product != null)
                {
                    int x = await _products.AddProduct(product);

                    if (x > 0)
                    {
                        if (product.ProductId > 0)
                        {
                            TempData["SuccessMessage"] = "Product updated successfully.";
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Product added successfully.";
                        }

                        return RedirectToAction("viewproduct", "Products");
                    }
                }

                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return RedirectToAction("viewproduct", "Products");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong.";
                return RedirectToAction("viewproduct", "Products");
            }
        }


        [HttpGet]
        [Route("backoffice/Products/GetProductByID/{id}")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var product = await _products.EditProduct(id);
                    if (product != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        var cat = await _products.GetCatDropdown();
                        ViewBag.CategoryList = new SelectList(cat, "Key", "Value", product.PcatId);

                        var subcategories = await _products.GetSubCategoriesByCategoryId(product.PcatId);
                        ViewBag.SubcategoryList = new SelectList(subcategories, "PSubCatId", "Category", product.PsubcatId);

                        return View("~/Views/backoffice/Products/product.cshtml", product);
                    }
                }
            }
            catch { }
            return RedirectToAction("viewproduct", "Products");
        }


        [HttpGet]
        [Route("backoffice/Products/Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedProductId = await _products.DeleteProduct(id);
                    if (deletedProductId > 0)
                    {
                        TempData["SuccessMessage"] = "✅ Product deleted successfully.";
                        return RedirectToAction("viewproduct", "Products");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "⚠️ Not deleted. Somethng went wrong";
                        return RedirectToAction("viewproduct", "Products");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "⚠️ Product id is required";
                    return RedirectToAction("viewproduct", "Products");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewproduct", "Products");
        }

        [HttpPost]
        [Route("backoffice/Products/ExportProductToExcel")]
        public async Task<IActionResult> ExportProductToExcel()
        {
            var catdtl = await _products.GetProductTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                worksheet.Cell(1, 1).Value = "Product";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Model Number";
                worksheet.Cell(1, 4).Value = "Date";
                worksheet.Cell(1, 5).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.ProductName;
                    worksheet.Cell(row, 2).Value = c.ProductTitle;
                    worksheet.Cell(row, 3).Value = c.ModelNo;
                    worksheet.Cell(row, 4).Value = c.trdate;
                    worksheet.Cell(row, 4).Style.DateFormat.Format = "yyyy-MM-dd";
                    worksheet.Cell(row, 5).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"ProductList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/Products/ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _products.ChangeStatus(id);
                    if (chngstatus > 0)
                    {
                        TempData["SuccessMessage"] = "Status changed successfully.";
                        TempData["Title"] = "Product";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to change the product status.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Product id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewproduct", "Products");
        }

        #endregion

        #region category
        [HttpGet]
        [Route("backoffice/Products/category")]
        public async Task<IActionResult> category()
        {
            clsCategory obj = new clsCategory();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.UpdateStatus = "Save";
            return View("~/Views/backoffice/Products/category.cshtml", obj);
        }

        [HttpGet]
        [Route("backoffice/Products/viewcategory")]
        public async Task<IActionResult> viewcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _products.BindProductCategory();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/Products/viewcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/Products/AddCategory")]
        public async Task<IActionResult> AddCategory(clsCategory c, IFormFile UploadAPDFFile, IFormFile BannerImgFile, IFormFile HomeImageFile)
        {
            try
            {
                clsCategory obj = new clsCategory();
                if (UploadAPDFFile != null && UploadAPDFFile.Length > 0)
                {
                    var uploadFileName = Path.GetFileName(UploadAPDFFile.FileName);
                    var uniqueName = $"{Guid.NewGuid()}_{uploadFileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadAPDFFile.CopyToAsync(stream);
                    }

                    obj.UploadAPDF = "/uploads/ProductImages/" + uniqueName;
                }

                if (BannerImgFile != null && BannerImgFile.Length > 0)
                {
                    var fileName = Path.GetFileName(BannerImgFile.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        BannerImgFile.CopyTo(stream);
                    }

                    obj.Banner = fileName;
                }
                else
                {
                    obj.Banner = c.Banner ?? string.Empty;
                }
                if (HomeImageFile != null && HomeImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(HomeImageFile.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        HomeImageFile.CopyTo(stream);
                    }

                    obj.HomeImage = fileName;
                }
                else
                {
                    obj.HomeImage = c.HomeImage ?? string.Empty;
                }
                obj.Category = c.Category;
                obj.Detail = c.Detail;
                obj.ShortDetail = c.ShortDetail;
                obj.HomeDesc = c.HomeDesc;
                obj.Status = c.Status;
                obj.DisplayOrder = c.DisplayOrder;
                obj.PcatId = c.PcatId;
                obj.RewriteUrl = c.RewriteUrl;
                obj.PageTitle = c.PageTitle;
                obj.PageMeta = c.PageMeta;
                obj.PageMetaDesc = c.PageMetaDesc;
                obj.Canonical = c.Canonical;
                if (obj.PcatId > 0)
                {
                    obj.Mode = 2;
                }
                else
                {
                    obj.Mode = 1;
                }
                if (c != null)
                {
                    int x = _products.AddProductCategory(obj);
                    if (x > 0)
                    {
                        if (c.PcatId > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Category updated successfully.");

                            return RedirectToAction("viewcategory", "Products");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Category added successfully.");
                            return RedirectToAction("viewcategory", "Products");
                        }
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return RedirectToAction("viewcategory", "Products");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "❌ Something went wrong.";
                return RedirectToAction("viewcategory", "Products");
            }
            return RedirectToAction("viewcategory", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/GetCategoryByID/{id}")]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var menuList = _menuService.GetMenu();
                    ViewBag.Menus = menuList;
                    clsCategory obj = new clsCategory();
                    var productTypes = await _products.BindProductCategory();
                    // Replace the following code block:

                    // With this code block:
                    var filteredRows = productTypes.AsEnumerable()
                        .Where(pt => pt.Field<int>("pcatid") == id)
                        .ToList();

                    if (filteredRows != null)
                    {
                        obj.Category = filteredRows[0]["category"].ToString();
                        obj.Detail = WebUtility.HtmlDecode(filteredRows[0]["detail"].ToString());
                        obj.ShortDetail = WebUtility.HtmlDecode(filteredRows[0]["shortdetail"].ToString());
                        obj.HomeDesc = WebUtility.HtmlDecode(filteredRows[0]["homedesc"].ToString());
                        obj.homedesc2 = WebUtility.HtmlDecode(filteredRows[0]["homedesc2"].ToString());


                        obj.Status = Convert.ToBoolean(Convert.ToInt32(filteredRows[0]["Status"]));
                        obj.DisplayOrder = Convert.ToInt32(filteredRows[0]["displayorder"]);
                        obj.PcatId = Convert.ToInt32(filteredRows[0]["pcatid"]);
                        obj.RewriteUrl = filteredRows[0]["rewriteurl"].ToString();
                        obj.PageTitle = filteredRows[0]["pagetitle"].ToString();
                        obj.PageMeta = filteredRows[0]["pagemeta"].ToString();
                        obj.PageMetaDesc = filteredRows[0]["pagemetadesc"].ToString();
                        obj.Banner = filteredRows[0]["banner"].ToString();
                        obj.HomeImage = filteredRows[0]["homeimage"].ToString();
                        obj.UploadAPDF = filteredRows[0]["uploadapdf"].ToString();
                        obj.Canonical = filteredRows[0]["canonical"].ToString();
                        ViewBag.UpdateStatus = "Update";
                        return View("~/Views/backoffice/Products/category.cshtml", obj);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Category not found.";
                        return RedirectToAction("viewcategory", "Products");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Category ID.";
                    return RedirectToAction("viewcategory", "Products");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching the category details.";
                return RedirectToAction("viewcategory", "Products");
            }
        }

        [HttpGet]
        [Route("backoffice/Products/DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedCat = await _products.DeleteCategory(id);
                    if (deletedCat > 0)
                    {
                        TempData["SuccessMessage"] = "Category deleted successfully.";
                        TempData["Title"] = "Category";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the category.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Category id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewcategory", "Products");
        }

        [HttpPost]
        [Route("backoffice/Products/ExportCategoryToExcel")]
        public async Task<IActionResult> ExportCategoryToExcel()
        {
            var catdtl = await _products.BindProductCategory();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Categories");
                worksheet.Cell(1, 1).Value = "Category";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Status";
                int row = 2;
                foreach (DataRow c in catdtl.Rows)
                {
                    worksheet.Cell(row, 1).Value = c["category"].ToString();
                    worksheet.Cell(row, 3).Value = c["trdate"].ToString();
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

        [HttpPost]
        [Route("backoffice/Products/ChangeCatStatus/{id}")]
        public async Task<IActionResult> ChangeCatStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    clsProduct obj = new clsProduct();
                    var productTypes = await _products.BindProductCategory();
                    // Replace the following code block:

                    // With this code block:
                    var filteredRows = productTypes.AsEnumerable()
                        .Where(pt => pt.Field<int>("pcatid") == id)
                        .ToList();


                    if (filteredRows.Count > 0)
                    {
                        obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status

                        var chngstatus = _products.CategoryUpdateStatus(obj.Status, id);
                        if (chngstatus > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Status update successfully.");

                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to change the status.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Id is necessary.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewcategory", "Products");
        }
        #endregion

        #region Sub-Category
        [HttpGet]
        [Route("backoffice/Products/subcategory")]
        public async Task<IActionResult> subcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catd = await _products.BindProductCategory();
            var filterresult = catd.AsEnumerable()
                .Where(row => row.Field<bool>("Status") == true)
                .CopyToDataTable();
            if (filterresult != null && filterresult.Rows.Count > 0)
            {

                var categoryList = filterresult.AsEnumerable()

                    .Select(row => new SelectListItem
                    {
                        Value = row["PcatId"].ToString(),  // or your key column
                        Text = row["Category"].ToString()  // or your name column
                    })
                    .ToList();


                ViewBag.Category = new SelectList(categoryList, "Value", "Text");
            }
            else
            {
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            ViewBag.CreateUpdate = "Update";
            return View("~/Views/backoffice/Products/subcategory.cshtml");
        }

        [HttpGet]
        [Route("backoffice/Products/viewsubcategory")]
        public async Task<IActionResult> viewsubcategory()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var catdtl = await _products.BindProductSubCategory();
            ViewBag.CateDtl = catdtl;
            return View("~/Views/backoffice/Products/viewsubcategory.cshtml");
        }

        [HttpPost]
        [Route("backoffice/Products/AddSubCategory")]
        public async Task<IActionResult> AddSubCategory(SubCategory c, IFormFile UploadAIFile, IFormFile BannerImgFile)
        {
            try
            {
                SubCategory obj = new SubCategory();
                if (UploadAIFile != null && UploadAIFile.Length > 0)
                {
                    var fileName = Path.GetFileName(UploadAIFile.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/ProductsImage", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        UploadAIFile.CopyTo(stream);
                    }

                    obj.Banner = fileName;
                }
                else
                {
                    obj.Banner = c.Banner ?? string.Empty;
                }
                if (BannerImgFile != null && BannerImgFile.Length > 0)
                {
                    var fileName = Path.GetFileName(BannerImgFile.FileName); // captures name
                    var filePath = Path.Combine("wwwroot/uploads/ProductsImage", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        BannerImgFile.CopyTo(stream);
                    }

                    obj.Banner = fileName;
                }
                else
                {
                    obj.Banner = c.Banner ?? string.Empty;
                }
                obj.PSubCatId = c.PSubCatId;
                obj.PCatId = c.PCatId;
                obj.Category = c.Category;
                obj.Detail = c.Detail;
                obj.ShortDetail = c.ShortDetail;
                obj.homedesc = c.homedesc;
                obj.homedesc2 = c.homedesc2;
                obj.tagline = c.tagline;
                obj.Status = c.Status;
                obj.DisplayOrder = c.DisplayOrder;
                obj.RewriteUrl = c.RewriteUrl;
                obj.PageTitle = c.PageTitle;
                obj.PageMeta = c.PageMeta;
                obj.PageMetaDesc = c.PageMetaDesc;
                obj.Canonical = c.Canonical;
                if (obj.PSubCatId > 0)
                {
                    obj.Mode = "2";
                }
                else
                {
                    obj.Mode = "1";
                }
                obj.Uname = HttpContext.Session.GetString("UserName");

                if (c != null)
                {
                    int x = _products.AddSubProductCategory(obj);
                    if (x > 0)
                    {
                        if (obj.PSubCatId > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Sub-Category updated successfully.");

                            return RedirectToAction("viewsubcategory", "Products");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Sub Category Added successfully.");

                            return RedirectToAction("subcategory", "Products");
                        }

                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return RedirectToAction("viewsubcategory", "Products");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("viewsubcategory", "Products");
            }
            return RedirectToAction("viewsubcategory", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/GetSubCategoryByID/{id}")]
        public async Task<IActionResult> GetSubCategoryByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    SubCategory obj = new SubCategory();
                    var menuList = _menuService.GetMenu();
                    ViewBag.Menus = menuList;
                    var data = _products.BindProductSubCategory();
                    var cat = data.Result.AsEnumerable()
                        .Where(pt => pt.Field<int>("psubcatid") == id)
                        .ToList();
                    if (cat != null)
                    {
                        obj.PSubCatId = Convert.ToInt32(cat[0]["psubcatid"]);
                        obj.PCatId = Convert.ToInt32(cat[0]["pcatid"]);
                        obj.Category = cat[0]["category"].ToString();
                        obj.PageTitle = cat[0]["pagetitle"].ToString();
                        obj.RewriteUrl = cat[0]["rewriteurl"].ToString();
                        obj.Status = Convert.ToBoolean(Convert.ToInt32(cat[0]["Status"]));
                        obj.DisplayOrder = Convert.ToInt32(cat[0]["displayorder"]);
                        obj.Banner = cat[0]["banner"].ToString();
                        obj.UploadAImage = cat[0]["homeimage"].ToString();
                        obj.Detail = WebUtility.HtmlDecode(cat[0]["detail"].ToString());
                        obj.ShortDetail = WebUtility.HtmlDecode(cat[0]["shortdetail"].ToString());
                        obj.PageMeta = cat[0]["pagemeta"].ToString();
                        obj.PageMetaDesc = cat[0]["pagemetadesc"].ToString();
                        obj.Canonical = cat[0]["canonical"].ToString();
                        obj.tagline = cat[0]["tagline"].ToString();
                        obj.homedesc = WebUtility.HtmlDecode(cat[0]["homedesc"].ToString());
                        obj.homedesc2 = WebUtility.HtmlDecode(cat[0]["homedesc2"].ToString());
                        ViewBag.CreateUpdate = "Update";
                        var catd = await _products.BindProductCategory();
                        if (catd != null && catd.Rows.Count > 0)
                        {

                            var categoryList = catd.AsEnumerable()
                                .Select(row => new SelectListItem
                                {
                                    Value = row["PcatId"].ToString(),  // or your key column
                                    Text = row["Category"].ToString()  // or your name column
                                })
                                .ToList();

                            // Bind to ViewBag for dropdown
                            ViewBag.Category = new SelectList(categoryList, "Value", "Text", obj.PCatId);
                        }
                        else
                        {
                            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                        }

                        return View("~/Views/backoffice/Products/subcategory.cshtml", obj);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Category not found.";
                        return RedirectToAction("viewsubcategory", "Products");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Category ID.";
                    return RedirectToAction("viewsubcategory", "Products");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching the category details.";
                return RedirectToAction("viewsubcategory", "Products");
            }
        }

        [HttpGet]
        [Route("backoffice/Products/DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedCat = _products.SubCategoryDeleteRecords(id);
                    if (deletedCat > 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Delete data successfully.");

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
            return RedirectToAction("viewsubcategory", "Products");
        }

        [HttpPost]
        [Route("backoffice/Products/ExportSubCategoryToExcel")]
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
        [Route("backoffice/Products/ChangeSubCatStatus/{id}")]
        public async Task<IActionResult> ChangeSubCatStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var data = _products.BindProductSubCategory();
                    var filterresults = data.Result.AsEnumerable()
                        .Where(pt => pt.Field<int>("psubcatid") == id)
                        .ToList();
                    if (filterresults.Count > 0)
                    {
                        clsProduct obj = new clsProduct();
                        obj.Status = Convert.ToString(Convert.ToInt32(filterresults[0]["Status"])) == "1" ? "False" : "True"; // Toggle status
                        var chngstatus = _products.SubCategoryUpdateStatus(obj.Status, id);
                        if (chngstatus > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Status update successfully.");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Failed to change the status.";
                        }
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
            return RedirectToAction("viewsubcategory", "Products");
        }
        #endregion

        [HttpGet]
        [Route("backoffice/products/addproductsolution")]
        public async Task<IActionResult> addproductsolution()
        {

            clsCategory objcls = new clsCategory();
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                ViewBag.CreateUpdate = "Save";



                return View("~/Views/backoffice/products/addproductsolution.cshtml", objcls);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/media/media_section.cshtml");
            }
            return View("~/Views/backoffice/products/addproductsolution.cshtml", objcls);
        }
        [HttpPost]
        [Route("backoffice/products/addproductsolution")]
        public async Task<IActionResult> addproductsolution(clsCategory obj, IFormFile file_Uploader, IFormFile file_Uploader2)
        {
            clsCategory objcls = new clsCategory();
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                ViewBag.CreateUpdate = "Save";

                if (!string.IsNullOrEmpty(obj.Category))
                {
                    objcls.Category = obj.Category;
                    objcls.Detail = obj.Detail;
                    objcls.ShortDetail = obj.ShortDetail;
                    objcls.DisplayOrder = obj.DisplayOrder;
                    objcls.productid = obj.productid;
                    objcls.shortname = obj.shortname ?? string.Empty;
                    objcls.PageTitle = obj.PageTitle ?? string.Empty;
                    objcls.PageMeta = obj.PageMeta ?? string.Empty;
                    objcls.PageMetaDesc = obj.PageMetaDesc ?? string.Empty;
                    objcls.UploadAPDF = obj.UploadAPDF ?? string.Empty;
                    if (obj.productid == "0")
                    {
                        objcls.Mode = 1;
                    }
                    else
                    {
                        objcls.Mode = 2;
                    }
                    objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                    objcls.Uname = HttpContext.Session.GetString("UserName");
                    if (file_Uploader != null && file_Uploader.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader.CopyTo(stream);
                        }

                        objcls.Banner = fileName;
                    }
                    else
                    {
                        objcls.Banner = obj.Banner ?? string.Empty;
                    }
                    if (file_Uploader2 != null && file_Uploader2.Length > 0)
                    {
                        var fileName = Path.GetFileName(file_Uploader2.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file_Uploader2.CopyTo(stream);
                        }

                        objcls.UploadAPDF = fileName;
                    }
                    else
                    {
                        objcls.UploadAPDF = obj.UploadAPDF ?? string.Empty;
                    }
                    objcls.Status = obj.Status;
                    int x = await _products.AddProductSolution(objcls);
                    if (x > 0)
                    {
                        if (obj.productid == "0")
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Save successfully.");
                            return RedirectToAction("addproductsolution", "products");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Update successfully.");
                            return RedirectToAction("viewproductsolution", "products");
                        }
                    }
                }

                return View("~/Views/backoffice/products/addproductsolution.cshtml", objcls);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/Products/addproducttype.cshtml");
            }
            return View("~/Views/backoffice/products/addproductsolution.cshtml", objcls);
        }
        [HttpGet]
        [Route("backoffice/products/viewproductsolution")]
        public async Task<IActionResult> viewproductsolution()
        {

            clsCategory objcls = new clsCategory();
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;

                var catdtl = await _products.BindProductSolution();
                ViewBag.CateDtl = catdtl;

                return View("~/Views/backoffice/products/viewproductsolution.cshtml", objcls);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/media/media_section.cshtml");
            }
            return View("~/Views/backoffice/products/viewproductsolution.cshtml", objcls);
        }

        [HttpPost]
        [Route("backoffice/Products/chkstatus/{id}")]
        public async Task<IActionResult> chkstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct objbannertype = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = _products.BindProductSolution();

                        var filterresults = productTypes.Result.AsEnumerable()
                       .Where(pt => pt.Field<int>("productid") == id)
                       .ToList();

                        if (filterresults.Count > 0)
                        {
                            objbannertype.Status = Convert.ToString(Convert.ToInt32(filterresults[0]["Status"])) == "1" ? "False" : "True"; // Toggle status
                            int x1 = _products.UpdateStatus(objbannertype.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("viewproductsolution", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("viewproductsolution", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/edit/{id}")]
        public async Task<IActionResult> edit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");
                        clsCategory obj = new clsCategory();
                        // Get the Media list by ID
                        var productTypes = _products.BindProductSolution();

                        var filterresults = productTypes.Result.AsEnumerable()
                       .Where(pt => pt.Field<int>("productid") == id)
                       .ToList();

                        if (filterresults.Count > 0)
                        {
                            obj.productid = filterresults[0]["productid"].ToString();
                            obj.shortname = filterresults[0]["displayname"].ToString();
                            obj.Banner = filterresults[0]["product_banner"].ToString();
                            obj.Category = filterresults[0]["productname"].ToString();
                            obj.Detail = WebUtility.HtmlDecode(filterresults[0]["productdescp"].ToString());
                            obj.ShortDetail = WebUtility.HtmlDecode(filterresults[0]["productshortdescp"].ToString());
                            obj.DisplayOrder = Convert.ToInt32(filterresults[0]["displayorder"]);
                            obj.Status = Convert.ToBoolean(Convert.ToInt32(filterresults[0]["Status"]));
                            obj.RewriteUrl = filterresults[0]["rewrite_url"].ToString();
                            obj.PageTitle = filterresults[0]["pagetitle"].ToString();
                            obj.PageMeta = filterresults[0]["pagemeta"].ToString();
                            obj.PageMetaDesc = filterresults[0]["pagemetadesc"].ToString();
                            obj.UploadAPDF = filterresults[0]["productsmallmg"].ToString();
                            ViewBag.CreateUpdate = "Update";

                            return View("~/Views/backoffice/products/addproductsolution.cshtml", obj);

                        }

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "An error occurred while processing your request.");
                        return RedirectToAction("viewproductsolution", "Products");
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
            return RedirectToAction("viewproductsolution", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsBannerType objbannertype = new clsBannerType();
                        // Get the Media list by ID
                        //  var productTypes = await _products.GetProductTypeyTblData();



                        int x1 = _products.DeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("viewproductsolution", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("viewproductsolution", "Products");
        }
        [HttpGet]
        [Route("backoffice/Products/addvehicletype")]
        public async Task<IActionResult> addvehicletype()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var cat = await _products.GetProductTypeList() ?? new List<clsCategory>();
            ViewBag.Category = new SelectList(cat, "PcatId", "Category");
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addvehicletype.cshtml");
        }

        [HttpPost]
        [Route("backoffice/Products/addvehicletype")]
        public async Task<IActionResult> addvehicletype(SubCategory obj, IFormFile fileuploader, IFormFile fileuploader2)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            SubCategory objcls = new SubCategory();
            if (!string.IsNullOrEmpty(obj.Category))
            {
                objcls.PSubCatId = obj.PSubCatId;
                objcls.PCatId = obj.PCatId;
                objcls.Category = obj.Category;
                objcls.ShortDetail = obj.ShortDetail;
                objcls.Detail = obj.Detail;
                objcls.SmartTyre = obj.SmartTyre;
                objcls.Others = obj.Others;
                objcls.PageTitle = obj.PageTitle ?? string.Empty;
                objcls.PageMeta = obj.PageMeta ?? string.Empty;
                objcls.PageMetaDesc = obj.PageMetaDesc ?? string.Empty;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.Canonical = obj.Canonical ?? string.Empty;
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (fileuploader != null && fileuploader.Length > 0)
                {
                    var fileName = Path.GetFileName(fileuploader.FileName); // captures name
                    var uniqueName = $"{Guid.NewGuid()}_{fileName}";
                    var filePath = Path.Combine("wwwroot/uploads/productimg", uniqueName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fileuploader.CopyTo(stream);
                    }
                    objcls.UploadAImage = uniqueName;
                }
                else
                {
                    objcls.UploadAImage = obj.UploadAImage ?? string.Empty;
                }
                if (fileuploader2 != null && fileuploader2.Length > 0)
                {
                    var fileName = Path.GetFileName(fileuploader2.FileName); // captures name
                    var uniqueName = $"{Guid.NewGuid()}_{fileName}";
                    var filePath = Path.Combine("wwwroot/uploads/productimg", uniqueName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        fileuploader2.CopyTo(stream);
                    }
                    objcls.Banner = uniqueName;
                }
                else
                {
                    objcls.Banner = obj.Banner ?? string.Empty;
                }
                if (obj.PSubCatId == 0)
                {
                    objcls.Mode = "1";
                    objcls.Status = true;
                }
                else
                {
                    objcls.Mode = "2";
                    objcls.Status = obj.Status;
                }
                int x = await _products.AddVehicleType(objcls);
                if (x > 0)
                {
                    if (obj.PSubCatId == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Save successfully.");
                        return RedirectToAction("addvehicletype", "products");
                    }
                    else
                    {
                        var catdtl = await _products.GetVehicleTyreList();
                        ViewBag.CateDtl = catdtl;

                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + " Update successfully.");
                        return RedirectToAction("viewvehicletype", "products");
                    }
                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addvehicletype.cshtml");
        }

        [HttpGet]
        [Route("backoffice/Products/viewvehicletype")]
        public async Task<IActionResult> viewvehicletype()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var catdtl = await _products.GetVehicleTyreList();
            ViewBag.CateDtl = catdtl;

            return View("~/Views/backoffice/Products/viewvehicletype.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/VehicleStatus/{id}")]
        public async Task<IActionResult> VehicleStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetVehicleTyreList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("vehicle_typeid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.VehicleUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("viewvehicletype", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("viewproducttype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/Vehicleedit/{id}")]
        public async Task<IActionResult> Vehicleedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");
                        SubCategory obj = new SubCategory();
                        var productTypes = await _products.GetVehicleTyreList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("vehicle_typeid") == id)
                            .ToList();

                        if (filteredRows.Count > 0)
                        {
                            obj.PSubCatId = Convert.ToInt16(filteredRows[0]["vehicle_typeid"]);
                            obj.PCatId = Convert.ToInt16(filteredRows[0]["product_typeid"]);
                            obj.Category = Convert.ToString(filteredRows[0]["vehicle_typetitle"]);
                            obj.ShortDetail = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["short_desc"]));
                            obj.Detail = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["detail_desc"]));
                            obj.SmartTyre = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["smarttyres"]));
                            obj.Others = WebUtility.HtmlDecode(Convert.ToString(filteredRows[0]["others"]));
                            obj.UploadAImage = Convert.ToString(filteredRows[0]["uploadimage"]);
                            obj.Banner = Convert.ToString(filteredRows[0]["banner"]);
                            obj.PageTitle = Convert.ToString(filteredRows[0]["pagetitle"]);
                            obj.PageMeta = Convert.ToString(filteredRows[0]["pagemeta"]);
                            obj.PageMetaDesc = Convert.ToString(filteredRows[0]["pagemetadesc"]);
                            obj.RewriteUrl = Convert.ToString(filteredRows[0]["rewriteurl"]);
                            obj.Canonical = Convert.ToString(filteredRows[0]["canonical"]);
                            obj.Status = Convert.ToBoolean(filteredRows[0]["Status"]);

                            var catd = await _products.GetProductTypeList() ?? new List<clsCategory>();
                            ViewBag.Category = new SelectList(catd, "PcatId", "Category", obj.PCatId);

                            ViewBag.CreateUpdate = "Update";

                            return View("~/Views/backoffice/products/addvehicletype.cshtml", obj);

                        }

                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addvehicletype", "Products");
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
            return RedirectToAction("viewvehicletype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/VehicleDelete/{id}")]
        public async Task<IActionResult> VehicleDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.DeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("viewproducttype", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("viewproducttype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/addbrand")]
        public async Task<IActionResult> addbrand()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetBrandList();
            ViewBag.Brand = brand;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addbrand.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/addbrand")]
        public async Task<IActionResult> addbrand(clsProduct obj)
        {
            clsProduct objcls = new clsProduct();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetBrandList();
            ViewBag.Brand = brand;
            // Insert data

            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.DisplayOrder = obj.DisplayOrder;
                objcls.Status = obj.Status ?? "False";
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.id == 0)
                {
                    objcls.Mode = 1;
                }
                else
                {
                    objcls.Mode = 2;
                }
                int x = await _products.AddBrand(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addbrand.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Products/BrandStatus/{id}")]
        public async Task<IActionResult> BrandStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetBrandList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("brandid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.BrandUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("addbrand", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addbrand", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/brandedit/{id}")]
        public async Task<IActionResult> brandedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsProduct objcls = new clsProduct();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _products.GetBrandList();
                        ViewBag.Brand = brand;

                        var x = _products.GetBrandList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("brandid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.id = Convert.ToInt16(filterresult[0]["brandid"]);
                                objcls.title = Convert.ToString(filterresult[0]["brandtitle"]);
                                objcls.RewriteUrl = Convert.ToString(filterresult[0]["rewriteurl"]);
                                objcls.Status = Convert.ToString(filterresult[0]["Status"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/products/addbrand.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addbrand", "Products");
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
            return RedirectToAction("viewvehicletype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/brandDelete/{id}")]
        public async Task<IActionResult> brandDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.BrandDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("addbrand", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addbrand", "Products");
        }
        [HttpGet]
        [Route("backoffice/Products/addmodel")]
        public async Task<IActionResult> addmodel()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var dt = await _products.GetBrandList();
            var cat = dt.AsEnumerable()
                        .Select(r => new clsProduct
                        {
                            id = r.Field<int>("brandid"),
                            title = r.Field<string>("brandtitle")
                        })
                        .ToList();
            ViewBag.Category = new SelectList(cat, "id", "title");

            var brand = await _products.GetModelList();
            ViewBag.ModelList = brand;


            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addmodel.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/addmodel")]
        public async Task<IActionResult> addmodel(clsProduct obj)
        {
            clsProduct objcls = new clsProduct();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var dt = await _products.GetBrandList();
            var cat = dt.AsEnumerable()
                        .Select(r => new clsProduct
                        {
                            id = r.Field<int>("brandid"),
                            title = r.Field<string>("brandtitle")
                        })
                        .ToList();
            ViewBag.Category = new SelectList(cat, "id", "title");

            var brand = await _products.GetModelList();
            ViewBag.ModelList = brand;

            // Insert data

            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.DisplayOrder = obj.DisplayOrder;
                objcls.Status = obj.Status ?? "False";
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.id == 0)
                {
                    objcls.Mode = 1;
                }
                else
                {
                    objcls.Mode = 2;
                }
                int x = await _products.AddModel(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addmodel.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Products/modelStatus/{id}")]
        public async Task<IActionResult> modelStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetModelList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("modelid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.ModelUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("addmodel", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addmoel", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/modeledit/{id}")]
        public async Task<IActionResult> modeledit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        var productTypes = await _products.GetModelList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("modelid") == id)
                            .ToList();

                        if (filteredRows.Count > 0)
                        {
                            obj.id = Convert.ToInt16(filteredRows[0]["modelid"]);
                            obj.title = Convert.ToString(filteredRows[0]["modeltitle"]);
                            obj.RewriteUrl = Convert.ToString(filteredRows[0]["rewriteurl"]);

                            obj.Status = Convert.ToString(filteredRows[0]["Status"]);
                            obj.PcatId = Convert.ToInt16(filteredRows[0]["brandid"]);
                            var dt = await _products.GetBrandList();
                            var cat = dt.AsEnumerable()
                                        .Select(r => new clsProduct
                                        {
                                            id = r.Field<int>("brandid"),
                                            title = r.Field<string>("brandtitle")
                                        })
                                        .ToList();
                            ViewBag.Category = new SelectList(cat, "id", "title", obj.PcatId);

                            var brand = await _products.GetModelList();
                            ViewBag.ModelList = brand;

                            ViewBag.CreateUpdate = "Update";

                            return View("~/Views/backoffice/products/addmodel.cshtml", obj);

                        }

                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addmodel", "Products");
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
            return RedirectToAction("addmodel", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/modelDelete/{id}")]
        public async Task<IActionResult> modelDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.ModelDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("addmodel", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addmodel", "Products");
        }

        // TYre TYpe

        [HttpGet]
        [Route("backoffice/Products/addtyretype")]
        public async Task<IActionResult> addtyretype()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetTyreTypeList();
            ViewBag.Brand = brand;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addtyretype.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/addtyretype")]
        public async Task<IActionResult> addtyretype(clsProduct obj)
        {
            clsProduct objcls = new clsProduct();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetTyreTypeList();
            ViewBag.Brand = brand;
            // Insert data

            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.DisplayOrder = obj.DisplayOrder;
                objcls.Status = obj.Status ?? "False";
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.id == 0)
                {
                    objcls.Mode = 1;
                }
                else
                {
                    objcls.Mode = 2;
                }
                int x = await _products.AddTYre_Type(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addtyretype.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Products/tyretypestatus/{id}")]
        public async Task<IActionResult> tyretypestatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetTyreTypeList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("tyre_typeid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.TyreTypeUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("addtyretype", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addtyretype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/tyretypeedit/{id}")]
        public async Task<IActionResult> tyretypeedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsProduct objcls = new clsProduct();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _products.GetTyreTypeList();
                        ViewBag.Brand = brand;

                        var x = _products.GetTyreTypeList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("tyre_typeid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.id = Convert.ToInt16(filterresult[0]["tyre_typeid"]);
                                objcls.title = Convert.ToString(filterresult[0]["tyre_typetitle"]);
                                objcls.RewriteUrl = Convert.ToString(filterresult[0]["rewriteurl"]);
                                objcls.Status = Convert.ToString(filterresult[0]["Status"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/products/addtyretype.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addtyretype", "Products");
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
            return RedirectToAction("addtyretype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/tyretypeDelete/{id}")]
        public async Task<IActionResult> tyretypeDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.TyretypeDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("addtyretype", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addtyretype", "Products");
        }

        // Position

        [HttpGet]
        [Route("backoffice/Products/addposition")]
        public async Task<IActionResult> addposition()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetPositionList();
            ViewBag.Brand = brand;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addposition.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/addposition")]
        public async Task<IActionResult> addposition(clsProduct obj)
        {
            clsProduct objcls = new clsProduct();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetPositionList();
            ViewBag.Brand = brand;
            // Insert data

            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.DisplayOrder = obj.DisplayOrder;
                objcls.Status = obj.Status ?? "False";
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.id == 0)
                {
                    objcls.Mode = 1;
                }
                else
                {
                    objcls.Mode = 2;
                }
                objcls.PageMeta = obj.PageMeta ?? string.Empty;
                objcls.PageMetaDesc = obj.PageMetaDesc ?? string.Empty;
                objcls.PageTitle = obj.PageTitle ?? string.Empty;
                objcls.Canonical = obj.Canonical ?? string.Empty;
                int x = await _products.AddPosition(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }

                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/addposition.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Products/postingstatus/{id}")]
        public async Task<IActionResult> postingstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetPositionList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("postingid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.PositionUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("addposition", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addposition", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/postingedit/{id}")]
        public async Task<IActionResult> postingedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsProduct objcls = new clsProduct();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _products.GetPositionList();
                        ViewBag.Brand = brand;

                        var x = _products.GetPositionList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("postingid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.id = Convert.ToInt16(filterresult[0]["postingid"]);
                                objcls.title = Convert.ToString(filterresult[0]["postingtitle"]);
                                objcls.RewriteUrl = Convert.ToString(filterresult[0]["rewriteurl"]);
                                objcls.Status = Convert.ToString(filterresult[0]["Status"]);
                                objcls.PageMeta = Convert.ToString(filterresult[0]["pagemeta"]);
                                objcls.PageMetaDesc = Convert.ToString(filterresult[0]["pagemetadesc"]);
                                objcls.PageTitle = Convert.ToString(filterresult[0]["PageTitle"]);
                                //objcls.Canonical = Convert.ToString(filterresult[0]["Canonical"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/products/addposition.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addposition", "Products");
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
            return RedirectToAction("addposition", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/postingDelete/{id}")]
        public async Task<IActionResult> postingDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.PositionDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("addposition", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("addposition", "Products");
        }

        // Design Type

        [HttpGet]
        [Route("backoffice/Products/adddesignType")]
        public async Task<IActionResult> adddesignType()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetDesignTypeList();
            ViewBag.Brand = brand;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/adddesigntype.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/adddesignType")]
        public async Task<IActionResult> adddesignType(clsProduct obj)
        {
            clsProduct objcls = new clsProduct();
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetDesignTypeList();
            ViewBag.Brand = brand;
            // Insert data

            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.RewriteUrl = obj.RewriteUrl ?? string.Empty;
                objcls.DisplayOrder = obj.DisplayOrder;
                objcls.Status = obj.Status ?? "False";
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.id == 0)
                {
                    objcls.Mode = 1;
                }
                else
                {
                    objcls.Mode = 2;
                }
                int x = await _products.AdddesignType(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/adddesigntype.cshtml");
        }
        [HttpGet]
        [Route("backoffice/Products/designTypeStatus/{id}")]
        public async Task<IActionResult> designTypeStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetDesignTypeList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("designtypeid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.DesignTypeUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("adddesigntype", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("adddesigntype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/designTypeedit/{id}")]
        public async Task<IActionResult> designTypeedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsProduct objcls = new clsProduct();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _products.GetDesignTypeList();
                        ViewBag.Brand = brand;

                        var x = _products.GetDesignTypeList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("designtypeid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.id = Convert.ToInt16(filterresult[0]["designtypeid"]);
                                objcls.title = Convert.ToString(filterresult[0]["designtypetitle"]);
                                objcls.RewriteUrl = Convert.ToString(filterresult[0]["rewriteurl"]);
                                objcls.Status = Convert.ToString(filterresult[0]["Status"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/products/adddesigntype.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("adddesigntype", "Products");
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
            return RedirectToAction("adddesigntype", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/designTypeDelete/{id}")]
        public async Task<IActionResult> designTypeDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.DesignTypeDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("adddesigntype", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("adddesigntype", "Products");
        }

        // Dealer Showroom
        [HttpGet]
        [Route("backoffice/Products/adddealer")]
        public async Task<IActionResult> adddealer()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetDealerList();
            ViewBag.Brand = brand;

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/adddealer.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/adddealer")]
        public async Task<IActionResult> adddealer(clsProduct obj)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var brand = await _products.GetDealerList();
            ViewBag.Brand = brand;

            clsProduct objcls = new clsProduct();
            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.id = obj.id;
                objcls.title = obj.title;
                objcls.branch = obj.branch;
                objcls.city = obj.city;
                objcls.telephone = obj.telephone;
                objcls.postal_code = obj.postal_code;
                objcls.street = obj.street;
                objcls.email = obj.email;
                objcls.Status = obj.Status ?? "False";
                if (obj.id > 0)
                {
                    objcls.Mode = 2;
                }
                else
                {
                    objcls.Mode = 1;
                }
                objcls.Uname = HttpContext.Session.GetString("UserName");
                objcls.DisplayOrder = obj.DisplayOrder;
                int x = await _products.AddDealer(objcls);
                if (x > 0)
                {
                    if (objcls.id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }

                }
            }

            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/Products/adddealer.cshtml");
        }
        [HttpPost]
        [Route("backoffice/Products/DealerStatus/{id}")]
        public async Task<IActionResult> DealerStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsProduct obj = new clsProduct();
                        // Get the Media list by ID
                        var productTypes = await _products.GetDealerList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("dsid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.Status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _products.DealerUpdateStatus(obj.Status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("adddealer", "Products");
                            }
                        }

                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("adddealer", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/Dealeredit/{id}")]
        public async Task<IActionResult> Dealeredit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsProduct objcls = new clsProduct();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _products.GetDealerList();
                        ViewBag.Brand = brand;

                        var x = _products.GetDealerList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("dsid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.id = Convert.ToInt16(filterresult[0]["dsid"]);
                                objcls.title = Convert.ToString(filterresult[0]["Name"]);
                                objcls.branch = Convert.ToString(filterresult[0]["branch"]);
                                objcls.city = Convert.ToString(filterresult[0]["city"]);
                                objcls.telephone = Convert.ToString(filterresult[0]["telephone"]);
                                objcls.postal_code = Convert.ToString(filterresult[0]["postal_code"]);
                                objcls.street = Convert.ToString(filterresult[0]["street"]);
                                objcls.email = Convert.ToString(filterresult[0]["email"]);
                                objcls.Status = Convert.ToString(filterresult[0]["Status"]);
                                objcls.DisplayOrder = Convert.ToInt16(filterresult[0]["DisplayOrder"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/products/adddealer.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("adddealer", "Products");
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
            return RedirectToAction("adddealer", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/DealereDelete/{id}")]
        public async Task<IActionResult> DealerDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _products.DealerDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("adddealer", "Products");
                        }


                    }
                    catch (Exception ex)
                    {

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
            return RedirectToAction("adddealer", "Products");
        }

        //Product Tyre
        [HttpGet]
        [Route("backoffice/Products/addproductstyre")]
        public async Task<IActionResult> addproductstyre()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var pType = await _products.GetProductTypeDropDown();
            ViewBag.PTypeDropdown = new SelectList(pType, "Key", "Value");

            ViewBag.VTypeDropdown = new SelectList(Enumerable.Empty<SelectListItem>());

            var tType = await _products.GetTyreTypeDropDown();
            ViewBag.TTypeDropdown = new SelectList(tType, "Key", "Value");

            var position = await _products.GetPositionDropDown();
            ViewBag.PositionDropdown = new SelectList(position, "Key", "Value");

            var dType = await _products.GetDesignTypeDropDown();
            ViewBag.DesignTypeDropdown = new SelectList(dType, "Key", "Value");
            ViewBag.Button = "Save";
            return View("~/Views/backoffice/Products/addproductstyre.cshtml");
        }

        [HttpPost]
        [Route("backoffice/Products/ChangeProductTyreStatus/{id}")]
        public async Task<IActionResult> ChangeTyreStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _products.ChangeTyreStatus(id);
                    if (chngstatus > 0)
                    {
                        TempData["SuccessMessage"] = "Status changed successfully.";
                        TempData["Title"] = "Product";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to change the product status.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Product id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("viewproducttyres", "Products");
        }

        [HttpGet]
        public async Task<IActionResult> VehicleTypeDropdown(int productTypeId)
        {
            var vType = await _products.GetVehicleTypeDropDown(productTypeId);
            var result = vType.Select(x => new { value = x.Key, text = x.Value }).ToList();
            return Json(result);
        }

        [HttpPost]
        [Route("backoffice/Products/addeditproductstyre")]
        public async Task<IActionResult> addeditproductstyre(ProductTyre t, IFormFile UploadThumbnailImage, IFormFile UploadImg, IFormFile UploadFileImg, IFormFile UploadDetailImg)
        {
            if (t.ProductTyreId == 0)
            {
                t.Mode = 1;
                t.Status = true;
                t.UName = HttpContext.Session.GetString("UserName");
            }
            else if (t.ProductTyreId > 0)
            {
                t.Mode = 2;
                t.Status = true;
                t.UName = HttpContext.Session.GetString("UserName");
            }
            if (UploadThumbnailImage != null && UploadThumbnailImage.Length > 0)
            {
                var FileName = Path.GetFileName(UploadThumbnailImage.FileName);
                var uniqueName = $"{Guid.NewGuid()}{FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadThumbnailImage.CopyToAsync(stream);
                }
                t.ThumbnailImage = "/uploads/ProductImages/" + uniqueName;
            }
            if (UploadImg != null && UploadImg.Length > 0)
            {
                var FileName = Path.GetFileName(UploadImg.FileName);
                var uniqueName = $"{Guid.NewGuid()}{FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadImg.CopyToAsync(stream);
                }
                t.UploadImage = "/uploads/ProductImages/" + uniqueName;
            }
            if (UploadFileImg != null && UploadFileImg.Length > 0)
            {
                var FileName = Path.GetFileName(UploadFileImg.FileName);
                var uniqueName = $"{Guid.NewGuid()}{FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadFileImg.CopyToAsync(stream);
                }
                t.UploadFile = "/uploads/ProductImages/" + uniqueName;
            }
            if (UploadDetailImg != null && UploadDetailImg.Length > 0)
            {
                var FileName = Path.GetFileName(UploadDetailImg.FileName);
                var uniqueName = $"{Guid.NewGuid()}{FileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/ProductImages", uniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadDetailImg.CopyToAsync(stream);
                }
                t.DetailImage = "/uploads/ProductImages/" + uniqueName;
            }
            var res = await _products.AddEditProductTyre(t);
            if (res > 0)
            {
                ViewBag.SuccessCreate = "Added successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Not added something went wrong.";
            }
            return RedirectToAction("viewproducttyres", "Products");
        }

        [HttpGet]
        [Route("backoffice/Products/GetPTyreByID/{id}")]
        public async Task<IActionResult> GetPTyreByID(int id)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var pTyredata = await _products.GetProductTyrebyID(id);

            if (pTyredata == null)
            {
                return View("~/Views/backoffice/Products/addproductstyre.cshtml", pTyredata);
            }

            var pType = await _products.GetProductTypeDropDown();
            ViewBag.PTypeDropdown = new SelectList(pType, "Key", "Value", pTyredata.ProductTypeId);

            var vType = await _products.GetVehicleTypeDropDown(0);
            ViewBag.VTypeDropdown = new SelectList(vType, "Key", "Value", pTyredata.VehicleTypeId);

            var tType = await _products.GetTyreTypeDropDown();
            ViewBag.TTypeDropdown = new SelectList(tType, "Key", "Value", pTyredata.TyreTypeId);

            var position = await _products.GetPositionDropDown();
            ViewBag.PositionDropdown = new SelectList(position, "Key", "Value", pTyredata.PostingId);

            var dType = await _products.GetDesignTypeDropDown();
            ViewBag.DesignTypeDropdown = new SelectList(dType, "Key", "Value", pTyredata.DesignType);
            ViewBag.Button = "Update";
            return View("~/Views/backoffice/Products/addproductstyre.cshtml", pTyredata);

        }

        [HttpGet]
        [Route("backoffice/Products/DeleteTyreByID/{id}")]
        public async Task<IActionResult> DeleteTyreByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var deletedProductId = await _products.DeleteTyreByID(id);
                    if (deletedProductId > 0)
                    {
                        TempData["SuccessMessage"] = "Product tyre deleted successfully.";
                        TempData["ErrorMessage"] = "Product";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the product tyre.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Product tyre id is necessary.";
                }
                return RedirectToAction("viewproducttyres", "Products");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("viewproducttyres", "Products");
            }
        }

        [HttpGet]
        [Route("backoffice/Products/viewproducttyres")]
        public async Task<IActionResult> viewproducttyres()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var productTyre = await _products.GetProductTyreList();
            ViewBag.TyreList = productTyre;
            return View("~/Views/backoffice/Products/viewproducttyres.cshtml");
        }

        [HttpGet]
        [Route("backoffice/Products/GetProductSize")]
        public async Task<IActionResult> GetProductSize(int id)
        {
            var data = await _products.GetProductSize(id);
            return PartialView("_AddProductSizePopup", data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductSize([FromBody] List<BrandModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    var mapping = new MappingDetail
                    {
                        ProjId = model.ProjId,
                        BrandId = model.BrandId,
                        ModelId = model.modelID,
                        BlogoId = model.BlogoId,
                        UName = "admin",
                        DesignTypeId = 1
                    };
                    var data = await _products.UpdateProductSize(mapping);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        [Route("backoffice/Products/GetTyreImage")]
        public async Task<IActionResult> GetTyreImage(int id)
        {
            var tyreImage = await _products.GetTyreImage(id);
            ViewBag.ProductTyreId = id;
            return PartialView("_TyreImage", tyreImage);
        }

        [HttpPost]
        public async Task<IActionResult> UploadTyreImage(IFormFile ImageFile, int ProductTyreId)
        {
            try
            {
                var m = new UTyrePhoto();
                if (ImageFile == null || ImageFile.Length == 0)
                    return Json(new { success = false, message = "No file received" });

                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/ProductImages");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);
                m.PhotoID = 0;
                m.ProductTyreID = ProductTyreId;
                m.PhotoTitle = ImageFile.Name;
                m.Status = true;
                m.UploadPhoto = fileName;
                m.Uname = HttpContext.Session.GetString("UserName");
                m.DisplayOrder = 0;
                m.Mode = 1;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                var res = await _products.UploadTyreImage(m);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMultipleData([FromBody] List<UTyrePhoto> m)
        {

            foreach (var item in m)
            {
                var t = new UTyrePhoto();
                t.PhotoID = item.PhotoID;
                t.ProductTyreID = item.ProductTyreID;
                t.PhotoTitle = item.PhotoTitle;
                t.Status = item.Status;
                t.UploadPhoto = item.UploadPhoto;
                t.Uname = HttpContext.Session.GetString("UserName");
                t.DisplayOrder = item.DisplayOrder;
                t.SizeID = item.SizeID;
                t.LargeImage = item.LargeImage;
                t.Mode = 2;
                var res = await _products.UploadMultipleTyreImage(t);
            }
            return Json(new { success = true });
        }
        [HttpGet]
        [Route("backoffice/products/aboutproduct/{name}/{pageid}")]
        public async Task<IActionResult> aboutproduct(string name, int pageid)
        {   
            clsCategory objcls = new clsCategory();
            try
            {   
                var menuList = _menuService.GetMenu(pageid);
                ViewBag.Menus = menuList;

                //Get Data
                var x = await _products.GetAboutProduct();
                if (x != null)
                {
                    var filterresult = x.AsEnumerable().Where(p => p.Field<int>("productid") == pageid).ToList();
                    if (filterresult.Count > 0)
                    {
                        objcls.PcatId = Convert.ToInt16(filterresult[0]["mid"]);
                        objcls.productid = Convert.ToString(filterresult[0]["productid"]);
                        objcls.ShortDetail = WebUtility.HtmlDecode(Convert.ToString(filterresult[0]["details"]));
                        ViewBag.CreateUpdate = "Update";
                    }
                    else
                    {
                        ViewBag.CreateUpdate = "Save";
                    }
                }
                //Get Name from route id
                var prodDtl = await _products.BindProductSolution();
                var filteredProdDtl = prodDtl.AsEnumerable()
                    .Where(p => p.Field<int>("productid") == pageid)
                    .OrderByDescending(row => row.Field<int>("productid"))
                    .CopyToDataTable();
                if (filteredProdDtl != null && filteredProdDtl.Rows.Count > 0)
                {
                    DataRow[] activeProducts = filteredProdDtl.Select("Status = 1");
                    objcls.shortname = Convert.ToString(activeProducts[0]["ProductName"]);

                }
                ViewBag._type = char.ToUpper(name[0]) + name.Substring(1).ToLower();

                return View("~/Views/backoffice/products/aboutproduct.cshtml", objcls);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/products/aboutproduct.cshtml");
            }
            return View("~/Views/backoffice/products/aboutproduct.cshtml", objcls);
        }


        [HttpGet]
        [Route("backoffice/products/addcapabilities/{name}/{pageid}")]
        public async Task<IActionResult> addcapabilities(string name, int pageid)
        {

            clsCategory objcls = new clsCategory();
            try
            {   
                var menuList = _menuService.GetMenu(pageid);
                ViewBag.Menus = menuList;
                //Get Data
                var x = await _products.GetProductCapabilities();
                if (x != null)
                {
                    var filterresult = x.AsEnumerable().Where(p => p.Field<int>("productid") == pageid).ToList();
                    if (filterresult.Count > 0)
                    {
                        objcls.PcatId = Convert.ToInt16(filterresult[0]["mid"]);
                        objcls.productid = Convert.ToString(filterresult[0]["productid"]);
                        objcls.ShortDetail = WebUtility.HtmlDecode(Convert.ToString(filterresult[0]["details"]));
                        ViewBag.CreateUpdate = "Update";
                    }
                    else
                    {
                        ViewBag.CreateUpdate = "Save";
                    }
                }
                //Get Name from route id
                var prodDtl = await _products.BindProductSolution();
                var filteredProdDtl = prodDtl.AsEnumerable()
                    .Where(p => p.Field<int>("productid") == pageid)
                    .OrderByDescending(row => row.Field<int>("productid"))
                    .CopyToDataTable();
                if (filteredProdDtl != null && filteredProdDtl.Rows.Count > 0)
                {
                    DataRow[] activeProducts = filteredProdDtl.Select("Status = 1");
                    objcls.shortname = Convert.ToString(activeProducts[0]["ProductName"]);

                }
                ViewBag._type = char.ToUpper(name[0]) + name.Substring(1).ToLower();

                return View("~/Views/backoffice/products/addcapabilities.cshtml", objcls);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                ViewBag.ErrorMessage = "An error occurred while loading the media section. Please try again later.";
                return View("~/Views/backoffice/products/addcapabilities.cshtml");
            }
            return View("~/Views/backoffice/products/addcapabilities.cshtml", objcls);
        }

        [HttpPost]
        [Route("backoffice/products/aboutproduct/{name}/{id}")]
        public async Task<IActionResult> aboutproduct(int id, clsCategory obj)
        {
            clsCategory objcls = new clsCategory();
            if (!string.IsNullOrEmpty(obj.ShortDetail))
            {
                obj.PcatId = obj.PcatId;
                objcls.productid = Convert.ToString(id);
                objcls.ShortDetail = obj.ShortDetail;
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.productid != null && Convert.ToInt32(objcls.productid) > 0)
                {
                    objcls.Mode = 2;
                }
                else
                {
                    objcls.Mode = 1;
                }
                int x = await _products.AddAboutProducts(objcls);
                if (x > 0)
                {
                    if (objcls.Mode == 1)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }
            return RedirectToAction("aboutproduct", new { id = id });

        }

        [HttpPost]
        [Route("backoffice/products/addcapabilities/{name}/{id}")]
        public async Task<IActionResult> addcapabilities(int id, clsCategory obj)
        {
            clsCategory objcls = new clsCategory();
            if (!string.IsNullOrEmpty(obj.ShortDetail))
            {
                objcls.PcatId = obj.PcatId;
                objcls.productid = Convert.ToString(id);
                objcls.ShortDetail = obj.ShortDetail;
                objcls.Uname = HttpContext.Session.GetString("UserName");
                if (objcls.productid != null && Convert.ToInt32(objcls.productid) > 0)
                {
                    objcls.Mode = 2;
                }
                else
                {
                    objcls.Mode = 1;
                }
                int x = await _products.AddProductsCapabilities(objcls);
                if (x > 0)
                {
                    if (objcls.Mode == 1)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
            }
            return RedirectToAction("addcapabilities", new { id = id });

        }
    }
}

