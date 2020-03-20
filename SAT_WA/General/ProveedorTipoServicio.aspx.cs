using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.General
{
    /// <summary>
    /// Clase que define el funcionamiento del formulario ProveedorTipoServicio mediante eventos.
    /// </summary>
    public partial class ProveedorTipoServicio : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento que define si la pagina se a realizado por una consulta inicial o si es una respuesta a una solicitud.
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene informacion de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Valida si la página es cargada por primera vez
            if (!Page.IsPostBack)
                //Invoca al método inicializaForma().
                inicializaForma();
        }
        /// <summary>
        /// Evento que permite almacenar los datos del formulario a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método guardarProveedorTipoServicio().
            guardarProveedorTipoServicio();
        }
        /// <summary>
        /// Evento que permite anular las acciones realizadas sobre el formulario (Ingreso de datos, edicion de datos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Acorde al estatus de la página valida:
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable de sessión id_registro el valor de 0;
                        Session["id_registro"] = 0;
                        break;
                    }
                //en caso de que el estado de la página sea de Edición.
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de sessión estatus el valor actual de la página(Lectura).
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
                        txtCompania.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(94, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaProveedorTipoServicio();
                        guardarProveedorTipoServicio();
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
                        txtCompania.Focus();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase ProveedorTipoServicio con el valor de la variable de sessión Id_registro.
                        using (SAT_CL.Global.ProveedorTipoServicio pts = new SAT_CL.Global.ProveedorTipoServicio((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if(pts.id_proveedor_tipo_servicio > 0)
                            //Asigna al objeto retorno los datos del usuario que elimino el registro, invocando al método Deshabilitar de la clase ProveedorTiposervicio
                            retorno = pts.DeshabilitarProveedorTipoServicio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
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
                        inicializaBitacora(Session["id_registro"].ToString(), "94", "Proveedor Tipo Servicio");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de ProveedorTipoServicio
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "94",
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
        /// Método que permite inicializar el aspecto de la página
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método habilitaControles();
            habilitaControles();
            //Invoca al método habilitaMenu();
            habilitaMenu();
            //Invoca al método inicializaValores().
            inicializaValores();
        }

        /// <summary>
        /// Método que permite modificar el estado de disponibilidad de los controles acorde al estdo de la página
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estado de la pagina para habilitar o deshabilitar los controles
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea Nuevo o Edición
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita los controles
                        txtCompania.Enabled =
                        txtDescripcion.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                //En caso decimal que el estado de la página se Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilita los controles
                        txtCompania.Enabled =
                        txtDescripcion.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que habilitara o deshabilitara las opciones del menú principal en base al estado de la página.
        /// </summary>
        private void habilitaMenu()
        {
            //Evalua cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo.
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
        /// Método que establece el valor inicial de los controles acorde al estado de la página
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página y asigna los valores a los controles
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {               
                        //Limpia los controles
                        txtCompania.Text = "";
                        txtDescripcion.Text = "";
                        break;
                    }
                //En caso de que el estado de la página sea Lectura o Edición.
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase ProveedorTipoServicio y asigna como parametro el valor de la variable id_registro
                        using (SAT_CL.Global.ProveedorTipoServicio pts = new SAT_CL.Global.ProveedorTipoServicio((int)Session["id_registro"]))
                        {
                            //Valida que el registro exista en la base de datos
                            if (pts.id_proveedor_tipo_servicio > 0)
                            {
                                //Invoca al constructor de la clase CompañiaEmisorReceptor para obtener el nombre de la compania en base al id_compania_emisor
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(pts.id_compania_emisor))
                                {
                                    //Valida que exista el registro
                                    if(cer.id_compania_emisor_receptor > 0)
                                        //Asigna al control el nombre y el id de la compañia.
                                        txtCompania.Text = string.Format("{0}   ID:{1}", cer.nombre, cer.id_compania_emisor_receptor);
                                }
                                //Asigna al control el dato del registro seleccionado
                                txtDescripcion.Text = pts.descripcion;
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite almacenar los datos ingresados en los controles a la base de datos
        /// </summary>
        private void guardarProveedorTipoServicio()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en Nuevo.
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna al objeto retorno los valores obtenidos de los controles, invocando al método de inserción de la clase ProveedorTipoServicio.
                        retorno = SAT_CL.Global.ProveedorTipoServicio.InsertarProveedorTipoServicio(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text,"ID:",1)), txtDescripcion.Text, 
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                       
                        break;
                    }
                //En caso de que el estado de la página este en Edición.
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al Constructor de la Clase ProveedorTiposervicio con el valor de la variable de sessión id_registro
                        using (SAT_CL.Global.ProveedorTipoServicio pts = new SAT_CL.Global.ProveedorTipoServicio((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (pts.id_proveedor_tipo_servicio > 0)
                            {
                                //Asigna a la variable retorno los valores de los controles, invocando al método de edición de la clase ProveedorTipoServicio.
                                retorno = pts.EditarProveedorTipoServicio(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), txtDescripcion.Text,
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }
            }
            //Valida que la operacion de insercion y edición se reañizaron corectamente.
            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable de session estatus el valor de Lectura.
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Asigna a la variable de session id_registro el valor generado en la base de datos(id)
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma().
                inicializaForma();
            }
            //Muestra un mensaje acorde a la validación de la operación
            lblError.Text = retorno.Mensaje;
        }

        /// <summary>
        ///  Método que permie la consulta del historial de modificicaciones realizadas a un registro 
        /// </summary>
        /// <param name="idRegistro">ID que permite identificar un registro de ProveedorTipoServicio</param>
        /// <param name="idTabla">Identificador de la tabla ProveedorTipoServicio</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  ProveedorTipoServicio.
            string url = Cadena.RutaRelativaAAbsoluta("~/General/ProveedorTipoServicio.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Proveedor Tipo Servicio", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla ProveedorTipoServicio
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla ProveedorTipoServicio registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla ProveedorTipoServicio
            string url = Cadena.RutaRelativaAAbsoluta("~/General/ProveedorTipoServicio.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de ProveedorTipoServicio
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla ProveedorTipoServicio
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Proveedor Tipo Servicio", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla ProveedorTipoServicio</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla ProveedorTipoServicio en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla ProveedorTipoServicio
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/ProveedorTipoServicio.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla ProveedorTipoServicio
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Proveedor Tipo Servicio", 800, 500, false, false, false, true, true, Page);
        }
     
        #endregion
    }
}