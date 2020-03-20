using System;
using System.Web.UI;
using TSDK.Base;

namespace SAT.UserControls
{
    /// <summary>
    /// Clase del control de usuario Lectura que permite realizar acciones sobre el mismo, mediante la configuración de métodos y eventos.
    /// </summary>
    public partial class Lectura : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Atributo que almacena el identificador de una lectura.
        /// </summary>
        private int _id_lectura;
        /// <summary>
        /// Atributo que almacena el identificador de una unidad de trasnporte
        /// </summary>
        private int _id_unidad;

        private int _id_operador;
        /// <summary>
        /// Permite obtener el valor de tabulación de cada control.
        /// </summary>
        public short TabIndex
        {
            //Asigna a los controles del control de usuario el valor de values 
            set
            {
                txtFecha.TabIndex =
                txtUnidad.TabIndex =
                txtOperador.TabIndex =
                txtLitrosLectura.TabIndex =
                txtKmsLectura.TabIndex =
                txtHrsLectura.TabIndex =
                txtKmsSistema.TabIndex =
                txtIdentificador.TabIndex =
                txtReferencia.TabIndex = value;
            }
            //obtiene el primer valor del primer control de usuario
            get { return txtFecha.TabIndex; }
        }
        /// <summary>
        /// Permite obtener el valor de disponibilidad de los controles (true/false).
        /// </summary>
        public bool Enabled
        {
            //Asigna un valor a los controles del control de usuario
            set
            {
                txtFecha.Enabled =
                txtUnidad.Enabled =
                txtOperador.Enabled =
                txtLitrosLectura.Enabled =
                txtKmsLectura.Enabled =
                txtHrsLectura.Enabled =
                txtKmsSistema.Enabled =
                txtIdentificador.Enabled =
                txtReferencia.Enabled = value;
            }
            //Obtiene el valor del primer control de usuario
            get { return this.txtFecha.Enabled; }
        }
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Manejador de Eventos

        /// <summary>
        /// Creación del evento ClickGuardarLectura
        /// </summary>
        public event EventHandler ClickGuardarLectura;
        /// <summary>
        /// Creación del evento ClicEliminarLectura
        /// </summary>
        public event EventHandler ClickEliminarLectura;

