using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IManagement _ser;
        private readonly clsMainMenuList _menuService;

        public ManagementController(clsMainMenuList menuService, IManagement ser)
        {
            _ser = ser;
            _menuService = menuService;
        }
        #region TeamType
        [HttpGet]
        [Route("backoffice/team/teamtype")]
        public async Task<IActionResult> teamtype()
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                var teamDtl = await _ser.GetTeamTblData();
                ViewBag.TeamTypeDtl = teamDtl;
                ViewBag.Button = "Save";
                return View("~/Views/backoffice/team/teamtype.cshtml", new clsTeamType());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the page.";
                return RedirectToAction("teamtype", "Management");
            }
        }

        [HttpPost]
        [Route("backoffice/team/AddTeamType")]
        public async Task<IActionResult> AddTeamType(clsTeamType blg)
        {
            try
            {
                if (blg != null)
                {
                    blg.CollageId = 0; //default value
                    blg.UName = HttpContext.Session.GetString("UserName") ?? "Admin";
                    int result = await _ser.AddTeamType(blg);
                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = blg.TTypeId > 0 ? "Team updated successfully." : "Team added successfully.";
                        return RedirectToAction("teamtype", "Management");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to save the blog.";
                        return RedirectToAction("teamtype", "Management");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid blog data.";
                    return RedirectToAction("teamtype", "Management");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the blog: " + ex.Message;
                return RedirectToAction("teamtype", "Management");
            }
        }

        [HttpGet]
        [Route("backoffice/team/GetTeamTypeByID/{id}")]
        public async Task<IActionResult> GetTeamTypeByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var blg = await _ser.EditTeamType(id);
                    if (blg != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        ViewBag.Button = "Update";
                        return View("~/Views/backoffice/team/teamtype.cshtml", blg);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Team not found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid team ID.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the team.";
            }
            return RedirectToAction("teamtype", "Management");
        }

        [HttpGet]
        [Route("backoffice/team/DeleteTeamType/{id}")]
        public async Task<IActionResult> DeleteTeamType(int id)
        {
            try
            {
                if (id > 0)
                {
                    var delResult = await _ser.DeleteTeamType(id);
                    if (delResult > 0)
                    {
                        TempData["SuccessMessage"] = "Team deleted successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the team.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Team id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the team.";
            }
            return RedirectToAction("teamtype", "Management");
        }

        [HttpPost]
        [Route("backoffice/team/ExportLeadershipTypeToExcel")]
        public async Task<IActionResult> ExportLeadershipTypeToExcel()
        {
            var catdtl = await _ser.GetTeamTblData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Team Type");
                worksheet.Cell(1, 1).Value = "Team Type";
                worksheet.Cell(1, 2).Value = "Description";
                worksheet.Cell(1, 3).Value = "Date";
                worksheet.Cell(1, 4).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.TType;
                    worksheet.Cell(row, 2).Value = c.ShortDesc;
                    worksheet.Cell(row, 3).Value = c.TrDate;
                    worksheet.Cell(row, 3).Style.DateFormat.Format = "yyyy-MM-dd";
                    worksheet.Cell(row, 4).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"TeamList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/team/ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _ser.ChangeStatus(id);
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
            return RedirectToAction("teamtype", "Management");
        }
        #endregion TeamType

        #region ManagementAndLeadership
        [HttpGet]
        [Route("backoffice/team/our-team")]
        public async Task<IActionResult> ourteam()
        {
            try
            {
                var menuList = _menuService.GetMenu();
                ViewBag.Menus = menuList;
                var team = await _ser.GetTeamDropdown();
                var subteam = await _ser.GetSubTeamDropdown();
                ViewBag.TeamType = new SelectList(team, "Key", "Value");
                ViewBag.SubTeamType = new SelectList(subteam, "Key", "Value");
                ViewBag.Button = "Save";
                return View("~/Views/backoffice/team/our-team.cshtml", new Management());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the page.";
                return RedirectToAction("teamtype", "Management");
            }
        }

        [HttpGet]
        [Route("backoffice/team/view-team")]
        public async Task<IActionResult> viewteam()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var team = await _ser.GetTeamManagementData();
            ViewBag.SubTeamdtl = team;
            return View("~/Views/backoffice/team/view-team.cshtml");
        }

        [HttpPost]
        [Route("backoffice/team/AddTeam")]
        public async Task<IActionResult> AddTeam(Management m, IFormFile UploadImageFile, IFormFile UploadImageFile1)
        {
            try
            {
                if (m != null)
                {
                    if (UploadImageFile != null && UploadImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(UploadImageFile.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/smallimages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            UploadImageFile.CopyTo(stream);
                        }

                        m.UploadPhoto = fileName;
                    }
                    else
                    {
                        m.UploadPhoto = m.UploadPhoto;
                    }
                    if (UploadImageFile1 != null && UploadImageFile1.Length > 0)
                    {
                        var fileName = Path.GetFileName(UploadImageFile1.FileName); // captures name
                        var filePath = Path.Combine("wwwroot/uploads/smallimages", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            UploadImageFile1.CopyTo(stream);
                        }

                        m.UploadPhoto1 = fileName;
                    }
                    else
                    {
                        m.UploadPhoto1 = m.UploadPhoto1;
                    }
                    m.Teamid = m.Teamid;
                    m.UName = HttpContext.Session.GetString("UserName") ?? "Admin";
                    int result = await _ser.AddTeam(m);
                    if (result > 0)
                    {
                        HttpContext.Session.SetString("Message", "Team Update successfully.");
                        return RedirectToAction("viewteam", "Management");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", "Failed to save the team.");
                        return RedirectToAction("viewteam", "Management");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid team data.";
                    return View("~/Views/backoffice/team/our-team.cshtml", m);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the team: " + ex.Message;
                return RedirectToAction("viewteam", "Management");
            }
        }

        [HttpGet]
        [Route("backoffice/team/GetTeamByID/{id}")]
        public async Task<IActionResult> GetTeamByID(int id)
        {
            try
            {
                if (id > 0)
                {
                    var t = await _ser.EditTeam(id);

                    if (t != null)
                    {
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        var team = await _ser.GetTeamDropdown();
                        var subteam = await _ser.GetSubTeamDropdown();
                        ViewBag.TeamType = new SelectList(team, "Key", "Value");
                        ViewBag.SubTeamType = new SelectList(subteam, "Key", "Value");

                        t.ShortDesc = WebUtility.HtmlDecode(t.ShortDesc);
                        t.DetailDesc = WebUtility.HtmlDecode(t.DetailDesc);


                        ViewBag.Button = "Update";
                        return View("~/Views/backoffice/team/our-team.cshtml", t);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Team not found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid team ID.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the team.";
            }
            return RedirectToAction("viewteam", "Management");
        }

        [HttpGet]
        [Route("backoffice/team/DeleteTeam/{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                if (id > 0)
                {
                    var delResult = await _ser.DeleteTeam(id);
                    if (delResult > 0)
                    {
                        HttpContext.Session.SetString("Message", "Team deleted successfully");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete the team.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Team id is necessary.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the team.";
            }
            return RedirectToAction("viewteam", "Management");
        }

        [HttpPost]
        [Route("backoffice/team/ExportTeamToExcel")]
        public async Task<IActionResult> ExportTeamToExcel()
        {
            var catdtl = await _ser.GetTeamManagementData();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Management team");
                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 3).Value = "Status";
                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.Name;
                    worksheet.Cell(row, 3).Value = c.Status ? "Active" : "Inactive";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"SubTeamList_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost]
        [Route("backoffice/team/ChangeManStatus/{id}")]
        public async Task<IActionResult> ChangeManStatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var chngstatus = await _ser.ChangeManStatus(id);
                    if (chngstatus > 0)
                    {
                        HttpContext.Session.SetString("Message", "Status changed successfully.");

                        TempData["Title"] = "Product";
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", "Failed to change the status.");
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
            return RedirectToAction("viewteam", "Management");
        }
        #endregion ManagementAndLeadership
    }
}
