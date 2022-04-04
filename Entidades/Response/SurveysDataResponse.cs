using ACME.ENCUESTAS.API.Models;
using System.Collections.Generic;

namespace ACME.ENCUESTAS.API.Entidades.Response
{
    public class SurveysDataResponse
    {
        public string SurveyId { get; set; }
        public string SurveyDataId { get; set; }
        public List<SurveyDataModel>Information { get; set; }
    }
}
