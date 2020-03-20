using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucCobroRecurrente : System.Web.UI.UserControl
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Producirse una Actualización de la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produzca un PostBack
            if (!Page.IsPostBack)
            {
                //Asignando Atributos
                asignaAtributos();
                //Invocando Método de Carga
                cargaCatalogos();
            }
            else
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarCobroRecurrente != null)
                //Iniciando Manejador
                OnClickGuardarCobroRecurrente(e);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelarCobroRecurrente != null)
                //Iniciando Manejador
                OnClickCancelarCobroRecurrente(e);
        }

        #endregion

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarCobroRecurrente;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarCobroRecurrente(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickGuardarCobroRecurrente != null)
                
                //Iniciando Evento
                ClickGuardarCobroRecurrente(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Cancelar"
        /// </summary>
        public event EventHandler ClickCancelarCobroRecurrente;
        /// <summary>
        /// Evento que Manipula el Manejador "Cancelar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelarCobroRecurrente(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCancelarCobroRecurrente != null)

                //Iniciando Evento
                ClickCancelarCobroRecurrente(this, e);
        }

        #endregion

        #region Atributos

        private int _id_compania;
        private int _id_cobro_recurrente;
        private byte _id_tipo_entidad;
        private int _id_entidad;
        private decimal _total_deuda;
        private int _id_tabla;
        private int _id_registro;

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Asignar los Atributos del Control
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Atributos
            ViewState["IdCompania"] = this._id_compania;
            ViewState["IdCobroRecurrente"] = this._id_cobro_recurrente;
            ViewState["idTipoEntidad"] = this._id_tipo_entidad;
            ViewState["IdEntidad"] = this._id_entidad;
            ViewState["TotalDeuda"] = this._total_deuda;
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos del Control
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdCompania"]) != 0)
                this._id_compania = Convert.ToInt32(ViewState["IdCompania"]);
            if (Convert.ToInt32(ViewState["IdCobroRecurrente"]) != 0)
                this._id_cobro_recurrente = Convert.ToInt32(ViewState["IdCobroRecurrente"]);
            if (Convert.ToInt32(ViewState["idTipoEntidad"]) != 0)
                this._id_tipo_entidad = Convert.ToByte(ViewState["idTipoEntidad"]);
            if (Convert.ToInt32(ViewState["IdEntidad"]) != 0)
                this._id_entidad = Convert.ToInt32(ViewState["IdEntidad"]);
            if (Convert.ToInt32(ViewState["TotalDeuda"]) != 0)
                this._total_deuda = Convert.ToDecimal(ViewState["TotalDeuda"]);
            if (Convert.ToInt32(ViewState["IdTabla"]) != 0)
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);
            if (Convert.ToInt32(ViewState["IdRegistro"]) != 0)
                this._id_registro = Convert.ToInt32(ViewState["IdRegistro"]);
        }
        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        private void inicializaControlUsuario()
        {
            //Validando que no exista el Tipo de Cobro Recurrente
            if (_id_cobro_recurrente == 0)
            {
                //Asignando Tipo de Entidad
                ddlTipoEntApl.SelectedValue = _id_tipo_entidad.ToString();
                
                //Validando Tipo de Entidad
                switch (_id_tipo_entidad)
                {
                    case 1:
                        {
                            //Instanciando Unidad
                            using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(_id_entidad))
                            {
                                //Validando que exista la Unidad
                                if (unidad.habilitar)

                                    //Asignando Valor
                                    txtEntidad.Text = unidad.numero_unidad + " ID:" + unidad.id_unidad.ToString();
                            }
                            break;
                        }
                    case 2:
                        {
                            //Instanciando Operador
                            using (SAT_CL.Global.Operador operador = new SAT_CL.Global.Operador(_id_entidad))
                            {
                                //Validando que exista el Operador
                                if (operador.habilitar)

                                    //Asignando Valor
                                    txtEntidad.Text = operador.nombre + " ID:" + operador.id_operador.ToString();
                            }
                            break;
                        }
                    case 3:
                        {
                            //Instanciando Proveedor
                            using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(_id_entidad))
                            {
                                //Validando que exista el Proveedor
                                if (proveedor.habilitar)

                                    //Asignando Valor
                                    txtEntidad.Text = proveedor.nombre + " ID:" + proveedor.id_compania_emisor_receptor.ToString();
                            }
                            break;
                        }
                }

                //Mostrando Total de la Deuda
                txtTotalDeuda.Text = string.Format("{0:0.00}", _total_deuda);

                //Limpiando Controles
                txtDescuento.Text = "";

                //Validando Entidad
                switch (this._id_tabla)
                {
                    case 82:
                        {
                            //Instanciando Liquidación
                            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(this._id_registro))
                            {
                                //Validando que exista la Liquidación
                                if (liq.habilitar)

                                    //Asignando Fecha de Liquidación
                                    txtFecInicio.Text = liq.fecha_liquidacion.ToString("dd/MM/yyyy");
                                else
                                    //Asignando Fecha Actual
                                    txtFecInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                            }
                            break;
                        }
                    default:
                        {
                            //Asignando Fecha Actual
                            txtFecInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                            break;
                        }
                }                
            }
            else
            {
                //Instanciando Cobro Recurrente
                using (SAT_CL.Liquidacion.CobroRecurrente cobro = new SAT_CL.Liquidacion.CobroRecurrente(_id_cobro_recurrente))
                {
                    //Validando que exista el Cobro
                    if (cobro.habilitar)
                    {
                        //Validando Tipo de Entidad
                        switch (cobro.id_tipo_entidad_aplicacion)
                        {
                            case 1:
                                {
                                    //Instanciando Unidad
                                    using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(cobro.id_unidad))
                                    {
                                        //Validando que exista la Unidad
                                        if (unidad.habilitar)

                                            //Asignando Valor
                                            txtEntidad.Text = unidad.numero_unidad + " ID:" + unidad.id_unidad.ToString();
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    //Instanciando Operador
                                    using (SAT_CL.Global.Operador operador = new SAT_CL.Global.Operador(cobro.id_operador))
                                    {
                                        //Validando que exista el Operador
                                        if (operador.habilitar)

                                            //Asignando Valor
                                            txtEntidad.Text = operador.nombre + " ID:" + operador.id_operador.ToString();
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    //Instanciando Proveedor
                                    using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(cobro.id_proveedor_compania))
                                    {
                                        //Validando que exista el Proveedor
                                        if (proveedor.habilitar)

                                            //Asignando Valor
                                            txtEntidad.Text = proveedor.nombre + " ID:" + proveedor.id_compania_emisor_receptor.ToString();
                                    }
                                    break;
                                }
                        }

                        //Asignando Valores
                        this._id_compania = cobro.id_compania_emisor;
                        txtTotalDeuda.Text = string.Format("{0:0.00}", cobro.total_deuda);
                        txtDescuento.Text = string.Format("{0:0.00}", cobro.monto_cobro);
                        txtFecInicio.Text = cobro.fecha_inicial.ToString("dd/MM/yyyy");
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos del Control
        /// </summary>
        private void cargaCatalogos()
        {
            //Tipo de Entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEntApl, "", 62);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Cobro Recurrente
        /// </summary>
        /// <param name="id_compania">Compania</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_entidad">Entidad</param>
        /// <param name="total_deuda">Total de la Deuda</param>
        /// <param name="id_tabla">Entidad de Origen</param>
        /// <param name="id_registro">Registro de Origen</param>
        public void InicializaCobroRecurrente(int id_compania, byte id_tipo_entidad, int id_entidad, decimal total_deuda, int id_tabla, int id_registro)
        {
            //Asignando Valores
            this._id_cobro_recurrente = 0;
            this._id_compania = id_compania;
            this._id_entidad = id_entidad;
            this._id_tipo_entidad = id_tipo_entidad;
            this._total_deuda = total_deuda;
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;

            //Invocando Método de Carga
            inicializaControlUsuario();
        }
        /// <summary>
        /// Método encargado de Inicializar el Cobro Recurrente
        /// </summary>
        /// <param name="id_cobro_recurrente">Cobro Recurrente</param>
        public void InicializaCobroRecurrente(int id_cobro_recurrente)
        {
            //Asignando Valores
            this._id_cobro_recurrente = id_cobro_recurrente;
            this._id_entidad = 0;
            this._id_tipo_entidad = 0;
            this._total_deuda = 0.00M;
            this._id_tabla = 0;
            this._id_registro = 0;

            //Invocando Método de Carga
            inicializaControlUsuario();
        }
        /// <summary>
        /// Método encargado de Guardar el Cobro Recurrente
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaCobroRecurrente()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando el Cobro Recurrente
            if (this._id_cobro_recurrente == 0)
            {
                //Obteniendo Fecha
                DateTime fec_ini;
                DateTime.TryParse(txtFecInicio.Text, out fec_ini);

                //Obteniendo Entidad
                int id_unidad = this._id_tipo_entidad == 1 ? this._id_entidad : 0;
                int id_operador = this._id_tipo_entidad == 2 ? this._id_entidad : 0;
                int id_proveedor = this._id_tipo_entidad == 3 ? this._id_entidad : 0;

                //Declarando Variable Auxiliares
                string referencia = "";

                //Validando Entidad de Origen
                switch (this._id_tabla)
                {
                    case 82:
                        {
                            //Instanciando Liquidación
                            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(this._id_registro))
                            {
                                //Validando que exista la Liquidación
                                if (liq.habilitar)

                                    //Asignando Referencia Personalizada
                                    referencia = string.Format("Liquidación No. {0} con Saldo en Contra", liq.no_liquidacion);
                            }
                            break;
                        }
                }

                //Insertando Cobro Recurrente
                result = SAT_CL.Liquidacion.CobroRecurrente.InsertaCobroRecurrente(41, this._id_compania, this._total_deuda, 0, Convert.ToDecimal(txtDescuento.Text),
                            1, 1, this._id_tipo_entidad, id_unidad, id_operador, id_proveedor, 0, referencia, fec_ini, fec_ini, 1, this._id_tabla, this._id_registro,
                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}