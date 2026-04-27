using dominio;
using infraestructura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlador
{
    public class CategoriaController
    {
        private readonly CategoriaRepository _repo;

        public CategoriaController(CategoriaRepository repo)
        {
            _repo = repo;
        }

        public List<string> Validate(Categoria categoria)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                errores.Add("La descripción es obligatoria.");
            else if (categoria.Descripcion.Length > 50)
                errores.Add("La descripción no puede superar 50 caracteres.");

            return errores;
        }

        public List<string> ValidateEdit(Categoria categoria)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                errores.Add("La descripción es obligatoria.");
            else if (categoria.Descripcion.Length > 50)
                errores.Add("La descripción no puede superar 50 caracteres.");

            try
            {
                var existentes = _repo.GetAll();

                bool repetido = existentes.Any(x =>
                    x.Id != categoria.Id &&
                    x.Descripcion.Trim().ToLower() ==
                    categoria.Descripcion.Trim().ToLower());

                if (repetido)
                    errores.Add("Ya existe una categoría con ese nombre.");

            }
            catch (Exception ex)
            {
                throw ex;
             
            }

            return errores;
        }


    }
}
