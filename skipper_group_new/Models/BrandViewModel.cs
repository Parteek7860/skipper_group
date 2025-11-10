namespace skipper_group_new.Models
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }

    public class VehicleModel
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public class FilterRequestModel
    {
        public List<int> VehicleIds { get; set; } = new();
        public List<int> BrandIds { get; set; } = new();
        public List<int> ModelIds { get; set; } = new();
        public List<int> TyreTypeIds { get; set; } = new();
        public List<int> PositionIds { get; set; } = new();
        public List<string> DesignTypes { get; set; } = new();
    }

    public class TyreProductModel
    {
        public int ProductTyreId { get; set; }
        public string ProductTyreTitle { get; set; }
        public string UploadImage { get; set; }
        public string ThumbnailImage { get; set; }
        public string TyreTypeTitle { get; set; }
        public int TyreTypeId { get; set; }
        public int VehicleTypeId { get; set; }
        public string SizeTitle { get; set; }
        public int DisplayOrder { get; set; }
    }
}
