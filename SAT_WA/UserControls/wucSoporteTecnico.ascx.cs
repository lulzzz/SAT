using SAT_CL;
using SAT_CL.Soporte;
using System;
using TSDK.Base;
using System.Transactions;

namespace SAT.UserControls
{
    public partial class wucSoporteTecnico : System.Web.UI.UserControl
    {
        #region Atributos

       
        /// <summary>
        /// Atributo encargado de Almacenar el Objeto "Soporte"
        /// </summary>
        private SoporteTecnico objSoporte;
        /// <summary>
        /// Atributo Público encargado de Almacenar el Id de la Soporte
        /// </summary>
        public int idSoporte { get { return this.objSoporte.id_soporte_tecnico; } }
        private string mensaje_operacion;
        private string no_servicio;
        private int tipo;

        /// <summary>
        /// Propiedad TabIndex del Control de Usuario
        /// </summary>
        public short TabIndex
        {
            set
            {   //Encabezado
                ddlTipoSoporte.TabIndex =
                lblMensaje.TabIndex =
                txtFechaInicio.TabIndex =
                txtFechaFin.TabIndex =
                txtSolicita.TabIndex =
                txtObservacion.TabIndex =
                btnGuardarSoporte.TabIndex =
                btnCancelarSoporte.TabIndex = value;
            }
            get { return ddlTipoSoporte.TabIndex; }
        }
        /// <summary>
        /// Propiedad Enabled del Control de Usuario
        /// </summary>
        public bool Enabled
        {
            set
            {   //Controles
                ddlTipoSoporte.Enabled =
                //Botones
                btnGuardarSoporte.Enabled =
                btnCancelarSoporte.Enabled = value;
            }
            get { return ddlTipoSoporte.Enabled; }
        }

        #endregion

        #region Manejador de Eventos
        /// <summary>
        /// Evento producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarSoporte_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarSoporte != null)

                //Inicializando
                OnClickGuardarSoporte(e);
            return;
        }
        /// <summary>
        /// Declarando el Evento ClickGuardarSoporte
        /// </summary>
        public event EventHandler ClickGuardarSoporte;
        /// <summary>
        /// Método que manipula el Evento "Guardar Soporte"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarSoporte(EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickGuardarSoporte != null)

