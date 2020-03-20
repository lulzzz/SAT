using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
using System.Transactions;


namespace SAT_CL.Calificacion
{
    /// <summary>
    /// Clase que realiza Insericion, Edición y Consulta sobre los registros de Calificación detalle
    /// </summary>
    public class CalificacionDetalle : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp de detalles Calificación
        /// </summary>
        private static string nom_sp = "calificacion.sp_calificacion_detalle_tcd";
        private int _id_calificacion_detalle;
        /// <summary>
        /// Identificador de los detalles de calificación
        /// </summary>
        public int id_calificacion_detalle
        {
            get { return _id_calificacion_detalle; }
        }
        private int _id_calificacion;
        /// <summary>
        /// Identificador de la calificación en general de una entidad (Cliente, Operador o Transportista)
        /// </summary>
        public int id_calificacion
        {
            get { return _id_calificacion; }
        }
        private int _id_concepto_calificacion;
        /// <summary>
        /// Identificador del concepto a calificar
        /// </summary>
        public int id_concepto_calificacion
        {
            get { return _id_concepto_calificacion; }
        }
        private byte _calificacion;
        /// <summary>
        /// Numéro de Evaluación a detalle de una entidad
        /// </summary>
        public byte calificacion
        {
            get { return _calificacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define la disponiblidad de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa en cero los atributos de la clase
        /// </summary>
        public CalificacionDetalle()
        {
            this._id_calificacion_detalle = 0;
            this._id_calificacion = 0;
            this._id_concepto_calificacion = 0;
            this._calificacion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_calificacion_detalle">Identificador que sirve como referencia para asignar valores a los atributos</param>
        public CalificacionDetalle(int id_calificacion_detalle)
        {
            //Invoca al método que realiza la asignación de valores a los atributos de la clase
            cargaAtributos(id_calificacion_detalle);
        }
        #endregion

        #region Destructor
        /// <summary>
        ///Destructor default de la clase 
        /// </summary>
        ~CalificacionDetalle()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de un registro y lo almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_calificacion_detalle">Identificador de un regsitro de Calificación Detalle</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_calificacion_detalle)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del objeto que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 3, id_calificacion_detalle, 0, 0, 0, 0, false, "", "" };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que existan)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos de la clase los valores de la fila del dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_calificacion_detalle = id_calificacion_detalle;
                        this._id_calificacion = Convert.ToInt32(r["IdCalificacion"]);
                        this._id_concepto_calificacion = Convert.ToInt32(r["IdConceptoCalificacion"]);
                        this._calificacion = Convert.ToByte(r["Calificacion"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retrono
                    retorno = true;
                }
            }
            //Retorna al método el objeto retrono
            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de los registros de Calificación Detalle
        /// </summary>
        /// <param name="id_calificacion">Actualiza el identificador de una Calificación general de las entidades Cliente,Operador o Transportista</param>
        /// <param name="id_concepto_calificacion">Actualiza el concepto que se va a evaluar</param>
        /// <param name="calificacion">Actualiza el Número que evalua a un concepto</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado - Disponible, Deshabilitado - No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarCalificacionDetalle(int id_calificacion, int id_concepto_calificacion, byte calificacion, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar un registro de calificación detalle
            object[] param = { 2, this._id_calificacion_detalle, id_calificacion, _id_concepto_calificacion, calificacion, id_usuario, habilitar, "", "" };
            //Invoca al método que realiza la actualización del registro
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que realiza la inserción de los registros de Calificación Detalle
        /// </summary>
        /// <param name="id_calificacion">Inserta el identificador de una Calificación general de las entdades Cliente,Operador o Transportista</param>
        /// <param name="id_concepto_calificacion">Inserta el concepto que se va a evaluar</param>
        /// <param name="calificacion">Inserta el Número que evalua a un concepto</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>        
        /// <returns></returns>
        public static RetornoOperacion InsertarCalificacionDetalle(int id_calificacion, int id_concepto_calificacion, byte calificacion, int id_contacto ,int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Creación del arreglo que almacena los datos necesarios para insertar un registro de calificación detalle
                object[] param = { 1, 0, id_calificacion, id_concepto_calificacion, calificacion, id_usuario, true, "", "" };
                //Invoca al método que realiza la actualización del registro
                retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
                //Retorna al método el objeto retorno

                if (retorno.OperacionExitosa)
                {
                    //Creación de la variable que almacena el promedio a asignar a una entidad
                    byte actualizaPormedioGeneral = 0;
                    //Instancia al método que realiza el calculo del promedio por conceptos hasta entonces evaluado
                    actualizaPormedioGeneral = SAT_CL.Calificacion.CalificacionDetalle.CalificacionAsignar(id_calificacion);

                    //Instancia a la clase calificación
                    using (SAT_CL.Calificacion.Calificacion cal = new SAT_CL.Calificacion.Calificacion(id_calificacion))
                    {
                        //Valida si la actualización es realizada por un contacto
                        if (id_contacto != 0)
                            //Realiza la actualización de la calificación en general con los datos del contacto
                            retorno = cal.ActualizaCalificacion(actualizaPormedioGeneral, id_contacto, 0);
                        //En caso contrario
                        else
                            //Realiza la actualización de la calificación en general con los datos del contaacto
                            retorno = cal.ActualizaCalificacion(actualizaPormedioGeneral, 0, id_usuario);
                    }
                }
                if (retorno.OperacionExitosa)
                {
                    trans.Complete();
                }

            }

            return retorno;
        }
        /// <summary>
        /// Método que realiza la actualización de los registros de Calificación Detalle
        /// </summary>
        /// <param name="id_calificacion">Actualiza el identificador de una Calificación general de las entdades Cliente,Operador o Transportista</param>
        /// <param name="id_concepto_calificacion">Actualiza el concepto que se va a evaluar</param>
        /// <param name="calificacion">Actualiza el Número que evalua a un concepto</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarCalificacionDetalle(int id_calificacion, int id_concepto_calificacion, byte calificacion, int id_usuario)
        {
            //Retorna al método el resultado de actualizar los registros
            return this.editarCalificacionDetalle(id_calificacion, id_concepto_calificacion, calificacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCalificacionDetalle(int id_contacto, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inserta una calificación (bloque transaccional)
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Retorna al método el resultado de actualizar los registros
                retorno = this.editarCalificacionDetalle(this._id_calificacion, this._id_concepto_calificacion, this._calificacion, id_usuario, false);
                //Valida que se deshabilito el registro
                if (retorno.OperacionExitosa)
                {
                    //Creación de la variable que almacena el promedio a asignar a una entidad
                    byte actualizaPormedioGeneral = 0;
                    //Instancia al método que realiza el calculo del promedio por conceptos hasta entonces evaluado
                    actualizaPormedioGeneral = SAT_CL.Calificacion.CalificacionDetalle.CalificacionAsignar(this._id_calificacion);
                    //Instancia a la clase Calificación
                    using (SAT_CL.Calificacion.Calificacion cal = new SAT_CL.Calificacion.Calificacion(this._id_calificacion))
                    {
                        //Valida si la actualización es realizada por un contacto
                        if (id_contacto != 0)
                            //Realiza la actualización de la calificación en general con los datos del contacto
                            retorno = cal.ActualizaCalificacion(actualizaPormedioGeneral, id_contacto, id_usuario);
                    }
                }
                //Valida que se actualizo el registro
                if (retorno.OperacionExitosa)
                {
                    //Completa la transacción
                    trans.Complete();
                }
            }
            return retorno;
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCalificacionDetalle()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_calificacion_detalle);
        }
        /// <summary>
        /// Método que realiza la busqueda de un registro y lo almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_calificacion_detalle">Identificador de un regsitro de Calificación Detalle</param>
        /// <returns></returns>
        public static DataTable ObtieneCalificacionDetalle(int id_calificacion, int id_tabla, int id_registro)
        {
            //Creación del objeto retorno
            DataTable dtCalificacionDetalle = null;
            //Creación del objeto que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 4, 0, id_calificacion, 0, 0, 0, false, id_tabla, id_registro };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que existan)
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    dtCalificacionDetalle = DS.Tables["Table"];
            }
            //Retorna al método el la tabla dtCalificacionDetalle
            return dtCalificacionDetalle;
        }
        /// <summary>
        /// Método que obtiene el promedio por concepto de una entidad
        /// </summary>
        /// <param name="id_tabla">Identificador de la entidad evaluada</param>
        /// <param name="id_registro">Identificador del registro perteneciente a la entidad evaluada</param>
        /// <param name="id_concepto_calificacion">Identificador del concepto a evaluar</param>
        /// <returns></returns>
        public static byte ObtieneCalificacionHistorialConcepto(int id_concepto_calificacion)
        {
            //Creación del objeto retorno
            byte calificacion = 0;
            //Creación del objeto que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 5, 0, 0, id_concepto_calificacion, 0, 0, false, "", "" };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que existan)
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    calificacion = (from DataRow r in DS.Tables[0].Rows
                                    select r.Field<byte>("CalificacionConcepto")).FirstOrDefault();
            }
            //Retorna al método el la tabla dtCalificacionDetalle
            return calificacion;
        }
        /// <summary>
        /// Método que Obtiene la calificación asignada por un contacto hasta el momento
        /// </summary>
        /// <param name="id_calificacion"></param>
        /// <returns></returns>
        public static byte CalificacionAsignar(int id_calificacion)
        {
            //Creación del objeto retorno
            byte totalCalificacion = 0;
            //Creación del objeto que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 6, 0, id_calificacion, 0, 0, 0, false, "", "" };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //ASigna a la tabla los valores del dataset
                    totalCalificacion = (from DataRow r in DS.Tables[0].Rows
                                         select r.Field<byte>("TotalCalificacion")).FirstOrDefault();
                }
            }
            //Retorna al método la calificacion Total
            return totalCalificacion;
        }
        #endregion
    }
}
