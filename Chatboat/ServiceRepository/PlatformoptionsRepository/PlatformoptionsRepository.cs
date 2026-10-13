using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Chatboat.Infra.RepositoryWrapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Chatboat.ServiceRepository.PlatformoptionsRepository
{
    public class PlatformoptionsRepository : IPlatformoptionsRepository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<Platformoptions> _repositoryBase;

        public PlatformoptionsRepository(Datacontext context, IRepositoryBase<Platformoptions> repositoryBase)
        {
            _context = context;
            _repositoryBase = repositoryBase;
        }

        public async Task<PagedResult<Platformoptions>> GetAllPlatformoptions(
            int start, int length, string sortColumn, string sortColumnDir, string searchValue)
        {
            SqlParameter[] parameters = null;

            var result = _repositoryBase.ExecuteWithPagination(
                "SP_Platformoptions_Get",
                parameters,
                start,
                length,
                sortColumn,
                sortColumnDir,
                searchValue
            );

            return await Task.FromResult(result);
        }

        public async Task<JsonResult> AddOrUpdatePlatformoptions(Platformoptions platformoptions)
        {
            try
            {
                var outputParam = new SqlParameter("@response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
                {
                    new SqlParameter("@PlatformoptionsId", platformoptions.PlatformoptionsId),
                    new SqlParameter("@PlatformoptionsName", platformoptions.PlatformoptionsName),
                    new SqlParameter("@SubServiceId", platformoptions.SubServiceId),
                    new SqlParameter("@Action", platformoptions.PlatformoptionsId == 0 ? "INSERT" : "UPDATE"),
                    outputParam
                };

                _repositoryBase.ExecuteStoredProcedure("SP_Platformoptions_Save", parameters);

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

        public async Task<Platformoptions?> GetPlatformoptionsById(long id)
        {
            List<Platformoptions> list = new();

            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("SP_Platformoptions_Get", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PlatformoptionsId", id));

                using var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);

                list = JsonConvert.DeserializeObject<List<Platformoptions>>(JsonConvert.SerializeObject(dt));
            }

            return list.FirstOrDefault();
        }

        public async Task<JsonResult> DeleteByPlatformoptionsId(long id, long operatedBy)
        {
            try
            {
                var outputparam = new SqlParameter("@Response", SqlDbType.VarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new[]
               {
                    new SqlParameter("@LeadId", id),
                    new SqlParameter("@Operated_By", operatedBy),
                    outputparam
                };

                _repositoryBase.ExecuteStoredProcedure("sp_Leads_Delete", parameters);

                return new JsonResult(outputparam.Value?.ToString());
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



        public async Task<List<Platformoptions>> PlatformoptionsbyService(long ServiceId)
        {
            try
            {
                List<Platformoptions> list = new List<Platformoptions>();

                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SP_PlatformOptionsbyservice_Get", con))
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
                            list = JsonConvert.DeserializeObject<List<Platformoptions>>(json) ?? new List<Platformoptions>();
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
