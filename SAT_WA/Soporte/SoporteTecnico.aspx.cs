using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Soporte
{
    public partial class SoporteTecnico : System.Web.UI.Page
    {

        private int _id_evaluacion_aplicacion;


        #region Eventos
        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recarga de págin
            if (!Page.IsPostBack)
                inicializaPagina();
        }
        /// <summary>
        /// Click en botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            guardaReporte();
        }
        
        /// <summary>
        /// Click en botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus actual es edición
            if (((TSDK.ASP.Pagina.Estatus)Session["estatus"]) == TSDK.ASP.Pagina.Estatus.Edicion)
                //Pasando a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
            //Inicializando forma
            inicializaPagina();
        }
        /// <summary>
        /// Evento Abrir Ventana Modal Boton Abrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregar_Click(object sender, EventArgs e)
        {

            //Instanciando registro reporte unidad foranea
            using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro existe
                if (r.id_soporte_tecnico > 0)
                {

                    if (r.id_estatus < 3)
                    {
                        alternaVentanaModal("Agregar", this.Page);
                        //ScriptServer.AlternarVentana(btnAgregar, "Soporte", "soporteTecnicoModal", "soporteTecnico");
                    }
                    
                }
            }

        }
       
        /// Click en botón guardar ventana modal "AGREGAR DETALLE SOPORTE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarSoporte_Click(object sender, EventArgs e)
        {
            ReporteDetalleInserta();
            cargaDetallesSoporteTecnicoDetalle();
        }
        /// <summary>
        /// Click en botón cancelar ventana modal "AGREGAR DETALLE SOPORTE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarSoporte_Click(object sender, EventArgs e)
        {
            //Si el estatus actual es edición
            if (((TSDK.ASP.Pagina.Estatus)Session["estatus"]) == TSDK.ASP.Pagina.Estatus.Edicion)
                //Pasando a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
            //Inicializando forma
            inicializaPagina();
        }

        /// Click en botón guardar ventana modal  "SOPORTE TECNICO TERMINAR FECHA"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarSoporteFecha_Click(object sender, EventArgs e)
        {
            TerminaSoporte();
            cargaDetallesSoporteTecnicoDetalle();


        }
        /// Click en botón guardar ventana modal "TERMINA TODOS LOS SOPORTES"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarSoporteTermino_Click(object sender, EventArgs e)
        {
            TerminadaporEstatusGeneral();
            alternaVentanaModal("TerminaDetalle", btnGuardarSoporteTermino);

        }

        /// Click en botón guardar ventana modal "AGREGAR DETALLE SOPORTE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarSoporteEdita_Click(object sender, EventArgs e)
        {
            ReporteDetalleEdita();
            cargaDetallesSoporteTecnicoDetalle();
           
        }
        /// <summary>
        /// Click en botón cancelar ventana modal "AGREGAR DETALLE SOPORTE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarSoporteEdita_Click(object sender, EventArgs e)
        {
            //Si el estatus actual es edición
            if (((TSDK.ASP.Pagina.Estatus)Session["estatus"]) == TSDK.ASP.Pagina.Estatus.Edicion)
                //Pasando a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
            //Inicializando forma
            inicializaPagina();
        }

        /// <summary>
        /// Click en elementos del menú de la forma
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
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaPagina();
                        //Se realiza un enfoque al primer control 
                        txtIdCompaniaEmisora.Focus();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(233, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaReporte(); 
                        break;
                    }
                case "Terminar":
                    {   //Termina el reporte
                        //Instanciando registro reporte unidad foranea
                        using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Si el registro existe
                            if (r.id_soporte_tecnico > 0)
                            {

                                if (r.id_estatus < 3)
                                {
                                    cargaEstatus();
                                }

                            }
                        }

                        
                        break;
                    }
                case "Salir":
                    {

                        break;
                    }
                case "Editar":
                    {
                        using (SAT_CL.Soporte.SoporteTecnico oc = new SAT_CL.Soporte.SoporteTecnico((int)Session["id_registro"]))
                        {
                            //Asigna a la variable session estaus el estado de la pagina nuevo
                            Session["estatus"] = Pagina.Estatus.Edicion;
                            //Invoca el método inicializaForma();
                            inicializaPagina();
                           
                            //Se realiza un enfoque al primer control 
                            txtIdCompaniaEmisora.Focus();
                        }
                        break;
                    }
                //En caso de seleccionar la opción Eliminar del menú.
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Invoca a la clase Actividad
                            using (SAT_CL.Soporte.SoporteTecnico sp = new SAT_CL.Soporte.SoporteTecnico((int)Session["id_registro"]))
                            {
                                //Valida que exista el registro
                                if (sp.id_soporte_tecnico > 0)
                                    //Asigna valores al objeto retorno
                                    retorno = sp.DeshabilitaSoporteTecnico(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Instancia a la clase requisición
                                //Valida si la inserción a la base de datos se realizo correctamente
                            }
                            //Valida si la inserción a la base de datos se realizo correctamente
                            if (retorno.OperacionExitosa)

                                trans.Complete();

                        }
                        //Valida si la inserción a la base de datos se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //A la variable de sessión estatus le asigna el estado de la pagina en modo Nuevo
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //A la variable de session id_registro le asigna el valor insertado en la tabla Actividad
                            Session["id_registro"] = 0;
                            //Invoca al método inicializa forma
                            inicializaPagina();
                        }
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "233", "SoporteTecnico");
                        break;
                    }

                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "233", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }

            }
        }
        #endregion

       

        #region Métodos

         /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaPagina()
        {
            //Habilitando controles
            habilitaControles();
            //Habilitando menú
            habilitaMenu();
            //Cargando catalogos
            cargaCatalogos();
            //Cargando contenido de controles
           cargaContenidoControles();

        }
        /// <summary>
        /// Habilita o deshabilita loc controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControles()
        {
            //En base al estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                    //Habilitara los controles a exepción de No Consecutivo
                    //Campos
                    txtIdCompaniaEmisora.Enabled = false;
                    txtNoConsecutivoCompania.Enabled = false;
                    txtIdEstatus.Enabled = false;
                    txtIdUsuarioAsistente.Enabled = false;
                    txtUsuarioSolicitante.Enabled = true;
                    txtFechaTerminoGeneral.Enabled =false;
                    txtFechaInicioGeneral.Enabled = false;
                    btnCancelar.Enabled =
                    btnGuardar.Enabled = true;
                    //Campo ventana modal 
                    txtIdSoporteE.Enabled = true;
                    txtObservacionE.Enabled = true;
                    txtFechaInicioE.Enabled = true;
                    //txtFechaTermino.Enabled = false;
                    btnCancelarSoporte.Enabled = true;
                    btnGuardarSoporte.Enabled = true;
                    btnAgregar.Enabled = false;
                    gvSoporteTecnico.Enabled = false;
                    break;
                    }

                case Pagina.Estatus.Edicion:
                    { 
                    //Habilitara los controles a exepción de No Consecutivo
                    //Campos
                    txtIdCompaniaEmisora.Enabled = false;
                    txtNoConsecutivoCompania.Enabled = false;
                    txtIdEstatus.Enabled = false;
                    txtIdUsuarioAsistente.Enabled = false;
                    txtUsuarioSolicitante.Enabled = true;
                    txtFechaTerminoGeneral.Enabled = true;
                    txtFechaInicioGeneral.Enabled = false;
                    btnCancelar.Enabled =
                     btnAgregar.Enabled = true;
                    //Campo ventana modal
                    txtIdSoporteE.Enabled = true;
                    txtObservacionE.Enabled = true;
                    txtFechaInicioE.Enabled = true;
                    btnGuardarSoporteEdita.Enabled = true;
                    btnGuardarSoporteEdita.Enabled = true;                   
                    btnCancelarSoporte.Enabled = true;
                    btnGuardarSoporte.Enabled = true;              
                    gvSoporteTecnico.Enabled = true;
                    break;
                     }
                case Pagina.Estatus.Lectura:
                    {
                    //Habilitara los controles a exepción de No Consecutivo
                    txtIdCompaniaEmisora.Enabled = false;
                    txtNoConsecutivoCompania.Enabled = false;
                    txtIdEstatus.Enabled = false;
                    txtIdUsuarioAsistente.Enabled = false;
                    txtUsuarioSolicitante.Enabled =
                    txtFechaTerminoGeneral.Enabled = false;
                    txtFechaInicioGeneral.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnCancelar.Enabled =
                    //Campo ventana modal
                    txtIdSoporteE.Enabled = true;
                    txtObservacionE.Enabled = true;
                    txtFechaInicioE.Enabled = true;
                    btnGuardarSoporteEdita.Enabled = true;
                    btnGuardarSoporteEdita.Enabled = true;
                    //txtFechaTermino.Enabled = false;
                    btnCancelarSoporte.Enabled = true;
                    btnGuardarSoporte.Enabled = false;
                    btnAgregar.Enabled = false;
                    gvSoporteTecnico.Enabled = false;
                    break;    
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled =
                        lkbTerminar.Enabled = false;
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        btnAgregar.Enabled = false;
                        //Edicion
                        lkbEliminar.Enabled = false;
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbTerminar.Enabled = true;
                        btnGuardar.Enabled = false;
                        btnCancelar.Enabled = true;
                        btnAgregar.Enabled = false;
                        //Edicion
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled = 
                        lkbReferencias.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled =
                        lkbTerminar.Enabled = true;
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        lkbEliminar.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Carga el conjunto de catalogos de la forma sobre los controles DropDownList
        /// </summary>
        private void cargaCatalogos()
        {
            //Tipos de estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(txtIdEstatus, "", 3204);
            //Tipos de soporte
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(txtIdSoporte, "", 3203);
            //Tipos de soporte
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(txtIdSoporteE, "", 3203);
            ////Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
           
        }
        /// <summary>
        /// Instancía un objeto TMS_CL.Global.Operador y asigna el valor de sus atributos sobre controles de asp
        /// </summary>
        private void cargaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Borrando el contenido 
                        //Formulario Soporte Tecnico
                        lblId.Text = "Por Asignar";
                        txtIdCompaniaEmisora.Text = "";
                        txtNoConsecutivoCompania.Text = "Por Asignar";
                        txtUsuarioSolicitante.Text = "";
                        txtIdUsuarioAsistente.Text = "";
                        txtFechaTerminoGeneral.Text = "";
                        txtFechaInicioGeneral.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Formulario Agregar
                        txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Formulario Editar Soporte Tecnico Detalle
                        txtFechaInicioE.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Formulario
                        txtFechaTerminoDetalle.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        Controles.InicializaGridview(gvSoporteTecnico);
                        //Instanciando Compañía de la sesión de usuario
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtIdCompaniaEmisora.Text = emisor.nombre + " ID:" + emisor.id_compania_emisor_receptor.ToString();
                        }

                        using (SAT_CL.Seguridad.Usuario sp = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtIdUsuarioAsistente.Text =sp.nombre+ "ID:" + sp.id_usuario.ToString();
                        }  
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando registro reporte unidad foranea
                    using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (r.id_soporte_tecnico > 0)
                        {
                            //Borrando el contenido 
                            txtNoConsecutivoCompania.Text = r.no_consecutivo_compania.ToString();
                            lblId.Text = r.id_soporte_tecnico.ToString();
                          
                            //Instanciando Compañía de la sesión de usuario
                            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(r.id_compania_emisora))
                            {
                                //Valida que el registro exista
                                if (emisor.id_compania_emisor_receptor > 0)
                                    //Asigna al control los nombres y id de cada compañia
                                    txtIdCompaniaEmisora.Text = string.Format("{0} ID:{1}", emisor.nombre, emisor.id_compania_emisor_receptor);
                            }
                            //Instanciando Compañía de la sesión de usuario
                             using (SAT_CL.Seguridad.Usuario sp = new SAT_CL.Seguridad.Usuario(r.id_usuario_asistente))
                            {
                                    //Asigna al control los nombres y id de cada compañia
                                    if (sp.id_usuario > 0)
                                    txtIdUsuarioAsistente.Text = string.Format("{0} ID:{1}", sp.nombre, r.id_usuario_asistente);
                            }
                            //Formulario Soporte Tecnico
                            txtIdEstatus.Text = r.id_estatus.ToString();
                            txtUsuarioSolicitante.Text = r.usuario_solicitante.ToString();
                            txtFechaInicioGeneral.Text = r.fecha_inicio_general.ToString("dd/MM/yyyy HH:mm");                        
                            txtFechaTerminoGeneral.Text = r.fecha_termino_general.ToString("dd/MM/yyyy HH:mm");
                            DateTime FF;
                            FF = DateTime.MinValue;
                            if (Convert.ToDateTime(txtFechaTerminoGeneral.Text).CompareTo(FF) == 0)
                            {
                                txtFechaTerminoGeneral.Text = "";

                            }
                            //Formulario Agregar
                            //txtIdSoporte.Text = "";
                            txtObservacion.Text = "";
                            txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                            //Formulario Termino
                            txtFechaTerminoDetalle.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                            //Inicializando Indices
                            Controles.InicializaIndices(gvSoporteTecnico);
                            cargaDetallesSoporteTecnicoDetalle();
                        }
                    }
                    using (SAT_CL.Soporte.SoporteTecnicoDetalle otf = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(_id_evaluacion_aplicacion)))
                    {
                        if (otf.id_soporte_tecnico_detalle > 0)
                        {
                            txtIdSoporteE.Text = otf.id_tipo_soporte.ToString();
                            txtObservacionE.Text = otf.observacion.ToString();
                            txtFechaInicioE.Text = otf.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                          
                          
                        }
                    }
                    break;

            }

            //Limpiando errores
            lblError.Text = "";
        }
       
        /// <summary>
        /// Inserta o Actualiza los valores del registro
        /// </summary>
        private void guardaReporte()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
                //Valida cada estado de la página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //En caso de que el estado de la página sea nuevo
                    case Pagina.Estatus.Nuevo:
                        {
                            //Asigna al objeto retorno los datos ingresados invocando al método inserción de la clase ordenCompra


                            resultado = SAT_CL.Soporte.SoporteTecnico.InsertaSoporteTecnico(
                                         ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                         0,Convert.ToByte(txtIdEstatus.SelectedValue),
                                         Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtIdUsuarioAsistente.Text, "ID:", 1)),
                                         Cadena.VerificaCadenaVacia(txtUsuarioSolicitante.Text.ToUpper(),""), Convert.ToDateTime( txtFechaInicioGeneral.Text)
                                        ,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no hay errores de guardado nuevo
                        if (resultado.OperacionExitosa)
                        {
                            //Guardando datos de registro para carga de estatus de lectura
                            Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                            Session["id_registro"] = resultado.IdRegistro;
                            inicializaPagina();
                        }

                        break;
                        }
                    //En caso de que el estado de la página sea edicion
                    case Pagina.Estatus.Edicion:
                        {
                            //Invoca al constructor de la clase OrdenCompra para poder instanciar sus  métodos 
                            using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                            {
                                
                                    if (r.id_soporte_tecnico > 0)
                                {
                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra

                                    resultado = r.EditaSoporteTecnicoFechaF(
                                       ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                        Convert.ToInt32(txtNoConsecutivoCompania.Text), Convert.ToByte(txtIdEstatus.SelectedValue),
                                         Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtIdUsuarioAsistente.Text, "ID:", 1)),
                                         Cadena.VerificaCadenaVacia(txtUsuarioSolicitante.Text.ToUpper(), ""), Convert.ToDateTime(txtFechaInicioGeneral.Text),
                                          ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                        //Si no hay errores de guardado edicion
                        if (resultado.OperacionExitosa)
                        {
                            //Guardando datos de registro para carga de estatus de lectura
                            Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                            Session["id_registro"] = resultado.IdRegistro;
                            inicializaPagina();
                        }
                        break;
                        }
                }
               
            
            //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        /// <summary>
        /// Inserta o Actualiza los valores del registro
        /// </summary>
        private void ReporteDetalleInserta()
        {
          //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
                            resultado = SAT_CL.Soporte.SoporteTecnicoDetalle.InsertaSoporteTecnicoDetalle(Convert.ToInt32(lblId.Text),
                                              Convert.ToByte(txtIdSoporte.SelectedValue), Convert.ToByte(txtIdEstatus.SelectedValue),
                                              Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(null),
                                              Cadena.VerificaCadenaVacia(txtObservacion.Text.ToUpper(), ""), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Si no hay errores de guardado
                if (resultado.OperacionExitosa)
                {
                    //Guardando datos de registro para carga de estatus de lectura
                    Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                    inicializaPagina();
                }
            
            //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Edita o Actualiza los valores del registro
        /// </summary>
        private void ReporteDetalleEdita()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Invoca al constructor de la clase OrdenCompra para poder instanciar sus  métodos
            using (SAT_CL.Soporte.SoporteTecnicoDetalle otf = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(gvSoporteTecnico.SelectedDataKey["Id"])))  
            {
                if (otf.id_soporte_tecnico_detalle > 0)
                 {     

                //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición soporte tecnico detalle
                resultado = otf.EditaSoporteTecnicoDetalleVentanaModal(Convert.ToByte(txtIdSoporteE.SelectedValue),
                Convert.ToDateTime(txtFechaInicioE.Text), Cadena.VerificaCadenaVacia(txtObservacionE.Text.ToUpper(), ""),
                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                _id_evaluacion_aplicacion = otf.id_soporte_tecnico_detalle;
            }
            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
     
                //Guardando datos de registro para carga de estatus de lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                inicializaPagina();           
            }
            //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Metodo encargado de terminar el link de soporte detalles
        /// </summary>
        private void TerminaSoporte()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Obteniendo Fechas
            DateTime fec_ini, fec_fin;
            DateTime.TryParse(txtFechaInicio.Text , out fec_ini);
            DateTime.TryParse(txtFechaTermino.Text, out fec_fin);

            //Invoca al método validaFecha  y asigna el resultado del método al objeto retorno.
            resultado = validaFecha();
            //Valida si el resultado del método se realizo correctamente (La validación de las Fechas)
            if (resultado.OperacionExitosa)
            {
                //Valida cada estado de la página
                switch ((Pagina.Estatus)Session["estatus"])
               {
                    //En caso de que el estado de la página sea edicion
                    case Pagina.Estatus.Edicion:
                       {                     
                           //Instanciando Falla
                           using (SAT_CL.Soporte.SoporteTecnicoDetalle otf = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(gvSoporteTecnico.SelectedDataKey["Id"])))
                           {
                          
                                if (otf.id_soporte_tecnico_detalle> 0)
                                {
                                    //Asigna al objeto retorno los datos ingresados en los controles invocando el método de edición de la clase ordenCompra
                                    resultado = otf.EditaSoporteTecnicoDetalle(Convert.ToDateTime(txtFechaTermino.Text),
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                              
                        }
                            break;
                        }
                }
                //Si no hay errores de guardado
              
            }
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                alternaVentanaModal("Terminar", this.Page);
                inicializaPagina();
            }

            //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles soporteDetalle
        /// </summary>
        private void cargaDetallesSoporteTecnicoDetalle()
        {
            //Invoca al dataset para inicializar los valores del gridview si existe en relación a una orden de compra
            //using (DataTable dtOrdenCompraDetalle = SAT_CL.Almacen.OrdenCompraDetalle.CargaDetallesOrdenCompra((int)Session["id_registro"]))
            using (DataTable dtSoporteTecnico = SAT_CL.Soporte.SoporteTecnicoDetalle.ObtieneDetalleSoporte((int)Session["id_registro"]))
            {
                //Valida si existen los datos del datase
                if (Validacion.ValidaOrigenDatos(dtSoporteTecnico))
                {
                    //Si existen, carga los valores del datatable al gridview
                    Controles.CargaGridView(gvSoporteTecnico, dtSoporteTecnico, "Id-Soporte-Fecha Inicio", "");
                    //Asigna a la variable de sesion los datos del dataset invocando al método AñadeTablaDataSet
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSoporteTecnico, "Table");
                }
                //Si no existen
                else
                {
                    //Inicializa el gridView 
                    Controles.InicializaGridview(gvSoporteTecnico);
                    //Elimina los datos del dataset si se realizo una consulta anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        } 
        /// <summary>
        /// Método encargado de Cargar los estatus para terminar el detalle
        /// </summary>
        protected void cargaEstatus()
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();
            //Creación 
            string variable1 = SAT_CL.Soporte.SoporteTecnicoDetalle.CargaEstatusS(Convert.ToInt32(Session["id_registro"]));

            if (variable1 == "Registrado")
            {
                alternaVentanaModal("TerminaDetalle", this.Page);
            }
            else
            {
                using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                {
                    //ScriptServer.AlternarVentana(this, "Soporte2", "soporteTecnico2Modal", "soporteTecnico2");
                    alternaVentanaModal("TerminaDetalle", this.Page);
                }

                if (result.OperacionExitosa)
                {
                    //Guardando datos de registro para carga de estatus de lectura
                    Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                    inicializaPagina();
                }

                //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            
        }
        /// <summary>
        /// Método encargado de Cargar los estatus para terminar el detalle
        /// </summary>
        protected void TerminadaporEstatusGeneral()
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo Fechas
            DateTime fec_ini, fec_fin;
            DateTime.TryParse(txtFechaInicioGeneral.Text, out fec_ini);
            DateTime.TryParse(txtFechaTerminoDetalle.Text, out fec_fin);
            //Invoca al método validaFecha  y asigna el resultado del método al objeto retorno.
            result = validaFechaTermino();
             //Valida si el resultado del método se realizo correctamente (La validación de las Fechas)
            if (result.OperacionExitosa)
            {
            //Invoca al dataset para inicializar los valores del gridview si existe en relación a una orden de compra
            //using (DataTable dtOrdenCompraDetalle = SAT_CL.Almacen.OrdenCompraDetalle.CargaDetallesOrdenCompra((int)Session["id_registro"]))
            using (DataTable dtSoporteTecnico = SAT_CL.Soporte.SoporteTecnicoDetalle.TerminarReportes((int)Session["id_registro"]))
            {
                if (Validacion.ValidaOrigenDatos(dtSoporteTecnico))
                {
                    List<DataRow> SoporteTecnicoDetallesEstatus = (from DataRow dep in dtSoporteTecnico.AsEnumerable()
                                               //where Convert.ToInt32(dep["Id"]) == 2
                                              select dep).ToList();
                    if (SoporteTecnicoDetallesEstatus.Count > 0)
                    {
                        foreach (DataRow row in SoporteTecnicoDetallesEstatus)
                        {
                            using (SAT_CL.Soporte.SoporteTecnicoDetalle r = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(row["Id"])))
                            {
                                result = r.EditaSoporteTecnicoDetalle(Convert.ToDateTime(txtFechaTerminoDetalle.Text),
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                            //using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                            //{
                            //    result = r.EditaSoporteTecnicoEstatus(3,
                            //    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //}

                            using (SAT_CL.Soporte.SoporteTecnico r = new SAT_CL.Soporte.SoporteTecnico(Convert.ToInt32(Session["id_registro"])))
                            {
                                result = r.TerminaSoporteTecnico(Convert.ToDateTime(txtFechaTerminoDetalle.Text),
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            }
                        }
             
                }
            }
         
                
            }
            if (result.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
               
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaPagina();
                /// Cierra ventana modal "Soporte Tecnico Detalles Termino"
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
            

            }
            //Mostrando Mensaje
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de validar la fecha 
        /// </summary>
        private RetornoOperacion validaFecha()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if (Convert.ToDateTime(txtFechaInicio.Text).CompareTo(Convert.ToDateTime(txtFechaTermino.Text)) > 0)
            {
                //Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha de Solicitud debe ser MENOR que Fecha Fecha Compromiso.");
            }
            //Retorna el resultado al método 
            return retorno;
        }

        /// <summary>
        /// Método encargado de validar la fecha 
        /// </summary>
        private RetornoOperacion validaFechaTermino()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if (Convert.ToDateTime(txtFechaInicioGeneral.Text).CompareTo(Convert.ToDateTime(txtFechaTerminoDetalle.Text)) > 0)
            {
                //Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha de Solicitud debe ser MENOR que Fecha Fecha Compromiso.");
            }
            //Retorna el resultado al método 
            return retorno;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = Cadena.RutaRelativaAAbsoluta("~/Soporte/SoporteTecnico.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int id_operador_captura)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = string.Format("{0}?P1={1}", Cadena.RutaRelativaAAbsoluta("~/Soporte/SoporteTecnico.aspx", "~/Accesorios/AbrirRegistro.aspx"), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_operador_captura">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_operador_captura)
        {   //Declarando variable para armado de URL
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Soporte/SoporteTecnico.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_operador_captura);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Soporte/SoporteTecnico.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Método abre ventanas modales de manera dinamica
        /// </summary>
        /// <param name="nombre_ventana"></param>
        /// <param name="control"></param>
        private void alternaVentanaModal(string nombre_ventana, Control control)
        {
            //Determina que modal abrira
            switch (nombre_ventana)
            {
                case "Terminar":
                    //ScriptServer.AlternarVentana(control, nombre_ventana, "ventanaConsultaMedica", "contenedorVentanaConsultaMedica");
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SoporteTerminar", "soporteTecnicoModalTerminar", "soporteTecnicoTerminar");
                    break;
                case "Edicion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SoporteEdicion", "soporteTecnicoModalEdicion", "soporteTecnicoEdicion");
                    break;
                case "Agregar":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SoporteAgregar", "soporteTecnicoModalAgregar", "soporteTecnicoAgregar");
                    break;
                case "TerminaDetalle":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "SoporteTerminoDetalle", "soporteTecnicoModalTerminoDetalle", "soporteTecnicoTerminoDetalle");
                    break;
            }
        }

        /// <summary>
        /// Evento producido al dar click en boton cerrar ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Validando que LinkButton se presiono
            switch (((LinkButton)sender).CommandName)
            {
                case "Terminar":
                    alternaVentanaModal("Terminar", (LinkButton)sender);
                    break;
                case "Edicion":
                    alternaVentanaModal("Edicion", (LinkButton)sender);
                    break;
                case "Agregar":
                    alternaVentanaModal("Agregar", (LinkButton)sender);
                    break;
                case "TerminaDetalle":
                    alternaVentanaModal("TerminaDetalle", (LinkButton)sender);
                    break;
            }
        }

        #endregion


        #region Eventos GridView "SoporteTecnico"

        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSoporteTecnico_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvSoporteTecnico, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Soporte Tecnico"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvSoporteTecnico, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Soporte Tecnico"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSoporteTecnico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSoporteTecnico, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }

        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Soporte Tecnico Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento que permite cargar los valores a los controles de la modal solo si se desea realizar una edición.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvSoporteTecnico.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvSoporteTecnico, sender, "lnk", false);

                //Invoca al Constructor de la clase ordenCompradetalle  para obtener el detalle de orden de compra
                using (SAT_CL.Soporte.SoporteTecnicoDetalle detalle = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(gvSoporteTecnico.SelectedDataKey["Id"])))
                {
                    //Valida que el valor del estatus sea diferente de 2(solicitada)
                    if (detalle.estatus == SAT_CL.Soporte.SoporteTecnicoDetalle.Estatus.Registrado || detalle.estatus == SAT_CL.Soporte.SoporteTecnicoDetalle.Estatus.Iniciado)
                    {

                        //Validando que Exista
                        if (detalle.id_soporte_tecnico_detalle > 0)
                        {
                            //Asignando valores
                            txtIdSoporteE.Text = detalle.id_tipo_soporte.ToString();
                            txtFechaInicioE.Text = detalle.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                            txtObservacionE.Text = detalle.observacion;
                            alternaVentanaModal("Edicion", this.Page);
                            //ScriptServer.AlternarVentana(this, "Soporte3", "soporteTecnico3Modal", "soporteTecnico3");
                        }

                    }
                    else
                    {
                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(this, "El Detalle debe de estar en Estatus 'Registrada' ó 'Solicitada' para su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Inicializando Indices
                        Controles.InicializaIndices(gvSoporteTecnico);
                    }
                }
            }
        }
        /// <summary>
        /// Evento producido al Eliminar el Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvSoporteTecnico.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvSoporteTecnico, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Detalle
                using (SAT_CL.Soporte.SoporteTecnicoDetalle detalle = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(gvSoporteTecnico.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Detalle
                    if (detalle.habilitar)
                    {
                        //Validando que este en Estatus Registrado
                        if (detalle.estatus == SAT_CL.Soporte.SoporteTecnicoDetalle.Estatus.Registrado)

                            //Deshabilitando Detalle
                            result = detalle.DeshabilitaSoporteTecnicoDetalle(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("El Detalle debe de estar en Estatus 'Registrado' para poder Eliminarlo");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede Acceder al Detalle, es posible que se haya Eliminado");

                    //Si la Operación fue Exitosa
                    if (result.OperacionExitosa)

                        //Cargando Detalles
                        cargaDetallesSoporteTecnicoDetalle();

                    //Inicializando Indices
                    Controles.InicializaIndices(gvSoporteTecnico);
                }
                if (result.OperacionExitosa)
                {
                    //Guardando datos de registro para carga de estatus de lectura
                    Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                    inicializaPagina();
                }

                //Mostrando Mensaje de la Operación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Editar el campo de grid "Soporte Tecnico Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkTerminarSoporte_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros en el gridView
            if (gvSoporteTecnico.DataKeys.Count > 0)
            {
                //Seleccionando Fila del grid view para editar los valores
                TSDK.ASP.Controles.SeleccionaFila(gvSoporteTecnico, sender, "lnk", false);

                //Invoca al Constructor de la clase ordenCompradetalle  para obtener el detalle de orden de compra
                using (SAT_CL.Soporte.SoporteTecnicoDetalle detalle = new SAT_CL.Soporte.SoporteTecnicoDetalle(Convert.ToInt32(gvSoporteTecnico.SelectedDataKey["Id"])))
                {
                    //Valida que el valor del estatus sea diferente de 2(solicitada)
                    if (detalle.estatus == SAT_CL.Soporte.SoporteTecnicoDetalle.Estatus.Registrado || detalle.estatus == SAT_CL.Soporte.SoporteTecnicoDetalle.Estatus.Iniciado)
                    {

                        //Validando que Exista
                        if (detalle.id_soporte_tecnico_detalle > 0)
                        {
                            //Asignando valores
                            txtFechaInicio.Text = detalle.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                            txtFechaTermino.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            alternaVentanaModal("Terminar", this.Page);
                      
                        }

                    }
                    else
                    {
                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(this, "El Detalle debe de estar en Estatus 'Registrada' ó 'Solicitada' para su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Inicializando Indices
                        Controles.InicializaIndices(gvSoporteTecnico);
                    }
                }
            }
        }
        /// <summary>
        /// Editar el campo de grid "Soporte Tecnico Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSoporteTecnico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Cargando Menu Contextual
            Controles.CreaMenuContextual(e, "menuContext", "menuOptions", "MostrarMenu", true, true);
        }


        #endregion

       

    }
}