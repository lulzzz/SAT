using System;
using System.Data;
using TSDK.Base;
using System.Transactions;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Clase que permite manipular los datos de la tabla Actividad (Consultar, Insertar, Editar)
    /// </summary>
    public class Actividad:Disposable
    {
        #region Enumeración
        /// <summary>
        /// Enumeración de las partes de transporte por las cuales se inicia una actividad de mantenimiento
        /// </summary>
        public enum Familia
        {
            /// <summary>
            /// Actividad de mantenimiento de Suspensiones.
            /// </summary>
            Suspension = 1,
            /// <summary>
            /// Actividad de mantenimietno de Dirección.
            /// </summary>
            Direccion = 2,
            /// <summary>
            /// Actividad de mantenimiento de Cabina.
            /// </summary>
            Cabina = 3
        }
        /// <summary>
        /// Enumeración acorde a enumeración Familia, especifica una actividad de mantenimiento
        /// </summary>
        public enum SubFamilia
        {
            /// <summary>
            /// Frenos
            /// </summary>
            Frenos = 1
        }
        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de la clase actividad
        /// </summary>
        private static string nom_sp = "mantenimiento.sp_actividad_ta";
        private int _id_actividad;
        /// <summary>
        /// Id que permite identificar el registro de una actividad de mantenimiento
        /// </summary>
        public int id_actividad
        {
            get { return _id_actividad; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Identificador de una compañia a la cual pertenece el registro de una actividad de mantenimiento
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private string _descripcion;
        /// <summary>
        /// Almacena las caracteristicas con las que se identifica una actividad de mantenimiento
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private byte _id_familia;
        /// <summary>
        /// Identificador que clasifica las actividades de mantenimieto de una unidad de trasnporte (suspensión, dirección,cabina)
        /// </summary>
        public byte id_familia
        {
            get { return _id_familia; }
        }
        /// <summary>
        /// Permite tener acceso a los elementos de la enumeración Familia.
        /// </summary>
        public Familia familia
        {
            get { return (Familia)this._id_familia; }
        }
        private byte _id_sub_familia;
        /// <summary>
        /// Id que determina la actividad de mantenimiento de una unidad de transporte acorde a la clasificacion de la actividad (id_familia)
        /// </summary>
        public byte id_sub_familia
        {
            get { return _id_sub_familia; }
        }
        /// <summary>
        /// Permite tener acceso a los elementos de la enumeración subFamilia
        /// </summary>
        public SubFamilia subFamilia
        {
            get { return (SubFamilia)this._id_sub_familia; }
        }
        private int _id_tipo_unidad;
        /// <summary>
        /// Almacena el tipo de unidad a la cual se realizara la actividad de mantenimiento (dolly, caja, tractor, etc.)
        /// </summary>
        public int id_tipo_unidad
        {
            get { return _id_tipo_unidad; }
        }
        private int _id_sub_tipo_unidad;
        /// <summary>
        /// Almacena el identificador de un subtipo de unidad acorde al tipo de uniad.
        /// </summary>
        public int id_sub_tipo_unidad
        {
            get { return _id_sub_tipo_unidad; }
        }
        private int _id_requisicion;
        /// <summary>
        /// Almacena el identificador del registro de una requisición
        /// </summary>
        public int id_requisicion
        {
            get { return _id_requisicion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estado de habilitación de un registro (Habilitado / Deshabilitado)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor default de la clase
        /// </summary>
        public Actividad()
        {
            this._id_actividad = 0;
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._id_familia = 0;
            this._id_sub_familia = 0;
            this._id_tipo_unidad = 0;
            this._id_sub_tipo_unidad = 0;
            this._id_requisicion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro 
        /// </summary>
        /// <param name="id_actividad">Identificador de una actividad</param>
        public Actividad(int id_actividad)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_actividad);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Actividad()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros, y almacena el resultado en los atributos
        /// </summary>
        /// <param name="id_actividad">Identiicador que sirve como referencia para la busqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_actividad)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el store procedure
            object[] param = { 3, id_actividad, 0, "", 0, 0, 0, 0, 0, 0, false, "", "" };
            //Crea un dataset y almacena el resultado del método EjecutaProcAlmacenadoDataSet 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan datos en el dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas del registro encontrado y almacena el resultado en cada actributo
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_actividad = id_actividad;
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompania"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._id_familia = Convert.ToByte(r["IdFamilia"]);
                        this._id_sub_familia = Convert.ToByte(r["IdSubFamilia"]);
                        this._id_tipo_unidad = Convert.ToInt32(r["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToInt32(r["IdSubTipoUnidad"]);
                        this._id_requisicion = Convert.ToInt32(r["IdRequisicion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorno el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el campo que identifica a una compañia</param>
        /// <param name="descripcion">Actualiza el campo que describe una actividad de mantenimiento</param>
        /// <param name="id_familia">Actualiza el campo del identificador de familia (suspensión, dirección,cabina)</param>
        /// <param name="id_sub_familia">Actualiza el campo del identificador de subfamilia (acorde a id_familia)</param>
        /// <param name="id_tipo_unidad">Actualiza el campo de tipo de unidad (dolly, caja, tractor, etc.)</param>
        /// <param name="id_sub_tipo_unidad">Actualiz el campo sub tipo de unidad dependiendo del tipo de unidad</param>
        /// <param name="id_requisicion">Actualiza el campo del identificador de una requisición</param>
        /// <param name="id_usuario">Actualiza el campo con el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el campo de habilitar con el estado de habilitacion del registro(Habilitado / deshabilitado)</param>
        /// <returns></returns>
        private RetornoOperacion editaActividad(int id_compania_emisor, string descripcion, Familia id_familia, SubFamilia id_sub_familia,
                                                int id_tipo_unidad, int id_sub_tipo_unidad, int id_requisicion, int id_usuario, bool habilitar)
        {
            //Crea el objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creacion del arreglo param
            object[] param = { 2, this._id_actividad, id_compania_emisor, descripcion, (byte)id_familia, (byte)id_sub_familia, id_tipo_unidad, id_sub_tipo_unidad, id_requisicion, id_usuario, habilitar, "", "" };
            //Asigan valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna el resultado al método
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que realiza la inserción de registro de actividad.
        /// </summary>
        /// <param name="id_compania_emisor">Inserta el identificador de una compañia</param>
        /// <param name="descripcion">Inserta la descripción de una actividad</param>
        /// <param name="id_familia">Inserta el identificador de familia (suspensión, dirección,cabina)</param>
        /// <param name="id_sub_familia">Inserta el identificador de subfamilia (acorde a id_familia)</param>
        /// <param name="id_tipo_unidad">Inserta el tipo de unidad (caja, tractor, dolly)</param>
        /// <param name="id_sub_tipo_unidad">Inserta el subtipo de unidad acorde al tipo de unidad</param>
        /// <param name="id_requisicion">Inserta el identificador de una requisición</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarActividad(int id_compania_emisor, string descripcion, Familia id_familia, SubFamilia id_sub_familia,
                                                         int id_tipo_unidad, int id_sub_tipo_unidad, int id_requisicion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo param
            object[] param = { 1, 0, id_compania_emisor, descripcion, (byte)id_familia, (byte)id_sub_familia, id_tipo_unidad, id_sub_tipo_unidad, id_requisicion, id_usuario, true, "", "" };
            //Asigna valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno el resultado al objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de un registro
        /// </summary>
        /// <param name="id_compania_emisor">Actualiza el campo que identifica a una compañia</param>
        /// <param name="descripcion">Actualiza el campo que describe una actividad de mantenimiento</param>
        /// <param name="id_familia">Actualiza el campo del identificador de familia (suspensión, dirección,cabina)</param>
        /// <param name="id_sub_familia">Actualiza el campo del identificador de subfamilia (acorde a id_familia)</param>
        /// <param name="id_tipo_unidad">Actualiza el campo de tipo de unidad (dolly, caja, tractor, etc.)</param>
        /// <param name="id_sub_tipo_unidad">Actualiz el campo sub tipo de unidad dependiendo del tipo de unidad</param>
        /// <param name="id_requisicion">Actualiza el campo del identificador de una requisición</param>
        /// <param name="id_usuario">Actualiza el campo con el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarActividad(int id_compania_emisor, string descripcion, Familia id_familia, SubFamilia id_sub_familia,
                                                int id_tipo_unidad, int id_sub_tipo_unidad, int id_requisicion, int id_usuario)
        {
            //Retorna el resultado del método editarActividad
            return editaActividad(id_compania_emisor, descripcion, id_familia, id_sub_familia, id_tipo_unidad, id_sub_tipo_unidad, id_requisicion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de habilitación de un registro
        /// </summary>
        /// <param name="id_usuario">Identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarActividad(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicia la transacción
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Asignación de valores al objeto retorno
                retorno = editaActividad(this._id_compania_emisor, this._descripcion, (Familia)this._id_familia, (SubFamilia)this._id_sub_familia, this._id_tipo_unidad, this._id_sub_tipo_unidad, this._id_requisicion, id_usuario, false);
                //Valida la operación exitosa del deshabilitar registro
                if (retorno.OperacionExitosa)
                {
                    //Instancia a la clase Actividad Puesto y obtiene los detalles
                    using (DataTable dtActvidadPuesto = ActividadPuesto.CargaPuestos(this._id_actividad))
                    {
                        //Valida los datos del datatable
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtActvidadPuesto))
                        {
                            //Recorre los registros
                            foreach (DataRow r in dtActvidadPuesto.Rows)
                            {
                                //Instancia a Actividad Puesto
                                using(ActividadPuesto actPuesto = new ActividadPuesto(Convert.ToInt32(r["Id"])))
                                {
                                    //Asigna a retorno el resultado de invocar a l métodoDeshabilitarActividadRegistro
                                    retorno= actPuesto.DeshabilitarActividadRegistro(id_usuario);
                                    //Valida que existan los detalles acorde a la actividad
                                    if (!retorno.OperacionExitosa)
                                        break;                                    
                                }
                            }
                        }
                    }
                }
                //Valida la transaccion
                if (retorno.OperacionExitosa)
                    //Invoca al método Complete
                    trans.Complete();
            }
            //Retorna el resultado al método.
            return retorno;
        }
        #endregion
    }
}
