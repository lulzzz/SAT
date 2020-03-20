using SAT_CL.FacturacionElectronica33;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucEmailCFDIV3_3 : System.Web.UI.UserControl
    {
        #region Manejador de Eventos

        /// <summary>
        /// Manejador de Evento Click en Cerrar Control
        /// </summary>
        public event EventHandler LkbCerrarEmail_Click;
        /// <summary>
        /// Manejador de Evento CLick en Enviar Email
        /// </summary>
        public event EventHandler BtnEnviarEmail_Click;

        /// <summary>
        /// Maneja Click en Cerrar Control
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnLkbCerrarEmail_Click(EventArgs e)
        {
            if (this.LkbCerrarEmail_Click != null)
                this.LkbCerrarEmail_Click(this, e);
        }
        /// <summary>
        /// Maneja Click en Cerrar Control
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnBtnEnviarEmail_Click(EventArgs e)
        {
            if (this.BtnEnviarEmail_Click != null)
                this.BtnEnviarEmail_Click(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Inicio de control de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recarga de pagina
            if (!Page.IsPostBack)
                recuperaAtributos();
            else
                asignaAtributos();
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Recuperando Atributos
            asignaAtributos();
        }
        /// <summary>
        /// Click en botón cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarEmail_Click(object sender, EventArgs e)
        {
            //Generando evento click del control correspondiente
            if (this.LkbCerrarEmail_Click != null)
                OnLkbCerrarEmail_Click(e);
        }
        /// <summary>
        /// Click en enviar email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEmail_Click(object sender, EventArgs e)
        {
            //Generando evento click del control correspondiente
            if (this.BtnEnviarEmail_Click != null)
                OnBtnEnviarEmail_Click(e);
        }
        /// <summary>
        /// Cambio de texto en destinatarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDestinatariosEmail_TextChanged(object sender, EventArgs e)
        {
            asignaAtributos();
        }
        /// <summary>
        /// Cambio de texto en mensaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMensaje_TextChanged(object sender, EventArgs e)
        {
            asignaAtributos();
        }
        /// <summary>
        /// Cambio de asunto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtAsunto_TextChanged(object sender, EventArgs e)
        {
            asignaAtributos();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido del control de usuario
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="asunto"></param>
        /// <param name="destinatarios"></param>
        /// <param name="mensaje"></param>
        /// <param name="id_comprobante"></param>
        public void InicializaControl(string remitente, string asunto, string destinatarios, string mensaje, int id_comprobante)
        {
            List<int> l = new List<int>();
            l.Add(id_comprobante);
            //Asignando atributos internos
            InicializaControl(remitente, asunto, destinatarios, mensaje, l);
        }

        /// <summary>
        /// Inicializa el contenido del control de usuario
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="asunto"></param>
        /// <param name="destinatarios"></param>
        /// <param name="mensaje"></param>
        /// <param name="comprobantes"></param>
        public void InicializaControl(string remitente, string asunto, string destinatarios, string mensaje, List<int> comprobantes)
        {
            ViewState["_remitente"] = remitente;
            ViewState["_comprobantes"] = comprobantes;
            txtAsunto.Text = asunto;
            txtDestinatariosEmail.Text = destinatarios;
            txtMensaje.Text = mensaje;
        }
        /// <summary>
        /// Realiza el envío del correo electrónico con los conprobantes solicitados
        /// </summary>
        /// <param name="xml">True para enviar archivo xml</param>
        /// <param name="pdf">True para enviar archivo pdf</param>
        /// <returns></returns>
        public RetornoOperacion EnviaEmail(bool xml, bool pdf)
        {
            //eclarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Creando validador de direcciones de correo electrónico
            System.Text.RegularExpressions.Regex regexv = new System.Text.RegularExpressions.Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            //Obteniendo direcciones de correo
            string[] destinatarios = txtDestinatariosEmail.Text.Replace("\n", "").Replace("\r", "").Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //Validando existencia de direcciones
            if (destinatarios.Length > 0)
            {
                //Validando el conjunto de direcciones
                if (destinatarios.Length != (from string email in destinatarios
                                             where regexv.IsMatch(email)
                                             select email).Count())
                    resultado = new RetornoOperacion("Una o más direcciones de correo electrónico no poseen formato válido.");
            }
            else
                resultado = new RetornoOperacion("No se han agregado direcciones de correo electrónico.");

            //Si no hay errores hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Recuperando lista de comprobantes
                List<int> comprobantes = (List<int>)ViewState["_comprobantes"];

                //Determinando el número de comprobantes a enviar
                //Si hay uno
                if (comprobantes.Count == 1)
                {
                    //Instanciando comprobante
                    using (Comprobante c = new Comprobante(comprobantes.First()))
                    {
                        //Enviando e mail
                        resultado = c.EnviaArchivosEmailV3_3(ViewState["_remitente"].ToString(), txtAsunto.Text, txtMensaje.Text, destinatarios, pdf, xml);
                    }
                }
                //Si hay más de uno
                else if (comprobantes.Count > 1)
                {

                    //TODO: IMPLEMENTAR LÓGICA DE ENVÍO MULTIPLE DE COMPROBANTES AL MISMO CONJUNTO DE DESTINATARIOS

                }
                else
                    resultado = new RetornoOperacion("No hay comprobantes por envíar.");
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_asunto"] = txtAsunto.Text;
            ViewState["_destinatarios"] = txtDestinatariosEmail.Text;
            ViewState["_mensaje"] = txtMensaje.Text;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["_asunto"] != null && ViewState["_destinatarios"] != null
                && ViewState["_mensaje"] != null)
            {
                txtAsunto.Text = ViewState["_asunto"].ToString();
                txtDestinatariosEmail.Text = ViewState["_destinatarios"].ToString();
                txtMensaje.Text = ViewState["_mensaje"].ToString();
            }
        }

        #endregion
    }
}