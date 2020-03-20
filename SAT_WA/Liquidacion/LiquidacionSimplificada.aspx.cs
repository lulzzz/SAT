using SAT_CL.CXP;
using SAT_CL.Despacho;
using SAT_CL.EgresoServicio;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using SAT_CL.Liquidacion;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class LiquidacionSimplificada : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que existe un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {
            //Obteniendo Archivo Codificado en UTF8
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
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Busqueda":
                    mtvEncabezado.ActiveViewIndex = 0;
                    btnBusqueda.CssClass = "boton_pestana_activo";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnResumen.CssClass = "boton_pestana";

                    /*/Inicializando Tipo de Recurso
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoRecurso, "", 62);

                    //Invocando Método de Configuración
                    configuraBusquedaUnidad();

                    //Inicializando GridView
                    Controles.InicializaGridview(gvLiquidacion);
                    Controles.InicializaIndices(gvLiquidacion);//*/

                    break;
                case "Liquidacion":
                    mtvEncabezado.ActiveViewIndex = 1;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana_activo";
                    btnResumen.CssClass = "boton_pestana";
                    break;
                case "Resumen":
                    mtvEncabezado.ActiveViewIndex = 2;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnResumen.CssClass = "boton_pestana_activo";

                    //Cargando Resumen Total de la Liquidación
                    cargaResumenTotalLiquidacion(Convert.ToInt32(Session["id_registro"]));
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Tipo de Entidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoRecurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraBusquedaUnidad();
        }
        /// <summary>
        /// Evento Producido al Buscar las Liquidaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarLiquidaciones_Click(object sender, EventArgs e)
        {
            //Obteniendo Recurso
            string recurso = ddlTipoRecurso.SelectedValue == "1" ? txtRecursoUn.Text : (ddlTipoRecurso.SelectedValue == "2" ? txtRecursoOp.Text : txtRecursoPr.Text);
            
            //Instanciando Liquidaciones
            using (DataTable dtLiquidaciones = SAT_CL.Liquidacion.Liquidacion.ObtieneLiquidacionesEntidad(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)Convert.ToInt32(ddlTipoRecurso.SelectedValue),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(recurso, "ID:", 1))))
            {
                //Validando si Existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtLiquidaciones))
                {
                    //Cargando Liquidaciones
                    TSDK.ASP.Controles.CargaGridView(gvLiquidacion, dtLiquidaciones, "Id", "");
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtLiquidaciones, "Table");
                }
                else
                {
                    //Cargando Liquidaciones
                    TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
                
                //Limpiando Mensaje
                lblErrorServicio.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al presionar el Boton "Crear"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrear_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int id_unidad = ddlTipoRecurso.SelectedValue == "1" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRecursoUn.Text, "ID:", 1)) : 0;
            int id_operador = ddlTipoRecurso.SelectedValue == "2" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRecursoOp.Text, "ID:", 1)) : 0;
            int id_proveedor = ddlTipoRecurso.SelectedValue == "3" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRecursoPr.Text, "ID:", 1)) : 0;
            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
            DateTime fecha_ultimo_viaje = DateTime.MinValue;

            //Validando que exista el Operador
            if (id_operador != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_operador, Convert.ToInt32(ddlTipoRecurso.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);
            //Validando que exista el Proveedor
            else if (id_proveedor != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_proveedor, Convert.ToInt32(ddlTipoRecurso.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);
            //Validando que exista el Unidad
            else if (id_unidad != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_unidad, Convert.ToInt32(ddlTipoRecurso.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);

            //Insertando Encabezado de Liquidación            
            result = SAT_CL.Liquidacion.Liquidacion.InsertaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0,
                        (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)Convert.ToByte(ddlTipoRecurso.SelectedValue), id_operador, id_unidad, id_proveedor,
                        TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, 0, 0, 0, 0, dias_pagados.Days, false, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //validando que la Operación fuese exitosa
            if (result.OperacionExitosa)
            {
                //Añadiendo Resultado a Sesión
                Session["id_registro"] = result.IdRegistro;
                    
                //Habilitando Controles
                habilitaControlesEncabezado(Pagina.Estatus.Lectura);
                    
                //Inicializando Valores
                cargaDatosEncabezadoLiq();
                    
                //Quitando Selección de Liquidacion
                TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            //Validando que Exista una Liquidación
            if (Convert.ToInt32(Session["id_registro"]) != 0)

                //Habilitación de Controles
                habilitaControlesEncabezado(Pagina.Estatus.Edicion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarLiq_Click(object sender, EventArgs e)
        {
            //Instanciando Excepción
            RetornoOperacion result = new RetornoOperacion();
            
            //Instanciando Liquidación
            using(SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Si existe la Liquidación
                if(liq.habilitar)
                {
                    //Validando Estatus
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)

                        //Gestiona Ventana
                        gestionaVentanas(btnEliminarLiq, "ConfirmaEliminaLiquidacion");
                    else
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Liquidación esta Cerrada, Imposible su Edición");

                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Confirmar la Eliminación de la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminaLiq_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Instanciando Liquidación
            using(SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Existe la Liquidación?
                if (liq.habilitar)
                {
                    //Validando Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Obteniendo Entidad
                        int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Obteniendo Movimientos
                            using (DataSet ds = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneMovimientosYDetallesPorLiquidacion(liq.id_liquidacion, DetalleLiquidacion.Estatus.Registrado, id_entidad, (byte)liq.id_tipo_asignacion))
                            {
                                //Validando que Existan Detalles de Liquidación
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                                {
                                    //Recorriendo Detalles de Liquidación
                                    foreach (DataRow drDL in ds.Tables["Table1"].Rows)
                                    {
                                        //Instanciando Detalle de Liquidación
                                        using (DetalleLiquidacion dl = new DetalleLiquidacion(Convert.ToInt32(drDL["Id"])))
                                        {
                                            //Validando que exista el Detalle de Liquidación
                                            if (dl.id_detalle_liquidacion > 0)
                                            {
                                                //Validando que sea un Pago o una Comprobación
                                                if (dl.id_tabla == 79 || dl.id_tabla == 104)
                                                {
                                                    //Validando Tabla
                                                    switch(dl.id_tabla)
                                                    {
                                                        case 79:
                                                            {
                                                                //Deshabilitando Pago y/o 
                                                                result = dl.DeshabilitaDetalleLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando Operación Exitosa
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Instanciando Pago
                                                                    using (Pago pago = new Pago(dl.id_registro))
                                                                    {
                                                                        //Validando Pago
                                                                        if (pago.habilitar)
                                                                        {
                                                                            //Deshabilitando Pago
                                                                            result = pago.DeshabilitaPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                            //Validando que se haya Realizado la Operación exitosamente
                                                                            if (result.OperacionExitosa)
                                                                            {
                                                                                //Obteniendo pago movimiento's
                                                                                using (DataTable dtPagoMovimientos = PagoMovimiento.ObtienePagosMovimiento(pago.id_pago))
                                                                                {
                                                                                    //Validando que existan Registros
                                                                                    if (Validacion.ValidaOrigenDatos(dtPagoMovimientos))
                                                                                    {
                                                                                        //Recorriendo Registros
                                                                                        foreach (DataRow dr in dtPagoMovimientos.Rows)
                                                                                        {
                                                                                            //Instanciando Pago Movimiento
                                                                                            using (PagoMovimiento pm = new PagoMovimiento(Convert.ToInt32(dr["Id"])))
                                                                                            {
                                                                                                //Validando que existan los Pagos
                                                                                                if (pm.habilitar)

                                                                                                    //Deshabilitando Pago Movimiento
                                                                                                    result = pm.DeshabilitaPagoMovimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                else
                                                                                                    //Terminando Ciclo
                                                                                                    break;

                                                                                                //Validando que se haya Realizado la Operación exitosamente
                                                                                                if (!result.OperacionExitosa)
                                                                                                    //Terminando Ciclo
                                                                                                    break;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                //Terminando Ciclo
                                                                                break;
                                                                        }
                                                                        else
                                                                        {
                                                                            //Instanciando Excepción
                                                                            result = new RetornoOperacion("No existe el Pago");

                                                                            //Terminando Ciclo
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 104:
                                                            {
                                                                //Instanciando Pago
                                                                using (Comprobacion cmp = new Comprobacion(dl.id_registro))
                                                                {
                                                                    //Validando Pago
                                                                    if (cmp.habilitar)
                                                                    {
                                                                        //Deshabilitando Pago
                                                                        result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                        //Validando que se haya Realizado la Operación exitosamente
                                                                        if (!result.OperacionExitosa)
                                                                            //Terminando Ciclo
                                                                            break;
                                                                    }
                                                                    else
                                                                    {
                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion("No existe la Comprobación");

                                                                        //Terminando Ciclo
                                                                        break;
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                                else
                                                    //Quitando Liga con Liquidación
                                                    result = dl.LiberaDetalleLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que se haya Realizado la Operación exitosamente
                                                if (!result.OperacionExitosa)
                                                    //Terminando Ciclo
                                                    break;
                                            }
                                            else
                                                //Terminando Ciclo
                                                break;
                                        }
                                    }

                                    
                                }
                                else
                                    //Instanciando Liquidación
                                    result = new RetornoOperacion(liq.id_liquidacion, "No Existen Detalles ni Movimiento Ligados", true);

                                //Validando que las Operaciones fueron Exitosas
                                if (result.OperacionExitosa)
                                {
                                    //Deshabilitando Liquidación
                                    result = liq.DeshabilitaLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que se Deshabilito
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Facturas Relación

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }

                        //Validando que se Deshabilito
                        if (result.OperacionExitosa)
                        {
                            //Eliminando de Sesión
                            Session["id_registro"] = 0;

                            //Inicializando Página
                            inicializaPagina();

                            //Inicializando Inidices
                            Controles.InicializaIndices(gvLiquidacion);

                            //Gestiona Ventana
                            gestionaVentanas(btnAceptarEliminaLiq, "ConfirmaEliminaLiquidacion");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Liquidación debe de estar Registrada");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Liquidación");
            }

            //Mostrando Mensaje Obtenido
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar la Eliminación de la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminaLiq_Click(object sender, EventArgs e)
        {
            //Gestiona Ventana
            gestionaVentanas(btnCancelarEliminaLiq, "ConfirmaEliminaLiquidacion");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar Liquidación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarLiquidacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Cerrar
            cierraLiquidacion();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAbrirLiq_Click(object sender, EventArgs e)
        {
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                if (liq.habilitar)
                {
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado)
                    {
                        using (SAT_CL.Bancos.EgresoIngreso egreso_liq = new SAT_CL.Bancos.EgresoIngreso(82, liq.id_liquidacion))
                        {
                            if (egreso_liq.habilitar)
                            {
                                if (liq.total_alcance == 0.00M && egreso_liq.monto == 0.00M && egreso_liq.estatus == SAT_CL.Bancos.EgresoIngreso.Estatus.Aplicada)
                                {
                                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                    {
                                        retorno = egreso_liq.DeshabilitarEgresoIngreso(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        if (retorno.OperacionExitosa)
                                        {
                                            retorno = liq.AbreLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            if (retorno.OperacionExitosa)
                                            {
                                                retorno = new RetornoOperacion(liq.id_liquidacion, string.Format("Liq. '{0}' reabierta. Operación del día '{1:dd/MM/yyyy HH:mm}'", liq.no_liquidacion, Fecha.ObtieneFechaEstandarMexicoCentro()), true);
                                                scope.Complete();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    retorno = egreso_liq.ActualizarEstatusEgresoLiquidacionCancelada(string.Format("Liq. '{0}' cerrada en 0's. Operación del día '{1:dd/MM/yyyy HH:mm}'", liq.no_liquidacion, Fecha.ObtieneFechaEstandarMexicoCentro()), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    if (retorno.OperacionExitosa)
                                        retorno = new RetornoOperacion(liq.id_liquidacion, string.Format("Liq. '{0}' reabierta. Operación del día '{1:dd/MM/yyyy HH:mm}'", liq.no_liquidacion, Fecha.ObtieneFechaEstandarMexicoCentro()), true);
                                }
                            }
                            else
                                retorno = new RetornoOperacion("La liquidación no esta en tesorería");
                        }
                    }
                    else
                        retorno = new RetornoOperacion("La liquidación no esta cerrada");
                }
                else
                    retorno = new RetornoOperacion("No existe la liquidación");

                if (retorno.OperacionExitosa)
                {
                    //Añadiendo a Sesión la Liquidación Seleccionada
                    Session["id_registro"] = Convert.ToInt32(retorno.IdRegistro);
                    //Invocando Método de Carga
                    cargaDatosEncabezadoLiq();
                    //Deshabilitando Controles
                    habilitaControlesEncabezado(Pagina.Estatus.Lectura);
                }
            }

            ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cerrar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando del Control
            switch (lnk.CommandName)
            {
                case "ReferenciasViaje":
                    //Cerrando ventana modal 
                    gestionaVentanas(lnk, "ReferenciasViaje");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Devolucion":
                            //Cerrando ventana modal 
                            gestionaVentanas(lnk, "AltaDevolucion");
                            break;
                    }
                    break;
                case "AltaDevolucion":
                    //Invocando Método de Gestión
                    gestionaVentanas(lnk, lnk.CommandName);
                    //Invocando Método de Gestion
                    gestionaVentanas(lnk, "Devolucion");
                    break;
                case "CerrarCambioCuentaPago":
                    //Invocando Método de Gestion
                    gestionaVentanas(lnk, "CambioCuentaPago");
                    break;
                default:
                    //Invocando Método de Gestión
                    gestionaVentanas(lnk, lnk.CommandName);
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "Liquidaciones":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                    break;
                case "Recurso":
                    //Exportando Contenido
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                    break;
                case "Pagos":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
                    break;
                case "CobrosRecurrentesLiquidacion":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Id");
                    break;
                case "Anticipos":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), "IdDetalle");
                    break;
                case "Comprobaciones":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), "IdDetalle");
                    break;
                case "FacturasComp":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), "Id");
                    break;
                case "CobrosRecurrentes":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), "Id");
                    break;
                case "TarifasAplicables":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), "Id");
                    break;
                case "Diesel":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), "Id");
                    break;
                case "Movimientos":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), "Id");
                    break;
                case "CobroRecurrenteHistorial":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table11"), "Id");
                    break;
                case "ResumenLiquidacion":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), "Id");
                    break;
                case "Evidencias":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table13"), "Id");
                    break;
                case "PagosLigados":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table14"), "Id");
                    break;
                case "Devoluciones":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table15"), "Id");
                    break;
                case "PagosMV":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table16"), "Id");
                    break;
                case "FacturasLiq":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table17"), "Id");
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Ver Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerCobrosPendientes_Click(object sender, EventArgs e)
        {
            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnVerCobrosPendientes, upbtnVerCobrosPendientes.GetType(), "VerCobrosPendientes", "contenedorVentanaCobrosPendientes", "ventanaCobrosPendientes");
        }
        /// <summary>
        /// Click en botón historial de viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkHistorialViajes_Click(object sender, EventArgs e)
        {
            //Abriendo ventana de historial
            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/LiquidacionSimplificada.aspx", "~/Accesorios/HistorialServicio.aspx");
            //Instanciando nueva ventana de navegador para apertura de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?&idRegistro={2}", urlReporte, "Porte", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=600", Page);
        }

        /// <summary>
        /// Evento generado al Dar click en Historial de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerHistorialUnidad_Click(object sender, EventArgs e)
        {
            //Construyendo URL de ventana de historial de unidad
            string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/LiquidacionSimplificada.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + "0" + "&idRegistroB=1");
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1300,height=650";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Historial de Unidades", configuracion, Page);
        }

        /// <summary>
        /// Evento Producido al Pagar el Servicio de Forma Manual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPagarServicioManual_Click(object sender, EventArgs e)
        {
            //Configurando Controles
            habilitaControlesPagos(true);
            txtDescripcion.Text = "Pago por Servicio";
            btnGuardarPago.CommandArgument = "Servicio";

            //Invocando Ventana de Pagos
            gestionaVentanas(this, "Pagos");

            //Cerrando ventana modal con resultados
            gestionaVentanas(this, "TarifasAplicables");
        }

        #region Eventos Edición Liquidación

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarLiq_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que exista la Liquidación
            if (Convert.ToInt32(Session["id_registro"]) != 0)
            {
                //Declarando Variable Auxiliar
                DateTime fecha_liquidacion;
                //Obteniendo Fecha
                DateTime.TryParse(txtFechaLiq.Text, out fecha_liquidacion);
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Liquidación
                    if (liq.id_liquidacion != 0)
                    {
                        //Validando Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Declarando Objetos Auxiliares 
                            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
                            DateTime fecha_ultimo_viaje = DateTime.MinValue;

                            //Validando que exista el Operador
                            if (liq.id_operador != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_operador, liq.id_tipo_asignacion, fecha_liquidacion, out fecha_ultimo_viaje);
                            //Validando que exista el Proveedor
                            else if (liq.id_proveedor != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_proveedor, liq.id_tipo_asignacion, fecha_liquidacion, out fecha_ultimo_viaje);
                            //Validando que exista el Unidad
                            else if (liq.id_unidad != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_unidad, liq.id_tipo_asignacion, fecha_liquidacion, out fecha_ultimo_viaje);

                            //Validando que la Fecha Ingresada sea mayor que la Fecha de la Liquidación
                            if (fecha_liquidacion.CompareTo(liq.fecha_liquidacion) > 0 || fecha_liquidacion.CompareTo(liq.fecha_liquidacion) == 0)

                                //Editando Liquidación
                                result = liq.EditaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, liq.no_liquidacion, liq.estatus,
                                            liq.tipo_asignacion, liq.id_operador, liq.id_unidad, liq.id_proveedor, fecha_liquidacion, liq.total_salario,
                                            liq.total_deducciones, liq.total_sueldo, liq.total_anticipos, liq.total_comprobaciones, liq.total_descuentos, liq.total_alcance,
                                            dias_pagados.Days, liq.bit_transferencia, liq.id_transferencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Si la Fecha Ingresada es menor a la Fecha de la Liquidación y Existen Pagos
                            else if (fecha_liquidacion.CompareTo(liq.fecha_liquidacion) < 0)
                            {
                                //Validando que existan Pagos
                                if (liq.ValidaPagosLiquidacion())

                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Fecha no puede ser menor a la Fecha actual de la Liquidación ya que Existen Pagos");
                                else
                                    //Editando Liquidación
                                    result = liq.EditaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, liq.no_liquidacion, liq.estatus,
                                                liq.tipo_asignacion, liq.id_operador, liq.id_unidad, liq.id_proveedor, fecha_liquidacion, liq.total_salario,
                                                liq.total_deducciones, liq.total_sueldo, liq.total_anticipos, liq.total_comprobaciones, liq.total_descuentos, liq.total_alcance,
                                                dias_pagados.Days, liq.bit_transferencia, liq.id_transferencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }

                            //Validando que la Operacion fue Correcta
                            if (result.OperacionExitosa)
                            {
                                //Habilitación de Controles
                                habilitaControlesEncabezado(Pagina.Estatus.Lectura);

                                //Actualizando Datos del Encabezado
                                cargaDatosEncabezadoLiq();

                                //Actualizando Carga de Totales de la Liquidación
                                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                            }
                        }
                        else
                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
            else
                //Instanciando Error
                result = new RetornoOperacion("No existe la Liquidación");
            
            //Mostrando mensaje Obtenido
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarLiq_Click(object sender, EventArgs e)
        {
            //Habilitación de Controles
            habilitaControlesEncabezado(Pagina.Estatus.Lectura);

            //Actualizando Datos del Encabezado
            cargaDatosEncabezadoLiq();

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);

            //Mostrando Mensaje de la Operación
            ScriptServer.MuestraNotificacion(btnGuardarLiq, "No se realizaron Cambios", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos GridView "Liquidaciones"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarLiquidacion_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvLiquidacion, sender, "lnk", false);
                
                //Añadiendo a Sesión la Liquidación Seleccionada
                Session["id_registro"] = Convert.ToInt32(gvLiquidacion.SelectedDataKey["Id"]);

                //Invocando Método de Carga
                cargaDatosEncabezadoLiq();

                //Deshabilitando Controles
                habilitaControlesEncabezado(Pagina.Estatus.Lectura);
            }
        }

        #endregion

        #region Eventos GridView "Recursos"

        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Pagar Mov. Vacio" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPagarMovVacio_Click(object sender, EventArgs e)
        {
            //Validando que existan registros
            if(gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Obteniendo Tipos de Pago
                using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                {
                    //Validando que existan los Conceptos de Pago
                    if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                        //Cargando DropDownList
                        Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                    else
                        //Inicializando DropDownList
                        Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                }
                
                //Obteniend Filas Sele
                GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvServiciosLiquidacion, "chkVariosMovimientos");

                //Habilitando Controles
                habilitaControlesPagos(true);

                //Limpiando Controles
                limpiaControlesPagos();

                //Validando que existan Filas Seleccionadas
                if (gvr.Length > 0)
                {
                    //Si no existen filas seleccionadas
                    if (gvr.Length > 0)
                    {
                        //Obteniendo Item de forma Dinamica*/
                        ListItem li = ddlTipoPago.Items.FindByText("Movimiento");
                        
                        //Validando que exista el Tipo
                        if (li != null)
                        {
                            //Limpiando Items
                            ddlTipoPago.Items.Clear();
                            //Añadiendo Item
                            ddlTipoPago.Items.Add(li);
                        }

                        //Asignando Valor
                        txtDescripcion.Text = "Pago por Movimiento";
                        //Mostrando Total de Movimientos
                        txtCantidad.Text = gvr.Length.ToString();

                    }
                    else
                        //Limpiando Controles de Pagos
                        limpiaControlesPagos();

                    //Asignando Comando
                    btnGuardarPago.CommandArgument = "Movimiento";
                    btnGuardarPago.CommandName = "PagoMovsVacio";
                    Controles.InicializaIndices(gvServiciosLiquidacion);

                    //Mostrando Ventana de pagos
                    gestionaVentanas(this, "Pagos");
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this, "No Existen Filas Seleccionadas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRecursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvServiciosLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 4);

            //Invocando Método de Suma
            sumaTotalesRecursos();
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenarRecursos.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvServiciosLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 4);

            //Invocando Método de Suma
            sumaTotalesRecursos();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvServiciosLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 4);

            //Invocando Método de Suma
            sumaTotalesRecursos();
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosLiquidacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Origen de la Fila
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                //Obteniendo Control
                using (CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosMovimientos"))
                {
                    //Validando si existe el Control
                    if (chk != null)
                    {
                        //Obteniendo Indicador
                        int idServicio = Convert.ToInt32(rowView["IdServicio"].ToString() == "" ? "0" : rowView["IdServicio"].ToString());
                        int idMovimiento = Convert.ToInt32(rowView["IdMovimiento"].ToString() == "" ? "0" : rowView["IdMovimiento"].ToString());

                        //Validando que sea un Movimiento en Vacio
                        if (idServicio == 0 && idMovimiento != 0)
                        {
                            //Visualizando Control
                            chk.Visible = true;
                            //Validando Habilitación
                            chk.Enabled = !SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(idMovimiento);
                        }
                        //Validando que sea un Servicio
                        else if (idServicio != 0 && idMovimiento == 0)
                            //Ocultando Control
                            chk.Visible = false;
                    }
                }

                //Obteniendo Control
                using (LinkButton lnk = (LinkButton)e.Row.FindControl("lnkDevoluciones"))
                {
                    //Validando que Exista el Control
                    if (lnk != null)
                    {
                        //Obteniendo Indicador
                        int indDevolucion = Convert.ToInt32(rowView["indDevoluciones"].ToString() == "" ? "0" : rowView["indDevoluciones"].ToString());

                        //Configurando Control
                        lnk.Visible = indDevolucion > 0 ? true : false;
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Marcar o Desmarcar los Controles CheckBox del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosMovimientos_CheckedChanged(object sender, EventArgs e)
        {
            //Obteniendo Control Disparador
            CheckBox chk = (CheckBox)sender;

            //Validando el Control
            switch (chk.ID)
            {
                case "chkTodosMovimientos":
                    {
                        //Validando que el Control haya sido Marcado
                        if(!chk.Checked)
                        {
                            //Seleccionando Fila
                            Controles.SeleccionaFilasTodas(gvServiciosLiquidacion, "chkVariosMovimientos", chk.Checked);

                            //Ocultando Control
                            btnPagarMovVacio.Visible = false;
                        }
                        else
                        {
                            //Obteniendo Control
                            CheckBox chkVarios;

                            //Recorriendo cada fila
                            foreach (GridViewRow gvr in gvServiciosLiquidacion.Rows)
                            {
                                //Obteniendo Control
                                chkVarios = (CheckBox)gvr.FindControl("chkVariosMovimientos");

                                //Validando que existe la Fila
                                if (chkVarios != null)
                                {    
                                    //Asignando Valor de Fila Actual
                                    gvServiciosLiquidacion.SelectedIndex = gvr.RowIndex;

                                    //Validando que no exista el Servicio
                                    if(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]) == 0)
                                    
                                        //Seleccionando Fila
                                        chkVarios.Checked = chkVarios.Enabled;
                                }
                            }

                            //Inicializando Indices
                            Controles.InicializaIndices(gvServiciosLiquidacion);
                        }

                        //Configurando Control
                        btnPagarMovVacio.Visible = chk.Checked;

                        break;
                    }
                case "chkVariosMovimientos":
                    {
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvServiciosLiquidacion, "chkVariosMovimientos");

                        //Validando que existan Filas Seleccionadas
                        if (gvr.Length > 0)

                            //Visualizando Control
                            btnPagarMovVacio.Visible = true;
                        else
                            //Ocultando Control
                            btnPagarMovVacio.Visible = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click a algún Detalle de Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDetalleLiq_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);
                
                //Declarando Variables Auxiliares
                int id_entidad = 0;
                int id_tipo_asignacion = 0;
                    
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Liquidación
                    if (liq.id_liquidacion > 0)
                    {
                        //Obteniendo Control
                        LinkButton lnk = (LinkButton)sender;

                        //Obteniendo Valores
                        int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                        int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                        //Validando si Existe el Movmiento
                        if (idServicio > 0)
                        {
                            //Asignando Entidad Encontrada
                            if (liq.id_operador != 0)
                            {
                                //Asignando Valores
                                id_entidad = liq.id_operador;
                                id_tipo_asignacion = 2;
                            }
                            else if (liq.id_unidad != 0)
                            {
                                //Asignando Valores
                                id_entidad = liq.id_unidad;
                                id_tipo_asignacion = 1;
                            }
                            else if (liq.id_proveedor != 0)
                            {
                                //Asignando Valores
                                id_entidad = liq.id_proveedor;
                                id_tipo_asignacion = 3;
                            }


                            //Cargando Movmientos por Servicio
                            cargaMovimientosServicio(idServicio, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                            //Validando Comando
                            switch (lnk.CommandName)
                            {
                                case "Pagos":
                                    {
                                        //Validando que el Servicio no Tenga Pagos Aplicados
                                        if (!SAT_CL.Liquidacion.Pago.ValidaPagoServicio(idServicio, id_entidad, id_tipo_asignacion))
                                        {
                                            //Obteniendo Tipos de Pago
                                            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                            {
                                                //Validando que existan los Conceptos de Pago
                                                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                                    //Cargando DropDownList
                                                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                                else
                                                    //Inicializando DropDownList
                                                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                            }
                                            
                                            //Inicializando transaccion
                                            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Declarando variables auxiliares
                                                int id_entidad_pago = 0;
                                                SAT_CL.TarifasPago.Tarifa.PerfilPago perfil_pago;
                                                    //Determinando el tipo de entidad a pagar
                                                if (liq.id_operador > 0)
                                                {
                                                    perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Operador;
                                                    id_entidad_pago = liq.id_operador;
                                                }
                                                else if (liq.id_unidad > 0)
                                                {
                                                    perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Unidad;
                                                    id_entidad_pago = liq.id_unidad;
                                                }
                                                else
                                                {
                                                    perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Transportista;
                                                    id_entidad_pago = liq.id_proveedor;
                                                }

                                                //Realizando búsqueda de tarifa de pago aplicable
                                                using (DataTable tarifas = SAT_CL.TarifasPago.Tarifa.ObtieneTarifasPagoServicio(idServicio, perfil_pago, id_entidad_pago, true))
                                                {
                                                    //Si existen registros
                                                    if (tarifas != null)
                                                    {
                                                        //Guardando tarifas en sesión
                                                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], tarifas, "Table13");

                                                        //Si existe más de una tarifa que aplique a este servicio
                                                        if (tarifas.Rows.Count > 1)
                                                        {
                                                            //Habilitando Controles
                                                            habilitaControlesPagos(true);
                                                            //Cargando conjunto de tarifas en control Gridview
                                                            TSDK.ASP.Controles.CargaGridView(gvTarifasPago, tarifas, "IdTarifa", lblOrdenadoTarifasAplicables.Text);
                                                            //Asignando Tooltip para diferenciar la entidad a la que se aplicará la tarifa (Tarifas de Pago Servicio / Tarifas de Pago Movimiento)
                                                            gvTarifasPago.ToolTip = "Tarifas de Pago Servicio";
                                                            //Mostrando ventana modal con resultados
                                                            gestionaVentanas(this, "TarifasAplicables");
                                                            //Instanciando Resultado Positivo
                                                            result = new RetornoOperacion(0,"", true);
                                                            //Mostrando Boton de Pago de Servicio
                                                            btnPagarServicioManual.Visible = true;
                                                        }
                                                        //Si sólo hay una coincidencia
                                                        else if (tarifas.Rows.Count == 1)
                                                        {
                                                            //Aplicando tarifa a servicio
                                                            result = Pago.AplicaTarifaPagoServicio(liq.id_liquidacion, tarifas.Rows[0].Field<int>("IdTarifa"), idServicio,
                                                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor,
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Si se actualizó correctamente
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Obteniendo Pago
                                                                int idPago = result.IdRegistro;

                                                                //Obteniendo Tarifas Secundarias
                                                                using (DataTable dtTarifasSecundarias = SAT_CL.TarifasPago.TarifaCompuesta.ObtieneTarifasSecundarias(tarifas.Rows[0].Field<int>("IdTarifa")))
                                                                {
                                                                    //Validando que Existen Registros
                                                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTarifasSecundarias))
                                                                    {
                                                                        //Recorriendo Ciclo de Tarifas Secundarias
                                                                        foreach (DataRow dr in dtTarifasSecundarias.Rows)
                                                                        {
                                                                            //Aplicando Tarifa Secundaria
                                                                            result = Pago.AplicaTarifaPagoServicio(liq.id_liquidacion, Convert.ToInt32(dr["IdTarifaSecundaria"]), idServicio,
                                                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                            //Validando que la Operación fuese Correcta
                                                                            if (!result.OperacionExitosa)

                                                                                //Terminando Ciclo
                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                        //Instanciando Resultado Positivo
                                                                        result = new RetornoOperacion(idPago);

                                                                    /*/Validando que la Operación fuese Exitosa
                                                                    if (result.OperacionExitosa)
                                                                    {
                                                                        //Realizando actualización de vales y anticipos
                                                                        result = actualizaLiquidacionDepositosValesServicio(idServicio, liq.id_liquidacion, id_entidad_pago, liq.id_tipo_asignacion);

                                                                        //Si no hay errores
                                                                        if (result.OperacionExitosa && liq.ActualizaLiquidacion())
                                                                        {
                                                                            //Cargando los Movimientos y los Pagos
                                                                            cargaServiciosMovimientosRecurso(id_entidad_pago, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                            //Confirmando cambios realizados
                                                                            scope.Complete();
                                                                        }
                                                                    }//*/
                                                                }
                                                            }

                                                            //Si se aplicó la tarifa correctamente
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualizando anticipos y vales de diesel
                                                                result = actualizaLiquidacionDepositosValesServicio(idServicio, liq.id_liquidacion, id_entidad_pago, liq.id_tipo_asignacion);

                                                                //Si no hay errores
                                                                if (result.OperacionExitosa && liq.ActualizaLiquidacion())
                                                                {
                                                                    //Cargando los Movimientos y los Pagos
                                                                    cargaServiciosMovimientosRecurso(id_entidad_pago, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                    //Inicializando Datos de Liquidación
                                                                    cargaDatosEncabezadoLiq();

                                                                    //Sumando Totales
                                                                    sumaTotalesRecursos();

                                                                    //Completando transacción
                                                                    scope.Complete();

                                                                    //Mostrando Mensaje de Operación
                                                                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                                }
                                                            }
                                                        }

                                                        //Validando que las Operaciones no hayan sido Exitosas
                                                        if (!result.OperacionExitosa)
                                                        {
                                                            //Validando excepción de Depositos
                                                            if(!(result.IdRegistro == -3))
                                                            {
                                                                //Habilitando Controles
                                                                habilitaControlesPagos(true);

                                                                //Configurando Control
                                                                btnGuardarPago.CommandArgument = "Servicio";

                                                                //Invocando Ventana de Pagos
                                                                gestionaVentanas(this, "Pagos");
                                                            }

                                                            //Mostrando Mensaje de Operación
                                                            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                        }
                                                    }
                                                    //Si no hay tarifas
                                                    else
                                                    {
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("No se encontraron Tarifas de Pago");
                                                        
                                                        //Habilitando Controles
                                                        habilitaControlesPagos(true);

                                                        //Configurando Control
                                                        btnGuardarPago.CommandArgument = "Servicio";
                                                        txtDescripcion.Text = "Pago por Viaje";

                                                        //Invocando Ventana de Pagos
                                                        gestionaVentanas(this, "Pagos");

                                                        //Mostrando Mensaje de Operación
                                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                        
                                                        /*/Configurando Control
                                                        btnGuardarPago.CommandArgument = "Servicio";
                                                        upbtnGuardarPago.Update();

                                                        //Mostrando Ventana de Movimientos
                                                        gestionaVentanas(this, "Movimientos");

                                                        //Configurando Controles
                                                        btnPagarServicio.Visible = true;
                                                        btnCrearPagoMov.Visible = false;
                                                        //Actualizando Controles
                                                        upbtnPagarServicio.Update();
                                                        upbtnCrearPagoMov.Update();//*/
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Mostrando Excepción
                                            ScriptServer.MuestraNotificacion(this, "Existen Pagos Aplicados al Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                            //Configurando Control
                                            btnGuardarPago.CommandArgument = "Servicio";

                                            //Mostrando Ventana de Movimientos
                                            gestionaVentanas(this, "Movimientos");

                                            //Configurando Controles
                                            btnPagarServicio.Visible = true;
                                            btnCrearPagoMov.Visible = false;
                                        }
                                        
                                        
                                        break;
                                    }
                                case "Anticipos":
                                    {
                                        //Mostrando Ventana de Anticipos
                                        gestionaVentanas(this, "Anticipos");
                                        break;
                                    }
                                case "Comprobaciones":
                                    {
                                        //Mostrando Ventana de Comprobaciones
                                        gestionaVentanas(this, "Comprobaciones");
                                        break;
                                    }
                                case "Diesel":
                                    {
                                        //Mostrando Ventana de Diesel
                                        gestionaVentanas(this, "Diesel");
                                        break;
                                    }
                            }
                        }
                        else if (idMovimiento > 0)
                        {
                            //Cargando Detalles de los Movimientos
                            cargaDetallesMovimientos(idMovimiento);
                            
                            //Validando Comando
                            switch (lnk.CommandName)
                            {
                                case "Pagos":
                                    {                                        
                                            //Validando si existen pagos 
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table16")))
                                        {
                                            //Inicializando Indices
                                            TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

                                            //Mostrando Ventana
                                            gestionaVentanas(this, "PagosMV");
                                        }
                                        else
                                        {
                                            //Validando el Estatus de la Liquidación
                                            if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                            {
                                                //Obteniendo Tipos de Pago
                                                using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                                {
                                                    //Validando que existan los Conceptos de Pago
                                                    if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                                        //Cargando DropDownList
                                                        Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                                    else
                                                        //Inicializando DropDownList
                                                        Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                                }

                                                //Obteniendo Control
                                                LinkButton pago = (LinkButton)sender;

                                                //Instanciando Movimiento
                                                using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                                                {
                                                    //Validando el Comando
                                                    switch (pago.CommandName)
                                                    {
                                                        case "Pagos":
                                                            {
                                                                //Realizando búsqueda de tarifa y en caso de ser tarifa única se aplica el pago
                                                                buscaTarifaPagoMovimiento(mov.id_movimiento, "MovimientoVacio");
                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Inicializando Indices
                                                TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);

                                                //Mostrando Mensaje de Operación
                                                TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                        }
                                        
                                        break;
                                    
                                    }
                                case "Anticipos":
                                    {
                                        //Mostrando Ventana de Anticipos
                                        gestionaVentanas(this, "Anticipos");
                                        break;
                                    }
                                case "Comprobaciones":
                                    {
                                        //Mostrando Ventana de Comprobaciones
                                        gestionaVentanas(this, "Comprobaciones");
                                        break;
                                    }
                                case "Diesel":
                                    {
                                        //Mostrando Ventana de Diesel
                                        gestionaVentanas(this, "Diesel");
                                        break;
                                    }
                            }
                        }
                    }
                    else
                    {   
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe la Liqudiación");

                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }

                
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "No. Servicio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkServicio_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);

                //Inicializando Referencias de Viaje
                ucReferenciasViaje.InicializaControl(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]));

                //Inicializando Ventana de Referencias
                gestionaVentanas(this, "ReferenciasViaje");

                //Actualizando Comando del Boton Cerrar
                lnkCerrarReferencias.CommandArgument = "";
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Ver Devoluciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDevoluciones_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);

                //Invocando Método de Carga
                cargaResumenDevoluciones(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]));

                //Gestionando Ventanas
                gestionaVentanas(this, "Devolucion");
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Vencimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEntidadLiquidacion_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Declarando Variable Auxiliar
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);

                //Validando Comando
                switch(lkb.CommandName)
                {
                    case "Operador":
                        {
                            //Validando que exista un Operador
                            if (Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdOperador"]) > 0)

                                //Inicializando Control
                                wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Operador, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdOperador"]));
                            break;
                        }
                    case "Unidad":
                        {
                            //Validando que exista un Operador
                            if (Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdUnidad"]) > 0)

                                //Inicializando Control
                                wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdUnidad"]));
                            break;
                        }
                    case "Remolque":
                        {
                            //Validando que exista un Operador
                            if (Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdUnidad2"]) > 0)

                                //Inicializando Control
                                wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdUnidad2"]));
                            break;
                        }
                }

                //Abriendo Ventana de Historial
                gestionaVentanas(this, "HistorialVencimiento");
            }
        }

        /// <summary>
        /// Evento Generado al dar click en Origen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOrigen_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);
                //Carga Paradas
                cargaParadas();
                //Cierra la ventana modal de Historial
                gestionaVentanas(gvServiciosLiquidacion, "ResumenParadas");
            }

        }

        /// <summary>
        /// Carga Paradas
        /// </summary>
        private void cargaParadas()
        {
            //Obtenemos Paradas ligados al Id de Servicio
            using (DataTable mit = Parada.CargaParadasParaVisualizacionDespacho(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"])))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvParadas, mit, "IdOrigen", "", true, 3);


                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "mit19");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "mi19");
                }
            }
        }

        #endregion

        #region Eventos GridView "Pagos"

        /// <summary>
        /// Eventos Producido al Cambiar el Ordenamiento de "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoPago.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);

            //Invocando Método de Suma de Pagos
            sumaTotalPagos();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página de "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);

            //Invocando Método de Suma de Pagos
            sumaTotalPagos();
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando la fila de datos correspondiente
                DataRow dr = ((DataRowView)e.Row.DataItem).Row.Table.Rows[e.Row.DataItemIndex];

                //Si hay registrosa ligados
                if (dr["ValorU"] != DBNull.Value)
                {
                    //Validando que el Valor sea Positivo
                    if (Convert.ToDecimal(dr["ValorU"]) > 0)

                        //Asignando Estilo
                        e.Row.CssClass = "pago_positivo";
                    else
                        //Asignando Estilo
                        e.Row.CssClass = "pago_negativo";
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoPago.SelectedValue));

            //Invocando Método de Suma de Pagos
            sumaTotalPagos();
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Eliminar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarPago_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagos, sender, "lnk", false);

                //Instanciando Pago
                using (Pago pago = new Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                {
                    //Validando que Exista
                    if (pago.id_pago > 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pago.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Asignando Comando
                                    btnAceptarOperacion.CommandName = "Pago";

                                    //Alternando Ventana Modal
                                    gestionaVentanas(upgvPagos, "ConfirmacionOperacion");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Editar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarPago_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagos, sender, "lnk", false);

                //Instanciando Pago
                using (SAT_CL.Liquidacion.Pago pag = new SAT_CL.Liquidacion.Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                {
                    //Validando que existe el Pago
                    if (pag.id_pago != 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pag.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Habilitando Controles
                                    habilitaControlesPagos(true);

                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))
                                        {
                                            //Validando que exista un Movmiento Seleccionado
                                            if (gvServiciosLiquidacion.SelectedIndex != -1)
                                            {
                                                //Validando Movmiento
                                                if (Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]) > 0)
                                                {
                                                    //Obteniendo Tipos de Pago
                                                    using (DataTable dtConceptosPagoGnral = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                                    {
                                                        //Validando que existan los Conceptos de Pago Generales
                                                        if (Validacion.ValidaOrigenDatos(dtConceptosPagoGnral))
                                                        {
                                                            //Recorriendo Conceptos Generales
                                                            foreach (DataRow dr in dtConceptosPagoGnral.Rows)
                                                            {
                                                                //Añadiendo Conceptos Generales
                                                                dtConceptosPago.Rows.Add(Convert.ToInt32(dr["id"]), dr["descripcion"]);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        }
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByValue(pag.id_tipo_pago.ToString());
                                    
                                    //Validando que exista el Tipo
                                    if (li != null)
                                    {
                                        //Limpiando Items
                                        ddlTipoPago.Items.Clear();
                                        //Añadiendo Item
                                        ddlTipoPago.Items.Add(li);
                                    }

                                    //Asignando Valores 
                                    ddlTipoPago.SelectedValue = pag.id_tipo_pago.ToString();
                                    txtCantidad.Text = pag.objDetallePago.cantidad.ToString();
                                    txtValorU.Text = (pag.objDetallePago.valor_unitario >= 0 ? pag.objDetallePago.valor_unitario : -1 * pag.objDetallePago.valor_unitario).ToString();
                                    txtTotal.Text = (pag.objDetallePago.monto >= 0 ? pag.objDetallePago.monto : -1 * pag.objDetallePago.monto).ToString();
                                    txtDescripcion.Text = pag.descripcion;
                                    txtReferencia.Text = pag.referencia;

                                    //Asignando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "EditaPago";

                                    //Mostrando ventana de captura
                                    TSDK.ASP.ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "ventanaTarifasPago", "contenedorVentanaPagos", "ventanaPagos");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                    else
                        //Asignando Valores 
                        limpiaControlesPagos();

                    //Quitando la selección de las Filas
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Bitacora"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacoraPago_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvPagos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagos, sender, "lnk", false);

                //Instanciando Pago
                using (Pago pago = new Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                {
                    //Validando que Exista el Pago
                    if (pago.habilitar)
                    {
                        //Construyendo URL 
                        string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/LiquidacionSimplificada.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=79&idR=" + gvPagos.SelectedDataKey["IdPago"].ToString() + "&tB=Bitacora Pagos");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
                        //Abriendo Nueva Ventana
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Contenido del Control "Cantidad"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal cantidad = 0.00M, valoru = 0.00M; ;

            //Obteniendo Valores
            if (Decimal.TryParse(txtCantidad.Text, out cantidad) && Decimal.TryParse(txtValorU.Text, out valoru))
            {
                //Asignando Valor Calculado
                txtCantidad.Text = string.Format("{0:0.00}", cantidad);
                txtTotal.Text = string.Format("{0:0.00}", cantidad * valoru);
            }
            else
            {
                //Asignando Valor por Defecto
                txtTotal.Text =
                txtCantidad.Text = "0.00";
            }

            //Asignando Enfoque al Control
            txtValorU.Focus();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Contenido del Control "Valor Unitario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtValorU_TextChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal cantidad = 0.00M, valoru = 0.00M; ;

            //Obteniendo Valores
            if (Decimal.TryParse(txtCantidad.Text, out cantidad) && Decimal.TryParse(txtValorU.Text, out valoru))
            {
                //Asignando Valor Calculado
                txtValorU.Text = string.Format("{0:0.00}", valoru);
                txtTotal.Text = string.Format("{0:0.00}", cantidad * valoru);
            }
            else
            {
                //Asignando Valor por Defecto
                txtTotal.Text =
                txtValorU.Text = "0.00";
            }

            //Validando si la Descripción esta Habilitada
            if (txtDescripcion.Enabled)

                //Asignando Enfoque al Control
                txtDescripcion.Focus();
            else
                //Asignando Enfoque al Control
                txtReferencia.Focus();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar Operación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOperacion_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Inicializando Transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Declarando Variable de Pago
                        int idPago = 0;
                        
                        //Validando Comando del Control
                        switch(btnAceptarOperacion.CommandName)
                        {
                            case "PagoLigado":
                                {
                                    //Obteniendo Pago a Eliminar
                                    idPago = Convert.ToInt32(gvPagosLigados.SelectedDataKey["IdPago"]);
                                    break;
                                }
                            case "Pago":
                                {
                                    //Obteniendo Pago a Eliminar
                                    idPago = Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"]);
                                    break;
                                }
                            case "PagoMV":
                                {
                                    //Obteniendo Pago a Eliminar
                                    idPago = Convert.ToInt32(gvPagosMV.SelectedDataKey["IdPago"]);
                                    break;
                                }
                        }

                        //Instanciando Pago
                        using (SAT_CL.Liquidacion.Pago pay = new SAT_CL.Liquidacion.Pago(idPago))
                        {
                            //Validando que existe 
                            if (pay.id_pago != 0)
                            {
                                //Deshabilitando Pago
                                result = pay.DeshabilitaPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Pago.");

                            //Validando que la Operación fuese exitosa
                            if (result.OperacionExitosa)
                            {
                                //Deshabilitando Detalle de Liquidación
                                result = pay.objDetallePago.DeshabilitaDetalleLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que el Detalle se haya deshabilitado
                                if (result.OperacionExitosa)
                                {
                                    //Validando si existe un solo Movimiento
                                    if (pay.objDetallePago.id_movimiento > 0)
                                    {
                                        //Instanciando Pago del Movimiento
                                        using (SAT_CL.Liquidacion.PagoMovimiento pm = new SAT_CL.Liquidacion.PagoMovimiento(pay.id_pago, pay.objDetallePago.id_movimiento))
                                        {
                                            //Validando que exista el Pago del Movimiento
                                            if (pm.id_pago_movimiento > 0)
                                            {
                                                //Deshabilitando Pago del Movimiento
                                                result = pm.DeshabilitaPagoMovimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación hay sido exitosa
                                                if (result.OperacionExitosa)

                                                    //Actualziando la Referencia para quitar la Liquidación 
                                                    result = actualizaLiquidacionDepositosVales(pay.objDetallePago.id_movimiento, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Obteniendo Movimientos
                                        using (DataTable dtPagoMovimientos = SAT_CL.Liquidacion.PagoMovimiento.ObtienePagosMovimiento(pay.id_pago))
                                        {
                                            //Validando que existan Pagos a Movimientos
                                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagoMovimientos))
                                            {
                                                //Iniciando Recorrido de Movimientos
                                                foreach (DataRow dr in dtPagoMovimientos.Rows)
                                                {
                                                    //Instanciando Pago del Movimiento
                                                    using (SAT_CL.Liquidacion.PagoMovimiento pm = new SAT_CL.Liquidacion.PagoMovimiento(Convert.ToInt32(dr["Id"])))
                                                    {
                                                        //Validando que exista el Pago del Movimiento
                                                        if (pm.id_pago_movimiento > 0)
                                                        {
                                                            //Deshabilitando Pago del Movimiento
                                                            result = pm.DeshabilitaPagoMovimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operacion Actual haya sido Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualziando la Referencia para quitar la Liquidación 
                                                                result = actualizaLiquidacionDepositosVales(pm.id_movimiento, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, false);

                                                                //Si la Operación Falló
                                                                if (!result.OperacionExitosa)
                                                                    break;
                                                            }
                                                            else
                                                                //Rompiendo Ciclo
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //Validando que la Operacion Actual haya sido Exitosa
                                    if (result.OperacionExitosa)

                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }

                    }
                    //Validando que las Operaciones hayan sido exitosas
                    if (result.OperacionExitosa)
                    {
                        //Validando que exista un Viaje Seleccionado
                        if (gvServiciosLiquidacion.SelectedIndex != -1)
                        {
                            //Obteniendo Valores
                            int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                            int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                            //Cargando Datos de Encabezado
                            cargaDatosEncabezadoLiq();

                            //Validando que Exista el Servicio
                            if (idServicio > 0)
                            
                                //Marcando Fila
                                Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                            
                            //Validando que Exista el Movimiento
                            else if (idMovimiento > 0)

                                //Marcando Fila
                                Controles.MarcaFila(gvServiciosLiquidacion, idMovimiento.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                            //Carga Movimientos, Anticipos etc.
                            cargaMovimientosServicio(idServicio, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                            //Sumando Totales
                            sumaTotalesRecursos();
                        }
                        else
                        {
                            //Cargando Datos de Encabezado
                            cargaDatosEncabezadoLiq();
                            
                            //Invocando Método de Carga de Pagos generales
                            cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);
                            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                        }

                        //Validando Comando
                        if (btnAceptarOperacion.CommandName == "PagoLigado")

                            //Abriendo Ventana de Movimiento
                            gestionaVentanas(this, "Movimientos");//*/


                        //Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvPagos);
                        TSDK.ASP.Controles.InicializaIndices(gvPagosLigados);
                        TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

                        //Alternando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptarOperacion, upbtnAceptarOperacion.GetType(), "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                    }
                }
            }

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarOperacion, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Operación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarOperacion_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);
            TSDK.ASP.Controles.InicializaIndices(gvPagosLigados);
            TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, upbtnCancelarOperacion.GetType(), "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");

            //Ocultando Ventana
            gestionaVentanas(upbtnCancelarOperacion, "ConfirmacionOperacion");
        }

        #endregion

        #region Eventos GridView "Pagos Ligados"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Pagos Ligados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPagosLigados_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvPagosLigados, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table14"), Convert.ToInt32(ddlTamanoPago.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Pagos Ligados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosLigados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoPagoLigado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPagosLigados, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table14"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar de Página del GridView "Pagos Ligados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosLigados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPagosLigados, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table14"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Editar el Pago Ligado al Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarPagoLigado_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagosLigados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagosLigados, sender, "lnk", false);

                //Instanciando Pago
                using (SAT_CL.Liquidacion.Pago pag = new SAT_CL.Liquidacion.Pago(Convert.ToInt32(gvPagosLigados.SelectedDataKey["IdPago"])))
                {
                    //Validando que existe el Pago
                    if (pag.id_pago != 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pag.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByValue(pag.id_tipo_pago.ToString());
                                    
                                    //Validando que exista el Tipo
                                    if (li != null)
                                    {
                                        //Limpiando Items
                                        ddlTipoPago.Items.Clear();
                                        //Añadiendo Item
                                        ddlTipoPago.Items.Add(li);
                                    }

                                    //Asignando Valores 
                                    ddlTipoPago.SelectedValue = pag.id_tipo_pago.ToString();
                                    txtCantidad.Text = pag.objDetallePago.cantidad.ToString();
                                    txtValorU.Text = pag.objDetallePago.valor_unitario.ToString();
                                    txtTotal.Text = pag.objDetallePago.monto.ToString();
                                    txtDescripcion.Text = pag.descripcion;
                                    txtReferencia.Text = pag.referencia;

                                    //Habilitando Controles
                                    habilitaControlesPagos(true);
                                    //Asignando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "EditaPagoLigado";

                                    //Mostrando ventana de captura
                                    gestionaVentanas(upgvPagosLigados, "Pagos");
                                    gestionaVentanas(upgvPagosLigados, "Movimientos");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                    else
                        //Asignando Valores 
                        limpiaControlesPagos();

                    //Quitando la selección de las Filas
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar el Pago Ligado al Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarPagoLigado_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagosLigados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagosLigados, sender, "lnk", false);

                //Instanciando Pago
                using (Pago pago = new Pago(Convert.ToInt32(gvPagosLigados.SelectedDataKey["IdPago"])))
                {
                    //Validando que Exista
                    if (pago.id_pago > 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pago.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Asignando Comando
                                    btnAceptarOperacion.CommandName = "PagoLigado";

                                    //Alternando Ventana Modal
                                    gestionaVentanas(upgvPagosLigados, "Movimientos");
                                    gestionaVentanas(upgvPagosLigados, "ConfirmacionOperacion");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagosLigados);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Bitacora"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacoraPagoLigado_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvPagosLigados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagosLigados, sender, "lnk", false);
                //Construyendo URL 
                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/LiquidacionSimplificada.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=79&idR=" + gvPagosLigados.SelectedDataKey["IdPago"].ToString() + "&tB=Bitacora Pagos");
                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
                //Abriendo Nueva Ventana
                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
            }
        }

        #endregion

        #region Eventos GridView "Cobros Recurrentes Liquidación"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCRL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoCRL.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobroRecurrenteLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCRL.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobroRecurrenteLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerHistorial_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvCobroRecurrenteLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvCobroRecurrenteLiquidacion, sender, "lnk", false);

                //Instanciando Cobro Recurrente
                using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(gvCobroRecurrenteLiquidacion.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Cobro Recurrente
                    if (cr.id_cobro_recurrente > 0)
                    {
                        //Instanciando Cobros Recurrentes
                        using (DataTable dtCobrosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneHistorialCobrosRecurrentes(cr.id_cobro_recurrente, cr.id_tipo_entidad_aplicacion, cr.id_unidad, cr.id_operador, cr.id_proveedor_compania, cr.id_compania_emisor))
                        {
                            //Validando que existen los Cobros Recurrentes
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosRecurrentes))
                            {
                                //Cargando Cobro Recurrente
                                TSDK.ASP.Controles.CargaGridView(gvCobrosRecurrentesHistorial, dtCobrosRecurrentes, "Id", "");

                                //Añadiendo Tabla a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCobrosRecurrentes, "Table11");
                            }
                            else
                            {
                                //Inicializando Cobro Recurrente
                                TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentesHistorial);

                                //Añadiendo Tabla a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table11");
                            }
                        }
                    }
                    else
                    {
                        //Inicializando Cobro Recurrente
                        TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentesHistorial);

                        //Añadiendo Tabla a Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table11");
                    }

                    //Alternando Ventana Modal
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvCobroRecurrenteLiquidacion, upgvCobroRecurrenteLiquidacion.GetType(), "VerCobrosHistorial", "contenedorVentanaHistorialCobrosRecurrentes", "ventanaHistorialCobrosRecurrentes");
                }
            }
        }

        #endregion

        #region Eventos GridView "Anticipos"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoAnticipos.SelectedValue));

            //Invocando Método de Suma
            sumaTotalAnticipos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoAnticipo.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression);

            //Invocando Método de Suma
            sumaTotalAnticipos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewSelectedIndex);

            //Invocando Método de Suma
            sumaTotalAnticipos();
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarDeposito_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando Estatus
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvAnticipos, sender, "lnk", false);

                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"])))
                        {
                            //Validando que este en el Estatus Correcto
                            if (dep.habilitar && dep.Estatus == Deposito.EstatusDeposito.PorLiquidar)
                            {
                                //Inicializando Controles
                                inicializaControlComprobaciones(0, Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"]));

                                //Gestionando Ventanas
                                gestionaVentanas(gvAnticipos, "Anticipos");
                                gestionaVentanas(gvAnticipos, "AltaComprobaciones");
                            }
                            else
                                //Mostarndo Excepción
                                ScriptServer.MuestraNotificacion(this, new RetornoOperacion("El Deposito debe de estar en Estatus 'Por Liquidar' para poder Comprobarse"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                    else
                        //Mostarndo Excepción
                        ScriptServer.MuestraNotificacion(this, new RetornoOperacion("La Liquidación esta Cerrada, Imposible su Edición"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Origen de la Fila
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                //Validando Estatus
                switch (rowView["IdEstatus"].ToString())
                {
                    case "3":
                        {
                            //Cambiando color de fondo de la fila a rojo
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                            break;
                        }
                }
            }
        }

        #endregion

        #region Eventos GridView "Comprobaciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), Convert.ToInt32(ddlTamanoComp.SelectedValue));

            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.SortExpression);
            
            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaTotalComprobaciones();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando Estatus
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                        //Inicializando Controles
                        inicializaControlComprobaciones(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"]), 0);

                        //Gestionando Ventanas
                        gestionaVentanas(gvComprobaciones, "Comprobaciones");
                        gestionaVentanas(gvComprobaciones, "AltaComprobaciones");
                    }
                    else
                        //Mostarndo Excepción
                        ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Variable Auxiliar
                            int idCmp = 0;
                            
                            //Seleccionando Fila
                            Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                            //Transacción
                            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            
                            //Instanciando Comprobación
                            using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"])))
                            {
                                //Validando que exista la COmprobación
                                if (cmp.id_comprobacion > 0)
                                {
                                    //Validando que el Movimiento este Terminado
                                    if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))
                                    {
                                        //Deshabilitando el Registro
                                        result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Operación Exitosa?
                                        if(result.OperacionExitosa)
                                        {
                                            //Obteniendo Comprobación
                                            idCmp = result.IdRegistro;
                                            
                                            //Obteniendo Facturas
                                            using (DataTable dtFacturas = FacturadoProveedorRelacion.ObtieneFacturasComprobacion(idCmp))
                                            {
                                                //Validando que existan registros
                                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                                                {
                                                    //Recorriendo Filas
                                                    foreach (DataRow dr in dtFacturas.Rows)
                                                    {
                                                        //Instanciando Fatura
                                                        using (FacturadoProveedorRelacion cf = new FacturadoProveedorRelacion(Convert.ToInt32(dr["IdFacturaRelacion"])))
                                                        {
                                                            //Validando que Exista
                                                            if(cf.habilitar)
                                                            {
                                                                //Deshabilita Factura
                                                                result = cf.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Operación Exitosa?
                                                                if(result.OperacionExitosa)
                                                                {
                                                                    //Instanciando Factura
                                                                    using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(dr["IdFactura"])))
                                                                    {
                                                                        //Deshabilitando Factura
                                                                        result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                        //Validando la Operación de la Transacción
                                                                        if (!result.OperacionExitosa)
                                                                            //Terminando Ciclo
                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                    //Terminando Ciclo
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Comprobación
                                                    result = new RetornoOperacion(idCmp);

                                                //Operación Exitosa?
                                                if(result.OperacionExitosa)

                                                    //Instanciando Comprobación
                                                    result = new RetornoOperacion(idCmp);

                                                //Inicializando Indices
                                                TSDK.ASP.Controles.InicializaIndices(gvFacturasComprobacion);
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");
                                    
                                    //Validando que la Operación haya sido exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Entidad
                                        int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                                        //Actualizando Carga de Totales de la Liquidación
                                        cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                                        //Obteniendo Valores
                                        int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                        int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                        //Actualizando Servicios y Movimientos en Vacio
                                        cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                        //Validando que sea un Servicio
                                        if (idServicio != 0)
                                            //Marcando Fila
                                            Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                                        //Si es Movimiento
                                        else
                                            //Marcando Fila
                                            Controles.MarcaFila(gvServiciosLiquidacion, idMovimiento.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                                        //Cargando Detalles
                                        cargaDetallesMovimientos(cmp.objDetalleComprobacion.id_movimiento);

                                        //Inicializando Indices
                                        TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                                        //Sumando Totales
                                        sumaTotalesRecursos();

                                        //Completando transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }

                //Mostrando Mensaje
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #region Alta Comprobaciones

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarComprobacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaComprobacion();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarComprobacion_Click(object sender, EventArgs e)
        {
            //Inicializando Controles
            inicializaControlComprobaciones(0, 0);

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                //Cargando resultados Actualizados
                cargaMovimientosServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
            }

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarComprobacion, upbtnCancelarComprobacion.GetType(), "VentanaComprobacion", "contenedorVentanaComprobaciones", "ventanaComprobaciones");
        }

        #endregion

        #endregion

        #region Eventos GridView "Facturas Comprobación"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), Convert.ToInt32(ddlTamanoFacComp.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoFacComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasComprobacion.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasComprobacion, sender, "lnk", false);

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Existe la Liquidación
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Inicializando Transacción
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Instanciando Factura
                                using (FacturadoProveedorRelacion cfp = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFacturaRelacion"])))
                                {
                                    //Validando que exista la Factura
                                    if (cfp.habilitar)
                                    {
                                        //Deshabilitando Factura
                                        result = cfp.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando la Operación de la Transacción
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFactura"])))
                                            {
                                                //Deshabilitando Factura
                                                result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando la Operación de la Transacción
                                                if (result.OperacionExitosa)
                                                {
                                                    //Instanciando Comprobación
                                                    using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text)))
                                                    {
                                                        //Validando que exista la Comprobación
                                                        if (cmp.id_comprobacion > 0)
                                                        {
                                                            //Actualizando el Total de las Comprobaciones
                                                            result = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando la Operación de la Transacción
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Determinando que entidad se afecta con esta liquidación
                                                                int id_entidad = 0;
                                                                switch (liq.tipo_asignacion)
                                                                {
                                                                    case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad:
                                                                        id_entidad = liq.id_unidad;
                                                                        break;
                                                                    case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador:
                                                                        id_entidad = liq.id_operador;
                                                                        break;
                                                                    case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor:
                                                                        id_entidad = liq.id_proveedor;
                                                                        break;
                                                                }

                                                                //Invocando Método de Actualización de Valores
                                                                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                                                                //Obteniendo Valores
                                                                int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                                                int idMovimeinto = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                                                //Cargando recursos
                                                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                //Validando que exista el Servicio
                                                                if(idServicio != 0)
                                                                    //Marcando Fila
                                                                    Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                                                                else
                                                                    //Marcando Fila
                                                                    Controles.MarcaFila(gvServiciosLiquidacion, idMovimeinto.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                                                                
                                                                //Cargando Facturas
                                                                cargaFacturasComprobacion(cmp.id_comprobacion);

                                                                //Sumando Totales
                                                                sumaTotalesRecursos();

                                                                //Completando Transacción
                                                                trans.Complete();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("La Liquidación se encuentra Cerrada");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe la Liquidación");
                }

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
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
            gestionaVentanas(btnAgregarFactura, "FacturacionWS");
            btnAceptarValidacion.CommandArgument = "Comprobacion";
            btnCancelarValidacion.CommandArgument = "Comprobacion";
        }

        #endregion

        #region Eventos GridView "Cobros Recurrentes Pendientes"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCR_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), Convert.ToInt32(ddlTamanoCR.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCR.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEstatus_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvCobrosRecurrentes.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvCobrosRecurrentes, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que la Liquidación se encuentre Registrada
                    if (liq.habilitar && liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Instanciando Cobro Recurrente
                        using (SAT_CL.Liquidacion.CobroRecurrente cobro = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(gvCobrosRecurrentes.SelectedDataKey["Id"])))
                        {
                            //Validando que exista el Cobro
                            if (cobro.habilitar)
                            {
                                //Validando Comando
                                switch (lkb.CommandName)
                                {
                                    case "Pausar":
                                        {
                                            //Pausando Cobro Recurrente
                                            result = cobro.ActualizaEstatusTerminoCobroRecurrente(SAT_CL.Liquidacion.CobroRecurrente.EstatusTermino.Pausa, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                    case "Activar":
                                        {
                                            //Pausando Cobro Recurrente
                                            result = cobro.ActualizaEstatusTerminoCobroRecurrente(SAT_CL.Liquidacion.CobroRecurrente.EstatusTermino.Vigente, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existe el Cobro Recurrente");

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Cargando Cobros Recurrentes
                                cargaCobrosRecurrentes(liq.id_tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora, liq.fecha_liquidacion);

                                //Calculando Valores
                                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Liquidación se encuentra Cerrada");
                }

                //Mostrando Resultado
                ScriptServer.MuestraNotificacion(lkb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Eventos GridView "Tarifas Aplicables"

        /// <summary>
        /// Maneja el cambio de selección en el catálogo de Tarifas Aplicables mostrados por página 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGridViewTarifasAplicables_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), Convert.ToInt32(ddlTamanoGridViewTarifasAplicables.SelectedValue));
        }
        /// <summary>
        /// Maneja el cambio de página de Gridview de tarifas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifasPago_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.NewPageIndex);
        }
        /// <summary>
        /// Maneja el ordenamiento de las Tarifas Aplicables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifasPago_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoTarifasAplicables.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.SortExpression);
        }
        /// <summary>
        /// Maneja el evento Click sobre el boton APLICAR del Gridview de Tarifas de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionarTarifaPago_Click(object sender, EventArgs e)
        {
            //Seleccionando fila actual
            Controles.SeleccionaFila(gvTarifasPago, sender, "lnk", false);
            //Aplicando tarifa seleccionada sobre el registro correspondiente
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Determinando que entidad se afecta con esta liquidación
                    int id_entidad = 0;
                    switch (liquidacion.tipo_asignacion)
                    {
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad:
                            id_entidad = liquidacion.id_unidad;
                            break;
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador:
                            id_entidad = liquidacion.id_operador;
                            break;
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor:
                            id_entidad = liquidacion.id_proveedor;
                            break;
                    }

                    //Si la búsqueda de tarifa se relizó por servicio
                    if (gvTarifasPago.ToolTip == "Tarifas de Pago Servicio")
                    {
                        //Aplicando tarifa al servicio
                        resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value), Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]),
                                                                liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si se actualizó correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Obteniendo Pago
                            int idPago = resultado.IdRegistro;

                            //Obteniendo Tarifas Secundarias
                            using (DataTable dtTarifasSecundarias = SAT_CL.TarifasPago.TarifaCompuesta.ObtieneTarifasSecundarias(Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value)))
                            {
                                //Validando que Existen Registros
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTarifasSecundarias))
                                {
                                    //Recorriendo Ciclo de Tarifas Secundarias
                                    foreach (DataRow dr in dtTarifasSecundarias.Rows)
                                    {
                                        //Aplicando Tarifa Secundaria
                                        resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(dr["IdTarifaSecundaria"]), Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]),
                                                                liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Correcta
                                        if (!resultado.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                }
                                else
                                    //Instanciando Resultado Positivo
                                    resultado = new RetornoOperacion(idPago);

                                //Validando que la Operación fuese Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Realizando actualización de vales y anticipos
                                    resultado = actualizaLiquidacionDepositosValesServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liquidacion.id_liquidacion, id_entidad, liquidacion.id_tipo_asignacion);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                    {
                                        //Obteniendo Valor
                                        int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                        int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                        //Actualizando Servicios y Movimientos en Vacio
                                        cargaServiciosMovimientosRecurso(id_entidad, liquidacion.id_tipo_asignacion, liquidacion.fecha_liquidacion);

                                        //Marcando Fila
                                        Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                                        //Sumando Totales
                                        sumaTotalesRecursos();

                                        //Inicializando Datos de Encabezado
                                        cargaDatosEncabezadoLiq();
                                        
                                        //Confirmando cambios realizados
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Configurando Controles
                            habilitaControlesPagos(true);
                            txtDescripcion.Text = "Pago por Servicio";
                            btnGuardarPago.CommandArgument = "Servicio";

                            //Invocando Ventana de Pagos
                            gestionaVentanas(this, "Pagos");
                        }
                    }
                    //Si la búsqueda fue para un movimiento
                    else
                    {
                        //Aplicando tarifa
                        resultado = Pago.AplicaTarifaPagoMovimiento(liquidacion.id_liquidacion, Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value), Convert.ToInt32(gvMovimientos.SelectedDataKey.Value),
                                                                  liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no hay problemas con el pago
                        if (resultado.OperacionExitosa)
                        {
                            //Realizando actualización de vales y anticipos
                            resultado = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad, liquidacion.id_tipo_asignacion, true);

                            //Si no hay errores
                            if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                            {
                                //Obteniendo Valor
                                int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                //Actualizando Servicios y Movimientos en Vacio
                                cargaServiciosMovimientosRecurso(id_entidad, liquidacion.id_tipo_asignacion, liquidacion.fecha_liquidacion);

                                //Marcando Fila
                                Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                                //Sumando Totales
                                sumaTotalesRecursos();

                                //Confirmando cambios realizados
                                scope.Complete();
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("Error al actualizar anticipos y vales del movimiento ID '{0}': {1}", gvMovimientos.SelectedDataKey.Value, resultado.Mensaje));
                        }
                        else
                        {

                        }
                    }

                    //Cerrando ventana modal con resultados
                    gestionaVentanas(upgvTarifasPago, "TarifasAplicables");
                }
            }

            //Borrando selección de elemento
            TSDK.ASP.Controles.InicializaIndices(gvTarifasPago);

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos GridView "Diesel"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDiesel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), Convert.ToInt32(ddlTamanoDiesel.SelectedValue));

            //Invocando Método de Suma
            sumaTotalDiesel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaTotalDiesel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoDiesel.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.SortExpression);

            //Invocando Método de Suma
            sumaTotalDiesel();
        }

        #endregion

        #region Eventos GridView "Evidencias"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEvidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), Convert.ToInt32(ddlTamanoEvidencia.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarEvidencia_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoEvidencias.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al Control GridView "Evidencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Control
                ImageButton ib = (ImageButton)e.Row.FindControl("ibEstatus");

                //Validando que exista el Control
                if (ib != null)
                {
                    //Validando el Estatus
                    switch (((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[2].ToString())
                    {
                        //Recibido
                        case "1":
                        //Recibido c/Reenvio
                        case "2":
                            {
                                //Asignando Valores
                                ib.ImageUrl = "~/Image/EstatusRecibido.png";
                                ib.Visible = true;
                                break;
                            }
                        default:
                            {
                                //Asignando Valores
                                ib.ImageUrl = "";
                                ib.Visible = false;
                                break;
                            }
                    }
                }
            }
        }

        #endregion

        #region Eventos Viajes y Movimientos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoMov_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), Convert.ToInt32(ddlTamanoMov.SelectedValue));
            //Deshabilitando Controles de Captura de Pagos
            habilitaControlesPagos(false);
            limpiaControlesPagos();
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoMov.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.NewPageIndex);
            //Deshabilitando Controles de Captura de Pagos
            habilitaControlesPagos(false);
            limpiaControlesPagos();
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando la fila de datos correspondiente
                DataRow dr = ((DataRowView)e.Row.DataItem).Row.Table.Rows[e.Row.DataItemIndex];
                //Si hay registrosa ligados
                if (dr["NoPago"] != DBNull.Value)
                {
                    //Obteniendo Indicador de Pago
                    int idPago = Convert.ToInt32(dr["NoPago"]);

                    //Obteniendo Controles de interés sobre la fila del gridview
                    LinkButton pago = (LinkButton)e.Row.FindControl("lnkPagarMov");
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkVarios");
                    LinkButton evidencia = (LinkButton)e.Row.FindControl("lnkEvidencias");

                    //Validando que exista el Control "lnkPagarMov"
                    if (pago != null)
                    {
                        //Cambiando el Texto del Control    
                        //Si ya tiene pago y no es de tarifa
                        if (idPago != 0)
                            pago.CommandName = pago.Text = "";
                        //Si no hay pago
                        else
                        {
                            //Validando que Existan Pagos Generales
                            if(SAT_CL.Liquidacion.Pago.ValidaPagoServicioGeneral(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"])))

                                //Limpiando Control
                                pago.CommandName = pago.Text = "";
                            else
                                //Asignando valor
                                pago.CommandName = pago.Text = "Pagar";
                        }
                    }

                    //Validando que exista el Control "chkVarios"
                    if (chk != null)
                        //Cambiando la Habilitación del Control
                        chk.Enabled = idPago != 0 ? false : (SAT_CL.Liquidacion.Pago.ValidaPagoServicioGeneral(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"])) ? false : true);

                    /*/Validando que exista el Control "lnkEvidencias"
                    if (evidencia != null)
                        //Cambiando el Texto del Control
                        evidencia.Visible = ((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[7].ToString() != "0" ? true : false;*/
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Marcador del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Obteniendo Control Disparador
            CheckBox chk = (CheckBox)sender;

            //Obteniendo Tipos de Pago
            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                //Validando que existan los Conceptos de Pago
                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                    //Cargando DropDownList
                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                else
                    //Inicializando DropDownList
                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
            }

            //Validando el Control
            switch (chk.ID)
            {
                case "chkTodos":
                    {
                        //Limpiando Controles de Pagos
                        limpiaControlesPagos();
                        //Validando que la fila sea seleccionada
                        if (!chk.Checked)
                        {
                            //Seleccionando Todas las Filas
                            TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", chk.Checked);
                            //Visualizando Control
                            btnCrearPagoMov.Visible = btnCrearPagoMov.Enabled = false;
                        }
                        else
                        {
                            //Invocando Método de Configuración
                            configuraVentanaPagos("VariosMovimientos");
                        }
                        //Habilitando Controles según el Valor
                        habilitaControlesPagos(chk.Checked);

                        break;
                    }
                case "chkVarios":
                    {
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvRows = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                        //Habilitando Controles según la condición
                        habilitaControlesPagos(gvRows.Length > 0);
                        //Si no existen filas seleccionadas
                        if (gvRows.Length > 0)
                        {
                            //Obteniendo Item de forma Dinamica*/
                            ListItem li = ddlTipoPago.Items.FindByText("Movimiento");

                            //Validando que exista el Tipo
                            if (li != null)
                            {
                                //Limpiando Items
                                ddlTipoPago.Items.Clear();
                                //Añadiendo Item
                                ddlTipoPago.Items.Add(li);
                            }
                            //Asignando Valor
                            txtDescripcion.Text = "Pago por Movimiento";
                            //Mostrando Total de Movimientos
                            txtCantidad.Text = gvRows.Length.ToString();

                            //Visualizando Control
                            btnCrearPagoMov.Visible = btnCrearPagoMov.Enabled = true;
                        }
                        else
                        {
                            //Limpiando Controles de Pagos
                            limpiaControlesPagos();

                            //Visualizando Control
                            btnCrearPagoMov.Visible = btnCrearPagoMov.Enabled = false;
                        }
                        break;
                    }
            }
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPagarMov_Click(object sender, EventArgs e)
        {
            //Validando que existan Movimientos
            if (gvMovimientos.DataKeys.Count > 0)
            {
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Seleccionando la Fila
                            Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

                            //Obteniendo Tipos de Pago
                            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                            {
                                //Validando que existan los Conceptos de Pago
                                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                    //Cargando DropDownList
                                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                else
                                    //Inicializando DropDownList
                                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                            }

                            //Obteniendo Control
                            LinkButton pago = (LinkButton)sender;

                            //Instanciando Movimiento
                            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                            {
                                //Validando el Comando
                                switch (pago.CommandName)
                                {
                                    case "Pagar":
                                        {
                                            //Realizando búsqueda de tarifa y en caso de ser tarifa única se aplica el pago
                                            buscaTarifaPagoMovimiento(mov.id_movimiento, "Movimiento");
                                            gestionaVentanas(this, "Movimientos");
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento disparado al Presionar el LinkButton "Evidencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEvidencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvMovimientos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

                //Validando si existen Evidencias
                if (Convert.ToInt32(gvMovimientos.SelectedDataKey["NoSegmento"]) > 0)
                {
                    //Instanciando el Movimiento
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                    {
                        //Validando que exista el Movimiento
                        if (mov.id_servicio > 0)
                        {
                            //Invocando Método de Carga
                            cargaImagenesDetalle(mov.id_servicio);

                            //Alternando Ventana Modal
                            gestionaVentanas(this, "Evidencias");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPagarServicio_Click(object sender, EventArgs e)
        {
            //Invocando Método de Pago del Servicio
            pagaServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearPagoMov_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista
                if (liq.id_liquidacion > 0)
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");

                        //Validando que Existan Filas
                        if (gvFilas.Length > 0)
                        {
                            //Actualizando Comando
                            btnGuardarPago.CommandArgument = "Movimiento";
                            btnGuardarPago.CommandName = "CreaPago";

                            //Deshabilitando Control
                            txtDescripcion.Enabled = false;

                            //Alternando Ventanas Modales
                            gestionaVentanas(this, "Pagos");
                            gestionaVentanas(this, "Movimientos");
                        }
                        else
                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPagoMov, "No hay Filas Seleccionadas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPagoMov, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPagoMov, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Crear Otros Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearOtrosPagos_Click(object sender, EventArgs e)
        {
            //Obteniendo Boton
            Button btn = (Button)sender;
            
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista
                if (liq.id_liquidacion > 0)
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Configurando Controles
                        habilitaControlesPagos(true);
                        limpiaControlesPagos();

                        //Deshabilitando Control
                        txtDescripcion.Enabled = true;
                        
                        //Validando Comando
                        switch(btn.CommandName)
                        {
                            case "OtrosPagos":
                                {
                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByText("Descuento");

                                    //Validando si Existe el Tipo
                                    if (li != null)
                                    
                                        //Limpiando Items
                                        ddlTipoPago.Items.Remove(li);
                                        

                                    //Actualizando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "CreaOtrosPagos";
                                    break;
                                }
                            case "BonoSemanal":
                                {
                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByText("Bono Semanal");

                                    //Validando si Existe el Tipo
                                    if (li != null)
                                    {
                                        //Limpiando Items
                                        ddlTipoPago.Items.Clear();
                                        //Añadiendo Item
                                        ddlTipoPago.Items.Add(li);
                                    }

                                    //Actualizando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "CreaOtrosPagos";
                                    
                                    break;
                                }
                            case "PagoNegativo":
                                {
                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByText("Descuento");

                                    //Validando si Existe el Tipo
                                    if (li != null)
                                    {
                                        //Limpiando Items
                                        ddlTipoPago.Items.Clear();
                                        //Añadiendo Item
                                        ddlTipoPago.Items.Add(li);
                                    }

                                    //Actualizando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "PagoNegativo";

                                    break;
                                }
                        }

                        //Alternando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(btn, btn.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(btn, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btn, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Eventos Producido al Presionar el Boton "Guardar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarPago_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Objeto de Contención de Mensajes
            string[] resultados = new string[1];

            //Obteniendo Control
            Button btn = (Button)sender;

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Validando Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Obteniendo Entidad
                        int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                        //Validando Argumento
                        switch (btn.CommandArgument)
                        {
                            case "Servicio":
                                {
                                    //Inicializando Bloque Transaccional
                                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                    {
                                        //Insertando Pago por Servicio
                                        result = Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text, liq.id_unidad,
                                                        liq.id_operador, liq.id_proveedor, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), 0, liq.id_liquidacion,
                                                        Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación haya sido Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Obteniendo Pago
                                            int idPago = result.IdRegistro;
                                            
                                            //Obteniendo Movimientos por Viaje
                                            using (DataSet ds = SAT_CL.Despacho.Reporte.ObtieneMovimientosYPagosPorViaje(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), 
                                                                                                                    liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus, false))
                                            {
                                                /*** Movimientos ***/
                                                //Validando que existen Registros
                                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                                                {
                                                    //Recorriendo Movimientos Ligados al Viaje y al Recurso
                                                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                                                    {
                                                        //Insertando Pago del Movimiento
                                                        result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(idPago, Convert.ToInt32(dr["Id"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que la Operación haya sido Exitosa
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Actualizando Depositos
                                                            result = actualizaLiquidacionDepositosVales(Convert.ToInt32(dr["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                            //Validando que la Operación haya sido Exitosa
                                                            if (!result.OperacionExitosa)

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                        else
                                                            //Terminando Ciclo
                                                            break;
                                                    }

                                                    //Validando que la Operación haya sido Exitosa
                                                    if (result.OperacionExitosa)

                                                        //Completando Transacción
                                                        trans.Complete();
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("No Existen Movimientos");
                                            }
                                        }

                                        //Mostrando Resultado Obtenido
                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                            case "Movimiento":
                                {
                                    //Validando Comando
                                    switch (btn.CommandName)
                                    {
                                        case "EditaPago":
                                            {
                                                //Si el Pago fue Seleccionado
                                                if (gvPagos.SelectedIndex != -1)
                                                {
                                                    //Instanciando Pago
                                                    using (SAT_CL.Liquidacion.Pago pago = new Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                                                    {
                                                        //Validando que Exista el Pago
                                                        if (pago.id_pago > 0)
                                                        {
                                                            //Declarando Variable Auxiliar
                                                            decimal valor_unitario = 0.00M;

                                                            //Obteniendo Valor Unitario
                                                            valor_unitario = pago.objDetallePago.valor_unitario >= 0 ? Convert.ToDecimal(txtValorU.Text) : -1 * Convert.ToDecimal(txtValorU.Text);
                                                            
                                                            //Editando Pago
                                                            result = pago.EditaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), pago.id_tarifa, txtDescripcion.Text, txtReferencia.Text,
                                                                                            liq.id_unidad, liq.id_operador, liq.id_proveedor, pago.objDetallePago.id_servicio,
                                                                                            pago.objDetallePago.id_movimiento, liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                                            valor_unitario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación fuese Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Cargando Servicios y Movimientos Vacios
                                                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                //Invocando Método de Pagos
                                                                cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                                                //Inicializando Indices
                                                                TSDK.ASP.Controles.InicializaIndices(gvPagos);

                                                                //Sumando Totales
                                                                sumaTotalesRecursos();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No Existe el Pago");
                                                    }
                                                }
                                                break;
                                            }
                                        case "EditaPagoMV":
                                            {
                                                //Si el Pago fue Seleccionado
                                                if (gvPagosMV.SelectedIndex != -1)
                                                {
                                                    //Instanciando Pago
                                                    using (SAT_CL.Liquidacion.Pago pago = new Pago(Convert.ToInt32(gvPagosMV.SelectedDataKey["IdPago"])))
                                                    {
                                                        //Validando que Exista el Pago
                                                        if (pago.id_pago > 0)
                                                        {
                                                            //Editando Pago
                                                            result = pago.EditaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), pago.id_tarifa, txtDescripcion.Text, txtReferencia.Text,
                                                                                            liq.id_unidad, liq.id_operador, liq.id_proveedor, pago.objDetallePago.id_servicio,
                                                                                            pago.objDetallePago.id_movimiento, liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                                            Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación fuese Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Cargando Servicios y Movimientos Vacios
                                                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                //Invocando Método de Pagos
                                                                cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                                                //Inicializando Indices
                                                                TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

                                                                //Sumando Totales
                                                                sumaTotalesRecursos();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No Existe el Pago");
                                                    }
                                                }
                                                break;
                                            }
                                        case "EditaPagoLigado":
                                            {
                                                //Si el Pago fue Seleccionado
                                                if (gvPagosLigados.SelectedIndex != -1)
                                                {
                                                    //Instanciando Pago
                                                    using (SAT_CL.Liquidacion.Pago pago = new Pago(Convert.ToInt32(gvPagosLigados.SelectedDataKey["IdPago"])))
                                                    {
                                                        //Validando que Exista el Pago
                                                        if (pago.id_pago > 0)
                                                        {
                                                            //Editando Pago
                                                            result = pago.EditaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), pago.id_tarifa, txtDescripcion.Text, txtReferencia.Text,
                                                                                            liq.id_unidad, liq.id_operador, liq.id_proveedor, pago.objDetallePago.id_servicio,
                                                                                            pago.objDetallePago.id_movimiento, liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                                            Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación fuese Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Cargando Servicios y Movimientos Vacios
                                                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                                //Inicializando Indices
                                                                TSDK.ASP.Controles.InicializaIndices(gvPagosLigados);

                                                                //Sumando Totales
                                                                sumaTotalesRecursos();
                                                            }
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("No Existe el Pago");
                                                    }
                                                }
                                                break;
                                            }
                                        case "CreaPago":
                                            {
                                                //Validando que exista una seleccion
                                                if (gvMovimientos.SelectedIndex != -1)
                                                {
                                                    //Validando que el Movimiento este Validado
                                                    if (validaMovimientoTerminado(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                                                    {
                                                        //Validando que existe un Pago
                                                        if (Convert.ToInt32(gvMovimientos.SelectedDataKey["NoPago"]) != 0)
                                                        {
                                                            //Insertando Pago
                                                            result = new RetornoOperacion("Ya Existe un Pago");
                                                        }
                                                        else
                                                        {
                                                            //Inicializando Bloque Transaccional
                                                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                            {
                                                                //Insertando Pago
                                                                result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]),
                                                                                    Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                                    Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando que la Operación haya sido Exitosa
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Insertando Pago del Movimiento
                                                                    result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(result.IdRegistro, Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]),
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                    //Validando que la Operación haya sido Exitosa
                                                                    if (result.OperacionExitosa)
                                                                    {
                                                                        //Actualizando Depositos
                                                                        result = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                                        //Validando que la Operación haya sido Exitosa
                                                                        if (result.OperacionExitosa)

                                                                            //Completando Transacción
                                                                            trans.Complete();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Validando que la Operación haya sido exitosa
                                                        if (result.OperacionExitosa)

                                                            //Cargando Movimientos y Pagos por Viaje
                                                            cargaMovimientosServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");
                                                }
                                                else
                                                {
                                                    //Inicializando Bloque Transaccional
                                                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                    {
                                                        //Obteniendo Filas Seleccionadas
                                                        GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");

                                                        //Validando que existan
                                                        if (gvFilas.Length > 0)
                                                        {
                                                            //Creando Longitud Dinamica
                                                            resultados = new string[gvFilas.Length];
                                                            //Declarando Contador
                                                            int contador = 0;
                                                            //Insertando Pago
                                                            result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                                liq.id_unidad, liq.id_operador, liq.id_proveedor, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), 0, liq.id_liquidacion,
                                                                                Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación fuese exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Obteniendo Id de Pago
                                                                int idPago = result.IdRegistro;

                                                                //Iniciando Recorrido de Movimientos
                                                                foreach (GridViewRow movimiento in gvFilas)
                                                                {
                                                                    //Declarando Objeto para Actualizar Depositos y Vales de Diesel
                                                                    RetornoOperacion res1 = new RetornoOperacion();

                                                                    //Seleccionando Indice Actual
                                                                    gvMovimientos.SelectedIndex = movimiento.RowIndex;

                                                                    //Validando que el Movimiento este Validado
                                                                    if (validaMovimientoTerminado(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                                                                    {
                                                                        //Insertando Pago del Movimiento
                                                                        result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(idPago, Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]),
                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                        //Validando que la Operación sea Exitosa
                                                                        if (result.OperacionExitosa)
                                                                        {
                                                                            //Actualizando Depositos
                                                                            res1 = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                                            //Validando que se hayan hecho las Actualizaciones
                                                                            if (!res1.OperacionExitosa)
                                                                            {
                                                                                //Asignando Valores
                                                                                resultados = new string[1];
                                                                                resultados[0] = "Error: Mov. " + gvMovimientos.SelectedDataKey["Id"].ToString() + " " + res1.Mensaje;

                                                                                //Terminando Ciclo
                                                                                break;
                                                                            }

                                                                            //Guardando Mensaje de Operación
                                                                            resultados[contador] = "Mov. " + gvMovimientos.SelectedDataKey["Id"].ToString() + " " + result.Mensaje;
                                                                        }

                                                                        //Incrementando Contador
                                                                        contador++;
                                                                    }
                                                                    else
                                                                    {
                                                                        //Instanciando Excepción
                                                                        result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");

                                                                        //Terminando Ciclo
                                                                        break;
                                                                    }
                                                                }

                                                                //Validando que la Operación fuese exitosa
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Obteniendo Registro
                                                                    int idAux = result.IdRegistro;
                                                                    
                                                                    //Completando Transaccion
                                                                    trans.Complete();

                                                                    //Validando si Existen Mensajes
                                                                    if (resultados.Length > 0)
                                                                    {
                                                                        //Instanciando Mensaje(s)
                                                                        result = new RetornoOperacion(0, String.Join("\n", resultados), true);

                                                                        //Mostrando Mensaje(s)
                                                                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                                    }

                                                                    //Instanciando Registro
                                                                    result = new RetornoOperacion(idAux);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Cargando Movimientos y Pagos
                                                    cargaMovimientosServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                                                }

                                                //Mostrando Mensaje
                                                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                                break;
                                            }
                                        case "CreaOtrosPagos":
                                            {
                                                //Insertando Pago
                                                result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, 0, 0, liq.id_liquidacion,
                                                                    Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando Operación Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Invocando Método de Carga
                                                    cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                                    //Inicializando Indices
                                                    TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);
                                                    TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                                                }
                                                break;
                                            }
                                        case "PagoNegativo":
                                            {
                                                //Insertando Pago
                                                result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, 0, 0, liq.id_liquidacion,
                                                                    Convert.ToDecimal(txtCantidad.Text), 0, -1*(Convert.ToDecimal(txtValorU.Text)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando Operación Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Invocando Método de Carga
                                                    cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                                    //Inicializando Indices
                                                    TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);
                                                    TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                                                }
                                                break;
                                            }
                                        case "PagoMovsVacio":
                                            {
                                                //Validando que exista una seleccion
                                                if (gvServiciosLiquidacion.SelectedIndex != -1)
                                                {
                                                    //Validando que el Movimiento este Validado
                                                    if (validaMovimientoTerminado(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                                                    {
                                                        //Validando que existe un Pago
                                                        if (SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                                                        
                                                            //Insertando Pago
                                                            result = new RetornoOperacion("Ya Existe un Pago");
                                                        else
                                                        {
                                                            //Inicializando Bloque Transaccional
                                                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                            {
                                                                //Insertando Pago
                                                                result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, 0, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]), 
                                                                                    liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando que la Operación haya sido Exitosa
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Insertando Pago del Movimiento
                                                                    result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(result.IdRegistro, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]),
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                    //Validando que la Operación haya sido Exitosa
                                                                    if (result.OperacionExitosa)
                                                                    {
                                                                        //Actualizando Depositos
                                                                        result = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                                        //Validando que la Operación haya sido Exitosa
                                                                        if (result.OperacionExitosa)

                                                                            //Completando Transacción
                                                                            trans.Complete();
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Validando que la Operación haya sido exitosa
                                                        if (result.OperacionExitosa)

                                                            //Cargando Movimientos y Pagos por Viaje
                                                            cargaMovimientosServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("El Movimiento " + gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"].ToString() + " no esta Terminado");
                                                }
                                                else
                                                {
                                                    //Inicializando Bloque Transaccional
                                                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                    {
                                                        //Obteniendo Filas Seleccionadas
                                                        GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvServiciosLiquidacion, "chkVariosMovimientos");

                                                        //Validando que existan
                                                        if (gvFilas.Length > 0)
                                                        {
                                                            //Creando Longitud Dinamica
                                                            resultados = new string[gvFilas.Length];
                                                            //Declarando Contador
                                                            int contador = 0;
                                                            //Insertando Pago
                                                            result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                                liq.id_unidad, liq.id_operador, liq.id_proveedor, 0, 0, liq.id_liquidacion,
                                                                                Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación fuese exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Obteniendo Id de Pago
                                                                int idPago = result.IdRegistro;

                                                                //Iniciando Recorrido de Movimientos
                                                                foreach (GridViewRow movimiento in gvFilas)
                                                                {
                                                                    //Declarando Objeto para Actualizar Depositos y Vales de Diesel
                                                                    RetornoOperacion res1 = new RetornoOperacion();

                                                                    //Seleccionando Indice Actual
                                                                    gvServiciosLiquidacion.SelectedIndex = movimiento.RowIndex;

                                                                    //Validando que sea un Movimiento en Vacio
                                                                    if (Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]) == 0)
                                                                    {
                                                                        //Validando que el Movimiento este Validado
                                                                        if (validaMovimientoTerminado(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                                                                        {
                                                                            //Valida si el Movimiento en Vacio tiene un Pago
                                                                            if (!SAT_CL.Liquidacion.PagoMovimiento.ValidaPagoMovimiento(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                                                                            {
                                                                                //Insertando Pago del Movimiento
                                                                                result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(idPago, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]),
                                                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                                //Validando que la Operación sea Exitosa
                                                                                if (result.OperacionExitosa)
                                                                                {
                                                                                    //Actualizando Depositos
                                                                                    res1 = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                                                    //Validando que se hayan hecho las Actualizaciones
                                                                                    if (!res1.OperacionExitosa)
                                                                                    {
                                                                                        //Asignando Valores
                                                                                        resultados = new string[1];
                                                                                        resultados[0] = "Error: Mov. " + gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"].ToString() + " " + res1.Mensaje;

                                                                                        //Terminando Ciclo
                                                                                        break;
                                                                                    }

                                                                                    //Guardando Mensaje de Operación
                                                                                    resultados[contador] = "Mov. " + gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"].ToString() + " " + result.Mensaje;
                                                                                }
                                                                            }
                                                                            else
                                                                                //Guardando Mensaje de Operación
                                                                                resultados[contador] = "Mov. " + gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"].ToString() + " Ya tiene un Pago asignado";


                                                                            //Incrementando Contador
                                                                            contador++;
                                                                        }
                                                                        else
                                                                        {
                                                                            //Instanciando Excepción
                                                                            result = new RetornoOperacion("El Movimiento " + gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"].ToString() + " no esta Terminado");

                                                                            //Terminando Ciclo
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                //Validando que la Operación fuese exitosa
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Inicializando Indices
                                                                    Controles.InicializaIndices(gvServiciosLiquidacion);

                                                                    //Mostrando Mensaje(s)
                                                                    ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                                                    //Completando Transaccion
                                                                    trans.Complete();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //Cargando Servicios y Movimientos en Vacio
                                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                break;
                                            }
                                    }
                                    break;
                                }
                        }

                        //Validando que la Operación fuese Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Carga de Totales de la Liquidación
                            cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                            //Validando que este Seleccionado un Viaje
                            if (gvServiciosLiquidacion.SelectedIndex != -1)
                            {
                                //Obteniendo Valores
                                int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                //Actualizando Servicios y Movimientos en Vacio
                                cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                //Validando que sea un Servicio
                                if (idServicio != 0)
                                    //Marcando Fila
                                    Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                                //Si es Movimiento
                                else
                                    //Marcando Fila
                                    Controles.MarcaFila(gvServiciosLiquidacion, idMovimiento.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                                //Sumando Totales
                                sumaTotalesRecursos();
                            }

                            //Ocultando ventana
                            TSDK.ASP.ScriptServer.AlternarVentana(upbtnGuardarPago, upbtnGuardarPago.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
                        }

                        //Mostrando Resultado de la Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarPago_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);
            TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

            //Limpiando Controles
            limpiaControlesPagos();
            habilitaControlesPagos(false);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarPago, upbtnCancelarPago.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
        }

        #endregion

        #region Eventos "Cobros Recurrentes Historial"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCRH_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table11"), Convert.ToInt32(ddlTamanoCRH.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentesHistorial_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCRH.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table11"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentesHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table11"), e.NewPageIndex);
        }

        #endregion

        #region Eventos "Resumen Liquidación"
        /// <summary>
        /// Cerrar Resumen de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbnCerrarResumenParadas_Click(object sender, EventArgs e)
        {
            //Cierra la ventana modal de Historial
            gestionaVentanas(lkbCerrarResumenParadas, "ResumenParadas");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarResumenLiq_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), "Id", "IdTipoRegistro");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Imprimir Liquidación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimirLiquidacion_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/Liquidacion.aspx", "~/RDLC/Reporte.aspx");

                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Liquidacion", liq.id_liquidacion), "Liquidación", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                }
                else
                    //Mostrando Excepción
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No Existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al enlazar Datos el GridView "Resumen Liquidación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumenLiquidacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Tipo de Registro
                string tipo_registro = ((DataRowView)e.Row.DataItem).Row.ItemArray[1].ToString();

                //Validando el Tipo de Registro
                switch (tipo_registro)
                {
                    //Servicio
                    case "1":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_servicio";
                            break;
                        }
                    //Movimiento
                    case "2":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_movimiento";
                            break;
                        }
                    //Detalle Liquidación
                    case "3":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            break;
                        }
                    //Detalle Liquidación con Valor Negativo
                    case "4":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;

                            break;
                        }
                    //Totales Por Servicio
                    case "5":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[6].CssClass =
                            e.Row.Cells[7].CssClass =
                            e.Row.Cells[8].CssClass =
                            e.Row.Cells[9].CssClass =
                            e.Row.Cells[10].CssClass = "liquidacion_totales_liquidacion";

                            //Validando que existan Depositos
                            if (e.Row.Cells[6].Text.Equals("Total Deposito"))
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                            else
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;

                            break;
                        }
                    //Cobros Recurrentes
                    case "6":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_cobro_recurrente";
                            break;
                        }
                    //Totales de la Liquidación
                    case "7":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[6].CssClass =
                            e.Row.Cells[7].CssClass =
                            e.Row.Cells[8].CssClass =
                            e.Row.Cells[9].CssClass =
                            e.Row.Cells[10].CssClass = "liquidacion_totales_liquidacion";

                            //Validando que existan Depositos/Deducciones/Descuentos
                            if (e.Row.Cells[6].Text.Equals("Total Deposito") || e.Row.Cells[6].Text.Equals("Total Deducci&#243;n") || e.Row.Cells[6].Text.Equals("Total Descuento"))
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                            else
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                            break;
                        }
                }
            }
        }

        #endregion

        #region Eventos Recibo de Nómina

        /// <summary>
        /// Evento generado al dar click en Recibo de Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReciboNomina_Click(object sender, EventArgs e)
        {
            //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Liquidación
                if (objLiquidacion.id_liquidacion > 0)
                {
                    //Creación del objeto botonMenú
                    LinkButton botonMenu = (LinkButton)sender;
                    
                    //Evalúa cada opcion del menú Principal.
                    switch (botonMenu.CommandName)
                    {
                        case "Timbrar":
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(lkbTimbrar, lkbTimbrar.GetType(), "AbrirVentana", "contenidoConfirmacionTimbrarLiquidacion", "confirmacionTimbrarLiquidacion");
                            break;
                        case "Cancelar":
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(lkbCancelarTimbrado, lkbCancelarTimbrado.GetType(), "AbrirrVentana", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al Cancelar el Recibo de Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelarTimbrado_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Liquidación
                if (objLiquidacion.id_liquidacion > 0)
                {
                    //Validando Estatus de la Liquidación
                    if (objLiquidacion.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado)
                    {
                        //Obtenemos Comprobante Vigente
                        using (SAT_CL.FacturacionElectronica.Comprobante objComprobante = new SAT_CL.FacturacionElectronica.Comprobante(SAT_CL.FacturacionElectronica.Comprobante.ObtieneReciboNominaVigente(objLiquidacion.id_liquidacion)))
                        {
                            //Validamos Comprobante
                            if (objComprobante.id_comprobante > 0)
                            {
                                //Timbramos Liquidación
                                resultado = objComprobante.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Cerrar Ventana Modal
                                    ScriptServer.AlternarVentana(btnAceptarCancelarTimbrado, btnAceptarCancelarTimbrado.GetType(), "CerrarVentana", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");

                                    //Limpiamos Etiqueta
                                    lblErrorCancelarTimbrado.Text = "";
                                }
                                else
                                    //Mostramos Error
                                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Mostramos Error
                                ScriptServer.MuestraNotificacion(this, "No existe Recibo de Nómina Vigente.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                    else
                        //Mostramos Error
                        ScriptServer.MuestraNotificacion(this, "La Liquidación no esta Cerrada", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento generado al Timbrar una Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrarLiquidacion_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Liquidación
                if (objLiquidacion.id_liquidacion > 0)
                {
                    //Timbramos Liquidación
               //   resultado = objLiquidacion.ImportaTimbraLiquidacionComprobante_V3_2(   ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,"N", HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2.xslt"),
                 //                                                   HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2_desconectado.xslt"), Catalogo.RegresaDescripcionCatalogo(1111, Convert.ToInt32(ddlPeriocidadPago.SelectedValue)), Convert.ToInt32(ddlMetodoPago.SelectedValue),
                   //                                                 Convert.ToInt32(ddlSucursal.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    
                    //Mostramos Error
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        
        #endregion

        #region Eventos Referencias Viaje

        /// <summary>
        /// Evento Producido al Guardar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                cargaDatosEncabezadoLiq();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// /// Evento Producido al Eliminar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                cargaDatosEncabezadoLiq();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos Devoluciones

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoDev.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table15"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table15"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table15"), Convert.ToInt32(ddlTamanoDev.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNoDevolucion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDevoluciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDevoluciones, sender, "lnk", false);

                //Instanciando Devolución
                using(SAT_CL.Despacho.DevolucionFaltante df = new DevolucionFaltante(Convert.ToInt32(gvDevoluciones.SelectedDataKey["IdDevolucion"])))
                {
                    //Validando que Exista la Devolución
                    if (df.habilitar)
                    {
                        //Inicializando Control
                        wucDevolucionFaltante.InicializaDevolucion(df.id_devolucion_faltante);

                        //Alternando Ventanas
                        gestionaVentanas(this, "AltaDevolucion");
                        gestionaVentanas(this, "Devolucion");
                    }
                }
            }
        }

        #endregion

        #region Eventos Alta Devolución

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDevolucion();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            
                //Invocando Método de Carga
                cargaResumenDevoluciones(wucDevolucionFaltante.objDevolucionFaltante.id_servicio);

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDetalleDevolucion();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaResumenDevoluciones(wucDevolucionFaltante.objDevolucionFaltante.id_servicio);

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickEliminarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.EliminaDetalleDevolucion();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaResumenDevoluciones(wucDevolucionFaltante.objDevolucionFaltante.id_servicio);

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDevolucion(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciasViaje.InicializaControl(wucDevolucionFaltante.objDevolucionFaltante.id_devolucion_faltante, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 156);

            //Alternando Ventanas Modales
            gestionaVentanas(this, "ReferenciasViaje");
            gestionaVentanas(this, "AltaDevolucion");

            //Actualizando Comando del Boton Cerrar
            lnkCerrarReferencias.CommandArgument = "Devolucion";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDetalle(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciasViaje.InicializaControl(wucDevolucionFaltante.idDetalle, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 157);

            //Alternando Ventanas Modales
            gestionaVentanas(this, "ReferenciasViaje");
            gestionaVentanas(this, "AltaDevolucion");

            //Actualizando Comando del Boton Cerrar
            lnkCerrarReferencias.CommandArgument = "Devolucion";
        }


        #endregion

        #region Eventos GridView "Pagos Mov. Vacio"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Pagos Mov. Vacio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPagoMV_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoPago.SelectedValue));

            //Invocando Método de Suma de Pagos
            sumaTotalPagosMV();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página de "Pagos Mov. Vacio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosMV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);

            //Invocando Método de Suma de Pagos
            sumaTotalPagosMV();
        }
        /// <summary>
        /// Eventos Producido al Cambiar el Ordenamiento de "Pagos Mov. Vacio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosMV_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoPago.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);

            //Invocando Método de Suma de Pagos
            sumaTotalPagosMV();
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Editar Pago Mov. Vacio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarPagoMV_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagosMV.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagosMV, sender, "lnk", false);

                //Instanciando Pago
                using (SAT_CL.Liquidacion.Pago pag = new SAT_CL.Liquidacion.Pago(Convert.ToInt32(gvPagosMV.SelectedDataKey["IdPago"])))
                {
                    //Validando que existe el Pago
                    if (pag.id_pago != 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pag.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByValue(pag.id_tipo_pago.ToString());

                                    //Validando que exista el Tipo
                                    if (li != null)
                                    {
                                        //Limpiando Items
                                        ddlTipoPago.Items.Clear();
                                        //Añadiendo Item
                                        ddlTipoPago.Items.Add(li);
                                    }

                                    //Asignando Valores 
                                    ddlTipoPago.SelectedValue = pag.id_tipo_pago.ToString();
                                    txtCantidad.Text = pag.objDetallePago.cantidad.ToString();
                                    txtValorU.Text = pag.objDetallePago.valor_unitario.ToString();
                                    txtTotal.Text = pag.objDetallePago.monto.ToString();
                                    txtDescripcion.Text = pag.descripcion;
                                    txtReferencia.Text = pag.referencia;

                                    //Habilitando Controles
                                    habilitaControlesPagos(true);
                                    //Asignando Comando
                                    btnGuardarPago.CommandArgument = "Movimiento";
                                    btnGuardarPago.CommandName = "EditaPagoMV";

                                    //Mostrando ventana de captura
                                    gestionaVentanas(upgvPagosMV, "Pagos");
                                    gestionaVentanas(upgvPagosMV, "PagosMV");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                    else
                        //Asignando Valores 
                        limpiaControlesPagos();

                    //Quitando la selección de las Filas
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
                }
            }
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Eliminar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarPagoMV_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagosMV.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvPagosMV, sender, "lnk", false);

                //Instanciando Pago
                using (Pago pago = new Pago(Convert.ToInt32(gvPagosMV.SelectedDataKey["IdPago"])))
                {
                    //Validando que Exista
                    if (pago.id_pago > 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pago.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Asignando Comando
                                    btnAceptarOperacion.CommandName = "PagoMV";

                                    //Alternando Ventana Modal
                                    gestionaVentanas(upgvPagosMV, "ConfirmacionOperacion");
                                    gestionaVentanas(upgvPagosMV, "PagosMV");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagosMV);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Historial Vencimientos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucVencimientosHistorial_btnNuevoVencimiento(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Evento Producido al Consultar el Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucVencimientosHistorial_lkbConsultar(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Evento Producido al Terminar el Vencimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucVencimientosHistorial_lkbTerminar(object sender, EventArgs e)
        {
            
        }

        #endregion

        #region Eventos Cambio de Cuenta
        /// <summary>
        /// Evento generado al Cambiar la Cuenta de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCambiarCuentaPago_Click(object sender, EventArgs e)
        {
            //Limpiamos Etiqueta
            txtNuevaCuentaPago.Text = "";
            //Obteniendo Referencias
            using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(Convert.ToInt32(Session["id_registro"]), 82, 2211))
            {
                //Valdiando que Existan
                if (Validacion.ValidaOrigenDatos(dtRef))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in dtRef.Rows)
                    {
                        //Validando que Exista el Registro
                        if (Convert.ToInt32(dr["Id"]) > 0)
                        {
                            //Instanciando Cuenta
                            using (SAT_CL.Global.Referencia cuenta = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                            {
                                //Validando que exista la Referencia
                                if (cuenta.habilitar)
                                {
                                    //Limpiamos Control de Cuenta
                                    txtNuevaCuentaPago.Text = cuenta.valor;

                                    //Terminamos el Ciclo
                                    break;
                                }

                            }
                        }
                    }
                }
                else
                {   
                    //Limpiamos Control de Cuenta
                    txtNuevaCuentaPago.Text = "";
                }

            }

            //Abrimos Ventana Modal
            gestionaVentanas(btnCambiarCuentaPago, "CambioCuentaPago");

        }

        /// <summary>
        /// Evento generado al Aceptar la Cuenta de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCuentaPago_Click(object sender, EventArgs e)
        {
            //Actualizamos Comentario   
            RetornoOperacion resultado = cambioCuenta();

            //Abrimos Ventana Modal
            gestionaVentanas(btnAceptarCuentaPago, "CambioCuentaPago");

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarCuentaPago, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);


        }
        #endregion

        #region Eventos Facturación Web Service

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
                    gestionaVentanas((Button)sender, "FacturacionWS");

                    //Validando Argumento
                    switch (((Button)sender).CommandArgument)
                    {
                        case "Comprobacion":
                            //Mostrando ventana modal
                            gestionaVentanas((Button)sender, "AltaComprobaciones");
                            break;
                    }
                    break;
                case "Continuar":
                    //Ocultando ventana modal
                    gestionaVentanas((Button)sender, "FacturacionWS");    
                
                    //Validando Argumento
                    switch (((Button)sender).CommandArgument)
                    {
                        case "Comprobacion":
                            {
                                //Realizando proceso de guardado de factura de proveedor
                                //Guardando Factura
                                guardaFacturaXML();

                                //Instanciando Liquidación
                                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Actualizando Carga de Totales de la Liquidación
                                    cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                                    //Determinando que entidad se afecta con esta liquidación
                                    int id_entidad = 0;
                                    switch (liq.tipo_asignacion)
                                    {
                                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad:
                                            id_entidad = liq.id_unidad;
                                            break;
                                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador:
                                            id_entidad = liq.id_operador;
                                            break;
                                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor:
                                            id_entidad = liq.id_proveedor;
                                            break;
                                    }

                                    //Obteniendo Valores
                                    int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                                    int idMovimeinto = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                                    //Cargando recursos
                                    cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                    //Validando que exista el Servicio
                                    if (idServicio != 0)
                                        //Marcando Fila
                                        Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                                    else
                                        //Marcando Fila
                                        Controles.MarcaFila(gvServiciosLiquidacion, idMovimeinto.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                                    //Sumando Totales
                                    sumaTotalesRecursos();
                                }

                                //Mostrando ventana modal
                                gestionaVentanas((Button)sender, "AltaComprobaciones");
                                break;
                            }

                    }
                    break;
            }
        }

        #endregion

        #region Eventos Lectura
        /// <summary>
        /// Evento que Permite Visualizar el Control de Usuario Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkLectura_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridView de Liquidación
            if (gvServiciosLiquidacion.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvServiciosLiquidacion, sender, "lnk", false);
                //Abre la ventana modal de Lectura
                gestionaVentanas(this, "LecturaHistorial");
                //Inicializa el control de Lectura
                wucLecturaHistorial.InicializaControl(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdUnidad"]), true, Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdOperador"]));
            }
        }
        /// <summary>
        /// Método encargado de Crear una Nueva Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLecturaHistorial_btnNuevaLectura(object sender, EventArgs e)
        {
            gestionaVentanas(this, "CierraLecturaHistorial");
            //Abre la ventana modal de Lectura
            gestionaVentanas(this, "Lectura");
            //Inicializamos Control para Registro de Lecturas
            wucLectura.InicializaControl(0, wucLecturaHistorial.id_unidad, wucLecturaHistorial.id_operador);

        }
        /// <summary>
        /// Método encargado de Eliminar una Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickEliminarLectura(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Guardamos Lectura
            resultado = wucLectura.DeshabilitarLectura();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            gestionaVentanas(this, "CierraLectura");
        }
        /// <summary>
        /// Método encaragdo de Guardar la lectura
        /// </summary>
        protected void wucLectura_ClickGuardarLectura(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Guardamos Lectura
            resultado = wucLectura.GuardarLectura();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            gestionaVentanas(this, "CierraLectura");
        }
        /// <summary>
        /// Método encargado de Consultar una Lectura
        /// </summary>
        protected void wucLecturaHistorial_lkbConsultar(object sender, EventArgs e)
        {
            //Método encargado de Abrir la ventana de Lectura
            gestionaVentanas(this, "Lectura");
            //Método encargado de Cerrar la Ventana de Historial de Lectura
            gestionaVentanas(this, "CierraLecturaHistorial");
            //Inicializamos Control de Usuario Lectura
            wucLectura.InicializaControl(wucLecturaHistorial.id_lectura, wucLecturaHistorial.id_unidad);
        }

        #endregion

        #region Eventos Control Diesel
        /// <summary>
        /// Evento que abre la ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkControlDiesel_Click(object sender, EventArgs e)
        {
            //Abre la ventana modal
            gestionaVentanas(this, "ControlDiesel");
            //Obtiene los datos de la liquidación
            using (SAT_CL.Liquidacion.Liquidacion li = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Inicializa los valores del control de usuario
                wucControlDiesel.InicializaControl(li.id_liquidacion, li.id_operador, li.fecha_liquidacion);
            }

        }
        #endregion

        #region Eventos Calificacion
        /// <summary>
        /// Evento que cierra ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarCalificacion_Click(object sender, EventArgs e)
        {
            //Obtiene el control linkButom para definir que accion se realizara
            LinkButton lnk = (LinkButton)sender;
            //Valida el linkbutton y determina la acción a ejecutar
            switch (lnk.CommandName)
            {
                //Cierra la ventana modal de Calificación
                case "CierraCalificacion":
                    {

                        //Invoca al método que carga los indicadores de calificación y cierra la ventan modal del Control de usuario CAlificación
                        cargaCalificacion();
                        gestionaVentanas(lnk, "CerrarVentanaCalificacion");
                        break;
                    }
                //Cierra la ventana modal de Historial Calificación
                case "CierraHistorialCalificacion":
                    //Cierra la ventana modal de Historial
                    gestionaVentanas(lnk, "CerrarVentanaHistorial");
                    break;
                case "CierraLectura":
                    {
                        gestionaVentanas(lnk, "CierraLectura");
                        break;
                    }
                case "CierraControlDiesel":
                    {
                        gestionaVentanas(lnk, "CierraControlDiesel");
                        break;
                    }
                case "CierraLecturaHistorial":
                    {
                        gestionaVentanas(lnk, "CierraLecturaHistorial");
                        break;
                    }
            }

        }
        /// <summary>
        /// Evento que alamcena la calificación general calificada
        /// </summary>
        protected void wucCalificacion_ClickGuardarCalificacionGeneral(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asigna al objeto retorno el resultado del método del control de usuario que guardar calificación general clificada
            retorno = wucCalificacion.GuardarCalificacionGeneral();
            //Valida la operación de almacenamiento de la operación
            if (retorno.OperacionExitosa)
            {
                cargaCalificacion();
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "CerrarVentana", "contenedorVentanaCalificacion", "ventanaCalificacion");
            }
        }
        /// <summary>
        /// Evento que inicializan los valores del control de usuario Calificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbCalificacion_Click(object sender, ImageClickEventArgs e)
        {
            //Instanciando Encabezado de Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Si la entidad a calificar es un operador
                if (liq.id_tipo_asignacion == (byte)SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador)
                    //Inicializa el control de usuario CAlificacion                
                    wucCalificacion.InicializaControl(76, liq.id_operador, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 0, true);
            }
            //Invoca al método que abre la ventana modal
            gestionaVentanas(this, "Calificacion");
        }
        /// <summary>
        /// Evento que inicializa el control de usuario Historial Calificación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentarios_Click(object sender, EventArgs e)
        {
            //Abre la ventana modal
            gestionaVentanas(this, "HistorialCalificacion");
            //Instanciando Encabezado de Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Valida si el historial pertenece a un operador
                if (liq.id_tipo_asignacion == (byte)SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador)
                    //Inicializa el control de usuario CAlificacion                
                    wucHistorialCalificacion.InicializaControl(76, liq.id_operador);
            }
        }
        #endregion
        
        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Invocando Método de Carga
            cargaCatalogos();

            //Limpiando Session
            Session["DS"] = null;
            Session["id_registro"] = 0;

            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
            TSDK.ASP.Controles.InicializaGridview(gvServiciosLiquidacion);
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvPagos);
            TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
            TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
            TSDK.ASP.Controles.InicializaGridview(gvFacturasComprobacion);
            TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentes);
            TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
            //TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);
            //TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);

            //Invocando Método de Configuración
            configuraBusquedaUnidad();

            //Limpiando Controles
            limpiaDatosEncabezadoLiquidacion();

            //Configurando Primer Vista
            mtvEncabezado.ActiveViewIndex = 0;

            //Validando Perfil
            using (SAT_CL.Seguridad.PerfilSeguridadUsuario perfil_activo = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilActivo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
            using (SAT_CL.Seguridad.PerfilSeguridad perfil = new SAT_CL.Seguridad.PerfilSeguridad(perfil_activo.id_perfil))
            {
                if (perfil_activo.habilitar && perfil.habilitar)
                {
                    if (perfil.descripcion.Equals("Administrador TECTOS"))
                        pnlAbrirLiquidacion.Visible = true;
                    else
                        pnlAbrirLiquidacion.Visible = false;
                }
                else
                    pnlAbrirLiquidacion.Visible = false;
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoRecurso, "", 62);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

            //Obteniendo Tipos de Pago
            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                //Validando que existan los Conceptos de Pago
                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                    //Cargando DropDownList
                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                else
                    //Inicializando DropDownList
                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
            }

            //Cargando Catalogos de Tamaño de los GridViews
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPago, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPagoMV, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCRL, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRecursos, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoAnticipos, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCR, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGridViewTarifasAplicables, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoMov, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEvidencia, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPagosLigados, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCRH, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDiesel, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDev, "", 26);
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacLiq, "", 26);
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacLigadas, "", 26);

            //Recibo de  Nómina
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriocidadPago, "", 1111);
            //Sucursales
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Métodos de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 80);
        }
        /// <summary>
        /// Método encargado de Configurar los Controles de Busqueda
        /// </summary>
        private void configuraBusquedaUnidad()
        {
            //Validando el Tipo de Recurso
            switch (ddlTipoRecurso.SelectedValue)
            {
                case "1":
                    {
                        //Configurando Controles
                        txtRecursoUn.Visible = true;
                        txtRecursoOp.Visible =
                        txtRecursoPr.Visible = false;
                        break;
                    }
                case "2":
                    {
                        //Configurando Controles
                        txtRecursoUn.Visible = false;
                        txtRecursoOp.Visible = true;
                        txtRecursoPr.Visible = false;
                        break;
                    }
                case "3":
                    {
                        //Configurando Controles
                        txtRecursoUn.Visible =
                        txtRecursoOp.Visible = false;
                        txtRecursoPr.Visible = true;
                        break;
                    }
            }

            //Limpiando Controles
            txtRecursoUn.Text =
            txtRecursoOp.Text =
            txtRecursoPr.Text = "";
        }
        /*// <summary>
        /// Método Privado encargado de Cargar el Catalogo Autocompleta
        /// </summary>
        private void cargaAutocompletaEntidad()
        {
            //Obteniendo Compania
            string id_compania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script
            string script = @"<script type='text/javascript'>
                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + ddlTipoRecurso.SelectedValue + @";
                            
                                //Evento Change
                                $('#" + ddlTipoRecurso.ClientID + @"').change(function () {
                                    
                                    //Limpiando Control
                                    $('#ctl00_content1_txtEntidad').val('');

                                    //Invocando Funcion
                                    CargaAutocompleta();
                                });
                                
                                //Declarando Función de Autocompleta
                                function CargaAutocompleta(){
                                    //Validando Tipo de Entidad
                                    switch (tipoEntidad) {
                                        case 1:
                                            {   
                                                //Cargando Catalogo de Unidades
                                                $('#" + txtRecurso.ClientID + @"').autocomplete('../WebHandlers/AutoCompleta.ashx?id=12&param=" + id_compania + @"');
                                                break;
                                            }
                                        case 2:
                                            {   
                                                //Cargando Catalogo de Operadores
                                                $('#" + txtRecurso.ClientID + @"').autocomplete('../WebHandlers/AutoCompleta.ashx?id=11&param=" + id_compania + @"');
                                                break;
                                            }
                                        case 3:
                                            {   
                                                //Cargando Catalogo de Proveedores
                                                $('#" + txtRecurso.ClientID + @"').autocomplete('../WebHandlers/AutoCompleta.ashx?id=16&param=" + id_compania + @"');
                                                break;
                                            }
                                    }
                                }
                                
                                //Invocando Funcion
                                CargaAutocompleta();
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaEntidad", script, false);
        }//*/
        /// <summary>
        /// Método encargado de Cargar los Datos del Encabezado de Liquidación
        /// </summary>
        private void cargaDatosEncabezadoLiq()
        {
            //Instanciando Encabezado de Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existe la Liquidacion
                if (liq.id_liquidacion != 0)
                {
                    //Cambiando Indice del MultiView
                    mtvEncabezado.ActiveViewIndex = 1;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana_activo";

                    //Asignando Valores
                    lblNoLiquidacion.Text = liq.no_liquidacion.ToString();
                    txtFechaLiq.Text = liq.fecha_liquidacion.ToString("dd/MM/yyyy HH:mm");
                    lblEstatus.Text = liq.estatus.ToString();

                    //Validando que el Tipo de Asignación del Recurso sea Unidad
                    switch (liq.tipo_asignacion)
                    {
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad:
                            //Instanciando Unidad
                            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(liq.id_unidad))
                                //Asignando Descripción
                                lblEntidad.Text = uni.numero_unidad;
                            lblTipoEntidad.Text = "Unidad";
                            imgbCalificacion.Enabled = 
                            lkbComentarios.Enabled =false;
                            imgbCalificacion.Visible =
                            lkbComentarios.Visible = false;
                            break;

                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador:
                            {
                                imgbCalificacion.Enabled =
                                lkbComentarios.Enabled = true;
                                imgbCalificacion.Visible =
                                lkbComentarios.Visible = true;
                                //TO DO: Clase de Operador
                                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(liq.id_operador))
                                {
                                    //Asignando Descripción
                                    lblEntidad.Text = op.nombre;
                                    lblTipoEntidad.Text = "Operador";
                                    cargaCalificacion();
                                }
                            }
                            break;

                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor:
                            //Instanciando Unidad
                            using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(liq.id_proveedor))
                                //Asignando Descripción
                                lblEntidad.Text = pro.nombre;
                            lblTipoEntidad.Text = "Proveedor";
                            imgbCalificacion.Enabled = 
                            lkbComentarios.Enabled =false;
                            imgbCalificacion.Visible =
                            lkbComentarios.Visible = false;
                            break;
                    }

                    //Validando Estatus
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)

                        //Calculando Valores
                        cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                    else
                    {
                        //Mostrando Valores por Defecto
                        lblPercepciones.Text = string.Format("{0:C2}", liq.total_salario);
                        lblSueldo.Text = string.Format("{0:C2}", liq.total_sueldo);
                        lblAnticipos.Text = string.Format("{0:C2}", liq.total_anticipos);
                        lblDescuentos.Text = string.Format("{0:C2}", liq.total_descuentos);
                        lblDeducciones.Text = string.Format("{0:C2}", liq.total_deducciones);
                        lblComprobaciones.Text = string.Format("{0:C2}", liq.total_comprobaciones);
                        lblAlcance.Text = string.Format("{0:C2}", liq.total_alcance);
                    }

                    //Declarando Variable Auxiliar
                    int id_entidad = 0;

                    //Asignando Entidad Encontrada
                    if (liq.id_operador != 0)
                        id_entidad = liq.id_operador;
                    else if (liq.id_unidad != 0)
                        id_entidad = liq.id_unidad;
                    else if (liq.id_proveedor != 0)
                        id_entidad = liq.id_proveedor;

                    //Cargando Pagos Generales
                    cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                    //Cargando Servicios y Movimientos en Vacio
                    cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                    //Sumando Totales
                    sumaTotalesRecursos();

                    //Habilitando Controles
                    btnCerrarLiquidacion.Enabled = btnCambiarCuentaPago.Enabled = liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado ? true : false;

                    //Cargando Cobros Recurrentes
                    cargaCobrosRecurrentes(liq.id_tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora, liq.fecha_liquidacion);

                    //Cargando Cobros Recurrentes Totales
                    cargaCobrosRecurrentesTotales(liq.id_liquidacion, liq.id_tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                }
                else
                    //Limpiando Datos de Encabezado
                    limpiaDatosEncabezadoLiquidacion();

                //Inicializando Movimientos y Pagos
                TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
                TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            }
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles del Encabezado de Liquidación
        /// </summary>
        private void limpiaDatosEncabezadoLiquidacion()
        {
            //Limpiando Controles
            lblNoLiquidacion.Text = 
            lblTipoEntidad.Text = "Por Asignar";
            lblEstatus.Text = "-";
            lblEntidad.Text = "Ninguna";

            //Inicializando Valores
            lblPercepciones.Text =
            lblSueldo.Text =
            lblAnticipos.Text =
            lblDeducciones.Text =
            lblDescuentos.Text =
            lblComprobaciones.Text =
            lblAlcance.Text = "0.00";

            //Inicializando GridView de Viajes
            TSDK.ASP.Controles.InicializaGridview(gvServiciosLiquidacion);
            
            //Añadiendo Tabla a Session
            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }
        /// <summary>
        /// Método Privado encargado de Cerrar la Liquidación
        /// </summary>
        private void cierraLiquidacion()
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Validando que no existan Depositos Pendientes
                    if (!SAT_CL.EgresoServicio.DetalleLiquidacion.ValidaDepositosPendientesLiquidacion(liq.id_liquidacion, liq.id_estatus, liq.id_tipo_asignacion, id_entidad))
                    {
                        //Obteniendo Montos Totales
                        using (DataTable dtTotales = SAT_CL.Liquidacion.Liquidacion.ObtieneMontosTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora))
                        {
                            //Declarando Variable Auxiliares
                            decimal t_salario = 0.00M, t_deducciones = 0.00M, t_sueldo = 0.00M, t_anticipos = 0.00M,
                                    t_comprobaciones = 0.00M, t_descuentos = 0.00M, t_alcanze = 0.00M;

                            //Validando que existan Valores
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtTotales.Rows)
                                {
                                    //Asignando Valores
                                    t_salario = Convert.ToDecimal(dr["TPercepcion"]);
                                    t_deducciones = Convert.ToDecimal(dr["TDeducciones"]);
                                    t_sueldo = Convert.ToDecimal(dr["TSueldo"]);
                                    t_anticipos = Convert.ToDecimal(dr["TAnticipos"]);
                                    t_comprobaciones = Convert.ToDecimal(dr["TComprobaciones"]);
                                    t_descuentos = Convert.ToDecimal(dr["TDescuentos"]);
                                    t_alcanze = Convert.ToDecimal(dr["TAlcance"]);
                                }
                            }

                            //Cerrando la Liquidación
                            result = liq.CierraLiquidacion(t_salario, t_deducciones, t_sueldo, t_anticipos, t_comprobaciones, t_descuentos, t_alcanze, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Liquidación se haya Cerrado
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Valores de la Liquidación
                                cargaDatosEncabezadoLiq();

                                /*/Validando que la Liquidación sea de Proveedor
                                if (liq.id_proveedor > 0)
                                {
                                    //Cargando Facturas Disponibles
                                    buscarFacturasLiquidacion();

                                    //Mostrando Ventana de Facturación
                                    gestionaVentanas(btnCerrarLiquidacion, "FacturacionLiq");
                                }//*/
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("Existen depositos pendientes, Imposible cerrar la Liquidación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Liquidación");

                //Mostrando Mensaje de la Operación
                ScriptServer.MuestraNotificacion(btnCerrarLiquidacion, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Pagos de la Liquidación que no Pertenecen a un Servicio y/o Movimiento
        /// </summary>
        /// <param name="id_liquidacion">Liquidacion Actual</param>
        private void cargaPagosGeneralLiquidacion(int id_liquidacion)
        {
            //Obteniendo Pagos
            using (DataTable dtPagosGenerales = SAT_CL.Liquidacion.Pago.ObtienePagosLiquidacion(id_liquidacion))
            {
                /*** Pagos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagosGenerales))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvPagos, dtPagosGenerales, "IdPago", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPagosGenerales.Copy(), "Table2");
                    //Mostrando Totales
                    gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPagos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    //Mostrando Sumatoria de Totales
                    gvPagos.FooterRow.Cells[5].Text = "0.00";
                }
            }

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvServiciosLiquidacion);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

            //Inicializando Grids
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
            TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
            TSDK.ASP.Controles.InicializaGridview(gvDiesel);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Valores Totales de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Asignación de la Liquidación</param>
        /// <param name="id_unidad">Unidad de la Liquidación</param>
        /// <param name="id_operador">Operador de la Liquidación</param>
        /// <param name="id_proveedor">Proveedor de la Liquidación</param>
        /// <param name="id_compania_emisora">Compania Emisora de la Liquidación</param>
        private void cargaValoresTotalesLiquidacion(int id_liquidacion, SAT_CL.Liquidacion.Liquidacion.TipoAsignacion tipo_asignacion, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisora)
        {
            //Obteniendo Totales
            using (DataTable dtTotalesLiq = SAT_CL.Liquidacion.Liquidacion.ObtieneMontosTotalesLiquidacion(id_liquidacion, tipo_asignacion, id_unidad, id_operador, id_proveedor, id_compania_emisora))
            {
                //Validando que existan los Valores
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesLiq))
                {
                    //Recorriendo Tabla
                    foreach (DataRow dr in dtTotalesLiq.Rows)
                    {
                        //Mostrando Valores Obtenidos
                        lblPercepciones.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TPercepcion"]));
                        lblSueldo.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TSueldo"]));
                        lblAnticipos.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TAnticipos"]));
                        lblDeducciones.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TDeducciones"]));
                        lblComprobaciones.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TComprobaciones"]));
                        lblAlcance.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TAlcance"]));
                        lblDescuentos.Text = string.Format("{0:C2}", Convert.ToDecimal(dr["TDescuentos"]));
                    }
                }
                else
                {
                    //Mostrando Valores por Defecto
                    lblPercepciones.Text =
                    lblSueldo.Text =
                    lblAnticipos.Text =
                    lblDeducciones.Text =
                    lblDescuentos.Text =
                    lblComprobaciones.Text =
                    lblAlcance.Text = string.Format("{0:C2}", 0);
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de habilitar los Controles del Encabezado del Liquidación
        /// </summary>
        /// <param name="estatus">Estatus de Controles</param>
        private void habilitaControlesEncabezado(Pagina.Estatus estatus)
        {
            //Validando Estatus
            switch (estatus)
            {
                //Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Habilitación
                        txtFechaLiq.Enabled =
                        txtDescripcion.Enabled =
                        txtReferencia.Enabled =
                        btnGuardarLiq.Enabled =
                        btnCancelarLiq.Enabled = false;
                        btnEditar.Enabled = true;
                        break;
                    }
                //Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Habilitación
                        txtFechaLiq.Enabled =
                        btnGuardarLiq.Enabled =
                        btnCancelarLiq.Enabled = true;
                        txtDescripcion.Enabled =
                        txtReferencia.Enabled =
                        btnEditar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Servicios y Movimientos en Vacio
        /// </summary>
        /// <param name="id_recurso">Recurso de la Liquidación</param>
        /// <param name="id_tipo_recurso">Tipo de Recurso</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        private void cargaServiciosMovimientosRecurso(int id_recurso, int id_tipo_recurso, DateTime fecha_liquidacion)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Buscando Servicios y Movimientos en Vacio
                using (DataTable dtServiciosLiquidacion = SAT_CL.Liquidacion.Reportes.ReporteServiciosMovimientosLiquidacion(id_recurso, id_tipo_recurso, fecha_liquidacion, liq.id_estatus, liq.id_liquidacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtServiciosLiquidacion))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvServiciosLiquidacion, dtServiciosLiquidacion, "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", lblOrdenarRecursos.Text, true, 4);

                        //Añadiendo Tabla a Session
                        TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosLiquidacion, "Table1");
                    }
                    else
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvServiciosLiquidacion);

                        //Eliminando Tabla de Session
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    }
                }

                //Invocando Método de Suma
                sumaTotalesRecursos();
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales al Pie del GridView
        /// </summary>
        private void sumaTotalesRecursos()
        {
            //Validando que existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Mostrando Totales
                gvServiciosLiquidacion.FooterRow.Cells[22].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Pagos)", "")));
                gvServiciosLiquidacion.FooterRow.Cells[23].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Anticipos)", "")));
                gvServiciosLiquidacion.FooterRow.Cells[24].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Comprobaciones)", "")));
                gvServiciosLiquidacion.FooterRow.Cells[25].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Diesel)", "")));
                gvServiciosLiquidacion.FooterRow.Cells[26].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(TFacturasAnticipos)", "")));
            }
            else
            {
                //Mostrando Totales
                gvServiciosLiquidacion.FooterRow.Cells[22].Text = 
                gvServiciosLiquidacion.FooterRow.Cells[23].Text = 
                gvServiciosLiquidacion.FooterRow.Cells[24].Text = 
                gvServiciosLiquidacion.FooterRow.Cells[25].Text = 
                gvServiciosLiquidacion.FooterRow.Cells[26].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método Privado encargado de Caragr los Cobros Recurrentes dada una Liquidación
        /// </summary>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        private void cargaCobrosRecurrentes(byte id_tipo_asignacion, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisora, DateTime fecha_liquidacion)
        {
            //Obteniendos Cargos Recurrentes
            using (DataTable dtCargosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesEntidad(id_tipo_asignacion, id_unidad, id_operador, id_proveedor, id_compania_emisora, fecha_liquidacion))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCargosRecurrentes))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvCobrosRecurrentes, dtCargosRecurrentes, "Id", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCargosRecurrentes, "Table7");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentes);

                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvCobrosRecurrentes);
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar Todos los Cobros Recurrentes que ha Tenido la Entidad
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        private void cargaCobrosRecurrentesTotales(int id_liquidacion, byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisor)
        {
            //Instanciando Cobros Recurrentes
            using (DataTable dtCobrosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesTotales(id_liquidacion, id_tipo_entidad, id_unidad, id_operador, id_proveedor, id_compania_emisor))
            {
                //Validando que existen los Cobros Recurrentes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosRecurrentes))
                {
                    //Cargando Cobro Recurrente
                    TSDK.ASP.Controles.CargaGridView(gvCobroRecurrenteLiquidacion, dtCobrosRecurrentes, "Id", "");

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCobrosRecurrentes, "Table3");
                }
                else
                {
                    //Inicializando Cobro Recurrente
                    TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }
        /// <summary>
        /// Obtiene los Dias Pagados
        /// </summary>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="fecha_liquidacion">Fecha de la Liquidación</param>
        /// <param name="fecha_ultimo_viaje">Fecha del Ultimo Viaje</param>
        /// <returns></returns>
        private TimeSpan ObtieneDiasPagados(int id_entidad, int tipo_entidad, DateTime fecha_liquidacion, out DateTime fecha_ultimo_viaje)
        {
            //Declarando Objeto de Retorno
            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
            fecha_ultimo_viaje = DateTime.MinValue;

            //Obteniendo la Ultima Fecha
            DateTime fecha_ultima_liq = SAT_CL.Liquidacion.Liquidacion.ObtieneUltimaFechaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)tipo_entidad, id_entidad);

            //Validando que exista una Fecha de Ultima Liquidación
            if (fecha_ultima_liq != DateTime.MinValue)
                //Obtiene Dias pagados
                dias_pagados = fecha_liquidacion - fecha_ultima_liq;
            else
            {
                //Obteniendo Primer Viaje
                DateTime fecha_primer_viaje = SAT_CL.Despacho.Reporte.ObtieneFechaPrimerViajeAsignado(id_entidad, tipo_entidad);
                //Validando que exista una Fecha del Primer Viaje
                if (fecha_primer_viaje != DateTime.MinValue)
                    //Obtiene Dias pagados
                    dias_pagados = fecha_liquidacion - fecha_primer_viaje;
            }

            //Obtiene Fecha del Ultimo Viaje Asignado
            fecha_ultimo_viaje = SAT_CL.Despacho.Reporte.ObtieneFechaUltimoViajeAsignado(id_entidad, tipo_entidad);
            //Devolviendo Dias Pagados
            return dias_pagados;
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender">Control que Ejecuta la Acción</param>
        /// <param name="nombre_ventana">Nombre de la Ventana</param>
        private void gestionaVentanas(Control sender, string nombre_ventana)
        {
            //Validando Nombre
            switch (nombre_ventana)
            {
                case "Pagos":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "PagosLiquidacion", "contenedorVentanaPagos", "ventanaPagos");
                    break;
                case "ConfirmacionOperacion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                    break;
                case "Anticipos":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "AnticiposLiquidacion", "contenedorVentanaAnticipos", "ventanaAnticipos");
                    break;
                case "Comprobaciones":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ComprobacionesLiquidacion", "contenedorVentanaComprobaciones", "ventanaComprobaciones");
                    break;
                case "AltaComprobaciones":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "AltaComprobacionesLiquidacion", "contenedorVentanaAltaComprobaciones", "ventanaAltaComprobaciones");
                    break;
                case "CobrosRecurrentesPend":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "AltaComprobacionesLiquidacion", "contenedorVentanaCobrosPendientes", "ventanaCobrosPendientes");
                    break;
                case "TarifasAplicables":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "TarifasPagoLiquidacion", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
                    break;
                case "ReferenciasViaje":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                    break;
                case "TimbrarLiquidacion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "TimbrarLiquidacion", "contenidoConfirmacionTimbrarLiquidacion", "confirmacionTimbrarLiquidacion");
                    break;
                case "CancelarTimbrado":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "CancelarTimbrado", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");
                    break;
                case "Diesel":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ValesDiesel", "contenedorVentanaDiesel", "ventanaDiesel");
                    break;
                case "Movimientos":
                    //Mostrando Ventana Modal CobroRecurrenteHistorial
                    ScriptServer.AlternarVentana(sender, "Movimientos", "contenedorVentanaMovimientos", "ventanaMovmientos");
                    break;
                case "CobroRecurrenteHistorial":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "CobroRecurrenteHistorial", "contenedorVentanaHistorialCobrosRecurrentes", "ventanaHistorialCobrosRecurrentes");
                    break;
                case "Evidencias":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "Evidencias", "contenidoVentanaEvidencias", "ventanaEvidencias");
                    gestionaVentanas(this, "Movimientos");
                    break;
                case "ConfirmaEliminaLiquidacion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ConfirmacionEliminaLiquidacion", "contenedorVentanaConfirmacionEliminaLiquidacion", "ventanaConfirmacionEliminaLiquidacion");
                    break;
                case "Devolucion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ResumenDevoluciones", "contenedorVentanaResumenDevoluciones", "ventanaResumenDevoluciones");
                    break;
                case "AltaDevolucion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "AltaDevoluciones", "modalDevolucionFaltante", "devolucionFaltante");
                    break;
                case "PagosMV":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "PagosMV", "contenedorVentanaPagoMovVacio", "ventanaPagoMovVacio");
                    break;
                case "HistorialVencimiento":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "HistorialVencimientos", "contenedorVentanaHistorialVencimientos", "ventanaHistorialVencimientos");
                    break;
                case "CambioCuentaPago":
                    ScriptServer.AlternarVentana(sender, "CambioCuentaPago","contenedorVentanaCambioCuentaPagoModal", "contenedorVentanaCambioCuentaPago");
                    break;
                case "Calificacion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "Calificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");
                    break;
                case "HistorialCalificacion":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "HistorialCalificacion", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");
                    break;
                case "FacturacionWS":
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(sender, "FacturacionWS", "contenedorVentanaFacturacionWebService", "ventanaFacturacionWebService");
                    break;
                case "FacturacionLiq":
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(sender, "FacturacionLiq", "contenedorVentanaFacturacionProveedor", "ventanaFacturacionProveedor");
                    break;
                case "CerrarVentanaCalificacion":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaCalificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");
                    break;
                case "CerrarVentanaHistorial":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaHistorial", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");
                    break;
                case "ResumenParadas":
                    ScriptServer.AlternarVentana(sender, "ResumenParadas", "contenedorVentanaResumenParadas", "ventanaResumenParadas");
                    break;
                case "Lectura":
                    {
                        ScriptServer.AlternarVentana(sender, "Lectura", "contenedorVentanaLectura", "ventanaLectura");
                        break;
                    }
                case "CierraLectura":
                    {
                        ScriptServer.AlternarVentana(sender, "CierraLectura", "contenedorVentanaLectura", "ventanaLectura");
                        break;
                    }
                case "ControlDiesel":
                    {
                        ScriptServer.AlternarVentana(sender, "ControlDiesel", "contenedorVentanaControlDiesel", "ventanaControlDiesel");
                        break;
                    }
                case "CierraControlDiesel":
                    {
                        ScriptServer.AlternarVentana(sender, "ControlDiesel", "contenedorVentanaControlDiesel", "ventanaControlDiesel");
                        break;
                    }
                case "LecturaHistorial":
                    {
                        ScriptServer.AlternarVentana(sender, "LecturaHistorial", "contenedorVentanaLecturaHistorial", "ventanaLecturaHistorial");
                        break;
                    }
                case "CierraLecturaHistorial":
                    {
                        ScriptServer.AlternarVentana(sender, "CierraLecturaHistorial", "contenedorVentanaLecturaHistorial", "ventanaLecturaHistorial");
                        break;
                    }

            }
        }
        /// <summary>
        /// Método Privado encargado de Validar el Estatus del Movimiento
        /// </summary>
        /// <param name="id_movimiento">Movimiento Por Validar</param>
        /// <returns></returns>
        private bool validaMovimientoTerminado(int id_movimiento)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;
                MovimientoAsignacionRecurso.Tipo tipo_asignacion = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? MovimientoAsignacionRecurso.Tipo.Operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? MovimientoAsignacionRecurso.Tipo.Unidad : MovimientoAsignacionRecurso.Tipo.Tercero;

                //Instanciando Movimiento
                using (Movimiento mov = new Movimiento(id_movimiento))
                {
                    //Validando que exista el Movimiento
                    if (mov.id_movimiento > 0)
                    {
                        //Instanciando Asignación del Recurso
                        using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(mov.id_movimiento, MovimientoAsignacionRecurso.Estatus.Terminado, tipo_asignacion, id_entidad))
                        {
                            //Validando que exista la Asignación
                            if (mar.id_movimiento_asignacion_recurso > 0)

                                //Asignando Positiva la Validación
                                result = true;
                            else
                                //Asignando Negativa la Validación
                                result = false;
                        }
                    }
                }
            }

            //Devovliendo Resultado Obtenido
            return result;
        }

        #region Método Calificacion Operador
        /// <summary>
        /// Método que obtiene las calificaciones de un operador
        /// </summary>       
        private void cargaCalificacion()
        {
            //Creación de la variable Calificación 
            byte Calificacion = 0;
            int CantidadComentarios = 0;
            //Instanciando Encabezado de Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Obtiene el promedio de calificación del operador
                Calificacion = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(76, liq.id_operador);
                CantidadComentarios = SAT_CL.Calificacion.Calificacion.ObtieneNumeroComentarios(76, liq.id_operador);
                //Acorde al promedio colocara el promedio
                switch (Calificacion)
                {
                    case 1:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella1.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 2:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella2.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 3:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella3.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 4:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella4.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 5:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella5.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    default:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella.png";
                        lkbComentarios.Text = "0 / 5" + " ( 0 Opiniones  )";
                        break;
                }
            }            
        }

   

        #endregion

        #region Métodos Movimientos

        /// <summary>
        /// Método Privado encargado de Cargar los Movimientos dado un Viaje
        /// </summary>
        /// <param name="id_viaje">Viaje Seleccionado</param>
        /// <param name="id_liquidacion">Liquidacion Actual</param>
        private void cargaMovimientosServicio(int id_viaje, int id_liquidacion, int id_entidad, byte id_tipo_entidad, byte id_estatus_liquidacion)
        {
            //Obteniendo Movimientos por Viaje
            using (DataSet ds = SAT_CL.Despacho.Reporte.ObtieneMovimientosYPagosPorViaje(id_viaje, id_liquidacion, id_entidad, id_tipo_entidad, id_estatus_liquidacion, false))
            {
                /*** Movimientos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Cargando Movimientos
                    TSDK.ASP.Controles.CargaGridView(gvMovimientos, ds.Tables["Table"], "Id-NoPago-NoSegmento", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"].Copy(), "Table10");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table10");
                }
                /*** Pagos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvPagosLigados, ds.Tables["Table1"], "IdPago", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"].Copy(), "Table14");
                    //Mostrando Totales
                    gvPagosLigados.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table14"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPagosLigados);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table14");
                    //Mostrando Sumatoria de Totales
                    gvPagosLigados.FooterRow.Cells[5].Text = "0.00";
                }
                /*** Anticipos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table2"))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvAnticipos, ds.Tables["Table2"], "Id", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table2"].Copy(), "Table4");
                    //Mostrando Totales
                    gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                    //Mostrando Sumatoria de Totales
                    gvAnticipos.FooterRow.Cells[6].Text = "0.00";
                }
                /*** Comprobaciones ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table3"))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvComprobaciones, ds.Tables["Table3"], "Id", "", true, 2);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table3"].Copy(), "Table5");
                    //Mostrando Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                    //Mostrando Sumatoria de Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = "0.00";
                }
                /*** Diesel ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table4"))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvDiesel, ds.Tables["Table4"], "Id", "", true, 2);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table4"].Copy(), "Table7");
                    //Mostrando Totales
                    gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvDiesel);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
                    //Mostrando Sumatoria de Totales
                    gvDiesel.FooterRow.Cells[5].Text = "0.00";
                }

                //Deshabilita los Controles de Captura y Edicion de Pagos
                limpiaControlesPagos();
                habilitaControlesPagos(false);

                //Inicializando Controles
                TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                TSDK.ASP.Controles.InicializaIndices(gvPagos);
                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
                TSDK.ASP.Controles.InicializaIndices(gvAnticipos);

                //Quitando Seleccion de las Filas
                TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
            }

            //Instanciando liquidación
            using (SAT_CL.Liquidacion.Liquidacion l = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
                //Actualziando información de liquidación
                cargaValoresTotalesLiquidacion(id_liquidacion, l.tipo_asignacion, l.id_unidad, l.id_operador, l.id_proveedor, l.id_compania_emisora);
        }
        /// <summary>
        /// Método encargado de Cargar los Detalles por Movimiento
        /// </summary>
        /// <param name="id_movimiento"></param>
        private void cargaDetallesMovimientos(int id_movimiento)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                //Obteniendo Pagos
                using (DataTable dtPagos = SAT_CL.Liquidacion.Pago.ObtienePagosMovimiento(id_movimiento, liq.id_liquidacion, liq.id_tipo_asignacion, id_entidad, liq.id_estatus, true))
                {
                    //Validando que existen Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagos))
                    {
                        //Cargando Pagos
                        TSDK.ASP.Controles.CargaGridView(gvPagos, dtPagos, "IdPago", "", true, 1);
                        //Añadiendo Tablas a Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPagos, "Table2");
                        //Mostrando Totales
                        gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(Total)", "")).ToString();
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvPagos);
                        //Eliminando Tabla de Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                        //Mostrando Totales
                        gvPagos.FooterRow.Cells[5].Text = "0.00";
                    }
                }

                //Obteniendo Pagos
                using (DataTable dtPagosMV = SAT_CL.Liquidacion.Pago.ObtienePagosMovimiento(id_movimiento, liq.id_liquidacion, liq.id_tipo_asignacion, id_entidad, liq.id_estatus, false))
                {
                    //Validando que existen Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagosMV))
                    {
                        //Cargando Pagos
                        TSDK.ASP.Controles.CargaGridView(gvPagosMV, dtPagosMV, "IdPago", "", true, 1);
                        //Añadiendo Tablas a Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPagosMV, "Table16");
                        //Mostrando Totales
                        gvPagosMV.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table16"].Compute("SUM(Total)", "")).ToString();
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvPagosMV);
                        //Eliminando Tabla de Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table16");
                        //Mostrando Totales
                        gvPagosMV.FooterRow.Cells[5].Text = "0.00";
                    }
                }

                //Instanciando Deposito
                using (DataTable dtAnticipos = SAT_CL.EgresoServicio.Deposito.CargaDepositosMovimiento(id_movimiento, id_entidad, liq.id_tipo_asignacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAnticipos))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvAnticipos, dtAnticipos, "Id", "", true, 1);
                        //Añadiendo tabla a Session
                        TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAnticipos, "Table4");
                        //Mostrando Totales
                        gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                        //Eliminando Tabla de Session
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                        //Mostrando Totales
                        gvAnticipos.FooterRow.Cells[6].Text = "0.00";
                    }
                }

                //Obteniendo Comprobaciones
                using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesMovimiento(id_movimiento, liq.id_liquidacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvComprobaciones, dtComprobaciones, "Id", "", true, 1);
                        //Añadiendo tabla a Session
                        TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtComprobaciones, "Table5");
                        //Mostrando Totales
                        gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                        //Eliminando Tabla de Session
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                        //Mostrando Totales
                        gvComprobaciones.FooterRow.Cells[4].Text = "0.00";
                    }
                }

                //Obteniendo Vales de Diesel
                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(id_movimiento, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvDiesel, dtDiesel, "Id", "", true, 2);
                        //Añadiendo tabla a Session
                        TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDiesel, "Table9");
                        //Mostrando Totales
                        gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table9"].Compute("SUM(Total)", "")).ToString();
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvDiesel);

                        //Eliminando Tabla de Session
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table9");

                        //Mostrando Sumatoria de Totales
                        gvDiesel.FooterRow.Cells[5].Text = "0.00";
                    }
                }//*/
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar la Tira de Imagenes
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        private void cargaImagenesDetalle(int id_servicio)
        {
            //Realizando la carga de URL de imagenes a mostrar
            using (DataTable mit = SAT_CL.ControlEvidencia.ControlEvidenciaDocumento.ObtieneControlEvidenciaDocumentosImagenes(id_servicio, Convert.ToInt32(gvMovimientos.SelectedDataKey["NoSegmento"])))
            {
                //Validando que existan imagenes a mostrar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEvidencias, mit, "Id", "");

                    //Añadiendo Tabla 
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table13");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEvidencias);

                    //Eliminando Tabla 
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table13");
                }
            }
        }

        #endregion

        #region Métodos Anticipos

        /// <summary>
        /// Método encargado de Sumar el Total de los Depositos
        /// </summary>
        private void sumaTotalAnticipos()
        {
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table4"))

                //Mostrando Totales
                gvAnticipos.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")));
            else
                //Mostrando Totales
                gvAnticipos.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
        }

        #endregion

        #region Métodos Comprobaciones

        /// <summary>
        /// Método encargado de Sumar el Total de las Comprobaciones
        /// </summary>
        private void sumaTotalComprobaciones()
        {
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table5"))

                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")));
            else
                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar el Control de Comprobaciones
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobacion</param>
        /// <param name="id_deposito">Referencia del Deposito</param>
        private void inicializaControlComprobaciones(int id_comprobacion, int id_deposito)
        {
            //Instanciando Comprobación
            using (Comprobacion cmp = new Comprobacion(id_comprobacion))

            //Instanciando Deposito
            using (Deposito dep = new Deposito(id_deposito))
            {
                //Validar que exista la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = cmp.id_comprobacion.ToString();
                    ddlConcepto.SelectedValue = cmp.id_concepto_comprobacion.ToString();
                    txtObservacion.Text = cmp.observacion_comprobacion;
                    txtValorUnitario.Text = cmp.objDetalleComprobacion.monto.ToString();
                }
                else
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = "Por Asignar";
                    txtObservacion.Text = "";

                    //Validando que exista el Depsoito
                    if (dep.id_deposito > 0)
                    {
                        //Concatenando Valor al Control TextBox
                        txtValorUnitario.Text = dep.objDetalleLiquidacion.monto.ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 39, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", dep.id_concepto, "");
                    }
                    else
                    {   //Limpiando Control
                        txtValorUnitario.Text = "";
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                    }
                    //Habilitando Control
                    txtValorUnitario.Enabled = true;
                }

                //Cargando Facturas
                cargaFacturasComprobacion(id_comprobacion);
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar las Facturas de la Comprobación
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        private void cargaFacturasComprobacion(int id_comprobacion)
        {
            //Obteniendo Facturas
            using (DataTable dtFacturas = FacturadoProveedorRelacion.ObtieneFacturasComprobacion(id_comprobacion))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvFacturasComprobacion, dtFacturas, "IdFactura-IdFacturaRelacion", "", true, 2);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table7");

                    //Deshabilitando Control
                    txtValorUnitario.Enabled = false;
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasComprobacion);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasComprobacion);
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Comprobación
        /// </summary>
        private void guardaComprobacion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Deposito
            int idDeposito = Convert.ToInt32(gvAnticipos.SelectedIndex == -1 ? 0 : gvAnticipos.SelectedDataKey["Id"]);

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))

            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(idDeposito))
            {
                //Validando que no exista la Comprobación
                if (lblIdComprobacion.Text == "Por Asignar")
                {
                    //Validando que el Movimiento se encuentre Terminado
                    if (validaMovimientoTerminado(dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_movimiento : 0))

                        //Insertando Comprobación
                        result = Comprobacion.InsertaComprobacion(idDeposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text, false, 0,
                                                                    DetalleLiquidacion.Estatus.Registrado, liq.id_unidad, liq.id_operador, liq.id_proveedor,
                                                                    dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_servicio : 0, 
                                                                    dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_movimiento : 0,
                                                                    liq.fecha_liquidacion, liq.id_liquidacion, 1, Convert.ToDecimal(txtValorUnitario.Text),
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");
                }
                else
                {
                    //Instanciando Comprobación
                    using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text)))
                    {
                        //Validando que exista el Registro
                        if (cmp.id_comprobacion > 0)
                        {
                            //Validando que el Movimiento se encuentre Terminado
                            if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))

                                //Editando Comprobación
                                result = cmp.EditaComprobacion(cmp.id_deposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text,
                                                        cmp.bit_transferencia, cmp.id_transferencia, (DetalleLiquidacion.Estatus)cmp.objDetalleComprobacion.id_estatus_liquidacion,
                                                        cmp.objDetalleComprobacion.id_unidad, cmp.objDetalleComprobacion.id_operador, cmp.objDetalleComprobacion.id_proveedor_compania,
                                                        cmp.objDetalleComprobacion.id_servicio, cmp.objDetalleComprobacion.id_movimiento, cmp.objDetalleComprobacion.fecha_liquidacion,
                                                        cmp.objDetalleComprobacion.id_liquidacion, cmp.objDetalleComprobacion.cantidad, Convert.ToDecimal(txtValorUnitario.Text == "" ? "0" : txtValorUnitario.Text),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");
                        }
                    }
                }

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                    //Inicializando Controles
                    inicializaControlComprobaciones(result.IdRegistro, idDeposito);

                    //Obteniendo Valores
                    int idServicio = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]);
                    int idMovimiento = Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"]);

                    //Invocando Método de Actualización de Valores
                    cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                    //Actualizando Servicios y Movimientos en Vacio
                    cargaServiciosMovimientosRecurso(id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                    //Validando que sea un Servicio
                    if (idServicio != 0)
                        //Marcando Fila
                        Controles.MarcaFila(gvServiciosLiquidacion, idServicio.ToString(), "IdServicio", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);
                    //Si es Movimiento
                    else
                        //Marcando Fila
                        Controles.MarcaFila(gvServiciosLiquidacion, idMovimiento.ToString(), "IdMovimiento", "IdServicio-IdMovimiento-IdOperador-IdUnidad-IdUnidad2", ((DataSet)Session["DS"]).Tables["Table1"], lblOrdenarRecursos.Text, Convert.ToInt32(ddlTamanoRecursos.SelectedValue), true, 3);

                    //Sumando Totales
                    sumaTotalesRecursos();
                }

                //Mostrando Mensaje de la Operacion
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
        {
            //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {
                //Asignando Atributo Obligatorios
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
            {
                //Asignando Atributo Obligatorios
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

            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {   //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idEmisor = 0;

                            //Validando que exista la Compania
                            if (emi.id_compania_emisor_receptor > 0)

                                //Asignando Emisor
                                idEmisor = emi.id_compania_emisor_receptor;

                            //Si no existe
                            else
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultado = new RetornoOperacion();

                                //Insertando Compania
                                resultado = CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(),
                                                                            doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(), 0, false, false, true, 0, "", "", "", 0, 0, 0, 0,
                                                                            "FACTURAS DE PROVEEDOR", "", 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Asignando Registro
                                idEmisor = resultado.IdRegistro;
                            }

                            //Validando que el RFC sea igual
                            if (idEmisor > 0)
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

                                                //Validando que Exista una Addenda
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
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
        /// Método Privado encargado de Guardar la Factura en XML
        /// </summary>
        private void guardaFacturaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando Objeto de Mensaje
            string mensaje = "";

            //Declarando Variable para Factura
            int idFactura = 0;

            //Instanciando Comprobacion
            using (SAT_CL.Liquidacion.Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text == "Por Asignar" ? "0" : lblIdComprobacion.Text)))
            {
                //Validando la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Validando que el Movimiento este Terminado
                    if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))
                    {
                        //Inicializando transacción
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Declarando Documento XML
                            XmlDocument doc = (XmlDocument)Session["XML"];

                            //Validando que exista una Factura
                            if (doc != null)
                            {
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
                                        //Validando que Exista el Emisor
                                        if (emisor.id_compania_emisor_receptor != 0)
                                        {
                                            //Instanciando Emisor-Compania
                                            using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                            {
                                                //Validando que coincida el RFC
                                                if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                                {
                                                    //Declarando Variables Auxiliares
                                                    decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                                    /** Retenciones **/
                                                    //Validando que no exista el Nodo
                                                    if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                                    {
                                                        //Validando que existan Retenciones
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                        {
                                                            //Validando que existan Nodos
                                                            if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                            {
                                                                //Recorriendo Retenciones
                                                                foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                                {
                                                                    //Sumando Impuestos Retenidos
                                                                    totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Asignando Total de Impuestos
                                                        totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                                    /** Traslados **/
                                                    //Validando que no exista el Nodo
                                                    if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                                    {
                                                        //Validando que existan Traslados
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                        {
                                                            //Validando que existan Nodos
                                                            if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                            {
                                                                //Recorriendo Traslados
                                                                foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                                {
                                                                    //Sumando Impuestos Trasladados
                                                                    totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Asignando Total de Impuestos
                                                        totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);
                                                    
                                                    //Insertando factura
                                                    resultado = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                        Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                        doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                        doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                        Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                                                        "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                        0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                        doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                        totalImpTrasladados, totalImpRetenidos,
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
                                            resultado = new RetornoOperacion("El Compania Proveedora no esta registrado");
                                    }

                                    //Validando que se inserto la Factura
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Obteniendo Factura
                                        idFactura = resultado.IdRegistro;

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

                                    //Validando la Operación de la Transacción
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertando Comprobacion de Factura
                                        resultado = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 104, Convert.ToInt32(lblIdComprobacion.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando la Operación de la Transacción
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizando el Total de las Comprobaciones
                                            resultado = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando la Operación de la Transacción
                                            if (resultado.OperacionExitosa)

                                                //Completando Transacción
                                                trans.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    //Instanciando Excepcion
                                    resultado = new RetornoOperacion(mensaje);

                                    //Limpiando Session
                                    Session["XML"] = null;
                                }
                            }
                            else
                            {
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe la Factura");

                                //Limpiando Session
                                Session["XML"] = null;
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");

                    //Validando que la Operación haya Sido Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Obteniendo Entidad
                            int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                            //Cargando resultados Actualizados
                            cargaMovimientosServicio(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdServicio"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
                        }

                        //Inicializando Controles
                        inicializaControlComprobaciones(Convert.ToInt32(lblIdComprobacion.Text), 0);
                    }
                }
                else
                {
                    //Instanciando Excepcion
                    resultado = new RetornoOperacion("No existe la Comprobación");

                    //Limpiando Session
                    Session["XML"] = null;
                }
            }

            //Mostrando Mensaje de la Operación
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos Pagos

        /// <summary>
        /// Método Privado encargado de Habilitar los Controles de Captura del Pago
        /// </summary>
        /// <param name="enabled"></param>
        private void habilitaControlesPagos(bool enabled)
        {
            //Asignando habilitación o Deshabilitación de Controles
            ddlTipoPago.Enabled =
            txtCantidad.Enabled =
            txtValorU.Enabled =
            //txtTotal.Enabled = 
            txtDescripcion.Enabled =
            txtReferencia.Enabled =
            btnGuardarPago.Enabled =
            btnCancelarPago.Enabled = enabled;
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los datos de los Controles de Pagos
        /// </summary>
        private void limpiaControlesPagos()
        {
            //Asignando habilitación o Deshabilitación de Controles
            txtCantidad.Text =
            txtValorU.Text =
            txtTotal.Text = "0";
            txtDescripcion.Text =
            txtReferencia.Text = "";
        }
        /// <summary>
        /// Método encargado de Sumar el Total de Pagos
        /// </summary>
        private void sumaTotalPagos()
        {
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table4"))

                //Mostrando Totales
                gvPagos.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(Total)", "")));
            else
                //Mostrando Totales
                gvPagos.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
        }
        /// <summary>
        /// Método encargado de Sumar el Total de Pagos
        /// </summary>
        private void sumaTotalPagosMV()
        {
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table16"))

                //Mostrando Totales
                gvPagosMV.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table16"].Compute("SUM(Total)", "")));
            else
                //Mostrando Totales
                gvPagosMV.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
        }
        /// <summary>
        /// Método encargado de Pagar el Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        private void pagaServicio(int id_servicio)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declarando Variables Auxiliares
            int idRecurso = 0;
            int idTipoAsignacion = 0;

            //Inicializando transaccion
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación actual
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Asignando Recurso
                    idRecurso = liquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liquidacion.id_operador :
                        (liquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liquidacion.id_unidad : liquidacion.id_proveedor);
                    //Asignando Tipo de Asignación
                    idTipoAsignacion = liquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? 2 :
                        (liquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? 1 : 3);

                    //Validando que el Servicio no Tenga Pagos Aplicados
                    if (!SAT_CL.Liquidacion.Pago.ValidaPagoServicio(id_servicio, idRecurso, idTipoAsignacion))
                    {
                        //Cerrando Ventana de Movimientos
                        gestionaVentanas(this, "Movimientos");

                        //Declarando variables auxiliares
                        int id_entidad_pago = 0;
                        SAT_CL.TarifasPago.Tarifa.PerfilPago perfil_pago;
                        //Determinando el tipo de entidad a pagar
                        if (liquidacion.id_operador > 0)
                        {
                            perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Operador;
                            id_entidad_pago = liquidacion.id_operador;
                        }
                        else if (liquidacion.id_unidad > 0)
                        {
                            perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Unidad;
                            id_entidad_pago = liquidacion.id_unidad;
                        }
                        else
                        {
                            perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Transportista;
                            id_entidad_pago = liquidacion.id_proveedor;
                        }

                        //Realizando búsqueda de tarifa de pago aplicable
                        using (DataTable tarifas = SAT_CL.TarifasPago.Tarifa.ObtieneTarifasPagoServicio(id_servicio, perfil_pago, id_entidad_pago, true))
                        {
                            //Si existen registros
                            if (tarifas != null)
                            {
                                //Guardando tarifas en sesión
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], tarifas, "Table13");

                                //Si existe más de una tarifa que aplique a este servicio
                                if (tarifas.Rows.Count > 1)
                                {
                                    //Cargando conjunto de tarifas en control Gridview
                                    TSDK.ASP.Controles.CargaGridView(gvTarifasPago, tarifas, "IdTarifa", lblOrdenadoTarifasAplicables.Text);
                                    //Asignando Tooltip para diferenciar la entidad a la que se aplicará la tarifa (Tarifas de Pago Servicio / Tarifas de Pago Movimiento)
                                    gvTarifasPago.ToolTip = "Tarifas de Pago Servicio";
                                    //Mostrando ventana modal con resultados
                                    TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "ventanaTarifasPago", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
                                }
                                //Si sólo hay una coincidencia
                                else if (tarifas.Rows.Count == 1)
                                {
                                    //Aplicando tarifa a servicio
                                    resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, tarifas.Rows[0].Field<int>("IdTarifa"), id_servicio,
                                                                            liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor,
                                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Si se actualizó correctamente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Obteniendo Pago
                                        int idPago = resultado.IdRegistro;

                                        //Obteniendo Tarifas Secundarias
                                        using (DataTable dtTarifasSecundarias = SAT_CL.TarifasPago.TarifaCompuesta.ObtieneTarifasSecundarias(tarifas.Rows[0].Field<int>("IdTarifa")))
                                        {
                                            //Validando que Existen Registros
                                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTarifasSecundarias))
                                            {
                                                //Recorriendo Ciclo de Tarifas Secundarias
                                                foreach (DataRow dr in dtTarifasSecundarias.Rows)
                                                {
                                                    //Aplicando Tarifa Secundaria
                                                    resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(dr["IdTarifaSecundaria"]), id_servicio,
                                                                            liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación fuese Correcta
                                                    if (!resultado.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                            }
                                            else
                                                //Instanciando Resultado Positivo
                                                resultado = new RetornoOperacion(idPago);

                                            //Validando que la Operación fuese Exitosa
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Realizando actualización de vales y anticipos
                                                resultado = actualizaLiquidacionDepositosValesServicio(id_servicio, liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion);

                                                //Si no hay errores
                                                if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                                {
                                                    //Ocultando Ventana de Movimientos
                                                    gestionaVentanas(this, "Movimientos");

                                                    //Cargando los Movimientos y los Pagos
                                                    cargaServiciosMovimientosRecurso(id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.fecha_liquidacion);

                                                    //Sumando Totales
                                                    sumaTotalesRecursos();

                                                    //Confirmando cambios realizados
                                                    scope.Complete();
                                                }
                                            }
                                        }
                                    }

                                    //Si se aplicó la tarifa correctamente
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Actualizando anticipos y vales de diesel
                                        resultado = actualizaLiquidacionDepositosValesServicio(id_servicio, liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion);

                                        //Si no hay errores
                                        if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                        {
                                            //Ocultando Ventana de Movimientos
                                            gestionaVentanas(this, "Movimientos");

                                            //Cargando los Movimientos y los Pagos
                                            cargaServiciosMovimientosRecurso(id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.fecha_liquidacion);

                                            //Sumando Totales
                                            sumaTotalesRecursos();

                                            //Completando transacción
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                            //Si no hay tarifas
                            else
                            {
                                //Obteniendo Tipos de Pago
                                using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                {
                                    //Validando que existan los Conceptos de Pago
                                    if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                        //Cargando DropDownList
                                        Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                    else
                                        //Inicializando DropDownList
                                        Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                }

                                //Habilitando Controles
                                habilitaControlesPagos(true);

                                //Configurando Control
                                btnGuardarPago.CommandArgument = "Servicio";

                                //Ocultando Ventana de Movimientos
                                gestionaVentanas(this, "Movimientos");
                                //Invocando Ventana de Pagos
                                gestionaVentanas(this, "Pagos");
                            }
                        }

                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "Existen Pagos Aplicados al Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Realiza la búsqueda de la tarifa de pago del movimiento seleccionado
        /// </summary>
        /// <param name="id_movimiento">Movimiento a Pagar</param>
        /// <param name="comandoPago">Comando que verifica Movimiento de Servicio o Vacio</param>
        private void buscaTarifaPagoMovimiento(int id_movimiento, string comandoPago)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación actual
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Declarando variables auxiliares
                    int id_entidad_pago = 0;
                    SAT_CL.TarifasPago.Tarifa.PerfilPago perfil_pago;
                    //Determinando el tipo de entidad a pagar
                    if (liquidacion.id_operador > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Operador;
                        id_entidad_pago = liquidacion.id_operador;
                    }
                    else if (liquidacion.id_unidad > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Unidad;
                        id_entidad_pago = liquidacion.id_unidad;
                    }
                    else
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Transportista;
                        id_entidad_pago = liquidacion.id_proveedor;
                    }

                    //Realizando búsqueda de tarifa de pago aplicable
                    using (DataTable tarifas = SAT_CL.TarifasPago.Tarifa.ObtieneTarifasPagoMovimiento(id_movimiento, perfil_pago, id_entidad_pago, false))
                    {
                        //Si existen registros
                        if (tarifas != null)
                        {
                            //Guardando tarifas en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], tarifas, "Table13");

                            //Si existe más de una tarifa que aplique a este servicio
                            if (tarifas.Rows.Count > 1)
                            {
                                //Cargando conjunto de tarifas en control Gridview
                                TSDK.ASP.Controles.CargaGridView(gvTarifasPago, tarifas, "IdTarifa", lblOrdenadoTarifasAplicables.Text);

                                //Asignando Tooltip para diferenciar la entidad a la que se aplicará la tarifa (Tarifas de Pago Servicio / Tarifas de Pago Movimiento)
                                gvTarifasPago.ToolTip = "Tarifas de Pago Movimiento";

                                //Mostrando ventana modal con resultados
                                gestionaVentanas(this, "TarifasAplicables");

                                //Ocultando Boton de Pago de Servicio
                                btnPagarServicioManual.Visible = false;
                            }
                            //Si sólo hay una coincidencia
                            else if (tarifas.Rows.Count == 1)
                            {
                                //Aplicando tarifa a servicio
                                resultado = Pago.AplicaTarifaPagoMovimiento(liquidacion.id_liquidacion, tarifas.Rows[0].Field<int>("IdTarifa"), id_movimiento,
                                                                        liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor,
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si no hay problemas con el pago
                                if (resultado.OperacionExitosa)
                                {
                                    //Realizando actualización de vales y anticipos
                                    resultado = actualizaLiquidacionDepositosVales(id_movimiento, liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion, true);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                    {
                                        //Cargando los Movimientos y los Pagos
                                        cargaServiciosMovimientosRecurso(id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.fecha_liquidacion);

                                        //Sumando Totales
                                        sumaTotalesRecursos();
                                        
                                        //Confirmando cambios realizados
                                        scope.Complete();
                                    }
                                    else
                                        resultado = new RetornoOperacion(string.Format("Error al actualizar anticipos y vales del movimiento ID '{0}': {1}", id_movimiento, resultado.Mensaje));
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No hay Tarifas Coincidentes");
                    }
                }
            }

            //Si no hay tarifas coincidentes o bien no se encontró una aplicación (detalle) en la tarifa coincidente
            if (!resultado.OperacionExitosa)
            {
                //Validando Excepción de Depositos
                if(!(resultado.IdRegistro == -3))
                
                    //Configurando Control
                    configuraVentanaPagos(comandoPago);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Configurar la Ventana de Pagos
        /// </summary>
        /// <param name="comand">Comando de Configuración</param>
        private void configuraVentanaPagos(string comand)
        {
            //Validando Comando
            switch (comand)
            {
                case "Movimiento":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }

                        //Se borra origen de datos anterior
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table13");

                        //Instanciando el movimiento de interés
                        using (Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                        {
                            //Obteniendo Item de forma Dinamica
                            ListItem li = ddlTipoPago.Items.FindByText("Km " + mov.TipoMovimiento.ToString());

                            //Validando que exista el Tipo
                            if (li != null)
                            {
                                //Limpiando Items
                                ddlTipoPago.Items.Clear();
                                //Añadiendo Item
                                ddlTipoPago.Items.Add(li);
                            }

                            //Habilitando Controles para Insercción de Pago
                            limpiaControlesPagos();
                            habilitaControlesPagos(true);
                            //Asignando Cantidad
                            txtCantidad.Text = mov.kms.ToString();

                            //Asignando Comando
                            btnGuardarPago.CommandArgument = "Movimiento";
                            btnGuardarPago.CommandName = "CreaPago";

                            //Deshabilitando Control
                            txtDescripcion.Enabled = false;

                            //Instanciando Paradas
                            using (SAT_CL.Despacho.Parada parada_o = new Parada(mov.id_parada_origen), parada_d = new Parada(mov.id_parada_destino))
                            {
                                //Validando que Existan
                                if (parada_o.id_parada > 0 && parada_d.id_parada > 0)

                                    //Asignando Valor
                                    txtDescripcion.Text = "Pago desde " + parada_o.descripcion + " a " + parada_d.descripcion;
                                else
                                    //Limpiando Descripción
                                    txtDescripcion.Text = "";
                            }
                        }

                        //Mostrando ventana de captura
                        gestionaVentanas(this, "Pagos");
                        break;
                    }
                case "MovimientoVacio":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }

                        //Se borra origen de datos anterior
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table13");

                        //Instanciando el movimiento de interés
                        using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosLiquidacion.SelectedDataKey["IdMovimiento"])))
                        {
                            //Obteniendo Item de forma Dinamica
                            ListItem li = ddlTipoPago.Items.FindByText("Km " + mov.TipoMovimiento.ToString());

                            //Validando que exista el Tipo
                            if (li != null)
                            {
                                //Limpiando Items
                                ddlTipoPago.Items.Clear();
                                //Añadiendo Item
                                ddlTipoPago.Items.Add(li);
                            }
                            //Habilitando Controles para Insercción de Pago
                            limpiaControlesPagos();
                            habilitaControlesPagos(true);
                            //Asignando Cantidad
                            txtCantidad.Text = mov.kms.ToString();

                            //Asignando Comando
                            btnGuardarPago.CommandArgument = "Movimiento";
                            btnGuardarPago.CommandName = "PagoMovsVacio";

                            //Deshabilitando Control
                            txtDescripcion.Enabled = false;

                            //Instanciando Paradas
                            using (SAT_CL.Despacho.Parada parada_o = new Parada(mov.id_parada_origen), parada_d = new Parada(mov.id_parada_destino))
                            {
                                //Validando que Existan
                                if (parada_o.id_parada > 0 && parada_d.id_parada > 0)

                                    //Asignando Valor
                                    txtDescripcion.Text = "Pago desde " + parada_o.descripcion + " a " + parada_d.descripcion;
                                else
                                    //Limpiando Descripción
                                    txtDescripcion.Text = "";
                            }
                        }

                        //Mostrando ventana de captura
                        gestionaVentanas(this, "Pagos");
                        break;
                    }
                case "VariosMovimientos":
                    {
                        //Limpiando Controles
                        limpiaControlesPagos();

                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }
                                                
                        //Obteniendo Item de forma Dinamica
                        ListItem li = ddlTipoPago.Items.FindByText("Movimiento");

                        //Validando que exista el Tipo
                        if(li != null)
                        {
                            //Limpiando Items
                            ddlTipoPago.Items.Clear();
                            //Añadiendo Item
                            ddlTipoPago.Items.Add(li);
                        }
                        
                        //Asignando Valor
                        txtDescripcion.Text = "Pago a Varios Movimientos";
                        //Obteniendo Control
                        CheckBox chkVarios;

                        //Recorriendo cada fila
                        foreach (GridViewRow gvr in gvMovimientos.Rows)
                        {
                            //Obteniendo Control
                            chkVarios = (CheckBox)gvr.FindControl("chkVarios");

                            //Validando que existe la Fila
                            if (chkVarios != null)
                                //Seleccionando Fila
                                chkVarios.Checked = chkVarios.Enabled;
                        }

                        //Mostrando Total de Movimientos
                        txtCantidad.Text = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios").Length.ToString();

                        //Asignando Comando
                        btnGuardarPago.CommandArgument = "Movimiento";
                        btnGuardarPago.CommandName = "CreaPago";

                        //Visualizando Control
                        btnCrearPagoMov.Visible = btnCrearPagoMov.Enabled = true;
                        break;
                    }
                case "SinMovimiento":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }

                        //Habilitando Controles para Insercción de Pago
                        limpiaControlesPagos();
                        habilitaControlesPagos(true);

                        //Asignando Comando Indicado
                        btnGuardarPago.CommandArgument = "Movimiento";
                        btnGuardarPago.CommandName = "CreaOtrosPagos";
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Actualizar la Liquidación de los Depositos
        /// </summary>
        /// <param name="id_movimiento">Movimiento de Referencia de los Depositos y Vales</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación Actual</param>
        /// <param name="estatus">Estatus por Actualizar</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_app">Tipo de la Entidad de la Liquidación</param>
        /// <param name="actualiza_liquidacion">Valor que indica si los Depositos y/o Vales se van a Referenciar o Regresar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaLiquidacionDepositosVales(int id_movimiento, int id_liquidacion, int id_entidad, byte id_tipo_app, bool actualiza_liquidacion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(id_movimiento);

            //Obteniendo Depositos
            using (DataTable dtDepositos = Deposito.CargaDepositosMovimiento(id_movimiento, id_entidad, id_tipo_app))
            {
                //Validando que existan los registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDepositos))
                {
                    //Declarando Variable Auxiliar
                    int idDeposito = 0;
                    
                    //Recorriendo Registros
                    foreach (DataRow dr in dtDepositos.Rows)
                    {
                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(dr["Id"])))
                        {
                            //Validando que exista
                            if (dep.id_deposito > 0)
                            {
                                //Obteniendo Deposito
                                idDeposito = dep.id_deposito;
                                
                                //Actualizando Liquidación del Deposito
                                result = dep.ActualizaLiquidacionDeposito(actualiza_liquidacion ? id_liquidacion : 0, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                {
                                    result = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", dep.no_deposito, result.Mensaje));
                                    //Se termina el ciclo
                                    break;
                                }
                                else
                                {
                                    //Actualizando Deposito
                                    dep.ActualizaDeposito();
                                    
                                    //Obteniendo Comprobaciones
                                    using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesDeposito(dep.id_deposito))
                                    {
                                        //Validando que existan Comprobaciones
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                                        {
                                            //Recorriendo Registros
                                            foreach (DataRow drComp in dtComprobaciones.Rows)
                                            {
                                                //Instanciando Comprobación
                                                using (SAT_CL.Liquidacion.Comprobacion cmp = new Comprobacion(Convert.ToInt32(drComp["Id"])))
                                                {
                                                    //Validando si Existe la Comprobación
                                                    if (cmp.id_comprobacion > 0)
                                                    {
                                                        //Validando si se 
                                                        if (actualiza_liquidacion)
                                                        {
                                                            //Actualizando la Liquidación en la Comprobación
                                                            result = cmp.ActualizaComprobacionLiquidacion(id_liquidacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando Operación Correcta
                                                            if (!result.OperacionExitosa)

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                        else
                                                        {
                                                            //Validando que este Ligada a un Liquidación
                                                            if(cmp.objDetalleComprobacion.id_liquidacion > 0)

                                                                //Deshabilitando Comprobación
                                                                result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            else
                                                                //Instanciando Valor Deposito
                                                                result = new RetornoOperacion(id_movimiento);

                                                            //Validando Operación Correcta
                                                            if (!result.OperacionExitosa)

                                                                //Terminando Ciclo
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Valor Deposito
                                            result = new RetornoOperacion(id_movimiento);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Si no hay errores
            if (result.OperacionExitosa)
            {
                //Obteniendo Asignación de Diesel
                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(id_movimiento, id_liquidacion, id_entidad, id_tipo_app))
                {
                    //Validando que existan los registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtDiesel.Rows)
                        {
                            //Instanciando Deposito
                            using (AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = ad.ActualizaLiquidacionVale(actualiza_liquidacion ? id_liquidacion : 0, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                {
                                    result = new RetornoOperacion(string.Format("Error al actualizar vale de diesel '{0}': {1}", ad.no_vale, result.Mensaje));
                                    //Se termina el ciclo
                                    break;
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Depositos
                        result = new RetornoOperacion(id_movimiento);
                }
            }

            //Validando que la Liquidacion se vaya a actualizar
            if (!actualiza_liquidacion && result.OperacionExitosa)
            {
                //Obteniendo Comprobaciones
                using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesMovimiento(id_movimiento, id_liquidacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtComprobaciones.Rows)
                        {
                            //Instanciando Deposito
                            using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                    break;
                            }
                        }
                    }
                    else
                        //Instanciando Resultado Correcto
                        result = new RetornoOperacion(id_movimiento);
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar la Liquidación de los Depositos
        /// </summary>
        /// <param name="id_servicio">Servicio de Referencia de los Depositos y Vales</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación Actual</param>
        /// <param name="estatus">Estatus por Actualizar</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_app">Tipo de la Entidad de la Liquidación</param>
        /// <returns></returns>
        private RetornoOperacion actualizaLiquidacionDepositosValesServicio(int id_servicio, int id_liquidacion, int id_entidad, byte id_tipo_app)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(id_servicio);

            //Obteniendo Depositos
            using (DataTable dtDepositos = Deposito.CargaDepositosServicio(id_servicio, id_entidad, id_tipo_app))
            {
                //Validando que existan los registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDepositos))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtDepositos.Rows)
                    {
                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(dr["Id"])))
                        {
                            //Actualizando Liquidación del Deposito
                            result = dep.ActualizaLiquidacionDeposito(id_liquidacion, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Si la Operación fue incorrecta
                            if (!result.OperacionExitosa)
                            {
                                result = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", dep.no_deposito, result.Mensaje));
                                //Se termina el ciclo
                                break;
                            }
                            else
                            {
                                //Actualizando Deposito
                                dep.ActualizaDeposito();

                                //Obteniendo Comprobaciones
                                using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesDeposito(dep.id_deposito))
                                {
                                    //Validando que existan Comprobaciones
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                                    {
                                        //Recorriendo Registros
                                        foreach (DataRow drComp in dtComprobaciones.Rows)
                                        {
                                            //Instanciando Comprobación
                                            using (SAT_CL.Liquidacion.Comprobacion cmp = new Comprobacion(Convert.ToInt32(drComp["Id"])))
                                            {
                                                //Validando si Existe la Comprobación
                                                if (cmp.id_comprobacion > 0)
                                                {
                                                    //Actualizando la Liquidación en la Comprobación
                                                    result = cmp.ActualizaComprobacionLiquidacion(id_liquidacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando Operación Correcta
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        //Instanciando Valor Deposito
                                        result = new RetornoOperacion(dep.id_deposito);
                                }
                            }
                        }
                    }
                }
            }

            //Si no hay errores
            if (result.OperacionExitosa)
            {
                //Obteniendo Asignación de Diesel
                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselServicio(id_servicio, id_liquidacion, id_entidad, id_tipo_app))
                {
                    //Validando que existan los registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtDiesel.Rows)
                        {
                            //Instanciando Deposito
                            using (AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = ad.ActualizaLiquidacionVale(id_liquidacion, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                {
                                    result = new RetornoOperacion(string.Format("Error al actualizar vale de diesel '{0}': {1}", ad.no_vale, result.Mensaje));
                                    //Se termina el ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Diesel

        /// <summary>
        /// Método encargado de Sumar el Total de Vales de Diesel
        /// </summary>
        private void sumaTotalDiesel()
        {
            //Validando que Existe la Tabla
            if(TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9")))
            
                //Mostrando Totales
                gvDiesel.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table9"].Compute("SUM(Total)", "")));
            else
                //Mostrando Totales
                gvDiesel.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
        }

        #endregion

        #region Métodos "Resumen Liquidación"

        /// <summary>
        /// Método Privado encargado de Cargar el Resumen Total de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion"></param>
        private void cargaResumenTotalLiquidacion(int id_liquidacion)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Reporte Total
                    using (DataTable dtResumenLiquidación = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneResumenTotalLiquidacion(id_liquidacion))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtResumenLiquidación))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvResumenLiquidacion, dtResumenLiquidación, "Id", "");

                            //Ajustando Tamaño del GridView
                            gvResumenLiquidacion.PageSize = dtResumenLiquidación.Rows.Count + 1;

                            //Añadiendo Tabla a DataSet de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtResumenLiquidación, "Table12");
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvResumenLiquidacion);

                            //Eliminando Tabla de DataSet de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table12");
                        }
                    }
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvResumenLiquidacion);

                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table12");
                }
            }
        }

        #endregion

        #region Métodos Devolucion

        /// <summary>
        /// Método encargado de Cargar el Resumen de las Devoluciones
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        private void cargaResumenDevoluciones(int id_servicio)
        {
            //Obteniendo Devoluciones
            using (DataTable dtDevoluciones = SAT_CL.Despacho.DevolucionFaltante.ObtieneDevolucionesServicio(id_servicio))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDevoluciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDevoluciones, dtDevoluciones, "Id-IdDevolucion", lblOrdenadoDev.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDevoluciones, "Table15");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDevoluciones);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table15");
                }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvDevoluciones);
        }

        #endregion

        #region Métodos Cambio de Cuenta
        /// <summary>
        /// Actualiza el  Cambio de Cuenta
        /// </summary>
        private RetornoOperacion cambioCuenta()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //VInstanciamos Liquidación
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
               //Validamos Estatus de la Liquidacion
                if (objLiquidacion.id_estatus ==1)
                {
                    //Obteniendo Referencias
                    using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(Convert.ToInt32(Session["id_registro"]), 82, 2211))
                    {
                        //Valdiando que Existan
                        if (Validacion.ValidaOrigenDatos(dtRef))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtRef.Rows)
                            {
                                //Validando que Exista el Registro
                                if (Convert.ToInt32(dr["Id"]) > 0)
                                {
                                    //Instanciando Cuenta
                                    using (SAT_CL.Global.Referencia cuenta = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista la Referencia
                                        if (cuenta.habilitar)
                                        {
                                            //Editamos Referencia
                                            resultado = SAT_CL.Global.Referencia.EditaReferencia(cuenta.id_referencia, txtNuevaCuentaPago.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Terminamos el Ciclo
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            //Insertando Referencia
                            resultado = SAT_CL.Global.Referencia.InsertaReferencia(Convert.ToInt32(Session["id_registro"]), 82, 2211, txtNuevaCuentaPago.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                        }

                    }
                }
                else
                {
                    //Establecemos Mensaje Error
                    resultado = new RetornoOperacion("El estatus de la Liquidación no permite su edición.");
                }
            }
                //Devolvemos Valor Return
                return resultado;          
        }


        #endregion

        #endregion

        /// <summary>
        /// Evento Producido al Dar Click en el Link "Agregar Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAgregarFacturaLiq_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista la Liquidación
                if (liq.habilitar && liq.id_proveedor > 0)
                {
                    //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
                    string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/LiquidacionSimplificada.ascx", "~/Accesorios/ServicioFacturas.aspx?idRegistro=82&idRegistroB=" + liq.id_liquidacion.ToString());
                    //Define las dimensiones de la ventana Abrir registros de Usuario
                    string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=400";
                    //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
                    ScriptServer.AbreNuevaVentana(url, "Facturas Ligadas", configuracion, Page);
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(lkbAgregarFacturaLiq, "Solo se pueden agregar Facturas a Liquidaciones de un Proveedor", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /*// <summary>
        /// Evento Producido al Ligar las Facturas a la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLigarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.habilitar && !(liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado))
                    {
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFac");

                        //Si existen Filas Seleccionadas
                        if (gvr.Length > 0)
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Recorriendo Filas
                                foreach (GridViewRow gv in gvr)
                                {
                                    //Seleccionando Fila
                                    gvFacturasDisponibles.SelectedIndex = gv.RowIndex;

                                    //Instanciando Factura de Proveedor
                                    using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"])))
                                    {
                                        //Insertando Relación Factura Proveedor
                                        result = SAT_CL.CXP.FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"]),
                                                                                82, Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando el Resultado
                                        if (result.IdRegistro > 0 || result.IdRegistro == -1)
                                        {
                                            //Mostrando Excepción
                                            ScriptServer.MuestraNotificacion(btnLigarFactura, string.Format("ResultadoNotificacion_{0}", Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"])),
                                                            string.Format(result.OperacionExitosa ? "La Factura '{0}{1}' ha sido añadida a la Liquidación" : "La Factura '{0}{1}' ya se encuentra en la Liquidación", fp.serie, fp.folio),
                                                            result.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                            //Instanciando 
                                            result = result.IdRegistro == -1 ? new RetornoOperacion("", true) : result;
                                        }
                                        else
                                            //Terminando Ciclo
                                            break;
                                    }
                                }

                                //Validando Operaciones Exitosas
                                if (result.OperacionExitosa)

                                    //Completando Transacción
                                    trans.Complete();
                            }

                            //Validando Relaciones
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Indices
                                Controles.InicializaIndices(gvFacturasDisponibles);

                                //Invocando Métodos de Busqueda
                                buscarFacturasLiquidacion();
                                buscaFacturasLigadas();
                            }
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(btnLigarFactura, "Debe Seleccionar Facturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(btnLigarFactura, "Debe de Cerrar la Liquidación para poder Agregar Facturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Agregar la Factura a la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFacLiq_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Operación
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando el Estatus de la Liquidación
                if (liq.habilitar && !(liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado))
                {
                    //Declarando Objeto de Mensaje
                    string mensaje = "";

                    //Declarando Variable para Factura
                    int idFactura = 0;

                    //Inicializando transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Declarando Documento XML
                        XmlDocument doc = (XmlDocument)Session["XML"];

                        //Validando que exista una Factura
                        if (doc != null)
                        {
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
                                    //Validando que Exista el Emisor
                                    if (emisor.id_compania_emisor_receptor != 0)
                                    {
                                        //Instanciando Emisor-Compania
                                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                        {
                                            //Validando que coincida el RFC
                                            if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                            {
                                                //Declarando Variables Auxiliares
                                                decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                                // Retenciones 
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                                {
                                                    //Validando que existan Retenciones
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Retenciones
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                            {
                                                                //Sumando Impuestos Retenidos
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                                // Traslados
                                                //Validando que no exista el Nodo
                                                if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                                {
                                                    //Validando que existan Traslados
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                    {
                                                        //Validando que existan Nodos
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                        {
                                                            //Recorriendo Traslados
                                                            foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                            {
                                                                //Sumando Impuestos Trasladados
                                                                totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Asignando Total de Impuestos
                                                    totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);

                                                //Insertando factura
                                                resultado = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                    0, doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                    doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                    doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                    Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                                                    (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                    0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                    doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                    totalImpTrasladados, totalImpRetenidos,
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
                                        resultado = new RetornoOperacion("El Compania Proveedora no esta registrado");
                                }

                                //Validando que se inserto la Factura
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Factura
                                    idFactura = resultado.IdRegistro;

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

                                //Validando la Operación de la Transacción
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Comprobacion de Factura
                                    resultado = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 82, Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando la Operación de la Transacción
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Vaciando Sesión
                                        Session["XML"] =
                                        Session["XMLFileName"] = null;

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                            else
                            {
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion(mensaje);

                                //Limpiando Session
                                Session["XML"] =
                                Session["XMLFileName"] = null;
                            }
                        }
                        else
                        {
                            //Instanciando Excepcion
                            resultado = new RetornoOperacion("No existe la Factura");

                            //Limpiando Session
                            Session["XML"] = null;
                        }
                    }

                    //Validando Operación Exitosa
                    if (resultado.OperacionExitosa)

                        //Buscando facturas Ligadas
                        buscaFacturasLigadas();
                }
                else
                    //Instanciando Excepcion
                    resultado = new RetornoOperacion("Debe de Cerrar la Liquidación para poder Agregar Facturas");
            }

            //Mostrando Mensaje de la Operación
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Buscar Facturas Disponibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFac_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscarFacturasLiquidacion();
        }
        /// <summary>
        /// Evento Producido al Cambiar la Vista de las Facturas de Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerFacturasLiq_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Nueva":
                    {
                        //Mostrando Vista
                        mtvFacturasLiquidacion.ActiveViewIndex = 1;

                        //Limpiando Sessión
                        Session["XML"] =
                        Session["XMLFileName"] = null;
                        break;
                    }
                case "Ver":
                    {
                        //Mostrando Vista
                        mtvFacturasLiquidacion.ActiveViewIndex = 0;

                        //Invocando Método de Busqueda
                        buscaFacturasLigadas();
                        break;
                    }
            }
        }

        #region Eventos GridView "Facturas Disponibles"

        /// <summary>
        /// Evento Producido al Marcar/Desmarcar una Factura Disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFac_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;

                //Validando que Exista el Control
                if (chk != null)
                {
                    //Validando ID del Control
                    switch(chk.ID)
                    {
                        case "chkTodosFac":
                            {
                                //Gestionando Filas
                                Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosFac", chk.Checked);
                                break;
                            }
                        case "chkVariosFac":
                            {
                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFac");

                                //Si existen Filas
                                if (gvr != null)
                                {
                                    //Obteniendo Encabezado
                                    CheckBox header = (CheckBox)gvFacturasDisponibles.HeaderRow.FindControl("chkTodosFac");

                                    //Validando que exista el Control
                                    if (header != null)
                                    {
                                        //Validando que 
                                        if (gvr.Length == gvFacturasDisponibles.Rows.Count)

                                            //Marcando Control de Encabezado
                                            header.Checked = true;
                                        else
                                            //Desmarcando Control de Encabezado
                                            header.Checked = false;
                                    }

                                }
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacLiq_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del gridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table17"), Convert.ToInt32(ddlTamanoFacLiq.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacLiq.Text = Controles.CambiaSortExpressionGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table17"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table17"), e.NewPageIndex, true, 2);
        }

        #endregion

        #region Eventos GridView "Facturas Ligadas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacLigadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del gridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table18"), Convert.ToInt32(ddlTamanoFacLiq.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacLiq.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table18"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table18"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento que actualiza el estatus de  una factura  a Aceptada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAceptarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Selecciona  una factura ligada a servicio 
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando registro actual
                using (FacturadoProveedor FP = new FacturadoProveedor(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"])))
                {
                    //Si la Recepcion existe
                    if (FP.habilitar)
                    {
                        //Validando los Estatus donde se puede Aceptar
                        if ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                            //Actualiza el estatus del registro
                            resultado = FP.AceptaFacturaProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                        {
                            //Validando Estatus de la Factura
                            switch ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura)
                            {
                                case FacturadoProveedor.EstatusFactura.Aceptada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ya ha sido Aceptada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                case FacturadoProveedor.EstatusFactura.Liquidada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura tiene Pagos Aplicados.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Refacturacion:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Refacturada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Cancelada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Cancelada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Rechazada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Rechazada.");
                                        break;
                                    }
                            }
                        }
                    }
                }

                //Valida la acción de actualización
                if (resultado.OperacionExitosa)

                    //Invoca al método cargaFacturasLigadas()
                    buscaFacturasLigadas();

                //Inicializando Indices
                Controles.InicializaIndices(gvFacturasLigadas);

                //Envia un mensaje con el resultado de la operación. 
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar la Relación de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFacturaLiq_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Decalrando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.habilitar && !(liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado))
                    {
                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                        //Instanciando Relación
                        using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFPR"])))
                        {
                            //Validando que exista la Relación
                            if (fpr.habilitar)

                                //Deshabilitando Factura
                                result = fpr.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepcion
                                result = new RetornoOperacion("No Existe la Relación de Facturas");

                            //Validando que se Deshabilitara
                            if (result.OperacionExitosa)
                            {
                                //Inicializando Indices
                                Controles.InicializaIndices(gvFacturasLigadas);
                                
                                //Recargando Facturas Ligadas
                                buscaFacturasLigadas();
                            }
                        }
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("Debe de Cerrar la Liquidación para poder Agregar Facturas");
                }

                //Envia un mensaje con el resultado de la operación. 
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Métodos Facturas Disponibles

        /// <summary>
        /// Método encargado de Buscar las Facturas para la Liquidación
        /// </summary>
        private void buscarFacturasLiquidacion()
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista el Registro
                if (liq.habilitar)
                {
                    //Obteniendo Facturas Disponibles
                    using (DataTable dtFacturasDisponibles = SAT_CL.CXP.Reportes.ObtieneFacturasDisponiblesLiquidacion(liq.id_compania_emisora,
                                                liq.id_proveedor, txtSerie.Text, Convert.ToInt32(txtFolio.Text == "" ? "0" : txtFolio.Text)))
                    {
                        //Validando que existen Registros
                        if (Validacion.ValidaOrigenDatos(dtFacturasDisponibles))
                        {
                            //Cargando GridView
                            Controles.CargaGridView(gvFacturasDisponibles, dtFacturasDisponibles, "Id", lblOrdenadoFacLiq.Text, true, 2);

                            //Añadiendo Tabla a Sessión
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasDisponibles, "Table17");
                        }
                        else
                        {
                            //Inicializando GridView
                            Controles.InicializaGridview(gvFacturasDisponibles);

                            //Eliminando Tabla de Sessión
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table17");
                        }
                    }
                }
            }
        }

        #endregion

        #region Métodos Facturas Ligadas

        /// <summary>
        /// Método encargado de Buscar las Facturas Ligadas
        /// </summary>
        private void buscaFacturasLigadas()
        {
            //Obteniendo Facturas por Liquidación
            using (DataTable dtFacturasLiq = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasLiquidacion(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Existan
                if (Validacion.ValidaOrigenDatos(dtFacturasLiq))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasLigadas, dtFacturasLiq, "Id-IdFPR", lblOrdenadoFacLigadas.Text, true, 2);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLiq, "Table18");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasLigadas);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table18");
                }
            }
        }

        #endregion

        //*/

    }
}