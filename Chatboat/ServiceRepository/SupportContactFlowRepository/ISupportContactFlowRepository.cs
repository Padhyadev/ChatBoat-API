using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.SupportContactFlowRepository
{
    public interface ISupportContactFlowRepository
    {
        Task<PagedResult<SupportContactFlow>> GetAll(
    int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdate(SupportContactFlow model);

        Task<SupportContactFlow?> GetById(long id);

        Task<JsonResult> Delete(long id);
    }
}
