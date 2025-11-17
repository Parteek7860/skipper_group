using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;

namespace skipper_group_new.Service
{
    //Rakesh Chauhan - 12/06/2024 - Backoffice Project Service Created
    public class clsBackofficeProject: IBacofficeProject
    {
        private readonly IBackofficeProjectRepository _repository;
        public clsBackofficeProject(IBackofficeProjectRepository repository)
        {
            _repository = repository;
        }
        public async Task<DataTable> GetProduct()
        {
           return await _repository.GetProduct();
        }
        public async Task<DataTable> GetCategory()
        {
            return await _repository.GetCategory();
        }

        public async Task<int> AddUpdateProject(ResearchModel research)
        {
            return await _repository.AddUpdateProject(research);
        }

        public async Task<DataTable> GetProjectData()
        {
            return await _repository.GetProjectData();
        }

        public async Task<int> ExecuteProjectAction(int researchId, int mode)
        {
            return await _repository.ExecuteProjectAction(researchId, mode);
        }
    }
}
