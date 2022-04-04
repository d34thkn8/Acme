using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ACME.ENCUESTAS.API.Models
{
    public class DBConnection
    {
        public string Default { get; set; }

        public SqlConnection Connection { get; set; }

        private void OpenConnectAsync()
        {
            Connection = new SqlConnection(connectionString: Default);
            Connection.Open();
        }

        public void ValidarConnection()
        {
            if (Connection == null)
            {
                OpenConnectAsync();
            }
            if (Connection.State == System.Data.ConnectionState.Closed)
                OpenConnectAsync();
        }
        public void CloseConnection()
        {
            if (Connection != null)
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                    Connection.Dispose();
                }

            }
        }

        public async Task<SqlDataReader> EjecutarCommand
            (List<SqlParameter> sqlParameter, string sp)
        {

            using var command = new SqlCommand(sp, Connection);
            if (sqlParameter.Count > 0)
            {
                foreach (var item in sqlParameter)
                {
                    command.Parameters.AddWithValue(item.ParameterName, item.Value);

                }
            }
            command.CommandType = System.Data.CommandType.StoredProcedure;

            return await command.ExecuteReaderAsync();


        }
    }
}
