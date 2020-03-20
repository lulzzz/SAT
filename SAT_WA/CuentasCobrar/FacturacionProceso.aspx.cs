using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasCobrar
{
    public partial class FacturacionProceso : System.Web.UI.Page
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
                inicializaPagina();

            //Cargando Controles
            cargaFacturasGlobales();
            cargaPaquetesTerminados();
        }
        /// <summary>
        /// Evento disparado al Presionar el Menu
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
                        inicializaPagina();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(152, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaPaqueteProceso();
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        //Cargando Catalogos Autocompleta
                        cargaFacturasGlobales();
                        cargaPaquetesTerminados();
                        break;
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Validando Estatus
                                if (validaEstatusPaquete())
                                {
                                    //Deshabilitando Producto
                                    result = pp.DeshabilitaPaqueteProceso(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación sea exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Limpiando registro de Session
                                        Session["id_registro"] = 0;
                                        //Cambiando a Estatus "Nuevo"
                                        Session["estatus"] = Pagina.Estatus.Nuevo;
                                        //Inicializando Forma
                                        inicializaPagina();
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

                                //Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Reversa":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Declarando Objeto de Retorno
                            RetornoOperacion retorno = new RetornoOperacion();

                            //Validando que exista un Id Valido
                            if (pp.habilitar)
                            {
                                //Validando Estatus Actual
                                switch (pp.estatus)
                                {
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Aceptado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Rechazado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Terminado:
                                        {
                                            //Ocultando Ventana Modal
                                            ScriptServer.AlternarVentana(this.Page, "ReversaPaquete", "confirmacionVentanaReversa", "ventanaReversa");
                                            //Instanciando Resultado Positivo
                                            retorno = new RetornoOperacion(Convert.ToInt32(Session["id_registro"]), "Puede reversar su proceso de revisión", true);
                                            //Limpiando Control
                                            txtMotivoReversa.Text = "";
                                            break;
                                        }
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado:
                                        {
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("El Paquete ya se encuentra en Estatus 'Registrado'");
                                            break;
                                        }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Paquete");

                            //Mostrando Resultado
                            ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Bitacora":
                    {
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "152", "Factura Proceso");
                        break;
                    }
                case "Referencias":
                    {
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "152", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
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
                case "Entregar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Validando Estatus
                                if (validaEstatusPaquete())
                                {
                                    //Validando que existan 
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table1"))
                                    {
                                        //Deshabilitando Producto
                                        result = pp.ActualizaEstatusPaqueteProceso(SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación sea exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Limpiando registro de Session
                                            Session["id_registro"] = result.IdRegistro;
                                            //Cambiando a Estatus "Nuevo"
                                            Session["estatus"] = Pagina.Estatus.Lectura;
                                            //Inicializando Forma
                                            inicializaPagina();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existen Facturas Ligadas al Paquete");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus de Paquete no permite su Edición");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede acceder al Registro");

                            //Mostrando Mensaje de Operación
                            lblError.Text = result.Mensaje;
                        }
                        break;
                    }
                case "Aceptar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Validando Estatus
                                if (pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado || pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado)
                                {
                                    //Validando que existan 
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table1"))
                                    {
                                        //Deshabilitando Producto
                                        result = pp.ActualizaEstatusPaqueteProceso(SAT_CL.Facturacion.PaqueteProceso.Estatus.Aceptado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación sea exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Limpiando registro de Session
                                            Session["id_registro"] = result.IdRegistro;
                                            //Cambiando a Estatus "Nuevo"
                                            Session["estatus"] = Pagina.Estatus.Lectura;
                                            //Inicializando Forma
                                            inicializaPagina();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existen Facturas Ligadas al Paquete");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus de Paquete no permite su Edición");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede acceder al Registro");

                            //Mostrando Mensaje de Operación
                            lblError.Text = result.Mensaje;
                        }
                        break;
                    }
                case "Rechazar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Validando el Estatus del Paquete
                                if (pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado || pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado)
                                {
                                    //Validando que existan 
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table1"))
                                    {
                                        //Deshabilitando Producto
                                        result = pp.ActualizaEstatusPaqueteProceso(SAT_CL.Facturacion.PaqueteProceso.Estatus.Rechazado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación sea exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Limpiando registro de Session
                                            Session["id_registro"] = result.IdRegistro;
                                            //Cambiando a Estatus "Nuevo"
                                            Session["estatus"] = Pagina.Estatus.Lectura;
                                            //Inicializando Forma
                                            inicializaPagina();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existen Facturas Ligadas al Paquete");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus de Paquete no permite su Edición");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede acceder al Registro");

                            //Mostrando Mensaje de Operación
                            lblError.Text = result.Mensaje;
                        }
                        break;
                    }
                case "Terminar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Validando Estatus
                                if (pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Aceptado || 
                                    pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado ||
                                    pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado)
                                {
                                    //Validando que existan 
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table1"))
                                    {
                                        //Deshabilitando Producto
                                        result = pp.ActualizaEstatusPaqueteProceso(SAT_CL.Facturacion.PaqueteProceso.Estatus.Terminado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación sea exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Limpiando registro de Session
                                            Session["id_registro"] = result.IdRegistro;
                                            //Cambiando a Estatus "Nuevo"
                                            Session["estatus"] = Pagina.Estatus.Lectura;
                                            //Inicializando Forma
                                            inicializaPagina();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No existen Facturas Ligadas al Paquete");
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus de Paquete no permite su Edición");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede acceder al Registro");

                            //Mostrando Mensaje de Operación
                            lblError.Text = result.Mensaje;
                        }
                        break;
                    }
                case "Imprimir":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/FacturacionProceso.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ProcesoRevision", Convert.ToInt32(Session["id_registro"])), "Proceso Facturación", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "AcuseProcter":
                    {
                        //Instanciando Paquete
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Cargando Facturas
                                using (DataSet ds = SAT_CL.Facturacion.PaqueteProceso.ReportePROCTER(pp.id_paquete_proceso))
                                {

                                    //Validando que existan datos
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                                    {
                                        //Exportando Contenido
                                        TSDK.ASP.Controles.ExportaContenidoGridView(ds.Tables[0]);
                                    }

                                }
                            }
                        }
                        break;
                    }
                case "AcuseColgate":
                    {
                        //Invoca a la clase paquete proceso
                        using (SAT_CL.Facturacion.PaqueteProceso paquete = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Valida que exista el registro de paquete
                            if (paquete.id_paquete_proceso > 0)
                            {
                                //Invoca al método ReporteColgate y almacena el resultado del método en el dataset DS
                                using (DataSet DS = SAT_CL.Facturacion.PaqueteProceso.ReporteColgate(paquete.id_paquete_proceso))
                                {
                                    //Valida los datos del DS
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS))
                                    {
                                        //Exporta el contenido a excel, invocando el metodo ExportaContenidoGridView
                                        TSDK.ASP.Controles.ExportaContenidoGridView(DS.Tables[0]);
                                    }
                                }
                            }
                        }
                        break;
                    }
                case "AcuseSchindler":
                    {
                        //Invoca a la clase paquete proceso
                        using (SAT_CL.Facturacion.PaqueteProceso paquete = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Valida que exista el registro de paquete
                            if (paquete.id_paquete_proceso > 0)
                            {
                                //Invoca al método ReporteColgate y almacena el resultado del método en el dataset DS
                                using (DataSet DS = SAT_CL.Facturacion.PaqueteProceso.ReporteSchindler(paquete.id_paquete_proceso))
                                {
                                    //Valida los datos del DS
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS))
                                    {
                                        //Exporta el contenido a excel, invocando el metodo ExportaContenidoGridView
                                        TSDK.ASP.Controles.ExportaContenidoGridView(DS.Tables[0]);
                                    }
                                }
                            }
                        }
                        break;
                    }

                case "AcuseABC":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/FacturacionProceso.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "AcuseABC", Convert.ToInt32(Session["id_registro"])), "Acuse ABC", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "AcuseLili":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasCobrar/FacturacionProceso.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "AcuseLili", Convert.ToInt32(Session["id_registro"])), "Acuse Lili", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaPaqueteProceso();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:

                    //Asignando a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
                default:

                    //Asignando a Nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    break;
            }

            //Invocando Inicialización de Página
            inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarReversa_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "ReversaPaquete", "confirmacionVentanaReversa", "ventanaReversa");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReversa_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Validando estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("Debe de existir un Paquete");
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Paquete
                        using (SAT_CL.Facturacion.PaqueteProceso paq = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando Existencia
                            if (paq.habilitar)
                            {
                                //Validando Estatus Actual
                                switch (paq.estatus)
                                {
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Aceptado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Rechazado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Terminado:
                                        {
                                            //Inicializando Bloque Transaccional
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Actualizando Estatus (Reversa)
                                                retorno = paq.ActualizaEstatusPaqueteProceso(SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando Operación Exitosa
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Insertando Motivo
                                                    retorno = SAT_CL.Global.Referencia.InsertaReferencia(paq.id_paquete_proceso, 152,
                                                                SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 152, "Motivo Reversa", 0, "General"), txtMotivoReversa.Text,
                                                                Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando Operación Exitosa
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        //Instanciando PP
                                                        retorno = new RetornoOperacion(paq.id_paquete_proceso);
                                                        //Completando Transacción
                                                        scope.Complete();
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado:
                                        {
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("El Paquete ya se encuentra en Estatus 'Registrado'");
                                            break;
                                        }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("Debe de existir un Paquete");
                        }

                        break;
                    }
            }

            //validando Resultado
            if (retorno.OperacionExitosa)
            {
                //Asignando valor a Session Registro
                Session["id_registro"] = retorno.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Ocultando Ventana Modal
                ScriptServer.AlternarVentana(btnReversa, "ReversaPaquete", "confirmacionVentanaReversa", "ventanaReversa");

                //Inicializando la Página
                inicializaPagina();
            }

            //Mostrando Excepción
            ScriptServer.MuestraNotificacion(btnReversa, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar - Factura Global"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFG_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus del Paquete
            if (validaEstatusPaquete())
            {
                //Declarando Variables Auxiliares
                string[] msn_error = new string[1];
                int contador = 0;

                //Instanciando Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtFacturaGlobal.Text, "ID:", 1, "0"))))
                {
                    //Validando que exista la Factura Global
                    if (fg.id_factura_global > 0)
                    {
                        //Obteniendo Detalles de la Factura Global
                        using (DataTable dtFacturacion = SAT_CL.Facturacion.FacturadoFacturacion.ObtieneDetallesFacturacionFacturaGlobal(fg.id_factura_global))
                        {
                            //Validando que existan las Facturas
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturacion))
                            {
                                //Creando Arreglo Dinamico
                                msn_error = new string[dtFacturacion.Rows.Count];

                                //Recorriendo Registros
                                foreach (DataRow dr in dtFacturacion.Rows)
                                {
                                    //Insertando Detalle
                                    result = SAT_CL.Facturacion.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(dr["IdFactura"]),
                                                Convert.ToInt32(dr["IdFacturaConcepto"]), false, false, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación haya sido Exitosa
                                    if (!result.OperacionExitosa)

                                        //Añadiendo Mensaje de Error
                                        msn_error[contador] = "*La Factura " + dr["IdFactura"].ToString() + ": no se encuentra disponible.<br />";

                                    //Incrementando Contador
                                    contador++;
                                }

                                //Personalizando Mensaje
                                result = new RetornoOperacion("Operación Exitosa.<br />" + string.Join("<br />", msn_error));
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede Acceder a la Factura Global, es posible que haya sido Eliminada");
                }

                //Limpienado Control
                txtFacturaGlobal.Text = "";

                //Cargando Facturas
                cargaFacturas();
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

            //Mostrando Mensaje de Error
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar - Paquete"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarPQ_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Objeto de Retorno
            string msn_operacion = "";

            //Validando Estatus del Paquete
            if (validaEstatusPaquete())
            {
                //Instanciando Factura Global
                using (SAT_CL.Facturacion.PaqueteProceso pq = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtPaqPrevio.Text, "ID:", 1, "0"))))
                {
                    //Validando que exista un Paquete
                    if (pq.id_paquete_proceso > 0)
                    {
                        //Obteniendo Detalles
                        using(DataTable dtDetallesPaquete = SAT_CL.Facturacion.PaqueteProcesoDetalle.ObtieneFacturacionPaqueteProceso(pq.id_paquete_proceso))
                        {
                            //Validando que Existan Detalles
                            if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesPaquete))
                            {
                                //Recorriendo Detalles
                                foreach(DataRow dr in dtDetallesPaquete.Rows)
                                {
                                    //Instanciando Detalle
                                    using(SAT_CL.Facturacion.PaqueteProcesoDetalle ppd = new SAT_CL.Facturacion.PaqueteProcesoDetalle(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que Exista el Detalle
                                        if(ppd.id_paquete_proceso_detalle > 0)
                                        {
                                            //Insertando Paquete
                                            result = SAT_CL.Facturacion.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(Convert.ToInt32(Session["id_registro"]), ppd.id_facturado, ppd.id_facturado_detalle,
                                                            false, false, DateTime.MinValue, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Si no se agrego
                                            if (!result.OperacionExitosa)
                                            
                                                //Obteniendo Mensaje de Operación
                                                msn_operacion += string.Format("Factura No. {0}: {1}.<br />", ppd.id_facturado, result.Mensaje);
                                        }
                                    }
                                }

                                //Personalizando Resultado
                                result = new RetornoOperacion("Operación Exitosa.", true);

                                //Cargando Facturas
                                cargaFacturas();
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede Acceder al Paquete, es posible que haya sido Eliminada");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No se puede Acceder al Paquete, es posible que haya sido Eliminada");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

            //Mostrando Mensaje de Error
            lblError.Text = msn_operacion;
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar - Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFacturas_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            cargaFacturas();
        }
        /// <summary>
        /// Evento producido para abrir el control de usuario Encabezado Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPorte_Click(object sender, EventArgs e)
        {
            //Crea la variable de tipo linkbutom
            LinkButton lkb = (LinkButton)sender;
            //Valida el nombre del comando para ejecutar una acción
            switch (lkb.CommandName)
            {
                //Si es CartaPorte
                case "CartaPorte":
                    {
                        //Selecciona la fila del  grid view Facturas Disponibles
                        Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);
                        //Valida que exista un servicio para consultar la carta porte
                        if (gvFacturasDisponibles.DataKeys.Count > 0)
                        {
                            //Almacena en la variable idServicio el valor numero de servicio
                            int idServicio = Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdServicio"]);
                            //Valida que exista servicio
                            if (idServicio > 0)                            
                                //Inicializa los valores del control de usuario EncabezadoServicio
                                wucEncabezadoServicio.InicializaEncabezadoServicio(idServicio);                            
                        }
                        break;
                    }
                //Si es Porte
                case "Porte":
                    {
                        //Selecciona la fila del grid view FacturasLigadas
                        Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false); 
                        //Valida que exista un servicio para consultar la carta porte
                        if (gvFacturasLigadas.DataKeys.Count > 0)
                        {
                            //Almacena en la variable idServicio el valor numero de servicio
                            int idServicio = Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdServicio"]);
                            //Valida que exista un numero de servicio
                            if (idServicio > 0)
                                //Inicializa el control de usuario Encabezado Servicio
                                wucEncabezadoServicio.InicializaEncabezadoServicio(idServicio);
                        }
                        break;
                    }
            }
            
            //Abre la ventana modal
            TSDK.ASP.ScriptServer.AlternarVentana(this, "encabezadoServicio", "encabezadoServicioModal", "encabezadoServicio");
        }
        #region Eventos GridView "Facturas Disponibles"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFacDisp.SelectedValue), true, 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacDisp_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacDisp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Evento Producido al Enlazar un Origen de Datos con el GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Controles
                using(CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosFactura"))
                using (LinkButton lnk = (LinkButton)e.Row.FindControl("lnkAgregarFactura"))
                {
                    //Validando que existan los Controles
                    if (chk != null && lnk != null)
                    {
                        //Obteniendo Origen de la Fila
                        DataRowView rowView = (DataRowView)e.Row.DataItem;

                        //Obteniendo Indicador
                        int indicador = Convert.ToInt32(rowView["Indicador"].ToString() == "" ? "1" : rowView["Indicador"].ToString());

                        //Validando Indicador
                        switch (indicador)
                        {
                            case 0:
                                {
                                    //Habilitando Controles
                                    //lnk.Enabled =
                                    chk.Enabled = true;
                                    break;
                                }
                            default:
                                {
                                    //Deshabilitando Controles
                                    //lnk.Enabled =
                                    chk.Enabled = false;
                                    break;
                                }
                        }
                    }
                }

                //Obteniendo Controles
                using (LinkButton lkbRef1 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia1"),
                                    lkbRef2 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia2"),
                                      lkbRef3 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia3"))
                {
                    //Validando que exista el Control
                    if (lkbRef1 != null && lkbRef2 != null && lkbRef3 != null)
                    {
                        //Obteniendo Origen de la Fila
                        DataRowView rowView = (DataRowView)e.Row.DataItem;

                        //Mostrando Control
                        lkbRef1.Visible =
                        lkbRef2.Visible =
                        lkbRef3.Visible = true;

                        //Validando que Exista el Servicio
                        switch (rowView["Servicio"].ToString())
                        {
                            //Si no es de Servicio
                            case "No Aplica":
                                {
                                    //Deshabilitando Control
                                    lkbRef1.Enabled =
                                    lkbRef2.Enabled =
                                    lkbRef3.Enabled = false;

                                    break;
                                }
                            default:
                                {
                                    //Habilitando Control
                                    lkbRef1.Enabled =
                                    lkbRef2.Enabled =
                                    lkbRef3.Enabled = true;

                                    //Validando que exista almenos un Valor
                                    if (rowView["Referencia1"].ToString().Equals(""))

                                        //Asignando Valor
                                        lkbRef1.Text = "Sin Referencia";
                                    break;
                                }
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// Evento Producido al Seleccionar el ChekBox "Todos" del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFactura_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Obteniendo Control Disparador
                CheckBox chk = (CheckBox)sender;
                CheckBox chkVarios;

                //Validando el Control
                switch (chk.ID)
                {
                    case "chkTodosFactura":
                        {
                            //Validando que la fila sea seleccionada
                            if (!chk.Checked)
                            {
                                //Seleccionando Todas las Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosFactura", chk.Checked);

                                //Visualizando Control
                                btnGuardarFactura.Visible = btnGuardarConceptos.Enabled = false;
                            }
                            else
                            {
                                //Recorriendo cada fila
                                foreach (GridViewRow gvr in gvFacturasDisponibles.Rows)
                                {
                                    //Obteniendo Control
                                    chkVarios = (CheckBox)gvr.FindControl("chkVariosFactura");

                                    //Validando que existe la Fila
                                    if (chkVarios != null)

                                        //Validando que el Control este Habilitado para su Seleccion
                                        chkVarios.Checked = chkVarios.Enabled ? chk.Enabled : false;
                                }

                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                                //Visualizando Control de Guardado
                                btnGuardarFactura.Visible = btnGuardarConceptos.Enabled = gvFilas.Length > 0 ? true : false;
                            }

                            break;
                        }
                    case "chkVariosFactura":
                        {
                            //Obteniendo Control de ENcabezado
                            CheckBox chkHeader = (CheckBox)gvFacturasDisponibles.HeaderRow.FindControl("chkTodosFactura");

                            //Validando que el control se haya desmarcado
                            if (!chk.Checked)

                                //Desmarcando Encabezado
                                chkHeader.Checked = false;

                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                            //Validando que existan filas seleccionadas
                            if (gvr.Length > 0)

                                //Visualizando Control de Guardado
                                btnGuardarFactura.Visible = true;
                            else
                                //Ocultando Control de Guardado
                                btnGuardarFactura.Visible = false;

                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Instanciando la Factura Global
                using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                {
                    //Cargando los Conceptos Disponibles de la Factura
                    using (DataTable dtConceptos = SAT_CL.Facturacion.PaqueteProcesoDetalle.CargaConceptosDisponibleFactura(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]), pp.id_cliente))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                        {
                            //Cargando GridView de Conceptos
                            TSDK.ASP.Controles.CargaGridView(gvFacturaConceptos, dtConceptos, "IdFacturaConcepto", "", true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["SD"], dtConceptos, "Table2");
                        }
                        else
                        {
                            //Inicializando GridView de Conceptos
                            TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["SD"], "Table2");
                        }
                    }

                    //Ocultando Control
                    btnGuardarConceptos.Visible = false;

                    //Inicializando GridView de Conceptos
                    TSDK.ASP.Controles.InicializaIndices(gvFacturaConceptos);

                    //Abriendo Ventana Modal de Conceptos
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasDisponibles, upgvFacturasDisponibles.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");

                }
            }
        }
        /// <summary>
        /// Evento Producido al Marcar o Desmarcar loc Controles CheckBox de las Facturas Ligadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkActualizaDetalle_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Declarando Variables Auxiliares
                bool no_entregado = false, rechazado = false;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "chk", false);

                //Obteniendo el Control
                CheckBox chk = (CheckBox)sender;

                //Validando que Exista el Control
                if (chk != null)
                {
                    //Validando que este marcada el control
                    if (chk.Checked)
                    {
                        //Validando el ID del Control
                        switch (chk.ID)
                        {
                            case "chkNoEntregado":
                                {
                                    //Asignando Controles
                                    no_entregado = true;
                                    rechazado = false;

                                    break;
                                }
                            case "chkRechazado":
                                {
                                    //Asignando Controles
                                    no_entregado = false;
                                    rechazado = true;
                                    break;
                                }
                        }

                        //Inicializando Bloque
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Factura
                            using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"])))
                            {
                                //Obteniendo Paquete
                                using (DataTable dtDetallesPaquete = SAT_CL.Facturacion.PaqueteProcesoDetalle.ObtieneFacturacionPaqueteProceso(fac.id_factura, Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Validando que Existan Registros
                                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesPaquete))
                                    {
                                        //Recorriendo Detalles
                                        foreach (DataRow dr in dtDetallesPaquete.Rows)
                                        {
                                            //Instanciando Detalle
                                            using (SAT_CL.Facturacion.PaqueteProcesoDetalle ppd = new SAT_CL.Facturacion.PaqueteProcesoDetalle(Convert.ToInt32(dr["Id"])))
                                            {
                                                //Instanciando Detalle
                                                if (ppd.id_paquete_proceso_detalle > 0)
                                                {
                                                    //Actualizando Proceso
                                                    result = ppd.ActualizaProcesoDetalle(no_entregado, rechazado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación haya sido exitosa
                                                    if (!result.OperacionExitosa)

                                                        //Terminando Ciclo
                                                        break;
                                                }
                                            }
                                        }

                                        //Validando que la Operación haya sido exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Cargando Facturas
                                            cargaFacturas();

                                            //Inicializa Indices
                                            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Mostrando Mensaje de Error
                    lblError.Text = result.Mensaje;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Agregar un Registro del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarFactura_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Validando el Estatus del Paquete
                if (validaEstatusPaquete())
                {
                    //Seleccionando Fila
                    TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                    //Validando que existe la Factura 
                    if (Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]) > 0)

                        //Insertando Registro de Relación
                        result = SAT_CL.Facturacion.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                        0, false, false, DateTime.MinValue, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Factura");

                    //Validando que las Operaciones hayan sido exitosas
                    if (result.OperacionExitosa)
                    {
                        //Personalizando Mensaje de la Operación
                        result = new RetornoOperacion(string.Format("La Factura {0}: se ha agregado exitosamente", gvFacturasDisponibles.SelectedDataKey["IdFactura"]));

                        //Invocando Método de Carga
                        cargaFacturas();
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

                //Mostrando Mensaje de Operación
                lblError.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Editar los Conceptos de una Factura del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Inicializando el Control
                ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]));

                //Mostrando Ventana
                TSDK.ASP.ScriptServer.AlternarVentana(this, "EdicionDetalles", "contenedorVentanaEdicionDetalles", "ventanaEdicionDetalles");
            }
        }
        /// <summary>
        /// Evento Producido al Editar los Conceptos de una Factura del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarConceptosFacturasLigadas_Click(object sender, EventArgs e)
        { 
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Inicializando el Control
                ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"]));

                //Mostrando Ventana
                TSDK.ASP.ScriptServer.AlternarVentana(this, "EdicionDetalles", "contenedorVentanaEdicionDetalles", "ventanaEdicionDetalles");
            }
        }
        /// <summary>
        /// Evento Producido al Agregar las Referencias de una Factura del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarReferencia_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Instanciando Factura
                using(SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"])))
                {
                    //Validando que Exista la Factura
                    if(fac.habilitar)
                    {
                        //Validando que exista el Servicio
                        if (fac.id_servicio > 0)

                            //Inicializando Control
                            ucReferenciasViaje.InicializaControl(fac.id_servicio);
                        else
                        {
                            //Instanciando Facturación Otros
                            using (SAT_CL.Facturacion.FacturacionOtros fo = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(fac.id_factura))
                            {
                                //Validando que exista el Registro
                                if (fo.habilitar)
                                
                                    //Inicializando Control
                                    ucReferenciasViaje.InicializaControl(fo.id_facturacion_otros, fo.id_compania_emisora, fo.id_cliente_receptor, 130);
                            }
                        }

                        //Mostrando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Agregar las Referencias de una Factura del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarReferenciaFacturasLigadas_Click(object sender, EventArgs e)
        { 
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"])))
                {
                    //Validando que Exista la Factura
                    if (fac.habilitar)
                    {
                        //Validando que exista el Servicio
                        if (fac.id_servicio > 0)

                            //Inicializando Control
                            ucReferenciasViaje.InicializaControl(fac.id_servicio);
                        else
                        {
                            //Instanciando Facturación Otros
                            using (SAT_CL.Facturacion.FacturacionOtros fo = SAT_CL.Facturacion.FacturacionOtros.ObtieneInstanciaFactura(fac.id_factura))
                            {
                                //Validando que exista el Registro
                                if (fo.habilitar)

                                    //Inicializando Control
                                    ucReferenciasViaje.InicializaControl(fo.id_facturacion_otros, fo.id_compania_emisora, fo.id_cliente_receptor, 130);
                            }
                        }

                        //Mostrando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                    }
                }
            }
        }
        /// <summary>
        /// Evento encargado de Guardar los Conceptos de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Concepto
            result = ucFacturadoConcepto.GuardarFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Eliminar los Conceptos de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucFacturadoConcepto.EliminaFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarEdicionConceptos_Click(object sender, EventArgs e)
        {
            //Deacuerdo a la vista Activa de Fcaturas Disponibles
            if (mtvFacturas.ActiveViewIndex == 0)
            {
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
                //Cargamos Facturas
                cargaFacturas();
                //Cargamos totales del proceso
                obtieneTotalesFacturaGlobal();
            }
            else
            {
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
            }
            
            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(this, "EdicionDetalles", "contenedorVentanaEdicionDetalles", "ventanaEdicionDetalles");
        }
        /// <summary>
        /// Evento Producido al Guardar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// /// Evento Producido al Eliminar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarReferencias_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
        }
        /// <summary>
        /// Evento que cierra la ventana modal de encabezado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarEncabezadoServicio_Click(object sender, EventArgs e)
        {
            //Cerrando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(this, "CerrarVentana", "encabezadoServicioModal", "encabezadoServicio");
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

        }
        /// <summary>
        /// Evento Producido al Guardar las Referencias del Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEncabezadoServicio_ClickGuardarReferencia(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Referencias
            result = wucEncabezadoServicio.GuardaEncabezadoServicio();

            //Carga Facturas Disponibles
            cargaFacturas();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
        }
        #endregion

        #region Eventos GridView "Facturas Ligadas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacLigadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacLigadas.SelectedValue), true, 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Validando el Estatus de la Factura
                if (validaEstatusPaquete())
                {
                    //Instanciando Registro
                    using (SAT_CL.Facturacion.PaqueteProcesoDetalle pp = new SAT_CL.Facturacion.PaqueteProcesoDetalle(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdPPD"])))
                    {
                        //Validando que exista el Registro
                        if (pp.id_paquete_proceso_detalle > 0)
                        
                            //Deshabilitando Registro
                            result = pp.DeshabilitaPaqueteProcesoDetalle(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }

                    //Validando que la Operación fuese exitosa
                    if (result.OperacionExitosa)
                    {
                        //Cargando Todas las Facturas
                        cargaFacturas();

                        //Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                
                //Mostrando Mensaje de Error
                lblError.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Link "Ver Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando la Factura Global
                using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                {
                    //Cargando los Conceptos Disponibles de la Factura
                    using (DataTable dtConceptos = SAT_CL.Facturacion.PaqueteProcesoDetalle.CargaConceptosLigadosFactura(Convert.ToInt32(Session["id_registro"]),
                                                                Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"]), pp.id_cliente))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                        {
                            //Cargando GridView de Conceptos
                            TSDK.ASP.Controles.CargaGridView(gvFacturaConceptos, dtConceptos, "IdFacturaConcepto", "", true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["SD"], dtConceptos, "Table2");
                        }
                        else
                        {
                            //Inicializando GridView de Conceptos
                            TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["SD"], "Table2");
                        }
                    }

                    //Obteniendo Control de Encabezado
                    CheckBox chkHeader = (CheckBox)gvFacturaConceptos.HeaderRow.FindControl("chkTodosConceptos");

                    //Validando que se Obtuvo el Control
                    if (chkHeader != null)
                        //Deshabilitando Control
                        chkHeader.Enabled = false;

                    //Ocultando Control
                    btnGuardarConceptos.Visible = false;

                    //Inicializando GridView de Conceptos
                    TSDK.ASP.Controles.InicializaIndices(gvFacturaConceptos);

                    //Abriendo Ventana Modal de Conceptos
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasLigadas, upgvFacturasLigadas.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true , 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacLigadas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);

            //Invocando Método de Suma de Totales
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacLigadas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //VALIDA LOS ESTATUS DE LA PÁGINA
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estatus de la página es nuevo
                case Pagina.Estatus.Nuevo:
                {
                    //Validando el Tipo de Registro
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //Obteniendo Controles
                        CheckBox chkNoEnt = (CheckBox)e.Row.FindControl("chkNoEntregado");
                        CheckBox chkRech = (CheckBox)e.Row.FindControl("chkRechazado");
                        LinkButton lkbPorte = (LinkButton)e.Row.FindControl("lkbPorte");
                        lkbPorte.Enabled = true;
                        //Validando que existan los Controles
                        if (chkNoEnt != null && chkRech != null)
                        {
                            //Obteniendo Origen de la Fila
                            DataRowView rowView = (DataRowView)e.Row.DataItem;

                            //Obteniendo Valores
                            int noEntregado = 0, rech = 0;
                            int.TryParse(rowView["NoEntregado"].ToString(), out noEntregado);
                            int.TryParse(rowView["Rechazado"].ToString(), out rech);

                            //Asignando Valores
                            chkNoEnt.Checked = Convert.ToBoolean(noEntregado);
                            chkRech.Checked = Convert.ToBoolean(rech);
                            //Habilitando Controles
                            chkNoEnt.Enabled = chkNoEnt.Checked ? false : chkRech.Checked ? false : true;
                            chkRech.Enabled = chkRech.Checked ? false : chkNoEnt.Checked ? false : true;

                        }
                    }
                    break;
                }
                //Si el estatus de la página es edicion
                case Pagina.Estatus.Edicion:
                {
                    //nstancia a la clase Paquete Proceso
                    using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Valida que exista el registro
                        if (pp.id_paquete_proceso > 0)
                        {

                            //Valida el estado del paquete
                            if (pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado || pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado)
                            {
                                //Habilita las opciones del gridview
                                gvFacturasLigadas.Enabled = true;
                            }
                            else 
                            {
                                //Validando el Tipo de Registro
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    //Obtiene los link del gridview FacturasLigadas
                                    LinkButton lkbPorte =(LinkButton)e.Row.FindControl("lkbPorte"),
                                               lkbVerConcepto = (LinkButton)e.Row.FindControl("lnkVerConceptos"),
                                               lkbEliminar = (LinkButton)e.Row.FindControl("lnkEliminarFactura");
                                    //Obteniendo Controles checkbox
                                    CheckBox chkNoEnt = (CheckBox)e.Row.FindControl("chkNoEntregado"),
                                             chkRech = (CheckBox)e.Row.FindControl("chkRechazado");
                                    //habilitaControles los link y los checkbox
                                    lkbPorte.Enabled=true;
                                    lkbVerConcepto.Enabled=true;
                                    chkNoEnt.Enabled =
                                    chkRech.Enabled =
                                    lkbEliminar.Enabled=false;
                                                        
                                }
                            }
                          }
                        }
                    }
                    break;                
            }
        }

        #endregion

        #region Eventos GridView "Factura Conceptos"

        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacConceptos.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturaConceptos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturaConceptos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Enlazar un Origen de Datos con el GridView "Facturas Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosConceptos");

                //Validando que exista el Control
                if (chk != null)
                {
                    //Obteniendo Origen de la Fila
                    DataRowView rowView = (DataRowView)e.Row.DataItem;

                    //Obteniendo Indicador
                    int indicador = Convert.ToInt32(rowView["Indicador"].ToString() == "" ? "1" : rowView["Indicador"].ToString());

                    //Validando Indicador
                    switch (indicador)
                    {
                        case 0:
                            {
                                //Habilitando Control
                                chk.Enabled = true;
                                break;
                            }
                        case 1:
                            {
                                //Deshabilitando Control
                                chk.Enabled = false;
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar el Control CheckBox "Todos o Varios" del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosConceptos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturaConceptos.DataKeys.Count > 0)
            {
                //Obteniendo Control Disparador
                CheckBox chk = (CheckBox)sender;
                CheckBox chkVarios;

                //Validando el Control
                switch (chk.ID)
                {
                    case "chkTodosConceptos":
                        {
                            //Validando que la fila sea seleccionada
                            if (!chk.Checked)
                            {
                                //Seleccionando Todas las Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturaConceptos, "chkVariosConceptos", chk.Checked);

                                //Visualizando Control
                                btnGuardarConceptos.Visible = btnGuardarConceptos.Enabled = false;
                            }
                            else
                            {
                                //Recorriendo cada fila
                                foreach (GridViewRow gvr in gvFacturaConceptos.Rows)
                                {
                                    //Obteniendo Control
                                    chkVarios = (CheckBox)gvr.FindControl("chkVariosConceptos");

                                    //Validando que existe la Fila
                                    if (chkVarios != null)

                                        //Validando que el Control este Habilitado para su Seleccion
                                        chkVarios.Checked = chkVarios.Enabled ? chk.Enabled : false;
                                }

                                //Visualizando Control de Guardado
                                btnGuardarConceptos.Visible = chk.Checked;
                            }

                            break;
                        }
                    case "chkVariosConceptos":
                        {
                            //Obteniendo Control de ENcabezado
                            CheckBox chkHeader = (CheckBox)gvFacturaConceptos.HeaderRow.FindControl("chkTodosConceptos");

                            //Validando que el control se haya desmarcado
                            if (!chk.Checked)

                                //Desmarcando Encabezado
                                chkHeader.Checked = false;

                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturaConceptos, "chkVariosConceptos");

                            //Validando que existan filas seleccionadas
                            if (gvr.Length > 0)

                                //Visualizando Control de Guardado
                                btnGuardarConceptos.Visible = true;
                            else
                                //Ocultando Control de Guardado
                                btnGuardarConceptos.Visible = false;

                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFacConceptos.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacConceptos_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }

        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Abriendo Ventana Modal de Conceptos
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");

            //Limpiando Control
            lblError.Text = "";
            
            //Inicializando GridViews
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno y Variables Auxiliares
            RetornoOperacion result = new RetornoOperacion();
            string[] mensaje_operacion = new string[1];
            int contador = 0;

            //Validando Estatus del Paquete
            if (validaEstatusPaquete())
            {
                //Obteniendo Filas Seleccionadas
                GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validand que existan Conceptos
                    if (gvr.Length > 0)
                    {
                        //Creando Arreglo Dinamico
                        mensaje_operacion = new string[gvr.Length];

                        //Iniciando Ciclo
                        foreach (GridViewRow gv in gvr)
                        {
                            //Seleccionando Indice
                            gvFacturasDisponibles.SelectedIndex = gv.RowIndex;

                            //Insertando Registro de Relación
                            result = SAT_CL.Facturacion.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                        0, false, false, DateTime.MinValue, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación haya Sido Incorrecta
                            if (!result.OperacionExitosa)
                            {
                                //Obteniendo Mensaje de Error
                                mensaje_operacion = new string[1];
                                mensaje_operacion[1] = result.Mensaje;

                                //Terminando Ciclo
                                break;
                            }
                            else
                            {
                                //Guardando Mensaje de la Operación
                                mensaje_operacion[contador] = "La Factura " + gvFacturasDisponibles.SelectedDataKey["IdFactura"].ToString() + ": Se ha Agregado Exitosamente";

                                //Incrementando Contador
                                contador++;
                            }
                        }

                        //Validando que las Operaciones hayan sido exitosas
                        if (result.OperacionExitosa)
                        {
                            //Guardando el Registro
                            int idReg = result.IdRegistro;

                            //Personalizando Mensaje
                            result = new RetornoOperacion(idReg, "La Factura " + gvFacturasDisponibles.SelectedDataKey["IdFactura"].ToString() + ": Se ha Agregado Exitosamente", true);

                            //Cargando Facturas
                            cargaFacturas();

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No hay Conceptos Seleccionados");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

            //Mostrando Mensaje de Operación
            lblError.Text = string.Join("<br />", mensaje_operacion);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarConceptos_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Filas Seleccionadas
            GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturaConceptos, "chkVariosConceptos");

            //Validando estatus de Paquete
            if (validaEstatusPaquete())
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validand que existan Conceptos
                    if (gvr.Length > 0)
                    {
                        //Iniciando Ciclo
                        foreach (GridViewRow gv in gvr)
                        {
                            //Seleccionando Indice
                            gvFacturaConceptos.SelectedIndex = gv.RowIndex;

                            //Insertando Registro de Relación
                            result = SAT_CL.Facturacion.PaqueteProcesoDetalle.InsertaPaqueteProcesoDetalle(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                    Convert.ToInt32(gvFacturaConceptos.SelectedDataKey["IdFacturaConcepto"]), false, false, DateTime.MinValue, "",
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación haya Sido Incorrecta
                            if (!result.OperacionExitosa)

                                //Terminando Ciclo
                                break;
                        }

                        //Validando que las Operaciones hayan sido exitosas
                        if (result.OperacionExitosa)
                        {
                            //Cargando Facturas
                            cargaFacturas();

                            //Completando Transacción
                            trans.Complete();
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No hay Conceptos Seleccionados");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");

            //Abriendo Ventana Modal de Conceptos
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnGuardarConceptos, upbtnGuardarConceptos.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;
        }

        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion
            if (Convert.ToInt32(Session["id_registro"]) > 0)
            {
                //Determinando la pestaña pulsada
                switch (((Button)sender).CommandName)
                {
                    case "FacturasDisponibles":
                        //Cambiando estilos de pestañas
                        btnPestanaFacturasDisponibles.CssClass = "boton_pestana_activo";
                        btnPestanaFacturasLigadas.CssClass = "boton_pestana";
                        //Asignando vista activa de la forma
                        mtvFacturas.SetActiveView(vwFacturasDisponibles);
                        break;
                    case "FacturasLigadas":
                        //Cambiando estilos de pestañas
                        btnPestanaFacturasDisponibles.CssClass = "boton_pestana";
                        btnPestanaFacturasLigadas.CssClass = "boton_pestana_activo";
                        //Asignando vista activa de la forma
                        mtvFacturas.SetActiveView(vwFacturasLigadas);
                        break;                        
                }

                //Cargamos Facturas
                cargaFacturas();
            }
        }

        /// <summary>
        /// Click en botón de importación desde archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbImportarArchivo_Click(object sender, EventArgs e)
        {
            //Si hay un registro activo en sesión
            if (Convert.ToInt32(Session["id_registro"]) > 0)
                //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("ImportadorTarifaCobro.aspx?idPaqueteRevision={0}", Session["id_registro"]), "Importación de Tarifa de Cobro", 1080, 620, false, false, false, true, true, true, Page);
        }
           
        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];

            //Obteniendo Compania
            using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que Exista la Compania
                if (cer.id_compania_emisor_receptor > 0)

                    //Asignando Compania
                    txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompania.Text = "";
            }
            
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();

            //Poner el cursor en el primer control
            ddlTipoProceso.Focus();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo de Estatus de Proceso
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 3141);
            //Cargando Catalogo de Tipo de Proceso
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoProceso, "", 3140);
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacConceptos, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacDisp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacLigadas, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbAcuseABC.Enabled =
                        lkbAcuceLily.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = 
                        lkbAcuseProcter.Enabled=
                        lkbAcuseSchindler.Enabled=
                        lkbAcuseColgate.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbAcuseABC.Enabled =
                        lkbAcuceLily.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled =
                        lkbAcuseProcter.Enabled =
                        lkbAcuseSchindler.Enabled =
                        lkbAcuseColgate.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbAcuseABC.Enabled =
                        lkbAcuceLily.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        lkbAcuseProcter.Enabled =
                        lkbAcuseSchindler.Enabled =
                        lkbAcuseColgate.Enabled = true;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignando Valores
                        ddlTipoProceso.Enabled =
                        txtCliente.Enabled =
                        txtFechaInicio.Enabled =
                        txtUsuarioResp.Enabled =
                        txtReferencia.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        txtFacturaGlobal.Enabled = 
                        btnAgregarFG.Enabled = 
                        txtPaqPrevio.Enabled =
                        btnAgregarPQ.Enabled =
                        txtReferenciaServicio.Enabled =
                        txtCartaPorteBusqueda.Enabled = 
                        btnBuscarFacturas.Enabled = false;
                        break;
                    }

                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Valores
                        ddlTipoProceso.Enabled =
                        txtCliente.Enabled =
                        txtFechaInicio.Enabled =
                        txtUsuarioResp.Enabled =
                        txtReferencia.Enabled =
                        txtFacturaGlobal.Enabled =
                        btnAgregarFG.Enabled =
                        txtPaqPrevio.Enabled =
                        btnAgregarPQ.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        txtReferenciaServicio.Enabled =
                        txtCartaPorteBusqueda.Enabled = 
                        btnBuscarFacturas.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Valores
                        ddlTipoProceso.Enabled =
                        txtCliente.Enabled =
                        txtFechaInicio.Enabled =
                        txtUsuarioResp.Enabled =
                        txtReferencia.Enabled =
                        txtFacturaGlobal.Enabled =
                        btnAgregarFG.Enabled =
                        txtPaqPrevio.Enabled =
                        btnAgregarPQ.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = false;
                        txtReferenciaServicio.Enabled =
                        txtCartaPorteBusqueda.Enabled = 
                        btnBuscarFacturas.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Valores
                        lblNoPaquete.Text = "Por Asignar";
                        txtCliente.Text =
                        txtFechaFin.Text = 
                        txtReferencia.Text = 
                        txtUsuarioResp.Text = "";
                        txtReferenciaServicio.Text = "";
                        txtCartaPorteBusqueda.Text = "";
                        txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);
                        TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);
                        TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                        //Asignando Valores
                        lblSubtotal.Text = string.Format("{0:0.00}", 0);
                        lblTrasladado.Text = string.Format("{0:0.00}", 0);
                        lblRetenido.Text = string.Format("{0:0.00}", 0);
                        lblTotal.Text = string.Format("{0:0.00}", 0);
                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Producto
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Inicializando Valores
                                lblNoPaquete.Text = pp.consecutivo_compania.ToString();
                                txtFechaInicio.Text = pp.fecha_inicio.ToString("dd/MM/yyyy HH:mm");
                                txtFechaFin.Text = pp.fecha_fin == DateTime.MinValue ? "" : pp.fecha_fin.ToString("dd/MM/yyyy HH:mm");
                                ddlEstatus.SelectedValue = pp.id_estatus.ToString();
                                ddlTipoProceso.SelectedValue = pp.id_tipo_proceso.ToString();
                                txtReferencia.Text = pp.referencia;
                                txtReferenciaServicio.Text = "";
                                txtCartaPorteBusqueda.Text = "";

                                //Instanciando Cliente
                                using (SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(pp.id_cliente))
                                {
                                    //Validando que Exista el Registro
                                    if (cli.id_compania_emisor_receptor > 0)

                                        //Asignando Cliente
                                        txtCliente.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Control
                                        txtCliente.Text = "";
                                }

                                //Instanciando Cliente
                                using (SAT_CL.Seguridad.Usuario user = new SAT_CL.Seguridad.Usuario(pp.id_usuario_responsable))
                                {
                                    //Validando que Exista el Registro
                                    if (user.id_usuario > 0)

                                        //Asignando Responsable
                                        txtUsuarioResp.Text = user.nombre + " ID:" + user.id_usuario.ToString();
                                    else
                                        //Limpiando Control
                                        txtUsuarioResp.Text = "";
                                }

                                //Validando Estatus
                                switch (pp.estatus)
                                {
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado:
                                        {
                                            //Cambiando estilos de pestañas
                                            btnPestanaFacturasDisponibles.CssClass = "boton_pestana_activo";
                                            btnPestanaFacturasLigadas.CssClass = "boton_pestana";
                                            //Asignando vista activa de la forma
                                            mtvFacturas.SetActiveView(vwFacturasDisponibles);
                                            break;
                                        }
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Aceptado:
                                    case SAT_CL.Facturacion.PaqueteProceso.Estatus.Rechazado:
                                        {
                                            //Cambiando estilos de pestañas
                                            btnPestanaFacturasDisponibles.CssClass = "boton_pestana";
                                            btnPestanaFacturasLigadas.CssClass = "boton_pestana_activo";
                                            //Asignando vista activa de la forma
                                            mtvFacturas.SetActiveView(vwFacturasLigadas);
                                            break;
                                        }
                                }

                                //Cargando Facturas
                                cargaFacturas();

                                //Configurando Controles
                                configuraControlesEstatus(pp.estatus);
                            }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar el Proceso del Paquete
        /// </summary>
        private void guardaPaqueteProceso()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha de Inicio
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;
            DateTime.TryParse(txtFechaInicio.Text, out fec_ini);
            DateTime.TryParse(txtFechaFin.Text, out fec_fin);

            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertando Registro
                        result = SAT_CL.Facturacion.PaqueteProceso.InsertaPaqueteProceso((SAT_CL.Facturacion.PaqueteProceso.TipoProceso)Convert.ToInt32(ddlTipoProceso.SelectedValue),
                                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0")), 0,
                                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), fec_ini, fec_fin,
                                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUsuarioResp.Text, "ID:", 1, "0")),
                                                                            txtReferencia.Text,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Factura Global
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista una Factura Global
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Validando el Estatus
                                if (validaEstatusPaquete())

                                    //Insertando Registro
                                    result = pp.EditaPaqueteProceso((SAT_CL.Facturacion.PaqueteProceso.TipoProceso)Convert.ToInt32(ddlTipoProceso.SelectedValue),
                                                            (SAT_CL.Facturacion.PaqueteProceso.Estatus)Convert.ToInt32(ddlEstatus.SelectedValue),
                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1, "0")), pp.consecutivo_compania,
                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), fec_ini, fec_fin,
                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUsuarioResp.Text, "ID:", 1, "0")),
                                                            txtReferencia.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Estatus del Paquete no permite su Edición");
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede acceder al Paquete");
                        }

                        break;
                    }
            }

            //Validando que la Operación haya sido exitosa
            if (result.OperacionExitosa)
            {
                //Asignando valor a Session Registro
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Inicializando la Página
                inicializaPagina();
            }

            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método encargado de Configurar
        /// </summary>
        /// <param name="estatus"></param>
        private void configuraControlesEstatus(SAT_CL.Facturacion.PaqueteProceso.Estatus estatus)
        {
            //Validando el Estatus
            switch(estatus)
            {
                case SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado:
                    {
                        //Habilitando Controles
                        //gvFacturasDisponibles.Enabled =
                        //gvFacturasLigadas.Enabled = 
                        btnGuardarFactura.Enabled = true;
                        break;
                    }
                case SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado:
                    {
                        //Habilitando Controles
                        //gvFacturasDisponibles.Enabled =
                        btnGuardarFactura.Enabled = false;
                        //gvFacturasLigadas.Enabled = true;
                        break;
                    }
                default:
                    {
                        //Deshabilitando Controles
                        //gvFacturasDisponibles.Enabled =
                        //gvFacturasLigadas.Enabled =
                        btnGuardarFactura.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Validar que se pueda Editar el Registro con respecto al Estatus
        /// </summary>
        /// <returns></returns>
        private bool validaEstatusPaquete()
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Instanciando Paquete
            using(SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Existe el Registro
                if(pp.id_paquete_proceso > 0)
                {
                    //Validando el Estatus
                    switch (pp.estatus)
                    {
                        case SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado:                        
                            {
                                //Asignando Retorno Positivo
                                result = true;
                                break;
                            }
                        default:
                            {
                                //Asignando Retorno Negativo
                                result = false;
                                break;
                            }
                    }
                }
            }

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método encargado de Cargar las Facturas Globales dado una Compania y un Cliente
        /// </summary>
        private void cargaFacturasGlobales()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Factura Global
                        using(SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista el Paquete
                            if(pp.id_paquete_proceso > 0)
                            {
                                //Creando Script de Autocompletado
                                string scriptAutocompleta = @"<script type='text/javascript'>
                                                                //Añadiendo Función de Autocompletado al Control (Cliente)
                                                                $('#" + txtFacturaGlobal.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=34&param=" + pp.id_compania.ToString() + @"&param2=" + pp.id_cliente.ToString() + @"'});
                                                              </script>";

                                //Ejecutando Script
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaConfiguracionPaqueteFG", scriptAutocompleta, false);
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Cargar los Paquetes Terminados dado una Compania y un Cliente
        /// </summary>
        private void cargaPaquetesTerminados()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Factura Global
                        using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista el Paquete
                            if (pp.id_paquete_proceso > 0)
                            {
                                //Creando Script de Autocompletado
                                string scriptAutocompleta = @"<script type='text/javascript'>
                                                                //Añadiendo Función de Autocompletado al Control (Cliente)
                                                                $('#" + txtPaqPrevio.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=35&param=" + pp.id_compania.ToString() + @"&param2=" + pp.id_cliente.ToString() + @"'});
                                                              </script>";

                                //Ejecutando Script
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaConfiguracionPaquetePT", scriptAutocompleta, false);
                            }
                        }
                        break;
                    }
            }
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/FacturacionProceso.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);

            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";

            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/FacturacionProceso.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());

            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";

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
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/FacturacionProceso.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);

            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Cargar las Facturas
        /// </summary>
        public void cargaFacturas()
        {
            //Instanciando Factura Global
            using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista el Registro
                if (pp.habilitar)
                {
                    //Validando Estatus del Paquete
                    if (pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Registrado || pp.estatus == SAT_CL.Facturacion.PaqueteProceso.Estatus.Entregado)
                    {
                        //Obteniendo Facturas Disponibles
                        using (DataTable dtFacDisp = SAT_CL.Facturacion.PaqueteProcesoDetalle.CargaFacturasDisponibles(pp.id_paquete_proceso, pp.id_cliente,
                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtReferenciaServicio.Text, txtCartaPorteBusqueda.Text))
                        {
                            /** Facturas Disponibles **/
                            //Validando que existan Facturas Disponibles
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacDisp))
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvFacturasDisponibles, dtFacDisp, "IdFactura-IdServicio", lblOrdenadoFacDisp.Text, true, 2);

                                //Añadiendo Tabla a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacDisp, "Table");
                            }
                            else
                            {
                                //Inicializando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);

                                //Eliminando Tabla de Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);

                        //Eliminando Tabla de Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    }
                    
                    //Obteniendo Facturas Ligadas
                    using (DataTable dtFacLigadas = SAT_CL.Facturacion.PaqueteProcesoDetalle.CargaFacturasLigadas(pp.id_paquete_proceso))
                    {
                        /** Facturas Ligadas **/
                        //Validando que existan Facturas Ligadas
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacLigadas))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvFacturasLigadas, dtFacLigadas, "IdFactura-IdServicio-IdPPD", lblOrdenadoFacLigadas.Text, true, 1);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacLigadas, "Table1");
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        }
                    }

                    //Invocando Método de Suma de Totales
                    sumaTotalesFacturasLigadas();
                    sumaTotalesFacturasDisponibles();

                    //Cargando Totales
                    obtieneTotalesFacturaGlobal();
                }
                else
                {
                    //Inicializando GridViews
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);

                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener los Totales de la Factura Global
        /// </summary>
        private void obtieneTotalesFacturaGlobal()
        {
            //Obteniendo Totales
            using (DataTable dtTotales = SAT_CL.Facturacion.PaqueteProcesoDetalle.ObtieneTotalesProcesoPaquete(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtTotales.Rows)
                    {
                        //Asignando Valores
                        lblSubtotal.Text = string.Format("{0:C2}", dr["SubTotal"]);
                        lblTrasladado.Text = string.Format("{0:C2}", dr["TrasladadoFac"]);
                        lblRetenido.Text = string.Format("{0:C2}", dr["RetenidoFac"]);
                        lblTotal.Text = string.Format("{0:C2}", dr["Total"]);
                    }
                }
                else
                {
                    //Asignando Valores
                    lblSubtotal.Text = string.Format("{0:0.00}", 0);
                    lblTrasladado.Text = string.Format("{0:0.00}", 0);
                    lblRetenido.Text = string.Format("{0:0.00}", 0);
                    lblTotal.Text = string.Format("{0:0.00}", 0);
                }
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales (Facturas Disponibles)
        /// </summary>
        private void sumaTotalesFacturasDisponibles()
        {
            //Validando que exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                /*gvFacturasDisponibles.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvFacturasDisponibles.FooterRow.Cells[5].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TrasladadoFac)", "")));
                gvFacturasDisponibles.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(RetenidoFac)", "")));//*/
                gvFacturasDisponibles.FooterRow.Cells[17].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales
                /*gvFacturasDisponibles.FooterRow.Cells[4].Text = 
                gvFacturasDisponibles.FooterRow.Cells[5].Text = 
                gvFacturasDisponibles.FooterRow.Cells[6].Text = */
                gvFacturasDisponibles.FooterRow.Cells[17].Text = string.Format("{0:C2}", 0);
            }

        }
        /// <summary>
        /// Método encargado de Sumar los Totales (Facturas Ligadas)
        /// </summary>
        private void sumaTotalesFacturasLigadas()
        {
            //Validando que exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Mostrando Totales
                //gvFacturasLigadas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(SubTotal)", "")));
                gvFacturasLigadas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Flete)", "")));
                gvFacturasLigadas.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Otros)", "")));
                gvFacturasLigadas.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(SubTotal)", "")));
                gvFacturasLigadas.FooterRow.Cells[17].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[12].Text =
                gvFacturasLigadas.FooterRow.Cells[13].Text =
                gvFacturasLigadas.FooterRow.Cells[16].Text =
                gvFacturasLigadas.FooterRow.Cells[17].Text = string.Format("{0:C2}", 0);
            }
            //Establecemos Estilos del Grid View
            gvFacturasLigadas.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Right;
        }

        #endregion
    }
}