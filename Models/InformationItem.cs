using ACME.ENCUESTAS.API.Entidades.Response;
using ACME.ENCUESTAS.API.Utils;

namespace ACME.ENCUESTAS.API.Models
{
    public class InformationItem
    {
        public string SurveyId { get; set; }
        public string ItemId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldTitle { get; set; }
        public bool Required { get; set; }
        public bool Status { get; set; }
        public GenericResponse IsValid()
        {
            #region Validar Campos

            if (string.IsNullOrEmpty(FieldName))
            {
                string msgError = Mensaje.ERROR_VAL_01;
                msgError = string.Format(msgError, "FieldName");

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = msgError
                };
            }
            if (string.IsNullOrEmpty(FieldTitle))
            {
                string msgError = Mensaje.ERROR_VAL_01;
                msgError = string.Format(msgError, "FieldTitle");

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = msgError
                };
            }
            if (string.IsNullOrEmpty(FieldType))
            {
                string msgError = Mensaje.ERROR_VAL_01;
                msgError = string.Format(msgError, "FieldType");

                return new GenericResponse
                {
                    CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                    Mensaje = msgError
                };
            }
            else
            {
                if (!FieldType.Equals("Text") && !FieldType.Equals("Number") && !FieldType.Equals("Date"))
                {
                    return new GenericResponse
                    {
                        CodigoMensaje = Mensaje.CODE_ERROR_VAL_01,
                        Mensaje = "El campo FieldType debe ser Text, Number o Date."
                    };
                }
            }

            return new GenericResponse
            {
                ProcesoExitoso = true
            };

            #endregion Validar Campos
        }
    }
}
