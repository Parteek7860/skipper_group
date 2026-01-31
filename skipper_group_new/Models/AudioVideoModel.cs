namespace skipper_group_new.Models
{
    public class AudioVideoModel
    {
        public int AudioId { get; set; }
        public string AudioTitle { get; set; }
        public string? AudioPath { get; set; }
        public int? displayOrder { get; set; }
        public bool Status { get; set; }
        public  DateTime trdate { get; set; }
        public int Mode { get; set; }
    }
}
