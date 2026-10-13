//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Chatboat.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EnquiryTypeController : ControllerBase
//    {
//    }
//}



using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.EnquiryTypeRepository;
using Microsoft.AspNetCore.Mvc;

namespace Chatboat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryTypeController : ControllerBase
    {
        private readonly IEnquiryTypeRepository _enquiryTypeRepository;
        private readonly CommonViewModel CommonViewModel = new();

        public EnquiryTypeController(IEnquiryTypeRepository enquiryTypeRepository)
        {
            _enquiryTypeRepository = enquiryTypeRepository;
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
                var data = await _enquiryTypeRepository.GetAllEnquiryTypes(
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
        public async Task<IActionResult> Add(EnquiryType enquiryType)
        {
            return await _enquiryTypeRepository.AddOrUpdateEnquiryType(enquiryType);
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var data = await _enquiryTypeRepository.GetEnquiryTypeById(id);

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
        public async Task<IActionResult> Delete(long id)
        {
            return await _enquiryTypeRepository.DeleteByEnquiryTypeId(id);
        }
    }
}
