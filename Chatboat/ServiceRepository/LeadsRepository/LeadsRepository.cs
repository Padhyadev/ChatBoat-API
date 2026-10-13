using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.LeadsRepository
{ 
    public class LeadsRepository : ILeadsRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<Leads> _repositoryBase;

        public LeadsRepository(Datacontext context, IRepositoryBase<Leads> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<Leads>> GetAllLeads(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_Leads_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdateLeads(Leads lead)
        {
            try
            {
                var output = new SqlParameter("@response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@LeadId", lead.LeadId),
                    new SqlParameter("@Name", lead.Name?? (object)DBNull.Value),
                    new SqlParameter("@Phone", lead.Phone ?? (object)DBNull.Value),
                    new SqlParameter("@ActionInput", lead.Action ?? (object)DBNull.Value),
                    new SqlParameter("@Service", lead.Service ?? (object)DBNull.Value),
                    new SqlParameter("@SubService", lead.SubService ?? (object)DBNull.Value),
                    new SqlParameter("@FollowUp", lead.FollowUp ?? (object)DBNull.Value),
                    new SqlParameter("@PlatformOpation", lead.PlatformOpation ?? (object)DBNull.Value),
                    new SqlParameter("@ServiceMasterId", lead.ServiceMasterId ?? (object)DBNull.Value),
                    new SqlParameter("@Action", lead.LeadId == 0 ? "INSERT" : "UPDATE"),
                    output
                };

                _repositoryBase.ExecuteStoredProcedure("SP_Leads_Save", parameters);
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

        public async Task<Leads?> GetLeadById(long id)
        {
            List<Leads> list = new();

            using SqlConnection con = new(_context.Database.GetConnectionString());
            con.Open();

            using SqlCommand cmd = new("SP_Leads_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@LeadId", id));

            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);

            list = JsonConvert.DeserializeObject<List<Leads>>(JsonConvert.SerializeObject(dt));
            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteByLeadId(long id, long operatedBy)
        {
            try
            {
                var outputParam = new SqlParameter("@Response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@LeadId", id),
                    new SqlParameter("@Operated_By", operatedBy),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure("sp_Leads_Delete", parameters);

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
