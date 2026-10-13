using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chatboat.ServiceRepository.ServicesRepository
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<Services> _repositoryBase;

        public ServicesRepository(
            Datacontext context,
            IRepositoryBase<Services> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<Services>> GetAllServices(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var pagedResult = _repositoryBase.ExecuteWithPagination(
                "SP_Services_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(pagedResult);
        }

        public async Task<JsonResult> AddOrUpdateServices(Services service)
        {
            try
            {
                var outputParam = new SqlParameter("@Response", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@Service_Id", service.Service_Id),
                    new SqlParameter("@Service_Name", service.Service_Name ?? (object)DBNull.Value),
                    new SqlParameter("@Status", service.Status),
                    new SqlParameter("@Display_Seq_No", service.Display_Seq_No),
                    //new SqlParameter("@Operated_By",service.Service_Id == 0 ? service.CreatedBy : service.LastModifiedBy),
                    new SqlParameter("@Action", service.Service_Id == 0 ? "INSERT" : "UPDATE"),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure("SP_Services_Save", parameters);

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

        public async Task<Services?> GetServiceById(long id)
        {
            List<Services> list = new();

            using (SqlConnection con =
                new SqlConnection(_context.Database.GetConnectionString()))
            {
                con.Open();

                using SqlCommand cmd = new("SP_Services_Get", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Service_Id", id));

                using var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);

                string json = JsonConvert.SerializeObject(dt);
                list = JsonConvert.DeserializeObject<List<Services>>(json);
            }

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteByServiceId(long id, long operatedBy)
        {
            try
            {
                long deletedby = operatedBy;
                var outputParam = new SqlParameter("@Response", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@Service_Id", id),
                    new SqlParameter("@Operated_By",  deletedby),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure(
                    "SP_Services_Delete", parameters);

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
    }
}
