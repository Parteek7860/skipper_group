namespace skipper_group_new.Models
{
    public class clsRedirection
    {
        public int Id { get; set; }
        public string OldUrl { get; set; } = string.Empty;
        public string NewUrl { get; set; } = string.Empty;
        public string Url { get; set; }
        public bool status { get; set; }
        public string redirect_type { get; set; }

    }
}
