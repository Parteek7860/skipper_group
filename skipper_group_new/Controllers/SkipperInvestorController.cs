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
       // private readonly clsMainMenuList _menuService;
        private readonly ISkipperInvestorPage _enterface;
        private readonly ISkipperHome _homePageService;

        public SkipperInvestorController(ISkipperInvestorPage enterface, ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService)
    : base(homePageService, menuService)
        {
            _homePageService = homePageService;
            _enterface = enterface;

        }
        //public SkipperInvestorController(ISkipperInvestorPage enterface, IConfiguration configuration, MenuDataService menuService) : base(homePageService, menuService)
        //{
        //    _enterface = enterface;
        //    _menuService = menuService;
        //}

        [HttpGet]
        [Route("/investor-relations/financial-performance/annual-reports")]
        public async Task<IActionResult> SkipperInvestor()
        {
           // ViewBag.MainMenu = _menuService.GetMenu();
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
        [Route("investor-relations/{pcatid}/{psubcatid}")]
        public async Task<IActionResult> GetReports(int pcatid, int psubcatid)
        {
            //ViewBag.MainMenu = _menuService.GetMenu();
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
                    reportList.Add(new ReportModel
                    {
                        pcatid = Convert.ToInt32(r["pcatid"]),
                        psubcatid = Convert.ToInt32(r["psubcatid"]),
                        productid = Convert.ToInt32(r["productid"]),
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

        public IActionResult DownloadFile(string file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/prospectus", file);
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/pdf", file);
        }
    }
}
