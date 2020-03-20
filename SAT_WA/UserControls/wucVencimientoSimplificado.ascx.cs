using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucVencimientoSimplificado : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_vencimiento;
        private int _id_tabla;
        private int _id_registro;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlTipoVencimiento.TabIndex =
                txtDescripcion.TabIndex =
                txtFecIni.TabIndex =
                txtFecFin.TabIndex =
                btnCancelar.TabIndex =
                btnGuardar.TabIndex = 
                btnTerminar.TabIndex = value;
            }
            get { return ddlTipoVencimiento.TabIndex; }
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
            //Validando que se Produjo un PostBack
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

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarVencimiento;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarVencimiento(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardarVencimiento != null)

                //Iniciando Evento
                ClickGuardarVencimiento(this, e);
        }
        /// <summary>
        /// Manejador de Evento "Terminar"
        /// </summary>
        public event EventHandler ClickTerminarVencimiento;
        /// <summary>
        /// Evento que Manipula el Manejador "Terminar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickTerminarVencimiento(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickTerminarVencimiento != null)

                //Iniciando Evento
                ClickTerminarVencimiento(this, e);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarVencimiento != null)

                //Iniciando Manejador
                OnClickGuardarVencimiento(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Inicializando control
            inicializaControles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickTerminarVencimiento != null)

                //Iniciando Manejador
                OnClickTerminarVencimiento(e);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        private void inicializaControles()
        {
            //Validando que exista el Vencimiento
            if (this._id_vencimiento > 0)
            {
                //Instanciando Vencimiento
                using (Vencimiento ven = new Vencimiento(this._id_vencimiento))
                {
                    //Validando que exista el Vencimiento
                    if (ven.habilitar)
                    {
                        //Configurando Entidad
                        configuraEntidad(ven.id_tabla, ven.id_registro);

                        //Catálogo de Tipo de Vencimientos
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoVencimiento, 48, "-- Seleccione un Tipo", ven.id_tabla == 76 ? 2 : 1, "", 0, "");

                        //Asignando Valores
                        //txtValorKm.Text = ven.valor_km.ToString();
                        ddlTipoVencimiento.SelectedValue = ven.id_tipo_vencimiento.ToString();
                        txtDescripcion.Text = ven.descripcion;
                        txtFecIni.Text = ven.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                        txtFecFin.Text = ven.fecha_fin == DateTime.MinValue ? Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm") : ven.fecha_fin.ToString("dd/MM/yyyy HH:mm");

                        //Configurando Controles
                        txtFecFin.Enabled = 
                        btnTerminar.Enabled = true;
                    }
                }
            }
            else if (this._id_tabla > 0 && this._id_registro > 0)
            {
                //Catálogo de Tipo de Vencimientos
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoVencimiento, 48, "-- Seleccione un Tipo", this._id_tabla == 76 ? 2 : 1, "", 0, "");

                //Configurando Entidad
                configuraEntidad(this._id_tabla, this._id_registro);
                
                //Asignando Valores
                //txtValorKm.Text = "0.00";
                txtDescripcion.Text = "";
                txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                txtFecFin.Text = "";

                //Configurando Controles
                txtFecFin.Enabled =
                btnTerminar.Enabled = false;
            }
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdVencimiento"] = this._id_vencimiento;
            ViewState["IdTabla"] = this._id_tabla;
            ViewState["IdRegistro"] = this._id_registro;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Exista un Vencimiento
            if (ViewState["IdVencimiento"] != null)
                //Asignando Vencimiento
                this._id_vencimiento = Convert.ToInt32(ViewState["IdVencimiento"]);

            //Validando que Exista Tabla de Origen para entidad
            if (ViewState["IdTabla"] != null)
                //Asignando Vencimiento
                this._id_tabla = Convert.ToInt32(ViewState["IdTabla"]);

            //Validando que Exista una entidad
            if (ViewState["IdRegistro"] != null)
                //Asignando Vencimiento
                this._id_registro = Convert.ToInt32(ViewState["IdRegistro"]);
        }
        /// <summary>
        /// Método encaragdo de Configurar la Entidad
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        private void configuraEntidad(int id_tabla, int id_registro)
        {
            //Inicializando Valores
            switch (id_tabla)
            {
                case 1:
                    {
                        //Configurando Unidad
                        lblEntidad.Text = "Servicio";

                        //Instanciando Operador
                        using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(id_registro))
                        {
                            //Validando que exista
                            if (serv.habilitar)

                                //Asignando Servicio
                                lblRegistro.Text = serv.no_servicio;
                            else
                                //Limpiando Control
                                lblRegistro.Text = "--";
                        }

                        //Configurando Control
                        //lblValorKm.Visible = 
                        //txtValorKm.Visible = false;
                        break;
                    }
                case 19:
                    {
                        //Configurando Unidad
                        lblEntidad.Text = "Unidad";

                        //Instanciando Operador
                        using (Unidad unidad = new Unidad(id_registro))
                        {
                            //Validando que exista
                            if (unidad.habilitar)

                                //Asignando Unidad
                                lblRegistro.Text = unidad.numero_unidad;
                            else
                                //Limpiando Control
                                lblRegistro.Text = "--";
                        }

                        //Configurando Control
                        //lblValorKm.Visible =
                        //txtValorKm.Visible = true;
                        break;
                    }
                case 25:
                    {
                        //Configurando Unidad
                        lblEntidad.Text = "Proveedor";

                        //Instanciando Proveedor
                        using (CompaniaEmisorReceptor cer = new CompaniaEmisorReceptor(id_registro))
                        {
                            //Validando que exista
                            if (cer.habilitar)

                                //Asignando Proveedor
                                lblRegistro.Text = cer.nombre;
                            else
                                //Limpiando Control
                                lblRegistro.Text = "--";
                        }

                        //Configurando Control
                        //lblValorKm.Visible =
                        //txtValorKm.Visible = false;
                        break;
                    }
                case 76:
                    {
                        //Configurando Unidad
                        lblEntidad.Text = "Operador";

                        //Instanciando Unidad
                        using (Operador operador = new Operador(id_registro))
                        {
                            //Validando que exista
                            if (operador.habilitar)

                                //Asignando Unidad
                                lblRegistro.Text = operador.nombre;
                            else
                                //Limpiando Control
                                lblRegistro.Text = "--";
                        }

                        //Configurando Control
                        //lblValorKm.Visible =
                        //txtValorKm.Visible = false;
                        break;
                    }
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encaragdo de Inicializar el Vencimiento
        /// </summary>
        /// <param name="id_vencimiento">Vencimiento Capturado</param>
        /// <param name="id_tabla">Tabla de la Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        public void InicializaVencimiento(int id_vencimiento, int id_tabla, int id_registro)
        {
            //Asignando Atributos
            this._id_vencimiento = id_vencimiento;
            this._id_tabla = id_tabla;
            this._id_registro = id_registro;

            //Invocando Método de Inicialización
            inicializaControles();
        }
        /// <summary>
        /// Método encargado de Guardar el Vencimiento
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaVencimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            DateTime fec_ini;
            DateTime.TryParse(txtFecIni.Text, out fec_ini);
            int id_tipo_vencimiento = 0;
            int id_prioridad = 0;
            //decimal valor_kms = txtValorKm.Visible ? Convert.ToDecimal(txtValorKm.Text) : 0.00M;

            //Validando que haya un Vencimiento
            if (this._id_vencimiento > 0)
            {
                //Instanciando Vencimiento
                using (Vencimiento vencimiento = new Vencimiento(this._id_vencimiento))
                {
                    //Validando que exista el Vencimiento
                    if (vencimiento.habilitar)
                    { 
                        //Validando Tipo
                        if (vencimiento.id_tipo_vencimiento != Convert.ToInt32(ddlTipoVencimiento.SelectedValue))
                        {
                            //Instanciando Tipo Vencimiento
                            using (TipoVencimiento tipo = new TipoVencimiento(Convert.ToInt32(ddlTipoVencimiento.SelectedValue)))
                            {
                                //Validando que exista el Tipo
                                if (tipo.habilitar)
                                {
                                    //Asignando Valores
                                    id_tipo_vencimiento = tipo.id_tipo_vencimiento;
                                    id_prioridad = tipo.id_prioridad;
                                }
                            }
                        }
                        else
                        {   
                            //Asignando Valores
                            id_tipo_vencimiento = vencimiento.id_tipo_vencimiento;
                            id_prioridad = vencimiento.id_prioridad;
                        }
                        
                        //Validando que exista el Tipo
                        if (id_tipo_vencimiento > 0 && id_prioridad > 0)
                        {
                            //Validando que este Activo
                            if (vencimiento.estatus == Vencimiento.Estatus.Activo)
                            
                                //Editando Vencimiento
                                result = vencimiento.EditaVencimiento(vencimiento.estatus, vencimiento.id_tabla, vencimiento.id_registro, id_prioridad,
                                                            id_tipo_vencimiento, txtDescripcion.Text, fec_ini, vencimiento.fecha_fin, 0,
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("El Vencimiento esta Terminado, Imposible su Edición");
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existe el Tipo de Vencimiento");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe el Vencimiento");
                }
            }
            //Validando que exista la Entidad y el Registro
            else if (this._id_tabla > 0 && this._id_registro > 0)
            {
                //Instanciando Tipo Vencimiento
                using (TipoVencimiento tipo = new TipoVencimiento(Convert.ToInt32(ddlTipoVencimiento.SelectedValue)))
                {
                    //Validando que exista el Tipo
                    if (tipo.habilitar)
                    {
                        //Asignando Valores
                        id_tipo_vencimiento = tipo.id_tipo_vencimiento;
                        id_prioridad = tipo.id_prioridad;
                    }

                    //Validando que exista el Tipo
                    if (id_tipo_vencimiento > 0 && id_prioridad > 0)
                    
                        //Insertando Vencimiento
                        result = Vencimiento.InsertaVencimiento(this._id_tabla, this._id_registro, id_prioridad, id_tipo_vencimiento,
                                        txtDescripcion.Text, fec_ini, DateTime.MinValue, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe el Tipo de Vencimiento");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existe la Entidad del Vencimiento");

            //Validando operación Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Atributos
                this._id_vencimiento = result.IdRegistro;
                this._id_tabla = 0;
                this._id_registro = 0;

                //Inicializando Controles
                inicializaControles();
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Terminar el Vencimiento
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion TerminaVencimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha de Fin
            DateTime fec_fin;
            DateTime.TryParse(txtFecFin.Text, out fec_fin);

            //Validando que Exista el Vencimiento
            if (this._id_vencimiento > 0)
            {
                //Instanciando Vencimiento
                using (SAT_CL.Global.Vencimiento ven = new SAT_CL.Global.Vencimiento(this._id_vencimiento))
                {
                    //Validando que exista el Vencimiento
                    if (ven.id_vencimiento > 0)
                        //Terminando Vencimiento
                        result = ven.TerminaVencimiento(fec_fin, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("El vencimiento no fue encontrado para su edición.");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existe un vencimiento que terminar.");

            //Validando que la Operación fue Exitosa
            if (result.OperacionExitosa)
            {
                //Guardando Id de registro actualizado
                this._id_vencimiento = result.IdRegistro;
                this._id_tabla = this._id_registro = 0;

                //Invocando Método de Inicialización
                inicializaControles();
            }

            //Devolviendo Objeto de Retorno
            return result;
        }

        #endregion
    }
}