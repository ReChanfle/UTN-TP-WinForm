using dominio;
using infraestructura;
using System;
using System.Collections.Generic;
using System.Linq;

namespace controlador
{
    public class MarcaController
    {
        private readonly MarcaRepository _repo;

        public MarcaController(MarcaRepository repo)
        {
            _repo = repo;
        }

        public List<string> Validate(Marca marca)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(marca.Descripcion))
                errores.Add("La descripción es obligatoria.");
            else if (marca.Descripcion.Length > 50)
                errores.Add("La descripción no puede superar 50 caracteres.");

            return errores;
        }

        public List<string> ValidateEdit(Marca marca)
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(marca.Descripcion))
                errores.Add("La descripción es obligatoria.");
            else if (marca.Descripcion.Length > 50)
                errores.Add("La descripción no puede superar 50 caracteres.");

            try
            {
                var existentes = _repo.GetAll();

                bool repetido = existentes.Any(x =>
                    x.Id != marca.Id &&
                    x.Descripcion.Trim().ToLower() ==
                    marca.Descripcion.Trim().ToLower());

                if (repetido)
                    errores.Add("Ya existe una marca con ese nombre.");
            }
            catch (Exception)
            {
                throw;
            }

            return errores;
        }
    }
}
