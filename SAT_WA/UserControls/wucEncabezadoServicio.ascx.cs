using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.ASP;
namespace SAT.UserControls
{
    public partial class wucEncabezadoServicio : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_servicio;

        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                txtReferencia.TabIndex =
                txtCartaPorte.TabIndex =
                txtObservacion.TabIndex = 
                btnGuardarRef.TabIndex = 
                btnCancelarRef.TabIndex = value;
            }
            get { return txtReferencia.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor de habilitación de los controles contenidos
        /// </summary>
        public bool Enabled
        {
            set
            {
                //Asignando Habilitación
                txtReferencia.Enabled =
                txtCartaPorte.Enabled =
                txtObservacion.Enabled =
                btnGuardarRef.Enabled =
                btnCancelarRef.Enabled = value;
            }
            get { return txtReferencia.Enabled; }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efecturse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
            {
                //Asignando Atributos
                ViewState["IdServicio"] = this._id_servicio;
            }
            else
            {    
                //Validando si existe el Valor
                if (Convert.ToInt32(ViewState["IdServicio"]) > 0)

                    //Recuperando Atributos
                    this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
                }
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Asignando Atributos
            ViewState["IdServicio"] = this._id_servicio;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarRef_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarReferencia != null)
                
                //Iniciando Manejador
                OnClickGuardarReferencia(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarRef_Click(object sender, EventArgs e)
        {
            //Invocando Método de Inicialización
            inicializaControl();
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarReferencia;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar Referencia"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarReferencia(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardarReferencia != null)

                //Iniciando Evento
                ClickGuardarReferencia(this, e);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
            {
                //Intsanciamos Cliente
                using (SAT_CL.Global.CompaniaEmisorReceptor objCliente = new SAT_CL.Global.CompaniaEmisorReceptor(serv.id_cliente_receptor))
                {
                    //Validando que existe
                    if (serv.id_servicio > 0)
                    {
                        //Asignando Valores
                        lblNoServicio.InnerText = "Encabezado del Servicio " + serv.no_servicio;
                        txtCliente.Text = objCliente.nombre+ " ID:" + objCliente.id_compania_emisor_receptor.ToString();
                        txtReferencia.Text = serv.referencia_cliente;
                        txtCartaPorte.Text = serv.porte;
                        txtObservacion.Text = serv.observacion_servicio;
                    }
                    else
                    {
                        //Asignando Valores
                        lblNoServicio.InnerText = "Por Asignar";
                        txtCliente.Text = "";
                        txtReferencia.Text =
                        txtCartaPorte.Text =
                        txtObservacion.Text = "";
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Inicializar el Control del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        public void InicializaEncabezadoServicio(int id_servicio)
        {
            //Asignando Atributo
            this._id_servicio = id_servicio;

            //Invocando el Método de Inicialización
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Guardar el Encabezado del Servicio (Referencias)
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaEncabezadoServicio()
        {
            //Declarando Objeto 
            RetornoOperacion result = new RetornoOperacion();
            //Declaramos Objeto Resultado para la Edición de Cliente
           RetornoOperacion  resultadoCliente = new RetornoOperacion();
            //Declaramos Objeto Resultado para Edición de la Referencia Principal
           RetornoOperacion resultadoReferenciaPrincipal = new RetornoOperacion();
            //Declaramos Objeto Resultado para la Carta Porte
           RetornoOperacion resultadoPorte = new RetornoOperacion();
            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
            {
                //Validando que existe
                if (serv.id_servicio > 0)

                    //Actualizando la referencia del Encabezado
                    result = serv.ActualizaReferenciaServicio(txtCartaPorte.Text.ToUpper(), txtReferencia.Text.ToUpper(), txtObservacion.Text.ToUpper(),
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, ':', 1)),out resultadoCliente, out resultadoReferenciaPrincipal, out resultadoPorte,
                         ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe un Servicio");
            }

            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnGuardarRef, "ResultadoCliente", resultadoCliente.Mensaje, resultadoCliente.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnGuardarRef, "ResultadoReferencia", resultadoReferenciaPrincipal.Mensaje, resultadoReferenciaPrincipal.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Muestra el mensaje de error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnGuardarRef, "ResultadoObservaciones" ,resultadoPorte.Mensaje,resultadoPorte.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}