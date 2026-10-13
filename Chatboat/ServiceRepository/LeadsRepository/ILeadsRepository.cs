using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.LeadsRepository
{
    public interface ILeadsRepository
    {
        Task<PagedResult<Leads>> GetAllLeads(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdateLeads(Leads lead);

        Task<Leads?> GetLeadById(long id);

        Task<JsonResult> DeleteByLeadId(long id, long operatedBy);
    }
}
