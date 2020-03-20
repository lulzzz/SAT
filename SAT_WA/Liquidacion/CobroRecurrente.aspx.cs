using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Web.UI;
namespace SAT.Liquidacion
{
    public partial class CobroRecurrente : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Provocarse un PostBack en la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
                
                //Inicializando Página
                inicializaPagina();

            //Declarando Script de Carga
            string script = @"<script type='text/javascript'>
                                //Obteniendo Entidad Seleccionada
                                var entidad = " + ddlTipoEntApl.SelectedValue + @";
                                
                                //Validando la Entidad Seleccionada
                                if(entidad == 1)
                                {   //Cargando Lista de Unidades
                                    $('#" + this.txtEntidad .ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"'});
                                }
                                else if(entidad == 2)
                                {   //Cargando Lista de Operadores
                                    $('#" + this.txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"'});
                                }
                                else if(entidad == 3)
                                {   //Cargando Lista de Proveedores
                                    $('#" + this.txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=16&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"'});
                                }
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaEntidadAplicacion", script, false);
        }
        /// <summary>
        /// Evento Producido al Seleccionarse un Elemento del Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;
            
            //De acuerdo al nombre de comando asignado 
            switch (boton.CommandName)
            {   
                //Establecemos la pagina en estatus Nuevo
                case "Nuevo":
                    {   
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializando Id de Registro activo
                        Session["id_registro"] = 0;
                        //Invocando Método de Inicialización
                        inicializaPagina();
                        break;
                    }
                //Permite abrir registros de la Recepcion de factura
                case "Abrir":
                    {   
                        //Inicializando Apertura
                        inicializaAperturaRegistro(77, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                //Guarda el registro en la BD
                case "Guardar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Invocando Método de Guardado
                        result = guardaCobroRecurrente();

                        //Mostrando Resultado de Operación
                        ScriptServer.MuestraNotificacion(lkbGuardar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                //Envia al usuario a la pagina principal de la aplicación
                case "Salir":
                    {   
                        //Regresando a Página Anterior
                        PilaNavegacionPaginas.DireccionaPaginaAnterior();
                        break;
                    }
                //Permite al usuario editar el registro actual
                case "Editar":
                    {   
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Inicializamos la pagina
                        inicializaPagina();
                        break;
                    }
                //Deshabilita un registro de la Recepcion de factura
                case "Eliminar":
                    {   
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();
                        //Instanciando registro actual
                        using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Si la Recepcion existe
                            if (cr.id_cobro_recurrente > 0)
                                //Deshabilitamos el registro
                                resultado = cr.DeshabilitaCobroRecurrente(((Usuario)Session["usuario"]).id_usuario);
                            
                            //Si se deshabilitó correctamente
                            if (resultado.OperacionExitosa)
                            {   
                                //Estableciendo estatus a nuevo registro
                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                //Inicializando Id de registro activo 
                                Session["id_registro"] = 0;
                                //Inicialziando la forma
                                inicializaPagina();
                            }
                            
                            //Mostrando resultado
                            ScriptServer.MuestraNotificacion(lkbTerminar, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Bitacora":
                    {   
                        //Inicializando Ventana de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "77", "Cobro Recurrente");
                        break;
                    }
                case "Referencias":
                    {   
                        //Inicializando Ventana de Referencia
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "77", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                //Si la elección es Terminar el Cobro Recurrente
                case "Terminar":
                    {
                        //Mostramos Ventana Modal
                        alternaVentanaModal("terminar", lkbTerminar);

                        //Limpiamos Etiqueta de Motivo
                        txtMotivo.Text = "";
                        break;
                    } 
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Invocando Método de Guardado
            result = guardaCobroRecurrente();

            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    //Asignando Estatus a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
            }
            
            //Invocando Método de Inicialización
            inicializaPagina();
        }

        /// <summary>
        /// Evento generado al Terminar un Cobro Recurrente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminar_Click(object sender, EventArgs e)
        {
            //Terminamos Cobro Recurrente
            terminaCobroRecurrente();
        }

        /// <summary>
        /// Evento generado al cerar la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarMotivo_Click(object sender, EventArgs e)
        {
            //Cerramos ventana
            alternaVentanaModal("terminar", lkbCerrarMotivo);
        }
        
            #region Eventos GridView "Cobro Recurrente Liquidación"

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void lnkExportarCRL_Click(object sender, EventArgs e)
            {
                //Exportando Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void ddlTamanoCRL_SelectedIndexChanged(object sender, EventArgs e)
            {
                //Cambiando Tamaño del GridView
                TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoCRL.SelectedValue));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void gvCobroRecurrenteLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
            {
                //Cambiando Expresion de Ordenamiento
                lblOrdenadoCRL.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void gvCobroRecurrenteLiquidacion_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
            {
                //Cambiando Indice de Página
                TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewSelectedIndex);
            }
            /// <summary>
            /// Evento activado al cambiar de página
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            protected void gvCobroRecurrenteLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                //Cambiar página
                Controles.CambiaIndicePaginaGridView(gvCobroRecurrenteLiquidacion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
            }
            #endregion
        
        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   
            //Cargando los Catalogos de la Forma
            cargaCatalogos();
            //Inicializando Cobro Recurrente
            TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
            //Habilitando Menu de la Forma
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {   
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoCobroRecurrente, 31, "-- Seleccione un Tipo de Cobro", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEntApl, "", 62);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusTermino, "", 87);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCRL, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbSalir.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbTerminar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbSalir.Enabled = true;
                        lkbEditar.Enabled = true;
                        lkbTerminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbSalir.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbTerminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {   
            //Validando el estatus de la Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        ddlTipoCobroRecurrente.Enabled =
                        txtTotalDeuda.Enabled = true;
                        txtTotalCobrado.Enabled = false;
                        txtMontoCobro.Enabled =
                        //txtDiasCobro.Enabled =
                        ddlTipoEntApl.Enabled =
                        txtEntidad.Enabled =
                        txtReferencia.Enabled =
                        txtFechaIni.Enabled =
                        ddlEstatusTermino.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        ddlTipoCobroRecurrente.Enabled =
                        txtTotalDeuda.Enabled =
                        txtTotalCobrado.Enabled =
                        txtMontoCobro.Enabled =
                        //txtDiasCobro.Enabled =
                        ddlTipoEntApl.Enabled =
                        txtEntidad.Enabled =
                        txtReferencia.Enabled =
                        txtFechaIni.Enabled =
                        ddlEstatusTermino.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        ddlTipoCobroRecurrente.Enabled =
                        txtTotalDeuda.Enabled =
                        txtTotalCobrado.Enabled =
                        ddlTipoEntApl.Enabled =
                        ddlEstatusTermino.Enabled =
                        txtEntidad.Enabled =
                        txtFechaIni.Enabled = false;
                        txtMontoCobro.Enabled =
                        //txtDiasCobro.Enabled =
                        txtReferencia.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   
            //Validando el estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Asignando Valores
                        lblId.Text = "Por Asignar";
                        txtTotalCobrado.Text = "0.00";
                        txtTotalDeuda.Text = 
                        
                        txtMontoCobro.Text =
                        txtSaldo.Text = 
                        //txtDiasCobro.Text = "0";
                        txtEntidad.Text = 
                        txtReferencia.Text = "";
                        txtFechaIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                        txtFechaUltCob.Text = "";

                        //Inicializando Cobro Recurrente
                        TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
                        
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    
                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Cobro Recurrente
                        using(SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista el Registro
                            if(cr.id_cobro_recurrente != 0)
                            {   
                                //Asignando Valores
                                lblId.Text = cr.id_cobro_recurrente.ToString();
                                txtTotalDeuda.Text = cr.total_deuda.ToString();
                                txtTotalCobrado.Text = cr.total_cobrado.ToString();
                                txtSaldo.Text = cr.saldo.ToString();
                                txtMontoCobro.Text = cr.monto_cobro.ToString();
                                //txtDiasCobro.Text = cr.dias_cobro.ToString();
                                ddlTipoEntApl.SelectedValue = cr.id_tipo_entidad_aplicacion.ToString();
                                ddlTipoCobroRecurrente.SelectedValue = cr.id_tipo_cobro_recurrente.ToString();
                                
                                //Validando si el tipo de entidad
                                if (cr.id_operador != 0)
                                {   
                                    /**** Operador ****/
                                    using (Operador op = new Operador(cr.id_operador))
                                    {   //Validando si existe 
                                        if(op.id_operador != 0)
                                            //Asignando Valor
                                            txtEntidad.Text = op.nombre + " ID:" + op.id_operador.ToString();
                                    }
                                }
                                else if (cr.id_proveedor_compania != 0)
                                {   
                                    /**** Proveedor ****/
                                    using (CompaniaEmisorReceptor pro = new CompaniaEmisorReceptor(cr.id_proveedor_compania))
                                    {    
                                        //Validando si existe 
                                        if (pro.id_compania_emisor_receptor != 0)
                                            //Asignando Valor
                                            txtEntidad.Text = pro.nombre + " ID:" + pro.id_compania_emisor_receptor.ToString();
                                    }
                                }
                                else if (cr.id_unidad != 0)
                                {   
                                    /**** Unidad ****/
                                    using (Unidad uni = new Unidad(cr.id_unidad))
                                    {   
                                        //Validando si existe
                                        if (uni.id_unidad != 0)
                                            //Asignando Valor
                                            txtEntidad.Text = uni.numero_unidad + " ID:" + uni.id_unidad.ToString();
                                    }
                                }
                                else if (cr.id_empleado != 0)
                                {   
                                    /**** Empleado ****/
                                    txtEntidad.Text = " ID:" + cr.id_empleado.ToString();
                                }

                                //Asignando los Valores
                                txtReferencia.Text = cr.referencia;
                                txtFechaIni.Text = cr.fecha_inicial.ToString("dd/MM/yyyy");
                                txtFechaUltCob.Text = cr.fecha_ultimo_cobro.ToString("dd/MM/yyyy") == "01/01/0001" ? "" : cr.fecha_ultimo_cobro.ToString("dd/MM/yyyy");
                                ddlEstatusTermino.SelectedValue = cr.id_estatus_termino.ToString();

                                //Invocando Método de Carga de Historial de Cobros
                                cargaCobrosRecurrentesTotales(cr.id_cobro_recurrente, cr.id_tipo_entidad_aplicacion, cr.id_unidad, cr.id_operador, cr.id_proveedor_compania, cr.id_compania_emisor);
                            }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar el Cobro Recurrente
        /// </summary>
        private RetornoOperacion guardaCobroRecurrente()
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Obteniendo Fecha
            DateTime fec_ini;
            DateTime.TryParse(txtFechaIni.Text, out fec_ini);
            
            //Obteniendo Valor de la Entidad
            int id_unidad = ddlTipoEntApl.SelectedValue == "1" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            int id_operador = ddlTipoEntApl.SelectedValue == "2" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            int id_proveedor = ddlTipoEntApl.SelectedValue == "3" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            int id_empleado = ddlTipoEntApl.SelectedValue == "4" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;

            //Validando que Exista un Tipo de Cobro
            if (ddlTipoCobroRecurrente.SelectedValue != "" && ddlTipoCobroRecurrente.SelectedValue != "0")
            {
                //Validando el Estatus de la Página
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        {
                            //Insertando Cobro Recurrente
                            result = SAT_CL.Liquidacion.CobroRecurrente.InsertaCobroRecurrente(Convert.ToInt32(ddlTipoCobroRecurrente.SelectedValue),
                                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                Convert.ToDecimal(txtTotalDeuda.Text), Convert.ToDecimal(txtTotalCobrado.Text),
                                                Convert.ToDecimal(txtMontoCobro.Text == "" ? "0" : txtMontoCobro.Text), 1,
                                                1, Convert.ToByte(ddlTipoEntApl.SelectedValue), id_unidad,
                                                id_operador, id_proveedor, id_empleado, txtReferencia.Text, fec_ini, fec_ini,
                                                Convert.ToByte(ddlEstatusTermino.SelectedValue), 0, 0, ((Usuario)Session["usuario"]).id_usuario);

                            break;
                        }
                    case Pagina.Estatus.Edicion:
                        {
                            //Instanciando Cobro Recurrente
                            using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista el Registro
                                if (cr.id_cobro_recurrente != 0)
                                {
                                    //Editando Cobro Recurrente
                                    result = cr.EditaCobroRecurrente(Convert.ToInt32(ddlTipoCobroRecurrente.SelectedValue),
                                                        cr.id_compania_emisor, Convert.ToDecimal(txtTotalDeuda.Text), Convert.ToDecimal(txtTotalCobrado.Text),
                                                        Convert.ToDecimal(txtMontoCobro.Text == "" ? "0" : txtMontoCobro.Text), 1,
                                                        cr.dias_cobro, Convert.ToByte(ddlTipoEntApl.SelectedValue), id_unidad,
                                                        id_operador, id_proveedor, id_empleado, txtReferencia.Text, fec_ini, cr.fecha_ultimo_cobro,
                                                        Convert.ToByte(ddlEstatusTermino.SelectedValue), cr.id_tabla, cr.id_registro, ((Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            break;
                        }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No Existe el Tipo de Cobro");
            
            //Validando Operación Exitosa
            if(result.OperacionExitosa)
            {   
                //Asignando Sessiones
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                
                //Inicializando Página
                inicializaPagina();
            }
            
            //Mostrando Mensaje
            return result;
        }

        /// <summary>
        /// Terminamos Cobro Recurrente
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion terminaCobroRecurrente()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciamos Cobro Recurrente
            using (SAT_CL.Liquidacion.CobroRecurrente objCobroRecurrente = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Cobro Recurrente
                if (objCobroRecurrente.id_cobro_recurrente > 0)
                {
                    //Modificamios Cobro Recurrente
                    result = objCobroRecurrente.ActualizaEstatusTerminoCobroRecurrente(SAT_CL.Liquidacion.CobroRecurrente.EstatusTermino.Terminado, ((Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if (result.OperacionExitosa)
                    {
                        //Insertando Referencia
                        result = SAT_CL.Global.Referencia.InsertaReferencia(objCobroRecurrente.id_cobro_recurrente, 77, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 77, "Motivo", 0, "General"),
                                    txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);

                        //Validamos Resultado
                        if (result.OperacionExitosa)
                        {
                            //Inicializamos Valores
                            inicializaValores();

                            //Cerramos Ventana Modal
                            alternaVentanaModal("terminar", btnTerminar);

                        }

                    }

                }
            }
            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(lkbGuardar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Devolvemos resultado
            return result;
        }


        /// <summary>
        /// Método Público encargado de Cargar Todos los Cobros Recurrentes que ha Tenido la Entidad
        /// </summary>
        /// <param name="id_cobro_recurrente">Referencia al Cobro Recurrente</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        private void cargaCobrosRecurrentesTotales(int id_cobro_recurrente, byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisor)
        {
            //Instanciando Cobros Recurrentes
            using (DataTable dtCobrosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneHistorialCobrosRecurrentes(id_cobro_recurrente, id_tipo_entidad, id_unidad, id_operador, id_proveedor, id_compania_emisor))
            {
                //Validando que existen los Cobros Recurrentes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosRecurrentes))
                {
                    //Cargando Cobro Recurrente
                    TSDK.ASP.Controles.CargaGridView(gvCobroRecurrenteLiquidacion, dtCobrosRecurrentes, "Id", "");

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCobrosRecurrentes, "Table");
                }
                else
                {
                    //Inicializando Cobro Recurrente
                    TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/CobroRecurrente.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Cobros Recurrentes", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/CobroRecurrente.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Cobros", configuracion, Page);
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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/CobroRecurrente.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana,System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "terminar":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionTerminoCobroMultipleModal", "ventanaconfirmacionTerminoCobroMultiple");
                    break;
            }
        }
        #endregion

    }
}