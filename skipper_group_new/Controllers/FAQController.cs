using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{

    public class FAQController : Controller
    {

        private readonly clsMainMenuList _menuService;
        private readonly IBackofficePage _backofficeService;

        public FAQController(IBackofficePage backofficeService, clsMainMenuList menuService)
        {
            _backofficeService = backofficeService;
            _menuService = menuService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("backoffice/faq/addfaq")]
        public async Task<IActionResult> addfaq()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var faq = await _backofficeService.GetFAQList();
            ViewBag.Faq = faq;

            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/faq/addfaq.cshtml");
        }
        [HttpPost]
        [Route("backoffice/faq/addfaq")]
        public async Task<IActionResult> addfaq(clsfaq obj)
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;


            clsfaq objcls = new clsfaq();
            if (!string.IsNullOrEmpty(obj.title))
            {
                objcls.title = obj.title;
                objcls.description = obj.description;
                objcls.displayorder = obj.displayorder;
                objcls.uname = HttpContext.Session.GetString("UserName");
                objcls.Id = obj.Id;
                objcls.status = obj.status;
                if (obj.Id == 0)
                {
                    objcls.Mode = "1";
                }
                else
                {
                    objcls.Mode = "2";
                }
                int x = await _backofficeService.AddFAQ(objcls);
                if (x > 0)
                {
                    if (objcls.Id == 0)
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Save successfully.");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Update successfully.");
                    }
                }
                ViewBag.CreateUpdate = "Save";
                objcls = new clsfaq();

                var faq = await _backofficeService.GetFAQList();
                ViewBag.Faq = faq;
            }

            return View("~/Views/backoffice/faq/addfaq.cshtml", objcls);
        }

        [HttpGet]
        [Route("backoffice/faq/faqstatus/{id}")]
        public async Task<IActionResult> faqstatus(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        clsfaq obj = new clsfaq();
                        // Get the Media list by ID
                        var productTypes = await _backofficeService.GetFAQList();
                        // Replace the following code block:

                        // With this code block:
                        var filteredRows = productTypes.AsEnumerable()
                            .Where(pt => pt.Field<int>("cid") == id)
                            .ToList();


                        if (filteredRows.Count > 0)
                        {
                            obj.status = Convert.ToString(Convert.ToInt32(filteredRows[0]["Status"])) == "1" ? "True" : "False"; // Toggle status
                            int x1 = _backofficeService.FAQUpdateStatus(obj.status, id);
                            if (x1 > 0)
                            {
                                HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Status Update successfully.");

                                return RedirectToAction("addfaq", "faq");
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
            return RedirectToAction("addfaq", "faq");
        }

        [HttpGet]
        [Route("backoffice/faq/faqedit/{id}")]
        public async Task<IActionResult> faqedit(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        clsfaq objcls = new clsfaq();
                        var menuList = _menuService.GetMenu();
                        ViewBag.Menus = menuList;
                        HttpContext.Session.Remove("Message");

                        var brand = await _backofficeService.GetFAQList();
                        ViewBag.Faq = brand;

                        var x = _backofficeService.GetFAQList();
                        if (x != null)
                        {
                            var filterresult = x.Result.AsEnumerable().Where(p => p.Field<int>("cid") == id).ToList();
                            if (filterresult.Count > 0)
                            {
                                objcls.Id = Convert.ToInt16(filterresult[0]["cid"]);
                                objcls.title = Convert.ToString(filterresult[0]["question"]);
                                objcls.description = WebUtility.HtmlDecode(Convert.ToString(filterresult[0]["description"]));
                                objcls.status = Convert.ToString(filterresult[0]["Status"]);
                                ViewBag.CreateUpdate = "Update";
                                return View("~/Views/backoffice/faq/addfaq.cshtml", objcls);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "An error occurred while processing your request.";
                        return RedirectToAction("addfaq", "faq");
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
            return RedirectToAction("addfaq", "faq");
        }

        [HttpGet]
        [Route("backoffice/FAQ/FaqDelete/{id}")]
        public async Task<IActionResult> FaqDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    try
                    {
                        HttpContext.Session.Remove("Message");
                        int x1 = _backofficeService.FAQDeleteRecords(id);
                        if (x1 > 0)
                        {
                            HttpContext.Session.SetString("Message", HttpContext.Session.GetString("Message") + "Delete successfully.");

                            return RedirectToAction("addfaq", "faq");
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
            return RedirectToAction("addfaq", "faq");
        }

        [HttpGet]
        [Route("backoffice/faq/imagefilepath")]
        public async Task<IActionResult> imagefilepath()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;

            var faq = await _backofficeService.GetImageFilePathList();
            ViewBag.Faq = faq;

            ViewBag.CreateUpdate = "Save";

            return View("~/Views/backoffice/faq/imagefilepath.cshtml");
        }
        [HttpGet]
        [Route("backoffice/faq/download/{id}")]
        public async Task<IActionResult> download(int id)
        {
            // Get file list from service
            var faq = await _backofficeService.GetImageFilePathList();

            // Find the row with matching id
            var filteredRow = faq.AsEnumerable()
                                 .FirstOrDefault(pt => pt.Field<int>("fileid") == id);

            if (filteredRow == null)
                return NotFound();

            var fileName = Convert.ToString(filteredRow["uploadfile"]);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/image", fileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }


    }
}
