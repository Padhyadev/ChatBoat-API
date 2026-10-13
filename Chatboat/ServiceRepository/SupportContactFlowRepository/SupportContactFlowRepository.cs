using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.SupportContactFlowRepository
{
    public class SupportContactFlowRepository : ISupportContactFlowRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<SupportContactFlow> _repositoryBase;

        public SupportContactFlowRepository(
            Datacontext context,
            IRepositoryBase<SupportContactFlow> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<SupportContactFlow>> GetAll(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_SupportContactFlow_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue);

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdate(SupportContactFlow model)
        {
            var outputParam = new SqlParameter("@response", SqlDbType.NVarChar, 200)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
            new SqlParameter("@Id", model.Id),
            new SqlParameter("@EnquiryType", model.EnquiryTypeId),
            new SqlParameter("@LeadId", model.LeadId),
            new SqlParameter("@Description", model.Description ?? (object)DBNull.Value),
            new SqlParameter("@Contact", model.Contact ?? (object)DBNull.Value),
            new SqlParameter("@Action", model.Id == 0 ? "INSERT" : "UPDATE"),
            outputParam
        };

            _repositoryBase.ExecuteStoredProcedure(
                "SP_SupportContactFlow_Save", parameters);

            return new JsonResult(outputParam.Value?.ToString());
        }

        public async Task<SupportContactFlow?> GetById(long id)
        {
            List<SupportContactFlow> list = new();

            using SqlConnection con =
                new(_context.Database.GetConnectionString());

            using SqlCommand cmd =
                new("SP_SupportContactFlow_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string json = JsonConvert.SerializeObject(dt);
            list = JsonConvert.DeserializeObject<List<SupportContactFlow>>(json);

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> Delete(long id)
        {
            var outputParam = new SqlParameter("@Response", SqlDbType.NVarChar, 200)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new[]
            {
            new SqlParameter("@Id", id),
            outputParam
        };

            _repositoryBase.ExecuteStoredProcedure(
                "SP_SupportContactFlow_Delete", parameters);

            return new JsonResult(outputParam.Value?.ToString());
        }
    }
}
