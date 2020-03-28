using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

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
        }

        private void cargaCatalogos()
        {
            //Cargando catálogo de perfiles de reportes
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPerfil, 199, "-- Seleccione un Perfil de la Lista", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
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
                                            txtNombre.Text.ToUpper(),
                                            txtTelefono.Text,
                                            txtEmail.Text,
                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
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
                                        txtNombre.Text.ToUpper(),
                                        txtTelefono.Text,
                                        txtEmail.Text,
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                    );

                                    if(resultado.OperacionExitosa)
                                    {
                                        using(SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(c.id_usuario_sistema))
                                        {
                                            if(u.id_usuario > 0)
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
    }
}