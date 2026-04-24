using System;
using infraestructura;
using dominio;

namespace tp_winform_equipo_1B
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            var conexion = new ConexionDb("Server=localhost,1433;Database=CATALOGO_P3_DB;User Id=sa;Password=NuevaPassword123;TrustServerCertificate=True;");
            var repo = new ArticuloRepository(conexion);
            var productos = repo.GetAll();
            dataGridView1.DataSource = productos;
        }
    }
}