                //Invocando al Delegado
                ClickGuardarSoporte(this, e);
        }
        /// <summary>
        /// Evento producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarSoporte_Click(object sender, EventArgs e)
        {
            //Validando que este vacio el Evento
            if (ClickCancelarSoporte != null)

                //Inicializando
                OnClickCancelarSoporte(e);
            return;
        }
        /// <summary>
        /// Manejador del Evento "Cancelar"
        /// </summary>
        public event EventHandler ClickCancelarSoporte;
        /// <summary>
        /// Evento que Manipula el Manejador "Cancelar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelarSoporte(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCancelarSoporte != null)
                //Iniciando Evento
                ClickCancelarSoporte(this, e);
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento desencadenado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!(Page.IsPostBack))

                //Invocando Método de Carga
                cargaCatalogos();
            else
                //Recuperando Valor de los Atributos
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
        /// Evento generado al Cambiar la Selección de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoSoporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTipoSoporte.SelectedValue = "2"; 
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            if(tipo == 1)
            {
                ddlTipoSoporte.SelectedValue = "1";
                lblSoporte.Text = "Vales: ";
            }
            if(tipo == 2)
            {
                ddlTipoSoporte.SelectedValue = "2";
                lblSoporte.Text = "Depositos: ";
            }
            if (tipo == 3)
            {
                ddlTipoSoporte.SelectedValue = "3";
                lblSoporte.Text = "Operador Anterior: ";
                Panel.Visible = true;
            }
            if(tipo == 4)
            {
                ddlTipoSoporte.SelectedValue = "4";
                Panel1.Visible = false;
                Panel.Visible = true;
                lblNvoOpe.Text = "";
                //lblSoporte.Text = "La Factura con folio: ";
                //txtObservacion.Text = mensaje_operacion;
            }
          
                if (mensaje_operacion != null)
                {//Asignando Valores
                if (tipo == 3)
                {
                    lblMensaje.Text = Cadena.RegresaCadenaSeparada(mensaje_operacion, "Nuevo Operador:", 0, "0");
                    lblNomOpe.Text = Cadena.RegresaCadenaSeparada(mensaje_operacion, "Nuevo Operador:", 1, "1");
                    lblMensaje.ToolTip = no_servicio;
                }
                else
                {
                    lblMensaje.Text = mensaje_operacion;
                    lblMensaje.ToolTip = no_servicio;
                }
                }
                else
                {
                    lblMensaje.Text = "Por asignar";
                }
                //Limpiando Mensaje
                lblErrorSoporte.Text = "";
            
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Catalogos del Encabezado
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoSoporte, "", 3203);
            //CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(txtSolicita, "",);
            txtSolicita.Focus();
        }
        /// <summary>
        /// Método Privado encargado de Asignar el Valor de los Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Valores a los ViewState
            ViewState["objSoporte"] = objSoporte == null ? 0 : objSoporte.id_soporte_tecnico;
            ViewState["mensaje_operacion"] = mensaje_operacion;
            ViewState["tipo"] = tipo;
            ViewState["no_servicio"] = no_servicio;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que existan los Valores
            if (ViewState["objSoporte"] != null)
                //Asignando Valor al Contructor
                this.objSoporte = new SoporteTecnico(Convert.ToInt32(ViewState["objSoporte"]));

            //Validando que existan los Valores
            if (Convert.ToString(ViewState["mensaje_operacion"]) != null)
                //Asignando Valor al Atributo
                this.mensaje_operacion = Convert.ToString(ViewState["mensaje_operacion"]);
            //Validando que existan los Valores
            if (Convert.ToString(ViewState["tipo"]) != null)
                //Asignando Valor al Atributo
                this.tipo = Convert.ToInt32(ViewState["tipo"]);
            //Validando que existan los Valores
            if (Convert.ToString(ViewState["no_servicio"]) != null)
                //Asignando Valor al Atributo
                this.no_servicio = Convert.ToString(ViewState["no_servicio"]);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles para Edicion
        /// </summary>
        /// <param name="result">Estatus de Habilitación</param>
        private void habilitaControles(bool result_enabled)
        {
            //Controles
            ddlTipoSoporte.Enabled =
            txtFechaInicio.Enabled =
            txtFechaFin.Enabled =
            //Botones
            btnGuardarSoporte.Enabled =
            btnCancelarSoporte.Enabled = result_enabled;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Inicializa el Control en base a un Id  Asignación de Diesel existente
        /// </summary>
        /// <param name="id_asignacion">Id de Asignación</param>
        public void InicializaControlUsuario(string mensaje_operacion, int tipo, string no_servicio)
        {
            //Cargando Controles
            cargaCatalogos();
            txtSolicita.Text = "";
            txtObservacion.Text = "";

            //Inicializando Fechas
            txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMinutes(-2).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Asignando Parametros
            this.mensaje_operacion = mensaje_operacion;
            this.tipo = tipo;
            this.no_servicio = no_servicio;
            
            //Invocando Método de Inicializacion
            inicializaControl();
        }
        /// <summary>
        /// Método Público encaragdo de Guardar los Cambios de las Facturas
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaSoporte()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            string mensaje = "";
            if (tipo <= 2)
            {
                mensaje = "NO. SERV: " + no_servicio + " | " + lblSoporte.Text + " " + lblMensaje.Text + " | " + txtObservacion.Text.ToUpper();
            }
            if (tipo == 3)
            {
                mensaje = "NO. SERV: " + no_servicio + " | " + lblSoporte.Text + " " + lblMensaje.Text + " " + lblNvoOpe.Text + " " + lblNomOpe.Text + " | " + txtObservacion.Text.ToUpper();
            }
            if(tipo == 4)
            {
                mensaje = lblSoporte.Text + " " + lblMensaje.Text + "|" + txtObservacion.Text.ToUpper();
            }
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                result = SAT_CL.Soporte.SoporteTecnico.InsertaSoporteTecnico(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, 3,
                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, txtSolicita.Text.ToUpper(), Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                
                if (result.OperacionExitosa)
                {
                            result = SAT_CL.Soporte.SoporteTecnicoDetalle.InsertaSoporteTecnicoDetalle(Convert.ToByte(ddlTipoSoporte.SelectedValue), 3, Convert.ToDateTime(txtFechaInicio.Text), Convert.ToDateTime(txtFechaFin.Text), mensaje, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                   
                }
                if (result.OperacionExitosa)
                {
                    trans.Complete();
                }
            }
            //Validando que la operación haya sido exitosa
            //if (result.OperacionExitosa)
                
            ////Mostrando Mensaje
            //lblErrorSoporte.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        #endregion
    }
}