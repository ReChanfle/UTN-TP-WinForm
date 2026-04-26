using dominio;
using infraestructura;
using System;
using System.Collections.Generic;

namespace servicio
{
    public class ImagenService
    {
        private ImagenRepository _repository;

   
        public ImagenService(ImagenRepository repository)
        {
            _repository = repository;
        }

        
        public void Add(Imagen img)
        {
            try
            {
                _repository.Add(img);
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

        
        public List<Imagen> ListarPorIdArticulo(int idArticulo)
        {
            try
            {
                //return _repository.ListarPorIdArticulo(idArticulo);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}