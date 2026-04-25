using System;
using System.Collections.Generic;
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

        public void Add(Articulo art)
        {
            _repo.Add(art);
        }

        public void Update(Articulo art)
        {
            _repo.Update(art);
        }

        public void Delete (int id)
        {
            _repo.Delete(id);
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
