using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Calificacion
{
    /// <summary>
    /// Clase que permite realizar Inserción, Edición y Consultas a los registros pertenecientes a una tabla Calificación Concepto
    /// </summary>
    public class CalificacionConcepto :Disposable
    {

        #region Atributos
        /// <summary>
        /// Almacena el nombre del sp de la tabla Calificación Concepto
        /// </summary>
        private static string nom_sp = "calificacion.sp_calificacion_concepto_tcc";
        private int _id_calificacion_concepto;
        /// <summary>
        /// Identifica el concepto por el cual se va a calificar a un Cliente, Operador o Transportista
        /// </summary>
        public int id_calificacion_concepto
        {
            get { return _id_calificacion_concepto; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identifica a la entidad a la que pertenece el registro a calificar (si es un Cliente, Operador o Transportista)
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Identifica la compañia a la que pertenecera el concepto a calificar
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private string _descripcion;
        /// <summary>
        /// Nombre o caracteristicas de identifican un concepto a evaluar
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Almacena el estado de habilitación de un registro
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos a cero
        /// </summary>
        public CalificacionConcepto()
        {
            this._id_calificacion_concepto = 0;
            this._id_tabla = 0;
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de Calificación Concepto
        /// </summary>
        /// <param name="id_calificacion_concepto"></param>
        public CalificacionConcepto(int id_calificacion_concepto)
        {
            //Invoca al método que busca y asigna  valores a los atributos de la case
            cargaAtributos(id_calificacion_concepto);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~CalificacionConcepto()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro y los campos los almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_calificacion_concepto">Id que sirve como referencia para la busqueda del registro de una Calificación Concepto</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_calificacion_concepto)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda del registro
            object[] param = { 3, id_calificacion_concepto, 0, 0, "", 0, false, "", "" };
            //Instancia a la clase que realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna los campos de las filas del datase a los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_calificacion_concepto = id_calificacion_concepto;
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompania"]);
                        this._descripcion = Convert.ToString(r["Descripcion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los datos de un Concepto Calificación
        /// </summary>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a la que pertenece el concepto de calificación (Cliente,Operador o Transportista)</param>
        /// <param name="id_compania_emisor">Actualiza el identificador de la compañia a la que pertenece un concepto de calificación</param>
        /// <param name="descripcion">Actualiza el nombre del concepto a calificar</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible,Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarCalificacionConcepto(int id_tabla, int id_compania_emisor, string descripcion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la actualización de un concepto de calificación
            object[] param = { 2, this._id_calificacion_concepto, id_tabla, id_compania_emisor, descripcion, id_usuario, habilitar, "", "" };
            //Reliza la actualización del concepto de calificación
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que Inserta un Concepto de Calificación
        /// </summary>
        /// <param name="id_tabla">Inserta el identificador de la entidad a la que pertenece el concepto de calificación (Cliente,Operador o Transportista)</param>
        /// <param name="id_compania_emisor">Inserta el identificador de la compañia a la que pertenece un concepto de calificación</param>
        /// <param name="descripcion">Inserta el nombre del concepto a calificar</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCalificacionConcepto(int id_tabla, int id_compania_emisor, string descripcion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para realizar la Inserción de un concepto de calificación
            object[] param = { 1, 0, id_tabla, id_compania_emisor, descripcion, id_usuario, true, "", "" };
            //Reliza la actualización del concepto de calificación
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los datos de una Calificación por Concepto
        /// </summary>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a la que pertenece el concepto de calificación (Cliente,Operador o Transportista)</param>
        /// <param name="id_compania_emisor">Actualiza el identificador de la compañia a la que pertenece un concepto de calificación</param>
        /// <param name="descripcion">Actualiza el nombre del concepto a calificar</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>        
        /// <returns></returns>
        public RetornoOperacion EditarCalificacionConcepto(int id_tabla, int id_compania_emisor, string descripcion, int id_usuario)
        {
            //Retorna al método el resultado del método encargado de actualizar los registros
            return this.editarCalificacionConcepto(id_tabla, id_compania_emisor, descripcion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro (Habilitado-Disponible,Deshabilitado-NoDisponible)</param>
        /// </summary>
        /// <param name="id_usuario">Permite identificar al usuario que realizo la acción de deshabilitar el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCalificacionConcepto(int id_usuario)
        {
            //Retorna al método el resultado del método encargado de actualizar los registros
            return this.editarCalificacionConcepto(this._id_tabla, this._id_compania_emisor, this._descripcion, id_usuario, false);
        }

        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCalificacionConcepto()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_calificacion_concepto);
        }
        #endregion
    }
}
