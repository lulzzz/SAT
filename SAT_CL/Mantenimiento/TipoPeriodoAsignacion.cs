using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los tipos de periodos de las asignaciones
    /// </summary>
    public class TipoPeriodoAsignacion : Disposable
    {
        #region Enumeraciones


        #endregion

        #region Propiedades y atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        public static string nombre_store_procedure = "mantenimiento.sp_tipo_periodo_asignacion_tpa";

        private int _id_tipo_interrupcion;
        private string _descripcion;
        private int _signo;
        private bool _habilitar;

        public int id_tipo_interrupcion { get { return _id_tipo_interrupcion; } }
        public string descripcion { get { return _descripcion; } }
        public int signo { get { return _signo; } }
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public TipoPeriodoAsignacion()
        {
            _id_tipo_interrupcion = 0;
            _descripcion = "";
            _signo = 0;
            _habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa el objeto tipo interrupcion en razon a un id de interrupcion
        /// </summary>
        /// <param name="IdTipoInterrupcion"></param>
        public TipoPeriodoAsignacion(int IdTipoInterrupcion)
        {
            //Inicializamos el arreglo de parametros
            object[] param = { 3, IdTipoInterrupcion, "", 0, 0, true, "", "" };

            //Realizamos la consulta 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_store_procedure, param))
            {
                //Verificamos que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorremos cada uno de los registros 
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_tipo_interrupcion = Convert.ToInt32(r["IdTipo"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._signo = Convert.ToInt32(r["Signo"]);
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
        ~TipoPeriodoAsignacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Metodo encargado de editar un registro de tipo interrupcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="signo"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaRegistroTipoInterrupcion(string descripcion, int signo, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_tipo_interrupcion, descripcion, signo, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }

        #endregion

        #region Metodos publicos (Interfaz)

        /// <summary>
        /// Metodo encargado de insertar un tipo de interrupcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="signo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoInterrupcion(string descripcion, int signo, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, descripcion, signo, id_usuario, true, "", "" };

            //Realizamos la inserción del registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_store_procedure, param);
        }
        /// <summary>
        /// Metodo encargado de editar un registro tipo interrupcion
        /// </summary>
        /// <param name="descripcion"></param>
        /// <param name="signo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaRegistroTipoInterrupcion(string descripcion, int signo, int id_usuario)
        {
            return this.editaRegistroTipoInterrupcion(descripcion, signo, id_usuario, this.habilitar);

        }
        /// <summary>
        /// Metodo encargado de deshabilitar un registro tipo interrupcion
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoInterrupcion(int id_usuario)
        {
            return this.editaRegistroTipoInterrupcion(this.descripcion, this.signo, id_usuario, false);
        }

        #endregion
    }
}
