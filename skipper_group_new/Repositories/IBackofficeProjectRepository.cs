using skipper_group_new.Models;
using System.Data;

namespace skipper_group_new.Repositories
{
    //Rakesh Chauhan - 14/11/2025 - Backoffice Project Repository Interface Created
    public interface IBackofficeProjectRepository
    {
        Task<DataTable> GetProduct();
        Task<DataTable> GetCategory();
        Task<int> AddUpdateProject(ResearchModel research);
        Task<DataTable> GetProjectData();
        Task<int> ExecuteProjectAction(int researchId, int mode);

        //Rakesh Chauhan - 17/11/2025 - CategoryModule
        Task<int> AddUpdateCategory(clsCategory category);

        Task<int> ExecuteCategoryAction(int pcatid, int mode);
    }
}
