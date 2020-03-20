using SAT_CL.Global;
using System;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucCambioOperador : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _id_unidad;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;
        /// <summary>
        /// Declaración de Evento ClickRegistrar
        /// </summary>
        public event EventHandler ClickRegistrar;


        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Tab
                txtNvoOperador.TabIndex =
                txtFechaInicioAsignacion.TabIndex =value;
            }
            get { return txtNvoOperador.TabIndex; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                
            }
            else
            {
                //Si es PostaBack
                //Recupera Atributos
                recuperaAtributos();
            }


        }
        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }

        /// <summary>
        /// Manipula Evento ClickRegistrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickRegistrar(EventArgs e)
        {
            if (ClickRegistrar != null)
                ClickRegistrar(this, e);

        }

        /// Evento disparado al presionar el boton Registrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnRegistrar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickRegistrar != null)
                OnClickRegistrar(new EventArgs());
        }

        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdUnidad"]) != 0)
                this._id_unidad = Convert.ToInt32(ViewState["IdUnidad"]);
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdUnidad"] = this._id_unidad;
        }

        /// <summary>
        /// Eveno generado al cambiar la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUnidad_TextChanged(object sender, EventArgs e)
        {
            //Asignamos Valor 
            this._id_unidad = Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ":", 1), "0"));
            //Validamos que exista la Unidad
            if (this._id_unidad != 0)
            {
                //Inicializamos Operador Actual
                inicializaControlOperadorActual();
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Inicializamos Control Operador Actual
        /// </summary>
        private void inicializaControlOperadorActual()
        {
            //Instanciando última asignación de operador para esta unidad
            using (SAT_CL.Global.AsignacionOperadorUnidad asignacion = SAT_CL.Global.AsignacionOperadorUnidad.ObtieneAsignacionActiva(SAT_CL.Global.AsignacionOperadorUnidad.TipoBusquedaAsignacion.Unidad, this._id_unidad))
            //Instanciando operador involucrado
            using (SAT_CL.Global.Operador operador = new SAT_CL.Global.Operador(asignacion.id_operador))
                //Mostrando datos del operador actual
                lblOperadorActual.Text = string.Format("{0}   [ Desde '{1}' - Hasta '{2}' ]", operador.id_operador > 0 ? operador.nombre : "Sin Asignar",
                                                                                                                            !Fecha.EsFechaMinima(asignacion.fecha_inicio) ? asignacion.fecha_inicio.ToString("dd/MM/yyyy HH:mm") : "",
                                                                                                                           !Fecha.EsFechaMinima(asignacion.fecha_fin) ? asignacion.fecha_fin.ToString("dd/MM/yyyy HH:mm") : "");
        }
        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validamos que exista la Unidad
            if(this._id_unidad !=0)
            {
            //Inicializamos Operador Actual
             inicializaControlOperadorActual();
            //Intsanciamos Unidad
            using(SAT_CL.Global.Unidad objUnidad = new Unidad(this._id_unidad))
            { 
            //Inicializamos Control de Unidad
                txtUnidad.Text = objUnidad.numero_unidad + " ID:" + objUnidad.id_unidad.ToString();
            }
            //Limpiando texto para nombre de nvo operador
            txtNvoOperador.Text = "";       
            //Asignando foco
            txtNvoOperador.Focus();
            }
            else
            {
                //Habilitamos Control de Unidad
                txtUnidad.Enabled = true;
                //Limpiamos Controles
                txtUnidad.Text = "";
                txtNvoOperador.Text = "";
                lblOperadorActual.Text = "";
            }
            //Inicializamos Controles
            txtFechaInicioAsignacion.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");  
        }

        /// <summary>
        /// Método encargado de Cambiar al Operador
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion CambioOperador()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando unidad actual
            using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(this._id_unidad))
            {
                //Si la unidad existe
                if (u.habilitar)
                    //Actualizando asignación de operador a unidad
                    resultado = u.ReemplazaOperadorAsignado(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtNvoOperador.Text, "ID:", 1)), Convert.ToDateTime(txtFechaInicioAsignacion.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Mostrando Error
                    resultado = new RetornoOperacion("La unidad no fue encontrada.");
            }
            //Devolvemos Resutltado
            return resultado;
        }

        /// <summary>
        /// Inicializa Control
        /// </summary>
        /// <param name="idUnidad">Id Unidad</param>
        public void InicializaControl(int idUnidad)
        {
            //Asignando Atributos
            this._id_unidad = idUnidad;
            
            //Inicializamos Valores
            inicializaValores();

        }

        /// <summary>
        /// Inicializamos Control
        /// </summary>
        /// <param name="idUnidad"></param>
        public void InicializaControl()
        {
            //Asignando Atributos
            this._id_unidad = 0;
            //Inicializamos Valores
            inicializaValores();

        }
        #endregion
    }
}