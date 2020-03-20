using System;
using System.Web.UI;
using TSDK.Base;

namespace SAT.UserControls
{
  /// <summary>
  /// Clase que permite dar de alta una cotizacion, Editarla y Deshabilitarla
  /// </summary>
  public partial class wucCotizacion : System.Web.UI.UserControl
    {
        #region Atributos
        // Atributo que permite almacenar el identificador de una cotización.
        private int _id_cotizacion;
        // Atributo que permite almacenar el identificador de una compañia
        private int _id_compania_emisor;
        // Atributo que permite almacenar el identificador de un producto
        private int _id_producto;
        /// <summary>
        /// Permite obtener el valor de tabulación de cada control.
        /// </summary>
        public short TabIndex
        {
            //Asigna a los controles del control de usuario el valor de values 
            set
            {
                txtCompaniaEmisor.TabIndex =
                txtNoRequisicion.TabIndex =
                txtProveedor.TabIndex =
                txtProducto.TabIndex =
                txtCantidad.TabIndex =
                txtPrecio.TabIndex =
                txtFechaCotizacion.TabIndex =
                ddlMoneda.TabIndex =
                txtVigencia.TabIndex =
                txtEntrega.TabIndex =
                txtComentario.TabIndex =
                btnEliminar.TabIndex =
                btnGuardar.TabIndex =
                btnCancelar.TabIndex = value;
            }
            //obtiene el primer valor del primer control de usuario
            get { return txtCompaniaEmisor.TabIndex; }
        }
        /// <summary>
        /// Permite obtener el valor de disponibilidad de los controles (true/false).
        /// </summary>
        public bool Enabled
        {
            //Asigna un valor a los controles del control de usuario
            set
            {
                txtCompaniaEmisor.Enabled = 
                txtNoRequisicion.Enabled =
                txtProveedor.Enabled =
                txtProducto.Enabled =
                txtCantidad.Enabled =
                txtPrecio.Enabled =
                txtFechaCotizacion.Enabled =
                ddlMoneda.Enabled =
                txtVigencia.Enabled =
                txtEntrega.Enabled =
                txtComentario.Enabled =
                btnEliminar.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled = value;
            }
            //Obtiene el valor del primer control de usuario
            get { return this.txtCompaniaEmisor.Enabled; }
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Manejador de Eventos
        /// <summary>
        /// Creación del evento ClickGuardarCotizacion
        /// </summary>
        public event EventHandler ClickGuardarCotizacion;
        /// <summary>
        /// Creación del evento ClickEliminarCotizacion
        /// </summary>
        public event EventHandler ClickEliminarCotizacion;
        /// <summary>
        /// Delegado del evento ClickGuardar, permite que las clases derivadas invoquen al método OnClickGuardar y generen el evento ClickGuardar.
        /// </summary>
        /// <param name="e">Contienen datos de eventos </param>
        public virtual void OnClickGuardarCotizacion(EventArgs e)
        {
            //Valida si el evento ClickGuardar es diferente de nulo
            if (ClickGuardarCotizacion != null)
                //Asigna al evento ClickGuardar los parametros this (origen del evento) y e (valor utilizado en los eventos que no incluyen datos de evento).
                ClickGuardarCotizacion(this, e);
        }
        /// <summary>
        /// Delegado del evento ClickEliminar que permite que las clases derivadas invoquen al método OnClickEliminar y generen el evento ClickEliminar.
        /// </summary>
        /// <param name="e">Contiene datos del evento</param>
        public virtual void OnClickEliminarCotizacion(EventArgs e)
        {
            //Valida si el evento ClickEliminar es diferente de nulo
            if (ClickEliminarCotizacion != null)
                //Asigna al evento ClickEliminar los parametros this y e.
                ClickEliminarCotizacion(this, e);
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Guardar" este Vacio
            if (ClickGuardarCotizacion != null)
                OnClickGuardarCotizacion(new EventArgs());           
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Cancelar" este Vacio
            if (ClickEliminarCotizacion != null)
                OnClickEliminarCotizacion(new EventArgs());
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se a cargado por primera vez.
            if (!(Page.IsPostBack))
            {
                //Invoca al método cargaCatalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
                //Sugiere un tipo de formato para la fecha de cotizacion
                txtFechaCotizacion.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm"); 
            }
            else
                //Invoca al método recuperaAtrubutos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento que realiza una carga previa de la página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invoca al método de asignación.
            asignaAtributos();
        }
        /// <summary>
        /// Evento que revoca las acciones realizadas sobre el formulario(insertar registros, deshacer edicion)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Invoca al método inicializaValores.
            inicializaValores(this._id_cotizacion);
        }
        #endregion
        
