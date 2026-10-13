using Chatboat.Infra;
using Chatboat.ServiceRepository.MasterServicesRepository;
using Chatboat.ServiceRepository.ServicesRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterServicesController : ControllerBase
    {
        private readonly IMasterServicesRepository _servicesRepository;
        private readonly CommonViewModel _commonViewModel = new();

        public MasterServicesController(IMasterServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllpage(
            int start = 0, int length = 10,
            string sortColumn = "",
            string sortColumnDir = "asc",
            string searchValue = "")
        {
            try
            {
                var data = await _servicesRepository.GetAllmasterServices(
                    start, length, sortColumn, sortColumnDir, searchValue);

                _commonViewModel.IsSuccess = true;
                _commonViewModel.StatusCode = ResponseStatusCode.Success;
                _commonViewModel.Data = data;
            }
            catch (Exception ex)
            {
                _commonViewModel.IsSuccess = false;
                _commonViewModel.StatusCode = ResponseStatusCode.Error;
                _commonViewModel.Message = ex.Message;
            }
            return Ok(_commonViewModel);
        }
    }
}
