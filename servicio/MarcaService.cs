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

        public void Add(Marca mar)
        {
            _repo.Add(mar);
        }

        public void Update(Marca mar)
        {
            _repo.Update(mar);
        }
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
    
}
