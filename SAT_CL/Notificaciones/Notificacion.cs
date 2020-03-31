using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Xml.Linq;

namespace SAT_CL.Notificaciones
{
    public class Notificacion : Disposable
    {
        #region Enumeracion
        /// <summary>
        /// Enumera las formas de codificar los mensajes de texto para los caracteres especiales no permitidos
        /// </summary>
        public enum CodificacionMensaje
        {
            /// <summary>
            /// Caracteres  minimo 160 (Sin acentos)
            /// </summary>
            defecto = 1,
            /// <summary>
            /// Caracteres minimos 70 (Acepta acentos)
            /// </summary>
            unicode
        }
        /// <summary>
        /// Determina las diferentes tipo de envio de mensajes (parametros)
        /// </summary>
        public enum TipoEnvio
        {
            /// <summary>
            /// Envia un mensaje a un destinatario
            /// </summary>
            MensajeyDestinatario = 1,
            /// <summary>
            /// Envia un mensaje a multiples destinatarios
            /// </summary>
            MultiplesDestinatariosMensaje,
            /// <summary>
            /// Envia diferentes mensajes a diferentes destinatarios
            /// </summary>
            MultiplesDestinatariosMultiplesMensajes
            /////// <summary>
            /////// Envia mensaje a un destinatario un mensaje con codigo de recepcion del mensaje
            /////// </summary>
            ////MensajeEnvioRecepcionDestinatario,
            /////// <summary>
            /////// Envia a multiples destinatarios un mensaje con codigo de recepcion del mensaje
            /////// </summary>
            ////MultiplesDestinatariosEnvioRecepcion,
            /////// <summary>
            /////// Envia a multiples destinatarios diferentes mensajes con codigo de recepcion del mensaje
            /////// </summary>
            ////MultiplesDestinatariosMultiplesMensajesEnvioRecepcion,
            /////// <summary>
            /////// Mensaje con codificación de acceso al sistema a un destinatario
            /////// </summary>
            ////MensajeAccesoSistemaDestinatario,
            /////// <summary>
            /////// Envia un mensaje con la liga de acceso al sistema a multiples destinatarios
            /////// </summary>
            ////MensajeAccesoSistemaMultiplesDestinatario,
            /////// <summary>
            /////// Envia multiples mensajes con liga de acceso al sistema (Módulo) a multiples destinatarios
            /////// </summary>
            ////MultiplesMensajeAccesoSistemaMultiplesDestinatario
        }
        public enum TiposCodifiacionTexto
        {
            MsgTexto = 1,
            UrlTexto,
            MsgMultimedia,
            UrlMultimedia
        }
        /// <summary>
        /// 
        /// </summary>
        public enum TipoMensaje
        {
            Texto = 1,
            Multimedia
        }
        #endregion

        #region Atributos
        private string _usuario;
        /// <summary>
        /// Nombre de usuraio del servicio altiria
        /// </summary>
        public string usuario { get => _usuario; }
        private string _password;
        /// <summary>
        /// Contraseña de seguridad del servicio Altiria
        /// </summary>
        public string password { get => _password; }
        private string _dominio;
        /// <summary>
        /// ID Dominio del servivcio Altiria
        /// </summary>
        public string dominio { get => _dominio; }
        private string _url;
        /// <summary>
        /// Direccion del la Api del servicio Altiria
        /// </summary>
        public string url { get => _url; }
        private string _xml_errores;
        #endregion

        #region Constructor

