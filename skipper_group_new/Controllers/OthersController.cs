using skipper_group_new.Interface;
using skipper_group_new.mainclass;
using skipper_group_new.Service;
using Microsoft.AspNetCore.Mvc;

namespace skipper_group_new.Controllers
{
    public class OthersController : Controller
    {
        private readonly IManagement _management;
        private readonly clsMainMenuList _menuService;

        public OthersController(clsMainMenuList menuService, IManagement management)
        {
            _management = management;
            _menuService = menuService;
        }

        [HttpGet]
        [Route("/backoffice/others/viewenquiry")]
        public async Task<IActionResult> viewenquiry()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var enquiry = await _management.GetEnquiry();
            var sortedList = enquiry.OrderByDescending(x => x.trdate).ToList();
            ViewBag.Enquiry = sortedList;
            return View("~/Views/backoffice/others/viewenquiry.cshtml");
        }

        [HttpGet]
        [Route("/backoffice/others/viewproductenquiry")]
        public async Task<IActionResult> viewproductenquiry()
        {
            var menuList = _menuService.GetMenu();
            ViewBag.Menus = menuList;
            var pEnquery = await _management.GetProductEnquiry();
            ViewBag.ProductEnquiry = pEnquery;
            return View("~/Views/backoffice/others/viewproductenquiry.cshtml");
        }
        [HttpGet]
        [Route("backoffice/others/ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var catdtl = await _management.GetEnquiry();
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Categories");
                
                worksheet.Cell(1, 1).Value = "Full Name";
                worksheet.Cell(1, 2).Value = "Email ID";
                worksheet.Cell(1, 3).Value = "Mobile";
                worksheet.Cell(1, 4).Value = "Company";
                worksheet.Cell(1, 5).Value = "Message";
                worksheet.Cell(1, 6).Value = "Date";
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
                    
                    worksheet.Cell(row, 1).Value = c.FName ?? "";
                    worksheet.Cell(row, 2).Value = c.EmailId ?? "";
                    worksheet.Cell(row, 3).Value = c.phone ?? "";
                    worksheet.Cell(row, 4).Value = c.OrganizationName ?? "";
                    worksheet.Cell(row, 5).Value = c.FMessage ?? "";
                    
                    worksheet.Cell(row, 6).Value = c.trdate;
                    worksheet.Cell(row, 6).Style.DateFormat.Format = "yyyy-MM-dd";
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
