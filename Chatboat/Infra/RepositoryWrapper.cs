using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Chatboat.Infra
{
    public class RepositoryWrapper
    {
        public interface IRepositoryBase<T>
        {
            public dynamic ExecuteStoredProcedure(string query, SqlParameter[] parameters = null);

            PagedResult<T> ExecuteWithPagination(
                string query,
                SqlParameter[] parameters = null,
                int start = 0,
                int length = 10,
                string sortColumn = "",
                string sortColumnDir = "asc",
                string searchValue = "");

            //dropdwon
            List<T> ExecuteForDropdown(string query, SqlParameter[] parameters = null);

        }
        public class RepositoryBase<T> : IRepositoryBase<T> where T : class
        {
            private static string _connectionString;
            public RepositoryBase(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DataConnection");
            }

            public dynamic ExecuteStoredProcedure(string query, SqlParameter[] parameters = null)
            {
                try
                {
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();

                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = query;

                        if (parameters != null && parameters.Count() > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }
                        cmd.CommandTimeout = 120;
                        cmd.ExecuteNonQuery();
                        cmd.CommandTimeout = 120;

                        SqlParameterCollection paramCollection = cmd.Parameters;

                        for (int i = 0; i < paramCollection.Count; i++)
                        {
                            if (paramCollection[i].Direction == ParameterDirection.Output)
                                ((IDictionary<String, Object>)exo).Add(paramCollection[i].ParameterName.Replace("@", ""), paramCollection[i].Value);
                        }
                    }

                    if (((IDictionary<String, Object>)exo) != null && ((IDictionary<String, Object>)exo).Count == 1 && ((IDictionary<String, Object>)exo).ContainsKey("response"))
                    {
                        return Convert.ToString(((IDictionary<String, Object>)exo).FirstOrDefault().Value);
                    }

                    return exo;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public PagedResult<T> ExecuteWithPagination(
                string query,
                SqlParameter[] parameters = null,
                int start = 0,
                int length = 10,
                string sortColumn = "",
                string sortColumnDir = "asc",
                string searchValue = "")
            {
                try
                {
                    List<T> list = new List<T>();

                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (parameters != null && parameters.Any())
                            {
                                foreach (var param in parameters)
                                {
                                    cmd.Parameters.Add(param);
                                }
                            }

                            using (var reader = cmd.ExecuteReader())
                            {
                                var dt = new DataTable();
                                dt.Load(reader);

                                string json = JsonConvert.SerializeObject(dt);
                                list = JsonConvert.DeserializeObject<List<T>>(json);
                            }
                        }
                    }

                    var recordsTotal = list.Count;

                    if (!string.IsNullOrEmpty(searchValue))
                        list = list.Where(x => JsonConvert.SerializeObject(x)
                            .ToLower().Contains(searchValue.ToLower())).ToList();

                    if (!string.IsNullOrEmpty(sortColumn))
                    {
                        list = sortColumnDir.ToLower() == "asc"
                            ? list.OrderBy(x => x.GetType().GetProperty(sortColumn)?.GetValue(x, null)).ToList()
                            : list.OrderByDescending(x => x.GetType().GetProperty(sortColumn)?.GetValue(x, null)).ToList();
                    }

                    var pagedData = list.Skip(start).Take(length).ToList();

                    return new PagedResult<T>
                    {
                        StartIndex = start,
                        Length = length,
                        RecordsFiltered = list.Count,
                        RecordsTotal = recordsTotal,
                        Data = pagedData
                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //  New method for Dropdowns 
            public List<T> ExecuteForDropdown(string query, SqlParameter[] parameters = null)
            {
                List<T> list = new List<T>();

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (parameters != null && parameters.Any())
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            var dt = new DataTable();
                            dt.Load(reader);

                            string json = JsonConvert.SerializeObject(dt);
                            list = JsonConvert.DeserializeObject<List<T>>(json);
                        }
                    }
                }

                return list;
            }
        }
    }
}
