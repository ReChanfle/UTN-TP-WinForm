using controlador;
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
using System.Text.RegularExpressions;
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

        
                dgvMarcas.DataSource = null;
                dgvMarcas.DataSource = listaMarcas;

               
                dgvCategorias.DataSource = null;
                dgvCategorias.DataSource = listaCategorias;

               
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

        public static string Prompt(string texto, string valorInicial = "")
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = texto,
                StartPosition = FormStartPosition.CenterScreen
            };

            TextBox txt = new TextBox()
            {
                Left = 20,
                Top = 20,
                Width = 240,
                Text = valorInicial
            };

            txt.SelectAll();
            txt.Focus();

            Button btnOk = new Button()
            {
                Text = "OK",
                Left = 180,
                Width = 80,
                Top = 50,
                DialogResult = DialogResult.OK
            };

            prompt.Controls.Add(txt);
            prompt.Controls.Add(btnOk);

            prompt.AcceptButton = btnOk;

            return prompt.ShowDialog() == DialogResult.OK
                ? txt.Text
                : "";
        }
        private void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = Prompt("Nueva marca:");

                var conexion = new ConexionDb();
                var repo = new MarcaRepository(conexion);
                var service = new MarcaService(repo);
                var controller = new MarcaController(repo);

                Marca marca = new Marca();
                marca.Descripcion = descripcion;

                List<string> errores = controller.Validate(marca);

                if (errores.Count > 0)
                {
                    MessageBox.Show(
                        string.Join(Environment.NewLine, errores),
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                service.Add(marca);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al agregar marca: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMarcas.CurrentRow == null)
                    return;

                Marca marca = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

                DialogResult confirm = MessageBox.Show(
                    "¿Eliminar marca?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar marca: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                string descripcion = Prompt("Nueva categoría:");

                var conexion = new ConexionDb();
                var repo = new CategoriaRepository(conexion);
                var service = new CategoriaService(repo);
                var controller = new CategoriaController(repo);

                Categoria categoria = new Categoria();
                categoria.Descripcion = descripcion;

                List<string> errores = controller.Validate(categoria);

                if (errores.Count > 0)
                {
                    MessageBox.Show(
                        string.Join(Environment.NewLine, errores),
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                service.Add(categoria);

                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al agregar categoría: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCategorias.CurrentRow == null)
                    return;

                Categoria categoria =
                    (Categoria)dgvCategorias.CurrentRow.DataBoundItem;

                DialogResult confirm = MessageBox.Show(
                    "¿Eliminar categoría?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    var service = new CategoriaService(
                        new CategoriaRepository(
                            new ConexionDb()
                        )
                    );

                    service.Delete(categoria.Id);

                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar categoría: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEditarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMarcas.CurrentRow == null)
                    return;

                Marca marca = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

                string nuevaDescripcion =
                Prompt("Editar categoría:", marca.Descripcion);

                Marca temp = new Marca();
                temp.Id = marca.Id;
                temp.Descripcion = nuevaDescripcion;

                var conexion = new ConexionDb();
                var repo = new MarcaRepository(conexion);
                var service = new MarcaService(repo);
                var controller = new MarcaController(repo);

                List<string> errores = controller.ValidateEdit(temp);

                if (errores.Count > 0)
                {
                    MessageBox.Show(
                   string.Join(Environment.NewLine, errores),
                   "Validación",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                    return;
                }

                temp.Descripcion = nuevaDescripcion;
                service.Update(temp);
                CargarDatos();


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al editar marca: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEditarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMarcas.CurrentRow == null)
                    return;

                Categoria categoria = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;

                string nuevaDescripcion =
                Prompt("Editar categoría:", categoria.Descripcion);

                Categoria temp = new Categoria();
                temp.Id = categoria.Id;
                temp.Descripcion = nuevaDescripcion;

                var conexion = new ConexionDb();
                var repo = new CategoriaRepository(conexion);
                var service = new CategoriaService(repo);
                var controller = new CategoriaController(repo);

                List<string> errores = controller.ValidateEdit(temp);

                if (errores.Count > 0)
                {
                    MessageBox.Show(
                    string.Join(Environment.NewLine, errores),
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }

                temp.Descripcion = nuevaDescripcion;
                service.Update(temp);
                CargarDatos();


            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al editar categoría: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
