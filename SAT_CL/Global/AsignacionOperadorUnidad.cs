using System;
using System.Data;
using System.Linq;
using TSDK.Datos;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Implementa los Métodos para administración de la Asignación de Unidad y Operadro
    /// </summary>
    public class AsignacionOperadorUnidad : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define las posibilidad de búsqueda de una asignación activa
        /// </summary>
        public enum TipoBusquedaAsignacion 
        { 
            /// <summary>
            /// Por Id de Operador
            /// </summary>
            Operador, 
            /// <summary>
            /// Por Id de Unidad
            /// </summary>
            Unidad 
        }

        #endregion

        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_asignacion_operador_unidad_taou";

        private int _id_asignacion_operador_unidad;
        /// <summary>
        /// Obtiene el Id de Asignación Operador - Unidad
        /// </summary>
        public int id_asignacion_operador_unidad { get { return this._id_asignacion_operador_unidad; } }
        private int _id_operador;
        /// <summary>
        /// Obtiene el Id de Operador involucrado en la asignación
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private int _id_unidad;
        /// <summary>
        /// Obtiene el Id de Unidad involucrada en la asignación
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Obtiene la fecha de inicio de la asignación
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Obtiene la fecha de fin de la asignación
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private bool _habilitar;
        /// <summary>
        /// True Indica si la asignación es considerada como existente
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Crea una instancia con los valores predeterminados
        /// </summary>
        public AsignacionOperadorUnidad()
        {
            this._id_asignacion_operador_unidad = 0;
            this._id_operador = 0;
            this._id_unidad = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Crea una nueva instancia del tipo AsignacionOperadorUnidad con los datos del registro solicitado
        /// </summary>
        /// <param name="id_registro"></param>
        public AsignacionOperadorUnidad(int id_registro)
        {
            cargaAtributosInstancia(id_registro);
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AsignacionOperadorUnidad()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_registro)
        {   
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_registro, 0, 0, null, null, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {   
                //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Valores
                        this._id_asignacion_operador_unidad = id_registro;
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._fecha_inicio = Convert.ToDateTime(dr["FechaInicio"]);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Realiza la edición del registro
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaAsignacionOperadorUnidad(int id_operador, int id_unidad, DateTime fecha_inicio, DateTime fecha_fin, int id_usuario, bool habilitar)
        { 
            //Declarando objeto para almacenar parámetros de edición
            object[] param = { 2, this._id_asignacion_operador_unidad, id_operador, id_unidad, fecha_inicio, Fecha.ConvierteDateTimeObjeto(fecha_fin), id_usuario, habilitar, "", "" };

            //Realizando actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        #endregion

        #region Métodos Publicos

        /// <summary>
        /// Asignamos el Operador a la Unidad
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="fecha_inicio"> Fecha de Inicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAsignacionOperadorAUnidad(int id_operador, int id_unidad, DateTime fecha_inicio, int id_usuario)
        {   //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Editamos Id Operador de la Unidad
            using (Unidad objUnidad = new Unidad(id_unidad))
                //Editamos Id Operador de la Unidad
                resultado = objUnidad.ActualizaOperadorAsignado(id_operador, fecha_inicio, id_usuario);

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {
                //Armando Objeto de Parametros
                object[] param = { 1, 0, id_operador, id_unidad, Fecha.ConvierteDateTimeObjeto(fecha_inicio), null, id_usuario, true, "", "" };

                //Realizando la actualización
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                //Si hay algún error
                if (!resultado.OperacionExitosa)
                    resultado = new RetornoOperacion("La fecha de Inicio de la nueva asignación se traslapa con alguna asignación previa o existe una asignación inconclusa.");
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Finaliza la asignación en la fecha indicada
        /// </summary>
        /// <param name="fecha_fin">Fecha de Fin de asignación</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion TerminaAsignacionOperadorUnidad(DateTime fecha_fin, int id_usuario)
        {
            //Declaramos Objeto Resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_asignacion_operador_unidad);

            //Validando que ambos recursos estén disponibles (tractor y operador)
            using (Operador o = new Operador(this._id_operador))
            {
                using (Unidad u = new Unidad(this._id_unidad))
                { 
                    //Si ambos elementos fueron encontrados
                    if (o.habilitar && u.habilitar)
                    {
                        //Validando que su estatus sea disponible
                        if ((o.estatus != Operador.Estatus.Disponible && o.estatus != Operador.Estatus.Registrado) || u.EstatusUnidad != Unidad.Estatus.ParadaDisponible)
                            resultado = new RetornoOperacion(string.Format("La unidad '{0}' y el operador '{1}' deben tener estatus disponible para terminar su asignación.", u.numero_unidad, o.nombre));

                        //Si no hay problemas con su estatus
                        if (resultado.OperacionExitosa)
                        {
                            //validando que la fecha de fin sea menor a la fecha de inicio de la asignación
                            if (fecha_fin.CompareTo(this._fecha_inicio) >= 0)
                            {
                                //Se actualiza el Id de Operador asignado a la unidad
                                resultado = u.ActualizaOperadorAsignado(0, fecha_fin, id_usuario);
                                //Si se actualizó la unidad
                                if (resultado.OperacionExitosa)
                                {
                                    //Se termina la asignación actual
                                    resultado = editaAsignacionOperadorUnidad(this._id_operador, this._id_unidad, this._fecha_inicio, fecha_fin, id_usuario, this._habilitar);

                                    //Si hay algún error
                                    if (!resultado.OperacionExitosa)
                                        resultado = new RetornoOperacion("La fecha de Fin de asignación se traslapa con alguna asignación previa.");
                                }
                            }
                            //De lo contrario
                            else
                                resultado = new RetornoOperacion(string.Format("La fecha de fin '{0:dd/MM/yyyy HH:mm}' debe ser mayor a la fecha de inicio '{1:dd/MM/yyyy HH:mm}' de la asignación.", fecha_fin, this._fecha_inicio));
                        }
                    }
                    else
                        resultado = new RetornoOperacion("La unidad o el operador no fueron localizados.");
                }
            }            

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene Operador asignado a la Unidad
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static int ObtieneOperadorAsignadoAUnidad(int id_unidad)
        {
            //Definiendo objeto de retorno
            int id_operador = 0;

            //Declarando arreglo de parámetros para consulta en BD
            //Armando Objeto de Parametros
            object[] param = { 4, 0, 0, id_unidad, null, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Id de Operador
                    id_operador = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("IdOperador")).FirstOrDefault();
                }

                //Devolviendo resultado
                return id_operador;
            }
        }

        /// <summary>
        /// Obtiene la Unidad asignada al Operador
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <returns></returns>
        public static int ObtieneUnidadAsignadoAOperador(int id_operador)
        {
            //Definiendo objeto de retorno
            int id_unidad = 0;

            //Armando Objeto de Parametros
            object[] param = { 5, 0, id_operador, 0, null, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Id de Unidad
                    id_unidad = (from DataRow r in ds.Tables["Table"].Rows
                                 select r.Field<int>("IdUnidad")).FirstOrDefault();
                }

                //Devolviendo resultado
                return id_unidad;
            }
        }

        /// <summary>
        /// Obtiene la asignación actual del recurso solicitado
        /// </summary>
        /// <param name="tipo">Tipo de Búsqueda</param>
        /// <param name="id_recurso">Id de Recurso a consultar</param>
        /// <returns></returns>
        public static AsignacionOperadorUnidad ObtieneAsignacionActiva(TipoBusquedaAsignacion tipo, int id_recurso)
        {
            //Definiendo objeto de retorno
            AsignacionOperadorUnidad asignacion = new AsignacionOperadorUnidad();

            //Armando Objeto de Parametros
            object[] param;
            if (tipo == TipoBusquedaAsignacion.Operador)
                param = new object[] { 7, 0, id_recurso, 0, null, null, 0, false, "", "" };
            else
                param = new object[] { 6, 0, 0, id_recurso, null, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        asignacion = new AsignacionOperadorUnidad(Convert.ToInt32(dr["Id"]));
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return asignacion;
        }

        /// <summary>
        /// Obtiene los Operadores ligado a la Unidad
        /// </summary>
        /// <param name="id_unidad"></param>
        /// <returns></returns>
        public static DataTable CargaOperadores(int id_unidad)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 8, 0, 0, id_unidad, null, null, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Tabla
                    mit = ds.Tables[0];
                }

            }
            //Devolviendo resultado
            return mit;
        }
        #endregion
    }
}
