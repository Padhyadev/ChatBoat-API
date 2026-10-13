using Chatboat.Infra;
using Chatboat.ServiceRepository.LeadMasterDataREpository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadMasterData : ControllerBase
    {
        private readonly ILeadMasterDataREpository _leadMasterDataREpository;
        private readonly CommonViewModel CommonViewModel = new();

        public LeadMasterData(ILeadMasterDataREpository dashboard)
        {
            _leadMasterDataREpository = dashboard;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllLeadMasterData()
        {
            try
            {
                var data = await _leadMasterDataREpository.GetAllLeadMasterData();

                if (data != null)
                {
                    CommonViewModel.IsSuccess = true;
                    CommonViewModel.StatusCode = ResponseStatusCode.Success;
                    CommonViewModel.Data = data;
                }
                else
                {
                    CommonViewModel.IsSuccess = false;
                    CommonViewModel.StatusCode = ResponseStatusCode.NotFound;
                    CommonViewModel.Message = ResponseStatusMessage.NotFound;
                }
            }
            catch (Exception ex)
            {
                CommonViewModel.IsSuccess = false;
                CommonViewModel.StatusCode = ResponseStatusCode.Error;
                CommonViewModel.Message = ex.Message;

            }
            return Ok(CommonViewModel);
        }
    }
}
