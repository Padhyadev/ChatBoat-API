using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.SupportContactFlowRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportContactFlowController : ControllerBase
    {
        private readonly ISupportContactFlowRepository _repo;
        private readonly CommonViewModel _response = new();

        public SupportContactFlowController(ISupportContactFlowRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            int start = 0, int length = 10,
            string sortColumn = "", string sortColumnDir = "asc",
            string searchValue = "")
        {
            _response.Data = await _repo.GetAll(
                start, length, sortColumn, sortColumnDir, searchValue);

            _response.IsSuccess = true;
            _response.StatusCode = ResponseStatusCode.Success;

            return Ok(_response);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(SupportContactFlow model)
        {
            return await _repo.AddOrUpdate(model);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            _response.Data = await _repo.GetById(id);
            _response.IsSuccess = true;
            _response.StatusCode = ResponseStatusCode.Success;

            return Ok(_response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            return await _repo.Delete(id);
        }
    }
}
