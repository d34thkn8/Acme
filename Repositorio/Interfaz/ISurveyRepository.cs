using ACME.ENCUESTAS.API.Entidades.Operacion;
using ACME.ENCUESTAS.API.Entidades.Response;
using ACME.ENCUESTAS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.ENCUESTAS.API.Datos.Interfaz
{
    public interface ISurveyRepository
    {
        Task<GenericResponse<List<SurveyModel>>> Listar();
        Task<GenericResponse<SurveyModel>> Consultar(string surveyId);
        Task<GenericResponse> Modificar(SurveyModel request);
        Task<GenericResponse<CreateSurveyResponse>> Ingresar(CreateSurveyRequest request);
        Task<GenericResponse> IngresarInformacionDeEncuesta(List<SurveyDataBaseModel> datos);
        Task<GenericResponse<List<SurveysDataResponse>>> ConsultarInformacionDeEncuestas();
        Task<GenericResponse> Eliminar(string surveyId);
    }
}
