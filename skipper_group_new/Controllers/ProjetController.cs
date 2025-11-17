using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using System.Data;
using System.Linq;

namespace skipper_group_new.Controllers
{
    //Rakesh Chauhan - 12/06/2024 - Backoffice Project Controller Created
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
            ModelState.Remove("ResearchEDate");
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
                if (file_Uploader1 != null && file_Uploader1.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file_Uploader1.FileName);
                    var filePath1 = Path.Combine("wwwroot/uploads/LargeImages", fileName1);
                    using var stream = new FileStream(filePath1, FileMode.Create);
                    await file_Uploader1.CopyToAsync(stream);
                    research.LargeImage = fileName1;
                }
                if (file_Uploader2 != null && file_Uploader2.Length > 0)
                {
                    var fileName2 = Path.GetFileName(file_Uploader2.FileName);
                    var filePath2 = Path.Combine("wwwroot/uploads/SmallImages", fileName2);
                    using var stream = new FileStream(filePath2, FileMode.Create);
                    await file_Uploader2.CopyToAsync(stream);
                    research.HomeImage = fileName2;
                }
                if (file_Uploader3 != null && file_Uploader3.Length > 0)
                {
                    var fileName3 = Path.GetFileName(file_Uploader3.FileName);
                    var filePath3 = Path.Combine("wwwroot/uploads/LargeImages", fileName3);

                    using var stream = new FileStream(filePath3, FileMode.Create);
                    await file_Uploader3.CopyToAsync(stream);

                    research.VeryLargeImage = fileName3;
                }
                var newId = await _project.AddUpdateProject(research);
                if (newId > 0)
                {
                    string msg = research.ResearchId == 0 ? "Project added successfully." : "Project updated successfully.";
                    HttpContext.Session.SetString("Message", msg);
                    return RedirectToAction("GetProject", "Projet");
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
                    NTypeId = row.Field<int?>("ntypeid"),
                    CatId = row.Field<string>("catid"),
                    ResearchTitle = row.Field<string>("researchtitle"),
                    Tagline = row.Field<string>("tagline"),
                    Location = row.Field<string>("location"),
                    Types = row.Field<string>("types"),
                    ResearchSDate = row.IsNull("researchsdate") ? null : row.Field<DateTime?>("researchsdate"),
                    ResearchEDate = row.IsNull("researchedate") ? null : row.Field<DateTime?>("researchedate"),
                    ShortDesc = row.Field<string>("shortdesc"),
                    ResearchDesc = row.Field<string>("researchdesc"),
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
                            Selected = (research.NTypeId?.ToString() == r["productid"].ToString())
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
                var result = await _project.ExecuteProjectAction(id,Mode);
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
    }
}
