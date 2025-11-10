namespace skipper_group_new.Models
{
    public class UrlValidationRule
    {
        public string Type { get; set; } = "Block";   // Block or Allow
        public string Pattern { get; set; } = string.Empty;
        public string Action { get; set; } = "RedirectToError"; // e.g., RedirectToError, Continue
    }
}
