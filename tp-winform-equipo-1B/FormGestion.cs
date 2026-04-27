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
        public FormGestion()
        {
            InitializeComponent();
        }

        private List<Marca> listaMarcas;
        private List<Categoria> listaCategorias;

        private void FormGestion_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            var conexion = new ConexionDb();

            listaMarcas = new MarcaService(new MarcaRepository(conexion)).Listar();
            listaCategorias = new CategoriaService(new CategoriaRepository(conexion)).Listar();

            dgvMarcas.DataSource = listaMarcas;
            dgvCategorias.DataSource = listaCategorias;
        }

        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox("Nueva Marca:");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var service = new MarcaService(
                    new MarcaRepository(
                        new ConexionDb()
                    )
                );

                service.Add(new Marca { Descripcion = nombre });
                CargarDatos();
            }
        }

        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow == null) return;

            Marca marca = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

            var confirm = MessageBox.Show("¿Eliminar marca?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                var service = new MarcaService(
                    new MarcaRepository(
                        new ConexionDb()
                    )
                );

                service.Delete(marca.Id);
                CargarDatos();
            }
        }
    }
}
