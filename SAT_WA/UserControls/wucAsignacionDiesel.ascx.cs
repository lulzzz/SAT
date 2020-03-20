using SAT_CL.EgresoServicio;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System;
using System.Data;
using System.Web.UI;
using TSDK.Base;
using System.Web;
using TSDK.ASP;
using System.Transactions;
using TSDK.Datos;
using System.Configuration;
using System.Web.UI.WebControls;

namespace SAT.UserControls
{
    public partial class wucAsignacionDiesel : System.Web.UI.UserControl
    {
        #region Atributos

        private int _idAsignacionDiesel;
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
        /// Id movimiento Asignación
        /// </summary>
        private int _id_movimiento_asignacion;

        /// <summary>
        /// Atributo que Almacena el Registro Asignación Diesel
        /// </summary>
        public int idAsignacionDiesel { get { return this._idAsignacionDiesel; } }
        private int _idUnidadDiesel;
        /// <summary>
        /// Id Unidad de la Asiganción de Diesel
        /// </summary>
        public int idUnidadDiesel { get { return this._idUnidadDiesel; } }

        /// <summary>
        /// Atributo encargado de Asignar el Indice de Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Tabulación
                txtOperadorProveedor.TabIndex =
                ddlUbicacion.TabIndex =
                txtFecSol.TabIndex =
                txtCostoCombustible.TabIndex =
                txtReferencia.TabIndex =
                ddlTipoVale.TabIndex =
                btnGuardar.TabIndex =
                btnCancelar.TabIndex = value;
            }
            //Obteniendo tabulación
            get { return txtOperadorProveedor.TabIndex; }
        }

        #endregion

        #region Manejador de Eventos

        /// <summary>
        /// Manejador del Evento "Guardar"
        /// </summary>
        public event EventHandler ClickGuardarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Guardar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickGuardarAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickGuardarAsignacion != null)
                //Iniciando Evento
                ClickGuardarAsignacion(this, e);
        }

        /// <summary>
        /// Manejador del Evento "Cancelar"
        /// </summary>
        public event EventHandler ClickCancelarAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Cancelar"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickCancelarAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickCancelarAsignacion != null)
                //Iniciando Evento
                ClickCancelarAsignacion(this, e);
        }

        /// <summary>
        /// Manejador del Evento "Lectura"
        /// </summary>
        public event EventHandler ClickReferenciaAsignacion;
        /// <summary>
        /// Evento que Manipula el Manejador "Referencia"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickReferenciaAsignacion(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickReferenciaAsignacion != null)
                //Iniciando Evento
                ClickReferenciaAsignacion(this, e);
        }


        /// <summary>
        /// Evento que Manipula el Manejador
        /// </summary>
        public event EventHandler ClickCalculadoDiesel;
        public virtual void OnClickCalculadoDiesel(EventArgs e)
        {
            if (ClickCalculadoDiesel != null)
                ClickCalculadoDiesel(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si fue un PostBack
            if (!(Page.IsPostBack))
                //Asignando Atributos
                asignaAtributos();
            else//Recuperando Atributos
                recuperaAtributos();

            //Calendar de Fecha de Carga
            cargaScriptFecha();

            //Inicializa Validador
            inicializaValidador(this._id_proveedor_compania);
        }


        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   //Invocando Método de Asignación
            asignaAtributos();
        }

        /// <summary>
        /// Evento generado al Seleccionar el Beneficiario de Diesel
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
                        txtOperadorProveedor.Text = objUnidad.numero_unidad;
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
                        txtOperadorProveedor.Text = objOperador.nombre;
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

        /// <summary>
        /// Evento generado al Cambiar la Selección de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUnidadDiesel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            
            configuraValorCalculado();
        }

        /// <summary>
        /// Evento desencadenado al cambiar el indice del Control "Ubicacion"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        protected void ddlUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Actualización
            obtieneCostoCombustible();

            //Validando que exista la Asignación de Diesel
            if (this._idAsignacionDiesel > 0)
            {
                //Declarando Variable Auxiliar
                int idCarga = 0;

                //Obteniendo Referencias
                using (DataTable dtReferencias = Referencia.CargaReferenciasGeneral(this._idAsignacionDiesel, 69))
                {
                    //Validando que existen
                    if (Validacion.ValidaOrigenDatos(dtReferencias))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtReferencias.Rows)
                        {
                            //Validando que sea la Referencia de "IdCarga"
                            if (dr["Tipo"].ToString().Equals("IdCarga"))

                                //Asignando Registro de Carga
                                idCarga = Convert.ToInt32(dr["Descripcion"]);
                        }
                    }
                }

                //Instanciando Carga Autotanque
                using (CargaAutoTanque carga = new CargaAutoTanque(idCarga))
                {
                    //Validando que exista la Carga Activa
                    if (carga.habilitar)

                        //Asignando Carga Actual
                        lblCantidadDisp.Text = carga.sobrante_carga_actual.ToString();
                    else
                        //Limpiando Control
                        lblCantidadDisp.Text = "-";
                }
            }
            else
            {
                //Instanciando Carga Autotanque
                using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(Convert.ToInt32(ddlUbicacion.SelectedValue),
                                                                                    TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                {
                    //Validando que exista la Carga Activa
                    if (carga.habilitar)

                        //Asignando Carga Actual
                        lblCantidadDisp.Text = carga.sobrante_carga_actual.ToString();
                    else
                        //Limpiando Control
                        lblCantidadDisp.Text = "-";
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Texto del Control "Costo Combustible"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCostoCombustible_TextChanged(object sender, EventArgs e)
        {
            //Validando que exista un Vale Registrado
            if (this._idAsignacionDiesel > 0)
            {
                //Instanciando Vale
                using (AsignacionDiesel ad = new AsignacionDiesel(this._idAsignacionDiesel))
                using (DetalleLiquidacion dl = new DetalleLiquidacion(ad.id_asignacion_diesel, 69))
                {
                    //Validando Existencia
                    if (ad.habilitar && dl.habilitar)
                    {
                        //Validando que venga de Deposito
                        if (ad.id_deposito > 0)
                        {
                            //Configurando Control
                            txtLitros.Enabled = false;
                            txtLitros.Text = (Math.Round(dl.monto / Convert.ToDecimal(txtCostoCombustible.Text), 2)).ToString();
                        }
                        else
                            //Habilitando Control
                            txtLitros.Enabled = true;
                    }
                }
            }

            //Invocando Método de Total
            obtieneTotalVale();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Texto del Control "Litros"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtLitros_TextChanged(object sender, EventArgs e)
        {
            //Invocando Método de Obtención
            obtieneTotalVale();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickGuardarAsignacion != null)
                //Inicializando Manejador de Eventos
                OnClickGuardarAsignacion(e);
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCancelarAsignacion != null)
                //Inicializando Manejador de Eventos
                OnClickCancelarAsignacion(e);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReferencias_Click(object sender, EventArgs e)
        {
            //Validando que exista un Asignación de Diesel
            if (this._idAsignacionDiesel > 0)
            {
                //Validando que exista un Evento
                if (ClickReferenciaAsignacion != null)
                    //Inicializando Manejador de Eventos
                    OnClickReferenciaAsignacion(e);
            }
            else
                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this.Page, "No Existe el Vale de Diesel", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //Validando que exista una Asignación
            if (this._idAsignacionDiesel != 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Instanciando Asignación
                using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(this._idAsignacionDiesel))
                {
                    //Validando que exista el Registro
                    if (ad.id_asignacion_diesel != 0)
                    {
                        //Incrementando Contador
                        result = ad.IncrementaConteoImpresion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        //Validando si la Operacion fue Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Asignando Registro
                            this._idAsignacionDiesel = result.IdRegistro;
                            //Obteniendo Ruta
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucAsignacionDiesel.ascx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ValeDiesel", Convert.ToInt32(this._idAsignacionDiesel)), "Vale Diesel", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            //Mostrando Mensaje de Operación
                            lblError.Text = result.Mensaje;
                            //Inicializando Control
                            inicializaControl();
                        }
                    }
                }
            }
            else
                //Mostrando Error
                lblError.Text = "No existe la Asignación";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarDiesel_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            obtieneCostoCombustible();

            //Calculando Lts Sistema
            configuraValorCalculado();

            /*/Validando que Exista una Master Page
            if (this.Page.Master == null)

                //Ocultando Ventana Modal
                TSDK.ASP.ScriptServer.AlternarVentana(btnAceptarDiesel, "ConfirmacionDiesel", "contenedorVentanaConfirmacionDiesel", "ventanaConfirmacionDiesel");
            //*/
        }

        /// <summary>
        /// Evento generado al dar click en Calcular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCalculado_Click(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickCalculadoDiesel != null)
                //Inicializando Manejador de Eventos
                OnClickCalculadoDiesel(e);
        }
        
        /// <summary>
        /// Evento Producido al Cambiar el Tipo de Combustible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbTipoCombustible_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            obtieneCostoCombustible();
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            //Cargando Controles
            cargaCatalogos();

            //Instanciando Asignacion
            using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(this._idAsignacionDiesel))
            {
                //Validando que exista la Asignación
                if (ad.id_asignacion_diesel != 0)
                {
                    //Asignando Valores
                    lblId.Text = ad.no_vale;
                    ddlEstatus.SelectedValue = ad.id_estatus.ToString();
                    lblImpresion.Text = ad.conteo_impresion.ToString() + " Impresión(es)";
                    txtOperadorProveedor.Text = ad.nombre_operador_proveedor;

                    //Mostrando Total
                    lblTotal.Text = string.Format("{0:C2}", (ad.objDetalleLiquidacion.cantidad * ad.objDetalleLiquidacion.valor_unitario));

                    //Validando Estación
                    if (ad.id_ubicacion_estacion == 0)

                        //Inicializando Drop
                        Controles.InicializaDropDownList(ddlUbicacion, "No tiene ubicaciones configuradas");
                    else
                        //Asignando Valor
                        ddlUbicacion.SelectedValue = ad.id_ubicacion_estacion.ToString();

                    //Instanciando Detalle
                    using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(ad.id_asignacion_diesel, 69))
                    {   
                        //Asignando Litros
                        txtLitros.Text = dl.cantidad.ToString("#############0.00##");

                        //Instanciando Costo Combustible
                        using (SAT_CL.EgresoServicio.CostoCombustible cc = new CostoCombustible(ad.id_costo_diesel))
                        {
                            //Validando si existe Costo de Combustible
                            if (cc.habilitar)
                            {
                                //Invocando Control de Configuración
                                configuraTipoCombustible(cc.id_tipo_combustible);
                                
                                //Configurando Controles
                                txtCostoCombustible.Text = cc.costo_combustible.ToString();
                            }
                            else
                            {
                                //Configurando Controles
                                txtCostoCombustible.Text = dl.valor_unitario.ToString("0.00##");
                                txtCostoCombustible.Enabled =
                                rdbDiesel.Checked = true;
                                rdbMagna.Checked =
                                rdbPremiun.Checked = false;
                            }
                        }
                    }

                    //Asignando Fechas
                    txtFecSol.Text = ad.fecha_solicitud.ToString("dd/MM/yyyy HH:mm");
                    txtFecCarga.Text = ad.fecha_carga == DateTime.MinValue ? "" : ad.fecha_carga.ToString("dd/MM/yyyy HH:mm");
                    //Instanciamos Unidad
                    using (SAT_CL.Global.Unidad uni = new Unidad(ad.id_unidad_diesel))
                    {
                        //Obtenemos Rendimiento de la Unidad
                        decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, ad.id_unidad_diesel, "Rendimiento Unidad", "Rendimiento"), "0"));
                        
                        //Si el Rendimiento No existe
                        if (rendimiento == 0)
                        {
                            //Validamos Que exista Kilometraje y  Cobustible Asignado
                            if (uni.kilometraje_asignado != 0)
                            {
                                if (uni.combustible_asignado != 0)
                                {
                                    //Calculamos rendimiento
                                    rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                                }
                                else
                                {
                                    //Mostramos Error de Kilometraje Asignado
                                    lnkCalculado.Text = "No hay Kilometraje Asignado para el Calculo de Rendimiento.";
                                }
                            }
                            else
                            {
                                //Mostramos Error de Kilometraje Asignado
                                lnkCalculado.Text = "No hay Kilometraje Asignado para el Calculo de Rendimiento.";
                            }
                        }
                        //Validamos que sea diferente de 0 para la Division
                        if (rendimiento > 0)
                        {
                            //Validamos Kilometros
                            if ((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(ad.id_unidad_diesel, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(ad.id_unidad_diesel, this._idAsignacionDiesel), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro())) > 0)
                            {
                                lnkCalculado.Text = "Calculado " + Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(ad.id_unidad_diesel, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(ad.id_unidad_diesel, this._idAsignacionDiesel), ad.fecha_carga) /
                                    Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                            }
                        }
                    }
                    //Deshabilitando Tipo cuando exista un registro
                    ddlTipoVale.Enabled = false;
                    txtReferencia.Text = ad.referencia;
                    ddlUnidadDiesel.SelectedValue = ad.id_unidad_diesel.ToString();
                    //Asignamos Atributo
                    this._idUnidadDiesel = ad.id_unidad_diesel;
                    //Declarando Variables Auxiliares
                    int idCarga = 0;

                    //Obteniendo Referencias
                    using (DataTable dtReferencias = Referencia.CargaReferenciasGeneral(ad.id_asignacion_diesel, 69))
                    {
                        //Validando que existen
                        if (Validacion.ValidaOrigenDatos(dtReferencias))
                        {
                            //Recorriendo Registros
                            foreach (DataRow dr in dtReferencias.Rows)
                            {
                                //Validando que sea la Referencia de "IdCarga"
                                if (dr["Tipo"].ToString().Equals("IdCarga"))

                                    //Asignando Registro de Carga
                                    idCarga = Convert.ToInt32(dr["Descripcion"]);
                            }
                        }
                    }

                    //Validando que exista 
                    if (idCarga > 0)
                    {
                        //Instanciando Carga
                        using (CargaAutoTanque carga = new CargaAutoTanque(idCarga))
                        {
                            //Validando que exista el Registro
                            if (carga.habilitar)

                                //Asignando Cantidad
                                lblCantidadDisp.Text = carga.sobrante_carga_actual.ToString();
                            else
                                //Asignando Cantidad
                                lblCantidadDisp.Text = "-";
                        }
                    }
                    else
                        //Limpiando
                        lblCantidadDisp.Text = "-";

                    /*** Configurando Controles segun el Origen del Vale ***/
                    //Validando si el Vale no viene de un Deposito
                    if (ad.id_deposito > 0)
                    
                        //Deshabilitando Litros
                        txtLitros.Enabled = false;
                    else
                        //Habilitando Litros
                        txtLitros.Enabled = true;
                }
                else
                {
                    //Inicializando Controles
                    ddlEstatus.SelectedValue = "1";
                    ddlEstatus.Enabled = false;
                    txtReferencia.Text = "";
                    //Mostrando Total
                    lblTotal.Text = "$0.00";
                    lblId.Text = "Por Asignar";
                    lblImpresion.Text = "Ninguna Impresión";
                    txtFecCarga.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    //Instanciamos Unidad
                    using (SAT_CL.Global.Unidad uni = new Unidad(Convert.ToInt32(ddlUnidadDiesel.SelectedValue)))
                    {
                        //Obtenemos rendimiento
                        decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, uni.id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                        //Si el Rendimiento No existe
                        if (rendimiento == 0)
                        {
                            //Validamos Que exista Kilometraje y  Cobustible Asignado
                            if (uni.kilometraje_asignado != 0)
                            {
                                if (uni.combustible_asignado != 0)
                                {
                                    //Calculamos rendimiento
                                    rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                                }
                                else
                                {
                                    //Mostramos Error de Kilometraje Asignado
                                    lnkCalculado.Text = "No hay Kilometraje Asignado para el Calculo de Rendimiento.";
                                }
                            }
                            else
                            {
                                //Mostramos Error de Kilometraje Asignado
                                lnkCalculado.Text = "No hay Kilometraje Asignado para el Calculo de Rendimiento.";
                            }
                        }
                        //Validamos Kilometros
                        if ((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), this._idAsignacionDiesel), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro())) > 0)
                        {
                            //Validamos que sea diferente de 0 para la Division
                            if (rendimiento > 0)
                            {
                                lnkCalculado.Text = "Calculado " + Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), this._idAsignacionDiesel), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()) / Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                            }
                        }
                        else
                        {
                            lnkCalculado.Text = "Kms Sin Asignar";
                        }
                    }
                    //Asignamos Atributo
                    this._idUnidadDiesel = Convert.ToInt32(ddlUnidadDiesel.SelectedValue);
                    ddlEstatus.SelectedValue = "1";
                    txtFecSol.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    txtLitros.Text = "0.00";
                    //Deshabilitando Tipo cuando el registro sea Nuevo
                    ddlTipoVale.Enabled = false;
                    /*/Obteniendo Fecha
                    DateTime fecha = DateTime.MinValue;
                    DateTime.TryParse(txtFecCarga.Text, out fecha);

                    //Declarando Variables Auxiliares
                    byte tipo_combustible = 0;

                    //Validando Tipo
                    if (rdbDiesel.Checked)
                        //Diesel
                        tipo_combustible = 1;
                    else if (rdbMagna.Checked)
                        //Magna
                        tipo_combustible = 2;
                    else if (rdbPremiun.Checked)
                        //Premiun
                        tipo_combustible = 3;

                    //Cargado Catalogo
                    using (DataTable dt = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(96, "", Convert.ToInt32(ddlUbicacion.SelectedValue), fecha.ToString("yyyy-MM-dd HH:mm"), 15, tipo_combustible.ToString()))
                    {
                        //Validando que existan registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                        {
                            //Recorriendo Costo
                            foreach (DataRow dr in dt.Rows)
                            {
                                //Instanciando Costo de Combustible
                                using (CostoCombustible cc = new CostoCombustible(Convert.ToInt32(dr["id"])))
                                {
                                    //Validando que exista
                                    if (cc.habilitar)

                                        //Asignando Costo de Combustible
                                        txtCostoCombustible.Text = cc.costo_combustible.ToString();
                                    else
                                        //Inicializando Costo de Combustible
                                        txtCostoCombustible.Text = "0.00";
                                }
                            }
                        }
                        else
                            //Inicializando Costo de Combustible
                            txtCostoCombustible.Text = "0.00";
                    }//*/

                    //Instanciando Carga Autotanque
                    using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(Convert.ToInt32(ddlUbicacion.SelectedValue),
                                                                                        TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                    {
                        //Validando que exista la Carga Activa
                        if (carga.habilitar)

                            //Asignando Carga Actual
                            lblCantidadDisp.Text = carga.sobrante_carga_actual.ToString();
                        else
                            //Limpiando Control
                            lblCantidadDisp.Text = "-";
                    }
                }
                //Limpiando Mensaje
                lblError.Text = "";
            }

        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 47);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoVale, "", 50);

            //Obteniendo Ubicaciones de Combustible
            using (DataTable dtUbicaciones = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(20, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                if (Validacion.ValidaOrigenDatos(dtUbicaciones))

                    Controles.CargaDropDownList(ddlUbicacion, dtUbicaciones, "id", "descripcion");
                else
                    Controles.InicializaDropDownList(ddlUbicacion, "No tiene ubicaciones configuradas");
            }

            //Cargamos Catalogo de Asignaciónes Validas
            using (DataTable mit = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaUnidadesRegistroDiesel(this._id_movimiento))
            
                //Cargamos Catalogo de Beneficiarios
                TSDK.ASP.Controles.CargaDropDownList(ddlUnidadDiesel, mit, "Id", "Recurso");
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["idAsignacionDiesel"] = this._idAsignacionDiesel;
            ViewState["idUnidad"] = this._id_unidad;
            ViewState["idOperador"] = this._id_operador;
            ViewState["idProveedorCompania"] = this._id_proveedor_compania;
            ViewState["idServicio"] = this._id_servicio;
            ViewState["idMovimiento"] = this._id_movimiento;
            ViewState["idMovimientoAsignacion"] = this._id_movimiento_asignacion;
            ViewState["idUnidadAsignacionDiesel"] = this._idUnidadDiesel;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {   //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idAsignacionDiesel"]) != 0)
                this._idAsignacionDiesel = Convert.ToInt32(ViewState["idAsignacionDiesel"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idUnidad"]) != 0)
                this._id_unidad = Convert.ToInt32(ViewState["idUnidad"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idOperador"]) != 0)
                this._id_operador = Convert.ToInt32(ViewState["idOperador"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idProveedorCompania"]) != 0)
                this._id_proveedor_compania = Convert.ToInt32(ViewState["idProveedorCompania"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["idServicio"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idMovimiento"]) != 0)
                this._id_movimiento = Convert.ToInt32(ViewState["idMovimiento"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idMovimientoAsignacion"]) != 0)
                this._id_movimiento_asignacion = Convert.ToInt32(ViewState["idMovimientoAsignacion"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idUnidadAsignacionDiesel"]) != 0)
                this._idUnidadDiesel = Convert.ToInt32(ViewState["idUnidadAsignacionDiesel"]);
        }
        /// <summary>
        /// Método encargado de Configurar el Valor Calculado
        /// </summary>
        private void configuraValorCalculado()
        {
            //Instanciamos Unidad
            using (SAT_CL.Global.Unidad uni = new Unidad(Convert.ToInt32(ddlUnidadDiesel.SelectedValue)))
            {
                //Asignamos Atributo de la Unidad
                this._idUnidadDiesel = uni.id_unidad;
                //Obtenemos rendimiento
                decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, uni.id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                //Si el Rendimiento No existe
                if (rendimiento == 0)
                {
                    //Validamos Que exista Kilometraje y  Cobustible Asignado
                    if (uni.kilometraje_asignado != 0)
                    {
                        if (uni.combustible_asignado != 0)
                        {
                            //Calculamos rendimiento
                            rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                        }
                        else
                        {
                            //Mostramos Error de Kilometraje Asignado
                            lnkCalculado.Text = "No hay Kilometra Asignado para el Calculo de Rendimiento.";
                        }
                    }
                    else
                    {
                        //Mostramos Error de Kilometraje Asignado
                        lnkCalculado.Text = "No hay Kilometra Asignado para el Calculo de Rendimiento.";
                    }
                }
                //Validamos Kilometros
                if ((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), this._idAsignacionDiesel), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro())) > 0)
                {
                    //Validamos que sea diferente de 0 para la Division
                    if (rendimiento > 0)
                    {
                        lnkCalculado.Text = "Calculado " + Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(Convert.ToInt32(ddlUnidadDiesel.SelectedValue), this._idAsignacionDiesel), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()) / Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                    }
                    else
                    {
                        lnkCalculado.Text = "Error Rendimiento ";
                    }
                }
                else
                {
                    lnkCalculado.Text = "Kms Sin Asignar";
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Script del Calendar de Fecha de Carga
        /// </summary>
        private void cargaScriptFecha()
        {
            //Declarando Script de Configuración
            string script = @"<script type='text/javascript'>
                                //Obteniendo instancia actual de la página y añadiendo manejador de evento
                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                                //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
                                function EndRequestHandler(sender, args) {
                                if (args.get_error() == undefined) {
                                ConfiguraJQueryControlFecha();
                                }
                                }
                                //Creando función para configuración de jquery en control de usuario
                                function ConfiguraJQueryControlFecha() {
                                    $(document).ready(function () {
                                        
                            //Fecha de Captura (Idioma: Español, Formato: 'dd:MM:aaaa')
                            $('#" + this.txtFecCarga.ClientID + @"').datetimepicker({
                                lang: 'es',
                                format: 'd/m/Y H:i',
                                closeOnDateSelect: true,
                                onSelectDate: function (selected, evnt) {
                                    //Mostrando ventana modal 
                                    $('#contenedorVentanaConfirmacionDiesel').animate({ width: 'toggle' });
                                    $('#ventanaConfirmacionDiesel').animate({ width: 'toggle' });
                                    $('#" + this.txtFecCarga.ClientID + @"').val(selected.format('dd/MM/yyyy HH:mm'));
                                },
                                onSelectTime: function (selected, evnt) {
                                    //Mostrando ventana modal 
                                    $('#contenedorVentanaConfirmacionDiesel').animate({ width: 'toggle' });
                                    $('#ventanaConfirmacionDiesel').animate({ width: 'toggle' });
                                    $('#" + this.txtFecCarga.ClientID + @"').val(selected.format('dd/MM/yyyy HH:mm'));
                                }
                            });
                          });
                        }
                        //Invocación Inicial de método de configuración JQuery
                        ConfiguraJQueryControlFecha();
                    </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Calendar", script, false);
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
                //Validando que exista un Vale Previo
                if (this._idAsignacionDiesel > 0)
                {
                    //Instanciando Vale de Diesel
                    using (AsignacionDiesel ad = new AsignacionDiesel(this._idAsignacionDiesel))
                    {
                        //Validando que exista el Vale
                        if (ad.habilitar && ad.id_deposito > 0)
                        {
                            //Cargando Configuración
                            script = @"<script type='text/javascript'>
                                    var validacion = function (evt) {
                                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                        var isValid1 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                        var isValid2 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                        var isValid3 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                        return isValid1 && isValid2 && isValid3;
                                    }; 
                                //Botón Buscar
                                $('#" + btnGuardar.ClientID + @"').click(validacion);
                              </script>";
                        }
                        else if (ad.habilitar && ad.id_deposito == 0)
                        {
                            //Cargando Configuración
                            script = @"<script type='text/javascript'>
                                    var validacion = function (evt) {
                                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                        var isValid1 = !$('#" + txtFecCarga.ClientID + @"').validationEngine('validate');
                                        var isValid2 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                        var isValid3 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                        var isValid4 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                        return isValid1 && isValid2 && isValid3 && isValid4;
                                    }; 
                                //Botón Buscar
                                $('#" + btnGuardar.ClientID + @"').click(validacion);
                              </script>";
                        }
                    }
                }
                else
                {
                    //Cargando Configuración
                    script = @"<script type='text/javascript'>
                                    var validacion = function (evt) {
                                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                        var isValid1 = !$('#" + txtFecCarga.ClientID + @"').validationEngine('validate');
                                        var isValid2 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                        var isValid3 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                        var isValid4 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                        return isValid1 && isValid2 && isValid3 && isValid4;
                                    }; 
                                //Botón Buscar
                                $('#" + btnGuardar.ClientID + @"').click(validacion);
                              </script>";
                }
            }
            else
            {
                //Validando que exista un Vale Previo
                if (this._idAsignacionDiesel > 0)
                {
                    //Instanciando Vale de Diesel
                    using (AsignacionDiesel ad = new AsignacionDiesel(this._idAsignacionDiesel))
                    {
                        //Validando que exista el Vale
                        if (ad.habilitar && ad.id_deposito > 0)
                        {
                            //Generamos script para validación de Fechas
                            script =
                               @"<script type='text/javascript'>
                                    var validacion= function (evt) {
                                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                        var isValid1 = !$('#" + txtOperadorProveedor.ClientID + @"').validationEngine('validate');
                                        var isValid2 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                        var isValid3 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                        var isValid4 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                        return isValid1 && isValid2 && isValid3 && isValid4;
                                    }; 

                                    //Botón Buscar
                                    $('#" + btnGuardar.ClientID + @"').click(validacion);
                                </script>";
                        }
                        else if (ad.habilitar && ad.id_deposito == 0)
                        {
                            //Generamos script para validación de Fechas
                            script =
                               @"<script type='text/javascript'>
                            var validacion= function (evt) {
                                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                var isValid1 = !$('#" + txtOperadorProveedor.ClientID + @"').validationEngine('validate');
                                var isValid2 = !$('#" + txtFecCarga.ClientID + @"').validationEngine('validate');
                                var isValid3 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                var isValid4 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                var isValid5 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                            }; 

                            //Botón Buscar
                            $('#" + btnGuardar.ClientID + @"').click(validacion);
                        </script>";
                        }
                    }
                }
                else
                {
                    //Generamos script para validación de Fechas
                    script =
                       @"<script type='text/javascript'>
                            var validacion= function (evt) {
                                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                var isValid1 = !$('#" + txtOperadorProveedor.ClientID + @"').validationEngine('validate');
                                var isValid2 = !$('#" + txtFecCarga.ClientID + @"').validationEngine('validate');
                                var isValid3 = !$('#" + txtReferencia.ClientID + @"').validationEngine('validate');
                                var isValid4 = !$('#" + txtLitros.ClientID + @"').validationEngine('validate');
                                var isValid5 = !$('#" + txtCostoCombustible.ClientID + @"').validationEngine('validate');
                                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                            }; 

                            //Botón Buscar
                            $('#" + btnGuardar.ClientID + @"').click(validacion);
                        </script>";
                }
            }

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Deposito", script, false);
        }
        /// <summary>
        /// Método encargado de Obtener el Monto Total de los Vales de Diesel
        /// </summary>
        private void obtieneTotalVale()
        {
            //Declarando Variables Auxiliares
            decimal litros = 0.00M, costo_combustible = 0.00M;

            //Obteniendo Cantidades
            litros = Convert.ToDecimal(txtLitros.Text == "" ? "0" : txtLitros.Text);
            costo_combustible = Convert.ToDecimal(txtCostoCombustible.Text == "" ? "0" : txtCostoCombustible.Text);

            //Mostrando Total de Diesel
            lblTotal.Text = string.Format("{0:C2}", litros * costo_combustible);
        }
        /// <summary>
        /// Obteniendo Tipo de Combutaible
        /// </summary>
        /// <returns></returns>
        private byte obtieneTipoCombustible()
        {
            //Declarando Variables Auxiliares
            byte tipo_combustible = 0;

            //Validando Tipo
            if (rdbDiesel.Checked)
                //Diesel
                tipo_combustible = 1;
            else if (rdbMagna.Checked)
                //Magna
                tipo_combustible = 2;
            else if (rdbPremiun.Checked)
                //Premiun
                tipo_combustible = 3;

            //Devolviendo Resultado
            return tipo_combustible;
        }
        /// <summary>
        /// Configurando Tipo de Combustible
        /// </summary>
        /// <param name="tipo_combutible"></param>
        private void configuraTipoCombustible(byte tipo_combutible)
        {
            //Validando Tipo de Combutible
            switch (tipo_combutible)
            {
                //Diesel
                case 3:
                    rdbDiesel.Checked = true;
                    rdbMagna.Checked =
                    rdbPremiun.Checked = false;
                    break;
                //Magna
                case 1:
                    rdbMagna.Checked = true;
                    rdbDiesel.Checked =
                    rdbPremiun.Checked = false;
                    break;
                //Premiun
                case 2:
                    rdbPremiun.Checked = true;
                    rdbMagna.Checked =
                    rdbDiesel.Checked = false;
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Obtener el Costo del Combustible
        /// </summary>
        private void obtieneCostoCombustible()
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFecCarga.Text, out fecha);

            //Declarando Variables Auxiliares
            byte tipo_combustible = obtieneTipoCombustible();

            //Cargado Catalogo
            using (DataTable dt = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(96, "", Convert.ToInt32(ddlUbicacion.SelectedValue), fecha.ToString("yyyy-MM-dd HH:mm"), 15, tipo_combustible.ToString()))
            {
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Recorriendo Costo
                    foreach (DataRow dr in dt.Rows)
                    {
                        //Instanciando Costo de Combustible
                        using (CostoCombustible cc = new CostoCombustible(Convert.ToInt32(dr["id"])))
                        {
                            //Validando que exista
                            if (cc.habilitar)

                                //Asignando Costo de Combustible
                                txtCostoCombustible.Text = cc.costo_combustible.ToString();
                            else
                                //Inicializando Costo de Combustible
                                txtCostoCombustible.Text = "0.00";
                        }
                    }
                }
                else
                    //Inicializando Costo de Combustible
                    txtCostoCombustible.Text = "0.00";

                //Invocando Método de Obtención
                obtieneTotalVale();
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inicializa el Control en base a un Id  Asignación de Diesel existente
        /// </summary>
        /// <param name="id_asignacion">Id de Asignación</param>
        public void InicializaControlUsuario(int id_asignacion)
        {
            //Asignando Parametros
            this._idAsignacionDiesel = id_asignacion;

            //Instanciando registro
            using (AsignacionDiesel asignacion = new AsignacionDiesel(this._idAsignacionDiesel))
            {
                this._id_unidad = asignacion.objDetalleLiquidacion.id_unidad;
                this._id_operador = asignacion.objDetalleLiquidacion.id_operador;
                this._id_proveedor_compania = asignacion.objDetalleLiquidacion.id_proveedor_compania;
                this._id_movimiento = asignacion.objDetalleLiquidacion.id_movimiento;
                this._id_servicio = asignacion.objDetalleLiquidacion.id_servicio;

                //Determinando el tipo de asignación a recuperar
                if (this._id_operador > 0)
                    this._id_movimiento_asignacion = MovimientoAsignacionRecurso.ObtieneAsignacionRecursoMovimiento(this._id_movimiento, MovimientoAsignacionRecurso.Tipo.Operador, this._id_operador).id_movimiento_asignacion_recurso;
                else if (this._id_unidad > 0)
                    this._id_movimiento_asignacion = MovimientoAsignacionRecurso.ObtieneAsignacionRecursoMovimiento(this._id_movimiento, MovimientoAsignacionRecurso.Tipo.Unidad, this._id_unidad).id_movimiento_asignacion_recurso;
                else if (this._id_proveedor_compania > 0)
                    this._id_movimiento_asignacion = MovimientoAsignacionRecurso.ObtieneAsignacionRecursoMovimiento(this._id_movimiento, MovimientoAsignacionRecurso.Tipo.Tercero, this._id_proveedor_compania).id_movimiento_asignacion_recurso;
            }

            //Invocando Método de Inicializacion
            inicializaControl();
            //Invocando Método de Carga
            obtieneCostoCombustible();
            //Activamos  Vista
            mtvBeneficiario.ActiveViewIndex = 0;
            //Validamos si el Vale es para Operador
            if (this._id_operador > 0)
            {
                //Obtenemos primer unidad 
                int id_recurso = MovimientoAsignacionRecurso.ObtienePrimerRecursoAsignado(this._id_movimiento, MovimientoAsignacionRecurso.Tipo.Unidad);

                //Validamos Recurso
                if (id_recurso > 0)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_recurso))
                    {
                        //Inicializamos Control
                        txtOperadorProveedor.Text = objUnidad.numero_unidad;
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
                if (this._id_unidad > 0)
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
                            txtOperadorProveedor.Text = objOperador.nombre;
                        }
                    }
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(this._id_unidad))
                    {
                        //Asignamos valor al control 
                        lblBeneficiario.Text = objUnidad.numero_unidad.ToString();
                    }
                }
                else
                {
                    //Instanciamos Tercero
                    using (CompaniaEmisorReceptor objCompaniaEmisorReceptor = new CompaniaEmisorReceptor(this._id_proveedor_compania))
                    {
                        //Asignamos valor al control 
                        lblBeneficiario.Text = objCompaniaEmisorReceptor.nombre;
                    }
                }
            }

        }
        /// <summary>
        /// Inicializamos Control de Usuario
        /// </summary>
        /// <param name="idServicio">Id Servicio</param>
        /// <param name="idMovimiento">Id Movimiento</param>
        public void InicializaControlUsuario(int idServicio, int idMovimiento)
        {
            //Guardando Parametros
            this._idAsignacionDiesel = 0;
            this._id_unidad = 0;
            this._id_operador = 0;
            this._id_proveedor_compania = 0;
            this._id_movimiento = idMovimiento;
            this._id_servicio = idServicio;
            this._id_movimiento_asignacion = 0;

            //Invocando Método de Inicializacion
            inicializaControl();
            //Activamos Vista
            mtvBeneficiario.ActiveViewIndex = 1;
            //Cargamos Catalogo de Asignaciónes Validas
            using (DataTable mit = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignacionesParaRegistroAnticipos(idMovimiento))
            {
                //Cargamos Catalogo de Beneficiarios
                TSDK.ASP.Controles.CargaDropDownList(ddlBeneficiario, mit, "Id", "Recurso");
            }


            //Limpiamos Texbox referencia Unidad/Operador
            txtOperadorProveedor.Text = "";
        }
        /// <summary>
        /// Método Público encargado de Inicializar el Control dado ciertos Parametros
        /// </summary>
        /// <param name="id_asignacion">Id de Asignación</param>
        /// <param name="idUnidad">Id de Unidad</param>
        /// <param name="idOperador">Id de Operador</param>
        /// <param name="idProveedorCompania">Compania Proveedora</param>
        /// <param name="idServicio">Id de Servicio</param>
        /// <param name="idMovimiento">Id de Movimiento</param>
        /// <param name="idMovimientoAsignacion">Id movimiento Asignación</param>
        public void InicializaControlUsuario(int id_asignacion, int idUnidad, int idOperador, int idProveedorCompania, int idServicio, int idMovimiento, int idMovimientoAsignacion)
        {
            //Guardando Parametros
            this._idAsignacionDiesel = id_asignacion;
            this._id_unidad = idUnidad;
            this._id_operador = idOperador;
            this._id_proveedor_compania = idProveedorCompania;
            this._id_movimiento = idMovimiento;
            this._id_servicio = idServicio;
            this._id_movimiento_asignacion = idMovimientoAsignacion;

            //Invocando Método de Inicializacion
            inicializaControl();
            //Activamos  Vista
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
                        //Inicializamos Control
                        txtOperadorProveedor.Text = objUnidad.numero_unidad;
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
                            //Inicializamos Control
                            txtOperadorProveedor.Text = objOperador.nombre;
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
                    }
                }
            }
        }
        /// <summary>
        /// Método Público encargado de Guardar el Vale de Diesel.
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaDiesel()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo Fecha
            DateTime FecCarga;
            DateTime.TryParse(txtFecCarga.Text, out FecCarga);

            //Obtiene Tipo Combustible
            byte tipo_combustible = 0;

            //Declarando Variable Auxiliar
            int id_AsignacionDiesel = 0;

            //Declarando Variables Auxiliares
            decimal litros = 0;

            //Obteniendo Litros
            litros = Convert.ToDecimal(txtLitros.Text == "" ? "0" : txtLitros.Text);

            //Validando que exista el Costo de Diesel 
            if (!txtCostoCombustible.Text.Equals("0"))
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando Asignación
                    using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(this._idAsignacionDiesel))
                    {
                        //Validamos que exista una Asignación Recurso
                        if (this._id_movimiento_asignacion > 0)
                        {
                            //Validando que exista una Asignación
                            if (ad.habilitar)
                            {
                                //Validando que el vale haya sido impreso
                                if (ad.conteo_impresion == 0)
                                {
                                    //Editando Asignación
                                    result = ad.EditaAsignacionDiesel(txtOperadorProveedor.Text.ToUpper(), ad.id_compania_emisor, Convert.ToInt32(ddlUbicacion.SelectedValue),
                                                        ad.fecha_solicitud, FecCarga, ad.id_costo_diesel, obtieneTipoCombustible(), 0, false, txtReferencia.Text.ToUpper(), ad.id_lectura,
                                                        ad.id_deposito, Convert.ToByte(ddlTipoVale.SelectedValue), litros, Convert.ToDecimal(txtCostoCombustible.Text), 
                                                        this._id_unidad, this._id_operador, this._id_proveedor_compania, this._id_servicio, this._id_movimiento, 
                                                        this.idUnidadDiesel, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Diesel
                                        id_AsignacionDiesel = result.IdRegistro;

                                        //Declarando Variables Auxiliares
                                        int idCarga = 0;

                                        //Obteniendo Referencias
                                        using (DataTable dtReferencias = Referencia.CargaReferenciasGeneral(id_AsignacionDiesel, 69))
                                        {
                                            //Validando que existen
                                            if (Validacion.ValidaOrigenDatos(dtReferencias))
                                            {
                                                //Recorriendo Registros
                                                foreach (DataRow dr in dtReferencias.Rows)
                                                {
                                                    //Validando que sea la Referencia de "IdCarga"
                                                    if (dr["Tipo"].ToString().Equals("IdCarga"))

                                                        //Asignando Registro de Carga
                                                        idCarga = Convert.ToInt32(dr["Descripcion"]);
                                                }
                                            }
                                        }

                                        //Validando que existen las Referencias Especificas
                                        if (idCarga > 0)
                                        {
                                            //Obteniendo Carga Autotanque Actva
                                            using (CargaAutoTanque carga = new CargaAutoTanque(idCarga))
                                            {
                                                //Validando que exista una carga
                                                if (carga.habilitar)
                                                {
                                                    //Actualizando Sobrante de Carga Actual
                                                    result = carga.ActualizaSobranteCargaActual((carga.sobrante_carga_actual + ad.objDetalleLiquidacion.cantidad) - litros,
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Vale de Diesel
                                            result = new RetornoOperacion(id_AsignacionDiesel);
                                    }
                                }
                                else
                                    //Instanciando Excepcion
                                    result = new RetornoOperacion("El Vale ha sido impreso. Imposible su Edición");
                            }
                            else
                            {
                                //Validando Estación de Combustible
                                if (Convert.ToInt32(ddlUbicacion.SelectedValue) > 0)
                                {
                                    //Insertando Asignación
                                    result = SAT_CL.EgresoServicio.AsignacionDiesel.InsertaAsignacionDiesel(txtOperadorProveedor.Text.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0,
                                                                                        Convert.ToInt32(ddlUbicacion.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), FecCarga,
                                                                                        0, obtieneTipoCombustible(), 0, false, txtReferencia.Text.ToUpper(), 0, 0, (byte)AsignacionDiesel.TipoVale.Original,
                                                                                        Convert.ToDecimal(txtLitros.Text), Convert.ToDecimal(txtCostoCombustible.Text), this._id_unidad,
                                                                                        this.idUnidadDiesel, this._id_operador, this._id_proveedor_compania, this._id_servicio,
                                                                                        this._id_movimiento_asignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo Asignación
                                        id_AsignacionDiesel = result.IdRegistro;

                                        //Obteniendo Carga Autotanque Actva
                                        using (CargaAutoTanque carga = CargaAutoTanque.ObtieneCargaAutoTanqueActiva(Convert.ToInt32(ddlUbicacion.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro()))
                                        {
                                            //Validando que exista una carga
                                            if (carga.habilitar)
                                            {
                                                //Instanciando Detalle de Liquidación
                                                using (DetalleLiquidacion dlDiesel = new DetalleLiquidacion(id_AsignacionDiesel, 69))
                                                {
                                                    //Validando que exista
                                                    if (dlDiesel.habilitar)
                                                    {
                                                        //Insertando Id de Carga como Referencia
                                                        result = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "IdCarga", 0, "Carga AutoTanque"),
                                                                    carga.id_carga_autotanque.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);

                                                        //Operación Exitosa
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Insertando Id de Carga como Referencia
                                                            result = Referencia.InsertaReferencia(id_AsignacionDiesel, 69, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 69, "SobranteDiesel", 0, "Carga AutoTanque"),
                                                                        carga.sobrante_carga_actual.ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);

                                                            //Validando Operación Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualiza Sobrante de la Carga Actual (Carga actual - litros asignados)
                                                                result = carga.ActualizaSobranteCargaActual(carga.sobrante_carga_actual - dlDiesel.cantidad, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("No existe el Vale de Diesel");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    //Mostramos Mensaje Error
                                    result = new RetornoOperacion("Seleccione una Estación de Combustible");
                            }
                        }
                        else
                            //Mostramos Mensaje Error
                            result = new RetornoOperacion("Seleccione el Beneficiario");
                    }

                    //Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Asignación Diesel
                        result = new RetornoOperacion(id_AsignacionDiesel);

                        //Completando Transacción
                        trans.Complete();
                    }
                }

                //Validando que la Operación sea Exitosa
                if (result.OperacionExitosa)
                {
                    //Asignando Valor al Atributo
                    this._idAsignacionDiesel = result.IdRegistro;

                    //Inicializando Control
                    inicializaControl();
                }
            }
            else
                //Instanciando Excepcion
                result = new RetornoOperacion("Debe de Existir un Costo de Diesel");

            //Mostrando Mensaje
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}