        public Notificacion()
        {
            this._url = ConfigurationManager.AppSettings["AltiraURL"].ToString();
            this._dominio = ConfigurationManager.AppSettings["domainID"].ToString();
            this._usuario = ConfigurationManager.AppSettings["user"].ToString();
            this._password = ConfigurationManager.AppSettings["password"].ToString();
            this._xml_errores = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../TECTOS_GMAO_CL/Notificacion/ErrorSMS.xml");//@"~/Notificacion/ErrorSMS.xml";
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Envio de un mensaje de texto a un destinatario
        /// </summary>
        /// <param name="destinatario">Numero telefonico del destinatario</param>
        /// <param name="mensaje">Mensaje al Destinatario (sustituir  &  = %26  signo menor que = %3C, signo mayor que = %3E)</param>
        /// <param name="url">Texto de tipo URL</param>
        /// <param name="tipo_codificacion">Determina la codificación del mensaje (unicode, defecto)</param>
        /// <returns></returns>
        private RetornoOperacion configuracionEnvioMensajeTextoSMS(string destinatario, string mensaje, CodificacionMensaje tipo_codificacion)
        {

            //Variable que obtendra el resultado del envio de mensaje de texto
            RetornoOperacion resultado = new RetornoOperacion();
            //Codifica el mensaje acorde al tipo Defecto o unicode
            string mensajeCodificado = codificaTexto(mensaje, tipo_codificacion, TiposCodifiacionTexto.MsgTexto);

            //almacena el cuerpo de envio del mensaje
            string lcPostData = "";
            //Valida que los datos de mensaje y destinatario sean correctos
            resultado = validaTextoMensajeyDestinatarios(mensaje, destinatario, null, null, tipo_codificacion, TipoEnvio.MensajeyDestinatario, TiposCodifiacionTexto.MsgTexto);
            //Si son correctos
            if (resultado.OperacionExitosa)
            {
                //Verifica si se cuenta con credito
                resultado = verificaCredito(1);
                //Si se cuenta con credito 
                if (resultado.OperacionExitosa)
                {

                    //CUERPO DEL MENSAJE
                    lcPostData =
                     "cmd=sendsms&domainId=" + this._dominio + "&login=" + this._usuario + "&passwd=" + this._password + "&dest=" + destinatario + "&msg=" + mensajeCodificado + "&senderId=TECTOS&encoding=" + tipo_codificacion.ToString();

                }

                //codificación del envio del mensaje con el proveedor altiria
                resultado = new RetornoOperacion(codigoErroresEnvio(envioMensaje(lcPostData)), true);

            }
            //retorna el resultado al método                                                      
            return resultado;
        }

        /// <summary>
        /// Envio del mismo mensaje de texto a multiples destinatarios
        /// </summary>
        /// <param name="destinatarios">Lista de numeros telefonicos a los cuales se les envia un mensaje</param>
        /// <param name="mensaje">Mensaje que se envia al destinatario (sustituir  &  = %26  signo menor que = %3C, signo mayor que = %3E)</param>
        /// <param name="url">Texto de tipo URL</param>
        /// <param name="tipo_codificacion">Determina la codificación del mensaje (unicode, defecto)</param>
        /// <returns></returns>
        private RetornoOperacion configuracionEnvioMensajeTextoSMS(List<string> destinatarios, string mensaje, CodificacionMensaje tipo_codificacion)
        {
            //Almacena la lista de destinatario codificada
            string telefonos = "";
            //Almacena el cuerpo del mensaje
            string lcPostData = "";

            //Codifica el mensaje acorde al tipo Defecto o unicode
            string mensajeCodificado = codificaTexto(mensaje, tipo_codificacion, TiposCodifiacionTexto.MsgTexto);

            //Variable de retorno al método
            RetornoOperacion retorno = new RetornoOperacion();

            //Verifica que los datos sean correctos
            retorno = validaTextoMensajeyDestinatarios(mensaje, "", destinatarios, null, tipo_codificacion, TipoEnvio.MultiplesDestinatariosMensaje, TiposCodifiacionTexto.MsgTexto);
            //Valida que sean correctos los datos
            if (retorno.OperacionExitosa)
            {
                //Verifica el credito
                retorno = verificaCredito(destinatarios.Count);

                //Valida que hay credito
                if (retorno.OperacionExitosa)
                {
                    //Recorre la lista de destinatarios  codifica la lista 
                    for (int i = 0; i <= destinatarios.Count(); i++)
                    {
                        telefonos = string.Format("{1}&dest={0}", destinatarios[i], telefonos);
                    }

                    //CUERPO DEL MENSAJE
                    lcPostData = "cmd=sendsms&domainId=" + this._dominio + "&login=" + this._usuario + "&passwd=" + this._password + telefonos + "&msg=" + mensajeCodificado + "&senderId=TECTOS&encoding=" + tipo_codificacion.ToString();

                    //Obtiene el resultado del envio
                    retorno = new RetornoOperacion(codigoErroresEnvio(envioMensaje(lcPostData)), true);
                }
            }
            //Retorna el resultado al método                
            return retorno;
        }

        /// <summary>
        /// Envia mensajes distintos por destinatario solo texto
        /// </summary>
        /// <param name="lista_mensajes_destinatarios">Tabla de mensajes por destinatario especifica En la columna "Destinatario", "Mensaje" ( en el mensaje sustituir  &  = %26  signo menor que = %3C, signo mayor que = %3E)</param>
        /// <param name="tipo_codificacion">Determina la codificación del mensaje (unicode, defecto)</param>
        /// <returns></returns>
        private RetornoOperacion configuracionEnvioMensajeTextoSMS(List<Tuple<string, string>> lista_mensajes_destinatarios, CodificacionMensaje tipo_codificacion)
        {
            //Variable de retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Almacena la lista de destinatarios con el formato requerido para el envio
            string resultado = "";
            //Almacena el cuerpo de envio del mensaje
            string lcPostData = "";

            //Valida si la lista de mensaje y destinatarios tienen el formato correcto
            retorno = validaTextoMensajeyDestinatarios("", "", null, lista_mensajes_destinatarios, tipo_codificacion, TipoEnvio.MultiplesDestinatariosMultiplesMensajes, TiposCodifiacionTexto.MsgTexto);
            //Si son correctos los datos
            if (retorno.OperacionExitosa)
            {
                //Verifica el credito
                retorno = verificaCredito(lista_mensajes_destinatarios.Count);
                //Si hay credito suficiente
                if (retorno.OperacionExitosa)
                {
                    //Recorre la tabla
                    foreach (Tuple<string, string> r in lista_mensajes_destinatarios)
                    {
                        string mensajeCodificado = codificaTexto(r.Item2, tipo_codificacion, TiposCodifiacionTexto.MsgTexto);
                        //CUERPO DEL MENSAJE
                        lcPostData = "cmd=sendsms&domainId=" + this._dominio + "&login=" + this._usuario + "&passwd=" + this._password + "&dest=" + r.Item1 + "&msg=" + mensajeCodificado + "&senderId=TECTOS&encoding=" + tipo_codificacion.ToString(); ;

                        resultado = string.Format("{0}.{1}", envioMensaje(lcPostData), retorno);
                    }

                    //Obtiene el resultado del envio
                    retorno = new RetornoOperacion(codigoErroresEnvio(resultado), true);
                }
            }

            //Retorna el resultado al método
            return retorno;

        }

        /// <summary>
        /// Método que realiza la conexión a la Api de altiria
        /// </summary>
        /// <param name="mensaje">Mensaje ya configurado para su envio</param>
        /// <returns></returns>
        private string envioMensaje(string mensaje)
        {
            //Variable de retorno para el método
            string retorno = "";

            //Se fija la URL sobre la que enviar la petición POST
            HttpWebRequest loHttp =
              (HttpWebRequest)WebRequest.Create(this._url);

            // Se codifica en utf-8
            byte[] lbPostBuffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(mensaje);
            loHttp.Method = "POST";
            loHttp.ContentType = "application/x-www-form-urlencoded";
            loHttp.ContentLength = lbPostBuffer.Length;

            //Fijamos tiempo de espera de respuesta = 60 seg
            loHttp.Timeout = 60000;
            String error = "";
            String response = "";
            // Envía la peticion
            try
            {
                Stream loPostData = loHttp.GetRequestStream();
                loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                loPostData.Close();
                // Prepara el objeto para obtener la respuesta
                HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
                // La respuesta vendría codificada en utf-8
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader loResponseStream =
                new StreamReader(loWebResponse.GetResponseStream(), enc);
                // Conseguimos la respuesta en una cadena de texto
                response = loResponseStream.ReadToEnd();
                loWebResponse.Close();
                loResponseStream.Close();
            }
            //Si existen problemas de conexion con la api de Altiria
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                    error = "ERROR errNum:CA";
                else if (e.Status == WebExceptionStatus.Timeout)
                    error = "ERROR errNum:TO";
                else
                    error = e.Message;
            }
            //Finalmente asigna a la variable retorno lo devuelto por altiria 
            finally
            {
                //Si existen errores de conexion
                if (error != "")
                    retorno = error.Replace("\n", " ");
                //Si existen errores de envio de SMS
                else
                    retorno = response.Replace("\n", " ");
            }
            //Devulve la variable al método
            return retorno;
        }

