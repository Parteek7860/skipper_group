namespace skipper_group_new.Models
{
    public class clsmainmenu
    {
        public string Title { get; set; }
        public string linkname { get; set; }
        public string pageid { get; set; }
        public string pagename { get; set; }
        public string rewriteurl { get; set; }
        public string pageurl { get; set; }

        public string tagline { get; set; }
        public string Url_name { get; set; }

        public string Moduleid { get; set; }
        public string modulename { get; set; }

        public string pareentcode { get; set; }

        public List<clsmainmenu> Forms { get; set; } = new List<clsmainmenu>();
    }
}
