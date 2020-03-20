using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los PAC de las Compañias Emisoras
    /// </summary>
    public class PacCompaniaEmisor : Disposable
    {

        #region Enumeraciones

        /// <summary>
        /// Enumera el Tipo Ubicación
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Patios Cliente
            /// </summary>
            Timbrar = 1,
            /// <summary>
            ///Terminal
            /// </summary>
            Cancelar,
        }

        /// <summary>
        /// Define todos los receptores que solicitan addenda
        /// </summary>
        public enum PAC
        {
            /// <summary>
            /// FACTUREMOS YA
            /// </summary>
            FACTUREMOSYA = 64,
            /// <summary>
            /// FACTIRADOR
            /// </summary>
            FACTURADOR = 1120,
            /// <summary>
            /// FACTURADOR
            /// </summary>
            FACTURADORSAE = 4,
        }


        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "fe.sp_pac_compania_emisor_tpce";

        private int _id_pac_compania_emisor;
        /// <summary>
        /// Describe el Id PAC de la Compañia Emisor
        /// </summary>
        public int id_pac_compania_emisor { get { return _id_pac_compania_emisor; } }
        private int _id_compania_pac;
        /// <summary>
        /// Describe la Compañia PAC
        /// </summary>
        public int id_compania_pac { get { return _id_compania_pac; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Describe la Compañia Emisor
        /// </summary>
        public int id_compania_emisor { get { return _id_compania_emisor; } }
        private string _version;
        /// <summary>
        /// Describe la Versión del Comprobante del Web Service
        /// </summary>
        public string version { get { return _version; } }
        private byte _id_tipo;
        /// <summary>
        /// Describe el Tipo de acción
        /// </summary>
        public byte id_tipo { get { return _id_tipo; } }
        private string _usuario_web_servie;
        /// <summary>
        /// Describe el Usuario del mWeb Service
        /// </summary>
        public string usuario_web_servie { get { return _usuario_web_servie; } }
        private string _contrasena_web_service;
        /// <summary>
        /// Describe la contraseña del Web Service
        /// </summary>
        public string contrasena_web_service { get { return _contrasena_web_service; } }
        private string _ubicacion_web_service;
        /// <summary>
        /// Describe la Ubicación del Web Service
        /// </summary>
        public string ubicacion_web_service { get { return _ubicacion_web_service; } }
        private byte _secuencia_prioridad;
        /// <summary>
        /// Describe la secuencia
        /// </summary>
        public byte secuencia_prioridad { get { return _secuencia_prioridad; } }
        private bool _hablitar;
        /// <summary>
        /// Describe el Habilitar
        /// </summary>
        public bool hablitar { get { return _hablitar; } }
        /// <summary>
        /// Enumera el Tipo Ubicación
        /// </summary>
        public Tipo TipoUbicacion { get { return (Tipo)_id_tipo; } }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~PacCompaniaEmisor()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public PacCompaniaEmisor()
        {

        }
        /// <summary>
        /// Genera instancia ligando el Id Pac Compañia Emisor
        /// </summary>
        /// <param name="id_pac_compania_emisor"></param>
        public PacCompaniaEmisor(int id_pac_compania_emisor)
        {
            cargaAtributosInstancia(id_pac_compania_emisor);
        }

        /// <summary>
        /// Genera instancia ligando el Id Pac Compañia Emisor
        /// </summary>
        /// <param name="id_pac_compania_emisor">Id Pac Compañia Emisor</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_pac_compania_emisor)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_pac_compania_emisor, 0, 0, "", 0, "", "", "", 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_pac_compania_emisor = Convert.ToInt32(r["Id"]);
                        _id_compania_pac = Convert.ToInt32(r["IdCompaniaPac"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _version = r["Version"].ToString();
                        _id_tipo = Convert.ToByte(r["IdTipo"]);
                        _usuario_web_servie = r["UsuarioWebService"].ToString();
                        _contrasena_web_service = r["ContrasenaWebService"].ToString();
                        _ubicacion_web_service = r["UbicacionWebService"].ToString();
                        _secuencia_prioridad = Convert.ToByte(r["Secuencia"]);
                        _hablitar = Convert.ToBoolean(r["Habilitar"]);

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        #endregion

        #region Metodos privados

        /// <summary>
        /// Método encargado de editar un PAC Compañia Emisor
        /// </summary>
        /// <param name="id_compania_pac">PAC</param>
        /// <param name="id_compania_emisor">Compañia Emisor que es asignado el PAC</param>
        /// <param name="version">Versión del Comprobante del Web Service</param>
        /// <param name="tipo">Tipo de método a realizar (Timbrar, Cancelra)</param>
        /// <param name="usuario_web_servie">Usuario de acceso al servicio web de timbrado<</param>
        /// <param name="contrasena_web_service">Contraseña para el consumo del web service</param>
        /// <param name="ubicacion_web_service">Ubicación del Web Service</param>
        /// <param name="secuencia_prioridad">Prioridad para utilización del Web Service</param>
        /// <param name="id_usuario">Usuario </param>
        /// <param name="hablitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaPacCompaniaEmisor(int id_compania_pac, int id_compania_emisor, string version, Tipo tipo, string usuario_web_servie, string contrasena_web_service,
                                                string ubicacion_web_service, byte secuencia_prioridad, int id_usuario, bool hablitar)
        {

            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_pac_compania_emisor, id_compania_pac, id_compania_emisor, version, tipo, usuario_web_servie, contrasena_web_service, 
                                 ubicacion_web_service, secuencia_prioridad, id_usuario, hablitar, "", ""};
            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        #endregion

        #region Metodos Públicos

        /// <summary>
        /// Insertó una PAC Compañia Emisor
        /// </summary>
        /// <param name="id_compania_pac">PAC</param>
        /// <param name="id_compania_emisor">Compañia Emisor que es asignado el PAC</param>
        /// <param name="version">Versión del CFDI del Web Service</param>
        /// <param name="tipo">Tipo de método a realizar (Timbrar, Cancelra)</param>
        /// <param name="usuario_web_servie">Usuario de acceso al servicio web de timbrado<</param>
        /// <param name="contrasena_web_service">Clave de acceso al servicio web de timbrado</param>
        /// <param name="ubicacion_web_service">Ubicación del Web Service</param>
        /// <param name="secuencia_prioridad">Prioridad para utilización del Web Service</param>
        /// <param name="id_usuario">Usuario </param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPacCompaniaEmisor(int id_compania_pac, int id_compania_emisor, string version, Tipo tipo, string usuario_web_servie, string contrasena_web_service,
                                               string ubicacion_web_service, byte secuencia_prioridad, int id_usuario)
        {

            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_compania_pac, id_compania_emisor, version, tipo, usuario_web_servie, contrasena_web_service, 
                                 ubicacion_web_service, secuencia_prioridad, id_usuario, true, "", ""};
            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Edita un PAC Compañia Emisor
        /// </summary>
        /// <param name="id_compania_pac">PAC</param>
        /// <param name="id_compania_emisor">Compañia Emisor que es asignado el PAC</param>
        /// <param name="version">Versión del CFDI del Web Service</param>
        /// <param name="tipo">Tipo de método a realizar (Timbrar, Cancelra)</param>
        /// <param name="usuario_web_servie">Usuario de acceso al servicio web de timbrado<</param>
        /// <param name="contrasena_web_service">Clave de acceso al servicio web de timbrado</param>
        /// <param name="ubicacion_web_service">Ubicación del Web Service</param>
        /// <param name="secuencia_prioridad">Prioridad para utilización del Web Service</param>
        /// <param name="id_usuario">Usuario </param>
        /// <returns></returns>
        public RetornoOperacion EditaPacCompaniaEmisor(int id_compania_pac, int id_compania_emisor, string version, Tipo tipo, string usuario_web_servie, string contrasena_web_service,
                                                string ubicacion_web_service, byte secuencia_prioridad, int id_usuario)
        {
            return editaPacCompaniaEmisor(id_compania_pac, id_compania_emisor, version, tipo, usuario_web_servie, contrasena_web_service,
                                ubicacion_web_service, secuencia_prioridad, id_usuario, this._hablitar);
        }
        /// <summary>
        /// Deshabilta un PAC Compañia Emisor
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPacCompaniaEmisor(int id_usuario)
        {
            return this.editaPacCompaniaEmisor(this._id_compania_pac, this._id_compania_emisor, this._version, (Tipo)this._id_tipo, this._ubicacion_web_service,
                                               this._contrasena_web_service, this._ubicacion_web_service, this._secuencia_prioridad, id_usuario, false);

        }
        /// <summary>
        /// Carga los pacs ligadon un Id Compania Emisor
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="tipo">Tipo Solicitado</param>
        /// <returns></returns>
        public static DataTable CargaPACSCompaniaEmisor(string version, int id_compania_emisor, Tipo tipo)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, 0, id_compania_emisor, version, tipo, "", "", "", 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    mit = DS.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion

        #region CFDI 3.2

        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Timbrado de Facturemos ya
        /// </summary>
        /// <param name="codigo">Codigo de Error</param>
        /// <param name="descripcion">descripion del error</param>
        /// <returns></returns>
        private static RetornoOperacion obtieneResultadoTimbrado(string codigo, string descripcion)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            switch (codigo)
            {
                //Éxito
                case "201":
                    {
                        resultado = new RetornoOperacion("CFDI Timbrado con éxito.", true);
                        break;
                    }
                //Error
                case "501":
                    {
                        //Certificado Revocado
                        if (descripcion.Contains("Error en timbrado PAC. 308 - 308"))
                            resultado = new RetornoOperacion(@"Certificado no expedido por el SAT");
                        
                        //Fechas de Expedición
                        else if (descripcion.Contains("Error en timbrado PAC. 305 - 305"))
                            resultado = new RetornoOperacion(@"La fecha de emisión no está dentro de la vigencia del CSD");

                        //Fechas de Expedición
                        else if (descripcion.Contains("Error en timbrado PAC. 402 - 402"))
                            resultado = new RetornoOperacion(@"RFC del emisor no se encuentra en el régimen de contribuyentes");
                        
                        //Error no Catalogado
                        else
                            resultado = new RetornoOperacion(descripcion);

                        break;
                    }
                default:
                    {
                        //byte[] bytes = Encoding.GetEncoding(1252).GetBytes(descripcion);
                        //string nameFixed = Encoding.UTF8.GetString(bytes);
                        resultado = new RetornoOperacion(descripcion);
                        break;
                    }
            }

            //Devolviendo resultado
            return resultado;
        }

        #region FacturemosYa

        /// <summary>
        /// Metodo de Crear el Mensaje Soap para Facturemos ya
        /// </summary>
        /// <param name="documento">xml del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        private static XDocument CreateSoapEnvelopeFacturemosYa(string documento, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
                                    xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Header/><soap:Body><RecibirXML soap:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/' >
                                    <usuario xsi:type=""xsd:string""></usuario>
                                    <contra xsi:type=""xsd:string""></contra>
                                    <documento></documento>
                                    </RecibirXML>
                                    </soap:Body>
                                    </soap:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</contra>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</documento"), "<![CDATA[" + documento + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Cancelación del PAC
        /// </summary>
        /// <param name="codigo">Codigo de Error</param>
        /// <param name="descripcion">descripion del error</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoCancelacionFACTUREMOSYA(string codigo, string descripcion)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Si el resultado de timbrado no es correcto
            if (codigo != "201")
                resultado = new RetornoOperacion(descripcion);
            else
                resultado = new RetornoOperacion("El folio UUID del CFDI ha sido exitosamente cancelado.", true);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera la Cancelación de un CFDI del PAC Facturemos Ya
        /// </summary>
        /// <param name="UUID">UUID</param>
        /// <returns></returns>
        private RetornoOperacion cancelaTimbreFACTUREMOSYA(string UUID)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeCancelacionFacturemosYa(UUID, this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoCancelacionFACTUREMOSYA(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("codigo").FirstOrDefault().Value,
                                                                   xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);
                }
            }
            else
                //Establecmos Mensaje Resultado
                resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para  Cancelación de Facturemos ya
        /// </summary>
        /// <param name="UUID">UUID del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeCancelacionFacturemosYa(string UUID, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
                                    xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Header/><soap:Body><CancelarCFDI soap:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/' >
                                    <usuario xsi:type=""xsd:string""></usuario>
                                    <contra xsi:type=""xsd:string""></contra>
                                    <uuid></uuid>
                                    </CancelarCFDI>
                                    </soap:Body>
                                    </soap:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</contra>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</uuid"), "<![CDATA[" + UUID + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera el Timbrado de un CFDI del PAC FCATUREMOS YA
        /// </summary>
        /// <param name="documento">XdDocument del comprabante</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion generaTimbreFACTUREMOSYA(XDocument documento, out XDocument xml_comprobante_recuperado)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos bytes
            xml_comprobante_recuperado = null;

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeFacturemosYa(documento.ToString(), this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoTimbrado(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("codigo").FirstOrDefault().Value,
                                                                    xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperando el comprobante timbrando
                        xml_comprobante_recuperado = XDocument.Parse(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);
                    }
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Facturador

        /// <summary>
        /// Metodo de Crear el Mensaje Soap para Facturemos ya
        /// </summary>
        /// <param name="documento">xml del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        private static XDocument CreateSoapEnvelopeFacturador(string documento, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:tim='http://facturadorelectronico.com/timbrado/' 
                                    xmlns:soap='http://www.w3.org/2003/05/soap-envelope'><soap:Header/>
                                    <soap:Body>
                                    <tim:obtenerTimbrado>
                                    <tim:CFDIcliente></tim:CFDIcliente>                                    
                                    <tim:Usuario></tim:Usuario>
                                    <tim:password></tim:password>
                                    </tim:obtenerTimbrado>
                                    </soap:Body>
                                    </soap:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</tim:Usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</tim:password>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</tim:CFDIcliente"), @"<![CDATA[<?xml version=""1.0"" encoding=""utf-8""?>" + documento + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Cancelación del PAC
        /// </summary>
        /// <param name="codigo">Codigo de Error</param>
        /// <param name="descripcion">descripion del error</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoCancelacionFACTURADOR(string codigo, string descripcion)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion(descripcion);

            //Dea acuerdo al Codigo de Cancelación
            switch (codigo)
            {
                case "201":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI ha sido exitosamente cancelado.", true);
                    break;
                case "202":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI ha sido previamente cancelado.", false);
                    break;
                case "203":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI no fue encontrado", false);
                    break;
                case "204":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID no es aplicable para ser cancelado", false);
                    break;
                case "205":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI aun no ha sido enviado al SAT", false);
                    break;
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el la Cancelación de un CFDI del PAC FACTURADOR
        /// </summary>
        /// <param name="rfcEmisor">RFC Emisor</param>
        /// <param name="certificado">Certificado en Base 64</param>
        /// <param name="xmlLlavePrivadaCertificado">xml de la llave Privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="xml_acuse">Acuse Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion cancelaTimbreFACTURADOR(string rfcEmisor, string certificado, string xmlLlavePrivadaCertificado, string UUID, out XDocument xml_acuse)
        {
            //Declaramos xml para el acuse
            xml_acuse = null;
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeCancelacionFACTURADOR(rfcEmisor, certificado, xmlLlavePrivadaCertificado, UUID, this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoCancelacionFACTURADOR(Convert.ToBoolean(xDoc.Descendants("solicitud").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("solicitud").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("codigo").Value : xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Attribute("Estatus").Value,
                        Convert.ToBoolean(xDoc.Descendants("solicitud").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("solicitud").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("mensaje").Value : xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Attribute("Estatus").Value);


                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperación de nodo Acuse Cancelacion
                        object acuse = xDoc.Root.Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Element("Acuse");

                        //Obtenemos Documento
                        xml_acuse = XDocument.Parse(acuse.ToString());
                    }
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para  Cancelación de Facturador
        /// </summary>
        /// <param name="rfcEmisor">RFC del Emisor</param>
        /// <param name="certificado">Certificado en Base 64</param>
        /// <param name="xmlLlavePrivadaCertificado">xml de la llave privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeCancelacionFACTURADOR(string rfcEmisor, string certificado, string xmlLlavePrivadaCertificado, string UUID, string usuario, string contrasena)
        {
            //Convertimos XML de la LLave Privada a Base 64
            string llaveCertificadoBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlLlavePrivadaCertificado));

            //Declaramos Variable para Armar Soap
            string xmlCancelacion = @"<Cancelacion rfcEmisor =""" + rfcEmisor + @""" certificado=""" + certificado + @""" llaveCertificado=""" + llaveCertificadoBase64 + @""">                                    
                                    <Folios>
                                    <Folio UUID =""" + UUID + @""">
                                    </Folio>
                                    </Folios>
                                    </Cancelacion>";

            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:tim='http://facturadorelectronico.com/timbrado/' 
                                    xmlns:soap='http://www.w3.org/2003/05/soap-envelope'><soap:Header/>
                                    <soap:Body>
                                    <tim:cancelarComprobante>
                                    <tim:xmlCancelacion> </tim:xmlCancelacion>                                    
                                    <tim:usuario></tim:usuario>
                                    <tim:password></tim:password>
                                    </tim:cancelarComprobante>
                                    </soap:Body>
                                    </soap:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</tim:password>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</tim:xmlCancelacion"), @"<![CDATA[<?xml version=""1.0"" encoding=""utf-8""?>" + xmlCancelacion.ToString() + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera el Timbrado de un CFDI del PAC FACTURADOR
        /// </summary>
        /// <param name="documento">XdDocument del comprabante</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion generaTimbreFACTURADOR(XDocument documento, out XDocument xml_comprobante_recuperado)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos bytes
            xml_comprobante_recuperado = null;

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeFacturador(documento.ToString(), this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Obteniendo Errores
                    XElement ERROR = xDoc.Descendants("Error").FirstOrDefault();

                    //Validando existencia de Error
                    if (ERROR == null)
                    
                        //Traduciendo resultado
                        resultado = obtieneResultadoTimbrado(Convert.ToBoolean(xDoc.Descendants("timbre").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("timbre").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("codigo").Value : "201",
                            Convert.ToBoolean(xDoc.Descendants("timbre").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("timbre").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("descripcion").Value : "");
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion(string.Format("{0} - {1}", ERROR.Attribute("codigo").Value, ERROR.Attribute("mensaje").Value));
                    
                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Declaración de namespaces a utilizar en el Comprobante
                        //SAT
                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT", 0);

                        //Recuperación de nodo Timbre Fiscal Digital
                        object timbre = xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault();

                        XElement xmlDocument = XElement.Parse(documento.ToString());
                        //Si se añadió correctamente el Complemento
                        if (documento.Root.Element(ns + "Complemento") == null)
                        {
                            //Creamos XML Element
                            XElement xElementComplemento = new XElement(ns + "Complemento");

                            //Añadimos Timbrado al Complemento del Elemento
                            xElementComplemento.Add(timbre);

                            //Añadimos Complemento al Comprobante
                            xmlDocument.Add(xElementComplemento);
                        }
                        else
                            //Añadimos Timbre al Complemento
                            xmlDocument.Element(ns + "Complemento").Add(timbre);

                        //Obtenemos Documento
                        xml_comprobante_recuperado = XDocument.Parse(xmlDocument.ToString());
                    }
                }
                else
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        /// <summary>
        /// Genera el Timbrado de un CFDI de acuerdo a los PACS asignados a la Compañia Emisor
        /// </summary>
        /// <param name="documento">Xml de comprobante a Timbrar</param>
        /// <param name="bytes_comprobante">Bytes XML del Comprobante a Timbrar</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <param name="id_compania_pac">PAC que realizó el Timbrado</param>
        /// <param name="id_compania_emisor">Compañia Emisor</param>
        /// <returns></returns>
        public static RetornoOperacion GeneraTimbrePAC(XDocument documento, byte[] bytes_comprobante, out XDocument xml_comprobante_recuperado, out int id_compania_pac,
                                                      int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se encontró Proveedor de Timbrado.");

            //Declaramos Variables de Salida
            xml_comprobante_recuperado = null;
            id_compania_pac = 0;
            //Cargamos Pac ligando la Compañia Emisor
            using (DataTable mitPac = CargaPACSCompaniaEmisor("3.2", id_compania_emisor, Tipo.Timbrar))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitPac))
                {
                    //Recorremos Cada uno de los PACS
                    foreach (DataRow r in mitPac.Rows)
                    {
                        //Determinando el PAC que genera el Timbrado
                        switch ((PAC)r.Field<int>("IdCompaniaPac"))
                        {
                            //Facturemos Ya
                            case PAC.FACTUREMOSYA:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTUREMOSYA(documento, out xml_comprobante_recuperado);
                                }
                                break;
                            //Facturador
                            case PAC.FACTURADOR:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTURADOR(documento, out xml_comprobante_recuperado);
                                }
                                break;
                            //Facturador
                            case PAC.FACTURADORSAE:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTURADOR(documento, out xml_comprobante_recuperado);
                                }
                                break;

                            //En caso de no existir el PAC
                            default:

                                break;
                        }
                        //Si el Timbrado fue correcto
                        if (resultado.OperacionExitosa)
                        {
                            //ASignamos la Compañia que realizó el Timbrado
                            id_compania_pac = r.Field<int>("IdCompaniaPac");
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Cancelar un UUID
        /// </summary>
        /// <param name="rfc_Emisor">RFC del Emisor</param>
        /// <param name="ceritificado">Certificado en Base 64</param>
        /// <param name="xml_llave_privada">xml de la Llave Privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="xml_acuse">Acuse</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <returns></returns>
        public static RetornoOperacion CancelaTimbrePAC(string rfc_Emisor, string ceritificado, string xml_llave_privada,
                                                        string UUID, out XDocument xml_acuse, int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se encontró Proveedor de Timbrado.");

            //Declaramos Variable xml Acuse
            xml_acuse = null;
            //Cargamos Pac ligando la Compañia Emisor
            using (DataTable mitPac = CargaPACSCompaniaEmisor("3.2",id_compania_emisor, Tipo.Cancelar))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitPac))
                {
                    //Recorremos Cada uno de los PACS
                    foreach (DataRow r in mitPac.Rows)
                    {
                        //Determinando el PAC que genera la cancelación
                        switch ((PAC)r.Field<int>("IdCompaniaPac"))
                        {
                            //TECTOS
                            case PAC.FACTUREMOSYA:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTUREMOSYA(UUID);
                                }
                                break;
                            //TECTOS
                            case PAC.FACTURADOR:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTURADOR(rfc_Emisor, ceritificado, xml_llave_privada, UUID, out xml_acuse);
                                }
                                break;
                            //TECTOS
                            case PAC.FACTURADORSAE:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTURADOR(rfc_Emisor, ceritificado, xml_llave_privada, UUID, out xml_acuse);
                                }
                                break;

                            //En caso de no existir PAC
                            default:

                                break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region CFDI 3.3

        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Timbrado de Facturemos ya
        /// </summary>
        /// <param name="esValido"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static RetornoOperacion obtieneResultadoTimbrado3_3(bool esValido, XElement error)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando Resultado
            if (esValido)

                //Instanciando Resultado Obtenido
                resultado = new RetornoOperacion("CFDI Timbrado con éxito.", true);
            else
            {
                //Validando Existencia de los Atributos
                if (error != null && error.Attribute("codigo") != null && error.Attribute("codigo") != null)
                {
                    //Validando Código del Error
                    switch (error.Attribute("codigo").Value)
                    {
                        case "CFDI33101":
                        case "CFDI33102":
                        case "CFDI33103":
                        case "CFDI33104":
                        case "CFDI33105":
                        case "CFDI33106":
                        case "CFDI33107":
                        case "CFDI33108":
                        case "CFDI33109":
                        case "CFDI33110":
                        case "CFDI33111":
                        case "CFDI33112":
                        case "CFDI33113":
                        case "CFDI33114":
                        case "CFDI33115":
                        case "CFDI33116":
                        case "CFDI33117":
                        case "CFDI33118":
                        case "CFDI33119":
                        case "CFDI33120":
                        case "CFDI33121":
                        case "CFDI33122":
                        case "CFDI33123":
                        case "CFDI33124":
                        case "CFDI33125":
                        case "CFDI33126":
                        case "CFDI33127":
                        case "CFDI33128":
                        case "CFDI33129":
                        case "CFDI33130":
                        case "CFDI33131":
                        case "CFDI33132":
                        case "CFDI33133":
                        case "CFDI33134":
                        case "CFDI33135":
                        case "CFDI33136":
                        case "CFDI33137":
                        case "CFDI33138":
                        case "CFDI33139":
                        case "CFDI33140":
                        case "CFDI33141":
                        case "CFDI33142":
                        case "CFDI33143":
                        case "CFDI33144":
                        case "CFDI33145":
                        case "CFDI33146":
                        case "CFDI33147":
                        case "CFDI33148":
                        case "CFDI33149":
                        case "CFDI33150":
                        case "CFDI33151":
                        case "CFDI33152":
                        case "CFDI33153":
                        case "CFDI33154":
                        case "CFDI33155":
                        case "CFDI33156":
                        case "CFDI33157":
                        case "CFDI33158":
                        case "CFDI33159":
                        case "CFDI33160":
                        case "CFDI33161":
                        case "CFDI33162":
                        case "CFDI33163":
                        case "CFDI33164":
                        case "CFDI33165":
                        case "CFDI33166":
                        case "CFDI33167":
                        case "CFDI33168":
                        case "CFDI33169":
                        case "CFDI33170":
                        case "CFDI33171":
                        case "CFDI33172":
                        case "CFDI33173":
                        case "CFDI33174":
                        case "CFDI33175":
                        case "CFDI33176":
                        case "CFDI33177":
                        case "CFDI33178":
                        case "CFDI33179":
                        case "CFDI33180":
                        case "CFDI33181":
                        case "CFDI33182":
                        case "CFDI33183":
                        case "CFDI33184":
                        case "CFDI33185":
                        case "CFDI33186":
                        case "CFDI33187":
                        case "CFDI33188":
                        case "CFDI33189":
                        case "CFDI33190":
                        case "CFDI33191":
                        case "CFDI33192":
                        case "CFDI33193":
                        case "CFDI33194":
                        case "CFDI33195":
                        case "CFDI33196":
                            //Instanciando Resultado Obtenido
                            resultado = new RetornoOperacion(error.Attribute("mensaje").Value);
                            break;
                        default:
                            //Instanciando Resultado Obtenido
                            resultado = new RetornoOperacion("CFDI Timbrado con éxito.", true);
                            break;
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Error Desconocido!");
            }
            
            //Si el resultado de timbrado no es correcto
            //if (codigo != "201")
            //    resultado = new RetornoOperacion(descripcion);
            //else
            //    resultado = new RetornoOperacion("CFDI Timbrado con éxito.", true);

            //Devolviendo resultado
            return resultado;
        }

        #region Facturemos Ya!

        /// <summary>
        /// Metodo de Crear el Mensaje Soap para Facturemos ya
        /// </summary>
        /// <param name="documento">xml del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        private static XDocument CreateSoapEnvelopeFacturemosYa3_3(string documento, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
                                            xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <RecibirXML soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                                                 <usuario xsi:type='xsd:string'></usuario>
                                                 <contra xsi:type='xsd:string'></contra>
                                                 <documento xsi:type='xsd:string'></documento>
                                              </RecibirXML>
                                           </soapenv:Body>
                                        </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</contra>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</documento"), "<![CDATA[" + documento + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Cancelación del PAC
        /// </summary>
        /// <param name="codigo">Codigo de Error</param>
        /// <param name="descripcion">descripion del error</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoCancelacionFACTUREMOSYA3_3(string codigo, string descripcion)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Si el resultado de timbrado no es correcto
            if (codigo != "201")
                resultado = new RetornoOperacion(descripcion);
            else
                resultado = new RetornoOperacion("El folio UUID del CFDI ha sido exitosamente cancelado.", true);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera la Cancelación de un CFDI del PAC Facturemos Ya
        /// </summary>
        /// <param name="UUID">UUID</param>
        /// <returns></returns>
        private RetornoOperacion cancelaTimbreFACTUREMOSYA3_3(string UUID)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeCancelacionFacturemosYa3_3(UUID, this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoCancelacionFACTUREMOSYA3_3(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("codigo").FirstOrDefault().Value,
                                                                   xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);
                }
            }
            else
                //Establecmos Mensaje Resultado
                resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para  Cancelación de Facturemos ya
        /// </summary>
        /// <param name="UUID">UUID del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeCancelacionFacturemosYa3_3(string UUID, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' 
                                        xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <CancelarCFDI soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                                             <usuario xsi:type='xsd:string'></usuario>
                                             <contra xsi:type='xsd:string'></contra>
                                             <uuid xsi:type='xsd:string'></uuid>
                                          </CancelarCFDI>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</contra>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</uuid"), "<![CDATA[" + UUID + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera el Timbrado de un CFDI del PAC FCATUREMOS YA
        /// </summary>
        /// <param name="documento">XdDocument del comprabante</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion generaTimbreFACTUREMOSYA3_3(XDocument documento, out XDocument xml_comprobante_recuperado)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos bytes
            xml_comprobante_recuperado = null;

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeFacturemosYa3_3(documento.ToString(), this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoTimbrado(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("codigo").FirstOrDefault().Value,
                                                                    xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperando el comprobante timbrando
                        xml_comprobante_recuperado = XDocument.Parse(xDoc.Descendants("respuesta").FirstOrDefault().Descendants("descripcion").FirstOrDefault().Value);
                    }
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Facturador

        /// <summary>
        /// Metodo de Crear el Mensaje Soap para Facturemos ya
        /// </summary>
        /// <param name="documento">xml del cfdi</param>
        /// <param name="usuario">Usuario para acceso al web service</param>
        /// <param name="contrasena">contraseña para el web service</param>
        /// <returns></returns>
        private static XDocument CreateSoapEnvelopeFacturador3_3(string documento, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                     <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tim=""http://facturadorelectronico.com/timbrado/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <tim:obtenerTimbrado>
                                             <tim:CFDIcliente></tim:CFDIcliente>
                                             <tim:Usuario></tim:Usuario>
                                             <tim:password></tim:password>
                                          </tim:obtenerTimbrado>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</tim:Usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</tim:password>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</tim:CFDIcliente"), @"<![CDATA[<?xml version=""1.0"" encoding=""utf-8""?>" + documento + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de Cancelación del PAC
        /// </summary>
        /// <param name="codigo">Codigo de Error</param>
        /// <param name="descripcion">descripion del error</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoCancelacionFACTURADOR3_3(string codigo, string descripcion)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion(descripcion);

            //Dea acuerdo al Codigo de Cancelación
            switch (codigo)
            {
                case "201":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI ha sido exitosamente cancelado.", true);
                    break;
                case "202":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI ha sido previamente cancelado.", true);
                    break;
                case "203":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI no fue encontrado", false);
                    break;
                case "204":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID no es aplicable para ser cancelado", false);
                    break;
                case "205":
                    //Establecemos Mensaje
                    resultado = new RetornoOperacion(Convert.ToInt32(codigo), "El folio UUID del CFDI aun no ha sido enviado al SAT", false);
                    break;
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Genera el la Cancelación de un CFDI del PAC FACTURADOR
        /// </summary>
        /// <param name="rfcEmisor">RFC Emisor</param>
        /// <param name="certificado">Certificado en Base 64</param>
        /// <param name="xmlLlavePrivadaCertificado">xml de la llave Privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="xml_acuse">Acuse Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion cancelaTimbreFACTURADOR3_3(string rfcEmisor, string certificado, string xmlLlavePrivadaCertificado, string UUID, out XDocument xml_acuse)
        {
            //Declaramos xml para el acuse
            xml_acuse = null;
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeCancelacionFACTURADOR3_3(rfcEmisor, certificado, xmlLlavePrivadaCertificado, UUID, this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = obtieneResultadoCancelacionFACTURADOR3_3(Convert.ToBoolean(xDoc.Descendants("solicitud").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("solicitud").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("codigo").Value : xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Attribute("Estatus").Value,
                        Convert.ToBoolean(xDoc.Descendants("solicitud").FirstOrDefault().Attribute("esValido").Value) == false ? xDoc.Descendants("solicitud").FirstOrDefault().Descendants("errores").FirstOrDefault().Descendants("Error").FirstOrDefault().Attribute("descripcion").Value : xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Attribute("Estatus").Value);


                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperación de nodo Acuse Cancelacion
                        //object acuse = xDoc.Root.Descendants().Descendants().Descendants().Descendants().FirstOrDefault().Element("Acuse"); //Sustituye Linea 
                        object acuse = xDoc.Descendants("solicitud").FirstOrDefault();

                        //Obtenemos Documento
                        xml_acuse = XDocument.Parse(acuse.ToString());
                    }
                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para  Cancelación de Facturador
        /// </summary>
        /// <param name="rfcEmisor">RFC del Emisor</param>
        /// <param name="certificado">Certificado en Base 64</param>
        /// <param name="xmlLlavePrivadaCertificado">xml de la llave privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeCancelacionFACTURADOR3_3(string rfcEmisor, string certificado, string xmlLlavePrivadaCertificado, string UUID, string usuario, string contrasena)
        {
            //Convertimos XML de la LLave Privada a Base 64
            string llaveCertificadoBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlLlavePrivadaCertificado));

            //Declaramos Variable para Armar Soap
            string xmlCancelacion = @"<Cancelacion rfcEmisor =""" + rfcEmisor + @""" certificado=""" + certificado + @""" llaveCertificado=""" + llaveCertificadoBase64 + @""">                                    
                                    <Folios>
                                    <Folio UUID =""" + UUID + @""">
                                    </Folio>
                                    </Folios>
                                    </Cancelacion>";

            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tim=""http://facturadorelectronico.com/timbrado/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <tim:cancelarComprobante>
                                             <tim:xmlCancelacion></tim:xmlCancelacion>
                                             <tim:usuario></tim:usuario>
                                             <tim:password></tim:password>
                                          </tim:cancelarComprobante>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</tim:password>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</tim:xmlCancelacion"), @"<![CDATA[<?xml version=""1.0"" encoding=""utf-8""?>" + xmlCancelacion.ToString() + "]]>");

            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera el Timbrado de un CFDI del PAC FACTURADOR
        /// </summary>
        /// <param name="documento">XdDocument del comprabante</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <returns></returns>
        private RetornoOperacion generaTimbreFACTURADOR3_3(XDocument documento, out XDocument xml_comprobante_recuperado)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos bytes
            xml_comprobante_recuperado = null;

            //Creamos Solicitud
            resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(this._ubicacion_web_service), CreateSoapEnvelopeFacturador3_3(documento.ToString(), this._usuario_web_servie,
                                            this._contrasena_web_service));

            //Validamos Solicitud exitosa
            if (resultado.OperacionExitosa)
            {
                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado.Mensaje);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Obteniendo Errores
                    XElement ERROR = xDoc.Descendants("Error").FirstOrDefault();
                    XElement validaTimbre = xDoc.Descendants("timbre").FirstOrDefault();
                    bool esValido = validaTimbre != null ? Convert.ToBoolean(validaTimbre.Attribute("esValido").Value) : false;

                    //Validando existencia de Error
                    if (esValido && ERROR == null)
                    
                        //Traduciendo resultado
                        resultado = obtieneResultadoTimbrado3_3(esValido, ERROR);
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion(string.Format("{0} - {1}", ERROR.Attribute("codigo").Value, ERROR.Attribute("mensaje").Value));

                    //Si no existe error
                    if (resultado.OperacionExitosa)
                    {
                        //Declaración de namespaces a utilizar en el Comprobante
                        //SAT
                        XNamespace ns = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace SAT", 0);

                        //Recuperación de nodo Timbre Fiscal Digital
                        object timbre = xDoc.Root.Descendants().Descendants().Descendants().Descendants().Descendants().FirstOrDefault();

                        XElement xmlDocument = XElement.Parse(documento.ToString());
                        //Si se añadió correctamente el Complemento
                        if (documento.Root.Element(ns + "Complemento") == null)
                        {
                            //Creamos XML Element
                            XElement xElementComplemento = new XElement(ns + "Complemento");

                            //Añadimos Timbrado al Complemento del Elemento
                            xElementComplemento.Add(timbre);

                            //Añadimos Complemento al Comprobante
                            xmlDocument.Add(xElementComplemento);
                        }
                        else
                            //Añadimos Timbre al Complemento
                            xmlDocument.Element(ns + "Complemento").Add(timbre);

                        //Obtenemos Documento
                        xml_comprobante_recuperado = XDocument.Parse(xmlDocument.ToString());
                    }
                }
                else
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de Timbrado");
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        /// <summary>
        /// Genera el Timbrado de un CFDI de acuerdo a los PACS asignados a la Compañia Emisor
        /// </summary>
        /// <param name="documento">Xml de comprobante a Timbrar</param>
        /// <param name="bytes_comprobante">Bytes XML del Comprobante a Timbrar</param>
        /// <param name="xml_comprobante_recuperado">Xml del Comprobante Recuperado</param>
        /// <param name="id_compania_pac">PAC que realizó el Timbrado</param>
        /// <param name="id_compania_emisor">Compañia Emisor</param>
        /// <returns></returns>
        public static RetornoOperacion GeneraTimbrePAC3_3(XDocument documento, byte[] bytes_comprobante, out XDocument xml_comprobante_recuperado, out int id_compania_pac,
                                                      int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se encontró Proveedor de Timbrado.");

            //Declaramos Variables de Salida
            xml_comprobante_recuperado = null;
            id_compania_pac = 0;
            //Cargamos Pac ligando la Compañia Emisor
            using (DataTable mitPac = CargaPACSCompaniaEmisor("3.3", id_compania_emisor, Tipo.Timbrar))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitPac))
                {
                    //Recorremos Cada uno de los PACS
                    foreach (DataRow r in mitPac.Rows)
                    {
                        //Determinando el PAC que genera el Timbrado
                        switch ((PAC)r.Field<int>("IdCompaniaPac"))
                        {
                            //Facturemos Ya
                            case PAC.FACTUREMOSYA:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTUREMOSYA3_3(documento, out xml_comprobante_recuperado);
                                }
                                break;
                            //Facturador
                            case PAC.FACTURADOR:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTURADOR3_3(documento, out xml_comprobante_recuperado);
                                }
                                break;
                            //Facturador
                            case PAC.FACTURADORSAE:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.generaTimbreFACTURADOR3_3(documento, out xml_comprobante_recuperado);
                                }
                                break;

                            //En caso de no existir el PAC
                            default:
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("No existe al PAC");
                                break;
                        }
                        //Si el Timbrado fue correcto
                        if (resultado.OperacionExitosa)
                        {
                            //ASignamos la Compañia que realizó el Timbrado
                            id_compania_pac = r.Field<int>("IdCompaniaPac");
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Cancelar un UUID
        /// </summary>
        /// <param name="rfc_Emisor">RFC del Emisor</param>
        /// <param name="ceritificado">Certificado en Base 64</param>
        /// <param name="xml_llave_privada">xml de la Llave Privada</param>
        /// <param name="UUID">UUID</param>
        /// <param name="xml_acuse">Acuse</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <returns></returns>
        public static RetornoOperacion CancelaTimbrePAC3_3(string rfc_Emisor, string ceritificado, string xml_llave_privada,
                                                        string UUID, out XDocument xml_acuse, int id_compania_emisor)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se encontró Proveedor de Timbrado.");

            //Declaramos Variable xml Acuse
            xml_acuse = null;
            //Cargamos Pac ligando la Compañia Emisor
            using (DataTable mitPac = CargaPACSCompaniaEmisor("3.3", id_compania_emisor, Tipo.Cancelar))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitPac))
                {
                    //Recorremos Cada uno de los PACS
                    foreach (DataRow r in mitPac.Rows)
                    {
                        //Determinando el PAC que genera la cancelación
                        switch ((PAC)r.Field<int>("IdCompaniaPac"))
                        {
                            //TECTOS
                            case PAC.FACTUREMOSYA:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTUREMOSYA3_3(UUID);
                                }
                                break;
                            //TECTOS
                            case PAC.FACTURADOR:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTURADOR3_3(rfc_Emisor, ceritificado, xml_llave_privada, UUID, out xml_acuse);
                                }
                                break;
                            //TECTOS
                            case PAC.FACTURADORSAE:
                                //Instanciamos Pac
                                using (PacCompaniaEmisor pac = new PacCompaniaEmisor(r.Field<int>("Id")))
                                {
                                    //Timbrando Comprobente
                                    resultado = pac.cancelaTimbreFACTURADOR3_3(rfc_Emisor, ceritificado, xml_llave_privada, UUID, out xml_acuse);
                                }
                                break;

                            //En caso de no existir PAC
                            default:

                                break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
