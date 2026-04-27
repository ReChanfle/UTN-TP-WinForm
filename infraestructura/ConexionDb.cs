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
         ?.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new Exception("No se encontró la cadena de conexión DefaultConnection. Por favor cargue una conexion valida a la base de datos");
        }

        public SqlConnection CreateConnection()
        {
            
            return new SqlConnection(_connectionString);
            
        }
    }
}
