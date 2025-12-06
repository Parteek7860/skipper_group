using skipper_group_new.Models;
using System.Data;
using skipper_group_new.Interface;

namespace skipper_group_new.mainclass
{
    public class clsMainMenuList
    {
        private readonly IHomePage _homePageService;
        public int parentcode = 0;
        public clsMainMenuList(IHomePage homePageService)
        {
            _homePageService = homePageService;
        }

        public List<clsmainmenu> GetMenu(int? pageid = null)
        {
            var menuList = _homePageService.GetMenuList().Result;

            if (pageid.HasValue && pageid != 0)
            {
                var rows = menuList.AsEnumerable()
                                   .Where(r => r.Field<int>("pareentcode") == 1);
                menuList = rows.Any() ? rows.CopyToDataTable() : menuList.Clone();
            }



            var menus = new List<clsmainmenu>();

            if (menuList != null && menuList.Rows.Count > 0)
            {
                foreach (DataRow row in menuList.Rows)
                {
                    var menuItem = new clsmainmenu
                    {
                        Title = row["moduleid"].ToString(),
                        linkname = row["modulename"].ToString(),
                        pareentcode = row["pareentcode"].ToString(),
                        Forms = new List<clsmainmenu>()
                    };

                    var formList = _homePageService.GetFormList(row["moduleid"].ToString()).Result;
                    if (formList != null && formList.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in formList.Rows)
                        {
                            menuItem.Forms.Add(new clsmainmenu
                            {
                                Moduleid = row1["formid"].ToString(),
                                modulename = row1["formcaption"].ToString(),
                                Url_name = "/backoffice/" + row1["formname"].ToString().Replace(".aspx", "")
                            });
                        }
                    }
                    menus.Add(menuItem);
                }
            }
            return menus;
        }
    }
}
