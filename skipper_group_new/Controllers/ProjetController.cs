using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;

using System.Linq;
using System.Net;

namespace skipper_group_new.Controllers
{
    //Rakesh Chauhan - 14/11/2025 - Backoffice Project Controller Created
    public class ProjetController : Controller
    {
        private readonly clsMainMenuList _menuService;
        private readonly IBacofficeProject _project;

        public ProjetController(IBacofficeProject Project, clsMainMenuList menuService)
        {
            _project = Project;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("backoffice/project/addproject")]
        public async Task<IActionResult> GetProject()
        {
            var cat = new ResearchModel();
            DataTable product = await _project.GetProduct();
            DataTable category = await _project.GetCategory();
            if (product != null && product.Rows.Count > 0)
            {
                cat.selectProduct = product.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["productid"]?.ToString() ?? string.Empty,
                        Text = row["productname"]?.ToString() ?? string.Empty
                    })
                    .ToList();
            }
            else
            {
                cat.selectProduct = new List<SelectListItem>();
            }
            if (category != null && category.Rows.Count > 0)
            {
                cat.selectCategory = category.AsEnumerable()
                    .Select(row => new SelectListItem
                    {
                        Value = row["pcatid"]?.ToString() ?? string.Empty,
                        Text = row["category"]?.ToString() ?? string.Empty
                    })
                    .ToList();
            }
            else
            {
                cat.selectCategory = new List<SelectListItem>();
            }
            ViewBag.Menus = _menuService.GetMenu();
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/project/project.cshtml", cat);
        }

        [HttpPost]
        [Route("backoffice/project/addProject")]
        public async Task<IActionResult> AddProject(ResearchModel research, IFormFile file_Uploader, IFormFile file_Uploader1, IFormFile file_Uploader2, IFormFile file_Uploader3)
        {
            HttpContext.Session.Remove("Message");
            ViewBag.Menus = _menuService.GetMenu();
            ModelState.Remove("Archive");
            ModelState.Remove("Canonical");
            ModelState.Remove("ColorCode");
            ModelState.Remove("HomeImage");
            ModelState.Remove("NoIndexFollow");
            ModelState.Remove("LCID");
            ModelState.Remove("LargeImage");
            ModelState.Remove("OtherSchema");
            ModelState.Remove("PageMeta");
            ModelState.Remove("PageMetaDesc");
            ModelState.Remove("PageTitle");
            ModelState.Remove("ResearchSDate");
            ModelState.Remove("RewriteUrl");
            ModelState.Remove("ShowOnGroup");
            ModelState.Remove("ShowOnSchool");
            ModelState.Remove("UName");
            ModelState.Remove("UploadEvents");
            ModelState.Remove("UploadFile");
            ModelState.Remove("VeryLargeImage");
            ModelState.Remove("YoutubeUrl");
            ModelState.Remove("Status");
            ModelState.Remove("ResearchId");
            ModelState.Remove("Mode");
            ModelState.Remove("TRDate");
            ModelState.Remove("file_Uploader");
            ModelState.Remove("file_Uploader1");
            ModelState.Remove("file_Uploader2");
            ModelState.Remove("file_Uploader3");
            
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("Message", "Please fill in all required fields.");
                return RedirectToAction("GetProject", "Projet");
            }

            try
            {
                research.Mode = research.ResearchId > 0 ? 2 : 1;
                research.Status = true;
                research.UName = HttpContext.Session.GetString("UserName");
                research.ShowOnSchool = true;
                research.ShowOnGroup = true;
                research.Archive = true;
                research.ShowOnHome = true;
                if (file_Uploader != null && file_Uploader.Length > 0)
                {
                    var fileName = Path.GetFileName(file_Uploader.FileName);
                    var filePath = Path.Combine("wwwroot/uploads/SmallImages", fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file_Uploader.CopyToAsync(stream);

                    research.UploadEvents = fileName;
                }
                else
                {
                    research.UploadEvents = research.UploadEvents;
                }
                if (file_Uploader1 != null && file_Uploader1.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file_Uploader1.FileName);
                    var filePath1 = Path.Combine("wwwroot/uploads/LargeImages", fileName1);
                    using var stream = new FileStream(filePath1, FileMode.Create);
                    await file_Uploader1.CopyToAsync(stream);
                    research.LargeImage = fileName1;
                }
                else
                {
                    research.LargeImage = research.LargeImage;
                }
                if (file_Uploader2 != null && file_Uploader2.Length > 0)
                {
                    var fileName2 = Path.GetFileName(file_Uploader2.FileName);
                    var filePath2 = Path.Combine("wwwroot/uploads/SmallImages", fileName2);
                    using var stream = new FileStream(filePath2, FileMode.Create);
                    await file_Uploader2.CopyToAsync(stream);
                    research.HomeImage = fileName2;
                }
                else
                {
                    research.HomeImage = research.HomeImage;
                }
                if (file_Uploader3 != null && file_Uploader3.Length > 0)
                {
                    var fileName3 = Path.GetFileName(file_Uploader3.FileName);
                    var filePath3 = Path.Combine("wwwroot/uploads/LargeImages", fileName3);

                    using var stream = new FileStream(filePath3, FileMode.Create);
                    await file_Uploader3.CopyToAsync(stream);

                    research.VeryLargeImage = fileName3;
                }
                else
                {
                    research.VeryLargeImage = research.VeryLargeImage;
                }
                var newId = await _project.AddUpdateProject(research);
                if (newId > 0)
                {
                    if(research.ResearchId == 0)
                    {
                        HttpContext.Session.SetString("Message", "Project added successfully.");
                        return RedirectToAction("GetProject", "Projet");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", "Project updated successfully.");
                        return RedirectToAction("ViewProject", "Projet");
                    }
                }
                HttpContext.Session.SetString("Message", "Failed to save project.");
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("GetProject", "Projet");
        }

        [HttpGet]
        [Route("backoffice/project/viewproject")]
        public async Task<IActionResult> ViewProject()
        {
            var cat = new ResearchModel();
            ViewBag.Menus = _menuService.GetMenu();
            var content = await _project.GetProjectData();
            if (content != null)
            {
                ViewBag.ProjectList = content;
            }
            return View("~/Views/backoffice/project/viewproject.cshtml", cat);
        }

        [HttpGet]
        [Route("backoffice/project/editProject/{id}")]
        public async Task<IActionResult> GetProjectByID(int id)
        {
            try
            {
                var x = await _project.GetProjectData();
                var rows = x.AsEnumerable()
                            .Where(r => r.Field<int>("researchid") == id)
                            .ToList();

                if (!rows.Any())
                {
                    HttpContext.Session.SetString("Message", "Project not found.");
                    return RedirectToAction("GetProject", "Projet");
                }

                var row = rows.First();

                var research = new ResearchModel
                {
                    ResearchId = row.Field<int>("researchid"),
                    NTypeId = row.Field<int>("ntypeid"),
                    CatId = row.Field<string>("catid"),
                    ResearchTitle = row.Field<string>("researchtitle"),
                    Tagline = row.Field<string>("tagline"),
                    Location = row.Field<string>("location"),
                    Types = row.Field<string>("types"),
                    ResearchEDate = row.Field<DateTime?>("researchedate"),
                    ResearchSDate = row.Field<DateTime?>("researchsdate"),
                    ShortDesc = WebUtility.HtmlDecode(row.Field<string>("shortdesc")??""),
                    ResearchDesc = WebUtility.HtmlDecode(row.Field<string>("researchdesc")??""),
                    UploadEvents = row.Field<string>("uploadevents"),
                    LargeImage = row.Field<string>("largeimage"),
                    HomeImage = row.Field<string>("homeimage"),
                    VeryLargeImage = row.Field<string>("verylargeimage"),
                    DisplayOrder = row.Field<int?>("displayorder"),
                    PageTitle = row.Field<string>("pagetitle"),
                    PageMeta = row.Field<string>("pagemeta"),
                    PageMetaDesc = row.Field<string>("pagemetadesc"),
                    OtherSchema = row.Field<string>("other_schema"),
                    Canonical = row.Field<string>("canonical"),
                    NoIndexFollow = row.Field<bool?>("no_indexfollow"),
                };


                var product = await _project.GetProduct();
                var category = await _project.GetCategory();
                if (product?.Rows.Count > 0)
                {
                    research.selectProduct = product.AsEnumerable()
                        .Select(r => new SelectListItem
                        {
                            Value = r["productid"].ToString(),
                            Text = r["productname"].ToString(),
                            Selected = (research.NTypeId.ToString() == r["productid"].ToString())
                        })
                        .ToList();
                }

                if (category?.Rows.Count > 0)
                {
                    research.selectCategory = category.AsEnumerable()
                        .Select(r => new SelectListItem
                        {
                            Value = r["pcatid"].ToString(),
                            Text = r["category"].ToString(),
                            Selected = (research.CatId == r["pcatid"].ToString())
                        })
                        .ToList();
                }

                ViewBag.Menus = _menuService.GetMenu();
                ViewBag.CreateUpdate = "Update";

                return View("~/Views/backoffice/project/project.cshtml", research);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
                return RedirectToAction("GetProject", "Projet");
            }
        }

        [HttpGet]
        [Route("backoffice/project/deleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                int Mode = 3;
                var result = await _project.ExecuteProjectAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Project deleted successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to delete project.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewProject", "Projet");

        }

        [HttpGet]
        [Route("backoffice/project/chngStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                int Mode = 8;
                var result = await _project.ExecuteProjectAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Changed successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to change status.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewProject", "Projet");

        }

        [HttpGet]
        [Route("backoffice/project/chngShowHome/{id}")]
        public async Task<IActionResult> ShowtoHome(int id)
        {
            try
            {
                int Mode = 9;
                var result = await _project.ExecuteProjectAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Changed successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to change project.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewProject", "Projet");

        }

        [HttpGet]
        [Route("backoffice/project/chngFeature/{id}")]
        public async Task<IActionResult> ChangeFeature(int id)
        {
            try
            {
                int Mode = 10;
                var result = await _project.ExecuteProjectAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Featured successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to feature project.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewProject", "Projet");
        }


        //RAKESH CHAUHAN 17/11/2025
        [HttpGet]
        [Route("backoffice/project/category")]
        public async Task<IActionResult> Category()
        {
            var cat = new clsCategory();
            ViewBag.Menus = _menuService.GetMenu();
            ViewBag.CreateUpdate = "Save";
            return View("~/Views/backoffice/project/category.cshtml", cat);
        }

        [HttpGet]
        [Route("backoffice/project/viewcategory")]
        public async Task<IActionResult> ViewCategory()
        {
            var cat = new clsCategory();
            ViewBag.Menus = _menuService.GetMenu();
            var content = await _project.GetCategory();
            if (content != null)
            {
                ViewBag.CategoryList = content;
            }
            return View("~/Views/backoffice/project/viewcategory.cshtml", cat);
        }

        [HttpPost]
        [Route("backoffice/project/addCategory")]
        public async Task<IActionResult> AddCategory(clsCategory m)
        {
            HttpContext.Session.Remove("Message");
            ViewBag.Menus = _menuService.GetMenu();
            ModelState.Remove("shortname");
            ModelState.Remove("Detail");
            ModelState.Remove("ShortDetail");
            ModelState.Remove("ShowOnHome");
            ModelState.Remove("Status");
            ModelState.Remove("Banner");
            ModelState.Remove("UploadAPDF");
            ModelState.Remove("productid");
            ModelState.Remove("PageTitle");
            ModelState.Remove("PageMeta");
            ModelState.Remove("PageMetaDesc");
            ModelState.Remove("RewriteUrl");
            ModelState.Remove("Canonical");
            ModelState.Remove("NoIndexFollow");
            ModelState.Remove("PageScript");
            ModelState.Remove("HomeImage");
            ModelState.Remove("HomeDesc");
            ModelState.Remove("Uname");
            ModelState.Remove("Mode");
            var errors = ModelState
   .Where(ms => ms.Value.Errors.Count > 0)
   .Select(ms => new { Key = ms.Key, Errors = ms.Value.Errors })
   .ToList();
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetString("Message", "Please fill in all required fields.");
                return RedirectToAction("Category", "Projet");
            }

            try
            {
                m.Mode = m.PcatId > 0 ? 2 : 1;
                m.Status = true;
                m.Uname = HttpContext.Session.GetString("UserName");
                m.ShowOnHome = false;
                var newId = await _project.AddUpdateCategory(m);
                if (newId > 0)
                {
                    if(m.PcatId == 0)
                    {
                        HttpContext.Session.SetString("Message", "Category added successfully.");
                        return RedirectToAction("Category", "Projet");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", "Category updated successfully.");
                        return RedirectToAction("ViewCategory", "Projet");
                    }   
                }
                HttpContext.Session.SetString("Message", "Failed to save project.");
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("Category", "Projet");
        }

        [HttpGet]
        [Route("backoffice/project/editCategory/{id}")]
        public async Task<IActionResult> GetCategoryByID(int id)
        {
            try
            {
                var x = await _project.GetCategory();
                var rows = x.AsEnumerable().Where(r => r.Field<int>("pcatid") == id).ToList();
                if (!rows.Any())
                {
                    HttpContext.Session.SetString("Message", "Category not found.");
                    return RedirectToAction("Category", "Projet");
                }
                var row = rows.First();
                var research = new clsCategory
                {
                    PcatId = row.Field<int>("pcatid"),
                    Category = row.Field<string>("category"),
                    Detail = WebUtility.HtmlDecode(row.Field<string>("detail")??""),
                    DisplayOrder = row.Field<int?>("displayorder"),
                    Mode = 2,
                    PageTitle = row.Field<string>("PageTitle"),
                    PageMeta = row.Field<string>("PageMeta"),
                    PageMetaDesc = row.Field<string>("PageMetaDesc"),
                    PageScript = row.Field<string>("pagescript"),
                    Canonical = row.Field<string>("canonical"),
                    NoIndexFollow = row.Field<bool>("no_indexfollow"),
                };
                ViewBag.Menus = _menuService.GetMenu();
                ViewBag.CreateUpdate = "Update";
                return View("~/Views/backoffice/project/category.cshtml", research);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
                return RedirectToAction("Category", "Projet");
            }
        }

        [HttpGet]
        [Route("backoffice/project/deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                int Mode = 3;
                var result = await _project.ExecuteCategoryAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Category deleted successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to delete category.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewCategory", "Projet");
        }

        [HttpGet]
        [Route("backoffice/project/chngCategoryStatus/{id}")]
        public async Task<IActionResult> ChangeCategoryStatus(int id)
        {
            try
            {
                int Mode = 5;
                var result = await _project.ExecuteCategoryAction(id, Mode);
                if (result > 0)
                {
                    HttpContext.Session.SetString("Message", "Status changed successfully.");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Failed to change category status.");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Message", "Unexpected Error: " + ex.Message);
            }
            return RedirectToAction("ViewCategory", "Projet");
        }        
    }
}
