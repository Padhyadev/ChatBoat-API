using Chatboat.Models;

namespace Chatboat.ServiceRepository.LeadMasterDataREpository
{
    public interface ILeadMasterDataREpository
    {
        Task<List<LeadMasterData>> GetAllLeadMasterData();



    }
}