        /// <summary>
        /// Método que verifica si existe credito para poder enviar notificaciones
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion verificaCredito(int cantidadMensajes)
        {
            //Variable de retorno al método
            RetornoOperacion retorno = new RetornoOperacion();
            //Variable que almacenara el valor de retorno de la consulta de credito
            string credito = "";
            //configuración de la consulta de credito
            string lcPostData =
            "cmd=getcredit&domainId=" + this._dominio + "&login=" + this._usuario + "&passwd=" + this._password;

            //REaliza la consulta
            credito = envioMensaje(lcPostData);

            //Verefica si hay credito
            if (credito.Contains("OK credit"))
            {
                //Verfica si el credito es lo suficiente
                if ((Convert.ToDecimal(Cadena.RegresaCadenaSeparada(credito, ":", 1)) > 5.0M) && (Convert.ToDecimal(Cadena.RegresaCadenaSeparada(credito, ":", 1)) - cantidadMensajes * 0.45M > 5.0M))
                {
                    //Envia retorno positivo
                    retorno = new RetornoOperacion(1);
                }
                //Si ya no tenemos mucho credito
                else
                {
                    //envia mensaje a Ivan dando el saldo actual 
                    retorno = configuracionEnvioMensajeTextoSMS("525536601029", string.Format("No cuenta con saldo suficiente para realizar envios de Mensajes Altiria. Solo cuenta con {0} de credito.", Convert.ToDecimal(Cadena.RegresaCadenaSeparada(credito, ":", 1))), CodificacionMensaje.unicode);

                }
            }
            //Si existe un error en la consulta de credito
            else if (credito.Contains("errNum"))
            {
                //Envia de error  recibido en positivo
                retorno = new RetornoOperacion(codigoErroresEnvio(credito), true);
            }
            //Retorna ál método el valor de la variable retorno
            return retorno;
        }
        /// <summary>
        /// Método que a partir de la respuesta del servidor altira devuelve valores
        /// </summary>
        /// <param name="resultado">Resultado devuelto de altira</param>
        /// <returns></returns>
        private string codigoErroresEnvio(string resultado)
        {
            //Variable de retorno al método
            string retorno = "";

            //Almacena las lineas de retorno de resultado
            string[] separador;

            //almacena la lista de resultados de errores
            List<string> listaErrores = new List<string>();
            //Almacena los destinatarios
            List<string> listaDestinatarios = new List<string>();

            //Obtiene la lista de errores 
            XDocument doc = XDocument.Load(this._xml_errores);
            //Obtiene los elementos errores 
            IEnumerable<XElement> Errores = doc.Elements("EsquemaAltiria");
            //Obtiene los elementos de codigo
            IEnumerable<XElement> codigo = Errores.Elements("CodigoErrores");
            IEnumerable<XElement> Errnum = codigo.Elements("ErrNum");
            //Elemento que almacenara el valor del elemento Codigo
            XElement num;

            //Elimina el ultimo caracter del string resultado
            resultado = resultado.EndsWith(".") ? resultado.Remove(resultado.Length - 1, 1) : resultado;

            //Obtiene la frase despues tomando como referencia el .
            separador = resultado.Split('.');

            //Tabla que almacena los datos separados del resultado
            using (DataTable dt = new DataTable())
            {
                dt.Columns.Add("Tipo", typeof(string));
                dt.Columns.Add("Destinatario", typeof(string));
                dt.Columns.Add("Codigo", typeof(string));

                //Variable de tipo row
                DataRow r;

                //Asignación de nueva fila a la tabla
                r = dt.NewRow();
                //Recorre el arreglo que almacena el resultado del envio de SMS
                for (int i = 0; i < separador.Length; i++)
                {
                    dt.Rows.Add(separador[i].Contains("ERROR") ? "ERROR" : "OK",
                                separador[i].Contains("dest") ? Cadena.RegresaCadenaSeparada(Convert.ToString(separador[i]), "dest", 1).Substring(Cadena.RegresaCadenaSeparada(Convert.ToString(separador[i]), "dest", 1).IndexOf(":") + 1, 12) : "Sin Destino",
                                separador[i].Contains("ERROR") ? Convert.ToString(Cadena.RegresaCadenaSeparada(Convert.ToString(separador[i]), "errNum:", 1)) : "OK");
                }
                //Recorre la tabla 
                foreach (DataRow p in dt.Rows)
                {
                    //Obtiene la descripcion del codigo de errores del xml
                    num = (from XElement en in Errnum
                           where p["Codigo"].Equals(en.Attribute("Value").Value)
                           select en).FirstOrDefault();
                    //Valida que no sea nullo
                    if (num != null)
                    {
                        //Variable de Retorno
                        retorno = string.Format("{1} Destino:{2}.\n{0}", retorno, num.Value, p["Destinatario"]);
                    }

                }

            }

            //Retorno del valor al método            
            return retorno;
        }

