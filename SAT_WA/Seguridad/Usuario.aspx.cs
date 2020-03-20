using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Seguridad
{
    public partial class Usuario : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento que permite determinar la inicialización del formulario Usuario
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene información de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida si la pagina se esta mostrando por primera vez o es respuesta a una peticion.
            if (!Page.IsPostBack)
                //Invoca al método inicializaForma().
                inicializaForma();           
        }

        /// <summary>
        /// Evento que permite almacenar los datos obtenidos de los controles del formulario a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarTipoPago().
            guardarUsuario();
        }
        /// <summary>
        /// Evento que me anula acciones realizadas sobre el formulario (Ingreso, edicion de datos, etc.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Valida cada estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la pagina se encuentra en modo  Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //Si el estado de la pagina se encuentra en modo de Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable session estatus el valor actual de la pagina.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma();
            inicializaForma();
        }
        /// <summary>
        /// Evento que permite seleccionar y ejecutar acciones del menú.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                //Si la elección del menú es la opción Nuevo
                case "Nuevo":
                    {
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Limpia los mensajes de error del lblError
                        lblError.Text = "";
                        //Hace un enfoque en el primer control
                        txtNombre.Focus();
                        //Asigna el valor de 0, al método que inicializa el control de usuario PerfilUsuarioAlta
                        ucPerfilUsuarioAlta.InicializaPerfilesUsuarioAlta(0, 0);
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(30, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaTipoPago();
                        guardarUsuario();
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma();
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        //Hace un efoque en el primer control
                        txtNombre.Focus();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Invoca al constructor de la clase y asigna el valor de la variable de session id_registro.
                            using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario((int)Session["id_registro"]))
                            {
                                //Valida si el registro existe.
                                if (us.id_usuario > 0)
                                {
                                    //Asigna al objeto retorno los datos del usuario que realizo el cambio de estado del registro(Deshabilito)                            
                                    retorno = us.DeshabilitaUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Instanciando registro de Usuario - Compania
                                        using (SAT_CL.Seguridad.UsuarioCompania uc = new SAT_CL.Seguridad.UsuarioCompania(us.id_usuario, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0"))))
                                        {
                                            //Validando que Existe el Registro
                                            if (uc.id_usuario_compania > 0)
                                            {
                                                //Deshabilitando 
                                                retorno = uc.DeshabilitaUsuarioCompania(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando Operación Exitosa
                                                if (retorno.OperacionExitosa)

                                                    //Completando Transacción
                                                    trans.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                            //Valida si la operación se realizo correctamente.
                        if (retorno.OperacionExitosa)
                        {
                            //Asigna el valor de estado lectura a la variable de session estatus 
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Asigna el valor 0 a la variable de session id_registro
                            Session["id_registro"] = 0;
                            //invoca al método inicializaForma().
                            inicializaForma();
                            ucPerfilUsuarioAlta.InicializaPerfilesUsuarioAlta(0, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0")));
                        }
                        

                        //Muestra un mensaje acorde a la validación de la operación
                        lblError.Text = retorno.Mensaje;
                        break;
                    }
                //Si la elección del menú en la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "30", "Usuario");
                        break;
                    }
                //Si la elección del menú en la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de TipoPago
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "30",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú en la opcion Sesiones Activas
                case "Sesiones":
                    {
                        //Validando Estatus el Sesión
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Nuevo:
                                {
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(botonMenu, "No hay Usuario", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    break;
                                }
                            case Pagina.Estatus.Lectura:
                            case Pagina.Estatus.Edicion:
                                {
                                    //Cargando Sesiones Activas
                                    cargaSessionesActivas();
                                    //Mostrando Ventana Modal
                                    ScriptServer.AlternarVentana(botonMenu, "SesionesActivas", "contenedorVentanaSesionesActivas", "ventanaSesionesActivas");
                                    break;
                                }
                        }

                        break;
                    }
                //Si la elección del menú en la opcion Archivo
                case "Archivo":
                    {
                        //Validando Estatus de Sesión
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Lectura:
                            case Pagina.Estatus.Edicion:
                                //Inicializando Archivos
                                inicializaArchivosRegistro(Session["id_registro"].ToString(), "30", "0");
                                break;
                            default:
                                //Mostrando Excepción
                                ScriptServer.MuestraNotificacion(lkbArchivos, new RetornoOperacion("Debe existir un Usuario"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                break;
                        }
                        break;
                    }
                //Si la elección del menú en la opcion Acerca
                case "Acerca":
                    {
                        break;
                    }
                //Si la elección del menú en la opcion Ayuda
                case "Ayuda":
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar las Sesiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarSesiones_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Sesiones Activas
            using (DataTable dtSesiones = SAT_CL.Seguridad.UsuarioSesion.ObtieneSesionesActivasUsuario(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtSesiones))
                {
                    //Recorriendo Sesiones
                    foreach(DataRow dr in dtSesiones.Rows)
                    {
                        //Inicializando Transacción
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Sesión de Usuario
                            using (SAT_CL.Seguridad.UsuarioSesion sesion = new SAT_CL.Seguridad.UsuarioSesion(Convert.ToInt32(dr["Id"])))
                            {
                                //Validando que exista la Sesión
                                if (sesion.habilitar)
                                {
                                    //Terminando Sesión
                                    result = sesion.TerminarSesion();

                                    //Validando si la Operación no fue exitosa
                                    if (!result.OperacionExitosa)

                                        //Terminando Ciclo
                                        break;
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Sesión no existe");
                            }

                            //Validando Operación exitosa
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No hay Sesiones Activas");

                //Validando Operación exitosa
                if (result.OperacionExitosa)
                {
                    //Cargando Sesiones Activas
                    cargaSessionesActivas();

                    //Mostrando Ventana Modal
                    //ScriptServer.AlternarVentana(btnCerrarSesiones, "SesionesActivas", "contenedorVentanaSesionesActivas", "ventanaSesionesActivas");
                }
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(btnCerrarSesiones, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrar_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrar, "SesionesActivas", "contenedorVentanaSesionesActivas", "ventanaSesionesActivas");
        }

        #region Eventos GridView "Sesiones Activas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Sesiones Activas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSA_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            Controles.CambiaTamañoPaginaGridView(gvSesionesActivas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoSA.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Terminar Sesión"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbTerminarSesion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSesionesActivas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvSesionesActivas, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion sesion = new SAT_CL.Seguridad.UsuarioSesion(Convert.ToInt32(gvSesionesActivas.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Sesión
                    if (sesion.habilitar)

                        //Terminando Sesión
                        result = sesion.TerminarSesion();
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no existe");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                
                    //Cargando Sesiones Activas
                    cargaSessionesActivas();

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion((LinkButton)sender, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Paginación del GridView "Sesiones Activas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSesionesActivas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSesionesActivas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Sesiones Activas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSesionesActivas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoSA.Text = Controles.CambiaSortExpressionGridView(gvSesionesActivas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Sesiones Activas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarSA_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }

        #endregion

        #region Eventos UserControl "Perfil Usuario Alta"

        /// <summary>
        /// Evento Producido 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucPerfilUsuarioAlta_ClickGuardarPerfilUsuario(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            ucPerfilUsuarioAlta.GuardaPerfilUsuario();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucPerfilUsuarioAlta_ClickEliminarPerfilUsuario(object sender, EventArgs e)
        {
            //Invocando Método de Deshabilitación
            ucPerfilUsuarioAlta.EliminaPerfilUsuario();
        }

        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Método que determina el aspecto inicial de la página
        /// </summary>
        private void inicializaForma()
        {
            //Instanciando Compania Emisor
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que Existe la Compania
                if (cer.id_compania_emisor_receptor > 0)

                    //Asignando Valor
                    txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompania.Text = "";
            }
            
            //Invoca al método de carga de Catalogos
            cargaCatalogos();
            //Invoca al método habilitaControles().
            habilitaControles();
            //Invoca al método habilitaMenu().
            habilitaMenu();
            //Invoca al método inicializaValores().
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Departamento
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDepartamento, 68, "");
            //Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSA, "", 18);
        }
        /// <summary>
        /// Método que permite cambiar el estado de los controles en base a cada estado de la pagina
        /// </summary>
        private void habilitaControles()
        {
            //Evalua cada estado de la pagina en base a su estado
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo,edicion, habilita los controles del formulario.
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        txtNombre.Enabled =
                        txtEmail.Enabled =
                        txtContrasena.Enabled =
                        txtRepitaContrasena.Enabled =
                        txtPregunta.Enabled =                        
                        txtRespuesta.Enabled =
                        txtSesiones.Enabled =
                        txtTiempo.Enabled =
                        txtVigencia.Enabled =
                        ddlDepartamento.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página sea lectura, se deshabilitaran los controles.
                case Pagina.Estatus.Lectura:
                    {
                        txtNombre.Enabled =
                        txtEmail.Enabled =
                        txtContrasena.Enabled =
                        txtRepitaContrasena.Enabled =
                        txtPregunta.Enabled =
                        txtRespuesta.Enabled =
                        txtSesiones.Enabled =
                        txtTiempo.Enabled =
                        txtVigencia.Enabled =
                        ddlDepartamento.Enabled =
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que en base al estado de la pagina, habilitara o deshabilitara las opciones del menú principal.
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estatus de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo, habilitara y deshabilitara las opciones del menú principal.
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la pagina sea Lectura, habilitara y deshabilitara las opciones del menú principal.
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = 
                        lkbArchivos.Enabled = true;
                        lkbAcercaDe.Enabled = false;
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;
                        break;
                    }
                //En caso de que la pagina este en estado de edición, se habilitaran y deshabilitaran las opciones del menú principal.
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = 
                        lkbArchivos.Enabled = 
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que inicializa los valores de los controles del formulario Usuario en base a su estatus.
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo limpia los datos de los controles
                case Pagina.Estatus.Nuevo:
                    {
                        txtNombre.Text =
                        txtEmail.Text =
                        txtContrasena.Text =
                        txtRepitaContrasena.Text =
                        txtPregunta.Text =
                        txtRespuesta.Text =
                        txtSesiones.Text =
                        txtTiempo.Text =
                        txtVigencia.Text =
                        lblError.Text = "";
                        //Asigna formato de password a la caja de texto 
                        txtContrasena.Attributes["type"] = "password";
                        txtRepitaContrasena.Attributes["type"] = "password";
                        txtRespuesta.Attributes["type"] = "password";
                        break;
                    }
                //En caso de que el estado de la página sea edicion y lectura, realiza una consulta a la base de datos y obtiene un registro.
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca a la clase usuario, y asigna el valor de la variable ssion id_registro al constructor de la clase para obtener un registro.
                        using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario((int)Session["id_registro"]))
                        {
                            //Validando que exista el Usuario
                            if (us.id_usuario != 0)
                            {
                                txtNombre.Text = us.nombre;
                                txtEmail.Text = us.email;
                                txtSesiones.Text = us.sesiones_disponibles.ToString();
                                txtTiempo.Text = us.tiempo_expiracion.ToString();
                                txtVigencia.Text = us.dias_vigencia.ToString();
                                txtPregunta.Text = us.pregunta_secreta;
                                //Deshabilita los controles para su edición.
                                txtContrasena.Enabled =
                                txtRepitaContrasena.Enabled =  
                                txtPregunta.Enabled =
                                txtRespuesta.Enabled = false;
                                //Asigna formato de password a la caja de texto 
                                txtContrasena.Attributes["type"] = "password";                                
                                txtContrasena.Text = "password";
                                txtRepitaContrasena.Attributes["type"] = "password";
                                txtRepitaContrasena.Text = "password";
                                txtRespuesta.Attributes["type"] = "password";
                                txtRespuesta.Text = "password"; 

                                //Inicializando Control
                                ucPerfilUsuarioAlta.InicializaPerfilesUsuarioAlta(Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                                                                
                                //Instanciando Usuario Compania
                                using (SAT_CL.Seguridad.UsuarioCompania uc = new SAT_CL.Seguridad.UsuarioCompania(us.id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que exista el Registro
                                    if (uc.id_usuario_compania > 0)
                                    {
                                        //Asignando Departamento
                                        ddlDepartamento.SelectedValue = uc.id_departamento.ToString();

                                        //Instanciando Compania Emisor
                                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(uc.id_compania_emisor_receptor))
                                        {
                                            //Validando que Existe la Compania
                                            if (cer.id_compania_emisor_receptor > 0)

                                                //Asignando Valor
                                                txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                            else
                                                //Limpiando Control
                                                txtCompania.Text = "";
                                        }
                                    }
                                }
                            }
                        }                       
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que almacena los datos obtenidos de los controles del formulario Usuario a la base de datos.
        /// </summary>
        private void guardarUsuario()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int id_usuario = 0;
            
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Valida cada estado del formulario.
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //En caso de que el estado de la página sea nuevo, realizara una inserción de datos.
                    case Pagina.Estatus.Nuevo:
                        {
                            //Validando que la Contraseña sea Identica
                            if (txtContrasena.Text == txtRepitaContrasena.Text)
                            {
                                //Asigna al objeto retorno los valores obtenidos del formulario Usuario, invocando al método de insercion de la clase usuario.
                                retorno = SAT_CL.Seguridad.Usuario.InsertaUsuario(txtNombre.Text, txtEmail.Text, txtContrasena.Text, DateTime.Today,
                                                                                Convert.ToByte(txtVigencia.Text), Convert.ToByte(txtTiempo.Text), txtPregunta.Text, txtRespuesta.Text,
                                                                                Convert.ToByte(txtSesiones.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if(retorno.OperacionExitosa)
                                {
                                    //Obteniendo Registro de Usuario
                                    id_usuario = retorno.IdRegistro;

                                    //Insertando Usuario - Compania
                                    retorno = SAT_CL.Seguridad.UsuarioCompania.InsertaUsuarioCompania(id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                                Convert.ToInt32(ddlDepartamento.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            break;
                        }
                    //En caso de que el estado de la página sea edición, realizara una actualización de datos. 
                    case Pagina.Estatus.Edicion:
                        {
                            //Invoca al constructor de la clase Usuario y asigna como parametro la variable de session id_registro
                            using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario((int)Session["id_registro"]))
                            {
                                //Valida que exista el registro en la base de datos
                                if (us.id_usuario != 0)
                                {   
                                    //Asigna al objeto retorno los datos obtenidos del formulario, invocando al método de actualización de la clase usuario.
                                    retorno = us.EditaInformaciónGeneral(txtNombre.Text, txtEmail.Text, Convert.ToByte(txtSesiones.Text), Convert.ToByte(txtTiempo.Text),
                                                                         Convert.ToByte(txtVigencia.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación fuese Exitosa
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Obteniendo Registro de Usuario
                                        id_usuario = retorno.IdRegistro;
                                        
                                        //Instanciando Usuario Compania
                                        using (SAT_CL.Seguridad.UsuarioCompania uc = new SAT_CL.Seguridad.UsuarioCompania(us.id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                        {
                                            //Validando que Exista un registro Usuario - Compania
                                            if (uc.id_usuario_compania > 0)

                                                //Editando Usuario - Compania
                                                retorno = uc.EditaUsuarioCompania(us.id_usuario, uc.id_compania_emisor_receptor, Convert.ToInt32(ddlDepartamento.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                //Valida que la operacion de inserción se realizo correctamente
                if (retorno.OperacionExitosa)
                {
                    //Instanciando Registro de Usuario
                    retorno = new RetornoOperacion(id_usuario);

                    //Completando Transacción
                    trans.Complete();
                }
            }

            //Valida que la operacion de inserción se realizo correctamente
            if (retorno.OperacionExitosa)
            {
                //Asigna el valor de estatus session en modo lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado en la base de datos(id).
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma.
                inicializaForma();
            }

            //Muestra un mensaje acorde a la validación de la operación.
            lblError.Text = retorno.Mensaje;
        }

        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de Usuario</param>
        /// <param name="idTabla">Identificador de la tabla Usuario</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Usuario.
            string url = Cadena.RutaRelativaAAbsoluta("~/Seguridad/Usuario.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Usuario", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla Usuario
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla Usuario registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
            string url = Cadena.RutaRelativaAAbsoluta("~/Seguridad/Usuario.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de Usuario
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Usuario", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Seguridad/Usuario.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla Usuario</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla Usuario en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Usuario
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Seguridad/Usuario.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Usuario
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Usuario", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de Cargar las Sesiones Activas de un Usuario
        /// </summary>
        private void cargaSessionesActivas()
        {
            //Obteniendo Sesiones Activas
            using (DataTable dtSesiones = SAT_CL.Seguridad.UsuarioSesion.ObtieneSesionesActivasUsuario(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtSesiones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvSesionesActivas, dtSesiones, "Id", lblOrdenadoSA.Text, true, 1);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSesiones, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvSesionesActivas);

                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion   
    }
}