using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class TemperaturaServicio : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_servicio;

        private bool _visualizaBotonGuardar;
        /// <summary>
        /// Atributo encargado de Almacenar el Indicador que Vizualiza el Boton "Guardar"
        /// </summary>
        public bool VisualizaBotonGuardar { set { this._visualizaBotonGuardar = value; } }

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   
                //Asignando Orden
                txtMaxima1.TabIndex =
                txtMedia1.TabIndex =
                txtMinima1.TabIndex =
                chkFull.TabIndex =
                txtTMaxima2.TabIndex =
                txtTMedia2.TabIndex =
                txtTMinima2.TabIndex = 
                btnGuardar.TabIndex = value;
            }
            get { return txtMaxima1.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                txtMaxima1.Enabled =
                txtMedia1.Enabled =
                txtMinima1.Enabled =
                chkFull.Enabled =
                txtTMaxima2.Enabled =
                txtTMedia2.Enabled =
                txtTMinima2.Enabled = 
                btnGuardar.Enabled = value;
            }
            get { return txtMaxima1.Enabled; }
        }

        #endregion

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar Temperaturas"
        /// </summary>
        public event EventHandler ClickGuardarTemperaturas;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar Temperaturas"
        /// </summary>
        /// <param name="e">Evento</param>
        public virtual void OnClickGuardarTemperaturas(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickGuardarTemperaturas != null)
                
                //Iniciando Evento
                ClickGuardarTemperaturas(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
                
                //Asignando Atributos
                asignaAtributos();
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarTemperaturas != null)

                //Iniciando Manejador
                OnClickGuardarTemperaturas(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFull_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlesFull();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="enable">Habilitación del Boton "Guardar"</param>
        private void inicializaControlUsuario(int id_servicio)
        {
            //Asignando Valor
            this._id_servicio = id_servicio;

            //Declarando variables Auxiliares
            string temp_max1, temp_med1, temp_min1, temp_max2, temp_med2, temp_min2;
            bool full = false;
            
            //Obteniendo Valores
            SAT_CL.Global.Referencia.ObtieneTemperaturasServicio(id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out temp_max1, out temp_med1, out temp_min1, out full,
                                                                                    out temp_max2, out temp_med2, out temp_min2);

            //Asignando Valores
            txtMaxima1.Text = temp_max1;
            txtMedia1.Text = temp_med1;
            txtMinima1.Text = temp_min1;
            txtTMaxima2.Text = temp_max2;
            txtTMedia2.Text = temp_med2;
            txtTMinima2.Text = temp_min2;
            chkFull.Checked = full;

            //Habilitando Control
            btnGuardar.Visible = this._visualizaBotonGuardar;
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {   
            //Asignando Atributos
            ViewState["idServicio"] = this._id_servicio;
            ViewState["visualizaBoton"] = this._visualizaBotonGuardar;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["idServicio"]);
            if (Convert.ToBoolean(ViewState["visualizaBoton"]) || !Convert.ToBoolean(ViewState["visualizaBoton"]))
                this.VisualizaBotonGuardar = Convert.ToBoolean(ViewState["visualizaBoton"]);
        }
        /// <summary>
        /// Método encargado de Configurar los Controles
        /// </summary>
        private void configuraControlesFull()
        {
            //Declarando variables Auxiliares
            string temp_max1, temp_med1, temp_min1, temp_max2, temp_med2, temp_min2;
            bool full = false;

            //Obteniendo Valores
            SAT_CL.Global.Referencia.ObtieneTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out temp_max1, out temp_med1, out temp_min1, out full,
                                                                                    out temp_max2, out temp_med2, out temp_min2);
            
            //Validando que existan Valores Full
            if (!temp_max2.Equals("") || !temp_med2.Equals("") || !temp_min2.Equals(""))
            {
                //Validando que marquen el control
                if (!chkFull.Checked)

                    //Desmarcando Control
                    chkFull.Checked = true;
            }
                
            
            //habilitando Controles
            txtTMaxima2.Enabled =
            txtTMedia2.Enabled =
            txtTMinima2.Enabled = chkFull.Checked;

            //Validando el Control
            if (!chkFull.Checked)
            {
                //Limpiando Controles
                txtTMaxima2.Text =
                txtTMedia2.Text =
                txtTMinima2.Text = "0";
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        public void InicializaTemperaturasServicio()
        {
            //Invocando Método Privado
            inicializaControlUsuario(0);
        }
        /// <summary>
        /// Método encargado de Inicializar el Control dado un Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        public void InicializaTemperaturasServicio(int id_servicio)
        {
            //Invocando Método Privado
            inicializaControlUsuario(id_servicio);
        }
        /// <summary>
        /// Método encargado de Guardar las Temperaturas del Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaTemperaturas()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista el Servicio
            if (this._id_servicio > 0)
            {
                //Declarando variables Auxiliares
                string temp_max1, temp_med1, temp_min1, temp_max2, temp_med2, temp_min2;
                bool full = false;

                //Validando que existan las Temperaturas
                if (SAT_CL.Global.Referencia.ObtieneTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out temp_max1, out temp_med1, out temp_min1, out full,
                                                                                        out temp_max2, out temp_med2, out temp_min2))

                    //Método encargado de Actualizar las Referencias
                    result = SAT_CL.Global.Referencia.EditaTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtMaxima1.Text, txtMedia1.Text, txtMinima1.Text,
                                    chkFull.Checked, txtTMaxima2.Text, txtTMedia2.Text, txtTMinima2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                else
                    //Método encargado de Ingresar las Referencias
                    result = SAT_CL.Global.Referencia.InsertaTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtMaxima1.Text, txtMedia1.Text, txtMinima1.Text,
                                    chkFull.Checked, txtTMaxima2.Text, txtTMedia2.Text, txtTMinima2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validando que la Operación fuese Exitosa
                if (result.OperacionExitosa)

                    //Inicializando Control
                    inicializaControlUsuario(result.IdRegistro);
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No Existe el Servicio");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Guardar las Temperaturas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public RetornoOperacion GuardaTemperaturas(int id_servicio)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Asignado Atributo
            this._id_servicio = id_servicio;

            //Declarando variables Auxiliares
            string temp_max1, temp_med1, temp_min1, temp_max2, temp_med2, temp_min2;
            bool full = false;

            //Validando que existan las Temperaturas
            if (SAT_CL.Global.Referencia.ObtieneTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out temp_max1, out temp_med1, out temp_min1, out full,
                                                                                    out temp_max2, out temp_med2, out temp_min2))

                //Método encargado de Actualizar las Referencias
                result = SAT_CL.Global.Referencia.EditaTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtMaxima1.Text, txtMedia1.Text, txtMinima1.Text,
                                chkFull.Checked, txtTMaxima2.Text, txtTMedia2.Text, txtTMinima2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            else
                //Método encargado de Ingresar las Referencias
                result = SAT_CL.Global.Referencia.InsertaTemperaturasServicio(this._id_servicio, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtMaxima1.Text, txtMedia1.Text, txtMinima1.Text, chkFull.Checked,
                                txtTMaxima2.Text, txtTMedia2.Text, txtTMinima2.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Control
                inicializaControlUsuario(result.IdRegistro);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}