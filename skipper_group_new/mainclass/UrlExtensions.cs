using System.Text.RegularExpressions;

namespace skipper_group_new.mainclass
{
    public static class UrlExtensions
    {
        public static string ToSlug(this string title)
        {
            if (string.IsNullOrEmpty(title)) return "";
            var slug = title.Trim().Replace(" ", "-");
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9\-]", "");
            return slug;
        }
    }
}
