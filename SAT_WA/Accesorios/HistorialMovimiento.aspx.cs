using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL;
using System.Configuration;

namespace SAT.Accesorios
{
    public partial class HistorialMovimiento : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucHistorialMovimiento_ClickVerReferencia(object sender, EventArgs e)
        {
            //Validando Comando
            switch (ucHistorialMovimiento.comandoReferencia)
            {
                case "Servicio":
                    {
                        //Instanciando Servicio
                        using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(ucHistorialMovimiento.idServicio))
                        {
                            //Validando que Exista el Servicio
                            if (serv.id_servicio > 0)
                            {
                                //Mostrar ventana modal 
                                alternaVentanaModal(this, "referencia");

                                //Inicializando
                                ucReferenciaViaje.InicializaControl(serv.id_servicio, serv.id_compania_emisor, serv.id_cliente_receptor, 1);

                                //Actualizando Comando del Boton Cerrar
                                lkbCerrarReferencias.CommandArgument = "";
                            }
                        }
                        break;
                    }
                case "Movimiento":
                    {
                        //Mostrar ventana modal 
                        alternaVentanaModal(this, "referencia");

                        //Inicializando
                        ucReferenciaViaje.InicializaControl(ucHistorialMovimiento.idMovimiento, 0, 0, 10);

                        //Actualizando Comando del Boton Cerrar
                        lkbCerrarReferencias.CommandArgument = "";
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucHistorialMovimiento_ClickCalcularKMS(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Referencia
            result = ucHistorialMovimiento.CalculaKMS();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)

                //Inicializando Página
                inicializaPagina();

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucHistorialMovimiento_ClickDiesel(object sender, EventArgs e)
        {
            //Mostramos ventana modal 
            alternaVentanaModal(this, "anticipos");

            //Cargamos Anticipos
            cargaAnticipos();

            //Cambiando Vista
            mtvAnticipos.ActiveViewIndex = 2;
        }

        /// <summary>
        /// Genera un Nuevo Vale de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoVale_Click(object sender, EventArgs e)
        {
            //Inicializamos Control de Usuario Diesel
            ucAsignacionDiesel.InicializaControlUsuario(ucHistorialMovimiento.idServicio, ucHistorialMovimiento.idMovimiento);

            //Mostramos  vista
            mtvAnticipos.ActiveViewIndex = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucHistorialMovimiento_ClickDepositos(object sender, EventArgs e)
        {
            //Cargamos Anticipos
            cargaAnticipos();

            //Cambiando Vista
            mtvAnticipos.ActiveViewIndex = 2;

            //Mostramos ventana modal 
            alternaVentanaModal(this, "anticipos");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucHistorialMovimiento_ClickEncabezadoServicio(object sender, EventArgs e)
        {
            //Cerrando Ventana Modal
            alternaVentanaModal(this, "encabezadoServicio");

            wucEncabezadoServicio.InicializaEncabezadoServicio(ucHistorialMovimiento.idServicio);
        }

        /// <summary>
        /// Genera un Nuevo deposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            ucDepositos.HabilitaConcepto = true;
            ucDepositos.MuestraSolicitar = false;
            //Inicializamos Control de Usuario Depositó
            ucDepositos.InicializaControl(ucHistorialMovimiento.idServicio, ucHistorialMovimiento.idMovimiento);

            //Mostramos  vista
            mtvAnticipos.ActiveViewIndex = 0;
        }

        protected void ucHistorialMovimiento_ClickDevolucion(object sender, EventArgs e)
        {
            //Validando si Existe la Devolución
            if (ucHistorialMovimiento.idDevolucion > 0)
            {
                //Inicializando Control de Devoluciones
                wucDevolucionFaltante.InicializaDevolucion(ucHistorialMovimiento.idDevolucion);

                //Abriendo ventana modal 
                alternaVentanaModal(this, "devolucion");
            }
            else
            {
                //Validando si Existe el Servicio
                if (ucHistorialMovimiento.idServicio > 0)
                {
                    //Instanciando Movimiento
                    using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(ucHistorialMovimiento.idMovimiento))
                    {
                        //Validando que exista el Movimiento
                        if (mov.habilitar)
                        {
                            //Inicializando Control
                            wucDevolucionFaltante.InicializaDevolucion(mov.id_compania_emisor, mov.id_servicio, mov.id_movimiento, mov.id_parada_destino);

                            //Abriendo ventana modal 
                            alternaVentanaModal(this, "devolucion");
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, "No Existe el Movimiento", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this, "No Existe el Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando del Control
            switch (lnk.CommandName)
            {
                case "Referencias":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "referencia");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Devolucion":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "devolucion");
                            break;
                        case "Anticipos":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "anticipos");
                            break;
                    }

                    break;
                case "Diesel":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "diesel");
                    break;
                case "Deposito":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "deposito");
                    break;
                case "Devolucion":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "devolucion");

                    //Obteniendo Valores
                    //int idRecurso = Convert.ToInt32(Request.QueryString["idRegistro"]);
                    //int idTipoAsignacion = Convert.ToInt32(Request.QueryString["idRegistroB"]);

                    //Inicializando Control de Usuario
                    //ucHistorialMovimiento.InicializaControlUsuario(idRecurso, idTipoAsignacion);

                    //Invocando Método de Busqueda
                    ucHistorialMovimiento.BuscaHistorialMovimiento();
                    break;
                case "Lectura":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "lectura");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Diesel":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "diesel");
                            break;
                    }
                    break;
                case "Anticipos":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "anticipos");
                    //Invocando Método de Busqueda
                    ucHistorialMovimiento.BuscaHistorialMovimiento();
                    break;
                case "EncabezadoServicio":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "encabezadoServicio");
                    //Invocando Método de Busqueda
                    ucHistorialMovimiento.BuscaHistorialMovimiento();
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Referencia
            result = ucReferenciaViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)
            {
                //Validando Tabla
                switch (ucReferenciaViaje.Tabla)
                {
                    case 1:
                    case 10:
                        {
                            //Invocando Método de Busqueda
                            ucHistorialMovimiento.BuscaHistorialMovimiento();
                            break;
                        }
                    case 156:
                        {
                            //Inicializando Devolución
                            wucDevolucionFaltante.InicializaDevolucion(ucReferenciaViaje.Registro);
                            break;
                        }
                    case 157:
                        {
                            //Instanciando Detalle
                            using (SAT_CL.Despacho.DevolucionFaltanteDetalle det = new SAT_CL.Despacho.DevolucionFaltanteDetalle(ucReferenciaViaje.Registro))
                            {
                                //Validando que Exista el Registro
                                if (det.habilitar)

                                    //Inicializando Devolución
                                    wucDevolucionFaltante.InicializaDevolucion(det.id_devolucion_faltante);
                            }
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Referencia
            result = ucReferenciaViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)
            {
                //Validando Tabla
                switch (ucReferenciaViaje.Tabla)
                {
                    case 1:
                        {
                            //Invocando Método de Busqueda
                            ucHistorialMovimiento.BuscaHistorialMovimiento();
                            break;
                        }
                    case 156:
                        {
                            //Inicializando Devolución
                            wucDevolucionFaltante.InicializaDevolucion(ucReferenciaViaje.Registro);
                            break;
                        }
                    case 157:
                        {
                            //Instanciando Detalle
                            using (SAT_CL.Despacho.DevolucionFaltanteDetalle det = new SAT_CL.Despacho.DevolucionFaltanteDetalle(ucReferenciaViaje.Registro))
                            {
                                //Validando que Exista el Registro
                                if (det.habilitar)

                                    //Inicializando Devolución
                                    wucDevolucionFaltante.InicializaDevolucion(det.id_devolucion_faltante);
                            }
                            break;
                        }
                }
            }
        }

        #region Eventos Anticipos

        /// <summary>
        /// Evento generado al Crear un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsigancionDeposito_ClickRegistrar(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Objeto Resultado
            resultado = ucDepositos.RegistraDeposito();

            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(resultado.IdRegistro))
            {
                //Instanciando Concepto de Deposito
                using (SAT_CL.EgresoServicio.ConceptoDeposito cd = new SAT_CL.EgresoServicio.ConceptoDeposito(dep.id_concepto))
                {
                    //Validando que existan los Registros
                    if (dep.habilitar && cd.habilitar)
                    {
                        //Validando que 
                        if (!cd.descripcion.Equals("Anticipo Proveedor"))
                        {
                            //Validamos que el Deppósito se se encuentre Registrado
                            if (dep.id_estatus != 1)
                            {
                                //Mostramos Reporte Anticipos
                                mtvAnticipos.ActiveViewIndex = 2;
                                //cargamos Anticipos
                                cargaAnticipos();
                            }
                        }
                    }

                }
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento generado al Solicitar un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsigancionDeposito_ClickSolicitar(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Objeto Resultado
            resultado = ucDepositos.SolicitaDeposito();
            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(resultado.IdRegistro))
            {
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Validamos que el Deppósito se se encuentre Registrado
                    if (dep.id_estatus != 1)
                    {
                        //Mostramos Reporte Anticipos
                        mtvAnticipos.ActiveViewIndex = 2;
                        //cargamos Anticipos
                        cargaAnticipos();
                    }
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de Eliminar un Deposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsigancionDeposito_ClickEliminar(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Objeto Resultado
            resultado = ucDepositos.DeshabilitaDeposito();

            //Validamos Inserrciòn de Depósito
            if (resultado.OperacionExitosa)
            {
                //Mostramos Reporte Anticipos
                mtvAnticipos.ActiveViewIndex = 2;
                //cargamos Anticipos
                cargaAnticipos();
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        /// <summary>
        /// Evento generado al Cancelar Un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsigancionDeposito_ClickCancelar(object sender, EventArgs e)
        {
            //Mostramos Reporte Anticipos
            mtvAnticipos.ActiveViewIndex = 2;

            //Cargamos Anticipos
            cargaAnticipos();
        }

        /// <summary>
        /// Evento generado al Guardar el Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionDiesel1_ClickGuardarAsignacion(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Objeto Resultado
            resultado = ucAsignacionDiesel.GuardaDiesel();

            //Validamos Onsercciòn de Diesel
            if (resultado.OperacionExitosa)
            {
                //Mostramos Reporte Anticipos
                mtvAnticipos.ActiveViewIndex = 2;

                //cargamos Anticipos
                cargaAnticipos();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento generado al Cancelar la Asignación de Diesel
        /// </summary>
        protected void wucAsignacionDiesel1_ClickCancelarAsignacion(object sender, EventArgs e)
        {
            //Mostramos Reporte Anticipos
            mtvAnticipos.ActiveViewIndex = 2;

            //cargamos Anticipos
            cargaAnticipos();

        }

        /// <summary>
        /// Evento generado al Abrir las Referencias de la Asignación de Diesel
        /// </summary>
        protected void wucAsignacionDiesel1_ClickReferenciaAsignacion(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciaViaje.InicializaControl(ucAsignacionDiesel.idAsignacionDiesel, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, 69);

            //Cerrando ventana modal 
            alternaVentanaModal(this, "anticipos");

            //Abriendo Ventana de Referencias
            alternaVentanaModal(this.Page, "referencia");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Anticipos";
        }

        /// <summary>
        /// Evento Generado al Calculado Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCalculadoDiesel(object sender, EventArgs e)
        {
            //Inicializamos Informacipon
            InicializaInformacionDieselKms(ucAsignacionDiesel);
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(ucAsignacionDiesel, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");
        }

        /// <summary>
        /// Evento generado al dar click en Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaInformacionCalculado_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrarVentanaInformacionCalculado, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");

        }

        #endregion

        #region Eventos Devolución

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDevolucion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDetalleDevolucion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickEliminarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.EliminaDetalleDevolucion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDevolucion(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciaViaje.InicializaControl(wucDevolucionFaltante.objDevolucionFaltante.id_devolucion_faltante, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 156);

            //Alternando Ventanas Modales
            alternaVentanaModal(this, "referencia");
            alternaVentanaModal(this, "devolucion");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Devolucion";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDetalle(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            ucReferenciaViaje.InicializaControl(wucDevolucionFaltante.idDetalle, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 157);

            //Alternando Ventanas Modales
            alternaVentanaModal(this, "referencia");
            alternaVentanaModal(this, "devolucion");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Devolucion";
        }


        #endregion

        /*#region Eventos Lectura

        /// <summary>
        /// Evento Producido al Guardar la Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickGuardarLectura(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Vale de Diesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel diesel = new SAT_CL.EgresoServicio.AsignacionDiesel(ucAsignacionDiesel.idAsignacionDiesel))
                {
                    //Validando que exista el Vale
                    if (diesel.habilitar)
                    {
                        //Guardando Lectura
                        result = wucLectura.GuardarLectura();

                        //Validando que se Haya Guardado el Registro
                        if(result.OperacionExitosa)
                        {
                            //Obteniendo Lectura de Diesel
                            int idLectura = result.IdRegistro;
                            
                            //Actualizando Lectura
                            result = diesel.ActualizaLecturaDiesel(idLectura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que se Haya Actualizado el Registro
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento Producido al Eliminar la Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickEliminarLectura(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Vale de Diesel
                using (SAT_CL.EgresoServicio.AsignacionDiesel diesel = new SAT_CL.EgresoServicio.AsignacionDiesel(ucAsignacionDiesel.idAsignacionDiesel))
                {
                    //Validando que exista el Vale
                    if (diesel.habilitar)
                    {
                        //Guardando Lectura
                        result = wucLectura.DeshabilitarLectura();

                        //Validando que se Haya Guardado el Registro
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Lectura
                            result = diesel.ActualizaLecturaDiesel(0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que se Haya Actualizado el Registro
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                trans.Complete();
                        }
                    }
                }
            }

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion//*/

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Valores
            int idRecurso = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idTipoAsignacion = Convert.ToInt32(Request.QueryString["idRegistroB"]);

            //Inicializando Control de Usuario
            ucHistorialMovimiento.InicializaControlUsuario(idRecurso, idTipoAsignacion);

            //Anticipos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoAnticipos, "", 18);
        }
        /// <summary>
        /// Administra la visualización de ventanas modales en la página (muestra/oculta)
        /// </summary>
        /// <param name="control">Control que afecta a la ventana</param>
        /// <param name="nombre_script_ventana">Nombre del script de la ventana</param>
        private void alternaVentanaModal(Control control, string nombre_script_ventana)
        {
            //Determinando que ventana será afectada (mostrada/ocultada)
            switch (nombre_script_ventana)
            {
                case "diesel":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalAsignacionDiesel", "asignacionDiesel");
                    break;
                case "deposito":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalAsignacionDeposito", "asignacionDeposito");
                    break;
                case "referencia":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "devolucion":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalDevolucionFaltante", "devolucionFaltante");
                    break;
                case "lectura":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalVentanaLectura", "ventanaLectura");
                    break;
                case "anticipos":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalAnticipos", "anticipos");
                    break;
                case "encabezadoServicio":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "encabezadoServicioModal", "encabezadoServicio");
                    break;
            }
        }




        #endregion

        #region Métodos "Anticipos

        /// <summary>
        /// Método Encargado de Cargar los Anticipos
        /// </summary>
        private void cargaAnticipos()
        {

            //Obteniendo depósitos y vales del viaje o movimiento vacío 
            using (DataTable dt = SAT_CL.EgresoServicio.Reportes.CargaAnticiposMovimiento(ucHistorialMovimiento.idMovimiento))
            {
                //Inicializando indices gridView
                Controles.InicializaIndices(gvAnticipos);
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {   //Cargando GridView
                    Controles.CargaGridView(gvAnticipos, dt, "Id-Tipo", "", true, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "AnticiposHistorial");
                }
                else
                {   //Inicializando GridView
                    Controles.InicializaGridview(gvAnticipos);
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "AnticiposHistorial");
                }
            }
        }

        #endregion

        #region Eventos GridView "Anticipos"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "AnticiposHistorial"), Convert.ToInt32(ddlTamanoAnticipos.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarAnticipos_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "AnticiposHistorial"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenarAnticipos.Text = Controles.CambiaSortExpressionGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "AnticiposHistorial"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Anticipos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "AnticiposHistorial"), e.NewPageIndex);
        }

        /// <summary>
        /// Evento producido al pulsar el link Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAnticipos_OnClick(object sender, EventArgs e)
        {
            //Validamos Existencia de Anticipos
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvAnticipos, sender, "lnk", false);
                //Instanciamos Servicio
                using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(ucHistorialMovimiento.idServicio))
                {

                    //Si el tipo de operaciòn es Depòsito
                    if (gvAnticipos.SelectedDataKey["Tipo"].ToString() == "Deposito")
                    {
                        //Intsanciamos Antcipo
                        using (SAT_CL.EgresoServicio.Deposito objDeposito = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(gvAnticipos.SelectedValue)))
                        {
                            //Instanciamos Detalle Liquidacion
                            using (SAT_CL.EgresoServicio.DetalleLiquidacion objDetalleLiquidacion = new SAT_CL.EgresoServicio.DetalleLiquidacion(objDeposito.id_deposito, 51))
                            {
                                //De acuerdo al Tipo de Asignación
                                SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo tipo = SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Operador;
                                int id_recurso = objDetalleLiquidacion.id_operador;
                                //Deacuerdo a la Asignación Actual
                                if (objDetalleLiquidacion.id_unidad != 0)
                                {
                                    //Asignamos Tipo Unidad
                                    tipo = SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Unidad;
                                    id_recurso = objDetalleLiquidacion.id_unidad;
                                }
                                //Si la Asignación es Tercero
                                else if (objDetalleLiquidacion.id_operador != 0)
                                {
                                    //Asignamos Tercero
                                    tipo = SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Tercero;
                                    id_recurso = objDetalleLiquidacion.id_proveedor_compania;
                                }
                                //Mostramos Vista Depòsito
                                mtvAnticipos.ActiveViewIndex = 0;
                                ucDepositos.HabilitaConcepto = false;
                                ucDepositos.MuestraSolicitar = true;

                                //Inicializamos Control Depòsito en Ediciòn
                                ucDepositos.InicializaControl(Convert.ToInt32(gvAnticipos.SelectedValue), objDetalleLiquidacion.id_unidad, objDetalleLiquidacion.id_operador, objDetalleLiquidacion.id_proveedor_compania,
                                            objServicio.id_servicio, objDetalleLiquidacion.id_movimiento, SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaTerminada(objDetalleLiquidacion.id_movimiento, tipo, id_recurso),
                                            objServicio.id_cliente_receptor);

                            }
                        }
                    }
                    else
                    {
                        //Mostramos Vista Vale
                        mtvAnticipos.ActiveViewIndex = 1;
                        //Inicializando control de edición de vales
                        ucAsignacionDiesel.InicializaControlUsuario(Convert.ToInt32(gvAnticipos.SelectedValue));
                    }

                }
            }
        }


        /// <summary>
        /// Inicializamos Información de Diesel y Kms
        /// <
        /// </summary>
        private void InicializaInformacionDieselKms(System.Web.UI.Control control)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declarando Variables Auxiliares cap_unidad = 0;
            decimal cap_unidad = 0;
            int id_unidad = 0;
            DateTime fecha_carga = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            //Instanciamos Diesel
            using (SAT_CL.EgresoServicio.AsignacionDiesel objAsignacionDiesel = new SAT_CL.EgresoServicio.AsignacionDiesel(ucAsignacionDiesel.idAsignacionDiesel))
            {
                //Validamos Vale
                if (objAsignacionDiesel.id_asignacion_diesel > 0)
                {
                    //Establecemos Fecha Carga
                    fecha_carga = objAsignacionDiesel.fecha_carga;
                }
            }
            //Instanciando Unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(ucAsignacionDiesel.idUnidadDiesel))
            {
                //Validando que Exista la Unidad
                if (uni.id_unidad > 0)
                {
                    //Obteniendo Capacidad de Combustible
                    cap_unidad = uni.capacidad_combustible;
                    id_unidad = uni.id_unidad;
                }
                else
                {
                    //Mostramos Mensaje
                    resultado = new RetornoOperacion("No Existe Unidad Motriz asignada.");
                }


                //Obtenemos rendimiento
                decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                //Si el Rendimiento No existe
                if (rendimiento == 0)
                {
                    //Validamos exist Kilometraje
                    if (uni.kilometraje_asignado != 0)
                    {

                        //Validamos combustible Asignado
                        if (uni.combustible_asignado != 0)
                        {
                            //Calculamos rendimiento
                            rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                        }
                    }
                }

                //Inicializamos Valores
                lblCapacidadTanque.Text = cap_unidad.ToString() + "lts";
                lblRendimiento.Text = Cadena.TruncaCadena(rendimiento.ToString(), 5, "") + "kms/lts";
                lblFechaUltimaCarga.Text = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel).ToString("dd/MM/yy HH:mm");
                lblKmsUltimaCarga.Text = SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga).ToString() + "kms";
                //Validamos que sea diferente de 0 para la Division
                if (rendimiento > 0)
                {
                    lblCalculado.Text = Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                    lblSobrante.Text = Cadena.TruncaCadena((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))).ToString(), 5, "") + "lts";
                    lblAlcanceKms.Text = Cadena.TruncaCadena(((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))) * rendimiento).ToString(), 5, "") + "kms.";
                }
                else
                {
                    //Mostramos Resultado
                    resultado = new RetornoOperacion("El rendimiento debe ser Mayor a 0");
                }
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        #endregion

        #region Referencias Encabezado Servicio

        /// <summary>
        /// Evento Producido al Guardar las Referencias del Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEncabezadoServicio_ClickGuardarReferencia(object sender, EventArgs e)
        {
            //Guardando Referencias
            wucEncabezadoServicio.GuardaEncabezadoServicio();

            //Invocando Método de Busqueda
            ucHistorialMovimiento.BuscaHistorialMovimiento();
            //Cerrando Ventana Modal
            alternaVentanaModal(this, "encabezadoServicio");

        }

        #endregion

        #region Eventos Encabezado Servicio
        #endregion
    }
}