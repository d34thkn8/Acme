using ACME.ENCUESTAS.API.Entidades.Response;
using ACME.ENCUESTAS.API.Utils;
using System.Collections.Generic;

namespace ACME.ENCUESTAS.API.Models
{
    public class SurveyModel
    {
        public string SurveyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool status { get; set; }
        public List<InformationItem> Information { get; set; }


        public GenericResponse IsValid()
        {
            #region Validar Campos

            if (string.IsNullOrEmpty(SurveyId))
            {

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = string.Format(Mensaje.ERROR_VAL_01, "SurveyId")
                };
            }
            if (string.IsNullOrEmpty(Name))
            {
                string msgError = Mensaje.ERROR_VAL_01;
                msgError = string.Format(msgError, "Name");

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = msgError
                };
            }
            if (string.IsNullOrEmpty(Description))
            {
                string msgError = Mensaje.ERROR_VAL_01;
                msgError = string.Format(msgError, "Description");

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = msgError
                };
            }
            if (Information == null || Information.Count == 0)
            {
                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = "Debe agregar los elementos de la encuesta"
                };
            }
            var error = false;
            var errorString = "";
            foreach (var item in Information)
            {
                var itemValido = item.IsValid();
                if (!itemValido.ProcesoExitoso)
                {
                    error = true;
                    errorString = itemValido.Mensaje;
                    break;
                }
            }
            if (error)
            {
                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = "Uno de los campos tiene informacion incompleta: " + errorString
                };
            }
            return new GenericResponse
            {
                ProcesoExitoso = true
            };

            #endregion Validar Campos
        }
    }
}
