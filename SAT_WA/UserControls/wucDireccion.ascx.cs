using SAT_CL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucDireccion : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_direccion;

        private DataTable dtBusqueda;
        /// <summary>
        /// Propiedad encargada del Orden de Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   ddlTipoUbicacion.TabIndex =
                txtCalle.TabIndex =
                txtNoInt.TabIndex =
                txtNoExt.TabIndex =
                txtColonia.TabIndex =
                txtLocalidad.TabIndex =
                txtReferencia.TabIndex =
                txtMunicipio.TabIndex =
                ddlPais.TabIndex =
                ddlIDSTA.TabIndex =
                txtCP.TabIndex =
                btnCancelar.TabIndex =
                btnBuscar.TabIndex =
                btnAceptar.TabIndex =
                ddlTamaño.TabIndex =
                lkbExportar.TabIndex =
                gvUbicaciones.TabIndex = value;
            }
            get { return ddlTipoUbicacion.TabIndex; }
        }
        /// <summary>
        /// Propiedad encargada de la Habilitación de los Controles
        /// </summary>
        public bool Enable
        {
            set
            {   ddlTipoUbicacion.Enabled =
                txtCalle.Enabled =
                txtNoInt.Enabled =
                txtNoExt.Enabled =
                txtColonia.Enabled =
                txtLocalidad.Enabled =
                txtReferencia.Enabled =
                txtMunicipio.Enabled =
                ddlPais.Enabled =
                ddlIDSTA.Enabled =
                txtCP.Enabled =
                btnAceptar.Enabled =
                btnBuscar.Enabled =
                btnCancelar.Enabled =
                ddlTamaño.Enabled =
                lkbExportar.Enabled =
                gvUbicaciones.Enabled = value;
            }
            get { return ddlTipoUbicacion.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Disparado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
                //Inicializando la Forma
                cargaCatalogos();
            else//Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento Disparado despues de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }

        #region Manejador de Eventos

        /// <summary>
        /// Manejador del Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarDireccion;
        /// <summary>
        /// Manejador del Evento "Eliminar"
        /// </summary>
        public event EventHandler ClickEliminarDireccion;
        /// <summary>
        /// Manejador del Evento "Seleccionar"
        /// </summary>
        public event EventHandler ClickSeleccionarDireccion;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarDireccion(EventArgs e)
        {   //Validando que no este vacio
            if (ClickGuardarDireccion != null)
                //Inicializando el Evento
                ClickGuardarDireccion(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Eliminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarDireccion(EventArgs e)
        {   //Validando que no este vacio
            if (ClickEliminarDireccion != null)
                //Inicializando el Evento
                ClickEliminarDireccion(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Seleccionar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickSeleccionarDireccion(EventArgs e)
        {   //Validando que no este vacio
            if (ClickSeleccionarDireccion != null)
                //Inicializando el Evento
                ClickSeleccionarDireccion(this, e);
        }

        #endregion

        /// <summary>
        /// Evento Disparado cuando se presiona el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {   //validando que exista el Evento
            if (ClickGuardarDireccion != null)
                //Inicializando Manejador
                OnClickGuardarDireccion(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   //Cargando Direcciones
            cargaDirecciones();
        }
        /// <summary>
        /// Evento Disparado cuando se presiona el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {   //Inicializa Indices
            Controles.InicializaIndices(gvUbicaciones);
            //Inicializando Atributo
            this._id_direccion = 0;
            //Limpia Controles
            limpiaValores();
        }
        /// <summary>
        /// Evento Disparado al Cambiar el Indice del Catalogo de Paises
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cargando Catalogo de Estado
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlIDSTA, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
        }

        #region Eventos GridView "Direcciones"

        /// <summary>
        /// Evento Disparado al cambiar el tamaño del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamaño_SelectedIndexChanged(object sender, EventArgs e)
        {   //Si se tienen registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {   //Asignando Orden
                this.dtBusqueda.DefaultView.Sort = lblOrden.Text;
                //Cambiando el tamaño
                TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvUbicaciones, this.dtBusqueda, Convert.ToInt32(ddlTamaño.SelectedValue), true, 1);
                //Número de Registros
                gvUbicaciones.FooterRow.Cells[1].Text = this.dtBusqueda.Rows.Count + " Registro(s) encontrado(s)";
            }
        }
        /// <summary>
        /// Evento Disparado al exportar a excel el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {   //Si se tienen registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {   //Exportando el grid
                TSDK.ASP.Controles.ExportaContenidoGridView(this.dtBusqueda, "*".ToCharArray());
            }
        }
        /// <summary>
        /// Evento Disparado al aplicar orden en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUbicaciones_Sorting(object sender, GridViewSortEventArgs e)
        {   //Si se tienen registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {   //Asignando Orden
                this.dtBusqueda.DefaultView.Sort = lblOrden.Text;
                //Cambiando la expresión
                lblOrden.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvUbicaciones, this.dtBusqueda, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento disparado al cambiar la página en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUbicaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Si se tienen registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {    //Asignando Orden
                this.dtBusqueda.DefaultView.Sort = lblOrden.Text;
                //Cambiando el indice de página
                TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvUbicaciones, this.dtBusqueda, e.NewPageIndex, true, 1);
            }
        }
        /// <summary>
        /// Evento que se Dispara cuando se presiona el Link de Editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditar_Click(object sender, EventArgs e)
        {   //Si se tienen registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {   //Selecionando la fila actual
                TSDK.ASP.Controles.SeleccionaFila(gvUbicaciones, sender, "lnk", false);
                //Instanciando Direccion
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(Convert.ToInt32(gvUbicaciones.SelectedDataKey["Id"])))
                {   //Validando que existe un Registro Valido
                    if (dir.id_direccion != 0)
                    {   //Asignando Valores
                        lblID.Text = dir.id_direccion.ToString();
                        ddlTipoUbicacion.SelectedValue = dir.id_tipo_direccion.ToString();
                        txtCalle.Text = dir.calle;
                        txtNoExt.Text = dir.no_exterior;
                        txtNoInt.Text = dir.no_interior;
                        txtColonia.Text = dir.colonia;
                        txtLocalidad.Text = dir.localidad;
                        txtReferencia.Text = dir.referencia;
                        txtMunicipio.Text = dir.municipio;
                        txtCP.Text = dir.codigo_postal;
                        ddlPais.SelectedValue = dir.id_pais.ToString();
                        CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlIDSTA, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
                        ddlIDSTA.SelectedValue = dir.id_estado.ToString();
                        //Mostrando Error
                        lblError.Text = "";
                    }
                    else
                    {   //Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvUbicaciones);
                        //Mostrando Error
                        lblError.Text = "No se encontro la Dirección en BD";
                    }
                }
            }
        }
        /// <summary>
        /// Evento que se Dispara cuando se presiona el Link Eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminar_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            Controles.SeleccionaFila(gvUbicaciones, sender, "lnk", false);
            //Validando que exista el Evento
            if (ClickEliminarDireccion != null)
                //Inicializando el manejador
                OnClickEliminarDireccion(e);
        }
        /// <summary>
        /// Evento que se Dispara cuando se presiona el Link Seleccionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionar_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            Controles.SeleccionaFila(gvUbicaciones, sender, "lnk", false);
            //Validando que exista el Evento
            if (ClickSeleccionarDireccion != null)
                //Inicializando el manejador
                OnClickSeleccionarDireccion(e);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargdo de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoUbicacion, "", 17);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPais, "", 15);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlIDSTA, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamaño, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Valores
        /// </summary>
        private void limpiaValores()
        {   //Asignando Valores
            lblID.Text = "Por Asignar";
            txtCalle.Text =
            txtNoExt.Text =
            txtNoInt.Text =
            txtColonia.Text =
            txtLocalidad.Text =
            txtReferencia.Text =
            txtMunicipio.Text =
            txtCP.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Seleccionar la Accion del Boton
        /// </summary>
        private void cargaDirecciones()
        {   //Obteniendo Direcciones segun los criterios de Busqueda
            using (DataTable dt = SAT_CL.Global.Direccion.ObtieneDirecciones(Convert.ToByte(ddlTipoUbicacion.SelectedValue),
                                            txtCalle.Text.ToUpper(), txtNoExt.Text.ToUpper(), txtNoInt.Text.ToUpper(), txtColonia.Text.ToUpper(),
                                            txtLocalidad.Text.ToUpper(), txtMunicipio.Text.ToUpper(), txtReferencia.Text.ToUpper(),
                                            Convert.ToInt32(ddlIDSTA.SelectedValue), Convert.ToInt32(ddlPais.SelectedValue),
                                            txtCP.Text.ToUpper()))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   //Añadiendo Tabla a Session
                    this.dtBusqueda = dt;
                    //Cargando Gridview
                    Controles.CargaGridView(gvUbicaciones, dt, "Id", "", true, 1);
                    //Inicializando Indices
                    Controles.InicializaIndices(gvUbicaciones);
                }
                else
                {   //Añadiendo Tabla a Session
                    this.dtBusqueda = null;
                    //Cargando Gridview
                    Controles.InicializaGridview(gvUbicaciones);
                    //Mensaje de Error
                    gvUbicaciones.FooterRow.Cells[1].Text = "No se encontraron registros";
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Asignar Valor a los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Atributos
            ViewState["idDireccion"] = this._id_direccion;
            ViewState["DT"] = this.dtBusqueda;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Validando si es una dirección Valida
            if (Convert.ToInt32(ViewState["idDireccion"]) != 0)
                //Asignando valor al Atributo
                this._id_direccion = Convert.ToInt32(ViewState["idDireccion"]);
            //Validando que existan registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                //Asignando valor al Atributo
                this.dtBusqueda = (DataTable)ViewState["DT"];
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control por Defecto
        /// </summary>
        public void InicializaControl()
        {   //Asignando Direccion
            this._id_direccion = 0;
            //Cargando Catalogos
            cargaCatalogos();
            //Limpiando valores
            limpiaValores();
            //Inicializando reporte
            Controles.InicializaGridview(gvUbicaciones);
        }
        /// <summary>
        /// Método Público encargado de Inicializar el Control dado un Id de Dirección
        /// </summary>
        /// <param name="id_direccion">Dirección</param>
        public void InicializaControl(int id_direccion)
        {   //Inicializando Control
            InicializaControl();
            //Asignando Atributo
            this._id_direccion = id_direccion;
            //Validando que sea una Direccion valida
            if (id_direccion != 0)
            {   //Instanciando Direccion
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(id_direccion))
                {   //Validando que existe un Registro Valido
                    if (dir.id_direccion != 0)
                    {   //Asignando Valores
                        lblID.Text = dir.id_direccion.ToString();
                        ddlTipoUbicacion.SelectedValue = dir.id_tipo_direccion.ToString();
                        txtCalle.Text = dir.calle;
                        txtNoExt.Text = dir.no_exterior;
                        txtNoInt.Text = dir.no_interior;
                        txtColonia.Text = dir.colonia;
                        txtLocalidad.Text = dir.localidad;
                        txtReferencia.Text = dir.referencia;
                        txtMunicipio.Text = dir.municipio;
                        txtCP.Text = dir.codigo_postal;
                        ddlPais.SelectedValue = dir.id_pais.ToString();
                        ddlIDSTA.SelectedValue = dir.id_estado.ToString();
                        //Mostrando Error
                        lblError.Text = "";
                    }
                    else//Limpiando Valores
                        limpiaValores();
                }
            }
        }
        /// <summary>
        /// Método Público encargado de Guardar la Dirección
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaDireccion()
        {   //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Evaluando Estatus
            if(gvUbicaciones.SelectedIndex != -1)
            {   //Instanciando Ubicación con los Valores Obtenidos
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(Convert.ToInt32(gvUbicaciones.SelectedDataKey["Id"])))
                {   //Validando que exista la Dirección
                    if (dir.id_direccion != 0)
                        //Obteniendo Resultado de la Edición
                        result = dir.EditaDireccion(Convert.ToByte(ddlTipoUbicacion.SelectedValue),
                                        txtCalle.Text.ToUpper(), txtNoExt.Text.ToUpper(), txtNoInt.Text.ToUpper(), txtColonia.Text.ToUpper(),
                                        txtLocalidad.Text.ToUpper(), txtMunicipio.Text.ToUpper(), txtReferencia.Text.ToUpper(),
                                        Convert.ToInt32(ddlIDSTA.SelectedValue), Convert.ToInt32(ddlPais.SelectedValue),
                                        txtCP.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else//Instanciando Exception
                        result = new RetornoOperacion("No se encontro la Dirección en BD");
                }
            }
            else
            {   //Validando que existe una Dirección Previa
                if (this._id_direccion != 0)
                {   //Instanciando Ubicación con los Valores Obtenidos
                    using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(this._id_direccion))
                    {   //Validando que exista la Dirección
                        if (dir.id_direccion != 0)
                            //Obteniendo Resultado de la Edición
                            result = dir.EditaDireccion(Convert.ToByte(ddlTipoUbicacion.SelectedValue),
                                            txtCalle.Text.ToUpper(), txtNoExt.Text.ToUpper(), txtNoInt.Text.ToUpper(), txtColonia.Text.ToUpper(),
                                            txtLocalidad.Text.ToUpper(), txtMunicipio.Text.ToUpper(), txtReferencia.Text.ToUpper(),
                                            Convert.ToInt32(ddlIDSTA.SelectedValue), Convert.ToInt32(ddlPais.SelectedValue),
                                            txtCP.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else//Instanciando Exception
                            result = new RetornoOperacion("No se encontro la Dirección en BD");
                    }
                }
                else//Obteniendo Resultado de la Inserción
                    result = SAT_CL.Global.Direccion.InsertaDireccion(Convert.ToByte(ddlTipoUbicacion.SelectedValue),
                                                        txtCalle.Text.ToUpper(), txtNoExt.Text.ToUpper(), txtNoInt.Text.ToUpper(), txtColonia.Text.ToUpper(),
                                                        txtLocalidad.Text.ToUpper(), txtMunicipio.Text.ToUpper(), txtReferencia.Text.ToUpper(),
                                                        Convert.ToInt32(ddlIDSTA.SelectedValue), Convert.ToInt32(ddlPais.SelectedValue),
                                                        txtCP.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Verificando si la Operación es Exitosa
            if (result.OperacionExitosa)
                //Inicializando Control
                InicializaControl();
            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);           
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Eliminar la Dirección
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaDireccion()
        {   //Declarando Variable de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista una Selección
            if (gvUbicaciones.SelectedIndex != -1)
            {   //Instanciando Ubicación con los Valores Obtenidos
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(Convert.ToInt32(gvUbicaciones.SelectedDataKey["Id"])))
                {   //Validando que exista una Dirección
                    if (dir.id_direccion != 0)
                        //Obteniendo Resultado de la Edición
                        result = dir.DeshabilitaDireccion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else//Instanciando Exception
                        result = new RetornoOperacion("No se encontro la Dirección en BD");
                }
            }
            else//Instanciando Exception
                result = new RetornoOperacion("Debe seleccionar una Dirección");
            //Validando que la operacion haya sido exitosa
            if (result.OperacionExitosa)
            {   //Inicializando Atributo
                this._id_direccion = 0;
                //Cargando Direcciones
                cargaDirecciones();
                //Limpiando Valores
                limpiaValores();
            }
            //Mostrando Mensaje
            lblError.Text = result.Mensaje;
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Seleccionar una Dirección
        /// </summary>
        /// <returns></returns>
        public int SeleccionaDireccion()
        {   //Declarando Objeto de Retorno
            int idDireccion = 0;
            //Validando que existan registros
            if (gvUbicaciones.DataKeys.Count > 0)
            {   //Instanciando Direccion
                using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(Convert.ToInt32(gvUbicaciones.SelectedDataKey["Id"])))
                {   //Validando que exista la Direccion
                    if (dir.id_direccion != 0)
                    {   //Inicializando Control
                        InicializaControl();
                        //Asignando Objeto de Retorno
                        idDireccion = dir.id_direccion;
                        //Mostrando Mensaje
                        lblError.Text = "La Dirección ha sido Seleccionada";
                    }
                    else//Mostrando Mensaje
                        lblError.Text = "No se encontro la Dirección en BD";
                    //Inicializando Control
                    Controles.InicializaIndices(gvUbicaciones);
                }
            }
            else//Mensaje de Error
                lblError.Text = "Deben de existir Registros";
            //Devolviendo Resultado Obtenido
            return idDireccion;
        }

        #endregion
    }
}