using SAT_CL;
using SAT_CL.FacturacionElectronica33;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.FacturacionElectronica33
{
    public partial class SerieFolioCFDI : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al presionar el botón "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si es estatus actual es "Edicion"
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    //Pasar Session a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
            }
            //Inicializando pagina
            inicializarForma();
        }

        /// <summary>
        /// Evento producido al presionar el botón "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Invocando el método guardar
            guardaSerieFolioCFDI();
        }        
                
        /// <summary>
        /// Evento producido al pulsar algún elemento del manú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkElementoMenu_Click(object sender, EventArgs e)
        {   
            using (LinkButton boton = (LinkButton)sender)//"boton" hace referencia al objeto que activa el evento
            {
                switch (boton.CommandName)
                {                    
                    case "Nuevo":
                        //Asignar el estatus de la página a "Nuevo"
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializar Id de Registro activo
                        Session["id_registro"] = 0;                        
                        //Inicializar Forma/Página
                        inicializarForma();
                        break;
                    case "Abrir":
                        //Inicializando Ventana de Registros
                        inicializaAperturaRegistro(219, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    case "Guardar":
                        //Invocar método de guardado
                        guardaSerieFolioCFDI();
                        break;
                    case "Editar":
                        using (SAT_CL.FacturacionElectronica33.SerieFolioCFDI oc = new SAT_CL.FacturacionElectronica33.SerieFolioCFDI ((int)Session["id_registro"]))
                        {
                            //Asignar estatus edicion
                            Session["estatus"] = Pagina.Estatus.Edicion;
                            //Limpiar contenido de la forma
                            inicializarForma();                            
                        }
                        break;
                    case "Bitacora":
                        //Inicializando bitacora
                        inicializarBitacora(Session["id_registro"].ToString(), "219", "Serie Folio CFDI");
                        break;
                    case "Eliminar":
                        deshabilitaSerieFolioCFDI();
                        break;
                    case "Referencias":                        
                        //Invocar método de incializacion de referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "219",
                                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;                    
                }
            }            
        }        
                
        /// <summary>
        /// Evento generado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validar que no se produzca un PostBack
            if (!Page.IsPostBack)
                //Inicializando estatus general de la forma
                inicializarForma();
        }
        
        #endregion
        
        #region Métodos
        /// <summary>
        /// Método que se encarga de llenar el (los) dropdownlist con su contenido desde un catálogo.
        /// </summary>
        private void cargaCatálogo()
        {
            //Carga los valores para el DropDownList TipoSerieFolio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoFolioSerie, "", 3199);
        }
        /// <summary>
        /// Método encargado de deshabilitar un registro
        /// </summary>
        private void deshabilitaSerieFolioCFDI()
        {
            //Crear objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Invocar contructor de la clase para instanciar metodos
            using (SAT_CL.FacturacionElectronica33.SerieFolioCFDI objSerie = new SAT_CL.FacturacionElectronica33.SerieFolioCFDI(Convert.ToInt32(Session["id_registro"])))
            {
                //Valida que exista un registro
                if (objSerie.id_folio_serie > 0)
                    resultado = objSerie.DeshabilitaSerieFolioCFDI(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                
            }
            //Validar si se realizó con éxito
            if (resultado.OperacionExitosa)
            {
                //Establecer el id de registro
                Session["id_registro"] = 0;
                //Establecer el estatus de la forma
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Inicializar forma
                inicializarForma();
            }
            //Mensaje: Se deshabilitó con éxito.
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de guardar un nuevo registro
        /// </summary>
        private void guardaSerieFolioCFDI()
        {
            //Declarando objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            resultado = null;
            //De acuerdo al estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Insertar Registro
                case Pagina.Estatus.Nuevo:
                    {
                        if (Convert.ToInt32(txtFolioInicial.Text) < Convert.ToInt32(txtFolioFinal.Text))
                        {
                            resultado = SAT_CL.FacturacionElectronica33.SerieFolioCFDI.InsertaSerieFolioCFDI(
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                            txtVersion.Text,
                            txtSerie.Text,
                            Convert.ToBoolean(chkActiva.Checked),
                            Convert.ToInt32(txtFolioInicial.Text),
                            Convert.ToInt32(txtFolioFinal.Text),
                            Convert.ToByte(ddlTipoFolioSerie.Text),
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                        }
                        else
                            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptar, "El folio incial no debe ser menor al folio final.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Error, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);              
                        break;
                    }
                case Pagina.Estatus .Edicion:
                    {
                        if (Convert.ToInt32(txtFolioInicial.Text) < Convert.ToInt32(txtFolioFinal.Text))
                        {
                            //Invocaal constructor de la clase SerieFolioCFDI para poder instancias sus métodos
                            using (SAT_CL.FacturacionElectronica33.SerieFolioCFDI serie = new SAT_CL.FacturacionElectronica33.SerieFolioCFDI((int)Session["id_registro"]))
                            {
                                if (serie.id_folio_serie > 0) //Si se hace referencia a un registro existente
                                {
                                    resultado = serie.EditaSerieFolioCFDI(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                        txtVersion.Text,
                                        txtSerie.Text,
                                        Convert.ToBoolean(chkActiva.Checked),
                                        Convert.ToInt32(txtFolioInicial.Text),
                                        Convert.ToInt32(txtFolioFinal.Text),
                                        Convert.ToByte(ddlTipoFolioSerie.Text),
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                                }
                            }
                        }    
                        else
                            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptar, "El folio incial no debe ser menor al folio final.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Error, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);              
                    break;
                    }
            }
            //Validar si la inersion se hizo correctamente
            if(resultado.OperacionExitosa)
            {
                //El arrelgo session en su posicion estatus, se asigna el valor lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //El arreglo session en su posicion id_registro se asigna el valor insertado
                Session["id_registro"] = resultado.IdRegistro;
                //Inicializar forma
                inicializarForma();
            }
            else
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptar, "No se realizaron los cambios, compruebe que los valores sean correctos.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Error, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
        }                       
        /// <summary>
        /// Método encargado de habilita los controles para editar o agregar
        /// </summary>
        private void habilitarControles()
        {//Validando estatus de sesion
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:                    
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitando controles
                        txtCompaniaEmisor.Enabled = false;
                        txtVersion.Enabled =
                        txtSerie.Enabled =
                        chkActiva.Enabled =
                        txtFolioInicial.Enabled =
                        txtFolioFinal.Enabled =
                        ddlTipoFolioSerie.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {//Deshabilitando controles                            
                        txtCompaniaEmisor.Enabled =
                        txtVersion.Enabled =
                        txtSerie.Enabled =
                        chkActiva.Enabled =
                        txtFolioInicial.Enabled =
                        txtFolioFinal.Enabled =
                        ddlTipoFolioSerie.Enabled = false;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método encargado de habilitar el menu
        /// </summary>
        private void habilitarMenu()
        { //Validando estatus de session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lnkNuevo.Enabled = 
                        lnkAbrir.Enabled = 
                        lnkGuardar.Enabled =
                        btnAceptar.Enabled = true;
                        lnkBitacora.Enabled = false;
                        lnkReferencias.Enabled = false;
                        //Edicion
                        lnkEditar.Enabled = 
                        lnkEliminar.Enabled = false;
                        //Herramientas
                        lnkBitacora.Enabled =
                        lnkReferencias.Enabled = false;
                        //Ayuda
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lnkNuevo.Enabled =
                        lnkGuardar.Enabled =
                        lnkAbrir.Enabled = true;
                        btnAceptar.Enabled = false;
                        //Edicion
                        lnkEditar.Enabled = true;
                        lnkEliminar.Enabled = false;
                        //Herramientas
                        lnkBitacora.Enabled =
                        lnkReferencias.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {//Archivo
                        lnkNuevo.Enabled =
                        lnkAbrir.Enabled =
                        lnkGuardar.Enabled = true;
                        btnAceptar.Enabled = true;
                        //Edicion
                        lnkEditar.Enabled = false;
                        lnkEliminar.Enabled = true;
                        //Herramientas
                        lnkBitacora.Enabled =
                        lnkReferencias.Enabled = true;
                        break;
                    }
            }

        }
        /// <summary>
        /// Método que inicializa el cuadro de dialogo para apertura de reigstros
        /// </summary>        
        private void inicializaAperturaRegistro(int idTabla, int idCompañia)
        {
            //Definiendo el Id de la tabla por abrir
            Session["id_tabla"] = idTabla;
            //Construyendo URL
            string url = String.Format("{0}?P1={1}", 
                Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/SerieFolioCFDI.aspx",
                "~/Accesorios/AbrirRegistro.aspx"),
                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
            //Definiendo configuracion de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo nueva ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla usada</param>
        /// <param name="Titulo">Titulo de la ventana</param>
        private void inicializarBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL
            string url = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/SerieFolioCFDI.aspx", 
                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo configuracion de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=700";
            //Abriendo Nueva ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora Serie Folio CFDI", configuracion, Page);
        }
        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializarForma()
        {
            //Validando que exista un Estatus
            if (Session["estatus"] == null)
                //Asignando a estatus "Nuevo"
                Session["estatus"] = Pagina.Estatus.Nuevo;                 
            //Invocar métodos de inicio de página
            habilitarMenu();
            habilitarControles();
            cargaCatálogo();
            inicializaValores();            
            //Mostrando enfoque al primer control
            txtVersion.Focus();
            this.Form.DefaultButton = btnAceptar.UniqueID;
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Producto
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/SerieFolioCFDI.aspx", 
                "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Producto
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencia de Registros Serie Folio CFDI", 
                800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de carga el valor de los controles por defecto
        /// </summary>
        private void inicializaValores()
        {   //Validando estatus de sesion
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Establecer valores vacios
                        //txtCompaniaEmisor.Text = "";
                        lblIdSerieFolioCFDI.Text = "";
                        txtVersion.Text = "";
                        txtSerie.Text = "";
                        chkActiva.Checked = false;
                        txtFolioInicial.Text = "";
                        txtFolioFinal.Text = "";
                        ddlTipoFolioSerie.SelectedIndex = 0;
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Asigina al txtCompania el nombre de la compañia del usuario.
                            txtCompaniaEmisor.Text = emisor.nombre + " ID:" + emisor.id_compania_emisor_receptor.ToString();
                        }
                        break;
                    }
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {   //Instanciando la clase
                        using(SAT_CL.FacturacionElectronica33.SerieFolioCFDI sfc = new SAT_CL.FacturacionElectronica33.SerieFolioCFDI(Convert.ToInt32 (Session["id_registro"])))
                        { //Asignando valores

                            lblIdSerieFolioCFDI.Text = sfc.id_folio_serie.ToString();
                            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                //Asigina al txtCompania el nombre de la compañia del usuario.
                                txtCompaniaEmisor.Text = emisor.nombre + " ID:" + sfc.id_compania_emisor.ToString();
                            }
                            //txtCompaniaEmisor.Text = sfc.id_compania_emisor.ToString();
                            txtVersion.Text = sfc.version_cfdi.ToString();
                            txtSerie.Text =sfc.serie .ToString ();
                            chkActiva.Checked = sfc.activa;
                            txtFolioInicial.Text = sfc.folio_inicial.ToString();
                            txtFolioFinal.Text = sfc.folio_final.ToString();
                            ddlTipoFolioSerie.SelectedValue = sfc.id_tipo_folio_serie.ToString();
                            break;
                        }
                    }
            }
        }                
                
        #endregion        
    }
}