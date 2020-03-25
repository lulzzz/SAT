using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using SAT_CL;
using System.Globalization;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Drawing;
using System.IO;
using SAT_CL.Nomina;
using System.Text;
using System.Configuration;
using BarcodeLib;
using SAT_CL.Global;

namespace SAT.RDLC
{
    public partial class Reporte : System.Web.UI.Page
    {
        /// <summary>
        /// Almacena el reporte que debe de cargarse
        /// </summary>
        private static string tipoReporte;

        private ReportViewer rv = new ReportViewer();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Asignamos el tipo de reporte
                tipoReporte = Request.QueryString["idTipoReporte"];
                //Invocamos al metodo que inicialice nuestro reporte
                inicializaReporte();
            }

        }
        /// <summary>
        /// Metodo general de inicializacion del reporte
        /// </summary>
        private void inicializaReporte()
        {
            //Establecemos el modo de procesamiento del reporte
            rvReporte.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            //De acuerdo al tipo de reportes asignamos rdcl, parametros y sources
            switch (tipoReporte)
            {
                case "Porte":
                    inicializaReporteCartaPorte16122015();
                    break;
                case "HojaInstruccion":
                    inicializaReporteHojaInstruccion();
                    break;
                case "Comprobante":
                    inicializaReporteComprobante();
                    //inicializaReporteComprobante();
                    break;
                case "ComprobanteV33":
                    inicializaReporteComprobanteVersion33();
                    break;
                case "ComprobantePago":
                    inicializaReporteComprobantePago();
                    break;
                case "DocumentosPago":
                    inicializaPagoDocumento();
                    break;
                case "ValeDiesel":
                    inicializaReporteValeDiesel();
                    break;
                case "Liquidacion":
                    inicializaReporteLiquidacion();
                    break;
                case "ProcesoRevision":
                    inicializaReporteProcesoRevision();
                    break;
                case "Renuncia":
                    inicializaReporteRenuncia();
                    break;
                case "ContratoIndeterminado":
                    inicializaReporteContratoIndeterminado();
                    break;
                case "ContratoTiempoDefinido":
                    inicializaReporteContratoTiempoDefinido();
                    break;
                case "AcuseReciboFacturas":
                    inicializaReporteAcuseReciboFacturas();
                    break;
                case "CajaDevolucion":
                    inicializaReporteCajaDevolucion();
                    break;
                case "ComprobanteNomina":
                    inicializaReporteComprobanteNomina();
                    break;
                case "ComprobanteNominaN12":
                    inicializaReporteComprobanteNomina12();
                    break;
                case "ComprobanteNominaN12v33":
                    inicializaReporteComprobanteNomina12v33();
                    break;
                case "FichaIngreso":
                    inicializaReporteFichaIngreso();
                    break;
                case "Finiquito":
                    inicializaReporteFiniquito();
                    break;
                case "Finiquito12":
                    inicializaReporteFiniquito12();
                    break;
                case "Requisicion":
                    inicializaReporteRequisicion();
                    break;
                case "OrdenTrabajo":
                    inicializaReporteOrdenTrabajo();
                    break;
                case "OrdenCompra":
                    inicializaReporteOrdenCompra();
                    break;
                case "AcuseABC":
                    inicializaReporteAcuseABC();
                    break;
                case "AcuseLili":
                    inicializaReporteAcuseLili();
                    break;
                case "Etiqueta":
                    inicializaReporteEtiquetas();
                    break;
                case "BitacoraViaje":
                    inicializaReporteBitacoraViaje();
                    break;
                case "QRUnidad":
                    inicializaQRUnidad();
                    break;
                case "PorteViajera":
                    inicializaReporteCartaPorteViajera();
                    break;
                case "HojaDeInstruccion":
                    inicializaReporteHojaDeInstruccion();
                    break;
                case "EnvioPaquete":
                    inicializaReporteEnvioPaquete();
                    break;
                case "AcuseReciboFacturas2":
                    inicializaReporteAcuseReciboFacturas2();
                    break;
                case "GastosGenerales":
                    inicializaReporteGastosGenerales();
                    break;
                case "FacturaGlobal":
                    inicializaFacturaGlobal();
                    break;
            }

            //Refrescando Reporte
            rvReporte.LocalReport.Refresh();
            rvReporte.Visible = true;
        }
        /// <summary>
        /// Metodo que inicializa en forma especifica el reporte CartaPorte
        /// </summary>
        private void inicializaReporteCartaPorte()
        {
            //Creación de la tabla para cargar el logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obteniendo Servicio
            int idServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);

            //Invoca la clase Servicio y obtienen los datos de consulta.
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                //Obtiene referencia de  cliente para impresion especifica de carta porte
                string cartaPorte = TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_cliente_receptor, "Configuración Formatos de Impresión", "Formato Impresión"), "~/RDLC/Porte.rdlc");
                //Asignamos la ruta del reporte local
                rvReporte.LocalReport.ReportPath = Server.MapPath(cartaPorte);
                //Invoca a la clase compañiaEmisorReceptor 
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                {
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                }
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_compania_emisor, "Color Empresa", "Color"));
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { color });
            }
            //Obtenemos la informacion general del servicio
            using (DataTable t = SAT_CL.Documentacion.Servicio.CargaDatosPorte(idServicio), tdf = SAT_CL.Facturacion.FacturadoConcepto.CargaDetallesFacturaServicio(idServicio))
            {
                //Validamos que se hayan retornado valores validos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(t) && TSDK.Datos.Validacion.ValidaOrigenDatos(tdf))
                {
                    //Recuperamos  los valores y creamos los parametros
                    foreach (DataRow r in t.Rows)
                    {
                        ReportParameter nombreCompania = new ReportParameter("NombreCompania", r["NombreCompania"].ToString());
                        ReportParameter rfcCompania = new ReportParameter("RFCCompania", r["RFCCompania"].ToString());
                        ReportParameter direccionCompania = new ReportParameter("DireccionCompania", r["DireccionCompania"].ToString().ToUpper());
                        ReportParameter telefonoCompania = new ReportParameter("TelefonoCompania", r["TelefonoCompania"].ToString());
                        ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                        ReportParameter porte = new ReportParameter("Porte", r["Porte"].ToString());
                        ReportParameter nombreCliente = new ReportParameter("NombreCliente", r["NombreCliente"].ToString());
                        ReportParameter rfcCliente = new ReportParameter("RFCCliente", r["RFCCliente"].ToString());
                        ReportParameter direccionCliente = new ReportParameter("DireccionCliente", r["DireccionCliente"].ToString());
                        ReportParameter telefonoCliente = new ReportParameter("TelefonoCliente", r["TelefonoCliente"].ToString());
                        ReportParameter nombreRemitente = new ReportParameter("NombreRemitente", r["NombreRemitente"].ToString());
                        ReportParameter datosRemitente = new ReportParameter("DatosRemitente", r["DatosRemitente"].ToString());
                        ReportParameter citaRemitente = new ReportParameter("CitaRemitente", r["CitaRemitente"].ToString());
                        ReportParameter nombreDestinatario = new ReportParameter("NombreDestinatario", r["NombreDestinatario"].ToString());
                        ReportParameter datosDestinatario = new ReportParameter("DatosDestinatario", r["DatosDestinatario"].ToString());
                        ReportParameter citaDestinatario = new ReportParameter("CitaDestinatario", r["CitaDestinatario"].ToString());
                        ReportParameter productos = new ReportParameter("Productos", r["Productos"].ToString());
                        ReportParameter referencias = new ReportParameter("Referencias", r["Referencias"].ToString());
                        ReportParameter observacion = new ReportParameter("Observacion", r["Observacion"].ToString());
                        ReportParameter importe = new ReportParameter("Importe", TSDK.Base.Cadena.ConvierteMontoALetra(r["Importe"].ToString()));
                        ReportParameter fecha = new ReportParameter("Fecha", r["Fecha"].ToString());
                        ReportParameter nounidad = new ReportParameter("NoUnidad", r["NoUnidad"].ToString());
                        ReportParameter placas = new ReportParameter("Placas", r["Placas"].ToString());
                        ReportParameter operador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                        //Agregamos los parametros al reporte
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreCompania, rfcCompania, direccionCompania, telefonoCompania,
                         servicio, porte, nombreCliente, rfcCliente, direccionCliente, telefonoCliente, nombreRemitente, datosRemitente, citaRemitente,
                         nombreDestinatario, datosDestinatario, citaDestinatario, productos, referencias, observacion, fecha, importe, nounidad, operador,placas});
                    }
                    //Agregamos el origen de datos 
                    rvReporte.LocalReport.DataSources.Clear();
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
                    ReportDataSource rsDetalleFacturado = new ReportDataSource("DetalleFacturado", tdf);
                    rvReporte.LocalReport.DataSources.Add(rsDetalleFacturado);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvLogotipo);
                }
            }
        }
        /// <summary>
        /// Metodo que inicializa en forma especifica el reporte CartaPorte-Traslado Regularizacion 16/12/2015
        /// </summary>
        private void inicializaReporteCartaPorte16122015()
        {
            //Creación de la tabla para cargar el logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obteniendo Servicio
            int idServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idOperador, idUnidadM, idUnidadA1, idUnidadA2;
            idOperador = idUnidadM = idUnidadA1 = idUnidadA2 = 0;

            //Obteniendo Valores de la Petición
            int.TryParse(Request.QueryString["idRegistroB"], out idOperador);
            int.TryParse(Request.QueryString["idRegistroC"], out idUnidadM);
            int.TryParse(Request.QueryString["idRegistroD"], out idUnidadA1);
            int.TryParse(Request.QueryString["idRegistroE"], out idUnidadA2);

            //Invoca la clase Servicio y obtienen los datos de consulta.
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                //Obtiene referencia de  cliente para impresion especifica de carta porte
                bool incluirQR = Convert.ToBoolean(TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_cliente_receptor, "Configuración Formatos de Impresión", "Bit QR Fijo Carta Porte"), "True"));
                //Asignamos la ruta del reporte local
                rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Porte_16122015.rdlc");
                //Invoca a la clase compañiaEmisorReceptor 
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                {
                    //Creación del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                }
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_compania_emisor, "Color Empresa", "Color"));
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { color });

                //Obtenemos la informacion general del servicio
                using (DataTable t = SAT_CL.Documentacion.Servicio.CargaDatosPorteRegularizacion16122015(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2),
                    tdf = SAT_CL.Facturacion.FacturadoConcepto.CargaDetallesFacturaServicioR16122015(idServicio))
                {
                    //Validamos que se hayan retornado valores validos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(t) && TSDK.Datos.Validacion.ValidaOrigenDatos(tdf))
                    {
                        //Recuperamos  los valores y creamos los parametros
                        foreach (DataRow r in t.Rows)
                        {
                            ReportParameter nombreCompania = new ReportParameter("NombreCompania", r["NombreCompania"].ToString());
                            ReportParameter rfcCompania = new ReportParameter("RFCCompania", r["RFCCompania"].ToString());
                            ReportParameter direccionCompania = new ReportParameter("DireccionCompania", r["DireccionCompania"].ToString().ToUpper());
                            ReportParameter telefonoCompania = new ReportParameter("TelefonoCompania", r["TelefonoCompania"].ToString());
                            ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                            ReportParameter porte = new ReportParameter("Porte", r["Porte"].ToString());
                            ReportParameter nombreCliente = new ReportParameter("NombreCliente", r["NombreCliente"].ToString());
                            ReportParameter rfcCliente = new ReportParameter("RFCCliente", r["RFCCliente"].ToString());
                            ReportParameter direccionCliente = new ReportParameter("DireccionCliente", r["DireccionCliente"].ToString());
                            ReportParameter telefonoCliente = new ReportParameter("TelefonoCliente", r["TelefonoCliente"].ToString());
                            ReportParameter nombreRemitente = new ReportParameter("NombreRemitente", r["NombreRemitente"].ToString());
                            ReportParameter datosRemitente = new ReportParameter("DatosRemitente", r["DatosRemitente"].ToString());
                            ReportParameter citaRemitente = new ReportParameter("CitaRemitente", r["CitaRemitente"].ToString());
                            ReportParameter nombreDestinatario = new ReportParameter("NombreDestinatario", r["NombreDestinatario"].ToString());
                            ReportParameter datosDestinatario = new ReportParameter("DatosDestinatario", r["DatosDestinatario"].ToString());
                            ReportParameter citaDestinatario = new ReportParameter("CitaDestinatario", r["CitaDestinatario"].ToString());
                            ReportParameter productos = new ReportParameter("Productos", r["Productos"].ToString());
                            ReportParameter referencias = new ReportParameter("Referencias", r["Referencias"].ToString());
                            ReportParameter observacion = new ReportParameter("Observacion", r["Observacion"].ToString());
                            ReportParameter importe = new ReportParameter("Importe", TSDK.Base.Cadena.ConvierteMontoALetra(r["Importe"].ToString()));
                            ReportParameter fecha = new ReportParameter("Fecha", r["Fecha"].ToString());
                            ReportParameter nounidad = new ReportParameter("NoUnidad", r["NoUnidad"].ToString());
                            ReportParameter placas = new ReportParameter("Placas", r["Placas"].ToString());
                            ReportParameter noCaja1 = new ReportParameter("NoCaja1", r["NoCaja1"].ToString());
                            ReportParameter noCaja2 = new ReportParameter("NoCaja2", r["NoCaja2"].ToString());
                            ReportParameter plcCaja1 = new ReportParameter("PlcCaja1", r["PlcCaja1"].ToString());
                            ReportParameter plcCaja2 = new ReportParameter("PlcCaja2", r["PlcCaja2"].ToString());
                            ReportParameter operador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                            ReportParameter incluir_QR = new ReportParameter("incluirQR", incluirQR.ToString());
                            //Agregamos los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreCompania, rfcCompania, direccionCompania, telefonoCompania,
                                 servicio, porte, nombreCliente, rfcCliente, direccionCliente, telefonoCliente, nombreRemitente, datosRemitente, citaRemitente,
                                 nombreDestinatario, datosDestinatario, citaDestinatario, productos, referencias, observacion, fecha, importe, nounidad,
                                 noCaja1, noCaja2, plcCaja1, plcCaja2, operador, placas, incluir_QR});
                        }
                        //Agregamos el origen de datos 
                        rvReporte.LocalReport.DataSources.Clear();
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
                        ReportDataSource rsDetalleFactura = new ReportDataSource("DetalleFactura", tdf);
                        rvReporte.LocalReport.DataSources.Add(rsDetalleFactura);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvLogotipo);
                    }
                }
            }
        }
        /// <summary>
        /// Metodo que inicializa en forma especifica el reporte CartaPorte-Traslado Regularizacion 16/12/2015
        /// </summary>
        private void inicializaReporteBitacoraViaje()
        {
            //Creación de la tabla para cargar el logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obteniendo Servicio
            int idServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idOperador, idUnidadM, idUnidadA1, idUnidadA2;
            idOperador = idUnidadM = idUnidadA1 = idUnidadA2 = 0;

            //Obteniendo Valores de la Petición
            int.TryParse(Request.QueryString["idRegistroB"], out idOperador);
            int.TryParse(Request.QueryString["idRegistroC"], out idUnidadM);
            int.TryParse(Request.QueryString["idRegistroD"], out idUnidadA1);
            int.TryParse(Request.QueryString["idRegistroE"], out idUnidadA2);

            //Invoca la clase Servicio y obtienen los datos de consulta.
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                //Asignamos la ruta del reporte local
                rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/BitacoraViaje.rdlc");
                //Invoca a la clase compañiaEmisorReceptor 
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                {
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                }
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_compania_emisor, "Color Empresa", "Color"));
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { color });

                //Obtenemos la informacion general del servicio
                using (DataTable t = SAT_CL.Documentacion.Servicio.CargaDatosPorteRegularizacion16122015(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2))
                {
                    //Validamos que se hayan retornado valores validos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                    {
                        //Recuperamos  los valores y creamos los parametros
                        foreach (DataRow r in t.Rows)
                        {
                            //Asignando Parametros
                            ReportParameter nombreCompania = new ReportParameter("NombreCompania", r["NombreCompania"].ToString());
                            ReportParameter rfcCompania = new ReportParameter("RFCCompania", r["RFCCompania"].ToString());
                            ReportParameter direccionCompania = new ReportParameter("DireccionCompania", r["DireccionCompania"].ToString().ToUpper());
                            ReportParameter telefonoCompania = new ReportParameter("TelefonoCompania", r["TelefonoCompania"].ToString());
                            ReportParameter datosRemitente = new ReportParameter("DatosRemitente", r["DatosRemitente"].ToString());
                            ReportParameter datosDestinatario = new ReportParameter("DatosDestinatario", r["DatosDestinatario"].ToString());
                            ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                            ReportParameter citaRemitente = new ReportParameter("CitaRemitente", Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm"));
                            ReportParameter operador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                            ReportParameter placas = new ReportParameter("Placas", r["Placas"].ToString());
                            ReportParameter marca = new ReportParameter("Marca", r["Marca"].ToString());
                            ReportParameter modelo = new ReportParameter("ModeloUnidad", r["ModeloUnidad"].ToString());
                            ReportParameter licencia = new ReportParameter("NoLicencia", r["NoLicencia"].ToString());
                            ReportParameter vigencia = new ReportParameter("VigenciaLicencia", r["VigenciaLicencia"].ToString());

                            //Agregamos los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreCompania, rfcCompania, direccionCompania, telefonoCompania,
                                 servicio, datosRemitente, citaRemitente, datosDestinatario, operador, placas, marca, modelo, licencia, vigencia});
                        }

                        //Agregamos el origen de datos 
                        rvReporte.LocalReport.DataSources.Clear();
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvLogotipo);
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Inicializar el Reporte de Hoja de Instrucción
        /// </summary>
        private void inicializaReporteHojaInstruccion()
        {
            //Obteniendo Segmento
            string idSegmento = Request.QueryString["idRegistro"];
            string idHI = Request.QueryString["idRegistroB"];

            //Habilitando Imagenes Externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Cargando Reporte
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/HojaInstruccion.rdlc");

            //Declarando Arreglo de Parametros por Recibir
            ReportParameter[] param = new ReportParameter[15];

            //Obteniendo Impresión
            using (DataTable dtImpresion = SAT_CL.ControlEvidencia.HojaInstruccion.CargaImpresionHojaInstruccion(Convert.ToInt32(idSegmento)),
                    dtDocumentos = SAT_CL.ControlEvidencia.HojaInstruccionDocumento.ObtieneDocumentosHIImpresion(Convert.ToInt32(idHI)),
                        dtAccesorios = SAT_CL.ControlEvidencia.HojaInstruccionAccesorio.ObtieneHojaInstruccionAccesoriosParaImpresion(Convert.ToInt32(idHI)),
                            dtMapas = SAT_CL.ControlEvidencia.HojaInstruccion.CargaRutaMapas(Convert.ToInt32(idHI)))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtImpresion))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtImpresion.Rows)
                    {
                        //Creando Parametros
                        param[0] = new ReportParameter("TerminalCobro", dr["TerminalCobro"].ToString());
                        param[1] = new ReportParameter("CompaniaEmisor", dr["CompaniaEmisor"].ToString());
                        param[2] = new ReportParameter("Descripcion", dr["Descripcion"].ToString());
                        param[3] = new ReportParameter("Remitente", dr["Remitente"].ToString());
                        param[4] = new ReportParameter("Destinatario", dr["Destinatario"].ToString());
                        param[5] = new ReportParameter("ClienteReceptor", dr["ClienteReceptor"].ToString());
                        param[6] = new ReportParameter("NombreOperador", dr["NombreOperador"].ToString());
                        param[7] = new ReportParameter("NoUnidad", dr["NoUnidad"].ToString());
                        param[8] = new ReportParameter("IDOperador", dr["IDOperador"].ToString());
                        param[9] = new ReportParameter("PlacasUnidad", dr["PlacasUnidad"].ToString());
                        param[10] = new ReportParameter("ClienteRFC", dr["ClienteRFC"].ToString());
                        param[11] = new ReportParameter("CompaniaRFC", dr["CompaniaRFC"].ToString());
                        param[12] = new ReportParameter("DireccionTerminal", dr["DireccionTerminal"].ToString());
                        param[13] = new ReportParameter("DireccionDestinatario", dr["DireccionDestinatario"].ToString());
                        param[14] = new ReportParameter("DireccionRemitente", dr["DireccionRemitente"].ToString());
                    }
                }

                //Limpiando Origenes de Datos
                rvReporte.LocalReport.DataSources.Clear();

                //Declarando Variables Auxiliares
                using (DataTable dtMapaCarga = new DataTable())
                using (DataTable dtMapaDescarga = new DataTable())
                {
                    //Validando que existan los Mapas
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtMapas))
                    {
                        //Declarando Arreglos Auxiliares
                        byte[] mapasC = null, mapasD = null;

                        //Creando Columnas
                        dtMapaCarga.Columns.Add("Imagen", typeof(byte[]));
                        dtMapaDescarga.Columns.Add("Imagen", typeof(byte[]));

                        //Recorriendo Registro
                        foreach (DataRow dr in dtMapas.Rows)
                        {
                            //Obteniendo Bytes de Mapa de Carga
                            try { mapasC = System.IO.File.ReadAllBytes(dr["RutaMapaCarga"].ToString()); }
                            catch { mapasC = null; }

                            //Agregando Mapa de Carga
                            dtMapaCarga.Rows.Add(mapasC);

                            //Obteniendo Bytes de Mapa de Descarga
                            try { mapasD = System.IO.File.ReadAllBytes(dr["RutaMapaDescarga"].ToString()); }
                            catch { mapasD = null; }

                            //Agregando Mapa de Descarga
                            dtMapaDescarga.Rows.Add(mapasD);
                        }
                    }
                    else
                    {
                        //Creando Columnas y Registros
                        dtMapaCarga.Columns.Add("Image", typeof(byte[]));
                        dtMapaCarga.Rows.Add(new byte[0]);
                        //Copiando Mapa
                        dtMapaDescarga.Columns.Add("Image", typeof(byte[]));
                        dtMapaDescarga.Rows.Add(new byte[0]);
                    }

                    //Agregamos el origen de datos del mapa de Carga
                    ReportDataSource rsMapaCarga = new ReportDataSource("ImagenMapaCarga", dtMapaCarga);
                    rvReporte.LocalReport.DataSources.Add(rsMapaCarga);

                    //Agregamos el origen de datos del mapa de Descarga
                    ReportDataSource rsMapaDescarga = new ReportDataSource("ImagenMapaDescarga", dtMapaDescarga);
                    rvReporte.LocalReport.DataSources.Add(rsMapaDescarga);
                }

                //Declarando Variables Auxiliares
                DataTable dtDocsCarga = new DataTable();
                DataTable dtDocsDescarga = new DataTable();
                byte[] fileImage = null;

                //Creando Columnas
                dtDocsCarga.Columns.Add("Documento", typeof(string));
                dtDocsCarga.Columns.Add("Accion", typeof(string));
                dtDocsCarga.Columns.Add("Formato", typeof(string));
                dtDocsCarga.Columns.Add("Sello", typeof(string));
                dtDocsCarga.Columns.Add("Observacion", typeof(string));
                dtDocsCarga.Columns.Add("Imagen", typeof(byte[]));

                //Copiando Tabla
                dtDocsDescarga = dtDocsCarga.Copy();

                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDocumentos))
                {
                    //Recorriendo Documentos
                    foreach (DataRow dr in dtDocumentos.Rows)
                    {
                        try
                        {
                            //Obteniendo Arreglo de Bytes
                            fileImage = System.IO.File.ReadAllBytes(dr["URL"].ToString());
                        }
                        catch (Exception e)
                        {
                            fileImage = null;
                        }
                        //Validando el Tipo de Evento
                        if (dr["Evento"].ToString() == "Carga")

                            //Insertando Documento de Carga
                            dtDocsCarga.Rows.Add(dr["Documento"].ToString(), dr["Accion"].ToString(), dr["Formato"].ToString(),
                                                 dr["Sello"].ToString(), dr["Observacion"].ToString(), fileImage);

                        else if (dr["Evento"].ToString() == "Descarga")

                            //Insertando Documento de Descarga
                            dtDocsDescarga.Rows.Add(dr["Documento"].ToString(), dr["Accion"].ToString(), dr["Formato"].ToString(),
                                                    dr["Sello"].ToString(), dr["Observacion"].ToString(), fileImage);
                    }
                }

                //Validando que Existen Documentos de Carga
                if (!TSDK.Datos.Validacion.ValidaOrigenDatos(dtDocsCarga))

                    //Insertando Documento Vacio de Carga
                    dtDocsCarga.Rows.Add("", "", "", "", "", null);

                //Validando que Existen Documentos de Descarga
                if (!TSDK.Datos.Validacion.ValidaOrigenDatos(dtDocsDescarga))

                    //Insertando Documento Vacio de Descarga
                    dtDocsDescarga.Rows.Add("", "", "", "", "", null);

                //Agregamos el origen de datos de Carga
                ReportDataSource rsHIDocumentosCarga = new ReportDataSource("HIDocumentosCarga", dtDocsCarga);
                rvReporte.LocalReport.DataSources.Add(rsHIDocumentosCarga);

                //Agregamos el origen de datos de Descarga
                ReportDataSource rsHIDocumentosDescarga = new ReportDataSource("HIDocumentosDescarga", dtDocsDescarga);
                rvReporte.LocalReport.DataSources.Add(rsHIDocumentosDescarga);

                //Instanciando Logo
                using (DataTable dtCompaniaLogo = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(25, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 19, 0))
                {
                    //Declarando Arreglo Auxiliar
                    byte[] logo = null;

                    //Declarando Tabla Auxiliar
                    using (DataTable dtLogoCFDI = new DataTable())
                    {
                        //Añadiendo Columnas
                        dtLogoCFDI.Columns.Add("Logotipo", typeof(byte[]));

                        //Validando que Exista el Logo
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCompaniaLogo))
                        {
                            //Recorriendo Registro
                            foreach (DataRow dr in dtCompaniaLogo.Rows)
                            {
                                //Obteniendo Arreglo de Bytes
                                try { logo = System.IO.File.ReadAllBytes(dr["URL"].ToString()); }
                                catch { logo = null; }
                            }
                        }

                        //Añadiendo Registro
                        dtLogoCFDI.Rows.Add(logo);

                        //Agregamos el origen de datos 
                        ReportDataSource rsLogotipoCFDI = new ReportDataSource("Logotipo", dtLogoCFDI);
                        rvReporte.LocalReport.DataSources.Add(rsLogotipoCFDI);
                    }
                }

                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAccesorios))
                {
                    //Agregamos el origen de datos 
                    ReportDataSource rsHIAccesorios = new ReportDataSource("HIAccesorios", dtAccesorios);
                    rvReporte.LocalReport.DataSources.Add(rsHIAccesorios);
                }
            }

            //Asignando Parametros
            this.rvReporte.LocalReport.SetParameters(param);
        }
        /// <summary>
        /// Método que permite la realización del reporte comprobante CFDI
        /// </summary>
        private void inicializaReporteComprobante()
        {
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));

            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Creación de la variable idComprobante
            int idComprobante = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Comprobante.rdlc");
            //Carga Conceptos del Comprobante
            using (DataTable Concepto = SAT_CL.FacturacionElectronica.Concepto.CargaImpresionConceptos(idComprobante))
            {
                //Agregar origen de datos 
                ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDI", Concepto);
                //Asigna los valores al conjunto de datos
                rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
            }
            //Instanciar el Comprobante
            using (SAT_CL.FacturacionElectronica.Comprobante objComprobante = new SAT_CL.FacturacionElectronica.Comprobante(idComprobante))
            {
                //Valida el estatus del comprobante.
                if (objComprobante.estatus_comprobante.Equals(SAT_CL.FacturacionElectronica.Comprobante.EstatusComprobante.Cancelado))
                {
                    //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                    ReportParameter estatusComprobante = new ReportParameter("EstatusComprobante", "CANCELADO");
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusComprobante });
                }
                //En caso contrario no envia nada al parametro estatusComprobante
                else
                {
                    ReportParameter estatusComprobante = new ReportParameter("EstatusComprobante", "");
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusComprobante });
                }

                //Asignamos valor a las variables
                serie_descargapdf = objComprobante.serie;
                folio_descargapdf = objComprobante.folio.ToString();
                //Intsanciamos Compania Emisor
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_emisor))
                {
                    //Asignamos valor a las variables
                    rfc_descargapdf = emisor.rfc;
                    nombrecorto_descargapdf = emisor.nombre_corto;
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(objComprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);

                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                    rvReporte.LocalReport.DataSources.Add(rvscod);

                    //Asigna el origen de datos a los parametros, obtenidos de la instancia a la clase compañiaEmisorReceptor
                    ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                    ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                    //Instancia a la clase Direccion para obtener la dirección del emisor
                    using (SAT_CL.Global.Direccion direm = new SAT_CL.Global.Direccion(objComprobante.id_direccion_emisor))
                    {
                        //Asigna valores a los parametros obtendos de la instancia a la clase Dirección.
                        ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", direm.ObtieneDireccionCompleta());
                        ReportParameter direccionEmisorSucursal = new ReportParameter("DireccionEmisorSucursal", direm.ObtieneDireccionCompleta());
                        //Asigna valores a los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, direccionEmisorMatriz, direccionEmisorSucursal });
                    }
                }
                //Instancia a la compania Receptor
                using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_receptor))
                {   //Asigna valores a los parametros obtendos de la instancia a la clase companiaEmisorReceptor
                    ReportParameter razonSocialReceptorCFDI = new ReportParameter("RazonSocialReceptorCFDI", receptor.nombre);
                    ReportParameter rfcReceptorCFDI = new ReportParameter("RFCReceptorCFDI", receptor.rfc);
                    //Obtiene la dirección del receptor
                    using (SAT_CL.Global.Direccion dirre = new SAT_CL.Global.Direccion(objComprobante.id_direccion_receptor))
                    {
                        //Asigna valores a los parametros obtenidos de la instancia a la clase Direccion
                        ReportParameter direccionReceptorCFDI = new ReportParameter("DireccionReceptorCFDI", dirre.ObtieneDireccionCompleta());
                        //Asigna valores a los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialReceptorCFDI, rfcReceptorCFDI, direccionReceptorCFDI });
                    }
                }
                //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                SAT_CL.FacturacionElectronica.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(objComprobante.id_comprobante);
                //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);

                string cadenaOriginal = "";

                TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(objComprobante.ruta_xml, Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"),
                                                                 Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), out cadenaOriginal);


                ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                //Asigna valores a los parametros del reporteComprobante
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });


                //Instancia para la obtencion de los datos de sucursal
                using (SAT_CL.Global.Sucursal suc = new SAT_CL.Global.Sucursal(objComprobante.id_sucursal))
                {
                    //Asigna los valores obtenidos de la instanca a la clase sucursal
                    ReportParameter sucursalCFDI = new ReportParameter("SucursalCFDI", suc.nombre);
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { sucursalCFDI });
                }
                //Instanciamos a la clase Certificado
                using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(objComprobante.id_certificado))
                {
                    //Cargando certificado (.cer)
                    using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                    {
                        //Asigna los valores instanciados a los parametros
                        ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                        //Asigna valores a los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                    }
                }
                //Instancia a la clase Cuenta bancos 
                using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos(objComprobante.id_cuenta_pago))
                {
                    if (objComprobante.id_cuenta_pago == 0 || cb.num_cuenta == "NO IDENTIFICADO")
                    {
                        //Asigna los valores de la clase cuentaBancos a los parametros
                        ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", "No Identificado");
                        //Asigna valores de los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                    }
                    else
                    {
                        //Obtiene el bit que mostrara la cuenta completa o no
                        string cuentaBancoCompleta = SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_receptor, "Leyendas Impresión CFD", "Bit Cuenta Banco Completa");
                        //Valida si la cuentaBancoCompleta es true, muestra la cuenta completa tal y como se dio de alta en el sistema
                        if (cuentaBancoCompleta == "TRUE")
                        {
                            //Asigna los valores de la clase cuentaBancos a los parametros
                            ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", cb.num_cuenta);
                            //Asigna valores de los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        }
                        //En caso contrario solo muestra los ultimos 4 digitos
                        else
                        {
                            //Asigna los valores de la clase cuentaBancos a los parametros
                            ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.InvierteCadena(cb.num_cuenta).Substring(0, 4)));
                            //Asigna valores de los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        }
                    }

                    //Si es Bit de banco Cunta Pagoes true
                    string BitBanco = SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_receptor, "Leyendas Impresión CFD", "Bit Banco Cuenta Pago");
                    if (BitBanco == "TRUE" || BitBanco == "true")
                    {

                        //Instanciamos Cuenta
                        using (SAT_CL.Bancos.Banco objBanco = new SAT_CL.Bancos.Banco(cb.id_banco))
                        {
                            //Si la cadena es vacia muestra la descripción del método de págo
                            ReportParameter bancoCFDI = new ReportParameter("BancoCFDI", objBanco.nombre_corto);
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { bancoCFDI });
                        }


                    }
                    //En caso de que el bit código método de págo no sea true mostrara la descripción del método de pago
                    else
                    {
                        ReportParameter bancoCFDI = new ReportParameter("BancoCFDI", " ");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { bancoCFDI });
                    }

                }
                //Asigna los valores de la clase comprobante a los parametros 
                ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", objComprobante.fecha_expedicion.ToString());
                ReportParameter serieCFDI = new ReportParameter("SerieCFDI", objComprobante.serie);
                ReportParameter folio = new ReportParameter("Folio", objComprobante.folio.ToString());
                ReportParameter formaPagoCFDI = new ReportParameter("FormaPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1099, objComprobante.id_forma_pago));

                ReportParameter totalCFDI = new ReportParameter("TotalCFDI", objComprobante.total_moneda_captura.ToString());
                ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Facturacion Electronica", "Regimen Fiscal"));
                ReportParameter leyendaImpresionCFDI1 = new ReportParameter("LeyendaImpresionCFDI1", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD1"));
                ReportParameter leyendaImpresionCFDI2 = new ReportParameter("LeyendaImpresionCFDI2", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Parametros Impresión CFD", "Total Comprobante") == "SI" ? SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2").Replace("TotalComprobante", objComprobante.total_moneda_nacional.ToString() + " (" + TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_moneda_nacional.ToString()) + ") ") : SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2"));
                ReportParameter comentario = new ReportParameter("Comentario", SAT_CL.Global.Referencia.CargaReferencia("0", 119, objComprobante.id_comprobante, "Facturacion Electrónica", "Comentario"));
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Color Empresa", "Color"));
                ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", objComprobante.subtotal_moneda_captura.ToString());
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,regimenFiscalCFDI,formaPagoCFDI,
                                                                                  totalCFDI,leyendaImpresionCFDI1, leyendaImpresionCFDI2, comentario, subtotalCFDI, color});
                //Obtiene la referencia bit código método de pago; permite definir si se mostrara el código o la descripción del método de págo definido por el sat.
                string codigo = SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_receptor, "Leyendas Impresión CFD", "Bit Código Método Pago");
                //Obtienen el valor cadena del método de págo (Codigo del método de págo definido por el SAT).
                string metodoPago = SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, objComprobante.id_metodo_pago);
                int moneda = objComprobante.id_moneda;
                switch (moneda)
                {
                    case 1:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_moneda_captura.ToString()));
                            ReportParameter mon = new ReportParameter("Mon", "");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, mon, tipoMoneda, figPeso, figEuro });
                            break;
                        }
                    case 2:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_moneda_captura.ToString(), "DÓLARES", "USD"));
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "USD");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                    case 3:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_moneda_captura.ToString(), "EUROS", "EUR"));
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "EUR");
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "€");
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                }
                //Si es codigo es true
                if (codigo == "TRUE" || codigo == "true")
                {
                    //Valida el valor de la cadadena   
                    if (metodoPago == "")
                    {
                        //Si la cadena es vacia muestra la descripción del método de págo
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, objComprobante.id_metodo_pago));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                    //En caso Contrario Muetra el Código 
                    else
                    {
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", metodoPago);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                }
                //En caso de que el bit código método de págo no sea true mostrara la descripción del método de pago
                else
                {
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, objComprobante.id_metodo_pago));
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                }
                //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                SAT_CL.FacturacionElectronica.Impuesto imp = (SAT_CL.FacturacionElectronica.Impuesto.RecuperaImpuestoComprobante(objComprobante.id_comprobante));

                //Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
                using (DataTable detalleImp = SAT_CL.FacturacionElectronica.DetalleImpuesto.CargaDetallesImpuesto(imp.id_impuesto))
                {
                    //Declarando variables auxiliares para recuperar impuestos
                    decimal totalIvaR = 0, totalIsr = 0, totalIvaT = 0, totalIeps = 0, tasaIvaT = 0, tasaIeps = 0;

                    //Si hay impuestos agregados al comprobante
                    if (imp.id_impuesto > 0)
                    {
                        //Asigna a las variablesel valor del desglose de impuesto 
                        totalIvaR = (from DataRow r in detalleImp.Rows
                                     where r.Field<int>("IdImpuestoRetenido") == 2
                                     select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                        totalIsr = (from DataRow r in detalleImp.Rows
                                    where r.Field<int>("IdImpuestoRetenido") == 1
                                    select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                        totalIvaT = (from DataRow r in detalleImp.Rows
                                     where r.Field<int>("IdImpuestoTrasladado") == 3
                                     select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                        totalIeps = (from DataRow r in detalleImp.Rows
                                     where r.Field<int>("IdImpuestoTrasladado") == 4
                                     select r.Field<decimal>("ImporteMonedaCaptura")).FirstOrDefault();
                        tasaIeps = (from DataRow r in detalleImp.Rows
                                    where r.Field<int>("IdImpuestoTrasladado") == 4
                                    select r.Field<decimal>("Tasa")).FirstOrDefault();
                        tasaIvaT = (from DataRow r in detalleImp.Rows
                                    where r.Field<int>("IdImpuestoTrasladado") == 3
                                    select r.Field<decimal>("Tasa")).FirstOrDefault();
                    }

                    //Asignación de valores a los parametros                            
                    ReportParameter EtiquetaTrasladado = new ReportParameter("EtiquetaTrasladado", totalIvaT.ToString() != "0" ? "I.V.A. Tras " + TSDK.Base.Cadena.TruncaCadena(tasaIvaT.ToString(), 3, "") + " %" : "IEPS " + TSDK.Base.Cadena.TruncaCadena(tasaIeps.ToString(), 3, "") + " %");
                    ReportParameter EtiquetaRetenido = new ReportParameter("EtiquetaRetenido", totalIvaR.ToString() != "0" ? "I.V.A. Ret" : "ISR");
                    ReportParameter Trasladado = new ReportParameter("Trasladado", totalIvaT.ToString() != "0" ? totalIvaT.ToString() : totalIeps.ToString());
                    ReportParameter Retenido = new ReportParameter("Retenido", totalIsr.ToString() != "0" ? totalIsr.ToString() : totalIvaR.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { EtiquetaTrasladado, EtiquetaRetenido, Trasladado, Retenido });
                }
            }
            //Carga SubInforma
            this.rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", nombrecorto_descargapdf != "" ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);


        }
        /// <summary>
        /// Método que permite la carga de datos para Comprobante CFDI Versión 3.3
        /// </summary>
        private void inicializaReporteComprobanteVersion33()
        {
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));

            //Habilita las imagenes externas
            this.rvReporte.LocalReport.EnableExternalImages = true;
            //Creación de la variable idComprobante
            int idComprobante = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Asignación de la ubicación del reporte local
            this.rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Comprobante_CFDI33.rdlc");

            //Carga Conceptos del Comprobante
            using (DataTable Concepto = SAT_CL.FacturacionElectronica33.Concepto.ObtieneConceptosComprobante(idComprobante))
            {
                //Validando Concepto
                if (Validacion.ValidaOrigenDatos(Concepto))
                {
                    //Agregar origen de datos 
                    ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDIV33", Concepto);
                    //Asigna los valores al conjunto de datos
                    this.rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
                }
                else
                    using (DataTable tabla = new DataTable())
                    {
                        tabla.Columns.Add("Id", typeof(int));
                        tabla.Columns.Add("IdComprobante", typeof(int));
                        tabla.Columns.Add("Cantidad", typeof(decimal));
                        tabla.Columns.Add("ClaveUnidad", typeof(int));
                        tabla.Columns.Add("Unidad", typeof(string));
                        tabla.Columns.Add("ClaveServProd", typeof(int));
                        tabla.Columns.Add("Concepto", typeof(string));
                        tabla.Columns.Add("NoIdentificacion", typeof(string));
                        tabla.Columns.Add("ValorUnitario", typeof(decimal));
                        tabla.Columns.Add("ImporteMonedaCaptura", typeof(decimal));
                        tabla.Columns.Add("ImporteMonedaNacional", typeof(decimal));
                        tabla.Columns.Add("Descuento", typeof(decimal));
                        tabla.Columns.Add("Impuesto", typeof(string));
                        //Insertar Registros
                        DataRow row = tabla.NewRow();
                        row["Id"] = 0;
                        row["IdComprobante"] = 0;
                        row["Cantidad"] = 0;
                        row["ClaveUnidad"] = 0;
                        row["Unidad"] = "";
                        row["ClaveServProd"] = 0;
                        row["Concepto"] = "";
                        row["NoIdentificacion"] = "";
                        row["ValorUnitario"] = 0;
                        row["ImporteMonedaCaptura"] = 0;
                        row["ImporteMonedaNacional"] = 0;
                        row["Descuento"] = 0;
                        row["Impuesto"] = "";
                        tabla.Rows.Add(row);
                        ReportDataSource rsComprobanteCFDI = new ReportDataSource("ComprobanteCFDIV33", tabla);
                        this.rvReporte.LocalReport.DataSources.Add(rsComprobanteCFDI);
                    }
            }
            //Instanciar el Comprobante
            using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(idComprobante))
            {
                //Valida el estatus del comprobante.
                if (objComprobante.id_estatus_vigencia.Equals((byte)SAT_CL.FacturacionElectronica33.Comprobante.EstatusVigencia.Cancelado))
                {
                    //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                    ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "CANCELADO");
                    this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                }
                //En caso contrario no envia nada al parametro estatusComprobante
                else
                {
                    ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "");
                    this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                }
                //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                using (SAT_CL.FacturacionElectronica33.Impuesto imp = (SAT_CL.FacturacionElectronica33.Impuesto.ObtieneImpuestoComprobante(objComprobante.id_comprobante33)))
                using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(objComprobante.id_forma_pago))
                using (SAT_CL.FacturacionElectronica33.TipoComprobante tc = new SAT_CL.FacturacionElectronica33.TipoComprobante(objComprobante.id_tipo_comprobante))
                {
                    //Asignando Parametros
                    ReportParameter formaPago = new ReportParameter("FormaPagoCFDI", string.Format("{0} - {1}", fp.clave, fp.descripcion));
                    ReportParameter tipoCFDI = new ReportParameter("TipoComprobante", string.Format("{0} - {1}", tc.clave, tc.descripcion));
                    //Creando Parametros de Totales
                    ReportParameter subTotal = new ReportParameter("SubTotal", objComprobante.subtotal_captura.ToString("C2"));
                    ReportParameter impTras = new ReportParameter("ImpTrasladado", imp.total_trasladado_captura.ToString("C2"));
                    ReportParameter impRet = new ReportParameter("ImpRetenido", imp.total_retenido_captura.ToString("C2"));
                    ReportParameter descuento = new ReportParameter("Descuento", objComprobante.descuentos_captura.ToString("C2"));
                    ReportParameter total = new ReportParameter("Total", objComprobante.total_captura.ToString("C2"));
                    //Asignando Valores de Parametros
                    this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { formaPago, tipoCFDI, subTotal, impTras, impRet, descuento, total });
                }

                /*/Carga Subtotal e impuestos del Comprobante
                using (DataTable SubTotal = SAT_CL.FacturacionElectronica33.Concepto.ObtineSubtotalComprobante(idComprobante))
                {
                    //Validando Subtotal(es)
                    if (Validacion.ValidaOrigenDatos(SubTotal))
                    {
                        //Agregar origen de datos 
                        ReportDataSource rsTotalesCFDI = new ReportDataSource("TotalesGeneralesCFDIV33", SubTotal);
                        //Asigna los valores al conjunto de datos
                        this.rvReporte.LocalReport.DataSources.Add(rsTotalesCFDI);
                    }
                    else
                        using (DataTable tabla = new DataTable())
                        {
                            tabla.Columns.Add("SubTotal", typeof(decimal));
                            tabla.Columns.Add("Descuento", typeof(decimal));
                            tabla.Columns.Add("ImpuestoTrasladado", typeof(decimal));
                            tabla.Columns.Add("ImpuestoRetenido", typeof(decimal));
                            tabla.Columns.Add("Total", typeof(decimal));
                            //Insertar Registros
                            DataRow row = tabla.NewRow();
                            row["SubTotal"] = 0;
                            row["Descuento"] = 0;
                            row["ImpuestoTrasladado"] = 0;
                            row["ImpuestoRetenido"] = 0;
                            row["Total"] = 0;
                            tabla.Rows.Add(row);
                            ReportDataSource rsTotalesCFDI = new ReportDataSource("TotalesGeneralesCFDIV33", tabla);
                            this.rvReporte.LocalReport.DataSources.Add(rsTotalesCFDI);
                        }
                }//*/

                //Asignamos valor a las variables
                serie_descargapdf = objComprobante.serie;
                folio_descargapdf = objComprobante.folio.ToString();
                //Intsanciamos Compania Emisor
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_emisor))
                {
                    //Asignamos valor a las variables
                    rfc_descargapdf = emisor.rfc;
                    nombrecorto_descargapdf = emisor.nombre_corto;
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(objComprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    this.rvReporte.LocalReport.DataSources.Add(rvs);
                    this.rvReporte.LocalReport.DataSources.Add(rvscod);

                    //Asigna el origen de datos a los parametros, obtenidos de la instancia a la clase compañiaEmisorReceptor
                    ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                    ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                    //Instancia a la clase Direccion para obtener la dirección del emisor
                    using (SAT_CL.Global.Direccion direm = new SAT_CL.Global.Direccion(objComprobante.id_direccion_lugar_expedicion))
                    {
                        //Asigna valores a los parametros obtendos de la instancia a la clase Dirección.
                        ReportParameter direccionEmisorSucursal = new ReportParameter("DireccionEmisorSucursal", direm.codigo_postal);
                        //Asigna valores a los parametros del reporteComprobante
                        this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, direccionEmisorSucursal });
                    }
                }
                //Instancia a la compania Receptor
                using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(objComprobante.id_compania_receptor))
                using (SAT_CL.Global.Direccion dirRec = new SAT_CL.Global.Direccion(receptor.id_direccion))
                {
                    //Variable Auxiliar
                    string razon_social = "";
                    string variable = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Código Postal CFDIv3.3", objComprobante.id_compania_emisor);

                    //Validando Configuración de Código Postal
                    if (Convert.ToInt32(variable.Equals("") ? "0" : variable) > 0)

                        //Asignando Valor a la Razón Social
                        razon_social = string.Format("{0} CP:{1}", receptor.nombre, dirRec.codigo_postal);
                    else
                        //Asignando solo Razón Social
                        razon_social = receptor.nombre;

                    //Asigna valores a los parametros obtendos de la instancia a la clase companiaEmisorReceptor
                    ReportParameter razonSocialReceptorCFDI = new ReportParameter("RazonSocialReceptorCFDI", razon_social);
                    ReportParameter rfcReceptorCFDI = new ReportParameter("RFCReceptorCFDI", receptor.rfc);
                    this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialReceptorCFDI, rfcReceptorCFDI });
                }
                //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                SAT_CL.FacturacionElectronica33.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(objComprobante.id_comprobante33);
                //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);

                string cadenaOriginal = "";

                TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(objComprobante.ruta_xml, System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                 System.Web.HttpContext.Current.Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cadenaOriginal);


                ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                //Asigna valores a los parametros del reporteComprobante
                this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });

                //Instanciamos a la clase Certificado
                using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(objComprobante.id_certificado))
                {
                    //Cargando certificado (.cer)
                    using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                    {
                        //Asigna los valores instanciados a los parametros
                        ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                        //Asigna valores a los parametros del reporteComprobante
                        this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                    }
                }
                //Asigna los valores de la clase comprobante a los parametros 
                ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", objComprobante.fecha_expedicion.ToString());
                ReportParameter serieCFDI = new ReportParameter("SerieCFDI", objComprobante.serie);
                ReportParameter folio = new ReportParameter("Folio", objComprobante.folio.ToString());
                ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCadenaValor(3197, objComprobante.regimen_fiscal));
                ReportParameter leyendaImpresionCFDI1 = new ReportParameter("LeyendaImpresionCFDI1", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD1"));
                ReportParameter leyendaImpresionCFDI2 = new ReportParameter("LeyendaImpresionCFDI2", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Parametros Impresión CFD", "Total Comprobante") == "SI" ? SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2").Replace("TotalComprobante", objComprobante.total_nacional.ToString() + " (" + TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_nacional.ToString()) + ") ") : SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Leyendas Impresión CFD", "Leyenda al Pie CFD2"));
                ReportParameter comentario = new ReportParameter("Comentario", SAT_CL.Global.Referencia.CargaReferencia("0", 209, objComprobante.id_comprobante33, "Facturacion Electrónica", "Comentario"));
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_emisor, "Color Empresa", "Color"));
                string uso_cfdi = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, objComprobante.id_uso_receptor);
                ReportParameter usoCFDI = new ReportParameter("UsoCFDI", uso_cfdi);
                //Asigna valores a los parametros del reporteComprobante
                this.rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,regimenFiscalCFDI,
                                                                          leyendaImpresionCFDI1, leyendaImpresionCFDI2, comentario,
                                                                          usoCFDI, color});

                //Obtiene la referencia bit código método de pago; permite definir si se mostrara el código o la descripción del método de págo definido por el sat.
                string codigo = SAT_CL.Global.Referencia.CargaReferencia("0", 25, objComprobante.id_compania_receptor, "Leyendas Impresión CFD", "Bit Código Método Pago");
                //Obtienen el valor cadena del método de págo (Codigo del método de págo definido por el SAT).
                string metodoPago = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago);
                int moneda = objComprobante.id_moneda;
                //
                switch (moneda)
                {
                    case 1:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString()));
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "MXN");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, mon, tipoMoneda, figPeso, figEuro });
                            break;
                        }
                    case 2:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString(), "DÓLARES", "USD"));
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "USD");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "$");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "");
                            this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                    case 3:
                        {
                            ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(objComprobante.total_captura.ToString(), "EUROS", "EUR"));
                            ReportParameter tipoMoneda = new ReportParameter("TipoMoneda", "EUR");
                            ReportParameter mon = new ReportParameter("Mon", "MONEDA:");
                            ReportParameter figPeso = new ReportParameter("FigPeso", "");
                            ReportParameter figEuro = new ReportParameter("FigEuro", "€");
                            this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { importeLetrasCFDI, tipoMoneda, mon, figPeso, figEuro });
                            break;
                        }
                }
                //Si es codigo es true
                if (codigo == "TRUE" || codigo == "true")
                {
                    //Valida el valor de la cadadena
                    if (metodoPago == "")
                    {
                        //Si la cadena es vacia muestra la descripción del método de págo
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", string.Format("{0} - {1}", SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, objComprobante.id_metodo_pago), SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago)));
                        this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                    //En caso Contrario Muetra el Código 
                    else
                    {
                        ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", metodoPago);
                        this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                    }
                }
                //En caso de que el bit código método de págo no sea true mostrara la descripción del método de pago
                else
                {
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", string.Format("{0} - {1}", SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, objComprobante.id_metodo_pago), SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, objComprobante.id_metodo_pago)));
                    this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { metodoPagoCFDI });
                }
            }
            //Carga SubInforma
            ReportParameter idcomprobante = new ReportParameter("IdComprobante", idComprobante.ToString());
            this.rvReporte.LocalReport.SetParameters(new ReportParameter[] { idcomprobante });
            this.rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandlerCompRel);

            //ReportViewer1.LocalReport.SetParameters(reportParameterCollection);

            //Carga SubInforma
            this.rvReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler33);

            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", !nombrecorto_descargapdf.Equals("") ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);
        }
        /// <summary>
        /// Método encargado de inicializar el Reporte del Comprobante de Pago
        /// </summary>
        private void inicializaReporteComprobantePago()
        {
            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Creando Columnas
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Datos del Nombre del Archivo de Descarga
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";

            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Creación de la variable idComprobante
            int idComprobante = Convert.ToInt32(Request.QueryString["idRegistro"]);

            //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
            ReportDataSource rvs, rvscod, rdsPagos, rdsConcepto;

            //Asignación de la ubicación del reporte local
            rvReporte.Reset();
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ComprobantePagoCFDI.rdlc");

            //Instanciando Comprobante 3.3
            using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(idComprobante))
            using (SAT_CL.FacturacionElectronica33.TimbreFiscalDigital tfd = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comp.id_comprobante33))
            {
                //Validando que el Comprobante este Habilitado
                if (comp.habilitar)// && tfd.habilitar)
                {
                    //Asignando parametro
                    ReportParameter cfdi_pago = new ReportParameter("IdCfdiPago", comp.id_comprobante33.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cfdi_pago });

                    //Valida el estatus del comprobante.
                    if (comp.id_estatus_vigencia.Equals(SAT_CL.FacturacionElectronica33.Comprobante.EstatusVigencia.Cancelado))
                    {
                        //Si el estatus es cancelado  envia al parametro estatusComprobante la leyenda CANCELADO
                        ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "CANCELADO");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                    }
                    //En caso contrario no envia nada al parametro estatusComprobante
                    else
                    {
                        ReportParameter estatusVigencia = new ReportParameter("EstatusVigencia", "");
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { estatusVigencia });
                    }

                    //Declarando Tabla Vacia
                    using (DataTable dtConceptoPago = new DataTable("Pagos"))
                    {
                        //Definiendo Columnas
                        dtConceptoPago.Columns.Add("Id", typeof(int));
                        dtConceptoPago.Columns.Add("ClaveProdServ", typeof(string));
                        dtConceptoPago.Columns.Add("Cantidad", typeof(decimal));
                        dtConceptoPago.Columns.Add("ValorUnitario", typeof(decimal));
                        dtConceptoPago.Columns.Add("Concepto", typeof(string));
                        dtConceptoPago.Columns.Add("ImporteMonedaCaptura", typeof(string));
                        dtConceptoPago.Columns.Add("ClaveUnidad", typeof(string));

                        //Insertando Unica Fila
                        dtConceptoPago.Rows.Add(1, "84111506", 1, 0, "PAGO", 0, "ACT");

                        //Asignando Concepto
                        rdsConcepto = new ReportDataSource("ComprobanteCFDIV33", dtConceptoPago);
                    }

                    //Obteniendo Pagos
                    using (DataTable dtPagos = SAT_CL.FacturacionElectronica33.EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(comp.id_comprobante33))
                    {
                        //Validando Datos
                        if (Validacion.ValidaOrigenDatos(dtPagos))

                            //Asignando Pagos
                            rdsPagos = new ReportDataSource("Pagos", dtPagos);
                        else
                        {
                            //Declarando Tabla Vacia
                            using (DataTable dtP = new DataTable("Pagos"))
                            {
                                //Definiendo Columnas
                                dtP.Columns.Add("Id", typeof(int));
                                dtP.Columns.Add("FormaPago", typeof(string));
                                dtP.Columns.Add("Fecha", typeof(string));
                                dtP.Columns.Add("Monto", typeof(decimal));
                                dtP.Columns.Add("Moneda", typeof(string));
                                dtP.Columns.Add("NumOperacion", typeof(string));
                                dtP.Columns.Add("RfcEmisorCtaBen", typeof(string));
                                dtP.Columns.Add("CtaBeneficiario", typeof(string));
                                dtP.Columns.Add("RfcEmisorCtaOrd", typeof(string));
                                dtP.Columns.Add("CtaOrdenante", typeof(string));

                                //Añadiendo Fila en Vacio
                                dtP.Rows.Add(0, "", "", 0.00M, "", "", "", "", "", "");

                                //Asignando Pagos
                                rdsPagos = new ReportDataSource("Pagos", dtP);
                            }
                        }
                    }

                    //Declarando Parametros
                    ReportParameter razonSocial, rfcEmisor, ColorImp, regimenFiscal, fechaComprobante, fechaCFDI, serieFolio,
                                    lugarExpedicion, uuid, certificadoSerieSAT, certificadoEmisor,
                                    cadenaOriginal, selloDigital, selloDigitalSAT, razonSocialRec, rfcReceptor, usoCFDI;

                    //Creació del arreglo necesario para la carga de la ruta del Código QR
                    byte[] imagenQR = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagenQR = System.IO.File.ReadAllBytes(comp.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagenQR = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagenQR);

                    //Asignando Atributos del Comprobante
                    fechaComprobante = new ReportParameter("FechaComprobanteCFDI", comp.fecha_expedicion.ToString("dd/MM/yyyy HH:mm"));
                    fechaCFDI = new ReportParameter("FechaCFDI", tfd.fecha_timbrado.ToString("dd/MM/yyyy HH:mm"));
                    serieFolio = new ReportParameter("SerieFolioCFDI", string.Format("{0}-{1}", comp.serie, comp.folio));
                    uuid = new ReportParameter("UUID", tfd.UUID);
                    certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", tfd.no_certificado);
                    serie_descargapdf = comp.serie;
                    folio_descargapdf = comp.folio;

                    //Obteniendo Cadena Orignal
                    string cad_ori = "";

                    //Validando Timbrado
                    if (tfd.habilitar)
                    {
                        //Obteniendo Datos
                        SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comp.ruta_xml, Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                             Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cad_ori);

                        //Instanciamos a la clase Certificado
                        using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comp.id_certificado))
                        {
                            //Cargando certificado (.cer)
                            using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))

                                //Asigna los valores instanciados a los parametros
                                certificadoEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                        }
                    }
                    else
                        //Asigna los valores instanciados a los parametros
                        certificadoEmisor = new ReportParameter("CertificadoDigitalEmisor", "");

                    //Asignando Datos
                    cadenaOriginal = new ReportParameter("CadenaOriginalCFDI", cad_ori);
                    selloDigital = new ReportParameter("SelloDigitalCFDI", comp.sello);
                    selloDigitalSAT = new ReportParameter("SelloDigitalSatCFDI", tfd.sello_SAT);



                    //Instanciando Emisor y Receptor
                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_emisor),
                            rec = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_receptor))
                    using (SAT_CL.Global.Direccion dirEmi = new SAT_CL.Global.Direccion(emi.id_direccion))
                    using (SAT_CL.Global.Direccion dirRec = new SAT_CL.Global.Direccion(rec.id_direccion))
                    {
                        //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                        byte[] logotipo = null;
                        //Permite capturar errores en caso de que no exista una ruta para el archivo
                        try
                        {
                            //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                            logotipo = System.IO.File.ReadAllBytes(emi.ruta_logotipo);
                        }
                        //En caso de que no exista una imagen, se devolvera un valor nulo.
                        catch { logotipo = null; }
                        //Agrega a la tabla un registro con valor a la ruta de la imagen.
                        dtLogo.Rows.Add(logotipo);

                        //Asignando Emisor
                        razonSocial = new ReportParameter("RazonSocialEmisorCFDI", emi.nombre);
                        rfcEmisor = new ReportParameter("RFCEmisorCFDI", emi.rfc);
                        ColorImp = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emi.id_compania_emisor_receptor, "Color Empresa", "Color"));
                        regimenFiscal = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3197, emi.id_regimen_fiscal));
                        lugarExpedicion = new ReportParameter("LugarExpedicion", dirEmi.codigo_postal);
                        nombrecorto_descargapdf = emi.nombre_corto;

                        //Asignando Receptor
                        razonSocialRec = new ReportParameter("RazonSocialReceptorCFDI", rec.nombre);
                        rfcReceptor = new ReportParameter("RFCReceptorCFDI", rec.rfc);
                        string uso_cfdi = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, comp.id_uso_receptor);
                        usoCFDI = new ReportParameter("UsoCFDI", string.Format("{0} - {1}", SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3194, comp.id_uso_receptor), uso_cfdi));
                    }

                    //Asignación de Parametros
                    rvReporte.LocalReport.SetParameters(new ReportParameter[]{ razonSocial, rfcEmisor, ColorImp, regimenFiscal, fechaComprobante,
                                                                               fechaCFDI, serieFolio, lugarExpedicion, uuid, certificadoSerieSAT,
                                                                               certificadoEmisor, cadenaOriginal, selloDigital, selloDigitalSAT, razonSocialRec,
                                                                               rfcReceptor, usoCFDI });

                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen
                    rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                    rvReporte.LocalReport.DataSources.Add(rvscod);
                    rvReporte.LocalReport.DataSources.Add(rdsPagos);
                    rvReporte.LocalReport.DataSources.Add(rdsConcepto);
                }
            }

            //Carga SubInforma
            rvReporte.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            rvReporte.LocalReport.Refresh();
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", nombrecorto_descargapdf != "" ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);
        }
        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //Obteniendo Pago
            int idPago = int.Parse(e.Parameters["IdPagoDocumento"].Values[0].ToString());
            int idCFDI = int.Parse(e.Parameters["IdCfdiPago"].Values[0].ToString());

            //Obteniendo Pagos 
            using (DataTable dtDocumentosPago = SAT_CL.FacturacionElectronica33.ComprobantePagoDocumentoRelacionado.ObtieneComprobantesPago(idPago, idCFDI))
            {
                //Añadiendo Origen de Datos
                e.DataSources.Add(new ReportDataSource("DocumentosPago", dtDocumentosPago));
            }//*/
        }

        private void inicializaPagoDocumento()
        {
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Creación de la variable idComprobante
            int idPago = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idCFDI = Convert.ToInt32(Request.QueryString["idRegistroB"]);

            //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
            ReportDataSource rdsDocsPago;

            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/DocumentosPago.rdlc");

            //Instanciando Documentos del Pago
            using (DataTable dtDocs = SAT_CL.FacturacionElectronica33.ComprobantePagoDocumentoRelacionado.ObtieneComprobantesPago(idPago, idCFDI))
            {
                //Validando Origen
                if (Validacion.ValidaOrigenDatos(dtDocs))
                {
                    //Asignando Valores
                    rdsDocsPago = new ReportDataSource("DocumentosPago", dtDocs);
                    rvReporte.LocalReport.DataSources.Add(rdsDocsPago);
                }
            }
        }
        /// <summary>
        /// Método que inicializa los valores del reporte ValeDiesel
        /// </summary>
        private void inicializaReporteValeDiesel()
        {
            //Creación d ela tabla para cargar el Logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Imagen", typeof(byte[]));
            DataTable dtQR = new DataTable();
            dtQR.Columns.Add("No", typeof(string));
            dtQR.Columns.Add("CodigoQr", typeof(byte[]));
            string cadena_codigo = "";
            string BD = ConfigurationManager.ConnectionStrings["TECTOS_SAT_db"].ConnectionString, BdProduccion = "";
            //Validando BD
            if (BD.Contains("AZTLAN"))
                BdProduccion = "AZTLAN";
            else if (BD.Contains("NEXTIA"))
                BdProduccion = "NEXTIA";
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obtiene el identificador de la asignación de vale
            int idAsignacionDiesel = Convert.ToInt32(Request.QueryString["idRegistro"]);
            ///Instanciando Servicio Web del Proveedor
            using (SAT_CL.EgresoServicio.AsignacionDiesel objAsignacion = new SAT_CL.EgresoServicio.AsignacionDiesel(idAsignacionDiesel))
            using (SAT_CL.Global.CompaniaEmisorReceptor com = new SAT_CL.Global.CompaniaEmisorReceptor(objAsignacion.id_compania_emisor))
            {
                //Validando si existe el Registro
                if (com.habilitar)
                {
                    /** VALIDANDO BD **/
                    switch (BdProduccion)
                    {
                        case "AZTLAN":
                            {
                                //Validando Compania
                                switch (com.id_compania_emisor_receptor)
                                {
                                    //ARI TECTOS
                                    case 1:
                                    //TRANSBEAR
                                    case 2:
                                    //TECTOS TEST
                                    case 72:
                                    //TEM
                                    case 76:
                                    //ETV1
                                    case 1081:
                                    //JARUMI
                                    case 1292:
                                    //AXEJIT
                                    case 1353:
                                    //TRANSFRIO
                                    case 1440:
                                    //MELK
                                    case 1758:
                                    default:
                                        //Instanciando Excepción
                                        ////Habilita las imagenes externas           
                                        rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ValeDiesel.rdlc");
                                        break;
                                }
                                break;
                            }
                        case "NEXTIA":
                            {
                                //Validando Compania
                                switch (com.id_compania_emisor_receptor)
                                {
                                    //GROCHA
                                    case 1127:
                                        {
                                            ////Habilita las imagenes externas           
                                            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ValeDieselQR.rdlc");
                                            break;
                                        }
                                    default:
                                        //Instanciando Excepción
                                        ////Habilita las imagenes externas           
                                        rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ValeDiesel.rdlc");
                                        break;
                                }
                                break;
                            }
                        default:
                            //Instanciando Excepción
                            ////Habilita las imagenes externas           
                            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ValeDiesel.rdlc");
                            break;
                    }
                }
            }
            //Invoca al constructor de la clase asignaciónDiesel y asigna el valor idAsignación.
            using (SAT_CL.EgresoServicio.AsignacionDiesel objAsignacion = new SAT_CL.EgresoServicio.AsignacionDiesel(idAsignacionDiesel))
            {
                //Instancia la clase ubicación para obtener la dirección de estación de la unidad.
                using (SAT_CL.Global.Ubicacion ubicacion = new SAT_CL.Global.Ubicacion(objAsignacion.id_ubicacion_estacion))
                {
                    //Creación y Asignación de valores al parametro ubicación
                    ReportParameter estacion = new ReportParameter("Estacion", ubicacion.descripcion);
                    //Asignación al reporte los parametros.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { estacion });
                }
                //Obtiene los datos de la unidad a la cual se le asigna diesel
                using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(objAsignacion.id_unidad_diesel))
                {
                    //Declara variable que almacena la referencia de rendimiento de la unidad
                    decimal rend = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, uni.id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                    //Valida que exista la referencia de rendimiento
                    if (rend == 0)
                    {
                        //Si no existe referencia calcula el rendimiento, obteniendo los kilometros y combustible asignados a la unidad                    
                        if (uni.kilometraje_asignado != 0 && uni.combustible_asignado != 0)
                        {
                            //Si existe Calcula el rendimiento
                            rend = uni.kilometraje_asignado / uni.combustible_asignado;
                        }
                    }
                    ReportParameter ultimaCarga = new ReportParameter("UltimaCarga", SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(uni.id_unidad, objAsignacion.id_asignacion_diesel).ToString("dd/MM/yy HH:mm"));
                    ReportParameter kmsRecorridos = new ReportParameter("KmsRecorridos", SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(uni.id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(uni.id_unidad, objAsignacion.id_asignacion_diesel), objAsignacion.fecha_carga).ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { ultimaCarga, kmsRecorridos });
                    //Validamos que el frendimiento sea diferente de 0
                    if (rend > 0)
                    {
                        //calcula el diesel recomendado
                        ReportParameter calculado = new ReportParameter("Calculado", Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(uni.id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(uni.id_unidad, objAsignacion.id_asignacion_diesel), objAsignacion.fecha_carga) / Convert.ToDecimal(rend)).ToString(), 5, ""));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { calculado });
                    }
                    ReportParameter capacidad = new ReportParameter("Capacidad", uni.capacidad_combustible.ToString());
                    ReportParameter unidad = new ReportParameter("Unidad", uni.numero_unidad.ToString());
                    ReportParameter placas = new ReportParameter("Placas", uni.placas.ToString());
                    ReportParameter rendimiento = new ReportParameter("Rendimiento", rend.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { capacidad, unidad, placas, rendimiento });
                    //Obtiene el tipo de unidad
                    using (SAT_CL.Global.UnidadTipo uniTip = new SAT_CL.Global.UnidadTipo(uni.id_tipo_unidad))
                    {
                        ReportParameter tipoUnidad = new ReportParameter("TipoUnidad", uniTip.descripcion_unidad);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { tipoUnidad });
                    }
                }
            }
            //Invoca al Método de la clase cargaImpresionValeDiesel para obener los valores del reporte
            using (DataTable t = SAT_CL.EgresoServicio.AsignacionDiesel.CargaImpresionValeDiesel(idAsignacionDiesel))
            {

                //Valida que el retorno del metodo sea valido
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                {
                    //Recorre los valores de retorno y los asigna a los parametros del reporte
                    foreach (DataRow r in t.Rows)
                    {
                        //Creación y asignación de valores a los parametros del reporte. 
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter noImpresion = new ReportParameter("NoImpresion", r["NoImpresion"].ToString());
                        ReportParameter NoServicio = new ReportParameter("NoServicio", r["NoServicio"].ToString());
                        ReportParameter nombreOperador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                        ReportParameter noVale = new ReportParameter("NoVale", r["NoVale"].ToString());
                        ReportParameter fechaCarga = new ReportParameter("FechaCarga", r["FechaCarga"].ToString());
                        ReportParameter costoDiesel = new ReportParameter("CostoDiesel", r["CostoDiesel"].ToString());
                        ReportParameter tipoCombustible = new ReportParameter("TipoCombustible", r["TipoCombustible"].ToString().ToUpper());
                        ReportParameter referencia = new ReportParameter("Referencia", r["Referencia"].ToString());
                        ReportParameter rutaLogo = new ReportParameter("RutaLogo", r["RutaLogo"].ToString());
                        ReportParameter litros = new ReportParameter("Litros", r["Litros"].ToString());
                        ReportParameter total = new ReportParameter("Total", r["Total"].ToString());
                        //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                        byte[] imagen = null;
                        //Permite capturar errores en caso de que no exista una ruta para el archivo
                        try
                        {
                            //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                            imagen = System.IO.File.ReadAllBytes(r["RutaLogo"].ToString());
                        }
                        //En caso de que no exista una imagen, se devolvera un valor nulo.
                        catch { imagen = null; }
                        //Agrega a la tabla un registro con valor a la ruta de la imagen.
                        dtLogo.Rows.Add(imagen);
                        //Asigancion de los parametros al reporte
                        rvReporte.LocalReport.SetParameters(new ReportParameter[]{noImpresion,NoServicio,nombreOperador,noVale, compania, fechaCarga, tipoCombustible, costoDiesel,referencia,
                                                                                  rutaLogo, litros,total});

                        cadena_codigo = string.Format("?NoVale={0}&Operador={1}&Fecha={2}",
                                                         r["NoVale"].ToString(), r["NombreOperador"].ToString(), r["FechaCarga"].ToString());
                        byte[] ArchivoBytes = Dibujo.GeneraCodigoBidimensional(cadena_codigo, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //Añadiendo Columna a Tabla
                        dtQR.Rows.Add(r["NoVale"].ToString(), ArchivoBytes);
                    }
                    //limpia el reporte local
                    rvReporte.LocalReport.DataSources.Clear();
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvlogotipoVale = new ReportDataSource("LogotipoVale", dtLogo);
                    ReportDataSource rsValeQR = new ReportDataSource("ValeDisel", dtQR);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvlogotipoVale);
                    rvReporte.LocalReport.DataSources.Add(rsValeQR);
                }

            }

        }
        /// <summary>
        /// Método que iniciliza los valores del reporte Liquidacion
        /// </summary>
        private void inicializaReporteLiquidacion()
        {
            //Creación de la abla para la carga del logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Imagen", typeof(byte[]));

            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Obtiene el identificador de liquidación
            int idLiquidacion = Convert.ToInt32(Request.QueryString["idRegistro"]);

            //int idLiquidacion = Convert.ToInt32(Request.QueryString["idRegistro"]);
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Liquidacion.rdlc");
            rvReporte.LocalReport.DataSources.Clear();
            //Declarando Arreglo de Parametros por Recibir
            ReportParameter[] param = new ReportParameter[13];

            //Invoca al constructor de la clase asignacion y asigna el valor idLiquidacion
            using (SAT_CL.Liquidacion.Liquidacion objliquidacion = new SAT_CL.Liquidacion.Liquidacion(idLiquidacion))
            {
                //Validando que exista la Liquidación
                if (objliquidacion.id_liquidacion > 0)
                {
                    //Instancia a la clase compañiaemisorreceptor para obtener el encabezado del documento
                    using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(objliquidacion.id_compania_emisora))
                    {
                        foreach (DataRow r in encabezado.Rows)
                        {
                            param[0] = new ReportParameter("Compania", r["Compania"].ToString());
                            ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                            ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                            ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { rfc, telefono, direccion });
                        }
                    }
                    using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objliquidacion.id_compania_emisora))
                    {
                        ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                        ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });

                        //Creación del arreglo de byte que permitira almacenar la ruta del la imagen correspondiente a la compañia
                        byte[] imagen = null;
                        //Permite capturar errores durante la ejecución del codigo
                        try
                        {
                            //Asigna al arreglo la ruta de la imagen
                            imagen = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                        }
                        //En caso de no encontrar la ruta del archivo, devuelve un valor nulo    
                        catch { imagen = null; }
                        //Agrega a la tabla el valor de la ruta de la imagen
                        dtLogo.Rows.Add(imagen);
                    }

                    //Validando que el Tipo de Asignación del Recurso sea Unidad
                    if (objliquidacion.id_tipo_asignacion == 1)
                    {
                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(objliquidacion.id_unidad))
                        {
                            //Asignando Descripción
                            param[1] = new ReportParameter("Entidad", uni.numero_unidad);
                            param[2] = new ReportParameter("TipoEntidad", "No. Unidad:");
                        }
                    }
                    //Validando que el Tipo de Asignación del Recurso sea Operador
                    else if (objliquidacion.id_tipo_asignacion == 2)
                    {
                        //TO DO: Clase de Operador
                        using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(objliquidacion.id_operador))
                        {
                            //Asignando Descripción
                            param[1] = new ReportParameter("Entidad", op.nombre);
                            param[2] = new ReportParameter("TipoEntidad", "Nombre:");
                        }
                    }
                    //Validando que el Tipo de Asignación del Recurso sea Proveedor
                    else if (objliquidacion.id_tipo_asignacion == 3)
                    {
                        //Instanciando Unidad
                        using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(objliquidacion.id_proveedor))
                        {
                            //Asignando Descripción
                            param[1] = new ReportParameter("Entidad", pro.nombre);
                            param[2] = new ReportParameter("TipoEntidad", "Nombre:");
                        }
                    }

                    //Asignando Descripción
                    param[3] = new ReportParameter("NoLiquidacion", objliquidacion.no_liquidacion.ToString());
                    param[4] = new ReportParameter("FechaLiquidacion", objliquidacion.fecha_liquidacion.ToString("dd/MM/yyyy HH:mm"));
                    param[5] = new ReportParameter("Estatus", objliquidacion.estatus.ToString());

                    //Validando si la Liquidación esta Cerrada
                    if (objliquidacion.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado)
                    {
                        //Asignando Totales
                        param[6] = new ReportParameter("TPercepciones", string.Format("{0:C2}", objliquidacion.total_salario));
                        param[7] = new ReportParameter("TDeducciones", string.Format("{0:C2}", objliquidacion.total_deducciones));
                        param[8] = new ReportParameter("TSueldo", string.Format("{0:C2}", objliquidacion.total_sueldo));
                        param[9] = new ReportParameter("TDescuentos", string.Format("{0:C2}", objliquidacion.total_descuentos));
                        param[10] = new ReportParameter("TAnticipos", string.Format("{0:C2}", objliquidacion.total_anticipos));
                        param[11] = new ReportParameter("TComprobaciones", string.Format("{0:C2}", objliquidacion.total_comprobaciones));
                        param[12] = new ReportParameter("TAlcance", string.Format("{0:C2}", objliquidacion.total_alcance));
                    }
                    else
                    {
                        //Obteniendo Totales
                        using (DataTable dtTotalesLiq = SAT_CL.Liquidacion.Liquidacion.ObtieneMontosTotalesLiquidacion(objliquidacion.id_liquidacion, objliquidacion.tipo_asignacion,
                                                                objliquidacion.id_unidad, objliquidacion.id_operador, objliquidacion.id_proveedor, objliquidacion.id_compania_emisora))
                        {
                            //Validando que existan los Valores
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesLiq))
                            {
                                //Recorriendo Tabla
                                foreach (DataRow dr in dtTotalesLiq.Rows)
                                {
                                    //Mostrando Valores Obtenidos
                                    param[6] = new ReportParameter("TPercepciones", string.Format("{0:C2}", dr["TPercepcion"].ToString()));
                                    param[7] = new ReportParameter("TDeducciones", string.Format("{0:C2}", dr["TDeducciones"].ToString()));
                                    param[8] = new ReportParameter("TSueldo", string.Format("{0:C2}", dr["TSueldo"].ToString()));
                                    param[9] = new ReportParameter("TDescuentos", string.Format("{0:C2}", dr["TDescuentos"].ToString()));
                                    param[10] = new ReportParameter("TAnticipos", string.Format("{0:C2}", dr["TAnticipos"].ToString()));
                                    param[11] = new ReportParameter("TComprobaciones", string.Format("{0:C2}", dr["TComprobaciones"].ToString()));
                                    param[12] = new ReportParameter("TAlcance", string.Format("{0:C2}", dr["TAlcance"].ToString()));
                                }
                            }
                            else
                            {
                                //Asignando Totales
                                param[6] = new ReportParameter("TPercepciones", string.Format("{0:C2}", 0));
                                param[7] = new ReportParameter("TDeducciones", string.Format("{0:C2}", 0));
                                param[8] = new ReportParameter("TSueldo", string.Format("{0:C2}", 0));
                                param[9] = new ReportParameter("TDescuentos", string.Format("{0:C2}", 0));
                                param[10] = new ReportParameter("TAnticipos", string.Format("{0:C2}", 0));
                                param[11] = new ReportParameter("TComprobaciones", string.Format("{0:C2}", 0));
                                param[12] = new ReportParameter("TAlcance", string.Format("{0:C2}", 0));
                            }
                        }
                    }

                    //Asignando Parametros
                    this.rvReporte.LocalReport.SetParameters(param);
                    //Creación de la variable de tipo entero idRecurso
                    int idRecurso = 0;
                    //Valida el tipo asignación; si la liquidación esta asignada a un operador
                    if (objliquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador)
                        //Asigna al idRecurso el valor del identificador del operador
                        idRecurso = objliquidacion.id_operador;

                    //Si la liquidación esta asignada a un Proveedor
                    else if (objliquidacion.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor)
                        //Asigna al IdRecurso el valor del identificador proveedor
                        idRecurso = objliquidacion.id_proveedor;

                    //Si la liquidación es asignada a una unidad
                    else
                        //Asigna a la variable idRecurso el identificador de una unidad
                        idRecurso = objliquidacion.id_unidad;

                    //Declarando Reporte Nuevo
                    ReportDataSource rsDetalleLiq;
                    ReportDataSource rdsLogotipoLiquidacion;

                    //Instancia a la clase Liquidación reporte modulo
                    using (DataTable dtDetalleLiq = SAT_CL.Liquidacion.Reportes.ReporteServiciosMovimientosLiquidacion(idRecurso, objliquidacion.id_tipo_asignacion, objliquidacion.fecha_liquidacion, objliquidacion.id_estatus, objliquidacion.id_liquidacion))
                    {
                        //Valida los datos del dtDetalleLiq
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalleLiq))

                            //Asignando Resultados
                            rsDetalleLiq = new ReportDataSource("DetalleLiquidacion", dtDetalleLiq);
                        else
                        {
                            //Creando Tabla Temporal para Impresion de Detalles
                            using (DataTable dtDetalles = new DataTable())
                            {
                                //Agrega columnas a la tabla creada
                                dtDetalles.Columns.Add("IdServicio", typeof(int));
                                dtDetalles.Columns.Add("IdMovimiento", typeof(int));
                                dtDetalles.Columns.Add("NoServicio", typeof(string));
                                dtDetalles.Columns.Add("NoViaje", typeof(string));
                                dtDetalles.Columns.Add("Porte", typeof(string));
                                dtDetalles.Columns.Add("Cliente", typeof(string));
                                dtDetalles.Columns.Add("Origen", typeof(string));
                                dtDetalles.Columns.Add("Destino", typeof(string));
                                dtDetalles.Columns.Add("Kms", typeof(decimal));
                                dtDetalles.Columns.Add("DuracionViaje", typeof(string));
                                dtDetalles.Columns.Add("*DuracionViaje", typeof(string));
                                dtDetalles.Columns.Add("FechaInicio", typeof(string));
                                dtDetalles.Columns.Add("FechaFin", typeof(string));
                                dtDetalles.Columns.Add("Total", typeof(decimal));
                                dtDetalles.Columns.Add("EstatusDocumentos", typeof(string));
                                dtDetalles.Columns.Add("IdOperador", typeof(int));
                                dtDetalles.Columns.Add("IdUnidad", typeof(int));
                                dtDetalles.Columns.Add("IdUnidad2", typeof(int));
                                dtDetalles.Columns.Add("Operador", typeof(string));
                                dtDetalles.Columns.Add("Unidad", typeof(string));
                                dtDetalles.Columns.Add("Remolque", typeof(string));
                                dtDetalles.Columns.Add("Diesel", typeof(decimal));
                                dtDetalles.Columns.Add("Anticipos", typeof(decimal));
                                dtDetalles.Columns.Add("Comprobaciones", typeof(decimal));
                                dtDetalles.Columns.Add("Pagos", typeof(decimal));
                                dtDetalles.Columns.Add("TFacturasAnticipos", typeof(decimal));
                                dtDetalles.Columns.Add("indDevoluciones", typeof(int));
                                dtDetalles.Columns.Add("FacturasAnticipos", typeof(int));

                                //Añadiendo Fila en Vacio
                                dtDetalles.Rows.Add(0, 0, "", "", "", "", "", "", 0.00M, "", "", "", "", 0.00M, "", 0, 0, 0, "", "", "", 0.00M, 0.00M, 0.00M, 0.00M, 0.00M, 0, 0);

                                //Asignando Resultados
                                rsDetalleLiq = new ReportDataSource("DetalleLiquidacion", dtDetalles);
                            }
                        }

                        //Asigna al conjunto de datos  los valores de la tabla
                        rdsLogotipoLiquidacion = new ReportDataSource("Logotipo", dtLogo);
                        //Asigna al reporte el datasourse con los valores asignados al conjunto
                        rvReporte.LocalReport.DataSources.Add(rdsLogotipoLiquidacion);

                        //Asigna el Reporte de Detalles de Liquidación
                        rvReporte.LocalReport.DataSources.Add(rsDetalleLiq);
                    }
                    //Crea una tabla para instanciar los valores  de cobros recurrentes acorde al estatus de la liquidación
                    using (DataTable dt = new DataTable())
                    {
                        //Agrega columnas a la tabla creada
                        dt.Columns.Add("Tipo", typeof(string));
                        dt.Columns.Add("Descripcion", typeof(string));
                        dt.Columns.Add("Cantidad", typeof(decimal));
                        dt.Columns.Add("MontoCobro", typeof(decimal));
                        dt.Columns.Add("Total", typeof(decimal));
                        //Valida el estatus de la liquidación  (Registrada =1, Terminada =2)
                        //Si el estatus de la liquidación esta en estatus registrada
                        if (objliquidacion.id_estatus == 1)
                        {
                            //Asigna a la tabla dtCobrosEnt los valores obtenidos de invocar al método ObtieneCobrosRecurrentesEntidad
                            using (DataTable dtCobrosEnt = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesEntidad(objliquidacion.id_tipo_asignacion, objliquidacion.id_unidad, objliquidacion.id_operador, objliquidacion.id_proveedor, objliquidacion.id_compania_emisora, objliquidacion.fecha_liquidacion))
                            {
                                //Valida los datos de la tabla dtCobrosEnt (Que existan y que no sean nulos)
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosEnt))
                                {
                                    //Recorre las filas de la tabla dtCobrosEnt
                                    foreach (DataRow dr in dtCobrosEnt.Rows)
                                    {
                                        //Asigna a las filas los valores encontrados de la tabla dtCobrosEnt
                                        dt.Rows.Add(dr["Tipo"].ToString(), dr["Descripcion"].ToString(), Convert.ToDecimal(dr["Cantidad"]),
                                                       Convert.ToDecimal(dr["MontoCobro"]), Convert.ToDecimal(dr["Total"]));
                                    }
                                    ReportDataSource rsCobrosRecurrentes = new ReportDataSource("DetalleLiquidacionRecurentes", dt);
                                    rvReporte.LocalReport.DataSources.Add(rsCobrosRecurrentes);
                                }
                            }
                        }
                        //Si el estatus de la liquidación esta terminada
                        else
                        {
                            //Asigna a la tabla dtCobrosLiq los valores obtenidos de invocar al método ObtieneCobrosRecurrentesTotales
                            using (DataTable dtCobrosLiq = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesTotales(objliquidacion.id_liquidacion, objliquidacion.id_tipo_asignacion, objliquidacion.id_unidad, objliquidacion.id_operador, objliquidacion.id_proveedor, objliquidacion.id_compania_emisora))
                            {
                                //Valida los datos de la tabla dtCobrosEnt (Que existan y que no sean nulos)
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosLiq))
                                {
                                    //Recorre las filas de la tabla dtCobrosLiq
                                    foreach (DataRow dr in dtCobrosLiq.Rows)
                                    {
                                        //Recorre las filas de la tabla dt y asigna los valores  encontrados de la tabla dtCobrosLiq
                                        dt.Rows.Add(dr["Tipo"].ToString(), dr["Descripcion"].ToString(), Convert.ToDecimal(dr["Cantidad"]),
                                                       Convert.ToDecimal(dr["MontoCobro"]), Convert.ToDecimal(dr["Total"]));
                                    }
                                    ReportDataSource rsCobrosRecurrentes = new ReportDataSource("DetalleLiquidacionRecurentes", dt);
                                    rvReporte.LocalReport.DataSources.Add(rsCobrosRecurrentes);
                                }
                            }
                        }
                        //Si el dt no contiene valores
                        if (!TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                        {
                            //Agrega valores en vacio 
                            dt.Rows.Add("", "", 0, 0, 0);
                            ReportDataSource rsCobrosRecurrentes = new ReportDataSource("DetalleLiquidacionRecurentes", dt);
                            rvReporte.LocalReport.DataSources.Add(rsCobrosRecurrentes);
                        }
                        //Instancia al método ObtienePagosLiquidacion y el resultado lo almacena en la tabla dtPagos
                        using (DataTable dtPagos = SAT_CL.Liquidacion.Pago.ObtienePagosLiquidacion(objliquidacion.id_liquidacion))
                        {
                            //Valida que existan los datos en la tabla dtPagos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagos))
                            {
                                ReportDataSource rsPagos = new ReportDataSource("DetalleLiquidacionPagos", dtPagos);
                                rvReporte.LocalReport.DataSources.Add(rsPagos);
                            }
                            //En caso de que no existan
                            else
                            {
                                //Crea una tabla 
                                using (DataTable dtOtrosPagos = new DataTable())
                                {
                                    //Agraga columnas a la tabla
                                    dtOtrosPagos.Columns.Add("Concepto", typeof(string));
                                    dtOtrosPagos.Columns.Add("Descripcion", typeof(string));
                                    dtOtrosPagos.Columns.Add("Cantidad", typeof(decimal));
                                    dtOtrosPagos.Columns.Add("ValorU", typeof(decimal));
                                    dtOtrosPagos.Columns.Add("Total", typeof(decimal));
                                    //Asigna valores a la tabla
                                    dtOtrosPagos.Rows.Add("", "", 0, 0, 0);
                                    ReportDataSource rsOtrosPagos = new ReportDataSource("DetalleLiquidacionPagos", dtOtrosPagos);
                                    rvReporte.LocalReport.DataSources.Add(rsOtrosPagos);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void inicializaReporteProcesoRevision()
        {
            //Obteniendo Segmento
            int idPQ = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Creación de la abla para la carga del logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Imagen", typeof(byte[]));
            //Habilitando Imagenes Externas
            rvReporte.LocalReport.EnableExternalImages = true;

            //Cargando Reporte
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ProcesoRevision.rdlc");
            //Limpia el datasource
            rvReporte.LocalReport.DataSources.Clear();
            //Declarando Arreglo de Parametros por Recibir
            ReportParameter[] param = new ReportParameter[7];
            using (SAT_CL.Facturacion.PaqueteProceso pp = new SAT_CL.Facturacion.PaqueteProceso(idPQ))
            {
                //Instancia a la clase compañiaemisorreceptor para obtener el encabezado del documento
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(pp.id_compania))
                {
                    foreach (DataRow r in encabezado.Rows)
                    {
                        param[0] = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { rfc, telefono, direccion });
                    }
                }
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(pp.id_compania))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });

                    //Creación del arreglo de byte que permitira almacenar la ruta del la imagen correspondiente a la compañia
                    byte[] imagen = null;
                    //Permite capturar errores durante la ejecución del codigo
                    try
                    {
                        //Asigna al arreglo la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de no encontrar la ruta del archivo, devuelve un valor nulo    
                    catch { imagen = null; }
                    //Agrega a la tabla el valor de la ruta de la imagen
                    dtLogo.Rows.Add(imagen);
                    //Asigna al conjunto de datos  los valores de la tabla
                    ReportDataSource rdsLogotipoProceso = new ReportDataSource("Logotipo", dtLogo);
                    //Asigna al reporte el datasourse con los valores asignados al conjunto
                    rvReporte.LocalReport.DataSources.Add(rdsLogotipoProceso);
                }
            }


            //Instanciando Proceso
            using (DataSet dsDatosPaquete = SAT_CL.Facturacion.PaqueteProceso.ObtieneDatosPaquete(Convert.ToInt32(idPQ)))
            {
                //Validando que Exista el Paquete
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsDatosPaquete, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in dsDatosPaquete.Tables["Table"].Rows)
                    {
                        //Creando Parametros
                        param[1] = new ReportParameter("NoPaquete", dr["NoPaquete"].ToString());
                        param[2] = new ReportParameter("Cliente", dr["Cliente"].ToString());
                        param[3] = new ReportParameter("Tipo", dr["TipoProceso"].ToString());
                        param[4] = new ReportParameter("Estatus", dr["Estatus"].ToString());
                        param[5] = new ReportParameter("UsuarioResp", dr["UsuarioResponsable"].ToString());//*/
                        param[6] = new ReportParameter("Fecha", dr["FechaInicio"].ToString());
                    }
                }

                //Declarando Variables Auxiliares
                DataTable dtFacturasLigadas = new DataTable();

                //Creando Columnas
                dtFacturasLigadas.Columns.Add("Servicio", typeof(string));
                dtFacturasLigadas.Columns.Add("Referencia1", typeof(string));
                dtFacturasLigadas.Columns.Add("Referencia2", typeof(string));
                dtFacturasLigadas.Columns.Add("Referencia3", typeof(string));
                dtFacturasLigadas.Columns.Add("EstatusDoc", typeof(string));
                dtFacturasLigadas.Columns.Add("Origen", typeof(string));
                dtFacturasLigadas.Columns.Add("Destino", typeof(string));
                dtFacturasLigadas.Columns.Add("FecFac", typeof(string));
                dtFacturasLigadas.Columns.Add("Total", typeof(string));
                dtFacturasLigadas.Columns.Add("FechaInicioServicio", typeof(DateTime));

                //Validando que Exista el Paquete
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsDatosPaquete, "Table1"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dsDatosPaquete.Tables["Table1"].Rows)

                        //Añadiendo Registros
                        dtFacturasLigadas.Rows.Add(dr["Servicio"].ToString(), dr["Referencia1"].ToString(), dr["Referencia2"].ToString(), dr["Referencia3"].ToString(),
                            dr["EstatusDoc"].ToString(), dr["Origen"].ToString(), dr["Destino"].ToString(), dr["FecFac"].ToString(), dr["Total"].ToString(), dr["FechaInicioServicio"].ToString());
                }
                else
                    //Añadiendo Registros
                    dtFacturasLigadas.Rows.Add("", "", "", "", "", "", "", "", "");

                //Agregamos el origen de datos de Carga
                ReportDataSource rdsFacturasLigadas = new ReportDataSource("FacturasLigadas", dtFacturasLigadas);
                rvReporte.LocalReport.DataSources.Add(rdsFacturasLigadas);

                //Asignando Parametros
                this.rvReporte.LocalReport.SetParameters(param);
            }

        }
        /// <summary>
        /// Evento de cargar el Subreporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            //Obtenemos valores para realizar filtrado de parametros
            int id_comprobante = int.Parse(e.Parameters["IdComprobanteReferencia"].Values[0].ToString());
            int id_concepto = int.Parse(e.Parameters["IdConceptoReferencia"].Values[0].ToString());
            //Cargamos Tabla
            using (DataTable mitReferencias = SAT_CL.FacturacionElectronica.Concepto.CargaImpresionReferencias(id_comprobante, id_concepto))
            {
                //Asignamos Origen de Datos
                e.DataSources.Add(new ReportDataSource("Referencias", mitReferencias));
            }
        }
        /// <summary>
        /// Evento de carga para el Subreporte versión 3.3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandler33(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath.ToString() == "ComprobanteReferencias")
            {
                int id_comprobante33 = int.Parse(e.Parameters["IdComprobanteReferencia"].Values[0].ToString());
                int id_concepto33 = int.Parse(e.Parameters["IdConceptoReferencia"].Values[0].ToString());
                using (DataTable mitReferencias = SAT_CL.FacturacionElectronica33.Concepto.CargaImpresionReferencias(id_concepto33, id_comprobante33))
                {
                    e.DataSources.Add(new ReportDataSource("Referencias", mitReferencias));
                }
            }
        }
        /// <summary>
        /// Evento de carga para el Subreporte de Comprobante Relacionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubreportProcessingEventHandlerCompRel(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath.ToString() == "ComprobanteRelacionado")
            {
                int id_comprobante = int.Parse(e.Parameters["IdComprobante"].Values[0].ToString());
                //using (DataTable CFDIRelacionados = SAT_CL.FacturacionElectronica33.ComprobanteRelacion.ObtieneRelacionesComprobante(id_comprobante))
                //{
                //    e.DataSources.Add(new ReportDataSource("ComprobanteRelacionado", CFDIRelacionados));
                //}
                //Carga CFDI Relacionados
                using (DataTable CFDIRelacionados = SAT_CL.FacturacionElectronica33.ComprobanteRelacion.ObtieneRelacionesComprobante(id_comprobante))
                {
                    //Agregar origen de datos 
                    //ReportDataSource rsCFDIRelacionado;
                    //Validando Concepto
                    if (Validacion.ValidaOrigenDatos(CFDIRelacionados))
                    {
                        e.DataSources.Add(new ReportDataSource("ComprobanteRelacionado", CFDIRelacionados));
                    }
                    else
                        using (DataTable tablaCFDI = new DataTable())
                        {
                            tablaCFDI.Columns.Add("Id", typeof(string));
                            tablaCFDI.Columns.Add("Secuencia", typeof(string));
                            tablaCFDI.Columns.Add("TipoRelacion", typeof(string));
                            tablaCFDI.Columns.Add("Relacion", typeof(string));
                            tablaCFDI.Columns.Add("UUID", typeof(string));
                            //Insertar Registros
                            DataRow row = tablaCFDI.NewRow();
                            row["Id"] = "";
                            row["Secuencia"] = "";
                            row["TipoRelacion"] = "";
                            row["Relacion"] = "";
                            row["UUID"] = "";
                            tablaCFDI.Rows.Add(row);
                            e.DataSources.Add(new ReportDataSource("ComprobanteRelacionado", tablaCFDI));
                        }
                    //Asigna los valores al conjunto de datos
                    //rvReporte.LocalReport.DataSources.Add(rsCFDIRelacionado);
                }
            }
        }
        /// <summary>
        /// Método que permite inicializar los parametros del reporte Renuncia
        /// </summary>
        private void inicializaReporteRenuncia()
        {
            //Variable que almacena el identificador del operador.
            int idOperador = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Renuncia.rdlc");
            //Invoca a la clase operador, obtiene los datos del operador.
            using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(idOperador))
            {
                //Invoca a la clase CompaniaEmisorReceptor para obtener el nombre de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emi = new SAT_CL.Global.CompaniaEmisorReceptor(op.id_compania_emisor))
                {
                    //Crea y Asigna los valores a las variables del reporte
                    ReportParameter compania = new ReportParameter("Compania", emi.nombre);
                    //Asigna los valores al reporte (rdlc)
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania });
                }
                //Crea y Asigna los valores a las variables del reporte
                ReportParameter nombreOperador = new ReportParameter("NombreOperador", op.nombre);
                //Asigna los valores al reporte (rdlc)
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreOperador });
            }


        }
        /// <summary>
        /// Método que iniciliaz los valores de los parametros del reporte ContratoIndeterminado
        /// </summary>
        private void inicializaReporteContratoIndeterminado()
        {
            //Creación de la variable IdOperador que almacena el identificador del operador.
            int idOperador = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ContratoIndeterminado.rdlc");
            //Invoca a la clase operador, obtiene los datos del operador.
            using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(idOperador))
            {
                //Creación de la variable de tipo datetime que almacena la fecha actual
                DateTime fechaactual = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
                //Variable que almacena el año de la fecha de nacimiento del operador
                int fechanac = Convert.ToInt32(op.fecha_nacimiento.Year);
                //Variable que almacena la edad, calculada de la resta del año actual con el año de nacimiento.
                int edadop = fechaactual.Year - fechanac;
                //Crea y Asigna los valores a las variables del reporte
                ReportParameter edad = new ReportParameter("Edad", edadop.ToString());
                ReportParameter nombreOperador = new ReportParameter("NombreOperador", op.nombre.ToUpper());
                ReportParameter fechaIngreso = new ReportParameter("FechaIngreso", op.fecha_ingreso.ToString().ToUpper());
                ReportParameter puesto = new ReportParameter("Puesto", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Puesto").ToUpper());
                ReportParameter estadoCivil = new ReportParameter("EstadoCivil", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Estado Civil").ToUpper());
                //Asigna los valores al reporte (rdlc)
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreOperador, edad, fechaIngreso, puesto, estadoCivil });
                //Invoca a la clase dirección y obtiene la dreccion del operador
                using (SAT_CL.Global.Direccion dirop = new SAT_CL.Global.Direccion(op.id_direccion))
                {
                    //Crea y Asigna los valores a las variables del reporte
                    ReportParameter direccionOperador = new ReportParameter("DireccionOperador", dirop.ObtieneDireccionCompleta());
                    //Asigna los valores al reporte (rdlc)
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionOperador });
                }
                //Invoca a la clase CompaniaEmisorReceptor para obtener el nombre de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emi = new SAT_CL.Global.CompaniaEmisorReceptor(op.id_compania_emisor))
                {
                    //Crea y Asigna los valores a las variables del reporte
                    ReportParameter compania = new ReportParameter("Compania", emi.nombre);
                    //Asigna los valores al reporte (rdlc)
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania });
                    //Invoca a ala clase Dirección y obtiene la dirección de la compañia
                    using (SAT_CL.Global.Direccion diremi = new SAT_CL.Global.Direccion(emi.id_direccion))
                    {
                        //Crea y Asigna los valores a las variables del reporte
                        ReportParameter direccionCompania = new ReportParameter("DireccionCompania", diremi.ObtieneDireccionCompleta());
                        //Asigna los valores al reporte (rdlc)
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionCompania });
                    }
                }
            }
        }
        /// <summary>
        /// Método que inicializa los parametros del reporte Contrato Tiempo definido
        /// </summary>
        private void inicializaReporteContratoTiempoDefinido()
        {
            //Almacena en una variable de tipo entero el valor obtenido del Request.QueryString
            int idOperador = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Almacena en una variable de tipo fecha el valor obtenido del Request.QueryString
            DateTime fechaPeriodoInicial = Convert.ToDateTime(Request.QueryString["fechaInicio"]);
            DateTime fechaPeriodoFin = Convert.ToDateTime(Request.QueryString["fechaFin"]);
            //Asignación de la ubicación del reporte local
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ContratoTiempoFijo.rdlc");
            //Invoca a la clase operador, obtiene los datos del operador.
            using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(idOperador))
            {
                //Declara variable de tipo fecha y obtiene la fecha actual.
                DateTime fechaactual = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
                //Declara la variable de tipo entero y almacena el año de la fecha de nacimiento
                int fechanac = Convert.ToInt32(op.fecha_nacimiento.Year);
                //Declara la variable edadop y almacena el resultado de la resta entre el año de la fecha actual con la de nacimiento.
                int edadop = fechaactual.Year - fechanac;
                //Crea y Asigna los valores a las variables del reporte
                ReportParameter fechaIngreso = new ReportParameter("FechaIngreso", fechaPeriodoInicial.Day.ToString());
                ReportParameter mesIngreso = new ReportParameter("MesIngreso", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaPeriodoInicial.Month)));
                ReportParameter anioIngreso = new ReportParameter("AnioIngreso", fechaPeriodoInicial.Year.ToString());
                ReportParameter fechaFin = new ReportParameter("FechaFin", fechaPeriodoFin.Day.ToString());
                ReportParameter mesFin = new ReportParameter("MesFin", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaPeriodoFin.Month)));
                ReportParameter anioFin = new ReportParameter("AnioFin", fechaPeriodoFin.Year.ToString());
                ReportParameter edad = new ReportParameter("Edad", edadop.ToString());
                ReportParameter nombreOperador = new ReportParameter("NombreOperador", op.nombre);
                ReportParameter puesto = new ReportParameter("Puesto", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Puesto").ToUpper());
                ReportParameter estadoCivil = new ReportParameter("EstadoCivil", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Estado Civil").ToUpper());
                ReportParameter sexo = new ReportParameter("Sexo", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Sexo").ToUpper());
                ReportParameter horarioLaboral = new ReportParameter("HorarioLaboral", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Contratos", "Horario Labores"));
                ReportParameter mesIngresoTer = new ReportParameter("MesIngresoTer", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fechaPeriodoInicial.Month).ToUpper());
                //Asigna los valores al reporte (rdlc)
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreOperador, edad, puesto, estadoCivil, fechaIngreso, mesIngreso, anioIngreso, fechaFin, mesFin, anioFin, sexo, horarioLaboral, mesIngresoTer });
                //Invoca a la clase dirección y obtiene la dreccion del operador
                using (SAT_CL.Global.Direccion dirop = new SAT_CL.Global.Direccion(op.id_direccion))
                {
                    //Crea y Asigna los valores a las variables del reporte
                    ReportParameter direccionOperador = new ReportParameter("DireccionOperador", dirop.ObtieneDireccionCompleta());
                    //Asigna los valores al reporte (rdlc)
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionOperador });
                }
                //Invoca a la clase CompaniaEmisorReceptor para obtener el nombre de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emi = new SAT_CL.Global.CompaniaEmisorReceptor(op.id_compania_emisor))
                {
                    //Crea y Asigna los valores a las variables del reporte
                    ReportParameter compania = new ReportParameter("Compania", emi.nombre);
                    //Asigna los valores al reporte (rdlc)
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania });
                    //Invoca a ala clase Dirección y obtiene la dirección de la compañia
                    using (SAT_CL.Global.Direccion diremi = new SAT_CL.Global.Direccion(emi.id_direccion))
                    {
                        //Crea y Asigna los valores a las variables del reporte
                        ReportParameter direccionCompania = new ReportParameter("DireccionCompania", diremi.ObtieneDireccionCompleta());
                        //Asigna los valores al reporte (rdlc)
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionCompania });
                    }
                }
            }
        }
        /// <summary>
        /// Método que permite inicializar los valores del reporte AcuseREciboFacturas
        /// </summary>
        private void inicializaReporteAcuseReciboFacturas()
        {
            //Creación de la variable que almacena el identificador de registro de una recepcion de factura proveedor
            int idAcuseRecibo = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Ubicación local del reporte
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/AcuseReciboFacturas.rdlc"); 
            //Creación de la variable tipo tabla que almacenara el logotipo de la empresa.
            DataTable dtLogo = new DataTable();
            //Agrega a la columna de la tabla el parametro Logotipo.
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte.
            rvReporte.LocalReport.EnableExternalImages = true;
            //Invoca a la clase recepcion y obtiene los datos de la recepción.
            using (SAT_CL.CXP.Recepcion rec = new SAT_CL.CXP.Recepcion(idAcuseRecibo))
            {
                //Creación de variables que almacenan los datos consultados de un registro
                ReportParameter fecha = new ReportParameter("Fecha", rec.fecha_recepcion.ToString());
                ReportParameter entregado = new ReportParameter("Entregado", rec.entregado_por);
                //Asigna al reporte (RDLC) las variables creadas.
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { fecha, entregado });
                //Invoca a la clase compañia para obtener los datos de la empresa emisor
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_receptor))
                {
                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter compania = new ReportParameter("Compania", emisor.nombre);
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, color });
                    //Invoca a la clase direccion y obtiene la dirección de la compañia.
                    using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(emisor.id_direccion))
                    {
                        //Creación de variables que almacenan los datos consultados de un registro
                        ReportParameter direccion = new ReportParameter("Direccion", dir.ObtieneDireccionCompleta());
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccion });
                    }
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista ruta de archivo, devuelve un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla el registro con la ruta del logotipo.
                    dtLogo.Rows.Add(logotipo);
                }
                //Invoca a la clase compania emisor receptor y obtiene el nombre del proveedor de facturas.
                using (SAT_CL.Global.CompaniaEmisorReceptor prov = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_proveedor))
                {
                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter proveedorEntrega = new ReportParameter("ProveedorEntrega", prov.nombre);
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { proveedorEntrega });
                }
                //Invoca a la clase Factura Proveedor 
                using (SAT_CL.Global.CompaniaEmisorReceptor prov = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_proveedor))
                {
                    //Valida que el provedor tenga días de credito
                    if (prov.dias_credito != 0)
                    {
                        //Si tieme dias de credito envia al parametros la leyenda de dias de credito proveedor.
                        ReportParameter diasPago = new ReportParameter("DiasPago", "DÍAS DE CRÉDITO: " + prov.dias_credito.ToString() + " días.");
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                    }
                    //En caso contrario
                    else
                    {
                        //No envia leyenda de días de crédito proveedor.
                        ReportParameter diasPago = new ReportParameter("DiasPago", "DÍAS DE CRÉDITO: 30 días.");
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                    }
                }
            }
            //Limpia los registros previos a la consulta de un datasources
            rvReporte.LocalReport.DataSources.Clear();
            //Carga la descripcion de la factura
            using (DataTable AcuseRecibo = SAT_CL.CXP.FacturadoProveedor.CargaAcuseReciboFactura(idAcuseRecibo))
            {
                //Asigna valores a los parametros del reporte 
                ReportDataSource rsDescripcionAcuse = new ReportDataSource("DescripcionAcuseFactura", AcuseRecibo);
                //Limpiamos los origenes de datos previos.
                rvReporte.LocalReport.DataSources.Add(rsDescripcionAcuse);
            }
            //Asigna valores a los parametros del reporte
            ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
            //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
            rvReporte.LocalReport.DataSources.Add(rvLogotipo);
        }
        /// <summary>
        /// Método que inicializa los parametros del reporte CajaDevolución.
        /// </summary>
        private void inicializaReporteCajaDevolucion()
        {
            //Almacena el identificador de una devolucion
            int idDevolucion = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del RDLC de CajaDevolucion
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/CajaDevolucion.rdlc");
            //Agrega a la columna de la tabla el parametro Logotipo.
            //Creación de la variable tipo tabla que almacenara el logotipo de la empresa.
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte.
            rvReporte.LocalReport.EnableExternalImages = true;
            //Invoca a la clase Devolucion faltante para obtener la compañia
            using (SAT_CL.Despacho.DevolucionFaltante dev = new SAT_CL.Despacho.DevolucionFaltante(idDevolucion))
            {
                //Invoca a la clase Compania y obtiene los datos de la empresa emisora,
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(dev.id_compania_emisora))
                {
                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter compania = new ReportParameter("Compania", emisor.nombre);
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter telefono = new ReportParameter("Telefono", emisor.telefono);
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString()));
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, color, telefono, fechaImpresion });
                    //Invoca a la clase direccion y obtiene la dirección de la compañia.
                    using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(emisor.id_direccion))
                    {
                        //Creación de variables que almacenan los datos consultados de un registro
                        ReportParameter direccion = new ReportParameter("Direccion", dir.ObtieneDireccionCompleta());
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccion });
                    }
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista ruta de archivo, devuelve un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla el registro con la ruta del logotipo.
                    dtLogo.Rows.Add(logotipo);
                }
            }
            //Limpia el conjunto de datos del reporte
            rvReporte.LocalReport.DataSources.Clear();
            //Asigna a la variable DescripciónDevolución el resultado del método CargaImpresionCajaDevolucio
            using (DataTable DescricionDevolucion = SAT_CL.Despacho.DevolucionFaltante.CargaImpresionCajaDevolucion(idDevolucion))
            {
                //Recorre las filas del dataset
                foreach (DataRow r in DescricionDevolucion.Rows)
                {
                    //Crea variables para los reportes
                    ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { servicio });
                }
                //Crea variables para los reportes
                ReportDataSource rsDescripcionDevolucion = new ReportDataSource("DescripcionCajaDevolucion", DescricionDevolucion);
                rvReporte.LocalReport.DataSources.Add(rsDescripcionDevolucion);
            }
            //Asigna valores a los parametros del reporte
            ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
            //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
            rvReporte.LocalReport.DataSources.Add(rvLogotipo);
        }
        /// <summary>
        /// Método que inicializa los parametros del reporte Comprobante Nomina
        /// </summary>
        private void inicializaReporteComprobanteNomina()
        {
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Almacena el identificador de un comprobante de nomina
            int idComprobanteNomina = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación Local del RDLC de una Nomina
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ComprobanteNomina.rdlc");
            //Creacion del la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Agrega una columna a la table donde almacenara el parametro logotipo
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Agrega una columna a la table donde almacenara el parametro Imagen
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            //Habilita la consulta de imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Limpia el reporte
            rvReporte.LocalReport.DataSources.Clear();
            //Invoca a la casle Nomina empleado para obtener los datos de nomina empleado
            using (SAT_CL.Nomina.NominaEmpleado nomOperador = new SAT_CL.Nomina.NominaEmpleado(idComprobanteNomina))
            {
                //Invoca a la clase operador y obtiene los datos del empleado
                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomOperador.id_empleado))
                {
                    ReportParameter nombreEmpleado = new ReportParameter("NombreEmpleado", op.nombre);
                    ReportParameter rfcEmpleado = new ReportParameter("RFCEmpleado", op.rfc);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreEmpleado, rfcEmpleado });
                    //Invoca a la clase Direccion apra obtener la direccion del empleado
                    using (SAT_CL.Global.Direccion dirEmpleado = new SAT_CL.Global.Direccion(op.id_direccion))
                    {
                        ReportParameter direccionEmpleado = new ReportParameter("DireccionEmpleado", dirEmpleado.ObtieneDireccionCompleta());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmpleado });
                    }
                }
                //Invoca a la clase comprobante y obtiene los datos referentes a la facturacion electronica del comprobante de nomina
                using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(nomOperador.id_comprobante))
                {   //Asignamos valor a las variables
                    serie_descargapdf = comprobante.serie;
                    folio_descargapdf = comprobante.folio.ToString();
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(comprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    rvReporte.LocalReport.DataSources.Add(rvscod);
                    //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                    SAT_CL.FacturacionElectronica.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comprobante.id_comprobante);
                    //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                    ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                    ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);
                    string cadenaOriginal = "";
                    TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comprobante.ruta_xml, Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"),
                                                                     Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), out cadenaOriginal);
                    ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                    ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                    ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                    ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });
                    //Instanciamos a la clase Certificado
                    using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comprobante.id_certificado))
                    {
                        //Cargando certificado (.cer)
                        using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                        {
                            //Asigna los valores instanciados a los parametros
                            ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                            //Asigna valores a los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                        }
                    }
                    //Instancia a la clase Cuenta bancos 
                    using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos(comprobante.id_cuenta_pago))
                    {
                        if (comprobante.id_cuenta_pago == 0 || cb.num_cuenta == "NO IDENTIFICADO")
                        {
                            //Asigna los valores de la clase cuentaBancos a los parametros
                            ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", "No Identificado");
                            //Asigna valores de los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        }
                        else
                        {
                            //Asigna los valores de la clase cuentaBancos a los parametros
                            ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.InvierteCadena(cb.num_cuenta).Substring(0, 4)));
                            //Asigna valores de los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        }
                        //Invoca a la clase banco y obtiene el banco del empleado
                        using (SAT_CL.Bancos.Banco ban = new SAT_CL.Bancos.Banco(cb.id_banco))
                        {
                            ReportParameter banco = new ReportParameter("Banco", ban.nombre_corto);
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
                        }
                    }
                    //Asigna los valores de la clase comprobante a los parametros 
                    ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", comprobante.fecha_expedicion.ToString());
                    ReportParameter serieCFDI = new ReportParameter("SerieCFDI", comprobante.serie);
                    ReportParameter folio = new ReportParameter("Folio", comprobante.folio.ToString());
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, comprobante.id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, comprobante.id_metodo_pago));
                    ReportParameter formaPagoCFDI = new ReportParameter("FormaPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1099, comprobante.id_forma_pago));
                    ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_moneda_nacional.ToString()));
                    ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_moneda_nacional.ToString());
                    ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_moneda_nacional.ToString());
                    ReportParameter importeDeduccion = new ReportParameter("ImporteDeduccion", comprobante.descuento_moneda_nacional.ToString());
                    //Asigna valores a los parametros del reporteComprobante                      
                    rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,metodoPagoCFDI,formaPagoCFDI,importeLetrasCFDI,
                                                                                  totalCFDI, subtotalCFDI,importeDeduccion});
                    //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                    SAT_CL.FacturacionElectronica.Impuesto imp = (SAT_CL.FacturacionElectronica.Impuesto.RecuperaImpuestoComprobante(comprobante.id_comprobante));
                    //Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
                    using (DataTable detalleImp = SAT_CL.FacturacionElectronica.DetalleImpuesto.CargaDetallesImpuesto(imp.id_impuesto))
                    {
                        //Declarando variables auxiliares para recuperar impuestos
                        decimal totalIsr = 0;
                        //Si hay impuestos agregados al comprobante
                        if (imp.id_impuesto > 0)
                        {
                            totalIsr = (from DataRow r in detalleImp.Rows
                                        where r.Field<int>("IdImpuestoRetenido") == 1
                                        select r.Field<decimal>("ImporteMonedaNacional")).FirstOrDefault();
                        }
                        ReportParameter ISR = new ReportParameter("ISR", totalIsr.ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { ISR });
                    }
                }
                //Invoca a la clase Nomina para obtener los datos necesarios para la nomina
                using (SAT_CL.Nomina.Nomina nomina = new SAT_CL.Nomina.Nomina(nomOperador.id_nomina))
                {
                    ReportParameter diasPagados = new ReportParameter("DiasPagados", nomina.dias_pago.ToString());
                    ReportParameter fechaPago = new ReportParameter("FechaPago", nomina.fecha_pago.ToString());
                    ReportParameter fechaInicioPeriodicidad = new ReportParameter("FechaInicioPeriodicidad", nomina.fecha_inicio_pago.ToString());
                    ReportParameter fechaFinPeriodicidad = new ReportParameter("FechaFinPeriodicidad", nomina.fecha_fin_pago.ToString());
                    ReportParameter periodicidad = new ReportParameter("Periodicidad", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3147, nomina.id_periodicidad_pago).ToString().ToUpper());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPagados, fechaPago, fechaInicioPeriodicidad, fechaFinPeriodicidad, periodicidad });
                    //Invoca a la clase compania y obtiene los datos de la compania emisora del comprobante
                    using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(nomina.id_compania_emisora))
                    {
                        ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Referencia.CargaReferencia("0", 25, nomina.id_compania_emisora, "Facturacion Electronica", "Regimen Fiscal"));
                        ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                        ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                        ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, color, regimenFiscalCFDI });
                        //Invoca a la clase direccion para obtener la direcciond e la compañia
                        using (SAT_CL.Global.Direccion dirEmisor = new SAT_CL.Global.Direccion(emisor.id_direccion))
                        {
                            ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", dirEmisor.ObtieneDireccionCompleta());
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmisorMatriz });
                        }
                        //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                        byte[] logotipo = null;
                        //Permite capturar errores en caso de que no exista una ruta para el archivo
                        try
                        {
                            //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                            logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                        }
                        //En caso de que no exista una imagen, se devolvera un valor nulo.
                        catch { logotipo = null; }
                        //Agrega a la tabla un registro con valor a la ruta de la imagen.
                        dtLogo.Rows.Add(logotipo);
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvs);
                    }
                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
                using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccion(idComprobanteNomina))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
                    {
                        ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                    }
                    else
                    {
                        //Creamos Tabla
                        using (DataTable mit = new DataTable())
                        {
                            //Añadimos columnas
                            mit.Columns.Add("Clave", typeof(string));
                            mit.Columns.Add("Concepto", typeof(string));
                            mit.Columns.Add("Importe", typeof(decimal));
                            //Añadimos registro
                            DataRow row = mit.NewRow();
                            row["Clave"] = "000";
                            row["Concepto"] = " ";
                            row["Importe"] = 0;
                            mit.Rows.Add(row);

                            ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
                            rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                        }
                    }
                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
                using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcion(idComprobanteNomina))
                {
                    ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", dtDetalleNominaPercepcion);
                    rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
                }
            }
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", nombrecorto_descargapdf != "" ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);
        }

        /// <summary>
        /// Método que inicializa los parametros del reporte Comprobante Nomina
        /// </summary>
        private void inicializaReporteComprobanteNomina12()
        {
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Almacena el identificador de un comprobante de nomina
            int idComprobanteNomina = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación Local del RDLC de una Nomina
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/ComprobanteNomina12.rdlc");
            //Creacion del la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Agrega una columna a la table donde almacenara el parametro logotipo
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Agrega una columna a la table donde almacenara el parametro Imagen
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            //Habilita la consulta de imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Limpia el reporte
            rvReporte.LocalReport.DataSources.Clear();
            //Invoca a la casle Nomina empleado para obtener los datos de nomina empleado
            using (SAT_CL.Nomina.NomEmpleado nomOperador = new SAT_CL.Nomina.NomEmpleado(idComprobanteNomina))
            {
                //Invoca a la clase operador y obtiene los datos del empleado
                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomOperador.id_empleado))
                {
                    ReportParameter nombreEmpleado = new ReportParameter("NombreEmpleado", op.nombre);
                    ReportParameter rfcEmpleado = new ReportParameter("RFCEmpleado", op.rfc);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreEmpleado, rfcEmpleado });
                    //Invoca a la clase Direccion apra obtener la direccion del empleado
                    using (SAT_CL.Global.Direccion dirEmpleado = new SAT_CL.Global.Direccion(op.id_direccion))
                    {
                        ReportParameter direccionEmpleado = new ReportParameter("DireccionEmpleado", dirEmpleado.ObtieneDireccionCompleta());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmpleado });
                    }
                }
                //Invoca a la clase comprobante y obtiene los datos referentes a la facturacion electronica del comprobante de nomina
                using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(nomOperador.id_comprobante))
                {   //Asignamos valor a las variables
                    serie_descargapdf = comprobante.serie;
                    folio_descargapdf = comprobante.folio.ToString();
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(comprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    rvReporte.LocalReport.DataSources.Add(rvscod);
                    //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                    SAT_CL.FacturacionElectronica.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comprobante.id_comprobante);
                    //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                    ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                    ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);
                    string cadenaOriginal = "";
                    TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comprobante.ruta_xml, Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"),
                                                                     Server.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_TFD_1_0.xslt"), out cadenaOriginal);
                    ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                    ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                    ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                    ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });
                    //Instanciamos a la clase Certificado
                    using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comprobante.id_certificado))
                    {
                        //Cargando certificado (.cer)
                        using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                        {
                            //Asigna los valores instanciados a los parametros
                            ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                            //Asigna valores a los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                        }
                    }


                    ReportParameter banco = new ReportParameter("Banco", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "Banco", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
                    //Asigna los valores de la clase cuentaBancos a los parametros
                    ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "CuentaBancaria", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
                    //Asigna valores de los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });

                    //Asigna los valores de la clase comprobante a los parametros 
                    ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", comprobante.fecha_expedicion.ToString());
                    ReportParameter serieCFDI = new ReportParameter("SerieCFDI", comprobante.serie);
                    ReportParameter folio = new ReportParameter("Folio", comprobante.folio.ToString());
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(80, comprobante.id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(80, comprobante.id_metodo_pago));
                    ReportParameter formaPagoCFDI = new ReportParameter("FormaPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(1099, comprobante.id_forma_pago));
                    ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_moneda_nacional.ToString()));
                    ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_moneda_nacional.ToString());
                    ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_moneda_nacional.ToString());
                    ReportParameter importeDeduccion = new ReportParameter("ImporteDeduccion", comprobante.descuento_moneda_nacional.ToString());
                    //Asigna valores a los parametros del reporteComprobante                      
                    rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,metodoPagoCFDI,formaPagoCFDI,importeLetrasCFDI,
                                                                                  totalCFDI, subtotalCFDI,importeDeduccion});
                    //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                    SAT_CL.FacturacionElectronica.Impuesto imp = (SAT_CL.FacturacionElectronica.Impuesto.RecuperaImpuestoComprobante(comprobante.id_comprobante));
                    //Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
                    using (DataTable detalleImp = SAT_CL.FacturacionElectronica.DetalleImpuesto.CargaDetallesImpuesto(imp.id_impuesto))
                    {
                        //Declarando variables auxiliares para recuperar impuestos
                        decimal totalIsr = 0;
                        //Si hay impuestos agregados al comprobante
                        if (imp.id_impuesto > 0)
                        {
                            totalIsr = (from DataRow r in detalleImp.Rows
                                        where r.Field<int>("IdImpuestoRetenido") == 1
                                        select r.Field<decimal>("ImporteMonedaNacional")).FirstOrDefault();
                        }
                        ReportParameter ISR = new ReportParameter("ISR", totalIsr.ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { ISR });
                    }
                }
                //Invoca a la clase Nomina para obtener los datos necesarios para la nomina
                using (SAT_CL.Nomina.Nomina12 nomina = new SAT_CL.Nomina.Nomina12(nomOperador.id_nomina))
                {
                    ReportParameter diasPagados = new ReportParameter("DiasPagados", nomina.dias_pago.ToString());
                    ReportParameter fechaPago = new ReportParameter("FechaPago", nomina.fecha_pago.ToString());
                    ReportParameter fechaInicioPeriodicidad = new ReportParameter("FechaInicioPeriodicidad", nomina.fecha_inicial_pago.ToString());
                    ReportParameter fechaFinPeriodicidad = new ReportParameter("FechaFinPeriodicidad", nomina.fecha_final_pago.ToString());
                    ReportParameter periodicidad = new ReportParameter("Periodicidad", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3186, nomina.id_periodicidad_pago).ToString().ToUpper());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPagados, fechaPago, fechaInicioPeriodicidad, fechaFinPeriodicidad, periodicidad });
                    //Invoca a la clase compania y obtiene los datos de la compania emisora del comprobante
                    using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(nomina.id_compania_emisor))
                    {
                        ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Referencia.CargaReferencia("0", 25, nomina.id_compania_emisor, "Facturacion Electronica", "Regimen Fiscal"));
                        ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                        ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                        ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, color, regimenFiscalCFDI });
                        //Invoca a la clase direccion para obtener la direcciond e la compañia
                        using (SAT_CL.Global.Direccion dirEmisor = new SAT_CL.Global.Direccion(emisor.id_direccion))
                        {
                            ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", dirEmisor.ObtieneDireccionCompleta());
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmisorMatriz });
                        }
                        //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                        byte[] logotipo = null;
                        //Permite capturar errores en caso de que no exista una ruta para el archivo
                        try
                        {
                            //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                            logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                        }
                        //En caso de que no exista una imagen, se devolvera un valor nulo.
                        catch { logotipo = null; }
                        //Agrega a la tabla un registro con valor a la ruta de la imagen.
                        dtLogo.Rows.Add(logotipo);
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvs);
                    }
                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
                using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccionNuevaV(idComprobanteNomina))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
                    {
                        ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                    }
                    else
                    {
                        //Creamos Tabla
                        using (DataTable mit = new DataTable())
                        {
                            //Añadimos columnas
                            mit.Columns.Add("Clave", typeof(string));
                            mit.Columns.Add("Concepto", typeof(string));
                            mit.Columns.Add("Importe", typeof(decimal));
                            //Añadimos registro
                            DataRow row = mit.NewRow();
                            row["Clave"] = "000";
                            row["Concepto"] = " ";
                            row["Importe"] = 0.00;
                            mit.Rows.Add(row);

                            ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
                            rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                        }
                    }
                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
                using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcionNuevaV(idComprobanteNomina))
                {
                    //Declara Variable que obtendra las filas que contengan datos.                     
                    DataRow[] percepciones = (from DataRow r in dtDetalleNominaPercepcion.Rows
                                              where r.Field<string>("CLAVE") != null
                                              select r).ToArray();

                    ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(percepciones));
                    rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
                }
            }
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", nombrecorto_descargapdf != "" ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);
        }

        /// <summary>
        /// Método que inicializa los parametros del reporte Comprobante Nomina
        /// </summary>
        private void inicializaReporteComprobanteNomina12v33()
        {
            //Declaramos variables para armar el nombre del archivo
            string serie_descargapdf = ""; string folio_descargapdf = ""; string rfc_descargapdf = ""; string nombrecorto_descargapdf = "";
            //Almacena el identificador de un comprobante de nomina
            int idComprobanteNomina = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación Local del RDLC de una Nomina
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/CFDINomina12_33.rdlc");
            //Creacion del la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Creación de la tabla para cargar el QR
            DataTable dtCodigo = new DataTable();
            //Agrega una columna a la table donde almacenara el parametro logotipo
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Agrega una columna a la table donde almacenara el parametro Imagen
            dtCodigo.Columns.Add("Imagen", typeof(byte[]));
            //Habilita la consulta de imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Limpia el reporte
            rvReporte.LocalReport.DataSources.Clear();
            //Invoca a la casle Nomina empleado para obtener los datos de nomina empleado
            using (SAT_CL.Nomina.NomEmpleado nomOperador = new SAT_CL.Nomina.NomEmpleado(idComprobanteNomina))
            {
                //Invoca a la clase operador y obtiene los datos del empleado
                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomOperador.id_empleado))
                {
                    ReportParameter nombreEmpleado = new ReportParameter("NombreEmpleado", op.nombre);
                    ReportParameter rfcEmpleado = new ReportParameter("RFCEmpleado", op.rfc);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreEmpleado, rfcEmpleado });
                    //Invoca a la clase Direccion apra obtener la direccion del empleado
                    using (SAT_CL.Global.Direccion dirEmpleado = new SAT_CL.Global.Direccion(op.id_direccion))
                    {
                        ReportParameter direccionEmpleado = new ReportParameter("DireccionEmpleado", dirEmpleado.ObtieneDireccionCompleta());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmpleado });
                    }
                }
                //Invoca a la clase comprobante y obtiene los datos referentes a la facturacion electronica del comprobante de nomina
                using (SAT_CL.FacturacionElectronica33.Comprobante comprobante = new SAT_CL.FacturacionElectronica33.Comprobante(nomOperador.id_comprobante33))
                {   //Asignamos valor a las variables
                    serie_descargapdf = comprobante.serie;
                    folio_descargapdf = comprobante.folio.ToString();
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] imagen = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        imagen = System.IO.File.ReadAllBytes(comprobante.ruta_codigo_bidimensional);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { imagen = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtCodigo.Rows.Add(imagen);
                    ReportDataSource rvscod = new ReportDataSource("CodigoQR", dtCodigo);
                    rvReporte.LocalReport.DataSources.Add(rvscod);
                    //Declaración del la variable timbre de tipo timbreFiscal para la obtencion de los datos del timbre fiscal
                    SAT_CL.FacturacionElectronica33.TimbreFiscalDigital timbre = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(comprobante.id_comprobante33);
                    //Asigna valores a los parametros obtenidos de la instancia a la clase TimbreFiscal
                    ReportParameter selloDigitalSatCFDI = new ReportParameter("SelloDigitalSatCFDI", timbre.sello_SAT);
                    ReportParameter selloDigitalCFDI = new ReportParameter("SelloDigitalCFDI", timbre.sello_CFD);
                    string cadenaOriginal = "";
                    TSDK.Base.RetornoOperacion resultado = SAT_CL.FacturacionElectronica.SelloDigitalSAT.CadenaCFD(comprobante.ruta_xml, Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                     Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), out cadenaOriginal);
                    ReportParameter cadenaOriginalCFDI = new ReportParameter("CadenaOriginalCFDI", cadenaOriginal);
                    ReportParameter certificadoSerieSAT = new ReportParameter("CertificadoSerieSAT", timbre.no_certificado);
                    ReportParameter fechaCFDI = new ReportParameter("FechaCFDI", timbre.fecha_timbrado.ToString());
                    ReportParameter uuid = new ReportParameter("UUID", timbre.UUID);
                    //Asigna valores a los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { selloDigitalSatCFDI, selloDigitalCFDI, cadenaOriginalCFDI, certificadoSerieSAT, fechaCFDI, uuid });
                    //Instanciamos a la clase Certificado
                    using (SAT_CL.Global.CertificadoDigital certificado = new SAT_CL.Global.CertificadoDigital(comprobante.id_certificado))
                    {
                        //Cargando certificado (.cer)
                        using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(certificado.ruta_llave_publica))
                        {
                            //Asigna los valores instanciados a los parametros
                            ReportParameter certificadoDigitalEmisor = new ReportParameter("CertificadoDigitalEmisor", cer.No_Serie);
                            //Asigna valores a los parametros del reporteComprobante
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { certificadoDigitalEmisor });
                        }
                    }


                    ReportParameter banco = new ReportParameter("Banco", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "Banco", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
                    //Asigna los valores de la clase cuentaBancos a los parametros
                    ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema("1.2", "CuentaBancaria", "Receptor", "Nomina"), nomOperador.id_nomina_empleado, 0));
                    //Asigna valores de los parametros del reporteComprobante
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });

                    //Asigna los valores de la clase comprobante a los parametros 
                    ReportParameter fechaComprobanteCFDI = new ReportParameter("FechaComprobanteCFDI", comprobante.fecha_expedicion.ToString());
                    ReportParameter serieCFDI = new ReportParameter("SerieCFDI", comprobante.serie);
                    ReportParameter folio = new ReportParameter("Folio", comprobante.folio.ToString());
                    string uso_cfdi = SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3194, comprobante.id_uso_receptor);
                    ReportParameter usoCFDI = new ReportParameter("UsoCFDI", uso_cfdi);
                    ReportParameter metodoPagoCFDI = new ReportParameter("MetodoPagoCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3195, comprobante.id_metodo_pago) + "  -  " + SAT_CL.Global.Catalogo.RegresaDescripcioValorCadena(3195, comprobante.id_metodo_pago));
                    ReportParameter formaPagoCFDI;

                    //Instanciando Forma de Pago.
                    using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(comprobante.id_forma_pago))
                        //Asignando Forma de Pago
                        formaPagoCFDI = new ReportParameter("FormaPagoCFDI", fp.descripcion);

                    //Asignando Valores
                    ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_nacional.ToString()));
                    ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_nacional.ToString());
                    ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_nacional.ToString());
                    ReportParameter importeDeduccion = new ReportParameter("ImporteDeduccion", comprobante.descuentos_nacional.ToString());
                    //Asigna valores a los parametros del reporteComprobante                      
                    rvReporte.LocalReport.SetParameters(new ReportParameter[]{fechaComprobanteCFDI, serieCFDI,folio,metodoPagoCFDI,formaPagoCFDI,importeLetrasCFDI,
                                                                                  totalCFDI, subtotalCFDI,importeDeduccion});
                    //Crea un objeto de tipo impuesto que permite obtener los impuestos dado un id_comprobante
                    SAT_CL.FacturacionElectronica33.Impuesto imp = (SAT_CL.FacturacionElectronica33.Impuesto.ObtieneImpuestoComprobante(comprobante.id_comprobante33));
                    //Instancia la clase DetalleImpuesto para obtener el desglose de los impuestos dado un id_impuesto
                    using (DataTable detalleImp = SAT_CL.FacturacionElectronica33.ImpuestoDetalle.ObtieneDetallesImpuesto(imp.id_impuesto))
                    {
                        //Declarando variables auxiliares para recuperar impuestos
                        decimal totalIsr = 0;
                        //Si hay impuestos agregados al comprobante
                        if (imp.id_impuesto > 0)
                        {
                            totalIsr = (from DataRow r in detalleImp.Rows
                                        where r.Field<string>("Detalle").Equals("Retencion")
                                        select r.Field<decimal>("ImporteNacional")).Sum();
                        }
                        ReportParameter ISR = new ReportParameter("ISR", totalIsr.ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { ISR });
                    }

                    //Invoca a la clase Nomina para obtener los datos necesarios para la nomina
                    using (SAT_CL.Nomina.Nomina12 nomina = new SAT_CL.Nomina.Nomina12(nomOperador.id_nomina))
                    {
                        ReportParameter diasPagados = new ReportParameter("DiasPagados", nomina.dias_pago.ToString());
                        ReportParameter fechaPago = new ReportParameter("FechaPago", nomina.fecha_pago.ToString());
                        ReportParameter fechaInicioPeriodicidad = new ReportParameter("FechaInicioPeriodicidad", nomina.fecha_inicial_pago.ToString());
                        ReportParameter fechaFinPeriodicidad = new ReportParameter("FechaFinPeriodicidad", nomina.fecha_final_pago.ToString());
                        ReportParameter periodicidad = new ReportParameter("Periodicidad", SAT_CL.Global.Catalogo.RegresaDescripcionCatalogo(3186, nomina.id_periodicidad_pago).ToString().ToUpper());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPagados, fechaPago, fechaInicioPeriodicidad, fechaFinPeriodicidad, periodicidad });
                        //Invoca a la clase compania y obtiene los datos de la compania emisora del comprobante
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(nomina.id_compania_emisor))
                        {
                            //Asignando Parametros
                            ReportParameter regimenFiscalCFDI = new ReportParameter("RegimenFiscalCFDI", SAT_CL.Global.Catalogo.RegresaDescripcionCadenaValor(3197, comprobante.regimen_fiscal));
                            ReportParameter razonSocialEmisorCFDI = new ReportParameter("RazonSocialEmisorCFDI", emisor.nombre);
                            ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                            ReportParameter rfcEmisorCFDI = new ReportParameter("RFCEmisorCFDI", emisor.rfc);
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { razonSocialEmisorCFDI, rfcEmisorCFDI, color, regimenFiscalCFDI });
                            //Invoca a la clase direccion para obtener la direcciond e la compañia
                            using (SAT_CL.Global.Direccion dirEmisor = new SAT_CL.Global.Direccion(emisor.id_direccion))
                            {
                                ReportParameter direccionEmisorMatriz = new ReportParameter("DireccionEmisorMatriz", dirEmisor.codigo_postal);
                                rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccionEmisorMatriz });
                            }
                            //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                            byte[] logotipo = null;
                            //Permite capturar errores en caso de que no exista una ruta para el archivo
                            try
                            {
                                //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                                logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                            }
                            //En caso de que no exista una imagen, se devolvera un valor nulo.
                            catch { logotipo = null; }
                            //Agrega a la tabla un registro con valor a la ruta de la imagen.
                            dtLogo.Rows.Add(logotipo);
                            //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                            ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                            //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                            rvReporte.LocalReport.DataSources.Add(rvs);
                        }
                    }
                    //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
                    using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccionNuevaV(idComprobanteNomina))
                    {
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
                        {
                            ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
                            rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                        }
                        else
                        {
                            //Creamos Tabla
                            using (DataTable mit = new DataTable())
                            {
                                //Añadimos columnas
                                mit.Columns.Add("Clave", typeof(string));
                                mit.Columns.Add("Concepto", typeof(string));
                                mit.Columns.Add("Importe", typeof(decimal));
                                //Añadimos registro
                                DataRow row = mit.NewRow();
                                row["Clave"] = "000";
                                row["Concepto"] = " ";
                                row["Importe"] = 0.00;
                                mit.Rows.Add(row);

                                ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
                                rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                            }
                        }
                    }
                    //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
                    using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcionNuevaV(idComprobanteNomina))
                    {
                        //Declara Variable que obtendra las filas que contengan datos.                     
                        DataRow[] percepciones = (from DataRow r in dtDetalleNominaPercepcion.Rows
                                                  where r.Field<string>("CLAVE") != null
                                                  select r).ToArray();

                        ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(percepciones));
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
                    }
                }
            }
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}{2}.pdf", nombrecorto_descargapdf != "" ? nombrecorto_descargapdf : rfc_descargapdf, serie_descargapdf, folio_descargapdf), TSDK.Base.Archivo.ContentType.application_PDF);
        }

        /// <summary>
        /// Método que inicializa los parametros del reporte Ficha Ingreso
        /// </summary>
        private void inicializaReporteFichaIngreso()
        {
            //Variable que almacena el identificador de una ficha de ingreso del query string
            int idFichaIngreso = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte de Ficha de Ingreso
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/FichaIngreso.rdlc");
            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();
            //Invoca a la clase EgresoIngreso y obtiene los datos principales de la ficha de ingreso
            using (SAT_CL.Bancos.EgresoIngreso ficha = new SAT_CL.Bancos.EgresoIngreso(idFichaIngreso))
            {
                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa                                
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(ficha.id_compania))
                {
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(ficha.id_compania))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });

                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }
                //Invoca a la clase EgresoIngreso y almacena en el datatable el reultado del método EncabezadoFichaIngreso
                using (DataTable encabezado = SAT_CL.Bancos.EgresoIngreso.EncabezadoFichaIngreso(idFichaIngreso))
                {
                    //Recorre las filas de la tabla y alacena en r los valores encontrados
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter noFicha = new ReportParameter("NoFicha", r["NoFicha"].ToString());
                        ReportParameter cliente = new ReportParameter("Cliente", r["Depositante"].ToString());
                        ReportParameter concepto = new ReportParameter("Concepto", r["Concepto"].ToString());
                        ReportParameter metodoPago = new ReportParameter("MetodoPago", r["MetodoPago"].ToString());
                        ReportParameter cuentaOrigen = new ReportParameter("CuentaOrigen", r["CuentaOrigen"].ToString());
                        ReportParameter cuentaDestino = new ReportParameter("CuentaDestino", r["CuentaDestino"].ToString());
                        ReportParameter noCheque = new ReportParameter("NoCheque", r["NoCheque"].ToString());
                        ReportParameter fecha = new ReportParameter("Fecha", r["Fecha"].ToString());
                        ReportParameter monto = new ReportParameter("Monto", r["Monto"].ToString());
                        ReportParameter moneda = new ReportParameter("Moneda", r["Moneda"].ToString());
                        ReportParameter fechaCaptura = new ReportParameter("FechaCaptura", r["FechaCaptura"].ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { noFicha,cliente, concepto, metodoPago, cuentaOrigen, cuentaDestino, noCheque, fecha, monto,
                                                                                    moneda,fechaCaptura});
                    }
                }
                //Invoca al método FacturasAplicadasFichaIngreso y almacena el resultado en la tabla Facturasaplicadas
                using (DataTable Facturasaplicadas = SAT_CL.CXC.FichaIngresoAplicacion.FacturasAplicadasFichaIngreso(idFichaIngreso))
                {
                    //Asigna al origen de datos FacturasAplicadasFI los datos almacenados en la tabla Facturasaplicadas
                    ReportDataSource rsFacturasAplicadas = new ReportDataSource("FacturasAplicadasFI", Facturasaplicadas);
                    rvReporte.LocalReport.DataSources.Add(rsFacturasAplicadas);
                }
            }

        }
        /// <summary>
        /// Método que inicializa los parametros del reporte Finiquito
        /// </summary>
        private void inicializaReporteFiniquito()
        {
            //Variable que alamcena el identificador de una ficha de ingreso de query string
            int idFiniquito = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte de Finiquito
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Finiquito.rdlc");
            //Creación de la variable tipo tabla dtLogo
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            //Limpia los origenes de datos
            rvReporte.LocalReport.DataSources.Clear();
            //Instancia a la clase NominaEmpleado
            using (SAT_CL.Nomina.NominaEmpleado nomina = new SAT_CL.Nomina.NominaEmpleado(idFiniquito))
            {
                ReportParameter dia = new ReportParameter("Dia", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Day.ToString());
                ReportParameter meses = new ReportParameter("Meses", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month)));
                ReportParameter anios = new ReportParameter("Anios", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year.ToString());

                rvReporte.LocalReport.SetParameters(new ReportParameter[] { dia, meses, anios });
                //Invoca a la clase Operador
                using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(nomina.id_empleado))
                {
                    ////Declara variable antTabajador y almacena la fecha actual
                    DateTime fechaActual = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
                    ////Variable de tipo entero que almace el año de la fecha de ingreso del empleado
                    DateTime fechaIngreso = op.fecha_ingreso;
                    //Creación del objeto timespan que almacena el resultado de la resta de fechaactual menos la fechaIngreso
                    TimeSpan TS = new TimeSpan();
                    TS = fechaActual - fechaIngreso;
                    //Crea las variables de tipo entero año y mese
                    int anio = 0, mes = 0;
                    //Asigna a la variable años los años de antiguedad
                    anio = TS.Days / 365;
                    //Asigna a la variable meses los meses de antiguedad
                    mes = Convert.ToInt32((TS.Days - (anio * 365)) / 30.4167);
                    //Valida que loe el mes y año sea mayor a 0
                    if (anio > 0 && mes > 0)
                    {
                        ReportParameter antiguedad = new ReportParameter("Antiguedad", string.Format("{0} Años  y {1} Meses", anio, mes));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { antiguedad });
                    }
                    //Si el año es igual a cero
                    else if (anio == 0)
                    {
                        //Solo muestra los meses
                        ReportParameter antiguedad = new ReportParameter("Antiguedad", string.Format("{0} Meses", mes));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { antiguedad });
                    }
                    //Si el mes es igual a cero
                    else if (mes == 0)
                    {
                        //Solo muestra lo años
                        ReportParameter antiguedad = new ReportParameter("Antiguedad", string.Format("{0} Años", anio));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { antiguedad });
                    }
                    //Obtiene el nombre del empleado
                    ReportParameter empleado = new ReportParameter("Empleado", op.nombre);
                    ReportParameter salarioDiario = new ReportParameter("SalarioDiario", TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Recibo Nómina", "Salario Base Cotizacion Apor"), "0.0"));
                    ReportParameter puesto = new ReportParameter("Puesto", SAT_CL.Global.Referencia.CargaReferencia("0", 76, op.id_operador, "Recibo Nómina", "Puesto"));
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { empleado, salarioDiario, puesto });
                    //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa

                }
                //Instancia a la clase comprobante
                using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(nomina.id_comprobante))
                {
                    ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_moneda_nacional.ToString()));
                    ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_moneda_nacional.ToString());
                    ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_moneda_nacional.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { totalCFDI, importeLetrasCFDI, subtotalCFDI });
                    //Instancia a la clase Cuenta bancos 
                    using (SAT_CL.Bancos.CuentaBancos cb = new SAT_CL.Bancos.CuentaBancos(comprobante.id_cuenta_pago))
                    {
                        //Asigna los valores de la clase cuentaBancos a los parametros
                        ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", cb.num_cuenta);
                        //Asigna valores de los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        //Invoca a la clase banco y obtiene el banco del empleado
                        using (SAT_CL.Bancos.Banco ban = new SAT_CL.Bancos.Banco(cb.id_banco))
                        {
                            ReportParameter banco = new ReportParameter("Banco", ban.nombre_corto);
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
                        }
                    }

                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
                using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccion(idFiniquito))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
                    {
                        ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                    }
                    else
                    {
                        //Creamos Tabla
                        using (DataTable mit = new DataTable())
                        {
                            //Añadimos columnas
                            mit.Columns.Add("Clave", typeof(string));
                            mit.Columns.Add("Concepto", typeof(string));
                            mit.Columns.Add("Importe", typeof(decimal));
                            //Añadimos registro
                            DataRow row = mit.NewRow();
                            row["Clave"] = "000";
                            row["Concepto"] = " ";
                            row["Importe"] = 0.00;
                            mit.Rows.Add(row);

                            ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
                            rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                        }
                    }
                }
                //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
                using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcion(idFiniquito))
                {
                    ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", dtDetalleNominaPercepcion);
                    rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
                }

            }
        }

        /// <summary>
        /// Método que inicializa los parametros del reporte Finiquito
        /// </summary>
        private void inicializaReporteFiniquito12()
        {
            //Variable que alamcena el identificador de una ficha de ingreso de query string
            int idFiniquito = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte de Finiquito
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Finiquito12.rdlc");
            //Creación de la variable tipo tabla dtLogo
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            //Limpia los origenes de datos
            rvReporte.LocalReport.DataSources.Clear();
            //Instancia a la clase NominaEmpleado
            using (SAT_CL.Nomina.NomEmpleado objNominaEmpleado = new SAT_CL.Nomina.NomEmpleado(idFiniquito))
            {
                //InstanciamosNomina
                using (SAT_CL.Nomina.Nomina12 objNomina = new Nomina12(objNominaEmpleado.id_nomina))
                {
                    ReportParameter dia = new ReportParameter("Dia", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Day.ToString());
                    ReportParameter meses = new ReportParameter("Meses", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month)));
                    ReportParameter anios = new ReportParameter("Anios", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year.ToString());

                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { dia, meses, anios });
                    //Invoca a la clase Operador
                    using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(objNominaEmpleado.id_empleado))
                    {
                        ////Declara variable antTabajador y almacena la fecha actual
                        DateTime fechaActual = objNomina.fecha_final_pago;
                        ////Variable de tipo entero que almace el año de la fecha de ingreso del empleado
                        DateTime fechaIngreso = op.fecha_ingreso;
                        //Obtenemos mensaje
                        string mensaje = TSDK.Base.Cadena.ObtieneFormatoFecha(fechaIngreso, fechaActual);
                        //Solo muestra lo años
                        ReportParameter antiguedad = new ReportParameter("Antiguedad", mensaje);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { antiguedad });
                        //Obtiene el nombre del empleado
                        ReportParameter empleado = new ReportParameter("Empleado", op.nombre);
                        ReportParameter salarioDiario = new ReportParameter("SalarioDiario", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema(objNomina.version, "SalarioDiarioIntegrado", "Receptor", "Nomina"), objNominaEmpleado.id_nomina_empleado, 0));
                        ReportParameter puesto = new ReportParameter("Puesto", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema(objNomina.version, "Puesto", "Receptor", "Nomina"), objNominaEmpleado.id_nomina_empleado, 0));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { empleado, salarioDiario, puesto });
                        //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa

                    }
                    //Instancia a la clase comprobante
                    using (SAT_CL.FacturacionElectronica.Comprobante comprobante = new SAT_CL.FacturacionElectronica.Comprobante(objNominaEmpleado.id_comprobante))
                    {
                        ReportParameter importeLetrasCFDI = new ReportParameter("ImporteLetrasCFDI", TSDK.Base.Cadena.ConvierteMontoALetra(comprobante.total_moneda_nacional.ToString()));
                        ReportParameter subtotalCFDI = new ReportParameter("SubtotalCFDI", comprobante.subtotal_moneda_nacional.ToString());
                        ReportParameter totalCFDI = new ReportParameter("TotalCFDI", comprobante.total_moneda_nacional.ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { totalCFDI, importeLetrasCFDI, subtotalCFDI });
                        //Asigna los valores de la clase cuentaBancos a los parametros
                        ReportParameter cuentaPagoCFDI = new ReportParameter("CuentaPagoCFDI", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema(objNomina.version, "CuentaBancaria", "Receptor", "Nomina"), objNominaEmpleado.id_nomina_empleado, 0));
                        //Asigna valores de los parametros del reporteComprobante
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { cuentaPagoCFDI });
                        ReportParameter banco = new ReportParameter("Banco", EsquemaRegistro.ObtieneValorEsquemaRegistro(Esquema.ObtieneIdEsquema(objNomina.version, "Banco", "Receptor", "Nomina"), objNominaEmpleado.id_nomina_empleado, 0));
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { banco });
                    }
                    //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de deducción de una nomina
                    using (DataTable dtDetalleNominaDeduccion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaDeduccionNuevaV(idFiniquito))
                    {
                        //Validamos Origen de Datos
                        if (Validacion.ValidaOrigenDatos(dtDetalleNominaDeduccion))
                        {
                            ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", dtDetalleNominaDeduccion);
                            rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                        }
                        else
                        {
                            //Creamos Tabla
                            using (DataTable mit = new DataTable())
                            {
                                //Añadimos columnas
                                mit.Columns.Add("Clave", typeof(string));
                                mit.Columns.Add("Concepto", typeof(string));
                                mit.Columns.Add("Importe", typeof(decimal));
                                //Añadimos registro
                                DataRow row = mit.NewRow();
                                row["Clave"] = "000";
                                row["Concepto"] = " ";
                                row["Importe"] = 0.00;
                                mit.Rows.Add(row);

                                ReportDataSource rsComprobanteNominaDeduccion = new ReportDataSource("ComprobanteNominaDeduccion", mit);
                                rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaDeduccion);
                            }
                        }
                    }
                    //Invoca al método Concepto ComprobanteNominaDeduccion que obtiene los conceptos de percepción de una nomina
                    using (DataTable dtDetalleNominaPercepcion = SAT_CL.Nomina.Reporte.ConceptoComprobanteNominaPercepcionNuevaV(idFiniquito))
                    {
                        ReportDataSource rsComprobanteNominaPercepcion = new ReportDataSource("ComprobanteNominaPercepcion", dtDetalleNominaPercepcion);
                        rvReporte.LocalReport.DataSources.Add(rsComprobanteNominaPercepcion);
                    }

                }
            }
        }

        /// <summary>
        /// Método que inicializa los parametros del reporte Requisición.
        /// </summary>
        private void inicializaReporteRequisicion()
        {
            //Instanciando Compania
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //SI ES LA EMPRESA AXEJIT CARGARA SU FORMATO
                switch (cer.id_compania_emisor_receptor)
                {
                    case 1353:
                    case 72:
                        {
                            //Variable qie almacena el identificador de una requisicion
                            int idRequisicion = Convert.ToInt32(Request.QueryString["idRegistro"]);
                            //Declara la ubicación local del reporte Requisicion
                            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/RequisicionAxejit.rdlc");
                            //Creación de la variable tipo tabla dtLogo.
                            DataTable dtLogo = new DataTable();
                            //Agrega una columna a la tabla 
                            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
                            //Habilita la consulta de imagenes externas al reporte
                            rvReporte.LocalReport.EnableExternalImages = true;
                            rvReporte.LocalReport.DataSources.Clear();
                            //Invoca a la clase EgresoIngreso y obtiene los datos principales de la ficha de ingreso
                            using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(idRequisicion))
                            {
                                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(req.id_compania_emisora))
                                {
                                    //Recorre las filas de la tabla
                                    foreach (DataRow r in encabezado.Rows)
                                    {
                                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion });
                                    }
                                }
                                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(req.id_compania_emisora))
                                {
                                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                                    byte[] logotipo = null;
                                    //Captura errores al momento de consultar la ubicación del logotipo.
                                    try
                                    {
                                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                                    }
                                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                                    catch { logotipo = null; }
                                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                                    dtLogo.Rows.Add(logotipo);
                                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                                    rvReporte.LocalReport.DataSources.Add(rvs);
                                }
                                //Parametros del encabezado de la requisición 
                                ReportParameter noRequisicion = new ReportParameter("NoRequisicion", req.no_requisicion.ToString());
                                ReportParameter fechaSolicitud = new ReportParameter("FechaSolicitud", req.fecha_solitud.ToString());
                                ReportParameter fechaRequerida = new ReportParameter("FechaRequerida", req.fecha_entrega_requerida.ToString());
                                ReportParameter referencia = new ReportParameter("Referencia", Cadena.TruncaCadena(req.referencia, 7, "..."));
                                rvReporte.LocalReport.SetParameters(new ReportParameter[] { noRequisicion, fechaSolicitud, fechaRequerida, referencia });
                                //Instancia a la clase almacen para obtener los datos necesarios del almacen
                                using (SAT_CL.Almacen.Almacen al = new SAT_CL.Almacen.Almacen(req.id_almacen))
                                {
                                    ReportParameter almacen = new ReportParameter("Almacen", al.descripcion);
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { almacen });
                                }
                                //Instancia para obtener los datos del soicitante
                                using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(req.id_usuario_solicitante))
                                {
                                    ReportParameter solicitante = new ReportParameter("Solicitante", us.nombre);
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { solicitante });
                                }
                                using (SAT_CL.Documentacion.Servicio s = new SAT_CL.Documentacion.Servicio(1))//req.id_servicio))
                                {
                                    ReportParameter citaCarga = new ReportParameter("CitaCarga", s.cita_carga.ToString());
                                    ReportParameter citaDescarga = new ReportParameter("CitaDescarga", s.cita_descarga.ToString());
                                    string NoViaje = SAT_CL.Global.Referencia.CargaReferencia("0", 1, s.id_servicio, "Referencia de Viaje", "No.Viaje");
                                    string Embarque = SAT_CL.Global.Referencia.CargaReferencia("0", 1, s.id_servicio, "Referencia de Viaje", "Embarque");
                                    ReportParameter viaje = new ReportParameter("Viaje", NoViaje);
                                    ReportParameter embarque = new ReportParameter("Embarque", Embarque);
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { citaCarga, citaDescarga, viaje, embarque });
                                }
                                //Invoca a clase requisición detalle y obtiene los detalles de una requisición
                                using (DataTable DetalleRequsicion = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicionAxejit(req.id_requisicion))
                                {
                                    ReportDataSource rsDetalleRequisicion = new ReportDataSource("DetalleRequisicion", DetalleRequsicion);
                                    rvReporte.LocalReport.DataSources.Add(rsDetalleRequisicion);
                                }
                            }
                            break;
                        }
                    //En caso contrario carga la plantilla por default de requisición
                    default:
                        {
                            //Variable qie almacena el identificador de una requisicion
                            int idRequisicion = Convert.ToInt32(Request.QueryString["idRegistro"]);
                            //Declara la ubicación local del reporte Requisicion
                            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/Requisicion.rdlc");
                            //Creación de la variable tipo tabla dtLogo.
                            DataTable dtLogo = new DataTable();
                            //Agrega una columna a la tabla 
                            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
                            //Habilita la consulta de imagenes externas al reporte
                            rvReporte.LocalReport.EnableExternalImages = true;
                            rvReporte.LocalReport.DataSources.Clear();
                            //Invoca a la clase EgresoIngreso y obtiene los datos principales de la ficha de ingreso
                            using (SAT_CL.Almacen.Requisicion req = new SAT_CL.Almacen.Requisicion(idRequisicion))
                            {
                                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(req.id_compania_emisora))
                                {
                                    //Recorre las filas de la tabla
                                    foreach (DataRow r in encabezado.Rows)
                                    {
                                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                                        ReportParameter referencia = new ReportParameter("Referencia", Cadena.TruncaCadena(req.referencia, 7, "..."));
                                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion, referencia });
                                    }
                                }
                                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(req.id_compania_emisora))
                                {
                                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                                    byte[] logotipo = null;
                                    //Captura errores al momento de consultar la ubicación del logotipo.
                                    try
                                    {
                                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                                    }
                                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                                    catch { logotipo = null; }
                                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                                    dtLogo.Rows.Add(logotipo);
                                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                                    rvReporte.LocalReport.DataSources.Add(rvs);
                                }
                                //Parametros del encabezado de la requisición 
                                ReportParameter noRequisicion = new ReportParameter("NoRequisicion", req.no_requisicion.ToString());
                                ReportParameter fechaSolicitud = new ReportParameter("FechaSolicitud", req.fecha_solitud.ToString());
                                ReportParameter fechaRequerida = new ReportParameter("FechaRequerida", req.fecha_entrega_requerida.ToString());
                                rvReporte.LocalReport.SetParameters(new ReportParameter[] { noRequisicion, fechaSolicitud, fechaRequerida });
                                //Instancia a la clase almacen para obtener los datos necesarios del almacen
                                using (SAT_CL.Almacen.Almacen al = new SAT_CL.Almacen.Almacen(req.id_almacen))
                                {
                                    ReportParameter almacen = new ReportParameter("Almacen", al.descripcion);
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { almacen });
                                }
                                //Instancia para obtener los datos del soicitante
                                using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(req.id_usuario_solicitante))
                                {
                                    ReportParameter solicitante = new ReportParameter("Solicitante", us.nombre);
                                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { solicitante });
                                }
                                //Invoca a clase requisición detalle y obtiene los detalles de una requisición
                                using (DataTable DetalleRequsicion = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(req.id_requisicion))
                                {
                                    ReportDataSource rsDetalleRequisicion = new ReportDataSource("DetalleRequisicion", DetalleRequsicion);
                                    rvReporte.LocalReport.DataSources.Add(rsDetalleRequisicion);
                                }
                            }
                            break;
                        }
                }
            }

        }
        /// <summary>
        /// Método que inicializa los valores de los parametros del reporte Orden de Trabajo
        /// </summary>
        private void inicializaReporteOrdenTrabajo()
        {
            //Variable qie almacena el identificador de una orden de trabajo
            int idOrdenTrabajo = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte Orden de trabajo
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/OrdenTrabajo.rdlc");
            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();
            //Invoca a la clase OrdenTrabajo y obtiene los datos principales de la Orden de Trabajo
            using (SAT_CL.Mantenimiento.OrdenTrabajo orden = new SAT_CL.Mantenimiento.OrdenTrabajo(idOrdenTrabajo))
            {
                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(orden.id_compania_emisora))
                {
                    //Recorre las filas de la tabla
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(orden.id_compania_emisora))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                //Encabezado de la orden de trabajo
                using (DataTable encabezado = SAT_CL.Mantenimiento.OrdenTrabajo.CargaEncabezadoOrdenTrabajo(idOrdenTrabajo))
                {
                    //Valida que el datatable encabezado contenga valores vaidos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(encabezado))
                    {
                        //Recorre las filas del datatable y el resultado los almacena en los parametros del reporte
                        foreach (DataRow r in encabezado.Rows)
                        {
                            ReportParameter noOrden = new ReportParameter("NoOrden", r["NoOrden"].ToString());
                            ReportParameter estatusOrdenTrabajo = new ReportParameter("EstatusOrdenTrabajo", r["Estatus"].ToString());
                            ReportParameter fechaRecepcion = new ReportParameter("FechaRecepcion", r["FechaRecepcion"].ToString());
                            ReportParameter fechaCompromiso = new ReportParameter("FechaCompromiso", r["FechaCompromiso"].ToString());
                            ReportParameter odometro = new ReportParameter("Odometro", r["Odometro"].ToString());
                            ReportParameter nivelCombustible = new ReportParameter("NivelCombustible", r["NivelCombustible"].ToString());
                            ReportParameter quienEntrega = new ReportParameter("QuienEntrega", r["Entegado"].ToString());
                            ReportParameter quienRecibe = new ReportParameter("QuienRecibe", r["Recibe"].ToString());
                            ReportParameter propietarioUnidad = new ReportParameter("PropietarioUnidad", r["PropietarioUnidad"].ToString());
                            ReportParameter rfcPU = new ReportParameter("RFCPU", r["RFCPU"].ToString());
                            ReportParameter direccionPU = new ReportParameter("DireccionPU", r["DireccionPU"].ToString());
                            ReportParameter telefonoPU = new ReportParameter("TelefonoPU", r["TelefonoPU"].ToString());
                            ReportParameter emailPU = new ReportParameter("EmailPU", r["EmailPU"].ToString());
                            ReportParameter unidad = new ReportParameter("Unidad", r["Unidad"].ToString());
                            ReportParameter tipoUnidad = new ReportParameter("TipoUnidad", r["Tipo"].ToString());
                            ReportParameter subTipoUnidad = new ReportParameter("SubTipoUnidad", r["SubTipo"].ToString());
                            ReportParameter nombreTaller = new ReportParameter("NombreTaller", r["NombreTaller"].ToString());
                            ReportParameter telefonoTaller = new ReportParameter("TelefonoTaller", r["TelefonoTaller"].ToString());
                            ReportParameter propietarioTaller = new ReportParameter("PropietarioTaller", r["PropietarioTaller"].ToString());
                            ReportParameter rfcPropietarioTaller = new ReportParameter("RFCPropietarioTaller", r["RFCPropietarioTaller"].ToString());
                            ReportParameter ubicacionTaller = new ReportParameter("UbicacionTaller", r["UbicacionTaller"].ToString());
                            ReportParameter numeroSiniestro = new ReportParameter("NumeroSiniestro", r["NoSiniestro"].ToString());
                            //Agrega los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[]{noOrden,estatusOrdenTrabajo,fechaRecepcion,fechaCompromiso,odometro,nivelCombustible,quienEntrega,
                                                                                      quienRecibe,propietarioUnidad,rfcPU,direccionPU,telefonoPU,emailPU,unidad,tipoUnidad,subTipoUnidad,
                                                                                      telefonoTaller,nombreTaller,propietarioTaller,rfcPropietarioTaller,numeroSiniestro, ubicacionTaller});
                        }
                    }
                }
                //Carga las actividades y fallas de la orden de trabajo
                using (DataTable dtAFOT = SAT_CL.Mantenimiento.OrdenTrabajo.CargaActividadesOrdenTrabajo(idOrdenTrabajo))
                {
                    //Valida los datos obtenidos de la consulta (que no sean nulos)
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAFOT))
                    {
                        //Asigna al origen de datos los valores del la tabal que almacena la consulta
                        ReportDataSource rsAFOT = new ReportDataSource("ActividadFalla", dtAFOT);
                        rvReporte.LocalReport.DataSources.Add(rsAFOT);
                    }
                    //Si no existen datos
                    else
                    {
                        //Crea una tabla temporal y declara las columnas necesarias.
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("Falla", typeof(string));
                        dtTem.Columns.Add("FechaFalla", typeof(string));
                        dtTem.Columns.Add("Actividad", typeof(string));
                        dtTem.Columns.Add("Estatus", typeof(string));
                        dtTem.Columns.Add("FechaInicio", typeof(string));
                        dtTem.Columns.Add("FechaFin", typeof(string));
                        dtTem.Columns.Add("Duracion", typeof(string));
                        dtTem.Columns.Add("DuracionReal", typeof(string));
                        //Inserta una fila en blanco a la tabla temporal
                        dtTem.Rows.Add("", "", "", "", "", "", "", "");
                        //Asigna al origen de datos lo que contenga la tabla temporal
                        ReportDataSource rsAFOT = new ReportDataSource("ActividadFalla", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsAFOT);
                    }

                }
                //Carga los productos necesarios para la orden de trabajo
                using (DataTable dtProducto = SAT_CL.Mantenimiento.OrdenTrabajo.CargaProductoOrdenTrabajo(idOrdenTrabajo))
                {
                    //Valida los datos del datatable (que no sean nulos)
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtProducto))
                    {
                        //Asigna los valores al conjunto de datos
                        ReportDataSource rsProducto = new ReportDataSource("ProductoOT", dtProducto);
                        rvReporte.LocalReport.DataSources.Add(rsProducto);
                    }
                    //En caso contrario
                    else
                    {
                        //Crea una tabla temporal y declara las columnas necesarias.
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("Producto", typeof(string));
                        dtTem.Columns.Add("CodProducto", typeof(string));
                        dtTem.Columns.Add("Total", typeof(int));
                        dtTem.Columns.Add("Cantidad", typeof(string));
                        dtTem.Columns.Add("Precio", typeof(int));
                        //Inserta una fila en blanco a la tabla temporal
                        dtTem.Rows.Add("", "", 0, "", 0);
                        //Asigna al origen de datos lo que contenga la tabla temporal
                        ReportDataSource rsProducto = new ReportDataSource("ProductoOT", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsProducto);
                    }


                }
                //Carga las asignaciones de trabajo
                using (DataTable dtAsignaciones = SAT_CL.Mantenimiento.OrdenTrabajo.CargaAsignacionesOrdenTrabajo(idOrdenTrabajo))
                {
                    //Valida los datos de la tabla (que no sean nulos)
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAsignaciones))
                    {
                        //Asigna los valores al conjunto de datos
                        ReportDataSource rsAsignacion = new ReportDataSource("Asignacion", dtAsignaciones);
                        rvReporte.LocalReport.DataSources.Add(rsAsignacion);
                    }
                    else
                    {
                        //Crea una tabla temporal y declara las columnas necesarias.
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("Actividad", typeof(string));
                        dtTem.Columns.Add("Asignado", typeof(string));
                        dtTem.Columns.Add("Puesto", typeof(string));
                        dtTem.Columns.Add("CostoHora", typeof(int));
                        dtTem.Columns.Add("ManoObra", typeof(int));
                        //Inserta una fila en blanco a la tabla temporal
                        dtTem.Rows.Add("", "", "", 0, 0);
                        //Asigna al origen de datos lo que contenga la tabla temporal
                        ReportDataSource rsAsignacion = new ReportDataSource("Asignacion", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsAsignacion);
                    }

                }
            }
        }
        /// <summary>
        /// Método que inicializa los calores de los parametros del reporte Orden de Compra
        /// </summary>
        private void inicializaReporteOrdenCompra()
        {
            //Variable qie almacena el identificador de una orden de compra
            int idOrdenCompra = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte orden de compra
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/OrdenCompra.rdlc");

            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            DataTable dtFirmaU = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            dtFirmaU.Columns.Add("Imagen", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();

            //Invoca a la clase Orden de Compra y obtiene los datos principales de la Orden de compra
            using (SAT_CL.Almacen.OrdenCompra orden = new SAT_CL.Almacen.OrdenCompra(idOrdenCompra))
            {
                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(orden.id_compania_emisor))
                {
                    //Recorre las filas de la tabla
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        ReportParameter email = new ReportParameter("Email", r["email"].ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion, email });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(orden.id_compania_emisor))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                //Instanciando Usuario
                using (SAT_CL.Seguridad.Usuario user = ((SAT_CL.Seguridad.Usuario)Session["usuario"]))
                {
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] firma_img = null;

                    //Validando Usuario
                    if (user.habilitar)
                    {
                        //Obteniendo Archivos de Firma
                        using (DataTable dtFirma = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(30, user.id_usuario, 24, 0))
                        {
                            //Validando Firma
                            if (Validacion.ValidaOrigenDatos(dtFirma))
                            {
                                //Recorriendo Fila
                                foreach (DataRow dr in dtFirma.Rows)
                                {
                                    //Captura errores al momento de consultar la ubicación del logotipo.
                                    try
                                    {
                                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                                        firma_img = System.IO.File.ReadAllBytes(dr["URL"].ToString());
                                    }
                                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                                    catch { firma_img = null; }
                                }
                            }
                        }
                    }

                    //Añadiendo Fila
                    dtFirmaU.Rows.Add(firma_img);

                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("FirmaUsuario", dtFirmaU);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                //Encabezado de la orden de Compra
                using (DataTable encabezado = SAT_CL.Almacen.OrdenCompra.EncabezadoOrdenCompra(idOrdenCompra))
                {
                    //Valida que el datatable encabezado contenga valores vaidos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(encabezado))
                    {
                        //Recorre las filas del datatable y el resultado los almacena en los parametros del reporte
                        foreach (DataRow r in encabezado.Rows)
                        {
                            //Obteniendo Estatus
                            string tit = "ORDEN DE COMPRA";

                            //Validando Configuración por Compania
                            switch (orden.id_compania_emisor)
                            {
                                case 72:
                                    {
                                        //Si la Orden este Cerrada
                                        tit = orden.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada ? "ORDEN DE COMPRA" : "REQUISICIÓN DE COMPRA";
                                        break;
                                    }
                                case 76:
                                    {
                                        //Si la Orden este Cerrada
                                        tit = orden.estatus == SAT_CL.Almacen.OrdenCompra.Estatus.Cerrada ? "ORDEN DE COMPRA" : "REQUISICIÓN DE COMPRA";
                                        break;
                                    }
                            }

                            //Asignando Valores a los Atributos
                            ReportParameter titulo = new ReportParameter("TituloOrdenCompra", tit);
                            ReportParameter noCompra = new ReportParameter("NoCompra", r["NoCompra"].ToString());
                            ReportParameter documentoProveedor = new ReportParameter("DocumentoProveedor", r["DocumentoProveedor"].ToString());
                            ReportParameter proveedor = new ReportParameter("Proveedor", r["Proveedor"].ToString());
                            ReportParameter rfcProveedor = new ReportParameter("RFCProveedor", r["RFC"].ToString());
                            ReportParameter direccionProveedor = new ReportParameter("DireccionProveedor", r["Direccion"].ToString().ToUpper());
                            ReportParameter almacen = new ReportParameter("Almacen", r["Almacen"].ToString());
                            ReportParameter estatus = new ReportParameter("Estatus", r["Estatus"].ToString());
                            ReportParameter formaEntrega = new ReportParameter("FormaEntrega", r["FormaEntrega"].ToString());
                            ReportParameter fechaSolicitud = new ReportParameter("FechaSolicitud", r["FechaSolicitud"].ToString());
                            ReportParameter fechaCompromiso = new ReportParameter("FechaCompromiso", r["FechaCompromiso"].ToString());
                            ReportParameter condicionesPago = new ReportParameter("CondicionesPago", r["CondicionesPago"].ToString());
                            ReportParameter total = new ReportParameter("Total", r["Total"].ToString());
                            ReportParameter subTotal = new ReportParameter("SubTotal", r["SubTotal"].ToString());
                            ReportParameter ivaTrasladado = new ReportParameter("IvaTrasladado", r["IvaTrasladado"].ToString());
                            ReportParameter ivaRetenido = new ReportParameter("IvaRetenido", r["IvaRetenido"].ToString());
                            ReportParameter importeLetra = new ReportParameter("ImporteLetra", TSDK.Base.Cadena.ConvierteMontoALetra(r["Total"].ToString()));
                            ReportParameter direccionAlmacen = new ReportParameter("DireccionAlmacen", r["DireccionALmacen"].ToString());
                            ReportParameter telefonoProveedor = new ReportParameter("TelefonoProveedor", r["TelefonoProv"].ToString());
                            //Agrega los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { titulo, noCompra, documentoProveedor, proveedor, rfcProveedor, direccionProveedor, almacen, estatus, formaEntrega, fechaSolicitud,
                                                                                        fechaCompromiso, condicionesPago, total, subTotal, ivaTrasladado, ivaRetenido, importeLetra,direccionAlmacen, telefonoProveedor });
                        }
                    }
                }
                //Carga las actividades y fallas de la orden de trabajo
                using (DataTable dtOrdenCompra = SAT_CL.Almacen.OrdenCompraDetalle.OrdenCompraImpresionDetalles(idOrdenCompra))
                {
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtOrdenCompra))
                    {
                        ReportDataSource rsOrdenCompra = new ReportDataSource("DetalleOrdenCompra", dtOrdenCompra);
                        rvReporte.LocalReport.DataSources.Add(rsOrdenCompra);
                    }
                    else
                    {
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("Estatus", typeof(string));
                        dtTem.Columns.Add("Codigo", typeof(string));
                        dtTem.Columns.Add("Producto", typeof(string));
                        dtTem.Columns.Add("Categoria", typeof(string));
                        dtTem.Columns.Add("CantidadInicial", typeof(string));
                        dtTem.Columns.Add("UnidadMedida", typeof(string));
                        dtTem.Columns.Add("PrecioUnitario", typeof(string));
                        dtTem.Columns.Add("Total", typeof(int));
                        dtTem.Rows.Add("", "", "", "", "", "", "", 0);
                        ReportDataSource rsOrdenCompra = new ReportDataSource("DetalleOrdenCompra", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsOrdenCompra);
                    }

                }

            }
        }
        /// <summary>
        /// Método que inicializa los calores de los parametros del reporte  Acuse ABC
        /// </summary>
        private void inicializaReporteAcuseABC()
        {
            //Variable que almacena el identificador de una orden de compra
            int idAcuse = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte orden de compra
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/AcuseABC.rdlc");
            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();
            //Variables para fecha
            DateTime fechaActual = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            //Invoca a la clase Orden de Compra y obtiene los datos principales de la Orden de compra
            using (SAT_CL.Facturacion.PaqueteProceso paquete = new SAT_CL.Facturacion.PaqueteProceso(idAcuse))
            {
                using (SAT_CL.Global.CompaniaEmisorReceptor cl = new SAT_CL.Global.CompaniaEmisorReceptor(paquete.id_cliente))
                {
                    ReportParameter cliente = new ReportParameter("Cliente", cl.nombre);
                    ReportParameter noAcuse = new ReportParameter("NoAcuse", paquete.consecutivo_compania.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cliente, noAcuse });
                }
                using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(paquete.id_usuario_responsable))
                {
                    ReportParameter responsable = new ReportParameter("Responsable", us.nombre);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { responsable });
                }

                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(paquete.id_compania))
                {
                    //Recorre las filas de la tabla
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        ReportParameter email = new ReportParameter("Email", r["email"].ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion, email });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(paquete.id_compania))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                using (DataTable dtAcuseABC = SAT_CL.Facturacion.PaqueteProceso.ReporteABC(paquete.id_paquete_proceso))
                {
                    //Valida los datos de la tabla
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAcuseABC))
                    {
                        ReportDataSource rsAcuseABC = new ReportDataSource("AcuseAbc", dtAcuseABC);
                        rvReporte.LocalReport.DataSources.Add(rsAcuseABC);
                    }
                    else
                    {
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("NoViaje", typeof(string));
                        dtTem.Columns.Add("OrdenCompra", typeof(string));
                        dtTem.Columns.Add("NoFactura", typeof(string));
                        dtTem.Columns.Add("Destino", typeof(string));
                        dtTem.Rows.Add("", "", "", "");
                        ReportDataSource rsAcuseAbc = new ReportDataSource("AcuseAbc", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsAcuseAbc);
                    }

                }
            }
        }
        /// <summary>
        /// Método que inicializa los calores de los parametros del reporte Acuse Lily
        /// </summary>
        private void inicializaReporteAcuseLili()
        {
            //Variable que almacena el identificador de una orden de compra
            int idAcuse = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte orden de compra
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/AcuseLili.rdlc");
            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            rvReporte.LocalReport.DataSources.Clear();
            //Variables para fecha
            DateTime fechaActual = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            //Invoca a la clase Orden de Compra y obtiene los datos principales de la Orden de compra
            using (SAT_CL.Facturacion.PaqueteProceso paquete = new SAT_CL.Facturacion.PaqueteProceso(idAcuse))
            {
                using (SAT_CL.Global.CompaniaEmisorReceptor cl = new SAT_CL.Global.CompaniaEmisorReceptor(paquete.id_cliente))
                {
                    ReportParameter cliente = new ReportParameter("Cliente", cl.nombre);
                    ReportParameter noAcuse = new ReportParameter("NoAcuse", paquete.consecutivo_compania.ToString());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { cliente, noAcuse });
                }
                using (SAT_CL.Seguridad.Usuario us = new SAT_CL.Seguridad.Usuario(paquete.id_usuario_responsable))
                {
                    ReportParameter responsable = new ReportParameter("Responsable", us.nombre);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { responsable });
                }

                //Invoca a la clase Compania Emisor Recepor y obtiene los datos de la empresa  (Encabezado impresión)                              
                using (DataTable encabezado = SAT_CL.Global.CompaniaEmisorReceptor.EncabezadoImpresión(paquete.id_compania))
                {
                    //Recorre las filas de la tabla
                    foreach (DataRow r in encabezado.Rows)
                    {
                        ReportParameter compania = new ReportParameter("Compania", r["Compania"].ToString());
                        ReportParameter rfc = new ReportParameter("RFC", r["RFC"].ToString());
                        ReportParameter telefono = new ReportParameter("Telefono", r["Telefono"].ToString());
                        ReportParameter direccion = new ReportParameter("Direccion", r["Direccion"].ToString().ToUpper());
                        ReportParameter email = new ReportParameter("Email", r["email"].ToString());
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, rfc, telefono, direccion, email });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(paquete.id_compania))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaImpresion = new ReportParameter("FechaImpresion", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString().ToUpper());
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaImpresion });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);
                }

                using (DataTable dtAcuseLili = SAT_CL.Facturacion.PaqueteProceso.Reportelili(paquete.id_paquete_proceso))
                {
                    //Valida los datos de la tabla
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAcuseLili))
                    {
                        ReportDataSource rsAcuselili = new ReportDataSource("AcuseLili", dtAcuseLili);
                        rvReporte.LocalReport.DataSources.Add(rsAcuselili);
                    }
                    else
                    {
                        DataTable dtTem = new DataTable();
                        dtTem.Columns.Add("FE", typeof(string));
                        dtTem.Columns.Add("FechaTermino", typeof(string));
                        dtTem.Columns.Add("Cliente", typeof(string));
                        dtTem.Columns.Add("Origen", typeof(string));
                        dtTem.Columns.Add("Destino", typeof(string));
                        dtTem.Rows.Add("", "", "", "", "");
                        ReportDataSource rsAcuselili = new ReportDataSource("AcuseLili", dtTem);
                        rvReporte.LocalReport.DataSources.Add(rsAcuselili);
                    }

                }
            }

        }
        /// <summary>
        /// Método que inicializa los calores de los parametros del reporte Etiquetas
        /// </summary>
        private void inicializaReporteEtiquetas()
        {
            //Variable que almacena el identificador de una orden de compra
            int idOrdenCompra = Convert.ToInt32(Request.QueryString["idRegistro"]);
            //Declara la ubicación local del reporte orden de compra EtiquetasEmbarque
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/EtiquetaEmbarque.rdlc");
            //Habilita la consulta de imagenes externas al reporte
            rvReporte.LocalReport.EnableExternalImages = true;
            //Creación de la variable tipo tabla dtLogo.
            DataTable dtLogo = new DataTable();
            //Agrega una columna a la tabla 
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            rvReporte.LocalReport.DataSources.Clear();
            using (SAT_CL.Almacen.OrdenCompra oc = new SAT_CL.Almacen.OrdenCompra(idOrdenCompra))
            {
                //Invoca a la clase Compania para obtener el nombre del proveedor
                using (SAT_CL.Global.CompaniaEmisorReceptor prove = new SAT_CL.Global.CompaniaEmisorReceptor(oc.id_proveedor))
                {
                    ReportParameter proveedor = new ReportParameter("Proveedor", prove.nombre);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { proveedor });
                    //Invoca a la clase almacen para obtener el nombre del almacen 
                    using (SAT_CL.Almacen.Almacen al = new SAT_CL.Almacen.Almacen(oc.id_almacen))
                    {
                        ReportParameter almacen = new ReportParameter("Almacen", al.descripcion);
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { almacen });
                    }
                }
                //Invoca a la clase compañia y obtiene la ruta del logotipo de la empresa
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(oc.id_compania_emisor))
                {
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    ReportParameter fechaEntrega = new ReportParameter("FechaEntrega", oc.fecha_entrega.ToString());
                    ReportParameter razonSocial = new ReportParameter("RazonSocial", emisor.nombre);
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { color, fechaEntrega, razonSocial });
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                    //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                    ReportDataSource rvs = new ReportDataSource("LogotipoCompania", dtLogo);
                    //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                    rvReporte.LocalReport.DataSources.Add(rvs);

                    //Obtiene los datos de la etiqueta
                    using (DataTable dtEtiqueta = SAT_CL.Almacen.OrdenCompraDetalle.ObtieneDetalleEtiquetasEmbarque(idOrdenCompra))
                    {
                        //Valida los datos de la tabla
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEtiqueta))
                        {
                            //Declarando Tabla Final
                            using (DataTable dtLabel = new DataTable())
                            {
                                //Definiendo COmlunmas
                                dtLabel.Columns.Add("Id", typeof(string));
                                dtLabel.Columns.Add("Codigo", typeof(string));
                                dtLabel.Columns.Add("Producto", typeof(string));
                                dtLabel.Columns.Add("Caducidad", typeof(string));
                                dtLabel.Columns.Add("Lote", typeof(string));
                                dtLabel.Columns.Add("CodigoEtiqueta", typeof(byte[]));
                                //Declarando variable tipo Bitmap
                                Bitmap bitmap;
                                //Recorre las filas del datatable etiqueta
                                foreach (DataRow dr in dtEtiqueta.Rows)
                                {
                                    //Obteniendo Valor a Convertir 
                                    string idDetOC = dr["CodigoEtiqueta"].ToString();
                                    //Inicializando el tamaño de la imagen acorde al tamaño del Codigo Etiqueta
                                    bitmap = new Bitmap(idDetOC.Length * 29, 25);
                                    //Asigna a la clase graphic la variable bitmap
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        //Aplica formato y tamaño de la fuente 
                                        Font oFont = new System.Drawing.Font("3 of 9 Barcode", 35);
                                        PointF point = new PointF(2f, 2f);
                                        //Crea acorde a la variable bitmap la imagen en fuente 3 of 9 Barcode
                                        g.FillRectangle(new SolidBrush(Color.White), 0, 0, bitmap.Width, bitmap.Height);
                                        g.DrawString(idDetOC, oFont, new SolidBrush(Color.Black), point);
                                    }
                                    //Creación del arreglo de byte que permitira leer la imagen
                                    byte[] barcode = null;
                                    //Invoca a la clase MemoryStream 
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        //Asigna un formatos de imagen a bitmap
                                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                        //Asigna a areglo de byte el valor de la MemoryStream
                                        barcode = ms.ToArray();
                                    }
                                    //Añadiendo Columna a Tabla
                                    dtLabel.Rows.Add(dr["Id"].ToString(),
                                                    dr["Codigo"].ToString(),
                                                    dr["Producto"].ToString(),
                                                    dr["Caducidad"].ToString(),
                                                    dr["Lote"].ToString(),
                                                    barcode);

                                }

                                //Asignando Reportes
                                ReportDataSource rsEtiqueta = new ReportDataSource("Etiqueta", dtLabel);
                                rvReporte.LocalReport.DataSources.Add(rsEtiqueta);
                            }
                        }
                        //En caso contrario carga la etiqueta con valores vacios
                        else
                        {
                            DataTable dtTem = new DataTable();
                            dtTem.Columns.Add("Id", typeof(string));
                            dtTem.Columns.Add("Codigo", typeof(string));
                            dtTem.Columns.Add("Producto", typeof(string));
                            dtTem.Columns.Add("Caducidad", typeof(DateTime));
                            dtTem.Columns.Add("Lote", typeof(string));
                            dtTem.Columns.Add("CodigoEtiqueta", typeof(byte[]));
                            dtTem.Rows.Add("", "", "", "", "", null);
                            ReportDataSource rsEtiqueta = new ReportDataSource("Etiqueta", dtTem);
                            rvReporte.LocalReport.DataSources.Add(rsEtiqueta);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método encargado de Imprimir los QR de Unidad
        /// </summary>
        private void inicializaQRUnidad()
        {
            //Obtiene Unidad
            string idUnidad = Request.QueryString["IdUnidad"].ToString();
            //Obteniendo Información Para Generar los codigos
            using (SAT_CL.ControlPatio.UnidadPatio Uni = new SAT_CL.ControlPatio.UnidadPatio(Convert.ToInt32(idUnidad)))
            {
                //Validando que exista la referencia del usuario
                if (Uni.habilitar)
                {
                    //Declarando Tabla Final
                    using (DataTable dtProd = new DataTable())
                    {

                        dtProd.Columns.Add("Sistema", typeof(string));
                        dtProd.Columns.Add("Descripcion", typeof(string));
                        dtProd.Columns.Add("Producto", typeof(string));
                        dtProd.Columns.Add("CodigoBarras", typeof(byte[]));
                        //Generamos Codigo Bidimensional
                        string cadena_codigo = string.Format("Unidad:{0} Placas:{1} ID:{2}", Uni.no_economico, Uni.placas, Uni.id_unidad_patio);
                        byte[] ArchivoBytes = Dibujo.GeneraCodigoBidimensional(cadena_codigo, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //Añadiendo Columna a Tabla
                        dtProd.Rows.Add("FEDEX", Uni.no_economico, Uni.id_unidad_patio, ArchivoBytes);
                        //Asignando Reportes
                        ReportDataSource rsEtiqueta = new ReportDataSource("Producto", dtProd);
                        rvReporte.LocalReport.DataSources.Add(rsEtiqueta);

                        //Declara la ubicación local del reporte orden de compra EtiquetasEmbarque
                        rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/QRUnidad.rdlc");
                        //Generando flujo del reporte 
                    }
                }
            }
        }
        /// <summary>
        /// Metodo que inicializa en forma especifica el reporte CartaPorte-Traslado Regularizacion 16/12/2015
        /// </summary>
        private void inicializaReporteCartaPorteViajera()
        {
            //Creación de la tabla para cargar el logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            DataTable dtDestino = new DataTable();
            dtDestino.Columns.Add("Destino", typeof(string));
            dtDestino.Columns.Add("Secuencia", typeof(string));
            dtDestino.Columns.Add("Destinatario", typeof(string));
            dtDestino.Columns.Add("Docimilio", typeof(string));
            dtDestino.Columns.Add("SeEntregara", typeof(string));
            dtDestino.Columns.Add("DetalleProducto", typeof(string));
            dtDestino.Columns.Add("CitaParada", typeof(DateTime));
            dtDestino.Columns.Add("Precinto", typeof(string));
            dtDestino.Columns.Add("CodigoBarras", typeof(byte[]));
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obteniendo Servicio
            int idServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idOperador, idUnidadM, idUnidadA1, idUnidadA2;
            idOperador = idUnidadM = idUnidadA1 = idUnidadA2 = 0;
            string no_porte = "";

            //Obteniendo Valores de la Petición
            int.TryParse(Request.QueryString["idRegistroB"], out idOperador);
            int.TryParse(Request.QueryString["idRegistroC"], out idUnidadM);
            int.TryParse(Request.QueryString["idRegistroD"], out idUnidadA1);
            int.TryParse(Request.QueryString["idRegistroE"], out idUnidadA2);

            //Invoca la clase Servicio y obtienen los datos de consulta.
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                //Obtiene referencia de  cliente para impresion especifica de carta porte
                bool incluirQR = Convert.ToBoolean(TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_cliente_receptor, "Configuración Formatos de Impresión", "Bit QR Fijo Carta Porte"), "True"));
                //Asignamos la ruta del reporte local
                rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/CartaPorteViajera.rdlc");
                //Invoca a la clase compañiaEmisorReceptor 
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                {
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                }
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_compania_emisor, "Color Empresa", "Color"));
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { color });

                //Obtenemos la informacion general del servicio
                using (DataSet t = SAT_CL.Documentacion.Servicio.CargaDatosPorteViajera(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2),
                    tdf = SAT_CL.Documentacion.Servicio.CargaDatosPorteViajera(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2))
                {
                    //Validamos que se hayan retornado valores validos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(t) && TSDK.Datos.Validacion.ValidaOrigenDatos(tdf))
                    {
                        //Recuperamos  los valores y creamos los parametros
                        foreach (DataRow r in t.Tables["Table"].Rows)
                        {
                            ReportParameter nombreCompania = new ReportParameter("NombreCompania", r["NombreCompania"].ToString());
                            ReportParameter rfcCompania = new ReportParameter("RFCCompania", r["RFCCompania"].ToString());
                            ReportParameter direccionCompania = new ReportParameter("DireccionCompania", r["DireccionCompania"].ToString().ToUpper());
                            ReportParameter telefonoCompania = new ReportParameter("TelefonoCompania", r["TelefonoCompania"].ToString());
                            ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                            ReportParameter porte = new ReportParameter("Porte", r["Porte"].ToString());
                            ReportParameter nombreCliente = new ReportParameter("NombreCliente", r["NombreCliente"].ToString());
                            ReportParameter rfcCliente = new ReportParameter("RFCCliente", r["RFCCliente"].ToString());
                            ReportParameter direccionCliente = new ReportParameter("DireccionCliente", r["DireccionCliente"].ToString());
                            ReportParameter telefonoCliente = new ReportParameter("TelefonoCliente", r["TelefonoCliente"].ToString());
                            ReportParameter fechageneral = new ReportParameter("FechaGeneral", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString().ToUpper());

                            ////Origen
                            ReportParameter origen = new ReportParameter("Origen", r["Origen"].ToString());
                            ReportParameter domicilioRemitente = new ReportParameter("DomicilioRemitente", r["DomicilioRemitente"].ToString());
                            ReportParameter nombreRemitente = new ReportParameter("NombreRemitente", r["NombreRemitente"].ToString());
                            ReportParameter seRecogera = new ReportParameter("SeRecogera", r["SeRecogera"].ToString());
                            ReportParameter citaRemitente = new ReportParameter("CitaRemitente", r["CitaRemitente"].ToString());
                            ////Observacion
                            ReportParameter referencias = new ReportParameter("Referencias", r["Referencias"].ToString());
                            ReportParameter precinto = new ReportParameter("Precinto", r["Precinto"].ToString());
                            ReportParameter serie = new ReportParameter("Serie", r["Serie"].ToString());
                            ReportParameter observacion = new ReportParameter("Observacion", r["Observacion"].ToString());
                            ReportParameter operador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                            ReportParameter telefonooperador = new ReportParameter("TelefonoOperador", r["TelefonoOperador"].ToString());
                            ReportParameter nounidad = new ReportParameter("NoUnidad", r["NoUnidad"].ToString());
                            ReportParameter placas = new ReportParameter("Placas", r["Placas"].ToString());
                            ReportParameter noCaja1 = new ReportParameter("NoCaja1", r["NoCaja1"].ToString());
                            ReportParameter noCaja2 = new ReportParameter("NoCaja2", r["NoCaja2"].ToString());
                            ReportParameter plcCaja1 = new ReportParameter("PlcCaja1", r["PlcCaja1"].ToString());
                            ReportParameter plcCaja2 = new ReportParameter("PlcCaja2", r["PlcCaja2"].ToString());

                            //Agregamos los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreCompania,rfcCompania, direccionCompania, telefonoCompania, servicio, porte,
                            nombreCliente, rfcCliente, direccionCliente, telefonoCliente, fechageneral, origen, domicilioRemitente, nombreRemitente, seRecogera,
                            citaRemitente
                            ,referencias, precinto, serie, observacion, operador, telefonooperador, nounidad, noCaja1, noCaja2, plcCaja1, plcCaja2, operador, placas
                          });
                            no_porte = r["Porte"].ToString();
                            //Creación del arreglo de byte que permitira leer la imagen
                            byte[] barcode = null;
                            //Obteniendo Valor a Convertir 
                            string Serie = r["Serie"].ToString().Trim();
                            if (Serie != "")
                            {
                                //Declarando variable tipo Bitmap
                                //Bitmap bitmap;
                                ////Inicializando el tamaño de la imagen acorde al tamaño de la Descripcion
                                //bitmap = new Bitmap(Serie.Length * 65, 95);
                                ////Asigna a la clase graphic la variable bitmap
                                //using (Graphics g = Graphics.FromImage(bitmap))
                                //{
                                //    //Obteniendo SKU 
                                //    string serie_cod128 = TSDK.Base.BarcodeConverter.StringToBarcode(Serie);

                                //    //Aplica formato y tamaño de la fuente 
                                //    String font = "Code 128";
                                //    Font barCodeF = new Font(font, serie_cod128.Length > 25 ? 85 : (serie_cod128.Length > 15 ? 95 : (serie_cod128.Length < 11 ? 100 : 105)));

                                //    PointF point = new PointF(0, 0);
                                //    //Crea acorde a la variable bitmap la imagen en fuente Cod 128
                                //    //g.FillRectangle(new SolidBrush(Color.White), 0, 0, bitmap.Width, bitmap.Height);
                                //    g.DrawString(serie_cod128, barCodeF, new SolidBrush(Color.Black), point);
                                //}
                                ////Invoca a la clase MemoryStream 
                                //using (MemoryStream ms = new MemoryStream())
                                //{
                                //    //Asigna un formatos de imagen a bitmap
                                //    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                //    //Asigna a areglo de byte el valor de la MemoryStream
                                //    barcode = ms.ToArray();
                                //}
                                //Declarando variable tipo Bitmap
                                using (BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode())
                                using (MemoryStream ms = new MemoryStream())
                                //using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    Codigo.IncludeLabel = true;
                                    Codigo.StandardizeLabel = true;
                                    Codigo.StandardizeLabel = true;
                                    System.Drawing.Image imgBarras = Codigo.Encode(BarcodeLib.TYPE.CODE128, Serie, Color.Black, Color.White, 400, 100);
                                    imgBarras.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //Asigna a areglo de byte el valor de la MemoryStream
                                    barcode = ms.ToArray();
                                }      
                            }
                            //Recuperamos  los valores y creamos los parametros
                            foreach (DataRow rd in t.Tables["Table1"].Rows)
                            {
                                //Añadiendo Columna a Tabla
                                dtDestino.Rows.Add(rd["Destino"].ToString(), rd["Secuencia"].ToString(), rd["Destinatario"].ToString(), rd["Docimilio"].ToString(), rd["SeEntregara"].ToString(), rd["DetalleProducto"].ToString(),Convert.ToDateTime(rd["CitaParada"]), rd["Precinto"].ToString(), barcode);
                            }
                        }

                        //Agregamos el origen de datos 
                        rvReporte.LocalReport.DataSources.Clear();
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
                        ReportDataSource rsDetalle = new ReportDataSource("CartaPorteViajera", dtDestino);
                        rvReporte.LocalReport.DataSources.Add(rsDetalle);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvLogotipo);
                        //Generando flujo del reporte 
                        byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
                        //Descargando Archivo PDF
                        //TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}.pdf", "CartaPorteViajera", no_porte), TSDK.Base.Archivo.ContentType.application_PDF);

                    }
                }
            }
        }

        /// <summary>
        /// Metodo que inicializa en forma especifica el reporte CartaPorte-Traslado Regularizacion 16/12/2015
        /// </summary>
        private void inicializaReporteHojaDeInstruccion()
        {
            //Creación de la tabla para cargar el logotipo de la compañia
            DataTable dtLogo = new DataTable();
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita las imagenes externas
            rvReporte.LocalReport.EnableExternalImages = true;
            //Obteniendo Servicio
            int idServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);
            int idOperador, idUnidadM, idUnidadA1, idUnidadA2;
            idOperador = idUnidadM = idUnidadA1 = idUnidadA2 = 0;
            //Variable
            string no_porte = "";
            //Obteniendo Valores de la Petición
            int.TryParse(Request.QueryString["idRegistroB"], out idOperador);
            int.TryParse(Request.QueryString["idRegistroC"], out idUnidadM);
            int.TryParse(Request.QueryString["idRegistroD"], out idUnidadA1);
            int.TryParse(Request.QueryString["idRegistroE"], out idUnidadA2);

            //Invoca la clase Servicio y obtienen los datos de consulta.
            using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(idServicio))
            {
                //Obtiene referencia de  cliente para impresion especifica de carta porte
                bool incluirQR = Convert.ToBoolean(TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_cliente_receptor, "Configuración Formatos de Impresión", "Bit QR Fijo Carta Porte"), "True"));
                //Asignamos la ruta del reporte local
                rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/HojaDeInstruccion.rdlc");
                //Invoca a la clase compañiaEmisorReceptor 
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(objServicio.id_compania_emisor))
                {
                    //Creació del arreglo necesario para la carga de la ruta del logotipo y del 
                    byte[] logotipo = null;
                    //Permite capturar errores en caso de que no exista una ruta para el archivo
                    try
                    {
                        //Asigna al arreglo el valor de la variable que tiene la ruta de la imagen
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista una imagen, se devolvera un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla un registro con valor a la ruta de la imagen.
                    dtLogo.Rows.Add(logotipo);
                }
                ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, objServicio.id_compania_emisor, "Color Empresa", "Color"));
                //Asigna valores a los parametros del reporteComprobante                      
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { color });

                //Obtenemos la informacion general del servicio
                using (DataSet t = SAT_CL.Documentacion.Servicio.CargaDatosHojaInstruccion(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2),
                    tdf = SAT_CL.Documentacion.Servicio.CargaDatosHojaInstruccion(idServicio, idOperador, idUnidadM, idUnidadA1, idUnidadA2))
                {
                    //Validamos que se hayan retornado valores validos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(t) && TSDK.Datos.Validacion.ValidaOrigenDatos(tdf))
                    {
                        //Recuperamos  los valores y creamos los parametros
                        foreach (DataRow r in t.Tables["Table"].Rows)
                        {
                            ReportParameter nombreCompania = new ReportParameter("NombreCompania", r["NombreCompania"].ToString());
                            ReportParameter rfcCompania = new ReportParameter("RFCCompania", r["RFCCompania"].ToString());
                            ReportParameter direccionCompania = new ReportParameter("DireccionCompania", r["DireccionCompania"].ToString().ToUpper());
                            ReportParameter telefonoCompania = new ReportParameter("TelefonoCompania", r["TelefonoCompania"].ToString());
                            ReportParameter servicio = new ReportParameter("Servicio", r["Servicio"].ToString());
                            ReportParameter porte = new ReportParameter("Porte", r["Porte"].ToString());
                            ReportParameter nombreCliente = new ReportParameter("NombreCliente", r["NombreCliente"].ToString());
                            ReportParameter citaremitente = new ReportParameter("CitaRemitente", r["CitaRemitente"].ToString());
                            ReportParameter fechageneral = new ReportParameter("FechaGeneral", TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString().ToUpper());
                            ////Operador
                            ReportParameter operador = new ReportParameter("NombreOperador", r["NombreOperador"].ToString());
                            ReportParameter telefonooperador = new ReportParameter("TelefonoOperador", r["TelefonoOperador"].ToString());
                            ReportParameter nounidad = new ReportParameter("NoUnidad", r["NoUnidad"].ToString());
                            ReportParameter placas = new ReportParameter("Placas", r["Placas"].ToString());
                            ReportParameter noCaja1 = new ReportParameter("NoCaja1", r["NoCaja1"].ToString());
                            ReportParameter noCaja2 = new ReportParameter("NoCaja2", r["NoCaja2"].ToString());
                            ReportParameter plcCaja1 = new ReportParameter("PlcCaja1", r["PlcCaja1"].ToString());
                            ReportParameter plcCaja2 = new ReportParameter("PlcCaja2", r["PlcCaja2"].ToString());

                            //Agregamos los parametros al reporte
                            rvReporte.LocalReport.SetParameters(new ReportParameter[] { nombreCompania,rfcCompania, direccionCompania, telefonoCompania, servicio, porte,
                            nombreCliente,citaremitente, fechageneral, operador, telefonooperador, nounidad, noCaja1, noCaja2, plcCaja1, plcCaja2, operador, placas
                          });
                          //Inicializando noporte
                          no_porte = r["Porte"].ToString();
                        }
                        //Agregamos el origen de datos 
                        rvReporte.LocalReport.DataSources.Clear();
                        //Asigna al Conjunto de datos los valores de la tabla que contiene la ruta de la imagen, 
                        ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
                        ReportDataSource rsDetalle = new ReportDataSource("HojaDeInstruccion", tdf.Tables["Table1"]);
                        rvReporte.LocalReport.DataSources.Add(rsDetalle);
                        //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
                        rvReporte.LocalReport.DataSources.Add(rvLogotipo);
                        //Generando flujo del reporte 
                        byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
                        //Descargando Archivo PDF
                        //TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("{0}_{1}.pdf", "HojaDeInstruccion", no_porte), TSDK.Base.Archivo.ContentType.application_PDF);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo que inicializa en forma específica el reporte de envío de paquetes 18/02/2020
        /// </summary>
        private void inicializaReporteEnvioPaquete()
        {
            //Obteniendo el paquete a enviar
            int idPaquete = Convert.ToInt32(Request.QueryString["idRegistro"]);
            
            //Obtenemos la información del contenido del paquete solicitado
            using (DataTable dtContenidoPaquete = SAT_CL.ControlEvidencia.Reportes.ObtieneServiciosPaqueteEnvio(idPaquete, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validamos que hayan retornado valores válidos
                if(Validacion.ValidaOrigenDatos(dtContenidoPaquete))
                {
                    //Declarando tabla
                    using(DataTable dttContenidoPaquete = new DataTable())
                    {
                        dttContenidoPaquete.Columns.Add("FechaDeViaje", typeof(string));
                        dttContenidoPaquete.Columns.Add("OrigenCliente", typeof(string));
                        dttContenidoPaquete.Columns.Add("PlantaDestino", typeof(string));
                        dttContenidoPaquete.Columns.Add("CP", typeof(string));
                        dttContenidoPaquete.Columns.Add("FolioDeEquipoVacio", typeof(string));
                        dttContenidoPaquete.Columns.Add("Operador", typeof(string));
                        dttContenidoPaquete.Columns.Add("EcoUnidad", typeof(string));
                        dttContenidoPaquete.Columns.Add("SellosCompletos", typeof(string));
                        dttContenidoPaquete.Columns.Add("FacturasCompletas", typeof(string));
                        dttContenidoPaquete.Columns.Add("Observaciones", typeof(string));

                        //dttContenidoPaquete.Rows.Add(dtContenidoPaquete);

                        //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                        foreach (DataRow r in dtContenidoPaquete.Rows)
                        {
                            dttContenidoPaquete.Rows.Add(r.ItemArray);
                        }

                        ReportDataSource rdsEnvioPaquete = new ReportDataSource("EnvioPaquete", dttContenidoPaquete);
                        rvReporte.LocalReport.DataSources.Add(rdsEnvioPaquete);

                        //Cargando reporte
                        rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/EnvioPaquete.rdlc");
                    }
                }
            }
        }
        /// <summary>
        /// Método que permite inicializar los valores del reporte AcuseREciboFacturas
        /// </summary>
        private void inicializaReporteAcuseReciboFacturas2()
        {
            //Creación de la variable que almacena el identificador de registro de una recepcion de factura proveedor
            int idAcuseRecibo = Convert.ToInt32(Request.QueryString["idRegistro"]);
            string proveedor, rfcp, folioCR = "";
            int dias_credito;
            //Ubicación local del reporte
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/AcuseReciboFacturas2.rdlc");
            //Creación de la variable tipo tabla que almacenara el logotipo de la empresa.
            DataTable dtLogo = new DataTable();
            //Agrega a la columna de la tabla el parametro Logotipo.
            dtLogo.Columns.Add("Logotipo", typeof(byte[]));
            //Habilita la consulta de imagenes externas al reporte.
            rvReporte.LocalReport.EnableExternalImages = true;
            //Invoca a la clase recepcion y obtiene los datos de la recepción.
            using (SAT_CL.CXP.Recepcion rec = new SAT_CL.CXP.Recepcion(idAcuseRecibo))
            {
                //Creación de variables que almacenan los datos consultados de un registro
                ReportParameter fecha = new ReportParameter("Fecha", rec.fecha_recepcion.ToString());
                ReportParameter entregado = new ReportParameter("Entregado", rec.entregado_por);
                ReportParameter foliocontrarecibo = new ReportParameter("FolioContraRecibo", Convert.ToString(rec.secuencia));
                folioCR = Convert.ToString(rec.secuencia);
                //Asigna al reporte (RDLC) las variables creadas.
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { fecha, entregado, foliocontrarecibo });
                //Invoca a la clase compañia para obtener los datos de la empresa emisor
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_receptor))
                {
                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter compania = new ReportParameter("Compania", emisor.nombre);
                    ReportParameter color = new ReportParameter("Color", SAT_CL.Global.Referencia.CargaReferencia("0", 25, emisor.id_compania_emisor_receptor, "Color Empresa", "Color"));
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { compania, color });
                    //Invoca a la clase direccion y obtiene la dirección de la compañia.
                    using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(emisor.id_direccion))
                    {
                        //Creación de variables que almacenan los datos consultados de un registro
                        ReportParameter direccion = new ReportParameter("Direccion", dir.ObtieneDireccionCompleta());
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { direccion });
                    }
                    //Creaciondel arreglo que almacenara la ruta de ubicacion del logotipo.
                    byte[] logotipo = null;
                    //Captura errores al momento de consultar la ubicación del logotipo.
                    try
                    {
                        //Si existe la ubicacion del archivo, asigna al arreglo la ruta del archivo.
                        logotipo = System.IO.File.ReadAllBytes(emisor.ruta_logotipo);
                    }
                    //En caso de que no exista ruta de archivo, devuelve un valor nulo.
                    catch { logotipo = null; }
                    //Agrega a la tabla el registro con la ruta del logotipo.
                    dtLogo.Rows.Add(logotipo);
                }
                //Invoca a la clase compania emisor receptor y obtiene el nombre del proveedor de facturas.
                using (SAT_CL.Global.CompaniaEmisorReceptor prov = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_proveedor))
                {
                    using (SAT_CL.Global.Direccion dirprov = new SAT_CL.Global.Direccion(prov.id_direccion))
                    {
                        //Creación de variables que almacenan los datos consultados de un registro
                        ReportParameter proveedorEntrega = new ReportParameter("ProveedorEntrega", prov.nombre);
                        proveedor = prov.nombre;
                        ReportParameter rfc = new ReportParameter("Rfc", prov.rfc);
                        rfcp = prov.rfc;
                        dias_credito = prov.dias_credito;
                        ReportParameter cp = new ReportParameter("CP", dirprov.codigo_postal);
                        ReportParameter domicilio = new ReportParameter("Domicilio", dirprov.ObtieneDireccionCompleta());
                        ReportParameter colonia = new ReportParameter("Colonia", dirprov.colonia);
                        //Asigna al reporte (RDLC) las variables creadas.
                        rvReporte.LocalReport.SetParameters(new ReportParameter[] { proveedorEntrega, rfc, domicilio });
                    }
                    //}
                    ////Invoca a la clase Factura Proveedor 
                    //using (SAT_CL.Global.CompaniaEmisorReceptor prov = new SAT_CL.Global.CompaniaEmisorReceptor(rec.id_compania_proveedor))
                    //{
                    //Valida que el provedor tenga días de credito
                    //if (prov.dias_credito != 0)
                    //{
                    //    //Si tieme dias de credito envia al parametros la leyenda de dias de credito proveedor.
                    //    ReportParameter diasPago = new ReportParameter("DiasPago", "PROGRAMACIÓN DE PAGO: " + rec.fecha_recepcion.AddDays(prov.dias_credito) + ".");
                    //    //Asigna al reporte (RDLC) las variables creadas.
                    //    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                    //}
                    ////En caso contrario
                    //else
                    //{
                    //    //No envia leyenda de días de crédito proveedor.
                    //    ReportParameter diasPago = new ReportParameter("DiasPago", "DÍAS DE CRÉDITO: 30 días.");
                    //    //Asigna al reporte (RDLC) las variables creadas.
                    //    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                    //}
                }
            }
            //Limpia los registros previos a la consulta de un datasources
            rvReporte.LocalReport.DataSources.Clear();
            //Carga la descripcion de la factura
            using (DataTable AcuseRecibo = SAT_CL.CXP.FacturadoProveedor.CargaAcuseReciboFactura(idAcuseRecibo))
            {
                string folios = "";
                DateTime fecha_factura = DateTime.MinValue;
                //Asigna valores a los parametros del reporte 
                ReportDataSource rsDescripcionAcuse = new ReportDataSource("DescripcionAcuseFactura", AcuseRecibo);
                //Limpiamos los origenes de datos previos.
                rvReporte.LocalReport.DataSources.Add(rsDescripcionAcuse);
                foreach (DataRow dr in AcuseRecibo.Select())
                {
                    folios = dr["Folio"].ToString() + " // " + folios ;
                    fecha_factura = Convert.ToDateTime(dr["FechaFactura"]);
                }
                string leyenda = folios;
                ReportParameter leyendas = new ReportParameter("Leyenda", leyenda);
                //Valida que el provedor tenga días de credito
                if (dias_credito != 0)
                {
                    //Si tieme dias de credito envia al parametros la leyenda de dias de credito proveedor.
                    ReportParameter diasPago = new ReportParameter("DiasPago", "PROGRAMACIÓN DE PAGO: " + fecha_factura.AddDays(dias_credito).ToString("dd/MM/yyyy") + ".");
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                }
                //En caso contrario
                else
                {
                    //No envia leyenda de días de crédito proveedor.
                    ReportParameter diasPago = new ReportParameter("DiasPago", "PROGRAMACIÓN DE PAGO: " + fecha_factura.ToString("dd/MM/yyyy"));
                    //Asigna al reporte (RDLC) las variables creadas.
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { diasPago });
                }
                //Asigna al reporte (RDLC) las variables creadas.
                rvReporte.LocalReport.SetParameters(new ReportParameter[] { leyendas });
            }            
            //Asigna valores a los parametros del reporte
            ReportDataSource rvLogotipo = new ReportDataSource("LogotipoCompania", dtLogo);
            //Asigna al reporte el datasource con los valores asignado al conjunto de datos.
            rvReporte.LocalReport.DataSources.Add(rvLogotipo);
        }

        /// <summary>
        /// Metodo que inicializa en forma específica el reporte de gastos generales de un viaje 11/03/2020
        /// </summary>
        private void inicializaReporteGastosGenerales()
        {
            //Asignando el ID de Servicio solicitado
            int IdServicio = Convert.ToInt32(Request.QueryString["idRegistro"]);
            
            //Obtenemos el registro completo del servicio proporcionado
            using (SAT_CL.Documentacion.Servicio Viaje = new SAT_CL.Documentacion.Servicio(IdServicio))
            {
                //Obtenemos los datos de la compañía emisora
                using (SAT_CL.Global.CompaniaEmisorReceptor Compania = new SAT_CL.Global.CompaniaEmisorReceptor(Viaje.id_compania_emisor))
                {
                    //Declaramos la ubicación reporte
                    rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/GastosGenerales.rdlc");
                    //Habilita la consulta de imagenes externas
                    rvReporte.LocalReport.EnableExternalImages = true;
                    //Limpia el reporte
                    rvReporte.LocalReport.DataSources.Clear();
                    //Declarando arreglo auxiliar
                    byte[] logo = null;
                    //Declarando la tabla para almacenar al logo
                    using (DataTable dtLogo = new DataTable())
                    {
                        //Añadiendo la única columna
                        dtLogo.Columns.Add("Logotipo", typeof(byte[]));
                        try { logo = System.IO.File.ReadAllBytes(Compania.ruta_logotipo); }
                        catch { logo = null; }

                        //Agregando imagen
                        dtLogo.Rows.Add(logo);

                        //Agregamos al origen de datos
                        ReportDataSource rdsLogo = new ReportDataSource("Logotipo", dtLogo);
                        rvReporte.LocalReport.DataSources.Add(rdsLogo);
                    }

                    //Creación de variables que almacenan los datos consultados de un registro
                    ReportParameter NoServicio = new ReportParameter("NoServicio", Viaje.no_servicio);
                    //Asignamos al reporte las variables creadas
                    rvReporte.LocalReport.SetParameters(new ReportParameter[] { NoServicio });
                    //Cargando todos los gastos generales del servicio especificado
                    using (DataSet DSGastosGenerales = SAT_CL.EgresoServicio.Reportes.CargaGastosGenerales(IdServicio))
                    {
                        //Valida que existan los registros del DataSet
                        if (Validacion.ValidaOrigenDatos(DSGastosGenerales))
                        {
                            //Separamos las tablas obtenidas del DataSet
                            DataTable MitCasetas = DSGastosGenerales.Tables[0];
                            DataTable MitConceptos = DSGastosGenerales.Tables[1];
                            DataTable MitDiesel = DSGastosGenerales.Tables[2];

                            if (MitCasetas.Rows.Count > 0)
                            {
                                //Declarando tabla
                                using (DataTable dtCasetas = new DataTable())
                                {
                                    dtCasetas.Columns.Add("Ruta", typeof(string));
                                    dtCasetas.Columns.Add("Caseta", typeof(string));
                                    dtCasetas.Columns.Add("TipoCaseta", typeof(string));
                                    dtCasetas.Columns.Add("RedCarretera", typeof(string));
                                    dtCasetas.Columns.Add("IAVE", typeof(string));
                                    dtCasetas.Columns.Add("Ejes", typeof(int));
                                    dtCasetas.Columns.Add("MontoIAVE", typeof(decimal));
                                    dtCasetas.Columns.Add("MontoEfectivo", typeof(decimal));
                                    dtCasetas.Columns.Add("Deposito", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitCasetas.Rows)
                                    {
                                        dtCasetas.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsCasetas = new ReportDataSource("Casetas", dtCasetas);
                                    rvReporte.LocalReport.DataSources.Add(rdsCasetas);
                                }
                            }
                            else
                            {
                                //Declarando tabla
                                using (DataTable dtCasetas = new DataTable())
                                {
                                    dtCasetas.Columns.Add("Ruta", typeof(string));
                                    dtCasetas.Columns.Add("Caseta", typeof(string));
                                    dtCasetas.Columns.Add("TipoCaseta", typeof(string));
                                    dtCasetas.Columns.Add("RedCarretera", typeof(string));
                                    dtCasetas.Columns.Add("IAVE", typeof(string));
                                    dtCasetas.Columns.Add("Ejes", typeof(int));
                                    dtCasetas.Columns.Add("MontoIAVE", typeof(decimal));
                                    dtCasetas.Columns.Add("MontoEfectivo", typeof(decimal));
                                    dtCasetas.Columns.Add("Deposito", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtCasetas.Rows.Clear();

                                    ReportDataSource rdsCasetas = new ReportDataSource("Casetas", dtCasetas);
                                    rvReporte.LocalReport.DataSources.Add(rdsCasetas);
                                }
                            }

                            if (MitConceptos.Rows.Count > 0)
                            {
                                //Declarando la tabla
                                using (DataTable dtConceptos = new DataTable())
                                {
                                    dtConceptos.Columns.Add("Id", typeof(string));
                                    dtConceptos.Columns.Add("Concepto", typeof(string));
                                    dtConceptos.Columns.Add("Cantidad", typeof(int));
                                    dtConceptos.Columns.Add("Precio", typeof(decimal));
                                    dtConceptos.Columns.Add("Monto", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitConceptos.Rows)
                                    {
                                        dtConceptos.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsConceptos = new ReportDataSource("Conceptos", dtConceptos);
                                    rvReporte.LocalReport.DataSources.Add(rdsConceptos);
                                }
                            }
                            else
                            {
                                //Declarando la tabla
                                using (DataTable dtConceptos = new DataTable())
                                {
                                    dtConceptos.Columns.Add("Id", typeof(string));
                                    dtConceptos.Columns.Add("Concepto", typeof(string));
                                    dtConceptos.Columns.Add("Cantidad", typeof(int));
                                    dtConceptos.Columns.Add("Precio", typeof(decimal));
                                    dtConceptos.Columns.Add("Monto", typeof(decimal));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtConceptos.Rows.Clear();

                                    ReportDataSource rdsConceptos = new ReportDataSource("Conceptos", dtConceptos);
                                    rvReporte.LocalReport.DataSources.Add(rdsConceptos);
                                }
                            }

                            if (MitDiesel.Rows.Count > 0)
                            {
                                //Declarando la tabla
                                using (DataTable dtDiesel = new DataTable())
                                {
                                    dtDiesel.Columns.Add("Id", typeof(string));
                                    dtDiesel.Columns.Add("Concepto", typeof(string));
                                    dtDiesel.Columns.Add("Cantidad", typeof(int));
                                    dtDiesel.Columns.Add("Precio", typeof(decimal));
                                    dtDiesel.Columns.Add("Monto", typeof(decimal));
                                    dtDiesel.Columns.Add("EstacionCombustible", typeof(string));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    foreach (DataRow r in MitDiesel.Rows)
                                    {
                                        dtDiesel.Rows.Add(r.ItemArray);
                                    }

                                    ReportDataSource rdsDiesel = new ReportDataSource("Diesel", dtDiesel);
                                    rvReporte.LocalReport.DataSources.Add(rdsDiesel);
                                }
                            }
                            else
                            {
                                //Declarando la tabla
                                using (DataTable dtDiesel = new DataTable())
                                {
                                    dtDiesel.Columns.Add("Id", typeof(string));
                                    dtDiesel.Columns.Add("Concepto", typeof(string));
                                    dtDiesel.Columns.Add("Cantidad", typeof(int));
                                    dtDiesel.Columns.Add("Precio", typeof(decimal));
                                    dtDiesel.Columns.Add("Monto", typeof(decimal));
                                    dtDiesel.Columns.Add("EstacionCombustible", typeof(string));

                                    //Recuperamos los valores y creamos la tabla a cargar en el RDLC
                                    dtDiesel.Rows.Clear();

                                    ReportDataSource rdsDiesel = new ReportDataSource("Diesel", dtDiesel);
                                    rvReporte.LocalReport.DataSources.Add(rdsDiesel);
                                }
                            }
                        }
                    }
                }
            }
            SAT_CL.Documentacion.Servicio Serv = new SAT_CL.Documentacion.Servicio(IdServicio);
            //Generando flujo del reporte 
            byte[] bytes = this.rvReporte.LocalReport.Render("PDF");
            //Descargando Archivo PDF
            //TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("Gastos_generales_servicio_{0}.pdf", Serv.no_servicio), TSDK.Base.Archivo.ContentType.application_PDF);
        }

        /// <summary>
        /// Metodo que inicializa en forma específica el reporte de facturas globales
        /// </summary>
        private void inicializaFacturaGlobal()
        {
            //Obteniendo Segmento
            int IdFacturaGlobal = Convert.ToInt32(Request.QueryString["idRegistro"]);

            //Cargando Reporte
            rvReporte.LocalReport.ReportPath = Server.MapPath("~/RDLC/FacturaGlobal.rdlc");
            //Limpia el datasource
            rvReporte.LocalReport.DataSources.Clear();
            //Declarando Arreglo de Parametros por Recibir
            ReportParameter[] param = new ReportParameter[6];
            //Instanciando Proceso
            using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(IdFacturaGlobal))
            {
                using (SAT_CL.Global.CompaniaEmisorReceptor CER = new SAT_CL.Global.CompaniaEmisorReceptor(fg.id_compania))
                {
                    byte[] logo = null;
                    //Declarando la tabla para almacenar al logo
                    using (DataTable dtLogo = new DataTable())
                    {
                        //Añadiendo la única columna
                        dtLogo.Columns.Add("Logotipo", typeof(byte[]));
                        try { logo = System.IO.File.ReadAllBytes(CER.ruta_logotipo); }
                        catch { logo = null; }

                        //Agregando imagen
                        dtLogo.Rows.Add(logo);

                        //Agregamos al origen de datos
                        ReportDataSource rdsLogo = new ReportDataSource("Logotipo", dtLogo);
                        rvReporte.LocalReport.DataSources.Add(rdsLogo);
                    }
                }
            }

            //Instanciando Proceso
            using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(IdFacturaGlobal))
            {
                using (SAT_CL.Global.CompaniaEmisorReceptor CER = new SAT_CL.Global.CompaniaEmisorReceptor(fg.id_compania))
                {
                    //DataSet DSFacturaGlobal = SAT_CL.FacturacionElectronica33.Reporte.cargaDetallesFacturaGlobal(CER.id_compania_emisor_receptor, IdFacturaGlobal)
                    using (DataSet dsFacturaGlobal = SAT_CL.FacturacionElectronica33.Reporte.cargaDetallesFacturaGlobal(CER.id_compania_emisor_receptor, IdFacturaGlobal))
                    {
                        //Validando que Exista el Paquete
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsFacturaGlobal, "Table"))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dsFacturaGlobal.Tables["Table"].Rows)
                            {
                                //Creando Parametros
                                param[0] = new ReportParameter("NoFactura", dr["NoFactura"].ToString());
                                param[1] = new ReportParameter("Cliente", dr["Cliente"].ToString());
                                param[2] = new ReportParameter("Estatus", dr["Estatus"].ToString());
                                param[3] = new ReportParameter("SerieFolio", dr["SerieFolio"].ToString());
                                param[4] = new ReportParameter("Descripcion", dr["Descripcion"].ToString());//*/
                                param[5] = new ReportParameter("FechaExp", dr["FechaExp"].ToString());
                            }
                        }

                        //Declarando Variables Auxiliares
                        DataTable dtviajesFacturaGlobal = new DataTable();

                        //Creando Columnas
                        dtviajesFacturaGlobal.Columns.Add("NoServicio", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("NoViaje", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("Referencia", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("Origen", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("Destino", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("FecFac", typeof(string));
                        dtviajesFacturaGlobal.Columns.Add("Subtotal", typeof(decimal));
                        dtviajesFacturaGlobal.Columns.Add("IVA", typeof(decimal));
                        dtviajesFacturaGlobal.Columns.Add("Retencion", typeof(decimal));
                        dtviajesFacturaGlobal.Columns.Add("Total", typeof(decimal));
                        //Validando que Exista el Paquete
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsFacturaGlobal, "Table1"))
                        {
                            //Recorriendo Registros
                            foreach (DataRow dr in dsFacturaGlobal.Tables["Table1"].Rows)

                                //Añadiendo Registros
                                dtviajesFacturaGlobal.Rows.Add(dr["NoServicio"].ToString(), dr["NoViaje"].ToString(), dr["Referencia"].ToString(), dr["Origen"].ToString(),
                                    dr["Destino"].ToString(), dr["FecFac"].ToString(), dr["Subtotal"].ToString(), dr["IVA"].ToString(), dr["Retencion"].ToString(), dr["Total"].ToString());
                        }
                        else
                            //Añadiendo Registros
                            dtviajesFacturaGlobal.Rows.Add("", "", "", "", "", "", 0, 0, 0, 0);

                        //Agregamos el origen de datos de Carga
                        ReportDataSource rdsviajesFacturasLigadas = new ReportDataSource("VajesFacturasGlobal", dtviajesFacturaGlobal);
                        rvReporte.LocalReport.DataSources.Add(rdsviajesFacturasLigadas);

                        //Asignando Parametros
                        this.rvReporte.LocalReport.SetParameters(param);
                    }
                }
            }
        }
    }
}