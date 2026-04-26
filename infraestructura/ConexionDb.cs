using System;
using System.Data.SqlClient;
using System.Configuration;

namespace infraestructura
{
    public class ConexionDb
    {
        private readonly string _connectionString;

        public ConexionDb()
        {
            _connectionString = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            
            return new SqlConnection(_connectionString);
            
        }
    }
}
