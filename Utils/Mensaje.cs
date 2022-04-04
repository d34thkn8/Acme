using System;

namespace ACME.ENCUESTAS.API.Utils
{
    internal class Mensaje
    {
        //Nombre de la app
        internal const string ApiNombre = "Surveys";
        // Códigos de error
        internal const string CODE_ERROR_VAL_00 = "VAL-00";
        internal const string CODE_ERROR_VAL_01 = "VAL-01";
        internal const string CODE_ERROR_VAL_02 = "VAL-02";
        internal const string CODE_ERROR_VAL_03 = "VAL-03";

        internal const string CODE_ERROR_API_01 = "API-01";

        // Mensajes de error
        internal const string ERROR_VAL_00 = "Se ejecutó exitosamente";
        internal const string ERROR_VAL_01 = "El campo {0} no puede ser Nullo o Vacio";
        internal const string ERROR_VAL_02 = "El campo {0} debe ser mayor a 0";
        internal const string ERROR_VAL_03 = "";

        internal const string ERROR_API_01 = "Error interno, Excepción no controlada";
    }
}
