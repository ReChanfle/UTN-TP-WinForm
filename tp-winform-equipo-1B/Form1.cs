using dominio;
using infraestructura;
using servicio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tp_winform_equipo_1B
{
    public partial class Form1 : Form
    {
        private List<Imagen> imagenesActuales = new List<Imagen>();
        private int indiceImagenActual = 0;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;

        }

        private void CargarArticulos ()
        {

            try
            {
                var conexion = new ConexionDb();
                var repo = new ArticuloRepository(conexion);
                var service = new ArticuloService(repo);
                var productos = service.Listar();
                dataGridView2.DataSource = productos;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.Columns["IdMarca"].Visible = false;
                dataGridView2.Columns["IdCategoria"].Visible = false;

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error al cargar los articulos: "+ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CargarArticulos();
            AddbuttonsActions();

        }

        private void LoadComboBrands_Click(object sender, EventArgs e)
        {
            try
            {

                List<Marca> marcas = new MarcaService(
                    new MarcaRepository(
                        new ConexionDb()
                        )
                    ).Listar();


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
                List<Categoria> categorias = new CategoriaService(
                    new CategoriaRepository(
                        new ConexionDb()
                        )
                    ).Listar();

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

                var conexion = new ConexionDb();

                var repo = new ArticuloRepository(conexion);

                var lista = repo.Filtrar(idMarca, idCategoria);

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

        private void AddbuttonsActions()
        {
            if (!dataGridView2.Columns.Contains("Editar"))
            {
                DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
                btnEditar.Name = "Editar";
                btnEditar.HeaderText = "Acciones";
                btnEditar.Text = "Editar";
                btnEditar.UseColumnTextForButtonValue = true;

                dataGridView2.Columns.Add(btnEditar);
            }

            if (!dataGridView2.Columns.Contains("Eliminar"))
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "";
                btnEliminar.Text = "Eliminar";
                btnEliminar.UseColumnTextForButtonValue = true;

                dataGridView2.Columns.Add(btnEliminar);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var grid = (DataGridView)sender;

            
            if (!(grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
                return;

            var art = (Articulo)grid.Rows[e.RowIndex].DataBoundItem;


            if (grid.Columns[e.ColumnIndex].Name == "Editar")
            {
                try
                {
                    using (FormArt formArt = new FormArt(art))
                    {
                        if (formArt.ShowDialog() == DialogResult.OK)
                        {
                            CargarArticulos();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "No se pudo abrir la ventana de edición.\n",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }


            else if (grid.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                DialogResult resultado = MessageBox.Show(
                    $"¿Deseás eliminar {art.Nombre}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado == DialogResult.Yes)
                {
                    try
                    {
                       
                        ArticuloService artService = new ArticuloService(
                            new ArticuloRepository(
                                 new ConexionDb()
                            )
                        );

                        artService.Delete(art.Id);
                        

                        MessageBox.Show("Registro eliminado");
                        CargarArticulos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
        }

        private void btnCrearArticulo_Click(object sender, EventArgs e)
        {
            try
            {
                using (FormArt formArt = new FormArt())
                {
                    if (formArt.ShowDialog() == DialogResult.OK)
                    {
                        CargarArticulos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir la ventana de creación.\n",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            MostrarImagenSeleccionada();
        }

        private void MostrarImagenSeleccionada()
        {
            try
            {
                if (dataGridView2.CurrentRow == null)
                    return;

                Articulo seleccionado =
                    (Articulo)dataGridView2.CurrentRow.DataBoundItem;

                if (seleccionado != null)
                {
                    imagenesActuales = seleccionado.Imagenes;
                    indiceImagenActual = 0;

                    MostrarImagenActual();
                }
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }


        private void MostrarImagenActual()
        {
            try
            {
                if (imagenesActuales == null || imagenesActuales.Count == 0)
                {
                    pictureBox1.Image = null;
                    return;
                }

                string url = imagenesActuales[indiceImagenActual].ImagenUrl;

                pictureBox1.Load(url);
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }

        private void btnNextPicture_Click(object sender, EventArgs e)
        {
            if (imagenesActuales == null || imagenesActuales.Count == 0)
                return;

            indiceImagenActual++;

            if (indiceImagenActual >= imagenesActuales.Count)
                indiceImagenActual = 0;

            MostrarImagenActual();
        }

        private void Filtrar()
        {
            try
            {
                string filtro = txtBuscar.Text.ToLower();

                var conexion = new ConexionDb();
                var repo = new ArticuloRepository(conexion);

                var lista = repo.GetAll();

                var filtrados = lista.FindAll(x =>
                    (x.Codigo != null && x.Codigo.ToLower().Contains(filtro)) ||
                    (x.Nombre != null && x.Nombre.ToLower().Contains(filtro)) ||
                    (x.Descripcion != null && x.Descripcion.ToLower().Contains(filtro))
                );

                dataGridView2.DataSource = filtrados;
            }
            catch
            {
                MessageBox.Show("Error al filtrar");
            }
        }

        private void btnLastPicture_Click(object sender, EventArgs e)
        {
            if (imagenesActuales == null || imagenesActuales.Count == 0)
                return;

            indiceImagenActual--;

            if (indiceImagenActual < 0)
                indiceImagenActual = imagenesActuales.Count - 1;

            MostrarImagenActual();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            using (FormGestion f = new FormGestion())
            {
                f.ShowDialog();
            }
        }
    }
}
