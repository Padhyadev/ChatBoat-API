using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.PlatformoptionsRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformoptionsController : ControllerBase
    {
        private readonly IPlatformoptionsRepository _repository;
        private readonly CommonViewModel _response = new();

        public PlatformoptionsController(IPlatformoptionsRepository repository)
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
                _response.Data = await _repository.GetAllPlatformoptions(start, length, sortColumn, sortColumnDir, searchValue);
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
        public async Task<IActionResult> Add(Platformoptions model)
        {
            return await _repository.AddOrUpdatePlatformoptions(model);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetById(long id)
        {
            var data = await _repository.GetPlatformoptionsById(id);
            return Ok(data);
        }

        [HttpDelete("[Action]")]
        public async Task<IActionResult> Delete(long id, long operatedBy)
        {
            return await _repository.DeleteByPlatformoptionsId(id, operatedBy);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> PlatformoptionsbyService(long ServiceId)
        {
            try
            {
                var data = await _repository.PlatformoptionsbyService(ServiceId);

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
