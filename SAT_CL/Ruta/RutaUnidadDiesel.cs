using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que realiza acciones sobre el ruta unidad diesel (Inserción, Edición y consulta de registros)
    /// </summary>
    public class RutaUnidadDiesel:Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumera el tipo de operación (pago) del diesel
        /// </summary>
        public enum TipoOperacion
        {
            /// <summary>
            /// Cuando el pago de diesel se hace en efectivo
            /// </summary>
            Efectivo = 1,
            /// <summary>
            /// Cualdo se utiliza un vale de diesel
            /// </summary>
            Vale
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla Ruta Unidad Diesel
        /// </summary>
        private static string nom_sp = "ruta.sp_ruta_unidad_diesel_trud";
        private int _id_ruta_unidad_diesel;
        /// <summary>
        /// Identificador de la ruta unidad diesel
        /// </summary>
        public int id_ruta_unidad_diesel
        {
            get { return _id_ruta_unidad_diesel; }
        }
        private int _id_ruta_tipo_unidad;
        /// <summary>
        /// Identificador de un tipo de unidad (tractor, caja, pipa,etc.)
        /// </summary>
        public int id_ruta_tipo_unidad
        {
            get { return _id_ruta_tipo_unidad; }
        }
        private int _secuencia;
        /// <summary>
        /// Identificador Secuencia
        /// </summary>
        public int secuencia
        {
            get { return _secuencia; }
        }
        private byte _id_tipo_operacion;
        /// <summary>
        /// Identificador de tipo de operación (Efectivo o Vale de diesel)
        /// </summary>
        public byte id_tipo_operacion
        {
            get { return _id_tipo_operacion; }
        }
        /// <summary>
        /// Permite el acceso a la enumeración Tipo Operación
        /// </summary>
        public TipoOperacion tipoOperacion
        {
            get { return (TipoOperacion)this._id_tipo_operacion; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identificador Entidad
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Identificador Registro
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private bool _habilitar;
        /// <summary>
        /// Estado de uso del registro (Habilitar-Disponible,Deshabilitar-NoDisponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que inicializa los atributos de la clase en cero
        /// </summary>
        public RutaUnidadDiesel()
        {
            this._id_ruta_unidad_diesel = 0;
            this._id_ruta_tipo_unidad = 0;
            this._secuencia = 0;
            this._id_tipo_operacion = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro ruta unidad diesel
        /// </summary>
        /// <param name="id_ruta_unidad_diesel">Sirve como referencia para la asignación de valores a los atributos</param>
        public RutaUnidadDiesel(int id_ruta_unidad_diesel)
        {
            //Invoca el método que realiza la asignación de valores a los atributos
            cargaAtributos(id_ruta_unidad_diesel);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~RutaUnidadDiesel()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros y el resultado lo asigna a los atributos de la clase
        /// </summary>
        /// <param name="id_ruta_unidad_diesel">Identificador que sirve como referencia para la busqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ruta_unidad_diesel)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del areglo que almacena los datos necesarios para realizar la consulta de registro RutaUnidad Diesel
            object[] param = { 3, id_ruta_unidad_diesel, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Realiza la busqueda del registro ruta unidad diesel
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida el dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del dataset y el resultado lo almacena en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ruta_unidad_diesel = id_ruta_unidad_diesel;
                        this._id_ruta_tipo_unidad = Convert.ToInt32(r["IdRutaTipoUnidad"]);
                        this._secuencia = Convert.ToInt32(r["Secuencia"]);
                        this._id_tipo_operacion = Convert.ToByte(r["IdTipoOperacion"]);
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro RutaUnidadDiesel
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Actualiza el identificador de un tipo de unidad (tractor, pipa,caja,etfc.).</param>
        /// <param name="litros">Actualiza los litros asignados a la unidad</param>
        /// <param name="tipo_operacion">Actualiza el tipo de pago de diesel (Efectivo o Vale)</param>
        /// <param name="id_ubicacion_estacion_combustible">Actualiza la ubicación de la estación de combustible donde se hizo la recarga</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo actualizaciones sobre registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible, Deshabilitado- No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarRutaUnidadDiesel(int id_ruta_tipo_unidad, TipoOperacion tipo_operacion, int id_tabla, int id_registro, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actualización
            object[] param = { 2, this._id_ruta_unidad_diesel, id_ruta_tipo_unidad, this._secuencia, (TipoOperacion)tipo_operacion, id_tabla, id_registro, id_usuario, habilitar, "", "" };
            //Realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que inserta un registro RutaUnidadDiesel
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Inserta el identificador de un tipo de unidad (tractor, pipa,caja,etfc.).</param>
        /// <param name="litros">Inserta los litros asignados a la unidad</param>
        /// <param name="tipo_operacion">Inserta el tipo de pago del diesel (Efectivo o Vale)</param>
        /// <param name="id_ubicacion_estacion_combustible">Inserta la ubicación de la estación de combustible donde se hizo la recarga</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo actualizaciones sobre registro</param>
        /// <param name="habilitar">Inserta el estado de uso del registro (Habilitado-Disponible, Deshabilitado- No Disponible)</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarRutaUnidadDiesel(int id_ruta_tipo_unidad, TipoOperacion tipo_operacion, int id_tabla, int id_registro, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la inserción del registro
            object[] param = { 1, 0, id_ruta_tipo_unidad, 0, (TipoOperacion)tipo_operacion, id_tabla, id_registro, id_usuario, true, "", "" };
            //Realiza la inserción del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro RutaUnidadDiesel
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Actualiza el identificador de un tipo de unidad (tractor, pipa,caja,etfc.).</param>
        /// <param name="litros">Actualiza los litros asignados a la unidad</param>
        /// <param name="tipo_operacion">Actualiza el tipo de pago del diesel (Efectivo o Vale)</param>
        /// <param name="id_ubicacion_estacion_combustible">Actualiza la ubicación de la estación de combustible donde se hizo la recarga</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo actualizaciones sobre registro</param>        
        /// <returns></returns>
        public RetornoOperacion EditarRutaUnidadDiesel(int id_ruta_tipo_unidad, TipoOperacion tipo_operacion, int id_tabla, int id_registro, int id_usuario)
        {
            //Retorna al método el resultado del método que actualiza los registros.
            return this.editarRutaUnidadDiesel(id_ruta_tipo_unidad, (TipoOperacion)tipo_operacion, id_tabla, id_registro, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que Cambia el estado de uso del registro (Habilitado a Deshabilitado)
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaRutaUnidadDiesel(int id_usuario)
        {
            //Retorna al método el resultado del método que actualiza los registros.
            return this.editarRutaUnidadDiesel(this._id_ruta_tipo_unidad, (TipoOperacion)this._id_tipo_operacion, this._id_tabla, this._id_registro, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRutaUnidadDiesel()
        {
            //Invoca al método que asigna valores a los atributos de la clase
            return this.cargaAtributos(this._id_ruta_unidad_diesel);
        }

        /// <summary>
        /// Carga las Vales
        /// </summary>
        /// <param name="id_ruta_tipo_unidad">Id Ruta Tipo de Unidad</param>
        /// <returns></returns>
        public static DataTable CargaVales(int id_ruta_tipo_unidad)
        {
            //Creación del areglo que almacena los datos necesarios para realizar la consulta de registro RutaUnidad Diesel
            object[] param = { 4, 0, id_ruta_tipo_unidad, 0, 0, 0, 0, 0, false, "", "" }; 

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        #endregion
    }
}
