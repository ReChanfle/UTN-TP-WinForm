using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
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

        public void Update(Articulo art)
        {
            var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"UPDATE Articulos SET
                    Codigo = @codigo,
                    Nombre = @nombre,
                    Descripcion = @desc,
                    Precio = @precio
                  WHERE Id = @id";

            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@codigo", art.Codigo);
            cmd.Parameters.AddWithValue("@nombre", art.Nombre);
            cmd.Parameters.AddWithValue("@desc", art.Descripcion);
            cmd.Parameters.AddWithValue("@precio", art.Precio);
            cmd.Parameters.AddWithValue("@id", art.Id);

            cmd.ExecuteNonQuery();
        }
        public List<Articulo> GetAll()
        {
            var lista = new List<Articulo>();

            var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"
                        SELECT 
                            A.Id,
                            A.Codigo,
                            A.Nombre,
                            A.Descripcion,
                            M.Id AS IdMarca,
                            M.Descripcion AS Marca,
                            C.Id AS IdCategoria,
                            C.Descripcion AS Categoria,
                            A.Precio
                        FROM ARTICULOS A
                        LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                        LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id";

            var cmd = new SqlCommand(query, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Articulo
                {
                    Id = (int)reader["Id"],
                    Codigo = reader["Codigo"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    Precio = (decimal)reader["Precio"],
                    Marca = reader["Marca"].ToString(),
                    Categoria = reader["Categoria"].ToString()
                });
            }

            return lista;
        }

        public List<Articulo> Filtrar(int idMarca, int idCategoria)
        {
            List<Articulo> lista = new List<Articulo>();

            var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"
                    SELECT 
                        A.Id,
                        A.Codigo,
                        A.Nombre,
                        A.Descripcion,
                        M.Id AS IdMarca,
                        M.Descripcion AS Marca,
                        C.Id AS IdCategoria,
                        C.Descripcion AS Categoria,
                        A.Precio
                    FROM ARTICULOS A
                    LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                    LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                    WHERE (@IdMarca = 0 OR A.IdMarca = @IdMarca)
                    AND (@IdCategoria = 0 OR A.IdCategoria = @IdCategoria)";

            SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@IdMarca", idMarca);
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                lista.Add(new Articulo
                {
                    Id = (int)reader["Id"],
                    Codigo = reader["Codigo"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    Descripcion = reader["Descripcion"].ToString(),
                    Precio = (decimal)reader["Precio"],
                    Marca = reader["Marca"].ToString(),
                    Categoria = reader["Categoria"].ToString()
                });
            }
     

            return lista;
        }

        public void Add(Articulo art)
        {
            var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"INSERT INTO Articulos 
                      (Codigo, Nombre, Descripcion, Precio) 
                      VALUES (@codigo, @nombre, @desc, @precio)";

            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@codigo", art.Codigo);
            cmd.Parameters.AddWithValue("@nombre", art.Nombre);
            cmd.Parameters.AddWithValue("@desc", art.Descripcion);
            cmd.Parameters.AddWithValue("@precio", art.Precio);

            cmd.ExecuteNonQuery();
        }
    }
}