        /// <summary>
        /// Método que valida la cantidad de caracteres de mensajes y Telefonos
        /// </summary>
        /// <param name="mensaje">Mensaje a enviar</param>
        /// <param name="destinatario">Telefono del destinatario</param>
        /// <param name="destinatarios">Lista de telefonos de destinatarios</param>
        /// <param name="multiplesMensajesDestinatarios">Lista de destinatarios con su mensaje propio (Telefono,Mensaje) </param>
        /// <param name="tipo_codificacion">Tipo de codificación de caracteres del mensaje (unicode o defecto)</param>
        /// <param name="tipo_envio">Acorde al tipo de envio deternima como realizara la validación</param>
        /// <param name="tipo_codificacion_texto">Acorde al tipo de envio deternima como realizara la validación MsgTexto,MsgMultimedia</param>
        /// <returns></returns>
        private RetornoOperacion validaTextoMensajeyDestinatarios(string mensaje, string destinatario, List<string> destinatarios, List<Tuple<string, string>> multiplesMensajesDestinatarios, CodificacionMensaje tipo_codificacion, TipoEnvio tipo_envio, TiposCodifiacionTexto tipo_codificacion_texto)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            string mensajeErrorTelefono = "", mensajeErrorTexto = "";
            int cantidadCaracteres = cantidadCaracteresTexto(mensaje, tipo_codificacion, tipo_codificacion_texto);

