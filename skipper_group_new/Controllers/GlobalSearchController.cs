using Microsoft.AspNetCore.Mvc;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Service;

namespace skipper_group_new.Controllers
{
    public class GlobalSearchController : BaseController
    {
        private readonly List<UrlValidationRule> _validationRules;
        private readonly ISkipperHome _homePageService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GlobalSearchController(ISkipperHome homePageService, IConfiguration configuration, MenuDataService menuService, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(homePageService, menuService, httpContextAccessor)
        {
            _homePageService = homePageService;
            _validationRules = configuration.GetSection("UrlValidationRules:Rules").Get<List<UrlValidationRule>>() ?? new();
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index", "SkipperHome");

            var result = await _homePageService.GetsearchList(q);

            ViewBag.Query = q;
            return View("~/Views/GlobalSearch/SearchResult.cshtml", result);
        }
    }
}
