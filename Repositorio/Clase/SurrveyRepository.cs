using ACME.ENCUESTAS.API.Datos.Interfaz;
using ACME.ENCUESTAS.API.Entidades.Operacion;
using ACME.ENCUESTAS.API.Entidades.Response;
using ACME.ENCUESTAS.API.Models;
using ACME.ENCUESTAS.API.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.ENCUESTAS.API.Datos.Clase
{
    public class SurrveyRepository : ISurveyRepository
    {
        private readonly IConfiguration configuration;
        private readonly DataBase db;

        public SurrveyRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            db = new DataBase();
        }

        
        public async Task<GenericResponse<CreateSurveyResponse>> Ingresar(CreateSurveyRequest request)
        {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "I";
            string id=Guid.NewGuid().ToString();
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_name", request.Name));
            lstParametro.Add(new SqlParameter("@i_description", request.Description));
            lstParametro.Add(new SqlParameter("@i_surveyId", id));

            #endregion parameteros

            spName = ConstanteString.spSurveyHeader;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                var dato = DataBase.ConvertDataTable<SurveyModel>(dt).FirstOrDefault();
                var listaItems=new List<InformationItem>();
                foreach (var item in request.Information)
                {
                    listaItems.Add(new InformationItem
                    {
                        FieldName = item.FieldName,
                        FieldType = item.FieldType,
                        FieldTitle = item.FieldTitle,
                        Required = item.Required,
                        ItemId=Guid.NewGuid().ToString(),
                        Status = true,
                        SurveyId = id
                    });
                }
                var entrada=XmlHelper.ConvertListObjectToXmlDetail(listaItems);
                var resp=await IngresarItems(entrada, "I");
                return new GenericResponse<CreateSurveyResponse>
                {
                    ProcesoExitoso = true,
                    Mensaje = dato != null ? Mensaje.ERROR_VAL_00 : Mensaje.ERROR_API_01,
                    EntidadResultado = new CreateSurveyResponse { SurveyLink="http://localhost/"+id
                    }
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<CreateSurveyResponse>
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }

        }

        public async Task<GenericResponse> Modificar(SurveyModel request)
        {

            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "M";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_name", request.Name));
            lstParametro.Add(new SqlParameter("@i_description", request.Description));
            lstParametro.Add(new SqlParameter("@i_surveyId", request.SurveyId));

            #endregion parameteros

            spName = ConstanteString.spSurveyHeader;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                var dato = DataBase.ConvertDataTable<CreateSurveyResponse>(dt).FirstOrDefault();
                var listaItems = new List<InformationItem>();
                foreach (var item in request.Information)
                {
                    if (item.ItemId == null)
                    {
                        item.ItemId = Guid.NewGuid().ToString();
                    }
                    if (item.SurveyId == null || !item.SurveyId.Equals(request.SurveyId))
                    {
                        item.SurveyId = request.SurveyId;
                    }
                }
                var eliminacion = await EliminarItems(request.SurveyId);
                var entrada = XmlHelper.ConvertListObjectToXmlDetail(listaItems);
                var resp = await IngresarItems(entrada, "I");
                return new GenericResponse
                {
                    ProcesoExitoso = dato != null,
                    Mensaje = dato != null ? Mensaje.ERROR_VAL_00 : Mensaje.ERROR_API_01,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
        public async Task<GenericResponse> Eliminar(string surveyId)
        {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "D";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_surveyId", surveyId));

            #endregion parameteros

            spName = ConstanteString.spSurveyHeader;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                return new GenericResponse
                {
                    ProcesoExitoso = true,
                    Mensaje = Mensaje.ERROR_VAL_00
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
        public async Task<GenericResponse<SurveyModel>> Consultar(string query)
        {
            List<SqlParameter> lstParametro = new List<SqlParameter>();
            string sql, connectionString;
            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "C";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_surveyId", query));

            #endregion parameteros

            sql = ConstanteString.spSurveyHeader;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, sql, lstParametro);
                var dato = DataBase.ConvertDataTable<SurveyModel>(dt).FirstOrDefault();
                if (dato != null)
                {
                    var lista = await ListarItems(dato.SurveyId);
                    if (lista.ProcesoExitoso)
                        dato.Information = lista.EntidadResultado;
                }
                return new GenericResponse<SurveyModel>
                {
                    ProcesoExitoso = dato != null,
                    Mensaje = dato != null ? Mensaje.ERROR_VAL_00 : Mensaje.ERROR_API_01,
                    EntidadResultado = dato
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<SurveyModel>
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }

        public async Task<GenericResponse<List<SurveyModel>>> Listar()
        {
            List<SqlParameter> lstParametro = new List<SqlParameter>();
            string sql, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "L";
            lstParametro.Add(accion);

            #endregion parameteros

            sql = ConstanteString.spSurveyHeader;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, sql, lstParametro);
                var dato = DataBase.ConvertDataTable<SurveyModel>(dt);
                return new GenericResponse<List<SurveyModel>>
                {
                    ProcesoExitoso = dato != null && dato.Count > 0,
                    Mensaje = dato != null && dato.Count > 0 ? Mensaje.ERROR_VAL_00 : Mensaje.ERROR_API_01,
                    EntidadResultado = dato
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<SurveyModel>>
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }

        }
        public async Task<GenericResponse>IngresarItems(string items, string action)
        {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = action;
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@xml_detalle", items));

            #endregion parameteros

            spName = ConstanteString.spSurveyDetail;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                return new GenericResponse
                {
                    ProcesoExitoso = true,
                    Mensaje = Mensaje.ERROR_VAL_00
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
        public async Task<GenericResponse> EliminarItems(string surveyId)
        {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "D";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_surveyId", surveyId));

            #endregion parameteros

            spName = ConstanteString.spSurveyDetail;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                return new GenericResponse
                {
                    ProcesoExitoso = true,
                    Mensaje = Mensaje.ERROR_VAL_00
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
        public async Task<GenericResponse<List<InformationItem>>> ListarItems(string surveyId)
        {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros

            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "L";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@i_surveyId", surveyId));

            #endregion parameteros

            spName = ConstanteString.spSurveyDetail;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                var data=DataBase.ConvertDataTable<InformationItem>(dt);
                return new GenericResponse<List<InformationItem>>
                {
                    ProcesoExitoso = data!=null && data.Count>0,
                    Mensaje = Mensaje.ERROR_VAL_00,
                    EntidadResultado=data
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<InformationItem>>
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }


        public async Task<GenericResponse> IngresarInformacionDeEncuesta(List<SurveyDataBaseModel> datos) {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros
            var dataId=Guid.NewGuid().ToString();
            List <SurveyDataModel> info=new List<SurveyDataModel>();
            foreach (var item in datos)
            {
                info.Add(new SurveyDataModel
                {
                    Information = item.Information,
                    FieldName = item.FieldName,
                    ItemId = item.ItemId,
                    SurveyId = item.SurveyId,
                    DataId = dataId
                });
            }
            var items=XmlHelper.ConvertListObjectToXmlDetail(info);
            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "I";
            lstParametro.Add(accion);
            lstParametro.Add(new SqlParameter("@xml_detalle", items));
            

            #endregion parameteros

            spName = ConstanteString.spSurveyData;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                return new GenericResponse
                {
                    ProcesoExitoso = true,
                    Mensaje = Mensaje.ERROR_VAL_00
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
        public async Task<GenericResponse<List<SurveysDataResponse>>> ConsultarInformacionDeEncuestas() {
            var lstParametro = new List<SqlParameter>();
            string spName, connectionString;

            connectionString = configuration.GetConnectionString("Conexion");

            #region parameteros
            SqlParameter accion = new SqlParameter();
            accion.ParameterName = "@accion";
            accion.SqlDbType = SqlDbType.Char;
            accion.Value = "L";
            lstParametro.Add(accion);
            
            #endregion parameteros

            spName = ConstanteString.spSurveyData;
            try
            {
                DataTable dt = await db.EjecutarSP(connectionString, spName, lstParametro);
                var data = DataBase.ConvertDataTable<SurveyDataModel>(dt);
                var lista=new List<SurveysDataResponse>();
                if (data!=null && data.Count > 0)
                {
                    var indices=data.Select(o => o.DataId).Distinct();
                    foreach (var item in indices)
                    {
                        lista.Add(new SurveysDataResponse
                        {
                            SurveyId = data.Where(reg => reg.DataId.Equals(item)).FirstOrDefault().SurveyId,
                            SurveyDataId = item,
                            Information = data.Where(reg => reg.DataId.Equals(item)).ToList()
                        });
                    }
                }
                return new GenericResponse<List<SurveysDataResponse>>
                {
                    ProcesoExitoso = true,
                    Mensaje = Mensaje.ERROR_VAL_00,
                    EntidadResultado=lista
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<SurveysDataResponse>>
                {
                    Mensaje = ex.Message,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };
            }
        }
    }
}
