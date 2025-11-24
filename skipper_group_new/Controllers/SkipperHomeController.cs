using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace skipper_group_new.Controllers
{
    public class SkipperHomeController : BaseController
    {
        private readonly List<UrlValidationRule> _validationRules;
        private readonly ISkipperHome _homePageService;

        public SkipperHomeController(ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService)
     : base(homePageService, menuService)
        {
            _homePageService = homePageService;

            _validationRules = configuration.GetSection("UrlValidationRules:Rules")
             .Get<List<UrlValidationRule>>() ?? new();
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {

            clsHomeModel obj = new clsHomeModel();
            await LoadSeoDataAsync(1);
            await BindProjectsList();
            await Task.Delay(1);

            Task<DataTable> x = this._homePageService.GetCMSData();
            if (x != null)
            {
                DataRow[] results = x.Result.Select($"pagestatus=1 and pageid='1'");
                if (results.Length == 0)
                {
                    return (IActionResult)this.RedirectToAction("Index", "SkipperHome");
                }
                else
                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.cmscontent = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
                    obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));
                    obj.pagedesc2 = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription1"]));
                    obj.pagedesc3 = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription2"]));

                    return View(obj);
                }

            }

            return View(obj);
        }
        [HttpGet]
        [Route("thankyou")]
        public IActionResult Thankyou()
        {
            clsHomeModel obj = new clsHomeModel();
            LoadCMSDataAsync(21);

            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> contactus(int id)
        {
            EnquiryModel obj = new EnquiryModel();
            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
            obj.capacha = CaptchaHelper.GenerateCaptcha();

            Task<DataTable> x = this._homePageService.GetCMSData();
            if (x != null)
            {
                DataRow[] results = x.Result.Select($"pagestatus=1 and pageid='{id.ToString()}'");
                if (results.Length == 0)
                {
                    return (IActionResult)this.RedirectToAction("Index", "SkipperHome");
                }
                else
                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.desc = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
                    obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));
                    return View("contactus", obj);
                }

            }

            return View("contactus", obj);
        }
        [HttpPost]

        public async Task<IActionResult> contactus(EnquiryModel cls, int id)
        {
            await LoadSeoDataAsync(10);
            await LoadCMSDataAsync(10);
            EnquiryModel obj = new EnquiryModel();


            Task<DataTable> x1 = this._homePageService.GetCMSData();
            if (x1 != null)
            {
                DataRow[] results = x1.Result.Select($"pagestatus=1 and pageid='10'");
                if (results.Length > 0)
                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.desc = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
                    obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));


                }

            }

            if (cls.CaptchaInput != cls.capacha)
            {
                ModelState.AddModelError("CaptchaInput", "Invalid Captcha. Please try again.");
                //obj = cls;
                obj.capacha = CaptchaHelper.GenerateCaptcha();
                return View("contactus", obj);
            }


            obj.phone = cls.phone;
            obj.FName = cls.FName;
            obj.EmailId = cls.EmailId;
            obj.address = cls.address;
            obj.country = cls.country;
            obj.company = cls.company;
            obj.FMessage = cls.FMessage;
            var x = _homePageService.SaveContactEnquiry(obj);
            if (x > 0)
            {
                return RedirectToAction("Thankyou", "SkipperHome");
            }

            return View("contactus", obj);
        }
        #region News and Events
        [HttpGet]
        public async Task<IActionResult> news(int id)
        {
            clsMediatype obj = new clsMediatype();

            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
            var x = await _homePageService.GetNewsEvents();
            var filterlist = x.AsEnumerable()
                   .Where(r => r.Field<bool>("status") == true)
                   .OrderBy(r => r.Field<int>("displayorder"))
                   .ToList();

            var top2 = filterlist.Take(2).ToList();

            var next3 = filterlist.Skip(2).Take(2).ToList();

            // --- Balance records ---
            var balance = filterlist.Skip(4).ToList();

            // Convert to DataTable only if needed
            DataTable dtTop2 = top2.Any() ? top2.CopyToDataTable() : x.Clone();
            DataTable dtNext3 = next3.Any() ? next3.CopyToDataTable() : x.Clone();
            DataTable dtBalance = balance.Any() ? balance.CopyToDataTable() : x.Clone();

            // Send to ViewBag
            ViewBag.Top2 = dtTop2;
            ViewBag.Next2 = dtNext3;
            ViewBag.Balance = dtBalance;

            return View("news", obj);
        }
        [HttpGet]
        [Route("news-details/{title}/{eventsid}")]
        public async Task<IActionResult> newsdetail(string title, string eventsid)
        {
            clsMediatype obj = new clsMediatype();
            await LoadCMSDataAsync(12);
            var x1 = await _homePageService.GetNewsEvents();
            ViewBag.RelatedEvents = x1.Select($"status=1 and eventsid not in ('{eventsid.ToString()}')").CopyToDataTable();
            var x = await _homePageService.GetNewsEvents();
            DataRow[] results = x.Select($"status=1 and eventsid='{eventsid.ToString()}'");
            if (results.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                obj.id = Convert.ToInt32(dt.Rows[0]["eventsid"]);
                obj.eventstitle= Convert.ToString(dt.Rows[0]["eventstitle"]);
                obj.eventsdate = dt.Rows[0]["eventsdate"] == DBNull.Value ? DateTime.MinValue
    : Convert.ToDateTime(dt.Rows[0]["eventsdate"]);
                obj.shortdetail = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["eventsdesc"]));
                obj.Largeimage = Convert.ToString(dt.Rows[0]["largeimage"]);
                // obj.detail = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["detail"]));

            }

            return View("newsdetail", obj);
        }
        #endregion

        [HttpGet]

        public async Task<IActionResult> leadership(int id)
        {
            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
            clsHomeModel obj = new clsHomeModel();


            return View("leadership", obj);
        }

        #region Career
        [HttpGet]
        public async Task<IActionResult> career(int id)
        {
            clsHomeModel obj = new clsHomeModel();
            await LoadSeoDataAsync(id);
            string parentid = await GetParentID(id);
            if (parentid != "0")
            {
                await LoadCMSDataAsync(Convert.ToInt32(parentid));
            }
            else
            {
                parentid = Convert.ToString(id);
            }


            var list = await _homePageService.GetCMSData();
            DataRow[] results = list.Select($"pagestatus=1 and pageid='{parentid}'");
            if (results.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                obj.Description = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
                obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));


            }
            var x = await _homePageService.GetCarrer();
            var filterlist = x.AsEnumerable()
                   .Where(r => r.Field<DateTime>("jobclosing_date") >= DateTime.Now)
                   .OrderBy(r => r.Field<int>("displayorder"))
                   .CopyToDataTable();



            ViewBag.CurrentOpenings = filterlist;
            return View("career", obj);
        }
        [HttpGet]
        [Route("career-details/{title}/{jobid}")]
        public async Task<IActionResult> careerdetail(string title, string jobid)
        {
            PostJobModel obj = new PostJobModel();
            await LoadCMSDataAsync(8);
            var x = await _homePageService.GetCarrer();
            DataRow[] results = x.Select($"status=1 and jobid='{jobid.ToString()}'");
            if (results.Length > 0)
            {
                DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                obj.Jobid = Convert.ToInt32(dt.Rows[0]["jobid"]);
                obj.JobTitle = Convert.ToString(dt.Rows[0]["jobtitle"]);
                obj.department = Convert.ToString(dt.Rows[0]["department"]);
                obj.company = Convert.ToString(dt.Rows[0]["company"]);
                obj.JobCode = Convert.ToString(dt.Rows[0]["jobcode"]);
                obj.Min_Expyear = Convert.ToInt32(dt.Rows[0]["min_expyear"]);
                obj.Max_Expyear = Convert.ToInt32(dt.Rows[0]["max_expyear"]);
                obj.Qualification = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["qualification"]));



                obj.Location = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["location"]));
                obj.shortdesc = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["shortdesc"]));
            }

            return View("careerdetail", obj);
        }

        [HttpGet]
        [Route("apply-now/{title}/{id}")]
        public async Task<IActionResult> apply(string title, string id)
        {
            await LoadSeoDataAsync(81);
            await LoadCMSDataAsync(81);
            EnquiryModel obj = new EnquiryModel();
            obj.capacha = CaptchaHelper.GenerateCaptcha();
            return View("apply", obj);
        }
        [HttpPost]
        [Route("apply-now/{title}/{id}")]
        public async Task<IActionResult> apply(EnquiryModel cls, IFormFile file_uploader)
        {
            await LoadSeoDataAsync(81);
            await LoadCMSDataAsync(81);
            EnquiryModel obj = new EnquiryModel();

            if (cls.CaptchaInput != cls.capacha)
            {
                ModelState.AddModelError("CaptchaInput", "Invalid Captcha. Please try again.");
                obj = cls;
                obj.capacha = CaptchaHelper.GenerateCaptcha();
                return View("apply", obj);
            }
            obj.Eid = Convert.ToInt32(HttpContext.Request.RouteValues["id"]?.ToString());
            obj.phone = cls.phone;
            obj.FName = cls.FName;
            obj.EmailId = cls.EmailId;
            obj.EmailId = cls.EmailId;
            obj.OrganizationName = cls.OrganizationName;
            obj.address = cls.address;
            obj.city = cls.city;
            obj.state = cls.state;
            obj.country = cls.country;
            obj.zipcode = cls.zipcode;
            obj.FMessage = cls.FMessage;


            if (file_uploader != null && file_uploader.Length > 0)
            {
                var fileName = Path.GetFileName(file_uploader.FileName); // captures name
                var filePath = Path.Combine("wwwroot/uploads/files", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file_uploader.CopyTo(stream);
                }

                obj.uploadfile = fileName;
            }
            var x = _homePageService.SaveEnquiryDetails(obj);
            if (x > 0)
            {
                return RedirectToAction("Thankyou", "SkipperHome");
            }

            return View("apply", obj);
        }

        #endregion

        #region Projects
        [HttpGet]
        public async Task<IActionResult> projectlist(int id)
        {
            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
            GetProjectList();
            clsHomeModel obj = new clsHomeModel();

            return View("projectlist", obj);
        }
        [HttpGet]
        [Route("project-details/{projectname}/{projectid}")]
        public async Task<IActionResult> projectdetail(string projectname, string projectid)
        {
            clsProduct obj = new clsProduct();
            obj.id = Convert.ToInt16(projectid);
            if (string.IsNullOrEmpty(projectid) || !int.TryParse(projectid, out int projId))
            {
                return RedirectToAction("projectlist", "SkipperHome");
            }
            else
            {
                var x = await this._homePageService.GetProjectsList();
                DataRow[] results = x.Select($"status=1 and researchid='{projectid.ToString()}'");
                if (results.Length > 0)

                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.ProductId = Convert.ToInt32(dt.Rows[0]["researchid"]);
                    obj.ProductName = Convert.ToString(dt.Rows[0]["researchtitle"]);
                    obj.ProductTitle = Convert.ToString(dt.Rows[0]["tagline"]);
                    obj.ShortDetail = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["shortdesc"]));
                    obj.ProductDetail = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["researchdesc"]));
                    obj.UploadAImage = Convert.ToString(dt.Rows[0]["largeimage"]);
                    obj.ModelNo = Convert.ToString(dt.Rows[0]["location"]);


                }
                return View("projectdetail", obj);
            }


            // return View("projectdetail", obj);
        }


        public IActionResult GetProjectList()
        {
            var list = _homePageService.GetProjectsList();
            var filterlist = list.Result.Select("status=1").OrderBy(r => Convert.ToInt32(r["displayorder"]));
            ViewBag.ProjectsList = filterlist.CopyToDataTable();
            return View();


        }
        [HttpGet]
        public async Task<IActionResult> BindProjectsList()
        {
            ResearchModel obj = new ResearchModel();
            var list = _homePageService.GetProjectsList();
            var filterlist = list.Result.Select("status=1 and showonhome=1 and showonschool=1");
            var top3 = filterlist.Take(3).CopyToDataTable();
            ViewBag.ProjectsList = top3;
            return View();
        }

        #endregion

        [HttpGet]
        [Route("cms/{title}/{id}")]
        public async Task<IActionResult> cms(string title, int id)
        {
            await LoadSeoDataAsync(id);
            //string parentid = await GetParentID(id);
            //if (parentid == "0")
            //{
            //    //   await LoadInnerMenuAsync(Convert.ToInt32(ViewBag.Currentid));
            //    id = Convert.ToInt32(parentid);

            //}
            clsHomeModel obj = new clsHomeModel();
            await LoadCMSDataAsync(id);
            Task<DataTable> x = this._homePageService.GetCMSData();
            if (x != null)
            {
                DataRow[] results = x.Result.Select($"pagestatus=1 and pageid='{id.ToString()}'");
                if (results.Length == 0)
                {
                    return (IActionResult)this.RedirectToAction("Index", "SkipperHome");
                }
                else
                {
                    DataTable dt = ((IEnumerable<DataRow>)results).CopyToDataTable<DataRow>();
                    obj.cmscontent = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["pagedescription"]));
                    obj.Name = Convert.ToString(dt.Rows[0]["pagename"]);
                    obj.uploadimage = Convert.ToString(dt.Rows[0]["uploadbanner"]);
                    obj.parentname = "";

                    return View("~/Views/SkipperHome/cms.cshtml", obj);
                }

            }

            return View();
        }
        [HttpGet("{*url}")]
        public async Task<IActionResult> DynamicRoute(string url)
        {


            if (string.IsNullOrEmpty(url))
                return RedirectToAction("Index", "SkipperHome");

            url = url.Trim('/').ToLower();

            // ✅ Remove query strings (e.g. ?v=2.1)
            url = url.Split('?')[0];

            // ✅ Folders to ignore (static content)
            string[] ignoredFolders = { "css", "js", "images", "uploads", "fonts", "lib", "content", "assets" };

            // ✅ File extensions to ignore (static files)
            string[] staticExtensions = { ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico", ".webp", ".woff", ".woff2", ".ttf", ".map", ".json", ".mp4", ".pdf" };

            // ✅ Routes to ignore (like admin/backoffice)
            string[] ignoredRoutes =
            {
        "backoffice",
        "admin",
        "api",
        "swagger"
    };


            // 🛑 Skip static content completely
            if (ignoredFolders.Any(f => url.StartsWith(f + "/", StringComparison.OrdinalIgnoreCase)) ||
             staticExtensions.Any(ext => url.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) ||
             ignoredRoutes.Any(r => url.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
            {
                return NotFound(); // Let normal routing handle it
            }
            string fullUrl = HttpContext.Request.GetDisplayUrl();

            // ✅ Validate dynamically
            if (!IsUrlValidDynamic(fullUrl))
            {
                return RedirectToAction("Handle", "Error", new { code = 400 });
            }
            // 🛑 Prevent recursive calls
            //if (url.StartsWith("SkipperHome/dynamicroute", StringComparison.OrdinalIgnoreCase))
            //    return RedirectToAction("Index", "SkipperHome");

            var dt = await _homePageService.GetSeoFriendlyUrls();
            if (dt == null || dt.Rows.Count == 0)
                return RedirectToAction("Index", "SkipperHome");

            var matchedRow = dt.AsEnumerable()
                .FirstOrDefault(r => Convert.ToString(r["rewriteurl"]).Split('#')[0].Trim('/').ToLower() == url);

            if (matchedRow == null)
                //return RedirectToAction("Index", "SkipperHome");
                return RedirectToAction("Handle", "Error", new { statusCode = 400 });

            string controller = Convert.ToString(matchedRow["controller"])?.Trim() ?? "SkipperHome";
            string action = Convert.ToString(matchedRow["action"])?.Trim() ?? "cms";

            int id = 0;
            if (matchedRow.Table.Columns.Contains("pageid") && matchedRow["pageid"] != DBNull.Value)
                int.TryParse(matchedRow["pageid"].ToString(), out id);

            // ✅ Get controller type
            var controllerType = typeof(SkipperHomeController).Assembly
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(Controller).IsAssignableFrom(t) &&
                    t.Name.Equals(controller + "Controller", StringComparison.OrdinalIgnoreCase));

            if (controllerType != null)
            {
                var controllerInstance = (Controller)ActivatorUtilities.CreateInstance(HttpContext.RequestServices, controllerType);
                var methodInfo = controllerType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m =>
                        m.Name.Equals(action, StringComparison.OrdinalIgnoreCase) &&
                        (m.ReturnType == typeof(IActionResult) || m.ReturnType == typeof(Task<IActionResult>)));

                if (methodInfo != null)
                {
                    var parameters = methodInfo.GetParameters();
                    object[] args = parameters.Length > 0
                        ? parameters.Select(p => p.ParameterType == typeof(int) ? (object)id : null).ToArray()
                        : null;

                    var result = methodInfo.Invoke(controllerInstance, args);

                    if (result is Task<IActionResult> taskResult)
                        return await taskResult;

                    if (result is IActionResult actionResult)
                        return actionResult;
                }
            }

            // Fallback to CMS
            return await cms(url, id);
        }

        private bool IsUrlValidDynamic(string inputUrl)
        {
            if (string.IsNullOrWhiteSpace(inputUrl))
                return true;

            if (!Uri.TryCreate(inputUrl, UriKind.Absolute, out var uri))
                return false;

            string path = uri.AbsolutePath.Trim('/');
            if (string.IsNullOrEmpty(path))
                return true;

            foreach (var rule in _validationRules)
            {
                if (Regex.IsMatch(path, rule.Pattern, RegexOptions.IgnoreCase))
                {
                    if (rule.Type.Equals("Block", StringComparison.OrdinalIgnoreCase))
                        return false;
                }
            }

            return true;
        }
    }


}
