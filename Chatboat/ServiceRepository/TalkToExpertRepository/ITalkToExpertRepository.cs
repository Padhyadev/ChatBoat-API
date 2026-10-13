using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.TalkToExpertRepository
{
    public interface ITalkToExpertRepository
    {
        Task<PagedResult<TalkToExpert>> GetAll(
    int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdate(TalkToExpert model);

        Task<TalkToExpert?> GetById(long id);

        Task<JsonResult> Delete(long id);
    }
}
