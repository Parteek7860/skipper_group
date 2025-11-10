using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    public interface IBackofficePageRepository
    {
        Task<DataTable> GetAlbumTypeList();
        Task<DataTable> GetPageList();
        Task<DataTable> GetAlbumList();
        int AddAlbum(clsGallery objgallery);
        int DeleteAlbum(int id);
        Task<DataTable> GetAlbumListByID(int id);
        int UpdateAlbumStatus(string status, int id);
        Task<DataTable> GetAlbumTypeListByID();

        Task<DataTable> BindVedioList();
        int UpdateVedioStatus(string status, int id);
        int DeleteVedio(int id);

        Task<DataTable> BindPageList();
        int AddCMS(clsCMS obj);

        Task<DataTable> GetPageListByID(int id);

        int DeleteRecords(int id);
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
    }

}
