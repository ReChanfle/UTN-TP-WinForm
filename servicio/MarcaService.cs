using dominio;
using infraestructura;
using System.Collections.Generic;


namespace servicio
{
    public class MarcaService
    {

        private readonly MarcaRepository _repo;

        public MarcaService(MarcaRepository repo)
        {
            _repo = repo;
        }

        public List<Marca> Listar()
        {
            return _repo.GetAll();
        }

        public void Add(Marca marca)
        {
            _repo.Update(marca);
        }

        public void Update(Marca marca)
        {
            _repo.Update(marca);
        }
    }

    
    }
