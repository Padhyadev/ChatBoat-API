using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.SubServiceRepository
{
    public interface ISubServiceRepository
    {
        Task<PagedResult<SubService>> GetAllSubService(
    int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdateSubService(SubService subService);

        Task<SubService?> GetSubServiceById(long id);

        Task<JsonResult> DeleteBySubServiceId(long id, long operatedBy);




        Task<List<SubService>> Subservicefilterbyservice(long ServiceId);
    }
}
