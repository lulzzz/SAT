using System;
using System.Data;
using TSDK.Base;


namespace SAT_CL.Global
{
    /// <summary>
    /// 
    /// </summary>
    public class OperadorUnidad :Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo nom_sp al que se le asigna el nombre del Store Procedure de la tabla
        /// </summary>
        private static string nom_sp = "global.sp_asignacion_operador_unidad_taou";

        private int _id_asignacion_operador_unidad;
        /// <summary>
        /// Id que corresponde a asignación operdor unidad
        /// </summary>
        public int id_asignacion_operador_unidad
        {
            get { return _id_asignacion_operador_unidad; }
        }
 
        private int _id_operador;
        /// <summary>
        /// Id que corersponde al operador
        /// </summary>
        public int id_operador
        {
            get { return _id_operador; }
        }
 
        private int _id_unidad;
        /// <summary>
        /// Id que corresponde a la unidad
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
  
        private DateTime _fecha_inicio;
        /// <summary>
        /// Fecha que corresponde a la fecha de inicio
        /// </summary>
        public DateTime  fecha_inicio
        {
            get { return _fecha_inicio; }
        }

        private DateTime _fecha_fin;
        /// <summary>
        /// Fecha que corresponde a la fecha fín
        /// </summary>
        public DateTime fecha_fin
        {
            get { return _fecha_fin; }
        }
   
        private bool _habilitar;
        /// <summary>
        /// Obtiene el Estatus de Habilitación del Registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por default que inicializa los atributos
        /// </summary>
        public OperadorUnidad()
        {
            this._id_asignacion_operador_unidad = 0;
            this._id_operador = 0;
            this._id_unidad = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que asigna valores a los atributos dado un id
        /// </summary>
        /// <param name="id_asignacion_operador_unidad">Id que permite realizar la busqueda de registros operador unidad</param>
        public OperadorUnidad(int id_asignacion_operador_unidad) 
        {
            //Invocación del método privado carga atributos
            cargaAtributosInstancia(id_asignacion_operador_unidad);
                    
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~OperadorUnidad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privado

        /// <summary>
        /// Método privado que permite cargar los atributos registros de operador unidad
        /// </summary>
        /// <param name="id_asignacion_operador_unidad">Id que permite realizar la búsqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_asignacion_operador_unidad)
        {
            //Declaración del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores  al arerglo, necesarios para el SP de la tabla
            object[] param = {3, id_asignacion_operador_unidad, 0, 0, "NULL","NULL",0,false,"","" };
            //Invoca al Store procedure para iniciar la búsqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos si existen o sean nulos
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas del Dataset, y los valores son almacenados en r
                    foreach(DataRow r in DS.Tables[0].Rows)
                    {
                        _id_asignacion_operador_unidad = id_asignacion_operador_unidad;
                        _id_operador = Convert.ToInt32(r["IdOperador"]);
                        _id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        _fecha_inicio = Convert.ToDateTime(r["FechaInicio"]);
                        _fecha_fin = Convert.ToDateTime(r["FechaFin"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                    }
                    //Cambio del valor al objeto solo si se cumplen las sentencias de validación de datos
                    retorno = true;
                }
            }
            //Retorno del resultado del método
            return retorno;
        }

        /// <summary>
        /// Método que permite actualizar registros de Operador Unidad
        /// </summary>
        /// <param name="id_operador">Permite actualizar el campo id_operador del operador unidad</param>
        /// <param name="id_unidad">Permite actualizar el campo id_unidad operador unidad</param>
        /// <param name="fecha_inicio">Permite actualizar el campo fecha_inicio operador unidad</param>
        /// <param name="fecha_fin">Permite actualizar el campo fecha_fin operador unidad</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario operador unidad</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar operador unidad</param>
        /// <returns></returns>
        private RetornoOperacion editarOperadorUnidad(int id_operador, int id_unidad,DateTime fecha_inicio, DateTime fecha_fin, int id_usuario, bool habilitar )
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo, necesarios para el SP de la tabla
            object[] param = { 2, this.id_asignacion_operador_unidad, id_operador, id_unidad, fecha_inicio, fecha_fin, id_usuario, habilitar, "", "" };
            //Asignación al objeto retorno los valores para el SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }
      
        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Método que me permite Insertar Registros a Operador Unidad
        /// </summary>
        /// <param name="id_operador">Permite Insertar en el campo id_operador de operador unidad</param>
        /// <param name="id_unidad">Permite Insertar en el campo id_unidad de operador unidad</param>
        /// <param name="fecha_inicio">Permite Insertar en el campo fecha_inicio de operador unidad</param>
        /// <param name="fecha_fin">Permite Insertar en el campo fecha_fin de operador unidad</param>
        /// <param name="id_usuario">Permite Insertar en el campo id_usuario de operador unidad</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarOperadorUnidad(int id_operador, int id_unidad, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arerglos, necesarios para el SP de la tabla
            object[] param = {1, 0, id_operador,id_unidad,fecha_inicio,fecha_fin,id_usuario, true,"",""};
            //Asignación al objeto los valores requeridos para el Sp de la tabla
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado para el método
            return retorno;
        }

        /// <summary>
        /// Método que me permite Actualizar registros de Operador Unidad
        /// </summary>
        /// <param name="id_operador">Permite actualizar el campo id_operador</param>
        /// <param name="id_unidad">Permite actualizar el campo id_unidad</param>
        /// <param name="fecha_inicio">Permite actualizar el campo fecha_inicio</param>
        /// <param name="fecha_fin">Permite actualizar el campo fecha_fin</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarOperadorUnidad(int id_operador, int id_unidad, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario) 
        {
            // Invoca y Retorna el resultado del método privado edita Operador Unidad
            return this.editarOperadorUnidad(id_operador, id_unidad, fecha_inicio, fecha_fin, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que deshabilitar un registro de Operador Unidad
        /// </summary>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario de Operador Unidad</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaOperadorUnidad(int id_usuario)
        {
             // Invoca y Retorna el resultado del método privado edita Operador Unidad
             return this.editarOperadorUnidad(this.id_operador, this.id_unidad, this.fecha_inicio, this.fecha_fin, id_usuario, false);
        }

        #endregion
    }
}
