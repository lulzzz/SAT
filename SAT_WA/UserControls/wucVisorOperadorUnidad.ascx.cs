using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucVisorOperadorUnidad : System.Web.UI.UserControl
    {
        #region Enumeraciones

        /// <summary>
        /// Tipos de búsqueda de recursos
        /// </summary>
        private enum TipoBusqueda
        {
            /// <summary>
            /// Búsqueda por Unidad
            /// </summary>
            /// 
            Unidad = 1,
            /// <summary>
            /// Búsqueda por Operador
            /// </summary>
            Operador = 2
        }

        #endregion

        #region Atributos
        
        private int _id_compania_emisor;
        private int _id_ubicacion_actual;
        private TipoBusqueda _tipo_busqueda;
        private DataTable _mitRecursos;

        /// <summary>
        /// Obtiene o establece el Indice de Tabulación del control de usuario
        /// </summary>
        public short TabIndex
        {
            get
            {
                return rdbUnUnidad.TabIndex;
            }
            set
            {
                rdbUnUnidad.TabIndex =
                    rdbOpOperador.TabIndex =
                    txtUnNumeroUnidad.TabIndex =
                    ddlUnTipoUnidad.TabIndex =
                    ddlUnEstatus.TabIndex =
                    chkUnNoPropio.TabIndex =
                    txtUnProveedor.TabIndex =
                    txtUnUbicacion.TabIndex =
                    txtOpNombre.TabIndex =
                    ddlOpEstatus.TabIndex =
                    txtOpUbicacion.TabIndex =
                    btnOUBuscar.TabIndex = value;
            }
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #region Atributos Búsqueda Operador

        private string _op_nombre;
        private byte _op_id_estatus;

        #endregion

        #region Atributos Búsqueda Unidad

        private string _un_numero;
        private byte _un_id_estatus;
        private int _un_id_tipo;
        private bool _un_no_propio;
        private int _un_id_compania_proveedor;

        #endregion

        #endregion

        #region Eventos

        /// <summary>
        /// Evento de carag del control de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si hay una recarga de página
            if (IsPostBack)
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
        /// Maneja el cambio de selección principal de búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbUnUnidad_CheckedChanged(object sender, EventArgs e)
        {
            //Si la búsqueda de se solicita por unidad
            if (rdbUnUnidad.Checked)
                InicializaControlUnidad(this._id_compania_emisor);
            //Si es por operador
            else
                InicializaControlOperador(this._id_compania_emisor);
        }
        /// <summary>
        /// maneja el click del botón Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOUBuscar_Click(object sender, EventArgs e)
        {
            //Realizando carga de recursos
            cargaRecursos();
        }
        /// <summary>
        /// Cambio de indeice de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOUResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvOUResultadosBusqueda, this._mitRecursos, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Enlace de datos de cada fila del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOUResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //TODO: Aplicando criterio de color de fila por estatus
        }
        /// <summary>
        /// Cambia el criterio de orden del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOUResultadosBusqueda_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvOUResultadosBusqueda.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitRecursos.DefaultView.Sort = lblOrdenadoddlTamanogvOUResultadosBusqueda.Text;
                //Cambiando Ordenamiento
                lblOrdenadoddlTamanogvOUResultadosBusqueda.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvOUResultadosBusqueda, this._mitRecursos, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Cambia el tamaño de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanogvOUResultadosBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvOUResultadosBusqueda, this._mitRecursos, Convert.ToInt32(ddlTamanogvOUResultadosBusqueda.SelectedValue), true, 1);
        }
        /// <summary>
        /// Exporta el contenido del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportargvOUResultadosBusqueda_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitRecursos, "");
        }
        /// <summary>
        /// Cambio en selección de propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkUnNoPropio_CheckedChanged(object sender, EventArgs e)
        {
            //Si se ha marcado (unidad no propia)
            if (chkUnNoPropio.Checked)
            {
                txtUnProveedor.Enabled = true;
                txtUnProveedor.Text = "";
                txtUnProveedor.Focus();
            }
            //Si es propia
            else
            {
                txtUnProveedor.Enabled = false;
                txtUnProveedor.Text = "Unidad Propia   ID:0";
                chkUnNoPropio.Focus();
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la inicialización de contenido de controles en base al tipo de búsqueda a realizar
        /// </summary>
        private void configuraControlesTipoBusqueda()
        { 
            //Cargando catálogos requeridos
            cargaCatalogos();

            //Determinando tipo de configuración
            switch (this._tipo_busqueda)
            { 
                case TipoBusqueda.Unidad:
                    txtUnNumeroUnidad.Text = this._un_numero != "" ? this._un_numero : "";
                    ddlUnTipoUnidad.SelectedValue = this._un_id_tipo != 0 ? this._un_id_tipo.ToString() : "0";
                    ddlUnEstatus.SelectedValue = this._un_id_estatus != 0 ? this._un_id_estatus.ToString() : "0";
                    chkUnNoPropio.Checked = this._un_no_propio;
                    //Si hay proveedor que asignar
                    using(SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(this._un_id_compania_proveedor))
                        txtUnProveedor.Text = proveedor.id_compania_emisor_receptor > 0 ? string.Format("{0}   ID:{1}", proveedor.nombre, proveedor.id_compania_emisor_receptor) : "Unidad Propia   ID:0";
                    //Si hay ubicación que asignar
                    using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(this._id_ubicacion_actual))
                        txtUnUbicacion.Text = ubicacion.id_ubicacion > 0 ? string.Format("{0}   ID:{1}", ubicacion.descripcion, ubicacion.id_ubicacion) : "";

                    //Controles no utilizados
                    txtOpNombre.Text = txtOpUbicacion.Text = "";
                    ddlOpEstatus.SelectedValue = "0";
                    break;
                case TipoBusqueda.Operador:
                    txtOpNombre.Text = this._op_nombre;
                    using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(this._id_ubicacion_actual))
                        txtOpUbicacion.Text = ubicacion.id_ubicacion > 0 ? string.Format("{0}   ID:{1}", ubicacion.descripcion, ubicacion.id_ubicacion) : "";
                    ddlOpEstatus.SelectedValue = this._op_id_estatus != 0 ? this._op_id_estatus.ToString() : "0";

                    //Controles no utilizados                    
                    txtUnNumeroUnidad.Text = "";
                    ddlUnTipoUnidad.SelectedValue = "0";
                    ddlUnEstatus.SelectedValue = "0";
                    chkUnNoPropio.Checked = false;
                    txtUnProveedor.Text = "Unidad Propia   ID:0";
                    txtUnUbicacion.Text = "";
                    break;
            }
        }
        /// <summary>
        /// Realiza la carga de los catalogos principales del control
        /// </summary>
        private void cargaCatalogos()
        { 
            //Estatus Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlUnEstatus, "Todos", 53);
            //Tipo Unidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnTipoUnidad, 24, "Todos", 0, "", 0, "");
            //Estatus Operador
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlOpEstatus, "Todos", 57);
            //Tamaño de página de gridview de resultados
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanogvOUResultadosBusqueda, "", 56);
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Guardando Atributos para recuperación posterior
            ViewState["_tipo_busqueda"] = this._tipo_busqueda;
            ViewState["_id_compania_emisor"] = this._id_compania_emisor;
            ViewState["_id_ubicacion_actual"] = Convert.ToInt32(Cadena.RegresaCadenaSeparada(this._tipo_busqueda == TipoBusqueda.Unidad?txtUnUbicacion.Text:txtOpUbicacion.Text, "ID:", 1));
            ViewState["_mitRecursos"] = this._mitRecursos;

            ViewState["_un_no_propio"] = chkUnNoPropio.Checked;
            ViewState["_un_id_compania_proveedor"] = this._tipo_busqueda == TipoBusqueda.Unidad ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnProveedor.Text, "ID:", 1)) : 0;
            ViewState["_un_id_tipo"] = Convert.ToInt32(ddlUnTipoUnidad.SelectedValue);
            ViewState["_un_id_estatus"] = Convert.ToByte(ddlUnEstatus.SelectedValue);
            ViewState["_un_numero"] = txtUnNumeroUnidad.Text;

            ViewState["_op_nombre"] = txtOpNombre.Text;
            ViewState["_op_id_estatus"] = Convert.ToByte(ddlOpEstatus.SelectedValue);
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["_tipo_busqueda"] != null 
                && ViewState["_id_compania_emisor"] != null
                && ViewState["_id_ubicacion_actual"] != null 
                && ViewState["_un_no_propio"] != null
                && ViewState["_un_id_compania_proveedor"] != null 
                && ViewState["_un_id_tipo"] != null
                && ViewState["_un_id_estatus"] != null 
                && ViewState["_un_numero"] != null
                && ViewState["_op_nombre"] != null
                && ViewState["_op_id_estatus"] != null)
            {
                this._tipo_busqueda = (TipoBusqueda)Convert.ToByte(ViewState["_tipo_busqueda"]);
                this._id_compania_emisor = (int)ViewState["_id_compania_emisor"];
                this._id_ubicacion_actual = (int)ViewState["_id_ubicacion_actual"];
                if (ViewState["_mitRecursos"] != null)
                this._mitRecursos = (DataTable)ViewState["_mitRecursos"];

                this._un_no_propio = (bool)ViewState["_un_no_propio"];
                this._un_id_compania_proveedor = (int)ViewState["_un_id_compania_proveedor"];
                this._un_id_tipo = (int)ViewState["_un_id_tipo"];
                this._un_id_estatus = (byte)ViewState["_un_id_estatus"];
                this._un_numero = ViewState["_un_numero"].ToString();

                this._op_nombre = ViewState["_op_nombre"].ToString();
                this._op_id_estatus = (byte)ViewState["_op_id_estatus"];
            }

        }
        /// <summary>
        /// Realiza la carga de los recursos coincidentes con los parametros indicados
        /// </summary>
        private void cargaRecursos()
        { 
            //Determinando que tipo de búsqueda se realizará
            switch (this._tipo_busqueda)
            {
                case TipoBusqueda.Unidad:
                    //Realizando carga de unidades
                    this._mitRecursos = SAT_CL.Global.Unidad.CargaReporteUnidades(this._id_compania_emisor, txtUnNumeroUnidad.Text, Convert.ToByte(ddlUnEstatus.SelectedValue),
                                                    Convert.ToInt32(ddlUnTipoUnidad.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnProveedor.Text, "ID:", 1)),
                                                    chkUnNoPropio.Checked, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnUbicacion.Text, "ID:", 1)));
                    break;
                case TipoBusqueda.Operador:
                    this._mitRecursos = SAT_CL.Global.Operador.CargaReporteOperadores(this._id_compania_emisor, txtOpNombre.Text, Convert.ToByte(ddlOpEstatus.SelectedValue),
                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOpUbicacion.Text, "ID:", 1)));
                    break;
            }

            //Llenando gridview
            TSDK.ASP.Controles.CargaGridView(gvOUResultadosBusqueda, this._mitRecursos, "Id", "", true, 1);

        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza la carga del contenido inicial del control de usuario, para realizar búsquedas de unidades
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        public void InicializaControlUnidad(int id_compania_emisor)
        {
            //Inicializando control
            InicializaControlUnidad(id_compania_emisor, "", 0, 0, false, 0, 0);
        }
        /// <summary>
        /// Realiza la carga del contenido inicial del control de usuario, para realizar búsquedas de unidades
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        /// <param name="numero_unidad">Número de Unidad</param>
        /// <param name="id_estatus">Id de Estatus</param>
        /// <param name="id_tipo_unidad">Id de Tipo de Unidad</param>
        /// <param name="bit_no_propio">Bit de Propiedad de la Unidad (True -> De Permisionario, de lo contrario False)</param>
        /// <param name="id_compania_proveedor">Id de Permisionario propietario (Sólo para unidades no propias)</param>
        /// <param name="id_ubicacion">Id de Ubicación a consultar</param>
        public void InicializaControlUnidad(int id_compania_emisor, string numero_unidad, byte id_estatus, int id_tipo_unidad, bool bit_no_propio, int id_compania_proveedor, int id_ubicacion)
        { 
            //Asignando valores a atributos de control a utilizar
            this._id_compania_emisor = id_compania_emisor;
            this._tipo_busqueda = TipoBusqueda.Unidad;
            this._un_numero = numero_unidad;
            this._un_id_tipo = id_tipo_unidad;
            this._un_id_estatus = id_estatus;
            this._un_no_propio = bit_no_propio;
            this._un_id_compania_proveedor = id_compania_proveedor;
            this._id_ubicacion_actual = id_ubicacion;

            //Asignando atributos no requeridos
            this._op_id_estatus = 0;
            this._op_nombre = "";            

            //Configurando acorde al tipo de búsqueda
            configuraControlesTipoBusqueda();

            //Inicializando gridview de resultados
            TSDK.ASP.Controles.InicializaGridview(gvOUResultadosBusqueda);
            this._mitRecursos = null;
            //Asignando vista activa
            mtvBusquedaOU.SetActiveView(vwBusquedaUnidad);
        }
        /// <summary>
        /// Realiza la carga del contenido inicial del control de usuario, para realizar búsquedas de operadores
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        public void InicializaControlOperador(int id_compania_emisor)
        {
            InicializaControlOperador(id_compania_emisor, "", 0, 0);
        }
        /// <summary>
        /// Realiza la carga del contenido inicial del control de usuario, para realizar búsquedas de operadores
        /// </summary>
        /// <param name="id_compania_emisor">Id de Compañía Emisor</param>
        /// <param name="nombre">Nombre del Operador</param>
        /// <param name="id_estatus">Id de Estatus</param>
        /// <param name="id_ubicacion">Id de Ubicación a consultar</param>
        public void InicializaControlOperador(int id_compania_emisor, string nombre, byte id_estatus, int id_ubicacion)
        {
            //Asignando valores a atributos de control a utilizar
            this._id_compania_emisor = id_compania_emisor;
            this._tipo_busqueda = TipoBusqueda.Operador;
            this._op_id_estatus = id_estatus;
            this._op_nombre = nombre;
            this._id_ubicacion_actual = id_ubicacion;
            
            //Asignando atributos no requeridos            
            this._un_numero = "";
            this._un_id_tipo = 0;
            this._un_id_estatus = 0;
            this._un_no_propio = false;
            this._un_id_compania_proveedor = 0;

            //Configurando acorde al tipo de búsqueda
            configuraControlesTipoBusqueda();

            //Inicializando gridview de resultados
            TSDK.ASP.Controles.InicializaGridview(gvOUResultadosBusqueda);
            this._mitRecursos = null;
            //Asignando vista activa
            mtvBusquedaOU.SetActiveView(vwBusquedaOperador);
        }

        #endregion                       
        
    }
}