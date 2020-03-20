using System;
using System.Data;
using System.Xml;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica {
	/// <summary>
	/// Proporciona los elementos requeridos para la administración de Comprobantes Fiscales Electrónicos (CFD y CFDI)
	/// </summary>
	public partial class Comprobante {

		#region Validación SAT

		/// <summary>
		/// Valida el estatus de publicación del CFDI en los servidores del SAT
		/// </summary>
		/// <param name="cfdi">CFDI</param>
		/// <param name="rfc_emisor"></param>
		/// <param name="rfc_receptor"></param>
		/// <param name="monto"></param>
		/// <param name="UUID"></param>
		/// <param name="fecha_expedicion"></param>
		/// <returns></returns>
		public static RetornoOperacion ValidaEstatusPublicacionSAT(XmlDocument cfdi, out string rfc_emisor, out string rfc_receptor, out decimal monto, out string UUID, out DateTime fecha_expedicion)
		{
			//Declarando objeto de resultado
			RetornoOperacion resultado = new RetornoOperacion();
			string tipoComprobante = "";
			//Asignando variables para datos de interés
			rfc_emisor = rfc_receptor = UUID = "";
			monto = 0; fecha_expedicion = DateTime.MinValue;

			//Validando que el cfdi exista
			if (cfdi != null)
			{
				//Obteniendo la versión del comprobante
				string version = "";
				try
				{
					version = cfdi.DocumentElement.Attributes["version"] != null ? cfdi.DocumentElement.Attributes["version"].Value : cfdi.DocumentElement.Attributes["Version"].Value;
				}
				catch { }

				//En base a la versión del cfdi
				switch (version)
				{
					case "3.2":
						try
						{
							//Recuperando datos de interés para consulta en línea del CFDI
							rfc_emisor = cfdi.DocumentElement["Emisor", cfdi.DocumentElement.NamespaceURI].Attributes["rfc"].Value;
							rfc_receptor = cfdi.DocumentElement["Receptor", cfdi.DocumentElement.NamespaceURI].Attributes["rfc"].Value;
							monto = Convert.ToDecimal(cfdi.DocumentElement.Attributes["total"].Value);
							UUID = cfdi.DocumentElement["Complemento", cfdi.DocumentElement.NamespaceURI]["TimbreFiscalDigital", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital")].Attributes["UUID"].Value;
							fecha_expedicion = Convert.ToDateTime(cfdi.DocumentElement.Attributes["fecha"].Value);
						}
						catch { }
						break;
					case "3.3":
						try
						{
							//Recuperando datos de interés para consulta en línea del CFDI v3.3
							rfc_emisor = cfdi.DocumentElement["Emisor", cfdi.DocumentElement.NamespaceURI].Attributes["Rfc"].Value;
							rfc_receptor = cfdi.DocumentElement["Receptor", cfdi.DocumentElement.NamespaceURI].Attributes["Rfc"].Value;
							monto = Convert.ToDecimal(cfdi.DocumentElement.Attributes["Total"].Value);
							UUID = cfdi.DocumentElement["Complemento", cfdi.DocumentElement.NamespaceURI]["TimbreFiscalDigital", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital")].Attributes["UUID"].Value;
							fecha_expedicion = Convert.ToDateTime(cfdi.DocumentElement.Attributes["Fecha"].Value);
							tipoComprobante = cfdi.DocumentElement.Attributes["TipoDeComprobante"].Value;
						}
						catch { }
						break;
				}

				//Verificando que los datos indispensables se hayan recuperdo exitosamente
				if (
						(rfc_emisor != "" && rfc_receptor != "" && UUID != "" && monto != 0 && fecha_expedicion != DateTime.MinValue)//Todas las facturas
						||
						(tipoComprobante == "P" && rfc_emisor != "" && rfc_receptor != "" && UUID != "" && monto == 0 && fecha_expedicion != DateTime.MinValue)//Facturas de tipo Pago, con total = 0
					)
				{
					//Creando mensaje de consulta
					string mensaje = string.Format("?re={0}&rr={1}&tt={2}&id={3}", rfc_emisor, rfc_receptor, monto, UUID);

					//Instanciando cliente de consulta
					ConsultaPublicacionCFDI.ConsultaCFDIServiceClient cliente = new ConsultaPublicacionCFDI.ConsultaCFDIServiceClient();
					//Abriendo canal de comunicación
					cliente.Open();
					//Realizando consulta
					ConsultaPublicacionCFDI.Acuse acuse = cliente.Consulta(mensaje);

					//Determinando estatus de respuesta
					switch (acuse.Estado)
					{
						case "Cancelado":
						case "No Encontrado":
							resultado = new RetornoOperacion(-1, string.Format("Comprobante '{0}': {1}", acuse.Estado, acuse.CodigoEstatus), false);
							break;
						case "Vigente":
							resultado = new RetornoOperacion(1, string.Format("Comprobante '{0}': {1}", acuse.Estado, acuse.CodigoEstatus), true);
							break;
						default:
							resultado = new RetornoOperacion(-1, "Error no clasificado.", false);
							break;
					}
				}
			}

			//Devolviendo resultado
			return resultado;
		}

		#endregion

		#region Métodos Públicos

		/// <summary>
		/// Método encargado de Validar si la Compania tiene Facturas Electronicas
		/// </summary>
		public static bool ValidaFacturacionElectronica(int id_compania_receptora)
		{
			//Declarando Objeto de Retorno
			bool result = false;

			//Inicializando parametros
			object[] parametros = { 15, 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, 0, 0,
																		0, 0, 0, 0, 0, id_compania_receptora, 0, null, null, null, false, false, 0,
																		false, 0, "", 0, "", 0, null, 0, 0, false, "", "" };

			//Instanciando dataset con resultado de consulta
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
			{
				//Validando datos
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
				{
					//Recorriendo las filas d ela tabla
					foreach (DataRow r in DS.Tables["Table"].Rows)

						//Obteniendo Validador
						result = Convert.ToBoolean(r["Indicador"]);
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}

		#endregion
	}
}
