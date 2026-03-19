using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using System.Data;
using System.Web;

namespace skipper_group_new.Controllers
{
    [Route("[controller]/[action]")]
    public class CareerController : Controller
    {
        private readonly IManagement _management;
        private readonly clsMainMenuList _menuService;

        public CareerController(clsMainMenuList menuService, IManagement management)
        {
            _management = management;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("/backoffice/career/jobposting")]
        public async Task<IActionResult> jobposting()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var model = new PostJobModel();

            var x = await _management.GetProductSolutionList();
            var filteredRows = x.AsEnumerable()
                .Where(row => row.Field<bool>("status") == true && row.Field<bool>("show_on_career") == true);

            if (filteredRows != null && filteredRows.Any())
            {
                DataTable dt = filteredRows.CopyToDataTable();

                var list = dt.AsEnumerable()
                    .Select(r => new SelectListItem
                    {
                        Value = r["productid"]?.ToString(),
                        Text = r["productname"]?.ToString()
                    }).ToList();

                ViewBag.EmpTypeList = new SelectList(list, "Value", "Text");
            }
            else
            {
                ViewBag.EmpTypeList = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }



            model.JobClosing_date = Convert.ToDateTime(DateTime.Now);
            model.JobOpening_date = Convert.ToDateTime(DateTime.Now);

            ViewBag.Button = "Save";
            return View("~/Views/backoffice/career/jobposting.cshtml", model);
        }

