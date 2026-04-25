using dominio;
using System.Collections.Generic;
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
            var lista = new List<Categoria>();

            using (var conn = _factory.CreateConnection())
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT Id, Descripcion FROM CATEGORIAS", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Categoria
                    {
                        Id = (int)reader["Id"],
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }

            return lista;
        }

        public void Update(Categoria cat)
        {
            var conn = _factory.CreateConnection();
            conn.Open();

            var query = @"UPDATE CATEGORIAS SET
                    Descripcion = @desc,
                  WHERE Id = @id";

            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@desc", cat.Descripcion);
            cmd.Parameters.AddWithValue("@id", cat.Id);

            cmd.ExecuteNonQuery();
        }
    }
}
