using SAT_CL.Global;
using System;
using System.Web.UI;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    /// <summary>
    /// Clase control de usuario Ciudad que permite realizar acciones sobre el mismo, mediante la configuración de métodos y eventos.
    /// </summary>
    public partial class wucCiudad : System.Web.UI.UserControl
    {
        #region Atributos
        //Atributo que permite obtener el valor del identificador de una ciudad.
        private int _id_ciudad;
        /// <summary>
        /// Permite obtener el valor de tabulación de cada control.
        /// </summary>
        public short TabIndex
        {
            //Asigna a los controles del control de usuario el valor de values 
            set
            {
                txtDescripcion.TabIndex =
                ddlEstado.TabIndex=
                ddlPais.TabIndex =
                btnEliminar.TabIndex = 
                btnGuardar.TabIndex=
                btnCancelar.TabIndex= value;                       
            }
            //obtiene el primer valor del primer control de usuario(txtDescripcion)
            get { return txtDescripcion.TabIndex; }
        }
        /// <summary>
        /// Permite obtener el valor de disponibilidad de los controles (true/false).
        /// </summary>
        public bool Enabled
        {
            //Asigna un valor a los controles del control de usuario
            set
            {
                txtDescripcion.Enabled =
                ddlEstado.Enabled =
                ddlPais.Enabled =
                btnEliminar.Enabled =
                btnGuardar.Enabled =
                btnCancelar.Enabled = value;
            }
            //Obtiene el valor del primer control de usuario(txtDescripcion)
            get { return this.txtDescripcion.Enabled; }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento que permite determinar el inicio del control de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la pagina se a cargado por primera vez.
            if (!(Page.IsPostBack))
                //Invoca al método cargaCatalogos
                cargaCatalogos();
            else
                //invoca al método recuperaAtributos
                recuperaAtributos();
        }

        /// <summary>
        /// Evento que realiza una carga previa de la página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }

        /// <summary>        
        /// Evento que revoca las aciones realizadas sobre el formulario (insertar registros,deshacer edición, etc.).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Invoca al método inicializaValores();
            inicializaValores(this._id_ciudad);

        }
        /// <summary>
        /// Evento que permite  realizar la consulta sobre el historial del registro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacora(object sender, EventArgs e)
        {
            //Valida que exista el registro
            if (this._id_ciudad > 0)
            {
                //invoca al método inicializaBitacora();
                inicializaBitacora(this._id_ciudad.ToString(), "54", "Bitácora Ciudad");
            }
        }
        /// <summary>
        /// Evento que permite cargar los valores del dropdownlist estado, dependiendo del la opcion seleccionada del dropdownlist país.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
                //Carga el catalo estado al ddlEstado 
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstado, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
        }
        #endregion

        #region Manejadores de Eventos
        /// <summary>
        /// Creación del evento ClickGuardar
        /// </summary>
        public event EventHandler ClickGuardarCiudad;

        /// <summary>
        /// Creación del evento ClickEliminar
        /// </summary>
        public event EventHandler ClickEliminarCiudad;

        /// <summary>
        /// Delegado del evento ClickGuardar, permite que las clases derivadas invoquen al método OnClickGuardar y generen el evento ClickGuardar.
        /// </summary>
        /// <param name="e">Contienen datos de eventos </param>
        public virtual void OnClickGuardarCiudad(EventArgs e)
        {
            //Valida si el evento ClickGuardar es diferente de nulo
            if (ClickGuardarCiudad != null)
                //Asigna al evento ClickGuardar los parametros this (origen del evento) y e (valor utilizado en los eventos que no incluyen datos de evento).
                ClickGuardarCiudad(this, e);
        }
        /// <summary>
        /// Delegado del evento ClickEliminar que permite que las clases derivadas invoquen al método OnClickEliminar y generen el evento ClickEliminar.
        /// </summary>
        /// <param name="e">Contiene datos del evento</param>
        public virtual void OnClickEliminarCiudad(EventArgs e)
        {
            //Valida si el evento ClickEliminar es diferente de nulo
            if (ClickEliminarCiudad != null)
                //Asigna al evento ClickEliminar los parametros this y e.
                ClickEliminarCiudad(this, e);
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Guardar" este Vacio
            if (ClickGuardarCiudad != null)
                OnClickGuardarCiudad(new EventArgs());
        }
        /// <summary>
        /// Evento disparado al presionar el boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_Click(object sender, EventArgs e)
        {   //Validando que el evento "Cancelar" este Vacio
            if (ClickEliminarCiudad != null)
                OnClickEliminarCiudad(new EventArgs());
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Carga los catalogos en los dropdownlist
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga los catalogos  a los dropdownlist
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPais, "", 15);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstado, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
        }
        /// <summary>
        /// Método que elimina los datos introducidos en los controles. 
        /// </summary>
        private void limpiaControles()
        {
            //Limpia los controles 
            txtDescripcion.Text = "";
            lblError.Text = "";
        }

        /// <summary>
        /// Método que permite almacenar los valores de las páginas
        /// </summary>
        private void asignaAtributos()
        {
            //Almacena en la variable ViewState idCiudad el valor del atributo id_ciudad;
            ViewState["idCiudad"] = this._id_ciudad;
        }
        /// <summary>
        /// Método que consulta o extrae los valores de la página
        /// </summary>
        private void recuperaAtributos()
        {
            //Valida el valor de la variable Viewstate idCiudad (que sea diferente de 0)
            if (Convert.ToInt32(ViewState["idCiudad"]) != 0)
                //Si se cumple la sentencia, asigna al atributo _id_ciudad el valor de la variable ViewState idCiudad
                this._id_ciudad = Convert.ToInt32(ViewState["idCiudad"]);
        }
        /// <summary>
        /// Método que permite inicializar los valores de los controles 
        /// </summary>
        /// <param name="id_ciudad">Id que sirve como referencia para la busqueda de registros</param>
        private void inicializaValores(int id_ciudad)
        {
            //Asigana al atributo privado el valor del parametro del método inicializaValores();
            this._id_ciudad = id_ciudad;
            //Valida si es una insercion de datos o una edición
            if (this._id_ciudad > 0)
            {
                //Invoca al constructor de la clase Ciudad para obtener un registro
                using (Ciudad cd = new Ciudad(this._id_ciudad))
                {
                    //Valida si existe el registro Ciudad en la base de datos.
                    if (cd.id_ciudad > 0)
                    {
                        //Asigna a los controles los valores del registro  Ciudad.
                        txtDescripcion.Text = cd.descripcion;
                        ddlPais.SelectedValue = Catalogo.RegresaDescripcionValorSuperior(16, cd.id_estado).ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstado, "", 16, Convert.ToInt32(ddlPais.SelectedValue));
                        ddlEstado.SelectedValue = cd.id_estado.ToString();
                    }
                }
            }
            //En caso contrario
            else
                //limpia los controles;
                limpiaControles();
        }

        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro de ciudad
        /// </summary>
        /// <param name="idRegistro">Identificador del Registro de una ciudad</param>
        /// <param name="idTabla">Identificador de Tabla ciudad en la base de datos</param>
        /// <param name="Titulo">Titulo que se le dara a la ventana</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = Cadena.RutaRelativaAAbsoluta("~/UserControls/wucCiudad.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Bitácora Ciudad", configuracion, Page);
        }

        #endregion

        #region Métodos Publicos
        
        /// <summary>
        /// Método que permite la carga de valores de los controles
        /// </summary>
        /// <param name="id_ciudad">Id que sirve como referencia para la carga de valores</param>
        public void InicializaControl(int id_ciudad)
        {
            //Invoca al método inicializaValores();
            this.inicializaValores(id_ciudad); 
        }
        /// <summary>
        /// Método que permite almacenar los datos de los controles del control de usuario Ciudad en la base de datos.
        /// </summary>
        public RetornoOperacion GuardarCiudad()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();         
            //Invoca al constructor de la clase ciudad.
            using (Ciudad cd = new Ciudad(this._id_ciudad))
            {
                //Valida si existe el registro en la base de datos.
                if (cd.id_ciudad > 0)
                    //Asigna al objeto retorno los valores del método editar.
                    retorno = cd.EditaCiudad(txtDescripcion.Text, Convert.ToByte(ddlEstado.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //En caso de que no exista el rgistro
                else
                    //Asigna al objeto retorno los valores del método de inserción.
                    retorno = Ciudad.InsertaCiudad(txtDescripcion.Text, Convert.ToByte(ddlEstado.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Comprueba que se realizo correctamente la operación
            if (retorno.OperacionExitosa)            
                //Invoca al método inicializaControles
                this.inicializaValores(this._id_ciudad);
                //Muestra un mensaje de que se realizo o no se realizo correctamente la operación.
                lblError.Text = retorno.Mensaje;
                //Retorna el resultado al método
                return retorno;

        }
        /// <summary>
        /// Método que permite cambiar el estado de un registro (Habilitar/Deshabilitar)
        /// </summary>
        public RetornoOperacion DeshabilitarCiudad()
        {            
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invoca al constructor de la clase ciudad.
            using (Ciudad cd = new Ciudad(this._id_ciudad))
            {
                //Valida que exista el registro en la base de datos
                if (cd.id_ciudad > 0)
                    //Asigna al objeto retorno los datos del usuario que realizo la deshabilitación del registro.
                    retorno = cd.DeshabilitaCiudad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Comprueba que se realizo correctamente la operación
            if (retorno.OperacionExitosa)
            {
                //Invoca al método inicializaValores
                this.inicializaValores(this._id_ciudad);
                //Invoca al método limpiaControles
                limpiaControles();
            }
            //Muestra un mensaje de que se realizo o no se realizo correctamente la operación.
            lblError.Text = retorno.Mensaje;
            //Retorno del resultado al método
            return retorno;
        }
        #endregion








    }
}