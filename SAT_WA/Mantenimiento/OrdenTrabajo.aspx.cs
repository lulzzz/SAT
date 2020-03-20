using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using System;
using System.Data;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Mantenimiento
{
    public partial class OrdenTrabajo : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando que se haya producido un PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando Página
                inicializaPagina();

                //Invocando Configuración
                configuraNivelCombustible();
            }   

            //Invocando Método de Configuración
            cargaTalleresInternoExternos();
        }
        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraNivelCombustible();
        }
        /// <summary>
        /// Evento producido al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;
            
            //Validando estatus de Página
            switch (lkb.CommandName)
            {
                case "Nuevo":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        //Invocando Script
                        configuraNivelCombustible();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(131, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaOrdenTrabajo();
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        //Invocando Script
                        configuraNivelCombustible();
                        break;
                    }
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion result = new RetornoOperacion();
                        //Bloque transaccional
                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Orden de trabajo                       
                            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista un Producto
                                if (ot.id_orden_trabajo > 0)
                                {
                                    //Validando que la Orden no este Terminada
                                    if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                                    {
                                        //Deshabilitando Orden de Trabajo
                                        result = ot.DeshabilitaOrdenTrabajo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        
                                        //Valida la operación de deshabilitar una orden de trabajo
                                        if (result.OperacionExitosa)
                                            
                                            //Invoca el método de Deshabilitar FallasOrden
                                            result = SAT_CL.Mantenimiento.OrdenTrabajoFalla.DeshabilitaFallasOrden(ot.id_orden_trabajo, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        
                                        //Valida la operación
                                        if (result.OperacionExitosa)
                                            //Invoca al método Complete().
                                            trans.Complete();
                                    }
                                    else
                                        //Instanciando Orden de Trabajo
                                        result = new RetornoOperacion("La Orden de Trabajo se encuentra 'Terminada'.");
                                }
                            }

                            //Validando que la Operación sea exitosa
                            if (result.OperacionExitosa)
                            {
                                //Limpiando registro de Session
                                Session["id_registro"] = 0;
                                //Cambiando a Estatus "Nuevo"
                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                //Inicializando Forma
                                inicializaPagina();
                            }

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Actividad":
                    {
                        //Instanciando Orden de trabajo                       
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Producto
                            if (ot.id_orden_trabajo > 0)
                            {
                                //Validando que la Orden no este Terminada
                                if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                                {
                                    //Inicializando Control
                                    ucActividadOrdenTrabajo.InicializaControl(0, Convert.ToInt32(Session["id_registro"]), 0);

                                    //Mostrando Ventana
                                    TSDK.ASP.ScriptServer.AlternarVentana(upMenuPrincipal, upMenuPrincipal.GetType(), "Actividades", "contenedorVentanaActividades", "ventanaActividades");
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Trabajo se encuentra 'Terminada'.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this, "No Existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Bitacora":
                    {
                        //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "131", "Orden de Trabajo");
                        break;
                    }
                case "Referencias":
                    {
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "131", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Imprimir":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/OrdenTrabajo.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "OrdenTrabajo", Convert.ToInt32(Session["id_registro"])), "OrdenTrabajo", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "Terminar":
                    {
                        //Instanciando Orden de trabajo                       
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Producto
                            if (ot.id_orden_trabajo > 0)
                            {
                                //Validando que la Orden no este Terminada
                                if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                                {
                                    //Asignando Fecha Actual
                                    txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

                                    //Gestionando ventana Modal
                                    gestionaVentanaModal(lkb, "TerminoOrden");
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this, "La Orden de Trabajo se encuentra 'Terminada'.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this, "No Existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Facturar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();
                        
                        //Instanciando Orden de trabajo                       
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Producto
                            if (ot.habilitar)
                            {
                                //Validando que la Orden no este Terminada
                                if (ot.EstatusOrden == SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                                {
                                    //Obteniendo Referencia de Facturación
                                    string idFacOt = SAT_CL.Global.Referencia.CargaReferencia(ot.id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Factura", 0, "General"));

                                    //Validando que exista una Referencia de Facturación
                                    if (idFacOt.Equals(""))

                                        //Insertando Factura
                                        result = ot.FacturaOrdenTrabajo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("La Orden ya ha sido Facturada");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Orden debe de estar Terminada para su Facturación");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existe la Orden de Trabajo");

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Asignando Sesión
                                Session["id_registro"] = result.IdRegistro;
                                Session["estatus"] = Pagina.Estatus.Edicion;

                                //Inicializando Forma
                                inicializaPagina();
                            }
                        }

                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "FacturaProveedor":
                    {
                        //Instanciando Orden de Trabajo
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que sea de un Proveedor
                            if (ot.habilitar && ot.id_compania_proveedor > 0)
                            {
                                //Validando que la Orden no este Terminada
                                if (ot.EstatusOrden == SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                                {
                                    //Cargando Facturas de la Orden
                                    cargaFacturasProveedorOT();

                                    //Mostrando Ventana Modal
                                    gestionaVentanaModal(lkb, "FacturaProveedor");
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(lkb, "La Orden debe de estar Terminada", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(lkb, "Solo puede agregar Facturas, cuando la Orden de Trabajo es de un Proveedor", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar alguna Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;
            
            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Lectura":
                    //Gestionando Ventana Modal
                    gestionaVentanaModal(lkb, "LecturaHistorial");
                    break;
            }

            //Gestionando Ventana Modal
            gestionaVentanaModal(lkb, lkb.CommandName);
        }
        /// <summary>
        /// Evento Producido al Dar Click al Boton "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Invocando Método de Guardado
            result = guardaXML();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Cargando Factura de Proveedor
                cargaFacturasProveedorOT();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Descargar el Contenido de los GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Fallas":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
                        break;
                    }
                case "FacturasProveedor":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link de Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLectura_Click(object sender, EventArgs e)
        {
            //Validando Estatus en Sesión
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Orden de Trabajo
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Orden de Trabajo
                            if (ot.habilitar)
                            {
                                //Validando que sea una Unidad Interna
                                if(!ot.bit_unidad_externa)
                                {
                                    //Inicializando contenido de Lecturas
                                    wucLecturaHistorial.InicializaControl(ot.id_unidad, true);

                                    //Mostrando Ventana Modal
                                    gestionaVentanaModal(this, "LecturaHistorial");
                                }
                                else
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(this, "La Unidad tiene que ser Propia para visualizar sus Lecturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Mostrando Excepción
                                ScriptServer.MuestraNotificacion(this, "No existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha
            DateTime fecha;
            DateTime.TryParse(txtFecFin.Text, out fecha);

            //Obteniendo Ruta
            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista el Registro
                if (ot.habilitar)
                    
                    //Terminando Orden de Trabajo
                    result = ot.TerminaOrdenTrabajo(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Orden de Trabajo");
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Sesión
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Inicializando Forma
                inicializaPagina();
            }

            //Cerrando Ventana Modal
            gestionaVentanaModal(btnTerminar, "TerminoOrden");

            //Mostrando Resultado de la Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBitUnidadExt_CheckedChanged(object sender, EventArgs e)
        {
            //Configurando Controles Unidad
            configuraUnidad();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoTaller_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configurando Controles
            configuraTercero();

            //Invocando Carga de Talleres
            cargaTalleresInternoExternos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando 
            guardaOrdenTrabajo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando Estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Inicializando Página
            inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUnidad_TextChanged(object sender, EventArgs e)
        {
            //Instanciando Unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1))))
            {
                //Validando que Exista
                if (uni.id_unidad > 0)
                {
                    txtEstatus.Text = uni.EstatusUnidad.ToString();
                    //Creación de la variable de tipo string
                    string ubicacionActual = "";
                    //Determinando la ubicación del operador en base a estatus, id de parada y movimiento
                    switch (uni.EstatusUnidad)
                    {
                        //Si el estatus de la unidad es Parada Disponible o ParadaOcupado
                        case SAT_CL.Global.Unidad.Estatus.ParadaDisponible:
                        case SAT_CL.Global.Unidad.Estatus.ParadaOcupado:
                            //Instancian a la clase EstanciaUnidad
                            using (SAT_CL.Despacho.EstanciaUnidad est = new SAT_CL.Despacho.EstanciaUnidad(uni.id_estancia))
                            //Instanciando a la clase Parada
                            using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(est.id_parada))
                                //Asigna a la variable ubicacionActual la descripción de la parada
                                ubicacionActual = p.descripcion;
                            break;
                        //En caso de que el estatus de la unidad este en transito
                        case SAT_CL.Global.Unidad.Estatus.Transito:
                            //Instanciando a la clase movimiento
                            using (SAT_CL.Despacho.Movimiento m = new SAT_CL.Despacho.Movimiento(uni.id_movimiento))
                                //Asigna valores a la variable ubicacionActual
                                ubicacionActual = m.descripcion;
                            break;
                        default:
                            //Si no se encentra en ningun estatus.
                            ubicacionActual = "No Disponible";
                            break;
                    }
                    txtUbicacion.Text = ubicacionActual;
                    txtDescUnidad.Text = "Placas: " + uni.placas + ", Motor: " + uni.modelo_motor + ", Año: " + uni.ano;
                    //Asignando Valores Predeterminados
                    ddlTipoUnidad.SelectedValue = uni.id_tipo_unidad.ToString();
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
                    ddlSubTipo.SelectedValue = uni.id_sub_tipo_unidad.ToString();

                }
                else
                {
                    txtUbicacion.Text = "";
                    txtEstatus.Text = "";
                    txtDescUnidad.Text = "";
                    //Tipo Unidad
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "");
                    //Subtipo de Unidad
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "-- No Aplica", 1109);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucActividadOrdenTrabajo_ClickRegistrar(object sender, EventArgs e)
        {
            //Insertando Actividad
            RetornoOperacion result = ucActividadOrdenTrabajo.RegistraOtrdenTrabajoActividad();

            //Cargando Actividad
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoCentro);
        }
        /// <summary>
        /// Evento que permite cargar las opciones del ddlSubtipo de unidad a partir de la selecci´ón de un tipo de unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Subtipo de Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
        }

        #region Eventos GridView "Fallas"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFallas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFallas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFallas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFallas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenar.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFallas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento que permite agregar actividades a una orden de trabajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarActividad_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFallas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFallas, sender, "lnk", false);

                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (ot.id_orden_trabajo > 0)
                    {
                        //Validando que no se encuentre Registrada
                        if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                        {
                            //Inicializando Control
                            ucActividadOrdenTrabajo.InicializaControl(0, Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(gvFallas.SelectedDataKey["Id"]));

                            //Mostrando Ventana
                            TSDK.ASP.ScriptServer.AlternarVentana(upgvFallas, upgvFallas.GetType(), "Actividades", "contenedorVentanaActividades", "ventanaActividades");
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this, "La Orden de Trabajo se encuentra 'Terminada'.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this, "No Existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento que permite eliminar la asignación de actividades a una orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarActividad_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida que existan registros en el gridview
            if (gvActividadesOrdenTrabajo.DataKeys.Count > 0)
            {
                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (ot.id_orden_trabajo > 0)
                    {
                        //Validando que no se encuentre Registrada
                        if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                        {
                            //Selecciona la fila del gridview que se va a eliminar
                            TSDK.ASP.Controles.SeleccionaFila(gvActividadesOrdenTrabajo, sender, "lnk", false);
                            //Invoca la clase Orden trabajo para eliminar la actividades ligadas
                            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad ota = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividadesOrdenTrabajo.SelectedDataKey["IdActividad"])))
                            {
                                //Valida que exista una orden de trabajo
                                if (ota.id_orden_actividad > 0)
                                {
                                    //Valida el estatus de la actividad sea solo por iniciar
                                    if (ota.id_estatus == 1)
                                    {
                                        //Asigna el resultado al objeto retorno
                                        retorno = ota.DeshabilitaOrdenTrabajoActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                    //En caso contrario
                                    else
                                        //Envia un mensaje de erro al iniciar
                                        retorno = new RetornoOperacion("Solo se pueden eliminar actividades asignadas en estatus Por Iniciar");
                                }

                            }

                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("La Orden de Trabajo se encuentra 'Terminada'.");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No Existe la Orden de Trabajo");
                }

                //Invoca al método que inicializa los valores del gridview.
                inicializaGridViewActividadOrdenTrabajo();

                //Mensaje del resultado de la operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFallas.DataKeys.Count > 0)
            {
                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (ot.id_orden_trabajo > 0)
                    {
                        //Validando que no se encuentre Registrada
                        if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                        {
                            //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvFallas, sender, "lnk", false);
                            //Instanciando Falla
                            using (SAT_CL.Mantenimiento.OrdenTrabajoFalla otf = new SAT_CL.Mantenimiento.OrdenTrabajoFalla(Convert.ToInt32(gvFallas.SelectedDataKey["Id"])))
                            {
                                //Validando que Exista
                                if (otf.id_falla > 0)
                                {
                                    //Asignando valores
                                    txtDescripcionFallo.Text = otf.descripcion;
                                    txtFechaFalla.Text = otf.fecha.ToString("dd/MM/yyyy HH:mm");
                                }
                                else
                                {
                                    //Asignando valores
                                    txtDescripcionFallo.Text = "";
                                    txtFechaFalla.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No se puede cargar la Falla", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this, "La Orden de Trabajo se encuentra 'Terminada'.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this, "No Existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Validando que Existan Registros
            if (gvFallas.DataKeys.Count > 0)
            {
                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (ot.id_orden_trabajo > 0)
                    {
                        //Validando que no se encuentre Registrada
                        if (ot.EstatusOrden != SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada)
                        {
                            //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvFallas, sender, "lnk", false);

                            //Instanciando Falla
                            using (SAT_CL.Mantenimiento.OrdenTrabajoFalla otf = new SAT_CL.Mantenimiento.OrdenTrabajoFalla(Convert.ToInt32(gvFallas.SelectedDataKey["Id"])))
                            {
                                //Validando que Exista
                                if (otf.id_falla > 0)
                                {
                                    using (DataTable dt = SAT_CL.Mantenimiento.OrdenTrabajoActividad.CargaActividadesAsignadas(otf.id_orden_trabajo, otf.id_falla))
                                    {
                                        //Si existen actividades ligadas a la Orden  de trabajo
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                                        {
                                            //Abre la ventana modal
                                            ScriptServer.AlternarVentana(upgvFallas, gvFallas.GetType(), "AbrirVentana", "contenidoActividadesFallasOT", "actividadesFallasOT");

                                        }
                                        else
                                        {
                                            //Desabilita la falla reportada                        
                                            retorno = otf.DeshabilitaFalla(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //Valida la operación
                                            if (retorno.OperacionExitosa)
                                                //Invoca al método inicializaForma
                                                inicializaPagina();

                                            //Mostrando Mensaje de Operación
                                            TSDK.ASP.ScriptServer.MuestraNotificacion(upgvFallas, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        }
                                    }

                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(upgvFallas, "La Orden de Trabajo se encuentra 'Terminada'.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(upgvFallas, "No Existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }


        #endregion
        
        #region Eventos GridView "Actividad Orden de Trabajo"
        /// <summary>
        /// Evento que cambia el tamaño de registros del GridView actividades ligadas a una orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvActividadesOrdenTrabajo, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoActividad.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento que exporta a un archivo de excel los registros del GridView actividades ligadas a una orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarActividad_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "IdActividad");
        }
        /// <summary>
        /// Evento que permite al cambio de paginación del GridView del GridView actividades ligadas a una orden de trabajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadesOrdenTrabajo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvActividadesOrdenTrabajo, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento que regula el ordenamiento de registros en base a la columna seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividadesOrdenTrabajo_Sorting(object sender, GridViewSortEventArgs e)
        { 
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoActividad.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvActividadesOrdenTrabajo, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 2);
        }
  
        /// <summary>
        /// Evento que elimina las fallas reportadas con actividades ligadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminarActividadFalla_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFallas.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Instanciando Falla
                using (SAT_CL.Mantenimiento.OrdenTrabajoFalla otf = new SAT_CL.Mantenimiento.OrdenTrabajoFalla(Convert.ToInt32(gvFallas.SelectedDataKey["Id"])))
                {
                    //Validando que Exista
                    if (otf.id_falla > 0)
                        //Deshabilitando Falla
                        result = otf.DeshabilitaFalla(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede acceder a la Falla");

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)
                        //Invoca al método inicializaPagina
                        inicializaPagina();

                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            //Invoca al método que ciera la ventana modal
            ScriptServer.AlternarVentana(btnEliminarActividadFalla, "CerrarVentana", "contenidoActividadesFallasOT", "actividadesFallasOT");
        }


        #endregion

        #region Eventos Mano de Obra

        /// <summary>
        /// Evento generado al cambiar el DropDown de Mnao de Obra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoManoObra_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvManoObra, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoManoObra), true, 2);
        }

        /// <summary>
        /// Evento generado al Cambiar la Pagina de Mano de Obra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvManoObra_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvManoObra, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento generado al cambiar el Sorting  de Mano de Obra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvManoObra_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenarManoObra.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvManoObra, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 2);
        }

        /// <summary>
        /// Evento generado al Exportar la Mano de Obra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarManoObra_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"));
        }
        #endregion

        #region Refacciones Consumidas

        /// <summary>
        /// Evento generado al cambiar el DropDown de  Refacciones Consumidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRefaccionesConsumidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvRefaccionesConsumidas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoRefaccionesConsumidas), true, 0);
        }

        /// <summary>
        /// Evento generado al Cambiar la Pagina de  Refacciones Consumidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRefaccionesConsumidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvRefaccionesConsumidas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento generado al cambiar el Sorting  de Refacciones Consumidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRefaccionesConsumidas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenarRefaccionesConsumidas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvRefaccionesConsumidas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression, true, 2);
        }

        /// <summary>
        /// Evento generado al Exportar Refacciones Consumidas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarRefaccionesConsumidas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"));
        }

        #endregion

        #region Eventos GridView "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFL.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }

        #endregion

        #region Fallas
        /// <summary>
        /// Evento que cancela las acciones sobre un registro 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarFalla_Click(object sender, EventArgs e)
        {
            //Inicializando indices
            TSDK.ASP.Controles.InicializaIndices(gvFallas);
            //Limpiando Controles
            txtDescripcionFallo.Text = "";
            txtFechaFalla.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Evento que guarda las fallas reportadas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarFalla_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo Fecha
            DateTime fecha_falla = DateTime.MinValue;
            DateTime.TryParse(txtFechaFalla.Text, out fecha_falla);
            //Validando que exista una Selección
            if (gvFallas.SelectedIndex != -1)
            {
                //Instanciando Falla
                using (SAT_CL.Mantenimiento.OrdenTrabajoFalla otf = new SAT_CL.Mantenimiento.OrdenTrabajoFalla(Convert.ToInt32(gvFallas.SelectedDataKey["Id"])))
                {
                    //Validando que Exista la Falla
                    if (otf.id_falla > 0)
                        //Editando Registro
                        result = otf.EditaRegistroFalla(Convert.ToInt32(Session["id_registro"]), txtDescripcionFallo.Text.ToUpper(), fecha_falla, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("No se puede acceder a la Falla");
                }
            }
            else
                //Insertando Falla
                result = SAT_CL.Mantenimiento.OrdenTrabajoFalla.InsertaFalla(Convert.ToInt32(Session["id_registro"]), txtDescripcionFallo.Text.ToUpper(), fecha_falla, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que la Operación haya sido Exitosa 
            if (result.OperacionExitosa)
            {
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFallas);
                //Cargando Fallas
                cargaFallasOrden();
                //Limpia los controles de insercion de fallas
                txtDescripcionFallo.Text = "";
                txtFechaFalla.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            }

            //Mostrando Mensajes
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento que Cierra la ventana modal que agrega Actividades.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFallas);
            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upgvFallas, upgvFallas.GetType(), "Actividades", "contenedorVentanaActividades", "ventanaActividades");
            //Inicializa los valores del gridview ActividadOrdenTrabajo
            inicializaGridViewActividadOrdenTrabajo();
        }
        /// <summary>
        /// Evento que permite cerra una ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarActividadFalla_Click(object sender, EventArgs e)
        {
            //Invoca al método que ciera la ventana modal
            ScriptServer.AlternarVentana(btnCancelarActividadFalla, "CerrarVentana", "contenidoActividadesFallasOT", "actividadesFallasOT");
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
                case "Fallas":
                    //Cambiando estilos de pestañas
                    btnPestanaFallas.CssClass = "boton_pestana_activo";
                    btnRefaccionesConsumidas.CssClass=
                    btnPestanaManoObra.CssClass =
                    btnPestnaActividades.CssClass = "boton_pestana";
                    //Asignando vista activa de la forma
                    mtvSecciones.SetActiveView(vwFallas);
                    break;
                case "Actividades":
                    //Cambiando estilos de pestañas
                    btnRefaccionesConsumidas.CssClass =
                    btnPestanaManoObra.CssClass =
                    btnPestanaFallas.CssClass = "boton_pestana";
                    btnPestnaActividades.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvSecciones.SetActiveView(vwActividades);
                    //Inicializamos Grid View Actividad
                    inicializaGridViewActividadOrdenTrabajo();
                    break;
                case "ManoObra":
                    //Cambiando estilos de pestañas
                    btnRefaccionesConsumidas.CssClass =
                    btnPestnaActividades.CssClass =
                    btnPestanaFallas.CssClass = "boton_pestana";
                    btnPestanaManoObra.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvSecciones.SetActiveView(vwManoObra);
                    //Inicializamos Grid View Mano Obra
                    inicializaGridViewManoObra();
                    break;
                case "RefaccionesConsumidas":
                    //Cambiando estilos de pestañas
                    btnPestnaActividades.CssClass =
                    btnPestanaManoObra.CssClass = 
                    btnPestanaFallas.CssClass = "boton_pestana";
                    btnRefaccionesConsumidas.CssClass=  "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvSecciones.SetActiveView(vwRefaccionesConsumidas);
                    //Inicializamos Grid View Refacciones
                    inicializaGridViewRefaccionesConsumidas();
                    break;
            }
        }
        #endregion

        #region Eventos Lectura

        /// <summary>
        /// Método encargado de Crear una Nueva Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLecturaHistorial_btnNuevaLectura(object sender, EventArgs e)
        {
            //Instanciando Orden de Trabajo
            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Orden de Trabajo
                if (ot.habilitar)
                {
                    //Validando que sea una Unidad Interna
                    if (!ot.bit_unidad_externa)
                    {
                        //Inicializamos Control para Registro de Lecturas
                        wucLectura.InicializaControl(0, ot.id_unidad);

                        //Mostrando Ventana Modal
                        gestionaVentanaModal(this, "LecturaHistorial");
                        //Ocultando Ventana Modal
                        gestionaVentanaModal(this, "Lectura");
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "La Unidad tiene que ser Propia para visualizar sus Lecturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this, "No existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
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

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Orden de Trabajo
                    if (ot.habilitar)
                    {
                        //Validando que sea una Unidad Interna
                        if (!ot.bit_unidad_externa)
                        {
                            //Método encargado de Cerrar la ventana de Lectura
                            gestionaVentanaModal(this, "Lectura");

                            //Inicializamos Control de Historial de Lectura
                            wucLecturaHistorial.InicializaControl(ot.id_unidad, true);

                            //Método encargado de Abrir la Ventana de Historial de Lectura
                            gestionaVentanaModal(this, "LecturaHistorial");
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, "La Unidad tiene que ser Propia para visualizar sus Lecturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "No existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
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

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Instanciando Orden de Trabajo
                using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Orden de Trabajo
                    if (ot.habilitar)
                    {
                        //Validando que sea una Unidad Interna
                        if (!ot.bit_unidad_externa)
                        {
                            //Método encargado de Cerrar la ventana de Lectura
                            gestionaVentanaModal(this, "Lectura");

                            //Inicializamos Control de Historial de Lectura
                            wucLecturaHistorial.InicializaControl(ot.id_unidad, true);

                            //Método encargado de Abrir la Ventana de Historial de Lectura
                            gestionaVentanaModal(this, "LecturaHistorial");
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, "La Unidad tiene que ser Propia para visualizar sus Lecturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "No existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Consultar una Lectura
        /// </summary>
        protected void wucLecturaHistorial_lkbConsultar(object sender, EventArgs e)
        {
            //Instanciando Orden de Trabajo
            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista la Orden de Trabajo
                if (ot.habilitar)
                {
                    //Validando que sea una Unidad Interna
                    if (!ot.bit_unidad_externa)
                    {
                        //Inicializamos Control de Usuario Lectura
                        wucLectura.InicializaControl(wucLecturaHistorial.id_lectura, ot.id_unidad);
                        //Método encargado de Abrir la ventana de Lectura
                        gestionaVentanaModal(this, "Lectura");
                        //Método encargado de Cerrar la Ventana de Historial de Lectura
                        gestionaVentanaModal(this, "LecturaHistorial");
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "La Unidad tiene que ser Propia para visualizar sus Lecturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this, "No existe la Orden de Trabajo", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitarControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Configurar el Nivel de Combustible
        /// </summary>
        private void configuraNivelCombustible()
        {
            //Declarando Objeto de Configuración
            string script = @"";

            //Obteniendo Valor
            string valor = txtNivelCombustible.Text.Equals("") ? "0" : txtNivelCombustible.Text;

            //Validando Estatus de Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Configurando Script
                        script = @"<script type='text/javascript'>
                                    //Configurando Barra
                                    $('#" + ctdNivelCombustible.ClientID + @"').slider({
                                        range: 'min',
                                        disabled: false,
                                        value: " + valor + @",
                                        min: 0,
                                        max: 100,
                                        slide: function (event, ui) {
                                            //Asignando Valor
                                            $('#" + txtNivelCombustible.ClientID + @"').val(ui.value);
                                        }
                                    });

                                    //Asignando Nivel de Combustible a la Barra
                                    $('#" + txtNivelCombustible.ClientID + @"').val($('#" + ctdNivelCombustible.ClientID + @"').slider('value'));
                                   </script>";
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Configurando Script
                        script = @"<script type='text/javascript'>
                                    //Configurando Barra
                                    $('#" + ctdNivelCombustible.ClientID + @"').slider({
                                        range: 'min',
                                        disabled: true,
                                        value: " + valor + @",
                                        min: 0,
                                        max: 100,
                                        slide: function (event, ui) {
                                            //Asignando Valor
                                            $('#" + txtNivelCombustible.ClientID + @"').val(ui.value);
                                        }
                                    });

                                    //Asignando Nivel de Combustible a la Barra
                                    $('#" + txtNivelCombustible.ClientID + @"').val($('#" + ctdNivelCombustible.ClientID + @"').slider('value'));
                                   </script>";
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Configurando Script
                        script = @"<script type='text/javascript'>
                                    //Configurando Barra
                                    $('#" + ctdNivelCombustible.ClientID + @"').slider({
                                        range: 'min',
                                        disabled: false,
                                        value: " + valor + @",
                                        min: 0,
                                        max: 100,
                                        slide: function (event, ui) {
                                            //Asignando Valor
                                            $('#" + txtNivelCombustible.ClientID + @"').val(ui.value);
                                        }
                                    });

                                    //Asignando Nivel de Combustible a la Barra
                                    $('#" + txtNivelCombustible.ClientID + @"').val($('#" + ctdNivelCombustible.ClientID + @"').slider('value'));
                                   </script>";
                        break;
                    }
            }

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ConfiguraNivelCombustible", script, false);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Tipo de Taller
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoTaller, "", 1123);
            //Tipo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 1124);
            //Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 1119);
            //Tipo Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "");
            //Subtipo de Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
            //Cargando Tamanos del Gridview
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 56);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoActividad, "", 56);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoManoObra, "", 56);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRefaccionesConsumidas, "", 56);

        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbImprimir.Enabled = true;
                        //Edicion
                        lkbEliminar.Enabled =
                        lkbAgregarActividad.Enabled =
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbFacturar.Enabled =
                        lkbFacturaProveedor.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbImprimir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEliminar.Enabled =
                        lkbAgregarActividad.Enabled =
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbFacturar.Enabled =
                        lkbFacturaProveedor.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbAgregarActividad.Enabled =
                        lkbEliminar.Enabled =
                        lkbImprimir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbFacturar.Enabled =
                        lkbFacturaProveedor.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Prvado encargado de Habilitar los Controles
        /// </summary>
        private void habilitarControles()
        {
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitando Controles
                        ddlEstatus.Enabled =
                        ddlTipo.Enabled = 
                        lkbLectura.Enabled = false;
                        txtFechaRecepcion.Enabled =
                        txtFechaCompromiso.Enabled =
                        //txtFechaInicio.Enabled =
                        //txtFechaFin.Enabled =
                        chkBitUnidadExt.Enabled =
                        ddlTipoTaller.Enabled =
                        txtProveedor.Enabled =
                        ddlSubTipo.Enabled =
                        txtNivelCombustible.Enabled =

                        txtNoSiniestro.Enabled =
                        txtEntregadoPor.Enabled =
                        txtRecibidoPor.Enabled =
                        txtOdometro.Enabled =
                        txtTaller.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Controles de Falla
                        txtDescripcionFallo.Enabled =
                        txtFechaFalla.Enabled =
                        btnGuardarFalla.Enabled =
                        btnCancelarFalla.Enabled =
                        gvActividadesOrdenTrabajo.Enabled =
                        gvFallas.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitando Controles
                        ddlEstatus.Enabled = false;
                        ddlTipo.Enabled =
                        lkbLectura.Enabled =
                        txtFechaRecepcion.Enabled =
                        txtFechaCompromiso.Enabled =
                        //txtFechaInicio.Enabled =
                        //txtFechaFin.Enabled =
                        chkBitUnidadExt.Enabled =
                        ddlTipoTaller.Enabled =
                        txtProveedor.Enabled =
                        ddlSubTipo.Enabled =
                        txtNivelCombustible.Enabled =
                        txtNoSiniestro.Enabled =
                        txtOdometro.Enabled =
                        txtEntregadoPor.Enabled =
                        txtRecibidoPor.Enabled =
                        txtTaller.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Controles de Falla
                        txtDescripcionFallo.Enabled =
                        txtFechaFalla.Enabled =
                        btnGuardarFalla.Enabled =
                        btnCancelarFalla.Enabled =
                        gvActividadesOrdenTrabajo.Enabled =
                        gvFallas.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Habilitando Controles
                        ddlEstatus.Enabled =
                        ddlTipo.Enabled =
                        lkbLectura.Enabled =
                        txtFechaRecepcion.Enabled =
                        txtFechaCompromiso.Enabled =
                        //txtFechaInicio.Enabled =
                        //txtFechaFin.Enabled =
                        chkBitUnidadExt.Enabled =
                        ddlTipoTaller.Enabled =
                        txtProveedor.Enabled =
                        ddlSubTipo.Enabled =
                        txtNivelCombustible.Enabled =
                        txtOdometro.Enabled =
                        txtNoSiniestro.Enabled =
                        txtEntregadoPor.Enabled =
                        txtRecibidoPor.Enabled =
                        txtTaller.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        //Controles de Falla
                        txtDescripcionFallo.Enabled =
                        txtFechaFalla.Enabled =
                        btnGuardarFalla.Enabled =
                        btnCancelarFalla.Enabled =
                        gvActividadesOrdenTrabajo.Enabled =
                        gvFallas.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que inicializa los valores del gridview acorde al estado de la página.
        /// </summary>
        private void inicializaGridViewActividadOrdenTrabajo()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializa el gridView
                        Controles.InicializaGridview(gvActividadesOrdenTrabajo);
                        break;
                    }
                //En caso de lectura y Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase actividad puesto
                        using (DataTable dtActividadOrdenTrabajo = SAT_CL.Mantenimiento.OrdenTrabajo.CargaActividadesOrdenTrabajo((int)Session["id_registro"]))
                        {
                            //Valida que existan datos en el datatable
                            if (Validacion.ValidaOrigenDatos(dtActividadOrdenTrabajo))
                            {
                                //Si existen registros, carga el gridview 
                                Controles.CargaGridView(gvActividadesOrdenTrabajo, dtActividadOrdenTrabajo, "IdActividad", lblOrdenadoActividad.Text, true, 2);
                                //Asigna valores a la variable de session del DS
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtActividadOrdenTrabajo, "Table2");
                            }
                            //En caso contrario
                            else
                            {
                                //Inicializa el gridView
                                Controles.InicializaGridview(gvActividadesOrdenTrabajo);
                                //Elimina los datos del dataset si se realizo una consulta anterior
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Tables2");
                            }

                        }
                    }
                    break;
            }

            //Inicializa indices GridView
            Controles.InicializaIndices(gvActividadesOrdenTrabajo);
        }
        /// <summary>
        /// Método que inicializa los valores del gridview acorde al estado de la página.
        /// </summary>
        private void inicializaGridViewManoObra()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializa el gridView
                        Controles.InicializaGridview(gvManoObra);
                        break;
                    }
                //En caso de lectura y Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase actividad puesto
                        using (DataTable dtManoObra = SAT_CL.Mantenimiento.ActividadAsignacion.CargaReporteManoObra((int)Session["id_registro"]))
                        {
                            //Valida que existan datos en el datatable
                            if (Validacion.ValidaOrigenDatos(dtManoObra))
                            {
                                //Si existen registros, carga el gridview 
                                Controles.CargaGridView(gvManoObra, dtManoObra, "Responsable", lblOrdenarManoObra.Text, true, 2);
                                //Asigna valores a la variable de session del DS
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtManoObra, "Table3");
                            }
                            //En caso contrario
                            else
                            {
                                //Inicializa el gridView
                                Controles.InicializaGridview(gvManoObra);
                                //Elimina los datos del dataset si se realizo una consulta anterior
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Tables3");
                            }

                        }
                    }
                    break;
            }

            //Inicializa indices GridView
            Controles.InicializaIndices(gvManoObra);
            //Suma Totales
            sumaTotalesManoObra();
        }

        /// <summary>
        /// Método que inicializa los valores del gridview acorde al estado de la página.
        /// </summary>
        private void inicializaGridViewRefaccionesConsumidas()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializa el gridView
                        Controles.InicializaGridview(gvRefaccionesConsumidas);
                        break;
                    }
                //En caso de lectura y Edición
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase actividad puesto
                        using (DataTable dtRefaccionesConsumidas = SAT_CL.Almacen.RequisicionDetalle.ObtieneRefaccionesConsumidas((int)Session["id_registro"]))
                        {
                            //Valida que existan datos en el datatable
                            if (Validacion.ValidaOrigenDatos(dtRefaccionesConsumidas))
                            {
                                //Si existen registros, carga el gridview 
                                Controles.CargaGridView(gvRefaccionesConsumidas, dtRefaccionesConsumidas, "SKU", lblOrdenarRefaccionesConsumidas.Text, true, 2);
                                //Asigna valores a la variable de session del DS
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtRefaccionesConsumidas, "Table4");
                            }
                            //En caso contrario
                            else
                            {
                                //Inicializa el gridView
                                Controles.InicializaGridview(gvRefaccionesConsumidas);
                                //Elimina los datos del dataset si se realizo una consulta anterior
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Tables4");
                            }

                        }
                    }
                    break;
            }

            //Inicializa indices GridView
            Controles.InicializaIndices(gvRefaccionesConsumidas);
            //Suma Totales 
            sumaTotalesRefaccionesConsumidas();
        }

        /// <summary>
        /// Método encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignando Valores
                        lblId.Text = "Por Asignar";
                        lblFacturada.Text = "--";
                        txtEstatus.Text = "";
                        txtUbicacion.Text = "";
                        txtFechaRecepcion.Text =
                        txtFechaFalla.Text =
                        txtFechaCompromiso.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        txtFechaInicio.Text =
                        txtFechaFin.Text = "";
                        chkBitUnidadExt.Checked = false;
                        txtCliente.Text =
                        txtUnidad.Text =
                        txtDescUnidad.Text =
                        txtProveedor.Text =
                            //txtNivelCombustible.Text =
                        txtTaller.Text =
                        txtOdometro.Text =
                        txtNoSiniestro.Text =
                        txtEntregadoPor.Text =
                        txtDescripcionFallo.Text =
                        txtRecibidoPor.Text = "";
                        txtNivelCombustible.Text = "0";

                        configuraUnidad();
                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        configuraUnidad();

                        txtFechaFalla.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Limpia la descripcioón de la falla reportada
                        txtDescripcionFallo.Text = "";
                        //Instanciando Registro
                        using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Exista
                            if (ot.id_orden_trabajo > 0)
                            {
                                //Obteniendo Referencia de Facturación
                                string idFacOt = SAT_CL.Global.Referencia.CargaReferencia(ot.id_orden_trabajo, 131, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 131, "Factura", 0, "General"));

                                //Asignando Valores
                                lblId.Text = ot.no_orden_trabajo.ToString();
                                lblFacturada.Text = idFacOt.Equals("") ? "NO" : "SI";
                                txtFechaRecepcion.Text = ot.fecha_recepcion.ToString("dd/MM/yyyy HH:mm");
                                txtFechaCompromiso.Text = ot.fecha_compromiso.ToString("dd/MM/yyyy HH:mm");
                                txtFechaInicio.Text = ot.fecha_inicio == DateTime.MinValue ? "" : ot.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                                txtFechaFin.Text = ot.fecha_fin == DateTime.MinValue ? "" : ot.fecha_fin.ToString("dd/MM/yyyy HH:mm");
                                chkBitUnidadExt.Checked = ot.bit_unidad_externa;
                                txtNivelCombustible.Text = Convert.ToInt32(ot.nivel_combustible).ToString();
                                txtNoSiniestro.Text = ot.no_siniestro;
                                txtEntregadoPor.Text = ot.entrega_unidad;
                                txtRecibidoPor.Text = ot.recibe_unidad; ;
                                txtOdometro.Text = ot.odometro.ToString();
                                //Instanciando Cliente
                                using (SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(ot.id_compania_cliente))
                                {
                                    //Validando que exista
                                    if (cli.id_compania_emisor_receptor > 0)
                                        //Asignando Valor
                                        txtCliente.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else
                                        //Asignando Valor
                                        txtCliente.Text = "";
                                }

                                //Instanciando Proveedor
                                using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(ot.id_compania_proveedor))
                                {
                                    //Validando que exista
                                    if (pro.id_compania_emisor_receptor > 0)
                                        //Asignando Valor
                                        txtProveedor.Text = pro.nombre + " ID:" + pro.id_compania_emisor_receptor.ToString();
                                    else
                                        //Asignando Valor
                                        txtProveedor.Text = "";
                                }

                                //Instanciando Unidad
                                using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(ot.id_unidad))
                                {
                                    //Validando que Exista
                                    if (uni.id_unidad > 0)
                                    {
                                        //Asignando Valor
                                        txtUnidad.Text = uni.numero_unidad + " ID:" + uni.id_unidad.ToString();
                                        //Creación de la variable de tipo string
                                        string ubicacionActual = "";
                                        //Determinando la ubicación del operador en base a estatus, id de parada y movimiento
                                        switch (uni.EstatusUnidad)
                                        {
                                            //Si el estatus de la unidad es Parada Disponible o ParadaOcupado
                                            case SAT_CL.Global.Unidad.Estatus.ParadaDisponible:
                                            case SAT_CL.Global.Unidad.Estatus.ParadaOcupado:
                                                //Instancian a la clase EstanciaUnidad
                                                using (SAT_CL.Despacho.EstanciaUnidad est = new SAT_CL.Despacho.EstanciaUnidad(uni.id_estancia))
                                                //Instanciando a la clase Parada
                                                using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(est.id_parada))
                                                    //Asigna a la variable ubicacionActual la descripción de la parada
                                                    ubicacionActual = p.descripcion;
                                                break;
                                            //En caso de que el estatus de la unidad este en transito
                                            case SAT_CL.Global.Unidad.Estatus.Transito:
                                                //Instanciando a la clase movimiento
                                                using (SAT_CL.Despacho.Movimiento m = new SAT_CL.Despacho.Movimiento(uni.id_movimiento))
                                                    //Asigna valores a la variable ubicacionActual
                                                    ubicacionActual = m.descripcion;
                                                break;
                                            default:
                                                //Si no se encentra en ningun estatus.
                                                ubicacionActual = "No Disponible";
                                                break;
                                        }
                                        txtUbicacion.Text = ubicacionActual;
                                        txtDescUnidad.Text = "Placas: " + uni.placas + ", Motor: " + uni.modelo_motor + ", Año: " + uni.ano;
                                        txtEstatus.Text = uni.EstatusUnidad.ToString();
                                    }
                                    else
                                    {
                                        //Asignando Valor
                                        txtUnidad.Text = "";
                                        txtUbicacion.Text = "";
                                        txtEstatus.Text = "";
                                        txtDescUnidad.Text = ot.descripcion_unidad;
                                    }

                                }

                                //Instanciando Taller
                                using (SAT_CL.Global.Ubicacion taller = new SAT_CL.Global.Ubicacion(ot.id_ubicacion_taller))
                                {
                                    //Validando que Exista
                                    if (taller.id_ubicacion > 0)
                                        //Asignando Valor
                                        txtTaller.Text = taller.descripcion + " ID:" + taller.id_ubicacion.ToString();
                                    else
                                        //Asignando Valor
                                        txtTaller.Text = "";
                                }

                                //Asignando Valores
                                ddlEstatus.SelectedValue = ot.id_estatus.ToString();
                                ddlTipo.SelectedValue = ot.id_tipo.ToString();
                                ddlTipoUnidad.SelectedValue = ot.id_tipo_unidad.ToString();
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipo, "-- No Aplica", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
                                ddlSubTipo.SelectedValue = ot.id_subtipo_unidad.ToString();
                                ddlTipoTaller.SelectedValue = ot.id_tipo_taller.ToString();
                            }
                        }

                        break;
                    }
            }

            //Configurando Controles según su Valor
            //configuraUnidad();
            configuraTercero();
            //Cargando Fallas
            cargaFallasOrden();
        }
        /// <summary>
        /// Método encargado de Configurar los Controles con respecto a la Unidad
        /// </summary>
        private void configuraUnidad()
        {
            //Validando el Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si la pagina esta en modo lectura, deshabilita los controles
                case Pagina.Estatus.Lectura:
                    {
                        //Habilitando Controles
                        txtCliente.Enabled =
                        txtDescUnidad.Enabled =
                        ddlSubTipo.Enabled =
                        ddlTipoUnidad.Enabled =
                        txtUnidad.Enabled = false;
                        break;
                    }
                //Si esta en modo lectura o edicion
                case Pagina.Estatus.Edicion:
                    {
                        //Validando el Valor del Control
                        switch (chkBitUnidadExt.Checked)
                        {
                            //Si el check es activado
                            case true:
                                {
                                    //Habilitando Controles
                                    txtCliente.Enabled =
                                    txtDescUnidad.Enabled =
                                    ddlTipoUnidad.Enabled =
                                    ddlSubTipo.Enabled = true;
                                    //Deshabilitando Control
                                    txtUnidad.Enabled = false;
                                    //Asignando Valores
                                    txtUnidad.Text = "";
                                    txtEstatus.Text = "";
                                    txtUbicacion.Text = "";
                                    txtDescUnidad.Text = "";
                                    break;
                                }
                            //Si el check esta desactivado
                            case false:
                                {
                                    //Deshabilitando Controles
                                    txtCliente.Enabled =
                                    txtDescUnidad.Enabled =
                                    ddlTipoUnidad.Enabled =
                                    ddlSubTipo.Enabled = false;
                                    //Habilitando Control
                                    txtUnidad.Enabled = true;
                                    txtDescUnidad.Text = "";
                                    txtCliente.Text = "";
                                    break;
                                }

                        }
                        break;
                    }
                case Pagina.Estatus.Nuevo:
                    {
                        //Validando el Valor del Control
                        switch (chkBitUnidadExt.Checked)
                        {
                            //Si el check es activado
                            case true:
                                {
                                    //Habilitando Controles
                                    txtCliente.Enabled =
                                    txtDescUnidad.Enabled =
                                    ddlTipoUnidad.Enabled =
                                    ddlSubTipo.Enabled = true;
                                    //Deshabilitando Control
                                    txtUnidad.Enabled = false;
                                    //Asignando Valores
                                    txtDescUnidad.Text = "";
                                    txtUnidad.Text = "";
                                    txtEstatus.Text = "";
                                    txtUbicacion.Text = "";
                                    break;
                                }
                            //Si el check esta desactivado
                            case false:
                                {
                                    //Deshabilitando Controles
                                    txtCliente.Enabled =
                                    txtDescUnidad.Enabled =
                                    ddlTipoUnidad.Enabled =
                                    ddlSubTipo.Enabled = false;
                                    //Habilitando Control
                                    txtUnidad.Enabled = true;
                                    txtCliente.Text = "";
                                    txtDescUnidad.Text = "";
                                    break;
                                }

                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Cargar la Configuración de los Talleres
        /// </summary>
        private void cargaTalleresInternoExternos()
        {
            //Obteniendo Compania
            string id_compania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script de Configuración
            string script = @"<script type='text/javascript'>
                                //Obteniendo Tipo de Taller (Externo/Interno)
                                var tipoTaller = " + ddlTipoTaller.SelectedValue + @";
                                
                                //Evento Change
                                $('#" + ddlTipoTaller.ClientID + @"').change(function () {
                                    
                                    //Limpiando Control
                                    $('#" + txtTaller.ClientID + @"').val('');

                                    //Invocando Funcion
                                    CargaAutocompleta();
                                });
                                
                                //Declarando Función de Autocompleta
                                function CargaAutocompleta(){
                                    
                                    //Validando Tipo de Entidad
                                    switch (tipoTaller) {
                                        case 1:
                                            {   
                                                //alert('Propio');
                                                //Cargando Catalogo de Talleres Internos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=1'});
                                                break;
                                            }
                                        case 2:
                                        case 3:
                                            {   
                                                //alert('Externo');
                                                //Cargando Catalogo de Talleres Externos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=2'});
                                                break;
                                            }
                                        default:
                                            {   
                                                //alert('Default');
                                                //Cargando Catalogo de Talleres Internos
                                                $('#" + txtTaller.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=40&param=" + id_compania + @"&param2=1'});
                                                break;
                                            }
                                    }
                                }
                                
                                //Invocando Funcion
                                CargaAutocompleta();
                                
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaTaller", script, false);
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(Control sender, string comando)
        {
            //Validando Comando
            switch (comando)
            {
                case "TerminoOrden":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, comando, "contenedorVentanaFechaTermino", "ventanaFechaTermino");
                        break;
                    }
                case "FacturaProveedor":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, comando, "contenedorVentanaFacturaProveedor", "ventanaFacturaProveedor");
                        break;
                    }
                case "LecturaHistorial":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, comando, "modalLecturaHistorial", "lecturaHistorial");
                        break;
                    }
                case "Lectura":
                    {
                        //Mostrando Ventana Modal
                        ScriptServer.AlternarVentana(sender, comando, "modalLectura", "Lectura");
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Configurar el Mantenimiento de Terceros
        /// </summary>
        private void configuraTercero()
        {
            //Acorde al cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En estatus Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Validando Tipo de Taller
                        switch (ddlTipoTaller.SelectedValue)
                        {
                            //Si el tipo taller es propio.
                            case "1":
                                {
                                    //Deshabilita y limpia los Controles.
                                    txtNoSiniestro.Enabled =
                                    txtProveedor.Enabled = false;
                                    txtTaller.Text = "";
                                    txtNoSiniestro.Text = "";
                                    txtProveedor.Text = "";
                                    break;
                                }
                            //Si el tipo taller es aseguradora.
                            case "2":
                                {
                                    //Habilita y limpia los Controles.
                                    txtNoSiniestro.Enabled =
                                    txtProveedor.Enabled = true;
                                    txtTaller.Text = "";
                                    txtNoSiniestro.Text = "";
                                    txtProveedor.Text = "";
                                    break;
                                }
                            //Si el tpo taller es externo
                            case "3":
                                {
                                    //Habilita, deshabilita y limpia los Controles.
                                    txtNoSiniestro.Enabled = false;
                                    txtNoSiniestro.Text = "";
                                    txtTaller.Text = "";
                                    txtProveedor.Enabled = true;
                                    txtProveedor.Text = "";
                                    break;
                                }
                        }
                        break;
                    }
                //En caso de que este en modo edición
                case Pagina.Estatus.Edicion:
                    {
                        //Valida Tipo taller
                        switch (ddlTipoTaller.SelectedValue)
                        {
                            //Si el tipo taller es Propia
                            case "1":
                                {
                                    //Deshabilita y Limpia Controles
                                    txtNoSiniestro.Enabled =
                                    txtProveedor.Enabled = false;
                                    txtProveedor.Text = "";
                                    txtNoSiniestro.Text = "";
                                    break;
                                }
                            //Si el tipo taller es aseguradora
                            case "2":
                                {
                                    //Habilitando Controles
                                    txtNoSiniestro.Enabled =
                                    txtProveedor.Enabled = true;

                                    break;
                                }
                            //Si el tipo taller es externo.
                            case "3":
                                {
                                    //Habilita, deshabilita y limpia los Controles.
                                    txtNoSiniestro.Enabled = false;
                                    txtProveedor.Enabled = true;
                                    txtNoSiniestro.Text = "";
                                    break;
                                }
                        }

                        break;
                    }
                //Si el estatus de la página es lectura.
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilitando Controles
                        txtNoSiniestro.Enabled =
                        txtProveedor.Enabled = false;
                        break;
                    }

            }
        }
        /// <summary>
        /// Método encargado de Guardar la Orden de Trabajo
        /// </summary>
        private void guardaOrdenTrabajo()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Crea objeto retorno y asigna el resultado del método validaFechas
            RetornoOperacion retorno = validaFechas();

            //Obteniendo Fechas
            DateTime fecha_recepcion, fecha_compromiso, fecha_inicio, fecha_fin;
            //fecha_recepcion = fecha_compromiso = fecha_inicio = fecha_fin = DateTime.MinValue;
            DateTime.TryParse(txtFechaRecepcion.Text, out fecha_recepcion);
            DateTime.TryParse(txtFechaCompromiso.Text, out fecha_compromiso);
            DateTime.TryParse(txtFechaInicio.Text, out fecha_inicio);
            DateTime.TryParse(txtFechaFin.Text, out fecha_fin);

            //Valida si la operación de validación de fechas es correcta
            if (retorno.OperacionExitosa)
            {
                //Validando Estatus
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        {
                            //Insertando Orden de Trabajo
                            result = SAT_CL.Mantenimiento.OrdenTrabajo.InsertaOrdenTrabajo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, fecha_recepcion, fecha_compromiso, fecha_inicio, fecha_fin, chkBitUnidadExt.Checked,
                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                                        Convert.ToByte(ddlTipoUnidad.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)), Convert.ToByte(ddlSubTipo.SelectedValue),
                                        chkBitUnidadExt.Checked ? txtDescUnidad.Text.ToUpper() : "", Convert.ToByte(ddlTipoTaller.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTaller.Text, "ID:", 1)),
                                        Convert.ToDecimal(txtOdometro.Text), Convert.ToDecimal(txtNivelCombustible.Text), txtNoSiniestro.Text, txtEntregadoPor.Text.ToUpper(), txtRecibidoPor.Text.ToUpper(),
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            break;
                        }
                    case Pagina.Estatus.Edicion:
                        {
                            //Instanciando Orden de Trabajo
                            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que Exista
                                if (ot.id_orden_trabajo > 0)

                                    //Editando Orden de Trabajo
                                    result = ot.EditaOrdenTrabajo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, (SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo)Convert.ToByte(ddlEstatus.SelectedValue),
                                                (SAT_CL.Mantenimiento.OrdenTrabajo.TipoOrdenTrabajo)Convert.ToByte(ddlTipo.SelectedValue), fecha_recepcion, fecha_compromiso, fecha_inicio, fecha_fin, chkBitUnidadExt.Checked,
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                                                Convert.ToByte(ddlTipoUnidad.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)), Convert.ToByte(ddlSubTipo.SelectedValue),
                                                chkBitUnidadExt.Checked ? txtDescUnidad.Text.ToUpper() : "", Convert.ToByte(ddlTipoTaller.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTaller.Text, "ID:", 1)),
                                                Convert.ToDecimal(txtOdometro.Text), Convert.ToDecimal(txtNivelCombustible.Text), txtNoSiniestro.Text, txtEntregadoPor.Text, txtRecibidoPor.Text,
                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede Acceder a la Orden de Trabajo");
                            }
                            break;
                        }
                }

                //Validando que la Operación fuese Exitosa
                if (result.OperacionExitosa)
                {
                    //Asignando Valores a Session
                    Session["id_registro"] = result.IdRegistro;
                    Session["estatus"] = Pagina.Estatus.Edicion;

                    //Inicializando Página
                    inicializaPagina();
                }

                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            //En caso contrario.
            else
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Cargar las Fallas de la Orden de Trabajo
        /// </summary>
        private void cargaFallasOrden()
        {
            //txtFechaFalla.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            //txtDescripcionFallo.Text = "";
            //Validando Estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFallas);
                        TSDK.ASP.Controles.InicializaIndices(gvFallas);
                        //Añadiendo Tabla a DataSet de Sesion
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Fallas
                        using (DataTable dtFallas = SAT_CL.Mantenimiento.OrdenTrabajoFalla.CargaFallasOrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Existen Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFallas))
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvFallas, dtFallas, "Id", "", true, 1);

                                //Añadiendo Tabla a DataSet de Sesion
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFallas, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvFallas);

                                //Añadiendo Tabla a DataSet de Sesion
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                            //Inicializa los indices del gridView Fallas
                            TSDK.ASP.Controles.InicializaIndices(gvFallas);
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método que valida las fechas de recepcion y fecha compromiso
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFechas()
        {
            //Creación del onjeto retorno
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Valida las fechas
            if ((Convert.ToDateTime(txtFechaRecepcion.Text).CompareTo(Convert.ToDateTime(txtFechaCompromiso.Text)) > 0))
                //Asigna al objeto retorno un mensaje de error
                retorno = new RetornoOperacion("Fecha de Recepcion debe ser MENOR que Fecha de Compromiso.");
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/OrdenTrabajo.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
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
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/OrdenTrabajo.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Orden de Trabajo", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Mantenimiento/OrdenTrabajo.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Orden de Trabajo", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotalesManoObra()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
            {
                //Mostrando Totales
                gvManoObra.FooterRow.Cells[5].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvManoObra.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotalesRefaccionesConsumidas()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4")))
            {
                //Mostrando Totales
                gvRefaccionesConsumidas.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Precio)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvRefaccionesConsumidas.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
            }
        }

        #region Métodos "Facturas Ligadas"

        /// <summary>
        /// Método encargado de Cargar las Facturas Ligadas a la Orden de Trabajo
        /// </summary>
        private void cargaFacturasProveedorOT()
        {
            //Validando Estatus
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvFacturasLigadas);

                        //Eliminando Tabla de Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Obtiene Facturas Ligadas
                        using (DataTable dtFacturasLigadas = SAT_CL.Mantenimiento.OrdenTrabajo.ObtieneFacturaProveedorOrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando Registros
                            if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                            {
                                //Cargando GridView
                                Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id", lblOrdenadoGrid.Text, true, 1);

                                //Añadiendo Tabla a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table1");
                            }
                            else
                            {
                                //Inicializando GridView
                                Controles.InicializaGridview(gvFacturasLigadas);

                                //Eliminando Tabla de Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private RetornoOperacion validaFacturaXML()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Limpiando Mensaje
            string mensaje = "";

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
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idProveedorEmisor = 0;

                            //Asignando Emisor
                            idProveedorEmisor = emi.id_compania_emisor_receptor;

                            //Instanciando Proveedor
                            result = new RetornoOperacion(idProveedorEmisor);

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

                                                //Validando que Existen Addendas
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
                                                    doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                //Obteniendo Validación
                                                bool value = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);

                                                //Instanciando Resultado
                                                result = new RetornoOperacion(mensaje, value);
                                            }
                                        }
                                    }
                                    else
                                        //Asignando Negativa el Objeto de retorno
                                        result = new RetornoOperacion("El RFC de la factura no coincide con el Receptor");
                                }
                            }
                            else
                                //Asignando Negativa el Objeto de retorno
                                result = new RetornoOperacion("El RFC de la factura no coincide con el Emisor");
                        }
                    }
                    catch (Exception e)
                    {
                        //Asignando Negativa el Objeto de retorno
                        result = new RetornoOperacion(e.Message);
                    }
                }
                else
                    //Mensaje de Error
                    result = new RetornoOperacion("No se ha podido cargar el Archivo");
            }
            else
                //Mensaje de Error
                result = new RetornoOperacion("No se ha podido localizar el Archivo");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Guardar la Factura apartir de un XML
        /// </summary>
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Orden de Trabajo
            using (SAT_CL.Mantenimiento.OrdenTrabajo ot = new SAT_CL.Mantenimiento.OrdenTrabajo(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existe la Orden
                if (ot.habilitar)
                {
                    //Declarando Variable para Factura
                    int idFactura = 0;

                    //Inicializando transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Validando XML
                        result = validaFacturaXML();

                        //Validando Resultado
                        if (result.OperacionExitosa)
                        {
                            try
                            {
                                //Declarando Documento XML
                                XmlDocument doc = (XmlDocument)Session["XML"];

                                //Declarando variables de Montos
                                decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                                //Obteniendo Valores
                                obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                                //Instanciando Emisor-Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que coincida el RFC del Emisor
                                    if (emisor.id_compania_emisor_receptor > 0)
                                    {
                                        //Validando que sea el mismo Proveedor
                                        if (emisor.id_compania_emisor_receptor == ot.id_compania_proveedor)
                                        {
                                            //Instanciando Emisor-Compania
                                            using (SAT_CL.Global.CompaniaEmisorReceptor receptor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                            {
                                                //Validando que coincida el RFC del Receptor
                                                if (receptor.id_compania_emisor_receptor > 0)
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
                                                    result = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                        0, doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
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

                                                    //Validando Operación
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Obteniendo Factura
                                                        idFactura = result.IdRegistro;

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
                                                            result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                                                    node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                                                    node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                                                    tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            //Incrementando Contador
                                                            contador++;
                                                            //Obteniendo resultado del Ciclo
                                                            res = contador >= xmlNL.Count ? false : result.OperacionExitosa;
                                                        }

                                                        //Validando resultado
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Declarando Variables Auxiliares
                                                            string ruta;
                                                            //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                                            ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());
                                                            //Insertamos Registro
                                                            result = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                                                Encoding.UTF8.GetBytes(doc.OuterXml), ruta);

                                                            //Validando Operación Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Insertando Relación de Factura Proveedor con el Deposito
                                                                result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 131, Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando Operación Exitosa
                                                                if (result.OperacionExitosa)

                                                                    //Completando Transacción
                                                                    trans.Complete();
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                    //Instanciando Excepcion
                                                    result = new RetornoOperacion("La Compania Receptora no esta registrada");
                                            }
                                        }
                                        else
                                            //Instanciando Excepcion
                                            result = new RetornoOperacion("El Proveedor no coincide con el de la Factura");
                                    }
                                    else
                                        //Instanciando Excepcion
                                        result = new RetornoOperacion("El Compania Proveedora no esta registrado");
                                }
                            }
                            catch (Exception e)
                            {
                                //Mostrando Excepción
                                result = new RetornoOperacion(e.Message);
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion("No Existe la Orden de Trabajo");
            }

            //Devolviendo Resultado Obtenido
            return result;
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
                //Asignando Valores
                tasa_imp_tras = Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value);
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }

        #endregion

        #endregion
    }
}