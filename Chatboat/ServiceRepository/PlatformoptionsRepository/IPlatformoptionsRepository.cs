using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.ServiceRepository.PlatformoptionsRepository
{
    public interface IPlatformoptionsRepository
    {
        Task<PagedResult<Platformoptions>> GetAllPlatformoptions(
    int start, int length, string sortColumn, string sortColumnDir, string searchValue);

        Task<JsonResult> AddOrUpdatePlatformoptions(Platformoptions platformoptions);

        Task<Platformoptions?> GetPlatformoptionsById(long id);

        Task<JsonResult> DeleteByPlatformoptionsId(long id, long operatedBy);


        Task<List<Platformoptions>> PlatformoptionsbyService(long ServiceId);

    }
}
