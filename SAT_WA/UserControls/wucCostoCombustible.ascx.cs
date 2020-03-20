using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Transactions;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Data;

namespace SAT.UserControls
{
    public partial class wucCostoCombustible : System.Web.UI.UserControl
    {

        #region Atributos
        /// <summary>
        /// DataSet con los registros a mostrar
        /// </summary>
        private DataTable _dt;

        private int _id_costo_combustible;
        /// <summary>
        /// Atributo que Guarda el Id del Costo del Combustible
        /// </summary>
        public int id_costo_combustible { get { return this._id_costo_combustible; } }
        private byte _id_tipo_combustible;
        /// <summary>
        /// Atributo que Guarda el Tipo de Combustible
        /// </summary>
        public byte id_tipo_combustible { get { return this._id_tipo_combustible; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo que Guarda la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo que Guarda el Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo que Guarda la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo que Guarda la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private decimal _costo_combustible;
        /// <summary>
        /// Atributo que Guarda el Costo del Combustible
        /// </summary>
        public decimal costo_combustible { get { return this._costo_combustible; } }
        private string _referencia;
        /// <summary>
        /// Atributo que Guarda la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que Guarda el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la pagina es cargada por primera vez.
            if (!Page.IsPostBack)
            {
                InicializaControl(this._id_registro, this._id_tipo_combustible);
            }
            //Invoca al método inicializa forma
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            //Validando que exista un Evento
            if (ClickGuardar != null)

                //Iniciando Manejador
                OnClickGuardar(e);
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            byte id_tipo_combustible;
            id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
            int id_costo_combustible;
            id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
            InicializaControl(id_costo_combustible, id_tipo_combustible);
        }


        #region Eventos GridView

        /// <summary>
        /// Evento desencadenado al cambiar el indice del Control "Ubicacion"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        protected void ddlUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaCostosRegistrados();
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            Controles.CambiaTamañoPaginaGridView(gvCostosRegistrados, mit, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento producido al cambiar el índice de página del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCostosRegistrados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            Controles.CambiaIndicePaginaGridView(gvCostosRegistrados, mit, e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCostosRegistrados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            lblOrdenarCostoCombustible.Text = Controles.CambiaSortExpressionGridView(gvCostosRegistrados, mit, e.SortExpression, true, 1);
        }

        /// <summary>
        /// Evento producido al dar click sobre algún botón de autorización pendiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditar_Click(object sender, EventArgs e)
        {
            //Si existen registraos en el Gridview
            if (gvCostosRegistrados.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvCostosRegistrados, sender, "lnk", false);
                //Creación del objeto retorno
                RetornoOperacion retorno = new RetornoOperacion();
                using (SAT_CL.EgresoServicio.CostoCombustible costoCombustible = new SAT_CL.EgresoServicio.CostoCombustible(Convert.ToInt32(gvCostosRegistrados.SelectedDataKey["Id"])))
                {
                    if (costoCombustible.id_costo_combustible != 0)
                    {
                        lblId.Text = costoCombustible.id_costo_combustible.ToString();
                        ddlTipoCombustible.SelectedValue = costoCombustible.id_tipo_combustible.ToString();
                        txtPrecioCombustible.Text = costoCombustible.costo_combustible.ToString();
                        txtReferencia.Text = costoCombustible.referencia.ToString();
                        txtFechaInicio.Text = costoCombustible.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                        txtFechaFin.Text = costoCombustible.fecha_fin.ToString("dd/MM/yyyy HH:mm");

                        //Validando tabla
                        switch (costoCombustible.id_tabla)
                        {
                            case 15:
                                {
                                    using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(costoCombustible.id_registro))
                                    {
                                        ddlUbicacion.SelectedValue = ubi.id_ubicacion.ToString();
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }

        protected void lkbEliminar_Click(object sender, EventArgs e)
        {
            //Si existen registraos en el Gridview
            if (gvCostosRegistrados.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvCostosRegistrados, sender, "lnk", false);
                //Creación del objeto retorno
                RetornoOperacion retorno = new RetornoOperacion();
                using (SAT_CL.EgresoServicio.CostoCombustible costoCombustible = new SAT_CL.EgresoServicio.CostoCombustible(Convert.ToInt32(gvCostosRegistrados.SelectedDataKey["Id"])))
                {
                    if (costoCombustible.id_costo_combustible > 0)
                    {
                        //Asignación de valores al objeto retorno, con los valores obtenidos del formaulario de la pagina PrecioCombustible
                        retorno = costoCombustible.DeshabilitaCostoCombustible(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        //Valida si la inserción a la base de datos se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //A la variable de session id_registro le asigna el valor insertado en la tabla CostoCombustible
                            Session["id_registro"] = retorno.IdRegistro;
                            //Invoca al método inicializa forma
                            byte id_tipo_combustible;
                            id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
                            int id_costo_combustible;
                            id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
                            InicializaControl(id_costo_combustible, id_tipo_combustible);
                        }
                        //Manda un mensaje dependiendo de la validación de la operación
                        ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }

                }
            }
        }

        #endregion

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardar;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardar(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardar != null)
                //Iniciando Evento
                ClickGuardar(this, e);
        }

        #endregion

        #region Metodos Publicos

        /// <summary>
        /// Inicializa el Control de Cuentas
        /// </summary>
        public void InicializaControl(int id_ubicacion, byte id_tipo_combustible)
        {

            this._id_registro = id_ubicacion;
            this._id_tipo_combustible = id_tipo_combustible;
            //Invoca al método cargaCatalogo
            cargaCatalogo();
            inicializaValores(this._id_registro, this._id_tipo_combustible);
            TSDK.ASP.Controles.InicializaGridview(gvCostosRegistrados);
            cargaCostosRegistrados();
        }
        #endregion

        #region Metodos Privados

        /// <summary>
        /// Método que inicializa los valores de los controles acorde a cada estatus de la página
        /// </summary>
        private void inicializaValores(int id_ubicacion, byte id_tipo_combustible)
        {
            if (id_ubicacion != 0)
            {
                ddlUbicacion.SelectedValue = id_ubicacion.ToString();
            }
            if (id_tipo_combustible != 0)
            {
                this._id_tipo_combustible = id_tipo_combustible;
                ddlTipoCombustible.SelectedValue = this._id_tipo_combustible.ToString();
            }
            DateTime fechafin = DateTime.Now.AddDays(31);
            txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = fechafin.ToString("dd/MM/yyyy HH:mm");
            txtPrecioCombustible.Text = "";
            txtReferencia.Text = "";
            lblError.Text = "";
            if (lblId.Text != "Por Asignar")
            {
                lblId.Text = "Por Asignar";
            }
        }

        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogo()
        {
            //Cargando catálogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCombustible, "", 49);
            //Controles.InicializaDropDownList(ddlUbicacion, "No tiene ubicaciones configuradas");
            //Obteniendo Ubicaciones de Combustible
            using (DataTable dtUbicaciones = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(20, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                if (Validacion.ValidaOrigenDatos(dtUbicaciones))

                    Controles.CargaDropDownList(ddlUbicacion, dtUbicaciones, "id", "descripcion");
                else
                    Controles.InicializaDropDownList(ddlUbicacion, "No tiene ubicaciones configuradas");
            }
        }

        ///// <summary>
        ///// Método que permite la actualizacion de los compos de la tabla Costo combustible, con los datos obtenidos del formulario PrecioCombustible
        ///// </summary>
        //private void editarPrecioCombustible()
        //{
        //    //Creación del objeto retorno
        //    RetornoOperacion retorno = new RetornoOperacion();
        //}

        /// <summary>
        /// Método que permite la actualizacion de los compos de la tabla Costo combustible, con los datos obtenidos del formulario PrecioCombustible
        /// </summary>
        //public RetornoOperacion guardarPrecioCombustible()
        //{
        //    //Declarando Objeto de Retorno
        //    RetornoOperacion retorno = new RetornoOperacion();
        //    //Inicializando el Bloque Transaccional 
        //    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
        //    {
        //        int id_costo;

        //        //Validando que no exista un Registro por editar
        //        if (lblId.Text != "Por Asignar")
        //        {
        //            //Invoca a la clase CostoCombustible para inicializar la busqueda de registros y poderlos editar
        //            using (SAT_CL.EgresoServicio.CostoCombustible cc = new SAT_CL.EgresoServicio.CostoCombustible(Convert.ToInt32(lblId.Text)))
        //            {
        //                if (cc.id_costo_combustible > 0)
        //                {
        //                    id_costo = cc.id_costo_combustible;
        //                    //Asignación de valores obtenidos del fromulario PrecioCombustible al objeto retorno, para la actualizacion del registro de la tabla CostoCombustible
        //                    retorno = cc.EditaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), 15, Convert.ToInt32(ddlUbicacion.SelectedValue), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        //                }
        //            }
        //            //Validación de la actualizacion de datos sobre el registro
        //            if (retorno.OperacionExitosa)
        //            {
        //                //Asignación a la variable de session id_registro  el valor del identificador generado en la base de datos, en la tabla CostoCombustible;
        //                //Session["id_registro"] = retorno.IdRegistro;
        //                retorno = new RetornoOperacion(id_costo);
        //                //Invoca al método inicializaForma
        //                byte id_tipo_combustible;
        //                id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
        //                int id_costo_combustible;
        //                id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
        //                InicializaControl(id_costo_combustible, id_tipo_combustible);
        //            }
        //            //Muestra un mensaje validando si se realizo la operación correctamente o no.
        //            lblError.Text = retorno.Mensaje;
        //        }
        //        else
        //        {
        //            //Invoca al método validaFecha  y asigna el resultado del método al objeto retorno.
        //            retorno = validaFechas();
        //            //Valida si el resultado del método se realizo correctamente (La validación de las Fechas)
        //            if (retorno.OperacionExitosa)
        //            {
        //                //Asignación de valores al objeto retorno, con los valores obtenidos del formaulario de la pagina PrecioCombustible
        //                retorno = SAT_CL.EgresoServicio.CostoCombustible.InsertaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), 15,
        //                                                                   Convert.ToInt32(ddlUbicacion.SelectedValue),
        //                                                                  Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text),
        //                                                                  Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        //                //Valida si la inserción a la base de datos se realizo correctamente
        //                if (retorno.OperacionExitosa)
        //                {
        //                    //A la variable de session id_registro le asigna el valor insertado en la tabla CostoCombustible
        //                    Session["id_registro"] = retorno.IdRegistro;
        //                    //Invoca al método inicializa forma
        //                    byte id_tipo_combustible;
        //                    id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
        //                    int id_costo_combustible;
        //                    id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
        //                    InicializaControl(id_costo_combustible, id_tipo_combustible);
        //                }
        //            }
        //            //Manda un mensaje dependiendo de la validación de la operación
        //            retorno = new RetornoOperacion("No se puede Acceder al Kilometraje");
        //        }
        //    }
        //}


        //private void guardarPrecioCombustible()
        //{
        //    //Creación del objeto retorno
        //    RetornoOperacion retorno = new RetornoOperacion();

        //}

        public RetornoOperacion guardarPrecioCombustible()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando el Bloque Transaccional 
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Declarando Variable Auxiliar
                int idCosto = 0;
                //this.id_costo_combustible = Convert.ToInt32(lblId.Text);
                //Validando que Existe el Kilometraje
                if (lblId.Text != "Por Asignar")
                {
                    //Instanciando Kilometraje
                    using (SAT_CL.EgresoServicio.CostoCombustible cc = new SAT_CL.EgresoServicio.CostoCombustible(Convert.ToInt32(lblId.Text)))
                    {
                        //Validando que exista el Kilometraje
                        if (cc.id_costo_combustible > 0)
                        {
                            //Editando Kilometraje
                            result = cc.EditaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), 15, Convert.ToInt32(ddlUbicacion.SelectedValue), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Asignación a la variable de session id_registro  el valor del identificador generado en la base de datos, en la tabla CostoCombustible;
                                //Session["id_registro"] = retorno.IdRegistro;
                                result = new RetornoOperacion(idCosto);
                                //Invoca al método inicializaForma
                                byte id_tipo_combustible;
                                id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
                                int id_costo_combustible;
                                id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
                                InicializaControl(id_costo_combustible, id_tipo_combustible);
                                trans.Complete();
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede Acceder al Costo Combustible");
                    }
                }
                else
                {
                    //Insertando Kilometraje
                    result = SAT_CL.EgresoServicio.CostoCombustible.InsertaCostoCombustible(Convert.ToByte(ddlTipoCombustible.SelectedValue), 15,
                                                                       Convert.ToInt32(ddlUbicacion.SelectedValue),
                                                                      Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text),
                                                                      Convert.ToDecimal(txtPrecioCombustible.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Insertando Kilometraje
                    if (result.OperacionExitosa)
                    {
                        //Guardando 1er kilometraje
                        idCosto = result.IdRegistro;
                        //A la variable de session id_registro le asigna el valor insertado en la tabla CostoCombustible
                        //Session["id_registro"] = retorno.IdRegistro;
                        //Invoca al método inicializa forma
                        byte id_tipo_combustible;
                        id_tipo_combustible = Convert.ToByte(ddlTipoCombustible.SelectedValue);
                        int id_costo_combustible;
                        id_costo_combustible = Convert.ToInt32(ddlUbicacion.SelectedValue);
                        InicializaControl(id_costo_combustible, id_tipo_combustible);
                        trans.Complete();
                    }
                }
            }
            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método que valida la fecha de inicio sea menor a la Fecha Fin
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFechas()
        {
            //Creación del objeto retorno con valor 1 al constructor de la clase
            RetornoOperacion retorno = new RetornoOperacion(1);
            //Compara los datos encontrados en los controles de fecha inicio y fecha fin(si la fechaInicio es menor a fechaFin y el resultado de la comparacion es a 0)
            if (Convert.ToDateTime(txtFechaInicio.Text).CompareTo(Convert.ToDateTime(txtFechaFin.Text)) > 0)
            {
                //  Al objeto retorno se le asigna un mensaje de error en la validación de las fechas.
                retorno = new RetornoOperacion(" Fecha Inicio debe ser MENOR que Fecha Fin.");
            }
            //Retorna el resultado al método 
            return retorno;
        }

        #region Metodos GridView

        /// <summary>
        /// Realiza la carga los costos de combustibles registrados
        /// </summary>
        private void cargaCostosRegistrados()
        {
            //Definiendo origen de datos
            using (DataTable mit = SAT_CL.EgresoServicio.CostoCombustible.ObtieneCostoCombustibleRegistro(15, Convert.ToInt32(ddlUbicacion.SelectedValue)))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los detalles pendientes en el GridView
                    Controles.CargaGridView(gvCostosRegistrados, mit, "Id-IdTabla-IdRegistro", lblOrdenarCostoCombustible.Text, true, 1);
                    //Controles.CargaGridView(gvValesDiesel, dtValesDiesel, "NoVale-Id-Unidad", "", true, 0);
                    //Almacenando en sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvCostosRegistrados);

                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion


        #endregion

    }
}