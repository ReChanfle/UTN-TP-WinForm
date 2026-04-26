using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace infraestructura
{
    public class CategoriaRepository
    {
        private readonly ConexionDb _factory;

        public CategoriaRepository(ConexionDb factory)
        {
            _factory = factory;
        }

        public List<Categoria> GetAll()
        {
            List<Categoria> lista = new List<Categoria>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                cmd = new SqlCommand(
                    "SELECT Id, Descripcion FROM CATEGORIAS",
                    conn);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Categoria categoria = new Categoria();

                    categoria.Id = (int)reader["Id"];
                    categoria.Descripcion = reader["Descripcion"].ToString();

                    lista.Add(categoria);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener categorías.", ex);
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

        public void Update(Categoria cat)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"UPDATE CATEGORIAS SET
                         Descripcion = @desc
                         WHERE Id = @id";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@desc", cat.Descripcion);
                cmd.Parameters.AddWithValue("@id", cat.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar categoría.", ex);
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
