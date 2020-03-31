using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.General
{
    public partial class Contacto : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recarga de página
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Click en botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            guardaContacto();
        }
        /// <summary>
        /// Click en botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Valida cada estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la pagina se encuentra en modo  Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //Si el estado de la pagina se encuentra en modo de Edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable session estatus el valor actual de la pagina.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma();
            inicializaForma();
        }

        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asigna a la variable de sesión de estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de sesión id_registro el valor de 0
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Limpiando mensaje de error del lblError
                        lblError.Text = "";
                        //Hace un enfoque en el primer control
                        txtNombre.Focus();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(180, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaContacto();
                        break;
                    }
                case "Editar":
                    {
                        //Asigna a la variable de sesión el estatus de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma()
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        //Hace enfoque en el primer control
                        txtNombre.Focus();
                        break;
                    }
                case "Eliminar":
                    {
                        //Invocanto al método de Eliminación
                        bajaContacto();
                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "180", "Contacto");
                        break;
                    }
                //Si la elección del menú es la opción Gestionar Tokens
                case "Tokens":
                    {
                        //Titulo de Control
                        h2EncabezadoGestionTokens.InnerText = string.Format("Gestión de Tokens del Contacto '{0}'", txtNombre.Text);
                        //ALternando Ventana
                        ScriptServer.AlternarVentana(this.Page, "VerGestionTokens", "contenedorGestionTokens", "ventanaGestionTokens");
                        //Invoca al método de carga del gestor de Tokens
                        CargaGestorTokens(Convert.ToInt32(Session["id_registro"]));
                        break;
                    }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catalogos
            cargaCatalogos();
            //Habilitando controles
            habilitaControles();
            //Habilitando menú
            habilitaMenu();
            //Cargando contenido de controles
            cargaContenidoControles();
            //Inicializando GridView de Gestión de Tokens
            Controles.InicializaGridview(gvGestionTokens);
        }

        private void cargaCatalogos()
        {
            //Cargando catálogo de perfiles de reportes
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfil, 199, "-- Seleccione un Perfil de la Lista", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewGestionTokens, "", 18);
        }

        private void cargaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Borrando el contenido 
                    lblId.Text = "Por Asignar";
                    txtNombre.Text =
                    txtTelefono.Text =
                    txtEmail.Text = "";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando registro de contacto
                    using (SAT_CL.Global.Contacto c = new SAT_CL.Global.Contacto(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (c.habilitar)
                        {
                            //Borrando el contenido 
                            lblId.Text = c.id_contacto.ToString();
                            txtNombre.Text = c.nombre;
                            txtTelefono.Text = c.telefono.ToString();
                            txtEmail.Text = c.email;
                            using (PerfilSeguridadUsuario psu = PerfilSeguridadUsuario.ObtienePerfilActivo(c.id_usuario_sistema))
                            {
                                if (psu.id_usuario > 0)
                                {
                                    ddlPerfil.SelectedValue = psu.id_perfil.ToString();
                                }
                            }
                        }
                    }
                    break;
            }

            //Limpiando errores
            lblError.Text = "";
        }

        /// <summary>
        /// Habilita o deshabilita los controles de la forma con base al estatus
        /// </summary>
        private void habilitaControles()
        {
            //Con base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    txtNombre.Enabled =
                    txtTelefono.Enabled =
                    txtEmail.Enabled =
                    ddlPerfil.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    txtNombre.Enabled =
                    txtTelefono.Enabled =
                    txtEmail.Enabled =
                    ddlPerfil.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtNombre.Enabled =
                    txtTelefono.Enabled =
                    txtEmail.Enabled = true;
                    ddlPerfil.Enabled = false;
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Método Privado encargado de Habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Validando estatus de Session
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled = false;
                        lkbTokens.Enabled = false;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled = true;
                        lkbTokens.Enabled = true;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbBajaEliminar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled = false;
                        lkbTokens.Enabled = false;
                        break;
                    }

            }
        }

        /// <summary>
        /// Inserta o actualiza los valores del registro
        /// </summary>
        private void guardaContacto()
        {
            //Declarando Variables Auxiliares
            int id_usuario = 0;
            string pwdAleatoria = TSDK.Base.Cadena.CadenaAleatoria(20);
            string QtnAwr = TSDK.Base.Cadena.CadenaAleatoria(20);
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            RetornoOperacion resultadoP = new RetornoOperacion();
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //En base al estatus
                switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
                {
                    case TSDK.ASP.Pagina.Estatus.Nuevo:
                        //Validando que se halla seleccionado un Perfil
                        if (ddlPerfil.SelectedValue != "0")
                        {
                            //Asigna al objeto retorno los valores obtenidos del formulario Contacto, invocando al método de insercion de la clase usuario.
                            resultado = SAT_CL.Seguridad.Usuario.InsertaUsuario(txtNombre.Text, txtEmail.Text, pwdAleatoria, DateTime.Today,
                                                                          0, 0, "NA", QtnAwr, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (resultado.OperacionExitosa)
                            {
                                //Obteniendo Registro de Usuario
                                id_usuario = resultado.IdRegistro;

                                //Insertando Usuario - Compania
                                resultado = SAT_CL.Seguridad.UsuarioCompania.InsertaUsuarioCompania(id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                            1, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Perfil
                                    resultado = SAT_CL.Seguridad.PerfilSeguridadUsuario.InsertaPerfilSeguridadUsuario(Convert.ToInt32(ddlPerfil.SelectedValue), id_usuario,
                                                                   ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    if (resultado.OperacionExitosa)
                                    {
                                        //Registrando el Contacto con el ID de Usuario del sistema generado
                                        resultado = SAT_CL.Global.Contacto.InsertaContacto(
                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            txtNombre.Text.ToUpper(),
                                            txtTelefono.Text,
                                            txtEmail.Text,
                                            id_usuario,
                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                        );
                                    }
                                }
                            }

                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("* Seleccione un Perfil de la Lista");
                        break;
                    case TSDK.ASP.Pagina.Estatus.Edicion:
                        //Validando que se halla seleccionado un Perfil
                        if (ddlPerfil.SelectedValue != "0")
                        {
                            //Instanciando contacto actual
                            using (SAT_CL.Global.Contacto c = new SAT_CL.Global.Contacto(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Si el Contacto existe
                                if (c.habilitar)
                                {
                                    //Insertando Nuevo Perfil Asignado (Si es que lo hay)
                                    /*resultadoP = SAT_CL.Seguridad.PerfilSeguridadUsuario.InsertaPerfilSeguridadUsuario(Convert.ToInt32(ddlPerfil.SelectedValue), c.id_usuario_sistema,
                                                                   ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);*/

                                    resultado = c.EditaContacto(
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                        txtNombre.Text.ToUpper(),
                                        txtTelefono.Text,
                                        txtEmail.Text,
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                    );

                                    if (resultado.OperacionExitosa)
                                    {
                                        using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(c.id_usuario_sistema))
                                        {
                                            if (u.id_usuario > 0)
                                            {
                                                Usuario OldU = u;
                                                resultado = u.EditaInformacionGeneral(
                                                    txtNombre.Text.ToUpper(),
                                                    txtEmail.Text,
                                                    OldU.sesiones_disponibles,
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                                );
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("* Seleccione un Perfil de la Lista");
                        break;
                }

                //Valida que la operacion de inserción se realizo correctamente
                if (resultado.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                    //Completando Transacción
                    trans.Complete();
                }
                else
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                inicializaForma();
            }

        }

        private void bajaContacto()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Invoca al constructor de la clase y asigna el valor de la variable de session id_registro.
                using (SAT_CL.Global.Contacto c = new SAT_CL.Global.Contacto((int)Session["id_registro"]))
                {
                    //Valida si el registro existe.
                    if (c.id_contacto > 0)
                    {
                        //Asigna al objeto retorno los datos del usuario que realizo el cambio de estado del registro (Deshabilitó)                            
                        retorno = c.DeshabilitaContacto(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Invoca al constructor de la clase y asigna el valor de la variable de session id_registro.
                            using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(c.id_usuario_sistema))
                            {
                                //Valida si el registro existe.
                                if (us.id_usuario > 0)
                                {
                                    //Asigna al objeto retorno los datos del usuario que realizo el cambio de estado del registro(Deshabilito)                            
                                    retorno = us.DeshabilitaUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Instanciando registro de Usuario - Compania
                                        using (SAT_CL.Seguridad.UsuarioCompania uc = new SAT_CL.Seguridad.UsuarioCompania(us.id_usuario, c.id_compania_emisor))
                                        {
                                            //Validando que Existe el Registro
                                            if (uc.id_usuario_compania > 0)
                                            {
                                                //Deshabilitando 
                                                retorno = uc.DeshabilitaUsuarioCompania(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                if (retorno.OperacionExitosa)
                                                {
                                                    SAT_CL.Seguridad.PerfilSeguridadUsuario IdPSU = SAT_CL.Seguridad.PerfilSeguridadUsuario.ObtienePerfilActivo(c.id_usuario_sistema);
                                                    //Instanciando Perfil Activo de Perfil Seguridad Usuario
                                                    using (SAT_CL.Seguridad.PerfilSeguridadUsuario psu = new SAT_CL.Seguridad.PerfilSeguridadUsuario(IdPSU.id_perfil_usuario))
                                                    {
                                                        if (psu.id_perfil_usuario > 0)
                                                        {
                                                            retorno = psu.DeshabilitaPerfilSeguridadUsuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                using (UsuarioToken activo = UsuarioToken.ObtieneTokenActivo(c.id_usuario_sistema, c.id_compania_emisor))
                                                                {
                                                                    retorno = activo.TerminaUsuarioToken(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Valida que la operacion de inserción se realizo correctamente
                if (retorno.OperacionExitosa)
                {
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);

                    //Completando Transacción
                    trans.Complete();
                }
                else
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            //Valida si la operación se realizo correctamente.
            if (retorno.OperacionExitosa)
            {
                //Asigna el valor de estado lectura a la variable de session estatus 
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Asigna el valor 0 a la variable de session id_registro
                Session["id_registro"] = 0;
                //invoca al método inicializaForma().
                inicializaForma();
            }
        }

        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando sesión
            Session["id_tabla"] = idTabla;
            //Construyendo URL
            string url = Cadena.RutaRelativaAAbsoluta("~/Global/Contacto.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo configuración de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Abrir registro de contacto", configuracion, Page);
        }

        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendrá la bitácora de un registros de Contacto.
            string url = Cadena.RutaRelativaAAbsoluta("~/Global/Contacto.aspx",
                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora  Contacto", configuracion, Page);
        }
        #endregion

        #region Gestión de Tokens del Usuario consultado(GridView)
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            ScriptServer.AlternarVentana(this.Page, "VerGestionTokens", "contenedorGestionTokens", "ventanaGestionTokens");
            //Recargando Grid
            Controles.InicializaIndices(gvGestionTokens);
        }
        /// <summary>
        /// Evento que carga el tamaño de registros mostrados en el GV de Tokens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewGestionTokens_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvGestionTokens, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewGestionTokens.SelectedValue), true, 3);
        }
        protected void gvGestionTokens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvGestionTokens, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Eventio que ordena los registros del GV de Tokens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGestionTokens_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewGestionTokens.Text = Controles.CambiaSortExpressionGridView(gvGestionTokens, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento que llena el GridView de Anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGestionTokens_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //validando Fila de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando información de la fila actual
                if (e.Row.DataItem != null)
                {
                    //Obteniendo Fila de Datos
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;

                    //Validando Fila
                    if (fila != null)
                    {
                        using (ImageButton imbEmail = (ImageButton)e.Row.FindControl("imbEmail"),
                                imbMsg = (ImageButton)e.Row.FindControl("imbMsg"),
                                imbFinalizar = (ImageButton)e.Row.FindControl("imbFinalizar"))
                        {
                            switch (fila["Estatus"].ToString())
                            {
                                case "Vigente":
                                    //Coloreando fila de verde por ser un Token vigente
                                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#85D27A");
                                    break;
                                case "Inválido":
                                    //Coloreando fila de rojo por ser un Token expirado sin terminar
                                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#EC6F5A");
                                    imbEmail.Visible = false;
                                    imbMsg.Visible = false;
                                    break;
                                case "Terminado":
                                    //Ocultamos las acciones exclusivas para Tokens activos
                                    imbEmail.Visible = false;
                                    imbMsg.Visible = false;
                                    imbFinalizar.Visible = false;
                                    break;

                            }
                        }
                    }
                }
            }
        }
        private void CargaGestorTokens(int IdContacto)
        {
            //Realizando la carga de los Tokens coincidentes
            using (DataTable mit = SAT_CL.Global.Contacto.CargaTokensUsuarioContacto(IdContacto))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvGestionTokens, mit, "IdContacto-IdUsuarioCompania-IdClienteProveedor-IdUsuarioToken-IdUsuarioSistema", lblCriterioGridViewGestionTokens.Text, true, 3);

                //Si no hay registros
                if (mit == null)
                    //Elimiando de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //Si existen registros, se sobrescribe
                else
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
            }
        }
        /// <summary>
        /// Evento que genera un nuevo Token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerarToken_Click(object sender, EventArgs e)
        {
            generaToken();
        }

        protected void btnGenerarTokenVigenciaPersonalizada_Click(object sender, EventArgs e)
        {
            //ALternando Ventana
            ScriptServer.AlternarVentana(this.Page, "ElegirFechaVigencia", "contenedorFechaVigenciaToken", "ventanaFechaVigenciaToken");
        }
        /// <summary>
        /// Método que genera Tokens para el usuario especificado
        /// </summary>
        private void generaToken()
        {
            RetornoOperacion resultado = new RetornoOperacion();
            int IdContacto = Convert.ToInt32(Session["id_registro"]);
            string Token;
            using (SAT_CL.Global.Contacto C = new SAT_CL.Global.Contacto(IdContacto))
            {
                //Validando que exista el contacto
                if (C.id_contacto > 0)
                {
                    resultado = SAT_CL.Seguridad.UsuarioToken.GeneraNuevoTokenUUID(C.id_usuario_sistema, C.id_compania_emisor,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 1, out Token);
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion("Generación existosa. Nuevo Token generado: " + Token, true);
                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        CargaGestorTokens(C.id_contacto);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }

        }

        /// <summary>
        /// Método para guardar las acciones lkb del Adán
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbTokens_OnClick(object sender, EventArgs e)
        {
            RetornoOperacion resultado = new RetornoOperacion();
            switch (((LinkButton)sender).CommandName)
            {
                case "AccionToken1":
                    break;
                case "FinalizarToken":
                    if (gvGestionTokens.DataKeys.Count > 0)
                    {
                        //Seleccionando fila actual
                        Controles.SeleccionaFila(gvGestionTokens, sender, "lnk", false);

                        using (UsuarioToken UT = new UsuarioToken(Convert.ToInt32(gvGestionTokens.SelectedDataKey["IdUsuarioToken"])))
                        {
                            resultado = UT.TerminaUsuarioTokenVigencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Mostrando Mensaje de Operación
                            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            CargaGestorTokens(Convert.ToInt32(Session["id_registro"]));
                        }
                        
                    }
                    break;
            }
        }
        /// <summary>
        /// Evento producido al dar click en imagebutton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbEnvio_Click(object sender, ImageClickEventArgs e)
        {
            RetornoOperacion resultado = new RetornoOperacion();
            if (gvGestionTokens.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvGestionTokens, sender, "imb", false);
                //Validando estatus de Página
                switch (((ImageButton)sender).CommandName)
                {
                    case "Correo":
                        {
                            //Enviamos Notificación
                            resultado = SAT_CL.Notificaciones.Notificacion.EnviaCorreo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                Convert.ToInt32(gvGestionTokens.SelectedDataKey["IdContacto"]), "ACCESO A PLATAFORMA DE REPORTES", "Encabezado.", "Titulo", "Subtitulo", "TituloCuerpo ", "Cuerpo", "idS=");
                            break;
                        }
                    case "Mensaje":
                        {

                            break;
                        }
                    case "FinalizarToken":
                        {
                            using (UsuarioToken UT = new UsuarioToken(Convert.ToInt32(gvGestionTokens.SelectedDataKey["IdUsuarioToken"])))
                            {
                                resultado = UT.TerminaUsuarioTokenVigencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Mostrando Mensaje de Operación
                                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                CargaGestorTokens(Convert.ToInt32(Session["id_registro"]));
                            }
                            break;
                        }
                }
            }
        }
        #endregion

        #region Eventos de la ventana modal para elejir fecha de vigencia de token
        protected void lkbCerrarVentanaModalFechaVigenciaToken_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            ScriptServer.AlternarVentana(this.Page, "ElegirFechaVigencia", "contenedorFechaVigenciaToken", "ventanaFechaVigenciaToken");
            //Recargando Grid
            Controles.InicializaIndices(gvGestionTokens);
        }
        /// <summary>
        /// Método que llama al método que genera tokens vigencia
        /// </summary>
        protected void btnGeneraTokenVigencia_Click(object sender, EventArgs e)
        {
            generaTokenVigencia();
        }
        /// <summary>
        /// Método que genera Tokens con una fecha de vigencia personalizada
        /// </summary>
        private void generaTokenVigencia()
        {
            RetornoOperacion resultado = new RetornoOperacion();
            int IdContacto = Convert.ToInt32(Session["id_registro"]);
            DateTime FechaInicial = Fecha.ObtieneFechaEstandarMexicoCentro();
            DateTime FechaFinal = Convert.ToDateTime(txtFechaVigenciaToken.Text);
            TimeSpan Dias = FechaFinal - FechaInicial;
            int Vigencia;
            string Token;

            Vigencia = Dias.Days;

            if (Vigencia >= 1)
            {

                using (SAT_CL.Global.Contacto C = new SAT_CL.Global.Contacto(IdContacto))
                {
                    //Validando que exista el contacto
                    if (C.id_contacto > 0)
                    {

                        resultado = SAT_CL.Seguridad.UsuarioToken.GeneraNuevoTokenUUID(C.id_usuario_sistema, C.id_compania_emisor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, Vigencia, out Token);
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion("Generación existosa. Nuevo Token generado: " + Token, true);
                            //Mostrando Mensaje de Operación
                            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            CargaGestorTokens(C.id_contacto);
                        }
                        else
                            //Mostrando Mensaje de Operación
                            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }

                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "ElegirFechaVigencia", "contenedorFechaVigenciaToken", "ventanaFechaVigenciaToken");
                //Recargando Grid
                Controles.InicializaIndices(gvGestionTokens);
            }
            else
            {
                resultado = new RetornoOperacion("La fecha de vigencia del Token debe ser mayor a la fecha de hoy.", false);
                //Mostrando Mensaje de Operación
                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        #endregion
    }
}