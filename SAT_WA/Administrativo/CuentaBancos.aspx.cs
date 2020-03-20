using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.Administrativo
{
    /// <summary>
    /// Clase del fromulario CuentaBancos que permite manipular el comportamiento en base a eventos.
    /// </summary>
    public partial class CuentaBancos : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Método que define la carga de la pagina, si es por primera vez o es en respuesta a una solicitud
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida la carga inicial de la página
            if (!Page.IsPostBack)
                //Invoca al método inicilizaForma
                inicializaForma();

        }
        /// <summary>
        /// Evento que controla el almacenamiento del registro a la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invoca al método de Guardar
            guardarCuentaBancos();
        }
        /// <summary>
        /// Evento que anula acciones realizadas sobre el formulario (Inserción y edición de datos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Acorde al estatus de la página valida:
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea Nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna a la variable de sessión id_registro el valor de 0.
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea de edición
                case Pagina.Estatus.Edicion:
                    {
                        //Asigna a la variable de session estatus el valor de Lectura
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
                        ddlBancos.Focus();
                        break;
                    }
                //Si la elección del menú es la opcion Abrir
                case "Abrir":
                    {
                        //Invoca al método inicializaAperturaRegistro();
                        inicializaAperturaRegistro(99, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }

                //Si la elección del menú es la opción Guardar
                case "Guardar":
                    {
                        //Invoca al método guardaCuentaBancos();
                        guardarCuentaBancos();
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
                        ddlBancos.Focus();
                        break;
                    }
                //Si la elección del menú es la opción Eliminar
                case "Eliminar":
                    {
                        //Creación del objeto retorno
                        RetornoOperacion retorno = new RetornoOperacion();
                        //Invoca al constructor de la clase CuentaBancos con el valor de la variable de sessión Id_registro.
                        using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro
                            if (cb.id_cuenta_bancos > 0)
                                //Asigna al objeto retorno los datos del usuario que elimino el registro, invocando al método Deshabilitar de la clase CuentaBancos
                                retorno = cb.DeshabilitarCuentaBancos(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
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
                        inicializaBitacora(Session["id_registro"].ToString(), "99", "Proveedor Tipo Servicio");
                        break;
                    }
                //Si la elección del menú es la opcion Referencia
                case "Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de CuentaBancos
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "99",
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
        /// Método encargado de inicializar el aspecto del formulario
        /// </summary>
        private void inicializaForma()
        {
            //Invoca al método carga catalogo
            cargaCatalogo();
            //Invoca al método habilitaControles().
            habilitaControles();
            //Invoca al método habilitaMenu().
            habilitaMenu();
            //Invoca al método inicializaValores().
            inicializaValores();

        }
        /// <summary>
        /// Método que iniciliza los valores del dropdownlist del formulario
        /// </summary>
        private void cargaCatalogo()
        {
            //Carga el catalogo Banco al dropdownlist Bancos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBancos, 22,"", 0, "", 0, "");
            //Carga el catalogo TipoCuenta al dropdownlist tipocuenta
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCuenta, "", 77);
            //Carga el catalogo de compania  al dropdownlist Tabla
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTabla, 53,"", 0, "", 0, "");
        }
        /// <summary>
        /// Método que permite habilitar o deshabilitar los controles en base al estado de la página
        /// </summary>
        private void habilitaControles()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea Nuevo o edición
                case Pagina.Estatus.Nuevo:
                    {
                        ddlBancos.Enabled =
                        txtNumCuenta.Enabled =
                        ddlTipoCuenta.Enabled =
                        ddlTabla.Enabled =
                        txtRegistro.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilita los controles del formularios
                        ddlBancos.Enabled = true;
                        txtNumCuenta.Enabled = false;
                        ddlTipoCuenta.Enabled =
                        ddlTabla.Enabled =
                        txtRegistro.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página sea lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilita los controles del formulario
                        ddlBancos.Enabled =
                        txtNumCuenta.Enabled =
                        ddlTipoCuenta.Enabled =
                        ddlTabla.Enabled =
                        txtRegistro.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que habilita las opciones del menú principal en base al estado de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Evalua cada estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en modo Nuevo
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
                //En caso de que el estado de la página este en modo de lectura
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
                //En caso de que el estado de la página este en modo edición
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
        /// Método que asigna valores a los controles en base al estado de la página 
        /// </summary>
        private void inicializaValores()
        {
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpia el contenido de los controles
                        txtNumCuenta.Text = "";
                        txtRegistro.Text = "";
                        break;
                    }
                //En caso de que el estado de la pagina sea edición o lectura
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase CuentaBancos y asigna como paramétro el valor de la variable de session id_registro
                        using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos((int)Session["id_registro"]))
                        {
                            //Valida que exista el registro de cuenta bancos
                            if (cb.id_cuenta_bancos > 0)
                            {
                                ddlBancos.SelectedValue = cb.id_banco.ToString();
                                txtNumCuenta.Text = cb.num_cuenta.ToString();
                                ddlTipoCuenta.SelectedValue = cb.id_tipo_cuenta.ToString();
                                ddlTabla.SelectedValue = cb.id_tabla.ToString();
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTabla, 53, "", 0, "", 0, "");
                                if (cb.id_tabla == 25)
                                {
                                    using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(cb.id_registro))
                                    {
                                        if(cer.id_compania_emisor_receptor > 0)
                                            txtRegistro.Text = string.Format("{0}   ID:{1}", cer.nombre, cer.id_compania_emisor_receptor);
                                    }
                                }
                                else
                                {
                                    using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(cb.id_registro))
                                    {
                                        if(o.id_operador > 0)
                                            txtRegistro.Text = string.Format("{0}   ID:{1}", o.nombre, o.id_operador);  
                                    }
                                }
                            }
                        }
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que almacena los datos de los controles validando si es un nuevo registro o una edición
        /// </summary>
        private void guardarCuentaBancos()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Asigna al objeto retorno los valores de los controles invocando al método de inserción de la clase CuentaBancos
                        retorno = SAT_CL.Bancos.CuentaBancos.InsertarCuentaBancos(Convert.ToInt32(ddlBancos.SelectedValue), Convert.ToInt32(ddlTabla.SelectedValue),
                                                                                 Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtRegistro.Text, "ID:", 1)), txtNumCuenta.Text,
                                                                                 (SAT_CL.Bancos.CuentaBancos.TipoCuenta)Convert.ToInt32(ddlTipoCuenta.SelectedValue),
                                                                                 ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Invoca al constructor de la clase cuentaBancos y asigna como parámetro el valor de la variable session id_registro
                        using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos((int)Session["id_registro"]))
                        {
                            //Valida si existe el registro cuenta banco
                            if (cb.id_cuenta_bancos > 0)
                            {
                                //Asigna al objeto retorno el valor de los controles invocando al método de edición de la clase CuentaBancos
                                retorno = cb.EditarCuentaBancos(Convert.ToInt32(ddlBancos.SelectedValue), Convert.ToInt32(ddlTabla.SelectedValue),
                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtRegistro.Text, "ID:", 1)), txtNumCuenta.Text,
                                                                (SAT_CL.Bancos.CuentaBancos.TipoCuenta)Convert.ToInt32(ddlTipoCuenta.SelectedValue),
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
        /// <param name="idRegistro">ID que permite identificar un registro de CuentaBancos</param>
        /// <param name="idTabla">Identificador de la tabla CuentaBancos</param>
        /// <param name="Titulo">Encabezado de la ventana Bitacora</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Creación de la variable url, que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  CuentaBancos.
            string url = Cadena.RutaRelativaAAbsoluta("~/Administrativo/CuentaBancos.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena la resolucion de la ventana bitacora
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
            ScriptServer.AbreNuevaVentana(url, "Bitacora Proveedor Tipo Servicio", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla CuentaBancos
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla CuentaBancos registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla CuentaBancos
            string url = Cadena.RutaRelativaAAbsoluta("~/Administrativo/CuentaBancos.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de CuentaBancos
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla CuentaBancos
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Proveedor Tipo Servicio", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla CuentaBancos</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla CuentaBancos en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla CuentaBancos
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/Administrativo/CuentaBancos.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla CuentaBancos
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Proveedor Tipo Servicio", 800, 500, false, false, false, true, true, Page);
        }


        #endregion
    }
}