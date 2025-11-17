using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Interface
{
    public interface IBackofficePage
    {        
        Task<DataTable> GetAlbumTypeList();
        Task<DataTable> GetPageList();

        Task<DataTable> GetAlbumTypeListByID();

        Task<DataTable> GetAlbumList();
        int AddAlbum(clsGallery objgallery);
        int DeleteAlbum(int id);
        Task<DataTable> GetAlbumListByID(int id);
        int UpdateAlbumStatus(string status, int id);

        Task<DataTable> BindVedioList();
        int UpdateVedioStatus(string status, int id);
        int DeleteVedio(int id);
        Task<DataTable> BindPageList();
        Task<DataTable> GetPageListByID(int id);
        int DeleteRecords(int id);
        int AddCMS(clsCMS obj);
        int CMSUpdateStatus(string status, int id);
        Task<DataTable> GetFAQList();
        Task<int> AddFAQ(clsfaq obj);
        int FAQDeleteRecords(int id);
        int FAQUpdateStatus(string status, int id);
        Task<DataTable> GetImageFilePathList();

        Task<DataTable> BindVedioTVCList();
        int UpdateTvcStatus(string status, int id);
        Task<int> AddTvc(clsGallery objgallery);
        Task<int> AddVedio(clsGallery objgallery);
        int DeleteTvc(int id);
        //Rakesh 12/11/2025
        Task<int> AddAlbumPhoto(clsGallery objgallery);
        Task<DataTable> BindPhotoGallaryList(int mode);

        Task<int> DeletePhotoGallary(int id);
        Task<int> changestatus(int id);
    }
}
