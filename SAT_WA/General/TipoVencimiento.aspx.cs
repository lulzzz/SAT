using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.General
{
    /// <summary>
    /// Clase del formulario TipoVencimiento que permite manipular el comportamiento del formulario en base a eventos.
    /// </summary>
    public partial class TipoVencimiento : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento que Permite definir el inicio del Formulario.
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene informacion de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página se esta cargando por primera vez.
            if (!Page.IsPostBack)
                //Invoca al método inicializaForma
                inicializaForma();
        }

        /// <summary>
        ///  Evento que permite almacenar los datos obtenidos de los controles del formulario a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guargaTipoVencimiento().
            guardaTipoVencimiento();
        }

        /// <summary>
        /// Evento que me anula acciones realizadas sobre el formulario (Ingreso, edicion de datos, etc.)
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
                        //Asigna a la variable session estatus el estado de nuevo.
                        Session["estatus"] = Pagina.Estatus.Nuevo;
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
                        //Asigna a la variable de session estatus el estado del formulario en nuevo.
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Asigna a la variable de session id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        //Invoca al método inicializaForma.
                        inicializaForma();
                        //Limpia los mensajes de error del lblError.
                        lblError.Text = "";
                        //hace un enfoque al primer control del formulario.
                        ddlTipoAplicacion.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(128, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                //Si la elección del menú es la opción Guardar.
                case "Guardar":
                    {
                        //Invoca al método guardaTipoVencimiento();
                        guardaTipoVencimiento();
                        break;
                    }
                //Si la elección del menú es la opción Editar.
                case "Editar":
                    {
                        //Asigna a la variable session estaus el estado de la pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca el método inicializaForma();
                        inicializaForma();
                        //Limpia los mensajes del lblError
                        lblError.Text = "";
                        //hace un Enfoque al primer control del formulario.
                        ddlTipoAplicacion.Focus();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar.
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase TipoVencimiento con el valor de la variable de session id_registro.
                        using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento((int)Session["id_registro"]))
                        {
                            //Valida que el id_tipo_vencimiento no sea igual a cero.
                            if (tv.id_tipo_vencimiento > 0)
                            {
                                //Asigna al objeto retorno los datos del usuario que realizo el cambio de estado del registro(Deshabilito)                            
                                retorno = tv.DeshabilitarTipoVencimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }                           
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
                        //Muestra un mensaje acorde a la validación de la operación
                        lblError.Text = retorno.Mensaje;
                        break;
                    }
                //Si la elección del menú en la opción Bitacora
                case "Bitacora":
                    {
                        //Invoca al método bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "128", "Tipo Vencimiento");
                        break;
                    }
                //Si la elección del menú en la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de TipoVencimiento
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "128",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección del menú en la opcion Archivo
                case "Archivo":
                    {
                        break;
                    }
                //Si la elección del menú en la opcion Acerca
                case "Acerca":
                    {
                        break;
                    }
                //Si la elección del menú en la opcion Ayuda
                case "Ayuda":
                    {
                        break;
                    }
            }
        }
 
        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de inicializar los aspectos de la página
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método cargaCatalogo();
            cargaCatalogo();
            //Invoca al método habilitaControles
            habilitaControles();
            //Invoca al método habilitaMenu();
            habilitaMenu();
            //invoca al método inicializaValores();
            inicializaValores();
        }

        /// <summary>
        /// Método que carga los valores del dropdownlist. 
        /// </summary>
        private void cargaCatalogo()
        {
            //Invoca al método cargaCatalogo e iniciliza los dropdownList TipoAplicación y Prioridad.
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAplicacion, "", 1102);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPrioridad, "", 1103);
        }

        /// <summary>
        /// Método que acorde al estado de la pagina habilita o deshabilita los controles del formulario.
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado sea Nuevo o edicion habilitaro los controles
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        ddlTipoAplicacion.Enabled = 
                        ddlPrioridad.Enabled = 
                        txtDescripcion.Enabled = 
                        btnGuardar.Enabled = 
                        btnCancelar.Enabled = true;
                        break;
                    }
                //En caso de que el estado sea Lectura deshabilita los controles
                case Pagina.Estatus.Lectura:
                    {
                        ddlTipoAplicacion.Enabled = 
                        ddlPrioridad.Enabled = 
                        txtDescripcion.Enabled = 
                        btnGuardar.Enabled = 
                        btnCancelar.Enabled = false;
                        break;
                    }

            }

        }

        /// <summary>
        /// Método que habilitara o deshabilitara las opciones del menú principal, en base al estado de la pagina
        /// </summary>
        private void habilitaMenu()
        {
            //Evalua cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo, habilita y deshabilita las opcionés del menú.
                case Pagina.Estatus.Nuevo:
                    {
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
                //En caso de que el estado sea Lectura, habilita y deshabilita las opcionés del menú.
                case Pagina.Estatus.Lectura:
                    {
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

                //En caso de que el estado de la página Edición, habilita y deshabilita las opcionés del menú.
                case Pagina.Estatus.Edicion:
                    {
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
        /// Método que inicializa los valores de los controles de la página acorde a su estatus.
        /// </summary>
        private void inicializaValores()
        {
            //Valida los estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia los controles
                        txtDescripcion.Text = "";
                        lblError.Text = "";
                        break;
                    }
                //En caso de que el estado de la página sea edicion y Lectura
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase TipoVencimiento y asigna como parametro el valor de la variable id_registro
                        using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento((int)Session["id_registro"]))
                        {
                            //Valida si el id_tipo_vencimiento no sea 0.
                            if (tv.id_tipo_vencimiento > 0) 
                            {
                                //Asigna a los controles los datos del registro seleccionado.
                                ddlPrioridad.SelectedValue = tv.id_prioridad.ToString();
                                ddlTipoAplicacion.SelectedValue = tv.id_tipo_aplicacion.ToString();
                                txtDescripcion.Text = tv.descripcion;
                            }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que almacena los datos extraidos de los controles para su almacenamiento en la base de datos.
        /// </summary>
        private void guardaTipoVencimiento()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Evalua cada estatus de la página.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la página este en estado de Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignación de valores obtenidos de los controles de la pagina al objeto retorno, para su insercion en la base de datos.
                        retorno = SAT_CL.Global.TipoVencimiento.InsertarTipoVencimiento(Convert.ToByte(ddlTipoAplicacion.SelectedValue), txtDescripcion.Text.ToUpper(),
                                                                               Convert.ToByte(ddlPrioridad.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //En caso de que la pagina este en estado de Edicion
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase TipoVencimiento con el valor de la variable de session id_registro.
                        using (SAT_CL.Global.TipoVencimiento tv = new SAT_CL.Global.TipoVencimiento((int)Session["id_registro"]))
                        {
                            //Valida que id_tipo_vencimiento no sea igual a 0.
                            if (tv.id_tipo_vencimiento > 0)
                            {
                                //Asigna a la variable retorno los valores obtenidos de los controles del formulario para actualizar el registro.
                                retorno = tv.EditarTipoVencimiento(Convert.ToByte(ddlTipoAplicacion.SelectedValue), txtDescripcion.Text.ToUpper(),
                                                                   Convert.ToByte(ddlPrioridad.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
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
        /// <param name="idRegistro">ID que permite identificar un registro de TipoVencimiento</param>
        /// <param name="idTabla">Identificador de la tabla TipoVencimiento</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  TipoVencimiento.
            string url = Cadena.RutaRelativaAAbsoluta("~/General/TipoVencimiento.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Tipo Vencimiento", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla TipoVencimiento
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla TipoVencimiento registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla TipoVencimiento
            string url = Cadena.RutaRelativaAAbsoluta("~/General/TipoVencimiento.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de TipoVencimiento
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla TipoVencimiento
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Tipo Vencimiento", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializar la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla TipoVencimiento</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla TipoVencimiento en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla TipoVencimiento
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/TipoVencimiento.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla TipoVencimiento
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Tipo Vencimiento", 800, 500, false, false, false, true, true, Page);
        }
        
        #endregion
    }
}