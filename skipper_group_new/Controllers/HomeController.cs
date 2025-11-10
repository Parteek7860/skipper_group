using skipper_group_new.Interface;
using skipper_group_new.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Data;
using System.Net;

namespace skipper_group_new.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IHome _homeService;
        clsmainmenu objmainmenu = new clsmainmenu();
        public HomeController(IHome homeService)
        {
            _homeService = homeService;
        }

        public async Task BindMenu()
        {
            var content = _homeService.GetMainManu();

            ViewBag.Content = content;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var content = _homeService.GetMainManu();
            ViewBag.Content = content;
            await homebanner();
            await CMSLandingPage();
            await NewsEventsList();
            await ProductList(true);
            return View("Index", content);
        }
       
        [HttpGet]
        [Route("Product")]
        public async Task<IActionResult> Product()
        {
            var content = await _homeService.GetMainManu();
            ViewBag.Content = content;
            var desc = await _homeService.GetSmallDescription(3);
            ViewBag.Description = WebUtility.HtmlDecode(desc);
            await ProductList(false);
            return View();
        }
        [HttpGet]
        [Route("product/{i}")]
        public async Task<IActionResult> Product(int i)
        {
            var content = await _homeService.GetMainManu();
            ViewBag.Content = content;
            ViewBag.ProductId = i;
            var desc = await _homeService.GetSmallDescription(3);
            ViewBag.Description = WebUtility.HtmlDecode(desc);
            await ProductList(false);

            return View();
        }
        // Contact Page
        [HttpGet]
        [Route("Contact")]
        public async Task<IActionResult> contact()
        {
            ViewBag.Content = "";
            var content = await _homeService.GetMainManu();
            ViewBag.Content = content;
            await bindcmslayoutdata(6);
            var desc = await _homeService.GetPageDescription(6);
            ViewBag.Description = WebUtility.HtmlDecode(desc);
            return View();
        }
       
        [HttpGet]
        [Route("Home/cpage/{i}")]
        public async Task<IActionResult> cpage(int i)
        {
            var content = _homeService.GetMainManu();
            ViewBag.Content = content;
            if (i > 0)
            {
                var desc = await _homeService.GetPageDescription(i);
                ViewBag.Description = WebUtility.HtmlDecode(desc);
                await bindcmslayoutdata(i);
                return View(objmainmenu);
               
            }
            return View("cpage", content); 
        }
        private async Task bindcmslayoutdata(int id)
        {
            DataTable dt = await _homeService.GetCMSDetail(id);
            if (dt != null)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string title = dt.Rows[0]["pagetitle"].ToString();
                    string description = dt.Rows[0]["tagline"].ToString();

                    ViewBag.Title = title;
                    ViewBag.tagline = description;

                    objmainmenu.linkname = title;
                    objmainmenu.tagline = description;

                }
            }

        }

        [HttpGet]
        [Route("Home/aboutus/{i}")]
        public async Task<IActionResult> AboutUs(int i)
        {
            ViewBag.Content = "";
            var content = await _homeService.GetMainManu();
            ViewBag.Content = content;
            if (i > 0)
            {
                var desc = await _homeService.GetPageDescription(i);
                ViewBag.Description = WebUtility.HtmlDecode(desc);               
            }
            return View();
        }

        [HttpGet]
        [Route("Home/news&events/{i}")]
        public async Task<IActionResult> NewsEvents(int i)
        {
            ViewBag.Content = "";
            var content = await _homeService.GetMainManu();
            ViewBag.Content = content;
            if (i > 0)
            {
                var desc = await _homeService.GetPageDescription(i);
                ViewBag.Description = WebUtility.HtmlDecode(desc);
            }
            return View();
        }

        // Home Banner
        [HttpGet]
        public async Task<IActionResult> homebanner()
        {
            var content = await _homeService.GetHomeBanner();
            ViewBag.Bannerlist = content;
            return View();
        }
        // CMS Landing Page
        public async Task<IActionResult> CMSLandingPage()
        {
            var desc = await _homeService.GetSmallDescription(1);
            ViewBag.SmallDesc = WebUtility.HtmlDecode(desc);

            var desc1 = await _homeService.GetPageDescription(1);
            ViewBag.PageDescription = WebUtility.HtmlDecode(desc1);

            //var Sustainability = await _homeService.GetSmallDescription(5);
            //ViewBag.Sustainability = WebUtility.HtmlDecode(Sustainability);

            //var Capabilities = await _homeService.GetSmallDescription(4);
            //ViewBag.Capabilities = WebUtility.HtmlDecode(Capabilities);

            return View();
        }
        // Contact Form Submission
        [HttpPost]
        [Route("contact")]
        public ActionResult Contact(clsContact contact)
        {
            var content = _homeService.GetMainManu();
            ViewBag.Content = content;
            if (ModelState.IsValid)
            {
                clsContact objML_contact = new clsContact();

                objML_contact.title = contact.title;
                objML_contact.Email = contact.Email;
                objML_contact.Phone = contact.Phone; ;
                objML_contact.Message = contact.Message;
                objML_contact.country = contact.country;
                objML_contact.Company = contact.Company;


                int x = _homeService.SaveContactDetails(objML_contact); 
                if (x > 0)
                {
                    return RedirectToAction("Thankyou", "home");
                }
                else
                {
                    ViewBag.Message = "There was an error processing your request. Please try again later.";
                }
            }
            else
            {
                ViewBag.Message = "Please fill in all required fields.";
                return View();

            }
            return View("/Views/home/contact.cshtml");
        }

        public IActionResult Thankyou()
        {
            var content = _homeService.GetMainManu();
            ViewBag.Content = content;
            return View();
        }
        
        public async Task<IActionResult> NewsEventsList()
        {
            var contentnews = _homeService.NewsEventsList();
            ViewBag.NewsEvents = contentnews;
            return View();
        }
       
        public async Task<IActionResult> ProductList(bool s)
        {
            var contentproduct = _homeService.ProductList(s);
            ViewBag.ProductList = contentproduct;
            if (ViewBag.ProductId != null)
            {
                ViewBag.CategorySmallDesc = contentproduct.Result.Where(p => p.Id == ViewBag.ProductId).Select(p => p.Description).FirstOrDefault(); 
                ViewBag.CategoryName = contentproduct.Result.Where(p => p.Id == ViewBag.ProductId).Select(p => p.Title).FirstOrDefault();
            }
            var productdetail = _homeService.Products();
            ViewBag.ProductDetail = productdetail;

            var chunkedProducts = productdetail.Result.Select((p, index) => new { Index = index, Value = p })
        .GroupBy(x => x.Index / 5)
        .Select(g => g.Select(x => x.Value).ToList())
        .ToList();

            ViewBag.ProductChunks = chunkedProducts;
            return View();
        }

    }
}