        /// <summary>
        /// Delegado del evento ClickEliminar que permite que las clases derivadas invoquen al método OnClickEliminar y generen el evento ClickEliminar.
        /// </summary>
        /// <param name="e">Contiene datos del evento</param>
        public virtual void OnClickEliminarLectura(EventArgs e)
        {
            //Valida si el evento ClickEliminar es diferente de nulo
            if (ClickEliminarLectura != null)
                //Asigna al evento ClickEliminar los parametros this y e.
                ClickEliminarLectura(this, e);
        }
        /// <summary>
        /// Delegado del evento ClickGuardar, permite que las clases derivadas invoquen al método OnClickGuardar y generen el evento ClickGuardar.
        /// </summary>
        /// <param name="e">Contienen datos de eventos </param>
        public virtual void OnClickGuardarLectura(EventArgs e)
        {
            //Valida si el evento ClickGuardar es Diferente de nulo
            if (ClickGuardarLectura != null)
                //Asigna al evento ClickGuardar los parametros this(origen del evento) y e (Valor utilizado que no incluye datos del evento)
                ClickGuardarLectura(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que permite determinar el inicio del control de usuario Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se a cargado por primera vez.
            if (!(Page.IsPostBack))
            {
                //Sugiere un tipo de formato para la fecha de cotizacion
                txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

                //Invoca al constructor de la clase unidad para obtener el id y el numero de la unidad
                using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(this._id_unidad))
                {
                    //Valida que exista el registro
                    if (uni.id_unidad > 0)
                        //Carga el valor de la unidad(numero e identificador)  al txtUnidad
                        txtUnidad.Text = string.Format("{0}   ID:{1}", uni.numero_unidad, uni.id_unidad);
                }
            }
            else
                //Invoca al método recuperaAtrubutos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento que realiza una carga previa del control de usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Invoca al método Asignacion
            asignaAtributos();
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            //Validando que el evento "Eliminar" este Vacio
            if (ClickEliminarLectura != null)
                OnClickEliminarLectura(new EventArgs());
        }
        /// <summary>
        /// Evento que revoca las acciones realizadas sobre el control de usuario Lectura, inicializando los valores a su estado anterior 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Invoca al método inicializaValores.
            inicializaValores(this._id_lectura, this._id_unidad);
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que el evento "Guardar" este Vacio
            if (ClickGuardarLectura != null)
                OnClickGuardarLectura(new EventArgs()); 

        }
        /// <summary>
        /// Evento Producido al Cambiar el Control RadioButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbGestionaLectura_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlesLectura();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que almacena los valores de la página(id_Lectura)
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable viewState idLectura el valor del atributo id_lectura
            ViewState["idLectura"] = this._id_lectura;
            ViewState["idUnidad"] = this._id_unidad;
        }
        /// <summary>
        /// Método que consulta o estrae los valores de la página. 
        /// </summary>   
        private void recuperaAtributos()
        {
            //Valida el valor de la variable ViewState idLectura(que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idLectura"]) != 0)
                //Si cumple la condición, asigna al atributo _id_lectura el valor de la variable ViewState idLectura
                this._id_lectura = Convert.ToInt32(ViewState["idLectura"]);
            //Valida el valor de la variable ViewState idLectura(que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idUnidad"]) != 0)
                //Si cumple la condición, asigna al atributo _id_lectura el valor de la variable ViewState idLectura
                this._id_unidad = Convert.ToInt32(ViewState["idUnidad"]);
        }
        /// <summary>
        /// Método que elimina los datos introducidos en los controles. 
        /// </summary>
        private void limpiaControles()
        {
            //Inicializando Valores
            txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtKmsSistema.Text = 
            txtLitrosLectura.Text = "0";
            txtIdentificador.Text = 
            txtReferencia.Text = "";

            //Asignando Valores
            rbKmsLec.Checked = true;
            rbHrsLec.Checked = false;

            //Invocando Método de Configuración
            configuraControlesLectura();
        }
        /// <summary>
        /// Métodoq ue permite inicializar con valores loc controles del control de usuario
        /// </summary>
        /// <param name="id_lectura">Id que sirve como referencia para la busqueda de registro</param>
        private void inicializaValores(int id_lectura, int id_unidad)
        {
            //Asigna al atributo _id_lectura el valor del parametro del método inicializaValores.
            this._id_lectura = id_lectura;
            this._id_unidad = id_unidad;
            //Valida el valor del atributo para definir una inserción o una edición de datos
            if (this._id_lectura > 0)
            {
                //Invoca al constructor de la clase para obtener el registros
                using (SAT_CL.Mantenimiento.Lectura lec = new SAT_CL.Mantenimiento.Lectura(this._id_lectura))
                {
                    //Valida que exista el registro en la base de datos
                    if (lec.id_lectura > 0)
                    {
                        //Invoca al constructor de la clase operador para obtener los datos del operador 
                        using (SAT_CL.Global.Operador ope = new SAT_CL.Global.Operador(lec.id_operador))
                        {
                            //Valida que exista el registro
                            if (ope.id_operador > 0)
                                //Carga los valores del operador(Nombre e identificador) al txtOperador
                                txtOperador.Text = string.Format("{0}   ID:{1}", ope.nombre, ope.id_operador);
                        }

                        //Si hay lectura en Km's
                        if(lec.kms_lectura > 0)
                        {
                            //Asignando Valores
                            rbKmsLec.Checked = true;
                            rbHrsLec.Checked = false;
                        }
                        //Si hay lectura en Hrs.
                        else if (lec.horas_lectura > 0)
                        {
                            //Asignando Valores
                            rbKmsLec.Checked = false;
                            rbHrsLec.Checked = true;
                        }
                        else
                        {
                            //Asignando Valores
                            rbKmsLec.Checked = true;
                            rbHrsLec.Checked = false;
                        }

                        //Asignando Valores
                        txtFecha.Text = lec.fecha_lectura.ToString("dd/MM/yyyy HH:mm");
                        txtIdentificador.Text = lec.identificador_operador_unidad.ToString();
                        txtKmsSistema.Text = lec.kms_sistema.ToString();
                        txtLitrosLectura.Text = lec.litros_lectura.ToString();
                        txtReferencia.Text = lec.referencia.ToString();

                        //Invocando Método de Configuración
                        configuraControlesLectura();
                    }
                }
            }
            else
                //Invocando Método de Limpieza
                limpiaControles();

            //Invoca al constructor de la clase unidad para obtener el id y el numero de la unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(this._id_unidad))
            {
                //Valida que exista el registro
                if (uni.id_unidad > 0)
                    //Carga el valor de la unidad(numero e identificador)  al txtUnidad
                    txtUnidad.Text = string.Format("{0}   ID:{1}", uni.numero_unidad, uni.id_unidad);

                //Validamos que no exista la Lectura para obtener Operador Asignado
                if(this._id_lectura == 0)
                {
                    //Invoca al constructor de la clase operador para obtener los datos del operador 
                    using (SAT_CL.Global.Operador ope = new SAT_CL.Global.Operador(uni.id_operador))
                    {
                        //Valida que exista el registro
                        if (ope.id_operador > 0)
                        {
                            //Carga los valores del operador(Nombre e identificador) al txtOperador
                            txtOperador.Text = string.Format("{0}   ID:{1}", ope.nombre, ope.id_operador);
                        }
                        else
                            //Carga los valores del operador(Nombre e identificador) al txtOperador
                            txtOperador.Text = string.Format("{0}   ID:{1}", "SIN ASIGNAR", 0);
                    }
                }
            }
        }
        /// <summary>
        /// Método que Asigna valores a los controles 
        /// </summary>
        /// <param name="id_lectura">Identificador de una lectura</param>
        /// <param name="id_unidad">Identificador de una unidad</param>
        /// <param name="id_operador">Identificador de un operador</param>
        private void inicializaValores(int id_lectura, int id_unidad, int id_operador)
        {
            //Asigna al atributo _id_lectura el valor del parametro del método inicializaValores.
            this._id_operador = id_operador;
            this._id_unidad = id_unidad;
            this._id_lectura = id_lectura;
                //Invocando Método de Limpieza
                limpiaControles();
            //Invoca al constructor de la clase unidad para obtener el id y el numero de la unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(this._id_unidad))
            {
                    //Carga el valor de la unidad(numero e identificador)  al txtUnidad
                    txtUnidad.Text = string.Format("{0}   ID:{1}", uni.numero_unidad, uni.id_unidad);
            }
            //Invoca al constructor de la clase operador para obtener los datos del operador 
            using (SAT_CL.Global.Operador ope = new SAT_CL.Global.Operador(this._id_operador))
            {
                //Carga los valores del operador(Nombre e identificador) al txtOperador
                txtOperador.Text = string.Format("{0}   ID:{1}", ope.nombre, ope.id_operador);
            }
        }
        
        /// <summary>
        /// Método encargado de Configurar los Controles de la Lectura
        /// </summary>
        private void configuraControlesLectura()
        {
            //Invoca al constructor de la clase para obtener el registros
            using (SAT_CL.Mantenimiento.Lectura lec = new SAT_CL.Mantenimiento.Lectura(this._id_lectura))
            {
                //Asignando Litros de Lectura
                txtLitrosLectura.Text = string.Format("{0:0.00}", lec.litros_lectura);
                
                //Si esta en Km's
                if (rbKmsLec.Checked)
                {
                    //Asignando Habilitación
                    txtKmsLectura.Enabled = true;
                    txtHrsLectura.Enabled = false;

                    //Asignando Unidad
                    lblUnidadRendimiento.Text = "Km/lt";
                    
                    //Validando si Existe la Lectura
                    if (lec.habilitar)
                    {
                        //Asignando Valores
                        txtKmsLectura.Text = lec.kms_lectura.ToString();
                        txtHrsLectura.Text = "0";
                    }
                    else
                    {
                        //Asignando Valores
                        txtKmsLectura.Text = "";
                        txtHrsLectura.Text = "0";
                    }
                }
                //Si esta en Hrs.
                else if (rbHrsLec.Checked)
                {
                    //Asignando Habilitación
                    txtKmsLectura.Enabled = false;
                    txtHrsLectura.Enabled = true;

                    //Asignando Unidad
                    lblUnidadRendimiento.Text = "Hr/lt";

                    //Validando si Existe la Lectura
                    if (lec.habilitar)
                    {
                        //Asignando Valores
                        txtKmsLectura.Text = "0.00";
                        txtHrsLectura.Text = lec.horas_lectura.ToString();
                    }
                    else
                    {
                        //Asignando Valores
                        txtKmsLectura.Text = "0.00";
                        txtHrsLectura.Text = "";
                    }                    
                }

                //Validando que exista la Lectura
                decimal litros = Convert.ToDecimal(txtLitrosLectura.Text.Equals("") ? "0" : txtLitrosLectura.Text);

                //Validando que existan los Litros
                if (litros != 0)
                {
                    //Si esta en Km's
                    if (rbKmsLec.Checked)
                        //Asignando Rendimiento
                        txtRendimiento.Text = string.Format("{0:0.00}", Convert.ToDecimal(txtKmsLectura.Text.Equals("") ? "0" : txtKmsLectura.Text) / litros);
                    
                    //Si esta en Hrs.
                    if (rbHrsLec.Checked)
                        //Asignando Rendimiento
                        txtRendimiento.Text = string.Format("{0:0.00}", Convert.ToDecimal(txtHrsLectura.Text.Equals("") ? "0" : txtHrsLectura.Text) / litros);
                }
                else
                    //Mostrando 
                    txtRendimiento.Text = "0.00";
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite la carga de valores a los controles
        /// </summary>
        /// <param name="id_lectura">Id que sirve como referencia para la carga de valores a los controles</param>
        public void InicializaControl(int id_lectura,int id_unidad)
        {
            //Invoca al método inicializaValores
            this.inicializaValores(id_lectura,id_unidad);
        }
        /// <summary>
        /// Método que permite cargar de valores a los controles
        /// </summary>
        /// <param name="id_lectura">Id que sirve como referencia para la carga de valores a los controles (Lectura)</param>
        /// <param name="id_unidad">Id que sirve como referencia para la carga de valores a los controles (Unidad)</param>
        /// <param name="id_operador">Id que sirve como referencia para la carga de valores a los controles (Operador)</param>
        public void InicializaControl(int id_lectura, int id_unidad, int id_operador)
        {
            //Invoca al método inicializaValores
            this.inicializaValores(id_lectura, id_unidad,id_operador);
        }
        /// <summary>
        /// Método que permite almacenr los datos de los controles del control de usuario Lectura
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardarLectura()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase lectura
            using (SAT_CL.Mantenimiento.Lectura lec = new SAT_CL.Mantenimiento.Lectura(this._id_lectura))
            {
                //Valida si existe el registro en base de datos
                if (lec.id_lectura > 0)
                {
                    //Asigna al objeto retorno los valores de los controles invocando al método de edición
                    retorno = lec.EditarLectura(Convert.ToDateTime(txtFecha.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, "ID:", 1)),
                                                txtIdentificador.Text, Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtKmsSistema.Text, "0.00")), Convert.ToDecimal(txtKmsLectura.Text), Convert.ToInt32(Cadena.VerificaCadenaVacia(txtHrsLectura.Text, "0")),
                                                Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtLitrosLectura.Text, "0.00")), txtReferencia.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //En caso contrario
                else
                    //Asigna al objeto retorno los valores de  los controles invocando al método de insercion
                    retorno = SAT_CL.Mantenimiento.Lectura.InsertarLectura(Convert.ToDateTime(txtFecha.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOperador.Text, "ID:", 1)),
                                                                          txtIdentificador.Text, Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtKmsSistema.Text, "0.00")), Convert.ToDecimal(txtKmsLectura.Text), Convert.ToInt32(Cadena.VerificaCadenaVacia(txtHrsLectura.Text,"0"))
                                                                          ,Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtLitrosLectura.Text, "0.00")),
                                                                          txtReferencia.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            }
            //Valida si se realizo correctamente la operación
            if(retorno.OperacionExitosa)
            {
                //Invoca al método inicializaValores
                this.inicializaValores(retorno.IdRegistro, this._id_unidad);
            }
            //Retorna al método el objeto retrono
            return retorno;
        }
        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLectura()
        {
            //Creación del objeto retrono
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase lectura
            using (SAT_CL.Mantenimiento.Lectura lec = new SAT_CL.Mantenimiento.Lectura(this._id_lectura))
            {
                //Valida que exista el registro
                if (lec.id_lectura > 0)
                    //Asigna al objeto retorno los valores del usuario que realizo el cambio de estado del registro
                    retorno = lec.DeshabilitarLectura(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Valida que se realizo la operacion 
            if (retorno.OperacionExitosa)
            {
                //Invoca al método inicializaValores
                this.inicializaValores(0, this._id_unidad);
                //Invoca al método limpiaControles
                limpiaControles();
            }
            //Retorna al método el objeto retorno.
            return retorno;      
        }

        #endregion
    }
}