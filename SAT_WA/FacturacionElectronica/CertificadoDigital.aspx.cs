using SAT_CL;
using SAT_CL.Global;
using System;
using System.IO;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.FacturacionElectronica
{
    public partial class CertificadoDigital : System.Web.UI.Page
    {        
        #region Eventos

        /// <summary>
        /// Evento generado al dar click en el botón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Guarda Certificado Digital
            guardaCertificadoDigital();

        }

        /// <summary>
        /// Evento generado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarContrasenaRevocacion_Click(object sender, EventArgs e)
        {
            revocar();
        }        

        /// <summary>
        /// Evento generado al Cargar los Archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCargarArchivos_Click(object sender, EventArgs e)
        {
            //Limpiamos Atributos ViewState
            ViewState["BytesArchivoCer"] = null;
            ViewState["ExtensionArchivoCer"] = null;
            ViewState["BytesArchivoKey"] = null;
            ViewState["ExtensionArchivoKey"] = null;
            //Asignando Atributos ViewState
            ViewState["BytesArchivoCer"] = fuArchivoCer.FileBytes;
            ViewState["ExtensionArchivoCer"] = Path.GetExtension(fuArchivoCer.PostedFile.FileName);
            ViewState["BytesArchivoKey"] = fuArchivoKey.FileBytes;
            ViewState["ExtensionArchivoKey"] = Path.GetExtension(fuArchivoKey.PostedFile.FileName);
        }

        /// <summary>
        /// Evento producido al pulsar algún elemento del menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Referenciando al botón pulsado
            using (LinkButton boton = (LinkButton)sender)
            {
                //Determinando que botón fue pulsado
                switch (boton.CommandName)
                {
                    case "Revocar":
                        //Abrir Ventana Modal
                        ScriptServer.AlternarVentana(lkbRevocar, lkbRevocar.GetType(), "AbrirVentana", "contenidoConfirmacionContrasenaRevocacion", "confirmacionContrasenaRevocacion");
                        //Limpiamos Etiqueta
                        lblErrorContrasenaRevocacion.Text = "";
                        break;
                    case "Nuevo":
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializando Id de Registro activo
                        Session["id_registro"] = 0;
                        //Inicializamos la pagina
                        inicializaPagina();
                        break;
                    //Permite abrir registros de los Proveedore
                    case "Abrir":
                        //Inicializando ventana de apertura
                        inicializaAperturaRegistro();
                        break;
                    //Guarda el registro en la BD
                    case "Guardar":
                        guardaCertificadoDigital();
                        break;
                    case "Bitacora":
                        //Mostrando ventana
                        inicializaBitacora(Session["id_registro"].ToString(), "114", "Certificado Digital");
                        break;
                    case "Eliminar":
                        deshabilitarCertificado();
                        break;
                    case "Referencia":
                        //Preparando consulta de accesorios
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(),"114", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;


                }
            }
        }        

        /// <summary>
        /// Evento generado al adr click en Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarContrasenaRevocacion_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarContrasenaRevocacion, lkbCerrarContrasenaRevocacion.GetType(), "CerarVentana", "contenidoConfirmacionContrasenaRevocacion", "confirmacionContrasenaRevocacion");

            //Inicializa Valores
            inicializoValoresRevocacion();
        }

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no hay recarga de la misma página
            if (!Page.IsPostBack)
                //Inicializando estatus general de la forma
                inicializaPagina();
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo encargado de cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga Catalogo Estatus Certificado Digital
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 1113);
            //Carga Catalogo Tipo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "", 1112);
            //Carga Catalogo Emisor
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEmisor, 8, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Carga Catalogo Sucursal
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", Convert.ToInt32(ddlEmisor.SelectedValue), "",
                                                                                              0, "");


        }

        /// <summary>
        ///  Deshabilitamos Certificado
        /// </summary>
        private void deshabilitarCertificado()
        {
            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando registro actual
                using (SAT_CL.Global.CertificadoDigital objCertificado = new SAT_CL.Global.CertificadoDigital(Convert.ToInt32(Session["id_registro"])))
                {
                    resultado = objCertificado.DeshabilitaCertificado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    if (resultado.OperacionExitosa)
                    {
                        //Deshabilitamos Registros Archivo
                        using (ArchivoRegistro objArchivoCer = new
                           ArchivoRegistro(objCertificado.idCer))

                            //Deshabilitamos Registro
                            resultado = objArchivoCer.DeshabilitaArchivoRegistro(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Deshabilitamos Registros Archivo
                            using (ArchivoRegistro objArchivoKey = new
                               ArchivoRegistro(objCertificado.idKey))

                                //Deshabilitamos Registro
                                resultado = objArchivoKey.DeshabilitaArchivoRegistro(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);



                            //Si se deshabilitó Correctamente
                            if (resultado.OperacionExitosa)
                            {
                                //Establecemos el id del registro
                                Session["id_registro"] = 0;
                                //Establecemos el estatus de la forma
                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                //Inicializamos la forma
                                inicializaPagina();
                            }
                        }

                    }

                }

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Mostrando error
            lblError.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Metodo encargado de Guardar un Proveedor
        /// </summary>
        private void guardaCertificadoDigital()
        {
            //Validamos que exista carga de Archivos  
            if (ViewState["ExtensionArchivoCer"] != null && ViewState["ExtensionArchivoKey"] != null)
            {
                //Validamos Archivo   
                if (ViewState["ExtensionArchivoCer"].ToString() == ".cer" && ViewState["ExtensionArchivoKey"].ToString() == ".key")
                {
                    //Declaracion de objeto resultado
                    RetornoOperacion resultado = new RetornoOperacion();
                    //Declaramos Variable para Guardar Certificado
                    int id_Certificado = 0, id_archivo_cer = 0;
                    //Creamos la transacción 
                    using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //De acuerdo al estatus de la pagina
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Insertando Proveedor
                            case Pagina.Estatus.Nuevo:
                                {

                                    resultado = SAT_CL.Global.CertificadoDigital.InsertaCertificado(Convert.ToInt32(ddlEmisor.SelectedValue),
                                                                                        Convert.ToInt32(ddlSucursal.SelectedValue), (SAT_CL.Global.CertificadoDigital.TipoCertificado)
                                                                                        Convert.ToByte(ddlTipo.SelectedValue), txtNuevaContrasena.Text,
                                                                                        txtNuevaContrasenaRevocacion.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                                                                        );
                                    //Asignamos Variable Certificado
                                    id_Certificado = resultado.IdRegistro;

                                    //Si se insertó Correctamen el Ceriticado Digital
                                    if (resultado.OperacionExitosa)
                                    {
                                        int key = 0;
                                        int cer = 0;
                                        //Validamos Tipo de Certificado
                                        switch ((SAT_CL.Global.CertificadoDigital.TipoCertificado)Convert.ToInt32(ddlTipo.SelectedValue))//FIEL
                                        {
                                            case SAT_CL.Global.CertificadoDigital.TipoCertificado.FIEL:
                                                cer = 12; ;
                                                key = 14;
                                                break;
                                            case SAT_CL.Global.CertificadoDigital.TipoCertificado.CSD:
                                                cer = 13;
                                                key = 15;
                                                break;
                                        }

                                        //Instanciamos Emisor
                                        using (CompaniaEmisorReceptor objEmisor = new CompaniaEmisorReceptor(Convert.ToInt32(ddlEmisor.SelectedValue)))
                                        {
                                            //Validamos Emisor
                                            if (objEmisor.id_compania_emisor_receptor > 0)
                                            {
                                                //Construyendo ruta de almacenamiento de certificados
                                                string ruta_certificado = string.Format(@"{0}{1}\{2}-{3}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 114.ToString("0000"), id_Certificado.ToString("0000000"), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyMMddHHmmss"));
                                                //Insertamos Archivo Registro .cer
                                                resultado = ArchivoRegistro.InsertaArchivoRegistro(114, id_Certificado, cer,
                                                                        "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, (byte[])ViewState["BytesArchivoCer"], ruta_certificado + ".cer");
                                                //Validamos Resultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Asignando Id de archivo certificado (.cer)
                                                    id_archivo_cer = resultado.IdRegistro;

                                                    //Insertamos Archivo Registro (.key)
                                                    resultado = ArchivoRegistro.InsertaArchivoRegistro(114, id_Certificado, key,
                                                                        "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, (byte[])ViewState["BytesArchivoKey"], ruta_certificado + ".key");

                                                    //Si no se guardó correctamente
                                                    if (!resultado.OperacionExitosa)
                                                    {
                                                        //Deshabilitamos Registros Archivo
                                                        using (ArchivoRegistro objArchivoKey = new ArchivoRegistro(id_archivo_cer))
                                                            //Deshabilitamos Registro
                                                            objArchivoKey.DeshabilitaArchivoRegistro(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                }
                                            }
                                            else
                                                resultado = new RetornoOperacion("No se encontrarón datos complementarios Emisor");
                                        }
                                    }

                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {

                                    break;
                                }
                        }
                        //ValidamosResultado
                        if (resultado.OperacionExitosa)
                        {
                            //Finalizamos Transacción
                            scope.Complete();
                        }
                    }
                    //Validamos que la operacion se haya realizado
                    if (resultado.OperacionExitosa)
                    {
                        //Establecemos el id del registro
                        Session["id_registro"] = id_Certificado;
                        //Establecemos el estatus de la forma
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        //Inicializamos la forma
                        inicializaPagina();
                    }

                    //Actualizamos la etiqueta de errores
                    lblError.Text = resultado.Mensaje;
                }
                else
                    lblError.Text = "Los archivos de certificado seleccionados no tienen las extensiones apropiadas.";
            }
            else
            {

                lblError.Text = "Los archivos aún no se han cargado.";
            }

        }

        /// <summary>
        /// Metodo encargado de habilitar y deshabilitar los controles de la forma 
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        ddlEstatus.Enabled = false;
                        ddlEmisor.Enabled =
                        ddlTipo.Enabled =
                        ddlSucursal.Enabled =
                        txtNuevaContrasena.Enabled =
                        txtConfirmarNuevaContrasena.Enabled =
                        txtNuevaContrasenaRevocacion.Enabled =
                        txtConfirmarNuevaContrasenaRevocacion.Enabled =
                        btnAceptar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        ddlEstatus.Enabled =
                        ddlEmisor.Enabled =
                        ddlTipo.Enabled =
                        ddlSucursal.Enabled =
                        txtNuevaContrasena.Enabled =
                        txtConfirmarNuevaContrasena.Enabled =
                        txtNuevaContrasenaRevocacion.Enabled =
                        txtConfirmarNuevaContrasenaRevocacion.Enabled =
                        btnAceptar.Enabled = false;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que inicializa el cuadro de dialogo para apertura de registros
        /// </summary>
        private void inicializaAperturaRegistro()
        {
            //Definiendo el Id de tabla por abrir
            Session["id_tabla"] = 114;

            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/CertificadoDigital.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Recepción de Facturas", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/CertificadoDigital.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Cobros", configuracion, Page);
        }

        /// <summary>
        /// Metodo encargado de Inicializar el Menu
        /// </summary>
        /// 
        private void inicializaMenu()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        lkbRevocar.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbRevocar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEliminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbRevocar.Enabled = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaPagina()
        {
            //Definiendo el Id de tabla por abrir
            Session["id_tabla"] = 217;
            //Carga catalogos
            cargaCatalogos();
            //habilita controles
            habilitaControles();
            //Inicialia Valores
            inicializaValores();
            //Inicializa Menu
            inicializaMenu();

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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/CertificadoDigital.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Metodo encargado de inicializar los valores de la forma en razon al perfil de usuario
        /// </summary>
        private void inicializaValores()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lblID.Text = "ID";
                        ddlEstatus.SelectedValue = "1";
                        ddlTipo.SelectedValue = "1";
                        ddlSucursal.SelectedValue = "0";
                        txtNuevaContrasena.Text = "";
                        txtConfirmarNuevaContrasena.Text = "";
                        txtNuevaContrasenaRevocacion.Text = "";
                        txtConfirmarNuevaContrasenaRevocacion.Text = "";
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    { 
                        using (SAT_CL.Global.CertificadoDigital objCertificado = new SAT_CL.Global.CertificadoDigital(Convert.ToInt32(Session["id_registro"])))
                        {
                            lblID.Text = objCertificado.id_certificado_digital.ToString();
                            ddlEstatus.SelectedValue = objCertificado.id_estatus_certificado.ToString();
                            ddlTipo.SelectedValue = objCertificado.id_tipo_certificado.ToString();
                            ddlEmisor.SelectedValue = objCertificado.id_emisor.ToString();
                            ddlSucursal.SelectedValue = objCertificado.id_sucursal.ToString();
                            txtNuevaContrasena.Text = "";
                            txtConfirmarNuevaContrasena.Text = "";
                            txtNuevaContrasenaRevocacion.Text = "";
                            txtConfirmarNuevaContrasenaRevocacion.Text = "";
                        }
                        break;
                    }
            }
            
            //Limpiando errores
            lblError.Text = "";
        } 

        /// <summary>
        /// Inicializo valores 
        /// </summary>
        private void inicializoValoresRevocacion()
        {
            //Inicializo Valores
            txtConfirmarContrasenaRevocacion.Text = "";
            txtContrasenaRevocacion.Text = "";
            lblErrorContrasenaRevocacion.Text = "";
        }
        
        /// <summary>
        /// Revocamos Certificado 
        /// </summary>
        private void revocar()
        {
            //Establecemos Objeto Resultado

            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando registro actual
            using (SAT_CL.Global.CertificadoDigital objCertificado = new SAT_CL.Global.CertificadoDigital(Convert.ToInt32(Session["id_registro"])))
            {
                resultado = objCertificado.ActualizaEstatusRevocado(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, txtContrasenaRevocacion.Text);
                //Si se deshabilitó Correctamente
                if (resultado.OperacionExitosa)
                {
                    //Establecemos el id del registro
                    Session["id_registro"] = resultado.IdRegistro;
                    //Establecemos el estatus de la forma
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    //Inicializamos la forma
                    inicializaPagina();
                    //Inicializo Valores Revocacion
                    inicializoValoresRevocacion();
                    //Cerrar Ventana Modal
                    ScriptServer.AlternarVentana(btnAceptarContrasenaRevocacion, btnAceptarContrasenaRevocacion.GetType(), "CerarVentana", "contenidoConfirmacionContrasenaRevocacion", "confirmacionContrasenaRevocacion");

                }

            }
            //Mostrando error
            lblErrorContrasenaRevocacion.Text = resultado.Mensaje;
        }
               
        #endregion
    }
}