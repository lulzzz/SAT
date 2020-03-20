using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.ASP;

namespace SAT.Administrativo
{
    /// <summary>
    /// Clase de la Pagina Web Bancos, que administra el comportamiento de los controles de la pagina en base a eventos.
    public partial class Bancos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento que determina como inicia la pagina web Bancos.
        /// </summary>
        /// <param name="sender">Almacena la referencia de quien inicio un evento</param>
        /// <param name="e">Contiene informacion de la clase EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Valida la carga inicial de la página
            //Valida si la página se esta cargado por primera vez.
            if (!Page.IsPostBack)
            {
                //Invoca al método inicializarForma
                inicializaForma();
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se da clic al botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
          //Invoca al método guardaBancos().
            guardaBanco();
        }

        /// <summary>
        /// Evento que se ejecutara cuando se de clic en el boton cancela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Válida cada estado de la pagina y ejecuta
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la pagina sea nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //A la variable de estatus se le asigna el estado de nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //A la variable id_registro se le asigna el valor de 0
                        Session["id_registro"] = 0;
                        break;
                    }
                //En caso de que el estado de la página sea edición
                case Pagina.Estatus.Edicion:
                    {
                        //A la variable estatus se le asigna el estado de lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }       
            }
            inicializaForma();

        }

        /// <summary>
        /// Evento que ejecuta los estados  del menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Creación del objeto botonMenu que obtiene las opciones de los menú desplegable .
            LinkButton botonMenu = (LinkButton)sender;
            //Permite validar cada una de las opciones del ménu.
            switch (botonMenu.CommandName)
            {
                //en caso de seleccionar la opción de Nuevo en el menú.
                case "Nuevo":
                    {
                        //A la variable estatus de la sesion, se le asigna el valor de pagina nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        // La variable id_registro se le inicializa en cero.
                        Session["id_registro"] = 0;
                        //Inicializa el estado de la forma.
                        inicializaForma();
                        //El label de error se mostrara sin caracteres ya que el estado es nuevo
                        lblError.Text = "";
                        break;
                    }

                case "Abrir":
                    {
                        //Invoca al método inicializaApertura para inicializar la apertura de un registro de Bancos
                        inicializaAperturaRegistro(98, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }

                case "Guardar":
                    {
                        guardaBanco();
                        break;
                    }
                //En caso de seleccionar la opción Editar del menú.
                case "Editar":
                    {
                        //Se asigna a la sesion el estado de edición
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Invoca al metodo inicializaFormana
                        inicializaForma();
                        //El lbel cualdo este en modo de edicion debera limpiarse
                        lblError.Text = "";
                        break;
                    }
                //En caso de seleccionar la opción Eliminar del menú.
                case"Eliminar":
                    {
                        //Invoca al método que deshabilita los registros
                        deshabilitaBanco();
                        break;
                    }
                case"Bitacora":
                    {
                        //Invoca al método inicializaBitacora que muestra las modificaciones hechas sobre un registro de bancos
                        inicicalizaBitacora(Session["id_registro"].ToString(), "98", "Bancos");
                        break;
                    }
                 
                case"Referencias":
                    {
                        //Invoca al método inicializaReferencia que muestra las observaciones hechas a un registro de bancos
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "98", 
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case"Archivos":
                    {
                        break;
                    }
                case"Acerca":
                    {
                        break;
                    }
                case"Ayuda":
                    {
                        break;
                    }
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de inicializar todos los aspectos de la página
        /// </summary>
        private void inicializaForma()
        {
            //Si el estatus de la pagina es nula, se carga la sesion al estado de nuevo, en caso contrario se carga el estado en el que se encuentre.
            if (Session["estatus"] == null)
            {
                //A la variable esatus se le asigna el estado de nuevo
                Session["estatus"] = Pagina.Estatus.Nuevo;
            }
            else
            //A la variable Estatus se asigna un estatus.
            Session["estatus"] = Session["estatus"];
            //Invoca al método habilitaControles
            habilitaControles();
            //Invoca al método habilitaMenú
            habilitaMenu();
            //Invoca al método inicializaValores
            inicializaValores();
        }

        /// <summary>
        /// Método encargado de habilitar o deshabilitar los controles de la pagina de acuerdo a su estatus.
        /// </summary>
        private void habilitaControles()
        {
            //Válida cada estado en la que puede estar una página.
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que el estado de la página este en estatus nuevo, se habilitará los controles para su uso.
                case Pagina.Estatus.Nuevo:
                    {
                        txtClave.Enabled = true;
                        txtNombre.Enabled = true;
                        txtRazonSocial.Enabled = true;
                        chkNacional.Enabled = true;
                        lblError.Enabled = true;
                        btnGuardar.Enabled = true;
                        btnCancelar.Enabled = true;
                        break;
                    }
                //En caso de que el estado de la página sea de lectura, los controles estarán deshabilitados.
                case Pagina.Estatus.Lectura:
                    {
                        txtClave.Enabled = false;
                        txtNombre.Enabled = false;
                        txtRazonSocial.Enabled = false;
                        chkNacional.Enabled = false;
                        lblError.Enabled = false;
                        btnGuardar.Enabled = false;
                        btnCancelar.Enabled = false;
                        break;
                    }
                //En el caso de que el estado de la pagina sea de edición,  los controles se habilitará para su uso.
                case Pagina.Estatus.Edicion:
                    {
                        txtClave.Enabled = true;
                        txtNombre.Enabled = true;
                        txtRazonSocial.Enabled = true;
                        chkNacional.Enabled = true;
                        lblError.Enabled = true;
                        btnGuardar.Enabled = true;
                        btnCancelar.Enabled = true;
                        break;
                    } 
            }
                
        }

        /// <summary>
        /// Método encargado de habilitar o deshabilitar las opciones de los menú en base al estado de la página
        /// </summary>
        private void habilitaMenu()
        {
            //Permite validar cada uno de estatus de una sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En el caso de que el estatus de la página sea nuevo, habilita las opciones del menú(abrir,guardar,acercade, ayuda).
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled = true;
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = true;  
                        break;
                    }
                //En el caso de que el estatus de la pagina sea lectura, habilita las opciones del ménu(nuevo,abrir,eliminar,editar,bitacora,referencia,Archivo,acerdade, ayuda).
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = true;
                        lkbAcercaDe.Enabled = true;
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = false;

                        break;
                    }
                //en el caso de que el estatus de la pagina sea de edicios, habilita las opciones del menú (nuevo,abrir,guardar,eliminar,bitacora,referencia,acercade,ayuda)
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcercaDe.Enabled = true;
                        lkbAyuda.Enabled = true;
                        btnGuardar.Enabled = true;
                        break;
                    }
            }
        }


        /// <summary>
        /// Método que inicializa los valores de los controles de acuerdo al estado de la página.
        /// </summary>
        private void inicializaValores()
        {
            //Válida cada uno de los estados que puede tener una pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En el caso de que el estado sea nuevo, los controles deberán de estar limpios.
                case Pagina.Estatus.Nuevo:
                    {
                        txtClave.Text = "";
                        txtNombre.Text = "";
                        txtRazonSocial.Text = "";
                        chkNacional.Checked = true;
                        lblError.Text = "";
                        break;
                    }
                //En caso de que el estado sea de Lectura o Edición se cargaran los controles de la pagina con los datos registrados en la base de datos.
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Creación del objeto que invoca a la clase banco para obtener el registro insertado en la base de datos
                        using (SAT_CL.Bancos.Banco ban = new SAT_CL.Bancos.Banco((int)Session["id_registro"]))
                        {
                            //Asigna los valores obtenidos de la base de datos a los controles.
                            txtClave.Text = ban.clave;
                            txtNombre.Text = ban.nombre_corto;
                            txtRazonSocial.Text = ban.razon_social;
                            chkNacional.Checked = ban.bit_nacional;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que permite almacenar los datos obtenidos de la pagina en la base de datos
        /// </summary>
        private void guardaBanco()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Valida cada caso de estatus de la pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //En caso de que la página este en estado de nuevo, se invocara al método guardaBanco
                case Pagina.Estatus.Nuevo:
                    { 
                        //Asignación de valores al objeto retorno, obtenidos de los controles de la pagina
                        retorno = SAT_CL.Bancos.Banco.InsertarBanco(txtClave.Text, txtNombre.Text.ToUpper(), txtRazonSocial.Text.ToUpper(), 
                                                       txtRFC.Text.ToUpper(), chkNacional.Checked,
                                                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                //En caso de que la pagina este en estado de edición, se invocara al método editaBanco
                case Pagina.Estatus.Edicion:
                    {
                        //Creación del objeto que invoca a la clase banco para obtener el registro insertado y poder actualizarlo.
                        using (SAT_CL.Bancos.Banco ban = new SAT_CL.Bancos.Banco((int)Session["id_registro"]))
                        {
                            //Asignación de valores al objeto retorno, obtenidos de los controles de la página tras la actualización de datos
                            retorno = ban.EditarBanco(txtClave.Text, txtNombre.Text.ToUpper(), txtRazonSocial.Text.ToUpper(),
                                                     txtRFC.Text.ToUpper(), chkNacional.Checked,
                                                     ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        break;
                    }
            }            
            //Válida si la inserción se ha realizado correctamente en la base de datos
            if (retorno.OperacionExitosa)
            {
                //Asigna a la variable estatus el estado de lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //La variable id_registro se le asigna el id generado en la base de datos
                Session["id_registro"] = retorno.IdRegistro;
                //Invoca al método inicializaForma
                inicializaForma();
            }
            //Mensaje de inserción correcta o erronea
            lblError.Text = retorno.Mensaje;
        }

        /// <summary>
        /// Método que permite el cambio de estado habilitado a deshabilitado de un registro pertenecientes a bancos
        /// </summary>
        private void deshabilitaBanco()
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del objeto ban que invoca a la clase banco para obtener el registro insertado en la base de datos.
            using (SAT_CL.Bancos.Banco ban = new SAT_CL.Bancos.Banco((int)Session["id_registro"]))
            {
                //Asignación de valores referentes al usuario que deshabilito el registro de bancos al objeto retorno.
                retorno = ban.DeshabilitarBanco(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Válida que la acción de deshabilitar un registro de realizó correctamente
                if(retorno.OperacionExitosa)
                {
                    //Asigna a la variable estatus el estado de nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    //La variable id_registro se le asigna el valor de 0
                    Session["id_registro"] = 0;
                    //Invoca al método de la forma
                    inicializaForma();
                }
                //Mensaje de que se deshabilito el registro dependiendo de la validación de la operación.
                lblError.Text = retorno.Mensaje;
            }
        }

        /// <summary>
        /// Método que muestra las modificaciones realizadas a un registro.
        /// </summary>
        /// <param name="idRegistro">Id que identifica un registro de la tabla banco</param>
        /// <param name="idTabla">Id que identifica a la tabla Bancos en la base de datos</param>
        /// <param name="Titulo">Nombre que se le asignara a la ventana de Bitacora</param>
        private void inicicalizaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Crea la variable url que almacena una ruta de ubicación de la ventana, que contendra la bitacora de un registros de  Bancos.
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/Bancos.aspx",
                                                                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Variable que almacena las medidas de la ventana que contendra los datos de Bitacora de  Bancos.
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimenciones.
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora Bancos", configuracion, Page);
        }

        /// <summary>
        /// Método que permite la busqueda de registros en la base de datos pertenecientes a la tabla bancos 
        /// </summary>
        /// <param name="idTabla">Id que identifica a la tabla bancos registrada en la base de datos</param>
        /// <param name="idCompania"> Id que identifica a una compañia</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {      
            //Asigna a la variable de session id_tabla el valor del parametro idTabla
            Session["id_tabla"] = idTabla;
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla bancos
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/Bancos.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Define las dimensiones de la ventana Abrir registros de banco
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Bancos
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir Registro Banco", configuracion, Page);
        }

        /// <summary>
        /// Método encargado de inicializr la referencias a un registro
        /// </summary>
        /// <param name="id_registro">Id que permite identificar un registro de la tabla bancos</param>
        /// <param name="id_tabla">Id que permite identificar a la tabla bancos en la base de datos</param>
        /// <param name="id_compania">Id que permite identificar a que compania pertenece </param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla bancos
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/Bancos.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla bancos
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Banco", 800, 500, false, false, false, true, true, Page);
        }

        #endregion





    }
}