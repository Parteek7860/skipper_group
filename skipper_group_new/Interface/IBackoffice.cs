using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Interface
{
    public interface IBackoffice
    {
        Task<DataTable> GetAlbumTypeList();

        Task<DataTable> GetAlbumTypeListByID();
        int AddAlbum(clsGallery objgallery);

        int DeleteAlbum(int id);
    }
}
