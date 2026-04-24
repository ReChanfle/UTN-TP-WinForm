using infraestructura;
using dominio;

namespace servicio
{
    public class ArticuloService
    {
        private readonly ArticuloRepository _repo;

        public ArticuloService(ArticuloRepository repo)
        {
            _repo = repo;
        }

        public List<Articulo> Listar()
        {
            return _repo.GetAll();
        }

        public void Agregar(Articulo art)
        {
            if (string.IsNullOrEmpty(art.Nombre))
                throw new Exception("El nombre es obligatorio");

            if (art.Precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");

            _repo.Add(art);
        }
    }
}
