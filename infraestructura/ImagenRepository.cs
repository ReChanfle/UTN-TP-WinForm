using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace infraestructura
{
    public class ImagenRepository
    {
        private readonly ConexionDb _factory;

        public ImagenRepository(ConexionDb factory)
        {
            _factory = factory;
        }

        public void Add(Imagen img)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"INSERT INTO IMAGENES
                             (IdArticulo, ImagenUrl)
                             VALUES (@idArticulo, @url)";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@idArticulo", img.IdArticulo);
                cmd.Parameters.AddWithValue("@url", img.ImagenUrl);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar imagen.", ex);
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

        public void DeleteByArticuloId(int idArticulo)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = "DELETE FROM IMAGENES WHERE IdArticulo = @id";

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idArticulo);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar imágenes del artículo.", ex);
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

        public List<Imagen> GetByArticuloId(int idArticulo)
        {
            List<Imagen> lista = new List<Imagen>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"SELECT Id, IdArticulo, ImagenUrl
                             FROM IMAGENES
                             WHERE IdArticulo = @id";

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idArticulo);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Imagen img = new Imagen();

                    img.Id = (int)reader["Id"];
                    img.IdArticulo = (int)reader["IdArticulo"];
                    img.ImagenUrl = reader["ImagenUrl"].ToString();

                    lista.Add(img);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener imágenes del artículo.", ex);
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
    }
}
