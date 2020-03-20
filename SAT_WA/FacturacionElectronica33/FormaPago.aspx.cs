using SAT_CL;
using SAT_CL.FacturacionElectronica33;
using System;
using System.Data;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.FacturacionElectronica33
{
    public partial class FormaPago : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Al dar click en el botón Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            guardaFormaPago();
        }
        /// <summary>
        /// Evento que se activa al presionar el boton Agregar Validacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarValidacion_Click(object sender, EventArgs e)
        {
            //Declarar objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            if (gvValidacionFormaPago.SelectedIndex == -1) //Si el indice es -1 es que no hay ningun registro seleccionado
            {
                //Insertar Nómina de Empleado
                retorno = SAT_CL.FacturacionElectronica33.ValidacionFormaPago.InsertaValidacionFormaPago(
                    Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlValidaciones.SelectedValue), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                //Validar que se insertó
                if (retorno.OperacionExitosa)
                {
                    //Vuelve a cargar el GV
                    cargaValidacionFormaPago();
                    //Mostrar mensaje
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarValidacion, "La validación se agregó correctamente.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
                }
            }
            else //Si es diferente, significa que hay uno seleccionado
            {
                //Editra registro
                using (SAT_CL.FacturacionElectronica33.ValidacionFormaPago valFP = new SAT_CL.FacturacionElectronica33.ValidacionFormaPago(Convert.ToInt32(gvValidacionFormaPago.SelectedValue)))
                {
                    valFP.EditaValidacionFormaPago((int)Session["id_registro"], Convert.ToInt32(ddlValidaciones.SelectedValue), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarValidacion, "La validación se editó correctamente.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
                }
            }
            cargaValidacionFormaPago();
        }
        /// <summary>
        /// Al dar click en el botón Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Pasar session a lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        lblAccionValicacion.Text = "Agregar: ";
                        break;
                    }
            }
            inicializaPagina();
        }
        /// <summary>
        /// Al dar click a un elemento del Nav
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkElementoMenu_Click(object sender, EventArgs e)
        {
            //Hacer referencia al control LinkButton
            using (LinkButton boton = (LinkButton)sender)
            {   //Según el nombre del control
                switch (boton.CommandName)
                {
                    case "Nuevo":
                        {
                            //Asignar el estatus de la página a "Nuevo".
                            Session["estatus"] = Pagina.Estatus.Nuevo;
                            //Iniciar variable de registro
                            Session["id_registro"] = 0;
                            inicializaPagina();
                            break;
                        }
                    case "Abrir":
                        {
                            //Inicializando ventana de búsqueda.
                            inicializaAperturaRegistro(205, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                            //Cambiando estatus a Lectura
                            Session["estatus"] = Pagina.Estatus.Lectura;
                            break;
                        }
                    case "Guardar":
                        {
                            //Método de guardado
                            guardaFormaPago();
                            break;
                        }
                    case "Editar":
                        {
                            using (SAT_CL.FacturacionElectronica33.FormaPago FP = new SAT_CL.FacturacionElectronica33.FormaPago((int)Session["id_registro"]))
                            {
                                //Cambiar Estatus a edicion
                                Session["estatus"] = Pagina.Estatus.Edicion;
                                //Limpia el contenido de la página
                                inicializaPagina();
                            }
                            break;
                        }
                    case "Eliminar":
                        {
                            //Método para deshabiltiar el registro actual
                            deshabilitaFormaPago();
                            break;
                        }
                    case "Bitacora":
                        {
                            //Inicializa la página para visualizar la bitacora del registro.
                            inicializaBitacora(Session["id_registro"].ToString(), "205", "Bitacora de Registro - Forma de Pago");
                            break;
                        }
                    case "Referencias":
                        {
                            //Inicializa la página para ver las Referencias del registro
                            inicializaReferenciaRegistro (Session["id_registro"].ToString(),"205",((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento producido al pulsar el LinkButton "Exportar Forma de Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "ValidacionFormaPago":
                    {
                        //Exportando Contenido
                        string[] columnasNoDeseadas = { "Id", "FormaPago", "TipoValidacion" };
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),columnasNoDeseadas);
                        break;
                    }                
            }
        }
        /// <summary>
        /// Evento al iniciar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad a la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validar que no se produjo un PostBack
            if (!Page.IsPostBack)
                //Inicializar el estatus general de la forma.
                inicializaPagina();
        }

        //Eventos del GridView        
        /// <summary>
        /// Evento producido al cambiar el orden edl GridView FormaPago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValidacionFormaPago_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiar Label Ordenado
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvValidacionFormaPago, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Método encargado de enlazar el GridView ValidacionFormaPago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValidacionFormaPago_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        /// <summary>
        /// Se activa al presionar Editar en un registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizaValidacion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvValidacionFormaPago.DataKeys.Count > 0)
            {
                //Seleccionar Fila
                Controles.SeleccionaFila(gvValidacionFormaPago, sender, "lnk", false);
                //Declarar objeto retorno
                RetornoOperacion retorno = new RetornoOperacion();
                //Instanciar Validacion Forma Pago
                using (SAT_CL.FacturacionElectronica33.ValidacionFormaPago ValFP = new SAT_CL.FacturacionElectronica33.ValidacionFormaPago(Convert.ToInt32(gvValidacionFormaPago.SelectedDataKey["Id"])))
                {
                    //Validar registro
                    if(ValFP.habilitar)
                    {
                        //Obtener control
                        LinkButton lnk = (LinkButton)sender;
                        //Validar Comando
                        switch (lnk.CommandName)
                        {
                            case "Eliminar":
                                {
                                    //Deshabilitar Validacion
                                    retorno = ValFP.DeshabilitaValidacionFormaPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    //Se deshabilitó con éxito?
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Inicializando indices
                                        Controles.InicializaIndices(gvValidacionFormaPago);
                                        //Recargar gvValidacion
                                        cargaValidacionFormaPago();
                                        TSDK.ASP.ScriptServer.MuestraNotificacion(lnk, "La validación se eliminó correctamente.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
                                    }
                                    break;
                                }
                            case "Editar"://Editar validacion
                                {
                                    int index = gvValidacionFormaPago.SelectedIndex;
                                    int value = Convert.ToInt32(gvValidacionFormaPago.SelectedValue);
                                    lblAccionValicacion.Text = "Editar: ";
                                    
                                    using (SAT_CL.FacturacionElectronica33.ValidacionFormaPago valFP = new SAT_CL.FacturacionElectronica33.ValidacionFormaPago(value))
                                    {
                                        ddlValidaciones.SelectedIndex = Convert.ToInt32(valFP.id_tipo_validacion)-1;
                                    }
                                    
                                    //TSDK.ASP.ScriptServer.MuestraNotificacion(lnk, "Entra. Indice="+index+" V:"+value, TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
                                    
                                    break;
                                }
                        }
                    }
                }
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de llenar los GridView de la página
        /// </summary>
        private void CargaCatalogos()
        {
            //Tamaño del gridview
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlValidaciones, "", 3192);
        }
        /// <summary>
        /// Método que elimina el registro actual
        /// </summary>
        private void deshabilitaFormaPago()
        {
            //Crear objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Invocar constructor de la clase para instanciar métodos
            using (SAT_CL.FacturacionElectronica33.FormaPago FP = new SAT_CL.FacturacionElectronica33.FormaPago(Convert.ToInt32(Session["id_registro"])))
            {
                //Valida que exista un registro
                if (FP.id_forma_pago > 0)
                    retorno = FP.DeshabilitaFormaPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Validar que se realizó con éxito
            if (retorno.OperacionExitosa)
            {
                //Cambiar el valor del id registro
                Session["id_registro"] = 0;
                //Cambiar el estatus a Nuevo
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Limpiar la página
                inicializaPagina();
                //Mensaje. se deshabilitó
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

        }
        /// <summary>
        /// Método que guarda el registro actual
        /// </summary>
        private void guardaFormaPago()
        {
            //Creando objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Segun el estatus de la página
            switch ((Pagina.Estatus)Session ["estatus"])
            {
                //Insertar registro
                case Pagina.Estatus.Nuevo:
                    {
                        retorno = SAT_CL.FacturacionElectronica33.FormaPago.InsertaFormaPago(
                            txtClave.Text,
                            txtDescripcion.Text,
                            txtCuentaOrdenante.Text,
                            txtCuentaBeneficiario.Text,
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Invocar contructor  de la clase FormaPago para poder instanciar métodos
                        using (SAT_CL.FacturacionElectronica33.FormaPago formaPago = new SAT_CL.FacturacionElectronica33.FormaPago((int)Session["id_registro"]))
                        {
                            if (formaPago.id_forma_pago > 0)//Si es un registro valido
                            {
                                retorno = formaPago.EditaFormaPago(
                                    txtClave.Text, 
                                    txtDescripcion.Text, 
                                    txtCuentaOrdenante.Text, 
                                    txtCuentaBeneficiario.Text,
                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                            }
                        }
                        break;
                    }
            }
            //Validar si se realizó con éxito
            if(retorno.OperacionExitosa)
            {
                //Cambiando el estatus a Lectura
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Cambiando el id_registro al valor insertado
                Session["id_registro"] = retorno.IdRegistro;
                //Limpiar Página
                inicializaPagina();
            }
            else
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptar, "Registro no guardado.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Error, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoCentro);
        }
        /// <summary>
        /// Método que permite habilitar controles segun el estatus.
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitar controles
                        txtClave.Enabled = 
                        txtCuentaBeneficiario.Enabled = 
                        txtCuentaOrdenante.Enabled = 
                        txtDescripcion.Enabled = true;
                        ddlValidaciones.Enabled = true;
                        btnAgregarValidacion.Enabled = true;
                        gvValidacionFormaPago.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilitar controles
                        txtClave.Enabled =
                        txtCuentaBeneficiario.Enabled =
                        txtCuentaOrdenante.Enabled =
                        txtDescripcion.Enabled = false;
                        ddlValidaciones.Enabled = false;
                        btnAgregarValidacion.Enabled = false;
                        gvValidacionFormaPago.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método que permite habilitar los menus segun el estatus.
        /// </summary>
        private void habilitaMenu()
        {
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
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lnkNuevo.Enabled = true;
                        lnkGuardar.Enabled = false;
                        lnkAbrir.Enabled = true;
                        btnAceptar.Enabled = false;
                        btnAgregarValidacion.Enabled = false;
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
                        btnAgregarValidacion.Enabled = true;
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
        /// Método que inicia la ventana Pop-up para buscar y abrir registros.
        /// </summary>
        /// <param name="idTabla">Número de tabla</param>
        /// <param name="idCompania">Número de la compania</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Definiendo el Id de la tabla por abrir
            Session["id_tabla"] = idTabla;
            //Construye el URL
            string url = String.Format("{0}?P1={1}", 
                                        Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FormaPago.aspx", "~/Accesorios/AbrirRegistro.aspx"), 
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
            //Definiendo configuracion de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo nueva ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRegistro">Número de registro</param>
        /// <param name="idTabla">Número de la tabla</param>
        /// <param name="Titulo">Título de la entana Pop-up</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL
            string url = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FormaPago.aspx",
                "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo configuracion de la ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=700";
            //Abriendo Nueva ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora de Forma Pago", configuracion, Page);
        }
        /// <summary>
        /// Método que inicializa los controles de toda la página
        /// </summary>
        private void inicializaPagina()
        {            
            //Validar que no exista un estatus
            if (Session["estatus"] == null)
                //Cambiar el estatus a Nuevo
                Session["estatus"] = Pagina.Estatus.Nuevo;
            //Invocar métodos para inciar página
            habilitaMenu();
            habilitaControles();
            CargaCatalogos();
            inicializaValores();            
            //Cambiando el foco al primer control
            txtClave.Focus();
            cargaValidacionFormaPago();
        }
        /// <summary>
        /// Método que inicia el Pop-up de Referencias del registro
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idTabla"></param>
        /// <param name="idCompania"></param>
        private void inicializaReferenciaRegistro(string idRegistro,string idTabla, string idCompania)
        {
            //Asigna a la variable urlDestino una ruta de ubucación de la ventana de Referencia de la tabla Producto
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FormaPago.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&idC=" + idCompania);
            //Invoca al método de la clase ScripServer que abrira una nueva ventana con los valores de Referencia de la tabla Producto
            ScriptServer.AbreNuevaVentana(urlDestino, "Referencias de Forma de Pago", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método que llena los controles con los valores de acuerdo al estatus.
        /// </summary>
        private void inicializaValores()
        {   //Según el estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Valores vacíos
                        txtClave.Text = "";
                        txtCuentaBeneficiario.Text = "";
                        txtCuentaOrdenante.Text = "";
                        txtDescripcion.Text = "";
                        ddlValidaciones.SelectedIndex = 0;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciar a la clase, en el constructor que espera un id de registro.
                        using (SAT_CL.FacturacionElectronica33.FormaPago formaPago = new SAT_CL.FacturacionElectronica33.FormaPago(Convert.ToInt32 (Session["id_registro"])))
                        {
                            //Asignando valores
                            txtClave.Text = formaPago.clave.ToString();
                            txtCuentaBeneficiario.Text = formaPago.patron_cta_beneficiario.ToString();
                            txtCuentaOrdenante.Text = formaPago.patron_cta_ordenante.ToString();
                            txtDescripcion.Text = formaPago.descripcion.ToString();
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de cargar la tabla ValidacionFormaPago
        /// </summary>
        private void cargaValidacionFormaPago()
        {
            lblAccionValicacion.Text = "Agregar: ";
            //Validando estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Cargar GridView
                        Controles.InicializaGridview(gvValidacionFormaPago);
                        
                        //Añade resultado a session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                        break;
                    }
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instanciar Validacion Forma Pago
                        using (DataTable dtValidacionFP = SAT_CL.FacturacionElectronica33.ValidacionFormaPago.ObtieneValidacionesFP(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que existan registros
                            if (Validacion.ValidaOrigenDatos(dtValidacionFP))
                            {
                                //Cargando GridView
                                Controles.CargaGridView(gvValidacionFormaPago, dtValidacionFP, "Id", lblOrdenado.Text, true, 1);
                                //Añade resultados a session
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtValidacionFP, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                Controles.InicializaGridview(gvValidacionFormaPago);
                                //Añade resultado a session
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}