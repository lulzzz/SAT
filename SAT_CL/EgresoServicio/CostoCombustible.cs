using System;
using System.Data;
using TSDK.Base;
using System.Linq;

namespace SAT_CL.EgresoServicio
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones de los Costos del Combustible
    /// </summary>
    public class CostoCombustible : Disposable
    {

        /// <summary>
        /// Enumeración de Estatus
        /// </summary>
        public enum Estatus
        {   /// <summary>
            /// Estatus que Indica que los Litros del Vale estan en 0
            /// </summary>
            Diesel = 1,
            /// <summary>
            /// Estatus que Indica que los Litros del Vale son distintos de 0
            /// </summary>
            Magna,
            /// <summary>
            /// Estatus que Indica que el Vale ya fue facturado
            /// </summary>
            Premium
        }
        #region Atributos

        /// <summary>
        /// Atributo que Guarda el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_costo_combustible_tcc";
        private int _id_costo_combustible;
        /// <summary>
        /// Atributo que Guarda el Id del Costo del Combustible
        /// </summary>
        public int id_costo_combustible { get { return this._id_costo_combustible; } }
        private byte _id_tipo_combustible;
        /// <summary>
        /// Atributo que Guarda el Tipo de Combustible
        /// </summary>
        public byte id_tipo_combustible { get { return this._id_tipo_combustible; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo que Guarda la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo que Guarda el Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo que Guarda la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo que Guarda la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private decimal _costo_combustible;
        /// <summary>
        /// Atributo que Guarda el Costo del Combustible
        /// </summary>
        public decimal costo_combustible { get { return this._costo_combustible; } }
        private string _referencia;
        /// <summary>
        /// Atributo que Guarda la Referencia
        /// </summary>
        public string referencia { get { return this._referencia; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que Guarda el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public CostoCombustible()
        {   //Asignando Valores
            this._id_costo_combustible = 0; 
            this._id_tipo_combustible = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._costo_combustible = 0;
            this._referencia = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_costo_combustible">Id de Registro</param>
        public CostoCombustible(int id_costo_combustible)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_costo_combustible);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~CostoCombustible()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_costo_combustible">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_costo_combustible)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_costo_combustible, 0, 0, 0, null, null, 0, "", 0, false, "", "" };
            //Instanciando Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo el Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_costo_combustible = id_costo_combustible;
                        this._id_tipo_combustible = Convert.ToByte(dr["IdTipoCombustible"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._costo_combustible = Convert.ToDecimal(dr["CostoCombustible"]);
                        this._referencia = dr["Referencia"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_tabla">Tabla (Ubicación: 15)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="costo_combustible">Costo del Combustible</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(byte id_tipo_combustible, int id_tabla, int id_registro, DateTime fecha_inicio, DateTime fecha_fin, 
                                            decimal costo_combustible, string referencia, int id_usuario, bool habilitar)
        {   //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_costo_combustible, id_tipo_combustible, id_tabla, id_registro, fecha_inicio, fecha_fin, costo_combustible, 
                                 referencia, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Costos de Combustible
        /// </summary>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_tabla">Tabla (Ubicación: 15)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="costo_combustible">Costo del Combustible</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCostoCombustible(byte id_tipo_combustible, int id_tabla, int id_registro, DateTime fecha_inicio, DateTime fecha_fin,
                                            decimal costo_combustible, string referencia, int id_usuario)
        {   //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_combustible, id_tabla, id_registro, fecha_inicio, fecha_fin, costo_combustible, 
                                 referencia, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Costos de Combustible
        /// </summary>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="id_tabla">Tabla (Ubicación: 15)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="costo_combustible">Costo del Combustible</param>
        /// <param name="referencia">Referencia</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaCostoCombustible(byte id_tipo_combustible, int id_tabla, int id_registro, DateTime fecha_inicio, DateTime fecha_fin,
                                            decimal costo_combustible, string referencia, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_tipo_combustible, id_tabla, id_registro, fecha_inicio, fecha_fin, costo_combustible,
                                 referencia, id_usuario, this._habilitar);
        }
        /// <summary>
        ///  Método Público encargado de Deshabilitar los Costos de Combustible
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCostoCombustible(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tipo_combustible, this._id_tabla, this._id_registro, this._fecha_inicio, this._fecha_fin, this._costo_combustible,
                                 this._referencia, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Costos de Combustible
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCostoCombustible()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_costo_combustible);
        }
        /// <summary>
        /// Método Público encargado de Obtener el Costo del Combustible por Región
        /// </summary>
        /// <param name="id_ubicacion">Ubicación de la Estación</param>
        /// <param name="fecha_carga">Fecha de Carga del Combustible</param>
        /// <returns></returns>
        public static DataTable ObtieneCostoCombustiblePorRegion(int id_ubicacion, string fecha_carga)
        {   //Declarando Objeto de Retorno
            DataTable dtCosto = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, 0, 0, null, null, 0, "", 0, false, fecha_carga, id_ubicacion.ToString() };
            //Obteniendo Reporte
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtCosto = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtCosto;
        }

        /// <summary>
        /// Método Público encargado de Obtener el Costo del Combustible por Región
        /// </summary>
        /// <param name="id_tabla">Tabla (Ubicación: 15)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="fecha_carga">Fecha de Carga del Combustible</param>
        /// <returns></returns>
        public static string ObtieneCostoCombustibleEntidad(int id_tabla, int id_registro, byte id_tipo_combustible, string fecha_carga)
        {   
            //Declarando Objeto de Retorno
            string costo = "";
            
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_tipo_combustible, id_tabla, id_registro, null, null, 0, "", 0, false, fecha_carga, "" };
            
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                
                    //Obtenemos Costo
                    costo = (from DataRow r in ds.Tables["Table"].Rows
                             select r.Field<string>("PrecioCombustible")).DefaultIfEmpty().FirstOrDefault();
            }
            
            //Devolviendo Resultado Obtenido
            return  costo;
        }

        /// <summary>
        /// Método Público encargado de Obtener los datos de Costos Combustible por Registro
        /// </summary>
        /// <param name="id_tabla">Tabla (Ubicación: 15)</param>
        /// <param name="id_registro">Registro</param>
        /// <param name="id_tipo_combustible">Tipo de Combustible</param>
        /// <param name="fecha_carga">Fecha de Carga del Combustible</param>
        /// <returns></returns>
        public static DataTable ObtieneCostoCombustibleRegistro(int id_tabla, int id_registro)
        {
            //Declarando Objeto de Retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_tabla, id_registro, null, null, 0, "", 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay resultados
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}
