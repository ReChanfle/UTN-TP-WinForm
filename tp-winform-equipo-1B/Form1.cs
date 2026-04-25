using dominio;
using infraestructura;
using servicio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tp_winform_equipo_1B
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                var conexion = new ConexionDb(
                    "server=localhost\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true"
                );

                var repo = new ArticuloRepository(conexion);
                var service = new ArticuloService(repo);
                var productos = service.Listar();
                dataGridView2.DataSource = productos;

                dataGridView2.DataSource = service.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
                return;

            Articulo seleccionado = (Articulo)dataGridView2.CurrentRow.DataBoundItem;

            FormArticulo frm = new FormArticulo(seleccionado);
            frm.ShowDialog();

            // refreshhhhh
            btnCargar_Click(sender, e);
        }


        private void LoadComboBrands_Click(object sender, EventArgs e)
        {

            try
            {
                List<Marca> marcas = new List<Marca>();

                var conexionDb = new ConexionDb(
                    "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;");

                using (SqlConnection conn = conexionDb.CreateConnection())
                {
                    conn.Open();

                    string query = "SELECT Id, Descripcion FROM MARCAS";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            marcas.Add(new Marca
                            {
                                Id = (int)reader["Id"],
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }

                toolStripComboBox3.ComboBox.DataSource = marcas;
                toolStripComboBox3.ComboBox.DisplayMember = "Descripcion";
                toolStripComboBox3.ComboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar marcas: " + ex.Message);
            }

        }


        private void LoadComboCategory_Click(object sender, EventArgs e)
        {

            try
            {
                List<Categoria> categorias = new List<Categoria>();

                var conexionDb = new ConexionDb(
                    "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;");

                using (SqlConnection conn = conexionDb.CreateConnection())
                {
                    conn.Open();

                    string query = "SELECT Id, Descripcion FROM CATEGORIAS";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                Id = (int)reader["Id"],
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }

                toolStripComboBox4.ComboBox.DataSource = categorias;
                toolStripComboBox4.ComboBox.DisplayMember = "Descripcion";
                toolStripComboBox4.ComboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorias: " + ex.Message);
            }

        }


        private void Filter_Click(object sender, EventArgs e)
        {
            try
            {
                int idMarca = Convert.ToInt32(toolStripComboBox3.ComboBox.SelectedValue);
                int idCategoria = Convert.ToInt32(toolStripComboBox4.ComboBox.SelectedValue);

                var conexion = new ConexionDb(
                    "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;");

                var repo = new ArticuloRepository(conexion);

                var lista = repo.Filtrar(idMarca, idCategoria);

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
            }
        }

        private void ResetFilter_Click(object sender, EventArgs e)
        {
            try
            {
              
               this.Form1_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al resetear filtros: " + ex.Message);
            }
        }

    }
}
