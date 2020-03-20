using SAT_CL.Global;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.General
{
    public partial class CompaniaEmisorReceptor : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se haya producido un PostBack
            if(!(Page.IsPostBack))                
                //Inicializando Forma
                inicializaForma();
        }
        /// <summary>
        /// Evento producido al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {   
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(25, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {   
                        //Invocando Método de Guardado
                        guardaCompania();
                        break;
                    }
                case "Editar":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Eliminar":
                    {   
                        //Instanciando Producto
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista un Producto
                            if (cer.id_compania_emisor_receptor != 0)
                            {   
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();
                                //Deshabilitando Producto
                                result = cer.DeshabilitaCompaniaEmisorRecepto(1);
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {   //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaForma();
                                }//Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "25", "Compania");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "25", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
                case "Archivos":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaArchivosRegistro(Session["id_registro"].ToString(), "25", "0");
                    break;
                case "HI":
                    {   
                        //Limpiando Sesión
                        Session["id_registro"] = 0;
                        Session["DS"] = null;
                        
                        //Declarando variable para armado de URL
                        string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/ControlEvidencia/HojaInstruccion.aspx");
                        
                        //Direccionamos a la pagina destino
                        Response.Redirect(urlDestino);
                        break;
                    }
                case "CuentasBanco":
                    //Validamos Exista Compañia
                if(Session["id_registro"].ToString() != "0")
                {
                    //Mostramos Modal
                    alternaVentanaModal("cuentaBancos", lkbCuentasBanco);

                    //Inicializamos Control
                    wucCuentaBancos.InicializaControl(25, Convert.ToInt32(Session["id_registro"]));


                }
                    break;
                case "ProveedorWS":
                    {
                        //Validando Estatus
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Edicion:
                            case Pagina.Estatus.Lectura:
                                {
                                    //Cerrando ventana de edición de vencimiento
                                    TSDK.ASP.ScriptServer.AlternarVentana((LinkButton)sender, "ProveedorWS", "contenedorVentanaProveedorWS", "ventanaProveedorWS");
                                    //Inicializamos Control de Historial de Lecturas
                                    wucProveedorGPSDiccionario.InicializaControl(25, Convert.ToInt32(Session["id_registro"]));
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Guardar
            guardaCompania();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Validando el Estatus de la Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                        //Cambiando Session a Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
            }
            
            //Inicializando Forma
            inicializaForma();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkOpcionesUso_Click(object sender, EventArgs e)
        {
            //Validando Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Mostrando Mensaje
                        ScriptServer.MuestraNotificacion(lnkOpcionesUso, new RetornoOperacion("Debe de Existir un Cliente"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Validando que exista un Regimen
                        if (!ddlRegimenFiscal.SelectedValue.Equals("0"))
                        {
                            //Instanciando Compania
                            using (SAT_CL.Global.CompaniaEmisorReceptor cu = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {
                                //Validando Compania y Regimen actualizado
                                if (cu.habilitar)
                                {
                                    //Cargando los Tipos Disponibles
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 107, "Ninguno", cu.id_regimen_fiscal, "", 0, "");

                                    //Cargando Usos
                                    cargaUsosClienteCFDI();

                                    //Mostrando Ventana
                                    alternaVentanaModal("UsosClienteCFDI", lnkOpcionesUso);
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(lnkOpcionesUso, new RetornoOperacion("Debe de Actualizar el Regimen Fiscal"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        else
                            //Mostrando Mensaje
                            ScriptServer.MuestraNotificacion(lnkOpcionesUso, new RetornoOperacion("Debe de Existir un Regimen Fiscal"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
            }
        }

        #region Eventos Usos Cliente

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarUsoCDFI_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion("Debe Seleccionar un Uso de CFDI Valido!");
            
            //Validando que exista un Uso
            if (!ddlUsoCFDI.SelectedValue.Equals("0"))
            {
                //Validando que exista un Registro Seleccionado
                if (gvUsosCliente.SelectedIndex != -1)
                {
                    //Inicializando Transacción
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Obteniendo Uso
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                        using (SAT_CL.FacturacionElectronica33.UsoCliente uso = new SAT_CL.FacturacionElectronica33.UsoCliente(Convert.ToInt32(gvUsosCliente.SelectedDataKey["Id"])))
                        {
                            //Validando Existencia
                            if (uso.habilitar && cer.habilitar)
                            {
                                //Editando Uso del CFDI
                                result = uso.EditaUsoCliente(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlUsoCFDI.SelectedValue), uso.secuencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando Excepción
                                if (result.OperacionExitosa)
                                {
                                    //Si el uso actual era el que se estaba Actualizando
                                    if (uso.id_uso == cer.id_uso_cfdi)
                                    {
                                        //Actualizando el Uso del CFDI
                                        result = cer.EditaCompaniaEmisorRecepto(txtIdAlterno.Text.ToUpper(), txtRFC.Text.ToUpper(), txtNombre.Text.ToUpper(), txtNombreCorto.Text.ToUpper(),
                                                      Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)), chkBitEmisor.Checked, chkBitReceptor.Checked,
                                                      chkBitProveedor.Checked, Convert.ToInt32(ddlTipoServicio.SelectedValue), txtContacto.Text.ToUpper(), txtCorreo.Text.ToLower(),
                                                      txtTelefono.Text.ToUpper(), Convert.ToDecimal(txtLimiteCredito.Text), Convert.ToInt32(txtDiasCredito.Text),
                                                      Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaUso.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaAgrup.Text, "ID:", 1)),
                                                      txtInfoAd1.Text.ToUpper(), txtInfoAd2.Text.ToUpper(), Convert.ToInt32(ddlRegimenFiscal.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue),
                                                      ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede recuperar el Uso del Cliente");

                            //Validando Excepción
                            if (result.OperacionExitosa)
                            
                                //Completando Transacción
                                scope.Complete();
                        }
                    }
                }
                else
                    //Insertando Uso del CFDI
                    result = SAT_CL.FacturacionElectronica33.UsoCliente.InsertaUsoCliente(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlUsoCFDI.SelectedValue), 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Inicializando indices
                Controles.InicializaIndices(gvUsosCliente);

                //Cargando Uso
                cargaUsosClienteCFDI();
                
                //Inicializando Forma
                inicializaForma();
            }
            
            //Devolviendo Resultado Obtenido
            ScriptServer.MuestraNotificacion(btnGuardarUsoCDFI, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCFDI_Click(object sender, EventArgs e)
        {
            //Instanciando Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor cu = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Cargando los Tipos Disponibles
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 107, "Ninguno", cu.id_regimen_fiscal, "", 0, "");

                //inicializando Indices
                Controles.InicializaIndices(gvUsosCliente);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarUsoCFDI_Click(object sender, EventArgs e)
        {
            //Validando Registros
            if (gvUsosCliente.DataKeys.Count > 0)
            {
                //Seleccionando Regitro
                Controles.SeleccionaFila(gvUsosCliente, sender, "lnk", false);

                //Instanciando Registros
                using (SAT_CL.Global.CompaniaEmisorReceptor cu = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                using (SAT_CL.FacturacionElectronica33.UsoCliente uso = new SAT_CL.FacturacionElectronica33.UsoCliente(Convert.ToInt32(gvUsosCliente.SelectedDataKey["Id"])))
                {
                    //Validando Existencia
                    if (cu.habilitar)
                    {
                        //Validando Existencia
                        if (uso.habilitar)
                        {
                            //Cargando los Tipos Disponibles
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 107, "Ninguno", cu.id_regimen_fiscal, "", 0, "");
                            //Asignando Valor
                            ddlUsoCFDI.SelectedValue = uso.id_uso.ToString();
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Uso del CFDI de este Cliente"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Cliente"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarUsoCFDI_Click(object sender, EventArgs e)
        {
            //Validando Registros
            if (gvUsosCliente.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Regitro
                Controles.SeleccionaFila(gvUsosCliente, sender, "lnk", false);

                //Instanciando Registros
                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                using (SAT_CL.FacturacionElectronica33.UsoCliente uso = new SAT_CL.FacturacionElectronica33.UsoCliente(Convert.ToInt32(gvUsosCliente.SelectedDataKey["Id"])))
                {
                    //Validando Existencia
                    if (cer.habilitar)
                    {
                        //Validando Existencia
                        if (uso.habilitar)
                        {
                            //Deshabilitando Uso del Cliente
                            result = uso.DeshabilitaUsoCliente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede recuperar el Uso del CFDI de este Cliente");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede recuperar el Cliente");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Inicializando Indices
                    Controles.InicializaIndices(gvUsosCliente);
                    
                    //Cargando Usos del Cliente
                    cargaUsosClienteCFDI();
                }

                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Dirección"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVentana_Click(object sender, EventArgs e)
        {   
            //Habilitando el Control
            ucDireccion.Enable = true;
            
            //Validando que exista un registro previo
            if (Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) != 0)
                
                //Inicializando Control con el Registro Previo
                ucDireccion.InicializaControl(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)));
            else
                //Inicializando Control por Defecto
                ucDireccion.InicializaControl();
            //Abre ventana modal direccion
            ScriptServer.AlternarVentana(lnkVentana, lnkVentana.GetType(), "AbrirVentana", "contenedorDireccionModal", "DireccionModal");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Servicio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarServicio_Click(object sender, EventArgs e)
        {
            //Cargando el Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicioAsignacion, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            
            //Cargando Asignaciones de Proveedor
            cargaAsignacionesProveedor();
            
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkAgregarServicio, uplnkAgregarServicio.GetType(), "Ventana Tipo Servicio", "contenidoVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// Evento Producido al Marcar y/o Desmarcar el "Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBitProveedor_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Función de Configuración
            configuraTipoServicioProveedor(chkBitProveedor.Checked);
        }

        /// <summary>
        /// Evento generado al dar click en Sucursal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSucursal_Click(object sender, EventArgs e)
        {
            //Validamos Existencia de comprobante
            if (Convert.ToInt32(Session["id_registro"]) != 0)
            {
                //Abriendo ventana de detalle
                 string ruta_ventana = string.Format("Sucursal.aspx?id_compania={0}", Session["id_registro"]);

                ScriptServer.AbreNuevaVentana(ruta_ventana, "Sucursal", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=950,height=990", Page);
            }
        }

        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Cuentas":
                    //Mostramos Modal
                    alternaVentanaModal("cuentaBancos", lkb);
                    break;
                default:
                    //Mostramos Modal
                    alternaVentanaModal(lkb.CommandName, lkb);
                    break;
            }            
            
        }

        /// <summary>
        /// Evento Producido al Cerrar la Ventana del Proveedor de WS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarProveedorWS_Click(object sender, EventArgs e)
        {
            //Cerrando ventana de edición de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarProveedorWS, "ProveedorWS", "contenedorVentanaProveedorWS", "ventanaProveedorWS");
        }
        #region Eventos UserControl "Dirección"

        /// <summary>
        /// Evento Producido al Guardar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickGuardarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo resultado de la Operación
            result = ucDireccion.GuardaDireccion();
            //Instanciando Direccion
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(result.IdRegistro))
            {   //Validando que exista
                if (dir.id_direccion != 0)
                {   //Mostrando Descripcion
                    txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickEliminarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Eliminando Dirección
            result = ucDireccion.EliminaDireccion();
            //Validando si la Operación fue Exitosa
            if (result.OperacionExitosa)
            {   //Validando si el registro eliminado es igual al Seleccionado
                if (Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) == result.IdRegistro)
                    //Limpiando Control
                    txtDireccion.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickSeleccionarDireccion(object sender, EventArgs e)
        {   //Instanciando Dirección
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(ucDireccion.SeleccionaDireccion()))
            {   //Validando que exista una Dirección
                if (dir.id_direccion != 0)
                {   //Mostrando Descripción
                    txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = "";
            }
        }

        #endregion

        #region Eventos Modal ""

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarServicio_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
                
            //Validando que la Asignación no este en "0"
            if (ddlTipoServicioAsignacion.SelectedValue != "0")
            {
                //Validando que existe un Registro Seleccionado
                if (gvServiciosAgregados.SelectedIndex != -1)
                {
                    //Instanciando la Asignación del Tipo de Servicio
                    using (SAT_CL.Global.AsignacionTipoServicio ats = new AsignacionTipoServicio(Convert.ToInt32(gvServiciosAgregados.SelectedDataKey["Id"])))
                    {
                        //Validando que existe
                        if (ats.id_asignacion_tipo_servicio > 0)

                            //Editando la Asignación
                            result = ats.EditarAsignacionTipoServicio(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlTipoServicioAsignacion.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                {
                    //Insertando la Asignación
                    result = SAT_CL.Global.AsignacionTipoServicio.InsertarAsignacionTipoServicio(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(ddlTipoServicioAsignacion.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe Existir una Asignación");

            //Validando que las Operaciones fuesen exitosas
            if (result.OperacionExitosa)
            {
                //Cargando Asignaciones de Proveedor
                cargaAsignacionesProveedor();

                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicioAsignacion, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                //Cargando Tipo de Servicio de Asignación
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 46, "-- Seleccione una Opción", Convert.ToInt32(Session["id_registro"]), "", 0, "");

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvServiciosAgregados);
            }

            //Mostrando Mensaje de la Operación
            lblErrorServicio.Text = result.Mensaje;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarServicio_Click(object sender, EventArgs e)
        {
            //Validando que exista una Fila seleccionada
            if(gvServiciosAgregados.SelectedIndex != -1)
            {
                //Cargando Catalogo
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicioAsignacion, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvServiciosAgregados);
            }
            else
            {
                //Invocando Configuración
                configuraTipoServicioProveedor(chkBitProveedor.Checked);
                
                //Alternando Ventana
                TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarServicio, upbtnCancelarServicio.GetType(), "Ventana Tipo Servicio", "contenidoVentanaConfirmacion", "ventanaConfirmacion");
            }
        }

        /// <summary>
        /// Evento que cierra la ventana modal de direccón 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagenDireccion_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrarImagenDireccion, lnkCerrarImagenDireccion.GetType(), "CerrarVentana", "contenedorDireccionModal", "DireccionModal");
            inicializaForma();

        }
        #region Eventos GridView "Tipos Servicios"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServiciosAgregados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosAgregados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvServiciosAgregados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosAgregados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvServiciosAgregados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if(gvServiciosAgregados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvServiciosAgregados, sender, "lnk", false);

                //Instanciando Asignación
                using(AsignacionTipoServicio ats = new AsignacionTipoServicio(Convert.ToInt32(gvServiciosAgregados.SelectedDataKey["Id"])))
                {
                    //Validando que exista una Asignación
                    if(ats.id_asignacion_tipo_servicio > 0)
                    
                        //Asignando Valor
                        ddlTipoServicioAsignacion.SelectedValue = ats.id_tipo_servicio.ToString();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvServiciosAgregados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvServiciosAgregados, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Asignación
                using (AsignacionTipoServicio ats = new AsignacionTipoServicio(Convert.ToInt32(gvServiciosAgregados.SelectedDataKey["Id"])))
                {
                    //Validando que exista una Asignación
                    if (ats.id_asignacion_tipo_servicio > 0)

                        //Deshabilitando Asignación
                        result = ats.DeshabilitarAsignacionTipoServicio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validando que la Deshabilitación fuese Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Cargando Asignaciones
                        cargaAsignacionesProveedor();

                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicioAsignacion, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                        //Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvServiciosAgregados);
                    }

                    //Mostrando Mensaje de Operación
                    lblErrorServicio.Text = result.Mensaje;
                }
            }
        }

        #endregion

        #endregion

        #region GridView Facturas
        /// <summary>
        /// Evento que cambia el tamaño de registros del GridView actividades ligadas a una orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacturasCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasCliente, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacturasCliente.SelectedValue), true, 0);
        }
        /// <summary>
        /// Evento que exporta a un archivo de excel los registros del GridView actividades ligadas a una orden de trabajo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacturasCliente_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// Evento que permite al cambio de paginación del GridView del GridView actividades ligadas a una orden de trabajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasCliente, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento que regula el ordenamiento de registros en base a la columna seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacturasCliente.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasCliente, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }
        /// <summary>
        /// Evento disparado al presionar el LinkButton "Bitacora" o "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDetalles_Click(object sender, EventArgs e)
        {   //Evaluando que el GridView tenga registros
            if (gvFacturasCliente.DataKeys.Count > 0)
            {   //Referenciando al botón pulsado
                using (LinkButton boton = (LinkButton)sender)
                {   //Seleccionando la fila actual
                    Controles.SeleccionaFila(gvFacturasCliente, sender, "lnk", false);
                    //Evaluando Boton Presionado
                    switch (boton.CommandName)
                    {
                        case "XML":
                            {
                                //Instanciamos Comprobante
                                using (SAT_CL.FacturacionElectronica.Comprobante c = new SAT_CL.FacturacionElectronica.Comprobante(Convert.ToInt32(gvFacturasCliente.SelectedValue)))
                                {
                                    //Si existe y está generado
                                    if (c.generado)
                                    {
                                        //Obteniendo bytes del archivo XML
                                        byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);

                                        //Realizando descarga de archivo
                                        if (cfdi_xml.Length > 0)
                                        {
                                            //Instanciando al emisor
                                            using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                                                TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                                        }
                                    }
                                }
                                break;
                            }
                        case "PDF":
                            {
                                //Obteniendo Ruta
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciamos Comprobante
                                using (SAT_CL.FacturacionElectronica.Comprobante objComprobante = new SAT_CL.FacturacionElectronica.Comprobante(Convert.ToInt32(gvFacturasCliente.SelectedValue)))
                                {
                                    //Validamos que el comprobante se encuentre Timbrado
                                    if (objComprobante.generado)
                                    {
                                        //Instanciando nueva ventana de navegador para apertura de registro
                                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Comprobante", objComprobante.id_comprobante), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }

        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {   
            //Validando que exista un Estatus
            if (Session["estatus"] == null)                
                //Asignando a Estatus "Nuevo"
                Session["estatus"] = Pagina.Estatus.Nuevo;            
            //Instanciando Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {   
                //Validando que exista la Compania
                if (cer.id_compania_emisor_receptor != 0)                    
                    //Asignando Valor
                    txtCompaniaUso.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
            }
            //Cargando Catalogos
            cargaCatalogos();
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvServiciosAgregados);            
            //Invocando Métodos de Inicio de Página
            habilitaMenu();
            habilitarControles();
            inicializaValores();
            //Inicializando Control
            ucDireccion.InicializaControl();
            //Inicializa grid view facturas
            inicializaFacturasCliente();
            //Mostrando Enfoque al Primer Control
            txtNombre.Focus();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Carga catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Cargando Tipo de Servicio de Asignación
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 46, "-- Seleccione una Opción", Convert.ToInt32(Session["id_registro"]), "", 0, "");

                        break;
                    }
            }
            //Cargando Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);

            //Carga Catalogos CFDI3.3
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlRegimenFiscal, 106, "Ninguno");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCliente, 108, "Ninguno", Convert.ToInt32(Session["id_registro"]), "", 0, "");

            //Cargando Tipo de Servicio de Asignación
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicioAsignacion, 28, "-- Seleccione una Opción", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturasCliente, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = true;
                        //Edicion
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
                        btnGuardar.Enabled = false;
                        //Edicion
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
                        btnGuardar.Enabled = true;
                        //Edicion
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
        /// Método Prvado encargado de Habilitar los Controles
        /// </summary>
        private void habilitarControles()
        {   //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitando Controles
                        txtIdAlterno.Enabled =
                        txtRFC.Enabled =
                        txtNombre.Enabled =
                        txtNombreCorto.Enabled =
                        lnkVentana.Enabled =
                        chkBitEmisor.Enabled =
                        chkBitReceptor.Enabled =
                        chkBitProveedor.Enabled =
                        txtContacto.Enabled =
                        txtCorreo.Enabled =
                        txtTelefono.Enabled =
                        txtLimiteCredito.Enabled =
                        txtDiasCredito.Enabled =
                        txtCompaniaAgrup.Enabled =
                        txtInfoAd1.Enabled =
                        txtInfoAd2.Enabled = 
                        ddlRegimenFiscal.Enabled =
                        ddlUsoCliente.Enabled = true;
                        lnkOpcionesUso.Enabled =
                        ddlTipoServicio.Enabled =
                        lkbSucursal.Enabled =
                        lnkAgregarServicio.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   //Habilitando Controles
                        txtIdAlterno.Enabled =
                        txtRFC.Enabled =
                        txtNombre.Enabled =
                        txtNombreCorto.Enabled =
                        lnkVentana.Enabled = 
                        chkBitEmisor.Enabled =
                        chkBitReceptor.Enabled =
                        chkBitProveedor.Enabled =
                        ddlTipoServicio.Enabled =
                        txtContacto.Enabled =
                        txtCorreo.Enabled =
                        txtTelefono.Enabled =
                        txtLimiteCredito.Enabled =
                        txtDiasCredito.Enabled =
                        txtCompaniaAgrup.Enabled =
                        txtInfoAd1.Enabled =
                        txtInfoAd2.Enabled =
                        ddlRegimenFiscal.Enabled =
                        ddlUsoCliente.Enabled =
                        lnkOpcionesUso.Enabled =
                        lkbSucursal.Enabled =
                        lnkAgregarServicio.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   //Deshabilitando Controles
                        txtIdAlterno.Enabled =
                        txtRFC.Enabled =
                        txtNombre.Enabled =
                        txtNombreCorto.Enabled =
                        lnkVentana.Enabled = 
                        chkBitEmisor.Enabled =
                        chkBitReceptor.Enabled =
                        chkBitProveedor.Enabled =
                        ddlTipoServicio.Enabled =
                        txtContacto.Enabled =
                        txtCorreo.Enabled =
                        txtTelefono.Enabled =
                        txtLimiteCredito.Enabled =
                        txtDiasCredito.Enabled =
                        txtCompaniaAgrup.Enabled =
                        txtInfoAd1.Enabled =
                        txtInfoAd2.Enabled =
                        ddlRegimenFiscal.Enabled =
                        ddlUsoCliente.Enabled =
                        lnkOpcionesUso.Enabled =
                        lkbSucursal.Enabled =
                        lnkAgregarServicio.Enabled = false;
                        break;
                    }
            }
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
                        lblId.Text = "Por Asignar";
                        txtIdAlterno.Text =
                        txtRFC.Text =
                        txtNombre.Text =
                        txtNombreCorto.Text = 
                        txtDireccion.Text = 
                        txtContacto.Text =
                        txtCorreo.Text =
                        txtTelefono.Text =
                        txtLimiteCredito.Text =
                        txtDiasCredito.Text =
                        txtInfoAd1.Text =
                        txtInfoAd2.Text = "";
                        chkBitEmisor.Checked =
                        chkBitReceptor.Checked =
                        chkBitProveedor.Checked = false;
                        ddlTipoServicio.SelectedValue = "0";
                        txtDireccion.Text = "";

                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {   //Instanciando la Compania
                        using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                        {   //Asignando Valores
                            lblId.Text = cer.id_compania_emisor_receptor.ToString();
                            txtIdAlterno.Text = cer.id_alterno;
                            txtRFC.Text = cer.rfc;
                            txtNombre.Text = cer.nombre;
                            txtNombreCorto.Text = cer.nombre_corto;
                            txtContacto.Text = cer.contacto;
                            txtCorreo.Text = cer.correo;
                            txtTelefono.Text = cer.telefono;
                            txtLimiteCredito.Text = cer.limite_credito.ToString();
                            txtDiasCredito.Text = cer.dias_credito.ToString();
                            txtInfoAd1.Text = cer.informacion_adicional1;
                            txtInfoAd2.Text = cer.informacion_adicional2;
                            chkBitEmisor.Checked = cer.bit_emisor;
                            chkBitReceptor.Checked = cer.bit_receptor;
                            chkBitProveedor.Checked = cer.bit_proveedor;

                            //Cargando Tipo de Servicio de Asignación
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 46, "-- Seleccione una Opción", cer.id_compania_emisor_receptor, "", 0, "");

                            //Invocando Configuración
                            configuraTipoServicioProveedor(cer.bit_proveedor);
                            ddlTipoServicio.SelectedValue = cer.id_tipo_servicio.ToString();
                            ddlRegimenFiscal.SelectedValue = cer.id_regimen_fiscal.ToString();
                            ddlUsoCliente.SelectedValue = cer.id_uso_cfdi.ToString();

                            //Instanciando Compania Agrupadora
                            using (SAT_CL.Global.CompaniaEmisorReceptor cu = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                            {   
                                //Validando que exista la Compania Agrupadora
                                if (cu.habilitar)
                                    //Cargando los Tipos Disponibles
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 107, "Ninguno", cu.id_regimen_fiscal, "", 0, "");
                                else
                                    //Cargando los Tipos Disponibles
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 107, "Ninguno", 0, "", 0, "");
                            }

                            //Instanciando Compania Agrupadora
                            using (SAT_CL.Global.CompaniaEmisorReceptor ca = new SAT_CL.Global.CompaniaEmisorReceptor(cer.id_compania_agrupador))
                            {   //Validando que exista la Compania Agrupadora
                                if (ca.id_compania_emisor_receptor != 0)
                                    //Concatenando Texto al Control
                                    txtCompaniaAgrup.Text = ca.nombre + " ID:" + ca.id_compania_agrupador.ToString();
                                else//Limpiando Control
                                    txtCompaniaAgrup.Text = "";
                            }
                            //Instanciando Direccion
                            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(cer.id_direccion))
                            {   //Validando que exista la Direccion
                                if (dir.id_direccion != 0)
                                    //Asignando Valor
                                    txtDireccion.Text = dir.calle + " ID:" + dir.id_direccion.ToString();
                                else//Limpiando Control
                                    txtDireccion.Text = "";
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Cambios en la Compania
        /// </summary>
        private void guardaCompania()
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando Estatus de Session
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Declarando Variable Auxiliar
                            int idCompania = 0;
                            
                            //Realizando Inserción
                            result = SAT_CL.Global.CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto( txtIdAlterno.Text.ToUpper(), 
                                                                                                        txtRFC.Text.ToUpper(), 
                                                                                                        txtNombre.Text.ToUpper(), 
                                                                                                        txtNombreCorto.Text.ToUpper(),
                                                                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)), 
                                                                                                        chkBitEmisor.Checked, 
                                                                                                        chkBitReceptor.Checked,
                                                                                                        chkBitProveedor.Checked, 
                                                                                                        Convert.ToInt32(ddlTipoServicio.SelectedValue), 
                                                                                                        Cadena.VerificaCadenaVacia(txtContacto.Text.ToUpper(), "S/C"), 
                                                                                                        Cadena.VerificaCadenaVacia(txtCorreo.Text.ToLower(), ""), 
                                                                                                        Cadena.VerificaCadenaVacia(txtTelefono.Text.ToUpper(), "S/N"),
                                                                                                        Convert.ToDecimal(Cadena.VerificaCadenaVacia(txtLimiteCredito.Text, "0.0")), 
                                                                                                        Convert.ToInt32(Cadena.VerificaCadenaVacia(txtDiasCredito.Text, "0")), 
                                                                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaUso.Text, "ID:", 1)),
                                                                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaAgrup.Text, "ID:", 1)), 
                                                                                                        txtInfoAd1.Text.ToUpper(), 
                                                                                                        txtInfoAd2.Text.ToUpper(),
                                                                                                        Convert.ToInt32(ddlRegimenFiscal.SelectedValue),
                                                                                                        Convert.ToInt32(ddlUsoCliente.SelectedValue), 
                                                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario
                                                                                                        );
                            //Validando que la Operación fuese Exitosa
                            if(result.OperacionExitosa)
                            {
                                //Guardando Id de Compania
                                idCompania = result.IdRegistro;

                                //Validando que Exista un Tipo de Servicio Valido
                                if (Convert.ToInt32(ddlTipoServicio.SelectedValue) > 0 && chkBitProveedor.Checked)
                                {
                                    //Insertando Asignación de Tipo de Servicio
                                    result = SAT_CL.Global.AsignacionTipoServicio.InsertarAsignacionTipoServicio(idCompania, Convert.ToInt32(ddlTipoServicio.SelectedValue),
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    //Instanciando Compania
                                    result = new RetornoOperacion(idCompania);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando Compania
                                    result = new RetornoOperacion(idCompania);

                                    //Completando Transacción
                                    trans.Complete();
                                }
                            }

                        }
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Declarando Variable Auxiliar
                            int idCompania = 0;
                            
                            //Instanciando Compania
                            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que sea un Registro Valido
                                if (cer.id_compania_emisor_receptor != 0)
                                    //Realizando Edición
                                    result = cer.EditaCompaniaEmisorRecepto(txtIdAlterno.Text.ToUpper(), txtRFC.Text.ToUpper(), txtNombre.Text.ToUpper(), txtNombreCorto.Text.ToUpper(),
                                                      Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)), chkBitEmisor.Checked, chkBitReceptor.Checked,
                                                      chkBitProveedor.Checked, Convert.ToInt32(ddlTipoServicio.SelectedValue), txtContacto.Text.ToUpper(), txtCorreo.Text.ToLower(),
                                                      txtTelefono.Text.ToUpper(), Convert.ToDecimal(txtLimiteCredito.Text), Convert.ToInt32(txtDiasCredito.Text),
                                                      Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaUso.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaAgrup.Text, "ID:", 1)),
                                                      txtInfoAd1.Text.ToUpper(), txtInfoAd2.Text.ToUpper(), Convert.ToInt32(ddlRegimenFiscal.SelectedValue), Convert.ToInt32(ddlUsoCliente.SelectedValue), 
                                                      ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Guardando Id de Compania
                                    idCompania = result.IdRegistro;

                                    //Validando que Exista un Tipo de Servicio Valido
                                    if (Convert.ToInt32(ddlTipoServicio.SelectedValue) > 0 && chkBitProveedor.Checked)
                                    {
                                        //Insertando Asignación de Tipo de Servicio
                                        result = SAT_CL.Global.AsignacionTipoServicio.InsertarAsignacionTipoServicio(idCompania, Convert.ToInt32(ddlTipoServicio.SelectedValue),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que ya Exista el Tipo de Servicio
                                        if (result.IdRegistro == -1)

                                            //Instanciando Compania
                                            result = new RetornoOperacion(idCompania);
                                    }
                                    else
                                        //Instanciando Compania
                                        result = new RetornoOperacion(idCompania);

                                    //Validando que la Operación fuese Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Compania
                                        result = new RetornoOperacion(idCompania);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            //Validando que la Operación haya sido exitosa
            if(result.OperacionExitosa)
            {   //Asignando Variables de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Inicializando Forma
                inicializaForma();
            }
            //Mostrando Mensaje de Error
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "cuentaBancos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaAltaCuentas", "ventanaAltaCuentas");
                    break;
                case "UsosClienteCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "ventanaContenedorUsoCFDI", "contenedorUsoCFDI");
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/CompaniaEmisorReceptor.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método encargado de Cargar las Asignaciones del Proveedor
        /// </summary>
        private void cargaAsignacionesProveedor()
        {
            //Obteniendo las Asignaciones en base al Proveedor
            using(DataTable dtAsignaciones = SAT_CL.Global.AsignacionTipoServicio.ObtieneAsignacionesPorProveedor(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existen las Asignaciones
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtAsignaciones))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvServiciosAgregados, dtAsignaciones, "Id", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAsignaciones, "Table");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvServiciosAgregados);

                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar los Usos del Cliente
        /// </summary>
        private void cargaUsosClienteCFDI()
        {
            //Validando Estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Grid
                        Controles.InicializaGridview(gvUsosCliente);

                        //Eliminando de Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"],  "Table2");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Obteniendo Usos
                        using (DataTable dtUsos = SAT_CL.FacturacionElectronica33.UsoCliente.ObtieneUsosClienteCFDI(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando Registros
                            if (Validacion.ValidaOrigenDatos(dtUsos))
                            {
                                //Inicializando Grid
                                gvUsosCliente.PageSize = dtUsos.Rows.Count;
                                Controles.CargaGridView(gvUsosCliente, dtUsos, "Id", "");

                                //Añadiendo a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUsos, "Table2");
                            }
                            else
                            {
                                //Inicializando Grid
                                Controles.InicializaGridview(gvUsosCliente);

                                //Eliminando de Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                            }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Configurar el Tipo de Servicio
        /// </summary>
        /// <param name="proveedor_checked">Indicador del Proveedor</param>
        private void configuraTipoServicioProveedor(bool proveedor_checked)
        {
            //Validando Estatus
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //Deshabilitando Control
                    lnkAgregarServicio.Enabled = false;
                    ddlTipoServicio.Enabled = proveedor_checked;
                    break;
                case Pagina.Estatus.Lectura:
                    //Configurando Control según el Indicador del Proveedor
                    lnkAgregarServicio.Enabled = 
                    ddlTipoServicio.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    //Configurando Control según el Indicador del Proveedor
                    lnkAgregarServicio.Enabled = proveedor_checked;
                    ddlTipoServicio.Enabled = proveedor_checked;
                    break;
            }

            //Validando que el Proveedor este Desmarcado
            if (!proveedor_checked)

                //Asignando Valor por Defecto
                ddlTipoServicio.SelectedValue = "0";
        }
        /// <summary>
        /// Método encargado de inicializar el gridview facturas acorde al estado de la página.
        /// </summary>
        private void inicializaFacturasCliente()
        {
            //Valida el estado de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estado de la página es nuevo
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializa gridview e indices
                        Controles.InicializaGridview(gvFacturasCliente);
                        Controles.InicializaIndices(gvFacturasCliente);
                        break;
                    }
                //Si la pagina se encentra en estatus de edicion o lectura
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Lectura:
                    {
                        //Instancia a la clase compañia
                        using (DataTable dtFactura = SAT_CL.FacturacionElectronica.Reporte.UltimasFacturasCliente((int)Session["id_registro"]))
                        {
                            //Valida que existan datos en el dataset
                            if (Validacion.ValidaOrigenDatos(dtFactura))
                            {
                                //Asigna registros al gridview
                                Controles.CargaGridView(gvFacturasCliente, dtFactura, "Id", "");
                                //Asigna valores a la variable de session DS
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFactura, "Table1");
                            }
                            //En caso contrario 
                            else
                            {
                                //Inicializa gridView
                                Controles.InicializaGridview(gvFacturasCliente);
                                //Elimina los datos de session del dataset
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                            }
                            //Inicializa los indices del GridView
                            Controles.InicializaIndices(gvFacturasCliente);
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}