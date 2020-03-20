using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucCargoRecurrente : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Id de Tarifa
        /// </summary>
        private int _idTarifa;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Detalle de la Tarifa
        /// </summary>
        private int _idTarifaDetalle;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        private int _idCompania;
        /// <summary>
        /// DataSet con los registros a mostrar
        /// </summary>
        private DataTable _dt;

        /// <summary>
        /// Atributo Público encargado de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Valor
                ddlTipo.TabIndex =
                txtCantidad.TabIndex =
                ddlUnidad.TabIndex =
                txtValorUnitario.TabIndex =
                btnGuardar.TabIndex = 
                btnCancelar.TabIndex = 
                btnEliminar.TabIndex =
                ddlTamano.TabIndex =
                lnkExportar.TabIndex =
                gvResultado.TabIndex = value;
            }
            get { return ddlTipo.TabIndex; }
        }
        /// <summary>
        /// Atributo Público encargado de la Habilitación de los Controles
        /// </summary>
        public bool Enabled
        {
            set
            {   //Asignando Valor
                ddlTipo.Enabled =
                txtCantidad.Enabled =
                ddlUnidad.Enabled =
                txtValorUnitario.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled =
                btnEliminar.Enabled =
                ddlTamano.Enabled =
                lnkExportar.Enabled =
                gvResultado.Enabled = value;
            }
            get { return ddlTipo.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se Produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando  Método de Asignación
                asignaAtributos();
            else//Invocando  Método de Recuperación
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }

        #region Manejador de Eventos

        /// <summary>
        /// Evento "ClickSeleccionarRequerimiento"
        /// </summary>
        public event EventHandler ClickGuardarCargoRecurrente;
        /// <summary>
        /// Evento "ClickEliminarRequerimiento"
        /// </summary>
        public event EventHandler ClickEliminarCargoRecurrente;
        /// <summary>
        /// Método que manipula el Evento "ClickSeleccionarRequerimiento"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarCargoRecurrente(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickGuardarCargoRecurrente != null)
                //Inicializando Evento
                ClickGuardarCargoRecurrente(this, e);
        }
        /// <summary>
        /// Método que manipula el Evento "ClickEliminarRequerimiento"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickEliminarCargoRecurrente(EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickEliminarCargoRecurrente != null)
                //Inicializando Evento
                ClickEliminarCargoRecurrente(this, e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickGuardarCargoRecurrente != null)
                //Invocando al Manejador de Evento
                OnClickGuardarCargoRecurrente(e);
            return;
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {   //Validando que el Evento no este Vacio
            if (ClickEliminarCargoRecurrente != null)
                //Invocando al Manejador de Evento
                OnClickEliminarCargoRecurrente(e);
            return;
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Limpiando Controles
            limpiaControles();
        }

        #region Eventos GridView "Resultados"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._dt.DefaultView.Sort = lblCriterio.Text;
                //Cambiando Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvResultado, this._dt, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._dt.DefaultView.Sort = lblCriterio.Text;
                //Cambiando Indice de Página del GridView
                Controles.CambiaIndicePaginaGridView(gvResultado, this._dt, e.NewPageIndex, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultado_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
            {   //Asignando Expresion de Ordenamiento
                this._dt.DefaultView.Sort = lblCriterio.Text;
                //Cambiando Indice de Página del GridView
                lblCriterio.Text = Controles.CambiaSortExpressionGridView(gvResultado, this._dt, e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvResultado.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                Controles.ExportaContenidoGridView(this._dt, "Id");
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if(gvResultado.DataKeys.Count > 0)
            {   //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvResultado, sender, "lnk", false);
                //Instanciando Cargo Recurrente
                using (SAT_CL.Tarifas.CargoRecurrente cr = new SAT_CL.Tarifas.CargoRecurrente(Convert.ToInt32(gvResultado.SelectedDataKey["Id"])))
                {   //Validando que exista el Registro
                    if (cr.id_cargo_recurrente != 0)
                    {   //Asignando Valores
                        ddlTipo.SelectedValue = cr.id_tipo_cargo.ToString();
                        ddlUnidad.SelectedValue = cr.id_unidad.ToString();
                        txtCantidad.Text = cr.cantidad.ToString();
                        txtValorUnitario.Text = cr.valor_unitario.ToString();
                    }
                    else
                    {   //Mostrando Error
                        lblError.Text = "No se pudo acceder al registro seleccionado, probablemente no existe";
                        //Quitando Selección
                        gvResultado.SelectedIndex = -1;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControl()
        {   //Cargando Catalogos
            limpiaControles();
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvResultado);
            //Cargando Catalogo de Registros
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            //Obteniendo Cargos
            cargaCargosRecurrentes();
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   //Asignando Valores
            ViewState["idTarifa"] = this._idTarifa;
            ViewState["idTarifaDetalle"] = this._idTarifaDetalle;
            ViewState["idCompania"] = this._idCompania;
            ViewState["DT"] = this._dt;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Valores
            if (Convert.ToInt32(ViewState["idTarifa"]) != 0)
                this._idTarifa = Convert.ToInt32(ViewState["idTarifa"]);
            if (Convert.ToInt32(ViewState["idTarifaDetalle"]) != 0)
                this._idTarifaDetalle = Convert.ToInt32(ViewState["idTarifaDetalle"]);
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                this._idCompania = Convert.ToInt32(ViewState["idCompania"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["DT"]))
                this._dt = (DataTable)ViewState["DT"];
        }
        /// <summary>
        /// Método Privado encargado de Obtener los Cargos Recurrentes de cada Detalle de Tarifa
        /// </summary>
        private void cargaCargosRecurrentes()
        {   
            //Instanciando Cargos
            using (DataTable dtCargos = SAT_CL.Tarifas.CargoRecurrente.ObtieneCargosRecurrentes(this._idTarifa, this._idTarifaDetalle))
            {   //Validando si existen los Cargos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCargos))
                {   //Cargando GridView
                    Controles.CargaGridView(gvResultado, dtCargos, "Id", "", true, 0);
                    //Asignando Tabla al Atributo
                    this._dt = dtCargos;
                }
                else
                {   //Inicializando GridView
                    Controles.InicializaGridview(gvResultado);
                    //Eliminando Tabla del Atributo
                    this._dt = null;
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControles()
        {   //Inicializando Indices
            Controles.InicializaIndices(gvResultado);
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 17, "", this._idCompania, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlUnidad, "", 25);
            //Limpiando Controles
            txtCantidad.Text =
            txtValorUnitario.Text = "";
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Inicializar el Control
        /// </summary>
        /// <param name="idTarifa">Id de Tarifa</param>
        /// <param name="idDetalle_tarifa">Id de Detalle de Tarifa</param>
        /// <param name="idCompania"></param>
        public void InicializaControlUsuario(int idTarifa, int idDetalleTarifa, int idCompania)
        {   //Asignando Id de registro
            this._idTarifa = idTarifa;
            this._idTarifaDetalle = idDetalleTarifa;
            this._idCompania = idCompania;
            //Inicializamos el control
            inicializaControl();
        }
        /// <summary>
        /// Metodo Público encargado de Guardar los Cambios sobre un Cargo Recurrente
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaCargoRecurrente()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista un Registro Seleccionado
            if (gvResultado.SelectedIndex != -1)
            {   //Instanciando Cargo Recurrente
                using (SAT_CL.Tarifas.CargoRecurrente cr = new SAT_CL.Tarifas.CargoRecurrente(Convert.ToInt32(gvResultado.SelectedDataKey["Id"])))
                {   //Validando que exista el Registro
                    if (cr.id_cargo_recurrente != 0)
                        //Editando Cargo
                        result = cr.EditaCargoRecurrente(this._idTarifa, this._idTarifaDetalle, Convert.ToInt32(ddlTipo.SelectedValue),
                                                          Convert.ToDecimal(txtCantidad.Text), Convert.ToByte(ddlUnidad.SelectedValue),
                                                          Convert.ToDecimal(txtValorUnitario.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            else
            {   //Validando que exista la Tarifa
                if (this._idTarifa != 0)
                    //Insertando Cargo
                    result = SAT_CL.Tarifas.CargoRecurrente.InsertaCargoRecurrente(this._idTarifa, this._idTarifaDetalle, Convert.ToInt32(ddlTipo.SelectedValue),
                                                                                    Convert.ToDecimal(txtCantidad.Text), Convert.ToByte(ddlUnidad.SelectedValue),
                                                                                    Convert.ToDecimal(txtValorUnitario.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else//Instanciando Excepcion
                    result = new RetornoOperacion("Debe existir la Tarifa");
            }
            //Validando si la Operacion fue Exitosa
            if(result.OperacionExitosa)
            {   //Limpiando Controles
                limpiaControles();
                //Cargando Reporte
                cargaCargosRecurrentes();
            }
            //Mostrando Mensaje
            lblError.Text = result.Mensaje;
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Metodo Público encargado de Eliminar el Cargo Recurrente
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaCargoRecurrente()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista un Registro Seleccionado
            if (gvResultado.SelectedIndex != -1)
            {   //Instanciando Cargo Recurrente
                using (SAT_CL.Tarifas.CargoRecurrente cr = new SAT_CL.Tarifas.CargoRecurrente(Convert.ToInt32(gvResultado.SelectedDataKey["Id"])))
                {   //Validando que exista el Registro
                    if (cr.id_cargo_recurrente != 0)
                        //Editando Cargo
                        result = cr.DeshabilitaCargoRecurrente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            else//Instanciando Exception
                result = new RetornoOperacion("Debe Seleccionar un Cargo");
            //Validando si la Operacion fue Exitosa
            if (result.OperacionExitosa)
            {   //Limpiando Controles
                limpiaControles();
                //Cargando Reporte
                cargaCargosRecurrentes();
            }
            //Mostrando Mensaje
            lblError.Text = result.Mensaje;
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}