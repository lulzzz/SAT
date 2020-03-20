using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.Seguridad
{
    public partial class PerfilSeguridadUsuario : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)
                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(149, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaPerfilUsuario();
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (psu.id_perfil_usuario != 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Deshabilitando Producto
                                result = psu.DeshabilitaPerfilSeguridadUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaPagina();
                                }

                                //Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "149", "Perfil Usuario");
                        break;
                    }
                case "Referencias":
                    {
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "149", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaPerfilUsuario();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    //Asignando a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
                default:
                    //Asignando a Nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    break;
            }

            //Invocando Inicialización de Página
            inicializaPagina();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tipos de Método de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfil, 61, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }
        /// <summary>
        /// Método encargado de habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:

                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Habilitación
                        lblPerfilUsuarioID.Enabled =
                        ddlPerfil.Enabled =
                        txtUsuario.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Habilitación
                        lblPerfilUsuarioID.Enabled =
                        ddlPerfil.Enabled =
                        txtUsuario.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Valores
                        lblPerfilUsuarioID.Text = "Por Asignar";
                        txtUsuario.Text = "";
                        chkActivo.Checked = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Producto
                        using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (psu.id_perfil_usuario > 0)
                            {
                                //Inicializando Valores
                                lblPerfilUsuarioID.Text = psu.id_perfil_usuario.ToString();
                                ddlPerfil.SelectedValue = psu.id_perfil.ToString();
                                chkActivo.Checked = psu.perfil_activo;
                                
                                //Instanciando Usuario
                                using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(psu.id_usuario))
                                {
                                    //Validando que Exista
                                    if (user.id_usuario > 0)

                                        //Asignando Valor
                                        txtUsuario.Text = user.nombre + " ID:" + user.id_usuario.ToString();
                                    else
                                        //Limpiando Control
                                        txtUsuario.Text = "";
                                }
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Guardar los Perfiles de Seguridad
        /// </summary>
        private void guardaPerfilUsuario()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertando Perfil
                        result = SAT_CL.Seguridad.PerfilSeguridadUsuario.InsertaPerfilSeguridadUsuario(Convert.ToInt32(ddlPerfil.SelectedValue),
                                       Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUsuario.Text, "ID:", 1, "0")),
                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Producto
                        using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (psu.id_perfil_usuario > 0)

                                //Editando Perfil
                                result = psu.EditaPerfilSeguridadUsuario(Convert.ToInt32(ddlPerfil.SelectedValue),
                                               Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUsuario.Text, "ID:", 1, "0")), chkActivo.Checked,
                                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        }
                        break;
                    }
            }

            //Validando que la Operación haya sido Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Valores de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;

                //Inovcando Método de Inicialización
                inicializaPagina();
            }

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Seguridad/PerfilSeguridad.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Seguridad/PerfilSeguridad.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Seguridad/PerfilSeguridad.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        #endregion
    }
}