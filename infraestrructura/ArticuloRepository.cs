using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using dominio;

namespace infraestructura
{
    public class ArticuloRepository
    {
        private readonly ConexionDb _factory;

        public ArticuloRepository(ConexionDb factory)
        {
            _factory = factory;
        }

        public List<Articulo> GetAll()
        {
            var lista = new List<Articulo>();

            using var conn = _factory.CreateConnection();
            conn.Open();

            var query = "SELECT Id, Codigo, Nombre, Descripcion, Precio FROM Articulos";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Articulo
                {
                    Id = (int)reader["Id"],
                    Codigo = reader["Codigo"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    Precio = (decimal)reader["Precio"]
                });
            }

            return lista;
        }

        public void Add(Articulo art)
        {
            using var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"INSERT INTO Articulos 
                      (Codigo, Nombre, Descripcion, Precio) 
                      VALUES (@codigo, @nombre, @desc, @precio)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@codigo", art.Codigo);
            cmd.Parameters.AddWithValue("@nombre", art.Nombre);
            cmd.Parameters.AddWithValue("@desc", art.Descripcion);
            cmd.Parameters.AddWithValue("@precio", art.Precio);

            cmd.ExecuteNonQuery();
        }
    }
}
