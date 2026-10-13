using Chatboat.Infra;
using Chatboat.Models;

namespace Chatboat.ServiceRepository.MasterServicesRepository
{
    public interface IMasterServicesRepository
    {
        Task<PagedResult<MasterServices>> GetAllmasterServices( int start, int length, string sortColumn, string sortColumnDir, string searchValue);

    }
}
