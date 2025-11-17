using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;

namespace skipper_group_new.Service
{
    public class clsBackofficePage : IBackofficePage
    {
        private readonly IBackofficePageRepository _repository;
        public clsBackofficePage(IBackofficePageRepository repository)
        {
            _repository = repository;
        }
        public Task<DataTable> GetAlbumTypeList()
        {
            return _repository.GetAlbumTypeList();
        }
        public int AddAlbum(clsGallery objgallery)
        {
            return _repository.AddAlbum(objgallery);
        }
        public Task<DataTable> GetAlbumList()
        {
            return _repository.GetAlbumList();
        }
        public int DeleteAlbum(int id)
        {
            return _repository.DeleteAlbum(id);
        }
        public Task<DataTable> GetAlbumListByID(int id)
        {
            return _repository.GetAlbumListByID(id);
        }
        public int UpdateAlbumStatus(string status, int id)
        {
            return _repository.UpdateAlbumStatus(status, id);
        }
        public int UpdateVedioStatus(string status, int id)
        {
            return _repository.UpdateVedioStatus(status, id);
        }
        public int DeleteVedio(int id)
        {
            return _repository.DeleteVedio(id);
        }
        public Task<DataTable> GetAlbumTypeListByID()
        {
            return _repository.GetAlbumTypeListByID();
        }
        public Task<DataTable> BindVedioList()
        {
            return _repository.BindVedioList();
        }
        public Task<DataTable> GetPageList()
        {
            return _repository.GetPageList();
        }
        public Task<DataTable> BindPageList()
        {
            return _repository.BindPageList();
        }
        public int AddCMS(clsCMS obj)
        {
            return _repository.AddCMS(obj);
        }
        public Task<DataTable> GetPageListByID(int id)
        {
            return _repository.GetPageListByID(id);
        }
        public int DeleteRecords(int obj)
        {
            return _repository.DeleteRecords(obj);
        }

        public int CMSUpdateStatus(string status, int id)
        {
            return _repository.CMSUpdateStatus(status, id);
        }
        public Task<DataTable> GetFAQList()
        {
            return _repository.GetFAQList();
        }
        public Task<int> AddFAQ(clsfaq obj)
        {
            return _repository.AddFAQ(obj);
        }
        public int FAQDeleteRecords(int obj)
        {
            return _repository.FAQDeleteRecords(obj);
        }

        public int FAQUpdateStatus(string status, int id)
        {
            return _repository.FAQUpdateStatus(status, id);
        }
        public Task<DataTable> GetImageFilePathList()
        {
            return _repository.GetImageFilePathList();
        }
        public Task<DataTable> BindVedioTVCList()
        {
            return _repository.BindVedioTVCList();
        }
        public int UpdateTvcStatus(string status, int id)
        {
            return _repository.UpdateTvcStatus(status, id);
        }
        public Task<int> AddTvc(clsGallery obj)
        {
            return _repository.AddTvc(obj);
        }

        public int DeleteTvc(int id)
        {
            return _repository.DeleteTvc(id);
        }
        public Task<int> AddVedio(clsGallery obj)
        {
            return _repository.AddVedio(obj);
        }
        //Rakesh 12/11/2025
        public async Task<int> AddAlbumPhoto(clsGallery objgallery)
        {
            return await _repository.AddAlbumPhoto(objgallery);
        }

        public Task<DataTable> BindPhotoGallaryList(int mode)
        {
            return _repository.BindPhotoGallaryList(mode);
        }

        public async Task<int> DeletePhotoGallary(int id)
        {
            return await _repository.DeletePhotoGallary(id);
        }

        public async Task<int> changestatus(int id)
        {
            return await _repository.changestatus(id);
        }
    }
}
