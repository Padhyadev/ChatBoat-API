//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Chatboat.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProjectQuotesController : ControllerBase
//    {
//    }
//}


using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.ProjectQuotesRepository;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectQuotesController : ControllerBase
    {
        private readonly IProjectQuotesRepository _repository;
        private readonly CommonViewModel CommonViewModel = new();

        public ProjectQuotesController(IProjectQuotesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAllpage(
            int start = 0,
            int length = 10,
            string sortColumn = "",
            string sortColumnDir = "asc",
            string searchValue = "")
        {
            try
            {
                var data = await _repository.GetAllProjectQuotes(
                    start, length, sortColumn, sortColumnDir, searchValue);

                CommonViewModel.IsSuccess = true;
                CommonViewModel.StatusCode = ResponseStatusCode.Success;
                CommonViewModel.Data = data;
            }
            catch (Exception ex)
            {
                CommonViewModel.IsSuccess = false;
                CommonViewModel.StatusCode = ResponseStatusCode.Error;
                CommonViewModel.Message = ex.Message;
            }

            return Ok(CommonViewModel);
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(ProjectQuotes quote)
        {
            return await _repository.AddOrUpdateProjectQuotes(quote);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var data = await _repository.GetProjectQuoteById(id);

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

        [HttpDelete("[Action]")]
        public async Task<IActionResult> Delete(long id, long operatedBy)
        {
            return await _repository.DeleteByProjectQuoteId(id, operatedBy);
        }
    }
}
