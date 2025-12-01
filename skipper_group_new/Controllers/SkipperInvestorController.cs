using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Models;
using skipper_group_new.Service;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{
    public class SkipperInvestorController : BaseController
    {
        // RAKESH CHAUHAN - 19/11/2025
        private readonly clsMainMenuList _menuService;
        private readonly ISkipperInvestorPage _enterface;
        private readonly ISkipperHome _homePageService;

        public SkipperInvestorController(ISkipperInvestorPage enterface, ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService):base(homePageService, menuService)
        {
            _homePageService = homePageService;

            _enterface = enterface;
        }
        [HttpGet]
        [Route("/investor-relations/financial-performance/annual-reports")]
        public async Task<IActionResult> SkipperInvestor()
        {
            
            var categoryData = new List<InvestorModel>();
            var catTable = await _enterface.GetCategoryItem();

            foreach (DataRow cat in catTable.Rows)
            {
                int catId = Convert.ToInt32(cat["pcatid"]);
                var subTable = await _enterface.GetSubCategoryItem(catId);
                var subList = subTable.AsEnumerable()
                    .Select(r => new InvSubCategoryModel
                    {
                        psubcatid = Convert.ToInt32(r["psubcatid"]),
                        subcategory = r["subcategory"].ToString().Trim(),
                        subcatrewriteurl = r["subcatrewriteurl"].ToString().Trim()
                    })
                    .GroupBy(x => x.subcategory.ToLower().Trim())
                    .Select(g => g.First())
                    .OrderBy(x => x.subcategory)
                    .ToList();
                categoryData.Add(new InvestorModel
                {
                    pcatid = catId,
                    category = cat["category"].ToString(),
                    rewriteurl = cat["rewriteurl"].ToString(),
                    subcategory = subList
                });
            }

            return View("~/Views/InvestorDetail/investor.cshtml", categoryData);
        }

        [HttpGet]
        [Route("investor-relations/{category}/{pcatid:int}")]
        public async Task<IActionResult> GetCatReports(int pcatid)
        {
            var categoryData = new List<InvestorModel>();
            var reportList = new List<ReportModel>();
            var catTable = await _enterface.GetCategoryItem();
            if (catTable != null)
            {
                foreach (DataRow cat in catTable.Rows)
                {
                    int catId = Convert.ToInt32(cat["pcatid"]);
                    var subTable = await _enterface.GetSubCategoryItem(catId);
                    var subList = subTable?.AsEnumerable()
                        .Select(r => new InvSubCategoryModel
                        {
                            psubcatid = Convert.ToInt32(r["psubcatid"]),
                            subcategory = r["subcategory"]?.ToString()?.Trim() ?? "",
                            subcatrewriteurl = r["subcatrewriteurl"]?.ToString()?.Trim() ?? ""
                        })
                        .GroupBy(x => x.subcategory.ToLower().Trim())
                        .Select(g => g.First())
                        .OrderBy(x => x.subcategory)
                        .ToList()
                        ?? new List<InvSubCategoryModel>();
                    categoryData.Add(new InvestorModel
                    {
                        pcatid = catId,
                        category = cat["category"]?.ToString(),
                        rewriteurl = cat["rewriteurl"]?.ToString(),
                        subcategory = subList
                    });
                }
            }

            var detailTable = await _enterface.GetCategoryDetail(pcatid);
            if (detailTable != null && detailTable.Rows.Count > 0)
            {
                var row = detailTable.Rows[0];

                reportList.Add(new ReportModel
                {
                    pcatid = Convert.ToInt32(row["pcatid"]),
                    psubcatid = 0,
                    productid = 0,
                    productname = null,
                    yearcategory = "",
                    prospectus = "",
                    uploadaimage = "",
                    purl = "",
                    shortDetail = WebUtility.HtmlDecode(
                        detailTable.Columns.Contains("detail")
                            ? row["detail"]?.ToString() ?? ""
                            : ""
                    )
                });
            }

            ViewBag.RightPartData = reportList;

            return View("~/Views/InvestorDetail/investor.cshtml", categoryData);
        }


        [HttpGet]
        [Route("investor-relations/{category}/{subcategoty}/{pcatid:int}/{psubcatid:int}")]
        public async Task<IActionResult> GetSubCatReports(int pcatid, int psubcatid)
        {
            var categoryData = new List<InvestorModel>();
            var reportList = new List<ReportModel>();
            var catTable = await _enterface.GetCategoryItem();
            foreach (DataRow cat in catTable.Rows)
            {
                int catId = Convert.ToInt32(cat["pcatid"]);
                var subTable = await _enterface.GetSubCategoryItem(catId);
                var subList = subTable.AsEnumerable()
                    .Select(r => new InvSubCategoryModel
                    {
                        psubcatid = Convert.ToInt32(r["psubcatid"]),
                        subcategory = r["subcategory"].ToString().Trim(),
                        subcatrewriteurl = r["subcatrewriteurl"].ToString().Trim()
                    })
                    .GroupBy(x => x.subcategory.ToLower().Trim())
                    .Select(g => g.First())
                    .OrderBy(x => x.subcategory)
                    .ToList();

                categoryData.Add(new InvestorModel
                {
                    pcatid = catId,
                    category = cat["category"].ToString(),
                    rewriteurl = cat["rewriteurl"].ToString(),
                    subcategory = subList
                });
            }

            if (psubcatid == 0)
            {
                var d = await _enterface.GetCategoryDetail(pcatid);
                if (d != null && d.Rows.Count > 0)
                {
                    var row = d.Rows[0];

                    reportList.Add(new ReportModel
                    {
                        pcatid = Convert.ToInt32(row["pcatid"]),
                        psubcatid = 0,
                        productid = 0,
                        productname = null,
                        yearcategory = "",
                        prospectus = "",
                        uploadaimage = "",
                        purl = "",
                        shortDetail = WebUtility.HtmlDecode(row.Table.Columns.Contains("detail") ? row["detail"]?.ToString() ?? "" : "")
                    });

                    ViewBag.RightPartData = reportList;
                    return View("~/Views/InvestorDetail/investor.cshtml", categoryData);
                }
            }
            var dt = await _enterface.GetReports(pcatid, psubcatid);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    int subCatId = Convert.ToInt32(r["psubcatid"]);
                    string subCategoryName = categoryData.SelectMany(c => c.subcategory).Where(s => s.psubcatid == subCatId).Select(s => s.subcategory).FirstOrDefault() ?? "";
                    reportList.Add(new ReportModel
                    {
                        pcatid = Convert.ToInt32(r["pcatid"]),
                        psubcatid = subCatId,
                        productid = Convert.ToInt32(r["productid"]),
                        title = subCategoryName,
                        productname = r["productname"].ToString(),
                        yearcategory = r["yearcategory"].ToString(),
                        prospectus = r["prospectus"].ToString(),
                        uploadaimage = r["uploadaimage"].ToString(),
                        purl = r["purl"].ToString(),
                        shortDetail = null
                    });
                }
            }

            ViewBag.RightPartData = reportList;
            return View("~/Views/InvestorDetail/investor.cshtml", categoryData);
        }

        [HttpGet]
        [Route("SkipperInvestor/GetCaptcha")]
        public IActionResult GetCaptcha()
        {
            string captcha = GenerateCaptcha();
            HttpContext.Session.SetString("Captcha", captcha);
            return Json(new { code = captcha });
        }

        private string GenerateCaptcha()
        {
            string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";    
            string lower = "abcdefghijkmnopqrstuvwxyz";  
            string numbers = "23456789";  
            string special = "!@#$%^&*?";
            string allChars = upper + lower + numbers + special;
            Random rnd = new Random();
            int length = rnd.Next(5, 9); 
            List<char> captcha = new List<char>
            {
                upper[rnd.Next(upper.Length)],
                lower[rnd.Next(lower.Length)],
                numbers[rnd.Next(numbers.Length)],
                special[rnd.Next(special.Length)]
            };
            for (int i = captcha.Count; i < length; i++) captcha.Add(allChars[rnd.Next(allChars.Length)]);
            return new string(captcha.OrderBy(x => rnd.Next()).ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> SaveInvestorQuery([FromBody] InvestorQueryModel model)
        {            
            string sessionCaptcha = HttpContext.Session.GetString("Captcha");
            if (string.IsNullOrEmpty(sessionCaptcha) || model.CaptchaCode != sessionCaptcha)
                return Json(new { status = false, message = "Invalid Captcha!" });

            var res = await _enterface.SaveQuery(model);
            if (res == 0)
                return Json(new { status = false, message = "Something went wrong. Query not submitted." });

            return Json(new { status = true, message = "Query submitted successfully!" });
        }

    }
}
