using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.General
{
    public partial class ClasificacionTipo : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento que define el estado inicial de una página. si es una solicitud inicial o es una respuesta.
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio el evento</param>
        /// <param name="e">Contene información de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página es cargada por primera vez
            if (!Page.IsPostBack)
                //Invoca al método inicializaForma();
                inicializaForma();
        }
        /// <summary>
        /// Evento que realiza el alamcenamiento de datos a la tabla ClasificaciónTipo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método de Guardar
            guardarClasificacionTipo();
        }
        /// <summary>
        /// Evento que anula las aciones realizadas sobre el formulario(Ingreso de datos a los conttroles, edicion de datos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Valda cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna el valor de 0 a la variable de session id_registro
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea edición.
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable session estatus el estado Lectura.
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Invoca al método inicializaForma().
            inicializaForma();
        }
        /// <summary>
        /// Evento que permite seleccionar y ejecutar acciones del menú.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite ejecutar acciones determinadas por cada opción del menú
            switch (botonMenu.CommandName)
            {
                //Si la elección del menú es la opción Nuevo
                case "Nuevo":
                    {
                        //Asigna a la variable de session estatus el estado del formulario en nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma
                        inicializaForma();
                        //Limpia los mensajes de error del lblError
                        lblError.Text = "";
                        //Se realiza un enfoque al primer control 
                        ddlClasificacion.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(4, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaClasificacionTipo();
                        guardarClasificacionTipo();
                        break;
                    }
                //Si la elección del menú es la opción Editar
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma();
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        //Se realiza un enfoque al primer control 
                        ddlClasificacion.Focus();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase ClasificacionTipo con el valor de la variable de sessión Id_registro.
                        using (SAT_CL.Global.ClasificacionTipo ct = new SAT_CL.Global.ClasificacionTipo((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (ct.id_clasificacion > 0)
                                //Asigna al objeto retorno los datos del usuario que elimino el registro, invocando al método Deshabilitar de la clase Clasificaciontipo
                                retorno = ct.DeshabilitaClasificacionTipo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        //Valida so i la operación de deshabilitar registro se realizo correctamente
                        if (retorno.OperacionExitosa)
                        {
                            //Asigna a la variable de sessión estatus el estado de la página Nuevo.
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Asigna a la variable de session id_registro el valor de 0.
                            Session["id_registro"] = 0;
                            //Invoca al método inicializaForma().
                            inicializaForma();
                        }
                        //Mensaje de que la operación de deshabilitar registros se realizo correctamente.
                        lblError.Text = retorno.Mensaje;
                        break;
                    }
                //Si la elección del menú es la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "4", "Clasificación Tipo");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de ClasificaciónTipo
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "4",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú es la opcion Archivo
                case "Archivo":
                    {
                        break;
                    }
                //Si la elección del menú es la opcion Acerca
                case "Acerca":
                    {
                        break;
                    }
                //Si la elección del menú es la opcion Ayuda
                case "Ayuda":
                    {
                        break;
                    }
            }
        }
        #endregion
        #region Métodos
        /// <summary>
        /// Método que establece los valores del formulario
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método habilitaControles().
            habilitaControles();
            //Invoca al método habilitaMenu().
            habilitaMenu();
            //Carga el catalogo Clasificación para el control ddlClasificacion
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlClasificacion, "", 1116);
            //Invoca al método inicializaValores().
            inicializaValores();
        }
        /// <summary>
        /// Método que permite habilitar o deshabilitar los controles del formulario en base al estado de la página.
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estatus de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo 
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitara los controles para su insercion.
                        txtCompania.Enabled = false;
                        ddlClasificacion.Enabled =
                        txtCodigo.Enabled =
                        txtDescripcion.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitara los controles
                        txtCompania.Enabled =
                        ddlClasificacion.Enabled = false;
                        txtCodigo.Enabled =
                        txtDescripcion.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página sea Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilitara los controles
                        txtCompania.Enabled =
                        ddlClasificacion.Enabled =
                        txtCodigo.Enabled =
                        txtDescripcion.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite habilitar o deshabilitar las opciones del menú acorde a cada estatus de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        // Habilita y deshabilita las opcionés del menú.
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }
                //En caso de que el estado sea Lectura.
                case Pagina.Estatus.Lectura:
                    {
                        // Habilita y deshabilita las opcionés del menú.
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled =
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;
                        break;
                    }

                //En caso de que el estado de la página Edición.
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita y deshabilita las opcionés del menú.
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled =
                        lkbAyuda.Enabled =
                        btnGuardar.Enabled = true;
                        break;
                    }

            }
        }
        /// <summary>
        /// M´étodo que acorde a cada estatus de la página asigna valores a los controles del formulario ClasificaciónTipo
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Invoca al constructor de la clase CompaniaEmisor y receptor para obtener el nombre de la compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                        }
                        //Limpia los controles del formulario
                        txtCodigo.Text = "";
                        txtDescripcion.Text = "";
                        break;
                    }
                //En caso de que el estdo de la pagina sea Edicion o Lectura
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Invoca al constructor de la clase y asigna como parametro el valor de la variable de session id_regsitro
                        using (SAT_CL.Global.ClasificacionTipo ct = new SAT_CL.Global.ClasificacionTipo((int)Session["id_registro"]))
                        {
                            //Valida que el registro exista
                            if (ct.id_clasificacion > 0)
                            {
                                //Invoca al Constructor de la clase CompañiaEmisor para obtener el nombre de la compañia en base al id_compania_emisor
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(ct.id_compania_emisor))
                                {
                                    //Valida que exista el registro de compania emisor receptor
                                    if (cer.id_compania_emisor_receptor > 0)
                                        //Asigna al control el nombre y id de la compañía
                                        txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString(); ;
                                }
                                //Asigna a los controles los datos del registro seleccionado.
                                ddlClasificacion.SelectedValue = ct.id_campo_clasificacion.ToString();
                                txtCodigo.Text= ct.codigo_clasificacion;
                                txtDescripcion.Text = ct.descripcion_clasificacion;
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite el almacenamiento de los datos en la labla ClasificaciónTipo
        /// </summary>
        private void guardarClasificacionTipo()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada estado de la clase
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estadod de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna al objeto retorno los valores de los controles invocando al método de inserción de la clase ClasificaciónTipo
                        retorno = SAT_CL.Global.ClasificacionTipo.InsertaClasificacionTipo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                             Convert.ToInt32(ddlClasificacion.SelectedValue), txtCodigo.Text.ToUpper(), txtDescripcion.Text, 
                                                                                             ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //En caso de que el estado de la página sea edición
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase y asigna como parametro el valor de la variable de sessión id_registro
                        using (SAT_CL.Global.ClasificacionTipo ct = new SAT_CL.Global.ClasificacionTipo((int)Session["id_registro"]))
                        {
                            //Valida que exista la clase
                            if (ct.id_clasificacion > 0)
                            {
                                //Asigna al objeto retorno el valor de los controles invocando al método de edición de la clase ClasificaciónTipo
                                retorno = ct.EditaClasificacionTipo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                      Convert.ToInt32(ddlClasificacion.SelectedValue), txtCodigo.Text.ToUpper(), txtDescripcion.Text,
                                                                      ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }                        
                        break;
                    }
            }
            //Valida que la operacion de inserción se realizo correctamente
            if (retorno.OperacionExitosa)
            {
                //Asigna el valor de estatus session en modo lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado en la base de datos(id).
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma.
                inicializaForma();
            }
            //Muestra un mensaje acorde a la validación de la operación.
            lblError.Text = retorno.Mensaje;  

        }

        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de ClasificacionTipo</param>
        /// <param name="idTabla">Identificador de la tabla ClasificacionTipo</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  ClasificacionTipo.
            string url = Cadena.RutaRelativaAAbsoluta("~/General/ClasificacionTipo.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Clasificacion Tipo", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla ClasificacionTipo
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla ClasificacionTipo registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla ClasificacionTipo
            string url = Cadena.RutaRelativaAAbsoluta("~/General/ClasificacionTipo.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de ClasificacionTipo
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla ClasificacionTipo
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Clasificacion Tipo", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla ClasificacionTipo</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla ClasificacionTipo en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla ClasificacionTipo
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/ClasificacionTipo.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla ClasificacionTipo
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Clasificacion Tipo", 800, 500, false, false, false, true, true, Page);
        }

        #endregion        
    }
}