            switch ((TipoEnvio)tipo_envio)
            {
                //Un mensaje un destinatario
                case TipoEnvio.MensajeyDestinatario:
                    {
                        //Valida la cantidad de digitos de telefono
                        if (destinatario.Length == 12)
                        {
                            //Acorde al tipo de codificacion valida la cantidad de caracteres
                            if (cantidadCaracteres <= (tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70))
                            {
                                retorno = new RetornoOperacion(1);
                            }
                            //envia mensaje de error ya que el mensaje es largo
                            else
                                retorno = new RetornoOperacion(string.Format("El mensaje es muy largo,cant. caracteres minimo {0}. Cant. de caracteres del Mensaje {1}.", tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70, cantidadCaracteres), false);
                        }
                        //envia mensaje de error ya que el numero telefonico no es valido
                        else
                            retorno = new RetornoOperacion(string.Format("El Teléfono {0} no es valido, Cant. digitos es {1} (2 - lada)(10 - Telefono).", destinatario, destinatario.Length), false);
                    }
                    break;
                //Un mensaje para multiples destinatarios
                case TipoEnvio.MultiplesDestinatariosMensaje:
                    {
                        //Recorre la lista de destinatarios y valida los telefonos
                        for (int i = 0; i < destinatarios.Count; i++)
                        {
                            //Valida la cantidad de digitos
                            if (destinatario[i].ToString().Length != 12)
                            {
                                //Muestra el telefono y la cantidad de digitos
                                mensajeErrorTelefono = string.Format("El Teléfono {0} no es valido, Cant. digitos es {1} (2 - lada)(10 - Telefono).{2}", destinatario[i], destinatario[i].ToString().Length, mensajeErrorTelefono);
                            }
                        }
                        //Valida si el mensaje de error  que almacena el resultado de la evaluacion de los telefonos
                        if (mensajeErrorTelefono == "")
                        {
                            //Acorde al tipo de codificacion valida la cantidad de caracteres
                            if (cantidadCaracteres <= (tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70))
                            {
                                retorno = new RetornoOperacion(1);
                            }
                            //envia mensaje de error ya que el mensaje es largo
                            else
                                retorno = new RetornoOperacion(string.Format("El mensaje es muy largo, cant. caracteres minimo {0}. Cant. de caracteres del Mensaje {1}", tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70, cantidadCaracteres), false);
                        }
                        //Muestra mensaje de error
                        else
                            retorno = new RetornoOperacion(mensajeErrorTelefono, false);
                    }
                    break;
                //Multiples destinatarios multiples mensajes
                case TipoEnvio.MultiplesDestinatariosMultiplesMensajes:
                    {
                        //Variable que almacena la cantidad de caracteres por mensajes 
                        int cantCaracteresMensaje = 0;
                        //Recorre la tupla de destinatarios y mensajes 
                        foreach (Tuple<string, string> r in multiplesMensajesDestinatarios)
                        {
                            //obtiene la cantidad de caracteres por mensaje
                            cantCaracteresMensaje = cantidadCaracteresTexto(r.Item2, tipo_codificacion, tipo_codificacion_texto);
                            //Valida la cantidad de caracteres para el destinatario sean 12
                            if (r.Item1.Length != 12)
                            {
                                //Envia mensaje de error en el destinatario
                                mensajeErrorTelefono = string.Format("El Teléfono {0} no es valido, Cant. digitos es {1}, cant. minima 12.{2}", r.Item1, r.Item1.Length, mensajeErrorTelefono);
                            }
                            //Valida cantidad de caracteres por mensaje a corde al tipo de codificacion defecto o unicode
                            if (cantCaracteresMensaje > (tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70))
                            {
                                //envia mensaje de error
                                mensajeErrorTexto = string.Format("El mensaje es muy largo {0} minimo. Cantidad de caracteres del Mensaje {1}", tipo_codificacion == CodificacionMensaje.defecto ? 160 : 70, cantCaracteresMensaje);
                            }
                        }
                        //Si no exite error en el mensaje o en los destinatarios
                        if (mensajeErrorTelefono == "" & mensajeErrorTexto == "")
                        {
                            //Devuelve correcto
                            retorno = new RetornoOperacion(1);
                        }
                        //si existe problema en destinatarios
                        else if (mensajeErrorTelefono != "")
                        {
                            retorno = new RetornoOperacion(mensajeErrorTelefono, false);
                        }
                        //Si existe problemas en los mnesajes
                        else if (mensajeErrorTexto != "")
                        {
                            retorno = new RetornoOperacion(mensajeErrorTexto, false);
                        }
                    }
                    break;
            }
            //Valor de retorno al método
            return retorno;
        }

        /// <summary>
        /// Método que codifica el mensaje y devuelve valor de caracter acorde al tipo de texto
        /// </summary>
        /// <param name="mensaje">Mensaje a codificar</param>
        /// <param name="codificacion">Determina si es por defecto o unicode</param>
        /// <param name="tipo_mensaje">Determina el tipo de mensaje Texto o Multimedia</param>
        /// <param name="texto">Determina si el texto es de tipo URL o Mensaje</param>
        /// <returns></returns>
        private List<Tuple<int, string, string, int, int>> obtieneMensajeCodificado(string mensaje, TiposCodifiacionTexto texto)
        {
            //Variable de valorMensaje
            string valorMensaje = "";

            //Id|Letra|codificacion|cantidad|codificacion
            List<Tuple<int, string, string, int, int>> mensajecodificado = new List<Tuple<int, string, string, int, int>>();
            //Obtiene los elementos del documento configuracion Altiria             HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2.xslt")
            //XDocument doc = XDocument.Load("ErrorSMS.xml");
            XDocument doc = XDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../TECTOS_GMAO_CL/Notificacion/ErrorSMS.xml"));
            //XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/Notificacion/ErrorSMS.xml"));
            //Obtiene los elementos EsquemaAltiria 
            IEnumerable<XElement> altiria = doc.Elements("EsquemaAltiria");
            //Obtiene los elementos de Caracteres
            IEnumerable<XElement> caracteres = altiria.Elements("Caracteres");
            //Obtiene los atributos del arbol caracter
            IEnumerable<XElement> caracter = caracteres.Elements("Caracter");
            //Almacena los elementos del nodo caracter
            XElement valorCaracter;

            //Obtiene la configuracion de los caracteres
            IEnumerable<XElement> configuracionCaracteres = altiria.Elements("ConfiguracionCaracter");
            //Obtiene los nodos caractares
            IEnumerable<XElement> confCaracter = configuracionCaracteres.Elements("Caracteres");
            //Almacena los elementos del nodo caracteres
            XElement valorConfCaracter;

            //Elimina los caracteres en blanco
            valorMensaje = mensaje.Replace(" ", "");

            //Del mensaje obtiene las letras agrupadas y la cantidad en las que se repite en la frase
            var menas = from p in valorMensaje.GroupBy(p => p)
                        select new
                        {
                            letra = p,
                            cant = p.Count()
                        };

            //Recorre la lista de caracteres
            foreach (var i in menas)
            {
                //Obtiene del esquema Codificacion caracter los caracteres de la lista
                valorCaracter = (from XElement en in caracter
                                 where i.letra.Key.ToString().Equals(en.Attribute("Value").Value)    //join xx in r on en.Attribute("").Value equals xx.letra                            
                                 select en).FirstOrDefault();
                //Obtiene la configuracion del esquema caracteres
                valorConfCaracter = (from XElement cf in confCaracter
                                     where cf.Attribute("Value").Value.Contains(string.Format("|{0}|", valorCaracter.Attribute("ID").Value))
                                     select cf).FirstOrDefault();

                //Acorde al tipo de mensaje



                //Asigna valores al mensaje codificado
                mensajecodificado.Add(new Tuple<int, string, string, int, int>(Convert.ToInt32(valorCaracter.Attribute("ID").Value), i.letra.Key.ToString(), valorCaracter.Attribute("Codificacion").Value, i.cant, Convert.ToInt32(valorConfCaracter.Attribute(texto.ToString()).Value)));


            }

            //Devuelve el resultado al método
            return mensajecodificado;
        }

