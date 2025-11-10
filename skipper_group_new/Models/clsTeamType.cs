namespace skipper_group_new.Models
{
    public class clsTeamType
    {
        public int TTypeId { get; set; }
        public string TType { get; set; }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }
        public string ShortDesc { get; set; }
        public int CollageId { get; set; }
        public string UName { get; set; }
    }

    public class TeamTypeDtl
    {
        public int TTypeId { get; set; }
        public string TType { get; set; }
        public string ShortDesc { get; set; }
        public string TrDate {  get; set; }
        public bool Status { get; set; }
    }
}
