//namespace Chatboat.ServiceRepository.EnquiryTypeRepository
//{
//    public interface IEnquiryTypeRepository
//    {
//    }
//}



using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.EnquiryTypeRepository
{
    public interface IEnquiryTypeRepository
    {
        Task<PagedResult<EnquiryType>> GetAllEnquiryTypes(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdateEnquiryType(EnquiryType enquiryType);

        Task<EnquiryType?> GetEnquiryTypeById(long id);

        Task<JsonResult> DeleteByEnquiryTypeId(long id);
    }
}
