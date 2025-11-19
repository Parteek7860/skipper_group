using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Service;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

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
            obj.Name = "Thankyou";
            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> contactus(int id)
        {
            await LoadSeoDataAsync(id);


            clsHomeModel obj = new clsHomeModel();
            await LoadSeoDataAsync(id);
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
                    obj.SmallDescription = WebUtility.HtmlDecode(Convert.ToString(dt.Rows[0]["smalldesc"]));
                  

                    return View("contactus", obj);
                }

            }

            return View("contactus", obj);
        }
        [HttpGet]
        [Route("blogs")]
        public IActionResult blogs()
        {
            clsHomeModel obj = new clsHomeModel();
            obj.Name = "Thankyou";
            return View(obj);
        }
        [HttpGet]

        public async Task<IActionResult> leadership(int id)
        {
            await LoadSeoDataAsync(id);
            await LoadCMSDataAsync(id);
            clsHomeModel obj = new clsHomeModel();
            

            return View("leadership", obj);
        }
        [HttpGet]
        [Route("whats-new")]
        public IActionResult newsevents()
        {
            clsHomeModel obj = new clsHomeModel();
          

            return View(obj);
        }
        [HttpGet]
        [Route("cms/{title}/{id}")]
        public async Task<IActionResult> cms(string title, int id)
        {
            await LoadSeoDataAsync(id);

            if (!string.IsNullOrEmpty(ViewBag.CurrentPageid))
            {
                //   await LoadInnerMenuAsync(Convert.ToInt32(ViewBag.Currentid));
                id = Convert.ToInt32(ViewBag.Currentid);

            }
            clsHomeModel obj = new clsHomeModel();
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
                .FirstOrDefault(r => Convert.ToString(r["rewriteurl"]).Trim('/').ToLower() == url);

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
