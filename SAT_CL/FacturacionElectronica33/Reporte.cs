using System;
using System.Configuration;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de Gestionar todos los Reportes de Facturación Electronica
    /// </summary>
    public class Reporte
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_reporte_modulo";

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de cargar los comprobantes
        /// </summary>
        /// <param name="id_compania_emisor">Número de la compañia que emite el comprobante</param>
        /// <param name="id_compania_receptor">Número de la compañia que recibe el comprobante</param>
        /// <param name="id_tipo">Número del tipo de comprobante</param>
        /// <param name="id_estatus">Estado del comprobante </param>
        /// <param name="inicioFechaExpedicion">Fecha inicial de expedición</param>
        /// <param name="finFechaExpedicion">Fecha final de expedición</param>
        /// <param name="generado">Indica si es generado o no</param>
        /// <param name="serie">Indica la serie del comprobante</param>
        /// <param name="folio">Indica el folio del comprobante</param>
        /// <param name="inicioFechaCaptura">Fecha inicial de captura</param>
        /// <param name="finFechaCaptura">Fecha final de captura</param>
        /// <param name="inicioFechaCancelacion">Fecha inicial de cancelación</param>
        /// <param name="finFechaCancelacion">Fecha final de cancelación</param>
        /// <param name="id_usuario_timbra">Número de usuario actual</param>
        /// <returns></returns>
        public static DataTable CargaComprobantes33(int id_compania_emisor, int id_compania_receptor, byte id_tipo, byte id_estatus, DateTime inicioFechaExpedicion, DateTime finFechaExpedicion, bool generado, string serie, int folio, DateTime inicioFechaCaptura, DateTime finFechaCaptura, DateTime inicioFechaCancelacion, DateTime finFechaCancelacion, int id_usuario_timbra)
        {
            //Declarando objeto de retorno
            DataTable dtFacturas = null;
            //Armando Arreglo de Parametros
            object[] param = { 8, id_compania_emisor, id_compania_receptor, id_tipo, id_estatus, inicioFechaExpedicion == DateTime.MinValue ? "" : inicioFechaExpedicion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), finFechaExpedicion == DateTime.MinValue ? "" : finFechaExpedicion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), generado ? "1" : "0", serie, folio, inicioFechaCaptura == DateTime.MinValue ? "" : inicioFechaCaptura.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), finFechaCaptura == DateTime.MinValue ? "" : finFechaCaptura.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), inicioFechaCancelacion == DateTime.MinValue ? "" : inicioFechaCancelacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), finFechaCancelacion == DateTime.MinValue ? "" : finFechaCancelacion.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), id_usuario_timbra, "", "", "", "", "", "", };
            //Obteniendo Reporte
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan registros
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando resultado obtenidi
                    dtFacturas = DS.Tables["Table"];
            }
            //Devolviendo tabla obtenida
            return dtFacturas;
        }

		/// <summary>
		/// Método encargado de Obtener los Datos de Facturado de Facturación Electronica v3.3
		/// </summary>
		/// <param name="id_factura">Facturado</param>
		/// <param name="id_forma_pago">Forma de Pago</param>
		/// <param name="id_cta_pago">Cuenta de Pago</param>
		/// <param name="id_metodo_pago">Método de Pago</param>
		/// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
		/// <param name="condiciones_pago">Condiciones de Pago</param>
		/// <param name="referencias_comprobante"></param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturadoFacturaElectronica(int id_factura, int id_forma_pago, int id_cta_pago, int id_metodo_pago, int id_tipo_comprobante, string condiciones_pago, string referencias_comprobante)
		{
			//Declarando Objeto de Retorno
			DataSet dsfactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 1, id_factura, id_forma_pago, id_cta_pago, id_metodo_pago, referencias_comprobante.Equals("") ? "0" : referencias_comprobante, id_tipo_comprobante, condiciones_pago, "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Instanciando Datos
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando Contenido
				if (Validacion.ValidaOrigenDatos(ds))
					//Devolviendo Resultado Obtenido
					dsfactura = ds;
			}
			//Devolviendo Resultado Obtenido
			return dsfactura;
		}
		/// <summary>
		/// Método encargado de Obtener los Datos de la Factura Global a Detalle
		/// </summary>
		/// <param name="id_factura_global"></param>
		/// <param name="id_forma_pago"></param>
		/// <param name="id_uso_cfdi"></param>
		/// <param name="id_metodo_pago"></param>
		/// <param name="id_condiciones_pago"></param>
		/// <param name="id_moneda"></param>
		/// <param name="fecha_cambio"></param>
		/// <param name="referencias"></param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturaGlobalDetalleFacturaElectronica(int id_factura_global, int id_forma_pago, int id_uso_cfdi, int id_metodo_pago, string condiciones_pago, int id_moneda, DateTime fecha_cambio, string referencias)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 2, id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago, condiciones_pago, id_moneda, referencias.Equals("") ? "0" : referencias, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando  Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dsFactura = ds;
			}
			//Devolviendo Objeto de Retorno
			return dsFactura; ;
		}
		/// <summary>
		/// Carga los datos de la Factura Global general por concepto a partir del esquema de facturación electrónica para su importación.
		/// </summary>
		/// <param name="id_factura_global">Id Factura Global</param>
		/// <param name="id_forma_pago">Id Forma Pago</param>
		/// <param name="id_uso_cfdi">No Cuenta Pago</param>
		/// <param name="id_metodo_pago">Metodo de Pago</param>
		/// <param name="condiciones_pago">Condiciones de Pago</param>
		/// <param name="id_moneda">Id Moneda</param>
		/// <param name="fecha_cambio">Fecha de Cambio</param>
		/// <param name="rerferencias">Referencias</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturaGlobalGeneralFacturaElectronica(int id_factura_global, int id_forma_pago, int id_uso_cfdi, int id_metodo_pago, string condiciones_pago, byte id_moneda, DateTime fecha_cambio, string referencias)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 3, id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago, condiciones_pago, id_moneda, referencias.Equals("") ? "0" : referencias, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando  Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Resultado Obtenido
					dsFactura = ds;
			}
			//Devolviendo Objeto de Retorno
			return dsFactura; ;
		}
		// <summary>
		/// Carga los datos de la Factura Global general por concepto sin distinción de montos a partir del esquema de facturación electrónica para su importación.
		/// </summary>
		/// <param name="id_factura_global">Id Factura Global</param>
		/// <param name="id_forma_pago">Id Forma Pago</param>
		/// <param name="no_cta_pago">No Cuenta Pago</param>
		/// <param name="id_metodo_pago">Metodo de Pago</param>
		/// <param name="id_condiciones_pago">Condiciones de Pago</param>
		/// <param name="id_moneda">Id Moneda</param>
		/// <param name="fecha_cambio">Fecha de Cambio</param>
		/// <param name="rerferencias">Referencias</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturaGlobalGeneralConceptoFacturaElectronica(int id_factura_global, int id_forma_pago, int id_uso_cfdi, int id_metodo_pago, string condiciones_pago, int id_moneda, DateTime fecha_cambio, string referencias)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 4, id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago, condiciones_pago, id_moneda, referencias, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dsFactura = ds;
			}
			return dsFactura;
		}
		/// <summary>
		/// Carga los datos de la Factura Global general por un solo concepto a partir del esquema de facturación electrónica para su importación.
		/// </summary>
		/// <param name="id_factura_global">Id Factura Global</param>
		/// <param name="id_forma_pago">Id Forma Pago</param>
		/// <param name="id_uso_cfdi">No Cuenta Pago</param>
		/// <param name="id_metodo_pago">Metodo de Pago</param>
		/// <param name="condiciones_pago">Condiciones de Pago</param>
		/// <param name="id_moneda">Id Moneda</param>
		/// <param name="fecha_cambio">Fecha de Cambio</param>
		/// <param name="no_identificacion">Identificación de los Conceptos</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(int id_factura_global, int id_forma_pago, int id_uso_cfdi, int id_metodo_pago, string condiciones_pago, int id_moneda, DateTime fecha_cambio, string no_identificacion)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 5, id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago, condiciones_pago, id_moneda, no_identificacion, fecha_cambio == DateTime.MinValue ? null : fecha_cambio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando  Registros
				if (Validacion.ValidaOrigenDatos(ds, "Table1"))
				{
					DataRow[] noIdentificacion = ds.Tables[1].Select("No_identificacion = ' '");
					if (noIdentificacion.Length > 0)
						noIdentificacion[0]["No_identificacion"] = no_identificacion;
					dsFactura = ds;
				}
			}
			//Devolviendo Objeto de Retorno
			return dsFactura; ;
		}
		/// <summary>
		/// Carga los datos requeridos para la importación de un CFDI de Comprobante de Recepción de Pagos
		/// </summary>
		/// <param name="id_egreso_ingreso">Id de Ingreso al que se asociará el CFDI de Recepción de Pagos</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosEncabezadoFIFacturaElectronicaRecepcionPago(int id_egreso_ingreso)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 6, id_egreso_ingreso, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
					dsFactura = ds;
			}
			return dsFactura;
		}
		/// <summary>
		/// Carga los datos requeridos para la importación de un CFDI de Comprobante de Recepción de Pagos, cuando se realiza una sustitución del mismo
		/// </summary>
		/// <param name="id_comprobante">Id del CFDI de Recepción de Pagos sustituido</param>
		/// <returns></returns>
		public static DataSet ObtienesDatosEncabezadoSustitucionFacturaElectronicaRecepcionPago(int id_comprobante)
		{
			//Declarando Objeto de Retorno
			DataSet dsFactura = null;
			//Armando Arreglo de Parametros
			object[] param = { 11, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
					dsFactura = ds;
			}
			return dsFactura;
		}
		/// <summary>
		/// Carga los datos de todos los documentos relacionados (CFDI) implicados en un CFDI de Comprobante de Recepción de Pagos
		/// </summary>
		/// <param name="id_egreso_ingreso">Id de Ingreso del que buscará Documentos Relacionados</param>
		/// <returns></returns>
		public static DataTable ObtienesDoctoRelacionadoFIFacturaElectronicaRecepcionPago(int id_egreso_ingreso)
		{
			//Declarando Objeto de Retorno
			DataTable mit = null;
			//Armando Arreglo de Parametros
			object[] param = { 7, id_egreso_ingreso, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					mit = ds.Tables["Table"];
			}
			return mit;
		}
		/// <summary>
		/// Carga el resumen de pagos / CFDI de Pagos con pendientes por atender, dado un Id de Compañía
		/// </summary>
		/// <param name="id_compania_emisor">Id de la Compañía Emisora de los CFDI o receptora de los Pagos</param>
		/// <returns></returns>
		public static DataTable ObtenerResumenPendientesCFDIPagosPorCliente(int id_compania_emisor)
		{
			//Declarando Objeto de Retorno
			DataTable mit = null;
			//Armando Arreglo de Parametros
			object[] param = { 9, id_compania_emisor, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					mit = ds.Tables["Table"];
			}
			return mit;
		}
		/// <summary>
		/// Cargalos CFDI de Comprobante de Recepción de Pagos ya existentes 
		/// </summary>
		/// <param name="id_compania_emisor">Id de la Compañía Emisora de los CFDI o receptora de los Pagos</param>
		/// <param name="id_compania_receptor">Id de Compañía que recibe el CFDI</param>
		/// <param name="fecha_inicio_timbrado"></param>
		/// <param name="fecha_fin_timbrado"></param>
		/// <param name="folio"></param>
		/// <param name="bit_registrado"></param>
		/// <param name="bit_timbrado"></param>
		/// <param name="bit_sustituido"></param>
		/// <param name="bit_por_sustituir"></param>
		/// <returns></returns>
		public static DataTable ObtenerCFDIRecepcionPagos(int id_compania_emisor, int id_compania_receptor, DateTime fecha_inicio_timbrado, DateTime fecha_fin_timbrado, string folio, bool bit_registrado, bool bit_timbrado, bool bit_sustituido, bool bit_por_sustituir)
		{
			//Declarando Objeto de Retorno
			DataTable mit = null;
			//Armando Arreglo de Parametros
			object[] param = { 10, id_compania_emisor, id_compania_receptor, Fecha.ConvierteDateTimeString(fecha_inicio_timbrado, "yyyyMMdd HH:mm"), Fecha.ConvierteDateTimeString(fecha_fin_timbrado, "yyyyMMdd HH:mm"), folio, Convert.ToInt32(bit_registrado), Convert.ToInt32(bit_timbrado), Convert.ToInt32(bit_sustituido), Convert.ToInt32(bit_por_sustituir), "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					mit = ds.Tables["Table"];
			}
			return mit;
		}
		/// <summary>
		/// Método encargado de Cargar los Comprobantes para Cancelación del Timbre Fiscal Digital (TFD) en se versión 3.3
		/// </summary>
		/// <param name="id_compania_emisor">Compania Emisora</param>
		/// <param name="id_tipo">Origen de Datos</param>
		/// <param name="id_compania_receptor">Compania Receptora</param>
		/// <param name="folio">Folio</param>
		/// <returns></returns>
		public static DataTable CargaCancelacionTimbreFiscalV33(int id_compania_emisor, byte id_tipo, int id_compania_receptor, int folio)
		{
			//Declarando Objeto de Retorno
			DataTable dtFacturas = null;
			//Armando Arreglo de Parametros
			object[] param = { 12, id_compania_emisor, id_tipo, id_compania_receptor, folio, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtFacturas = ds.Tables["Table"];
			}
			//Devolviendo Objeto de Retorno
			return dtFacturas;
		}
		/// <summary>
		/// Método encargado de Obtener los Comprobantes con Saldo de un Cliente
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="id_cliente">Cliente por Filtrar</param>
		/// <param name="serie">Serie</param>
		/// <param name="folio">Folio</param>
		/// <param name="fecha_exp_ini">Fecha de Expedición (Inicio)</param>
		/// <param name="fecha_exp_fin">Fecha de Expedición (Termino)</param>
		/// <returns></returns>
		public static DataTable ObtieneComprobantesConSaldo(int id_compania, int id_cliente, string serie, int folio, DateTime fecha_exp_ini, DateTime fecha_exp_fin)
		{
			//Declarando Objeto de Retorno
			DataTable dtComprobantesConSaldo = null;
			//Armando Arreglo de Parametros
			object[] param = { 13, id_compania, id_cliente, serie, folio, Fecha.ConvierteDateTimeString(fecha_exp_ini, "yyyyMMdd HH:mm"), Fecha.ConvierteDateTimeString(fecha_exp_fin, "yyyyMMdd HH:mm"), "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtComprobantesConSaldo = ds.Tables["Table"];
			}
			return dtComprobantesConSaldo;
		}
		/// <summary>
		/// Método encargado de Obtener el Saldo Pendiente dado un Comprobante
		/// </summary>
		/// <param name="id_comprobante">Comprobante v3.3</param>
		/// <returns></returns>
		public static decimal ObtieneSaldoPendienteComprobante(int id_comprobante)
		{
			//Declarando Objeto de Retorno
			decimal saldo_pendiente = 0.00M;
			//Armando Arreglo de Parametros
			object[] param = { 15, id_comprobante, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						saldo_pendiente = Convert.ToDecimal(dr["SaldoPendiente"]);
						break;
					}
				}
			}
			return saldo_pendiente;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id_compania_emisora"></param>
		/// <param name="id_compania_receptora"></param>
		/// <returns></returns>
		public static DataTable ObtieneComprobantesPendientesCancelacion(int id_compania_emisora, int id_compania_receptora, string serie, int folio)
		{
			//Declarando Objeto de Retorno
			DataTable dtCfdiPendientes = null;
			//Armando Arreglo de Parametros
			object[] param = { 16, id_compania_emisora, id_compania_receptora, "", 0, "", "", "", serie, folio, "", "", "", "", "", "", "", "", "", "", "" };
			//Obteniendo Reporte
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
					dtCfdiPendientes = ds.Tables["Table"];
			}
			return dtCfdiPendientes;
		}
		/// <summary>
		/// Método encagado de traer consulta para la forma FacturadoRecepcionPagosV10
		/// </summary>
		/// <returns></returns>
		public static DataTable ObtieneEgresos(int idCompania, DateTime fInicio, DateTime fFinal, int noEgreso, int noAnticipo, int noLiquidacion)
		{
			//Declarar retorno
			DataTable dtEgresos = null;
			//Crer arreglo de parametros
			object[] param = { 17, idCompania,
				fInicio == DateTime.MinValue ? "" : fInicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fFinal == DateTime.MinValue ? "" :  fFinal.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				noEgreso, noAnticipo, noLiquidacion, "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Consultar reporte
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					dtEgresos = DS.Tables["Table"];
			}
			return dtEgresos;
		}
        public static DataTable ObtieneComprobantes(int id_compania, int id_cliente, string serie, string folio, string no_ficha, DateTime fecha_inicio, DateTime fecha_fin, int formapago, string uuid)
        {
            //Declarar retorno
            DataTable dtcomprobantes = null;
            //Crer arreglo de parametros
            object[] param = { 18, id_compania, id_cliente, serie, folio, fecha_inicio == DateTime.MinValue ? "" : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
                fecha_fin == DateTime.MinValue ? "" :  fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), uuid, formapago, no_ficha, "", "", "", "", "", "", "", "", "", "", "" };
            //Consultar reporte
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    dtcomprobantes = DS.Tables["Table"];
            }
            return dtcomprobantes;
        }
        #endregion
    }
}