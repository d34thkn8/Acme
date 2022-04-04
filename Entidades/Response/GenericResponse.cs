using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.ENCUESTAS.API.Entidades.Response
{
    public class GenericResponse<T>
    {
        public T EntidadResultado { get; set; }
        public bool ProcesoExitoso { get; set; }
        public string Mensaje { get; set; }

        public string CodigoMensaje { get; set; }

    }

    public class GenericResponse
    {

        public bool ProcesoExitoso { get; set; }
        public string Mensaje { get; set; }

        public string CodigoMensaje { get; set; }
    }
}