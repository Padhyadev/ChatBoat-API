using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.TalkToExpertRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalkToExpertController : ControllerBase
    {
        private readonly ITalkToExpertRepository _repository;
        private readonly CommonViewModel _response = new();

        public TalkToExpertController(ITalkToExpertRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            int start = 0,
            int length = 10,
            string sortColumn = "",
            string sortColumnDir = "asc",
            string searchValue = "")
        {
            try
            {
                _response.Data = await _repository.GetAll(
                    start, length, sortColumn, sortColumnDir, searchValue);

                _response.IsSuccess = true;
                _response.StatusCode = ResponseStatusCode.Success;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = ResponseStatusCode.Error;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(TalkToExpert model)
        {
            return await _repository.AddOrUpdate(model);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var data = await _repository.GetById(id);

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

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            return await _repository.Delete(id);
        }
    }
}
