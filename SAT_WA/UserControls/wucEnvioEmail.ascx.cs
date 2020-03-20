using SAT_CL.FacturacionElectronica33;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucEnvioEmail : System.Web.UI.UserControl
    {
        #region Atributos

        private string _titulo_correo;
        private string _archvios64_adjuntos;

        #endregion

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

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_asunto"] = txtAsunto.Text;
            ViewState["_destinatarios"] = txtDestinatariosEmail.Text;
            ViewState["_mensaje"] = txtMensaje.Text;
            ViewState["_adjuntos"] = this._archvios64_adjuntos;
            ViewState["_titulo"] = this._titulo_correo;
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

            //Validando Titulo
            if (ViewState["_titulo"] != null)

                //Asignando Valor
                this._titulo_correo = ViewState["_titulo"].ToString();

            //Validando que existan Adjuntos
            if (ViewState["_adjuntos"] != null)

                //Asignando Adjuntos
                this._archvios64_adjuntos = ViewState["_adjuntos"].ToString();
        }
        /// <summary>
        /// Método encargado de construir el Envio de los archivos
        /// </summary>
        /// <returns></returns>
        private List<Tuple<string, byte[], string>> ConvierteAdjuntos()
        {
            //Declarando Objeto de Retorno
            List<Tuple<string, byte[], string>> adjuntos = new List<Tuple<string, byte[], string>>();

            //Validando Adjuntos
            if (!this._archvios64_adjuntos.Equals(""))
            {
                //Validando que son Varios Archivos
                string[] archivos = this._archvios64_adjuntos.Split(new string[]{"&&"}, StringSplitOptions.None);

                //Validando Archivos
                if (archivos.Length > 0)
                {
                    //Recorriendo Archivos
                    foreach (string archivo_comp in archivos)
                    {
                        //Validando que son Varios Archivos
                        string[] archivo = archivo_comp.Split(new string[] {"|"}, StringSplitOptions.None);

                        //Validando que exista el Nombre, el Archivo y la Extensión
                        if (archivo.Length == 3)
                        {
                            //Creando y Añadiendo Elemento de Tupla
                            adjuntos.Add(
                                new Tuple<string, byte[], string>(
                                    archivo[0],
                                    Convert.FromBase64String(archivo[1]),
                                    archivo[2]));
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return adjuntos;
        }
        /// <summary>
        /// Método encargado de Generar el Cuerpo del Mensaje
        /// </summary>
        /// <returns></returns>
        private string generaCuerpoMensajeEmail()
        {
            //Formato redeterminado
            string formato = File.ReadAllText(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "EnvioEmail1.html");

            //Obteniendo Imagenes
            string logoTectosBase64 = Convert.ToBase64String(File.ReadAllBytes(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "LogoTECTOS_2.png")),
                   logoCompania = "";
            
            //Obteniendo Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando Compania
                if (emisor.habilitar)
                {
                    //Obteniendo Logo de la Compania
                    logoCompania = Convert.ToBase64String(File.ReadAllBytes(emisor.ruta_logotipo));

                    /**** Personalizando Cuerpo del Correo ****/
                    //Titulo del Correo
                    formato = formato.Replace("{0}", this._titulo_correo);
                    formato = formato.Replace("{1}", emisor.nombre.ToUpper());
                    formato = formato.Replace("{2}", txtMensaje.Text);
                    formato = formato.Replace("{3}", logoCompania);
                    formato = formato.Replace("{4}", logoTectosBase64);
                }
            }

            //Devolviendo mensaje
            return formato;
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
        /// <param name="archivo_adjunto"></param>
        public void InicializaControl(string titulo, string remitente, string asunto, string destinatarios, string mensaje, Tuple<string, byte[], string> archivo_adjunto)
        {
            //Inicializando Atributos
            List<Tuple<string, byte[], string>> list = new List<Tuple<string, byte[], string>>();
            list.Add(archivo_adjunto);
            //Asignando atributos internos
            InicializaControl(titulo, remitente, asunto, destinatarios, mensaje, list);
        }
        /// <summary>
        /// Inicializa el contenido del control de usuario
        /// </summary>
        /// <param name="remitente"></param>
        /// <param name="asunto"></param>
        /// <param name="destinatarios"></param>
        /// <param name="mensaje"></param>
        /// <param name="comprobantes"></param>
        public void InicializaControl(string titulo, string remitente, string asunto, string destinatarios, string mensaje, List<Tuple<string, byte[], string>> archivos_adjuntos)
        {
            //Asignando Atributos
            ViewState["_remitente"] = remitente;
            txtAsunto.Text = asunto;
            txtDestinatariosEmail.Text = destinatarios;
            txtMensaje.Text = mensaje;
            _archvios64_adjuntos = "";

            /**** Gestionando Adjuntos ****/
            //Validando Adjuntos
            if (archivos_adjuntos.Count > 0)
            {
                //Recorriendo Adjuntos
                foreach (Tuple<string, byte[], string> adj in archivos_adjuntos)
                {
                    //Validando archivos Anteriores
                    if (_archvios64_adjuntos.Equals(""))

                        //Asignando Valores
                        _archvios64_adjuntos = string.Format("{0}|{1}|{2}", adj.Item1, Convert.ToBase64String(adj.Item2), adj.Item3);
                    else
                        //Asignando Valores
                        _archvios64_adjuntos += string.Format("&&{0}|{1}|{2}", adj.Item1, Convert.ToBase64String(adj.Item2), adj.Item3);
                }

                //Asignando ViewState
                ViewState["_adjuntos"] = _archvios64_adjuntos;
            }

            //Asignando atributos
            asignaAtributos();
        }
        /// <summary>
        /// Realiza el envío del correo electrónico con los conprobantes solicitados
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EnviaEmail()
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
                    
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Una o más direcciones de correo electrónico no poseen formato válido.");
            }
            else
                resultado = new RetornoOperacion("No se han agregado direcciones de correo electrónico.");

            //Si no hay errores hasta este punto
            if (resultado.OperacionExitosa)
            {
                //Recuperando lista de comprobantes
                List<Tuple<string, byte[], string>> adjuntos = ConvierteAdjuntos();

                //Enviamos Email
                //Instanciando Correo Electronico
                using (Correo email = new Correo(ViewState["_remitente"].ToString(), destinatarios, txtAsunto.Text, generaCuerpoMensajeEmail(), true))
                {
                    //Validando adjuntos
                    if (adjuntos.Count > 0)
                    {
                        //Recorriendo Archivos Adjuntos
                        foreach (Tuple<string, byte[], string> archivo in adjuntos)
                        {
                            //Creando representación impresa (pdf)
                            MemoryStream flujo = new MemoryStream(archivo.Item2);
                            //Adjuntando archivo pdf
                            email.ArchivosAdjuntos.Add(new System.Net.Mail.Attachment(flujo, string.Format("{0}.{1}", archivo.Item1, archivo.Item3)));
                        }
                    }

                    //Enviando Correo Electronico
                    bool enviar = email.Enviar();

                    //Si no se envío el mensaje
                    if (!enviar)
                    {
                        string errores = "";
                        //Recorriendo los errores del envío
                        foreach (string error in email.Errores)
                            //Añadiendo errores a la lista
                            errores = errores + error + "<br />";
                        resultado = new RetornoOperacion(errores);
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}