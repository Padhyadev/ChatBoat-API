using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.TalkToExpertRepository
{
    public class TalkToExpertRepository : ITalkToExpertRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<TalkToExpert> _repositoryBase;

        public TalkToExpertRepository(
            Datacontext context,
            IRepositoryBase<TalkToExpert> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<TalkToExpert>> GetAll(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_TalkToExpert_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue);

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdate(TalkToExpert model)
        {
            try
            {
                var outputParam = new SqlParameter("@response", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@Email", model.Email ?? (object)DBNull.Value),
                    new SqlParameter("@PreferredCallTime", model.PreferredCallTime ?? (object)DBNull.Value),
                    new SqlParameter("@LeadId", model.LeadId ?? (object)DBNull.Value),
                    new SqlParameter("@Action", model.Id == 0 ? "INSERT" : "UPDATE"),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure(
                    "SP_TalkToExpert_Save", parameters);

                return new JsonResult(outputParam.Value?.ToString());
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

        public async Task<TalkToExpert?> GetById(long id)
        {
            List<TalkToExpert> list = new();

            using SqlConnection con =
                new(_context.Database.GetConnectionString());

            using SqlCommand cmd =
                new("SP_TalkToExpert_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            var dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            string json = JsonConvert.SerializeObject(dt);
            list = JsonConvert.DeserializeObject<List<TalkToExpert>>(json);

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> Delete(long id)
        {
            try
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
                    "SP_TalkToExpert_Delete", parameters);

                return new JsonResult(outputParam.Value?.ToString());
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
