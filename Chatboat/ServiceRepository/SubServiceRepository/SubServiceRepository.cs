using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.SubServiceRepository
{
    public class SubServiceRepository : ISubServiceRepository
    {

        private readonly Datacontext _context;
        private readonly IRepositoryBase<SubService> _repositoryBase;

        public SubServiceRepository(Datacontext context, IRepositoryBase<SubService> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<SubService>> GetAllSubService(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_SubService_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdateSubService(SubService subService)
        {
            try
            {
                var outputParam = new SqlParameter("@response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@SubServiceId", subService.SubServiceId),
                    new SqlParameter("@SubServiceName", subService.SubServiceName),
                    new SqlParameter("@ServiceId", subService.ServiceId),
                    new SqlParameter("@Action", subService.SubServiceId == 0 ? "INSERT" : "UPDATE"),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure("SP_SubService_Save", parameters);

                return new JsonResult(outputParam.Value?.ToString());
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

        public async Task<SubService?> GetSubServiceById(long id)
        {
            List<SubService> list = new();

            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("SP_SubService_Get", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SubServiceId", id));

                using var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);

                list = JsonConvert.DeserializeObject<List<SubService>>(JsonConvert.SerializeObject(dt));
            }

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteBySubServiceId(long id, long operatedBy)
        {
            try
            {

                var outputParam = new SqlParameter("@Response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@SubServiceId", id),
                    new SqlParameter("@Operated_By", operatedBy),
                    outputParam };


                _repositoryBase.ExecuteStoredProcedure("sp_SubService_Delete", parameters);



                return new JsonResult(outputParam.Value?.ToString());
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



        public async Task<List<SubService>> Subservicefilterbyservice(long ServiceId)
        {
            try
            {
                List<SubService> list = new List<SubService>();

                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_SubServiceFilterbyserviceId_Get", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SubserviceId", SqlDbType.BigInt)
                        {
                            Value = ServiceId == 0 ? DBNull.Value : ServiceId
                        });
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            string json = JsonConvert.SerializeObject(dt);
                            list = JsonConvert.DeserializeObject<List<SubService>>(json) ?? new List<SubService>();
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