        /// <summary>
        /// Método que codifica el mensaje acorde al tipo
        /// </summary>
        /// <param name="mensaje">Mensaje a enviar</param>
        /// <param name="codificacion">Codifica el texto por defecto o unicode</param>
        /// <param name="texto">Identifica si es un MsgTexto, UrlTexto, MsgMultimedia, UrlMultimedia</param>
        /// <returns></returns>
        private string codificaTexto(string mensaje, CodificacionMensaje codificacion, TiposCodifiacionTexto texto)
        {
            //Obtiene los valores y la codificación de un mensaje
            List<Tuple<int, string, string, int, int>> obtieneMensaje = obtieneMensajeCodificado(mensaje, texto);

            //Acorde al tipo de codificación
            if (codificacion == CodificacionMensaje.defecto)
            {
                //Recorre la tupla que obtiene la codificación por caracter
                foreach (Tuple<int, string, string, int, int> it in obtieneMensaje)
                {
                    //Realiza un replace de caracteres
                    mensaje = mensaje.Replace(it.Item2, Convert.ToInt32(it.Item5) == 2 ? "?" : it.Item2);
                }
            }
            //Acorde al tipo de codificación unicode
            else if (codificacion == CodificacionMensaje.unicode)
            {
                //Recorre la tupla
                foreach (Tuple<int, string, string, int, int> it in obtieneMensaje)
                {
                    //Realiza el replace
                    mensaje = mensaje.Replace(it.Item2, Convert.ToInt32(it.Item5) == 2 ? it.Item3 : it.Item2);
                }
            }
            //Retorna la variable al método
            return mensaje;
        }

        /// <summary>
        /// Devuelve la cantidad de caracteres acorde a la codificación del mensaje y el tipo de codificacion del texto
        /// </summary>
        /// <param name="mensaje">Texto</param>
        /// <param name="codificacion">Codifica el texto por defecto o unicode</param>
        /// <param name="texto">Identifica si es un MsgTexto, UrlTexto, MsgMultimedia, UrlMultimedia</param>
        /// <returns></returns>
        private int cantidadCaracteresTexto(string mensaje, CodificacionMensaje codificacion, TiposCodifiacionTexto texto)
        {
            //Variable de retorno
            int retorno = 0;
            //Obtiene los caracteres codificados del mensaje
            List<Tuple<int, string, string, int, int>> obtieneMensaje = obtieneMensajeCodificado(mensaje, texto);

            //Acorde al tipo de codificación
            if (codificacion == CodificacionMensaje.defecto)
            {
                //Suma la cantidad de caracteres repetidas en el mensaje
                retorno = Convert.ToInt32(obtieneMensaje.Select(itu => itu.Item4).Sum());
            }
            //Acorde al tipo de codificación unicode
            else if (codificacion == CodificacionMensaje.unicode)
            {
                //Recorre la tupla
                foreach (Tuple<int, string, string, int, int> it in obtieneMensaje)
                {
                    //Suma (la cantidad de caracteres repetidas multiplicado por el valor codificado acorde al tipo)
                    retorno += Convert.ToInt32(it.Item4) * Convert.ToInt32(it.Item5);
                }
            }
            //Suma los espacios en blanco
            retorno += mensaje.Count(Char.IsWhiteSpace);

            //Devuelve la variable al método
            return retorno;
        }
        /// <summary>
        /// Envia un Correo de Tipo Notificación
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="remitente">Remitente</param>
        /// <param name="asunto">Asunto del Correo</param>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <param name="pie">Pie de la Plantilla</param>
        /// <param name="destinatarios">Destinatario</param>
        /// <param name="url_calificacion">URL Calificacion</param>
        ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        private static RetornoOperacion EnviaArchivosEmail(int id_compania_emisor, string remitente, string asunto, string encabezado, string titulo, string subtitulo,
                                               string tituloCuerpo, string cuerpo, string pie, string[] destinatarios)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Creando mensaje personalizado para CFDI
            string mensajeCFDI = generaMensajeEmail(encabezado, titulo, subtitulo, tituloCuerpo, cuerpo, pie);

