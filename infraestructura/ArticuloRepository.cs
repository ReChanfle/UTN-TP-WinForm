using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"UPDATE Articulos SET
                         Codigo = @codigo,
                         Nombre = @nombre,
                         Descripcion = @descripcion,
                         Precio = @precio,
                         IdMarca = @idMarca,
                         IdCategoria = @idCategoria
                         WHERE Id = @id";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@codigo", art.Codigo);
                cmd.Parameters.AddWithValue("@nombre", art.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", art.Descripcion);
                cmd.Parameters.AddWithValue("@precio", art.Precio);
                cmd.Parameters.AddWithValue("@idMarca", art.IdMarca);
                cmd.Parameters.AddWithValue("@idCategoria", art.IdCategoria);
                cmd.Parameters.AddWithValue("@id", art.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el artículo.", ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }
        }

        public List<Articulo> GetAll()
        {
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"
                        SELECT 
                            A.Id,
                            A.Codigo,
                            A.Nombre,
                            A.Descripcion,
                            M.Id AS IdMarca,
                            M.Descripcion AS MarcaDes,
                            C.Id AS IdCategoria,
                            C.Descripcion AS CategoriaDes,
                            A.Precio
                        FROM ARTICULOS A
                        LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                        LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id";

                cmd = new SqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)reader["Id"];
                    art.Codigo = reader["Codigo"].ToString();
                    art.Nombre = reader["Nombre"].ToString();
                    art.Descripcion = reader["Descripcion"].ToString();
                    art.Precio = (decimal)reader["Precio"];

                    art.Marca = reader["MarcaDes"] != DBNull.Value
                        ? reader["MarcaDes"].ToString()
                        : "Sin marca";

                    art.Categoria = reader["CategoriaDes"] != DBNull.Value
                        ? reader["CategoriaDes"].ToString()
                        : "Sin categoria";

                    art.IdMarca = reader["IdMarca"] != DBNull.Value
                        ? (int?)reader["IdMarca"]
                        : null;

                    art.IdCategoria = reader["IdCategoria"] != DBNull.Value
                        ? (int?)reader["IdCategoria"]
                        : null;
                    
                    art.Imagenes = new ImagenRepository(new ConexionDb()).GetByArticuloId(art.Id);

                    lista.Add(art);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener artículos.", ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }
        }

        public List<Articulo> Filtrar(int idMarca, int idCategoria)
        {
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"
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

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@IdMarca", idMarca);
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)reader["Id"];
                    art.Codigo = reader["Codigo"].ToString();
                    art.Nombre = reader["Nombre"].ToString();
                    art.Descripcion = reader["Descripcion"].ToString();
                    art.Precio = (decimal)reader["Precio"];

                    art.Marca = reader["Marca"] != DBNull.Value
                        ? reader["Marca"].ToString()
                        : "Sin marca";

                    art.Categoria = reader["Categoria"] != DBNull.Value
                        ? reader["Categoria"].ToString()
                        : "Sin categoría";

                    art.IdMarca = reader["IdMarca"] != DBNull.Value
                        ? (int?)reader["IdMarca"]
                        : null;

                    art.IdCategoria = reader["IdCategoria"] != DBNull.Value
                        ? (int?)reader["IdCategoria"]
                        : null;

                    lista.Add(art);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar artículos.", ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }
        }

        public int Add(Articulo art)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"
                    INSERT INTO Articulos
                    (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria)
                    VALUES
                    (@codigo, @nombre, @descripcion, @precio, @idMarca, @idCategoria);

                    SELECT SCOPE_IDENTITY();";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@codigo", art.Codigo);
                cmd.Parameters.AddWithValue("@nombre", art.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", art.Descripcion);
                cmd.Parameters.AddWithValue("@precio", art.Precio);
                cmd.Parameters.AddWithValue("@idMarca", art.IdMarca);
                cmd.Parameters.AddWithValue("@idCategoria", art.IdCategoria);

                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar artículo.", ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }
        }

        public void Delete(int id)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = "DELETE FROM ARTICULOS WHERE Id = @id";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar artículo.", ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }
        }
    }
}
