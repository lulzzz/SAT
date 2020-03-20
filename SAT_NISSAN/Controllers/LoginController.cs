using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using SAT_CL.Seguridad;
using SAT_NISSAN.Models;
using TSDK.Base;

namespace SAT_NISSAN.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/suat")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }
        
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //Declaramos Objeto Resultado
            RetornoOperacion Resultado = new RetornoOperacion();

            //Instanciamos Usuario de acuerdo al e-mail proporcionado
            using (Usuario objUsuario = new SAT_CL.Seguridad.Usuario(login.UserName))
            {
                //Autenticando Usuario
                Resultado = objUsuario.AutenticaUsuario(login.Password);
                
                //Si la Autenticación es correcta
                if (Resultado.OperacionExitosa)
                {
                    DataTable UsuarioCompanias = SAT_CL.Seguridad.UsuarioCompania.ObtieneCompaniasUsuario(objUsuario.id_usuario);
                    if (UsuarioCompanias.Rows.Count == 1)
                    {
                        DataRow DR = UsuarioCompanias.Rows[0];
                        string token = TokenGenerator.GenerateTokenJwt(objUsuario.id_usuario + "|" + Convert.ToString(DR["IdCompaniaEmisorReceptor"]));
                        
                        AuthResponse authresponse = new AuthResponse
                        {
                            success = true,
                            data = new DataLogin { Token = token },
                            errors = new Errors { code = 0, source = new Source { pointer = "" }, title = "", detail = "Sin errores" }
                        };
                        //string json = JsonConvert.SerializeObject(authresponse);
                        return Ok(authresponse);
                    }
                    else
                    {
                        FailAuthResponse failauthresponse = new FailAuthResponse
                        {
                            isSucceded = false,
                            message = "El usuario tiene asignadas más de una compañía."
                        };
                        //string json = JsonConvert.SerializeObject(failauthresponse);
                        return Json(failauthresponse);
                    }
                }
                else
                {
                    FailAuthResponse failauthresponse = new FailAuthResponse
                    {
                        isSucceded = false,
                        message =  "No se encontraron datos para este cliente."
                    };
                    //string json = JsonConvert.SerializeObject(failauthresponse);
                    return Json(failauthresponse);
                }
            }
        }

        [HttpPost]
        [Route("Error")]
        public IHttpActionResult ObtieneError(int NumError)
        {
            //string json;
            //De acuerdo al tipo de error, devolvemos el JSON correspondiente
            switch (NumError)
            {
                case 100:
                    var e100 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Continue",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e100);
                    return Json(e100);
                case 101:
                    var e101 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Switching Protocols",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e101);
                    return Json(e101);
                case 102:
                    var e102 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Processing",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e102);
                    return Json(e102);
                case 103:
                    var e103 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Early Hints",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e103);
                    return Json(e103);
                case int n when (n >= 104 && n <= 199):
                    var e104_e199 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e104_e199);
                    return Json(e104_e199);
                case 200:
                    var e200 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "OK",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e200);
                    return Json(e200);
                case 201:
                    var e201 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Created",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e201);
                    return Json(e201);
                case 202:
                    var e202 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Accepted",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e202);
                    return Json(e202);
                case 203:
                    var e203 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Non-Authoritative Information",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e203);
                    return Json(e203);
                case 204:
                    var e204 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "No Content",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e204);
                    return Json(e204);
                case 205:
                    var e205 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Reset Content",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e205);
                    return Json(e205);
                case 206:
                    var e206 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Partial Content",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e206);
                    return Json(e206);
                case 207:
                    var e207 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Multi-Status",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e207);
                    return Json(e207);
                case 208:
                    var e208 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Already Reported",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e208);
                    return Json(e208);
                case int n when (n >= 209 && n <= 225):
                    var e209_e225 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e209_e225);
                    return Json(e209_e225);
                case 226:
                    var e226 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "IM Used",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e226);
                    return Json(e226);
                case int n when (n >= 227 && n <= 299):
                    var e227_e299 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e227_e299);
                    return Json(e227_e299);
                case 300:
                    var e300 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Multiple Choices",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e300);
                    return Json(e300);
                case 301:
                    var e301 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Moved Permanently",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e301);
                    return Json(e301);
                case 302:
                    var e302 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Found",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e302);
                    return Json(e302);
                case 303:
                    var e303 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "See Other",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e303);
                    return Json(e303);
                case 304:
                    var e304 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Not Modified",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e304);
                    return Json(e304);
                case 305:
                    var e305 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Use Proxy",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e305);
                    return Json(e305);
                case 306:
                    var e306 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "(Unused)",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e306);
                    return Json(e306);
                case 307:
                    var e307 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Temporary Redirect",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e307);
                    return Json(e307);
                case 308:
                    var e308 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Permanent Redirect",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e308);
                    return Json(e308);
                case int n when (n >= 309 && n <= 399):
                    var e309_e399 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e309_e399);
                    return Json(e309_e399);
                case 400:
                    var e400 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Bad Request",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e400);
                    return Json(e400);
                case 401:
                    var e401 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unauthorized",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e401);
                    return Json(e401);
                case 402:
                    var e402 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Payment Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e402);
                    return Json(e402);
                case 403:
                    var e403 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Forbidden",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e403);
                    return Json(e403);
                case 404:
                    var e404 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Not Found",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e404);
                    return Json(e404);
                case 405:
                    var e405 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Method Not Allowed",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e405);
                    return Json(e405);
                case 406:
                    var e406 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Not Acceptable",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e406);
                    return Json(e406);
                case 407:
                    var e407 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Proxy Authentication Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e407);
                    return Json(e407);
                case 408:
                    var e408 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Request Timeout",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e408);
                    return Json(e408);
                case 409:
                    var e409 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Conflict",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e409);
                    return Json(e409);
                case 410:
                    var e410 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Gone",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e410);
                    return Json(e410);
                case 411:
                    var e411 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Length Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e411);
                    return Json(e411);
                case 412:
                    var e412 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Precondition Failed",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e412);
                    return Json(e412);
                case 413:
                    var e413 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Payload Too Large",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e413);
                    return Json(e413);
                case 414:
                    var e414 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "URI Too Long",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e414);
                    return Json(e414);
                case 415:
                    var e415 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unsupported Media Type",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e415);
                    return Json(e415);
                case 416:
                    var e416 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Range Not Satisfiable",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e416);
                    return Json(e416);
                case 417:
                    var e417 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Expectation Failed",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e417);
                    return Json(e417);
                case int n when (n >= 418 && n <= 420):
                    var e418_e420 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e418_e420);
                    return Json(e418_e420);
                case 421:
                    var e421 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Misdirected Request",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e421);
                    return Json(e421);
                case 422:
                    var e422 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unprocessable Entity",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e422);
                    return Json(e422);
                case 423:
                    var e423 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Locked",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e423);
                    return Json(e423);
                case 424:
                    var e424 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Failed Dependency",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e424);
                    return Json(e424);
                case 425:
                    var e425 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Too Early",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e425);
                    return Json(e425);
                case 426:
                    var e426 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Upgrade Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e426);
                    return Json(e426);
                case 427:
                    var e427 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e427);
                    return Json(e427);
                case 428:
                    var e428 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Precondition Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e428);
                    return Json(e428);
                case 429:
                    var e429 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Too Many Requests",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e429);
                    return Json(e429);
                case 430:
                    var e430 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e430);
                    return Json(e430);
                case 431:
                    var e431 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Request Header Fields Too Large",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e431);
                    return Json(e431);
                case int n when (n >= 432 && n <= 450):
                    var e432_e450 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e432_e450);
                    return Json(e432_e450);
                case 451:
                    var e451 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unavailable For Legal Reasons",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e451);
                    return Json(e451);
                case int n when (n >= 452 && n <= 499):
                    var e452_e499 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e452_e499);
                    return Json(e452_e499);
                case 500:
                    var e500 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Internal Server Error",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e500);
                    return Json(e500);
                case 501:
                    var e501 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Not Implemented",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e501);
                    return Json(e501);
                case 502:
                    var e502 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Bad Gateway",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e502);
                    return Json(e502);
                case 503:
                    var e503 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Service Unavailable",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e503);
                    return Json(e503);
                case 504:
                    var e504 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Gateway Timeout",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e504);
                    return Json(e504);
                case 505:
                    var e505 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "HTTP Version Not Supported",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e505);
                    return Json(e505);
                case 506:
                    var e506 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Variant Also Negotiates",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e506);
                    return Json(e506);
                case 507:
                    var e507 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Insufficient Storage",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e507);
                    return Json(e507);
                case 508:
                    var e508 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Loop Detected",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e508);
                    return Json(e508);
                case 509:
                    var e509 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e509);
                    return Json(e509);
                case 510:
                    var e510 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Not Extended",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e510);
                    return Json(e510);
                case 511:
                    var e511 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Network Authentication Required",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e511);
                    return Json(e511);
                case int n when (n >= 512 && n <= 599):
                    var e512_e599 = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = NumError,
                            source = new Source { pointer = "" },
                            title = "Unassigned",
                            detail = ""
                        }
                    };
                    //json = JsonConvert.SerializeObject(e512_e599);
                    return Json(e512_e599);
                default:
                    var FailedRequest = new SuatResponse
                    {
                        success = false,
                        data = {},
                        errors = new Errors
                        {
                            code = 0,
                            source = new Source { pointer = "" },
                            title = "Unknown error",
                            detail = "An unexpected error has ocurred."
                        }
                    };
                    //json = JsonConvert.SerializeObject(FailedRequest);
                    return Json(FailedRequest);
            }
        }
    }
}