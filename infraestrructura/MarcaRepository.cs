using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using dominio;

namespace infraestructura
{
    public class MarcaRepository
    {
        private readonly ConexionDb _factory; 

        public MarcaRepository(ConexionDb factory)
        {
            _factory = factory;
        }

        public List<Marca> GetAll()
        {
            var lista = new List<Marca>();

            using var conn = _factory.CreateConnection();
            conn.Open();

            var cmd = new SqlCommand("SELECT Id, Descripcion FROM Marcas", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Marca
                {
                    Id = (int)reader["Id"],
                    Descripcion = reader["Descripcion"].ToString()
                });
            }

            return lista;
        }
    }
}
