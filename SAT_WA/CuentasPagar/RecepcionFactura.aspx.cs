using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Seguridad;
using SAT_CL;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Collections.Generic;
using System.Web.Services;

namespace SAT.CuentasPagar
{
    public partial class RecepcionFactura : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        ///Evento encargado de  realizar la carga de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no hay recarga de la misma página
            if (!Page.IsPostBack)
            {   //Inicializando estatus general de la forma
                inicializaPagina();
            }
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {   //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());
            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();
            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))
                //Cargando Documento XML
                doc.Load(ms);
            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;
        }
        /// <summary>
        /// Evento disparado al dar click en el botón "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();
            string archivo = "";
            //int compania = 0;
            List<int> companias = new List<int>();
            //Validando el Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Instanciando Excepcion
                        //Validando que existe el Archivo
                        if (Session["id_registro_b"] != null && Session["id_registro_c"] != null)
                        {
                            //Para cada uno de los archivos cargados
                            foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                            {
                                //Traduciendo a texto XML
                                XDocument doc = XDocument.Load(new MemoryStream(b));
                                //si se cargó correctamente
                                if (doc != null)
                                {
                                    //Convirtiendo Documento
                                    XmlDocument xmlDocument = new XmlDocument();
                                    using (var xmlReader = doc.CreateReader())
                                    {
                                        //Cargando XML Document
                                        xmlDocument.Load(xmlReader);
                                    }
                                    //Validando versión
                                    switch (doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value)
                                    {
                                        case "3.2":
                                            {
                                                //Instanciando Compania Emisora (Proveedor)
                                                using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                {
                                                    companias.Add(emi.id_compania_emisor_receptor);
                                                }
                                            }
                                            break;
                                        case "3.3":
                                            {
                                                //Instanciando Compania Emisora (Proveedor)
                                                using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                {
                                                    companias.Add(emi.id_compania_emisor_receptor);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            var compania = (from m in companias
                                            group m by m).ToList();
                            if (compania.Count == 1)
                            {
                                //Para cada uno de los archivos cargados
                                foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                                {
                                    //Obteniendo Documento
                                    //XmlDocument doc = (XmlDocument)Session["XML"];

                                    try
                                    {
                                        //Traduciendo a texto XML
                                        XDocument doc = XDocument.Load(new MemoryStream(b));
                                        //si se cargó correctamente
                                        if (doc != null)
                                        {
                                            //Convirtiendo Documento
                                            XmlDocument xmlDocument = new XmlDocument();
                                            using (var xmlReader = doc.CreateReader())
                                            {
                                                //Cargando XML Document
                                                xmlDocument.Load(xmlReader);
                                            }
                                            //Convirtiendo a XDocument
                                            //XDocument cfdi = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);

                                            //Validando versión
                                            switch (doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value)
                                            {
                                                case "3.2":
                                                    {
                                                        //string serie = doc.Root.Attribute("serie").Value;
                                                        string folio = "";
                                                        if (doc.Root.Attribute("folio") != null)
                                                        {
                                                            folio = doc.Root.Attribute("folio").Value;
                                                        }
                                                        XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                                                    XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                                                       where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                                                       select el).DefaultIfEmpty(null).FirstOrDefault();
                                                                    string uuid = timbre.Attribute("UUID").Value;
                                                        foreach (string a in (List<string>)Session["id_registro_c"])
                                                        {
                                                            if (a.Contains(folio) || a.Contains(uuid))
                                                                archivo = a.ToString();
                                                        }
                                                        //Instanciando Compania Emisora (Proveedor)
                                                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                        {
                                                            //Validando que Exista el Proveedor
                                                            if (emi.id_compania_emisor_receptor > 0)
                                                            {
                                                                //Invocando Método de Guardado de XML
                                                                //validaEstatusPublicacionSAT(btnAceptar);
                                                                guardarRecepcionFactura(xmlDocument, archivo);
                                                            }
                                                            else
                                                            {
                                                                //Mostrando Proveedor por Ingresar
                                                                lblProveedorFactura.Text = xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper();

                                                                //Mostrando ventana Modal
                                                                TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptar, upbtnAceptar.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                                            }

                                                        }
                                                        break;
                                                    }
                                                case "3.3":
                                                    {
                                                        //string serie = doc.Root.Attribute("Serie").Value;
                                                        string folio = "";
                                                        if (doc.Root.Attribute("Folio") != null)
                                                        {
                                                            folio = doc.Root.Attribute("Folio").Value;
                                                        }
                                                        XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                                        XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                                           where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                                           select el).DefaultIfEmpty(null).FirstOrDefault();
                                                        string uuid = timbre.Attribute("UUID").Value;
                                                        foreach (string a in (List<string>)Session["id_registro_c"])
                                                        {
                                                            if (a.Contains(folio) || a.Contains(uuid))
                                                                archivo = a.ToString();
                                                        }
                                                        //Instanciando Compania Emisora (Proveedor)
                                                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                        {
                                                            //Validando que Exista el Proveedor
                                                            if (emi.id_compania_emisor_receptor > 0)
                                                                //Invocando Método de Guardado de XML
                                                                guardarRecepcionFactura(xmlDocument, archivo);//validaEstatusPublicacionSAT(btnAceptar);
                                                            else
                                                            {
                                                                //Mostrando Proveedor por Ingresar
                                                                lblProveedorFactura.Text = xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Nombre"].Value.ToUpper();

                                                                //Mostrando ventana Modal
                                                                TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptar, upbtnAceptar.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                                            }
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion(ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                //Estableciendo estatus a nuevo registro
                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                //Inicialziando la forma
                                inicializaPagina();
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No todos los CFDI provienen del mismo Proveedor.");
                            }
                        }
                        else
                            resultado = new RetornoOperacion("No existe la Factura");
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Obteniendo Fecha
                        DateTime fec_recepcion;
                        DateTime.TryParse(txtFechaRecepcion.Text, out fec_recepcion);

                        //Instanciamos la recepcion que se desea modificar 
                        using (Recepcion recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Editando el encabezado
                            resultado = recepcion.EditaRecepcion(recepcion.id_compania_proveedor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                        recepcion.secuencia, fec_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        break;
                    }
            }

            //Operación Exitosa?
            if (resultado.OperacionExitosa)
            {
                //Estableciendo estatus a nuevo registro
                Session["estatus"] = Pagina.Estatus.Edicion;
                //Inicializando Id de registro activo 
                Session["id_registro"] = resultado.IdRegistro;
                Session["id_registro_b"] = null;
                Session["id_registro_c"] = null;
                //Inicialziando la forma
                inicializaPagina();
            }
            //}            
            //Mostrando Notificación
            //TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOperacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de validación en SAT
            //validaEstatusPublicacionSAT(btnAceptarOperacion);
            string archivo = "";
            //Validando que existe el Archivo
            if (Session["id_registro_b"] != null && Session["id_registro_c"] != null)//if (Session["XML"] != null)
            {
                //Para cada uno de los archivos cargados
                foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                {
                    //Obteniendo Documento
                    //XmlDocument doc = (XmlDocument)Session["XML"];

                    try
                    {
                        //Traduciendo a texto XML
                        XDocument doc = XDocument.Load(new MemoryStream(b));
                        //si se cargó correctamente
                        if (doc != null)
                        {
                            //Convirtiendo Documento
                            XmlDocument xmlDocument = new XmlDocument();
                            using (var xmlReader = doc.CreateReader())
                            {
                                //Cargando XML Document
                                xmlDocument.Load(xmlReader);
                            }
                            //Convirtiendo a XDocument
                            //XDocument cfdi = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);

                            //Validando versión
                            switch (doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value)
                            {
                                case "3.2":
                                    {
                                        //string serie = doc.Root.Attribute("serie").Value;
                                        string folio = "";
                                        if (doc.Root.Attribute("folio") != null)
                                        {
                                            folio = doc.Root.Attribute("folio").Value;
                                        }
                                        XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                        XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                           where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                           select el).DefaultIfEmpty(null).FirstOrDefault();
                                        string uuid = timbre.Attribute("UUID").Value;
                                        foreach (string a in (List<string>)Session["id_registro_c"])
                                        {
                                            if (a.Contains(folio) || a.Contains(uuid))
                                                archivo = a.ToString();
                                        }
                                        //Invocando Método de Guardado de XML
                                        guardarRecepcionFactura(xmlDocument, archivo);
                                        break;
                                    }
                                case "3.3":
                                    {
                                        //string serie = doc.Root.Attribute("Serie").Value;
                                        string folio = "";
                                        if (doc.Root.Attribute("Folio") != null)
                                        {
                                            folio = doc.Root.Attribute("Folio").Value;
                                        }
                                        XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                        XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                           where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                           select el).DefaultIfEmpty(null).FirstOrDefault();
                                        string uuid = timbre.Attribute("UUID").Value;
                                        foreach (string a in (List<string>)Session["id_registro_c"])
                                        {
                                            if (a.Contains(folio) || a.Contains(uuid))
                                                archivo = a.ToString();
                                        }
                                        //Invocando Método de Guardado de XML
                                        guardarRecepcionFactura(xmlDocument, archivo);
                                        break;
                                    }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Instanciando Excepción
                        lblError.Text = ex.Message;
                    }
                }
            }

            //Ocultando ventana Modal de selección de tipo de servicio del proveedor
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptarOperacion, upbtnAceptarOperacion.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarOperacion_Click(object sender, EventArgs e)
        {
            //Mostrando ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, upbtnCancelarOperacion.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// Evento click en ventana modal de confirmación de validación de factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidacionSAT_Click(object sender, EventArgs e)
        {
            //Determinando respuesta del usuario
            switch (((Button)sender).CommandName)
            {
                case "Descartar":
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnCanelarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
                case "Continuar":
                    //Realizando proceso de guardado de factura de proveedor
                    //guardarRecepcionFactura();

                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnAceptarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
            }
        }
        /// Evento producido al pulsar el botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancelar_Click(object sender, EventArgs e)
        {   //Si el estatus es edición
            if (((Pagina.Estatus)Session["estatus"]) == Pagina.Estatus.Edicion)
                Session["estatus"] = Pagina.Estatus.Lectura;
            //Inicializando forma
            inicializaPagina();
        }
        /// <summary>
        /// Evento producido al dar click sobre algún elemento de menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;
            //De acuerdo al nombre de comando asignado 
            switch (boton.CommandName)
            {
                //Establecemos la pagina en estatus Nuevo
                case "Nuevo":
                    {   //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializando Id de Registro activo
                        Session["id_registro"] = 0;
                        //Eliminando Contenido en Session
                        Session["DS"] =
                        Session["id_registro_b"] =
                        Session["id_registro_c"] = null;
                        //Limpiando nombre de archivo
                        ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
                        //Inicializamos la pagina
                        inicializaPagina();
                        break;
                    }
                //Permite abrir registros de la Recepcion de factura
                case "Abrir":
                    {   //Inicializando Apertura
                        inicializaAperturaRegistro(74, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                //Guarda el registro en la BD
                case "Guardar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();
                        string archivo = "";
                        //int compania = 0;
                        List<int> companias = new List<int>();                   
                            //Validando el Estatus de la Página
                            switch ((Pagina.Estatus)Session["estatus"])
                            {
                                case Pagina.Estatus.Nuevo:
                                    {
                                        //Validando que existe el Archivo
                                        if (Session["id_registro_b"] != null && Session["id_registro_c"] != null)
                                        {
                                            //Para cada uno de los archivos cargados
                                            foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                                            {
                                                //Traduciendo a texto XML
                                                XDocument doc = XDocument.Load(new MemoryStream(b));
                                                //si se cargó correctamente
                                                if (doc != null)
                                                {
                                                    //Convirtiendo Documento
                                                    XmlDocument xmlDocument = new XmlDocument();
                                                    using (var xmlReader = doc.CreateReader())
                                                    {
                                                        //Cargando XML Document
                                                        xmlDocument.Load(xmlReader);
                                                    }
                                                    //Validando versión
                                                    switch (doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value)
                                                    {
                                                        case "3.2":
                                                            {
                                                                //Instanciando Compania Emisora (Proveedor)
                                                                using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                                {
                                                                    companias.Add(emi.id_compania_emisor_receptor);
                                                                }
                                                            }
                                                            break;
                                                        case "3.3":
                                                            {
                                                                //Instanciando Compania Emisora (Proveedor)
                                                                using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                                {
                                                                    companias.Add(emi.id_compania_emisor_receptor);
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            var compania = (from m in companias
                                                            group m by m).ToList();
                                            if (compania.Count == 1)
                                            {
                                                //Para cada uno de los archivos cargados
                                                foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                                                {
                                                    //Obteniendo Documento
                                                    //XmlDocument doc = (XmlDocument)Session["XML"];

                                                    try
                                                    {
                                                        //Traduciendo a texto XML
                                                        XDocument doc = XDocument.Load(new MemoryStream(b));
                                                        //si se cargó correctamente
                                                        if (doc != null)
                                                        {
                                                            //Convirtiendo Documento
                                                            XmlDocument xmlDocument = new XmlDocument();
                                                            using (var xmlReader = doc.CreateReader())
                                                            {
                                                                //Cargando XML Document
                                                                xmlDocument.Load(xmlReader);
                                                            }
                                                            //Convirtiendo a XDocument
                                                            //XDocument cfdi = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);

                                                            //Validando versión
                                                            switch (doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value)
                                                            {
                                                                case "3.2":
                                                                    {
                                                                    //string serie = doc.Root.Attribute("serie").Value;
                                                                    string folio = "";
                                                                    if (doc.Root.Attribute("folio") != null)
                                                                    {
                                                                        folio = doc.Root.Attribute("folio").Value;
                                                                    }
                                                                    XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                                                    XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                                                       where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                                                       select el).DefaultIfEmpty(null).FirstOrDefault();
                                                                    string uuid = timbre.Attribute("UUID").Value;
                                                                    foreach (string a in (List<string>)Session["id_registro_c"])
                                                                        {
                                                                            if (a.Contains(folio) || a.Contains(uuid))
                                                                                archivo = a.ToString();
                                                                        }
                                                                    //Instanciando Compania Emisora (Proveedor)
                                                                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                                        {
                                                                            //Validando que Exista el Proveedor
                                                                            if (emi.id_compania_emisor_receptor > 0)
                                                                            {
                                                                                //Invocando Método de Guardado de XML
                                                                                //validaEstatusPublicacionSAT(btnAceptar);
                                                                                guardarRecepcionFactura(xmlDocument, archivo);
                                                                            }
                                                                            else
                                                                            {
                                                                                //Mostrando Proveedor por Ingresar
                                                                                lblProveedorFactura.Text = xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper();

                                                                                //Mostrando ventana Modal
                                                                                TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptar, upbtnAceptar.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                                                            }

                                                                        }
                                                                        break;
                                                                    }
                                                                case "3.3":
                                                                    {
                                                                    //string serie = doc.Root.Attribute("Serie").Value;
                                                                    string folio = "";
                                                                    if (doc.Root.Attribute("Folio") != null)
                                                                    {
                                                                        folio = doc.Root.Attribute("Folio").Value;
                                                                    }
                                                                    XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                                                                    XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                                                                       where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                                                                       select el).DefaultIfEmpty(null).FirstOrDefault();
                                                                    string uuid = timbre.Attribute("UUID").Value;
                                                                    foreach (string a in (List<string>)Session["id_registro_c"])
                                                                    {
                                                                        if (a.Contains(folio) || a.Contains(uuid))
                                                                            archivo = a.ToString();
                                                                    }
                                                                    //Instanciando Compania Emisora (Proveedor)
                                                                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                                                        {
                                                                            //Validando que Exista el Proveedor
                                                                            if (emi.id_compania_emisor_receptor > 0)
                                                                                //Invocando Método de Guardado de XML
                                                                                guardarRecepcionFactura(xmlDocument, archivo);//validaEstatusPublicacionSAT(btnAceptar);
                                                                            else
                                                                            {
                                                                                //Mostrando Proveedor por Ingresar
                                                                                lblProveedorFactura.Text = xmlDocument.DocumentElement["cfdi:Emisor"].Attributes["Nombre"].Value.ToUpper();

                                                                                //Mostrando ventana Modal
                                                                                TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptar, upbtnAceptar.GetType(), "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion(ex.Message);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Estableciendo estatus a nuevo registro
                                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                                //Inicialziando la forma
                                                inicializaPagina();
                                                //Instanciando Excepcion
                                                resultado = new RetornoOperacion("No todos los CFDI provienen del mismo Proveedor.");
                                            }
                                        }
                                        else
                                        //Instanciando Excepcion
                                        resultado = new RetornoOperacion("No existe la Factura");
                                        break;
                                    }
                                case Pagina.Estatus.Edicion:
                                    {
                                        //Obteniendo Fecha
                                        DateTime fec_recepcion;
                                        DateTime.TryParse(txtFechaRecepcion.Text, out fec_recepcion);

                                        //Instanciamos la recepcion que se desea modificar 
                                        using (Recepcion recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                                        {
                                            //Editando el encabezado
                                            resultado = recepcion.EditaRecepcion(recepcion.id_compania_proveedor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                        recepcion.secuencia, fec_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        }
                                        break;
                                    }
                            }
                            //Operación Exitosa?
                            if (resultado.OperacionExitosa)
                            {
                                //Estableciendo estatus a nuevo registro
                                Session["estatus"] = Pagina.Estatus.Edicion;
                                //Inicializando Id de registro activo 
                                Session["id_registro"] = resultado.IdRegistro;
                                //Inicialziando la forma
                                inicializaPagina();
                            }
                        
                        //Mostrando Notificación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                //Envia al usuario a la pagina principal de la aplicación
                case "Imprimir":
                    {
                        //Valida los estatus de la página
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Si el estatus de la página es nuevo no realiza ninguna acción
                            case Pagina.Estatus.Nuevo:
                                { }
                                break;
                            //Si el estatus de la página es edicion o lectura abre la ventana de gestor de impresión de reportes (RDLC).
                            case Pagina.Estatus.Lectura:
                            case Pagina.Estatus.Edicion:
                                {
                                    //Obteniendo Ruta
                                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuestasPagar/RecepcionFactura.aspx", "~/RDLC/Reporte.aspx");
                                    //Instanciando nueva ventana de navegador para apertura de registro
                                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "AcuseReciboFacturas2", Convert.ToInt32(Session["id_registro"])), "AcuseReciboFacturas2", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                    break;
                                }
                        }
                        break;
                    }
                //Permite al usuario editar el registro actual
                case "Editar":
                    {
                        //Instanciando Recepción
                        using (Recepcion Recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Establecemos el estatus de la pagina a nuevo 
                            Session["estatus"] = Pagina.Estatus.Edicion;
                            //Inicializamos la pagina
                            inicializaPagina();
                        }
                        break;
                    }
                //Deshabilita un registro de la Recepcion de factura
                case "Eliminar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();

                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando registro actual
                            using (Recepcion DesRec = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Si la Recepcion existe
                                if (DesRec.id_recepcion > 0)

                                    //Deshabilitamos el registro
                                    resultado = DesRec.DeshabilitaRecepcion(((Usuario)Session["usuario"]).id_usuario);

                                //Si se deshabilitó correctamente
                                if (resultado.OperacionExitosa)
                                {
                                    //Cargando los detalles ya almacenados en BD
                                    using (DataTable mit = FacturadoProveedor.CargaFacturasRecepcion(DesRec.id_recepcion))
                                    {
                                        //Validando que existan Facturas
                                        if (mit != null)
                                        {
                                            //Recorriendo Registros
                                            foreach (DataRow dr in mit.Rows)
                                            {
                                                //Instanciando Factura
                                                using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(dr["IdFactura"])))
                                                {
                                                    //Validando que exista la Factura
                                                    if (fp.habilitar)
                                                    {
                                                        //Deshabilitando Factura
                                                        resultado = fp.DeshabilitaFacturadoProveedor(((Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que se haya Deshabilitado
                                                        if (!resultado.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;
                                                    }

                                                }
                                            }

                                            //Si se realizaron las Operaciones
                                            if (resultado.OperacionExitosa)

                                                //Instanciando Recepción
                                                resultado = new RetornoOperacion(DesRec.id_recepcion);

                                        }
                                        else
                                            //Instanciando Recepción
                                            resultado = new RetornoOperacion(DesRec.id_recepcion);
                                    }

                                    //Si se deshabilitó correctamente
                                    if (resultado.OperacionExitosa)

                                        //COmpletando Transacción
                                        trans.Complete();


                                }

                            }
                        }

                        //Validando Operación Exitosa
                        if (resultado.OperacionExitosa)
                        {
                            //Estableciendo estatus a nuevo registro
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Inicializando Id de registro activo 
                            Session["id_registro"] = 0;
                            //Inicialziando la forma
                            inicializaPagina();
                        }

                        //Mostrando resultado
                        //lblError.Text = resultado.Mensaje;

                        //Mostrando Notificación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        break;
                    }
                //Permite al usuario editar el registro actual
                case "Confirmar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();
                        //Declarando variables auxiliares
                        int revision = 0;
                        string facturas = "", seriefolio = "", aceptadas = "", pagos = "", refacturadas = "", canceladas = "", rechazadas = "", seriefolioA = "", seriefolioP = "", seriefolioRef = "", seriefolioC = "", seriefolioRec = "";
                        //Cargando los detalles ya almacenados en BD
                        using (DataTable mit = FacturadoProveedor.CargaFacturasRecepcion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que existan Facturas
                            if (mit != null)
                            {
                                //Recorriendo Registros
                                foreach (DataRow dr in mit.Rows)
                                {
                                    //Instanciando Factura
                                    using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(dr["IdFactura"])))
                                    {
                                        //Validando que exista la Factura
                                        if (fp.habilitar)
                                        {                                            
                                            //Validando Estatus de la Factura
                                            switch ((FacturadoProveedor.EstatusFactura)fp.id_estatus_factura)
                                            {
                                                case FacturadoProveedor.EstatusFactura.EnRevision:
                                                    {
                                                        //Instanciando Excepción
                                                        revision = revision + 1;
                                                        seriefolio = fp.serie + fp.folio;//resultado = new ReornoOperacion("La Factura ya ha sido Aceptada.");
                                                        facturas = seriefolio + " // " + facturas;
                                                        lblFacturasR.Visible = true;
                                                        lblFacturasR.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffb5c2");
                                                        break;
                                                    }
                                                case FacturadoProveedor.EstatusFactura.Aceptada:
                                                    {
                                                        //Instanciando Excepción
                                                        //aceptada = aceptada + 1;
                                                        seriefolioA = fp.serie + fp.folio;
                                                        aceptadas = seriefolioA + " // " + aceptadas;
                                                        lblFacturasA.BackColor = System.Drawing.ColorTranslator.FromHtml("#aed47b");
                                                        PAceptadas.Visible = true;
                                                        //resultado = new RetornoOperacion("La Factura ya ha sido Aceptada.");
                                                        break;
                                                    }
                                                case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                                case FacturadoProveedor.EstatusFactura.Liquidada:
                                                    {
                                                        //Instanciando Excepción
                                                        seriefolioP = fp.serie + fp.folio;
                                                        pagos = seriefolioP + " // " + pagos;
                                                        lblFacturasP.BackColor = System.Drawing.ColorTranslator.FromHtml("#aed47b");
                                                        PPagos.Visible = true;
                                                        //pagos = pagos + 1;//resultado = new RetornoOperacion("La Factura tiene Pagos Aplicados.");
                                                        break;
                                                    }
                                                case FacturadoProveedor.EstatusFactura.Refacturacion:
                                                    {
                                                        //Instanciando Excepción
                                                        seriefolioRef = fp.serie + fp.folio;
                                                        refacturadas = seriefolioRef + " // " + refacturadas;
                                                        lblFacturasRef.BackColor = System.Drawing.ColorTranslator.FromHtml("#aed47b");
                                                        PRefacturadas.Visible = true;
                                                        //refacturacion = refacturacion + 1; //resultado = new RetornoOperacion("La Factura ha sido Refacturada.");
                                                        break;
                                                    }
                                                case FacturadoProveedor.EstatusFactura.Cancelada:
                                                    {
                                                        //Instanciando Excepción
                                                        seriefolioC = fp.serie + fp.folio;
                                                        canceladas = seriefolioC + " // " + canceladas;
                                                        lblFacturasC.BackColor = System.Drawing.ColorTranslator.FromHtml("#aed47b");
                                                        PCanceladas.Visible = true;
                                                        //cancelada = cancelada + 1;//resultado = new RetornoOperacion("La Factura ha sido Cancelada.");
                                                        break;
                                                    }
                                                case FacturadoProveedor.EstatusFactura.Rechazada:
                                                    {
                                                        //Instanciando Excepción
                                                        seriefolioRec = fp.serie + fp.folio;
                                                        rechazadas = seriefolioRec + " // " + rechazadas;
                                                        lblFacturasRec.BackColor = System.Drawing.ColorTranslator.FromHtml("#aed47b");
                                                        PRechazadas.Visible = true;
                                                        //rechazada = rechazada + 1;// resultado = new RetornoOperacion("La Factura ha sido Rechazada.");
                                                        break;
                                                    }
                                            }                                            
                                        }                                                                                                                                                                                                                                                                                 
                                    }                                   
                                }                                
                                if(revision == 0)
                                    //Mostrando Notificación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "AceptarConfirmacion", "Las facturas ya han sido aceptadas.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                else
                                {
                                    lblTotalFacturas.Text = "Este proceso aceptara " + revision.ToString() + " factura(s). ¿Estas de acuerdo?";
                                    lblFacturasR.Text = "Se aceptaran la(s) factura(s) " + facturas;
                                    lblFacturasA.Text = "La(s) factura(s) "+ aceptadas + " han sido aceptadas.";
                                    lblFacturasP.Text = "La(s) factura(s) " + pagos + " tienen pagos aplicados.";
                                    lblFacturasRef.Text = "La(s) factura(s) " + refacturadas + " han sido refacturadas.";
                                    lblFacturasC.Text = "La(s) factura(s) " + canceladas + " han sido canceladas.";
                                    lblFacturasRec.Text = "La(s) factura(s) " + rechazadas + " han sido rechazadas.";
                                    //Ocultando ventana modal
                                    ScriptServer.AlternarVentana(lkbConfirmar, "AceptarConfirmacion", "contenidoConfirmarAceptacionModal", "contenidoConfirmarAceptacion");
                                }
                            }
                        }                        
                        break;
                    }
                case "Bitacora":
                    {
                        //Inicializando Ventana de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "74", "Recepción");
                        break;
                    }
                case "Referencias":
                    {
                        //Inicializando Ventana de Referencia
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "74", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
            }
        }
        /// <summary>
        /// Metodo encargado de mandar inprimir documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbImprimir_Click(object sender, EventArgs e)
        {
            //Valida los estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estatus de la página es nuevo no realiza ninguna acción
                case Pagina.Estatus.Nuevo:
                    { }
                    break;
                //Si el estatus de la página es edicion o lectura abre la ventana de gestor de impresión de reportes (RDLC).
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuestasPagar/RecepcionFactura.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "AcuseReciboFacturas2", Convert.ToInt32(Session["id_registro"])), "AcuseReciboFacturas2", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
            }
        }
        /// <summary>
        /// Metodo encargado validar ante el SAT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbValidacion_Click(object sender, ImageClickEventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID;
            decimal monto; DateTime fecha_expedicion;
            if (gvDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalles, sender, "imb", false);                
                //Obteniendo Documento XML
                XDocument xDocument = XDocument.Parse(gvDetalles.SelectedDataKey["xml"].ToString());
                XNamespace ns = xDocument.Root.GetNamespaceOfPrefix("cfdi");
                XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

                //Convirtiendo Documento
                XmlDocument xmlDocument = new XmlDocument();
                using (var xmlReader = xDocument.CreateReader())
                {
                    //Cargando XML Document
                    xmlDocument.Load(xmlReader);
                }

                //Validando versión
                switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
                {
                    case "3.2":
                    case "3.3":
                        {
                            //Realizando validación de estatus en SAT
                            retorno = Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);
                            //Colocando resultado sobre controles
                            imgValidacionSAT.Src = retorno.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                            headerValidacionSAT.InnerText = retorno.Mensaje;
                            lblRFCEmisor.Text = rfc_emisor;
                            lblRFCReceptor.Text = rfc_receptor;
                            lblUUID.Text = UUID;
                            lblTotalFactura.Text = monto.ToString("C");
                            lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                            break;
                        }
                }
                //Mostrando resultado de consulta en SAT (ventana modal)
                ScriptServer.AlternarVentana(gvDetalles, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                //validaEstatusPublicacionSAT(gvDetalles);
            }
        }

        #region Eventos de Detalle

        /// <summary>
        /// Evento producido al cambiar el elemento seleccionado del catalogo de tamaño de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewDetalles_SelectedIndexChanged(object sender, EventArgs e)
        {   //Aplicando nuevo tamaño de página
            Controles.CambiaTamañoPaginaGridView(gvDetalles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                                                    Convert.ToInt32(ddlTamañoGridViewDetalles.SelectedValue));
        }
        /// <summary>
        /// Evento producido al pulsar el botón de exportación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcelDetalles_Click(object sender, EventArgs e)
        {   //Exportando contenido a nuevo archivo excel
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdFactura");
        }
        /// <summary>
        /// Evento producido al aplicar un criterio de orden al contenido del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_Sorting(object sender, GridViewSortEventArgs e)
        {   //Aplicando nuevo criterio de orden
            lblCriterioGridViewDetalles.Text = Controles.CambiaSortExpressionGridView(gvDetalles,
                                               TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                                               e.SortExpression);
        }
        /// <summary>
        /// Evento producido al cambiar la página del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Aplicando nuevo indice de página
            Controles.CambiaIndicePaginaGridView(gvDetalles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                e.NewPageIndex);
        }
        /// <summary>
        /// Click en botón vista previa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            generarVistaPrevia();
        }
        /// <summary>
        /// Click en boton confirmar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            //Determinando respuesta del usuario
            switch (((Button)sender).CommandName)
            {
                case "Confirmar":
                    //Declarando Objeto de Retorno
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Cargando los detalles ya almacenados en BD
                    using (DataTable mit = FacturadoProveedor.CargaFacturasRecepcion(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Validando que existan Facturas
                        if (mit != null)
                        {
                            //Recorriendo Registros
                            foreach (DataRow dr in mit.Rows)
                            {
                                //Instanciando Factura
                                using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(dr["IdFactura"])))
                                {
                                    //Validando que exista la Factura
                                    if (fp.habilitar)
                                    {
                                        //Validando los Estatus donde se puede Aceptar
                                        if ((FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)
                                            //Deshabilitamos el registro
                                            resultado = fp.AceptaFacturaProveedor(((Usuario)Session["usuario"]).id_usuario);                                        
                                    }
                                }
                            }
                        }
                    }
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnConfirmar, "AceptarConfirmacion", "contenidoConfirmarAceptacionModal", "contenidoConfirmarAceptacion");
                    break;
                case "Cancelar":
                    //Realizando proceso de guardado de factura de proveedor
                    //guardarRecepcionFactura();

                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnCancelar, "AceptarConfirmacion", "contenidoConfirmarAceptacionModal", "contenidoConfirmarAceptacion");
                    break;
            }
        }
        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo encargado de inicializar la pagina de acuerdo al estatus en el que se encuentre
        /// </summary>
        private void inicializaPagina()
        {   //Inicializamos el menu de acuerdo al estatus de la pagina
            inicializaMenu();
            //Cargamos los catalogos de la pagina 
            cargaCatalogos();
            //Cargamos los valores predefinidos para cada campo
            inicializaValores();
            //Habilitamos los controles
            habilitaControles();
            //Inicializando Registros
            inicializaRegistrosDetalle();
        }
        /// <summary>
        /// Metodo encargado de inicializar el menu de la forma
        /// </summary>
        private void inicializaMenu()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        //lkbSalir.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        //lkbSalir.Enabled = true;
                        lkbEditar.Enabled = true;
                        lkbEliminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        //lkbSalir.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Metodo encargado de cargar los catalogos de Recepcion
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando el catalogo de tamaño de GirdView (30 Registros)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewDetalles, "", 18);
            //Tipos de Servicio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "-- Seleccione el Tipo --", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }
        /// <summary>
        /// Metodo encargado de inicializar los valores de la forma en razon al Estatus de la pagina
        /// </summary>
        private void inicializaValores()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lblID.Text = "Por Asignar";
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            //Asignando Valor
                            txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                        //Actualizando criterio de filtrado en autocomplete de proveedor
                        txtProveedor.Text = "";
                        txtFechaRecepcion.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy hh:mm");
                        txtEntregadoPor.Text = "";
                        lblError.Text = "";
                        txtMedioRecepcion.Text = "Fisico";
                        //Session["id_registro_c"] = null;
                        //Session["id_registro_b"] = null;
                        //Limpiando nombre de archivo
                        //ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");
                        break;
                    }
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        using (Recepcion ObjRecepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                        {
                            lblID.Text = ObjRecepcion.secuencia.ToString();
                            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(ObjRecepcion.id_compania_receptor))
                                //Actualizando criterio de filtrado en autocomplete de proveedor
                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(ObjRecepcion.id_compania_proveedor))
                                //Actualizando criterio de filtrado en autocomplete de proveedor
                                txtProveedor.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            txtFechaRecepcion.Text = ObjRecepcion.fecha_recepcion.ToString("dd/MM/yyyy hh:mm");
                            txtEntregadoPor.Text = ObjRecepcion.entregado_por.ToString();
                            txtMedioRecepcion.Text = ObjRecepcion.id_medio_recepcion == 1 ? "Fisico" : "Correo";
                        }
                        break;
                    }
            }
            //Limpiando erorres
            lblError.Text = "";
        }
        /// <summary>
        /// Metodo encargado de habilitar y deshabilitar los controles de la forma 
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        txtProveedor.Enabled =
                        txtEntregadoPor.Enabled =
                        btnAceptar.Enabled =
                        txtFechaRecepcion.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Nuevo:
                    {
                        txtProveedor.Enabled =
                        txtEntregadoPor.Enabled =
                        btnAceptar.Enabled =
                        txtFechaRecepcion.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        txtProveedor.Enabled =
                        txtFechaRecepcion.Enabled =
                        txtEntregadoPor.Enabled =
                        btnAceptar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Carga los detalles de Factura asociados al registro activo en sesión
        /// </summary>
        private void inicializaRegistrosDetalle()
        {   //Cargando los detalles ya almacenados en BD
            using (DataTable mit = FacturadoProveedor.CargaFacturasRecepcion(Convert.ToInt32(Session["id_registro"])))
            {   //Si existen registros en el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {   //Cargando origen de datos temporal en GV
                    TSDK.ASP.Controles.CargaGridView(gvDetalles, mit, "IdFactura", "", true, 1);
                    //Almacenando tabla temporal en sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {   //Inicializando indices
                    TSDK.ASP.Controles.InicializaGridview(gvDetalles);
                    //Almacenando tabla temporal en sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Limpiando errores
            lblError.Text = "";
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private bool validaFacturaXML(out string mensaje)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Limpiando Mensaje
            mensaje = "";

            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Compania
                            using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                //Declarando Variable Auxiliar
                                int idProveedorEmisor = 0;

                                //Validando que exista la Compania
                                if (emi.id_compania_emisor_receptor > 0)
                                {
                                    //Asignando Emisor
                                    idProveedorEmisor = emi.id_compania_emisor_receptor;

                                    //Instanciando Proveedor
                                    resultado = new RetornoOperacion(idProveedorEmisor);
                                }
                                //Si no existe
                                else
                                {
                                    //Insertando Compania
                                    resultado = SAT_CL.Global.CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(),
                                                                                doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(), 0, false, false, true, Convert.ToInt32(ddlTipoServicio.SelectedValue), "", "", "", 0, 0,
                                                                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, "FACTURAS DE PROVEEDOR", "", 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Inserción haya sido Exitosa
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Asignando Registro
                                        idProveedorEmisor = resultado.IdRegistro;

                                        //Instanciando Proveedor
                                        using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(idProveedorEmisor))
                                        {
                                            //Validando que Existe el Registro
                                            if (pro.id_compania_emisor_receptor > 0)
                                            {
                                                //Declarando Variables Auxiliares
                                                int idPais = 0, idEstado = 0;

                                                //Obtiene Pais Estado
                                                SAT_CL.Global.Direccion.ObtienePaisEstado(doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["pais"].Value,
                                                            doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["estado"].Value, out idPais, out idEstado);

                                                //Validando que Existan el Pais y el Estado
                                                if (idPais > 0 && idEstado > 0)
                                                {
                                                    //Insertando Dirección
                                                    resultado = SAT_CL.Global.Direccion.InsertaDireccion(2, doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["calle"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noExterior"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noExterior"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noInterior"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noInterior"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["colonia"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["colonia"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["localidad"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["localidad"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["municipio"].Value,
                                                        doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["referencia"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["referencia"].Value,
                                                        idEstado, idPais, doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["codigoPostal"].Value, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación fuese Exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Guardando Dirección
                                                        int idDireccion = resultado.IdRegistro;

                                                        //Actualizando la Dirección del Proveedor
                                                        resultado = pro.EditaCompaniaEmisorRecepto(pro.id_alterno, pro.rfc, pro.nombre, pro.nombre_corto, resultado.IdRegistro, pro.bit_emisor, pro.bit_receptor,
                                                                        pro.bit_proveedor, pro.id_tipo_servicio, pro.contacto, pro.correo, pro.telefono, pro.limite_credito, pro.dias_credito, pro.id_compania_uso,
                                                                        pro.id_compania_agrupador, pro.informacion_adicional1, pro.informacion_adicional2, pro.id_regimen_fiscal, pro.id_uso_cfdi, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                }
                                                else
                                                {
                                                    //Instanciando Resultado
                                                    resultado = new RetornoOperacion(pro.id_compania_emisor_receptor, "La Compania fue dad de alta Exitosamente, pero no se registro la Dirección", true);
                                                }
                                            }
                                        }

                                    }
                                }

                                //Validando que el RFC sea igual
                                if (idProveedorEmisor > 0)
                                {
                                    //Instanciando Compania
                                    using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                    {
                                        //Validando que el RFC sea igual
                                        if (cer.rfc.ToUpper() == doc.DocumentElement["cfdi:Receptor"].Attributes["rfc"].Value.ToUpper())
                                        {
                                            //Instanciando XSD de validación
                                            using (EsquemasFacturacion ef = new EsquemasFacturacion(doc["cfdi:Comprobante"].Attributes["version"].Value))
                                            {
                                                //Validando que exista el XSD
                                                if (ef.id_esquema_facturacion != 0)
                                                {
                                                    //Declarando variables Auxiliares
                                                    bool addenda;

                                                    //Obteniendo XSD
                                                    string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(ef.version, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out addenda);

                                                    //
                                                    if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                        //
                                                        doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                    //Obteniendo Validación
                                                    result = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Mostrando el Mensaje
                                            mensaje = "El RFC de la factura no coincide con el Receptor";

                                            //Asignando Negativa el Objeto de retorno
                                            result = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //Mostrando el Mensaje
                                    mensaje = "El RFC de la factura no coincide con el Emisor";

                                    //Asignando Negativa el Objeto de retorno
                                    result = false;
                                }

                                //Validando que las Operaciónes se Completaron
                                if (resultado.OperacionExitosa)

                                    //Completando Transacción
                                    trans.Complete();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Mostrando Mensaje
                        mensaje = e.Message;
                    }
                }
                else//Mensaje de Error
                    mensaje = "No se ha podido cargar el Archivo";
            }
            else//Mensaje de Error
                mensaje = "No se ha podido localizar el Archivo";

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la validación del estatus de publicación del CFDI en servidores del SAT
        /// </summary>
        private void validaEstatusPublicacionSAT(System.Web.UI.Control control)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID;
            decimal monto; DateTime fecha_expedicion;

            //Realizando validación de estatus en SAT
            result = Comprobante.ValidaEstatusPublicacionSAT((XmlDocument)Session["XML"], out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

            //Colocando resultado sobre controles
            imgValidacionSAT.Src = result.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
            headerValidacionSAT.InnerText = result.Mensaje;
            lblRFCEmisor.Text = rfc_emisor;
            lblRFCReceptor.Text = rfc_receptor;
            lblUUID.Text = UUID;
            lblTotalFactura.Text = monto.ToString("C");
            lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");

            //Mostrando resultado de consulta en SAT (ventana modal)
            ScriptServer.AlternarVentana(control, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de la Factura
        /// </summary>
        /// <param name="document">Factura XML</param>
        /// <param name="total_p">Total en Pesos</param>
        /// <param name="subtotal_p">Subtotal en Pesos</param>
        /// <param name="descuento_p">Descuento en Pesos</param>
        /// <param name="traslado_p">Importe Trasladado en Pesos</param>
        /// <param name="retenido_p">Importe Retenido en Pesos</param>
        /// <param name="monto_tc">Monto del Tipo de Cambio</param>
        private void obtieneCantidades(XmlDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
        {   //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {   //Asignando Atributo Obligatorios
                monto_tc = 1;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value);
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value);
                traslado_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0;
                retenido_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0;
                //Asignando Atributos Opcionales
                descuento_p = document.DocumentElement.Attributes["descuento"] == null ? 0 :
                    Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value);
            }
            else
            {   //Asignando Atributo Obligatorios
                monto_tc = Convert.ToDecimal(document.DocumentElement.Attributes["TipoCambio"].Value);
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value) * monto_tc;
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value) * monto_tc;
                traslado_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0) * monto_tc;
                retenido_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0) * monto_tc;
                //Asignando Atributos Opcionales
                descuento_p = (document.DocumentElement.Attributes["descuento"] == null ? 0 : Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value)) * monto_tc;
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private void obtieneCantidadesConcepto(XmlDocument cfdi, XmlNode concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {
            //Validación de Retenciones
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
            {
                //Validando que el Importe no sea "0"
                if (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) > 0)

                    //Asignando Valores
                    tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
                else
                    //Asignando Valores
                    tasa_imp_ret = 0;
            }
            else
                //Asignando Valores
                tasa_imp_ret = 0;

            //Validación de Traslados
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
            {
                //Obteniendo Valor de la Tasa
                string tasaImpTrasladado = cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value;

                //Remplazando Puntos Decimales
                tasaImpTrasladado = tasaImpTrasladado.Replace(".", "|");

                //Validando que exista un valor despues del Punto decimal
                if (Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1)) > 0.00M)

                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1));
                else
                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 0));
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }
        ///<summary>
        ///Metodo encargado de guardar la Recepcion
        ///</summary>
        private void guardarRecepcion()
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando Mensaje de Excepcion
            string mensaje = "";

            //Variable para almacenar Id de registro recepción afectado
            int idRecepcion = 0;
            int idProveedorEmisor = 0;

            //Obteniendo Fecha de Recepción
            DateTime fecha_recepcion = DateTime.MinValue;
            DateTime.TryParse(txtFechaRecepcion.Text, out fecha_recepcion);

            //Inicializando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que Exista el Documento
                if (doc != null)
                {
                    //Instanciando Emisor (Proveedor)
                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Validando que Exista el Proveedor
                        if (emi.id_compania_emisor_receptor > 0)
                        {
                            //Asignando Emisor
                            idProveedorEmisor = emi.id_compania_emisor_receptor;

                            //Instanciando Proveedor
                            resultado = new RetornoOperacion(idProveedorEmisor);
                        }
                        else
                        {
                            //Insertando Compania
                            resultado = SAT_CL.Global.CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(),
                                                                        doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(), 0, true, false, true, Convert.ToInt32(ddlTipoServicio.SelectedValue), "", "", "", 0, 0, 0, 0,
                                                                        "FACTURAS DE PROVEEDOR", "", 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Inserción haya sido Exitosa
                            if (resultado.OperacionExitosa)
                            {
                                //Asignando Registro
                                idProveedorEmisor = resultado.IdRegistro;

                                //Instanciando Proveedor
                                using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(idProveedorEmisor))
                                {
                                    //Validando que Existe el Registro
                                    if (pro.id_compania_emisor_receptor > 0)
                                    {
                                        //Declarando Variables Auxiliares
                                        int idPais = 0, idEstado = 0;

                                        //Obtiene Pais Estado
                                        SAT_CL.Global.Direccion.ObtienePaisEstado(doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["pais"].Value,
                                                    doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["estado"].Value, out idPais, out idEstado);

                                        //Validando que Existan el Pais y el Estado
                                        if (idPais > 0 && idEstado > 0)
                                        {
                                            //Insertando Dirección
                                            resultado = SAT_CL.Global.Direccion.InsertaDireccion(2, doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["calle"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noExterior"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noExterior"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noInterior"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["noInterior"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["colonia"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["colonia"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["localidad"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["localidad"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["municipio"].Value,
                                                doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["referencia"] == null ? "" : doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["referencia"].Value,
                                                idEstado, idPais, doc.DocumentElement["cfdi:Emisor"]["cfdi:DomicilioFiscal"].Attributes["codigoPostal"].Value, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la Operación fuese Exitosa
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Guardando Dirección
                                                int idDireccion = resultado.IdRegistro;

                                                //Actualizando la Dirección del Proveedor
                                                resultado = pro.EditaCompaniaEmisorRecepto(pro.id_alterno, pro.rfc, pro.nombre, pro.nombre_corto, resultado.IdRegistro, pro.bit_emisor, pro.bit_receptor,
                                                                pro.bit_proveedor, pro.id_tipo_servicio, pro.contacto, pro.correo, pro.telefono, pro.limite_credito, pro.dias_credito, pro.id_compania_uso,
                                                                pro.id_compania_agrupador, pro.informacion_adicional1, pro.informacion_adicional2, pro.id_regimen_fiscal, pro.id_uso_cfdi, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                        else
                                        {
                                            //Instanciando Resultado
                                            resultado = new RetornoOperacion(pro.id_compania_emisor_receptor, "La Compania fue dad de alta Exitosamente, pero no se registro la Dirección", true);
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("No existe el Proveedor");
                                }
                            }
                        }
                    }

                    //Validando que las Operaciones fuesen Exitosas
                    if (resultado.OperacionExitosa)
                    {
                        //De acuerdo al estatus de la pagina
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Nuevo:
                                {
                                    //Insertando encabezado
                                    resultado = Recepcion.InsertaRecepcion(idProveedorEmisor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                    fecha_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Instanciamos la recepcion que se desea modificar 
                                    using (Recepcion recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                                    {
                                        //Editando el encabezado
                                        resultado = recepcion.EditaRecepcion(idProveedorEmisor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                    recepcion.secuencia, fecha_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                }

                                break;
                        }

                        //Validamos que la operacion se haya realizado
                        if (resultado.OperacionExitosa)
                        {
                            //Guardando Id de Encabezado
                            idRecepcion = resultado.IdRegistro;

                            //Recuperando tabla temporal
                            if (validaFacturaXML(out mensaje))
                            {
                                //Declarando variables de Montos
                                decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                                //Obteniendo Valores
                                obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                                //Instanciando Emisor-Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que coincida el RFC
                                    if (emisor.id_compania_emisor_receptor > 0)
                                    {
                                        //Instanciando Emisor-Compania
                                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                        {
                                            //Validando que coincida el RFC del Receptor
                                            if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                            {
                                                //Insertando factura
                                                resultado = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                    0, doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                    doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                    doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                    Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI, "I", 1,
                                                                                    (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                    idRecepcion, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                    doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                    doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                                                                                    Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0,
                                                                                    doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                                                                                    Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0,
                                                                                    doc["cfdi:Comprobante"].Attributes["Moneda"] == null ? "" : doc["cfdi:Comprobante"].Attributes["Moneda"].Value,
                                                                                    monto_tc, Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), total_p, subtotal_p, descuento_p, traslado_p,
                                                                                    retenido_p, false, DateTime.MinValue, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value),
                                                                                    doc["cfdi:Comprobante"].Attributes["condicionesDePago"] == null ? "" : doc["cfdi:Comprobante"].Attributes["condicionesDePago"].Value,
                                                                                    emisor.dias_credito, 1, (byte)FacturadoProveedor.EstatusValidacion.ValidacionSintactica, "",
                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                            else
                                                //Instanciando Excepcion
                                                resultado = new RetornoOperacion("La Compania Receptora no esta registrada");
                                        }
                                    }
                                    else
                                        //Instanciando Excepcion
                                        resultado = new RetornoOperacion("La Compania Proveedora no esta registrado");
                                }
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Factura
                                    int idFactura = resultado.IdRegistro;
                                    //Obteniendo Nodos de Concepto
                                    XmlNodeList xmlNL = doc.GetElementsByTagName("cfdi:Concepto");
                                    //Declarando Variables Auxiliares
                                    decimal tasa_imp_ret, tasa_imp_tras;
                                    bool res = true;
                                    int contador = 0;
                                    //Recorriendo cada 
                                    while (res)
                                    {
                                        //Obteniendo Concepto
                                        XmlNode node = xmlNL[contador];
                                        //Obteniendo Cantidades del Concepto
                                        obtieneCantidadesConcepto(doc, node, out tasa_imp_tras, out tasa_imp_ret);
                                        //Insertando Cocepto de Factura
                                        resultado = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                                node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                                node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                                Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                                Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                                tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Incrementando Contador
                                        contador++;
                                        //Obteniendo resultado del Ciclo
                                        res = contador >= xmlNL.Count ? false : resultado.OperacionExitosa;
                                    }

                                    //Validando resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Declarando Variables Auxiliares
                                        string ruta;
                                        //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                        ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());
                                        //Insertamos Registro
                                        resultado = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                            Encoding.UTF8.GetBytes(doc.OuterXml), ruta);
                                    }
                                }
                            }
                            else//Instanciando Excepcion
                                resultado = new RetornoOperacion(mensaje);
                        }
                    }
                }
                else
                {   //Validando el Estatus de la Página
                    switch ((Pagina.Estatus)Session["estatus"])
                    {
                        case Pagina.Estatus.Nuevo:
                            {   //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe la Factura");
                                break;
                            }
                        case Pagina.Estatus.Edicion:
                            {
                                //Obteniendo Fecha
                                DateTime fec_recepcion;
                                DateTime.TryParse(txtFechaRecepcion.Text, out fec_recepcion);

                                //Instanciamos la recepcion que se desea modificar 
                                using (Recepcion recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Editando el encabezado
                                    resultado = recepcion.EditaRecepcion(recepcion.id_compania_proveedor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                recepcion.secuencia, fec_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                break;
                            }
                    }
                }

                //Validando la Operación de la Transacción
                if (resultado.OperacionExitosa)
                    //Completando Transacción
                    trans.Complete();
            }
            //Validando que exista
            if (resultado.OperacionExitosa)
            {
                //Reasignando Id de registro
                resultado = new RetornoOperacion(idRecepcion);
                //Establecemos el id del registro
                Session["id_registro"] = resultado.IdRegistro;
                //Establecemos el estatus de la forma
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Inicializamos la forma
                inicializaPagina();
            }

            //Eliminando Contenido en Sessión del XML (Ya que existe un script que limpia el contenido del área de arrastre del xml después del click final de guardado)
            Session["XML"] = null;

            //Actualizamos la etiqueta de errores
            lblError.Text = resultado.Mensaje;
        }
        ///<summary>
        ///Metodo encargado de guardar la Recepcion (Facturación V3.2 v3.3)
        ///</summary>
        private void guardarRecepcionFactura(XmlDocument xml, string nombre)
        {
            //Declaracion de objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Variable para almacenar Id de registro recepción afectado
            int idRecepcion = 0;

            //Obteniendo Fecha de Recepción
            DateTime fecha_recepcion = DateTime.MinValue;
            DateTime.TryParse(txtFechaRecepcion.Text, out fecha_recepcion);

            //Inicializando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Documento XML
                XmlDocument doc = xml;//(XmlDocument)Session["XML"];

                //Validando que Exista el Documento
                if (doc != null)
                {
                    //Convirtiendo a XDocument
                    XDocument cfdi = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);

                    //Validando XDocument
                    if (cfdi != null)
                    {
                        //Validando versión
                        switch (cfdi.Root.Attribute("version") != null ? cfdi.Root.Attribute("version").Value : cfdi.Root.Attribute("Version").Value)
                        {
                            case "3.2":
                                {
                                    //Insertando CFDI 3.2
                                    resultado = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                             Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlTipoServicio.SelectedValue), nombre, cfdi, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    break;
                                }
                            case "3.3":
                                {
                                    //if(txtProveedor.Text == )
                                    //Insertando CFDI 3.3 y Recepción de Factura
                                    resultado = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                             Convert.ToInt32(Session["id_registro"]), fecha_recepcion, Cadena.RegresaCadenaSeparada(txtEntregadoPor.Text.ToUpper()," ID:", 0), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                             Convert.ToInt32(ddlTipoServicio.SelectedValue), nombre, cfdi, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    break;
                                }
                        }
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("Archivo XML de la Factura mal formado, imposible recuperar la Factura");
                }
                else
                {   //Validando el Estatus de la Página
                    switch ((Pagina.Estatus)Session["estatus"])
                    {
                        case Pagina.Estatus.Nuevo:
                            {   //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe la Factura");
                                break;
                            }
                        case Pagina.Estatus.Edicion:
                            {
                                //Obteniendo Fecha
                                DateTime fec_recepcion;
                                DateTime.TryParse(txtFechaRecepcion.Text, out fec_recepcion);

                                //Instanciamos la recepcion que se desea modificar 
                                using (Recepcion recepcion = new Recepcion(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Editando el encabezado
                                    resultado = recepcion.EditaRecepcion(recepcion.id_compania_proveedor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                recepcion.secuencia, fec_recepcion, txtEntregadoPor.Text.ToUpper(), Convert.ToByte(txtMedioRecepcion.Text == "Fisico" ? "1" : "2"),
                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                break;
                            }
                    }
                }

                //Validando la Operación de la Transacción
                if (resultado.OperacionExitosa)
                {
                    //Instanciando Recepción
                    idRecepcion = resultado.IdRegistro;
                    //Completando Transacción
                    trans.Complete();
                }
            }
            //Validando que exista
            if (resultado.OperacionExitosa)
            {
                //Reasignando Id de registro
                resultado = new RetornoOperacion(idRecepcion);
                //Establecemos el id del registro
                Session["id_registro"] = resultado.IdRegistro;
                //Establecemos el estatus de la forma
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Inicializamos la forma
                inicializaPagina();
            }

            //Eliminando Contenido en Sessión del XML (Ya que existe un script que limpia el contenido del área de arrastre del xml después del click final de guardado)
            //Session["id_registro_b"] = null;
            //Session["id_registro_c"] = null;

            //Actualizamos la etiqueta de errores
            lblError.Text = resultado.Mensaje;
            //Mostrando Notificación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/RecepcionFactura.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Recepción de Facturas", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/RecepcionFactura.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Recepción", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/RecepcionFactura.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Almacena un archivo en memoria de sesión
        /// </summary>
        /// <param name="archivoBase64">Contenido del archivo en formato Base64</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="mimeType">Tipo de contendio del archivo</param>
        /// <returns></returns>
        [WebMethod]
        public static string LecturaArchivo(string archivoBase64, string nombreArchivo, string mimeType)
        {
            //Definiendo objeto de retorno
            string resultado = "";
            //Si hay elementos
            if (!string.IsNullOrEmpty(archivoBase64))
            {
                //Validando tipo de archivo (mime type), debe ser .xml
                if (mimeType == "text/xml")
                {
                    List<byte[]> lista = new List<byte[]>();
                    List<string> archivos = new List<string>();
                    //Convietiendo archivo a bytes
                    byte[] array = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));
                    if (HttpContext.Current.Session["id_registro_b"] != null && HttpContext.Current.Session["id_registro_c"] != null)
                    {
                        try
                        {
                            lista = (List<byte[]>)HttpContext.Current.Session["id_registro_b"];
                            archivos = (List<string>)HttpContext.Current.Session["id_registro_c"];
                        }
                        catch (Exception ex) { }
                    }
                    if (lista.Count < 10)
                    {
                        //Añadiendo archivo
                        lista.Add(array);
                        archivos.Add(nombreArchivo);
                        //Salvando en sesión
                        HttpContext.Current.Session["id_registro_b"] = lista;
                        HttpContext.Current.Session["id_registro_c"] = archivos;
                        if (lista.Count > 1)
                            resultado = string.Format("'{0}' Archivos cargados correctamente!!!", lista.Count);
                        else
                            resultado = string.Format("Archivo '{0}' cargado correctamente!!!", nombreArchivo);
                    }
                    else
                    {
                        //Limpiando archivos de sesión
                        //HttpContext.Current.Session["id_registro_b"] = null;
                        //HttpContext.Current.Session["id_registro_c"] = null;
                        resultado = "'Solo puede cargar máximo 10 archivos!!!";
                    }
                }
                //Si el tipo de archivo no es válido
                else
                    resultado = "El archivo seleccionado no tiene un formato válido. Formato permitido '.xml'.";
            }
            //Archivo sin contenido
            else
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el proceso de visualización de cambios a realizar en base a la información recuperada desde el archivo
        /// </summary>
        private void generarVistaPrevia()
        {
            //Declarando objeto de resultado
            List<RetornoOperacion> resultados = new List<RetornoOperacion>();
            string archivo = "";
            bool res = true;

            //Validando Sesión Registro B
            if (Session["id_registro_b"] != null && Session["id_registro_c"] != null)
            {
                //Creando tabla concentradora de información
                DataTable dtImportacion = new DataTable();
                //Añadiendo columna para enumerar resultados
                DataColumn cID = new DataColumn("cont", typeof(int));
                cID.AutoIncrement = true;
                cID.AutoIncrementSeed = 1;
                cID.AutoIncrementStep = 1;
                dtImportacion.Columns.Add(cID);
                dtImportacion.Columns.Add("id", typeof(int));
                dtImportacion.Columns.Add("xml", typeof(string));
                dtImportacion.Columns.Add("nombre", typeof(string));
                dtImportacion.Columns.Add("Serie", typeof(string));
                dtImportacion.Columns.Add("Folio", typeof(string));
                dtImportacion.Columns.Add("UUID", typeof(string));
                dtImportacion.Columns.Add("FechaFactura", typeof(DateTime));
                dtImportacion.Columns.Add("SubTotal", typeof(int));
                dtImportacion.Columns.Add("Descuento", typeof(int));
                dtImportacion.Columns.Add("Trasladado", typeof(int));
                dtImportacion.Columns.Add("Retenido", typeof(int));
                dtImportacion.Columns.Add("Total", typeof(int));
                dtImportacion.Columns.Add("EstatusSAT", typeof(string));
                //Para cada uno de los archivos cargados
                foreach (byte[] b in (List<byte[]>)Session["id_registro_b"])
                {
                    //Inicializando resultado
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Intentando Obtener campos del XML 
                    try
                    {
                        //Traduciendo a texto XML
                        XDocument doc = XDocument.Load(new MemoryStream(b));
                        //si se cargó correctamente
                        if (doc != null)
                        {
                            //Recuperando datos de interés desde el XML
                            XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");
                            string version = doc.Root.Attribute("version") != null ? doc.Root.Attribute("version").Value : doc.Root.Attribute("Version").Value;
                            //string IdByte = doc.ToString();
                            //int id_cfdi = 0;
                            //string rfcE = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("rfc").Value : doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value;
                            //string rfcR = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("rfc").Value : doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value;
                            //string emisor = version.Equals("3.2") ? doc.Root.Element(ns + "Emisor").Attribute("nombre").Value : doc.Root.Element(ns + "Emisor").Attribute("Nombre").Value;
                            //string receptor = version.Equals("3.2") ? doc.Root.Element(ns + "Receptor").Attribute("nombre").Value : doc.Root.Element(ns + "Receptor").Attribute("Nombre").Value;
                            byte tipoCFDI = 0;
                            if (version.Equals("3.2"))
                            {
                                switch (doc.Root.Attribute("tipoDeComprobante").Value.ToLower())
                                {
                                    case "ingreso":
                                        tipoCFDI = 1;
                                        break;
                                    case "egreso":
                                        tipoCFDI = 2;
                                        break;
                                    default:
                                        tipoCFDI = 3;
                                        break;
                                }
                            }
                            else
                            {
                                switch (doc.Root.Attribute("TipoDeComprobante").Value.ToUpper())
                                {
                                    case "I":
                                        tipoCFDI = 1;
                                        break;
                                    case "E":
                                        tipoCFDI = 2;
                                        break;
                                    default:
                                        tipoCFDI = 3;
                                        break;
                                }
                            }

                            string serie = "";
                            if (doc.Root.Attribute("serie") != null || doc.Root.Attribute("Serie") != null)
                            {
                                serie = version.Equals("3.2") ? doc.Root.Attribute("serie").Value : doc.Root.Attribute("Serie").Value;
                            }
                            string folio = "";
                            if (doc.Root.Attribute("folio") != null || doc.Root.Attribute("Folio") != null)
                            {
                                folio = version.Equals("3.2") ? doc.Root.Attribute("folio").Value : doc.Root.Attribute("Folio").Value;
                            }
                            DateTime fecha = DateTime.Parse(version.Equals("3.2") ? doc.Root.Attribute("fecha").Value : doc.Root.Attribute("Fecha").Value);
                            XElement timbre = (from XElement el in doc.Root.Element(ns + "Complemento").Elements()
                                               where el.Name.Equals(el.GetNamespaceOfPrefix("tfd") + "TimbreFiscalDigital")
                                               select el).DefaultIfEmpty(null).FirstOrDefault();
                            string uuid = timbre.Attribute("UUID").Value;
                            //string tipoComprobante = doc.Root.Attribute("TipoDeComprobante").Value.ToUpper();
                            decimal subtotal = Convert.ToDecimal(version.Equals("3.2") ? doc.Root.Attribute("subTotal").Value : doc.Root.Attribute("SubTotal").Value);
                            decimal retenciones = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "totalImpuestosRetenidos", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "TotalImpuestosRetenidos", "0"));
                            decimal traslados = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "totalImpuestosTrasladados", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root.Element(ns + "Impuestos"), "TotalImpuestosTrasladados", "0"));
                            decimal descuento = Convert.ToDecimal(version.Equals("3.2") ? TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root, "descuento", "0") : TSDK.Base.Xml.DevuleveValorAtibutoElementoCadena(doc.Root, "Descuento", "0"));
                            decimal total = Convert.ToDecimal(version.Equals("3.2") ? doc.Root.Attribute("total").Value : doc.Root.Attribute("Total").Value);
                            //string estatus = FacturadoProveedor.EstatusFactura.EnRevision.ToString();                             
                            //Para cada uno de los archivos cargados
                            foreach (string a in (List<string>)Session["id_registro_c"])
                            {
                                if (a.Contains(folio) || a.Contains(uuid))
                                    archivo = a.ToString();
                            }
                            dtImportacion.Rows.Add(null, null, doc, archivo, serie, folio, uuid, fecha, subtotal, descuento, traslados, retenciones, total);
                            
                        }
                        else
                            resultado = new RetornoOperacion("Error al leer contenido de archivo XML.");
                    }
                    catch (Exception ex)
                    {
                        resultado = new RetornoOperacion(string.Format("Excepción al importar archivo: {0}", ex.Message));
                    }
                    //Añadiendo resultado
                    resultados.Add(resultado);
                }
                //Almacenando resultados en sesión
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportacion, "TableImportacion");

                //Borrando archivo de memoria, una vez que se cargó a una tabla
                //Session["id_registro_b"] = null;
                //Session["id_registro"] = null;
                //Limpiando nombre de archivo
                //ScriptServer.EjecutaFuncionDefinidaJavaScript(this, "<script> BorraNombreArchivoCargado(); </script>", "NombreArchivo");

                //Llenando gridview de vista previa (Sin llaves de selección)
                Controles.CargaGridView(gvDetalles, dtImportacion, "cont-xml", lblCriterioGridViewDetalles.Text, true, 1);

                //Señalando resultado exitoso
                resultados.Add(new RetornoOperacion("Vista Previa generada con éxito.", true));
            }
            else
                //Instanciando Excepcion
                resultados.Add(new RetornoOperacion("Debe de cargar una Factura"));

            //Mostrando resultado general
            RetornoOperacion global = RetornoOperacion.ValidaResultadoOperacionMultiple(resultados, RetornoOperacion.TipoValidacionMultiple.Cualquiera, " ");
            ScriptServer.MuestraNotificacion(this.Page, global, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion        
    }
}