        [HttpGet]
        [Route("/backoffice/career/viewpostedjob")]
        public async Task<IActionResult> viewpostedjob()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var jobList = await _management.GetJobList();
            ViewBag.Jobs = jobList;
            return View("~/Views/backoffice/career/viewpostedjob.cshtml");
        }

        [HttpGet]
        [Route("/backoffice/career/viewgeneral")]
        public async Task<IActionResult> viewgeneral()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var applicantDtl = await _management.GetApplicantsDetail();
            ViewBag.Applications = applicantDtl;
            return View("~/Views/backoffice/career/viewgeneral.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/backoffice/career/jobposting")]
        public async Task<IActionResult> jobposting(PostJobModel m)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var x = await _management.GetProductSolutionList();
            var filteredRows = x.AsEnumerable()
                .Where(row => row.Field<bool>("status") == true);

            DataTable dt = filteredRows.CopyToDataTable();


            var list = dt.AsEnumerable()
                .Select(r => new SelectListItem
                {
                    Value = r["productid"].ToString(),
                    Text = r["productname"].ToString()
                }).ToList();



            ViewBag.EmpTypeList = new SelectList(list, "Value", "Text");
            m.Uname = HttpContext.Session.GetString("UserName");
            m.Mode = (m.Jobid > 0) ? 2 : 1;
            m.JobCode = m.JobCode ?? "";
            if (m.Jobid <= 0)
            {
                m.Status = true;
            }
            var resultJobId = await _management.AddEditJob(m);
            if (resultJobId > 0)
            {
                if (m.Jobid > 0)
                {
                    HttpContext.Session.SetString("Message", "Job Update successfully.");
                    return RedirectToAction("viewpostedjob", "Career");
                }
                else
                {
                    HttpContext.Session.SetString("Message", "Job Save successfully.");
                    return RedirectToAction("jobposting", "Career");
                }
            }
            return View("~/Views/backoffice/career/jobposting.cshtml", m);
        }

        [HttpGet]
        [Route("/backoffice/career/GetJobPostById/{jobID}")]
        public async Task<IActionResult> GetJobPostById(int jobID)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.Button = "Update";

            var result = await _management.GetJobPostById(jobID);

            if (result == null)
                return RedirectToAction("viewpostedjob");

            result.Skills = HttpUtility.HtmlDecode(result.Skills ?? "");
            result.Qualification = HttpUtility.HtmlDecode(result.Qualification ?? "");
            result.shortdesc = HttpUtility.HtmlDecode(result.shortdesc ?? "");
            result.NoOfVacancies = HttpUtility.HtmlDecode(result.NoOfVacancies ?? "");

            var x = await _management.GetProductSolutionList();
            var list = x.AsEnumerable()
                .Where(row => row.Field<bool>("status") == true)
                .Select(r => new SelectListItem
                {
                    Value = r["productid"].ToString(),
                    Text = r["productname"].ToString()
                }).ToList();

            ViewBag.EmpTypeList = new SelectList(list, "Value", "Text", result.EmpTypeId);

            return View("~/Views/backoffice/career/jobposting.cshtml", result);
        }

        [HttpGet]
        [Route("/backoffice/career/delete/{jobID}")]
        public async Task<IActionResult> delete(int jobID)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var result = await _management.Delete(jobID);
            if (result > 0)
            {
                HttpContext.Session.SetString("Message", "Delete successfully.");
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong.";
            }
            return RedirectToAction("viewpostedjob", "career");
        }

        [HttpGet]
        [Route("/backoffice/career/GetApplicantDetail/{App_id}")]
        public async Task<IActionResult> GetApplicantDetail(int App_id)
        {
            var a = await _management.GetApplicantsDetailByID(App_id);

            if (a == null)
                return Json(new { success = false, message = "Applicant not found." });

            var applicant = new
            {
                Name = $"{a.FName} {a.LName}",
                Address = a.App_Address,
                Mobile = a.Mobile,
                Telephone = a.Telephone,
                Email = a.App_Email,
                City = a.City,
                State = a.State,
                Qualification = a.App_Qualification,
                Experience = $"{a.App_Expyear} Years {a.App_Expmonth} Months",
                Skills = a.App_Skills,
                Function = a.Funarea,
                Industry = a.CurrIndustries,
                Location = a.PrefLocation,
                Salary = a.Csalary,
                country = a.Country ?? ""

            };

            return Json(new { success = true, data = applicant });
        }

        [HttpGet]
        [Route("/backoffice/career/downloadresume/{App_id}")]
        public async Task<IActionResult> DownloadResume(int App_id)
        {
            try
            {
                var applicant = await _management.GetApplicantsDetailByID(App_id);
                if (applicant == null)
                {
                    TempData["ErrorMessage"] = "Applicant not found.";
                    return RedirectToAction("viewgeneral");
                }

                if (string.IsNullOrEmpty(applicant.AttachCV))
                {
                    TempData["ErrorMessage"] = "Resume not uploaded.";
                    return RedirectToAction("viewgeneral");
                }

                var cleanFileName = applicant.AttachCV.Replace("~/Uploads/files/", "").TrimStart('/', '\\');
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "files", cleanFileName);
                if (!System.IO.File.Exists(filePath))
                {
                    TempData["ErrorMessage"] = "Resume file not found on server.";
                    return RedirectToAction("viewgeneral");
                }

                var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out string contentType))
                    contentType = "application/octet-stream";

                return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while downloading the resume.";
                Console.WriteLine($"Error in DownloadResume: {ex.Message}");
                return RedirectToAction("viewgeneral");
            }
        }

        [HttpGet]
        [Route("backoffice/career/editstatus/{id:int}")]
        public async Task<IActionResult> editstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    var x = _management.GetJobList();
                    var filtetedRows = x.Result.AsEnumerable()
                        .Where(row => row.Jobid == id);
                    if (filtetedRows.Any())
                    {
                        var dt = filtetedRows.First();
                        string status = dt.Status == true ? "True" : "False";
                        var chngstatus = _management.JobChangeStatus(status, id);
                        if (chngstatus > 0)
                        {
                            HttpContext.Session.SetString("Message", "Status Update successfully.");
                            return RedirectToAction("viewpostedjob", "career");
                        }

                    }

                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("viewpostedjobs", "career");
        }

        [HttpGet]
        [Route("backoffice/career/ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var catdtl = await _management.GetApplicantsDetail();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Categories");
                worksheet.Cell(1, 1).Value = "Job Title";
                worksheet.Cell(1, 2).Value = "Full Name";
                worksheet.Cell(1, 3).Value = "Email ID";
                worksheet.Cell(1, 4).Value = "Mobile";
                worksheet.Cell(1, 5).Value = "Address";
                worksheet.Cell(1, 6).Value = "Country";
                worksheet.Cell(1, 7).Value = "State";
                worksheet.Cell(1, 8).Value = "City";
                worksheet.Cell(1, 9).Value = "Date";
                // 👉 Header Styling
                var headerRange = worksheet.Range("A1:I1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                headerRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                headerRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                worksheet.Columns().AdjustToContents();

                int row = 2;
                foreach (var c in catdtl)
                {
                    worksheet.Cell(row, 1).Value = c.jobtitle ?? "";
                    worksheet.Cell(row, 2).Value = c.FName ?? "" + " " + c.LName ?? "";
                    worksheet.Cell(row, 3).Value = c.App_Email ?? "";
                    worksheet.Cell(row, 4).Value = c.Mobile ?? "";
                    worksheet.Cell(row, 5).Value = c.app_address ?? "";
                    worksheet.Cell(row, 6).Value = c.country ?? "";
                    worksheet.Cell(row, 7).Value = c.state ?? "";
                    worksheet.Cell(row, 8).Value = c.city ?? "";
                    worksheet.Cell(row, 9).Value = c.Trdate;
                    worksheet.Cell(row, 9).Style.DateFormat.Format = "yyyy-MM-dd";
                    row++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    string fileName = $"List_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


    }
}
