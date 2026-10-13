//namespace Chatboat.ServiceRepository.ProjectQuotesRepository
//{
//    public class ProjectQuotesRepository
//    {
//    }
//}



using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.ProjectQuotesRepository
{
    public class ProjectQuotesRepository : IProjectQuotesRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<ProjectQuotes> _repositoryBase;

        public ProjectQuotesRepository(
            Datacontext context,
            IRepositoryBase<ProjectQuotes> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<ProjectQuotes>> GetAllProjectQuotes(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_ProjectQuotes_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdateProjectQuotes(ProjectQuotes quote)
        {
            try
            {
                var output = new SqlParameter("@Response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@ProjectQuoteId", quote.ProjectQuoteId),
                    new SqlParameter("@LeadId", quote.LeadId),
                    new SqlParameter("@ProjectName", quote.ProjectName ?? (object)DBNull.Value),
                    new SqlParameter("@ProjectDescription", quote.ProjectDescription ?? (object)DBNull.Value),
                    new SqlParameter("@Budget", quote.Budget ?? (object)DBNull.Value),
                    new SqlParameter("@Timeline", quote.Timeline ?? (object)DBNull.Value),
                    new SqlParameter("@Action", quote.ProjectQuoteId == 0 ? "INSERT" : "UPDATE"),
                    output
                };

                _repositoryBase.ExecuteStoredProcedure(
                    "SP_ProjectQuotes_Save",
                    parameters
                );

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

        public async Task<ProjectQuotes?> GetProjectQuoteById(long id)
        {
            List<ProjectQuotes> list = new();

            using SqlConnection con = new(_context.Database.GetConnectionString());
            con.Open();

            using SqlCommand cmd = new("SP_ProjectQuotes_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProjectQuoteId", id));

            using var reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);

            list = JsonConvert.DeserializeObject<List<ProjectQuotes>>(
                JsonConvert.SerializeObject(dt)
            );

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteByProjectQuoteId(long id, long operatedBy)
        {
            try
            {
                var outputParam = new SqlParameter("@Response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@ProjectQuoteId", id),
                    new SqlParameter("@Operated_By", operatedBy),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure(
                    "SP_ProjectQuotes_Delete",
                    parameters
                );

                var response = outputParam.Value?.ToString() ?? "{}";
                return new JsonResult(response);
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
