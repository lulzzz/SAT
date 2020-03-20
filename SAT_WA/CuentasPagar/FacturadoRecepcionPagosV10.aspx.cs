using SAT_CL;
using SAT_CL.CXP;
using SAT_CL.CXC;
using SAT_CL.Seguridad;
using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using FE = SAT_CL.FacturacionElectronica;
using FE33 = SAT_CL.FacturacionElectronica33;

namespace SAT.CuentasPagar {
	public partial class FacturadoRecepcionPagosV10 : System.Web.UI.Page {
		#region Eventos
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAceptarAltaProveedor_Click(object sender, EventArgs e)
		{
			//Ocultando ventana Modal de inserción de proveedor
			ScriptServer.AlternarVentana(upbtnAceptarAltaProveedor, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
			//Validando estatus de publicación
			ValidarEstatusPublicacionSAT(btnAceptarAltaProveedor);
		}
		/// <summary>
		/// Al presionar el boton Buscar, consulta la base de datos, carga el gvEgresos y almacena esa misma consulta en Session. Si no encuentra, vaciar gv y Session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnBuscarEgreso_Click(object sender, EventArgs e)
		{
			CargaEgresos();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnCancelarAltaProveedor_Click(object sender, EventArgs e)
		{
			//Ocultando ventana Modal
			ScriptServer.AlternarVentana(upbtnCancelarAltaProveedor, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
		}
		/// <summary>
		/// Al presionar el boton Importar, de la ventana modal. Lee el XML recibido y valida la informacion que contiene
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnImportar_Click(object sender, EventArgs e)
		{
			if(Session["XML"] != null)
			{
				XDocument factura_xml = (XDocument)Session["XML"];
				XNamespace nsCFDI = factura_xml.Root.GetNamespaceOfPrefix("cfdi");
				XNamespace nsPago = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace Pagos SAT");

				string rfc = "", nombre = "";
				//Intenta leer y obtener los atributos nombre y RFC del emisor, segun la version 
				try
				{
					switch(factura_xml.Root.Attribute("Version") != null ? factura_xml.Root.Attribute("Version").Value : factura_xml.Root.Attribute("version").Value)
					{
						case "3.2":
						{
							rfc = factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("rfc").Value.ToUpper();
							nombre = factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("nombre").Value.ToUpper();
							break;
						}
						case "3.3":
						{
							rfc = factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("Rfc").Value.ToUpper();
							nombre = factura_xml.Root.Element(nsCFDI + "Emisor").Attribute("Nombre").Value.ToUpper();
							break;
						}
					}
				}
				catch (Exception ex)
				{
					ScriptServer.MuestraNotificacion(btnImportar, ex.Message, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
				}
				if (rfc != "" && nombre != "")
				{
					//Validar el documento por medio de la compania emisor
					using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(rfc, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
					{
						if (emisor.id_compania_emisor_receptor > 0)
							ValidarEstatusPublicacionSAT(btnImportar);
						else
						{
							CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "");
							lblProveedorFactura.Text = nombre;
							ScriptServer.AlternarVentana(upbtnImportar, "Ventana Confirmacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
						}
					}
				}
				else
					ScriptServer.MuestraNotificacion(btnImportar, "El documento no contiene RFC y Nombre del Emisor.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
			}
			else
				ScriptServer.MuestraNotificacion(btnImportar, "Arrastre un nuevo documento al recuadro.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnValidacionSAT_Click(object sender, EventArgs e)
		{
			//Determinando respuesta del usuario
			switch (((Button)sender).CommandName)
			{
				case "Descartar":
					ScriptServer.AlternarVentana(btnCancelarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
					break;
				case "Continuar":
					//Realizando proceso de guardado de factura de proveedor
					guardaXML();
					ScriptServer.AlternarVentana(btnAceptarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void chkIncluirFechas_CheckedChanged(object sender, EventArgs e)
		{
			HabilitarFechas();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ddlMostrar_SelectedIndexChanged(object sender, EventArgs e)
		{
			Controles.CambiaTamañoPaginaGridView(gvEgresos, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlMostrar.SelectedValue), true, 1);		
		}
		/// <summary>
		/// Al cambiar de página el gvEgresos
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvEgresos_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			Controles.CambiaIndicePaginaGridView(gvEgresos, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex, true, 1);
		}
		/// <summary>
		/// Al presionar el nombre de una columna, ordenar por ese campo. Intermitente ASC-DESC
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvEgresos_Sorting(object sender, GridViewSortEventArgs e)
		{
			lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvEgresos, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression, true, 1);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lkbExportar_Click(object sender, EventArgs e)
		{
			//Obteniendo Control
			LinkButton lkb = (LinkButton)sender;
			//Validando Comando
			switch (lkb.CommandName)
			{
				case "Pagos":
					Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdEgreso");
					break;
				case "Facturas":
					Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
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
			//Obteniendo Control
			LinkButton lkb = (LinkButton)sender;
			//Validando Comando
			switch (lkb.CommandName)
			{
				default:
					alternarVentana(lkb, lkb.CommandName);
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkSeleccionarEgreso_Click(object sender, EventArgs e)
		{
			if (gvEgresos.DataKeys.Count > 0)
			{
				LinkButton lnk = (LinkButton)sender;
				alternarVentana(lnk, lnk.CommandName);
				#region Seleccionar fila y enviar datos a la ventana modal
				Controles.SeleccionaFila(gvEgresos, sender, "lnk", false);
				lblNoEgreso.Text = gvEgresos.SelectedDataKey["NoEgreso"].ToString();
				lblEstatus.Text = gvEgresos.SelectedDataKey["Estatus"].ToString();
				lblOrigen.Text = gvEgresos.SelectedDataKey["Origen"].ToString();
				lblFormaPago.Text = gvEgresos.SelectedDataKey["FormaPago"].ToString(); 
				lblFechaPago.Text = Convert.ToDateTime(gvEgresos.SelectedDataKey["FechaPago"].ToString()).ToString("dd/MM/yyyy");
				lblMonto.Text = $"${gvEgresos.SelectedDataKey["Monto"].ToString()} {gvEgresos.SelectedDataKey["Moneda"]}";
				lblMonto.ToolTip = $"${gvEgresos.SelectedDataKey["MontoPesos"].ToString()} MXN";
				lblBeneficiario.Text = gvEgresos.SelectedDataKey["Beneficiario"].ToString();
				using (DataTable dtPagos = SAT_CL.Bancos.EgresoIngreso.ObtienePagos(Convert.ToInt32(gvEgresos.SelectedDataKey["IdTEI"]), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
				{
					if (Validacion.ValidaOrigenDatos(dtPagos))
					{
						Controles.CargaGridView(gvPagosEgreso, dtPagos, "IdTPF", "IdTPF");
						Controles.InicializaGridview(gvDocumentosPago);
					}
					else
					{
						Controles.InicializaGridview(gvPagosEgreso);
						ScriptServer.MuestraNotificacion(this.Page, "Ningun pago coincide con los datos del egreso.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
					}
				}
				#endregion
			}
			else ScriptServer.MuestraNotificacion(this.Page, "Ningun egreso seleccionado.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
		}
		/// <summary>
		/// Al iniciar la página: Aplicar seguridad e inicializar interfaz
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			Forma.AplicaSeguridadForma(this, "content1", ((Usuario)Session["usuario"]).id_usuario);
			if (!Page.IsPostBack)
				InicializaPagina();
		}
		/// <summary>
		/// Evento activado al cargar informacion en el gvPagosEgreso
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvPagosEgreso_RowDataBound(object sender, GridViewRowEventArgs e){}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lnkSeleccionarPago_Click(object sender, EventArgs e)
		{
			if (gvPagosEgreso.DataKeys.Count > 0)
			{
				LinkButton lnk = (LinkButton)sender;
				Controles.SeleccionaFila(gvPagosEgreso, lnk, "lnk", false);
				switch (lnk.CommandName)
				{
					case "Abrir":
					{
						try
						{
							using (DataTable dtDocumentos = PagoFacturado.ObtieneDocumentos(Convert.ToInt32(gvPagosEgreso.SelectedDataKey["IdTPF"])))
							{
								if (Validacion.ValidaOrigenDatos(dtDocumentos))
									Controles.CargaGridView(gvDocumentosPago, dtDocumentos, "IdTDP-IdPF", "IdTDP");
								else
									Controles.InicializaGridview(gvDocumentosPago);
							}
						}
						catch
						{
							ScriptServer.MuestraNotificacion(this.Page, "No hay Documentos coincidentes", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
						}
						break;
					}
					case "Ligar":
					{
						decimal totalSaldoDocumentos = 0;
						int idCompania = ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
						int idUsuario = ((Usuario)Session["usuario"]).id_usuario;
						RetornoOperacion resultado = new RetornoOperacion();
						try
						{
							//Volver a obtener los Documentos Relacionados del Pago
							using (DataTable dtDocumentos = PagoFacturado.ObtieneDocumentos(Convert.ToInt32(gvPagosEgreso.SelectedDataKey["IdTPF"])))
							{
								if (Validacion.ValidaOrigenDatos(dtDocumentos))
								{
									//Revisar si el total de los saldos pagados de todos los Documentos Relacionados, coincide con el monto del Pago
									foreach (DataRow rowDocumento in dtDocumentos.Rows)
										totalSaldoDocumentos += Convert.ToDecimal(rowDocumento["ImportePagTDP"]);
									using (PagoFacturado pago = new PagoFacturado(Convert.ToInt32(gvPagosEgreso.SelectedDataKey["IdTPF"])))
									{
										if (totalSaldoDocumentos == pago.monto)
										{
											//Crear ficha entre el Egreso seleccionado y la factura del Documento Relacionado
											using (TransactionScope transaccionFactura = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
											{
												resultado = FE33.EgresoIngresoComprobante.InsertaEgresoIngresoComprobante(
													Convert.ToInt32(gvEgresos.SelectedDataKey["IdTEI"]),
													2, //Tipo Operacion: Egreso
													pago.idFacturadoProveedor,
													1, //Estatus: Capturada
													0, //No reemplaza
													idUsuario);
												if (resultado.OperacionExitosa)
												{
													int idEgresoIngresoComp = resultado.IdRegistro;
													foreach (DataRow rowDocumento in dtDocumentos.Rows)
													{
														//Instanciar por el UUID del Documento Relacionado
														using (FacturadoProveedor facturado = new FacturadoProveedor(rowDocumento["UUIDTDP"].ToString(), idCompania))
														{
															if (facturado.habilitar)
															{
																DataTable listaAplicaciones = FichaIngresoAplicacion.ObtieneAplicacionesFacturas(72, facturado.id_factura, Convert.ToInt32(gvEgresos.SelectedDataKey["IdTEI"]));
																int idFichaAplicacion = (
																	from DataRow rowAplicacion 
																	in listaAplicaciones.Rows
																	where rowAplicacion.Field<int>("IdRegistro") == facturado.id_factura
																	select rowAplicacion.Field<int>("Id")).FirstOrDefault();
																resultado = FE33.ComprobantePagoDocumentoRelacionado.InsertarComprobantePagoDocumentoRelacionado(
																	FE33.ComprobantePagoDocumentoRelacionado.TipoOperacion.Egreso,
																	pago.idFacturadoProveedor,
																	FE33.ComprobantePagoDocumentoRelacionado.TipoOperacionDocumento.Egreso,
																	facturado.id_factura,
																	Convert.ToInt32(gvEgresos.SelectedDataKey["IdTEI"]),
																	idFichaAplicacion,
																	Convert.ToDecimal(rowDocumento["ImporteSalAntTDP"]),
																	Convert.ToDecimal(rowDocumento["ImportePagTDP"]),
																	Convert.ToByte(rowDocumento["NoParcialidadTDP"]),
																	idEgresoIngresoComp, idUsuario);
															}
															else
															{
																resultado = new RetornoOperacion($"La factura con UUID: {rowDocumento["UUIDTDP"].ToString()} no se encuentra en el sistema.", false);
																break;
															}
														}
													}
												}
												//Cambiar estatus al Pago a Ligado
												if (resultado.OperacionExitosa)
													resultado = pago.ActualizaEstatus(PagoFacturado.EstatusPago.Ligado, idUsuario);
												if (resultado.OperacionExitosa)
													transaccionFactura.Complete();
											}
										}
										else
											resultado = new RetornoOperacion("Los Importes de los Documentos Relacionados, no coinciden con el monto del Pago.", false);
									}
								}
								else
									resultado = new RetornoOperacion("El Pago no contiene Documentos Relacionados", false);
							}
						}
						catch
						{
							ScriptServer.MuestraNotificacion(this.Page, "No hay Pagos coincidentes", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
						}
						ScriptServer.MuestraNotificacion(this.Page, resultado.Mensaje, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
						break;
					}
				}
			}
		}
		/// <summary>
		/// Evento activado al cargar información en el gvDocumentosPago
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void gvDocumentosPago_RowDataBound(object sender, GridViewRowEventArgs e){}
		#endregion

		#region Métodos
		/// <summary>
		/// Método encargado de Alternar las ventanas Modales
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="comando"></param>
		private void alternarVentana(System.Web.UI.Control sender, string comando)
		{
			//Validando Comando
			switch (comando)
			{
				case "ResultadoSAT":
					ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
					break;
				case "ImportacionCxP":
					ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaImportacionCxP", "ventanaImportacionCxP");
					break;
				case "LigaEgresos":
					ScriptServer.AlternarVentana(sender, sender.GetType(), comando, "contenedorVentanaLigaEgresos", "ventanaLigaEgresos");
					break;
			}
		}
		/// <summary>
		/// Cargar datos en los DropDownListCapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);s
		/// </summary>
		private void CargaCatalogos()
		{
			CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMostrar, "", 18);
		}
		/// <summary>
		/// Método encargado en llenar gvEgresos
		/// </summary>
		private void CargaEgresos()
		{
			//Obtener fechas desde UI si son requeridas
			DateTime fInicio, fFinal;
			fInicio = fFinal = DateTime.MinValue;
			if (chkIncluirFechas.Checked)
			{
				DateTime.TryParse(txtFechaInicio.Text, out fInicio);
				DateTime.TryParse(txtFechaFin.Text, out fFinal);
			}
			//Consultar y llenar gridview
			using (DataTable dtEgresos = FE33.Reporte.ObtieneEgresos(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, fInicio, fFinal, Convert.ToInt32(txtNoEgreso.Text == "" ? "0" : txtNoEgreso.Text), Convert.ToInt32(txtNoAnticipo.Text == "" ? "0" : txtNoAnticipo.Text), Convert.ToInt32(txtNoLiquidacion.Text == "" ? "0" : txtNoLiquidacion.Text)))
			{
				if (Validacion.ValidaOrigenDatos(dtEgresos))
				{
					Controles.CargaGridView(gvEgresos, dtEgresos, "IdTEI-NoEgreso-Estatus-Origen-FormaPago-MetodoPago-FechaPago-Monto-Moneda-MontoPesos-Beneficiario", "IdTEI");
					Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtEgresos, "Table");
					Controles.InicializaIndices(gvEgresos);
				}
				else
				{
					Controles.InicializaGridview(gvEgresos);
					Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
				}
			}
		}		
		/// <summary>
		/// Método encargado de Importar el Archvio XML
		/// </summary>
		/// <returns></returns>
		private RetornoOperacion guardaXML()
		{
			RetornoOperacion resultado = new RetornoOperacion();
			int idFacturadoProveedor = 0;

			if (Session["XML"] != null)
			{
				XDocument factura = (XDocument)Session["XML"];
				XNamespace nsCFDI = factura.Root.GetNamespaceOfPrefix("cfdi");
				XNamespace nsTFD = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

				switch ((Pagina.Estatus)Session["estatus"])
				{
					default:
					{
						using (TransactionScope transaccionFactura = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
						{
							//Validando versión
							switch (factura.Root.Attribute("version") != null ? factura.Root.Attribute("version").Value : factura.Root.Attribute("Version").Value)
							{
								case "3.2":
								{
									ScriptServer.MuestraNotificacion(this.Page, "La versión 3.2 ya no es soportada", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);
									break;
								}
								case "3.3":
								{
									//Insertando CFDI 3.3 usando el tipo de servicio de la compañia
									using (SAT_CL.Global.CompaniaEmisorReceptor compania = new SAT_CL.Global.CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
										resultado = FacturadoProveedor.ImportaComprobanteVersion33(compania.id_compania_emisor_receptor, compania.id_tipo_servicio, Session["XMLFileName"].ToString(), factura, ((Usuario)Session["usuario"]).id_usuario);
									break;
								}
							}
							if (resultado.OperacionExitosa) transaccionFactura.Complete();
						}
						break;
					}
				}
			}
			else
				//Instanciando Excepción
				resultado = new RetornoOperacion("Cargue su archivo XML para importar");
			//Validando que exista
			if (resultado.OperacionExitosa)
			{
				//Reasignando Id de registro
				resultado = new RetornoOperacion(idFacturadoProveedor);
				//Establecemos el id del registro
				Session["id_registro"] = resultado.IdRegistro;
				//Establecemos el estatus de la forma
				Session["estatus"] = Pagina.Estatus.Lectura;
				//Eliminando Contenido en Sessión del XML
				Session["XML"] =
				Session["XMLFileName"] = null;
				//Inicializamos la forma
				//inicializaPagina();
				//Actualizamos la etiqueta de errores
				ScriptServer.MuestraNotificacion(this, resultado.Mensaje, ScriptServer.NaturalezaNotificacion.Exito,ScriptServer.PosicionNotificacion.AbajoDerecha );
			}
			//Devolviendo resultado Obtenido
			return resultado;
		}		
		/// <summary>
		/// 
		/// </summary>
		private void HabilitarFechas()
		{
			if (chkIncluirFechas.Checked) txtFechaInicio.Enabled = txtFechaFin.Enabled = true;
			else txtFechaInicio.Enabled = txtFechaFin.Enabled = false;
		}
		/// <summary>
		/// Método encargado de iniciar la interfaz
		/// </summary>
		private void InicializaPagina()
		{
			txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString();
			txtFechaInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString();
			CargaCatalogos(); //Muestra dropdownlist
			HabilitarFechas();
			Controles.InicializaGridview(gvEgresos); //Muestra el gvEgresos
			Session["XML"] = null; //Vacia el XML
		}
		/// <summary>
		/// Método web para leer el archivo xml arrastrado a la ventana modal
		/// </summary>
		/// <param name="archivoBase64"></param>
		/// <param name="nombreArchivo"></param>
		/// <param name="mimeType"></param>
		/// <returns></returns>
		[WebMethod]
		public static string LecturaArchivo(string archivoBase64, string nombreArchivo, string mimeType)
		{
			//Definiendo objeto de retorno
			string resultado = "";
			//Si hay elementos
			if (!string.IsNullOrEmpty(archivoBase64))
			{
				//Validando tipo de archivo (mime type), debe ser .xml
				if (mimeType == "text/xml")
				{
					try
					{
						//Convietiendo archivo a bytes
						byte[] responseData = Convert.FromBase64String(Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));
						//Declarando Documento XML
						XmlDocument doc = new XmlDocument();
						XDocument responseXml = new XDocument();
						//Obteniendo XML en cadena y Cargando Documento XML
						using (MemoryStream ms = new MemoryStream(responseData)) doc.Load(ms);
						//Convirtiendo XML
						responseXml = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);
						//Almacenando en variables de sesión
						HttpContext.Current.Session["XML"] = responseXml;
						HttpContext.Current.Session["XMLFileName"] = nombreArchivo;
						//Instanciando Resultado Positivo
						resultado = $"Archivo {nombreArchivo} cargado con éxito.";
					}
					catch (Exception ex)
					{
						//Limpiando en variables de sesión
						HttpContext.Current.Session["XML"] = null;
						HttpContext.Current.Session["XMLFileName"] = "";
						//Instanciando Excepción
						resultado = $"Error al Cargar el Archivo: {ex.Message}";
					}
				}
				else resultado = "El archivo seleccionado no tiene un formato válido. Formatos permitidos '.xls' / '.xlsx'."; //Si el tipo de archivo no es válido
			}
			else resultado = "No se encontró contenido en el archivo."; //Instanciando Excepción
			return resultado;
		}
		/// <summary>
		/// Realiza la validacion del estatus de publicacion del CFDI en servidores del SAT, y obtiene datos del emisor y receptor
		/// </summary>
		/// <param name="btnImportar"></param>
		private void ValidarEstatusPublicacionSAT(System.Web.UI.Control btnImportar)
		{
			//Variables auxiliares
			RetornoOperacion resultado = new RetornoOperacion();
			string rfcEmisor, rfcReceptor, uuid;
			decimal monto;
			DateTime fechaExpedicion;
			XDocument xDoc = (XDocument)Session["XML"];
			XNamespace xNS = xDoc.Root.GetNamespaceOfPrefix("cfdi");
			XNamespace XNSTFD = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");
			XmlDocument xmlDoc = new XmlDocument();
			using (var xmlReader = xDoc.CreateReader())	xmlDoc.Load(xmlReader);
			//Validar segun la version del XML
			switch (xDoc.Root.Attribute("version") != null ? xDoc.Root.Attribute("version").Value : xDoc.Root.Attribute("Version").Value)
			{
				case "3.2": case "3.3":
				{
					//Consultar y Usar la informacion obtenida
					resultado = FE.Comprobante.ValidaEstatusPublicacionSAT(xmlDoc, out rfcEmisor, out rfcReceptor, out monto, out uuid, out fechaExpedicion);
					imgValidacionSAT.Src = resultado.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
					headerValidacionSAT.InnerText = resultado.Mensaje;
					lblRFCEmisor.Text = rfcEmisor;
					lblRFCReceptor.Text = rfcReceptor;
					lblUUID.Text = uuid;
					lblTotalFactura.Text = monto.ToString("C");
					lblFechaExpedicion.Text = fechaExpedicion.ToString("MM/dd/yyyy h:mm tt");
					break;
				}
			}
			//Mostrar resultado obtenido
			ScriptServer.AlternarVentana(btnImportar, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
		}
		#endregion
		
	}
}