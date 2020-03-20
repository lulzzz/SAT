using System;
using TSDK.Base;
using SAT_CL.EgresoServicio;
using SAT_CL.Seguridad;
using TSDK.ASP;
using System.Transactions;
using TSDK.Datos;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using SAT_CL.FacturacionElectronica;
using SAT_CL.CXP;
using System.Web;

namespace SAT.UserControls
{
    public partial class wucAnticipoProgramado : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Deposito
        /// </summary>
        private int _id_deposito;
        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _id_unidad;
        /// <summary>
        /// Id Operador
        /// </summary>
        private int _id_operador;
        /// <summary>
        /// Id proveedor compañia
        /// </summary>
        private int _id_proveedor_compania;
        /// <summary>
        /// Id servicio
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id movimiento
        /// </summary>
        private int _id_movimiento;
        /// <summary>
        /// Id movimiento Asignacion
        /// </summary>
        private int _id_movimiento_asignacion;
        /// <summary>
        /// Id Cliente Receptor
        /// </summary>
        private int _id_cliente_receptor;
        /// <summary>
        /// Tabla de Facturas Ligadas
        /// </summary>
        private DataTable _dtFacturasLigadas;
        /// <summary>
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickRegistrar;
        /// <summary>
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickSolicitar;
        /// <summary>
        /// Declaración de Evento ClickCancelar
        /// </summary>
        public event EventHandler ClickCancelar;
        /// <summary>
        /// Declaración de Evento ClickEliminar
        /// </summary>
        public event EventHandler ClickEliminar;


        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Tab
                ddlConcepto.TabIndex =
                txtMonto.TabIndex =
                chkDepositoExtraEfectivo.TabIndex =
                ddlTipoCargo.TabIndex =
                txtReferencia.TabIndex =
                btnEliminar.TabIndex =
                btnRegistrar.TabIndex =
                btnCancelar.TabIndex = value;
                //Ventana Modal
                //lnkCerrarVentanaFL.TabIndex =
                //btnAgregarFactura.TabIndex = value;
            }
            get { return ddlConcepto.TabIndex; }
        }
        /// <summary>
        /// Obtiene o establece el valor que indica sólo debe cargarse el concepto 'Anticipo Proveedor'
        /// </summary>
        public bool SoloAnticipoProveedor;
        public bool HabilitaConcepto;
        public bool MuestraSolicitar;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                //Carga catalogos
                cargaCatalogos();
            }
            else
            {
                //Si es PostaBack
                //Recupera Atributos
                recuperaAtributos();
            }

            //Inicializamos Validador
            inicializaValidador(this._id_proveedor_compania);

            //Invocando Script
            inicializaScriptFacturaXML();
        }

        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
            string url = Cadena.RutaRelativaAAbsoluta("~/UserControls/wucDeposito.ascx", "~/Accesorios/ServicioFacturas.aspx?idRegistro=51&idRegistroB=" + this._id_deposito.ToString() + "&idRegistroC=" + this._id_servicio.ToString());
            //Define las dimensiones de la ventana Abrir registros de Usuario
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=400";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Usuario", configuracion, Page);
        }
        /// <summary>
        /// Evento Producido al Presionar el Link "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAgregarFactura_Click(object sender, EventArgs e)
        {
            //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
            string url = Cadena.RutaRelativaAAbsoluta("~/UserControls/wucDeposito.ascx", "~/Accesorios/ServicioFacturas.aspx?idRegistro=51&idRegistroB=" + this._id_deposito.ToString() + "&idRegistroC=" + this._id_servicio.ToString());
            //Define las dimensiones de la ventana Abrir registros de Usuario
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=400";
            //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
            ScriptServer.AbreNuevaVentana(url, "Abrir Registro Usuario", configuracion, Page);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Concepto del Deposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControlFactura();
        }

        #region Eventos "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaFL_Click(object sender, EventArgs e)
        {
            //Validando que Exista una Master Page
            if (this.Page.Master == null)

                //Mostrando Ventana para Guardado
                ScriptServer.AlternarVentana(this.Page, "FacturasXML", "contenedorVentanaFacturaAnticipo", "ventanaFacturaAnticipo");
        }

        #endregion

        /// <summary>
        /// Manipula Evento ClickRegistrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickRegistrar(EventArgs e)
        {
            if (ClickRegistrar != null)
                ClickRegistrar(this, e);

        }

        /// <summary>
        /// Manipula Evento ClickSolicitar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickSolicitar(EventArgs e)
        {
            if (ClickSolicitar != null)
                ClickSolicitar(this, e);

        }

        /// Evento disparado al presionar el boton Registrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnRegistrar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickRegistrar != null)
                OnClickRegistrar(e);
        }

        /// <summary>
        /// Manipula Evento ClickCancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickCancelar(EventArgs e)
        {
            if (ClickCancelar != null)
                ClickCancelar(this, e);

        }
        /// <summary>
        /// Evento disparado al presionar el boton cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnCancelar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelar != null)
                OnClickCancelar(new EventArgs());
        }
        /// <summary>
        /// Manipula Evento ClickEliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickEliminar(EventArgs e)
        {
            if (ClickEliminar != null)
                ClickEliminar(this, e);

        }

        /// Evento disparado al presionar el boton Eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnEliminar(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickEliminar != null)
                OnClickEliminar(new EventArgs());
        }

        /// <summary>
        /// Evento generado al cambiar el Beneficiario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBeneficiario_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Instanciamos Asignación
            using (SAT_CL.Despacho.MovimientoAsignacionRecurso objAsignacion = new MovimientoAsignacionRecurso(Convert.ToInt32(ddlBeneficiario.SelectedValue)))
            {
                //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, objAsignacion.id_movimiento_asignacion_recurso,
                                       objAsignacion.id_recurso_asignado);


                //Asignando Atributos
                this._id_unidad = id_unidad;
                this._id_operador = id_operador;
                this._id_proveedor_compania = id_tercero;
                this._id_movimiento_asignacion = objAsignacion.id_movimiento_asignacion_recurso;
            }
            //Inicializamos Valor a control de Referencia de Operador /Unidad

            //Validamos si el Depósito es para Operador
            if (id_operador > 0)
            {
                //Obtenemos primer unidad 
                int id_recurso = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionUnidadPropia(this._id_movimiento);

                //Validamos Recurso
                if (id_recurso > 0)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_recurso))
                    {
                        //Inicializamos Control
                        txtOperadorUnidad.Text = objUnidad.numero_unidad;
                    }
                }
            }
            //Si la Asignación es para Unidad
            else if (id_unidad > 0)
            {
                //Obtenemos primer unidad 
                int id_recurso = MovimientoAsignacionRecurso.ObtienePrimerRecursoAsignado(this._id_movimiento, MovimientoAsignacionRecurso.Tipo.Operador);

                //Validamos Recurso
                if (id_recurso > 0)
                {
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(id_recurso))
                    {
                        //Inicializamos Control
                        txtOperadorUnidad.Text = objOperador.nombre;
                    }
                }
            }
            //En caso de Error
            if (!resultado.OperacionExitosa)
            {
                //Mostramos Mensaje
                lblError.Text = resultado.Mensaje;
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoCargo, "", 44);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUbicaciones, 20, "Seleccione la Ubicación de Combustible", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }

        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdDeposito"]) != 0)
                this._id_deposito = Convert.ToInt32(ViewState["IdDeposito"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdUnidad"]) != 0)
                this._id_unidad = Convert.ToInt32(ViewState["IdUnidad"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdOperador"]) != 0)
                this._id_operador = Convert.ToInt32(ViewState["IdOperador"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdProveedorCompania"]) != 0)
                this._id_proveedor_compania = Convert.ToInt32(ViewState["IdProveedorCompania"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdMovimiento"]) != 0)
                this._id_movimiento = Convert.ToInt32(ViewState["IdMovimiento"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdMovimientoAsignacion"]) != 0)
                this._id_movimiento_asignacion = Convert.ToInt32(ViewState["IdMovimientoAsignacion"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdClienteReceptor"]) != 0)
                this._id_cliente_receptor = Convert.ToInt32(ViewState["IdClienteReceptor"]);
            //Recuperando Atributos
            if (Validacion.ValidaOrigenDatos((DataTable)ViewState["FacturasLigadas"]))
                this._dtFacturasLigadas = (DataTable)ViewState["FacturasLigadas"];
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdDeposito"] = this._id_deposito;
            ViewState["IdUnidad"] = this._id_unidad;
            ViewState["IdOperador"] = this._id_operador;
            ViewState["IdProveedorCompania"] = this._id_proveedor_compania;
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["IdMovimiento"] = this._id_movimiento;
            ViewState["IdMovimientoAsignacion"] = this._id_movimiento_asignacion;
            ViewState["IdClienteReceptor"] = this._id_cliente_receptor;
            ViewState["FacturasLigadas"] = this._dtFacturasLigadas;
        }
        /// <summary>
        /// Método encargado de Ejecutar el Script de Configuración
        /// </summary>
        private void inicializaScriptFacturaXML()
        {
            //Declarando Script de Configuración XML
            string scriptXML = @"<script type='text/javascript'>
                                    
                                </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "FacturaXML", scriptXML, false);
        }


        /// <summary>
        /// Inicializa Valores Forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga Catalogos
            cargaCatalogos();

        }

        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            //Verificamos Id Depósito
            if (this._id_deposito == 0)
            {
                //Inicializamos Valores
                lblNoDeposito.Text = "";
                ddlConcepto.SelectedValue = "0";
                txtEstatus.Text = "Registrado";
                txtMonto.Text = "";
                chkDepositoExtraEfectivo.Checked = false;
                ddlTipoCargo.SelectedValue = "1";
                txtReferencia.Text = "";
                txtFechaProgramada.Text = "";
                lblError.Text = "";
                btnEliminar.Enabled = false;
            }
            else
            {
                //Instanciamos Deposito
                using (Deposito objDeposito = new Deposito(this._id_deposito))
                {
                    //Inicializamos Valores
                    lblNoDeposito.Text = objDeposito.no_deposito.ToString();
                    ddlConcepto.SelectedValue = objDeposito.id_concepto.ToString();
                    string estatus = ((Deposito.EstatusDeposito)objDeposito.id_estatus).ToString();
                    switch ((Deposito.EstatusDeposito)objDeposito.id_estatus)
                    {
                        case Deposito.EstatusDeposito.EnAutorizacion:
                            estatus = "En Autorización";
                            break;
                        case Deposito.EstatusDeposito.PorLiquidar:
                            estatus = "Por Liquidar";
                            break;
                        case Deposito.EstatusDeposito.PorDepositar:
                            estatus = "Por Depositar";
                            break;
                    }
                    txtEstatus.Text = estatus;
                    txtOperadorUnidad.Text = objDeposito.identificador_operador_unidad;
                    txtMonto.Text = objDeposito.objDetalleLiquidacion.valor_unitario.ToString();
                    chkDepositoExtraEfectivo.Checked = objDeposito.bit_efectivo;
                    ddlTipoCargo.SelectedValue = objDeposito.id_tipo_cargo.ToString();
                    txtReferencia.Text = objDeposito.referencia;
                    //Instanciamos Depósito Programado
                    using (AnticipoProgramado objAnticipoProgramado = new AnticipoProgramado(this._id_deposito, objDeposito.id_compania_emisor))
                    {
                        txtFechaProgramada.Text = objAnticipoProgramado.fecha_ejecucion.ToString("dd/MM/yyyy");
                    }
                    btnEliminar.Enabled = true;
                    lblError.Text = "";
                }


            }
        }

        /// <summary>
        /// Habilita Controles
        /// </summary>
        private void habilitaControles()
        {
            ddlConcepto.Enabled = HabilitaConcepto;
            chkDepositoExtraEfectivo.Enabled =
            txtOperadorUnidad.Enabled =
            txtMonto.Enabled = true;
            txtEstatus.Enabled = false;
            ddlTipoCargo.Enabled =
            txtReferencia.Enabled =
            btnCancelar.Enabled =
            btnRegistrar.Enabled = true;
            divUbicaciones.Visible = false;
        }

        /// <summary>
        /// Método encargado de Inicializar los validadores requeridos
        /// </summary>
        /// <param name="id_tercero"></param>
        private void inicializaValidador(int id_tercero)
        {
            string script = "";
            //Validamos depòsito para Tercero
            if (id_tercero != 0)
            {
                //Generamos script para validación de Fechas
                script =
               @"<script type='text/javascript'>
                
                    var validacionDepositoTercero = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#" + txtMonto.ClientID + @"').validationEngine('validate');
                        var isValid2 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                        return isValid1 && isValid2
                    }; 
                //Botón Buscar
                $('#" + btnRegistrar.ClientID + @"').click(validacionDepositoTercero);
                                    </script>";

            }
            else
            {
                //Generamos script para validación de Fechas
                script =
                   @"<script type='text/javascript'>
                
                    var validacionDeposito = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#" + txtMonto.ClientID + @"').validationEngine('validate');
                        var isValid2 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                        var isValid3 = !$('#" + txtOperadorUnidad.ClientID + @"').validationEngine('validate');
                        return isValid1 && isValid2 && isValid3
                    }; 
                //Botón Buscar
                $('#" + btnRegistrar.ClientID + @"').click(validacionDeposito);
                                    </script>";

            }


            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Deposito", script, false);
        }
        /// <summary>
        /// Método encargado de Configurar el Control de Facturas
        /// </summary>
        private void configuraControlFactura()
        {
            //Validando que Exista el Deposito
            if (this._id_deposito > 0)
            {
                //Validando que existe Valor en Sesión
                if (ddlConcepto.SelectedItem.Text.Contains("Anticipo Proveedor"))

                    //Mostrando Control
                    lkbAgregarFactura.Visible = true;
                else
                    //Ocultando Control
                    lkbAgregarFactura.Visible = false;
            }
            else
                //Ocultando Control
                lkbAgregarFactura.Visible = false;
            //Validanco que exista Valor en Sesión
            if (ddlConcepto.SelectedItem.Text.Contains("Diesel (Tractor)") || ddlConcepto.SelectedItem.Text.Contains("G.Magna") || ddlConcepto.SelectedItem.Text.Contains("G.Premium"))
                //Mostrando control
                divUbicaciones.Visible = true;
            else
                divUbicaciones.Visible = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idServicio"></param>
        /// <param name="idMovimiento"></param>
        public void InicializaControl(int idServicio, int idMovimiento)
        {
            //Asignando Atributos
            this._id_deposito = 0;
            this._id_unidad = 0;
            this._id_operador = 0;
            this._id_proveedor_compania = 0;
            this._id_movimiento_asignacion = 0;
            this._id_movimiento = idMovimiento;

            //Instanciamos Servicio
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                this._id_servicio = objServicio.id_servicio;
                this._id_cliente_receptor = objServicio.id_cliente_receptor;
            }

            //Inicializa Valores
            inicializaValores();

            //Limpiamos Referencia de Operador
            txtOperadorUnidad.Text = "";
            //Cambiamos Vista
            mtvBeneficiario.ActiveViewIndex = 1;

            //Habilita Controles
            habilitaControles();

            //Instanciamos Usuario Compañia para Obtener Departamentos
            using (UsuarioCompania objUsuarioCompania = new UsuarioCompania(((Usuario)Session["usuario"]).id_usuario, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Si sólo se requiere el concepto de Anticipo Proveedor
                if (this.SoloAnticipoProveedor)
                {
                    //Cargando catálogo personalizado
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 90, "-Seleccione el concepto-", this._id_servicio,
                        (this._id_unidad > 0 ? 1 : (this._id_operador > 0 ? 2 : 3)).ToString(), objUsuarioCompania.id_departamento, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                }
                //Si se deben cargar los conceptos disponibles
                else
                {
                    //Cargando sobre control catálogo
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 21, "-Seleccione el concepto-", this._id_servicio,
                        (this._id_unidad > 0 ? 1 : (this._id_operador > 0 ? 2 : 3)).ToString(), objUsuarioCompania.id_departamento, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                }
            }

            //Cargamos Catalogo de Asignaciónes Validas
            using (DataTable mit = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignacionesParaRegistroAnticipos(idMovimiento))
            {
                //Cargamos Catalogo de Beneficiarios
                TSDK.ASP.Controles.CargaDropDownList(ddlBeneficiario, mit, "Id", "Recurso");
            }

            //Invocando Método de Configuración
            configuraControlFactura();
        }

        /// <summary>
        /// Inicializamos Controles 
        /// </summary>
        /// <param name="id_deposito">Id Deposito</param>
        /// <param name="idUnidad">Unidad</param>
        /// <param name="idOperador">Operador</param>
        /// <param name="idProveedorCompania"> Proveedor Compañia</param>
        /// <param name="idServicio">Servicio</param>
        /// <param name="idMovimiento">Movimiento</param>
        /// <param name="idMovimientoAsignacion">Movimiento Asignación Recurso</param>
        /// <param name="idClienteReceptor">Cliente Receptor</param>
        public void InicializaControl(int id_deposito, int idUnidad, int idOperador, int idProveedorCompania, int idServicio, int idMovimiento, int idMovimientoAsignacion,
                                     int idClienteReceptor)
        {
            //Asignando Atributos
            this._id_deposito = id_deposito;
            this._id_unidad = idUnidad;
            this._id_operador = idOperador;
            this._id_proveedor_compania = idProveedorCompania;
            this._id_movimiento_asignacion = idMovimientoAsignacion;
            this._id_movimiento = 0;
            this._id_servicio = idServicio;
            this._id_cliente_receptor = idClienteReceptor;

            //Validando que exista un Deposito
            if (this._id_deposito > 0)
            {
                //Instanciando Deposito
                using (SAT_CL.EgresoServicio.Deposito dep = new Deposito(this._id_deposito))
                {
                    //Validando que exista el Registro
                    if (dep.habilitar)
                    {
                        //Asignando Valores del Deposito
                        this._id_unidad = dep.objDetalleLiquidacion.id_unidad;
                        this._id_operador = dep.objDetalleLiquidacion.id_operador;
                        this._id_proveedor_compania = dep.objDetalleLiquidacion.id_proveedor_compania;
                        this._id_servicio = dep.objDetalleLiquidacion.id_servicio;
                    }
                }
            }

            //Inicializa Valores
            inicializaValores();

            //Cambiamos Vista
            mtvBeneficiario.ActiveViewIndex = 0;

            //Validamos si el Depósito es para Operador
            if (idOperador > 0)
            {
                //Obtenemos primer unidad 
                int id_recurso = MovimientoAsignacionRecurso.ObtienePrimerRecursoAsignado(idMovimiento, MovimientoAsignacionRecurso.Tipo.Unidad);

                //Validamos Recurso
                if (id_recurso > 0)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_recurso))
                    {
                        //Validando que exista el Registro
                        if (objUnidad.habilitar)

                            //Inicializamos Control
                            txtOperadorUnidad.Text = objUnidad.numero_unidad;
                        else
                            //Inicializamos Control
                            txtOperadorUnidad.Text = "";
                    }
                }

                //Instanciamos Operador
                using (Operador objOperador = new Operador(_id_operador))
                {
                    //Asignamos Valor al Control
                    lblBeneficiario.Text = objOperador.nombre;
                }
            }
            else
            {
                //Validamos si el Depósito es Unidad
                if (idUnidad > 0)
                {
                    //Obtenemos primer unidad 
                    int id_recurso = MovimientoAsignacionRecurso.ObtienePrimerRecursoAsignado(idMovimiento, MovimientoAsignacionRecurso.Tipo.Operador);

                    //Validamos Recurso
                    if (id_recurso > 0)
                    {
                        //Instanciamos Operador
                        using (Operador objOperador = new Operador(id_recurso))
                        {
                            //Validando Operador
                            if (objOperador.habilitar)

                                //Inicializamos Control
                                txtOperadorUnidad.Text = objOperador.nombre;
                            else
                                //Inicializamos Control
                                txtOperadorUnidad.Text = "";
                        }
                    }

                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(idUnidad))
                    {
                        //Asignamos valor al control 
                        lblBeneficiario.Text = objUnidad.numero_unidad.ToString();
                    }
                }
                else
                {
                    //Instanciamos Tercero
                    using (CompaniaEmisorReceptor objCompaniaEmisorReceptor = new CompaniaEmisorReceptor(idProveedorCompania))
                    {
                        //Asignamos valor al control 
                        lblBeneficiario.Text = objCompaniaEmisorReceptor.nombre;

                        //Instanciando Servicio
                        using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                        {
                            //Validando que exista
                            if (serv.habilitar)
                            {
                                //Asignando Operador
                                txtOperadorUnidad.Text = SAT_CL.Global.Referencia.CargaReferencia(serv.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 1, "Operador", 0, "Referencia de Viaje"));
                            }
                        }

                        //Limpiando Sesión
                        Session["XML"] = null;
                    }
                }
            }

            //Habilita Controles
            habilitaControles();
            //Instanciamos Usuario Compañia para Obtener Departamentos
            using (UsuarioCompania objUsuarioCompania = new UsuarioCompania(((Usuario)Session["usuario"]).id_usuario, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Si sólo se requiere el concepto de Anticipo Proveedor
                if (this.SoloAnticipoProveedor)
                {
                    //Cargando catálogo personalizado
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 90, "-Seleccione el concepto-", this._id_servicio,
                        (this._id_unidad > 0 ? 1 : (this._id_operador > 0 ? 2 : 3)).ToString(), objUsuarioCompania.id_departamento, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                }
                //Si se deben cargar los conceptos disponibles
                else
                {
                    //Cargando sobre control catálogo
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 21, "-Seleccione el concepto-", this._id_servicio,
                        (this._id_unidad > 0 ? 1 : (this._id_operador > 0 ? 2 : 3)).ToString(), objUsuarioCompania.id_departamento, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                }
            }

            //Invocando Método de Configuración
            configuraControlFactura();
        }

        /// <summary>
        /// Deshabilitar Depósito 
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDeposito(int IdDeposito)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            RetornoOperacion resultadoP = new RetornoOperacion();

            //Validamos Existencia de Id de Depósito
            if (IdDeposito > 0)
            {
                //Obtenemos la fecha de ejecución del Anticipo Programado
                AnticipoProgramado objFechaEjecucion = new AnticipoProgramado(IdDeposito, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Instanciamos Depósito
                using (Deposito objDeposito = new Deposito(IdDeposito))
                {
                    //Deshabilitamos Depósito
                    resultado = objDeposito.DeshabilitaDeposito(((Usuario)Session["usuario"]).id_usuario, objFechaEjecucion.fecha_ejecucion);

                    if (resultado.OperacionExitosa)
                    {
                        //Instanciamos depósito programado
                        using (AnticipoProgramado objAnticipoProgramado = new AnticipoProgramado(IdDeposito, objDeposito.id_compania_emisor))
                        {
                            resultadoP = objAnticipoProgramado.DeshabilitaAnticipoProgramado(((Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
            }
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Solicitamos Depósito
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion SolicitaDeposito()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Deposito
            using (Deposito objDeposito = new Deposito(this._id_deposito))
            {

                //Solicitamos Depósito
                resultado = objDeposito.SolicitarDeposito(Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)), ((Usuario)Session["usuario"]).id_usuario);
                //Asignando Deposito
                this._id_deposito = resultado.IdRegistro;

                //Validando Operación Exitosa
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Control
                    InicializaControl(resultado.IdRegistro, this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_servicio,
                                     this._id_movimiento, this._id_movimiento_asignacion, this._id_cliente_receptor);
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Registra Depósito Validando la Asignación del Recurso
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion RegistraDeposito()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            RetornoOperacion resultadoP = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int id_deposito = 0;

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos que se encuentre Seleccionado el Concepto
                if (ddlConcepto.SelectedValue != "0")
                {
                    //Validamos Existencia de Id de Depósito
                    if (this._id_deposito == 0)
                    {
                        //Validamos que exista la Asignación
                        if (this._id_movimiento_asignacion > 0)
                        {
                            //Registramos Depósito
                            resultado = Deposito.InsertaDeposito(Convert.ToInt32(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor), txtOperadorUnidad.Text,
                                                        this._id_cliente_receptor, Convert.ToInt32(ddlConcepto.SelectedValue), 0, 0, txtReferencia.Text.ToUpper(),
                                                        (Deposito.TipoCargo)Convert.ToByte(ddlTipoCargo.SelectedValue), chkDepositoExtraEfectivo.Checked, Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)),
                                                        this._id_unidad, this._id_operador, 0, this._id_servicio, this._id_movimiento_asignacion, Convert.ToDecimal(txtMonto.Text),
                                                        Convert.ToDateTime(txtFechaProgramada.Text),((Usuario)Session["usuario"]).id_usuario);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Registramos el depósito como programado
                                resultadoP = SAT_CL.EgresoServicio.AnticipoProgramado.InsertaAnticipoProgramado(resultado.IdRegistro, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                                    this._id_servicio, Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)), Convert.ToDecimal(txtMonto.Text),
                                                                                                    Convert.ToDateTime(txtFechaProgramada.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                
                                //Instanciando Resultado con el Deposito
                                //resultado = new RetornoOperacion(id_deposito);
                                //Instanciamos Depósito
                                using (Deposito objDeposito = new Deposito(resultado.IdRegistro))
                                {
                                    //Obteniendo Deposito
                                    id_deposito = resultado.IdRegistro;

                                    //Solicitamos Depósito
                                    resultado = objDeposito.SolicitarDeposito(Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)), ((Usuario)Session["usuario"]).id_usuario);

                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Actualizando Atributos
                                        objDeposito.ActualizaDeposito();

                                        //Instanciamos Concepto
                                        using (SAT_CL.EgresoServicio.ConceptoDeposito objConceptoDeposito = new ConceptoDeposito(Convert.ToInt32(ddlConcepto.SelectedValue)))
                                        {
                                            //Validando que el Concepto sea de Diesel para Tractor o Remolque
                                            if (objConceptoDeposito.descripcion == "Diesel (Tractor)" || objConceptoDeposito.descripcion == "Diesel (Remolque)" || objConceptoDeposito.descripcion == "G.Magna" || objConceptoDeposito.descripcion == "G.Premium")
                                            {
                                                //Declarando Variables Auxiliares
                                                decimal litros = 0.00M;
                                                decimal costo_diesel = Convert.ToDecimal(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Precio Combustible"));

                                                //Obteniendo Litros
                                                litros = Convert.ToDecimal(txtMonto.Text) / costo_diesel;

                                                //Insertando Vale de Diesel* corregir Unidad Diesel
                                                resultado = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaValeDieselPorDepositoProgramado(txtOperadorUnidad.Text.ToUpper(),
                                                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0,
                                                                            Convert.ToInt32(ddlUbicaciones.SelectedValue),
                                                                            TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), objDeposito.fecha_deposito,
                                                                            costo_diesel, false, txtReferencia.Text.ToUpper(), id_deposito, (byte)AsignacionDiesel.TipoVale.Original,
                                                                            litros, this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_servicio,
                                                                            this._id_movimiento_asignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);//
                                            }
                                        }
                                    }
                                }
                            }


                            //Si la operación fue exitosa
                            if (resultado.OperacionExitosa)

                                //Instanciando Resultado con el Deposito
                                resultado = new RetornoOperacion(id_deposito);
                        }
                        else
                            //Mostrando Error
                            resultado = new RetornoOperacion("Seleccione el Beneficiario.");
                    }
                    else
                    {
                        //Instanciamos Depósito
                        using (Deposito objDeposito = new Deposito(this._id_deposito))
                        {
                            //Instanciamos la fecha de ejecución del Anticipo Programado
                            AnticipoProgramado FechaEjecucion = new AnticipoProgramado(this._id_deposito, objDeposito.id_compania_emisor);

                            if(FechaEjecucion.fecha_ejecucion >= DateTime.Today)
                            {
                                //Editamos Depósito
                                resultado = objDeposito.EditaDeposito(Convert.ToInt32(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor),
                                                             Convert.ToInt32(ddlConcepto.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)), Convert.ToDecimal(txtMonto.Text)
                                                             , txtReferencia.Text.ToUpper(), (Deposito.TipoCargo)Convert.ToByte(ddlTipoCargo.SelectedValue),
                                                                 chkDepositoExtraEfectivo.Checked, Convert.ToDateTime(txtFechaProgramada.Text), ((Usuario)Session["usuario"]).id_usuario);
                                //
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Concepto
                                    using (SAT_CL.EgresoServicio.ConceptoDeposito objConceptoDeposito = new ConceptoDeposito(Convert.ToInt32(ddlConcepto.SelectedValue)))
                                    {
                                        //Validando que el Concepto sea de Diesel para Tractor o Remolque
                                        if (objConceptoDeposito.descripcion == "Diesel (Tractor)" || objConceptoDeposito.descripcion == "Diesel (Remolque)" || objConceptoDeposito.descripcion == "G.Magna" || objConceptoDeposito.descripcion == "G.Premium")
                                        {
                                            //Instanciando Vale de Diesel
                                            using (SAT_CL.EgresoServicio.AsignacionDiesel vale_diesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneValePorDeposito(objDeposito.id_deposito))
                                            {
                                                //Validando que exista el Vale
                                                if (vale_diesel.habilitar)
                                                {
                                                    //Declarando Variables Auxiliares
                                                    decimal litros = 0.00M;
                                                    decimal costo_diesel = Convert.ToDecimal(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Precio Combustible"));

                                                    //Obteniendo Litros
                                                    litros = Convert.ToDecimal(txtMonto.Text) / costo_diesel;

                                                    //Editando Vale de Diesel
                                                    resultado = vale_diesel.objDetalleLiquidacion.EditaDetalleLiquidacion(69, vale_diesel.id_asignacion_diesel, vale_diesel.objDetalleLiquidacion.id_estatus_liquidacion,
                                                                    vale_diesel.objDetalleLiquidacion.id_unidad, vale_diesel.objDetalleLiquidacion.id_operador, vale_diesel.objDetalleLiquidacion.id_proveedor_compania,
                                                                    vale_diesel.objDetalleLiquidacion.id_servicio, vale_diesel.objDetalleLiquidacion.id_movimiento, vale_diesel.objDetalleLiquidacion.fecha_liquidacion,
                                                                    vale_diesel.objDetalleLiquidacion.id_liquidacion, litros, vale_diesel.objDetalleLiquidacion.id_unidad_medida, costo_diesel,
                                                                    ((Usuario)Session["usuario"]).id_usuario);

                                                    //Validando Operación Exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Instanciando Deposito
                                                        resultado = new RetornoOperacion(this._id_deposito);
                                                    }
                                                }
                                                else
                                                    //Instanciando Exceción
                                                    resultado = new RetornoOperacion("No se puede encontrar el Vale de Diesel");
                                            }
                                        }

                                        using (AnticipoProgramado objAnticipoProgramado = new AnticipoProgramado(this._id_deposito, objDeposito.id_compania_emisor))
                                        {
                                            if (objAnticipoProgramado.habilitar)
                                            {
                                                resultadoP = objAnticipoProgramado.EditarAnticipoProgramado(this._id_deposito, Convert.ToInt32(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor), this._id_servicio,
                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(ddlConcepto.SelectedItem.Text, ':', 1)), Convert.ToDecimal(txtMonto.Text),
                                                                Convert.ToDateTime(txtFechaProgramada.Text), txtReferencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }

                                        }
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El depósito no puede ser editado porque ya fue ejecutado.");

                        }
                    }

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Validando que existe Valor en Sesión
                        if (ddlConcepto.SelectedItem.Text.Contains("Anticipo Proveedor"))
                        {
                            //Asignando Deposito
                            this._id_deposito = resultado.IdRegistro;

                            //Mostrando Ventana para Guardado
                            ScriptServer.AlternarVentana(this.Page, "FacturasXML", "contenedorVentanaFacturaAnticipo", "ventanaFacturaAnticipo");
                        }
                        //Validando Operación Exitosa
                        else if (resultado.OperacionExitosa)
                        {
                            //Muestra Boton Solicitar
                            this.MuestraSolicitar = true;

                            //Inicializamos Control
                            InicializaControl(resultado.IdRegistro, this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_servicio,
                                             this._id_movimiento, this._id_movimiento_asignacion, this._id_cliente_receptor);

                        }

                        //Completando Transacción
                        trans.Complete();
                    }

                }
                else
                    //Mostramos Mensaje Error
                    lblError.Text = "Seleccione el concepto";
            }

            //Devolvemos Valor
            return resultado;
        }


        #endregion
    }
}