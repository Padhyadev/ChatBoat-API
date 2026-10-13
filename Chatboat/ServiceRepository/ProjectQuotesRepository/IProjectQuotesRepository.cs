//namespace Chatboat.ServiceRepository.ProjectQuotesRepository
//{
//    public interface IProjectQuotesRepository
//    {
//    }
//}


using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.ProjectQuotesRepository
{
    public interface IProjectQuotesRepository
    {
        Task<PagedResult<ProjectQuotes>> GetAllProjectQuotes(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdateProjectQuotes(ProjectQuotes quote);

        Task<ProjectQuotes?> GetProjectQuoteById(long id);

        Task<JsonResult> DeleteByProjectQuoteId(long id, long operatedBy);
    }
}
