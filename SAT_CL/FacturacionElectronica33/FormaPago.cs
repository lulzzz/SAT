using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33 {
	/// <summary>
	/// Clase encargada de todas las funciones relacionadas con las Formas de Pago
	/// </summary>
	public class FormaPago : Disposable {
		#region Atributos

		/// <summary>
		/// Atributo que almacena el Nombre del SP
		/// </summary>
		private static string _nom_sp = "fe33.sp_forma_pago_tfp";

		private int _id_forma_pago;
		/// <summary>
		/// Atributo encargado de Obtener la Forma de Pago
		/// </summary>
		public int id_forma_pago { get { return this._id_forma_pago; } }
		private string _clave;
		/// <summary>
		/// Atributo encargado de Obtener la Clave
		/// </summary>
		public string clave { get { return this._clave; } }
		private string _descripcion;
		/// <summary>
		/// Atributo encargado de Obtener la Clave
		/// </summary>
		public string descripcion { get { return this._descripcion; } }
		private string _patron_cta_ordenante;
		/// <summary>
		/// Atributo encargado de Obtener la Clave
		/// </summary>
		public string patron_cta_ordenante { get { return this._patron_cta_ordenante; } }
		private string _patron_cta_beneficiario;
		/// <summary>
		/// Atributo encargado de Obtener la Clave
		/// </summary>
		public string patron_cta_beneficiario { get { return this._patron_cta_beneficiario; } }
		private bool _habilitar;
		/// <summary>
		/// Atributo encargado de Obtener el Estatus Habilitar
		/// </summary>
		public bool habilitar { get { return this._habilitar; } }


		#endregion

		#region Constructores

		/// <summary>
		/// Constructor que Inicializa los Valores por Defecto
		/// </summary>
		public FormaPago()
		{
			//Asignando Valores
			this._id_forma_pago = 0;
			this._clave =
			this._descripcion =
			this._patron_cta_ordenante =
			this._patron_cta_beneficiario = "";
			this._habilitar = false;
		}
		/// <summary>
		/// Constructor que Inicializa los Valores dado una Forma de Pago
		/// </summary>
		/// <param name="id_forma_pago">Forma de Pago</param>
		public FormaPago(int id_forma_pago)
		{
			//Invocando Método de Carga
			cargaAtributosInstancia(id_forma_pago);
		}

		#endregion

		#region Destructores

		/// <summary>
		/// Destructor de la Clase
		/// </summary>
		~FormaPago()
		{
			Dispose(false);
		}

		#endregion

		#region Métodos Privados

		/// <summary>
		/// Método encargado de Cargar los Atributos dado un Registro
		/// </summary>
		/// <param name="id_forma_pago">Forma de Pago</param>
		/// <returns></returns>
		private bool cargaAtributosInstancia(int id_forma_pago)
		{
			//Declarando Objeto de Retorno
			bool result = false;

			//Armando Arreglo de Parametros
			object[] param = { 3, id_forma_pago, "", "", "", "", 0, false, "", "" };

			//Instanciando Resultado del SP
			using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
			{
				//Validando Datos
				if (Validacion.ValidaOrigenDatos(ds, "Table"))
				{
					//Recorriendo Registro
					foreach (DataRow dr in ds.Tables["Table"].Rows)
					{
						//Asignando Valores
						this._id_forma_pago = id_forma_pago;
						this._clave = dr["Clave"].ToString();
						this._descripcion = dr["Descripcion"].ToString();
						this._patron_cta_ordenante = dr["PatronCtaOrdenamiento"].ToString();
						this._patron_cta_beneficiario = dr["PatronCtaBeneficiario"].ToString();
						this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

						//Terminando Ciclo
						break;
					}

					//Asignando Resultado Positivo
					result = true;
				}
			}

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Actualizar los Registros en la BD
		/// </summary>
		/// <param name="clave">Clave definida por el SAT</param>
		/// <param name="descripcion">Descripción de la Forma de Pago</param>
		/// <param name="patron_cta_ordenante">Patron que seguira la Cuenta del Ordenante</param>
		/// <param name="patron_cta_beneficiario">Patron que seguira la Cuenta del Beneficiario</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <param name="habilitar">Estatus Habilitar</param>
		/// <returns></returns>
		private RetornoOperacion actualizaRegistrosBD(string clave, string descripcion, string patron_cta_ordenante, string patron_cta_beneficiario,
																									int id_usuario, bool habilitar)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Armando Arreglo de Parametros
			object[] param = { 2, this._id_forma_pago, clave, descripcion, patron_cta_ordenante, patron_cta_beneficiario, id_usuario, habilitar, "", "" };

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}

		#endregion

		#region Métodos Públicos

		/// <summary>
		/// Método encargado de Insertar las Formas de Pago
		/// </summary>
		/// <param name="clave">Clave definida por el SAT</param>
		/// <param name="descripcion">Descripción de la Forma de Pago</param>
		/// <param name="patron_cta_ordenante">Patron que seguira la Cuenta del Ordenante</param>
		/// <param name="patron_cta_beneficiario">Patron que seguira la Cuenta del Beneficiario</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public static RetornoOperacion InsertaFormaPago(string clave, string descripcion, string patron_cta_ordenante, string patron_cta_beneficiario, int id_usuario)
		{
			//Declarando Objeto de Retorno
			RetornoOperacion result = new RetornoOperacion();

			//Armando Arreglo de Parametros
			object[] param = { 1, 0, clave, descripcion, patron_cta_ordenante, patron_cta_beneficiario, id_usuario, true, "", "" };

			//Ejecutando SP
			result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

			//Devolviendo Resultado Obtenido
			return result;
		}
		/// <summary>
		/// Método encargado de Editar las Formas de Pago
		/// </summary>
		/// <param name="clave">Clave definida por el SAT</param>
		/// <param name="descripcion">Descripción de la Forma de Pago</param>
		/// <param name="patron_cta_ordenante">Patron que seguira la Cuenta del Ordenante</param>
		/// <param name="patron_cta_beneficiario">Patron que seguira la Cuenta del Beneficiario</param>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion EditaFormaPago(string clave, string descripcion, string patron_cta_ordenante, string patron_cta_beneficiario, int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(clave, descripcion, patron_cta_ordenante, patron_cta_beneficiario, id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de Deshabilitar las Formas de Pago
		/// </summary>
		/// <param name="id_usuario">Usuario que Actualiza el Registro</param>
		/// <returns></returns>
		public RetornoOperacion DeshabilitaFormaPago(int id_usuario)
		{
			//Devolviendo Resultado Obtenido
			return this.actualizaRegistrosBD(this._clave, this._descripcion, this._patron_cta_ordenante, this._patron_cta_beneficiario,
																			 id_usuario, this._habilitar);
		}
		/// <summary>
		/// Método encargado de Actualizar las Formas de Pago
		/// </summary>
		/// <returns></returns>
		public bool ActualizaFormaPago()
		{
			//Devolviendo resultado Obtenido
			return this.cargaAtributosInstancia(this._id_forma_pago);
		}

		/// <summary>
		/// ENCARGADO DE DEVOLVER EL IDENTIFICADOR DE UNA MONEDA, POR SU CLAVE
		/// </summary>
		/// <param name="clave">Por ejemplo MXN, USD, EUR</param>
		/// <returns></returns>
		public static byte ObtenerId(string clave)
		{
			byte idFormaPago = 0;

			object[] param = { 4, 0, clave, "", "", "", 0, true, "", "" };

			using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
				if (Validacion.ValidaOrigenDatos(DS, "Table"))
					foreach (DataRow r in DS.Tables["Table"].Rows)
						idFormaPago = Convert.ToByte(r["IdFormaPago"]);

			return idFormaPago;
		}
		#endregion
	}
}
