using SAT_CL.Seguridad;
using System;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.FacturacionElectronica33
{
    public partial class FacturacionOtrosV33 : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento producido al dar clic sobre algún elemento de menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Determinando el botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    //Limpiando Id de sesión
                    Session["id_registro"] = 0;
                    //Limpiando contenido de forma
                    inicializaPagina();
                    break;
                case "Abrir":
                    inicializaAperturaRegistro(130, false);
                    break;
                case "Guardar":
                    guardaFacturaOtros();
                    break;
                case "Editar":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    //Limpiando contenido de forma
                    inicializaPagina();
                    break;
                case "Cancelar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Factura de Otros
                        using (SAT_CL.Facturacion.FacturacionOtros fo = new SAT_CL.Facturacion.FacturacionOtros(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista el Registro
                            if (fo.id_facturacion_otros > 0)
                            {
                                //Instanciando Facturación
                                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(fo.id_facturado))
                                {
                                    //Validando que Exista
                                    if (fac.id_factura > 0)

                                        //Cancelando Factura
                                        result = fac.CancelaFactura(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }

                            //Validando que la Operación haya Sido Exitosa
                            if (result.OperacionExitosa)

                                //Inicializando Página
                                inicializaPagina();

                            //Mostrando Mensaje de Operación
                            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Eliminar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion result = new RetornoOperacion();

                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(IsolationLevel.ReadCommitted))
                        {
                            //Instanciando Factura de Otros
                            using (SAT_CL.Facturacion.FacturacionOtros fo = new SAT_CL.Facturacion.FacturacionOtros(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista el Registro
                                if (fo.id_facturacion_otros > 0)
                                {
                                    //Deshabilitando la relación de la Factura
                                    result = fo.DeshabilitaFacturacionOtros(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación fuese correcta
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Facturado
                                        using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(fo.id_facturado))
                                        {
                                            //Validando que Existe la Factura
                                            if (fac.id_factura > 0)
                                            {
                                                //Deshabilitando la Factura
                                                result = fac.DeshabilitaFactura(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación fuese Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Asignando estatus nuevo
                                                    Session["estatus"] = Pagina.Estatus.Nuevo;

                                                    //Limpiando contenido de forma
                                                    inicializaPagina();

                                                    //Completando Transacción
                                                    trans.Complete();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                case "Bitacora":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaBitacora(Session["id_registro"].ToString(), "130", "Facturación Otros");
                    break;
                case "Referencias":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "130", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                    break;
                case "Archivos":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaArchivosRegistro(Session["id_registro"].ToString(), "130", "0");
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //Obteniendo Comprobante
                    using (SAT_CL.FacturacionElectronica33.Comprobante cmp = new SAT_CL.FacturacionElectronica33.Comprobante(100))
                    using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Obteniendo PDF en Bytes
                        byte[] cfdi_pdf = cmp.GeneraPDFComprobantePagoV33();

                        //Descargando Archivo PDF
                        TSDK.Base.Archivo.DescargaArchivo(cfdi_pdf, string.Format("{0}_{1}{2}.pdf", cer.nombre_corto != "" ? cer.nombre_corto : cer.rfc, cmp.serie, cmp.folio), TSDK.Base.Archivo.ContentType.application_PDF);
                    }
                    //Construyendo URL 
                    //string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", "~/RDLC/Reporte.aspx");
                    //Instanciando nueva ventana de navegador para apertura de registro 
                    //TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobantePago", 100), "Comprobante de Pago", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                    //TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "DocumentosPago", 28307), "Documentos del Pago", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                    break;
                /*/
                case "Imprimir":
                    {
                        //Obteniendo Comprobante
                        using (SAT_CL.FacturacionElectronica33.Comprobante cmp = new SAT_CL.FacturacionElectronica33.Comprobante(100))
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Obteniendo PDF en Bytes
                            byte[] cfdi_pdf = cmp.GeneraPDFComprobantePagoV33();

                            //Descargando Archivo PDF
                            TSDK.Base.Archivo.DescargaArchivo(cfdi_pdf, string.Format("{0}_{1}{2}.pdf", cer.nombre_corto != "" ? cer.nombre_corto : cer.rfc, cmp.serie, cmp.folio), TSDK.Base.Archivo.ContentType.application_PDF);
                        }
                        //Construyendo URL 
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro 
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobantePago", 100), "Comprobante de Pago", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        //TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "DocumentosPago", 28307), "Documentos del Pago", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }//*/
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //
            guardaFacturaOtros();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Cambiando estatus a Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                    }
            }

            //Inicializando Página
            inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturado_ClickGuardarFactura(object sender, EventArgs e)
        {
            //Guardando Factura
            RetornoOperacion result = ucFacturado.GuardaFactura();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Conceptos
                ucFacturadoConcepto.InicializaControl(ucFacturado.idFactura);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Eliminando Factura Concepto
            RetornoOperacion result = ucFacturadoConcepto.EliminaFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Inicializando Conceptos
                ucFacturadoConcepto.InicializaControl(ucFacturado.idFactura);

                //Inicializando Control
                ucFacturado.InicializaControl(ucFacturado.idFactura, false);

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Guardando Factura Concepto
            RetornoOperacion result = ucFacturadoConcepto.GuardarFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Control
                ucFacturado.InicializaControl(ucFacturado.idFactura, true);

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Habilitando Menus
            habilitaMenus();

            //Habilitando Controles
            habilitaControles();

            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Habilita o deshabilita opciones del menú de la forma en base a su estatus actual
        /// </summary>
        private void habilitaMenus()
        {
            //Determinando el estatus de carga de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled = true;
                    lkbEditar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = false;
                    break;
                case Pagina.Estatus.Lectura:
                    lkbNuevo.Enabled =
                    lkbEditar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = true;
                    lkbGuardar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = true;
                    lkbEditar.Enabled = false;
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validando Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Asignando Valores
                        txtClienteReceptor.Enabled = true;
                        txtCompaniaEmisor.Enabled =
                        ucFacturado.Enabled =
                        ucFacturadoConcepto.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Valores
                        txtClienteReceptor.Enabled =
                        txtCompaniaEmisor.Enabled = false;
                        ucFacturado.Enabled =
                        ucFacturadoConcepto.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Valores
                        txtClienteReceptor.Enabled = true;
                        txtCompaniaEmisor.Enabled = false;
                        ucFacturado.Enabled =
                        ucFacturadoConcepto.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus de la Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Limpiando Controles
                        txtClienteReceptor.Text = "";

                        //Inicializando Controles
                        ucFacturado.InicializaControl(0, true);
                        ucFacturadoConcepto.InicializaControl(0);

                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Registro
                        using (SAT_CL.Facturacion.FacturacionOtros fo = new SAT_CL.Facturacion.FacturacionOtros(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Existe el Registro
                            if (fo.id_facturacion_otros > 0)
                            {
                                //Instanciando Compania Emisora
                                using (SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(fo.id_cliente_receptor))
                                {
                                    //Validando que Exista la Compania
                                    if (cli.id_compania_emisor_receptor > 0)

                                        //Asignando Valor
                                        txtClienteReceptor.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Control
                                        txtClienteReceptor.Text = "";
                                }

                                //Inicializando Controles de Usuario
                                ucFacturado.InicializaControl(fo.id_facturado, true);
                                ucFacturadoConcepto.InicializaControl(fo.id_facturado);
                            }
                        }

                        break;
                    }
            }

            //Instanciando Compania Emisora
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que Exista la Compania
                if (cer.id_compania_emisor_receptor > 0)

                    //Asignando Valor
                    txtCompaniaEmisor.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompaniaEmisor.Text = "";
            }
        }
        /// <summary>
        /// Método encargado de Guardar las Facturas de Otros Servicios
        /// </summary>
        private void guardaFacturaOtros()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Guardando Factura
                            result = ucFacturado.GuardaFactura();

                            //Validando que se haya Insertado la Factura
                            if (result.OperacionExitosa)
                            {
                                //Guardando Factura
                                int idFactura = result.IdRegistro;

                                //Insertando Facturación de Otros Servicios
                                result = SAT_CL.Facturacion.FacturacionOtros.InsertaFacturacionOtros(idFactura, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaEmisor.Text, "ID:", 1)),
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtClienteReceptor.Text, "ID:", 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que se insertara la Facturación de Otros
                                if (result.OperacionExitosa)

                                    //Completando Transacción
                                    trans.Complete();
                            }
                        }

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando la Factura de Otro Servicio
                        using (SAT_CL.Facturacion.FacturacionOtros fo = new SAT_CL.Facturacion.FacturacionOtros(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que Exista el Registro
                            if (fo.id_facturacion_otros > 0)
                            {
                                //Editando la Factura de Otro Servicio
                                result = fo.EditaFacturacionOtros(fo.id_facturado, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaEmisor.Text, "ID:", 1)),
                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtClienteReceptor.Text, "ID:", 1)), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }

                        break;
                    }
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando el Registro a Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;

                //Inicializando Página
                inicializaPagina();
            }

            //Asignando Mensaje de la Operación
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro solicitado
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de apertura de registros
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="maestros">True para mostrar sólo servicios maestros, False para mostrar todos excepto paestros</param>
        private void inicializaAperturaRegistro(int idTabla, bool maestros)
        {
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", string.Format("~/Accesorios/AbrirRegistro.aspx?P1={0}&P3={1}", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToByte(maestros)));
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de referencias del registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencias", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturacionOtrosV33.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }

        #endregion
    }
}