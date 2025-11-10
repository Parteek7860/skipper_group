using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class ProductTyre
    {
        public int ProductTyreId { get; set; }

        [Required(ErrorMessage = "Please select a tyre title.")]
        public string ProductTyreTitle { get; set; }

        public string ShortDesc { get; set; }
        public string DetailDesc { get; set; }

        [Required(ErrorMessage = "Please select a tyre tagline.")]
        public string Tagline { get; set; }

        public string Review { get; set; }
        [Required(ErrorMessage = "Please select a product type.")]
        public int ProductTypeId { get; set; }
        [Required(ErrorMessage = "Please select a vehicle type.")]
        public int VehicleTypeId { get; set; }
        [Required(ErrorMessage = "Please select a tyre type.")]
        public int TyreTypeId { get; set; }
        [Required(ErrorMessage = "Please select a posting type.")]
        public int PostingId { get; set; }
        public string? UploadImage { get; set; }
        public string? UploadFile { get; set; }
        public string? RewriteUrl { get; set; }
        public string? RewriteUrlSec { get; set; }
        public int? DisplayOrder { get; set; }
        public string? PageTitle { get; set; }
        public string? PageMeta { get; set; }
        public string? PageMetaDesc { get; set; }
        public bool? Status { get; set; }
        public bool? showOnHome {  get; set; }
        public string? UName { get; set; }
        public DateTime? trdate { get; set; }
        public string? ThumbnailImage { get; set; }   
        public string? PageScript { get; set; }
        public string? canonical {  get; set; }
        public bool NoIndexFollow { get; set; }
        public string Features { get; set; }
        public string DesignType { get; set; }
        public string SizeTitle { get; set; }
        public string? DetailImage { get; set; }
        public int? Mode { get; set; }        
    }

    public class ProductDrop
    {
        public int product_typeid {  get; set; }
        public string product_typetitle {  get; set; }
    }
    public class VehicleDrop
    {
        public int vehicle_typeid { get; set; }
        public string vehicle_typetitle { get; set; }
    }
    public class TyreDrop
    {
        public int tyre_typeid { get; set; }
        public string tyre_typetitle { get; set; }
    }
    public class Position
    {
        public int postingid { get; set; }
        public string postingtitle { get; set; }
    }
    public class Design
    {
        public int designtypeid { get; set; }
        public string designtypetitle { get; set; }
    }

    public class PTyreView
    {
        public int ProductTyreId { get; set; }
        public string Title { get; set; }
        public string TyreSize { get; set; }
        
        public string ProductType { get; set; }
        public string VehicleType { get; set; }
        public string TyreType { get; set; }
        public string Posting { get; set; }
        public string Design { get; set; }
        public bool Status { get; set; }

    }

    public class BrandListModel
    {
        public int brandID { get; set; }
        public string brandTitle { get; set; }               
    }

    public class MapingModel
    {
        public int Projid { get; set; }
        public int brandid { get; set; }
        public int modelid { get; set; }
        public int blogoid { get; set; }
    }
    public class ProductBrands
    {
        public int brandID { get; set; }
        public string brandTitle { get; set; }   
        public List<BrandModel> Model { get;set; }
    }

    public class BrandModel
    {
        public int modelID { get; set; }
        public int BrandId { get; set; }
        public int BlogoId { get; set; }   
        public int ProjId { get; set; }
        public string modeTitle { get; set; }   
        public bool Status { get; set; }
    }
       

    public class UTyrePhoto
    {
        public int PhotoID { get; set; }

        public int ProductTyreID { get; set; }

        public string? PhotoTitle { get; set; }

        public string? UploadPhoto { get; set; }

        public bool Status { get; set; }

        public string Uname { get; set; } = string.Empty;

        public DateTime TrDate { get; set; }

        public int DisplayOrder { get; set; }

        public string LargeImage { get; set; } = string.Empty;

        public int? SizeID { get; set; }
        public int Mode { get; set; }   
    }

    public class MappingDetail
    {
        public int MModelId { get; set; }      
        public int ProjId { get; set; }        
        public int BrandId { get; set; }       
        public int ModelId { get; set; }       
        public int BlogoId { get; set; }       
        public string UName { get; set; } 
        public int? DesignTypeId { get; set; } 
    }

    public class Tyre
    {
        public int id { get; set; }
        public string Title { get; set; }
        public int productid { get; set; }
    }
}
