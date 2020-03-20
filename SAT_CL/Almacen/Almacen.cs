using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Almacen
{
    /// <summary>
    /// Clase encargado de Todas las Operaciones relacionadas con los Almacenes
    /// </summary>
    public class Almacen : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// 
        /// </summary>
        public enum TipoAlmacen
        {
            /// <summary>
            /// Interno
            /// </summary>
            Interno = 1,
            /// <summary>
            /// Externo
            /// </summary>
            Externo
        }
        /// <summary>
        /// 
        /// </summary>
        public enum EstatusAlmacen
        {
            /// <summary>
            /// Conductor activo
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Conductor inactivo
            /// </summary>
            Inactivo
        }

        #endregion

        #region Propiedades y atributos        
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_procedimiento_almacenado = "almacen.sp_almacen_tal";
      
        private int _id_almacen;
        private int _id_compania_emisora;
        private string _descripcion;
        private int _id_ubicacion;
        private int _id_estatus;
        private int _id_tipo;
        
        private int _id_usuario;
        private string _dir_desc;
        private bool _habilitar;
        
        /// <summary>
        /// Obtiene el Id de Almacén
        /// </summary>
        public int id_almacen { get { return this._id_almacen; } }
        /// <summary>
        /// Obtiene la Compania Emisora del almacén
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        /// <summary>
        /// Obtiene la descripción del almacén
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        /// <summary>
        /// Obtiene la ubicacion del almacén
        /// </summary>
        public int id_ubicacion { get { return this._id_ubicacion; } }
        /// <summary>
        /// Obtiene el estatus del almacén
        /// </summary>
        public int id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Obtiene el estatus del almacén
        /// </summary>
        public EstatusAlmacen estatus { get { return (EstatusAlmacen)this._id_estatus; } }
        /// <summary>
        /// Obtiene el tipo del almacén
        /// </summary>
        public int id_tipo { get { return this._id_tipo; } }
        /// <summary>
        /// Obtiene el tipo del almacén
        /// </summary>
        public TipoAlmacen tipo { get { return (TipoAlmacen)this._id_tipo; } }

       
        /// <summary>
        /// Obtiene El usuario del almacén
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }
        /// <summary>
        /// Obtiene la descripción del almacén
        /// </summary>
        public string dirdesc { get { return this._dir_desc; } }       
        /// <summary>
        /// Obtiene el valor de habilitación de registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor default
        /// </summary>
        public Almacen()
        {
            //Asignando Valores
            this._id_almacen = 0;
            this._id_compania_emisora = 0;
            this._descripcion = "";
            this._id_ubicacion = 0;
            this._id_estatus = 0;
            this._id_tipo = 0;
            this._id_usuario = 0;
            this._dir_desc = "";
            this._habilitar = false;    
        }
        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id del almacen
        /// </summary>
        /// <param name="idAlmacen"></param>
        public Almacen(int idAlmacen)
        {
            //Inicializamos el arreglo de parametros                               
            object[] param = { 3, idAlmacen, 0, "", 0, 0, 0, 0, false, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_almacen = Convert.ToInt32(r["IdAlmacen"]);
                        this._id_compania_emisora = Convert.ToInt32(r["IdCompania"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._id_ubicacion = Convert.ToInt32(r["IdUbicacion"]);
                        this._id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        this._id_tipo = Convert.ToInt32(r["IdTipo"]);
                        this._dir_desc = r["DirDesc"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]); 
                    }
                }
            }
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~Almacen()
        {
            Dispose(false);
        }
        #endregion

        #region Metodos privados
        /// <summary>
        /// Carga los valores de registro sobre atributos de instancia
        /// </summary>
        /// <param name="id_compania_emisora"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>     
        /// <returns></returns>
        private RetornoOperacion editaAlmacen(int id_compania_emisora, string descripcion, int id_ubicacion, EstatusAlmacen id_estatus, 
                                                  TipoAlmacen id_tipo, int id_usuario, bool habilitar)
        {
            //Incializando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = { 2, this.id_almacen, id_compania_emisora, descripcion, id_ubicacion,(int) estatus, (int) id_tipo, id_usuario,
                               habilitar,  "", "",  };
            //Realizando la actualización solicitada
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param);
            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Metodos Publicos (interfaz)
        /// <summary>
        /// Inserta un nuevo registro Almacen
        /// </summary>       
        /// <param name="id_compania_emisora"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_usuario"></param>                  
        /// <returns></returns>
        public static RetornoOperacion InsertaAlmacen(int id_compania_emisora, string descripcion, int id_ubicacion, EstatusAlmacen id_estatus,
                                                          TipoAlmacen id_tipo, int id_usuario)
        {
            //Incializando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializamos el arreglo de parametros
            object[] param = { 1, 0, id_compania_emisora, descripcion, id_ubicacion, id_estatus, id_tipo, id_usuario, true,  "", ""};
            //Realizando la actualización solicitada
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_procedimiento_almacenado, param);
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de los registros Almacen
        /// </summary>
        /// <param name="id_almacen"></param>
        /// <param name="descripcion"></param>
        /// <param name="id_ubicacion"></param>
        /// <param name="id_estatus"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAlmacen(int id_compania_emisora, string descripcion, int id_ubicacion, EstatusAlmacen id_estatus,
                                                 TipoAlmacen id_tipo, int id_usuario)
        {
            //Devolviendo resultado
            return this.editaAlmacen(id_compania_emisora, descripcion, id_ubicacion, id_estatus, id_tipo, id_usuario, true);
        }
        /// <summary>
        /// Deshabilita un registro Almacen
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAlmacen(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Devolviendo resultado
            resultado = editaAlmacen(this._id_compania_emisora, this.descripcion, this.id_ubicacion, this.estatus, this.tipo , this.id_usuario, false);            
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// da ubicacion de Almacen
        /// </summary>   
        public static string UbicacionAlmacen(int Almacen)
        {
            //Asignando arreglo
            object[] param = { 4, Almacen, 0, "", 0, 0, 0, 0, false, "", "" };
            string retorno = "";

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_procedimiento_almacenado, param))
            {
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        retorno = r[0].ToString();
                }
            }

            //Ejecutando el SP
            return retorno;
        }
        /*// <summary>
        /// Metodo encargado de Deshabilitar Almacen
        /// </summary>
        /// <param name="id_almacen"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAlmacen(int id_almacen, int id_usuario)
        {
            //Declarando objeto retorno
            RetornoOperacion resultado = new RetornoOperacion(0);


            //Inicializamos Transaccion 
            SqlTransaction Transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.ReadCommitted);

            //Deshabilitamos Compañias
            resultado = AlmacenCompania.DeshabilitaRegistroAlmacenCompania(id_almacen, id_usuario, Transaccion);

            //En caso de no existir errores
            if (resultado.OperacionExitosa)
            {
                //Desahabilitamo Proveedor
                resultado = editaAlmacen(this._id_almacen, this._descripcion, this._id_ubicacion, (EstatusAlmacen)this._id_estatus, (TipoAlmacen)this._id_tipo, this._id_UEN, id_usuario, false, Transaccion);
            }
            //Finalizando Transaccion
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(Transaccion, resultado.OperacionExitosa);

            return resultado;
        }*/

        #endregion
    }
}
