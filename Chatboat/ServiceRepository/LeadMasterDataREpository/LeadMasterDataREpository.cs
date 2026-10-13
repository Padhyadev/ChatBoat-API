using Chatboat.Infra;
using Chatboat.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using static Chatboat.Infra.RepositoryWrapper;

namespace Chatboat.ServiceRepository.LeadMasterDataREpository
{
    public class LeadMasterDataREpository : ILeadMasterDataREpository
    {
        private readonly Datacontext _context;
        private readonly IRepositoryBase<LeadMasterData> _Properties;

        public LeadMasterDataREpository(Datacontext context,
            IRepositoryBase<LeadMasterData> Propertiedash)

        {
            _context = context;
            _Properties = Propertiedash;
        }



        public async Task<List<LeadMasterData>> GetAllLeadMasterData()
        {
            try
            {
                List<LeadMasterData> result = new();

                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(
                        "SP_LeadMasterdata_Get", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            string json = JsonConvert.SerializeObject(dt);

                            result = JsonConvert
                                .DeserializeObject<List<LeadMasterData>>(json)
                                ?? new List<LeadMasterData>();
                        }
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }


    }
}