            //Enviamos Email
            //Instanciando Correo Electronico
            using (Correo email = new Correo(remitente, destinatarios, asunto, mensajeCFDI, true))
            {

                //Enviando Correo Electronico
                bool enviar = email.Enviar();

                //Si no se envío el mensaje
                if (!enviar)
                {
                    string errores = "";
                    //Recorriendo los errores del envío
                    foreach (string error in email.Errores)
                        //Añadiendo errores a la lista
                        errores = errores + error + "<br />";
                    resultado = new RetornoOperacion(errores);
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el mensaje del contenido de correo en base a la plantilla predefinida para esta opción de Notificación
        /// </summary>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo del Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <param name="pie">Pie de la Plantilla</param>
        ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        protected static string generaMensajeEmail(string encabezado, string titulo, string subtitulo, string tituloCuerpo, string cuerpo, string pie)
        {
            //Declaramos Variable Retorno
            cuerpo = "";
            //Formato redeterminado
            string formato = File.ReadAllText(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "Notificacion.html");
            //Si se Desea Registrar una Tabla en el Cuerpo
            //if (mitCuerpo != null)
            //{
            //    //Obtenmos HTML de la Tabla
            //    cuerpo = DevuelveTablaAHTML(mitCuerpo, actualBase64, realizadoBase64, sinRealizarBase64);
            //}
            //Obteniendo las imagenes a incluir en base64
            string estrellasCalificar = Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + ("Estrella5.png")));

            //Declarando objeto de retorno
            return formato.Replace("{0}", encabezado).Replace("{1}", titulo).Replace("{2}", subtitulo).Replace("{3}", tituloCuerpo).Replace("{4}", cuerpo).Replace("{5}", pie);

        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que envía mensajes acorde al tipo Multimedia  
        /// </summary>
        /// <param name="tipo_mensaje">Determina el tipo de Mensaje Texto o Multimedia</param>
        /// <param name="mensaje">Un mensaje de texto</param>
        /// <param name="destinatario">Un numero de destinatario</param>
        /// <param name="destinatarios">Lista de destinatarios</param>
        /// <param name="multiplesMensajesDestinatarios">Multiples destinatarios y mensajes</param>
        /// <param name="tipo_codificacion">Determina si se codifica el texto Unicode o por defecto</param>
        /// <param name="tipo_envio">Determina los tipos de envio acorde a la cantidad de destinatarios y mensajes </param>
        /// <param name="tipo_codificacion_texto">Permite determinar la codificación de Texto y URL acorde al tipo de mensaj Multimedia o Texto</param>
        /// <returns></returns>
        public RetornoOperacion EnviaMensaje(TipoMensaje tipo_mensaje, string mensaje, string destinatario, List<string> destinatarios, List<Tuple<string, string>> multiplesMensajesDestinatarios, CodificacionMensaje tipo_codificacion, TipoEnvio tipo_envio, TiposCodifiacionTexto tipo_codificacion_texto)
        {
            //Almacena el resultado del método
            RetornoOperacion retorno = new RetornoOperacion();
            //Acorde al tipo de mensaje

            switch (tipo_mensaje)
            {
                //Si es texto
                case TipoMensaje.Texto:
                    //De un destinatario y mensaje
                    if (destinatario != "" && mensaje != "")
                    {
                        retorno = configuracionEnvioMensajeTextoSMS(destinatario, mensaje, tipo_codificacion);
                    }
                    //Una lista de destinatarios y un mismo mensaje
                    else if (destinatarios != null && mensaje != "")
                    {
                        retorno = configuracionEnvioMensajeTextoSMS(destinatarios, mensaje, tipo_codificacion);
                    }
                    //Multiples destinatarios y mensaje
                    else if (multiplesMensajesDestinatarios != null)
                    {
                        retorno = configuracionEnvioMensajeTextoSMS(multiplesMensajesDestinatarios, tipo_codificacion);
                    }
                    break;
                case TipoMensaje.Multimedia:
                    break;
            }


            //devuelve el valor de la variable al método
            return retorno;
        }
        /// <summary>
        /// Método en envio a los contactos de una lista de distribución
        /// </summary>
        /// <param name="tipo_autorizacion"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public RetornoOperacion EnviaNotificacionLista(int id_lista_distribucion, string mensaje)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            List<string> destinatarios = new List<string>();
            List<Tuple<string, string>> multiplesMensajesDestinatarios = new List<Tuple<string, string>>();
            //using (DataTable dtLista = Autorizacion.TipoAutorizacionListaDistribucion.ObtieneListaDistribucion(tipo_autorizacion))
            //{
            //    if (Validacion.ValidaOrigenDatos(dtLista))
            //    {
            //        //Recorriendo Detalles
            //        foreach (DataRow dr in dtLista.Rows)
            //        {
                        using (DataTable dtDetalle = Notificaciones.ListaDistribucionDetalleContacto.ObtieneContactosLista(id_lista_distribucion))
                        {
                            if (Validacion.ValidaOrigenDatos(dtDetalle))
                            {
                                //Recorriendo Detalles
                                foreach (DataRow drd in dtDetalle.Rows)
                                {
                                    using (SAT_CL.Global.Contacto c = new Global.Contacto(Convert.ToInt32(drd["IdContacto"])))
                                    {
                                        retorno = EnviaMensaje(TipoMensaje.Texto, mensaje, c.telefono, destinatarios, multiplesMensajesDestinatarios, CodificacionMensaje.defecto, TipoEnvio.MensajeyDestinatario, TiposCodifiacionTexto.MsgTexto);
                                    }
                                }
                            }
                            else
                                retorno = new RetornoOperacion("No se encontraron Detalles de Lista");
                        }
            //        }
            //    }
            //    else
            //        retorno = new RetornoOperacion("No se encontraron Listas Ligadas");
            //}
            return retorno;
        }
        /// <summary>
        /// Envia notificacion a un contacto en especifico
        /// </summary>
        /// <param name="id_contacto"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public RetornoOperacion EnviaNotificacionContacto(int id_contacto, string mensaje)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            List<string> destinatarios = new List<string>();
            List<Tuple<string, string>> multiplesMensajesDestinatarios = new List<Tuple<string, string>>();
            using (SAT_CL.Global.Contacto c = new Global.Contacto(id_contacto))
            {
                retorno = EnviaMensaje(TipoMensaje.Texto, mensaje, c.telefono, destinatarios, multiplesMensajesDestinatarios, CodificacionMensaje.defecto, TipoEnvio.MensajeyDestinatario, TiposCodifiacionTexto.MsgTexto);
            }                    
            return retorno;
        }
        /// <summary>
        /// Enviamos Correo de Notificaciones 
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="asunto">Asunto del Mensaje</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_tabla">Id Tabla (Ubicacion=15)</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_tipo_evento">Tipo de Evento a Realizar</param>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        public static RetornoOperacion EnviaCorreo(int id_compania_emisor, int id_contacto, string asunto, string encabezado, string titulo, string subtitulo, string tituloCuerpo, string cuerpo, 
                                       string mensaje)
        {
            //Declaramos Variables 
            RetornoOperacion resultado = new RetornoOperacion();
            //Intsnamos Tipo de Evento
            //using (TipoEventoNotificacion tipoEventoNotificacion = new TipoEventoNotificacion(id_tipo_evento))
            //{
            //    //Cargamos Notificaciones de acuerdo a los criterios Obtenidos
            //    using (DataTable mitNotificaciones = CargaNotificaciones(id_compania_emisor, id_cliente, id_tabla, id_registro, id_tipo_evento))
            //    {
            //        //Validamos que existan Notificaciones
            //        if (Validacion.ValidaOrigenDatos(mitNotificaciones))
            //        {
            //            //Recorremos cada uno de los Correos
            //            foreach (DataRow r in mitNotificaciones.Rows)
            //            {
            using (SAT_CL.Global.Contacto c = new Global.Contacto(id_contacto))
            {
                //Declaramos Variable Destinatario
                string[] destinatario = new string[] { c.email };
                //Enviamos E-mail
                resultado = EnviaArchivosEmail(id_compania_emisor, destinatario[0], asunto, encabezado, titulo, subtitulo, tituloCuerpo, cuerpo, mensaje, destinatario);
            }
            //            }
            //        }
            //    }
            //}
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Devolver una Tabla HTML a Partir de un DataTable
        /// </summary>
        /// <param name="mit">Tabla a convertir</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <returns></returns>
        public static string DevuelveTablaAHTML(DataTable mit, string actualBase64, string realizadoBase64, string sinRealizarBase64)
        {

            //Declaramos Variables
            StringBuilder html = new StringBuilder();

            //Validamos Origen de Datos
            if (Validacion.ValidaOrigenDatos(mit))
            {
                //Creamos el encabezado de la Tabla
                html.Append("<table>");

                //Creamos el encabezado de la fila
                html.Append("<tr>");
                foreach (DataColumn column in mit.Columns)
                {
                    html.Append("<th>");
                    html.Append(column.ColumnName);
                    html.Append("</th>");
                }
                html.Append("</tr>");

                //Construimos la información de cada una de la Filas
                foreach (DataRow row in mit.Rows)
                {
                    html.Append("<tr>");
                    foreach (DataColumn column in mit.Columns)
                    {
                        html.Append("<td>");
                        //Validamos si es Columna de Imagen
                        //Imagen de Eventos, Paradas, etc. Sin Realizar
                        if (row[column.ColumnName].ToString() == "SinRealizar")
                        {
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + sinRealizarBase64 + "/></DIR></DIR>");
                        }
                        //Imagen de Eventos, Paradas, etc Actual.
                        else if (row[column.ColumnName].ToString() == "Actual")
                        {
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + actualBase64 + "/></DIR></DIR>");
                        }
                        else if (row[column.ColumnName].ToString() == "Realizado")
                        {
                            //Imagen de Eventos, Paradas Realizadas.
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + realizadoBase64 + "/></DIR></DIR>");
                        }
                        else
                        {
                            html.Append(row[column.ColumnName]);
                        }
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                }

                //Cerramo etiqueta de creación de Tabla
                html.Append("</table>");
            }
            //Devolvemos String HTML de la Tabla
            return html.ToString().Replace("src=", "src=\"").ToString().Replace("==/>", "==\">");
        }
        #endregion
    }
}

