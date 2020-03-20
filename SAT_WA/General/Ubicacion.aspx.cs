using Microsoft.SqlServer.Types;
using RestSharp;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.General
{
    public partial class Ubicacion : System.Web.UI.Page
    {
        #region Atributos

        /// <summary>
        /// Método encargado de Obtener la Clave de la API de Maps
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string _maps_api() { return ConfigurationManager.AppSettings["MapsEmbedAPI"]; }
        /// <summary>
        /// Método que obtiene la Posición Actual de quien Consulta
        /// </summary>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string ObtienePosicionActual(string latitud, string longitud)
        {
            //Definiendo objeto de retorno
            string resultado = "";

            //Si hay elementos en Latitud
            if (!string.IsNullOrEmpty(latitud))
            {
                //Si hay elementos en Latitud
                if (!string.IsNullOrEmpty(longitud))
                {
                    double lat, lon;
                    double.TryParse(latitud, out lat);
                    double.TryParse(latitud, out lon);

                    if (lat != 0 && lon != 0)
                    {
                        //Almacenando en variables de sesión
                        HttpContext.Current.Session["LastLocation"] = string.Format("{0},{1}", lat, lon);
                        //Instanciando Resultado Positivo
                        resultado = string.Format("La Posición '{0}'|'{1}' fue obtenida correctamente!!!", lat, lon);
                    }
                    else
                        resultado = "No se puede recuperar la Longitud";
                }
                else
                    resultado = "No se puede recuperar la Longitud";
            }
            else
                resultado = "No se puede recuperar la Latitud";

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es una recarga de página
            if (!this.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Carga Catalogos 
                cargaCatalogos();
                //Boton Default
                this.Form.DefaultButton = btnAceptar.UniqueID;
            }            
        }

        /// <summary>
        /// Evento generado al Cancelar una Ubicación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus es edición
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion)
                Session["estatus"] = Pagina.Estatus.Lectura;
            inicializaForma();
        }

        /// <summary>
        /// Evento generado al guardar la Ubicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            guardaUbicacion();
        }

        /// <summary>
        /// Evento producido al dar clic sobre algún elemento de menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Determinando el botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    //Limpiando Id de sesión
                    Session["id_registro"] = 0;
                    //Limpiando contenido de forma
                    inicializaForma();
                    //Foco a primer control de captura
                    txtDescripcion.Focus();
                    break;
                case "Abrir":
                    //Inicializa apertura registro
                    inicializaAperturaRegistro(15, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                    break;
                case "Guardar":
                    guardaUbicacion();
                    break;
                case "Imprimir":
                    //TODO: Implementar uso de impresión requerida
                    break;
                case "Editar":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    //Limpiando contenido de forma
                    inicializaForma();
                    //Foco a primer control de captura
                    txtDescripcion.Focus();
                    break;
                case "Cancelar":

                    break;
                case "Eliminar":
                    deshabilitaUbicacion();
                    break;
                case "Bitacora":
                    //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  ClasificacionTipo.
                    string url = Cadena.RutaRelativaAAbsoluta("~/General/Ubicacion.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=15&idR=" + Session["id_registro"].ToString() + "&tB=" + "Ubicación");
                    //Variable que almacena la resolucion de la ventana bitacora
                    string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                    //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
                    ScriptServer.AbreNuevaVentana(url, "Bitacora Clasificacion Tipo", configuracion, Page);
                    //Cargando mapa
                    construyeScriptMapa("", "");
                    break;
                case "Referencias":
                    //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla ClasificacionTipo
                    string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/Ubicacion.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=15&idR=" + Session["id_registro"].ToString() + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                    //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla ClasificacionTipo
                    ScriptServer.AbreNuevaVentana(urlDestino, "Referencias del Registro Ubicación", 800, 500, false, false, false, true, true, Page);
                    //Cargando mapa
                    construyeScriptMapa("", "");
                    break;
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
                case "ControlPatios":
                    {
                        //Validando Estatus
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Nuevo:
                                {
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Debe de tener una Ubicación abierta."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    break;
                                }
                            case Pagina.Estatus.Lectura:
                            case Pagina.Estatus.Edicion:
                                {
                                    //Validando Ubicación
                                    using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                                    {
                                        if (ubi.habilitar && (ubi.id_tipo_ubicacion == 1 || ubi.id_tipo_ubicacion == 2))
                                        {
                                            //Inicializando Controles
                                            inicializaUbicacionPatio(ubi.id_ubicacion, ubi.id_compania_emisor);
                                            //Mostrando Ventana
                                            ScriptServer.AlternarVentana(this, "ControlPatios", "contenedorVentanaPatios", "ventanaControlPatios");
                                        }
                                        else
                                            //Instanciando Excepción
                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("La Ubicación no es de tipo 'Patio Cliente' ó 'Terminal Cobro'"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "AgregarCarga":
                    {
                        //Validando Estatus
                        switch((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Lectura:

                            case Pagina.Estatus.Edicion:
                                {
                                    //Validando Tipo de Ubicación
                                    if (ddlTipoUbicacion.SelectedValue.Equals("3"))
                                    {
                                        //Inicializa Valores de Carga
                                        inicializaValoresCarga();

                                        //Cargando Autotanques Anteriores
                                        cargaAutotanquesAnteriores();

                                        //Mostrando Ventana
                                        ScriptServer.AlternarVentana(this, "Carga Autotanque", "contenedorVentanaCargaAutotanque", "ventanaCargaAutotanque");
                                    }
                                    break;
                                }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Evento producido al pulsar el botón Limpiar Mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLimpiarMapa_Click(object sender, EventArgs e)
        {
            //Limpiando caja de texto con ubicación anterior
            txtGeoUbicacion.Text = "";
            //Borrando contenido 
            Session["geoubicacion"] = null;
            //Ejecutando función de limpieza, definida en javascript
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbLimpiarMapa, uplkbLimpiarMapa.GetType(), "limpiarMapa", "limpiaMapa();", true);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de localización en mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLocalizarMapa_Click(object sender, EventArgs e)
        {
            //Limpiando errores
            lblError.Text = "";

            //Determinando que se encuentren todos los datos requeridos para la búsqueda
            if (txtDireccion.Text != "" || txtCiudad.Text != "" || txtCodigoPostal.Text != "")
            {   //Instanciando Ciudad
                using (SAT_CL.Global.Ciudad ciu = new SAT_CL.Global.Ciudad(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudad.Text, "ID:", 1))))
                {//Construyendo url para consumo de Web Service de Geocodificación
                    string url = string.Format(@"{0}/xml?address={1}&key={2}", ConfigurationManager.AppSettings["GeocodingAPI_URL"],
                        txtDireccion.Text.ToUpper().Replace("#", "").Replace(" ","+")+(!string.IsNullOrEmpty(ciu.descripcion) ? "," + txtCodigoPostal.Text + " " + ciu.descripcion.ToUpper() : "").Replace("#", "").Replace(" ", "+")+
                        (!string.IsNullOrEmpty(ciu.estado) && !string.IsNullOrEmpty(ciu.pais) ? "," + ciu.estado.ToUpper() + "," + ciu.pais.ToUpper() : "").Replace("#", "").Replace(" ", "+"), 
                        ConfigurationManager.AppSettings["GeocodingAPI_SAT_DriverWebKey"]);
                    try
                    {
                        /** REST **/
                        RestClient cliente = new RestClient(url);
                        cliente.Timeout = -1;
                        RestRequest peticion = new RestRequest(Method.GET);
                        //Obteniendo Respuesta
                        IRestResponse respuesta = cliente.Execute(peticion);
                        //Creando XML con cadena devuelta
                        XDocument d = XDocument.Parse(respuesta.Content);

                        /** SOAP **/
                        ////Realizando petición web
                        //HttpWebRequest peticion = (HttpWebRequest)WebRequest.Create(url);
                        //HttpWebResponse respuesta = (HttpWebResponse)peticion.GetResponse();
                        //Creando XML con cadena devuelta
                        //XDocument d = XDocument.Load(respuesta.Content);

                        //Localizando el estatus de respuesta
                        string status = d.Element("GeocodeResponse").Element("status").Value;
                        //Si es correcto
                        if (status == "OK")
                        {
                            //Recuperando posición señalada
                            construyeScriptMapa(d.Element("GeocodeResponse").Element("result").Element("geometry").Element("location").Element("lat").Value,
                                                d.Element("GeocodeResponse").Element("result").Element("geometry").Element("location").Element("lng").Value);
                        }
                        //De lo contrario
                        else
                        {   
                            //Mostrando mensaje de error
                            lblError.Text = "No fue posible recuperar ninguna ubicación coincidente.";



                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(string.Format("{0} - {1}",
                                status, d.Element("GeocodeResponse").Element("error_message").Value)),
                                ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }

                    }
                    catch (Exception ex) { ex.ToString(); }
                }
            }
            //De lo contrario
            else
                //Mostrando mensaje de error
                lblError.Text = "Proporcione un dato adicional de búsqueda (Calle, número, colonia, municipio o C.P.).";
        }

        #region Eventos Carga AutoTanque

        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Cerrando Ventana
            ScriptServer.AlternarVentana(this, "Carga Autotanque", "contenedorVentanaCargaAutotanque", "ventanaCargaAutotanque");
        }
        /// <summary>
        /// Evento Producido al Guardar la Carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarCarga_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            DateTime fecha_carga = DateTime.MinValue;

            //Obteniendo Fecha
            DateTime.TryParse(txtFechaCarga.Text, out fecha_carga);
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Carga Activa Anterior
                using (CargaAutoTanque carga_anterior_activa = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(Convert.ToInt32(Session["id_registro"]), Fecha.ObtieneFechaEstandarMexicoCentro()))
                {
                    //Validando que exista la Carga
                    if (carga_anterior_activa.habilitar)

                        //Actualizando Estatus
                        result = carga_anterior_activa.ActualizaEstatusCargaActual(CargaAutoTanque.Estatus.Inactivo, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Retorno Positivo
                        result = new RetornoOperacion(0, "", true);


                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Insertando Carga
                        result = CargaAutoTanque.InsertarCargaAutoTanque(Convert.ToInt32(Session["id_registro"]), fecha_carga, Convert.ToDecimal(txtLitros.Text),
                                                    carga_anterior_activa.habilitar ? carga_anterior_activa.sobrante_carga_actual : Convert.ToDecimal(txtSobranteAnterior.Text),
                                                    Convert.ToDecimal(txtLitros.Text) + (carga_anterior_activa.habilitar ? carga_anterior_activa.sobrante_carga_actual : Convert.ToDecimal(txtSobranteAnterior.Text)),
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }

            //Validando Operación Exitosa
            if(result.OperacionExitosa)
            {
                //Inicializando Valores
                inicializaValoresCarga();
                
                //Cargando Autotanques
                cargaAutotanquesAnteriores();
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar la Carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCarga_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Evento Producido al Cambiar el Valor de los "Litros"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtLitros_TextChanged(object sender, EventArgs e)
        {
            //Instanciando Carga Anterior
            using (CargaAutoTanque carga_anterior = CargaAutoTanque.ObtieneCargaAnteriorUbicacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la carga Anterior
                if (carga_anterior.habilitar)

                    //Asignando Valor de la Carga Anterior
                    txtSobranteAnterior.Text = carga_anterior.sobrante_carga_actual.ToString();
                else
                    //Asignando Valor de la Carga Anterior
                    txtSobranteAnterior.Text = "0.00";
            }

            //Sumando Total Sobrante Actual
            txtSobranteActual.Text = (Convert.ToDecimal(txtLitros.Text) + Convert.ToDecimal(txtSobranteAnterior.Text)).ToString();
        }

        #region Eventos GridView "Cargas Anteriores"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarCarga_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvCargasAnteriores.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvCargasAnteriores, sender, "lnk", true);

                //Instanciando Carga
                using (CargaAutoTanque carga = new CargaAutoTanque(Convert.ToInt32(gvCargasAnteriores.SelectedDataKey["Id"])))
                {
                    //Validando que existe una carga
                    if (carga.habilitar)
                    {
                        //Deshabilitando Carga
                        result = carga.Deshabilitar(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Cargando Tanques
                            cargaAutotanquesAnteriores();

                            //Inicializando Valores
                            inicializaValoresCarga();
                        }

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCargas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño
            Controles.CambiaTamañoPaginaGridView(gvCargasAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoCargas.SelectedValue), true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarCargas_Click(object sender, EventArgs e)
        {
            //Exportando Cargas
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCargasAnteriores_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            Controles.CambiaSortExpressionGridView(gvCargasAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCargasAnteriores_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //Cambiando Paginación del GridView
            Controles.CambiaIndicePaginaGridView(gvCargasAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewSelectedIndex);
        }

        #endregion

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Habilitando controles
            habilitaControles();
            //Inicializando valores
            inicializaValores();
            //Habilita Menu
            habilitaMenus();
            //Cargando mapa
            construyeScriptMapa("", "");
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoUbicacion, "", 22);

            //Cargando Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3159);

            //Cargando tamaño del Grid
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCargas, "", 26);

            //Cargando Tipos de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoPatio, "", 3209);
        }
        /// <summary>
        /// Habilita Controles
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    txtDescripcion.Enabled =
                    ddlTipoUbicacion.Enabled =
                        //txtGeoUbicacion.Enabled =
                    txtDireccion.Enabled =
                    txtCiudad.Enabled =
                    txtCodigoPostal.Enabled =
                    txtTelefono.Enabled =
                    btnAceptar.Enabled =
                    btnCancelar.Enabled = true;

                    lkbLimpiarMapa.Enabled = true;
                    break;
                case Pagina.Estatus.Lectura:
                    txtDescripcion.Enabled =
                    ddlTipoUbicacion.Enabled =
                        //txtGeoUbicacion.Enabled =
                    txtDireccion.Enabled =
                    txtCiudad.Enabled =
                    txtCodigoPostal.Enabled =
                    txtTelefono.Enabled =
                     btnAceptar.Enabled =
                    btnCancelar.Enabled = false;

                    lkbLimpiarMapa.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    txtDescripcion.Enabled =
                    ddlTipoUbicacion.Enabled =
                        //txtGeoUbicacion.Enabled =
                    txtDireccion.Enabled =
                    txtCiudad.Enabled =
                    txtCodigoPostal.Enabled =
                    txtTelefono.Enabled =
                     btnAceptar.Enabled =
                    btnCancelar.Enabled = true;

                    lkbLimpiarMapa.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Metodo encargado de inicializar los valores de la forma en razon al perfil de usuario
        /// </summary>
        private void inicializaValores()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lblId.Text = "Por Asignar";
                        txtDescripcion.Text = "";
                        ddlTipoUbicacion.SelectedValue = "1";
                        txtGeoUbicacion.Text = "";
                        txtDireccion.Text = "";
                        txtCiudad.Text = "";
                        txtCodigoPostal.Text = "";
                        txtTelefono.Text = "";
                        Session["geoubicacion"] = null;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                        {
                            lblId.Text = objUbicacion.id_ubicacion.ToString();
                            txtDescripcion.Text = objUbicacion.descripcion;
                            ddlTipoUbicacion.SelectedValue = objUbicacion.id_tipo_ubicacion.ToString();
                            txtGeoUbicacion.Text = string.Format("{0}, {1}", objUbicacion.latitud, objUbicacion.longitud);
                            txtDireccion.Text = objUbicacion.direccion;
                            txtCiudad.Text = objUbicacion.ciudad + " ID:" + objUbicacion.id_ciudad.ToString();
                            txtCodigoPostal.Text = objUbicacion.codigo_postal;
                            txtTelefono.Text = objUbicacion.telefono;
                            Session["geoubicacion"] = objUbicacion.geoubicacion;
                        }
                        break;
                    }
            }

            //Limpiando errores
            lblError.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }

        /// <summary>
        /// Habilita o deshabilita menú de la forma
        /// </summary>
        private void habilitaMenus()
        {
            //Determinando el estatus de carga de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled = true;

                    lkbEditar.Enabled =
                    lkbEliminar.Enabled =
                    lkbImprimir.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = false;
                    break;
                case Pagina.Estatus.Lectura:
                    lkbNuevo.Enabled =
                    lkbImprimir.Enabled =
                    lkbEditar.Enabled =
                    lkbBitacora.Enabled = true;

                    lkbGuardar.Enabled =
                    lkbEliminar.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    lkbNuevo.Enabled =
                    lkbImprimir.Enabled =
                    lkbGuardar.Enabled =
                    lkbEliminar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = true;

                    lkbEditar.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Método encargado de guardar la Ubicación
        /// </summary>
        private void guardaUbicacion()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            /*
            //Validando que el objeto geografico está en sesión
            if (Session["geoubicacion"] != null)
            {*/
                //Recuperando objeto 
            SqlGeography geoubicacion = Session["geoubicacion"] != null ? (SqlGeography)Session["geoubicacion"] : SqlGeography.Point(0, 0, 4326);

                //En base al estatus de página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:

                        //Insertamos Ubicación
                        resultado = SAT_CL.Global.Ubicacion.InsertaUbicacion(txtDescripcion.Text.ToUpper(), (SAT_CL.Global.Ubicacion.Tipo)Convert.ToByte(ddlTipoUbicacion.SelectedValue), false,
                                                          ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, geoubicacion, txtDireccion.Text.ToUpper(), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudad.Text, "ID:", 1)),
                                                          txtCodigoPostal.Text, txtTelefono.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        break;
                    case Pagina.Estatus.Edicion:
                        //Instanciamos Ubicacion 
                        using (SAT_CL.Global.Ubicacion objUbicaion = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                        {

                            //Realizando la actualización de la Ubicación 
                            resultado = objUbicaion.EditaUbicacion(txtDescripcion.Text.ToUpper(), (SAT_CL.Global.Ubicacion.Tipo)Convert.ToByte(ddlTipoUbicacion.SelectedValue), false,
                                                      objUbicaion.id_compania_emisor, geoubicacion, txtDireccion.Text.ToUpper(), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudad.Text, "ID:", 1)),
                                                      txtCodigoPostal.Text, txtTelefono.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        break;
                }

                //SI no existen errores
                if (resultado.OperacionExitosa)
                {
                    //Asignando estatus de lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    //Asignamos Id de Registro
                    Session["id_registro"] = resultado.IdRegistro;
                    //Limpiando geoubicación
                    Session["geoubicacion"] = null;
                    //Inicialzaindo contenido de forma
                    inicializaForma();
                }
            /*
            }
            //Si no hay objeto geográfico
            else
                resultado = new RetornoOperacion("No se ha recuperado ningún trazo del mapa; Realice el trazado de la geoubicación antes de guardar.");
            */

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Deshabilita Ubicación
        /// </summary>
        private void deshabilitaUbicacion()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando registro actual
            using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))

                //Realizando deshabilitación
                resultado = objUbicacion.DeshabilitaUbicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no existe error
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus 
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asignando Id de Registro
                Session["id_registro"] = 0;
                //Inicializando el contenido de la página
                inicializaForma();
            }

            //Mostrando resultado de deshabilitación
            lblError.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Realiza el armado del script principal de visualización y manipulación de mapas de google
        /// </summary>
        /// <param name="lat">Latitud inicial del mapa</param>
        /// <param name="lng">Longitud inical del mapa</param>
        private void construyeScriptMapa(string lat, string lng)
        {            
            //Declarando variables auxiliares para creación de script
            string configuracionMapa = "", configuracionDrawingManager = "", geoUbicacion = "";
            double lt = 0, ln = 0;
            SqlGeography geo = new SqlGeography();

            //Determinando cual es el estatus actual de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    string zoom = "14";
                    //Si no hay posición inicial
                    if (lat == "" || lng == "")
                    {
                        //Asignando predeterminada
                        obtieneUltimaPosicion(out lt, out ln);
                        lat = lt.ToString();
                        lng = ln.ToString();
                        geo = SqlGeography.Point(lt, ln, 4326);
                    }
                    else
                        geo = SqlGeography.Point(Convert.ToDouble(lat), Convert.ToDouble(lng), 4326);

                    //Zoom sobre el DF, con centro en plaza de la constitución
                    configuracionMapa = creaScriptPosicionMapa(zoom, lat, lng);
                    //Asistente de Dibujo
                    configuracionDrawingManager = creaScriptAsistenteDibujo(true, geo);
                    //Geoubicación inical
                    //geoUbicacion = creaScriptGeoubicacionMapa(geo, true);
                    break;
                case Pagina.Estatus.Lectura:
                    //Instanciando ubicación
                    using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si no hay posición inicial
                        if (lat == "" || lng == "")
                        {
                            //Validando Tipo de Geometria
                            switch (u.geoubicacion.STGeometryType().Value)
                            {
                                case "LineString":
                                case "CompoundCurve":
                                case "Polygon":
                                case "CurvePolygon":
                                    {
                                        if (u.geoubicacion.EnvelopeCenter().Lat.Value != 0 && u.geoubicacion.EnvelopeCenter().Long.Value != 0)
                                        {
                                            lat = u.geoubicacion.EnvelopeCenter().Lat.Value.ToString();
                                            lng = u.geoubicacion.EnvelopeCenter().Long.Value.ToString();
                                            geo = u.geoubicacion;
                                        }
                                        else
                                        {
                                            //Asignando predeterminada
                                            obtieneUltimaPosicion(out lt, out ln);
                                            lat = lt.ToString();
                                            lng = ln.ToString();
                                            geo = SqlGeography.Point(lt, ln, 4326);
                                        }
                                        break;
                                    }
                                case "Point":
                                    {
                                        if (u.geoubicacion.Lat.Value != 0 && u.geoubicacion.Long.Value != 0)
                                        {
                                            //Asignando predeterminada
                                            lat = u.geoubicacion.Lat.Value.ToString();
                                            lng = u.geoubicacion.Long.Value.ToString();
                                            geo = u.geoubicacion;
                                        }
                                        else
                                        {
                                            //Asignando predeterminada
                                            obtieneUltimaPosicion(out lt, out ln);
                                            lat = lt.ToString();
                                            lng = ln.ToString();
                                            geo = SqlGeography.Point(lt, ln, 4326);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        //Asignando predeterminada
                                        obtieneUltimaPosicion(out lt, out ln);
                                        lat = lt.ToString();
                                        lng = ln.ToString();
                                        geo = SqlGeography.Point(lt, ln, 4326);
                                        break;
                                    }
                            }
                        }
                        else
                            geo = SqlGeography.Point(Convert.ToDouble(lat), Convert.ToDouble(lng), 4326);

                        //Zoom sobre el DF, con centro en plaza de la constitución
                        configuracionMapa = creaScriptPosicionMapa("14", lat, lng);
                        //Asistente de Dibujo
                        configuracionDrawingManager = creaScriptAsistenteDibujo(false, geo);
                        //Geoubicación inical
                        geoUbicacion = creaScriptGeoubicacionMapa(geo, false);
                    }
                    break;
                case Pagina.Estatus.Edicion:
                    //Instanciando ubicación
                    using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si no hay posición inicial
                        if (lat == "" || lng == "")
                        {
                            //Validando Tipo de Geometria
                            switch (u.geoubicacion.STGeometryType().Value)
                            {
                                case "LineString":
                                case "CompoundCurve":
                                case "Polygon":
                                case "CurvePolygon":
                                    {
                                        if (u.geoubicacion.EnvelopeCenter().Lat.Value != 0 && u.geoubicacion.EnvelopeCenter().Long.Value != 0)
                                        {
                                            lat = u.geoubicacion.EnvelopeCenter().Lat.Value.ToString();
                                            lng = u.geoubicacion.EnvelopeCenter().Long.Value.ToString();
                                            geo = u.geoubicacion;
                                        }
                                        else
                                        {
                                            //Asignando predeterminada
                                            obtieneUltimaPosicion(out lt, out ln);
                                            lat = lt.ToString();
                                            lng = ln.ToString();
                                            geo = SqlGeography.Point(lt, ln, 4326);
                                        }
                                        break;
                                    }
                                case "Point":
                                    {
                                        if (u.geoubicacion.Lat.Value != 0 && u.geoubicacion.Long.Value != 0)
                                        {
                                            //Asignando predeterminada
                                            lat = u.geoubicacion.Lat.Value.ToString();
                                            lng = u.geoubicacion.Long.Value.ToString();
                                            geo = u.geoubicacion;
                                        }
                                        else
                                        {
                                            //Asignando predeterminada
                                            obtieneUltimaPosicion(out lt, out ln);
                                            lat = lt.ToString();
                                            lng = ln.ToString();
                                            geo = SqlGeography.Point(lt, ln, 4326);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        //Asignando predeterminada
                                        obtieneUltimaPosicion(out lt, out ln);
                                        lat = lt.ToString();
                                        lng = ln.ToString();
                                        geo = SqlGeography.Point(lt, ln, 4326);
                                        break;
                                    }
                            }
                        }
                        else
                            geo = SqlGeography.Point(Convert.ToDouble(lat), Convert.ToDouble(lng), 4326);

                        //Zoom sobre el DF, con centro en plaza de la constitución
                        configuracionMapa = creaScriptPosicionMapa("16", lat, lng);
                        //Asistente de Dibujo
                        configuracionDrawingManager = creaScriptAsistenteDibujo(true, geo);
                        //Geoubicación inical
                        geoUbicacion = creaScriptGeoubicacionMapa(geo, true);
                    }
                    break;
            }
            //Definiendo variable de script principal (Se conforma de tres elementos dinámicos: Configuración de Mapa, Asistente de Dibujo y Geoubicación; Y un elemento fijo: Funciones de Guardado y borrado)
            string script =

            @"<script type='text/javascript'>
                //Variable de mapa
                var map;
                //Variable de Asistente de dibujo
                var drawingManager;
                //Variable para almacenar figura trazada
                var figura;
                //Variables para creación de nueva figura a partir de BD
                var coordenadasSuperficie;
                var superficie;
                //Indicador de borrado manual de usuario
                var limpiarMapa = false;
                //Declarando arreglo de coordenadas a enviar a método web de guardado en sesión
                var coordenadasSQL = [];

                //Función de inicialización de mapa
                function InicializaMapa() {
                    //Configuración General del Mapa
                    " + configuracionMapa + @"
                    //Creando mapa y aplicando configuración
                    map = new google.maps.Map(document.getElementById('mapa'), mapOptions);
                    //En caso de requerir la creación de un poligono o marcador, el código aparecerá aquí
                    " + geoUbicacion + @"
                    //Aplicando la configuración del asistente de dibujo
                    " + configuracionDrawingManager + @" 

                    //Añadiendo manejador de evento al terminar de dibujar poligono
                    google.maps.event.addListener(drawingManager, 'polygoncomplete', function (polygon) {                        
                        //Guardando figura trazada
                        figura = { tipo: 'polygon', ver: polygon };
                        //Añadiendo manejador de evento al añadir nuevo vertice (vertice intermedio)
                        google.maps.event.addListener(polygon.getPath(), 'insert_at', function (vertice) {
                            //Guardando figura trazada
                            figura = { tipo: 'polygon', ver: polygon };
                        });
                        //Añadiendo manejador de evento deshacer nuevo vertice (vertice intermedio)
                        google.maps.event.addListener(polygon.getPath(), 'remove_at', function (vertice) {
                            //Guardando figura trazada
                            figura = { tipo: 'polygon', ver: polygon };
                        });
                        //Añadiendo manejador de evento al mover un vertice ya definido a una nueva posición
                        google.maps.event.addListener(polygon.getPath(), 'set_at', function (vertice) {
                            //Guardando figura trazada
                            figura = { tipo: 'polygon', ver: polygon };
                        });

                        //Deshabilitando controles de asistente de dibujo
                        drawingManager.setOptions({
                            drawingMode: null,
                            drawingControl: false
                        });
                        //Asignando indicador de limpieza de mapa a falso
                        limpiarMapa = false;
                    });

                    //Añadiendo manejador de evento al terminar de dibujar marcador
                    google.maps.event.addListener(drawingManager, 'markercomplete', function (marker) {
                        //Guardando marcador indicado
                        figura = { tipo: 'marker', ver: marker };
                        //Añadiendo manejador de evento, al terminar arrastre de marcador
                        google.maps.event.addListener(marker, 'dragend', function (evt) {
                            //Guardando marcador indicado
                            figura = { tipo: 'marker', ver: marker };
                        });

                        //Deshabilitando controles de asistente de dibujo
                        drawingManager.setOptions({
                            drawingMode: null,
                            drawingControl: false
                        });
                    });
                }

                //Añadiendo manejador de evento Load de la página
                google.maps.event.addDomListener(window, 'load', InicializaMapa);

                //Función de guardado de puntos de mapa en sesión
                function guardaPuntos() {
                    //Declarando variable de resultado
                    var result = false;

                    //Validando existencia de figura
                    if (figura !== undefined && figura !== null) {
                        //Declarando variable contenedora de vertices
                        var vertices;
                        //En base al tipo de figura dibujada, se recuperan sus vertices
                        switch (figura['tipo']) {
                            case 'polygon':
                                //Obteniendo arreglo de vertices
                                vertices = figura['ver'].getPath().getArray();
                                break;
                            case 'marker':
                                //Creando arreglo con un solo vertice
                                vertices = [figura['ver'].position];
                                break;
                        }

                        //Construyendo Arreglo de Coordenadas para almacenamiento en sesión
                        coordenadasSQL = [];
                        for (var i = 0; i < vertices.length; i++) {
                            coordenadasSQL.push([vertices[i].lng(),vertices[i].lat()]);
                        }


                        //Invocando método web para guardado
                        PageMethods.GuardaPuntosSesion(coordenadasSQL, function (response) {
                            //Si el resultado es correcto
                        if(response){
                            //Limpiando superficies existentes
                            figura = null;
                            coordenadasSQL = [];
                        }

                        //Devolviendo resultado
                        return response;
                        }, function (error) { alert(error._message); }, this);                        
                    }
/*
                    else                  
                        alert('No se ha recuperado ningún trazo del mapa; Realice el trazado de la geoubicación antes de guardar.');
*/
                };                

                //Función de limpieza de contenido de mapa
                function limpiaMapa(){
                    //Validando existencia de figura
                    if (figura !== undefined && figura !== null) {
                            //Quitando objeto del mapa
                            figura['ver'].setMap(null);
                            //Borrando contenido almacenado
                            figura = null;
                            //Indicando que al recargar script de mapa, se debe omitir la carga de la geocerca existente
                            limpiarMapa = true;

                            //Mostrando mensaje de éxito
                            alert('Se ha borrado el contenido del mapa.');
                    }  
                };
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbNuevo, uplkbNuevo.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbGuardar, uplkbGuardar.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbEditar, uplkbEditar.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbEliminar, uplkbEliminar.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptar, upbtnAceptar.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnCancelar, upbtnCancelar.GetType(), "actualizacionMapa", script, false);
            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbLocalizarMapa, uplkbLocalizarMapa.GetType(), "actualizacionMapa", script, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        private void obtieneUltimaPosicion(out double lat, out double lng)
        {
            //Inicializando Datos
            lat = 0; lng = 0;

            //Validando Sessión
            if (Session["LastLocation"] != null)
            {
                //Obteniendo Posición de Sessión
                string point = Session["LastLocation"].ToString();
                if (!string.IsNullOrEmpty(point))
                {
                    double.TryParse(Cadena.RegresaCadenaSeparada(point, "|", 0, "0"), out lat);
                    double.TryParse(Cadena.RegresaCadenaSeparada(point, "|", 1, "0"), out lng);
                }
                else
                {
                    lat = 19.432351;
                    lng = -99.133641;
                }
            }
            else
            {
                lat = 19.432351;
                lng = -99.133641;
            }
        }
        /// <summary>
        /// Retorna el fragmento de código para un mapa sin contenido, sólo inicializado por una ubicación y zoom inicial
        /// </summary>
        /// <returns></returns>
        private string creaScriptPosicionMapa(string zoom, string lat, string lng)
        {
            return string.Format(@"
            var mapOptions = {{zoom:{0},
                    center: new google.maps.LatLng({1}, {2})
            }};", zoom, lat, lng);
        }
        /// <summary>
        /// Retorna el fragmento de código para un mapa sin contenido, sólo inicializado por una ubicación y zoom inicial
        /// </summary>
        /// <returns></returns>
        private string editaScriptPosicionMapa(string zoom, string lat, string lng)
        {
            return string.Format(@"
             mapOptions = {{zoom:{0},
                    center: new google.maps.LatLng({1}, {2})
            }};", zoom, lat, lng);
        }
        /// <summary>
        /// Retorna el fragmento de código para creación de una geoubicación dentro del mapa
        /// </summary>
        /// <param name="u">Objeto con los detalles de la ubicación a construir</param>
        /// <param name="edicion">True para indicar que puede ser editable una vez construida</param>
        /// <returns></returns>
        private string creaScriptGeoubicacionMapa(SqlGeography u, bool edicion)
        {
            //Inicializando cadena de retorno
            string geoUbicacion = "";

            //Recuperando puntos de la ubicación
            List<PointF> puntos = SAT_CL.Global.Ubicacion.RecuperaPuntosUbicacion(u);
            //Geoubicación ya registrada
            //Si es un poligono
            if (puntos.Count > 1)
            {
                geoUbicacion = @"
                            //Si no se ha solicitado limpiar mapa
                            if(!limpiarMapa){
                                //Declarando variable de coordenadas de superficie a representar           
                                coordenadasSuperficie = [ " + string.Join(",", (from PointF p in puntos
                                                                                select string.Format("new google.maps.LatLng({0}, {1})", p.X, p.Y))) + @"];

                                // Creando superficie, con los puntos señalados y sobre el mapa ya definido
                                superficie = new  google.maps.Polygon({
                                    paths: coordenadasSuperficie,
                                    map:map,
                                    editable: " + string.Format("{0}", edicion ? "true" : "false") + @",
                                    clickable: false,
                                    strokeOpacity: 0.7,
                                    strokeWeight: 3,
                                    fillOpacity: 0.3
                                });
                            
                                //Añadiendo superficie a variable 'figura', que será la misma que se pueda editar y que tendrá manejadores de evento asociados
                                figura = { tipo: 'polygon', ver: superficie };
                            }";
            }
            //Si es un marcador
            else
            {
                geoUbicacion = @"
                            //Si no se ha solicitado limpiar mapa
                            if(!limpiarMapa){
                                //Declarando variable de coordenadas de superficie a representar           
                                coordenadasSuperficie = " + string.Format("new google.maps.LatLng({0}, {1})", u.Lat, u.Long) + @"
                                //Creando superficie
                                superficie = new  google.maps.Marker({
                                position:coordenadasSuperficie,   
                                map:map,             
                                draggable: " + string.Format("{0}", edicion ? "true" : "false") + @"
                                });
                            
                                //Añadiendo superficie a variable 'figura', que será la misma que se pueda editar y que tendrá manejadores de evento asociados
                                figura = { tipo: 'marker', ver: superficie };
                            }";
            }

            //Devolviendo resultado
            return geoUbicacion;
        }
        /// <summary>
        /// Retorna el fragmento de código para creación del asistente de dibujo en el mapa
        /// </summary>
        /// <param name="edicion"></param>
        /// <param name="geoubicacion"></param>
        /// <returns></returns>
        private string creaScriptAsistenteDibujo(bool edicion, SqlGeography geoubicacion)
        {
            //Declarando cadena de retorno
            string asistente = "";

            asistente = @"
            //Si no se ha solicitado limpieza de mapa
            if(!limpiarMapa)
            {
                //Configurando y creando asistente de dibujo
                drawingManager = new google.maps.drawing.DrawingManager({" +
                                string.Format(@"{0}", edicion ? @"drawingModes: google.maps.drawing.OverlayType.POLYGON,
                                                                drawingControl: true," :
                                                                @"drawingModes: null,
                                                                drawingControl: false,") + @"
                    //Configuración de asistente
                    drawingControlOptions: {
                        //Ubicación
                        position: google.maps.ControlPosition.TOP_RIGHT,
                        //Herramientas disponibles al usuario
                        drawingModes: [
                            google.maps.drawing.OverlayType.MARKER,
                            google.maps.drawing.OverlayType.POLYGON
                            //google.maps.drawing.OverlayType.RECTANGLE, (Descartado, solo devuelve extremo noreste y suroeste)
                            //google.maps.drawing.OverlayType.CIRCLE,
                            //google.maps.drawing.OverlayType.POLYLINE                      
                        ]
                    },
                    //Propiedades específicas de marcadores
                    markerOptions: {
                        draggable: true,
                        clickable: false
                    },
                    //Propiedades específicas de polígonos
                    polygonOptions: {
                        editable: true,
                        clickable: false
                    }
                });
            }
            //Si hay que limpiar contenido, el asistente siempre estará disponible
            else
            {
                //Configurando y creando asistente de dibujo
                drawingManager = new google.maps.drawing.DrawingManager({drawingModes: google.maps.drawing.OverlayType.POLYGON,
                                                                drawingControl: true,
                    //Configuración de asistente
                    drawingControlOptions: {
                        //Ubicación
                        position: google.maps.ControlPosition.TOP_RIGHT,
                        //Herramientas disponibles al usuario
                        drawingModes: [
                            google.maps.drawing.OverlayType.MARKER,
                            google.maps.drawing.OverlayType.POLYGON
                            //google.maps.drawing.OverlayType.RECTANGLE, (Descartado, solo devuelve extremo noreste y suroeste)
                            //google.maps.drawing.OverlayType.CIRCLE,
                            //google.maps.drawing.OverlayType.POLYLINE                      
                        ]
                    },
                    //Propiedades específicas de marcadores
                    markerOptions: {
                        draggable: true,
                        clickable: false
                    },
                    //Propiedades específicas de polígonos
                    polygonOptions: {
                        editable: true,
                        clickable: false
                    }
                });
            }" + ((Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Nuevo ? @" 
            //Aplicando configuración de mapa
            map.set('zoom', 16);
            map.set('center', " + string.Format("new google.maps.LatLng({0}, {1})", geoubicacion.EnvelopeCenter().Lat.ToString(), geoubicacion.EnvelopeCenter().Long.ToString()) + @");" : "") + @"

            //Cargando asistente en mapa
            drawingManager.setMap(map);";

            //Devolviendo resultado
            return asistente;
        }

        /// <summary>
        /// Realiza el guardado de la geoubicación
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod(EnableSession=true)]
        public static bool GuardaPuntosSesion(object[] vertices)
        {
            //Declarando variable para guardado de resultado
            bool resultado = false;
            string geographySQL = "";
            //Determinando el tipo de figura a guardar
            //Si es un polígono
            if (vertices.Length > 1)
            {
                //Iniciando de cadena de poligono
                geographySQL = "POLYGON((";

                //Declarando variable de diccionario
                Dictionary<string, object> d;

                //Para cada vertice
                foreach (object[] o in vertices)
                {
                    //Añadiendo vertice a resultado
                    geographySQL += string.Format("{0} {1},", o[0], o[1]);
                }

                //Añadiendo vertice inicial a resultado
                geographySQL += string.Format("{0} {1}", ((object[])vertices[0])[0], ((object[])vertices[0])[1]);

                //Fin de cadena de poligono
                geographySQL += "))";
                try
                {
                    //Creando poligono desde texto
                    System.Data.SqlTypes.SqlChars cadenaSQL = new System.Data.SqlTypes.SqlChars(geographySQL.ToCharArray());
                    SqlGeography g = SqlGeography.STPolyFromText(cadenaSQL, 4326);
                    //Creando poligono invertido
                    SqlGeography gi = g.ReorientObject();
                    //Comparando las áreas de ambos (si el área de el poligono invertido es menor a la de poligono original (el poligono original fue creado en sentido de las manecillas de reloj, esto crea un WKT erroneo))
                    if (gi.STArea() < g.STArea())
                        //Indicando que el poligono invertido es el correcto
                        g = gi;
                    //Guardando en sesión el objeto creado
                    System.Web.HttpContext.Current.Session["geoubicacion"] = g;
                    //Indicando que la conversión y guardado han sido correctos
                    resultado = true;
                }
                catch (Exception e) { e.ToString(); }
            }
            //Si es un marcador
            else
            {
                //Inicio de cadena de punto
                geographySQL = "POINT(";

                
                //Añadiendo vertice a resultado
                geographySQL += string.Format("{0} {1}", ((object[])vertices[0])[0], ((object[])vertices[0])[1]);

                //Fin de cadens de punto
                geographySQL += ")";

                try
                {
                    //Creando poligono desde texto
                    System.Data.SqlTypes.SqlChars cadenaSQL = new System.Data.SqlTypes.SqlChars(geographySQL.ToCharArray());
                    SqlGeography g = SqlGeography.STPointFromText(cadenaSQL, 4326);
                    //Guardando en sesión el objeto creado
                    System.Web.HttpContext.Current.Session["geoubicacion"] = g;
                    //Indicando que la conversión y guardado han sido correctos
                    resultado = true;
                }
                catch (Exception e) { e.ToString(); }
            }            

            //Devolviendo resultado
            return resultado;
        }

        #region Métodos Carga Auto Tanque

        /// <summary>
        /// Método encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValoresCarga()
        {
            //Asignando Valores
            txtLitros.Text = "0.00";
            txtFechaCarga.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Instanciando Carga Anterior
            using (CargaAutoTanque carga_anterior = CargaAutoTanque.ObtieneCargaAnteriorUbicacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la carga Anterior
                if (carga_anterior.habilitar)

                    //Asignando Valor de la Carga Anterior
                    txtSobranteAnterior.Text = carga_anterior.sobrante_carga_actual.ToString();
                else
                    //Asignando Valor de la Carga Anterior
                    txtSobranteAnterior.Text = "0.00";
            }

            //Sumando Total Sobrante Actual
            txtSobranteActual.Text = (Convert.ToDecimal(txtLitros.Text) + Convert.ToDecimal(txtSobranteAnterior.Text)).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaAutotanquesAnteriores()
        {
            //Validando estatus
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                    
                case Pagina.Estatus.Edicion:
                    {
                        //Obteniendo Cargas Anteriores
                        using(DataTable dtCargasAnteriores = CargaAutoTanque.ObtieneCargasAnterioresUbicacion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que existan Datos
                            if (Validacion.ValidaOrigenDatos(dtCargasAnteriores))
                            {
                                //Cargando
                                Controles.CargaGridView(gvCargasAnteriores, dtCargasAnteriores, "Id", lblOrdenadoCargas.Text, true, 2);

                                //Añadiendo a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCargasAnteriores, "Table");
                            }
                            else
                            {
                                //Inicializando
                                Controles.InicializaGridview(gvCargasAnteriores);

                                //Añadiendo a Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }
                        break;
                    }
            }
        }

        #endregion

        #endregion

        #region Control Patios

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarUbicacionPatio_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Validando Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("Debe de tener una Ubicación abierta.");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Validando Ubicación
                        using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
                        {
                            if (ubi.habilitar)
                            {
                                if (ubi.id_tipo_ubicacion == 1 || ubi.id_tipo_ubicacion == 2)
                                {
                                    //Instanciando Ubicación del Patio
                                    using (SAT_CL.ControlPatio.UbicacionPatio patio = new SAT_CL.ControlPatio.UbicacionPatio(ubi.id_ubicacion, ubi.id_compania_emisor))
                                    {
                                        //Validando
                                        if (patio.habilitar)

                                            //Actualizando Ubicación
                                            retorno = patio.EditaUbicacionPatio(ubi.id_ubicacion, txtNombrePatio.Text, Convert.ToInt32(ddlTipoPatio.SelectedValue), ubi.id_compania_emisor,
                                                                Convert.ToInt32(txtTiempoPatio.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        else
                                            //Insertando Patio
                                            retorno = SAT_CL.ControlPatio.UbicacionPatio.InsertarUbicacionPatio(ubi.id_ubicacion, txtNombrePatio.Text, Convert.ToInt32(ddlTipoPatio.SelectedValue), 
                                                            ubi.id_compania_emisor, Convert.ToInt32(txtTiempoPatio.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando Operación
                                        if (retorno.OperacionExitosa)
                                            
                                            //Inicializando Controles
                                            inicializaUbicacionPatio(ubi.id_ubicacion, ubi.id_compania_emisor);
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion("La Ubicación no es de tipo 'Patio Cliente' ó 'Terminal Cobro'");
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("Debe de tener una Ubicación abierta.");
                        }

                        break;
                    }
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarUbicacionPatio_Click(object sender, EventArgs e)
        {
            //Validando Ubicación
            using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Inicializando Valores
                inicializaUbicacionPatio(ubi.id_ubicacion, ubi.id_compania_emisor);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarModal_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana
            ScriptServer.AlternarVentana(this.Page, "ControlPatios", "contenedorVentanaPatios", "ventanaControlPatios");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_compania"></param>
        private void inicializaUbicacionPatio(int id_ubicacion, int id_compania)
        {
            //Instanciando Ubicación del Patio
            using (SAT_CL.ControlPatio.UbicacionPatio patio = new SAT_CL.ControlPatio.UbicacionPatio(id_ubicacion, id_compania))
            {
                //Validando
                if (patio.habilitar)
                {
                    //Asignando Controles
                    lblUbicacion.Text = txtDescripcion.Text;
                    txtNombrePatio.Text = patio.nombre_corto;
                    ddlTipoPatio.SelectedValue = patio.id_tipo_patio.ToString();
                    txtTiempoPatio.Text = patio.tiempo_limite.ToString();
                    //Configurando control
                    btnGuardarUbicacionPatio.Text = "Actualizar";
                }
                else
                {
                    //Asignando Controles
                    lblUbicacion.Text = txtDescripcion.Text;
                    txtNombrePatio.Text = "";
                    ddlTipoPatio.SelectedValue = "1";
                    txtTiempoPatio.Text = "0";
                    //Configurando control
                    btnGuardarUbicacionPatio.Text = "Insertar";
                }
            }
        }
        

        #endregion
    }
}