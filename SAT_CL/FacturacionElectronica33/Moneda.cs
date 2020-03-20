using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33 {
	/// <summary>
	/// Clase encargada de gestionar todas las funciones de las Monedas
	/// </summary>
	public class Moneda : Disposable {
		#region Atributos

		/// <summary>
		/// Atributo encargado de Almacenar el Nombre del SP
		/// </summary>
		private static string _nom_sp = "fe33.sp_moneda_tm";

		private int _id_moneda;
		/// <summary>
		/// Atributo encargado de almacenar la Moneda
		/// </summary>
		public int id_moneda { get { return this._id_moneda; } }
		private string _clave;
		/// <summary>
		/// Atributo encargado de almacenar la Clave SAT
		/// </summary>
		public string clave { get { return this._clave; } }
		private string _descripcion;
		/// <summary>
		/// Atributo encargado de almacenar la Descripción
		/// </summary>
		public string descripcion { get { return this._descripcion; } }
		private int _decimales;
		/// <summary>
		/// Atributo encargado de almacenar los Decimales permitidos de la Moneda
		/// </summary>
		public int decimales { get { return this._decimales; } }
		private bool _habilitar;
		/// <summary>
		/// Atributo encargado de almacenar el Estatus Habilitar
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }

		#endregion

		#region Constructores

		/// <summary>
		/// Constructor encargado de Inicializar los Atributos por Defecto
		/// </summary>
		public Moneda()
		{
			//Inicializando Valores
			this._id_moneda = 0;
			this._clave =
			this._descripcion = "";
			this._decimales = 0;
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor encargado de Inicializar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_moneda"></param>
		public Moneda(int id_moneda)
		{
			//Cargando Atributos
			cargaAtributosInstancia(id_moneda);
		}

		#endregion

		#region Destructores

		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~Moneda()
		{
			Dispose(false);
		}

		#endregion

		#region Métodos Privados

		/// <summary>
		/// Método encargado de Asignar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_moneda">Moneda</param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_moneda)
		{
			//Declarando Objeto de Retorno
			bool result = false;

			//Armando Arreglo de Parametros
			object[] param = { 3, id_moneda, "", "", 0, 0, false, "", "" };

			//Instanciando dataset con resultado de consulta
			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando datos
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
				{
					//Recorriendo las filas d ela tabla
					foreach (DataRow r in DS.Tables["Table"].Rows)
					{
						//Inicializando Valores
						this._id_moneda = id_moneda;
						this._clave = r["Clave"].ToString();
						this._descripcion = r["Descripcion"].ToString();
						this._decimales = Convert.ToInt32(r["Decimal"]);
						this._habilitar = Convert.ToBoolean(r["Habilitar"]);
					}

					//Asignando Resultado Positivo
					result = true;
				}
			}

			//Devolviendo resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Actualizar los Registros en la BD
		/// </summary>
		/// <param name="clave">Clave</param>
		/// <param name="descripcion">Descripción</param>
		/// <param name="decimales">Decimales</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <param name="habilitar">Estatus Habilitar</param>
		/// <returns></returns>
		private RetornoOperacion actualizaRegistrosBD(string clave, string descripcion, int decimales, int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_moneda, clave, descripcion, decimales, id_usuario, habilitar, "", "" };

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}

		#endregion

		#region Métodos Públicos

		/// <summary>
		/// Método encargado de Insertar la Moneda
		/// </summary>
		/// <param name="clave">Clave del SAT</param>
		/// <param name="descripcion">Descripción de la Moneda</param>
		/// <param name="decimales">Decimales Permitidos</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaMoneda(string clave, string descripcion, int decimales, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Armando Arreglo de Parametros
			object[] param = { 1, 0, clave, descripcion, decimales, id_usuario, true, "", "" };

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Editar la Moneda
		/// </summary>
		/// <param name="clave">Clave del SAT</param>
		/// <param name="descripcion">Descripción de la Moneda</param>
		/// <param name="decimales">Decimales Permitidos</param>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion EditaMoneda(string clave, string descripcion, int decimales, int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(clave, descripcion, decimales, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de Deshabilitar la Moneda
		/// </summary>
		/// <param name="id_usuario">Usuario que actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaMoneda(int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(this._clave, this._descripcion, this._decimales, id_usuario, false);
		}
		/// <summary>
		/// Método encargado de Actualizar los Atributos de la Moneda
		/// </summary>
		/// <returns></returns>
		public bool ActualizaMoneda()
		{
			//Devolviendo Recarga de Atributos
			return this.cargaAtributosInstancia(this._id_moneda);
		}

		/// <summary>
		/// ENCARGADO DE DEVOLVER EL IDENTIFICADOR DE UNA MONEDA, POR SU CLAVE
		/// </summary>
		/// <param name="clave">Por ejemplo MXN, USD, EUR</param>
		/// <returns></returns>
		public static byte ObtenerId(string clave)
		{
			byte idMoneda = 0;

			object[] param = { 4, 0, clave, "", 0, 0, false, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					foreach (DataRow r in DS.Tables["Table"].Rows)
						idMoneda = Convert.ToByte(r["IdMoneda"]);

			return idMoneda;
		}
		#endregion
	}
}
