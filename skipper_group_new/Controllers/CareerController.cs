using DocumentFormat.OpenXml.InkML;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;

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
        [Route("/backoffice/career/AddEditJobPost")]
        public async Task<IActionResult> AddEditJobPost(PostJobModel m)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            if (m.JobOpening_date == DateTime.MinValue || m.JobClosing_date == DateTime.MinValue)
            {
                return View("~/Views/backoffice/career/jobposting.cshtml");
            }
            m.Uname = HttpContext.Session.GetString("UserName");
            m.Mode = (m.Jobid > 0) ? 2 : 1;
            var resultJobId = await _management.AddEditJob(m);
            TempData["SuccessMessage"] = m.Mode == 1 ? "Job posted successfully." : "Job updated successfully.";
            return RedirectToAction("viewpostedjob", "Career");
        }

        [HttpGet]
        [Route("/backoffice/career/GetJobPostById/{jobID}")]
        public async Task<IActionResult> GetJobPostById(int jobID)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            ViewBag.Button = "Update";
            var result = await _management.GetJobPostById(jobID);
            return View("~/Views/backoffice/career/jobposting.cshtml", result);
        }

        [HttpPost]
        [Route("/backoffice/career/Delete/{jobID}")]
        public async Task<IActionResult> Delete(int jobID)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var result = await _management.Delete(jobID);
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong.";
            }
            return View("~/Views/backoffice/career/viewpostedjob.cshtml");
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
                DOB = a.App_DOB.HasValue ? a.App_DOB.Value.ToString("dd-MM-yyyy") : "",
                Gender = a.Gender,
                MaritalStatus = a.MaritalStatus,
                FatherName = a.Father_HusbandName,
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
                Salary = a.Csalary
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
                    return RedirectToAction("viewgeneral");                }

                var cleanFileName = applicant.AttachCV.Replace("~/Uploads/Applications/", "").TrimStart('/', '\\');
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Applications", cleanFileName);
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

    }
}
