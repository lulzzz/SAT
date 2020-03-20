using SAT_CL;
using SAT_CL.Global;
using System;
using System.Data;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Collections.Generic;
//using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.Tarifas
{
    public partial class TipoCargo : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {   //Invocando Método de Guardado
            guardaTipoCargo();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {   //Cambiando Estatus a Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }
            //Inicializando Valores
            inicializaForma();
        }
        /// <summary>
        /// Evento activado al hacer click en el enlace "Agregar Clave"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarClave_Click(object sender, EventArgs e)
        {
            //Validar estatus
            //TSDK.ASP.ScriptServer.AlternarVentana(lnkAgregarClave, "ModalClavePS", "contenedorVentanaAltaClavePS", "ventanaAltaClavePS");
            TSDK.ASP.ScriptServer.AlternarVentana(this, "ModalClavePS", "contenedorVentanaAltaClavePS", "ventanaAltaClavePS");
        }
        /// <summary>
        /// Evento Producido al Presionar un Elemento del Menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando el Nombre del Menú
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {   //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(44, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {   //Invocando Método de Guardado
                        guardaTipoCargo();
                        break;
                    }
                case "Editar":
                    {   //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Eliminar":
                    {   //Instanciando Producto
                        using (SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista un Producto
                            if (tc.id_tipo_cargo != 0)
                            {   //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Deshabilitando Producto
                                result = tc.DeshabilitaTipoCargo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
                                }//Mostrando Mensaje de Operación
                                ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "44", "Compania");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "44", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// Evento activado al presionar el link Exportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;
            //Validando el comando
            switch (lnk.CommandName)
            {
                case "ExportarClaveSP":
                    {
                        //Exportando Contenido
                        string[] columnasNoDeseadas = { "Id", "" };
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), columnasNoDeseadas);
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
                //Inicializando Forma
                inicializaForma();
        }
        
        #region Eventos de Modal/GridView
        /// <summary>
        /// Evento provocado al presionar el botón Agregar dentro de la ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarClave_Click(object sender, EventArgs e)
        {
            //Declarar objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            if (gvClaveSP.SelectedIndex == -1) //Si el índice seleccionado es -1 significa que no hay ningun registro seleccionado y se inserta uno nuevo
            {
                //Insertar Catalogo
                retorno = SAT_CL.Global.Catalogo.InsertaCatalogoConsecutivo(
                    3196,
                    Cadena.RegresaCadenaSeparada(txtClave.Text, "-", 0),
                    Cadena.RegresaCadenaSeparada(txtClave.Text, "-", 1),
                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                //Valida que se insertó
                if (retorno.OperacionExitosa)
                {
                    //Vuelve a cargar el GV
                    cargaClavesSP();
                    //Mostrar mensaje
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarClave, "La clave se agregó correctamente.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            else //Si es diferente, es porque hay uno seleccionado
            {
                //Editar registro
                using (SAT_CL.Global.Catalogo catalogo = new Catalogo(Convert.ToInt32(gvClaveSP.SelectedDataKey["Id"])))
                {
                    catalogo.editaVCadenaDescripcionCatalogo(
                        Cadena.RegresaCadenaSeparada(txtClave.Text, "-", 0),
                        Cadena.RegresaCadenaSeparada(txtClave.Text, "-", 1),
                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario
                        );
                }
            }
            //Reiniciar indices
            Controles.InicializaIndices(gvClaveSP); ;
            //Recargar gridview
            cargaClavesSP();
            txtClave.Text = "";
        }
        /// <summary>
        /// Evento al ordenar las columnas del gvClaveSP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClaveSP_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenar y escribir el orden
            lblOrden.Text = Controles.CambiaSortExpressionGridView(gvClaveSP, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Se activa al presionar Editar o Eliminar en un registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizaClave_Click(object sender, EventArgs e)
        {
            //Validando que existan registros
            if (gvClaveSP.DataKeys.Count > 0)
            {
                //Selecciona Fila
                Controles.SeleccionaFila(gvClaveSP, sender, "lnk", false);
                //Declarar Objeto Retorno
                RetornoOperacion retorno = new RetornoOperacion();
                //Instanciar clase catalogo
                using (SAT_CL.Global.Catalogo catalogo = new Catalogo(Convert.ToInt32(gvClaveSP.SelectedDataKey["Id"])))
                {
                    //Validar registro
                    if (catalogo.habilitar)
                    {
                        //Determinar accion
                        LinkButton lnk = (LinkButton)sender;
                        switch (lnk.CommandName)
                        {
                            case "Editar":
                                {
                                    txtClave.Text = String.Format("{0}-{1}", Convert.ToString(catalogo.idValorCadena), Convert.ToString(catalogo.descripcion));
                                    break;
                                }
                            case "Eliminar":
                                {
                                    //Deshabilitar
                                    retorno = catalogo.DeshabilitarCatalogo(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    //Se deshabilitó con éxito?
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Reinicia indices
                                        Controles.InicializaIndices(gvClaveSP);
                                        //Recarga gridview
                                        cargaClavesSP();
                                        TSDK.ASP.ScriptServer.MuestraNotificacion(lnk, "La clave se eliminó correctamente.", TSDK.ASP.ScriptServer.NaturalezaNotificacion.Exito, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Alternar ventana
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrar, "ModalClavePS", "contenedorVentanaAltaClavePS", "ventanaAltaClavePS");
        }
        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlUnidadMedida, "", 25);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCargo, "", 27);
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMoneda, 110, "-Moneda-");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTarifaBase, 15, "Ninguna");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoImpTras, "", 94, 2);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoImpRet, "", 94, 1);
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Cambios de los Tipos de Cargo
        /// </summary>
        private void guardaTipoCargo()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Insertando el Tipo de Cargo
                        result = SAT_CL.Tarifas.TipoCargo.InsertaTipoCargo(
                            txtDescripcion.Text,
                            Convert.ToByte(ddlUnidadMedida.SelectedValue),
                            (SAT_CL.Tarifas.TipoCargo.TipoImpTrasladado)Convert.ToByte(ddlTipoImpTras.SelectedValue),
                            Convert.ToDecimal(txtTasaIT.Text),
                            (SAT_CL.Tarifas.TipoCargo.TipoImpRetenido)Convert.ToByte(ddlTipoImpRet.SelectedValue),
                            Convert.ToDecimal(txtTasaIR.Text),
                            Convert.ToByte(ddlTipoCargo.SelectedValue),
                            Convert.ToByte(ddlMoneda.SelectedValue),
                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                            Convert.ToDecimal(txtTasaImp1.Text.Equals("") ? "0" : txtTasaImp1.Text),
                            Convert.ToDecimal(txtTasaImp2.Text.Equals("") ? "0" : txtTasaImp2.Text),
                            txtCtaContable.Text,
                            Convert.ToInt32(ddlTarifaBase.SelectedValue),
                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtIdClaveSAT.Text, "ID:", 1)),
                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Tipo de Cargo
                        using (SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista el Registro
                            if (tc.id_tipo_cargo != 0)
                            {   //Editando el Tipo de Cargo
                                result = tc.EditaTipoCargo(
                                    txtDescripcion.Text,
                                    Convert.ToByte(ddlUnidadMedida.SelectedValue),
                                    (SAT_CL.Tarifas.TipoCargo.TipoImpTrasladado)Convert.ToByte(ddlTipoImpTras.SelectedValue),
                                    Convert.ToDecimal(txtTasaIT.Text),
                                    (SAT_CL.Tarifas.TipoCargo.TipoImpRetenido)Convert.ToByte(ddlTipoImpRet.SelectedValue),
                                    Convert.ToDecimal(txtTasaIR.Text),
                                    Convert.ToByte(ddlTipoCargo.SelectedValue),
                                    Convert.ToByte(ddlMoneda.SelectedValue),
                                    ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                    Convert.ToDecimal(txtTasaImp1.Text.Equals("") ? "0" : txtTasaImp1.Text),
                                    Convert.ToDecimal(txtTasaImp2.Text.Equals("") ? "0" : txtTasaImp2.Text),
                                    txtCtaContable.Text,
                                    Convert.ToInt32(ddlTarifaBase.SelectedValue),
                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtIdClaveSAT.Text, "ID:", 1)),
                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }
            }
            //Validando que la Operación haya sido Exitosa
            if (result.OperacionExitosa)
            {   //Asignando Valores de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Inicializando Forma
                inicializaForma();
            }
            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método Prvado encargado de Habilitar los Controles
        /// </summary>
        private void habilitarControles()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                    {   //Habilitando Controles
                        txtIdClaveSAT.Enabled =
                        txtDescripcion.Enabled =
                        txtTasaIT.Enabled =
                        txtTasaIR.Enabled =
                        ddlTipoCargo.Enabled =
                        ddlMoneda.Enabled =
                        txtTasaImp1.Enabled =
                        txtTasaImp2.Enabled =
                        txtCtaContable.Enabled =
                        ddlTarifaBase.Enabled =
                        btnCancelar.Enabled =
                        ddlUnidadMedida.Enabled =
                        ddlTipoImpRet.Enabled =
                        ddlTipoImpTras.Enabled =
                        btnAceptar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Habilitando Controles
                        txtIdClaveSAT.Enabled =
                        txtDescripcion.Enabled =
                        txtTasaIT.Enabled =
                        txtTasaIR.Enabled =
                        ddlTipoCargo.Enabled =
                        ddlMoneda.Enabled =
                        txtTasaImp1.Enabled =
                        txtTasaImp2.Enabled =
                        txtCtaContable.Enabled =
                        ddlTarifaBase.Enabled =
                        btnCancelar.Enabled =
                        ddlUnidadMedida.Enabled =
                        ddlTipoImpTras.Enabled =
                        ddlTipoImpRet.Enabled =
                        btnAceptar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnAceptar.Enabled = true;
                        //Edicion
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnAceptar.Enabled = false;
                        //Edicion
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnAceptar.Enabled = true;
                        //Edicion
                        lkbEliminar.Enabled =
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {   //Habilitando Menú
            habilitaMenu();
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Controles
            habilitarControles();
            //Inicializando Valores
            inicializaValores();
            //Carga GridView
            Controles.InicializaGridview(gvClaveSP);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Asignando Valores
                        txtIdClaveSAT.Text =
                        txtDescripcion.Text =
                        txtCtaContable.Text = "";
                        txtTasaIT.Text =
                        txtTasaIR.Text =
                        txtTasaImp1.Text =
                        txtTasaImp2.Text = "0.00";
                        
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {   //Instanciando Tipo de Carga
                        using(SAT_CL.Tarifas.TipoCargo tc = new SAT_CL.Tarifas.TipoCargo(Convert.ToInt32(Session["id_registro"])))
                        {   //Validando que exista el Registro
                            if(tc.id_tipo_cargo != 0)
                            {   //Asignando Valores
                                txtIdClaveSAT.Text = string.Format("[{0}] {1} ID:{2}", Catalogo.RegresaDescripcioValorCadena(3196, tc.id_catalogo_sat), Catalogo.RegresaDescripcionCatalogo(3196, tc.id_catalogo_sat), tc.id_catalogo_sat);
                                txtDescripcion.Text = tc.descripcion;
                                txtTasaIT.Text = tc.tasa_impuesto_trasladado.ToString();
                                txtTasaIR.Text = tc.tasa_impuesto_retenido.ToString();
                                txtTasaImp1.Text = tc.tasa_impuesto1.ToString();
                                txtTasaImp2.Text = tc.tasa_impuesto2.ToString();
                                txtCtaContable.Text = tc.cuenta_contable;
                                ddlUnidadMedida.SelectedValue = tc.id_unidad.ToString();
                                ddlMoneda.SelectedValue = tc.id_moneda.ToString();
                                ddlTarifaBase.SelectedValue = tc.id_base_tarifa.ToString();
                                ddlTipoCargo.SelectedValue = tc.tipo_cargo.ToString();
                                ddlTipoImpTras.SelectedValue = tc.id_tipo_impuesto_trasladado.ToString();
                                ddlTipoImpRet.SelectedValue = tc.id_tipo_impuesto_retenido.ToString();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/TipoCargo.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/TipoCargo.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Tarifas/TipoCargo.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de cargar las Claves de SAT para Servicio o Producto en la ventana modal
        /// </summary>
        private void cargaClavesSP()
        {
            //Validacion
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                //{
                //    //CargarGridView
                //    Controles.InicializaGridview(gvClaveSP);
                //    //Añade resultado a session
                //    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //    break;
                //}
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instanciar Catalogo
                        //TipoCatalogo 3196: Claves Servicio-Producto
                        using (DataTable dtClavesSP = SAT_CL.Global.Catalogo.ObtieneCatalogos(3196))
                        {
                            //Validar que contenga registros
                            if (Validacion.ValidaOrigenDatos(dtClavesSP))
                            {
                                //Cargar Gridview
                                Controles.CargaGridView(gvClaveSP, dtClavesSP, "Id", lblOrden.Text, true, 1);
                                //Añade resultados a session
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtClavesSP, "Table");
                            }
                            else
                            {
                                //Cargar gridview
                                Controles.InicializaGridview(gvClaveSP);
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