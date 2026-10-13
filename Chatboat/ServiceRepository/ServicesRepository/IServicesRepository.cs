using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Chatboat.ServiceRepository.ServicesRepository
{
    public interface IServicesRepository
    {
        Task<PagedResult<Services>> GetAllServices(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdateServices(Services service);

        Task<Services?> GetServiceById(long id);

        Task<JsonResult> DeleteByServiceId(long id, long operatedBy);

    }
}
