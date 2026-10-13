using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.SubServiceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubServiceController : ControllerBase
    {

        private readonly ISubServiceRepository _repository;
        private readonly CommonViewModel _response = new();

        public SubServiceController(ISubServiceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllpage(
            int start = 0, int length = 10,
            string sortColumn = "", string sortColumnDir = "asc",
            string searchValue = "")
        {
            try
            {
                _response.IsSuccess = true;
                _response.StatusCode = ResponseStatusCode.Success;
                _response.Data = await _repository.GetAllSubService(start, length, sortColumn, sortColumnDir, searchValue);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = ResponseStatusCode.Error;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(SubService model)
        {
            return await _repository.AddOrUpdateSubService(model);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _repository.GetSubServiceById(id);
            return Ok(data);
        }

        [HttpDelete("[Action]")]
        public async Task<IActionResult> Delete(long id, long operatedBy)
        {
            return await _repository.DeleteBySubServiceId(id, operatedBy);
        }


        [HttpGet("[Action]")]
        public async Task<IActionResult> Subservicefilterbyservice(long ServiceId)
        {
            try
            {
                var data = await _repository.Subservicefilterbyservice(ServiceId);

                if (data != null)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = ResponseStatusCode.Success;
                    _response.Data = data;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = ResponseStatusCode.NotFound;
                    _response.Message = ResponseStatusMessage.NotFound;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = ResponseStatusCode.Error;
                _response.Message = ex.Message;

            }

            return Ok(_response);
        }


    }
}
