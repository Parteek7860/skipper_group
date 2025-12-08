using DocumentFormat.OpenXml.EMMA;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;

namespace skipper_group_new.Service
{
    public interface IManagement
    {
        Task<List<TeamTypeDtl>> GetTeamTblData();
        Task<int> AddTeamType(clsTeamType team);
        Task<clsTeamType> EditTeamType(int id);
        Task<int> DeleteTeamType(int id);

        Task<List<ManagementDtl>> GetTeamManagementData();
        Task<int> AddTeam(Management m);
        Task<Management> EditTeam(int id);
        Task<int> DeleteTeam(int id);

        Task<Dictionary<int, string>> GetTeamDropdown();
        Task<Dictionary<int, string>> GetSubTeamDropdown();
        Task<int> ChangeStatus(int id);
        Task<int> ChangeManStatus(int id);

        Task<List<EnquiryModel>> GetEnquiry();
        Task<List<ProductEnquiryModel>> GetProductEnquiry();

        Task<int> AddEditJob(PostJobModel m);
        Task<List<PostJobModel>> GetJobList();

        Task<DataTable> GetProductSolutionList();
        Task<PostJobModel> GetJobPostById(int jobID);
        Task<int> Delete(int jobID);
        Task<List<Applicationview>> GetApplicantsDetail();

        
        Task<int> DeleteStore(int storeID);
        Task<int> ChangeDelerStatus(int storeId);

        Task<List<clsReview>> GetReviewData();
        int UpdateReviewStatus(string status, int id);
        int AddReview(clsReview obj);
        Task<int> SaveApplication(JobApplicationModel model);
        

        Task<Application> GetApplicantsDetailByID(int App_id);

        
    }
    public class ManagementService : IManagement
    {
        private readonly IManagementRepo _repository;
        

        public ManagementService(IManagementRepo repository)
        {
            _repository = repository;
          
        }

        public Task<List<TeamTypeDtl>> GetTeamTblData()
        {
            return _repository.GetTeamTblData();
        }

        public Task<int> AddTeamType(clsTeamType team)
        {
            return _repository.AddTeamType(team);
        }

        public Task<clsTeamType> EditTeamType(int id)
        {
            return _repository.EditTeamType(id);
        }

        public Task<int> DeleteTeamType(int id)
        {
            return _repository.DeleteTeamType(id);
        }


        public Task<List<ManagementDtl>> GetTeamManagementData()
        {
            return _repository.GetTeamManagementData();
        }

        public Task<int> AddTeam(Management m)
        {
            return _repository.AddTeam(m);
        }

        public Task<Management> EditTeam(int id)
        {
            return _repository.EditTeam(id);
        }

        public Task<int> DeleteTeam(int id)
        {
            return _repository.DeleteTeam(id);
        }

        public Task<Dictionary<int, string>> GetTeamDropdown()
        {
            return _repository.GetTeamDropdown();
        }

        public Task<Dictionary<int, string>> GetSubTeamDropdown()
        {
            return _repository.GetSubTeamDropdown();
        }

        public Task<int> ChangeStatus(int id)
        {
            return _repository.ChangeStatus(id);
        }

        public Task<int> ChangeManStatus(int id)
        {
            return _repository.ChangeManStatus(id);
        }

        public async Task<List<EnquiryModel>> GetEnquiry()
        {
            return await _repository.GetEnquiry();
        }
        public async Task<List<ProductEnquiryModel>> GetProductEnquiry()
        {
            return await _repository.GetProductEnquiry();
        }

        public async Task<int> AddEditJob(PostJobModel m)
        {
            var res = await _repository.AddEditJob(m);
            return res;
        }

        public async Task<List<PostJobModel>> GetJobList()
        {
            return await _repository.GetJobList();
        }

        public async Task<DataTable> GetProductSolutionList()
        {
            return await _repository.GetProductSolutionList();
        }

        public async Task<PostJobModel> GetJobPostById(int jobID)
        {
            return await _repository.GetJobPostById(jobID);
        }
        public async Task<int> Delete(int jobID)
        {
            return await _repository.Delete(jobID);
        }
        public async Task<List<Applicationview>> GetApplicantsDetail()
        {
            return await _repository.GetApplicantsDetail();
        }

      

        public async Task<int> DeleteStore(int storeID)
        {
            return await _repository.DeleteStore(storeID);
        }

        public async Task<int> ChangeDelerStatus(int storeId)
        {
            return await _repository.ChangeDelerStatus(storeId);
        }


        public async Task<List<clsReview>> GetReviewData()
        {
            return await _repository.GetReviewData();
        }

        public int UpdateReviewStatus(string status, int id)
        {
            return _repository.UpdateReviewStatus(status, id);
        }

        public int AddReview(clsReview obj)
        {
            return _repository.AddReview(obj);
        }

        public async Task<int> SaveApplication(JobApplicationModel model)
        {
            return await _repository.SaveApplication(model);
        }

    
        public async Task<Application> GetApplicantsDetailByID(int App_id)
        {
            return await _repository.GetApplicantsDetailByID(App_id);
        }

      
    }
}
