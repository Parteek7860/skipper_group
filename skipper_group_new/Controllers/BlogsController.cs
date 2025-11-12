using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlog _blog;
        private readonly clsMainMenuList _menuService;

        public BlogsController(clsMainMenuList menuService, IBlog blog)
        {
            _blog = blog;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("backoffice/Blogs/add-blogs")]
        public async Task<IActionResult> addblogs()
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                ViewBag.CreateUpdate = "Save";
                return View("~/Views/backoffice/Blogs/add-blogs.cshtml", new clsBlog());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the page.";
                return RedirectToAction("viewblog", "Blogs");
            }
        }

        [HttpGet]
        [Route("backoffice/Blogs/view-blogs")]
        public async Task<IActionResult> viewblog()
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;

                var blogDtl = await _blog.GetBlogTblData();
                ViewBag.BlogDtl = blogDtl;

                if (TempData["SuccessCreate"] != null)
                    ViewBag.SuccessCreate = TempData["SuccessCreate"];
                if (TempData["ErrorMessage"] != null)
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View("~/Views/backoffice/Blogs/view-blogs.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading blogs.";
                return View("~/Views/backoffice/Blogs/view-blogs.cshtml");
            }
        }

        [HttpPost]
        [Route("backoffice/Blogs/AddBlog")]
        public async Task<IActionResult> AddBlog(clsBlog blg, IFormFile BlogImageFile, IFormFile LargeImageFile)
        {
            try
            {
                if (blg != null && blg.BlogId != 0 && blg.Mode != 0)
                {
                    if (BlogImageFile != null && BlogImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(BlogImageFile.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/vedio", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            BlogImageFile.CopyTo(stream);
                        }

                        blg.BlogImage = fileName;
                    }
                    else
                    {
                        blg.BlogImage = blg.BlogImage;
                    }

                    if (LargeImageFile != null && LargeImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(LargeImageFile.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/vedio", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            LargeImageFile.CopyTo(stream);
                        }

                        blg.LargeImage = fileName;
                    }
                    else
                    {
                        blg.LargeImage = blg.LargeImage;
                    }

                    int result = await _blog.AddBlog(blg);
                    if (result > 0)
                    {
                        if (blg.BlogId > 0)
                        {
                            HttpContext.Session.SetString("Message", "Blog Update successfully.");
                            return RedirectToAction("viewblog", "Blogs");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Message", "Blog Added successfully.");
                            return RedirectToAction("addblog", "Blogs");
                        }


                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to save the blog.";
                        return RedirectToAction("viewblog", "Blogs");
                    }
                }
                else
                {
                    var menuList = _menuService.GetMenu();
                    ViewBag.Menus = menuList;
                    HttpContext.Session.SetString("Message", "Fill blogs required fields");
                    return View("~/Views/backoffice/blogs/add-blogs.cshtml", blg);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the blog: " + ex.Message;
                return RedirectToAction("viewblog", "Blogs");
            }
        }

        [HttpGet]
        [Route("backoffice/Blogs/GetBlogByID/{id}")]
        public async Task<IActionResult> GetBlogByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var blg = await _blog.EditBlog(id);
                    if (blg != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        blg.SmallDesc = WebUtility.HtmlDecode(blg.SmallDesc);
                        blg.LongDesc = WebUtility.HtmlDecode(blg.LongDesc);
                        blg.BlogImage = blg.BlogImage;
                        ViewBag.CreateUpdate = "Update";
                        return View("~/Views/backoffice/Blogs/add-blogs.cshtml", blg);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Blog not found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid blog ID.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the blog.";
            }
            return RedirectToAction("viewblog", "Blogs");
        }

        [HttpGet]
        [Route("backoffice/Blogs/DeleteBlog/{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                if (id > 0)
                {
                    var delResult = await _blog.DeleteBlog(id);
                    if (delResult > 0)
                    {
                        TempData["SuccessCreate"] = "Blog deleted successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the blog.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Blog id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the blog.";
            }
            return RedirectToAction("viewblog", "Blogs");
        }

        [HttpPost]
        [Route("backoffice/Blogs/ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var catdtl = await _blog.GetBlogTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blogs");
                worksheet.Cell(1, 1).Value = "Title";
                worksheet.Cell(1, 2).Value = "Description";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.BlogTitle;
                    worksheet.Cell(row, 2).Value = c.SmallDesc;
                    worksheet.Cell(row, 3).Value = c.BlogDate;
                    worksheet.Cell(row, 3).Style.DateFormat.Format = "yyyy-MM-dd";
                    worksheet.Cell(row, 5).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"BlogList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/Blogs/ChangeBlogStatus/{id}")]
        public async Task<IActionResult> ChangeBlogStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _blog.ChangeBlogStatus(id);
                    if (chngstatus > 0)
                    {
                        ViewBag.SuccessCreate = "Status changed successfully.";
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
            return RedirectToAction("viewblog", "Blogs");
        }


        [HttpGet]
        [Route("backoffice/Blogs/addblogcat")]
        public async Task<IActionResult> addblogcat()
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                var blogDtl = await _blog.GetBlogCatTblData();
                ViewBag.BlogCatDtl = blogDtl;

                if (TempData["SuccessCreate"] != null)
                    ViewBag.SuccessCreate = TempData["SuccessCreate"];
                if (TempData["ErrorMessage"] != null)
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View("~/Views/backoffice/Blogs/addblogcat.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading blogs.";
                return View("~/Views/backoffice/Blogs/addblogcat.cshtml");
            }
        }

        [HttpPost]
        [Route("backoffice/Blogs/AddBlogCat")]
        public async Task<IActionResult> AddBlogCat(clsBlogCategory blg)
        {
            try
            {
                if (blg != null)
                {
                    blg.Uname = HttpContext.Session.GetString("UserName");
                    int result = await _blog.AddBlogCat(blg);
                    if (result > 0)
                    {
                        if (blg.BcatId > 0)
                        {
                            HttpContext.Session.SetString("Message", "Blog category Update successfully.");
                        }

                        else
                        {
                            HttpContext.Session.SetString("Message", "Blog category Added successfully.");
                        }
                        return RedirectToAction("addblogcat", "Blogs");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to save the blog category.";
                        return RedirectToAction("addblogcat", "Blogs");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid blog category data.";
                    return RedirectToAction("addblogcat", "Blogs");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the blog category: " + ex.Message;
                return RedirectToAction("addblogcat", "Blogs");
            }
        }

        [HttpGet]
        [Route("backoffice/Blogs/GetBlogCatByID/{id}")]
        public async Task<IActionResult> GetBlogCatByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var blg = await _blog.EditBlogCat(id);
                    if (blg != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;

                        return View("~/Views/backoffice/Blogs/addblogcat.cshtml", blg);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Blog not found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid blog ID.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the blog category.";
            }
            return RedirectToAction("addblogcat", "Blogs");
        }

        [HttpGet]
        [Route("backoffice/Blogs/DeleteBlogCat/{id}")]
        public async Task<IActionResult> DeleteBlogCat(int id)
        {
            try
            {
                if (id > 0)
                {
                    var delResult = await _blog.DeleteBlogCat(id);
                    if (delResult > 0)
                    {
                        TempData["SuccessCreate"] = "Blog category deleted successfully.";
                        return RedirectToAction("addblogcat", "Blogs");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the blog category.";
                        return RedirectToAction("addblogcat", "Blogs");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Blog category id is necessary.";
                    return RedirectToAction("addblogcat", "Blogs");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the blog category.";
            }
            return RedirectToAction("addblogcat", "Blogs");
        }

        [HttpPost]
        [Route("backoffice/Blogs/ExportBlogCatToExcel")]
        public async Task<IActionResult> ExportBlogCatToExcel()
        {
            var catdtl = await _blog.GetBlogCatTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blogs Category");
                worksheet.Cell(1, 1).Value = "Title";
                worksheet.Cell(1, 2).Value = "Date";
                worksheet.Cell(1, 3).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.BcatTitle;
                    worksheet.Cell(row, 2).Value = c.TrDate;
                    worksheet.Cell(row, 2).Style.DateFormat.Format = "yyyy-MM-dd";
                    worksheet.Cell(row, 3).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"BlogCatList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/Blogs/ChangeBlogCatStatus/{id}")]
        public async Task<IActionResult> ChangeBlogCatStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _blog.ChangeBlogCatStatus(id);
                    if (chngstatus > 0)
                    {
                        TempData["SuccessMessage"] = "Status changed successfully.";
                        TempData["Title"] = "Product";
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
            }
            return RedirectToAction("addblogcat", "Blogs");
        }
    }
}
