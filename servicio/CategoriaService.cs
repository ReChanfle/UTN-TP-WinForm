using dominio;
using infraestructura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servicio
{
    public class CategoriaService
    {

        private readonly CategoriaRepository _repo;

        public CategoriaService(CategoriaRepository repo)
        {
            _repo = repo;
        }

        public List<Categoria> Listar()
        {
            return _repo.GetAll();
        }

        public void Add(Categoria cat)
        {
            _repo.Update(cat);
        }

        public void Update(Categoria cat)
        {
            _repo.Update(cat);
        }


    }
}
