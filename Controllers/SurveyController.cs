using ACME.ENCUESTAS.API.Datos.Interfaz;
using ACME.ENCUESTAS.API.Entidades.Operacion;
using ACME.ENCUESTAS.API.Entidades.Response;
using ACME.ENCUESTAS.API.Models;
using ACME.ENCUESTAS.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ACME.ENCUESTAS.API.Controllers
{
    [ApiController]
    [Route("/ACME/Api/Surveys/controller/v1/")]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyRepository repositorio;
        private readonly IConfiguration configuration;

        public SurveyController(ISurveyRepository repo, IConfiguration configuration)
        {
            this.repositorio = repo;
            this.configuration = configuration;
        }
        [Authorize]
        [HttpPost("grabar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> Grabar([FromBody] CreateSurveyRequest request)
        {
            #region Implementacion Metodo Grabar

            try
            {
                
                var validacion = request.IsValid();

                if (validacion.ProcesoExitoso)
                {
                    var respuesta = await repositorio.Ingresar(request);

                    return Ok(respuesta);
                }
                else
                {
                    // no cumplio la validacion
                    return BadRequest(validacion);
                }
            }
            catch (Exception e)
            {
                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Grabar
        }

        [Authorize]
        [HttpPut("modificar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> Modificar([FromBody] SurveyModel request)
        {
            #region Implementacion Metodo Modificar

            try
            {

                var validacion = request.IsValid();

                if (validacion.ProcesoExitoso)
                {
                    return Ok(await repositorio.Modificar(request));
                }
                else
                {
                    // no cumplio la validacion
                    return BadRequest(validacion);
                }
            }
            catch (Exception e)
            {

                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Modificar
        }

        [Authorize]
        [HttpPut("eliminar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> Eliminar(string SurveyId)
        {
            #region Implementacion Metodo Modificar

            try
            {

                var validacion = SurveyId!=null;

                if (validacion)
                {
                    return Ok(await repositorio.Eliminar(SurveyId));
                }
                else
                {
                    // no cumplio la validacion
                    return BadRequest("Debe especificar la encuesta a eliminar");
                }
            }
            catch (Exception e)
            {

                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Modificar
        }
        [Authorize]
        [HttpGet("consultar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> Consultar([FromQuery] string surveyId)
        {
            #region Implementacion Metodo Consultar

            try
            {

                var validacion = string.IsNullOrEmpty(surveyId);

                if (!validacion)
                {
                    return Ok(await repositorio.Consultar(surveyId));
                }
                else
                {
                    // no cumplio la validacion
                    return BadRequest("Debe especificar el ID de la encuesta a solicitar");
                }
            }
            catch (Exception e)
            {
                
                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Consultar
        }

        [Authorize]
        [HttpGet("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<List<CreateSurveyResponse>>>> Listar()
        {
            #region Implementacion Metodo Listar

            try
            {
                
                return Ok(await repositorio.Listar());
                
            }
            catch (Exception e)
            {
                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Listar
        }


        [HttpPost("grabarDatosEncuesta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> GrabarDatosEncuesta([FromBody] List<SurveyDataBaseModel> request)
        {
            #region Implementacion Metodo Grabar

            try
            {

                var validacion = request.Count>0;

                if (validacion)
                {
                    var respuesta = await repositorio.IngresarInformacionDeEncuesta(request);

                    return Ok(respuesta);
                }
                else
                {
                    // no cumplio la validacion
                    return BadRequest("No hay datos para ingresar");
                }
            }
            catch (Exception e)
            {
                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Grabar
        }
        [Authorize]
        [HttpGet("consultarDatosEncuestas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenericResponse<InformationItem>>> ConsultarDatosEncuestas()
        {
            #region Implementacion Metodo Consultar

            try
            {

                return Ok(await repositorio.ConsultarInformacionDeEncuestas());
            }
            catch (Exception e)
            {
                
                #region Manejo Excepcion

                string mensaje;

                if (e.Message == null)
                {
                    mensaje = e.InnerException.Message;
                }
                else
                {
                    mensaje = e.Message;
                }

                GenericResponse validacion = new GenericResponse
                {
                    ProcesoExitoso = false,
                    Mensaje = mensaje,
                    CodigoMensaje = Mensaje.ERROR_API_01
                };

                return BadRequest(validacion);

                #endregion Manejo Excepcion

            }

            #endregion Implementacion Metodo Consultar
        }
        
        [HttpGet("CrearToken")]
        public ActionResult CrearToken()
        {
            var claims = new List<Claim>();
            var token = new GenerateToken(configuration).CrearToken(claims);

            return Ok(token);
        }
    }
}
