using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.Data.SqlClient;
using static Chatboat.Infra.RepositoryWrapper;

namespace Chatboat.ServiceRepository.MasterServicesRepository
{
    public class MasterServicesRepository : IMasterServicesRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<MasterServices> _repositoryBase;

        public MasterServicesRepository(
            Datacontext context,
            IRepositoryBase<MasterServices> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<MasterServices>> GetAllmasterServices(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var pagedResult = _repositoryBase.ExecuteWithPagination(
                "SP_MasterServices_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(pagedResult);

        }
    }
}
