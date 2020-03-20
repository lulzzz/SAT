using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Tarifas
{
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Cargos Recurrentes
    /// </summary>
    public class CargoRecurrente : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas.sp_cargo_recurrente_tcr";

        private int _id_cargo_recurrente;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Cargo Concurrente
        /// </summary>
        public int id_cargo_recurrente { get { return this._id_cargo_recurrente; } }
        private int _id_tarifa;
        /// <summary>
        /// Atributo encargado de Almacenar la Tarifa
        /// </summary>
        public int id_tarifa { get { return this._id_tarifa; } }
        private int _id_tarifa_matriz;
        /// <summary>
        /// Atributo encargado de Almacenar la Tarifa Matriz
        /// </summary>
        public int id_tarifa_matriz { get { return this._id_tarifa_matriz; } }
        private int _id_tipo_cargo;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de cargo
        /// </summary>
        public int id_tipo_cargo { get { return this._id_tipo_cargo; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo encargado de Almacenar la Cantidad
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private byte _id_unidad;
        /// <summary>
        /// Atributo encargado de Almacenar la Unidad
        /// </summary>
        public byte id_unidad { get { return this._id_unidad; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo encargado de Almacenar el Valor Unitario
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Valores por Defecto
        /// </summary>
        public CargoRecurrente()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public CargoRecurrente(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~CargoRecurrente()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_cargo_recurrente = 0;
            this._id_tarifa = 0;
            this._id_tarifa_matriz = 0;
            this._id_tipo_cargo = 0;
            this._cantidad = 0;
            this._id_unidad = 0;
            this._valor_unitario = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Instanciando Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo los Registros
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_cargo_recurrente = id_registro;
                        this._id_tarifa = Convert.ToInt32(dr["IdTarifa"]);
                        this._id_tarifa_matriz = Convert.ToInt32(dr["IdTarifaMatriz"]);
                        this._id_tipo_cargo = Convert.ToInt32(dr["IdTipoCargo"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad = Convert.ToByte(dr["IdUnidad"]); ;
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]); ;
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_tarifa">Tarifa del Cargo</param>
        /// <param name="id_tarifa_matriz">Matriz de la Tarifa del Cargo</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="cantidad">Cantidad del Cargo</param>
        /// <param name="id_unidad">Unidad del Cargo</param>
        /// <param name="valor_unitario">Valor Unitario del Cargo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tarifa, int id_tarifa_matriz, int id_tipo_cargo,	decimal cantidad,
	                                                byte id_unidad,	decimal valor_unitario,	int id_usuario,	bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_cargo_recurrente, id_tarifa, id_tarifa_matriz, id_tipo_cargo, cantidad, id_unidad, 
                                 valor_unitario, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Cargos Recurrentes
        /// </summary>
        /// <param name="id_tarifa">Tarifa del Cargo</param>
        /// <param name="id_tarifa_matriz">Matriz de la Tarifa del Cargo</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="cantidad">Cantidad del Cargo</param>
        /// <param name="id_unidad">Unidad del Cargo</param>
        /// <param name="valor_unitario">Valor Unitario del Cargo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCargoRecurrente(int id_tarifa, int id_tarifa_matriz, int id_tipo_cargo, decimal cantidad,
                                                    byte id_unidad, decimal valor_unitario, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tarifa, id_tarifa_matriz, id_tipo_cargo, cantidad, id_unidad, 
                                 valor_unitario, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Cargos Recurrentes
        /// </summary>
        /// <param name="id_tarifa">Tarifa del Cargo</param>
        /// <param name="id_tarifa_matriz">Matriz de la Tarifa del Cargo</param>
        /// <param name="id_tipo_cargo">Tipo de Cargo</param>
        /// <param name="cantidad">Cantidad del Cargo</param>
        /// <param name="id_unidad">Unidad del Cargo</param>
        /// <param name="valor_unitario">Valor Unitario del Cargo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaCargoRecurrente(int id_tarifa, int id_tarifa_matriz, int id_tipo_cargo, decimal cantidad,
                                                    byte id_unidad, decimal valor_unitario, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tarifa, id_tarifa_matriz, id_tipo_cargo, cantidad, id_unidad,
                                 valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Cargos Recurrentes
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCargoRecurrente(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tarifa, this._id_tarifa_matriz, this._id_tipo_cargo, cantidad, this._id_unidad,
                                 this._valor_unitario, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de los Cargos Recurrentes
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCargoRecurrente()
        {   //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_cargo_recurrente);
        }
        /// <summary>
        /// Método Público encargado de Obtener los Cargos Recurrentes de la Tarifa y sus Detalles
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_detalle_tarifa">Id de Detalle de Tarifa</param>
        /// <returns></returns>
        public static DataTable ObtieneCargosRecurrentes(int id_tarifa, int id_detalle_tarifa)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_tarifa, id_detalle_tarifa, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Cargos Recurrentes
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que contenga Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método Público encargado de Obtener los Cargos Recurrentes de un Detalle de Tarifa
        /// </summary>
        /// <param name="id_detalle_tarifa">Id de Detalle de Tarifa</param>
        /// <returns></returns>
        public static DataTable ObtieneCargosRecurrentesPorDetalle(int id_detalle_tarifa)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_detalle_tarifa, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte de Cargos Recurrentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que contenga Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando resultado Obtenido
                    dt = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }

        #endregion
    }
}
