using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            List<Marca> lista = new List<Marca>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                cmd = new SqlCommand(
                    "SELECT Id, Descripcion FROM MARCAS",
                    conn);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Marca marca = new Marca();

                    marca.Id = (int)reader["Id"];
                    marca.Descripcion = reader["Descripcion"].ToString();

                    lista.Add(marca);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener marcas.", ex);
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

        public void Update(Marca marca)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = _factory.CreateConnection();
                conn.Open();

                string query = @"UPDATE MARCAS SET
                         Descripcion = @desc
                         WHERE Id = @id";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@desc", marca.Descripcion);
                cmd.Parameters.AddWithValue("@id", marca.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar marca.", ex);
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
