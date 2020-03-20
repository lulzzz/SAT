using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucHistorialMovimiento : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_recurso_asignado;
        private int _id_tipo_asignacion;
        private DataSet _ds;
        private int _id_servicio;
        private int _id_movimiento;
        private int _id_devolucion;
        private string _comando_referencia;
        /// <summary>
        /// Atributo encargado de Obtener el Id del Servicio
        /// </summary>
        public int idServicio { get { return this._id_servicio; } }
        /// <summary>
        /// Atributo encargado de Obtener el Id del Movimiento
        /// </summary>
        public int idMovimiento { get { return this._id_movimiento; } }
        /// <summary>
        /// Atributo encargado de Obtener el Id de la Devolución
        /// </summary>
        public int idDevolucion { get { return this._id_devolucion; } }
        /// <summary>
        /// Atributo encargado de Obtener el Comando para la gestión de Referencias
        /// </summary>
        public string comandoReferencia { get { return this._comando_referencia; } }

        /// <summary>
        /// Propiedad encargada de Asignar el Orden de Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            //Asignando Valor de Habilitación
            set
            {
                ddlTipoAsignacion.TabIndex =
                txtRecursoAsignado.TabIndex =
                btnBuscar.TabIndex =
                ddlTamano.TabIndex =
                lnkExportar.TabIndex =
                gvHistorialAsignacion.TabIndex = value;
            }
            get { return ddlTipoAsignacion.TabIndex; }
        }
        /// <summary>
        /// Propiedad encargada de Habilitar lo Controles
        /// </summary>
        public bool Enabled
        {
            //Asignando Valor de Habilitación
            set
            {
                ddlTipoAsignacion.Enabled =
                txtRecursoAsignado.Enabled =
                btnBuscar.Enabled =
                ddlTamano.Enabled =
                lnkExportar.Enabled =
                lnkExportar.Enabled =
                gvHistorialAsignacion.Enabled = value;
            }
            get { return ddlTipoAsignacion.Enabled; }
        }

        #endregion

        #region Manejadores de Evento

        /// <summary>
        /// Declarando el Evento "Ver Referencia"
        /// </summary>
        public event EventHandler ClickVerReferencia;
        /// <summary>
        /// Método que manipula el Evento "Ver Referencia"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickVerReferencia(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickVerReferencia != null)
                //Invocando al Delegado
                ClickVerReferencia(this, e);
        }

        /// <summary>
        /// Declarando el Evento "Calcular KMS"
        /// </summary>
        public event EventHandler ClickCalcularKMS;
        /// <summary>
        /// Método que manipula el Evento "Calcular KMS"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCalcularKMS(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickCalcularKMS != null)
                //Invocando al Delegado
                ClickCalcularKMS(this, e);
        }

        /// <summary>
        /// Declarando el Evento "Depositos"
        /// </summary>
        public event EventHandler ClickDepositos;
        /// <summary>
        /// Método que manipula el Evento "Depositos"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickDepositos(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickDepositos != null)
                //Invocando al Delegado
                ClickDepositos(this, e);
        }

        /// <summary>
        /// Declarando el Evento "Diesel"
        /// </summary>
        public event EventHandler ClickDiesel;
        /// <summary>
        /// Método que manipula el Evento "Diesel"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickDiesel(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickDiesel != null)
                //Invocando al Delegado
                ClickDiesel(this, e);
        }

        /// <summary>
        /// Declarando el Evento "Devolución"
        /// </summary>
        public event EventHandler ClickDevolucion;
        /// <summary>
        /// Método que manipula el Evento "Devolución"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickDevolucion(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickDevolucion != null)
                //Invocando al Delegado
                ClickDevolucion(this, e);
        }
        /// <summary>
        /// Declarando el Evento "EncabezadoServicio"
        /// </summary>
        public event EventHandler ClickEncabezadoServicio;
        /// <summary>
        /// Método que manipula el Evento "Devolución"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEncabezadoServicio(EventArgs e)
        {   //Validando que este vacio el Evento
            if (ClickEncabezadoServicio != null)
                //Invocando al Delegado
                ClickEncabezadoServicio(this, e);
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscarHistorial();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializando Reporte
            TSDK.ASP.Controles.InicializaGridview(gvHistorialAsignacion);

            //Eliminando Tabla de DataSet
            this._ds = TSDK.Datos.OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
        }
        /// <summary>
        /// Evento que permite visualizar la bitacora de cada registro del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Valida que existan registros en el gridview Historial Asignación
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Selecciona la fila del gridview al cual se relizara la consulta de bitacora
                Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);
                //Invoca al método inicializaBitacora, mandando como parametros el numero de movimiento, el identificador de la tabla movimiento, y la descripción de la tabla.
                inicicalizaBitacora(gvHistorialAsignacion.SelectedDataKey["NoMov"].ToString(), "10", "Bitacora Historial Movimientos");
            }

        }
        #region Eventos GridView "Historial Asignaciones"

        /// <summary>
        /// Evento generado al dar enlazar datos del control Historial Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHistorialAsignacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Encontrando controles de interés de Servicio
                    using (LinkButton lkbNoServicio = (LinkButton)e.Row.FindControl("lkbNoServicio"))
                    {
                        //Si no existe el Servicio
                        if (lkbNoServicio.Text == "")
                        {
                            //Asignamos Valor Default
                            lkbNoServicio.Text = "Hacer Servicio";
                            //Habilitamos link
                            lkbNoServicio.Enabled = true;
                        }
                    }

                    //Encontrando controles de interés de Servicio
                    using (LinkButton lkbDevolucion = (LinkButton)e.Row.FindControl("lkbDevolucion"))
                    {
                        //Si no existe el Servicio
                        if (lkbDevolucion.Text == "")
                        {
                            //Asignamos Valor Default
                            lkbDevolucion.Text = "Sin Devoluciones";
                            //Habilitamos link
                            lkbDevolucion.Enabled = true;
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
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Cambiando Tamaño del la Página
                TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvHistorialAsignacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), Convert.ToInt32(ddlTamano.SelectedValue));

                //Sumando KMS
                sumaTotalKMS();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uplnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHistorialAsignacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

                //Asignando Expresión de Ordenamiento
                lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvHistorialAsignacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.SortExpression);

                //Sumando KMS
                sumaTotalKMS();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHistorialAsignacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

                //Cambiando Indice de Página
                TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvHistorialAsignacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.NewPageIndex);

                //Sumando KMS
                sumaTotalKMS();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferenciaServicio_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Asignando Servicio
                this._id_servicio = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]);

                //Asignando Comando
                this._comando_referencia = "Servicio";

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvHistorialAsignacion);

                //Validando que este vacio el Evento
                if (ClickVerReferencia != null)
                    
                    //Inicializando Manejador de Evento
                    OnClickVerReferencia(e);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLtsDespuesCargar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Asignando Servicio
                this._id_movimiento = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"]);

                //Asignando Comando
                this._comando_referencia = "Servicio";

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvHistorialAsignacion);

                //Validando que este vacio el Evento
                if (ClickVerReferencia != null)

                    //Inicializando Manejador de Evento
                    OnClickVerReferencia(e);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbKms_Click(object sender, EventArgs e)
        {
            //Validamos que existan Movimientos
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;
                
                //Seleccionando la fila actual
                TSDK.ASP.Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Validando que este vacio el Evento
                if (ClickCalcularKMS != null)

                    //Inicializando Manejador de Evento
                    OnClickCalcularKMS(e);
                return;
            }
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaControl()
        {
            //Cargando Catalogos
            cargaCatalogo();

            //Marcando Control
            chkIncluir.Checked = true;

            //Obteniendo Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            
            //Validando que Existen 
            if (this._id_recurso_asignado > 0 && this._id_tipo_asignacion > 0)
            {
                //Asignando Tipo al Control
                ddlTipoAsignacion.SelectedValue = this._id_tipo_asignacion.ToString();
                
                //Validando el Tipo de Asignación
                switch(this._id_tipo_asignacion)
                {
                    //Unidad
                    case 1:
                        {
                            //Instanciando Unidad
                            using(SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(this._id_recurso_asignado))
                            {
                                //Validando que exista la Unidad
                                if (uni.id_unidad > 0)
                                {
                                    //Asignando Recurso Asignado
                                    txtRecursoAsignado.Text = uni.numero_unidad + " ID:" + uni.id_unidad.ToString();
                                }
                                else
                                    //Limpiando Control
                                    txtRecursoAsignado.Text = "";
                            }

                            break;
                        }
                    //Operador
                    case 2:
                        {
                            //Instanciando Operador
                            using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(this._id_recurso_asignado))
                            {
                                //Validando que exista el Operador
                                if (op.id_operador > 0)
                                {
                                    //Asignando Recurso Asignado
                                    txtRecursoAsignado.Text = op.nombre + " ID:" + op.id_operador.ToString();
                                }
                                else
                                    //Limpiando Control
                                    txtRecursoAsignado.Text = "";
                            }

                            break;
                        }
                }
            }

            //Invocando Método de Busqueda
            buscarHistorial();
        }
        /// <summary>
        /// Método encargado de Cargar el Catalogo
        /// </summary>
        private void cargaCatalogo()
        {
            //Cargando Tipos de Asignación
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAsignacion, "", 1117);
            //Cargando Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdRecursoAsignado"] = this._id_recurso_asignado;
            ViewState["IdTipoAsignacion"] = this._id_tipo_asignacion;
            ViewState["DS"] = this._ds;
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["IdMovimiento"] = this._id_movimiento;
            ViewState["IdDevolucion"] = this._id_devolucion;
            ViewState["Referencia"] = this._comando_referencia;
        }
        /// <summary>
        /// Método encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributo "Recurso Asignado"
            if (Convert.ToInt32(ViewState["IdRecursoAsignado"]) != 0)
                this._id_recurso_asignado = Convert.ToInt32(ViewState["IdRecursoAsignado"]);

            //Recuperando Atributo "Tipo Asignación"
            if (Convert.ToInt32(ViewState["IdTipoAsignacion"]) != 0)
                this._id_tipo_asignacion = Convert.ToInt32(ViewState["IdTipoAsignacion"]);

            //Recuperando Atributo "Servicio"
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);

            //Recuperando Atributo "Movimiento"
            if (Convert.ToInt32(ViewState["IdMovimiento"]) != 0)
                this._id_movimiento = Convert.ToInt32(ViewState["IdMovimiento"]);

            //Recuperando Atributo "Devolución"
            if (Convert.ToInt32(ViewState["IdDevolucion"]) != 0)
                this._id_devolucion = Convert.ToInt32(ViewState["IdDevolucion"]);

            //Recuperando Atributo "Referencia"
            if (ViewState["Referencia"] != null)
                this._comando_referencia = ViewState["Referencia"].ToString();

            //Recuperando Atributo "DataSet"
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)ViewState["DS"], "Table"))
                this._ds = (DataSet)ViewState["DS"];
        }
        /// <summary>
        /// Método encargado de Buscar el Historial
        /// </summary>
        private void buscarHistorial()
        {
            //Validando que Existan las Fechas de Ingreso
            DateTime fec_ini = DateTime.MinValue;
            DateTime fec_fin = DateTime.MinValue;

            //Validando la Inclusión de Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas de los Controles
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }
            
            //Obteniendo Historial
            using(DataTable dtHistorial = SAT_CL.Despacho.Reporte.ObtieneHistorialMovimiento(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRecursoAsignado.Text, "ID:", 1)),
                                                                    Convert.ToInt32(ddlTipoAsignacion.SelectedValue), fec_ini, fec_fin))
            {
                //Validando que Existen Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtHistorial))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvHistorialAsignacion, dtHistorial, "NoMov-IdServicio-IdDevolucion", "", true, 1);

                    //Añadiendo Tabla a DataSet
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dtHistorial, "Table");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvHistorialAsignacion);

                    //Eliminando Tabla de DataSet
                    this._ds = TSDK.Datos.OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
                }

                //Sumando KMS
                sumaTotalKMS();
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvHistorialAsignacion);
        }
        /// <summary>
        /// Método encargado de Sumar el Total de Kilometraje
        /// </summary>
        private void sumaTotalKMS()
        {
            //Validando que Existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._ds, "Table"))
            {
                //Mostrando Total de Kilometraje
                gvHistorialAsignacion.FooterRow.Cells[14].Text = 
                lblKms.Text = string.Format("{0:0.00}", this._ds.Tables["Table"].Compute("SUM(Kms)", ""));
                gvHistorialAsignacion.FooterRow.Cells[19].Text = string.Format("{0:C2}", this._ds.Tables["Table"].Compute("SUM(TotalDepositos)", ""));
                gvHistorialAsignacion.FooterRow.Cells[20].Text = string.Format("{0:0.00}", this._ds.Tables["Table"].Compute("SUM(TotalDieselTractor)", ""));
                gvHistorialAsignacion.FooterRow.Cells[21].Text = string.Format("{0:0.00}", this._ds.Tables["Table"].Compute("SUM(TotalDieselRemolque)", ""));
                //Validamos Que exista Kilometros Rrecorridos
                if (Convert.ToDecimal(this._ds.Tables["Table"].Compute("SUM(Kms)", "")) > 0 && Convert.ToDecimal(this._ds.Tables["Table"].Compute("SUM(TotalDieselTractor)", ""))>0)
                {
                    gvHistorialAsignacion.FooterRow.Cells[22].Text = "Rendimiento Tractor: " + string.Format("{0:0.00}", Convert.ToDecimal(this._ds.Tables["Table"].Compute("SUM(Kms)", "")) / Convert.ToDecimal(this._ds.Tables["Table"].Compute("SUM(TotalDieselTractor)", "")));
                }
                else
                {
                    gvHistorialAsignacion.FooterRow.Cells[22].Text = "Rendimiento Tractor: 0";
                }
            }
            else
            {
                //Mostrando Total de Kilometraje
                gvHistorialAsignacion.FooterRow.Cells[14].Text =
                lblKms.Text = string.Format("{0:0.00}", 0);
                gvHistorialAsignacion.FooterRow.Cells[18].Text = string.Format("{0:C2}", 0);
                gvHistorialAsignacion.FooterRow.Cells[19].Text = string.Format("{0:0.00}", 0);
                gvHistorialAsignacion.FooterRow.Cells[20].Text = string.Format("{0:0.00}", 0);
                gvHistorialAsignacion.FooterRow.Cells[21].Text = string.Format("{0:0.00}", 0);
            }
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        /// <param name="id_recurso">Recurso Asignado</param>
        /// <param name="id_tipo">Tipo de Asignación</param>
        public void InicializaControlUsuario(int id_recurso, int id_tipo)
        {
            //Asignando Atributos
            this._id_recurso_asignado = id_recurso;
            this._id_tipo_asignacion = id_tipo;

            //Inicializando Control
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Recargar la Busqueda del Historial
        /// </summary>
        public void BuscaHistorialMovimiento()
        {
            //Invocando Método de Busqueda
            buscarHistorial();
        }
        /// <summary>
        /// Método encargado de Calcular el Kilometraje
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion CalculaKMS()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciamos nuestro movimiento 
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"])))
            {
                //Validamos que el movimiento tenga un id de servicio ligado 
                if (objMovimiento.id_servicio != 0)
                {
                    //En caso de que el movimiento tenga un servicio ligado, instanciamos nuestro servicio
                    using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                    {
                        //Realizamos la actualizacion del kilometraje
                        retorno = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                {
                    //En caso de que el movimiento no tenga id de servicio ligado
                    //Invocamos el metodo de actualizacion de kilometraje del movimiento
                    retorno = Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }

            //Buscando Historial
            buscarHistorial();

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvHistorialAsignacion);

            //Devolviendo Objeto de Retorno
            return retorno;
        }

        /// <summary>
        /// Método que muestra las modificaciones realizadas a un registro (Bitacora Registro).
        /// </summary>
        /// <param name="idRegistro">Id que identifica un registro de la tabla Movimiento</param>
        /// <param name="idTabla">Id que identifica a la tabla Movimiento en la base de datos</param>
        /// <param name="Titulo">Nombre que se le asignara a la ventana de Bitacora</param>
        private void inicicalizaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Crea la variable url que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de Movimiento.
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucHistorialMovimiento.ascx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena las medidas de la ventana que contendra los datos de Bitacora de Movimiento.
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimenciones.
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora Historial Movimiento", configuracion, Page);
        }
        #endregion

        #region Hacer Servicio

        #region Eventos Hacer Servicio

        /// <summary>
        /// Evento generado al pulsar el botón Aceptar Hacer Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarHacerServicio_Click(object sender, EventArgs e)
        {
            //Evento generado al Hacer un Servicio un Movimiento en Vacio
            hacerServicio();
        }

        /// <summary>
        /// Evento generado al Cerrar el Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarHacerServicio_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(uplkbCerrarHacerServicio, uplkbCerrarHacerServicio.GetType(), "CerrarVentanaModalHacerServicio", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");
        }

        /// <summary>
        /// Evento generado al dar click en Hacer Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbHacerServicio_Click(object sender, EventArgs e)
        {
            //Validamos que existan Servicios
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Inccializamos Control
                txtClienteHacerServicio.Text = "";
                lblErrorHacerServicio.Text = "";
                
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upgvHistorialAsignacion, upgvHistorialAsignacion.GetType(), "AbreVentanaModal", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");
            }
        }

        #endregion

        #region Métodos Hacer Servicio

        /// <summary>
        /// Método encargado de Hacer un Servicio un Movimiento en Vacio
        /// </summary>
        private void hacerServicio()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"])))
            {
                //Hacemos Servicio el Movimiento
                resultado = objMovimiento.HacerMovimientoVacioaServicio(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteHacerServicio.Text, ":", 1)), 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Historial de Movimientos
                buscarHistorial();
                //Inicializamos Indices del Historial
                Controles.InicializaIndices(gvHistorialAsignacion);
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(btnAceptarHacerServicio, btnAceptarHacerServicio.GetType(), "AceptarVentanaModalHacerServicio", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");

            }
            //Mostramos Resultado
            lblErrorHacerServicio.Text = resultado.Mensaje;
        }

        #endregion

        
        #endregion

        #region Eventos Anticipos

        /// <summary>
        /// Evento generado al dar click en Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAnticipos_Click(object sender, EventArgs e)
        {
            //Validamos que existan de Asignaciones
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);
                //Determinando el tipo de acción a realizar
                switch (((LinkButton)sender).CommandName)
                {
                    case "Ruta":
                        {
                            //Inicializando contenido de control para el Calculo de Ruta
                            wucCalcularRuta.InicializaControl(Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]));
                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(gvHistorialAsignacion, "contenidoCalcularRuta", "contenidoCalcularRuta", "confirmacionCalcularRuta");
                            break;
                        }
                    case "DieselArrastre":
                    case "DieselMotriz":
                        {
                            //Asignando Atributos
                            this._id_servicio = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]);
                            this._id_movimiento = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"]);

                            //Validando que este vacio el Evento
                            if (ClickDiesel != null)

                                //Inicializando Manejador de Evento
                                OnClickDiesel(e);
                            return;
                        }
                    case "Deposito":
                        {
                            //Asignando Atributos
                            this._id_servicio = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]);
                            this._id_movimiento = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"]);

                            //Validando que este vacio el Evento
                            if (ClickDepositos != null)

                                //Inicializando Manejador de Evento
                                OnClickDepositos(e);
                            return;

                        }
                }
            }
        }

        /// <summary>
        /// Evento generado al dar click en Accion de Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionMovimiento_Click(object sender, EventArgs e)
        {
            //Validamos que existan de Asignaciones
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);
                //Determinando el tipo de acción a realizar
                switch (((LinkButton)sender).CommandName)
                {
                    case "Eliminar":
                        {
                            //Eliminamos Movimiento
                            eliminarMovimiento();
                          break;
                        }
                }
            }
        }

        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {

                case "calcularRuta":
                    //Cerramos Ventana Modal
                    //Registrando el script sólo para los paneles que producirán actualización del mismo
                    ScriptServer.AlternarVentana(lkbCerrarCalcularRuta, "contenidoCalcularRuta", "contenidoCalcularRuta", "confirmacionCalcularRuta");
                    break;


            }
        }
        #endregion

        #region Eventos Devoluciones

        /// <summary>
        /// Evento Producido al Seleccionar el Campo de Devolución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDevolucion_Click(object sender, EventArgs e)
        {
            //Validamos que existan de Asignaciones
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Asignando Atributos
                this._id_devolucion = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdDevolucion"]);
                this._id_servicio = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]);
                this._id_movimiento = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"]);

                //Validando que este vacio el Evento
                if (ClickDevolucion != null)

                    //Inicializando Manejador de Evento
                    OnClickDevolucion(e);
            }
        }

        #endregion

        #region Métodos Movimientos Vacio
        /// <summary>
        /// Elimina Movimiento
        /// </summary>
        private void eliminarMovimiento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["NoMov"])))
            {
                //Validamos Estatus del Movimiento
                if ((Movimiento.Estatus)objMovimiento.id_estatus_movimiento == Movimiento.Estatus.Terminado)
                {
                    //Deshabilitamos Movimiento Vacio Terminado
                    resultado = objMovimiento.DeshabilitaMovimientoVacioTerminado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                    resultado = objMovimiento.DeshabilitaMovimientoVacioIniciado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Buscando Historial
                buscarHistorial();

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvHistorialAsignacion);

            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(gvHistorialAsignacion, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion

        #region Eventos Encabezado de Servicio

        /// <summary>
        /// Evento generado al Dar click en Carta Porte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCartaPorte_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvHistorialAsignacion.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenado.Text;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvHistorialAsignacion, sender, "lnk", false);

                //Asignando Servicio
                this._id_servicio = Convert.ToInt32(gvHistorialAsignacion.SelectedDataKey["IdServicio"]);

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvHistorialAsignacion);
                //Validando que este vacio el Evento
                if (ClickEncabezadoServicio != null)

                    //Inicializando Manejador de Evento
                    OnClickEncabezadoServicio(e);
            }
        }

        #endregion
    }
}