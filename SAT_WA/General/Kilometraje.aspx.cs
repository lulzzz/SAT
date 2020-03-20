using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using SAT_CL.Seguridad;
using TSDK.Base;
using System.Transactions;

namespace SAT.General
{
    public partial class Kilometraje : System.Web.UI.Page
    {
        #region Eventos Página

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Determinando en que pestaña será realizada la búsqueda
            switch (mtvKilometrajes.GetActiveView().ID)
            {
                case "vwKilometrajeExistente":
                    //Cargando kilometraje ya registrado
                    cargaKilometrajeExistente();
                    break;
                case "vwKilometrajePendiente":
                    //Cargando Kilometraje pendiente o con movimientos sin actualizar
                    cargaKilometrajePendiente();
                    break;
            }            
        }        
        /// <summary>
        /// Evento Producido al Presionar el Boton "Nuevo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            //Inicializando Control
            ucKilometraje.InicializaControlKilometraje(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

            //Mostrando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnNuevo, upbtnNuevo.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");
        }
        /// <summary>
        /// Evento Producido al Guardar el Kilometraje
        /// </summary>
        protected void ucKilometraje_ClickGuardar(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            RetornoOperacion resultado = ucKilometraje.GuardaKilometraje();

            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Mostrando Ventana Modal
                TSDK.ASP.ScriptServer.AlternarVentana(ucKilometraje.Page, "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");

                //Cargando movimientos
                cargaMovimientosPendiente();

                //Mostrar venta modal de movimientos pendientes
                TSDK.ASP.ScriptServer.AlternarVentana(ucKilometraje.Page, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");
            }

            //Mostrando resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrar_Click(object sender, EventArgs e)
        {
            //Determinando en que pestaña será realizada la búsqueda
            switch (mtvKilometrajes.GetActiveView().ID)
            {
                case "vwKilometrajeExistente":
                    //Cargando kilometraje ya registrado
                    cargaKilometrajeExistente();                    
                    break;
                case "vwKilometrajePendiente":
                    //Cargando Kilometraje pendiente o con movimientos sin actualizar
                    cargaKilometrajePendiente();                    
                    break;
            }  
                        
            //Cerrando ventana modal con resultados
            TSDK.ASP.ScriptServer.AlternarVentana(uplkbCerrar, uplkbCerrar.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");
        }
        /// <summary>
        /// Evento Producido al Actualizar Todos los Movimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarMovimientos_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Cargando Reporte
            using (DataTable mit = SAT_CL.Despacho.Movimiento.CargaMovimientosSinKilometraje(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                            Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdOrigen"]),
                                                            Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdDestino"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorriendo Movimientos
                        foreach (DataRow dr in mit.Rows)
                        {
                            //Instanciamos nuestro movimiento 
                            using (SAT_CL.Despacho.Movimiento objMovimiento = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(dr["NoMovimiento"])))
                            {
                                //Si el movimiento pertenece a un servicio
                                if (objMovimiento.id_servicio > 0)
                                {
                                    //Instanciamos nuestro servicio
                                    using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objMovimiento.id_servicio))

                                        //Realizamos la actualizacion del kilometraje
                                        result = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                //En caso contrario
                                else
                                    //Actualizando kilometraje de de movimiento
                                    result = SAT_CL.Despacho.Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación no fuese Exitosa
                                if (!result.OperacionExitosa)

                                    //Terminando Ciclo
                                    break;
                            }
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existen Movimientos por Actualizar");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Actualizando lista de movimientos pendientes
                    cargaMovimientosPendiente();

                    //Mostrar venta modal de movimientos pendientes
                    TSDK.ASP.ScriptServer.AlternarVentana(btnActualizarMovimientos, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");

                    //Actualizando Kilometraje Pendiente
                    cargaKilometrajePendiente();
                }

                //Mostrando Resultado de Operación
                ScriptServer.MuestraNotificacion(btnActualizarMovimientos, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Eventos GridView "Kilometrajes"

        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Kilometrajes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKilometrajes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvKilometrajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 0);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar" del GridView "Kilometrajes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Determinando el botón pulsado
            switch(((LinkButton)sender).CommandName)
            {
                case "Kilometraje":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                    break;
                case"Pendiente":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id", "IdOrigen", "IdDestino");
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Kilometrajes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvKilometrajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 0);
        }
        /// <summary>
        /// Evento Producido al Cambiar Indice de Página del GridView "Kilometrajes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKilometrajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvKilometrajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvKilometrajes.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvKilometrajes, sender, "lnk", false);

                //Inicializando Control
                ucKilometraje.InicializaControlKilometraje(Convert.ToInt32(gvKilometrajes.SelectedDataKey["Id"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Mostrando Ventana Modal
                TSDK.ASP.ScriptServer.AlternarVentana(upgvKilometrajes, upgvKilometrajes.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");
            }
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando GridView de Kilometraje existente
            TSDK.ASP.Controles.InicializaGridview(gvKilometrajes);
            //Inicializando GridView de Kilometraje pendiente
            TSDK.ASP.Controles.InicializaGridview(gvKilometrajesPendientes);

            //Instanciando Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor cmp = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que exista
                if(cmp.id_compania_emisor_receptor > 0)
                    //Asignando Valor
                    txtCompania.Text = cmp.nombre + " ID:" + cmp.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompania.Text = "";
            }

            //Cargando Catalogos
            cargaCatalogos();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño del GridView Kilometraje
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Tamaño del GridView Kilometraje Pendiente
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPendiente, "", 26);
        }
        /// <summary>
        /// Carga los kilometrajes registrados en BD
        /// </summary>
        private void cargaKilometrajeExistente()
        {
            //Inicializando indices de selección
            TSDK.ASP.Controles.InicializaIndices(gvKilometrajes);

            //Cargando Reporte
            using (DataTable dtKilometraje = SAT_CL.Global.Kilometraje.ObtieneKilometrajes(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionOrigen.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionDestino.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudadOrigen.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudadDestino.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0"))))
            {
                //Si hay registros
                if (dtKilometraje != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtKilometraje, "Table");
                //Si no hay registros en el origen de datos
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                //Cargando GridView
                TSDK.ASP.Controles.CargaGridView(gvKilometrajes, dtKilometraje, "Id", "", true, 0);                
            }
        }
        /// <summary>
        /// Carga los kilometrajes pendientes de registrar en BD o con movimientos sin actualizar
        /// </summary>
        private void cargaKilometrajePendiente()
        {
            //Inicializando indices de selección
            TSDK.ASP.Controles.InicializaIndices(gvKilometrajesPendientes);

            //Cargando Reporte
            using (DataTable dtKilometraje = SAT_CL.Global.Kilometraje.ObtieneKilometrajeFaltante(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionOrigen.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUbicacionDestino.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudadOrigen.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCiudadDestino.Text, "ID:", 1, "0")),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0"))))
            {
                //Si hay registros
                if (dtKilometraje != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtKilometraje, "Table1");
                //Si no hay registros en el origen de datos
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                //Cargando GridView
                TSDK.ASP.Controles.CargaGridView(gvKilometrajesPendientes, dtKilometraje, "Id-IdOrigen-IdDestino", lblOrdenadoPendiente.Text, true, 0);
            }
        }
        /// <summary>
        /// Carga los movimientos pendientes de actualización de kilometraje
        /// </summary>
        private void cargaMovimientosPendiente()
        {
            //Inicializando indices de selección
            TSDK.ASP.Controles.InicializaIndices(gvMovimientosPendientes);
            //Cargando Reporte
            using (DataTable mit = SAT_CL.Despacho.Movimiento.CargaMovimientosSinKilometraje(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                            Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdOrigen"]),
                                                            Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdDestino"])))
            {
                //Si hay registros
                if (mit != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");
                //Si no hay registros en el origen de datos
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                //Cargando GridView
                TSDK.ASP.Controles.CargaGridView(gvMovimientosPendientes, mit, "NoMovimiento", "", true, 2);
            }
        }

        #endregion

        #region Eventos GridView "Pendientes"

        /// <summary>
        /// Click en criterio de ordenamiento de GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKilometrajesPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoPendiente.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvKilometrajesPendientes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 0);
        }
        /// <summary>
        /// Click en indice de página de GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvKilometrajesPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvKilometrajesPendientes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 0);
        }
        /// <summary>
        /// Cambio de tamaño de página en GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPendiente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvKilometrajesPendientes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoPendiente.SelectedValue), true, 0);
        }
        /// <summary>
        /// Click en botón del Gv de Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPendientes_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvKilometrajesPendientes.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvKilometrajesPendientes, sender, "lnk", false);

                //Determinando el comando del botón pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Editar":
                        //Determinando si existe o no un registro kilometraje
                        if (gvKilometrajesPendientes.SelectedDataKey["Id"].ToString() == "0")
                            //Inicializando Control
                            ucKilometraje.InicializaControlKilometraje(0, ((UsuarioSesion)Session["usuario_Sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdOrigen"]), Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["IdDestino"]));
                        //Si ya existe un kilometraje
                        else
                            ucKilometraje.InicializaControlKilometraje(Convert.ToInt32(gvKilometrajesPendientes.SelectedDataKey["Id"]), ((UsuarioSesion)Session["usuario_Sesion"]).id_compania_emisor_receptor);

                        //Mostrando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upgvKilometrajesPendientes, upgvKilometrajesPendientes.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");
                        break;
                    case "VerPendientes":
                        //Cargando movimientos
                        cargaMovimientosPendiente();
                        //Mostrar venta modal de movimientos pendientes
                        TSDK.ASP.ScriptServer.AlternarVentana(gvKilometrajesPendientes, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botónes de vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_Click(object sender, EventArgs e)
        {
            //Determinando el botón pulsado
            switch (((Button)sender).CommandName)
            { 
                case "Kilometraje":
                    //Aplicando estilos de botónes
                    btnVistaKilometraje.CssClass = "boton_pestana_activo";
                    btnVistaPendiente.CssClass = "boton_pestana";
                    //Cambiando vista activa
                    mtvKilometrajes.SetActiveView(vwKilometrajeExistente);
                                        
                    //Inicializando GridView de Kilometraje pendiente
                    TSDK.ASP.Controles.InicializaGridview(gvKilometrajes);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    break;
                case "Pendientes":
                    //Aplicando estilos de botónes
                    btnVistaKilometraje.CssClass = "boton_pestana";
                    btnVistaPendiente.CssClass = "boton_pestana_activo";
                    //Cambiando vista activa
                    mtvKilometrajes.SetActiveView(vwKilometrajePendiente);

                    //Inicializando GridView de Kilometraje pendiente
                    TSDK.ASP.Controles.InicializaGridview(gvKilometrajesPendientes);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    break;
            }
        }

        #endregion

        #region Eventos Movimientos Sin Kilometraje

        /// <summary>
        /// Click en botón cerrar ventana de movimientos pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarMovimientos_Click(object sender, EventArgs e)
        {
            //Actualizando lista de kilometraje pendiente
            cargaKilometrajePendiente();
            //Cerrando ventana modal
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarMovimientos, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");
        }
        /// <summary>
        /// Cambio de Indice de página del GV de Movimientos pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvMovimientosPendientes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Click en algún botón del GV de Movimientos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMovimientoPendiente_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvMovimientosPendientes.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;
                
                //Declarando retorno de operación
                TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

                //seleccionando fila
                TSDK.ASP.Controles.SeleccionaFila(gvMovimientosPendientes, sender, "lnk", false);

                //Instanciamos nuestro movimiento 
                using (SAT_CL.Despacho.Movimiento objMovimiento = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvMovimientosPendientes.SelectedDataKey["NoMovimiento"])))
                {
                    //Si el movimiento pertenece a un servicio
                    if (objMovimiento.id_servicio > 0)
                    {
                        //Instanciamos nuestro servicio
                        using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objMovimiento.id_servicio))
                            //Realizamos la actualizacion del kilometraje
                            resultado = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    //En caso contrario
                    else
                        //Actualizando kilometraje de de movimiento
                        resultado = SAT_CL.Despacho.Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Actualizando lista de movimientos pendientes
                    cargaMovimientosPendiente();

                    //Validando que existan Movimientos
                    if (!Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
                    {
                        //Mostrar venta modal de movimientos pendientes
                        TSDK.ASP.ScriptServer.AlternarVentana(lkb, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");

                        //Actualizando Kilometraje Pendiente
                        cargaKilometrajePendiente();
                    }
                }

                //Mostrando resultado de actualización
                TSDK.ASP.ScriptServer.MuestraNotificacion(lkb, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion
    }
}