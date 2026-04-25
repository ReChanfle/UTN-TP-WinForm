using dominio;
using infraestructura;
using servicio;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tp_winform_equipo_1B
{
    public partial class Form1 : Form
    {
        private List<Imagen> listaImagenesActuales = new List<Imagen>();
        private int indiceActual = 0;

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
                var conexion = new ConexionDb("Server=localhost,1433;Database=CATALOGO_P3_DB;User Id=sa;Password=BaseDatos#2;TrustServerCertificate=True;");
                var repo = new ArticuloRepository(conexion);
                var service = new ArticuloService(repo);
                var productos = service.Listar();
                dataGridView2.DataSource = productos;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.Columns["IdMarca"].Visible = false;
                dataGridView2.Columns["IdCategoria"].Visible = false;

            }
            catch (Exception)
            {

                MessageBox.Show($"Error al cargar los articulos");
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
                        new ConexionDb(
                            "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                            )
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
                        new ConexionDb(
                            "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                            )
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

                var conexion = new ConexionDb(
                    "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;");

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
            if (e.RowIndex < 0) return;

            if (dataGridView2.Columns[e.ColumnIndex].Name == "Editar")
            {
                Articulo artSelecccionado =(Articulo)dataGridView2.Rows[e.RowIndex].DataBoundItem;

                FormArt formArt = new FormArt(artSelecccionado);
                formArt.ShowDialog();

                if (formArt.DialogResult == DialogResult.OK)
                {
                    CargarArticulos();
                }

            }

            if (dataGridView2.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                Articulo art = (Articulo)dataGridView2.Rows[e.RowIndex].DataBoundItem;

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
                            new ConexionDb(
                                "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                            )
                        )
                    );
                        artService.Delete(art.Id);
                        MessageBox.Show("Registro eliminado");
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Error al eliminar el registro");
                    }
                    
                }

            }
        }

        private void btnCrearArticulo_Click(object sender, EventArgs e)
        {
            FormArt formArt = new FormArt();
            formArt.ShowDialog();
            if (formArt.DialogResult == DialogResult.OK)
            {
                CargarArticulos();
            }
        }

        private void MostrarImagen()
        {
            
            string imagenPorDefecto = "https://static.vecteezy.com/system/resources/previews/022/059/000/non_2x/no-image-available-icon-vector.jpg";

           
            if (listaImagenesActuales != null && listaImagenesActuales.Count > 0)
            {
                try
                {
                   
                    pbVisor.Load(listaImagenesActuales[indiceActual].ImagenUrl);
                }
                catch (Exception)
                {
                    
                    pbVisor.Load(imagenPorDefecto);
                }
            }
            else
            {
                
                pbVisor.Load(imagenPorDefecto);
            }
        }


        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
           
            if (dataGridView2.CurrentRow != null)
            {
                Articulo artSeleccionado = (Articulo)dataGridView2.CurrentRow.DataBoundItem;

                if (artSeleccionado != null)
                {
                    try
                    {
                       
                        var conexion = new ConexionDb("Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=BaseDatos#2;TrustServerCertificate=True;");
                        var repoImg = new ImagenRepository(conexion);
                        var serviceImg = new ImagenService(repoImg);

                        
                        listaImagenesActuales = serviceImg.ListarPorIdArticulo(artSeleccionado.Id);

              
                        indiceActual = 0;

                        MostrarImagen();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar las imágenes: " + ex.Message);
                    }
                }
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (indiceActual < listaImagenesActuales.Count - 1)
            {
                indiceActual++; 
                MostrarImagen(); 
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (indiceActual > 0)
            {
                indiceActual--; 
                MostrarImagen(); 
            }
        }
    }
}
