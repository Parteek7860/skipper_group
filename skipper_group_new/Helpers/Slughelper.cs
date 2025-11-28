using System.Text.RegularExpressions;

namespace skipper_group_new.Helpers
{
    public class Slughelper
    {
        public static string ToSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            return "";
            string slug = input.Trim().ToLower();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');
            return slug;
        }

    }
}
