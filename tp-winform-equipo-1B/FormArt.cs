using controlador;
using dominio;
using infraestructura;
using servicio;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tp_winform_equipo_1B
{
    public partial class FormArt : Form
    {
        private Articulo articulo;
        private int idArticulo;

        public FormArt()
        {
            InitializeComponent();

            articulo = new Articulo();
            articulo.Imagenes = new List<Imagen>();

            this.Load += FrmArticulo_Load;
        }
        public FormArt(Articulo articuloSeleccionado)
        {
            InitializeComponent();

            articulo = articuloSeleccionado;

            if (articulo.Imagenes == null)
                articulo.Imagenes = new List<Imagen>();

            this.Load += FrmArticulo_Load;
        }
        public FormArt(int id)
        {
            InitializeComponent();
            this.Load += FrmArticulo_Load;
            idArticulo = id;
        }

        private async void FrmArticulo_Load(object sender, EventArgs e)
        {
            CargarCombos();

            if (articulo.Id > 0)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtPrecio.Text = articulo.Precio.ToString();

                ImagenRepository imgRepo = new ImagenRepository(
                new ConexionDb()
                );

                articulo.Imagenes = imgRepo.GetByArticuloId(articulo.Id);

                // refrescar listurl
                RefrescarListaImagenes();

                if (articulo.IdMarca.HasValue)
                    cboMarca.SelectedValue = articulo.IdMarca.Value;

                if (articulo.IdCategoria.HasValue)
                    cboCategoria.SelectedValue = articulo.IdCategoria.Value;

                this.Text = "Editar Artículo";
            }
            else
            {
                this.Text = "Nuevo Artículo";
            }
        }
        private void CargarCombos()
        {
            List<Marca> marcas = new MarcaService(
                new MarcaRepository(
                    new ConexionDb()
                )
            ).Listar();

            List<Categoria> categorias = new CategoriaService(
                new CategoriaRepository(
                    new ConexionDb()
                )
            ).Listar();

            cboMarca.DataSource = marcas;
            cboMarca.DisplayMember = "Descripcion";
            cboMarca.ValueMember = "Id";

            cboCategoria.DataSource = categorias;
            cboCategoria.DisplayMember = "Descripcion";
            cboCategoria.ValueMember = "Id";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
              
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;

                decimal precio;
                if (decimal.TryParse(txtPrecio.Text, out precio))
                {
                    articulo.Precio = precio;
                }
                else
                {
                    MessageBox.Show("El precio ingresado no es válido.");
                    return;
                }

                articulo.IdMarca = Convert.ToInt32(cboMarca.SelectedValue);
                articulo.IdCategoria = Convert.ToInt32(cboCategoria.SelectedValue);

                articulo.Marca = cboMarca.Text;
                articulo.Categoria = cboCategoria.Text;

                ArticuloController controller = new ArticuloController();
                var errores = controller.Validate(articulo);

                if (errores.Count > 0)
                {
                    MessageBox.Show(string.Join("\n", errores));
                    return;
                }

                
                ArticuloService artService = new ArticuloService(
                    new ArticuloRepository(
                        new ConexionDb()
                    )
                );

                
                if (articulo.Id > 0)
                    artService.Update(articulo);
                else
                    artService.Add(articulo);

                

                ImagenRepository imgRepo = new ImagenRepository(
                    new ConexionDb()
                );

                if (articulo.Id > 0)
                {
                    imgRepo.DeleteByArticuloId(articulo.Id);
                }

                foreach (Imagen img in articulo.Imagenes)
                {
                    img.IdArticulo = articulo.Id;
                    imgRepo.Add(img);
                }

                if (articulo.Id > 0)
                    MessageBox.Show("El registro se modificó exitosamente");
                else
                    MessageBox.Show("El registro se agregó exitosamente");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImagen.Text))
                return;

            articulo.Imagenes.Add(new Imagen
            {
                ImagenUrl = txtImagen.Text,
                IdArticulo = articulo.Id
            });

            RefrescarListaImagenes();

            txtImagen.Clear();
        }

        private void RefrescarListaImagenes()
        {
            lstImagenes.DataSource = null;
            lstImagenes.DataSource = articulo.Imagenes;
            lstImagenes.DisplayMember = "ImagenUrl";
        }

        private void lstImagenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstImagenes.SelectedItem != null)
            {
                Imagen img = (Imagen)lstImagenes.SelectedItem;
                pictureBox1.LoadAsync(img.ImagenUrl);
            }
        }

        private void btnEliminarImagen_Click(object sender, EventArgs e)
        {
            if (lstImagenes.SelectedItem == null) return;

            articulo.Imagenes.Remove((Imagen)lstImagenes.SelectedItem);

            RefrescarListaImagenes();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            this.Close();

        }
    }
}