        #region Métodos Privados
        /// <summary>
        /// Método que elimina los datos introducidos en los controles. 
        /// </summary>
        private void limpiaControles()
        {
            txtCompaniaEmisor.Text = "";
            txtProveedor.Text = "";
            txtNoRequisicion.Text = "";
            txtProveedor.Text = "";
            txtProducto.Text = "";
            txtCantidad.Text = "";
            txtPrecio.Text = "";
            txtFechaCotizacion.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm"); 
            txtVigencia.Text = "";
            txtEntrega.Text = "";
            txtComentario.Text = "";            
        }
        /// <summary>
        /// Método que permite almacenar los valores de la página
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable Viewstate idCotizacion el valor del atributo id_cotizacion
            ViewState["idCotizacion"] = this._id_cotizacion;
            //Almacena en la variable Viewstate idCompania el valor del atributo id_compania
            ViewState["idCompania"] = this._id_compania_emisor;
            //Almacena en la variable Viewstate idProducto el valor del atributo id_producto
            ViewState["idProducto"] = this._id_producto;
        }
        /// <summary>
        /// Método que consulta o estrae los valores de la página. 
        /// </summary>   
        private void recuperaAtributos()
        {
            //Valida el valor de la variable Viewstate idCotizacion (que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idCotizacion"]) != 0)
                //Si se cumple la sentencia, asigna al atributo _id_cotizacion el valor de la variable ViewState idCotizacion
                this._id_cotizacion = Convert.ToInt32(ViewState["idCotizacion"]);
            //Valida el valor de la variable Viewstate idCompania (que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idCompania"]) != 0)
                //Si se cumple la sentencia, asigna al atributo _id_compania_emisor el valor de la variable ViewState idCompania
                this._id_compania_emisor = Convert.ToInt32(ViewState["idCompania"]);
            //Valida el valor de la variable Viewstate idProducto (que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idProducto"]) != 0)
                //Si se cumple la sentencia, asigna al atributo _id_producto el valor de la variable ViewState idProducto
                this._id_producto = Convert.ToInt32(ViewState["idProducto"]);
        }
        /// <summary>
        /// Método que permite inicializar los valores de los controles 
        /// </summary>
        /// <param name="id_ciudad">Id que sirve como referencia para la busqueda de registros</param>
        private void inicializaValores(int id_cotizacion)
        {
            //Asigna al atributo privado el valor del parametro del método inicializaValores.
            this._id_cotizacion = id_cotizacion;
            //Valida si es una inserción de datos o una edición.
            if (this._id_cotizacion > 0)
            {
                //Invoca al constructor de la clase Cotizacion para obtener el registro.
                using (SAT_CL.Almacen.Cotizacion cot = new SAT_CL.Almacen.Cotizacion(this._id_cotizacion))
                {
                    //Valida si existe el registro Cotización en la base de datos.
                    if (cot.id_cotizacion > 0)
                    {
                        //Invoca al constructor CompaniaEmisorReceptor para obtener los datos de la compañia
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(cot.id_compania_emisor))                        
                        {
                            //Valida que exista el registro compañia
                            if (emisor.id_compania_emisor_receptor > 0)
                                //Caraga el valor de la compañia (nombre e identificador) al txtCompaniaEmisor
                                txtCompaniaEmisor.Text = string.Format("{0}   ID:{1}", emisor.nombre, emisor.id_compania_emisor_receptor);                               
                        }
                        //Asigna a los controles los valores del registro Cotización.
                        txtNoRequisicion.Text = cot.no_requisicion.ToString();
                        //Invoca al constructor de la clase CompaniaEmisorReceptor para obtener el nombre del preveedor de la cotización.
                        using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(cot.id_proveedor))
                        {
                            //Valida que exista el registro
                            if (proveedor.id_compania_emisor_receptor > 0)
                                //Carga el valor de proveedor (nombre e identificador) al txtProveedor
                                txtProveedor.Text = string.Format("{0}   ID:{1}", proveedor.nombre, proveedor.id_compania_emisor_receptor);
                        }
                        //Invoca al constructor de la clase producto  para obtener el nombre del producto de la cotización.
                        using (SAT_CL.Global.Producto prod = new SAT_CL.Global.Producto(cot.id_producto))
                        {
                            //Valida que exista el registro
                            if (prod.id_producto > 0)
                                //Carga los valores del producto(nombre e identificador) al txtProducto
                                txtProducto.Text = string.Format("{0}   ID:{1}", prod.descripcion, prod.id_producto);
                        }                        
                        txtCantidad.Text = cot.cantidad.ToString();
                        txtPrecio.Text = cot.precio.ToString();
                        txtFechaCotizacion.Text = cot.fecha_cotizacion.ToString("dd/MM/yyyy HH:mm");
                        ddlMoneda.SelectedValue = cot.id_moneda.ToString();
                        txtVigencia.Text = cot.dias_vigencia.ToString();
                        txtEntrega.Text = cot.dias_entrega.ToString();
                        txtComentario.Text = cot.comentario;
                    }
                }
            }
            else
                //Invoca al método limpiaControles.
                limpiaControles();
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que permite la carga de valores de los controles
        /// </summary>
        /// <param name="id_ciudad">Id que sirve como referencia para la carga de valores</param>
        public void InicializaControl(int id_cotizacion)
        {
            this.inicializaValores(id_cotizacion);
        }
        /// <summary>
        /// Método que permite almacenar los datos de los controles del control de usuario Cotización en la base de datos
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardarCotizacion()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase cotizacion.
            using (SAT_CL.Almacen.Cotizacion cot = new SAT_CL.Almacen.Cotizacion(this._id_cotizacion))
            {
                //Valida si existe el registro en base de datos
                if (cot.id_cotizacion > 0)
                {
                    //Asigna al objeto retorno los valores de los controles, invocando al método de edición
                    retorno = cot.EditarCotizacion(Convert.ToInt32(txtNoRequisicion.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1)), Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtPrecio.Text),
                                                    Convert.ToDateTime(txtFechaCotizacion.Text), Convert.ToByte(ddlMoneda.SelectedValue), Convert.ToInt32(txtVigencia.Text),Convert.ToInt32(txtEntrega.Text),
                                                    txtComentario.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }  
                //En caso contrario
                else
                    //Asigna al objeto retorno los valores de  los controles invocando al método de insercion
                    retorno = SAT_CL.Almacen.Cotizacion.InsertarCotizacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompaniaEmisor.Text, "ID:", 1)), Convert.ToInt32(txtNoRequisicion.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1)), Convert.ToDecimal(txtCantidad.Text), Convert.ToDecimal(txtPrecio.Text),
                            Convert.ToDateTime(txtFechaCotizacion.Text), Convert.ToByte(ddlMoneda.SelectedValue), Convert.ToInt32(txtVigencia.Text), Convert.ToInt32(txtEntrega.Text),
                            txtComentario.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Valida si se realizo correctamente la operacion
            if (retorno.OperacionExitosa)   
                //Invoca al método inicializaValores
                this.inicializaValores(this._id_cotizacion);
                //Muestra un mensaje Validando la operación.
                lblError.Text = retorno.Mensaje;
                //retorna al método el objeto retorno.
                return retorno;
            
        }
        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCotizacion()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase cotización 
            using (SAT_CL.Almacen.Cotizacion cot = new SAT_CL.Almacen.Cotizacion(this._id_cotizacion))
            {
                //Valida si existe el registro
                if (cot.id_cotizacion > 0)
                    //Asigna al objeto retorno los valores del usuario que realizo el cambio de estado del registro
                    retorno=cot.DeshabilitarCotizacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                
            }
            //Valida que se realizo la operacion
            if (retorno.OperacionExitosa)
            {
                //Invoca al método inicializaValores
                this.inicializaValores(this._id_cotizacion);
                //Invoca al método limpiaControles.
                limpiaControles();
            }
            //Muestra un mensaje Validando la operación.
            lblError.Text = retorno.Mensaje;
            //Retorna al método el objeto retorno.
            return retorno;
        }
        #endregion


    }
}
    
