using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Interface
{
    //Rakesh Chauhan - 12/06/2024 - Backoffice Project Service Interface Created
    public interface IBacofficeProject
    {
        Task<DataTable> GetProduct();
        Task<DataTable> GetCategory();
        Task<int> AddUpdateProject(ResearchModel research);
        Task<DataTable> GetProjectData();
        Task<int> ExecuteProjectAction(int researchId, int mode);
    }
}
