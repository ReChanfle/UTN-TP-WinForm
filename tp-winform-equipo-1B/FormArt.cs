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

        // ===== NUEVO (ALTA) =====
        public FormArt()
        {
            InitializeComponent();
            this.Load += FrmArticulo_Load;
            articulo = new Articulo();
        }

        // ===== EDITAR =====
        public FormArt(Articulo articuloSeleccionado)
        {
            InitializeComponent();
            this.Load += FrmArticulo_Load;
            articulo = articuloSeleccionado;
        }

        private void FrmArticulo_Load(object sender, EventArgs e)
        {
            CargarCombos();

            // SI ES EDICION
            if (articulo.Id > 0)
            {
                txtCodigo.Text = articulo.Codigo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                txtPrecio.Text = articulo.Precio.ToString();

                cboMarca.SelectedValue = articulo.IdMarca;
                cboCategoria.SelectedValue = articulo.IdCategoria;

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
                    new ConexionDb(
                        "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                    )
                )
            ).Listar();

            List<Categoria> categorias = new CategoriaService(
                new CategoriaRepository(
                    new ConexionDb(
                        "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                    )
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
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                articulo.IdMarca = Convert.ToInt32(cboMarca.SelectedValue);
                articulo.IdCategoria = Convert.ToInt32(cboCategoria.SelectedValue);

                articulo.Marca = cboMarca.Text;
                articulo.Categoria = cboCategoria.Text;

                ArticuloService artService = new ArticuloService(
                    new ArticuloRepository(
                        new ConexionDb(
                            "Server=localhost;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;"
                        )
                    )
                );

                // SI EXISTE => UPDATE
                if (articulo.Id > 0)
                {
                    artService.Update(articulo);
                    MessageBox.Show("El registro se modificó exitosamente");
                }
                else
                {
                    artService.Add(articulo);
                    MessageBox.Show("El registro se agregó exitosamente");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
    }
}
