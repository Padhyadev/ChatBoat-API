using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.ServicesRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly CommonViewModel _commonViewModel = new();

        public ServicesController(IServicesRepository servicesRepository)
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
                var data = await _servicesRepository.GetAllServices(
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

        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(Services service)
        {
            return await _servicesRepository.AddOrUpdateServices(service);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var data = await _servicesRepository.GetServiceById(id);

                if (data != null)
                {
                    _commonViewModel.IsSuccess = true;
                    _commonViewModel.StatusCode = ResponseStatusCode.Success;
                    _commonViewModel.Data = data;
                }
                else
                {
                    _commonViewModel.IsSuccess = false;
                    _commonViewModel.StatusCode = ResponseStatusCode.NotFound;
                    _commonViewModel.Message = ResponseStatusMessage.NotFound;
                }
            }
            catch (Exception ex)
            {
                _commonViewModel.IsSuccess = false;
                _commonViewModel.StatusCode = ResponseStatusCode.Error;
                _commonViewModel.Message = ex.Message;
            }

            return Ok(_commonViewModel);
        }

        [HttpDelete("[Action]")]
        public async Task<IActionResult> Delete(long id, long operatedBy)
        {
            return await _servicesRepository.DeleteByServiceId(id, operatedBy);
        }

    }
}
