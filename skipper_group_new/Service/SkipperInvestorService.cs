using skipper_group_new.Interface;
using skipper_group_new.Models;
using skipper_group_new.Repositories;
using System.Data;

namespace skipper_group_new.Service
{
    public class SkipperInvestorService: ISkipperInvestorPage
    {
        // RAKESH CHAUHAN - 19/11/2025
        private readonly ISkipperInvestorRepo _skipperInvestorRepo;
        public SkipperInvestorService(ISkipperInvestorRepo skipperInvestorRepo)
        {
            _skipperInvestorRepo = skipperInvestorRepo;
        }

        public async Task<DataTable> GetCategoryItem()
        {
            return await _skipperInvestorRepo.GetCategoryItem();
        }

        public async Task<DataTable> GetSubCategoryItem(int id)
        {
            return await _skipperInvestorRepo.GetSubCategoryItem(id);
        }

        public async Task<DataTable> GetReports(int pcatid, int psubcatid)
        {
            return await _skipperInvestorRepo.GetReports(pcatid, psubcatid);
        }

        public async Task<DataTable> GetCategoryDetail(int pcatid)
        {
            return await _skipperInvestorRepo.GetCategoryDetail(pcatid);
        }
    }
}
