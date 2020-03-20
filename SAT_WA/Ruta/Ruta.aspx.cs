using SAT_CL;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.Ruta
{
    public partial class Ruta : System.Web.UI.Page
    {
        /// <summary>
        /// Contenedor de la Forma
        /// </summary>
        private string Contenedor;
        /// <summary>
        /// Evento generado al Cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recraga de página
            if (!this.IsPostBack)
                inicializaForma();
        }

        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Determinando la pestaña pulsada
            switch (((Button)sender).CommandName)
            {
                case "Casetas":
                    //Cambiando estilos de pestañas
                    btnPestanaCasetas.CssClass = "boton_pestana_activo";
                    btnPestanaConcepto.CssClass =
                    btnPestanaDiesel1.CssClass =
                    btnPestanaDiesel.CssClass = "boton_pestana";
                    //Asignando vista activa de la forma
                    mtvRuta.SetActiveView(vwCasetas);
                    //Validamos Estatus de la Forma
                    if ((Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Copia)
                    {
                        //Cargamos Casetas
                        cargaCasetas();
                    }
                    break;
                case "Diesel":
                    //Cambiando estilos de pestañas
                    btnPestanaConcepto.CssClass =
                    btnPestanaCasetas.CssClass =
                    btnPestanaDiesel1.CssClass = "boton_pestana";
                    btnPestanaDiesel.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvRuta.SetActiveView(vwDiesel);
                    //Validamos Estatus de la Forma
                    if ((Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Copia)
                    {
                        //Cargamos Tipo Unidad
                        cargaTipoUnidad();
                        //Inicializamos Grid
                        Controles.InicializaGridview(gvVales);
                        //Inicializamos Indices
                        Controles.InicializaIndices(gvTipoUnidad);
                    }
                    break;
                case "Diesel1":
                    //Cambiando estilos de pestañas
                    btnPestanaConcepto.CssClass =
                    btnPestanaDiesel.CssClass =
                    btnPestanaCasetas.CssClass = "boton_pestana";
                    btnPestanaDiesel1.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvRuta.SetActiveView(vwDiesel1);
                    //Validamos Estatus de la Forma
                    if ((Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Copia)
                    {
                        //Cargamos Tipo Unidad
                        cargaTipoUnidad();
                        //Inicializamos Grid
                        Controles.InicializaGridview(gvVale);
                        //Inicializamos Indices
                        Controles.InicializaIndices(gvTiposUnidad);
                    }
                    break;
                case "Concepto":
                    //Cambiando estilos de pestañas
                    btnPestanaDiesel.CssClass =
                    btnPestanaCasetas.CssClass = "boton_pestana";
                    btnPestanaConcepto.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvRuta.SetActiveView(vwConcepto);
                    //Validamos Estatus de la Forma
                    if ((Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Copia)
                    {

                        //Cargamos Conceptos
                        cargaConcepto();
                        //Inicializamos Grid
                        Controles.InicializaIndices(gvConcepto);
                    }
                    break;
            }
        }

        /// <summary>
        /// Evento disparado al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(175, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        guardaRuta(lkbGuardar);
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "175", "Ruta");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "175", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Copiar":
                    //Establecemos Estatus Forma
                    Session["estatus"] = Pagina.Estatus.Copia;
                    //Inicializamos Forma
                    inicializaForma();
                    Session["id_registro"] = 0;
                    break;
                case "Eliminar":
                    {
                        //Deshabilita Ruta
                        deshabilitaRuta();
                        break;
                    }
            }
        }


        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma (cargando catalogos, contenido y asignando habilitación de controles)
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catálogos
            cargaCatalogosRuta();
            //Cargando contenido de controles
            inicializaControlesRuta();
            //Habilitando controles
            habilitaControlesRuta();
            //Habilitando elementos del menú
            habilitaMenu();
            //Carga Autocomplete
            inicializaAutocompleteUbicacionOrigen(this.Page);
            inicializaAutocompleteUbicacionDestino(this.Page);
            //Inicializamos Controles de Casetas
            inicializaControlesCaseta();
            //Carga Catalogo de las Casetas
            cargaCatalogosCaseta();
            //Habilitamos Controles
            habilitaControlesCasetas();
            //Craga Catalogos Diesel
            cargaCatalogosDiesel();
            //Inicializamos Controles Tipo Unidad
            inicializaControlesTipoUnidad();
            //Habilita Controles Tipo Unidad
            habilitaControlesTipoUnidad();
            //Cargamos Concepto
            inicializaControlesConcepto();
            //Carga Catalogos
            cargaCatalogosConcepto();
            //Actualiza autocomplete
            cargaOpcionesDestino();
            cargaOpcionesOrigen();

        }

        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbCopiar.Enabled =
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        //Edicion
                        lkbCopiar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled =
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
            }
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = string.Format("{0}?P1={1}", Cadena.RutaRelativaAAbsoluta("~/Ruta/Ruta.aspx", "~/Accesorios/AbrirRegistro.aspx"), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Ruta/Ruta.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        #endregion
        #region Métodos Rutas
        /// <summary>
        /// Inicializa el contenido de los controles  de la Ruta
        /// </summary>
        private void inicializaControlesRuta()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Inicializamos Controles
                    txtAliasRuta.Text = "";
                    txtDescripcion.Text = "";
                    txtCliente.Text = "";
                    ddlTipoOrigen.SelectedValue = "15";
                    txtOrigen.Text = "";
                    ddlTipoDestino.SelectedValue = "15";
                    txtDestino.Text = "";
                    txtKilometros.Text = "";
                    chkPermisionario.Checked = false;
                    lblCopia.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando registro de Ruta
                    using (SAT_CL.Ruta.Ruta objRuta = new SAT_CL.Ruta.Ruta(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (objRuta.habilitar)
                        {
                            //Inicializamos Controles
                            txtAliasRuta.Text = objRuta.alias_ruta;
                            txtDescripcion.Text = objRuta.descripcion;
                            //Instanciamos Cliente
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new SAT_CL.Global.CompaniaEmisorReceptor(objRuta.id_cliente))
                            {
                                string nombre = objCompania.id_compania_emisor_receptor != 0 ? objCompania.nombre : "TODOS";
                                txtCliente.Text = nombre + " ID:" + objCompania.id_compania_emisor_receptor.ToString();

                            }
                            //De acuerdo  a la Tabla Ubicación
                            if (objRuta.id_tabla_origen == 15)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objRuta.id_registro_origen))
                                {
                                    //Asignamos la Descripción
                                    string origen = objUbicacion.id_ubicacion != 0 ? objUbicacion.descripcion : "TODOS";
                                    txtOrigen.Text = origen + " ID:" + objUbicacion.id_ubicacion.ToString();
                                }
                            }
                            //Ciudad
                            else if (objRuta.id_tabla_origen == 54)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ciudad objCiudad = new SAT_CL.Global.Ciudad(objRuta.id_registro_origen))
                                {
                                    //Asignamos la Descripción
                                    string origen = objCiudad.id_ciudad != 0 ? objCiudad.descripcion : "TODOS";
                                    txtOrigen.Text = origen + " ID:" + objCiudad.id_ciudad.ToString();
                                }
                            }
                            ddlTipoOrigen.SelectedValue = objRuta.id_tabla_origen.ToString();
                            ddlTipoAplicacion.SelectedValue = objRuta.tipo_aplicacion.ToString();
                            //De acuerdo  a la Tabla Ubicación
                            if (objRuta.id_tabla_destino == 15)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objRuta.id_registro_destino))
                                {
                                    //Asignamos la Descripción
                                    string destino = objUbicacion.id_ubicacion != 0 ? objUbicacion.descripcion : "TODOS";
                                    txtDestino.Text = destino + " ID:" + objUbicacion.id_ubicacion.ToString();
                                }
                            }
                            //Ciudad
                            else if (objRuta.id_tabla_destino == 54)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ciudad objCiudad = new SAT_CL.Global.Ciudad(objRuta.id_registro_destino))
                                {
                                    //Asignamos la Descripción
                                    string destino = objCiudad.id_ciudad != 0 ? objCiudad.descripcion : "TODOS";
                                    txtDestino.Text = destino + " ID:" + objCiudad.id_ciudad.ToString();
                                }
                            }
                            ddlTipoDestino.SelectedValue = objRuta.id_tabla_destino.ToString();
                            txtKilometros.Text = objRuta.kilometraje.ToString();
                            chkPermisionario.Checked = objRuta.bit_permisionario;
                        }
                    }
                    lblCopia.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Copia:
                    //Instanciando registro de Ruta
                    using (SAT_CL.Ruta.Ruta objRuta = new SAT_CL.Ruta.Ruta(Convert.ToInt32(Session["id_registro"])))
                    {

                        //Si el registro existe
                        if (objRuta.habilitar)
                        {
                            //Inicializamos Controles
                            txtAliasRuta.Text = "";
                            txtDescripcion.Text = "";
                            //Instanciamos Cliente
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new SAT_CL.Global.CompaniaEmisorReceptor(objRuta.id_cliente))
                            {
                                txtCliente.Text = objCompania.nombre + " ID:" + objCompania.id_compania_emisor_receptor.ToString();
                            }
                            //De acuerdo  a la Tabla Ubicación
                            if (objRuta.id_tabla_origen == 15)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objRuta.id_registro_origen))
                                {
                                    //Asignamos la Descripción
                                    txtOrigen.Text = objUbicacion.descripcion + " ID:" + objUbicacion.id_ubicacion.ToString();
                                }
                            }
                            //Ciudad
                            else if (objRuta.id_tabla_origen == 54)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ciudad objCiudad = new SAT_CL.Global.Ciudad(objRuta.id_registro_origen))
                                {
                                    //Asignamos la Descripción
                                    txtOrigen.Text = objCiudad.descripcion + " ID:" + objCiudad.id_ciudad.ToString();
                                }
                            }
                            ddlTipoOrigen.SelectedValue = objRuta.id_tabla_origen.ToString();
                            ddlTipoAplicacion.SelectedValue = objRuta.tipo_aplicacion.ToString();
                            //De acuerdo  a la Tabla Ubicación
                            if (objRuta.id_tabla_destino == 15)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objRuta.id_registro_destino))
                                {
                                    //Asignamos la Descripción
                                    txtDestino.Text = objUbicacion.descripcion + " ID:" + objUbicacion.id_ubicacion.ToString();
                                }
                            }
                            //Ciudad
                            else if (objRuta.id_tabla_destino == 54)
                            {
                                //Instanciamos Ubicación
                                using (SAT_CL.Global.Ciudad objCiudad = new SAT_CL.Global.Ciudad(objRuta.id_registro_destino))
                                {
                                    //Asignamos la Descripción
                                    txtDestino.Text = objCiudad.descripcion + " ID:" + objCiudad.id_ciudad.ToString();
                                }
                            }
                            ddlTipoDestino.SelectedValue = objRuta.id_tabla_destino.ToString();
                            txtKilometros.Text = objRuta.kilometraje.ToString();
                            chkPermisionario.Checked = objRuta.bit_permisionario;
                        }
                    }
                    lblCopia.Text = "Copia";
                    break;
            }
        }

        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosRuta()
        {
            //Tipo Tabla Origen
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOrigen, "", 3162);
            //Tipo Tabla tipo aplicacion
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAplicacion, "--Selecciona Opción--", 3211);
            //Tipo Tabla Destino
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoDestino, "", 3162);
            
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoDeposito, "", 3210);
        }
        /// <summary>
        /// Método encargado de iniciar autocomplete segun el tipo de origen
        /// </summary>
        private void cargaOpcionesOrigen()
        {
            //Inicializamos Valor 
            //txtOrigen.Text = "";
            //De acueero al Tipo de Tbla Ubicación
            if (ddlTipoOrigen.SelectedValue == "15")
            {
                //Inicializamos Autocomplete de Ubicación
                inicializaAutocompleteUbicacionOrigen(ddlTipoOrigen);
            }
            //Ciudad
            else if (ddlTipoOrigen.SelectedValue == "54")
            {
                //Inicializamos Autocomplete de Ciudad
                inicializaAutocompleteCiudadOrigen(ddlTipoOrigen);
            }
        }
        /// <summary>
        /// Método encargado de iniciar autocomplete segun el tipo de destino
        /// </summary>
        private void cargaOpcionesDestino()
        {
            //Inicializamos Valor 
            //txtDestino.Text = "";
            //De acueero al Tipo de Tbla Ubicación
            if (ddlTipoDestino.SelectedValue == "15")
            {
                //Inicializamos Autocomplete de Ubicación
                inicializaAutocompleteUbicacionDestino(ddlTipoDestino);
            }
            //Ciudad
            else if (ddlTipoDestino.SelectedValue == "54")
            {
                //Inicializamos Autocomplete de Ciudad
                inicializaAutocompleteCiudadDestino(ddlTipoDestino);
            }
        }
        /// <summary>
        /// Habilita o deshabilita los controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControlesRuta()
        {
            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    txtAliasRuta.Enabled =
                    txtDescripcion.Enabled =
                    txtCliente.Enabled =
                    ddlTipoOrigen.Enabled =
                    ddlTipoAplicacion.Enabled =
                    ddlTipoDeposito.Enabled =
                    txtOrigen.Enabled =
                    ddlTipoDestino.Enabled =
                    txtDestino.Enabled =
                    txtKilometros.Enabled =
                    chkPermisionario.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Copia:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtAliasRuta.Enabled =
                    txtDescripcion.Enabled =
                    txtCliente.Enabled =
                    ddlTipoOrigen.Enabled =
                    ddlTipoAplicacion.Enabled =
                    ddlTipoDeposito.Enabled =
                    txtOrigen.Enabled =
                    ddlTipoDestino.Enabled =
                    txtDestino.Enabled =
                    txtKilometros.Enabled =
                    chkPermisionario.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;
                    break;
            }
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Ubicacion de Origen
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteUbicacionOrigen(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtOrigen.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteUbicacionOrigen", script, false);
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Ubicacion de Destino
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteUbicacionDestino(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtDestino.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteUbicacionDestino", script, false);
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Ciudad de Origen
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteCiudadOrigen(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtOrigen.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=53&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteCiudadOrigen", script, false);
        }

        /// <summary>
        ///  Inicializamos Autocomplete de Ciudad de Destino
        /// </summary>
        /// <param name="control"></param>
        private void inicializaAutocompleteCiudadDestino(System.Web.UI.Control control)
        {
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtDestino.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=53&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteCiudadnDestino", script, false);
        }

        /// <summary>
        /// Método encargado de guardar la Ruta
        /// </summary>
        /// <param name="control">Control que Dispara la Acción</param>
        private void guardaRuta(System.Web.UI.Control control)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            string descripcion = txtDescripcion.Text;
            int id_ruta = 0;

            //En base al estatus de página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Copia:

                    //Insertamos Ruta
                    resultado = SAT_CL.Ruta.Ruta.InsertarRuta(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, ':', 1)), descripcion.ToUpper(), txtAliasRuta.Text.ToUpper(), Convert.ToInt32(ddlTipoAplicacion.SelectedValue),
                        Convert.ToInt32(ddlTipoOrigen.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOrigen.Text, ':', 1)), Convert.ToInt32(ddlTipoDestino.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDestino.Text, ':', 1)),
                                                      Convert.ToDecimal(txtKilometros.Text), chkPermisionario.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    //Asignamos Valor a la Ruta
                    id_ruta = resultado.IdRegistro;
                    break;
                case Pagina.Estatus.Edicion:
                    //Editamos Ruta
                    using (SAT_CL.Ruta.Ruta objRuta = new SAT_CL.Ruta.Ruta(Convert.ToInt32(Session["id_registro"])))
                    {

                        //Realizando la actualización de la Ubicación 
                        resultado = objRuta.EditarRuta(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, ':', 1)), descripcion.ToUpper(), txtAliasRuta.Text.ToUpper(), Convert.ToInt32(ddlTipoAplicacion.SelectedValue),
                        Convert.ToInt32(ddlTipoOrigen.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOrigen.Text, ':', 1)), Convert.ToInt32(ddlTipoDestino.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDestino.Text, ':', 1)),
                                                      Convert.ToDecimal(txtKilometros.Text), chkPermisionario.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    }
                    break;
            }

            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //En base al estatus de página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        //Asignando estatus de lectura
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Asignamos Id de Registro
                        Session["id_registro"] = resultado.IdRegistro;
                        //Inicialzaindo contenido de forma
                        inicializaForma();
                        break;
                    case Pagina.Estatus.Edicion:
                        //Asignando estatus de lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        //Asignamos Id de Registro
                        Session["id_registro"] = resultado.IdRegistro;
                        //Inicialzaindo contenido de forma
                        inicializaForma();
                        break;

                    case Pagina.Estatus.Copia:
                        //Copiamos Ruta
                        resultado = SAT_CL.Ruta.Ruta.CopiaRuta(resultado.IdRegistro, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"),
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Asignando estatus de lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        //Asignamos Id de Registro
                        Session["id_registro"] = id_ruta;
                        //Inicialzaindo contenido de forma
                        inicializaForma();
                        break;
                }
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de guardar de Deshabilitar una Ruta
        /// </summary>
        private void deshabilitaRuta()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Ruta
            using (SAT_CL.Ruta.Ruta objRuta = new SAT_CL.Ruta.Ruta(Convert.ToInt32(Session["id_registro"])))
            {
                //Deshabilitamos Tipo Unidad
                resultado = objRuta.Deshabilitar(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de lectura
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asignamos Id de Registro
                Session["id_registro"] = 0;
                //Inicialzaindo contenido de forma
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(lkbEliminar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        #endregion

        #region Eventos Rutas
        /// <summary>
        /// Evento generado al guardar la Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guardamos Ruta
            guardaRuta(btnGuardar);
        }

        /// <summary>
        /// Evento Generado al Cancelar la Edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Asignando estatus nuevo
            Session["estatus"] = Pagina.Estatus.Lectura;
            //Limpiando contenido de forma
            inicializaForma();
        }

        #endregion
        #region Eventos Casetas
        /// <summary>
        /// Evento corting de gridview de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCasetas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvCasetas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").DefaultView.Sort = lblOrdenadoCasetas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoCasetas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCasetas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCasetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCasetas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCasetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCasetas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoCasetas.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarCasetas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
        }

        /// <summary>
        /// Evento generado al Agregar una Caseta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbAgregarCaseta_Click(object sender, EventArgs e)
        {
            //Guardamos Caseta
            guardaCaseta();
        }

        /// <summary>
        /// Bitacora de Caseta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacoraCaseta_Click(object sender, EventArgs e)
        {
            //Invocando Método de Inicializacion de Bitacora
            inicializaBitacora(gvCasetas.SelectedValue.ToString(), "172", "Casetas");
        }

        /// <summary>
        /// Evento Generado al Cambiar la Selección de Origen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Inicializamos Valor 
            txtOrigen.Text = "";
            ////De acueero al Tipo de Tbla Ubicación
            //if (ddlTipoOrigen.SelectedValue == "15")
            //{
            //    //Inicializamos Autocomplete de Ubicación
            //    inicializaAutocompleteUbicacionOrigen(ddlTipoOrigen);
            //}
            ////Ciudad
            //else if (ddlTipoOrigen.SelectedValue == "54")
            //{
            //    //Inicializamos Autocomplete de Ciudad
            //    inicializaAutocompleteCiudadOrigen(ddlTipoOrigen);
            //}
            cargaOpcionesOrigen();
        }

        /// <summary>
        /// Evento generado al Cambiar a la Selección Destino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Inicializamos Valor 
            txtDestino.Text = "";
            ////De acueero al Tipo de Tbla Ubicación
            //if (ddlTipoDestino.SelectedValue == "15")
            //{
            //    //Inicializamos Autocomplete de Ubicación
            //    inicializaAutocompleteUbicacionDestino(ddlTipoDestino);
            //}
            ////Ciudad
            //else if (ddlTipoDestino.SelectedValue == "54")
            //{
            //    //Inicializamos Autocomplete de Ciudad
            //    inicializaAutocompleteCiudadDestino(ddlTipoDestino);
            //}
            cargaOpcionesDestino();
        }

        /// <summary>
        /// Evento generado al Deshabilitar una Caseta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDeshabilitar_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvCasetas.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvCasetas, sender, "lnk", false);

                //Deshabilitamos las Caseta
                deshabilitaCaseta();
            }
        }
        #endregion

        #region Métodos Casetas

        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosCaseta()
        {
            //Tamaño de la Caseta
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCasetas, "", 56);

        }

        /// <summary>
        /// Método encargado de guardar la Caseta
        /// </summary>
        private void guardaCaseta()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Insertamos Ruta
            resultado = SAT_CL.Ruta.RutaCaseta.InsertarRutaCaseta(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCaseta.Text, ':', 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, Convert.ToByte(ddlTipoDeposito.SelectedValue));


            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Controles
                inicializaControlesCaseta();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(imbAgregarCaseta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de deshabilitar la Caseta
        /// </summary>
        private void deshabilitaCaseta()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Caseta
            using (SAT_CL.Ruta.RutaCaseta objCaseta = new SAT_CL.Ruta.RutaCaseta(Convert.ToInt32(gvCasetas.SelectedValue)))
            {
                //Insertamos Ruta
                resultado = objCaseta.DeshabilitarRutaCaseta(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Controles
                inicializaControlesCaseta();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvCasetas, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Inicializa el contenido de los controles  de la Ruta
        /// </summary>
        private void inicializaControlesCaseta()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Inicializamos Controles
                    txtCaseta.Text = "";
                    //Inicializa Grid View
                    Controles.InicializaGridview(gvCasetas);
                    break;
                case Pagina.Estatus.Copia:
                    //Inicializamos Controles
                    txtCaseta.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //cargamos Casetas
                    cargaCasetas();
                    txtCaseta.Text = "";
                    break;
            }
        }

        /// <summary>
        /// Habilita o deshabilita los controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControlesCasetas()
        {
            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Copia:
                    txtCaseta.Enabled =
                    ddlTipoDeposito.Enabled=
                    imbAgregarCaseta.Enabled == false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtCaseta.Enabled =
                    imbAgregarCaseta.Enabled == true;
                    break;
            }
        }

        /// <summary>
        /// Método encargado de cargar las Casetas
        /// </summary>
        private void cargaCasetas()
        {
            //Obtenemos Depósito
            using (DataTable mit = SAT_CL.Ruta.RutaCaseta.CargaCasetas(Convert.ToInt32(Session["id_registro"])))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvCasetas, mit, "Id", lblOrdenadoCasetas.Text, true, 3);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");


                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvCasetas);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
                //Suma Totales
                sumaTotalesCasetas();
            }
        }

        /// <summary>
        /// Método encargado de Sumar los Totales de las Casetas
        /// </summary>
        private void sumaTotalesCasetas()
        {
            //Validando que existe la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Calculamos Totales
                gvCasetas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C1)", "")));
                gvCasetas.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C2)", "")));
                gvCasetas.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C3)", "")));
                gvCasetas.FooterRow.Cells[7].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C4)", "")));
                gvCasetas.FooterRow.Cells[8].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C5)", "")));
                gvCasetas.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C6)", "")));
                gvCasetas.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C7)", "")));
                gvCasetas.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C8)", "")));
                gvCasetas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(C9)", "")));
            }
            else
            {
                //Calculamos Totales
                gvCasetas.FooterRow.Cells[4].Text =
                gvCasetas.FooterRow.Cells[5].Text =
                gvCasetas.FooterRow.Cells[6].Text =
                gvCasetas.FooterRow.Cells[7].Text =
                gvCasetas.FooterRow.Cells[8].Text =
                gvCasetas.FooterRow.Cells[9].Text =
                gvCasetas.FooterRow.Cells[10].Text =
                gvCasetas.FooterRow.Cells[11].Text = 
                gvCasetas.FooterRow.Cells[12].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion


        #region Métodos Diesel
        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosDiesel()
        {
            //Tamaño de Vales
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVales, "", 56);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVale, "", 56);
            //Tipos de Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 183, "", 0, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTiposUnidad, 183, "", 0, "", 0, "");
            //Configuracion
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConfiguracion, "Ninguna", 3163);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlConfiguraciones, "Ninguna", 3163);
        }


        /// <summary>
        /// Método encargado de guardar el Tipo Unidad
        /// </summary>
        private void guardaTipoUnidad()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Insertamos Ruta
            resultado = SAT_CL.Ruta.RutaTipoUnidad.InsertarRutaTipoUnidad(Convert.ToInt32(Session["id_registro"]), Convert.ToByte(ddlTipoUnidad.SelectedValue),
                        Convert.ToByte(ddlConfiguracion.SelectedValue), 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Controles
                inicializaControlesTipoUnidad();
                //inicializamos Grid View de Vales
                Controles.InicializaGridview(gvVales);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarTipoUnidad, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de guardar el Tipo Unidad
        /// </summary>
        private void guardaTiposUnidad()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Insertamos Ruta
            resultado = SAT_CL.Ruta.RutaTipoUnidad.InsertarRutaTipoUnidad(Convert.ToInt32(Session["id_registro"]), Convert.ToByte(ddlTiposUnidad.SelectedValue),
                        Convert.ToByte(ddlConfiguraciones.SelectedValue), 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Controles
                inicializaControlesTipoUnidad();
                cargaTipoUnidad();
                //inicializamos Grid View de Vales
                //Controles.InicializaGridview(gvVale);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarTiposUnidad, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de guardar el Vale
        /// </summary>
        private void guardaVale()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            int id_tabla = 0;
            //Validamos Selección de Tipo de Unidad
            if (gvTiposUnidad.SelectedIndex != -1)
            {

                //Recuperando controles 
                //using (TextBox txtlitros = (TextBox)gvVales.FooterRow.FindControl("txtlitros"))
                //{
                using (DropDownList ddlTiposOperacion = (DropDownList)gvVale.FooterRow.FindControl("ddlTiposOperacion"))
                using (TextBox txtEstaciones = (TextBox)gvVale.FooterRow.FindControl("txtEstaciones"),
                               txtProveedores = (TextBox)gvVale.FooterRow.FindControl("txtProveedores"))
                {
                    //Validamos Estación Efectivo
                    if (ddlTiposOperacion.SelectedValue == "1" || ddlTiposOperacion.SelectedValue == "2")
                    {
                        //Asignando tabla de ubicaciones
                        id_tabla = 15;
                    }
                    else if (ddlTiposOperacion.SelectedValue == "3")
                    {
                        //Asignando tabla de proveedores
                        id_tabla = 25;
                    }
                    if (txtEstaciones.Text == " " || txtProveedores.Text == " ")
                    {
                        //Establecemos Resultado
                        resultado = new RetornoOperacion("La Estación de Combustible es obligatoria.");
                    }

                    //Validamos Mensaje Error
                    if (resultado.OperacionExitosa)
                    {
                        //Insertamos Vale
                        resultado = SAT_CL.Ruta.RutaUnidadDiesel.InsertarRutaUnidadDiesel(Convert.ToInt32(gvTiposUnidad.SelectedValue),
                                  (SAT_CL.Ruta.RutaUnidadDiesel.TipoOperacion)Convert.ToByte(ddlTiposOperacion.SelectedValue), id_tabla, 
                                  txtEstaciones.Visible == true ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEstaciones.Text, " ID:", 1)) : Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedores.Text, " ID:", 1)), 
                                  ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    }
                }
                //}
            }
            else
            {
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("Seleccione el Tipo de Unidad");
            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Vales
                cargaVales();
                //Inicializamos Indices
                Controles.InicializaIndices(gvVale);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvVale, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        /// <summary>
        /// Metodo encargado de Editar el Vale
        /// </summary>
        private void editaVale()
        {
            //Declaracion de objeto resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            int id_tabla = 0;
            //using (TextBox txtLitrosE = (TextBox)gvVales.SelectedRow.FindControl("txtlitrosE"))
            //{
            using (DropDownList ddlTiposOperacionE = (DropDownList)gvVale.SelectedRow.FindControl("ddlTiposOperacionE"))
            using (TextBox txtEstacionesE = (TextBox)gvVale.SelectedRow.FindControl("txtEstacionesE"),
                           txtProveedoresE = (TextBox)gvVale.SelectedRow.FindControl("txtProveedoresE"))
            //Editamos Vale
            using (SAT_CL.Ruta.RutaUnidadDiesel objVale = new SAT_CL.Ruta.RutaUnidadDiesel(Convert.ToInt32(gvVale.SelectedDataKey.Value)))
            {
                //Validamos Estación Efectivo
                if (ddlTiposOperacionE.SelectedValue == "1" || ddlTiposOperacionE.SelectedValue == "2")
                {
                    //Estacion Sin Asignar
                    id_tabla = 15;
                    //ddlEstacionesE.SelectedValue = "0";
                }
                else if (ddlTiposOperacionE.SelectedValue == "3")
                {
                    //Estacion Sin Asignar
                    id_tabla = 25;
                    //ddlEstacionesE.SelectedValue = "0";
                }
                if (txtEstacionesE.Text == " "|| txtProveedoresE.Text == " ")
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion("La Estación de Combustible es obligatoria.");
                }
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    resultado = objVale.EditarRutaUnidadDiesel(Convert.ToInt32(gvTiposUnidad.SelectedValue),
                       (SAT_CL.Ruta.RutaUnidadDiesel.TipoOperacion)Convert.ToByte(ddlTiposOperacionE.SelectedValue), id_tabla, 
                       txtEstacionesE.Visible == true ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEstacionesE.Text, " ID:", 1)) : Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedoresE.Text, " ID:", 1)), 
                       (((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario));
                }
            }
                //Validamos que la operacion se haya realizado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Indices
                    Controles.InicializaIndices(gvVale);
                    //Cargamos Vales
                    cargaVales();


                }
                //Muestra el mensaje de error
                TSDK.ASP.ScriptServer.MuestraNotificacion(gvVale, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //}
        }
        /// <summary>
        /// Método encargado de Deshabilitar el Vale
        /// </summary>
        private void deshabilitaVale()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Innstanciamos Vales
            using (SAT_CL.Ruta.RutaUnidadDiesel objDiesel = new SAT_CL.Ruta.RutaUnidadDiesel(Convert.ToInt32(gvVale.SelectedValue)))
            {
                //Deshabilitamos Vale
                resultado = objDiesel.DeshabilitaRutaUnidadDiesel(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Vales
                cargaVales();
                //Inicializamos Indices
                Controles.InicializaIndices(gvVale);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvVale, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de guardar el Tipo Unidad
        /// </summary>
        private void deshabilitaTipoUnidad()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Tipo de Unidad
            using (SAT_CL.Ruta.RutaTipoUnidad objRutaTipoUnidad = new SAT_CL.Ruta.RutaTipoUnidad(Convert.ToInt32(gvTiposUnidad.SelectedValue)))
            {
                //Deshabilitamos Tipo Unidad
                resultado = objRutaTipoUnidad.DeshabilitaRutaTipoUnidad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Controles
                inicializaControlesTipoUnidad();
                cargaTipoUnidad();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvTiposUnidad, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Inicializa el contenido de los controles  del Tipo de Unidad
        /// </summary>
        private void inicializaControlesTipoUnidad()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Inicializamos Controles
                    ddlTipoUnidad.SelectedValue = "1";
                    ddlConfiguracion.SelectedValue = "0";
                    txtRendimiento.Text = "0";
                    //Inicializamos Indices
                    Controles.InicializaGridview(gvTipoUnidad);
                    Controles.InicializaGridview(gvVales);
                    ddlTiposUnidad.SelectedValue = "1";
                    ddlConfiguraciones.SelectedValue = "0";
                    //txtRendimiento.Text = "0";
                    //Inicializamos Indices
                    Controles.InicializaGridview(gvTiposUnidad);
                    Controles.InicializaGridview(gvVale);
                    break;
                case Pagina.Estatus.Copia:
                    //Inicializamos Controles
                    ddlTipoUnidad.SelectedValue = "1";
                    ddlConfiguracion.SelectedValue = "0";
                    txtRendimiento.Text = "0";
                    ddlTiposUnidad.SelectedValue = "1";
                    ddlConfiguraciones.SelectedValue = "0";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //cargamos Tipo Unidad
                    //cargaTipoUnidad();
                    //Inicializamos Grid View
                    //Controles.InicializaGridview(gvVale);
                    ddlTipoUnidad.SelectedValue = "1";
                    ddlConfiguracion.SelectedValue = "0";
                    //Controles.InicializaGridview(gvVale);
                    ddlTiposUnidad.SelectedValue = "1";
                    ddlConfiguraciones.SelectedValue = "0";
                    txtRendimiento.Text = "";
                    break;
            }
        }



        /// <summary>
        /// Habilita o deshabilita los controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControlesTipoUnidad()
        {
            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Copia:
                    ddlTipoUnidad.Enabled =
                    ddlConfiguracion.Enabled =
                    txtRendimiento.Enabled =
                    btnAceptarTipoUnidad.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    ddlTipoUnidad.Enabled =
                     ddlConfiguracion.Enabled =
                     txtRendimiento.Enabled =
                     btnAceptarTipoUnidad.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Método encargado de cargar los Tipos de Unidad
        /// </summary>
        private void cargaTipoUnidad()
        {
            //Obtenemos Depósito
            using (DataTable mit = SAT_CL.Ruta.RutaTipoUnidad.CargaTipoUnidad(Convert.ToInt32(Session["id_registro"])))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvTiposUnidad, mit, "Id", "", true, 1);
                    Controles.CargaGridView(gvTipoUnidad, mit, "Id", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvTiposUnidad);
                    Controles.InicializaGridview(gvTipoUnidad);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
            //Inicializamos Indices
            Controles.InicializaIndices(gvTiposUnidad);
            Controles.InicializaIndices(gvTipoUnidad);
        }

        /// <summary>
        /// Método encargado de cargar los Vales
        /// </summary>
        private void cargaVales()
        {
            //Obtenemos Depósito
            using (DataTable mit = SAT_CL.Ruta.RutaUnidadDiesel.CargaVales(Convert.ToInt32(gvTiposUnidad.SelectedValue)))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvVale, mit, "Id", "", false, 1);
                    //Controles.CargaGridView(gvVales, mit, "Id", "", false, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvVale);
                    //Controles.InicializaGridview(gvVales);

                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        #endregion

        #region Eventos Diesel

        /// <summary>
        /// Evento corting de gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVales_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvVales.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView.Sort = lblOrdenadoVales.Text;
                //Cambiando Ordenamiento
                lblOrdenadoVales.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvVales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvVales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVales_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvVales, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoVales.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarVales_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
        }
        /// <summary>
        /// Evento corting de gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVale_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvVale.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView.Sort = lblOrdenadoVale.Text;
                //Cambiando Ordenamiento
                lblOrdenadoVale.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvVale, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVale_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvVale, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVale_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvVale, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoVale.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportaVales_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
        }
        /// <summary>
        /// Evento Generado al Aceptar la Uniddad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTipoUnidad_Click(object sender, EventArgs e)
        {
            //Guarda Tipo de Unidad
            guardaTipoUnidad();
        }
        /// <summary>
        /// Evento Generado al Aceptar la Uniddad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTiposUnidad_Click(object sender, EventArgs e)
        {
            //Guarda Tipo de Unidad
            guardaTiposUnidad();
        }

        /// <summary>
        /// Click en algún Link de GV de Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAccionTipoUnidad_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvTipoUnidad.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvTipoUnidad, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Seleccionar":
                        //Cargamos Vales
                        cargaVales();
                        break;
                    case "Deshabilitar":
                        //Deshabilitamos Tipo Unidad
                        deshabilitaTipoUnidad();
                        break;
                    case "Bitacora":
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvTipoUnidad.SelectedValue.ToString(), "173", "Tipo Unidad");
                        break;
                }
            }
        }

        /// <summary>
        /// Click en algún Link de GV de Tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAccionTiposUnidad_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvTiposUnidad.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvTiposUnidad, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Seleccionar":
                        //Cargamos Vales
                        cargaVales();
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        inicializaControlesTipoUnidad();
                        break;
                    case "Deshabilitar":
                        //Deshabilitamos Tipo Unidad
                        deshabilitaTipoUnidad();
                        break;
                    case "Bitacora":
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvTiposUnidad.SelectedValue.ToString(), "173", "Tipo Unidad");
                        break;
                }
            }
        }
        /// <summary>
        /// Evento generado al Editar un Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGuardarE_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvVales.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvVales, sender, "lnk", true);
                //Carga Vales
                cargaVales();
                //Inicializamos Valores
                using (SAT_CL.Ruta.RutaUnidadDiesel objVales = new SAT_CL.Ruta.RutaUnidadDiesel(Convert.ToInt32(gvVales.SelectedValue)))
                {
                    //Instanciamos Litrso
                    using (TextBox txtLitrosE = (TextBox)gvVales.SelectedRow.FindControl("txtLitrosE"))
                    {
                        using (DropDownList ddlTipoOperacionE = (DropDownList)gvVales.SelectedRow.FindControl("ddlTipoOperacionE"),
                           ddlEstacionE = (DropDownList)gvVales.SelectedRow.FindControl("ddlEstacionE"))
                        {
                            //cargando catalogo de estaciones
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionE, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                            //cargando catalogo Tipo Operacion
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperacionE, "", 3167);
                            //Inicializamos Valores
                            ddlTipoOperacionE.SelectedValue = objVales.id_tipo_operacion.ToString();
                            ddlEstacionE.SelectedValue = objVales.id_registro.ToString();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al Editar un Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGuardaE_Click(object sender, EventArgs e)
        {
            string script = "";
            //Si hay registros
            if (gvVale.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvVale, sender, "lnk", true);
                //Carga Vales
                cargaVales();
                //Inicializamos Valores
                using (SAT_CL.Ruta.RutaUnidadDiesel objVales = new SAT_CL.Ruta.RutaUnidadDiesel(Convert.ToInt32(gvVale.SelectedValue)))
                {
                    //Instanciamos Litrso
                    //using (TextBox txtLitrosE = (TextBox)gvVale.SelectedRow.FindControl("txtLitrosE"))
                    //{
                    using (DropDownList ddlTiposOperacionE = (DropDownList)gvVale.SelectedRow.FindControl("ddlTiposOperacionE"))
                    using (TextBox txtEstacionesE = (TextBox)gvVale.SelectedRow.FindControl("txtEstacionesE"),
                           txtProveedoresE = (TextBox)gvVale.SelectedRow.FindControl("txtProveedoresE"))
                    {
                        //cargando catalogo Tipo Operacion
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTiposOperacionE, "", 3167);
                        //Inicializamos Valores
                        ddlTiposOperacionE.SelectedValue = objVales.id_tipo_operacion.ToString();
                        if (ddlTiposOperacionE.SelectedValue == "1" || ddlTiposOperacionE.SelectedValue == "2")
                        {
                            txtEstacionesE.Visible = true;
                            txtProveedoresE.Visible = false;
                            using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(objVales.id_registro))
                                //cargando catalogo de estaciones
                                //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionesE, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                                txtEstacionesE.Text = u.descripcion + " ID:" + objVales.id_registro.ToString();
                        }
                        else if (ddlTiposOperacionE.SelectedValue == "3")
                        {
                            txtEstacionesE.Visible = false;
                            txtProveedoresE.Visible = true;
                            //cargando catalogo de estaciones
                            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionesE, 197, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                            using (SAT_CL.Global.CompaniaEmisorReceptor cep = new SAT_CL.Global.CompaniaEmisorReceptor(objVales.id_registro))
                                //cargando catalogo de estaciones
                                //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionesE, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                                txtEstacionesE.Text = cep.nombre + " ID:" + objVales.id_registro.ToString();
                        }
                        if (txtEstacionesE.Visible == true)
                        {
                            //Declarando Script de Ventana Modal
                            script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Estaciones'
                                                $('*[id$=gvVale] input[id$=txtEstacionesE]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=13&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                               </script>";
                        }
                        else
                        //Declarando Script de Ventana Modal
                        {
                            script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Estaciones'
                                                $('*[id$=gvVale] input[id$=txtProveedoresE]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=65&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                               </script>";
                        }
                        //Registrando Script
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaEstacionesProveedoresE", script, false);
                    }
                    //}
                }
            }
        }
        /// <summary>
        /// Click en algún Link de GV de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionVale_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvVales.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvVales, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Deshabilitar":
                        //Deshabilitamos Vale
                        deshabilitaVale();
                        break;
                    case "Bitacora":
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvVales.SelectedValue.ToString(), "174", "Diesel");
                        break;
                    case "GuardarE":
                        //Editamos el Vale
                        editaVale();
                        break;
                    case "CancelarE":
                        //Inicializamos Indice
                        Controles.InicializaIndices(gvVales);
                        //Cargamos Vales
                        cargaVales();
                        break;
                }
            }
        }
        /// <summary>
        /// Click en algún Link de GV de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionVales_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvVale.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvVale, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Deshabilitar":
                        //Deshabilitamos Vale
                        deshabilitaVale();
                        break;
                    case "Bitacora":
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvVale.SelectedValue.ToString(), "174", "Diesel");
                        break;
                    case "GuardarE":
                        //Editamos el Vale
                        editaVale();
                        break;
                    case "CancelarE":
                        //Inicializamos Indice
                        Controles.InicializaIndices(gvVale);
                        //Cargamos Vales
                        cargaVales();
                        break;
                }
            }
        }
        /// <summary>
        /// Evento generado al Insertar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkInsertar_Click(object sender, EventArgs e)
        {
            //Cargamos Vales
            guardaVale();
        }
        /// <summary>
        /// Evento generado al Insertar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkInserta_Click(object sender, EventArgs e)
        {
            //Cargamos Vales
            guardaVale();
        }
        /// <summary>
        /// Inicializa los Controles del GV Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkEditar = (LinkButton)fila.FindControl("lnkEditar"),
                      lnkBitacora = (LinkButton)fila.FindControl("lnkBitacora"),
                      lnkDeshabilitar = (LinkButton)fila.FindControl("lnkDeshabilitar"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvVales.EditIndex == -1)
                                    {
                                        lnkEditar.Enabled =
                                        lnkDeshabilitar.Enabled =
                                        lnkBitacora.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        //Creamos Instancias de Tipo TextBox y DropDownList
                        using (TextBox txtlitros = (TextBox)fila.FindControl("txtlitros"))
                        {
                            using (DropDownList ddlTipoOperacion = (DropDownList)fila.FindControl("ddlTipoOperacion"),
                                                ddlEstacion = (DropDownList)fila.FindControl("ddlEstacion"))
                            {
                                using (LinkButton lnkInsertar = (LinkButton)fila.FindControl("lnkInsertar"))
                                {
                                    //Validando Estatus de la Pagina
                                    switch ((Pagina.Estatus)Session["estatus"])
                                    {
                                        case Pagina.Estatus.Nuevo:
                                            //case Pagina.Estatus.Copiar:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                                ddlEstacion.Enabled =
                                                txtlitros.Enabled =
                                                ddlTipoOperacion.Enabled = false;
                                            }
                                            break;
                                        case Pagina.Estatus.Lectura:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                              ddlEstacion.Enabled =
                                              txtlitros.Enabled =
                                              ddlTipoOperacion.Enabled = false;

                                            }
                                            break;
                                        case Pagina.Estatus.Edicion:
                                            {
                                                //Habilitamos controles 
                                                lnkInsertar.Enabled =
                                                ddlTipoOperacion.Enabled =
                                                txtlitros.Enabled = true;
                                                ddlEstacion.Enabled = true;
                                                //Si la fila esta en modo de Edicion
                                                if (gvVales.EditIndex != -1)
                                                {
                                                    //Deshabilitamos controles 
                                                    lnkInsertar.Enabled =
                                                    ddlEstacion.Enabled =
                                                    txtlitros.Enabled =
                                                ddlTipoOperacion.Enabled = false;
                                                }
                                            }
                                            break;
                                    }
                                    //cargando catalogo de estaciones
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacion, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                                    //cargando catalogo Tipo Operacion
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperacion, "", 3167);
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Inicializa los Controles del GV Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVale_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkEdita = (LinkButton)fila.FindControl("lnkEdita"),
                      lnkBitacor = (LinkButton)fila.FindControl("lnkBitacor"),
                      lnkDeshabilit = (LinkButton)fila.FindControl("lnkDeshabilit"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEdita.Enabled =
                                    lnkBitacor.Enabled =
                                    lnkDeshabilit.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEdita.Enabled =
                                    lnkBitacor.Enabled =
                                    lnkDeshabilit.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvVale.EditIndex == -1)
                                    {
                                        lnkEdita.Enabled =
                                        lnkDeshabilit.Enabled =
                                        lnkBitacor.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        //Creamos Instancias de Tipo TextBox y DropDownList
                        //using (TextBox txtlitros = (TextBox)fila.FindControl("txtlitros"))
                        //{
                        using (DropDownList ddlTiposOperacion = (DropDownList)fila.FindControl("ddlTiposOperacion"))
                        using (TextBox txtEstaciones = (TextBox)fila.FindControl("txtEstaciones"),
                                       txtProveedores = (TextBox)fila.FindControl("txtProveedores"))
                        {
                            using (LinkButton lnkInserta = (LinkButton)fila.FindControl("lnkInserta"))
                            {
                                //Validando Estatus de la Pagina
                                switch ((Pagina.Estatus)Session["estatus"])
                                {
                                    case Pagina.Estatus.Nuevo:
                                        //case Pagina.Estatus.Copiar:
                                        {
                                            //Deshabilitamos controles
                                            lnkInserta.Enabled =
                                            txtEstaciones.Enabled =
                                            txtProveedores.Enabled =
                                            txtProveedores.Visible =
                                            //txtlitros.Enabled =
                                            ddlTiposOperacion.Enabled = false;
                                        }
                                        break;
                                    case Pagina.Estatus.Lectura:
                                        {
                                            //Deshabilitamos controles
                                            lnkInserta.Enabled =
                                          txtEstaciones.Enabled =
                                            txtProveedores.Enabled =
                                            txtProveedores.Visible =
                                          //txtlitros.Enabled =
                                          ddlTiposOperacion.Enabled = false;

                                        }
                                        break;
                                    case Pagina.Estatus.Edicion:
                                        {
                                            //Habilitamos controles 
                                            lnkInserta.Enabled =
                                            ddlTiposOperacion.Enabled =
                                            //txtlitros.Enabled = true;
                                            txtEstaciones.Enabled =
                                            txtProveedores.Enabled = true;
                                            txtProveedores.Visible = false;
                                            //Si la fila esta en modo de Edicion
                                            if (gvVale.EditIndex != -1)
                                            {
                                                //Deshabilitamos controles 
                                                lnkInserta.Enabled =
                                                txtEstaciones.Enabled =
                                            txtProveedores.Enabled =
                                            txtProveedores.Visible =
                                            //txtlitros.Enabled =
                                            ddlTiposOperacion.Enabled = false;
                                            }
                                        }
                                        break;
                                }
                                //cargando catalogo de estaciones
                                //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstaciones, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                                //Declarando Script de Ventana Modal
                                string script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Estaciones'
                                                $('*[id$=gvVale] input[id$=txtEstaciones]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=13&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                               </script>";

                                //Registrando Script
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaEstacionesProveedores", script, false);
                                //cargando catalogo Tipo Operacion
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTiposOperacion, "", 3167);
                            }
                        }
                        //}
                    }
                    break;
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTipoUnidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTipoUnidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Inicializa los Controles del GV Tipo Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTipoUnidad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkBitacora = (LinkButton)fila.FindControl("lnkBitacora"),
                      lnkDeshabilitar = (LinkButton)fila.FindControl("lnkDeshabilitar"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                // case Pagina.Estatus.Copiar:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvVales.EditIndex == -1)
                                    {
                                        lnkDeshabilitar.Enabled =
                                        lnkBitacora.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTiposUnidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTipoUnidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Inicializa los Controles del GV Tipo Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTiposUnidad_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkBitacoras = (LinkButton)fila.FindControl("lnkBitacoras"),
                      lnkDeshabilita = (LinkButton)fila.FindControl("lnkDeshabilita"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                // case Pagina.Estatus.Copiar:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacoras.Enabled =
                                    lnkDeshabilita.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkBitacoras.Enabled =
                                    lnkDeshabilita.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvVales.EditIndex == -1)
                                    {
                                        lnkDeshabilita.Enabled =
                                        lnkBitacoras.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTiposOperacionE_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scriptE = "";
            //Obteniendo Control para Andenes
            using (TextBox txtEstacionesE = (TextBox)gvVale.SelectedRow.FindControl("txtEstacionesE"))
            //Obteniendo Control para Cajones
            using (TextBox txtProveedoresE = (TextBox)gvVale.SelectedRow.FindControl("txtProveedoresE"))
                if (((DropDownList)sender).SelectedValue == "1" || ((DropDownList)sender).SelectedValue == "2")
                {
                    txtEstacionesE.Visible = true;
                    txtProveedoresE.Visible = false;
                    txtProveedoresE.Text = "";
                    //Declarando Script de Ventana Modal
                    scriptE = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Estaciones'
                                                $('*[id$=gvVale] input[id$=txtEstacionesE]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=13&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                               </script>";
                }
                else if (((DropDownList)sender).SelectedValue == "3")
                {
                    txtProveedoresE.Visible = true;
                    txtEstacionesE.Visible = false;
                    txtEstacionesE.Text = "";                    
                    //Declarando Script de Ventana Modal
                    scriptE = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Proveedores'
                                                $('*[id$=gvVale] input[id$=txtProveedoresE]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=65&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                              </script>";
                }
            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaEstacionesProveedoresE", scriptE, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTiposOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string script = "";
            //Obteniendo Control para Andenes
            using (TextBox txtEstaciones = (TextBox)gvVale.FooterRow.FindControl("txtEstaciones"))
            //Obteniendo Control para Cajones
            using (TextBox txtProveedores = (TextBox)gvVale.FooterRow.FindControl("txtProveedores"))
                if (((DropDownList)sender).SelectedValue == "1" || ((DropDownList)sender).SelectedValue == "2")
                {
                    txtEstaciones.Visible = true;
                    txtProveedores.Visible = false;
                    txtProveedores.Text = "";
                    //Declarando Script de Ventana Modal
                    script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Estaciones'
                                                $('*[id$=gvVale] input[id$=txtEstaciones]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=13&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                               </script>";
                }
                else if (((DropDownList)sender).SelectedValue == "3")
                {
                    txtProveedores.Visible = true;
                    txtEstaciones.Visible = false;
                    txtEstaciones.Text = "";
                    //Declarando Script de Ventana Modal
                    script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Proveedores'
                                                $('*[id$=gvVale] input[id$=txtProveedores]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=65&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + @"'});
                                              </script>";
                }
            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaEstacionesProveedores", script, false);
        }
        #endregion



        #region Eventos Concepto
        /// <summary>
        /// Evento corting de gridview de Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConcepto_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvConcepto.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView.Sort = lblOrdenadoConcepto.Text;
                //Cambiando Ordenamiento
                lblOrdenadoConcepto.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvConcepto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConcepto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvConcepto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvConcepto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoConcepto.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarConcepto_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
        }

        /// <summary>
        /// Inicializa los Controles del GV Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConcepto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkEditar = (LinkButton)fila.FindControl("lnkEditar"),
                      lnkBitacora = (LinkButton)fila.FindControl("lnkBitacora"),
                      lnkDeshabilitar = (LinkButton)fila.FindControl("lnkDeshabilitar"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvConcepto.EditIndex == -1)
                                    {
                                        lnkEditar.Enabled =
                                        lnkDeshabilitar.Enabled =
                                        lnkBitacora.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        //Creamos Instancias de Tipo TextBox y DropDownList
                        using (TextBox txtMonto = (TextBox)fila.FindControl("txtMonto"))
                        {
                            using (DropDownList lblTipoMonto = (DropDownList)fila.FindControl("ddlTipoMonto"),
                                                ddlConcepto = (DropDownList)fila.FindControl("ddlConcepto"))
                            {
                                using (LinkButton lnkInsertar = (LinkButton)fila.FindControl("lnkInsertar"))
                                {
                                    //Validando Estatus de la Pagina
                                    switch ((Pagina.Estatus)Session["estatus"])
                                    {
                                        case Pagina.Estatus.Nuevo:
                                            //case Pagina.Estatus.Copiar:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                                ddlConcepto.Enabled =
                                                txtMonto.Enabled =
                                                lblTipoMonto.Enabled = false;
                                            }
                                            break;
                                        case Pagina.Estatus.Lectura:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                              ddlConcepto.Enabled =
                                              txtMonto.Enabled =
                                              lblTipoMonto.Enabled = false;

                                            }
                                            break;
                                        case Pagina.Estatus.Edicion:
                                            {
                                                //Habilitamos controles 
                                                lnkInsertar.Enabled =
                                                ddlConcepto.Enabled =
                                                txtMonto.Enabled =
                                                lblTipoMonto.Enabled = true;
                                                //Si la fila esta en modo de Edicion
                                                if (gvVales.EditIndex != -1)
                                                {
                                                    //Deshabilitamos controles 
                                                    lnkInsertar.Enabled =
                                                    ddlConcepto.Enabled =
                                                    txtMonto.Enabled =
                                                    lblTipoMonto.Enabled = false;
                                                }
                                            }
                                            break;
                                    }
                                    //cargando catalogo de Concepto
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 93, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                                    //cargando catalogo Tipo Monto
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(lblTipoMonto, "", 3169);


                                }
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Click en algún Link de GV de Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionConcepto_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvConcepto.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvConcepto, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Deshabilitar":
                        //Deshabilitamos
                        deshabilitaConcepto();
                        break;
                    case "Bitacora":
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(gvConcepto.SelectedValue.ToString(), "176", "Deposito");
                        break;
                    case "GuardarE":
                        //Editamos el Concepto
                        editaConcepto();
                        break;
                    case "CancelarE":
                        //Inicializamos Indice
                        Controles.InicializaIndices(gvConcepto);
                        //Cargamos Cponceptos
                        cargaConcepto();
                        break;
                }
            }
        }

        /// <summary>
        /// Evento generado al Editar un C oncepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGuardarConceptoE_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvConcepto.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvConcepto, sender, "lnk", true);
                //Carga Conceptos
                cargaConcepto();
                //Inicializamos Valores
                using (SAT_CL.Ruta.RutaDeposito objConcepto = new SAT_CL.Ruta.RutaDeposito(Convert.ToInt32(gvConcepto.SelectedValue)))
                {
                    //Instanciamos Litrso
                    using (TextBox txtMontoE = (TextBox)gvConcepto.SelectedRow.FindControl("txtMontoE"))
                    {
                        using (DropDownList ddlTipoMontoE = (DropDownList)gvConcepto.SelectedRow.FindControl("ddlTipoMontoE"),
                           ddlConceptoE = (DropDownList)gvConcepto.SelectedRow.FindControl("ddlConceptoE"))
                        using (CheckBox chkComprobacionE = (CheckBox)gvConcepto.SelectedRow.FindControl("chkComprobacionE"))
                        {
                            //cargando catalogo de Concepto
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConceptoE, 93, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                            //cargando catalogo Tipo Monto
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoMontoE, "", 3169);
                            //Inicializamos Valores
                            txtMontoE.Text = objConcepto.Monto.ToString();
                            ddlTipoMontoE.SelectedValue = objConcepto.id_tipo_monto.ToString();
                            ddlConceptoE.SelectedValue = objConcepto.id_concepto_restriccion.ToString();
                            chkComprobacionE.Checked = objConcepto.bit_comprobacion;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al Insertar un Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkInsertarConcepto_Click(object sender, EventArgs e)
        {
            //Guardamos Concepto
            guardaConcepto();
        }
        #endregion

        #region Métodos Conceptos
        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosConcepto()
        {
            //Tamaño de la Caseta
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoConcepto, "", 56);

        }

        /// <summary>
        /// Método encargado de guardar el Concepto
        /// </summary>
        private void guardaConcepto()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Recuperando controles 
            using (TextBox txtMonto = (TextBox)gvConcepto.FooterRow.FindControl("txtMonto"))
            {
                using (DropDownList ddlConcepto = (DropDownList)gvConcepto.FooterRow.FindControl("ddlConcepto"),
                       ddlTipoMonto = (DropDownList)gvConcepto.FooterRow.FindControl("ddlTipoMonto"))
                using (CheckBox chkComprobacion = (CheckBox)gvConcepto.FooterRow.FindControl("chkComprobacion"))
                {
                    //Insertamos Vale
                    resultado = SAT_CL.Ruta.RutaDeposito.InsertarRutaDeposito(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlConcepto.SelectedValue),
                              (SAT_CL.Ruta.RutaDeposito.TipoMonto)Convert.ToByte(ddlTipoMonto.SelectedValue), Convert.ToDecimal(txtMonto.Text), chkComprobacion.Checked, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                }
            }

            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Concepto
                cargaConcepto();
                //Inicializamos Indices
                Controles.InicializaIndices(gvConcepto);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvConcepto, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Metodo encargado de Editar el Concepto
        /// </summary>
        private void editaConcepto()
        {
            //Declaracion de objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            using (TextBox txtMontoE = (TextBox)gvConcepto.SelectedRow.FindControl("txtMontoE"))
            {
                using (DropDownList ddlConceptoE = (DropDownList)gvConcepto.SelectedRow.FindControl("ddlConceptoE"),
                                    ddlTipoMontoE = (DropDownList)gvConcepto.SelectedRow.FindControl("ddlTipoMontoE"))
                using (CheckBox chkComprobacion = (CheckBox)gvConcepto.SelectedRow.FindControl("chkComprobacionE"))
                //Editamos Vale
                using (SAT_CL.Ruta.RutaDeposito objDeposito = new SAT_CL.Ruta.RutaDeposito(Convert.ToInt32(gvConcepto.SelectedDataKey.Value)))
                {
                    resultado = objDeposito.EditarRutaDeposito(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlConceptoE.SelectedValue),
                        (SAT_CL.Ruta.RutaDeposito.TipoMonto)Convert.ToByte(ddlTipoMontoE.SelectedValue), Convert.ToDecimal(txtMontoE.Text), chkComprobacion.Checked, (((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario));
                }
                //Validamos que la operacion se haya realizado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Indices
                    Controles.InicializaIndices(gvConcepto);
                    //Cargamos Conceptos
                    cargaConcepto();

                }
                //Muestra el mensaje de error
                TSDK.ASP.ScriptServer.MuestraNotificacion(gvConcepto, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Método encargado de Deshabilitar el Concepto
        /// </summary>
        private void deshabilitaConcepto()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Innstanciamos Vales
            using (SAT_CL.Ruta.RutaDeposito objDeposito = new SAT_CL.Ruta.RutaDeposito(Convert.ToInt32(gvConcepto.SelectedValue)))
            {
                //Deshabilitamos Vale
                resultado = objDeposito.DeshabilitaRutaDeposito(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //SI no existen errores
            if (resultado.OperacionExitosa)
            {
                //Cargamos Conceptos
                cargaConcepto();
                //Inicializamos Indices
                Controles.InicializaIndices(gvConcepto);
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(gvConcepto, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de cargar los Conceptos
        /// </summary>
        private void cargaConcepto()
        {

            //Obtenemos Depósito
            using (DataTable mit = SAT_CL.Ruta.RutaDeposito.ObtieneConceptoDeposito(Convert.ToInt32(Session["id_registro"])))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvConcepto, mit, "Id", "", false, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table3");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvConcepto);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }

        /// <summary>
        /// Inicializa el contenido de los controles  Concepto
        /// </summary>
        private void inicializaControlesConcepto()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    Controles.InicializaGridview(gvConcepto);
                    break;
                case Pagina.Estatus.Copia:
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //cargamos Conceptos
                    cargaConcepto();
                    break;
            }
        }
        #endregion        
    }
}