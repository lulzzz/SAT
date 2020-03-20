using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAT_CL.Nomina {
	/// <summary>
	/// Clase
	/// </summary>
	public class Reporte {
		#region Atributos
		/// <summary>
		/// 
		/// </summary>
		private static string _nom_sp = "nomina.sp_reporte_modulo";
		#endregion
		#region Métodos
		/// <summary>
		/// Método encargado de Obtener el Reporte de Nomina
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="no_consecutivo">No. Consecutivo de Nomina</param>
		/// <param name="fecha_ini_pago">Fecha de Inicio del Pago</param>
		/// <param name="fecha_fin_pago">Fecha de Fin del Pago</param>
		/// <param name="fecha_ini_nomina">Fecha de Inicio de la Nomina</param>
		/// <param name="fecha_fin_nomina">Fecha de fin de la Nomina</param>
		/// <returns></returns>
		public static DataTable ObtieneNominaEncabezado(int id_compania, int no_consecutivo, DateTime fecha_ini_pago, DateTime fecha_fin_pago, DateTime fecha_ini_nomina, DateTime fecha_fin_nomina)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominas = null;
			//Inicializando los parámetros de consulta 
			object[] param = { 1, id_compania, no_consecutivo,
				fecha_ini_pago == DateTime.MinValue ? "": fecha_ini_pago.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_fin_pago == DateTime.MinValue ? "": fecha_fin_pago.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_ini_nomina == DateTime.MinValue ? "": fecha_ini_nomina.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_fin_nomina == DateTime.MinValue ? "": fecha_fin_nomina.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				"", "", "", "", "","","","","", "", "", "", "", "" };
			//Cargando los registros de interés
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que Existan Registros
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Valores Obtenidos
					dtNominas = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dtNominas;
		}
		/// <summary>
		/// Método que permite almacenar los conceptos de nomina.
		/// </summary>
		/// <param name="id_nomina_empleado">Identificador del empleado al cual se consultara los conceptos de nomina</param>
		/// <returns></returns>
		public static DataTable ConceptoComprobanteNominaDeduccion(int id_nomina_empleado)
		{
			//Creacion de la tabla ConceptoComprobanteNomina
			DataTable dtConceptoComprobanteNominaDeduccion = null;
			//Creación del objeto param que almacena los parametros 
			object[] param = { 2, id_nomina_empleado, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Invoca al método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida que existan registros 
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
					//Almacena la consulta de los datos en la tabla ConceptoComprobanteNomina
					dtConceptoComprobanteNominaDeduccion = DS.Tables["Table"];
			}
			//Retorna la tabla al método.
			return dtConceptoComprobanteNominaDeduccion;
		}
		/// <summary>
		/// Método que permite almacenar los conceptos de nomina.
		/// </summary>
		/// <param name="id_nomina_empleado">Identificador del empleado al cual se consultara los conceptos de nomina</param>
		/// <returns></returns>
		public static DataTable ConceptoComprobanteNominaPercepcion(int id_nomina_empleado)
		{
			//Creacion de la tabla ConceptoComprobanteNomina
			DataTable dtConceptoComprobanteNominaPercepcion = null;
			//Creación del objeto param que almacena los parametros 
			object[] param = { 3, id_nomina_empleado, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Invoca al método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida que existan registros 
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
					//Almacena la consulta de los datos en la tabla ConceptoComprobanteNomina
					dtConceptoComprobanteNominaPercepcion = DS.Tables["Table"];
			}
			//Retorna la tabla al método.
			return dtConceptoComprobanteNominaPercepcion;
		}
		/// <summary>
		/// Método encargado de Obtener el Reporte de Nomina Version 1.2
		/// </summary>
		/// <param name="id_compania">Compania Emisora</param>
		/// <param name="no_consecutivo">No. Consecutivo de Nomina</param>
		/// <param name="fecha_ini_pago">Fecha de Inicio del Pago</param>
		/// <param name="fecha_fin_pago">Fecha de Fin del Pago</param>
		/// <param name="fecha_ini_nomina">Fecha de Inicio de la Nomina</param>
		/// <param name="fecha_fin_nomina">Fecha de fin de la Nomina</param>
		/// <returns></returns>
		public static DataTable ObtieneNominaEncabezadoV_1_2(int id_compania, int no_consecutivo, DateTime fecha_ini_pago, DateTime fecha_fin_pago, DateTime fecha_ini_nomina, DateTime fecha_fin_nomina)
		{
			//Declarando Objeto de Retorno
			DataTable dtNominas = null;
			//Inicializando los parámetros de consulta 
			object[] param = { 4, id_compania, no_consecutivo,
				fecha_ini_pago == DateTime.MinValue ? "": fecha_ini_pago.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_fin_pago == DateTime.MinValue ? "": fecha_fin_pago.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_ini_nomina == DateTime.MinValue ? "": fecha_ini_nomina.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fecha_fin_nomina == DateTime.MinValue ? "": fecha_fin_nomina.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				"", "", "", "", "","","","","", "", "", "", "", "" };
			//Cargando los registros de interés
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando que Existan Registros
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
					//Asignando Valores Obtenidos
					dtNominas = ds.Tables["Table"];
			}
			//Devolviendo Resultado Obtenido
			return dtNominas;
		}
		/// <summary>
		/// Método que permite almacenar los conceptos de nomina 1.2
		/// </summary>
		/// <param name="id_nomina_empleado">Identificador del empleado al cual se consultara los conceptos de nomina</param>
		/// <returns></returns>
		public static DataTable ConceptoComprobanteNominaDeduccionNuevaV(int id_nomina_empleado)
		{
			//Creacion de la tabla ConceptoComprobanteNomina
			DataTable dtConceptoComprobanteNominaDeduccion = null;
			//Creación del objeto param que almacena los parametros 
			object[] param = { 5, id_nomina_empleado, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Invoca al método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida que existan registros 
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
					//Almacena la consulta de los datos en la tabla ConceptoComprobanteNomina
					dtConceptoComprobanteNominaDeduccion = DS.Tables["Table"];
			}
			//Retorna la tabla al método.
			return dtConceptoComprobanteNominaDeduccion;
		}
		/// <summary>
		/// Método que permite almacenar los conceptos de nomina 1.2.
		/// </summary>
		/// <param name="id_nomina_empleado">Identificador del empleado al cual se consultara los conceptos de nomina</param>
		/// <returns></returns>
		public static DataTable ConceptoComprobanteNominaPercepcionNuevaV(int id_nomina_empleado)
		{
			//Creacion de la tabla ConceptoComprobanteNomina
			DataTable dtConceptoComprobanteNominaPercepcion = null;
			//Creación del objeto param que almacena los parametros 
			object[] param = { 6, id_nomina_empleado, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			//Invoca al método EjecutaProcAlmacenadoDataSet
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Valida que existan registros 
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
					//Almacena la consulta de los datos en la tabla ConceptoComprobanteNomina
					dtConceptoComprobanteNominaPercepcion = DS.Tables["Table"];
			}
			//Retorna la tabla al método.
			return dtConceptoComprobanteNominaPercepcion;
		}
		/// <summary>
		/// Método que obtiene una tabla con el Reporte de Nomina de Empleados
		/// </summary>
		/// <param name="id_empleado"></param>
		/// <param name="no_nomina"></param>
		/// <param name="fec_pago_i"></param>
		/// <param name="fec_pago_f"></param>
		/// <param name="fec_nomi_i"></param>
		/// <param name="fec_nom_f"></param>
		/// <param name="tipo_nomina"></param>
		/// <returns></returns>
		public static DataTable ObtieneReporteNominaEmpleados(int id_empleado, int no_nomina, DateTime fec_pago_i, DateTime fec_pago_f, DateTime fec_nomi_i, DateTime fec_nomi_f, byte id_estatus, int id_compania)
		{
			//Crear objeto retorno
			DataTable dtReporteNominaEmpleados = null;
			//Crear arreglo de parámetros
			object[] param = { 7, id_empleado, no_nomina,
				fec_pago_i == DateTime.MinValue ? "" : fec_pago_i.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fec_pago_f == DateTime.MinValue ? "" : fec_pago_f.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fec_nomi_i == DateTime.MinValue ? "" : fec_nomi_i.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				fec_nomi_f == DateTime.MinValue ? "" : fec_nomi_f.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]),
				id_compania, id_estatus, "","","","","","","","","","","","" };
			//Ejecutando SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validar que existan registros
				if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
					//Vaciando resultado al objeto retorno
					dtReporteNominaEmpleados = ds.Tables["Table"];
			}
			//Devolver objeto
			return dtReporteNominaEmpleados;
		}
		#endregion
	}
}
