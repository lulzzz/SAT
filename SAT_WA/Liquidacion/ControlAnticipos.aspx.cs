using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class ControlAnticipos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)
                //Inicializando Página
                inicializaForma();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargaControlAnticipos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtProveedor_TextChanged(object sender, EventArgs e)
        {
            cargaControlAnticipos();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccionesAnticipos_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.CommandName)
            {
                case "Registrar":
                    {
                        RetornoOperacion retorno = new RetornoOperacion();
                        if (gvViajesAnticipos.SelectedIndex != -1)
                        {
                            foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Select("Id = " + gvViajesAnticipos.SelectedDataKey["Id"].ToString()))
                            {
                                //Obteniendo Montos por Evaluar
                                int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
                                byte tipoAnticipo = btn.CommandArgument.ToUpper().Equals("CC") ? (byte)1 : (byte)2;
                                decimal monto_ant = 0.00M, ant_auto = 0.00M;
                                decimal.TryParse(txtMontoAnt.Text, out monto_ant);
                                
                                //Validando Monto del tipo del Anticipo
                                if (tipoAnticipo == 1)
                                    decimal.TryParse(dr["AnticipoAutoCC"].ToString(), out ant_auto);
                                else if (tipoAnticipo == 2)
                                    decimal.TryParse(dr["AnticipoAutoSC"].ToString(), out ant_auto);
                                
                                //Validando monto
                                if (monto_ant > 0)
                                {
                                    if (monto_ant <= ant_auto)
                                    {
                                        //Obteniendo Datos de Interes
                                        String[] separador_serv_imp = { "|" }, separador_servicio_dep = { ":" };
                                        string[] servicio = dr["ServImps"].ToString().Split(separador_serv_imp, StringSplitOptions.None);
                                        if (servicio.Length > 0)
                                        {
                                            //Obteniendo Servicios y Servicios de Importación
                                            List<Tuple<int, int>> servicios_imp = new List<Tuple<int, int>>();
                                            foreach (string serv in servicio)

                                                servicios_imp.Add(new Tuple<int, int>
                                                    (Convert.ToInt32(Cadena.RegresaCadenaSeparada(serv, ":", 0, "0")), //Servicio Importacion
                                                     Convert.ToInt32(Cadena.RegresaCadenaSeparada(serv, ":", 1, "0")))); //Primer Servicio de la Importación

                                            if (servicios_imp.Count > 0)
                                            {
                                                //Obteniendo Primer importación
                                                using (SAT_CL.Documentacion.ServicioImportacion si = new SAT_CL.Documentacion.ServicioImportacion(servicios_imp[0].Item1))
                                                using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(si.id_servicio_maestro))
                                                using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(servicios_imp[0].Item2, 1))
                                                using (SAT_CL.TarifasPago.TarifaProveedor tar_prov = new SAT_CL.TarifasPago.TarifaProveedor(Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"])))
                                                {
                                                    if (si.habilitar && maestro.habilitar && mov.habilitar && tar_prov.habilitar)
                                                    {
                                                        //Obteniendo Concepto de Anticipo a Proveedor
                                                        int idConceptoAnticipoProveedor = SAT_CL.EgresoServicio.ConceptoDeposito.ObtieneConcepto("Anticipo Proveedor", si.id_compania);
                                                        if (idConceptoAnticipoProveedor > 0)
                                                        {
                                                            int idConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(idConceptoAnticipoProveedor);
                                                            //Obteniendo Asignación del Recurso
                                                            using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionRecursoMovimiento(mov.id_movimiento, SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Tercero, si.id_transportista))
                                                            {
                                                                if (mar.habilitar && mar.EstatusMovimientoAsignacion != SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus.Liquidado)
                                                                {
                                                                    //Inicializando Bloque Transaccional
                                                                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                    {
                                                                        //Insertando Deposito
                                                                        retorno = SAT_CL.EgresoServicio.Deposito.InsertaDeposito(si.id_compania, string.Format("{0}:{1}", tipoAnticipo == 1 ? "CC" : "SC", tipoAnticipo == 1 ? dr["CC"] : dr["SC"]),
                                                                                        maestro.id_cliente_receptor, idConceptoAnticipoProveedor, 0, 0,
                                                                                        string.Format("{0} - '{1}' Total de Viajes del día {2:dd/MM/yyyy} - {3}", dr["Maestro"], dr["TotalViajes"], Convert.ToDateTime(dr["FechaOperacion"]), txtReferenciaDep.Text),
                                                                                        SAT_CL.EgresoServicio.Deposito.TipoCargo.Depositante, false, idConceptoRestriccion,
                                                                                        0, 0, si.id_transportista, mov.id_servicio, mar.id_movimiento_asignacion_recurso, Convert.ToDecimal(txtMontoAnt.Text),
                                                                                        idUsuario);
                                                                        if (retorno.OperacionExitosa)
                                                                        {
                                                                            int idDeposito = retorno.IdRegistro;
                                                                            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(idDeposito))
                                                                            {
                                                                                if (dep.habilitar)
                                                                                {
                                                                                    retorno = dep.SolicitarDeposito(idConceptoRestriccion, idUsuario);
                                                                                    if (retorno.OperacionExitosa)
                                                                                    {
                                                                                        using (DataTable dtDetImps = SAT_CL.Documentacion.ServicioImportacionDetalle.ObtieneDetallesImportacion(si.id_compania, si.id_transportista, si.id_servicio_maestro, Convert.ToDateTime(dr["FechaOperacion"])))
                                                                                        {
                                                                                            if (Validacion.ValidaOrigenDatos(dtDetImps))
                                                                                            {
                                                                                                foreach (DataRow det in dtDetImps.Rows)
                                                                                                {
                                                                                                    if (retorno.OperacionExitosa)
                                                                                                        retorno = SAT_CL.EgresoServicio.ServicioImportacionAnticipos.InsertaServicioImportacionAnticipos
                                                                                                                    (Convert.ToInt32(det["Id"]), idDeposito, tipoAnticipo, tar_prov.monto_cc, tar_prov.monto_sc, 0, 0, idUsuario);
                                                                                                    else
                                                                                                        break;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                    retorno = new RetornoOperacion("No se puede recuperar el Anticipo");

                                                                                if (retorno.OperacionExitosa)
                                                                                {
                                                                                    retorno = new RetornoOperacion(dep.id_deposito, string.Format("Anticipo '{0}' Registrado Exitosamente", dep.no_deposito), true);
                                                                                    scope.Complete();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    retorno = new RetornoOperacion("La Asignación se encuentra Liquidada");
                                                            }
                                                        }
                                                        else
                                                            retorno = new RetornoOperacion("No se puede recuperar el Concepto 'Anticipo de Proveedor'");
                                                    }
                                                    else
                                                        retorno = new RetornoOperacion("No se puede recuperar la primer importación");
                                                }
                                            }
                                        }
                                    }
                                    else
                                        retorno = new RetornoOperacion(string.Format("El monto '{0}' excede la cantidad permitida de '{1}'", monto_ant, ant_auto));
                                }
                            }
                        }
                        else
                            retorno = new RetornoOperacion("No selección de Viajes");

                        if (retorno.OperacionExitosa)
                        {
                            cargaControlAnticipos();
                            //Mostrando Ventana Modal
                            gestionaVentanaModal(this.Page, "AnticipoAutorizado");
                        }

                        ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Cancelar":
                    {
                        //Mostrando Ventana Modal
                        gestionaVentanaModal(this.Page, "AnticipoAutorizado");
                        Controles.InicializaIndices(gvViajesAnticipos);
                        break;
                    }
                case "RegistrarFiniquito":
                    {
                        RetornoOperacion retorno = new RetornoOperacion();
                        if (gvViajesAnticipos.SelectedIndex != -1)
                        {
                            foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Select("Id = " + gvViajesAnticipos.SelectedDataKey["Id"].ToString()))
                            {
                                //Obteniendo Montos por Evaluar
                                decimal monto_fin = 0.00M, fin_auto = 0.00M;
                                decimal.TryParse(txtMontoFiniquito.Text, out monto_fin);
                                decimal.TryParse(dr["FiniquitoAuto"].ToString(), out fin_auto);
                                int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
                                //Validando monto
                                if (monto_fin > 0)
                                {
                                    if (monto_fin <= fin_auto)
                                    {
                                        //Obteniendo Datos de Interes
                                        List<int> serv_anticipos = new List<int>();
                                        String[] separador_serv_anticipos = { "|" }, separador_serv_imp = { "|" }, separador_servicio_dep = { ":" };
                                        string[] serv_imp_anticipos = dr["ServImpsAnt"].ToString().Split(separador_serv_anticipos, StringSplitOptions.None);
                                        string[] servicio = dr["ServImps"].ToString().Split(separador_serv_imp, StringSplitOptions.None);

                                        if (!dr["ServImpsAnt"].ToString().Equals(""))
                                        {
                                            if (serv_imp_anticipos.Length > 0)
                                            {
                                                //Obteniendo Anticipos registrados previamente
                                                List<string> anticipos_ids = (from string ant in serv_imp_anticipos
                                                                              select Cadena.RegresaCadenaSeparada(ant, "&", 1)).ToList();
                                                if (anticipos_ids.Count > 0)
                                                {
                                                    foreach (string ants in anticipos_ids)
                                                    {
                                                        string[] ant_imp = ants.Split(separador_servicio_dep, StringSplitOptions.None);
                                                        if (ant_imp.Length > 0)
                                                        {
                                                            foreach (string ant in ant_imp)
                                                                serv_anticipos.Add(Convert.ToInt32(ant));
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //Obteniendo Servicios y Servicios de Importación
                                        List<Tuple<int, int>> servicios_imp = new List<Tuple<int, int>>();
                                        foreach (string serv in servicio)
                                            servicios_imp.Add(new Tuple<int, int>
                                                (Convert.ToInt32(Cadena.RegresaCadenaSeparada(serv, ":", 0, "0")), //Servicio Importacion
                                                 Convert.ToInt32(Cadena.RegresaCadenaSeparada(serv, ":", 1, "0")))); //Primer Servicio de la Importación

                                        if (servicios_imp.Count > 0)
                                        {
                                            //Obteniendo Primer importación
                                            using (SAT_CL.Documentacion.ServicioImportacion si = new SAT_CL.Documentacion.ServicioImportacion(servicios_imp[0].Item1))
                                            using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(si.id_servicio_maestro))
                                            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(servicios_imp[0].Item2, 1))
                                            using (SAT_CL.TarifasPago.TarifaProveedor tar_prov = new SAT_CL.TarifasPago.TarifaProveedor(Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"])))
                                            {
                                                if (si.habilitar && maestro.habilitar && mov.habilitar && tar_prov.habilitar)
                                                {
                                                    //Obteniendo Concepto de Anticipo a Proveedor
                                                    int idConceptoFiniquitoProveedor = SAT_CL.EgresoServicio.ConceptoDeposito.ObtieneConcepto("Finiquito Proveedor", si.id_compania);
                                                    if (idConceptoFiniquitoProveedor > 0)
                                                    {
                                                        int idConceptoRestriccion = SAT_CL.EgresoServicio.ConceptoRestriccion.ObtieneConceptoRestriccionConcepto(idConceptoFiniquitoProveedor);
                                                        //Obteniendo Asignación del Recurso
                                                        using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionRecursoMovimiento(mov.id_movimiento, SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Tercero, si.id_transportista))
                                                        {
                                                            if (mar.habilitar && mar.EstatusMovimientoAsignacion != SAT_CL.Despacho.MovimientoAsignacionRecurso.Estatus.Liquidado)
                                                            {
                                                                //Inicializando Bloque Transaccional
                                                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                {
                                                                    //Insertando Deposito
                                                                    retorno = SAT_CL.EgresoServicio.Deposito.InsertaDeposito(si.id_compania, string.Format("SC:{0} CC:{1}", dr["SC"], dr["CC"]),
                                                                                    maestro.id_cliente_receptor, idConceptoFiniquitoProveedor, 0, 0,
                                                                                    string.Format("{0} - '{1}' Total de Viajes del día {2:dd/MM/yyyy} - {3}", dr["Maestro"], dr["TotalViajes"], Convert.ToDateTime(dr["FechaOperacion"]), txtReferenciaFin.Text),
                                                                                    SAT_CL.EgresoServicio.Deposito.TipoCargo.Depositante, false, idConceptoRestriccion, 0, 0, 
                                                                                    si.id_transportista, mov.id_servicio, mar.id_movimiento_asignacion_recurso, monto_fin, idUsuario);
                                                                    if (retorno.OperacionExitosa)
                                                                    {
                                                                        int idFiniquito = retorno.IdRegistro;
                                                                        using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(idFiniquito))
                                                                        {
                                                                            if (dep.habilitar)
                                                                            {
                                                                                retorno = dep.SolicitarDeposito(idConceptoRestriccion, idUsuario);
                                                                                if (retorno.OperacionExitosa)
                                                                                {
                                                                                    if (serv_anticipos.Count > 0)
                                                                                    {
                                                                                        //Inicializando Ciclo de Insercción en "Servicio Importación Anticipo"
                                                                                        foreach (int ant_imp in serv_anticipos)
                                                                                        {
                                                                                            if (retorno.OperacionExitosa)
                                                                                            {
                                                                                                using (SAT_CL.EgresoServicio.ServicioImportacionAnticipos sia = new SAT_CL.EgresoServicio.ServicioImportacionAnticipos(ant_imp))
                                                                                                {
                                                                                                    if (sia.habilitar)
                                                                                                    {
                                                                                                        if (sia.id_anticipo_finiquito == 0)
                                                                                                            retorno = sia.ActualizaFiniquitoServicioImportacionAnticipos(idFiniquito, idUsuario);
                                                                                                        else
                                                                                                            retorno = SAT_CL.EgresoServicio.ServicioImportacionAnticipos.InsertaServicioImportacionAnticipos(
                                                                                                                sia.id_servicio_importacion_detalle, 0, 0, tar_prov.monto_cc, tar_prov.monto_sc, idFiniquito, 0, idUsuario);
                                                                                                    }
                                                                                                    else
                                                                                                        retorno = new RetornoOperacion("No se puede recuperar el Anticipo de las Importaciones");
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                                break;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        using (DataTable dtDetImps = SAT_CL.Documentacion.ServicioImportacionDetalle.ObtieneDetallesImportacion(si.id_compania, si.id_transportista, si.id_servicio_maestro, Convert.ToDateTime(dr["FechaOperacion"])))
                                                                                        {
                                                                                            if (Validacion.ValidaOrigenDatos(dtDetImps))
                                                                                            {
                                                                                                foreach (DataRow det in dtDetImps.Rows)
                                                                                                {
                                                                                                    if (retorno.OperacionExitosa)
                                                                                                        retorno = SAT_CL.EgresoServicio.ServicioImportacionAnticipos.InsertaServicioImportacionAnticipos
                                                                                                                    (Convert.ToInt32(det["Id"]), 0, 0, tar_prov.monto_cc, tar_prov.monto_sc, idFiniquito, 0, idUsuario);
                                                                                                    else
                                                                                                        break;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                retorno = new RetornoOperacion("No se puede recuperar el Anticipo");

                                                                            if (retorno.OperacionExitosa)
                                                                            {
                                                                                retorno = new RetornoOperacion(dep.id_deposito, string.Format("Finiquito '{0}' Registrado Exitosamente", dep.no_deposito), true);
                                                                                scope.Complete();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                retorno = new RetornoOperacion("La Asignación se encuentra Liquidada");
                                                        }
                                                    }
                                                    else
                                                        retorno = new RetornoOperacion("No se puede recuperar el Concepto 'Anticipo de Proveedor'");
                                                }
                                                else
                                                    retorno = new RetornoOperacion("No se puede recuperar la primer importación");
                                            }
                                        }

                                    }
                                    else
                                        retorno = new RetornoOperacion(string.Format("El monto '{0}' excede la cantidad permitida de '{1}'", monto_fin, fin_auto));
                                }
                            }
                        }
                        else
                            retorno = new RetornoOperacion("No selección de Viajes");

                        if (retorno.OperacionExitosa)
                        {
                            cargaControlAnticipos();
                            //Mostrando Ventana Modal
                            gestionaVentanaModal(this.Page, "FiniquitoAutorizado");
                        }

                        ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "CancelarFiniquito":
                    {
                        //Mostrando Ventana Modal
                        gestionaVentanaModal(this.Page, "FiniquitoAutorizado");
                        Controles.InicializaIndices(gvViajesAnticipos);
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            switch (((LinkButton)sender).CommandName)
            {
                case "ControlAnticipos":
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id-IdServicioM-IdTarifaProveedor-ServImps-ServImpsAnt-Depositos");
                        break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            LinkButton lnkCerrar = (LinkButton)sender;
            switch (lnkCerrar.CommandName)
            {
                default:
                    gestionaVentanaModal(lnkCerrar, lnkCerrar.CommandName);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarTarifa_Click(object sender, EventArgs e)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
            int idProveedor = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1, "0"));
            using (SAT_CL.TarifasPago.TarifaProveedor tp = new SAT_CL.TarifasPago.TarifaProveedor(Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"])))
            {
                if (tp.habilitar)
                {
                    if (idProveedor > 0)
                        retorno = tp.ActualizaMontosTarifa(Convert.ToDecimal(txtCostoCC.Text), Convert.ToDecimal(txtCostoSC.Text), idUsuario);
                    else
                        retorno = new RetornoOperacion("Proveedor no encontrado");
                }
                else if (Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"]) == 0)

                    retorno = SAT_CL.TarifasPago.TarifaProveedor.InsertaTarifaProveedor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                idProveedor, Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdServicioM"]), Convert.ToDecimal(txtCostoCC.Text), Convert.ToDecimal(txtCostoSC.Text), idUsuario);
                else
                    retorno = new RetornoOperacion("No se puede recuperar la Tarifa del Proveedor");

                if (retorno.OperacionExitosa)
                {
                    cargaControlAnticipos();
                    retorno = new RetornoOperacion(retorno.IdRegistro, "La Tarifa se actualizo correctamente", true);
                    //Mostrando Ventana
                    gestionaVentanaModal(this.Page, "TarifaProveedor");
                }

                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #region Eventos GridView "Control Anticipos"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvViajesAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajesAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Validando Fila Obtenida
                if (row != null)
                {
                    if (row["AnticipoAutoCC"] != null)
                    {
                        //Obteniendo Controles
                        using (LinkButton lkbAnticipoAutorizadoCC = (LinkButton)e.Row.FindControl("lkbAnticipoAutorizadoCC"))
                        using (Label lblAnticipoAutorizadoCC = (Label)e.Row.FindControl("lblAnticipoAutorizadoCC"))
                        {
                            if (lkbAnticipoAutorizadoCC != null && lblAnticipoAutorizadoCC != null)
                            {
                                decimal ant_auto = 0.00M;
                                decimal.TryParse(row["AnticipoAutoCC"].ToString(), out ant_auto);
                                if (ant_auto > 0)
                                {
                                    lkbAnticipoAutorizadoCC.Visible = true;
                                    lblAnticipoAutorizadoCC.Visible = false;
                                }
                                else
                                {
                                    lkbAnticipoAutorizadoCC.Visible = false;
                                    lblAnticipoAutorizadoCC.Visible = true;
                                }
                            }
                        }
                    }

                    if (row["AnticipoAutoSC"] != null)
                    {
                        //Obteniendo Controles
                        using (LinkButton lkbAnticipoAutorizadoSC = (LinkButton)e.Row.FindControl("lkbAnticipoAutorizadoSC"))
                        using (Label lblAnticipoAutorizadoSC = (Label)e.Row.FindControl("lblAnticipoAutorizadoSC"))
                        {
                            if (lkbAnticipoAutorizadoSC != null && lblAnticipoAutorizadoSC != null)
                            {
                                decimal ant_auto = 0.00M;
                                decimal.TryParse(row["AnticipoAutoSC"].ToString(), out ant_auto);
                                if (ant_auto > 0)
                                {
                                    lkbAnticipoAutorizadoSC.Visible = true;
                                    lblAnticipoAutorizadoSC.Visible = false;
                                }
                                else
                                {
                                    lkbAnticipoAutorizadoSC.Visible = false;
                                    lblAnticipoAutorizadoSC.Visible = true;
                                }
                            }
                        }
                    }

                    if (row["FiniquitoAuto"] != null)
                    {
                        //Obteniendo Controles
                        using (LinkButton lkbFiniquitoAutorizado = (LinkButton)e.Row.FindControl("lkbFiniquitoAutorizado"))
                        using (Label lblFiniquitoAutorizado = (Label)e.Row.FindControl("lblFiniquitoAutorizado"))
                        {
                            if (lkbFiniquitoAutorizado != null && lblFiniquitoAutorizado != null)
                            {
                                decimal fin_auto = 0.00M;
                                decimal.TryParse(row["FiniquitoAuto"].ToString(), out fin_auto);
                                if (fin_auto > 0)
                                {
                                    lkbFiniquitoAutorizado.Visible = true;
                                    lblFiniquitoAutorizado.Visible = false;
                                }
                                else
                                {
                                    lkbFiniquitoAutorizado.Visible = false;
                                    lblFiniquitoAutorizado.Visible = true;
                                }
                            }
                        }
                    }

                    if (row["Depositos"] != null)
                    {
                        //Obteniendo Controles
                        using (ImageButton imbDepositos = (ImageButton)e.Row.FindControl("imbDepositos"))
                        {
                            if (imbDepositos != null)
                            {
                                if (!string.IsNullOrEmpty(row["Depositos"].ToString()))

                                    imbDepositos.Visible = true;
                                else
                                    imbDepositos.Visible = false;
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
        protected void gvViajesAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvViajesAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajesAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvViajesAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionesAnticipos_Click(object sender, EventArgs e)
        {
            if (gvViajesAnticipos.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvViajesAnticipos, sender, "lnk", false);
                int idProveedor = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1, "0"));
                LinkButton lkb = (LinkButton)sender;
                if (lkb != null)
                {
                    switch (lkb.CommandName)
                    {
                        case "TarifasProveedor":
                            {
                                using (SAT_CL.TarifasPago.TarifaProveedor tp = new SAT_CL.TarifasPago.TarifaProveedor(Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"])))
                                {
                                    if (tp.habilitar)
                                    {
                                        using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(tp.id_proveedor))
                                        using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(tp.id_servicio_maestro))
                                        {
                                            if (proveedor.habilitar && maestro.habilitar)
                                            {
                                                if (proveedor.id_compania_emisor_receptor == idProveedor)
                                                {
                                                    lblProveedor.Text = proveedor.nombre;
                                                    lblServicioMaestro.Text = SAT_CL.Global.Referencia.CargaReferencia(maestro.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Identificador", 0, "Servicio Maestro"));
                                                    txtCostoCC.Text = string.Format("{0:0.00}", tp.monto_cc);
                                                    txtCostoSC.Text = string.Format("{0:0.00}", tp.monto_sc);
                                                    //Mostrando Ventana
                                                    gestionaVentanaModal(this.Page, "TarifaProveedor");
                                                }
                                                else
                                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No coincide el Proveedor de la Tarifa con el Ingesado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                            else
                                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Proveedor o el Servicio Maestro"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        }
                                    }
                                    else if (Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdTarifaProveedor"]) == 0)
                                    {
                                        using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(idProveedor))
                                        using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvViajesAnticipos.SelectedDataKey["IdServicioM"])))
                                        {
                                            if (proveedor.habilitar && maestro.habilitar)
                                            {
                                                lblProveedor.Text = proveedor.nombre;
                                                lblServicioMaestro.Text = SAT_CL.Global.Referencia.CargaReferencia(maestro.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Identificador", 0, "Servicio Maestro"));
                                                txtCostoCC.Text = string.Format("{0:0.00}", tp.monto_cc);
                                                txtCostoSC.Text = string.Format("{0:0.00}", tp.monto_sc);
                                                //Mostrando Ventana
                                                gestionaVentanaModal(this.Page, "TarifaProveedor");
                                            }
                                            else
                                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No coincide el Proveedor de la Tarifa con el Ingesado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        }
                                    }
                                    else
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la Tarifa del Proveedor"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                break;
                            }
                        case "AnticipoAutorizadoSC":
                        case "AnticipoAutorizadoCC":
                            {
                                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Select("Id = " + gvViajesAnticipos.SelectedDataKey["Id"].ToString()))
                                {
                                    string tipo = lkb.CommandName.Contains("CC") ? "CC" : lkb.CommandName.Contains("SC") ? "SC" : "";
                                    decimal ant_auto = 0.00M, monto_permitido = 0.00M, deps_previos = 0.00M;

                                    //Validando Monto del tipo del Anticipo
                                    if (tipo.Equals("CC"))
                                    {
                                        decimal.TryParse(dr["AnticipoAutoCC"].ToString(), out ant_auto);
                                        decimal.TryParse(dr["PermitidoCC"].ToString(), out monto_permitido);
                                        decimal.TryParse(dr["DepsCC"].ToString(), out deps_previos);
                                    }
                                    else if (tipo.Equals("SC"))
                                    {
                                        decimal.TryParse(dr["AnticipoAutoSC"].ToString(), out ant_auto);
                                        decimal.TryParse(dr["PermitidoSC"].ToString(), out monto_permitido);
                                        decimal.TryParse(dr["DepsSC"].ToString(), out deps_previos);
                                    }
                                    if (ant_auto > 0 && monto_permitido > 0)
                                    {
                                        //Inicializando Control
                                        lblConceptoAnt.Text = "ANTICIPO PROVEEDOR " + tipo;
                                        lblFechaViajes.Text = string.Format("{0:dd/MM/yyyy}", dr["FechaOperacion"]);
                                        lblTotalViajes.Text = string.Format("{0}", dr["TotalViajes"]);
                                        lblMontoPer.Text = string.Format("{0:C2}", monto_permitido);
                                        lblDepPrevios.Text = string.Format("{0:C2}", deps_previos);
                                        txtMontoAnt.Text = string.Format("{0:0.00}", ant_auto);
                                        txtReferenciaDep.Text = "";
                                        //Validando Tipo (CC/SC)
                                        btnRegistrarDep.CommandArgument = lkb.CommandName.Contains("CC") ? "CC" : lkb.CommandName.Contains("SC") ? "SC" : "";
                                        //Mostrando Ventana Modal
                                        gestionaVentanaModal(this.Page, "AnticipoAutorizado");
                                    }
                                    else
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No tiene un monto permitido para anticipar"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                break;
                            }
                        case "FiniquitoAutorizado":
                            {
                                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Select("Id = " + gvViajesAnticipos.SelectedDataKey["Id"].ToString()))
                                {
                                    decimal ant_auto = 0.00M;
                                    decimal.TryParse(dr["FiniquitoAuto"].ToString(), out ant_auto);
                                    if (ant_auto > 0)
                                    {
                                        //Inicializando Control
                                        lblConceptoFiniquito.Text = "FINIQUITO A PROVEEDOR";
                                        lblFechaVFin.Text = string.Format("{0:dd/MM/yyyy}", dr["FechaOperacion"]);
                                        lblTotalVFin.Text = string.Format("{0}", dr["TotalViajes"]);
                                        lblMontoTFin.Text = string.Format("{0:C2}", dr["AdeudoTotal"]);
                                        lblMontoPerFin.Text = string.Format("{0:C2}", dr["FiniquitoAuto"]);
                                        txtMontoFiniquito.Text = string.Format("{0:0.00}", dr["FiniquitoAuto"]);
                                        txtReferenciaFin.Text = "";
                                        //Mostrando Ventana Modal
                                        gestionaVentanaModal(this.Page, "FiniquitoAutorizado");
                                    }
                                    else
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No tiene un monto permitido para anticipar"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                break;
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
        protected void imbDepositos_Click(object sender, ImageClickEventArgs e)
        {
            if (gvViajesAnticipos.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvViajesAnticipos, sender, "imb", false);

                foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Select("Id = " + gvViajesAnticipos.SelectedDataKey["Id"].ToString()))
                {
                    using (DataTable dtDepositosCxP = SAT_CL.EgresoServicio.ServicioImportacionAnticipos.ObtieneAnticiposImportaciones
                        (((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)), Convert.ToInt32(dr["IdServicioM"]), Convert.ToDateTime(dr["FechaOperacion"]), dr["Depositos"].ToString()))
                    {
                        if (Validacion.ValidaOrigenDatos(dtDepositosCxP))
                        {
                            Controles.CargaGridView(gvAnticiposCxP, dtDepositosCxP, "IdDeposito", "", true, 3);
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDepositosCxP, "Table1");
                        }
                        else
                        {
                            Controles.InicializaGridview(gvAnticiposCxP);
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        }

                        //Mostrando Ventana Modal
                        gestionaVentanaModal(this.Page, "DepositosCxP");
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbFacturaCxP_Click(object sender, ImageClickEventArgs e)
        {
            if (gvAnticiposCxP.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvAnticiposCxP, sender, "imb", false);
                //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla Usuario
                string url = Cadena.RutaRelativaAAbsoluta("~/Liquidacion/ControlAnticipos.aspx", "~/Accesorios/ServicioFacturas.aspx?idRegistro=51&idRegistroB=" + gvAnticiposCxP.SelectedDataKey["IdDeposito"].ToString() + "&idRegistroC=0");
                //Define las dimensiones de la ventana Abrir registros de Usuario
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1160,height=450";
                //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Usuario
                ScriptServer.AbreNuevaVentana(url, "Abrir Registro Usuario", configuracion, Page);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        private void inicializaForma()
        {
            cargaCatalogos();
            txtProveedor.Text = "";
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            txtCitaCarga.Text = fecha_actual.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtCitaDescarga.Text = fecha_actual.ToString("dd/MM/yyyy HH:mm");
            Controles.InicializaGridview(gvViajesAnticipos);
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaCatalogos()
        {
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 3182);
        }
        /// <summary>
        /// Método encargado de Traer la consulta de control de Anticipos
        /// </summary>
        private void cargaControlAnticipos()
        {
            DateTime inicio, termino;
            DateTime.TryParse(txtCitaCarga.Text, out inicio);
            DateTime.TryParse(txtCitaDescarga.Text, out termino);
            using (DataTable dtControlAnticipos = SAT_CL.Documentacion.ServicioImportacion.ObtieneImportacionesControlAnticipo(
                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)), inicio, termino))
            {
                if (Validacion.ValidaOrigenDatos(dtControlAnticipos))
                {
                    Controles.CargaGridView(gvViajesAnticipos, dtControlAnticipos, "Id-IdServicioM-IdTarifaProveedor-AnticipoAutoCC-AnticipoAutoSC-ServImps-ServImpsAnt-Depositos", "", true, 5);
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtControlAnticipos, "Table");
                }
                else
                {
                    Controles.InicializaGridview(gvViajesAnticipos);
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
                Controles.InicializaIndices(gvViajesAnticipos);
            }

            //Sumando Pies
            sumaTotales();
        }
        /// <summary>
        /// 
        /// </summary>
        private void sumaTotales()
        {
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvViajesAnticipos.FooterRow.Cells[6].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalViajes)", "")));
                gvViajesAnticipos.FooterRow.Cells[7].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CC)", "")));
                gvViajesAnticipos.FooterRow.Cells[8].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SC)", "")));
                gvViajesAnticipos.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(DepsPrevios)", "")));
                gvViajesAnticipos.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(DepsCC)", "")));
                gvViajesAnticipos.FooterRow.Cells[18].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(DepsSC)", "")));
                gvViajesAnticipos.FooterRow.Cells[20].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(LiqsCerradas)", "")));
            }
            else
            {
                gvViajesAnticipos.FooterRow.Cells[6].Text = 
                gvViajesAnticipos.FooterRow.Cells[7].Text = 
                gvViajesAnticipos.FooterRow.Cells[8].Text = string.Format("{0:0}", 0);
                gvViajesAnticipos.FooterRow.Cells[11].Text =
                gvViajesAnticipos.FooterRow.Cells[14].Text =
                gvViajesAnticipos.FooterRow.Cells[18].Text =
                gvViajesAnticipos.FooterRow.Cells[20].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(Control sender, string comando)
        {
            switch (comando)
            {
                case "TarifaProveedor":
                    ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaTarifaProveedor", "ventanaTarifaProveedor");
                    break;
                case "AnticipoAutorizado":
                    ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaRegistroAnticipo", "ventanaRegistroAnticipo");
                    break;
                case "FiniquitoAutorizado":
                    ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaRegistroFiniquito", "ventanaRegistroFiniquito");
                    break;
                case "DepositosCxP":
                    ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaCxP", "ventanaCxP");
                    break;
            }
        }

        #endregion
    }
}