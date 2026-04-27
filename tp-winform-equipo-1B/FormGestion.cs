using dominio;
using infraestructura;
using servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tp_winform_equipo_1B
{
    public partial class FormGestion : Form
    {
        private List<Marca> listaMarcas;
        private List<Categoria> listaCategorias;
        public FormGestion()
        {
            InitializeComponent();
            this.Load += FormGestion_Load;
        }

        private void FormGestion_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                var conexion = new ConexionDb();

                listaMarcas = new MarcaService(new MarcaRepository(conexion)).Listar();
                listaCategorias = new CategoriaService(new CategoriaRepository(conexion)).Listar();

                // marcas
                dgvMarcas.DataSource = null;
                dgvMarcas.DataSource = listaMarcas;

                //categorias
                dgvCategorias.DataSource = null;
                dgvCategorias.DataSource = listaCategorias;

                //ocultar ID
                if (dgvMarcas.Columns["Id"] != null)
                    dgvMarcas.Columns["Id"].Visible = false;

                if (dgvCategorias.Columns["Id"] != null)
                    dgvCategorias.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        public static string Prompt(string texto)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = texto
            };

            TextBox txt = new TextBox() { Left = 20, Top = 20, Width = 240 };
            Button btnOk = new Button() { Text = "OK", Left = 180, Width = 80, Top = 50 };

            btnOk.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(txt);
            prompt.Controls.Add(btnOk);

            prompt.ShowDialog();

            return txt.Text;
        }
        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            string descripcion = Prompt("Nueva marca:");

            if (!string.IsNullOrWhiteSpace(descripcion))
            {
                var service = new MarcaService(
                    new MarcaRepository(
                        new ConexionDb()
                    )
                );

                service.Add(new Marca { Descripcion = descripcion });
                CargarDatos();
            }
        }

        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow == null)
                return;

            Marca marca = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

            var confirm = MessageBox.Show("¿Eliminar marca?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                var service = new MarcaService(
                    new MarcaRepository(new ConexionDb())
                );

                service.Delete(marca.Id);
                CargarDatos();
            }
        }

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            string descripcion = Prompt("Nueva categoría:");

            if (!string.IsNullOrWhiteSpace(descripcion))
            {
                var service = new CategoriaService(
                    new CategoriaRepository(
                        new ConexionDb()
                    )
                );

                service.Add(new Categoria { Descripcion = descripcion });
                CargarDatos();
            }
        }

        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow == null)
                return;

            Categoria categoria = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;

            var confirm = MessageBox.Show("¿Eliminar categoría?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                var service = new CategoriaService(
                    new CategoriaRepository(new ConexionDb())
                );

                service.Delete(categoria.Id);
                CargarDatos();
            }
        }
    }
}
