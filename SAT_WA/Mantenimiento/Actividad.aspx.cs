using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;

namespace SAT.Mantenimiento
{
    public partial class Actividad : System.Web.UI.Page
    {
        #region Eventos
        #region Eventos Forma
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida la carga inicial de la página
            //Valida si la página se esta cargado por primera vez.
            if (!Page.IsPostBack)
                //Invoca al método inicializarForma
                inicializaForma();
        }
        /// <summary>
        /// Evento que ejecuta los estados  del menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite validar cada una de las opciones del ménu.
            switch (botonMenu.CommandName)
            {
                //en caso de seleccionar la opción de Nuevo en el menú.
                case "Nuevo":
                    {
                        //A la variable estatus de la sesion, se le asigna el valor de pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        // La variable id_registro se le inicializa en cero.
                        Session["id_registro"] = 0;
                        //Se realiza un enfoque al primer control 
                        txtDescripcion.Focus();
                        //Inicializa el estado de la forma.
                        inicializaForma();
                        break;
                    }

                case "Abrir":
                    {
                        //Invoca al método inicializaApertura para inicializar la apertura de un registro de Actividad
                        inicializaAperturaRegistro(134, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                case "Guardar":
                    {
                        guardarActividad();
                        break;
                    }
                //En caso de seleccionar la opción Editar del menú.
                case "Editar":
                    {
                        //Se asigna a la sesion el estado de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Se realiza un enfoque al primer control 
                        txtDescripcion.Focus();
                        //Invoca al metodo inicializaFormana
                        inicializaForma();
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
                            using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
                            {
                                //Valida que exista el registro
                                if (act.id_actividad > 0)
                                    //Asigna valores al objeto retorno
                                    retorno = act.DeshabilitarActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Instancia a la clase requisición
                                //Valida si la inserción a la base de datos se realizo correctamente
                                if (retorno.OperacionExitosa)
                                {
                                    using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(act.id_requisicion))
                                    {
                                        //Asigna Valores al objeto retorno
                                        retorno = req.DeshabilitaRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                }
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
                            inicializaForma();
                        }
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Bitacora":
                    {
                        //Invoca al método inicializaBitacora que muestra las modificaciones sobre un registro de Actividad
                        inicializaBitacora(Session["id_registro"].ToString(), "134", "Actividad");
                        break;
                    }
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de Actividad
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "134",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Producto":
                    {
                        inicializaControlUsuario();
                        ScriptServer.AlternarVentana(lkbProducto, lkbProducto.GetType(), "AbrirVentana", "RequisicionModal", "Requisicion");
                        break;
                    }

            }
        }
        /// <summary>
        /// Evento que se ejecuta cuando se da clic al botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método que almacena las actividades
            guardarActividad();
        }
        /// <summary>
        /// Evento que se ejecutara cuando se de clic en el boton cancela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Válida cada estado de la pagina y ejecuta
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //A la variable id_registro se le asigna el valor de 0
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea edición
                case Pagina.Estatus.Edicion:
                    {
                        //A la variable estatus se le asigna el estado de lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método que inicializa los valores del formulario.
            inicializaForma();

        }
        /// <summary>
        /// Evento que carga el catalogo al dropdownlist subFamilia dependiendo de la opción seleccionada del dropdownlist Familia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga los valores al dropdownlist SubFamilia
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubFamilia, "--No Aplica", 1122, Convert.ToInt32(ddlFamilia.SelectedValue));
        }
        /// <summary>
        /// Evento que carga el catalogo al dropdownlist subtipounidad dependiendo de la opción seleccionada del dropdownlist tipounidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga los valores al dropdownlist Subtipo de Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipoUnidad, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
        }
        /// <summary>
        /// Evento que Cierra la ventana modal que agrega Actividades.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrarImagen, lnkCerrarImagen.GetType(), "CerrarVentana", "RequisicionModal", "Requisicion");
            //Invoca al método que inicializa los valores del grid view Producto
            cargaProducto();
        }
        #endregion

        #region Eventos Actividad Puesto
        /// <summary>
        /// Evento que almacena una actividad puesto solo cuando se crea una actividad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardaActividadPuesto_Click(object sender, EventArgs e)
        {
            //Invoca al método guardar Actividad Puesto
            guardaActividadPuesto();
        }
        /// <summary>
        /// Evento qeu cancela las acciones a realizar sobre un registro del GridView Actividad Puesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarActividadPuesto_Click(object sender, EventArgs e)
        {
            //Invoca al método que inicializa los valores de la forma
            inicializaForma();
            //Carga los valores al gridView Actividad Puesto
            cargaActividadPuesto();
        }
        /// <summary>
        /// Evento que Elimina el Puesto a una actividad asignada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida que esictan registros en el gridView
            if (gvActividadPuesto.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a editar
                Controles.SeleccionaFila(gvActividadPuesto, sender, "lnk", false);
                //Invoca a la clase Actividad Detalle
                using (SAT_CL.Mantenimiento.ActividadPuesto actPuesto = new SAT_CL.Mantenimiento.ActividadPuesto((int)gvActividadPuesto.SelectedValue))
                {
                    //Asigna a retorno el resultado de invocar el método DeshabilitarActividadRegistro
                    retorno = actPuesto.DeshabilitarActividadRegistro(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //Valida la acción de eliminar
                if (retorno.OperacionExitosa)
                {
                    //Invoca al método que inicializa la Forma
                    inicializaForma();
                    //Envia mensaje 
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }            

        }
        /// <summary>
        /// Evento que Edita el Puesto a una actividad asignada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridView
            if (gvActividadPuesto.DataKeys.Count > 0)
            {
                //Selecciona el registro que se va a editar
                Controles.SeleccionaFila(gvActividadPuesto, sender, "lnk", false);
                //Invoca a la clase Actividad Detalle
                using (SAT_CL.Mantenimiento.ActividadPuesto actPuesto = new SAT_CL.Mantenimiento.ActividadPuesto((int)gvActividadPuesto.SelectedValue))
                {
                    //Carga los controles para su edición
                    txtTiempoActividad.Text = actPuesto.tiempo_actividad.ToString();
                    ddlPuesto.SelectedValue = actPuesto.id_puesto.ToString();
                    //Focaliza el cursor en txtTiempoActividad
                    txtTiempoActividad.Focus();

                }
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvActividadPuesto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
            //Invoca al método que calcula el precio de los productos
            calculaTotalPuesto();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Ordenes de Actividades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Actividades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadPuesto_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenar.Text = Controles.CambiaSortExpressionGridView(gvActividadPuesto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            //Invoca al método que calcula el precio de los productos
            calculaTotalPuesto();

        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Actividades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadPuesto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvActividadPuesto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            //Invoca al método que calcula el precio de los productos
            calculaTotalPuesto();
        }
       
        #endregion

        #region Eventos Control de Usuario
        /// <summary>
        /// Evento que almacena una requicisión 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucRequisicion_ClickGuardarRequisicion(object sender, EventArgs e)
        {
            //Creacion del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignación de valores al objeto retorno
            retorno = wucRequisicion.GuardaRequisicion();
            //Valida resultado de la operación
            if (retorno.OperacionExitosa)
                //Inicializa control de usuario
                wucRequisicion.InicializaRequisicion(retorno.IdRegistro, 0, 0, 0, true,false);
            //Envia mensaje de contifmación de almacenamiento de requisición
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento que solicita una requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucRequisicion_ClickSolicitarRequisicion(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignación de valores al objeto retorno
            retorno = wucRequisicion.SolicitaRequisicion();
            //Valida el resultado de la operación
            if (retorno.OperacionExitosa)
                //Envia mensaje de confirmación de operación
                ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        #endregion

        #region Evento Producto
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Producto"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoProducto.SelectedValue));
            //Invovca al método que calcula el monto total de los productos
            calculaTotalProducto();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Producto"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarProducto_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProducto_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenarProducto.Text = Controles.CambiaSortExpressionGridView(gvProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
            //Invovca al método que calcula el monto total de los productos
            calculaTotalProducto();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvProducto, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
            //Invovca al método que calcula el monto total de los productos
            calculaTotalProducto();
        }       
       
        #endregion

        #endregion
        #region Método Privados
        /// <summary>
        /// Método encargado de inicializar todos los aspectos de la página
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método cargaCatalogos();
            cargaCatalogos();
            //Invoca al método habilitaControles().
            habilitaControles();
            //Invoca al método habilitaMenu().
            habilitaMenu();
            //Invoca al método inicializaControlUsuario().
            inicializaControlUsuario();
            //Invoca al método que inicializa el gridview Actividad Puesto
            cargaActividadPuesto();
            //Invoca al método que inicializa al grid View Producto
            cargaProducto();
            //Invoca al método inicializaValores().
            inicializaValores();

        }
        /// <summary>
        /// Método que inicializa los valores de los controles dropdown list
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga los valores al dropdownlist Familia
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlFamilia, "", 1121);
            //Carga los valores al dropdownlist SubFamilia
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubFamilia, "--No Aplica", 1122, Convert.ToInt32(ddlFamilia.SelectedValue));
            //Carga los valores al dropdownlist Tipo Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "");
            //Carga los valores al dropdownlist Subtipo de Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipoUnidad, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
            //Carga los valores al dropdownlist puesto
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPuesto, "", 3158);
            //Cargando los valores del dropdownlist Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 39);
            //Cargando los valores del dropdownlist TamañoProducto
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoProducto, "", 39);
        }
        /// <summary>
        /// Método que hablita los controles de acuerdo al estado de la págia
        /// </summary>
        private void habilitaControles()
        {
            //Valida el estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En casod e que la página este en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        gvActividadPuesto.Enabled = false;
                        //Deshabilita los controles de actividad puesto
                        txtTiempoActividad.Enabled =
                        ddlPuesto.Enabled =
                        btnGuardaActividadPuesto.Enabled = false;
                        //Habilita los controles para su uso
                        txtDescripcion.Enabled =
                        ddlFamilia.Enabled =
                        ddlSubFamilia.Enabled =
                        ddlTipoUnidad.Enabled =
                        ddlSubTipoUnidad.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        wucRequisicion.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        gvActividadPuesto.Enabled = 
                        //Habilita los controles para su uso
                        txtTiempoActividad.Enabled =
                        ddlPuesto.Enabled =
                        btnGuardaActividadPuesto.Enabled =
                        txtDescripcion.Enabled =
                        ddlFamilia.Enabled =
                        ddlSubFamilia.Enabled =
                        ddlTipoUnidad.Enabled =
                        ddlSubTipoUnidad.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = 
                        wucRequisicion.Enabled = true;
                        break;
                    }
                //En caso de que la página este en Lectura
                case Pagina.Estatus.Lectura:
                    {
                        gvActividadPuesto.Enabled =
                        //Deshabilita los controles para su uso
                        txtTiempoActividad.Enabled =
                        ddlPuesto.Enabled =
                        btnGuardaActividadPuesto.Enabled =
                        txtDescripcion.Enabled =
                        ddlFamilia.Enabled =
                        ddlSubFamilia.Enabled =
                        ddlTipoUnidad.Enabled =
                        ddlSubTipoUnidad.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        wucRequisicion.Enabled = false;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que habilita las opciones del ménu acorde al estatus de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en modo nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = 
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        btnCancelarActividadPuesto.Enabled =
                        lkbProducto.Enabled = 
                        btnGuardaActividadPuesto.Enabled = false;
                        btnGuardar.Enabled = 
                        btnCancelar.Enabled = true;
                        break;
                    }
                //Si se encuentra en modo Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = 
                        lkbGuardar.Enabled = 
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled = 
                        lkbReferencias.Enabled = 
                        btnCancelarActividadPuesto.Enabled =
                        btnGuardaActividadPuesto.Enabled = 
                        btnCancelar.Enabled =
                        lkbProducto.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //Si se encuentra en modo Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Habilita y Deshabilita las opciones del menú acorde a su funcionamiento
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled = 
                        lkbEditar.Enabled = 
                        lkbBitacora.Enabled =
                        lkbProducto.Enabled = 
                        lkbReferencias.Enabled = true;
                        btnCancelarActividadPuesto.Enabled = 
                        btnGuardaActividadPuesto.Enabled = 
                        btnCancelar.Enabled =
                        btnGuardar.Enabled = false;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que iniciliza el control de usuario
        /// </summary>
        private void inicializaControlUsuario()
        {
            //Instancia a la clase actividad
            using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
            {
                //Valida si existe la actividad
                if (act.id_requisicion > 0)
                    //Si existe asigna valores al control de usuario wucRequisicion
                    wucRequisicion.InicializaRequisicion(act.id_requisicion, 0, 0, 0, true,false);                
                //En caso contrario
                else
                    //Inicializa el control de usuario a 0.
                    wucRequisicion.InicializaRequisicion(0, 0, 0, 0, true,false);
            }
        }
        /// <summary>
        /// Método que inicializa los controles acorde al estado de la página
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si se encuentra en modo Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles.
                        txtDescripcion.Text = "";
                        txtTiempoActividad.Text = "";
                        break;
                    }
                //Si se encuentra en modo Edicion o Lectura
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //limpia el control de usuario Tiempo Actividad
                        txtTiempoActividad.Text = "";
                        //Carga los controles con información de un registro
                        using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad(Convert.ToInt32(Session["id_registro"])))
                        {
                            txtDescripcion.Text = act.descripcion;
                            ddlFamilia.SelectedValue = act.id_familia.ToString();
                            //Carga el catalogo al dropdownlist ddlSubFamilia
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubFamilia, "--No Aplica", 1122, Convert.ToInt32(ddlFamilia.SelectedValue));
                            ddlSubFamilia.SelectedValue = act.id_sub_familia.ToString();
                            ddlTipoUnidad.SelectedValue = act.id_tipo_unidad.ToString();
                            //Carga el catalogo al dropdownlist ddlSubTipoUnidad
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipoUnidad, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
                            ddlSubTipoUnidad.SelectedValue = act.id_sub_tipo_unidad.ToString();
                            using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(act.id_requisicion))
                            {
                                //Inicializa el control de usuario.
                                wucRequisicion.InicializaRequisicion(req.id_requisicion, 0, 0, 0);
                            }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método que almacena los datos obtenidos de la página en la base de datos
        /// </summary>
        private void guardarActividad()
        {
            //Creación del objeto retorno
            RetornoOperacion result = new RetornoOperacion();
            //Inicia el bloque trnsaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {                
                //Valida cada estatus de la página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    //si el estado de la página es Nuevo
                    case Pagina.Estatus.Nuevo:
                        {
                            //Invoca al método guardar del control de usuario Requisicion
                            result = wucRequisicion.GuardaRequisicion();
                            //Crea variable que almacena el valor del registro de una requicisión
                            int idRequisicion = result.IdRegistro;
                            //Valida la operación de Guardar Requisición
                            if (result.OperacionExitosa)
                            {
                                //Asigna valores al objeto retorno
                                result = SAT_CL.Mantenimiento.Actividad.InsertarActividad(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text.ToUpper(),
                                                                                            (SAT_CL.Mantenimiento.Actividad.Familia)Convert.ToByte(ddlFamilia.SelectedValue),
                                                                                            (SAT_CL.Mantenimiento.Actividad.SubFamilia)Convert.ToByte(ddlSubFamilia.SelectedValue), Convert.ToInt32(ddlTipoUnidad.SelectedValue),
                                                                                            Convert.ToInt32(ddlSubTipoUnidad.SelectedValue), idRequisicion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                                                             
                            }
                            break;
                        }
                    //Si el estado de la página es Edición
                    case Pagina.Estatus.Edicion:
                        {
                            //Instanciá a la clase actividad
                            using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
                            {
                                //Valida que exista el registro
                                if (act.id_actividad > 0)
                                {
                                    //Asigna valores al objeto retorno
                                    result = act.EditarActividad(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtDescripcion.Text.ToUpper(),
                                                                                        (SAT_CL.Mantenimiento.Actividad.Familia)Convert.ToByte(ddlFamilia.SelectedValue),
                                                                                        (SAT_CL.Mantenimiento.Actividad.SubFamilia)Convert.ToByte(ddlSubFamilia.SelectedValue), Convert.ToInt32(ddlTipoUnidad.SelectedValue),
                                                                                        Convert.ToInt32(ddlSubTipoUnidad.SelectedValue), act.id_requisicion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            break;
                        }
                }
                //Valida las operaciónes 
                if (result.OperacionExitosa)    
                    //Invoca al método Complete.
                    trans.Complete();                
            }
            //Valida si la inserción a la base de datos se realizo correctamente
            if (result.OperacionExitosa)
            {                
                //A la variable de sessión estatus le asigna el estado de la pagina en modo lectura
                Session["estatus"] = Pagina.Estatus.Edicion;
                //A la variable de session id_registro le asigna el valor insertado en la tabla Actividad
                Session["id_registro"] = result.IdRegistro;
                //Invoca al método inicializa forma
                inicializaForma();
            }
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método que almacena los datos obtenidos de los controles de actividad puesto
        /// </summary>
        private void guardaActividadPuesto()
        {
            //Creaón del objeto retorno
            RetornoOperacion retorno = validaTiempoActividad();
            //Valida Tiempo Actividad
            if (retorno.OperacionExitosa)
            {                
                //Instancia a la clase Actividad            
                using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
                {
                    //Valida que exista una actividad
                    if (act.id_actividad != 0)
                    {
                        //Valida si hay registros en el gridvie ActividadPuesto
                        if (gvActividadPuesto.SelectedIndex != -1)
                        {
                            //Invoca a la clase Actividad Puesto
                            using (SAT_CL.Mantenimiento.ActividadPuesto actpuesto = new SAT_CL.Mantenimiento.ActividadPuesto(Convert.ToInt32(gvActividadPuesto.SelectedDataKey["Id"])))
                            {
                                //Asigna retorno a valor con los valores a editar.
                                retorno = actpuesto.EditarActividadPuesto(act.id_actividad, (SAT_CL.Mantenimiento.ActividadPuesto.Puesto)Convert.ToByte(ddlPuesto.SelectedValue), Convert.ToInt32(txtTiempoActividad.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        else
                        {
                            //Realiza una insercio
                            retorno = SAT_CL.Mantenimiento.ActividadPuesto.InsertarActividadPuesto(act.id_actividad, (SAT_CL.Mantenimiento.ActividadPuesto.Puesto)Convert.ToByte(ddlPuesto.SelectedValue),
                                                                                                                      Convert.ToInt32(Cadena.VerificaCadenaVacia(txtTiempoActividad.Text, "0")), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }                  
                    }
                }
            }
            //Valida si se realizo crectamente la accion
            if (retorno.OperacionExitosa)
            {
                //Invoca al método cargaGridViewActividadPuesto.
                cargaActividadPuesto();
                //Limpia el contenido del control txtTiempoActividad.
                txtTiempoActividad.Text = "";
                //Invoca al método inicializaForma
                inicializaForma();                
            }
            //Envia un ménsaje acorde a la acción.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        /// <summary>
        /// Método que inicializa el gridView Actividad Puesto acorde al estado de la página.
        /// </summary>
        private void cargaActividadPuesto()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        Controles.InicializaGridview(gvActividadPuesto);
                        break;
                    }
                //En caso de lectura y Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase actividad
                        using (SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
                        {
                            //Valida que exista una actividad
                            if (act.id_actividad != 0)
                            {
                                //Instancia a la clase actividad puesto
                                using (DataTable dtActPuesto = SAT_CL.Mantenimiento.ActividadPuesto.CargaPuestos(act.id_actividad))
                                {
                                    //Valida que existan datos en el datatable
                                    if (Validacion.ValidaOrigenDatos(dtActPuesto))
                                    {
                                        //Si existen registros, carga el gridview 
                                        Controles.CargaGridView(gvActividadPuesto, dtActPuesto, "Id", "");
                                        //Asigna valores a la variable de session del DS
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtActPuesto, "Table");
                                    }
                                    //En caso contrario
                                    else
                                    {
                                        //Inicializa el gridView
                                        Controles.InicializaGridview(gvActividadPuesto);
                                        //Elimina los datos del dataset si se realizo una consulta anterior
                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                                    }
                                }
                            }

                        }
                        break;
                    }
            }
            //Inicializa el gridView
            Controles.InicializaIndices(gvActividadPuesto);
            //Invoca al método que calcula el precio de los productos y Puesto
            calculaTotalPuesto();
        }
        /// <summary>
        /// Método que carga los valores del grid view Producto acorde al estado de la página
        /// </summary>
        private void cargaProducto()
        {
            //Valida el estado de la págin
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la página este en estado de Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializa GridView
                        Controles.InicializaGridview(gvProducto);
                        break;
                    }
                //Si ele stado de la página esta en Lectura o Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Realiza la consulta a la clase actividad para obtener el identificador de requisición
                        using(SAT_CL.Mantenimiento.Actividad act = new SAT_CL.Mantenimiento.Actividad((int)Session["id_registro"]))
                        {
                            //Valida que existan registros
                            if(act.id_actividad>0)
                            {
                                using (DataTable dtProducto = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(act.id_requisicion))
                                {
                                    //Valida los datos del la tabla producto
                                    if (Validacion.ValidaOrigenDatos(dtProducto))
                                    {
                                       //Carga el GridView prodcto con los valores almacenados en la tabla producto
                                        Controles.CargaGridView(gvProducto, dtProducto, "IdRequisicion", "");
                                        //Asigna a la variable de sessión DS el valor de la tabla
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtProducto, "Table1");
                                    }
                                    //En caso contrario
                                    else
                                    {
                                        //Inicializa el GridView
                                        Controles.InicializaGridview(gvProducto);
                                        //Elimina los datos almasenado en la variable de session DS
                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                                    }
                                }
                            }
                        }

                        break;
                    }

            }
            //Inicializa los indices del GridView
            Controles.InicializaIndices(gvProducto);
            //Invoca al método que calcula el precio de los productos
            calculaTotalProducto();
        }
        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de Actividad</param>
        /// <param name="idTabla">Identificador de la tabla Actividad</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Actividad.
            string url = Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/Actividad.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuración de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora  Actividad", configuracion, Page);
        }
        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla Actividad
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla Actividad registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Actividad
            string url = Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/Actividad.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de Actividad
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Actividad
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Actividad", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla Actividad</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla Actividad en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Actividad
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/Actividad.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Actividad
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Actividad", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método que valida los valores de Tiempo Actividad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaTiempoActividad()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Valida Los valores del corntrol Tiempo Actividad
            if (Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtTiempoActividad.Text, "0.0")) == 0)            
                //Asigna al objeto retorno un mensaje de que no bede existir duración en cero.
                retorno = new RetornoOperacion("La duración de la actividad debe ser mayo a cero.");
            //Retorna el objeto retrono al método
            return retorno;
        }
        /// <summary>
        /// Método que realiza la suma de los totales de producto
        /// </summary>
        private void calculaTotalProducto()
        {
            //Instancia a la variable de session DS
            using (DataTable dtProducto = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"))
            {
                //Valida los valores de la tabla Producto
                if(Validacion.ValidaOrigenDatos(dtProducto))
                {
                    //Realiza la suma  de las columnas Cantidad y Total
                    gvProducto.FooterRow.Cells[6].Text = string.Format("{0:C2}", (dtProducto.Compute("SUM(Cantidad)", "")));
                    gvProducto.FooterRow.Cells[7].Text = string.Format("{0:C2}", (dtProducto.Compute("SUM(Total)", "")));
                }
                else
                {
                    //Muestra en 0 el footerRow
                    gvProducto.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
                    gvProducto.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                }
            }         
        }
        /// <summary>
        /// Método que realiza la suma de totales de los Puestos
        /// </summary>
        private void calculaTotalPuesto()
        {
            //Instancia a la variable de session DS
            using (DataTable dtPuesto = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"))
            {
                //Recupera los valores de la variable de sessionDS y calcula los totales del gridView Puesto
                if (Validacion.ValidaOrigenDatos(dtPuesto))
                    //Agrega el resultado de la suma de la columna ManoObra
                    gvActividadPuesto.FooterRow.Cells[4].Text = string.Format("{0:C2}", (dtPuesto.Compute("SUM(ManoObra)", "")));
                //En caso contrario
                else
                    //Muestra en 0 el footerRow
                    gvActividadPuesto.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion
    }
}