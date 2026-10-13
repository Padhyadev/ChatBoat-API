//namespace Chatboat.ServiceRepository.EnquiryTypeRepository
//{
//    public class EnquiryTypeRepository
//    {
//    }
//}


using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using static Chatboat.Infra.RepositoryWrapper;

namespace Chatboat.ServiceRepository.EnquiryTypeRepository
{
    public class EnquiryTypeRepository : IEnquiryTypeRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<EnquiryType> _repositoryBase;

        public EnquiryTypeRepository(Datacontext context, IRepositoryBase<EnquiryType> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<EnquiryType>> GetAllEnquiryTypes(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_EnquiryType_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdateEnquiryType(EnquiryType enquiryType)
        {
            try
            {
                var output = new SqlParameter("@response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@EnquiryTypeId", enquiryType.EnquiryTypeId),
                    new SqlParameter("@EnquiryTypeName", enquiryType.EnquiryTypeName ?? (object)DBNull.Value),
                    new SqlParameter("@IsActive", enquiryType.IsActive),
                    new SqlParameter("@Action", enquiryType.EnquiryTypeId == 0 ? "INSERT" : "UPDATE"),
                    output
                };

                _repositoryBase.ExecuteStoredProcedure("SP_EnquiryType_Save", parameters);
                return new JsonResult(output.Value?.ToString());
            }
            catch (Exception ex)
            {
                return new JsonResult(new CommonViewModel
                {
                    IsSuccess = false,
                    StatusCode = ResponseStatusCode.Error,
                    Message = ex.Message
                });
            }
        }

        public async Task<EnquiryType?> GetEnquiryTypeById(long id)
        {
            List<EnquiryType> list = new();

            using SqlConnection con = new(_context.Database.GetConnectionString());
            con.Open();

            using SqlCommand cmd = new("SP_EnquiryType_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EnquiryTypeId", id));

            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);

            list = JsonConvert.DeserializeObject<List<EnquiryType>>(JsonConvert.SerializeObject(dt));
            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteByEnquiryTypeId(long id)
        {
            try
            {
                var outputParam = new SqlParameter("@response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@EnquiryTypeId", id),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure("SP_EnquiryType_Delete", parameters);

                var response = outputParam.Value?.ToString() ?? "{}";
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                var response = new CommonViewModel
                {
                    IsSuccess = false,
                    StatusCode = ResponseStatusCode.Error,
                    Message = ex.Message
                };

                return new JsonResult(response);
            }
        }
    }
}
