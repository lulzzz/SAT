using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de Gestionar todos los Métodos relacionados con la Carga de Auto Tanques
    /// </summary>
    public class CargaAutoTanque:Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el Estatus de la Carga
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Expresa que la Carga de Autotanque esta Activa
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Expresa que la Carga de Autotanque esta Inactiva
            /// </summary>
            Inactivo
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla CargaAutoTanque
        /// </summary>
        private static string nom_sp = "global.sp_carga_autotanque_tca";

        private int _id_carga_autotanque;
        /// <summary>
        /// Permite identificar el registro de una carga de tanque
        /// </summary>
        public int id_carga_autotanque
        {
            get { return _id_carga_autotanque; }
        }
        private byte _id_estatus;
        /// <summary>
        /// Determina si es estatus de una carga de tanque (Activo / Inactivo)
        /// </summary>
        public byte id_estatus
        {
            get { return _id_estatus; }
        }
        /// <summary>
        /// Permite tener acceso a los elementos de la enumeración Estatus (Activo / Inactivo)
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this._id_estatus; }
        }
        private int _id_ubicacion;
        /// <summary>
        /// Id que identifica el registro de una ubicación realacionada a la carga de tanque.
        /// </summary>
        public int id_ubicacion
        {
            get { return _id_ubicacion; }
        }
        private DateTime _fecha_carga;
        /// <summary>
        /// Almacen la fecha en la que se realizo la accion de carga de tanque.
        /// </summary>
        public DateTime fecha_carga
        {
            get { return _fecha_carga; }
        }
        private decimal _litros_carga;
        /// <summary>
        /// Almacena la cantidad expresada en litros de carga de tanque.
        /// </summary>
        public decimal litros_carga
        {
            get { return _litros_carga; }
        }
        private decimal _sobrante_carga_anterior;
        /// <summary>
        /// Almacena la cantidad de litros sobrantes de una carga de tanque previa.
        /// </summary>
        public decimal sobrante_carga_anterior
        {
            get { return _sobrante_carga_anterior; }
        }
        /// <summary>
        /// Almacen la cantidad de litros de la carga de tanque actual más los litros  sobrantes
        /// </summary>
        private decimal _sobrante_carga_actual;
        /// <summary>
        /// Almacena la cantidad de litros sobrantes de una carga de tanque actual.
        /// </summary>
        public decimal sobrante_carga_actual
        {
            get { return _sobrante_carga_actual; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estado de habilitación del registro (Habilitado / Deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa los atributos a un valor 0.
        /// </summary>
        public CargaAutoTanque()
        {
            this._id_carga_autotanque = 0;
            this._id_estatus = 0;
            this._id_ubicacion = 0;
            this._fecha_carga = DateTime.MinValue;
            this._litros_carga = 0.0m;
            this._sobrante_carga_anterior = 0.0m;
            this._sobrante_carga_actual = 0.0m;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro dado
        /// </summary>
        /// <param name="id_carga_autotanque">Identificador de un registro de CargaAutoTanque</param>
        public CargaAutoTanque(int id_carga_autotanque)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_carga_autotanque);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~CargaAutoTanque()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que realiza la busqueda de registro  y almacena el regustado en los atributos de la clase
        /// </summary>
        /// <param name="id_carga_autotanque">Sirve como identificador de un registro a buscar</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_carga_autotanque)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la consulta a base de datos
            object[] param = { 3, id_carga_autotanque, 0, 0, null, 0.0m, 0.0m, 0.0m, 0, false, "", "" };
            //Instancia al método, y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset DS (que exista o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas y alamcena el resultado del registro en los atributos
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_carga_autotanque = id_carga_autotanque;
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_ubicacion = Convert.ToInt32(r["IdUbicacion"]);
                        this._fecha_carga = Convert.ToDateTime(r["FechaCarga"]);
                        this._litros_carga = Convert.ToDecimal(r["LitrosCarga"]);
                        this._sobrante_carga_anterior = Convert.ToDecimal(r["SobranteCargaAnterior"]);
                        this._sobrante_carga_actual = Convert.ToDecimal(r["SobranteCargaActual"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación del Dataset.
                    retorno = true;
                }
            }
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de CargaAutoTanque
        /// </summary>
        /// <param name="id_estatus">Actualiza el identificador estatus (Activo / Inactivo)</param>
        /// <param name="id_ubicacion">Actualiza el identificador perteneciente a un registro de ubicación </param>
        /// <param name="fecha_carga">Actualiza la fecha de carga de tanque</param>
        /// <param name="litros_carga">Actualiza la cantidad de litros de carga de tanque</param>
        /// <param name="sobrante_carga_anterior">Actualiza la cantidad de litros sobrantes de una carga de tanque previa</param>
        /// <param name="sobrante_carga_actual">Actualiza la cantidad de litros de una carga de tanque actual más el sobrantes</param>
        /// <param name="id_usuario">Actualiza el identificador de un usuario que realiza acciones sobre el registro </param>
        /// <param name="habilitar">Permite cambiar el estado de habilitación de un registro (Habilita / Deshabilita)</param>
        /// <returns></returns>
        private RetornoOperacion editarCargaAutoTanque(byte id_estatus, int id_ubicacion, DateTime fecha_carga, decimal litros_carga, decimal sobrante_carga_anterior, decimal sobrante_carga_actual, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param que almacena los datos necesarios para realizar la actualización de campos de un registro.
            object[] param = { 2, this._id_carga_autotanque, id_estatus, id_ubicacion, fecha_carga, litros_carga, sobrante_carga_anterior, sobrante_carga_actual, id_usuario, habilitar, "", "" };
            //Asigna al objeto retorno el resultado del método EjecutaProcAlmacenadoObjeto().
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }

        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que realiza la inserción de registros a base de datos
        /// </summary>
        /// <param name="id_ubicacion">Inserta el identificador de una ubicación donde se realizo la Carga de Tanque</param>
        /// <param name="fecha_carga">Inserta la fecha de carga de tanque </param>
        /// <param name="litros_carga">Inserta la cantidad de litros de carga de un tanque</param>
        /// <param name="sobrante_carga_anterior">Inserta la cantidad sobrante de una carga previa de tanque</param>
        /// <param name="sobrante_carga_actual">Inserta la cantidad de litros de la carga actual más la sobrante</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la acción de inserción</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCargaAutoTanque(int id_ubicacion, DateTime fecha_carga, decimal litros_carga, decimal sobrante_carga_anterior, decimal sobrante_carga_actual, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarión para realizar la inserción de un registro
            object[] param = { 1, 0, (byte)Estatus.Activo, id_ubicacion, fecha_carga, litros_carga, sobrante_carga_anterior, sobrante_carga_actual, id_usuario, true, "", "" };
            //Asigna Valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro de CargaAutoTanque
        /// </summary>
        /// <param name="id_estatus">Actualiza el identificador estatus (Activo / Inactivo)</param>
        /// <param name="id_ubicacion">Actualiza el identificador perteneciente a un registro de ubicación </param>
        /// <param name="fecha_carga">Actualiza la fecha de carga de tanque</param>
        /// <param name="litros_carga">Actualiza la cantidad de litros de carga de tanque</param>
        /// <param name="sobrante_carga_anterior">Actualiza la cantidad de litros sobrantes de una carga de tanque previa</param>
        /// <param name="sobrante_carga_actual">Actualiza la cantidad de litros de una carga de tanque actual más el sobrantes</param>
        /// <param name="id_usuario">Actualiza el identificador de un usuario que realiza acciones sobre el registro </param>
        /// <returns></returns>
        public RetornoOperacion EditarCargaAutoTanque(byte id_estatus, int id_ubicacion, DateTime fecha_carga, decimal litros_carga, decimal sobrante_carga_anterior, decimal sobrante_carga_actual, int id_usuario)
        {
            //Retorna el resultado al método
            return editarCargaAutoTanque(id_estatus, id_ubicacion, fecha_carga, litros_carga, sobrante_carga_anterior, sobrante_carga_actual, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que deshabilita un registro
        /// </summary>
        /// <param name="id_usuario">Id que identifica al usuario que realizo la acción de deshabilitar el registro.</param>
        /// <returns></returns>
        public RetornoOperacion Deshabilitar(int id_usuario)
        {
            //Retorna el resultado al método
            return editarCargaAutoTanque(this._id_estatus, this._id_ubicacion, this._fecha_carga, this._litros_carga, this._sobrante_carga_anterior, this._sobrante_carga_actual, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Sobrante de la Carga Actual de Diesel
        /// </summary>
        /// <param name="sobrante_carga_actual">Carga Actual</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaSobranteCargaActual(decimal sobrante_carga_actual, int id_usuario)
        {
            //Retorna el resultado al método
            return editarCargaAutoTanque(this._id_estatus, this._id_ubicacion, this._fecha_carga, this._litros_carga, this._sobrante_carga_anterior, sobrante_carga_actual, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualiza el Estatus de la Carga
        /// </summary>
        /// <param name="estatus">Estatus de la Carga</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusCargaActual(Estatus estatus, int id_usuario)
        {
            //Retorna el resultado al método
            return editarCargaAutoTanque((byte)estatus, this._id_ubicacion, this._fecha_carga, this._litros_carga, this._sobrante_carga_anterior, this._sobrante_carga_actual, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de la Carga Autotanque
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCargaAutoTanque()
        {
            //Devolviendo Resultado Obtenido
            return this.cargaAtributos(this._id_carga_autotanque);
        }
        /// <summary>
        /// Método encargado de Obtener la Instancia de la Carga Autotanque Activa para una Ubicación
        /// </summary>
        /// <param name="id_ubicacion">Estación de Combustible</param>
        /// <returns></returns>
        public static CargaAutoTanque ObtieneCargaAutoTanqueActiva(int id_ubicacion, DateTime fecha_carga)
        {
            //Declarando Objeto de Retorno
            CargaAutoTanque carga = new CargaAutoTanque();

            //Creación del arreglo que almacena los datos necesarión para realizar la inserción de un registro
            object[] param = { 4, 0, 0, id_ubicacion, fecha_carga, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Instancia al método, y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset DS (que exista o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas y alamcena el resultado del registro en los atributos
                    foreach (DataRow dr in DS.Tables["Table"].Rows)
                    {
                        //Instanciando Carga Autotanque
                        carga = new CargaAutoTanque(Convert.ToInt32(dr["Id"]));

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return carga;
        }
        /// <summary>
        /// Método encargado de Obtener la Carga Anterior dada una Ubicación
        /// </summary>
        /// <param name="id_ubicacion">Ubicación</param>
        /// <returns></returns>
        public static CargaAutoTanque ObtieneCargaAnteriorUbicacion(int id_ubicacion)
        {
            //Declarando Objeto de Retorno
            CargaAutoTanque carga_anterior = new CargaAutoTanque();

            //Creación del arreglo que almacena los datos necesarión para realizar la inserción de un registro
            object[] param = { 5, 0, 0, id_ubicacion, null, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Instancia al método, y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset DS (que exista o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas y alamcena el resultado del registro en los atributos
                    foreach (DataRow dr in DS.Tables["Table"].Rows)
                    {
                        //Instanciando Carga Autotanque
                        carga_anterior = new CargaAutoTanque(Convert.ToInt32(dr["Id"]));

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return carga_anterior;
        }
        /// <summary>
        /// Método encargado de Obtener las Cargas Anteriores dada una Ubicaciones
        /// </summary>
        /// <param name="id_ubicacion">Ubicaciones</param>
        /// <returns></returns>
        public static DataTable ObtieneCargasAnterioresUbicacion(int id_ubicacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtCargasAnteriores = null;

            //Creación del arreglo que almacena los datos necesarión para realizar la inserción de un registro
            object[] param = { 6, 0, 0, id_ubicacion, null, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Instancia al método, y almacena el resultado en el dataset DS
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset DS (que exista o sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asignando Cargas Anteriores
                    dtCargasAnteriores = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCargasAnteriores;
        }

        #endregion
    }
}
