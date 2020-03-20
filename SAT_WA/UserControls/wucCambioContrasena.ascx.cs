using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucCambioContrasena : System.Web.UI.UserControl
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)

                //Limpiando Controles
                limpiaControles();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCambiarContrasena != null)
                //Iniciando Manejador
                OnClickCambiarContrasena(e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelar != null)
                //Iniciando Manejador
                OnClickCancelar(e);
        }

        #region Eventos (Manejadores)

        /// <summary>
        /// Manejador de Evento "Guardar"
        /// </summary>
        public event EventHandler ClickCambiarContrasena;
        /// <summary>
        /// Manejador de Evento "Cancelar"
        /// </summary>
        public event EventHandler ClickCancelar;
        /// <summary>
        /// Evento que Manipula el Manejador "Cambiar Contraseña"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCambiarContrasena(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickCambiarContrasena != null)
                //Iniciando Evento
                ClickCambiarContrasena(this, e);
        }
        /// <summary>
        /// Evento que Manipula el Manejador "Cancelar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelar(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCancelar != null)
                //Iniciando Evento
                ClickCancelar(this, e);
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControles()
        {
            //Limpiando Controles
            txtClave.Text =
            txtClaveNueva.Text =
            txtClaveNueva2.Text =
            lblError.Text = "";
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar el Control de Usuario
        /// </summary>
        public void InicializaControlUsuario()
        {
            //Invocando Método de Limpieza
            limpiaControles();
        }
        
        /// <summary>
        /// Método encargado de Actualizar la Contraseña del Usuario
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ActualizaContrasenaUsuario()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(IsolationLevel.ReadCommitted))
            {
                //Instanciando Usuario
                using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))

                //Instanciando Usuario Sesión
                using (SAT_CL.Seguridad.UsuarioSesion us = new SAT_CL.Seguridad.UsuarioSesion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario_sesion))
                {
                    //Validando que Exista el Usuario
                    if (user.id_usuario > 0)
                    {
                        //Editando Contraseña
                        result = user.EditaContrasena(txtClaveNueva.Text, txtClave.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando que la Operación fuese Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Sesion
                            us.ActualizaUsuarioSesion();
                            
                            //Finalizando Sesiones
                            result = us.FinalizaSesionesUsuario();

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Actualizando Atributos
                                user.ActualizaAtributos();

                                //Actualizando Objeto de Sessión
                                Session["usuario"] = user;

                                //Instanciando Usuario
                                result = new RetornoOperacion(user.id_usuario);

                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe el Usuario");
                }
            }

            //Limpiando Controles
            limpiaControles();

            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Cancelar el Panel del Control
        /// </summary>
        /// <param name="identificador_script"></param>
        /// <param name="nombre_panel"></param>
        public void CancelaContrasenaUsuario(string identificador_script, params string[] nombre_panel)
        {
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelar, upbtnCancelar.GetType(), identificador_script, nombre_panel);
        }

        #endregion
    }
}