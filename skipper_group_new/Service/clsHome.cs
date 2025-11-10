using System.Data;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using skipper_group_new.Service;

namespace skipper_group_new.Service
{
    public class clsHome : IHome
    {

        private readonly IHomeRepository _repository;

        public clsHome(IHomeRepository repository)
        {
            _repository = repository;
        }
        public string Title { get; set; } = "Welcome to skipper_group_new";

        public Task<DataTable> GetCMSDetail(int id)
        {
            return _repository.GetCMSDetail(id);
        }

        public Task<List<clsmainmenu>> GetMainManu()
        {
            return _repository.GetMainMenu();
        }

        public Task<string> GetPageDescription(int id)
        {
            return _repository.GetPageDescription(id);
        }
        public Task<string> GetSmallDescription(int id)
        {
            return _repository.GetSmallDescription(id);
        }
        public Task<List<clsHomeBanner>> GetHomeBanner()
        {
            return _repository.GetHomeBanner();
        }
        public int SaveContactDetails(clsContact objML_contact)
        {
            return _repository.SaveContactDetails(objML_contact);
        }
        public Task<List<clsHomeModel>> NewsEventsList()
        {
            return _repository.NewsEventsList();
        }
        public Task<List<clsHomeModel>> ProductList(bool s)
        {
            return _repository.ProductList(s);
        }
        public Task<List<clsHomeModel>> Products()
        {
            return _repository.Products();
        }
        
    }
}
