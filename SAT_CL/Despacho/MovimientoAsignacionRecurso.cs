using SAT_CL.Documentacion;
using SAT_CL.Global;
using SAT_CL.EgresoServicio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.Web.UI.WebControls;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de Movimientos Asignación
    /// </summary>
    public class MovimientoAsignacionRecurso : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus del Movimiento Asignación
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Iniciado
            /// </summary>
            Iniciado,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado,
            /// <summary>
            /// Liquidado
            /// </summary>
            Liquidado,
            /// <summary>
            /// Cancelado
            /// </summary>
            Cancelado
        }

        /// <summary>
        /// Enumera el Tipo de Movimiento Asignación
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Unidad
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Operador
            /// </summary>
            Operador,
            /// <summary>
            /// Tercero
            /// </summary>
            Tercero

        }
        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_movimiento_asignacion_recurso_tma";


        private int _id_movimiento_asignacion_recurso;
        /// <summary>
        /// Describe el Id de movimiento asignación
        /// </summary>
        public int id_movimiento_asignacion_recurso
        {
            get { return _id_movimiento_asignacion_recurso; }
        }
        private int _id_movimiento;
        /// <summary>
        /// Describe el Id de movimiento
        /// </summary>
        public int id_movimiento
        {
            get { return _id_movimiento; }
        }
        private byte _id_estatus_asignacion;
        /// <summary>
        /// Describe el Id estatus
        /// </summary>
        public byte id_estatus_asignacion
        {
            get { return _id_estatus_asignacion; }
        }
        private byte _id_tipo_asignacion;
        /// <summary>
        /// Describe el Id tipo  asignación
        /// </summary>
        public byte id_tipo_asignacion
        {
            get { return _id_tipo_asignacion; }
        }
        private int _id_recurso_asignado;
        /// <summary>
        /// Describe el Id Recurso Asignado
        /// </summary>
        public int id_recurso_asignado
        {
            get { return _id_recurso_asignado; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        private byte[] _row_version;
        /// <summary>
        /// Describe la versión
        /// </summary>
        public byte[] row_version
        {
            get { return _row_version; }
        }

        /// <summary>
        /// Enumera el Estatus Movimiento Asignación
        /// </summary>
        public Estatus EstatusMovimientoAsignacion
        {
            get { return (Estatus)_id_estatus_asignacion; }
        }

        /// <summary>
        /// Enumera el Tipo MovimientoAsignación
        /// </summary>
        public Tipo TipoMovimientoAsignacion
        {
            get { return (Tipo)_id_tipo_asignacion; }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~MovimientoAsignacionRecurso()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public MovimientoAsignacionRecurso()
        {

        }


        /// <summary>
        /// Genera una Instancia de Tipo Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id Movimiento Asignacion Recurso</param>
        public MovimientoAsignacionRecurso(int id_movimiento_asignacion_recurso)
        {
            cargaAtributosInstancia(id_movimiento_asignacion_recurso);
        }

        /// <summary>
        ///  Genera una Instancia de Tipo Movimiento Asignación Recurso ligando un Id movimiento, id estatus, tipo asignacion, id_recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso">id recurso</param>
        public MovimientoAsignacionRecurso(int id_movimiento, Estatus id_estatus, Tipo tipo_asignacion, int id_recurso)
        {
            cargaAtributosInstancia(id_movimiento, id_estatus, tipo_asignacion, id_recurso);
        }

        /// <summary>
        ///  Genera una Instancia de Tipo Movimiento Asignación Recurso ligando un Id movimiento, id estatus, tipo asignacion
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_estatus">Id Estatus</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        ///  /// <param name="motriz">En caso de ser Unidad filtramos si es Motriz</param>
        public MovimientoAsignacionRecurso(int id_movimiento, Estatus id_estatus, Tipo tipo_asignacion, bool motriz)
        {
            cargaAtributosInstancia(id_movimiento, id_estatus, tipo_asignacion, motriz);
        }
        /// <summary>
        /// Genera una Instancia de Tipo Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id Movimiento Asignacion Recurso</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_movimiento_asignacion_recurso)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_movimiento_asignacion_recurso, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento_asignacion_recurso = Convert.ToInt32(r["Id"]);
                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_estatus_asignacion = Convert.ToByte(r["IdEstatusAsignacion"]);
                        _id_tipo_asignacion = Convert.ToByte(r["IdTipoAsignacion"]);
                        _id_recurso_asignado = Convert.ToInt32(r["IdRecursoAsignado"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }


        /// <summary>
        /// Genera una Instancia de Tipo Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_estatus">Estatus de la Asignacion</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_movimiento, Estatus id_estatus, Tipo tipo_asignacion, int id_recurso)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 12, 0, id_movimiento, id_estatus, tipo_asignacion, id_recurso, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento_asignacion_recurso = Convert.ToInt32(r["Id"]);
                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_estatus_asignacion = Convert.ToByte(r["IdEstatusAsignacion"]);
                        _id_tipo_asignacion = Convert.ToByte(r["IdTipoAsignacion"]);
                        _id_recurso_asignado = Convert.ToInt32(r["IdRecursoAsignado"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Genera una Instancia de Tipo Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_estatus">Estatus de la Asignacion</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="motriz">En caso de ser Unidad filtramos si es Motriz</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_movimiento, Estatus id_estatus, Tipo tipo_asignacion, bool motriz)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 49, 0, id_movimiento, id_estatus, tipo_asignacion, 0, 0, false, null, motriz, "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento_asignacion_recurso = Convert.ToInt32(r["Id"]);
                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_estatus_asignacion = Convert.ToByte(r["IdEstatusAsignacion"]);
                        _id_tipo_asignacion = Convert.ToByte(r["IdTipoAsignacion"]);
                        _id_recurso_asignado = Convert.ToInt32(r["IdRecursoAsignado"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _row_version = (byte[])r["RowVersion"];

                        //Asignamos Resultado
                        resultado = true;
                    }
                }
            }
            //Obtenemos Resultado
            return resultado;
        }

        #endregion

        #region Metodos privados

        /// <summary>
        ///  Método encargado de Editar un Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <param name="estatus_asignacion">Estatus de la Asignación entre el recurso y Movimiento</param>
        /// <param name="tipo_asignacion">Tipo de Asignación entre el recurso</param>
        /// <param name="id_recurso_asignado">Id de Recurso Asignado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaMovimientoAsignacionRecurso(int id_movimiento, Estatus estatus_asignacion, Tipo tipo_asignacion, int id_recurso_asignado, int id_usuario,
                                                                 bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = {2, this._id_movimiento_asignacion_recurso, id_movimiento, estatus_asignacion, tipo_asignacion, id_recurso_asignado, id_usuario, habilitar,
                                     this._row_version, "", ""};

                //Establecemos Resultado
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            }
            else
            {
                //Establecmeos Error
                resultado = new RetornoOperacion("El registro fue modificado en BD desde la última vez que fue consultado.");
            }

            return resultado;
        }

        /// <summary>
        /// Validamos versión de Registro desde la Base de Datos y Instancia creada
        /// </summary>
        /// <returns></returns>
        private bool validaVersionRegistro()
        {
            //Declaramos Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, this._id_movimiento_asignacion_recurso, 0, 0, 0, 0, 0, false, this._row_version, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Establecemos Resultado correcto
                    resultado = true;
            }

            //Obtenemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene la cantidad de veces que ha sido asignado el recurso al Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo_asignacion_unidad">Para conteo de Unidad enviamos e Tipo de asignacion de Unidad (Remolque, Tractor etc.)</param>
        /// <param name="total_unidad">Total de Unidades Asignadas</param>
        /// <param name="total_operador">Total de Operadores Asignados</param>
        /// <param name="total_tercero">Total de Tercero Asignados</param>
        private static void obtieneTotalAsignacionesRecurso(int id_movimiento, int tipo_asignacion_unidad, out int total_unidad, out int total_operador, out int total_tercero)
        {
            //Asignando parámetros de salida
            total_unidad = total_operador = total_tercero = 0;


            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, id_movimiento, 0, 0, 0, 0, false, null, tipo_asignacion_unidad, "" };

            //Realziando la consulta de conteos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1", "Table2"))
                {
                    //Obteniendo total de Unidades
                    total_unidad = ds.Tables["Table"].Rows[0].Field<int>("TotalAsignacionesUnidad");
                    //Obteniendo total de Operadores
                    total_operador = ds.Tables["Table1"].Rows[0].Field<int>("TotalAsignacionesOperador");
                    //Obteniendo total de Proveedores
                    total_tercero = ds.Tables["Table2"].Rows[0].Field<int>("TotalAsignacionesTercero");
                }
            }
        }

        /// <summary>
        /// Inserta el operador asignado a una Unidad
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor </param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_recurso_asignado">Id Recurso (Operador)</param>
        /// <param name="estatus">Estatus de la Asignación</param>
        /// <param name="id_parada">Parada ctual del resurso</param>
        /// <param name="fecha_actualizacion">Fecha actualización de los atributos de Ubicación del Operador</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion insertaOperadorAsignadoAUnidad(int id_compania_emisor, int id_movimiento, int id_recurso_asignado, Estatus estatus,
                                        int id_parada, DateTime fecha_actualizacion, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultadoOperador = new RetornoOperacion(0);
            //Auxiliares
            bool validacionAsignacionActiva = false;
            int id_asignacion_operador = 0;

            //Obtenemos Id operador
            int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(id_recurso_asignado);

            //Si existe el operador ligado a la Unidad
            if (id_operador > 0)
            {
                //Validamos asignacion activa
                validacionAsignacionActiva = ValidaAsignacionActiva(Tipo.Operador, id_operador, id_movimiento);

                //Validamos que la La unidad no se encuentre activa
                if (!validacionAsignacionActiva)
                {
                    //Validamos tipo de recurso a ingresar (Operador de acuerdo a los tipos de recurso existentes al movimiento.
                    resultadoOperador = ValidaTipoRecurso(id_movimiento, estatus, Tipo.Operador, 0, id_operador);

                    //Validamos Tipo de Recurso
                    if (resultadoOperador.OperacionExitosa)
                    {
                        //Validamos el limite de recursos  que se permiten al movimiento.
                        resultadoOperador = ValidaLimiteMovimientoRecurso(id_compania_emisor, id_movimiento, Tipo.Operador, 0, id_operador);

                        //Si la Validación fue correcta
                        if (resultadoOperador.OperacionExitosa)
                        {
                            //Instanciando Movimiento
                            using (Movimiento mov = new Movimiento(id_movimiento))
                            {
                                //Validando Movimiento
                                if (mov.habilitar)
                                {
                                    //Validando Servicio
                                    if (mov.id_servicio > 0)

                                        //Obteniendo Validación de Evidencias
                                        resultadoOperador = SAT_CL.ControlEvidencia.Reportes.ValidaViajesSinEvidenciasOperador(id_compania_emisor, id_operador);
                                    else
                                        //Instanciando Excepción
                                        resultadoOperador = new RetornoOperacion("");

                                    //Validando Operación
                                    if (!resultadoOperador.OperacionExitosa)
                                    {
                                        //Insertamos Operador
                                        //Inicializando arreglo de parámetros
                                        object[] param = { 1, 0, id_movimiento, estatus, Tipo.Operador, id_operador, id_usuario, true, null, "", "" };

                                        //Establecemos Resultado
                                        resultadoOperador = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
                                    }
                                    else
                                        //Instanciando Excepción
                                        resultadoOperador = new RetornoOperacion(resultadoOperador.Mensaje);
                                }
                                else
                                    resultadoOperador = new RetornoOperacion("No se puede obtener el Movimiento");
                            }
                        }
                    }
                }
                else
                    resultadoOperador = new RetornoOperacion(ObtieneAsignacionActiva(Tipo.Operador, id_operador, id_movimiento));
            }
            else
            {
                //Buscamos operador el primer operador registrado al movimiento
                id_operador = ObtienePrimerOperadorRegistrado(id_movimiento);

                //Si existe el Operador
                if (id_operador > 0)
                {

                    //Asignamos el Operador a la Unidad.
                    resultadoOperador = AsignacionOperadorUnidad.InsertaAsignacionOperadorAUnidad(id_operador, id_recurso_asignado, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);
                }
            }
            //Validamos Resultado
            if (resultadoOperador.OperacionExitosa)
            {
                //Recuperando id de asignación
                id_asignacion_operador = resultadoOperador.IdRegistro;

                //Validamos que exista Operador y la Asignación sea Iniciada y no se encuentre la asignación
                if (id_operador > 0 && estatus == Estatus.Iniciado && !validacionAsignacionActiva)
                {
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(id_operador))
                    {
                        //Actualizamos estatus de Operador a Parada Ocupado
                        resultadoOperador = objOperador.ActualizaEstatus(Operador.Estatus.Ocupado, id_usuario);

                        //Validamos Resultado
                        if (resultadoOperador.OperacionExitosa)
                        {
                            //Actualizamos Atributos
                            if (objOperador.ActualizaAtributosInstancia())
                            {
                                //Actualizamos atributos del Operador de Ubicación
                                objOperador.ActualizaParadaYMovimiento(id_parada, 0, fecha_actualizacion, id_usuario);

                                //Si no hubo errores de actualización
                                if (resultadoOperador.OperacionExitosa)
                                    //Actualizando resultado principal (inserción de asignación de operador)
                                    resultadoOperador = new RetornoOperacion(id_asignacion_operador);
                            }
                            else
                            {
                                //Establecemos Mensaje
                                resultadoOperador = new RetornoOperacion("No se encontró datos complementarios del Operador " + objOperador.nombre + ".");
                            }
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultadoOperador;
        }

        /// <summary>
        /// Inserta Unidad asignado al Operador
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor </param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="fecha_actualizacion">Fecha de Actualización de los atributos de la Unidad</param>
        /// <param name="tipo_estancia">Tipo Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Tipo Actualización Inicio</param>
        /// <param name="id_recurso_asignado">Id Recurso (Unidad)</param>
        /// <param name="id_tipo_unidad">Tipo Unidad</param>
        /// <param name="estatus">Estatus de la Asignacion</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private static RetornoOperacion insertaUnidadAsignadoAOperador(int id_compania_emisor, int id_movimiento, int id_parada, DateTime fecha_actualizacion, EstanciaUnidad.Tipo tipo_estancia, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_inicio,
                                                                       int id_recurso_asignado, Estatus estatus, int id_tipo_unidad, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Auxiliares
            int id_asignacion_unidad = 0;
            bool validacionAsignacionActiva = false;

            //Obtenemos la Unidad asignado al Operador
            int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso_asignado);

            //Validamos Si existe Unidad
            if (id_unidad > 0)
            {
                //Validamos asignacion activa
                validacionAsignacionActiva = ValidaAsignacionActiva(Tipo.Unidad, id_unidad, id_movimiento);

                //Si la Unidad ya se encuentra activa
                if (!validacionAsignacionActiva)
                {
                    //Validamos tipo de recurso a ingresar (Unidad) de acuerdo a los tipos de recurso existentes al movimiento.
                    resultado = ValidaTipoRecurso(id_movimiento, estatus, Tipo.Unidad, 1, id_unidad);

                    //Validamos Tipo de Recurso
                    if (resultado.OperacionExitosa)
                    {
                        //Validamos el limite que se permiten al movimiento.
                        resultado = ValidaLimiteMovimientoRecurso(id_compania_emisor, id_movimiento, Tipo.Unidad, 1, id_unidad);

                        //Si la Validación fue correcta
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciando Movimiento
                            using (Movimiento mov = new Movimiento(id_movimiento))
                            {
                                //Validando que Exista el Movimiento
                                if (mov.habilitar)
                                {
                                    //Instanciando Parada Origen
                                    using (Parada p1 = new Parada(mov.id_parada_origen))
                                    {
                                        //Validando que Exista la Parada
                                        if (p1.id_parada > 0)
                                        {
                                            //Inicializando arreglo de parámetros
                                            object[] param = { 1, 0, id_movimiento, estatus, Tipo.Unidad, id_unidad, id_usuario, true, null, "", "" };

                                            //Establecemos Resultado
                                            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                                            ////Validando que no exista una Asignación en la misma Ubicación
                                            //if (!(MovimientoAsignacionRecurso.ObtieneAsignacionRegistrada(Tipo.Unidad, id_unidad, p1.id_ubicacion) > 0))
                                            //{
                                            //    //Inicializando arreglo de parámetros
                                            //    object[] param = { 1, 0, id_movimiento, estatus, Tipo.Unidad, id_unidad, id_usuario, true, null, "", "" };

                                            //    //Establecemos Resultado
                                            //    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
                                            //}
                                            //else
                                            //    //Instanciando Excepción
                                            //    resultado = new RetornoOperacion("Este recurso ya tiene una asignación pendiente con esta ubicación como origen.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                //Buscamos asignación registrada de la Unidad
                id_unidad = ObtienePrimerUnidadRegistrada(id_movimiento);

                //Si existe la Unidad
                if (id_unidad > 0)
                {
                    //Insertamos Asignación Operador ligado a la Unidad
                    resultado = AsignacionOperadorUnidad.InsertaAsignacionOperadorAUnidad(id_recurso_asignado, id_unidad, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);

                }
            }
            //Validamos Actualizaciones
            if (resultado.OperacionExitosa)
            {
                //Recuperando asignación de unidad
                id_asignacion_unidad = resultado.IdRegistro;

                //Validamos que exista Unidad y la Asignación sea Iniciada para actualización de estatus de la Unidad 
                if (id_unidad > 0 && estatus == Estatus.Iniciado && !validacionAsignacionActiva)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_unidad))
                    {
                        //Declaramos Variable para obtener la estancia actual de la unidad
                        int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_unidad);

                        //Actualizamos estatus de la Unidad a Parada Ocupado
                        resultado = objUnidad.ActualizaEstatusAParadaOcupado(id_parada, id_estancia, id_usuario);

                        //Validamos Actualiación del Estatus de la Unidad
                        if (resultado.OperacionExitosa)
                        {
                            //Refrescamos Atributos
                            if (objUnidad.ActualizaAtributosInstancia())
                            {
                                //Validamos Atributos de la Principales de la Unidad
                                resultado = objUnidad.ActualizaEstanciaYMovimiento(id_estancia, 0, fecha_actualizacion, id_usuario);

                                //Validamos Resultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Estancia
                                    using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(id_estancia))
                                    {
                                        //Validamos que el Id de Parada sea Diferente al actual
                                        if (objEstanciaUnidad.id_parada != id_parada)
                                        {
                                            //Actualizamos parada de la Estancia
                                            resultado = objEstanciaUnidad.CambiaParadaEstanciaUnidad(id_parada, id_usuario);

                                            //SI no hay errores de actualización
                                            if (resultado.OperacionExitosa)
                                                resultado = new RetornoOperacion(id_asignacion_unidad);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Establecemos Mensaje
                                resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + objUnidad.numero_unidad + ".");
                            }
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Declaramos Unidades para su Asignación
        /// </summary>
        /// <param name="id_tipo_unidad">Tipo de Unidad (Remolque, Tractor)</param>
        /// <param name="id_recurso_asignado">Id de la Unidad</param>
        /// <param name="conteoUnidad">Total de Unidades actualmente asignadas</param>
        /// <param name="objConfiguracionAsignacionRecurso">Configuracion</param>
        /// <returns></returns>
        [Obsolete]
        private static RetornoOperacion validacionUnidad(int id_tipo_unidad, int id_recurso_asignado, int conteoUnidad, ConfiguracionAsignacionRecurso objConfiguracionAsignacionRecurso)
        {
            //Declaramos Objeto 
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciamos Unidad
            using (Unidad objUnidad = new Unidad(id_recurso_asignado))
            {
                //Instanciamos el Tipo de Unidad
                using (UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                {
                    //Instanciamos la Configuracion de la Unidad coincidente con el Tipo de Unidad y el Subtipo.
                    using (ConfiguracionAsignacionRecursoTipo objConfiguracionAsignacionRecursoTipo = new ConfiguracionAsignacionRecursoTipo(objConfiguracionAsignacionRecurso.id_configuracion_asignacion_recurso,
                            ConfiguracionAsignacionRecursoTipo.TipoRecurso.Unidad, id_tipo_unidad, objUnidad.id_sub_tipo_unidad))
                    {
                        //Validamos que exista configuración
                        if (objConfiguracionAsignacionRecursoTipo.id_configuracion_asignacion_recurso_tipo > 0)
                        {
                            //Validamos que la  Unidad no sea Motriz
                            if (objUnidadTipo.bit_motriz == false)
                            {
                                //Validamos existencia de Unidades motrices acepten unidades de Arrastre
                                if (!Validacion.ValidaOrigenDatos(ConfiguracionAsignacionRecursoTipo.CargaUnidadesMotricesPermitanArrastre(objConfiguracionAsignacionRecurso.id_configuracion_asignacion_recurso)))
                                {
                                    //Mostramos Error
                                    resultado = new RetornoOperacion("La unidad motriz no permite unidades de arrastre");
                                }
                            }
                            //Validamos Cantidad de Asignaciones
                            if (conteoUnidad >= objConfiguracionAsignacionRecursoTipo.cantidad)
                            {
                                //Mostramos Error
                                resultado = new RetornoOperacion("La cantidad máxima de Unidades (" + objConfiguracionAsignacionRecursoTipo.cantidad + ") se ha alcanzado.");

                            }
                        }
                        else
                        {
                            //Establecemos Error
                            resultado = new RetornoOperacion("No existe configuración para asignación de Unidad");
                        }
                    }
                }
            }
            return resultado;
        }

        private static RetornoOperacion validacionUnidad(int id_tipo_unidad, int id_recurso_asignado, int conteoUnidad, List<ConfiguracionAsignacionRecurso> configuraciones)
        {
            //Declaramos Objeto 
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciamos Unidad
            using (Unidad objUnidad = new Unidad(id_recurso_asignado))
            {
                //Instanciamos el Tipo de Unidad
                using (UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                {
                    //Validando Lista
                    if (configuraciones.Count > 0)
                    {
                        foreach (ConfiguracionAsignacionRecurso conf in configuraciones)
                        {
                            //Instanciamos la Configuracion de la Unidad coincidente con el Tipo de Unidad y el Subtipo.
                            using (ConfiguracionAsignacionRecursoTipo objConfiguracionAsignacionRecursoTipo = new ConfiguracionAsignacionRecursoTipo(conf.id_configuracion_asignacion_recurso,
                                    ConfiguracionAsignacionRecursoTipo.TipoRecurso.Unidad, id_tipo_unidad, objUnidad.id_sub_tipo_unidad))
                            {
                                //Validamos que exista configuración
                                if (objConfiguracionAsignacionRecursoTipo.id_configuracion_asignacion_recurso_tipo > 0)
                                {
                                    //Validamos que la  Unidad no sea Motriz
                                    if (objUnidadTipo.bit_motriz == false)
                                    {
                                        //Validamos existencia de Unidades motrices acepten unidades de Arrastre
                                        if (!Validacion.ValidaOrigenDatos(ConfiguracionAsignacionRecursoTipo.CargaUnidadesMotricesPermitanArrastre(conf.id_configuracion_asignacion_recurso)))
                                        {
                                            //Mostramos Error
                                            resultado = new RetornoOperacion("La unidad motriz no permite unidades de arrastre");
                                        }
                                        else
                                            resultado = new RetornoOperacion(0);
                                    }
                                    //Validamos Cantidad de Asignaciones
                                    if (conteoUnidad >= objConfiguracionAsignacionRecursoTipo.cantidad)
                                    
                                        //Mostramos Error
                                        resultado = new RetornoOperacion("La cantidad máxima de Unidades (" + objConfiguracionAsignacionRecursoTipo.cantidad + ") se ha alcanzado.");
                                    else
                                        resultado = new RetornoOperacion(0);
                                }
                                else
                                {
                                    //Establecemos Error
                                    resultado = new RetornoOperacion("No existe configuración para asignación de Unidad");
                                }
                            }

                            if (resultado.OperacionExitosa)
                                break;
                        }
                    }
                    
                    
                }
            }
            return resultado;
        }

        /// <summary>
        /// Mètodo encargado de Validar si Existen Anticicpos ligados a un Recurso *
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion validaAnticipos(int id_servicio, int id_operador, int id_unidad, int id_proveedor_compania, int id_usuario)
        {

            //Establecemos Mensaje Error
            return DetalleLiquidacion.ActualizaAnticiposPorMovimiento(id_servicio, this._id_movimiento, id_operador, id_unidad, id_proveedor_compania, id_usuario);
        }

        /// <summary>
        /// Validamos Operadores para su Asignación
        /// </summary>
        /// <param name="conteoOperador">Total de Operadores actualmente asignados</param>
        /// <param name="objConfiguracionAsignacionRecurso">Configuracion</param>
        /// <returns></returns>
        [Obsolete]
        private static RetornoOperacion validacionOperador(int conteoOperador, ConfiguracionAsignacionRecurso objConfiguracionAsignacionRecurso)
        {
            //Declaramos Objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Obtenemos Configuración para operador
            using (ConfiguracionAsignacionRecursoTipo objConfiguracionAsignacionRecursoTipo = new ConfiguracionAsignacionRecursoTipo(objConfiguracionAsignacionRecurso.id_configuracion_asignacion_recurso, ConfiguracionAsignacionRecursoTipo.TipoRecurso.Operador))
            {
                //Validamos que exista Configuración Asgnación Recurso para operador
                if (objConfiguracionAsignacionRecursoTipo.id_configuracion_asignacion_recurso_tipo > 0)
                {
                    //Validamos Asignaciones Permitidas vs Registradas
                    if (conteoOperador >= objConfiguracionAsignacionRecursoTipo.cantidad)
                    {
                        //Mostramos Error
                        resultado = new RetornoOperacion("La cantidad máxima de Operadores se ha alcanzado.");
                    }

                }
                else
                {
                    //Establecemos Error
                    resultado = new RetornoOperacion("No existe configuración para asignación de Operador");
                }
            }
            return resultado;
        }

        private static RetornoOperacion validacionOperador(int conteoOperador, List<ConfiguracionAsignacionRecurso> configuraciones)
        {
            //Declaramos Objeto Resultante
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validando Lista
            if (configuraciones.Count > 0)
            {
                foreach (ConfiguracionAsignacionRecurso conf in configuraciones)
                {
                    //Obtenemos Configuración para operador
                    using (ConfiguracionAsignacionRecursoTipo objConfiguracionAsignacionRecursoTipo = new ConfiguracionAsignacionRecursoTipo(conf.id_configuracion_asignacion_recurso, ConfiguracionAsignacionRecursoTipo.TipoRecurso.Operador))
                    {
                        //Validamos que exista Configuración Asgnación Recurso para operador
                        if (objConfiguracionAsignacionRecursoTipo.id_configuracion_asignacion_recurso_tipo > 0)
                        {
                            //Validamos Asignaciones Permitidas vs Registradas
                            if (conteoOperador >= objConfiguracionAsignacionRecursoTipo.cantidad)
                                //Mostramos Error
                                resultado = new RetornoOperacion("La cantidad máxima de Operadores se ha alcanzado.");
                            else
                                resultado = new RetornoOperacion(0);
                        }
                        else
                        {
                            //Establecemos Error
                            resultado = new RetornoOperacion("No existe configuración para asignación de Operador");
                        }
                    }

                    if (resultado.OperacionExitosa)
                        break;
                }
            }

            //Devolviendo Resultado
            return resultado;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar un  Movimiento  Asignación sin validación de Unidad y Operador
        /// </summary>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <param name="estatus">Estatus de la asignación</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso_asignado">Id de Recurso Asignado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        [Obsolete]
        public static RetornoOperacion InsertaMovimientoAsignacionRecursoProgramado(int id_movimiento, Estatus estatus, Tipo tipo_asignacion,
                                                                            int id_recurso_asignado, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(id_movimiento))
            {
                //Validando que Exista el Movimiento
                if (mov.habilitar)
                {
                    //Instanciando Parada Origen
                    using (Parada p1 = new Parada(mov.id_parada_origen))
                    {
                        //Validando que Exista la Parada
                        if (p1.id_parada > 0)
                        {
                            //Inicializando arreglo de parámetros
                            object[] param = { 1, 0, id_movimiento, estatus, tipo_asignacion, id_recurso_asignado, id_usuario, true, null, "", "" };

                            //Establecemos Resultado
                            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                            ////Validando que no exista una Asignación en la misma Ubicación
                            //if (!(MovimientoAsignacionRecurso.ObtieneAsignacionRegistrada(tipo_asignacion, id_recurso_asignado, p1.id_ubicacion) > 0))
                            //{
                                
                            //}
                            //else
                            //    //Instanciando Excepción
                            //    resultado = new RetornoOperacion("Este recurso ya tiene una asignación pendiente con esta ubicación como origen.");
                        }
                    }
                }
            }

            //Establecemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Insertar un  Movimiento  Asignación sin validación de Unidad y Operador ()
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="estatus"></param>
        /// <param name="tipo_asignacion"></param>
        /// <param name="id_recurso_asignado"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoAsignacionRecurso(int id_movimiento, Estatus estatus, Tipo tipo_asignacion,
                                                                            int id_recurso_asignado, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(id_movimiento))
            {
                //Validando que Exista el Movimiento
                if (mov.habilitar)
                {
                    //Instanciando Parada Origen
                    using (Parada p1 = new Parada(mov.id_parada_origen))
                    {
                        //Validando que Exista la Parada
                        if (p1.id_parada > 0)
                        {
                            //Inicializando arreglo de parámetros
                            object[] param = { 1, 0, id_movimiento, estatus, tipo_asignacion, id_recurso_asignado, id_usuario, true, null, "", "" };

                            //Establecemos Resultado
                            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                            ////Validando que no exista una Asignación en la misma Ubicación
                            //if (!(MovimientoAsignacionRecurso.ObtieneAsignacionRegistrada(tipo_asignacion, id_recurso_asignado, p1.id_ubicacion) > 0))
                            //{
                            //    //Inicializando arreglo de parámetros
                            //    object[] param = { 1, 0, id_movimiento, estatus, tipo_asignacion, id_recurso_asignado, id_usuario, true, null, "", "" };

                            //    //Establecemos Resultado
                            //    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
                            //}
                            //else
                            //    //Instanciando Excepción
                            //    resultado = new RetornoOperacion("Este recurso ya tiene una asignación pendiente con esta ubicación como origen.");
                        }
                    }
                }
            }

            //Establecemos Resultado
            return resultado;
        }
        /// <summary>
        /// Inserta una asignación de recurso por cada elemento especificado al movimiento indicado 
        /// </summary>
        /// <param name="id_recurso">Conjunto de recursos por asignar</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="estatus">Id de Estatus</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoAsignacionRecursos(List<KeyValuePair<int, Tipo>> id_recurso, int id_movimiento, Estatus estatus, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se ha especificado ningún recurso por asignar.");

            //Validando que existan recursos por asignar
            if (id_recurso != null)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Para cada recurso por asignar
                    foreach (KeyValuePair<int, Tipo> r in id_recurso)
                    {
                        //Insertando nueva asignación
                        resultado = InsertaMovimientoAsignacionRecurso(id_movimiento, estatus, r.Value, r.Key, id_usuario);

                        //Si existe algún error
                        if (!resultado.OperacionExitosa)
                            //Saliendo del ciclo
                            break;
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos estatus de la Asignación
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusMovimientoAsignacionRecurso(Estatus estatus, int id_usuario)
        {

            //Editamos estatus de la Asignación
            return editaMovimientoAsignacionRecurso(this._id_movimiento, estatus, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado de Insertar un  Movimiento  Asignación Recurso validando la cantidad de Unidades  y Operadores Permitidos
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_recurso_asignado">Id de Recurso Asignado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoAsignacionRecurso(int id_compania_emisor, int id_movimiento, Tipo tipo_asignacion, int id_tipo_unidad,
                                                                            int id_recurso_asignado, int id_usuario)
        {
            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            bool validaAsignacionActiva = false;
            //Declarando auxiliar para determinar Id de Unidad Involucrada (Id de recurso asignado directamente o por vinculación con operador)
            int id_unidad_motriz = 0;

            //Auxiliares mensaje
            int id_mov_asig_unidad = 0, id_mov_asig_tercero = 0, id_mov_asig_operador = 0;

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                if (objMovimiento.habilitar)
                {
                    //Instanciamos Servicio
                    using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                    {
                        //Validamos que el Servicio se encuentre Documentado
                        if (objServicio.estatus == Servicio.Estatus.Documentado)
                        {
                            //Validamos que el estatus del Movimiento se encuentre Registrado
                            if ((Movimiento.Estatus)objMovimiento.id_estatus_movimiento == Movimiento.Estatus.Registrado)
                            {
                                //Creamos la transacción 
                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Validamos existencia de Asignación activa
                                    validaAsignacionActiva = ValidaAsignacionActiva(tipo_asignacion, id_recurso_asignado, id_movimiento);

                                    //Si no existe una asignación
                                    if (!validaAsignacionActiva)
                                    {
                                        //Validamos tipo de recurso a ingresar (Tercero, Unidad, Remolque) de acuerdo a los tipos de recurso existentes al movimiento.
                                        resultado = ValidaTipoRecurso(id_movimiento, Estatus.Registrado, tipo_asignacion, id_tipo_unidad, id_recurso_asignado);

                                        //Validamos Resultado del Tipo de Recurso
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Validamos el limite de recursos que se permiten.
                                            resultado = ValidaLimiteMovimientoRecurso(id_compania_emisor, id_movimiento, tipo_asignacion, id_tipo_unidad, id_recurso_asignado);
                                        }
                                    }
                                    //Validamos Limite de Recursos
                                    if (resultado.OperacionExitosa)
                                    {
                                        if (tipo_asignacion == Tipo.Unidad)
                                        {
                                            //Validamos que el Tipo de Unidad sea motriz para asignar el Operador correspondiente
                                            using (UnidadTipo unidadTipo = new UnidadTipo(id_tipo_unidad))
                                            {
                                                if (unidadTipo.bit_motriz)
                                                {
                                                    id_unidad_motriz = id_recurso_asignado;
                                                    //Insertamos operador asignado a la Unidad
                                                    resultado = insertaOperadorAsignadoAUnidad(id_compania_emisor, id_movimiento, id_recurso_asignado, Estatus.Registrado, 0, DateTime.MinValue, id_usuario);

                                                    //recuperando asignación de operador
                                                    id_mov_asig_operador = resultado.IdRegistro;
                                                }
                                            }
                                        }

                                        //Si el Tipo es Operador
                                        else if (tipo_asignacion == Tipo.Operador)
                                        {
                                            //Validando Servicio
                                            if (objMovimiento.id_servicio > 0)

                                                //Validando Evidencias
                                                resultado = SAT_CL.ControlEvidencia.Reportes.ValidaViajesSinEvidenciasOperador(id_compania_emisor, id_recurso_asignado);
                                            else
                                                //Instanciando Retorno
                                                resultado = new RetornoOperacion("");

                                            //Validando Resultado Evidencias
                                            if (!resultado.OperacionExitosa)
                                            {
                                                //Insertamos Unidad Asignada al Operador
                                                resultado = insertaUnidadAsignadoAOperador(id_compania_emisor, id_movimiento, 0, DateTime.MinValue, EstanciaUnidad.Tipo.Operativa, EstanciaUnidad.TipoActualizacionInicio.Manual, id_recurso_asignado, Estatus.Registrado, id_tipo_unidad, id_usuario);

                                                //Recuperando asignación de tractor
                                                id_mov_asig_unidad = resultado.IdRegistro;

                                                //Obteniendo unidad asignada al operador
                                                id_unidad_motriz = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso_asignado);
                                            }
                                        }
                                    }

                                    //Si la Validación fue correcta y no exista la asignación
                                    if (resultado.OperacionExitosa && validaAsignacionActiva == false)
                                    {
                                        //Instanciando Movimiento
                                        using (Movimiento mov = new Movimiento(id_movimiento))
                                        {
                                            //Validando que Exista el Movimiento
                                            if (mov.habilitar)
                                            {
                                                //Instanciando Parada Origen
                                                using (Parada p1 = new Parada(mov.id_parada_origen))
                                                {
                                                    //Validando que Exista la Parada
                                                    if (p1.id_parada > 0)
                                                    {
                                                        //Inicializando arreglo de parámetros
                                                        object[] param = { 1, 0, id_movimiento, Estatus.Registrado, tipo_asignacion, id_recurso_asignado, id_usuario, true, null, "", "" };

                                                        //Establecemos Resultado
                                                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                                                        ////Validando que no exista una Asignación en la misma Ubicación
                                                        //if (!(MovimientoAsignacionRecurso.ObtieneAsignacionRegistrada(tipo_asignacion, id_recurso_asignado, p1.id_ubicacion) > 0))
                                                        //{
                                                            
                                                        //}
                                                        //else
                                                        //    //Instanciando Excepción
                                                        //    resultado = new RetornoOperacion("Este recurso ya tiene una asignación pendiente con esta ubicación como origen.");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Si se asignó un recurso unidad motriz
                                        if (id_unidad_motriz > 0)
                                            //Realizando envío de información a plataforma de tercero
                                            Movimiento.ActualizaPlataformaTerceros(id_movimiento, id_usuario);

                                        //Determinando tipo de asignación final
                                        switch (tipo_asignacion)
                                        {
                                            case Tipo.Operador:
                                                id_mov_asig_operador = resultado.IdRegistro;
                                                break;
                                            case Tipo.Unidad:
                                                id_mov_asig_unidad = resultado.IdRegistro;
                                                break;
                                            case Tipo.Tercero:
                                                id_mov_asig_tercero = resultado.IdRegistro;
                                                break;
                                        }

                                        //Enviando notificaciones segun corresponda
                                        if (id_mov_asig_unidad > 0)
                                            SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_unidad);
                                        if (id_mov_asig_operador > 0)
                                            SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_operador);
                                        if (id_mov_asig_tercero > 0)
                                            SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_tercero);

                                        //Validando Alcance y Actualización del Servicio
                                        resultado = ValidaAlcanceServicio(objMovimiento.id_movimiento, objMovimiento.id_servicio, resultado.IdRegistro, id_usuario);
                                        if (resultado.OperacionExitosa)
                                            //Confirmando acciones realizadas
                                            scope.Complete();
                                    }

                                }//using Transacción
                            }//Valida Estatus Movimiento
                            else
                            {
                                //Mostramos mensaje Error
                                resultado = new RetornoOperacion("El estatus del movimiento '" + (Movimiento.Estatus)objMovimiento.id_estatus_movimiento + "' no permite su edición");
                            }
                        }//Valida Estatus Servicio
                        else
                        {
                            //Mostramos mensaje Error
                            resultado = new RetornoOperacion("El estatus del servicio '" + objServicio.estatus + "' no permite su edición.");
                        }

                    }//using Servicio
                }
                else
                    resultado = new RetornoOperacion("No existe el movimiento");
            }//using Movimiento
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="id_mov_asig_rec"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion ValidaAlcanceServicio(int id_movimiento, int id_servicio, int id_mov_asig_rec, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion(id_mov_asig_rec);
            string configuracion_compania = "";
            
            //Validando Servicio
            using (Servicio serv = new Servicio(id_servicio))
            using (Global.Clasificacion alcance = new Global.Clasificacion(1, id_servicio, 0))
            {
                if (serv.habilitar && serv.estatus == Servicio.Estatus.Documentado && alcance.habilitar)
                {
                    configuracion_compania = SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Alcance Servicio", serv.id_compania_emisor);
                    if (!string.IsNullOrEmpty(configuracion_compania))
                    {
                        if (alcance.id_alcance_servicio == 0)
                        {
                            //Inicializando arreglo de parámetros
                            object[] param = { 59, 0, id_movimiento, 0, 0, id_servicio, id_usuario, true, null, "", "" };

                            //Establecemos Resultado
                            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
                            {
                                //Validando Datos
                                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                                {
                                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                                    {
                                        int idRegistro = 0; bool op = false;
                                        int.TryParse(dr["IdRegistro"].ToString(), out idRegistro);
                                        bool.TryParse(dr["OperacionExitosa"].ToString(), out op);
                                        if (op)
                                        {
                                            if (idRegistro > 0)
                                            {
                                                //Actualizando el Alcance
                                                retorno = alcance.EditaClasificacion(alcance.id_tabla, alcance.id_registro, alcance.id_tipo, alcance.id_flota,
                                                            alcance.id_region, alcance.id_ubicacion_terminal, alcance.id_tipo_servicio, idRegistro, alcance.id_detalle_negocio,
                                                            alcance.id_clasificacion1, alcance.id_clasificacion2, id_usuario);
                                                if (retorno.OperacionExitosa)
                                                    retorno = new RetornoOperacion(id_mov_asig_rec, "Se actualizo el Alcance del Servicio", true);
                                            }
                                            else if (idRegistro == -1)
                                                retorno = new RetornoOperacion(id_mov_asig_rec);
                                            else
                                                retorno = new RetornoOperacion(id_mov_asig_rec, dr["Mensaje"].ToString(), true);
                                        }
                                        else
                                            retorno = new RetornoOperacion("No se pudieron recuperar las Clasificaciones");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return retorno;
        }
        /// <summary>
        /// Método encargado de Insertar un  Movimiento  Asignación Recurso validando la cantidad de Unidades  y Operadores Permitidos ya en un Viaje Iniciado
        /// </summary>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="tipo_estancia">Tipo Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Tipo actualización Estantacia Inicio (Manual, GPS)</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad (Tractor, Remolque etc.)</param>
        /// <param name="id_recurso_asignado">Id de Recurso Asignado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoAsignacionRecursoParaDespacho(int id_compania_emisor, int id_movimiento, int id_parada, EstanciaUnidad.Tipo tipo_estancia, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_inicio,
                                                                            Tipo tipo_asignacion, int id_tipo_unidad, int id_recurso_asignado, int id_usuario)
        {
            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            bool validacionAsignacionActiva = false;
            //Declarando auxiliar para determinar Id de Unidad Involucrada (Id de recurso asignado directamente o por vinculación con operador)
            int id_unidad_motriz = 0;

            //Auxiliares mensaje
            int id_mov_asig_unidad = 0, id_mov_asig_tercero = 0, id_mov_asig_operador = 0;

            //Establecemos Estatus de la Asignación
            Estatus estatusAsignacion = Estatus.Iniciado;

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                if (objMovimiento.habilitar)
                {                
                    //Instanciamos Parada actual
                    using (Parada objParada = new Parada(id_parada))
                    {
                        //Obtenemos Parada anterior
                        using (Parada paradaAnterior = new Parada(Parada.BuscaParadaAnterior(objParada.id_servicio, objParada.secuencia_parada_servicio)))
                        {
                            //Validamos estatus del Movimiento
                            if ((Movimiento.Estatus)objMovimiento.id_estatus_movimiento == Movimiento.Estatus.Registrado)
                            {
                                //Instanciamos Servicio
                                using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                                {
                                    //Validamos Estatus del Servicio sea Inicicado o Documentado
                                    if (objServicio.estatus == Servicio.Estatus.Iniciado || objServicio.estatus == Servicio.Estatus.Documentado)
                                    {
                                        //De acuerdo al estatus del servicio
                                        if (objServicio.estatus == Servicio.Estatus.Documentado)
                                        {
                                            //Validamos que la parada sea la Inicial
                                            if (objParada.secuencia_parada_servicio == 1)
                                            {
                                                //Establecemos Estatus de la Asignación  a Registrado
                                                estatusAsignacion = Estatus.Registrado;
                                            }
                                            else
                                            {
                                                //Establecemos Mensaje Error
                                                resultado = new RetornoOperacion("Soló puedes asignar recursos al primer movimiento ya que el Servicio se encuentrá en estatus Documentado.");
                                            }
                                        }
                                        else if (paradaAnterior.secuencia_parada_servicio != 0)
                                        {
                                            //Validamos que la parada anterior no se encuentre Registrada
                                            if (paradaAnterior.Estatus == Parada.EstatusParada.Registrado)
                                            {
                                                //Establecemos Mensaje Resultado
                                                resultado = new RetornoOperacion("No es posible la asignación de recursos ya que la parada anterior no se encuentra iniciada");
                                            }
                                        }

                                        //Validamos Resultado 
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Obtenemos validación de la existencia de Asignación Activa
                                            validacionAsignacionActiva = ValidaAsignacionActiva(tipo_asignacion, id_recurso_asignado, id_movimiento);

                                            //Creamos la transacción 
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Si no existe la Asignacion
                                                if (!validacionAsignacionActiva)
                                                {
                                                    //Validamos tipo de recurso a ingresar (Tercero, Unidad, Remolque) de acuerdo a los tipos de recurso existentes al movimiento.
                                                    resultado = ValidaTipoRecurso(id_movimiento, estatusAsignacion, tipo_asignacion, id_tipo_unidad, id_recurso_asignado);

                                                    //Validamos Resultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Validamos el limite de recursos que se permiten.
                                                        resultado = ValidaLimiteMovimientoRecurso(id_compania_emisor, id_movimiento, tipo_asignacion, id_tipo_unidad, id_recurso_asignado);

                                                    }
                                                }

                                                //Si no hay errores en validaciones
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Si el Tipo  de Asignacion sea Unidad
                                                    if (tipo_asignacion == Tipo.Unidad)
                                                    {
                                                        //Declaramos Variable para asignar Estancia
                                                        int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_recurso_asignado);

                                                        //Validamos que el Tipo de Unidad sea motriz para asignar el Operador correspondiente
                                                        using (UnidadTipo unidadTipo = new UnidadTipo(id_tipo_unidad))
                                                        {
                                                            if (unidadTipo.bit_motriz)
                                                            {
                                                                id_unidad_motriz = id_recurso_asignado;
                                                                //Insertamos operador asignado a la Unidad
                                                                resultado = insertaOperadorAsignadoAUnidad(id_compania_emisor, id_movimiento, id_recurso_asignado, estatusAsignacion, id_parada, objParada.fecha_llegada, id_usuario);

                                                                //Recuperando asignación de operador
                                                                id_mov_asig_operador = resultado.IdRegistro;
                                                            }
                                                        }

                                                        //Si no existe la Asignación
                                                        if (!validacionAsignacionActiva)
                                                        {
                                                            //Insertamos Estacias y Actualizamos Estatus de la Unidad Ocupado sólo para Servicios Iniciados.
                                                            if ((Servicio.Estatus)objServicio.estatus == Servicio.Estatus.Iniciado)
                                                            {
                                                                //Validamos Insercción soló para Tractores
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Instanciamos Unidad
                                                                    using (Unidad objUnidad = new Unidad(id_recurso_asignado))
                                                                    {
                                                                        //Validamos insercción de estancia en caso de ser necesario
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Actualizamos Estatus de la unidad
                                                                            resultado = objUnidad.ActualizaEstatusAParadaOcupado(id_parada, id_estancia, id_usuario);

                                                                            //Validamos Actualizacion de la Unidad
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Actualizamos atributos de la Unidad
                                                                                if (objUnidad.ActualizaAtributosInstancia())
                                                                                {
                                                                                    //Actualizamos Atributos principales de la Unidad
                                                                                    resultado = objUnidad.ActualizaEstanciaYMovimiento(id_estancia, 0, objParada.fecha_llegada, id_usuario);
                                                                                    //Validamos Resultado
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        //Instanciamos Estancia
                                                                                        using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(id_estancia))
                                                                                        {
                                                                                            //Validamos que el Id de Parada sea Diferente al actual
                                                                                            if (objEstanciaUnidad.id_parada != id_parada)
                                                                                            {
                                                                                                //Actualizamos parada de la Estancia
                                                                                                resultado = objEstanciaUnidad.CambiaParadaEstanciaUnidad(id_parada, id_usuario);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    //Establecemos Mensaje 
                                                                                    resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + objUnidad.numero_unidad + ".");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else //Si el Tipo es Operador
                                                    {
                                                        if (tipo_asignacion == Tipo.Operador)
                                                        {
                                                            //Validando Servicio
                                                            if (objMovimiento.id_servicio > 0)

                                                                //Validando Evidencias
                                                                resultado = SAT_CL.ControlEvidencia.Reportes.ValidaViajesSinEvidenciasOperador(id_compania_emisor, id_recurso_asignado);
                                                            else
                                                                //Instanciando Retorno
                                                                resultado = new RetornoOperacion("");

                                                            //Validando Resultado Evidencias
                                                            if (!resultado.OperacionExitosa)
                                                            {
                                                                //Insertamos Unidad Asignada al Operador
                                                                resultado = insertaUnidadAsignadoAOperador(id_compania_emisor, id_movimiento, id_parada, objParada.fecha_llegada, tipo_estancia, tipo_actualizacion_inicio, id_recurso_asignado,
                                                                                                       estatusAsignacion, id_tipo_unidad, id_usuario);

                                                                //Recuperando asignación de tractor
                                                                id_mov_asig_unidad = resultado.IdRegistro;

                                                                //Obteniendo unidad asignada al operador
                                                                id_unidad_motriz = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso_asignado);

                                                                //Validamos  que no exista la asignación
                                                                if (!validacionAsignacionActiva)
                                                                {
                                                                    //Actualiza Estatus del Operador a Ocupado, sólo para Servicios Iniciados
                                                                    if ((Servicio.Estatus)objServicio.estatus == Servicio.Estatus.Iniciado)
                                                                    {
                                                                        //Validamos Insercción
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Instanciamos Operador
                                                                            using (Operador objOperador = new Operador(id_recurso_asignado))
                                                                            {
                                                                                //Actualizamos Estatus
                                                                                resultado = objOperador.ActualizaOperadorAParadaOcupado(id_parada, id_usuario);

                                                                                //Validamos Resultado
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Actualizamos Atributos
                                                                                    if (objOperador.ActualizaAtributosInstancia())
                                                                                    {

                                                                                        //Actualizamos atributos de Ubicación de la Unidad
                                                                                        resultado = objOperador.ActualizaParadaYMovimiento(objParada.id_parada, 0, objParada.fecha_llegada, id_usuario);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Establecemos Mensaje
                                                                                        resultado = new RetornoOperacion("No se encontró datos complementarios del Operador" + objOperador.nombre + ".");
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }//Fin validación de estatus Servicio
                                                                }
                                                            }
                                                        }//Validación tipo de asignación.
                                                    }
                                                }

                                                //Si la Validación fue correcta y no existe la asignación 
                                                if (resultado.OperacionExitosa && !validacionAsignacionActiva)
                                                {
                                                    //Instanciando Movimiento
                                                    using (Movimiento mov = new Movimiento(id_movimiento))
                                                    {
                                                        //Validando que Exista el Movimiento
                                                        if (mov.habilitar)
                                                        {
                                                            //Instanciando Parada Origen
                                                            using (Parada p1 = new Parada(mov.id_parada_origen))
                                                            {
                                                                //Validando que Exista la Parada
                                                                if (p1.id_parada > 0)
                                                                {
                                                                    //Inicializando arreglo de parámetros
                                                                    object[] param = { 1, 0, id_movimiento, estatusAsignacion, tipo_asignacion, id_recurso_asignado, id_usuario, true, null, "", "" };

                                                                    //Establecemos Resultado
                                                                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                                                                    ////Validando que no exista una Asignación en la misma Ubicación
                                                                    //if (!(MovimientoAsignacionRecurso.ObtieneAsignacionRegistrada(tipo_asignacion, id_recurso_asignado, p1.id_ubicacion) > 0))
                                                                    //{
                                                                        
                                                                    //}
                                                                    //else
                                                                    //    //Instanciando Excepción
                                                                    //    resultado = new RetornoOperacion("Este recurso ya tiene una asignación pendiente con esta ubicación como origen.");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //Validamos Resultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Si se asignó un recurso unidad motriz
                                                    if (id_unidad_motriz > 0)
                                                        //Realizando envío de información a plataforma de tercero
                                                        Movimiento.ActualizaPlataformaTerceros(id_movimiento, id_usuario);

                                                    //Determinando tipo de asignación final
                                                    switch (tipo_asignacion)
                                                    {
                                                        case Tipo.Operador:
                                                            id_mov_asig_operador = resultado.IdRegistro;
                                                            break;
                                                        case Tipo.Unidad:
                                                            id_mov_asig_unidad = resultado.IdRegistro;
                                                            break;
                                                        case Tipo.Tercero:
                                                            id_mov_asig_tercero = resultado.IdRegistro;
                                                            break;
                                                    }

                                                    //Enviando notificaciones segun corresponda
                                                    if (id_mov_asig_unidad > 0)
                                                        SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_unidad);
                                                    if (id_mov_asig_operador > 0)
                                                        SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_operador);
                                                    if (id_mov_asig_tercero > 0)
                                                        SAT_CL.Global.NotificacionPush.Instance.NuevoServicioAsignado(id_mov_asig_tercero);

                                                    //Confirmando acciones realizadas
                                                    scope.Complete();
                                                }
                                            }
                                        }//using transacción
                                    }//fin validación estatus servicio
                                    else
                                    {
                                        //Establecemos Mensaje
                                        resultado = new RetornoOperacion("El estatus del servicio '" + (Servicio.Estatus)objServicio.estatus + "' no permite su edición.");
                                    }
                                }//using servicio
                            }//fin validación estatus Movimiento
                            else
                            {
                                //Establecemos Mensaje
                                resultado = new RetornoOperacion("El estatus del movimiento '" + objMovimiento.EstatusMovimiento + "' no permite su edición.");
                            }
                        }
                    }
                }
            }//using movimiento
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de editar un Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <param name="estatus_asignacion">Estatus de la Asignación entre el recurso y Movimiento</param>
        /// <param name="tipo_asignacion">Tipo de Asignación entre el recurso</param>
        /// <param name="id_recurso_asignado">Id de Recurso Asignado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaMovimientoAsignacionRecurso(int id_movimiento, Estatus estatus_asignacion, Tipo tipo_asignacion,
                                                                 int id_recurso_asignado, int id_usuario)
        {
            return this.editaMovimientoAsignacionRecurso(id_movimiento, estatus_asignacion, tipo_asignacion, id_recurso_asignado, id_usuario, this._habilitar);
        }
        
        [Obsolete]
        public static RetornoOperacion ValidaLimiteMovimientoRecurso2(int id_compania_emisor, int id_movimiento, Tipo tipo_asignacion, int id_tipo_unidad, int id_recurso_asignado)
        {

            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos variables para almacenar el conte de asignaciones
            int conteoUnidad = 0, conteoOperador = 0, conteoTercero = 0;

            //Obtenemos el total de veces que elrecurso que ha sido asignado.
            obtieneTotalAsignacionesRecurso(id_movimiento, id_tipo_unidad, out conteoUnidad, out conteoOperador, out conteoTercero);

            //Instanciamos Configuración Disponible por la Compañia
            using (ConfiguracionAsignacionRecurso objConfiguracionAsignacionRecurso = new ConfiguracionAsignacionRecurso(ConfiguracionAsignacionRecurso.TipoRegistro.IdCompania, id_compania_emisor))
            {
                //Validamos que exista Configuración
                if (objConfiguracionAsignacionRecurso.id_configuracion_asignacion_recurso > 0)
                {
                    //De acuerdo al tipo de recurso que se desea ingresa (Unidad, Remolque, Tercero).
                    switch (tipo_asignacion)
                    {
                        //En caso de Operador
                        case Tipo.Operador:
                            //Validamos Asignación de Operador
                            resultado = validacionOperador(conteoOperador, objConfiguracionAsignacionRecurso);
                            break;
                        //En caso de Tercero
                        case Tipo.Tercero:
                            //Validamos que no exista un Tercero
                            if (conteoTercero >= 1)
                            {
                                //Mostramos Error
                                resultado = new RetornoOperacion("No es posible registrar el Tercero, la cantidad máxima de asignaciones se ha alcanzado.");
                            }

                            break;
                        //En caso de Unidad
                        case Tipo.Unidad:
                            //Si el Tipo de Configuración es Unidad
                            resultado = validacionUnidad(id_tipo_unidad, id_recurso_asignado, conteoUnidad, objConfiguracionAsignacionRecurso);
                            break;
                    }
                }
                else
                {
                    resultado = new RetornoOperacion("No existe configuración para asignación del recurso.");
                }
            }
            return resultado;
        }
        /// <summary>
        /// Validamos el limite de recursos que se permiten al movimiento.
        /// </summary>
        /// <param name="id_compania_emisor">Id compañia Emisor</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo_asignacion">Tipo Asignación (Unidad, Operador, Tercero)</param>
        /// <param name="id_tipo_unidad">Tipo Unidad (Remolque, Tractor, etc.)</param>
        /// <param name="id_recurso_asignado">Id Recurso Asignado</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaLimiteMovimientoRecurso(int id_compania_emisor, int id_movimiento, Tipo tipo_asignacion, int id_tipo_unidad, int id_recurso_asignado)
        {

            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos variables para almacenar el conte de asignaciones
            int conteoUnidad = 0, conteoOperador = 0, conteoTercero = 0;

            //Obtenemos el total de veces que elrecurso que ha sido asignado.
            obtieneTotalAsignacionesRecurso(id_movimiento, id_tipo_unidad, out conteoUnidad, out conteoOperador, out conteoTercero);

            //Instanciamos Configuración Disponible por la Compañia
            List<ConfiguracionAsignacionRecurso> configuraciones = ConfiguracionAsignacionRecurso.ObtieneConfiguracionAsignacionRecurso(ConfiguracionAsignacionRecurso.TipoRegistro.IdCompania, id_compania_emisor);
            if (configuraciones.Count > 0)
            {
                //De acuerdo al tipo de recurso que se desea ingresa (Unidad, Remolque, Tercero).
                switch (tipo_asignacion)
                {
                    //En caso de Operador
                    case Tipo.Operador:
                        //Validamos Asignación de Operador
                        resultado = validacionOperador(conteoOperador, configuraciones);
                        break;
                    //En caso de Tercero
                    case Tipo.Tercero:
                        //Validamos que no exista un Tercero
                        if (conteoTercero >= 1)
                        {
                            //Mostramos Error
                            resultado = new RetornoOperacion("No es posible registrar el Tercero, la cantidad máxima de asignaciones se ha alcanzado.");
                        }

                        break;
                    //En caso de Unidad
                    case Tipo.Unidad:
                        //Si el Tipo de Configuración es Unidad
                        resultado = validacionUnidad(id_tipo_unidad, id_recurso_asignado, conteoUnidad, configuraciones);
                        break;
                }
            }
            else
                resultado = new RetornoOperacion("No existe configuraciones para la asignación del recurso.");


            return resultado;
        }

        /// <summary>
        /// Terminamos Asignación de Recurso
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaMovimientoAsignacionRecurso(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que se encuentre en estatus Iniciado.
            if ((Estatus)this._id_estatus_asignacion == Estatus.Iniciado)
            {
                //Actualizamos estatus
                resultado = this.editaMovimientoAsignacionRecurso(this._id_movimiento, Estatus.Terminado, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado,
                                                              id_usuario, this._habilitar);
            }
            else
            {
                resultado = new RetornoOperacion("Sólo se puede terminar la asignación en estatus registrado.");
            }

            //Decolvemos Resultado
            return resultado;
        }


        /// <summary>
        /// Cancela la Asignación
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaMovimientoAsignacionRecurso(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que se encuentre en estatus Iniciado.
            if ((Estatus)this._id_estatus_asignacion != Estatus.Terminado)
            {
                //Instanciamos Movimiento
                using (Movimiento objMovimiento = new Movimiento(this._id_movimiento))
                {
                    if (objMovimiento.habilitar)
                    {
                        //Si existe el Servicio
                        if (objMovimiento.id_servicio != 0)
                        {
                            //Validamos que no existan Anticipos
                            resultado = validaAnticipos(objMovimiento.id_servicio, (MovimientoAsignacionRecurso.Tipo)this._id_tipo_asignacion == MovimientoAsignacionRecurso.Tipo.Operador ? this._id_recurso_asignado : 0,
                               (MovimientoAsignacionRecurso.Tipo)this._id_tipo_asignacion == MovimientoAsignacionRecurso.Tipo.Unidad ? this._id_recurso_asignado : 0,
                               (MovimientoAsignacionRecurso.Tipo)this._id_tipo_asignacion == MovimientoAsignacionRecurso.Tipo.Tercero ? this._id_recurso_asignado : 0, id_usuario);
                        }
                        else if (!DetalleLiquidacion.ValidaAnticiposMovimiento(this._id_movimiento).OperacionExitosa)
                        {
                            //Mostrando Error
                            resultado = new RetornoOperacion("Existen Anticipos ligado al Movimiento.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("No se puede recuperar el Movimiento deseado");
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                    //Actualizamos estatus
                    resultado = this.editaMovimientoAsignacionRecurso(this._id_movimiento, Estatus.Cancelado, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado,
                                                                  id_usuario, this._habilitar);
            }
            else
                resultado = new RetornoOperacion("El estatus de la asignación no permite la cancelación.");

            //Decolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Iniciar una Asignación Recurso.
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaMovimientoAsignacionRecurso(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Estatus
            if (Estatus.Iniciado != (Estatus)this._id_estatus_asignacion)
            {
                //Actualizamos Estatus
                resultado = this.editaMovimientoAsignacionRecurso(this._id_movimiento, Estatus.Iniciado, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado,
                                                            id_usuario, this._habilitar);
            }
            else
                //Establecemos error
                resultado = new RetornoOperacion("La Asignación ya se encuentra Iniciada.");

            //Decolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Cambiar una Asignación Recurso a Registrada.
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CambiaMovimientoAsignacionRecursoARegistrado(int id_usuario)
        {

            //Actualizamos Estatus
            return this.editaMovimientoAsignacionRecurso(this._id_movimiento, Estatus.Registrado, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado,
                                                               id_usuario, this._habilitar);
        }

        /// <summary>
        /// Valida Asignaciones Ligadas al recurso para su liberación
        /// </summary>
        /// <returns></returns>
        public static RetornoOperacion ValidaLiberacionRecurso(MovimientoAsignacionRecurso.Tipo tipo, int id_recurso)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Validamos Tipo de Asignación
            if (tipo == Tipo.Unidad)
            {
                //Obtenemos el Operador Asignado
                int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(id_recurso);

                //Si existe el Operador
                if (id_operador > 0)
                {
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(id_operador))
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("El operador " + objOperador.nombre + " será liberado ya que se encuentra asignado a la unidad. ¿Desea contiuar?");
                    }
                }
            }
            else
            {
                //Validamos Tipo de Asignación Operador
                if (tipo == Tipo.Operador)
                {
                    //Obtenemos la Unidad Asignada al Operador
                    int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso);

                    //Validamos Id Recurso
                    if (id_unidad > 0)
                    {
                        //Instanciamos Unidad
                        using (Unidad objUnidad = new Unidad(id_unidad))
                        {
                            //Establecemos Mensaje Error
                            resultado = new
                          RetornoOperacion("La unidad " + objUnidad.numero_unidad.ToString() + " será liberada  ya que se encuentrá asignada al operador. ¿Desea continuar?");
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos la Deshabilitación de Recurso
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaDeshabilitacionRecursos()
        {
            //Declaramos Id Recurso
            int id_recurso = 0;
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Instanciamos Mpovimiento
            using (Movimiento objMovimiento = new Movimiento(this._id_movimiento))
            {
                //Valiamos existencia de Movimiento
                if (objMovimiento.habilitar)
                {
                    //Validamos Tipo de Asignación
                    if ((Tipo)this._id_tipo_asignacion == Tipo.Unidad)
                    {
                        //Obtenemos el Operador Asignado
                        id_recurso = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(this._id_recurso_asignado);

                        //Validamos Recurso
                        if (id_recurso > 0)
                        {
                            //Obtenemos la asignación recurso
                            if (ObtieneAsignacionRegistradaRecurso(objMovimiento.id_servicio, id_recurso, Tipo.Operador) > 0)
                            {
                                //Instanciamos Operador
                                using (Operador objOperador = new Operador(id_recurso))
                                {
                                    //Establecemos Mensaje Error
                                    resultado = new RetornoOperacion("Se eliminará el operador " + objOperador.nombre + " ya que se encuentra asignado a la Unidad. ¿Desea continuar? ");
                                }
                            }
                        }
                    }
                    else
                    {
                        //Validamos Tipo de Asignación Operador
                        if ((Tipo)this._id_tipo_asignacion == Tipo.Operador)
                        {
                            //Obtenemos la Unidad Asignada al Operador
                            id_recurso = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(this._id_recurso_asignado);

                            //Validamos Id Recurso
                            if (id_recurso > 0)
                            {
                                //Obtenemos la asignación recurso
                                if (ObtieneAsignacionRegistradaRecurso(objMovimiento.id_servicio, id_recurso, Tipo.Unidad) > 0)
                                {
                                    //Instanciamos Unidad
                                    using (Unidad objUnidad = new Unidad(id_recurso))
                                    {
                                        //Establecemos Mensaje Error
                                        resultado = new RetornoOperacion("Se eliminará la unidad " + objUnidad.numero_unidad + " ya que se encuentra asignada al Operador. ¿Desea continuar? ");

                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Valida las estancias de la Asignación del  Recurso
        /// </summary>
        /// <returns></returns>
        public static RetornoOperacion ValidaEstanciaAsignacionRecurso(MovimientoAsignacionRecurso.Tipo tipo, int id_recurso)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos Tipo de Asignación
            if (tipo == Tipo.Unidad)
            {
                //Obtenemos Estancia de la Unidad
                if (EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_recurso) == 0)
                {

                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_recurso))
                    {
                        //Validamos que el estatus de la unidad no sea tránsito
                        if ((Unidad.Estatus)objUnidad.id_estatus_unidad != Unidad.Estatus.Transito)
                        {
                            //Establecemos Mensaje Error
                            resultado = new RetornoOperacion("No se encontró la estancia de la Unidad " + objUnidad.numero_unidad + ". ¿Desea añadir la estancia para crear la asignación?");
                        }
                    }
                }
            }
            else
            {
                //Validamos Tipo de Asignación Operador
                if (tipo == Tipo.Operador)
                {
                    //Obtenemos la Unidad Asignada al Operador
                    int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso);

                    //Validamos Id Recurso
                    if (id_unidad > 0)
                    {
                        //Validamos Estancia
                        if (EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_unidad) == 0)
                        {
                            //Instanciamos Unidad
                            using (Unidad objUnidad = new Unidad(id_unidad))
                            {
                                //Validamos que el estatus de la unidad no sea tránsito
                                if ((Unidad.Estatus)objUnidad.id_estatus_unidad != Unidad.Estatus.Transito)
                                {
                                    //Establecemos Mensaje Error
                                    resultado = new
                                  RetornoOperacion("No se encontró la estancia de la Unidad " + objUnidad.numero_unidad.ToString() + ". ¿Desea añadir la estancia para crear la asignación?");
                                }
                            }
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Valida Asignaciones Ligadas al recurso
        /// </summary>
        /// <returns></returns>
        public static RetornoOperacion ValidaAsignacionesLigadasAlRecurso(MovimientoAsignacionRecurso.Tipo tipo, int id_recurso)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Validamos Tipo de Asignación
            if (tipo == Tipo.Unidad)
            {
                //Obtenemos el Operador Asignado
                int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(id_recurso);

                //Si existe el Operador
                if (id_operador > 0)
                {
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(id_operador))
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("Se añadirá el operador " + objOperador.nombre + " ya que se encuentrá asignado a la unidad. ¿Desea contiuar?");
                    }
                }
            }
            else
            {
                //Validamos Tipo de Asignación Operador
                if (tipo == Tipo.Operador)
                {
                    //Obtenemos la Unidad Asignada al Operador
                    int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(id_recurso);

                    //Validamos Id Recurso
                    if (id_unidad > 0)
                    {
                        //Instanciamos Unidad
                        using (Unidad objUnidad = new Unidad(id_unidad))
                        {
                            //Establecemos Mensaje Error
                            resultado = new
                          RetornoOperacion("Se añadirá la unidad " + objUnidad.numero_unidad.ToString() + " ya que se encuentrá asignada al operador. ¿Desea continuar?");
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilitamos Movimiento Asignación Recurso
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoAsignacionRecursoDespacho(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Estatus
            if (((Estatus)this._id_estatus_asignacion == Estatus.Cancelado) || ((Estatus)this._id_estatus_asignacion == Estatus.Registrado))
            {
                //Deshabilitamos Asignación
                resultado = editaMovimientoAsignacionRecurso(this._id_movimiento, (Estatus)this._id_estatus_asignacion, (Tipo)this._id_tipo_asignacion,
                            this._id_recurso_asignado, id_usuario, false);
            }
            else
            {
                //Instanciamos Movimiento
                using (Movimiento objMovimiento = new Movimiento(this._id_movimiento))
                {
                    //Establecemos Mensaje Resultado
                    resultado = new RetornoOperacion("Existen asignaciones activas al movimiento " + objMovimiento.descripcion + ", para continuar cancele las asignaciones.");
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilitamos la asignación ligada al recurso y la edición de la misma
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientosAsignacionesRecurso(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            int id_asignacion = 0;
            Tipo tipo_asignacion_alternativa = Tipo.Unidad;
            int id_recurso = 0;

            //Instanciamos Mpovimiento
            using (Movimiento objMovimiento = new Movimiento(this._id_movimiento))
            {
                //Valiamos existencia de Movimiento
                if (objMovimiento.habilitar)
                {
                    //Creamos la transacción 
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        // Validamos que el estatus este Registrado
                        if ((Estatus)this._id_estatus_asignacion == Estatus.Registrado)
                        {
                            //Validamos Tipo de Asignación
                            if ((Tipo)this._id_tipo_asignacion == Tipo.Unidad)
                            {
                                //Obtenemos el Operador Asignado
                                id_recurso = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(this._id_recurso_asignado);

                                //establecemos tipo de asignación que se encuentrá ligada al recurso  que se desea eliminar
                                tipo_asignacion_alternativa = Tipo.Operador;
                            }
                            else
                            {
                                //Validamos Tipo de Asignación Operador
                                if ((Tipo)this._id_tipo_asignacion == Tipo.Operador)
                                {
                                    //Obtenemos la Unidad Asignada al Operador
                                    id_recurso = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(this._id_recurso_asignado);

                                    //Realizando envio de notificación al recurso asignado
                                  //  Global.NotificacionPush.Instance.EliminaAsignacionServicio(this._id_movimiento_asignacion_recurso, MovimientoAsignacionRecurso.ValidaViajeActivo(this._id_tipo_asignacion, this._id_recurso_asignado, this._id_movimiento));


                                }
                            }
                            //Si existe el recurso Unidad/operador
                            if (id_recurso > 0)
                            {
                                //Obtenemos la asignación recurso ligado al recurso actual que se desea eliminar.
                                id_asignacion = ObtieneAsignacionRegistradaRecurso(objMovimiento.id_servicio, id_recurso, tipo_asignacion_alternativa);

                                //Si existe asignación
                                if (id_asignacion > 0)
                                {
                                    //Instanciamos Asignación 
                                    using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso(id_asignacion))
                                    {
                                        //Validamos el  Tipo de Asignación es Operador para Envió de Notificación
                                        //if ((MovimientoAsignacionRecurso.Tipo)objMovimientoAsignacionRecurso.id_tipo_asignacion == MovimientoAsignacionRecurso.Tipo.Operador)
                                        //{
                                        //    Realizando envio de notificación al recurso asignado
                                        //    Global.NotificacionPush.Instance.EliminaAsignacionServicio(objMovimientoAsignacionRecurso.id_movimiento_asignacion_recurso, MovimientoAsignacionRecurso.ValidaViajeActivo(objMovimientoAsignacionRecurso.id_tipo_asignacion, objMovimientoAsignacionRecurso._id_recurso_asignado, objMovimientoAsignacionRecurso.id_movimiento));
                                        //}

                                        //Deshabilitamos Asignación ligado al recurso que se desea eliminar.
                                        resultado = objMovimientoAsignacionRecurso.editaMovimientoAsignacionRecurso(objMovimientoAsignacionRecurso.id_movimiento,
                                                   (Estatus)objMovimientoAsignacionRecurso.id_estatus_asignacion, (Tipo)objMovimientoAsignacionRecurso.id_tipo_asignacion,
                                                    objMovimientoAsignacionRecurso.id_recurso_asignado, id_usuario, false);

                                    }
                                }
                            }
                            //Validamos deshabilitación de recursos ligados
                            if (resultado.OperacionExitosa)
                            {
                                //Deshabilitamos la Asignación Recurso
                                resultado = this.editaMovimientoAsignacionRecurso(this._id_movimiento, (Estatus)this._id_estatus_asignacion, (Tipo)this._id_tipo_asignacion,
                                                                                  this._id_recurso_asignado, id_usuario, false);
                            }

                            //Si el resultado es Exitoso
                            if (resultado.OperacionExitosa)
                            {
                                //Terminamos Transacción
                                scope.Complete();
                            }
                        }//using transacción
                        else
                        {
                            resultado = new RetornoOperacion("El estatus de la asignación no permite su edición.");
                        }
                    }//fin validación movimiento
                }
                else
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion("No se encontró datos complementarios movimiento.");
                }
            }//using movimiento
            return resultado;
        }

        /// <summary>
        /// Cancela la Asignación del  Recurso  
        /// </summary>
        /// <param name="tipo_actualizacion_fin">Tipo actualización Fin</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CancelaMovimientoAsignacionRecursoParaDespacho(EstanciaUnidad.TipoActualizacionFin tipo_actualizacion_fin,
                                                                                 int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            int id_asignacion = 0;
            int id_recurso = 0;
            Tipo tipo_asignacion_alternativa = Tipo.Unidad;

            //Instanciamos Mpovimiento
            using (Movimiento objMovimiento = new Movimiento(this._id_movimiento))
            {
                //Valiamos existencia de Movimiento
                if (objMovimiento.habilitar)
                {
                    //Instanciamos Servicio
                    using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                    {
                        //Validamos Estatus del Servicio  corresponda a Iniciado o Documentado
                        if ((Servicio.Estatus)objServicio.estatus == Servicio.Estatus.Iniciado || (Servicio.Estatus)objServicio.estatus == Servicio.Estatus.Documentado)
                        {
                            //Validamos Estatus de la asignación corresponda a Iniciado o Registrado
                            if ((Estatus)this._id_estatus_asignacion == Estatus.Iniciado || (Estatus)this._id_estatus_asignacion == Estatus.Registrado)
                            {
                                //Creamos la transacción 
                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Validando anticipos
                                    resultado = validaAnticipos(objServicio.id_servicio, (Tipo)this._id_tipo_asignacion == Tipo.Operador ? _id_recurso_asignado : 0, (Tipo)this._id_tipo_asignacion == Tipo.Unidad ? _id_recurso_asignado : 0, (Tipo)this._id_tipo_asignacion == Tipo.Tercero ? _id_recurso_asignado : 0, id_usuario);

                                    //Validando que no existan anticipos asignados
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Validamos Tipo de Asignación
                                        if ((Tipo)this._id_tipo_asignacion == Tipo.Unidad)
                                        {
                                            //Obtenemos el Operador Asignado
                                            id_recurso = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(this._id_recurso_asignado);

                                            //Establecemos Variable para Almacenar Tipo de Asignación Alternativa
                                            tipo_asignacion_alternativa = Tipo.Operador;

                                            //Actualizamos Estatus de Operador y Unidad a Disponibles sólo para servicios Iniciados
                                            if (objServicio.estatus == Servicio.Estatus.Iniciado)
                                            {
                                                //Variable para declarar la Parada Comodin
                                                int idParadaNuevo = 0;
                                                DateTime fecha_actualizacion = DateTime.MinValue;
                                                //Instanciamos Unidad
                                                using (Unidad objUnidad = new Unidad(this._id_recurso_asignado))
                                                {
                                                    //Actualizamos Estatus
                                                    resultado = objUnidad.ActualizaEstatusADisponible(id_usuario);

                                                    //Validamos resultado
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Instanciamos Parada Origen
                                                        using (Parada objParada = new Parada(objMovimiento.id_parada_origen))
                                                        {
                                                            //Asignamos Fecha de Actualización  a variables
                                                            fecha_actualizacion = objParada.fecha_llegada;
                                                            //Verificando existencia de parada alterna en la ubicación actual (parada comodín para asignación de estancias de unidades)
                                                            idParadaNuevo = Parada.ObtieneParadaComodinUbicacion(objParada.id_ubicacion, true, id_usuario);

                                                            //Validamos Existencia de Parada Nueva
                                                            if (idParadaNuevo != 0)
                                                            {
                                                                //Validamos resultado
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Obtenemos Estancia de la Unidad
                                                                    using (EstanciaUnidad objEstancia = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(this._id_recurso_asignado)))
                                                                    {
                                                                        //Modificamos el Id de Parada de la Estancia actual de la unidad (parada alterna)
                                                                        resultado = objEstancia.CambiaParadaEstanciaUnidad(idParadaNuevo, id_usuario);

                                                                        //Validamos Resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Actualizamos atributos de la Unidad
                                                                            if (objUnidad.ActualizaAtributosInstancia())
                                                                            {
                                                                                //Actualizamos Atributos principales de la Unidad
                                                                                objUnidad.ActualizaEstanciaYMovimiento(objEstancia.id_estancia_unidad, 0, fecha_actualizacion, id_usuario);
                                                                            }
                                                                            else
                                                                            {
                                                                                //Mostramos Error
                                                                                resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + objUnidad.numero_unidad + ".");
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                //Mostramos Mensaje Error
                                                                resultado = new RetornoOperacion("Error al recuperar o crear la parada alterna.");
                                                        }
                                                    }
                                                }
                                                //Si existe el Recurso
                                                if (id_recurso > 0)
                                                {
                                                    //Validamos Actualizaciones de la Unidad
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Instanciamos Operador
                                                        using (Operador objOperador = new Operador(id_recurso))
                                                        {
                                                            //Actualizamos Estatus
                                                            resultado = objOperador.ActualizaEstatusADisponible(id_usuario);

                                                            //Validamos Resultado 
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Refrecamos Atributos del Operador
                                                                if (objOperador.ActualizaAtributosInstancia())
                                                                {
                                                                    //Actualizamos Operador
                                                                    resultado = objOperador.ActualizaParadaYMovimiento(idParadaNuevo, 0, fecha_actualizacion, id_usuario);

                                                                    //Obtenemos el recurso Unidad /Operador ligado al recurso que se desea cancelar.
                                                                    int id_asignacion_operador = ObtieneAsignacionRegistradaRecurso(objMovimiento.id_servicio, id_recurso, tipo_asignacion_alternativa);

                                                                    ////Obtenemos Si la Asignación es de un Viaje Activo
                                                                    //validaViajeActivoRecurso= ValidaViajeActivo(2, id_recurso, this._id_movimiento);


                                                                }
                                                                else
                                                                {
                                                                    //Establecemos Mensaje Error
                                                                    resultado = new RetornoOperacion("No se encontró datos complementarios del Operador " + objOperador.nombre + ".");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Validamos Tipo de Asignación Operador
                                            if ((Tipo)this._id_tipo_asignacion == Tipo.Operador)
                                            {
                                                //Obtenemos la Unidad Asignada al Operador
                                                id_recurso = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(this._id_recurso_asignado);

                                                //Actualizamos Estatus de Operador y Unidad a Disponibles sólo para servicios Iniciados
                                                if (objServicio.estatus == Servicio.Estatus.Iniciado)
                                                {
                                                    //Atributo para almacenar el Id de Parada Comodin
                                                    int idParadaNuevo = 0;
                                                    DateTime fecha_actualizacion = DateTime.MinValue;

                                                    //Si existe el Recurso
                                                    if (id_recurso > 0)
                                                    {
                                                        //Instanciamos Unidad
                                                        using (Unidad objUnidad = new Unidad(id_recurso))
                                                        {
                                                            //Actualizamos Estatus
                                                            resultado = objUnidad.ActualizaEstatusADisponible(id_usuario);

                                                            //Validamos resultado
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Instanciamos Parada Origen
                                                                using (Parada objParada = new Parada(objMovimiento.id_parada_origen))
                                                                {
                                                                    //Asignamos Fecha de actualización a la variable
                                                                    fecha_actualizacion = objParada.fecha_llegada;
                                                                    //Verificando existencia de parada alterna en la ubicación actual (parada comodín para asignación de estancias de unidades)
                                                                    idParadaNuevo = Parada.ObtieneParadaComodinUbicacion(objParada.id_ubicacion, true, id_usuario);
                                                                    //Valida que exista la parada
                                                                    if (idParadaNuevo != 0)
                                                                    {
                                                                        //Validamos resultado
                                                                        if (resultado.OperacionExitosa)
                                                                        {
                                                                            //Obtenemos Estancia de la Unidad
                                                                            using (EstanciaUnidad objEstancia = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(id_recurso)))
                                                                            {
                                                                                //Modificamos el Id de Parada de la Estancia actual de la unidad (parada alterna)
                                                                                resultado = objEstancia.CambiaParadaEstanciaUnidad(idParadaNuevo, id_usuario);

                                                                                //Validamos Resultado 
                                                                                if (resultado.OperacionExitosa)
                                                                                {
                                                                                    //Actualizamos Atributos de la Unidad
                                                                                    if (objUnidad.ActualizaAtributosInstancia())
                                                                                    {
                                                                                        //Actualizamos Atributos de la Unidad
                                                                                        resultado = objUnidad.ActualizaEstanciaYMovimiento(objEstancia.id_estancia_unidad, 0, fecha_actualizacion, id_usuario);


                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        //Establecemos Mensaje 
                                                                                        resultado = new RetornoOperacion("No se encontró datos complementarios de la Unidad " + objUnidad.numero_unidad + ".");
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("Error al recuperar o crear la parada alterna.");
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Validamos Actualizaciones de la Unidad
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Instanciamos Operador
                                                        using (Operador objOperador = new Operador(this._id_recurso_asignado))
                                                        {
                                                            //Actualizamos Estatus
                                                            resultado = objOperador.ActualizaEstatusADisponible(id_usuario);

                                                            //Actualizamos Atributos del Opreador
                                                            if (objOperador.ActualizaAtributosInstancia())
                                                            {

                                                                //Actualizamos Atributos de Ubicación del Operador
                                                                resultado = objOperador.ActualizaParadaYMovimiento(idParadaNuevo, 0, fecha_actualizacion, id_usuario);

                                                            }
                                                            else
                                                            {
                                                                //Establecemos Mensaje Resultado
                                                                resultado = new RetornoOperacion("No se encontró datos complementarios del Operador " + objOperador.nombre + ".");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //Si la asignación es Tercero
                                                //Validamos que el movimiento se encuentre Registrado
                                                if ((Movimiento.Estatus)objMovimiento.id_estatus_movimiento != Movimiento.Estatus.Registrado)
                                                {
                                                    //Establecemos mensaje Resultado
                                                    resultado = new RetornoOperacion("El estatus del movimiento no permite su edición.");
                                                }
                                            }

                                        }
                                    }

                                    //Validamos Resultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Si existe el recurso Unidad/operador ligado al recurso que se desea cancelar.
                                        if (id_recurso > 0)
                                        {
                                            //Obtenemos el recurso Unidad /Operador ligado al recurso que se desea cancelar.
                                            id_asignacion = ObtieneAsignacionRegistradaRecurso(objMovimiento.id_servicio, id_recurso, tipo_asignacion_alternativa);

                                            //Si existe asignación
                                            if (id_asignacion > 0)
                                            {
                                                //Instanciamos Asignación 
                                                using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso(id_asignacion))
                                                {
                                                    //Actualizamos Estatus de Operador y Unidad a Disponibles sólo para servicios Iniciados
                                                    if (objServicio.estatus == Servicio.Estatus.Iniciado)
                                                    {

                                                        //Terminamos asignación
                                                        resultado = objMovimientoAsignacionRecurso.CancelaMovimientoAsignacionRecurso(id_usuario);

                                                    }
                                                    //Si el Servicio se encuentrá Dcumentado.
                                                    else
                                                    {
                                                        //Eliminamos  asignación
                                                        resultado = objMovimientoAsignacionRecurso.editaMovimientoAsignacionRecurso(objMovimientoAsignacionRecurso.id_movimiento, objMovimientoAsignacionRecurso.EstatusMovimientoAsignacion,
                                                                    objMovimientoAsignacionRecurso.TipoMovimientoAsignacion, objMovimientoAsignacionRecurso.id_recurso_asignado, id_usuario, false);
                                                    }
                                                }
                                            }
                                        }
                                        //Validamos deshabilitación de recursos ligados
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizamos Estatus de Operador y Unidad a Disponibles sólo para servicios Iniciados
                                            if (objServicio.estatus == Servicio.Estatus.Iniciado)
                                            {
                                                //Terminamos la Asignación Recurso
                                                resultado = CancelaMovimientoAsignacionRecurso(id_usuario);
                                            }
                                            //Si el Servicio se encuentrá Documentado.
                                            else
                                            {
                                                //eliminamos asignación
                                                resultado = editaMovimientoAsignacionRecurso(this._id_movimiento, (Estatus)this._id_estatus_asignacion, (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado,
                                                                                            id_usuario, false);
                                            }
                                        }
                                    }
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Terminamos Transacción
                                        scope.Complete();
                                    }
                                }
                            }
                            else
                            {
                                //Establecemos Mensaje
                                resultado = new RetornoOperacion("El estatus de la asignación no permite su edición.");
                            }
                        }
                        else
                        {
                            //Establecemos Mensaje
                            resultado = new RetornoOperacion("El estatus del servicio '" + (Servicio.Estatus)objServicio.estatus + "' no permite su edición.");
                        }
                    }//Using Servicio.
                }//validación Movimiento
                else
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion("No se encontró datos complementarios movimiento.");
                }
            }//using movimiento
            return resultado;
        }

        /// <summary>
        ///  Carga Movimientos Asignación ligando un Id Movimiento para visualizaciòn 
        /// </summary>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <returns></returns>
        public static DataTable CargaMovimientosAsignacionParaVisualizacion(int id_movimiento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Sevicios Asignados al Recurso (incluyendo movimientos en vacío)
        /// </summary>
        /// <param name="tipo">Tipo Recurso</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static DataTable CargaServiciosAsignadosAlRecurso(Tipo tipo, int id_recurso)
        {
            return CargaServiciosAsignadosAlRecurso(tipo, id_recurso, true);
        }
        /// <summary>
        /// Carga Sevicios Asignados al Recurso
        /// </summary>
        /// <param name="tipo">Tipo Recurso</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <param name="incluir_movimientos">True para incluir movimientos en vacío</param>
        /// <returns></returns>
        public static DataTable CargaServiciosAsignadosAlRecurso(Tipo tipo, int id_recurso, bool incluir_movimientos)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { incluir_movimientos ? 7 : 53, 0, 0, 0, tipo, id_recurso, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Sevicios Terminados Asignados al Recurso
        /// </summary>
        /// <param name="tipo">Tipo Recurso</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static DataTable CargaServiciosTerminadosAsignadosAlRecurso(Tipo tipo, int id_recurso)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 38, 0, 0, 0, tipo, id_recurso, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga unidades Iniciales del Servicio
        /// </summary>
        /// <param name="id_parada_origen">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesIniciales(int id_parada_origen)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, 0, 0, 0, 0, 0, false, null, id_parada_origen, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtenemos el Sevicio actual  que se encuentra la Unidad
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static int ObtenemosServicioActualUnidad(int id_unidad)
        {
            //Definiendo objeto de retorno
            int id_servicio = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, 0, 0, 0, id_unidad, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_servicio = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("IdServicio")).FirstOrDefault();

                //Devolviendo resultado
                return id_servicio;
            }
        }

        /// <summary>
        /// Carga la unidad que actualmente se asignara a la Parada pero se encuentran en otra Ubicación
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_ubicacion">Id Ubicación</param>
        /// <param name="id_unidad">Obtiene la unidad asignada a la parada que se encuentra en otra ubicación</param>
        /// <param name="id_ubicacion_unidad">Obtiene la ubicación de la unidad asignada a la parada</param>
        /// <returns></returns>
        public static void CargaRecursoAsignadoAParadaDifUbicacion(int id_parada, int id_ubicacion, out  int id_unidad, out int id_ubicacion_unidad)
        {
            //Definiendo objeto de retorno
            id_unidad = 0; id_ubicacion_unidad = 0;


            //Inicializando arreglo de parámetros
            object[] param = { 10, 0, 0, 0, 0, 0, 0, false, null, id_parada, id_ubicacion };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_unidad = (from DataRow r in ds.Tables["Table"].Rows
                                 select r.Field<int>("IdUnidad")).FirstOrDefault();
                //Asignando a objeto de retorno
                id_ubicacion_unidad = (from DataRow r in ds.Tables["Table"].Rows
                                       select r.Field<int>("IdUbicacion")).FirstOrDefault();

            }
        }


        /// <summary>
        /// Carga las asignaciones del movimiento solicitado, en el estatus indicado, cargando adicionalmente información de tipo de asignación
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="estatus">Estatus de la asignación</param>
        /// <returns></returns>
        public static DataTable CargaAsignaciones(int id_movimiento, Estatus estatus)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 11, 0, id_movimiento, estatus, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga las asignaciones del movimiento solicitado, en el estatus indicado, cargando adicionalmente información de tipo de asignación
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="estatus">Estatus de la asignación</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesMovimiento(int id_movimiento, Estatus estatus)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 29, 0, id_movimiento, estatus, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Determina si las unidades se encuentran disponibles para realizar la reversa del término del movimiento indicado
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento a validar para reversa</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRecursosParaReversaTerminaMovimiento(int id_movimiento)
        {
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(1, "No existen asignaciones de recurso que validar en este movimiento.", true);

            //Obtener el conjunto de asignaciones terminadas del movimiento
            using (DataTable mitAsignaciones = CargaAsignacionesMovimiento(id_movimiento, Estatus.Terminado))
            {
                //Validando que existe algúna asignación
                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                {
                    //Instanciando el movimiento en cuestion
                    using (Movimiento movimiento = new Movimiento(id_movimiento))
                    {
                        //Si el movimiento fue localizado
                        if (movimiento.habilitar)
                        {
                            //Parada cada una de las asignaciones encontradas
                            foreach (DataRow r in mitAsignaciones.Rows)
                            {
                                //Determianndo el tipo de asignación
                                switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                                {
                                    case Tipo.Unidad:
                                        //Instanciando la unidad
                                        using (Unidad unidad = new Unidad(r.Field<int>("IdRecurso")))
                                        {
                                            //Validando que la unidad se haya recuperado
                                            if (unidad.id_unidad > 0)
                                            {
                                                //Determinando el estatus actual de la unidad
                                                switch (unidad.EstatusUnidad)
                                                {
                                                    //Ocupado en una parada
                                                    case Unidad.Estatus.ParadaOcupado:
                                                        //Instanciando estancia de unidad
                                                        using (EstanciaUnidad estancia = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(unidad.id_unidad)))
                                                        {
                                                            //Recuperando la parada a la que pertenece
                                                            using (Parada parada = new Parada(estancia.id_parada))
                                                            {
                                                                //Si la parada fue recuperada
                                                                if (parada.id_parada > 0)
                                                                {
                                                                    //Comparando el servicio al que pertenece 
                                                                    if (parada.id_servicio == movimiento.id_servicio)
                                                                    {
                                                                        //Comparando la fecha de inicio de la estancia contra la llegada a esta parada del servicio 
                                                                        if (parada.fecha_llegada.CompareTo(estancia.inicio_estancia) == 0)
                                                                        {
                                                                            //Indicando que la unidad puede ser utilizada
                                                                            resultado = new RetornoOperacion(unidad.id_unidad);
                                                                        }
                                                                        //Si las Fechas no coinciden
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("La unidad '{0}' tiene movimientos posteriores al movimiento que será actualizado, primero verifique que no exista ningún otro movimiento.", unidad.numero_unidad));
                                                                    }
                                                                    //Si los servicios no coinciden
                                                                    else
                                                                    {
                                                                        using (Documentacion.Servicio servicio = new Servicio(parada.id_servicio))
                                                                            resultado = new RetornoOperacion(string.Format("La unidad '{0}' se encuentra ocupada por el servicio '{1}'", unidad.numero_unidad, servicio.no_servicio));
                                                                    }
                                                                }
                                                                //De lo contrario
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Error al recuperar la parada actual de la unidad '{0}'.", unidad.numero_unidad));
                                                            }
                                                        }
                                                        break;
                                                    //Disponible en una parada
                                                    case Unidad.Estatus.ParadaDisponible:
                                                        //Instanciando parada de fin del movimiento a reversar
                                                        using (Parada parada = new Parada(movimiento.id_parada_destino))
                                                        {
                                                            //Si la parada fue recuperada
                                                            if (parada.id_parada > 0)
                                                            {
                                                                //Validando que no existan estancias iniciadas con fecha posterior o igual a la fecha de llegada a la parada destino del movimiento a reversar
                                                                using (DataTable mitEstancias = EstanciaUnidad.CargaEstanciasPosterioresFechaInicio(unidad.id_unidad, parada.fecha_llegada))
                                                                {
                                                                    //Si no hay estancias con esas caracteristicas
                                                                    if (!Validacion.ValidaOrigenDatos(mitEstancias))
                                                                        //Indicando que la unidad está disponible para su uso
                                                                        resultado = new RetornoOperacion(unidad.id_unidad);
                                                                    else
                                                                        resultado = new RetornoOperacion(string.Format("La unidad '{0}' tiene movimientos posteriores al movimiento que será actualizado, primero verifique que no exista ningún otro movimiento.", unidad.numero_unidad));
                                                                }
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion("Error al recuperar la parada final del movimiento actual.");
                                                        }
                                                        break;
                                                    default:
                                                        //Señalando el estatus actual en el eque se encuentra
                                                        resultado = new RetornoOperacion(string.Format("La unidad '{0}' se encuentra en estatus '{1}' y no puede ser utilizada.", unidad.numero_unidad, unidad.EstatusUnidad));
                                                        break;
                                                }
                                            }
                                            //SI la unidad no se recuperó
                                            else
                                                resultado = new RetornoOperacion(string.Format("Unidad ID '{0}', no recuperada.", r.Field<int>("IdRecurso")));
                                        }
                                        break;
                                    case Tipo.Operador:
                                        //Instanciando al operador
                                        using (Operador operador = new Operador(r.Field<int>("IdRecurso")))
                                        {
                                            //Validando que el operador se haya recuperado
                                            if (operador.id_operador > 0)
                                            {
                                                //Determinando el estatus del operador
                                                switch (operador.estatus)
                                                {
                                                    case Operador.Estatus.Ocupado:
                                                        //Instanciando parada de destino del movimiento a reversar
                                                        using (Parada pd = new Parada(movimiento.id_parada_destino))
                                                            //Si el operador se mantiene en la parada destino del movimiento (mantiene la parada y fecha de actualización sin cambios)
                                                            if (pd.id_parada == operador.id_parada)
                                                            {
                                                                //Validamos Fecha de Actalización sin cambios
                                                                if (pd.fecha_llegada.CompareTo(operador.fecha_actualizacion) == 0)
                                                                {
                                                                    //Indicando que el operador puede ser actualizado
                                                                    resultado = new RetornoOperacion(operador.id_operador);
                                                                }
                                                                else
                                                                    //Mostrando Mensaje Error
                                                                    resultado = new RetornoOperacion(string.Format("El operador '{0}' tiene movimientos posteriores al movimiento que será actualizado, primero verifique que no exista ningún otro movimiento.", operador.nombre));

                                                            }
                                                            //Si no es el mismo
                                                            else
                                                                resultado = new RetornoOperacion("El operador se encuentra en uso por otro servicio.");
                                                        break;
                                                    case Operador.Estatus.Disponible:
                                                        //Indicando que el operador puede ser utilizado
                                                        resultado = new RetornoOperacion(operador.id_operador);
                                                        break;
                                                    default:
                                                        //Operador no disponible
                                                        resultado = new RetornoOperacion(string.Format("El operador '{0}' se encuentra en estatus '{1}' y no es posible utilizarlo para esta acción.", operador.nombre, operador.estatus));
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }

                                //Si hay error en su estado actual
                                if (!resultado.OperacionExitosa)
                                    //Terminando ciclo de validación
                                    break;
                            }
                        }
                        //Si no se encontró el movimiento
                        else
                            resultado = new RetornoOperacion("Error al recuperar los datos del movimiento solicitado.");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }


        /// <summary>
        /// Carga los Recursos actuales ligado a la parada destino donde el movimiento se encuentre Iniciado
        /// </summary>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesActualesLLegada(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 13, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga las Unidades actuales ligado a la parada destino donde el movimiento se encuentre Terminado
        /// </summary>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesSalida(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 14, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtiene la asignación en estatus registrado para el recurso solicitado en elservicio señalado
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_recuro">Id Recurso</param>
        /// <param name="tipo">Tipo Asignación</param>
        /// <returns></returns>
        public static int ObtieneAsignacionRegistradaRecurso(int id_servicio, int id_recuro, Tipo tipo)
        {
            //Definiendo objeto de retorno
            int id_asignacion_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 15, 0, 0, 0, tipo, id_recuro, 0, false, null, id_servicio, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_asignacion_recurso = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_asignacion_recurso;
            }
        }

        /// <summary>
        /// Carga los Recursos actuales (solo Tercero y Unidad Motriz ) ligado a la parada origen. 
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_origen">Id Parada Origen</param>
        /// <returns></returns>
        public static DataTable CargaRecursosInicialesTerceroUnidad(int id_servicio, int id_parada_origen)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 16, 0, 0, 0, 0, 0, 0, false, null, id_parada_origen, id_servicio };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga  todos los Recursos actuales ligado a la parada origen. 
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_origen">Id Parada Origen</param>
        /// <returns></returns>
        public static DataTable CargaRecursosTodosSalida(int id_servicio, int id_parada_origen)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 17, 0, 0, 0, 0, 0, 0, false, null, id_parada_origen, id_servicio };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método encargado de terminar las Asignaciones  ligando un Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion TerminaMovimientosAsignacionRecurso(int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Iniciado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Instanciamos cada una de las asignaciones de recursos
                        using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            //Terminamos las Asignaciones
                            resultado = objMovimientoAsignacionResurso.TerminaMovimientoAsignacionRecurso(id_usuario);

                        //Validamos Resultado
                        if (!resultado.OperacionExitosa)
                            //Salimos del Ciclo
                            break;
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización del estatus de los recursos asignados al movimiento (cuyo estatus de asignación actual es iniciado) a tránsito (el estatus se actualiza en el recurso, sin afectar la asignación al movimiento).
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento por afectar</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de parada</param>
        /// <param name="id_usuario">Id de Usuario que actualiza el estatus</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusRecursosAsignadosATransito(int id_movimiento, DateTime fecha_actualizacion, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(string.Format("No hay unidades asignadas en estatus '{0}'.", Estatus.Iniciado));

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando el conjunto de asignaciones
                using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Iniciado))
                {
                    //Si existen asignaciones 
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las asignaciones encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Determinar el tipo de recurso que es
                            switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                            {
                                case Tipo.Unidad:
                                    //Instanciando unidad
                                    using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                    {
                                        //Si la unidad existe
                                        if (u.id_unidad > 0)
                                        {
                                            //Actualizando su estatus
                                            resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.Transito, id_usuario);
                                            //Si se actualiza el estatus correctamente
                                            if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                                //Actualizando parada donde se mantendrá el recurso
                                                resultado = u.ActualizaEstanciaYMovimiento(0, id_movimiento, fecha_actualizacion, id_usuario);
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                    }
                                    break;
                                case Tipo.Operador:
                                    //Instanciando operador
                                    using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                    {
                                        //Si la unidad existe
                                        if (o.id_operador > 0)
                                        {
                                            //Actualizando su estatus
                                            resultado = o.ActualizaEstatus(Operador.Estatus.Transito, id_usuario);
                                            //Si se actualizó correctamente el estatus del operador
                                            if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                resultado = o.ActualizaParadaYMovimiento(0, id_movimiento, fecha_actualizacion, id_usuario);
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                    }
                                    break;
                                case Tipo.Tercero:
                                    //Establecemos Resultado
                                    resultado = new RetornoOperacion(0);
                                    break;
                            }

                            //Si hay errores
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización del estatus de los recursos asignados al movimiento (cuyo estatus de asignación actual es iniciado) a tránsito (el estatus se actualiza en el recurso, sin afectar la asignación al movimiento).
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento por afectar</param>
        /// <param name="id_parada"></param>
        /// <param name="fecha_actualizacion"></param>
        /// <param name="id_usuario">Id de Usuario que actualiza el estatus</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusRecursosTransitoAOcupado(int id_movimiento, int id_parada, DateTime fecha_actualizacion, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(string.Format("No hay unidades asignadas en estatus '{0}'.", Estatus.Iniciado));

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando el conjunto de asignaciones
                using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Iniciado))
                {
                    //Si existen asignaciones 
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las asignaciones encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Determinar el tipo de recurso que es
                            switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                            {
                                case Tipo.Unidad:
                                    //Instanciando unidad
                                    using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                    {
                                        //Si la unidad existe
                                        if (u.id_unidad > 0)
                                        {
                                            //Actualizando su estatus
                                            resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.ParadaOcupado, id_usuario);
                                            //Si se actualiza el estatus correctamente
                                            if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                            {
                                                //Buscando estancia actual
                                                int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad);
                                                //Si la estancia se localizó
                                                if (id_estancia > 0)
                                                    //Actualizando parada donde se mantendrá el recurso
                                                    resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, fecha_actualizacion, id_usuario);
                                                else
                                                    resultado = new RetornoOperacion(string.Format("Estancia actual no encontrada para la unidad '{0}'", u.numero_unidad));
                                            }
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                    }
                                    break;
                                case Tipo.Operador:
                                    //Instanciando operador
                                    using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                    {
                                        //Si la unidad existe
                                        if (o.id_operador > 0)
                                        {
                                            //Actualizando su estatus
                                            resultado = o.ActualizaEstatus(Operador.Estatus.Ocupado, id_usuario);
                                            //Si se actualizó correctamente el estatus del operador
                                            if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                resultado = o.ActualizaParadaYMovimiento(id_parada, 0, fecha_actualizacion, id_usuario);
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                    }
                                    break;
                                case Tipo.Tercero:
                                    //Establecemos Resultado
                                    resultado = new RetornoOperacion(0);
                                    break;
                            }

                            //Si hay errores
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización del estatus de los recursos asignados al movimiento (cuyo estatus de asignación actual es terminado) a disponible (el estatus se actualiza en el recurso, sin afectar la asignación al movimiento).
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento por afectar</param>
        /// <param name="id_ultima_parada">Id de la última Parda del Servicio</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de parada</param>
        /// <param name="id_usuario">Id de Usuario que actualiza el estatus</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusRecursosTerminadosADisponibleTerminoServicio(int id_movimiento, int id_ultima_parada, DateTime fecha_actualizacion, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciamos Ultima Parada del Servicio
            using (Parada objUltimaParada = new Parada(id_ultima_parada))
            {
                //Verificando existencia de parada alterna en la ubicación actual (parada comodín para asignación de estancias de unidades)
                int idParadaNuevo = Parada.ObtieneParadaComodinUbicacion(objUltimaParada.id_ubicacion, true, id_usuario);
                
                //Validamos obtención de la parada alterna
                if (idParadaNuevo > 0)
                {
                    //Cargando el conjunto de asignaciones en estatus terminado
                    DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Terminado);

                    //Determinando si existen asignaciones liquidadas
                    using (DataTable mitLiq = CargaAsignacionesMovimiento(id_movimiento, Estatus.Liquidado))
                    {
                        //Si hay registros
                        if (mitLiq != null)
                        {
                            //Si hay registros
                            if (mit != null)

                                //Añadiendo a tabla principal de recursos
                                mit.Merge(mitLiq);
                            else
                                //Asignando Tabla de recursos
                                mit = mitLiq;
                        }
                    }

                    //Si existen asignaciones 
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Inicializando transacción
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Para cada una de las asignaciones encontradas
                            foreach (DataRow r in mit.Rows)
                            {
                                //Determinar el tipo de recurso que es
                                switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                                {
                                    case Tipo.Unidad:
                                        //Instanciando unidad
                                        using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                        {
                                            //Si la unidad existe
                                            if (u.id_unidad > 0)
                                            {
                                                //Buscando estancia actual
                                                int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad);

                                                //Instanciamos Estancia
                                                using (EstanciaUnidad objEstanciaunidad = new EstanciaUnidad(id_estancia))
                                                {
                                                    //Validamos que la estancia se encuentre en la parada actual
                                                    //Si la estancia se localizó
                                                    if (id_estancia > 0)
                                                    {
                                                        //Validamos que la Parada de la Estancia sea el de la última Parada
                                                        if (objUltimaParada.id_parada == objEstanciaunidad.id_parada)
                                                        {
                                                            //Actualizando su estatus
                                                            resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.ParadaDisponible, id_usuario);
                                                            //Si se actualiza el estatus correctamente
                                                            if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                                            {
                                                                //Actualizando parada donde se mantendrá el recurso
                                                                resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, objUltimaParada.fecha_llegada, id_usuario);

                                                                //Validamos resultado
                                                                if (resultado.OperacionExitosa)
                                                                    //Modificamos el Id de Parada de la Estancia actual de la unidad (parada alterna)
                                                                    resultado = objEstanciaunidad.CambiaParadaEstanciaUnidad(idParadaNuevo, id_usuario);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                //De lo contrario
                                                resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                        }
                                        break;
                                    case Tipo.Operador:
                                        //Instanciando operador
                                        using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                        {
                                            //Si la unidad existe
                                            if (o.id_operador > 0)
                                            {
                                                //Validamos que el Operador se encuentre en la última parada
                                                if (o.id_parada == objUltimaParada.id_parada)
                                                {
                                                    //Actualizando su estatus
                                                    resultado = o.ActualizaEstatus(Operador.Estatus.Disponible, id_usuario);

                                                    //Si se actualiza el estatus correctamente
                                                    if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                        //Actualizando parada donde se mantendrá el recurso
                                                        resultado = o.ActualizaParadaYMovimiento(idParadaNuevo, 0, objUltimaParada.fecha_llegada, id_usuario);
                                                }
                                            }
                                            //De lo contrario
                                            else
                                                resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                        }
                                        break;
                                    case Tipo.Tercero:
                                        //Establecemos Resultado
                                        resultado = new RetornoOperacion(0);
                                        break;
                                }

                                //Si hay errores
                                if (!resultado.OperacionExitosa)
                                    break;
                            }

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                                scope.Complete();
                        }
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("No hay unidades asignadas en estatus '{0}'.", Estatus.Terminado));
                }
                else
                    resultado = new RetornoOperacion("Error al recuperar o crear la parada alterna.");
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización del estatus de los recursos asignados al movimiento (cuyo estatus de asignación actual es terminado) a disponible (el estatus se actualiza en el recurso, sin afectar la asignación al movimiento).
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento por afectar</param>
        /// <param name="id_parada">Id de Parada dónde se localizaran los recursos</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de parada</param>
        /// <param name="estatus">Estatus de la Asignación del Movimiento</param>
        /// <param name="id_usuario">Id de Usuario que actualiza el estatus</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaEstatusRecursosTerminadosADisponible(int id_movimiento, int id_parada, DateTime fecha_actualizacion, Estatus estatus, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(string.Format("No hay unidades asignadas en estatus '{0}'.", estatus));

            //Cargando el conjunto de asignaciones
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, estatus))
            {
                //Si existen asignaciones 
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Inicializando transacción
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Para cada una de las asignaciones encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Determinar el tipo de recurso que es
                            switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                            {
                                case Tipo.Unidad:
                                    {
                                        //Instanciando unidad
                                        using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                        {
                                            //Si la unidad existe
                                            if (u.id_unidad > 0)
                                            {
                                                //Buscando estancia actual
                                                int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad);
                                                //Validamos que la parada sea la
                                                //Actualizando su estatus
                                                resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.ParadaDisponible, id_usuario);
                                                //Si se actualiza el estatus correctamente
                                                if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                                {

                                                    //Si la estancia se localizó
                                                    if (id_estancia > 0)
                                                    {
                                                        //Instanciando estancia
                                                        using (EstanciaUnidad est = new EstanciaUnidad(id_estancia))
                                                            //Actualizando parada donde se mantendrá el recurso
                                                            resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, est.inicio_estancia, id_usuario);
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("Estancia actual no encontrada para la unidad '{0}'", u.numero_unidad));
                                                }
                                            }
                                            //De lo contrario
                                            else
                                                resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                        }
                                        break;
                                    }
                                case Tipo.Operador:
                                    {
                                        //Instanciando operador
                                        using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                        {
                                            //Si la unidad existe
                                            if (o.id_operador > 0)
                                            {
                                                //Actualizando su estatus
                                                resultado = o.ActualizaEstatus(Operador.Estatus.Disponible, id_usuario);

                                                //Si se actualiza el estatus correctamente
                                                if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                    //Actualizando parada donde se mantendrá el recurso
                                                    resultado = o.ActualizaParadaYMovimiento(id_parada, 0, fecha_actualizacion, id_usuario);
                                            }
                                            //De lo contrario
                                            else
                                                resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                        }
                                        break;
                                    }
                                case Tipo.Tercero:
                                    //Establecemos Resultado
                                    resultado = new RetornoOperacion(0);
                                    break;
                            }

                            //Si hay errores
                            if (!resultado.OperacionExitosa)
                                break;
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                            scope.Complete();
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de iniciar las Asignaciones ligando un Id Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion IniciaMovimientosAsignacionRecurso(int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Registrado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Instanciamos cada una de las asignaciones de recursos
                        using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            //Iniciamos las Asignaciones
                            resultado = objMovimientoAsignacionResurso.IniciaMovimientoAsignacionRecurso(id_usuario);

                        //Validamos Resultado, si hay errores se termina ciclo iterativo
                        if (!resultado.OperacionExitosa)
                            break;
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de devolver a estatus INICIADO todas las asignaciones TERMINADAS del movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion IniciaMovimientosAsignacionRecursoTerminados(int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, Estatus.Terminado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Instanciamos cada una de las asignaciones de recursos
                        using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            //Iniciamos las Asignaciones
                            resultado = objMovimientoAsignacionResurso.IniciaMovimientoAsignacionRecurso(id_usuario);

                        //Validamos Resultado, si hay errores se termina ciclo iterativo
                        if (!resultado.OperacionExitosa)
                            break;
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene todas las asignaciones de un recurso en particular sobre un movimiento señalado
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="tipo_asignacion">Tipo de Asignación de recurso</param>
        /// <param name="id_recurso">Id del Recurso asignado</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesRecursoMovimiento(int id_movimiento, Tipo tipo_asignacion, int id_recurso)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Declarando conjunto de parámetros para recuperación de las asignaciones del recurso
            object[] param = { 37, 0, id_movimiento, 0, (byte)tipo_asignacion, id_recurso, 0, false, null, "", "" };

            //Obteniendo todas las asignaciones del recurso en el movimiento indicado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si existen asignaiones
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Deshabilita todas las asignaciones de un recurso en particular sobre un movimiento señalado
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="tipo_asignacion">Tipo de Asignación de recurso</param>
        /// <param name="id_recurso">Id del Recurso asignado</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaAsignacionesRecursoMovimiento(int id_servicio,int id_movimiento, Tipo tipo_asignacion, int id_recurso, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo asignaciones del recurso indicado
                using (DataTable mit = CargaAsignacionesRecursoMovimiento(id_movimiento, tipo_asignacion, id_recurso))
                {
                    //Si hay recursos coincidentes
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Parada cada uno de las asignaciones encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciando Asignación
                            using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            {
                                //Si la asignación se instanció correctamente
                                if (asignacion.id_movimiento_asignacion_recurso > 0)
                                    //Deshabilitando asignación
                                    resultado = asignacion.DeshabilitaMovimientoAsignacionRecurso(id_servicio,id_usuario);
                                //Si no se localizó
                                else
                                    resultado = new RetornoOperacion(string.Format("Asignación de recurso '{0}'", r.Field<int>("Id")));
                            }

                            //Si existe algún error
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Crea movimientos asignación Recurso en esatus Iniciado
        /// </summary>
        /// <param name="id_movimiento_origen">Id Movimiento origen</param>
        /// <param name="id_movimiento_destino">Id Movimiento Destino</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CreaMovimientosAsignacionRecurso(int id_movimiento_origen, int id_movimiento_destino, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignaciones(id_movimiento_origen, Estatus.Iniciado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos cada una de las asignaciones de recursos
                            using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            {
                                //Insertamos Asignaciones Iniciadas
                                resultado = InsertaMovimientoAsignacionRecurso(id_movimiento_destino, Estatus.Iniciado, (Tipo)objMovimientoAsignacionResurso.id_tipo_asignacion,
                                            objMovimientoAsignacionResurso.id_recurso_asignado, id_usuario);
                            }
                        }
                        else
                        {
                            //Salimos del Ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Crea movimientos asignación Recurso en esatus Iniciado
        /// </summary>
        /// <param name="id_ultima_parada">Id de la Ultima Parada del Movimiento</param>
        /// <param name="id_movimiento_origen">Id Movimiento origen</param>
        /// <param name="id_movimiento_destino">Id Movimiento Destino</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CreaMovimientosAsignacionRecursoParaParadaAlFinal(int id_ultima_parada, int id_movimiento_origen, int id_movimiento_destino, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento_origen, Estatus.Terminado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos cada una de las asignaciones de recursos
                            using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            {
                                //Validamos que exista el operador y Unidad en la parada actual 

                                //Validamos Tipo de Asignación Operador
                                if (objMovimientoAsignacionResurso.TipoMovimientoAsignacion == Tipo.Operador)
                                {
                                    //Instanciamos Operador
                                    using (Operador objOperador = new Operador(objMovimientoAsignacionResurso.id_recurso_asignado))
                                    {
                                        //Validamos que el Operador se encuentre en la parada
                                        if (objOperador.id_parada == id_ultima_parada)
                                        {
                                            //Insertamos Asignaciones Iniciadas
                                            resultado = InsertaMovimientoAsignacionRecurso(id_movimiento_destino, Estatus.Iniciado, (Tipo)objMovimientoAsignacionResurso.id_tipo_asignacion,
                                                        objMovimientoAsignacionResurso.id_recurso_asignado, id_usuario);
                                        }
                                    }
                                }
                                //Validamos Tipo de Asignación Unidad
                                else if (objMovimientoAsignacionResurso.TipoMovimientoAsignacion == Tipo.Unidad)
                                {
                                    //Instanciamos Unidad
                                    using (Unidad objUnidad = new Unidad(objMovimientoAsignacionResurso.id_recurso_asignado))
                                    {
                                        //Instanciamos la Estancia de la Unidad
                                        using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(objUnidad.id_unidad)))
                                        {
                                            //Validamos que la Unidad se encuentre en la parada
                                            if (objEstanciaUnidad.id_parada == id_ultima_parada)
                                            {
                                                //Insertamos Asignaciones Iniciadas
                                                resultado = InsertaMovimientoAsignacionRecurso(id_movimiento_destino, Estatus.Iniciado, (Tipo)objMovimientoAsignacionResurso.id_tipo_asignacion,
                                                            objMovimientoAsignacionResurso.id_recurso_asignado, id_usuario);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                            //Samimos del ciclo 
                            break;
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }




        /*
        /// <summary>
        /// Deshabilitamos asignaciones en estatus iniciados  ligando un id de movimiento origen
        /// </summary>
        /// <param name="id_movimiento_origen">Id Movimiento actual de las asignaciones</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaMovimientosAsignacionRecursoIniciados(int id_movimiento_origen, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones Iniciadas
            using (DataTable mit = CargaAsignacionesIniciadas(id_movimiento_origen, Estatus.Iniciado))
            {
                //Validamos origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Por cada Asignación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos cada una de las asignaciones de recursos
                            using (MovimientoAsignacionRecurso objMovimientoAsignacionResurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            {
                                //Deshabilitamos Asignaciones Iniciadas
                                resultado = objMovimientoAsignacionResurso.editaMovimientoAsignacionRecurso(objMovimientoAsignacionResurso.id_movimiento,
                                            (Estatus)objMovimientoAsignacionResurso.id_estatus_asignacion, (Tipo)objMovimientoAsignacionResurso.id_tipo_asignacion,
                                              objMovimientoAsignacionResurso.id_recurso_asignado, id_usuario, false);
                            }
                        }
                        else
                        {
                            //Salimos del Ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }*/

        /// <summary>
        /// Validamos Ubicación de los Recursos
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="inicio_estancia">Fecha Inicio estancia</param>
        /// <param name="mitRecursos">Recursos asignados al Servicio</param>
        /// <param name="id_ubicacion_inicial">Ubicación Inicial de la Parada</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="tipo_actualizacion">Tipo actualizacion estancia (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_salida">Tipo actualización Salida</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionUbicacionUnidades(int id_servicio, DateTime inicio_estancia, DataTable mitRecursos, int id_ubicacion_inicial, int id_parada,
                                                                   EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion, Parada.TipoActualizacionSalida tipo_actualizacion_salida,
                                                                    int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos variable para almacenar el Id de parada para la estancia que tendrán las unidades
            int id_parada_estancia = id_parada;

            //Validamos Recursos
            if (Validacion.ValidaOrigenDatos(mitRecursos))
            {
                //Recorremos las Unidades
                foreach (DataRow r in mitRecursos.Rows)
                {
                    //Obtenemos Estancia Actual de la Unidad
                    using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(r.Field<int>("Id"))))
                    {
                        //Si no existe la Estancia
                        if (objEstanciaUnidad.id_estancia_unidad == 0)
                            //Añadimos Estancia con la ubicación Actual
                            resultado = EstanciaUnidad.InsertaEstanciaUnidad(id_parada, r.Field<int>("Id"), EstanciaUnidad.Tipo.Operativa, inicio_estancia, tipo_actualizacion, id_usuario);
                        //Si ya existe una estancia activa
                        else
                        {
                            //Sobreescribiendo Id de Parada de estancia
                            id_parada_estancia = objEstanciaUnidad.id_parada;

                            //Validamos en caso de Insercción de Estancia de la Unidad
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Parada de la Estancia actual de la unidad, con el fin de conocer su ubicación actual
                                using (Parada objParadaUnidad = new Parada(id_parada_estancia))
                                {
                                    //Validamos si la Ubicación de la Unidad se encuentra en la Ubicación inicial del Servicio
                                    if (id_ubicacion_inicial != objParadaUnidad.id_ubicacion)
                                        //Establecemos Mensaje Error
                                        resultado = new RetornoOperacion(-3, "La Unidad no se encuentra en la ubicación para Iniciar el Servicio", false);
                                    //Si ya existe la estancia
                                    else if (objEstanciaUnidad.id_estancia_unidad > 0)
                                    {
                                        //Validamos que la la fecha de inicio de la estancia de la unidad sea mayor igual a la fecha de llegada de la parada
                                        if (inicio_estancia < objEstanciaUnidad.inicio_estancia)
                                        {
                                            //Instanciamos la Unidad
                                            using (Unidad objUnidad = new Unidad(objEstanciaUnidad.id_unidad))
                                            {
                                                //Establecemos Mensaje Error
                                                resultado = new RetornoOperacion("La Unidad " + objUnidad.numero_unidad + " se encuentra disponible a partir del dia " + objEstanciaUnidad.inicio_estancia.ToString("dd/MM/yyyy HH:mm") + ", para continuar edite la fecha de llegada de la parada.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Si hay errores hasta este punto
                    if (!resultado.OperacionExitosa)
                        break;
                }
            }
            else
                //Establecemos mensaje
                resultado = new RetornoOperacion("No existen recursos asignados al Servicio.");

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos recursos para la Salida de la Parada
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_origen">Id Parada Actual</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaRecursosSalida(int id_servicio, int id_parada_origen)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            int total_tercero = 0, total_unidad_motriz_arrastre = 0, total_unidad_motriz, total_unidad_arrastre = 0,
                total_unidad_arrastre_union = 0, total_operadores = 0, total_unidad_motriz_propia = 0;

            //Cargamos todos los recursos
            using (DataTable mit = MovimientoAsignacionRecurso.CargaRecursosTodosSalida(id_servicio, id_parada_origen))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Obtenemos  la asignación de Tercero
                    total_tercero = (from DataRow r in mit.Rows
                                     where Convert.ToByte(r["IdTipoAsignacion"]) == 3
                                     select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades Motrices que permitan unidades de arrastre
                    total_unidad_motriz_arrastre = (from DataRow r in mit.Rows
                                                    where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true
                                                        && Convert.ToBoolean(r["BitPermiteArrastre"]) == true
                                                    select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades Motrices 
                    total_unidad_motriz = (from DataRow r in mit.Rows
                                           where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true
                                           select Convert.ToInt32(r["Id"])).Count();
                    //Obtenemos sólo Unidades Motrices Propias
                    total_unidad_motriz_propia = (from DataRow r in mit.Rows
                                                  where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true && Convert.ToBoolean(r["BitNoPropia"]) == false
                                                  select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades de Arrastre
                    total_unidad_arrastre = (from DataRow r in mit.Rows
                                             where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == false
                                                    && Convert.ToByte(r["BitUnionArrastre"]) == 0
                                             select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades de Arrastre que permiten union entre otras (dolly)
                    total_unidad_arrastre_union = (from DataRow r in mit.Rows
                                                   where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == false
                                                          && Convert.ToByte(r["BitUnionArrastre"]) == 1
                                                   select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Operadores
                    total_operadores = (from DataRow r in mit.Rows
                                        where Convert.ToByte(r["IdTipoAsignacion"]) == 2
                                        select Convert.ToInt32(r["Id"])).Count();


                    //Validamos que exista una Unidad Motriz o Tercero
                    if (total_tercero != 0 || total_unidad_motriz != 0)
                    {
                        //Si existe Unidad Motriz Propia
                        if (total_unidad_motriz_propia != 0)
                        {
                            //Validamos existencia de Operador
                            if (total_operadores == 0)
                                //Mostramos Mensaje
                                resultado = new RetornoOperacion("El Operador es obligatorio.");
                        }

                        //Si no hay errores hasta este punto
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos total de unidades motrices que permitan unidades de arrastre o tercero.
                            if ((total_unidad_motriz_arrastre == 0 && total_tercero == 0) && total_unidad_arrastre != 0)
                                //Mostramos Mensaje
                                resultado = new RetornoOperacion("No se permiten sólo unidades de arrastre.");
                            //Si hay dos o mas unidades de arrastre, debe existir una unidad de conexión entre ellas
                            else if (total_unidad_arrastre > 1 && total_unidad_arrastre_union < total_unidad_arrastre - 1)
                                resultado = new RetornoOperacion("No existen suficientes unidades de unión entre las unidades de arrastre a utilizar.");
                            /* SE QUITA VALIDACIÓN DE UNIDADES DE ARRASTRE OBLIGATORIAS PARA UNIDADES MOTRICES QUE PERMITEN UNIDADES DE ARRASTRE
                             * else if (total_unidad_motriz_arrastre != 0 && total_unidad_arrastre == 0)
                                //Mostramos Mensaje
                                resultado = new RetornoOperacion("La Unidad de Arrastre es Obligartaria.");
                             * */
                        }
                    }
                    //Si no hay unidad motríz o tercero
                    else
                        resultado = new RetornoOperacion("Ingrese la Unidad o el Tercero.");
                }

                //Si no hay recursos
                else
                    resultado = new RetornoOperacion("No se encontrarón Recursos.");
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtenemos las Asignaciones ligando un Id Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="estatus">Estatus de la asignación</param>
        /// <param name="total_tercero">Total asignaciones de Terceros</param>
        /// <param name="total_unidad_motriz">Total de asignaciones de Unidades Motrices</param>
        /// <param name="total_unidad_motriz_propia">Total de asignaciones de Unidades Motrices de Permisionario</param>
        /// <param name="total_unidad_arrastre">Total de asignaciones de Unidades de Arrastre</param>
        /// <param name="total_operador">Total de asignaciones de operadores</param>
        public static void ObtieneRecursosMovimiento(int id_movimiento, Estatus estatus, out int total_tercero, out int total_unidad_motriz, out int total_unidad_motriz_no_propia, out int total_unidad_arrastre,
                                                                      out int total_operador)
        {
            //Declaramos Variables
            total_tercero = total_unidad_motriz = total_unidad_arrastre = total_operador = total_unidad_motriz_no_propia = 0;

            //Cargamos todos los recursos
            using (DataTable mit = CargaAsignacionesMovimiento(id_movimiento, estatus))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Obtenemos  la asignación de Tercero
                    total_tercero = (from DataRow r in mit.Rows
                                     where Convert.ToByte(r["IdTipoAsignacion"]) == 3
                                     select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades Motrices 
                    total_unidad_motriz = (from DataRow r in mit.Rows
                                           where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true
                                           select Convert.ToInt32(r["Id"])).Count();
                    //Obtenemos Unidades Motrices Propia
                    total_unidad_motriz_no_propia = (from DataRow r in mit.Rows
                                                     where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true && Convert.ToBoolean(r["BitNoPropio"]) == true
                                                     select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Unidades de Arrastre
                    total_unidad_arrastre = (from DataRow r in mit.Rows
                                             where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == false
                                             select Convert.ToInt32(r["Id"])).Count();

                    //Obtenemos Operadores
                    total_operador = (from DataRow r in mit.Rows
                                      where Convert.ToByte(r["IdTipoAsignacion"]) == 2
                                      select Convert.ToInt32(r["Id"])).Count();
                }
            }
        }

        /// <summary>
        /// Validamos el tipo de recurso a insertar de acuerdo a los recursos ya existentes (Tercero, Permisionario, Propio).
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="estatus">Estatus de las Asignaciones a validar</param>
        /// <param name="tipo">Tipo de Asignacion (Unidad, Operador, Tercero)</param>
        /// <param name="id_tipo_unidad">Tipo de Unidad (Unidad, Remolque, Dolly)</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaTipoRecurso(int id_movimiento, Estatus estatus, MovimientoAsignacionRecurso.Tipo tipo, int id_tipo_unidad, int id_recurso)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Declaramos Variables
            int total_tercero = 0; int total_unidad_motriz = 0; int total_unidad_motriz_no_propia = 0; int total_unidad_arrastre = 0; int total_operadores = 0;

            //Obtenemos los recusos ligados al movimiento
            ObtieneRecursosMovimiento(id_movimiento, estatus, out total_tercero, out total_unidad_motriz, out total_unidad_motriz_no_propia, out total_unidad_arrastre, out total_operadores);

            //De acuerdo al tipo e recurso que se desea añadir.
            switch (tipo)
            {
                //Si la Asignación es Tercero
                case Tipo.Tercero:
                    //Validamos que no exita Unidad Motriz 
                    if (total_unidad_motriz > 0)
                    {
                        //Establecemos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible la agregar la asignación del Tercero ya que existe la Unidad Motriz.");
                    }
                    else if (total_operadores > 0)
                    {

                        //Establecemos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible agregar la asignación de Tercero ya que existe el Operador.");
                    }
                    break;
                //Si la Asignación es Unidad
                case Tipo.Unidad:
                    //Instanciamos Tipo Unidad
                    using (UnidadTipo objUnidadTipo = new UnidadTipo(id_tipo_unidad))
                    {
                        //Validamos que la Unidad sea Motriz
                        if (objUnidadTipo.bit_motriz == true)
                        {
                            //Validamos que no exista Tercero
                            if (total_tercero > 0)
                            {
                                //Establecemos Mensaje Resultado
                                resultado = new RetornoOperacion("No es posible agregar la asignación de la Unidad ya que existe un Tercero.");
                            }
                            else
                            {
                                //Instanciamos Unidad
                                using (Unidad objUnidad = new Unidad(id_recurso))
                                {
                                    //Si es Unidad de Permisionario
                                    if (objUnidad.bit_no_propia == true)
                                    {
                                        //Si existen Operadores
                                        if (total_operadores > 0)
                                        {
                                            //Establecemos Mensaje Resultado
                                            resultado = new RetornoOperacion("No es posible agregar la asignación de la Unidad de Permisionario ya que existe Operador(es).");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                //Si la asignación es Operador
                case Tipo.Operador:
                    //Validamos que no exista Tercero
                    if (total_tercero > 0)
                    {
                        //Establecemos Mensaje Resultado
                        resultado = new RetornoOperacion("No es posible agregar el Operador ya que existe un Tercero.");
                    }
                    else
                    {
                        //Si existen Unidades de Permisionario
                        if (total_unidad_motriz_no_propia > 0)
                        {
                            //Establecemos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible agregar el Operador ya que existe Unidades de Permisionario.");
                        }
                    }

                    break;
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos los Recursos de la Llegada (Tercero o Unidad Motriz)
        /// </summary>
        /// <param name="id_servicio">Id servicio</param>
        /// <param name="id_parada_origen">Id parada Origen</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionRecursosLlegada(int id_servicio, int id_parada_origen)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Inicializando variables para almacenar total de unidades y tercero
            int id_tercero = 0; int id_unidad_motriz = 0;

            //Cargamos Recursos (Tercero y Unidad)
            using (DataTable mit = CargaRecursosInicialesTerceroUnidad(id_servicio, id_parada_origen))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Obtenemos  la asignación de Tercero
                    id_tercero = (from DataRow r in mit.Rows
                                  where Convert.ToByte(r["IdTipoAsignacion"]) == 3
                                  select Convert.ToInt32(r["Id"])).FirstOrDefault();

                    //Obtenemos  la asignación de Unidad Motriz
                    id_unidad_motriz = (from DataRow r in mit.Rows
                                        where Convert.ToByte(r["IdTipoAsignacion"]) == 1 && Convert.ToBoolean(r["BitMotriz"]) == true
                                        select Convert.ToInt32(r["Id"])).FirstOrDefault();
                }

                //Validamos Asignación Tercero
                if (id_tercero == 0 && id_unidad_motriz == 0)
                    //Mostramos mensaje
                    resultado = new RetornoOperacion("Ingrese la asignación del Tercero o la Unidad motriz.");
            }

            //Devolvemos Reusltado
            return resultado;
        }

        /// <summary>
        /// Validamos disponibilidad de Operadores del Servicio 
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="mitRecursos">Recursos Iniciales</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionDisponibilidadOperadores(int id_servicio, DataTable mitRecursos)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos que existan Recursos
            if (Validacion.ValidaOrigenDatos(mitRecursos))
            {
                //Recorremos las Unidades
                foreach (DataRow r in mitRecursos.Rows)
                {
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(r.Field<int>("Id")))
                    {
                        //Validamos Estatus del operador en cuestión
                        switch (objOperador.estatus)
                        {
                            case Operador.Estatus.Disponible:
                                //Sin acciones, es el estatus ideal
                                break;
                            case Operador.Estatus.Ocupado:
                            case Operador.Estatus.Transito:
                                //Obtenemos el Id de Servicio que se encuentra actualmente la Unidad
                                using (Servicio objServicio = new Servicio(MovimientoAsignacionRecurso.ObtieneServicioOperadorIniciado(objOperador.id_operador)))
                                {
                                    //Si el servicio es diferente al actual
                                    if (id_servicio != objServicio.id_servicio)
                                        //Establecemos Mensaje Error 
                                        resultado = new RetornoOperacion("El Operador " + objOperador.nombre + " se encuentra asignado al servicio " + objServicio.no_servicio + ".");
                                }
                                break;
                            default:
                                //El resto de los estatus no son permitidos
                                resultado = new RetornoOperacion(string.Format("El estatus actual del operador '{0}' es '{1}'.", objOperador.nombre, objOperador.estatus));
                                break;
                        }
                    }

                    //En caso de error
                    if (!resultado.OperacionExitosa)
                        break;
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos disponibilidad de Unidades del Servicio (sólo Unidad)
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="mitRecursos">Recursos Iniciales</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionDisponibilidadUnidades(int id_servicio, DataTable mitRecursos)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos que existan Recursos
            if (Validacion.ValidaOrigenDatos(mitRecursos))
            {
                //Recorremos las Unidades
                foreach (DataRow r in mitRecursos.Rows)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(r.Field<int>("Id")))
                    {
                        //Validando estatus de unidad
                        switch (objUnidad.EstatusUnidad)
                        {
                            case Unidad.Estatus.ParadaDisponible:
                                //Sin acciones que realizar, este es el estatus ideal
                                break;
                            case Unidad.Estatus.ParadaOcupado:
                            case Unidad.Estatus.Transito:
                                //Obtenemos el Id de Servicio que se encuentra actualmente la Unidad
                                using (Servicio objServicio = new Servicio(MovimientoAsignacionRecurso.ObtenemosServicioActualUnidad(objUnidad.id_unidad)))
                                {
                                    //Si el servicio es diferente al actual
                                    if (id_servicio != objServicio.id_servicio)
                                        //Establecemos Mensaje Error 
                                        resultado = new RetornoOperacion("La Unidad " + objUnidad.numero_unidad + " se encuentrá asignada al servicio " + objServicio.no_servicio + ".");
                                }
                                break;
                            default:
                                //El resto de los estatus no son permitidos
                                resultado = new RetornoOperacion(string.Format("El estatus actual de la unidad '{0}' es '{1}'.", objUnidad.numero_unidad, objUnidad.EstatusUnidad));
                                break;
                        }
                    }

                    //Validamos Resultado en caso de error
                    if (!resultado.OperacionExitosa)
                        //Salimos del ciclo 
                        break;
                }
            }
            //Si no hay unidades
            else
                resultado = new RetornoOperacion("No existen unidades asignadas al Servicio.");

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Validamos Recursos (Disponibilidad y Ubicación).
        /// </summary>
        /// <param name="fecha_llegada">Fecha de llegada del Recurso</param>
        /// <param name="tipo_actualizacion_salida">Tipo Actualización de Salida de la parada (Manual, GPS)</param>
        /// <param name="tipo_actualizacion_estancia_inicio">Tipo actualización Inicio de la estancia (Manual,GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="mitUnidades">Tabla de las Unidades</param>
        /// <param name="mitOperadores">Tabla de los Operadores</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_ubicacion_inicial">Ubicación actual de la parada</param>
        /// <param name="id_parada">Id parada actual</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionDeRecursos(DateTime fecha_llegada, Parada.TipoActualizacionSalida tipo_actualizacion_salida, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_estancia_inicio, int id_usuario,
                                                     DataTable mitUnidades, DataTable mitOperadores, int id_servicio, int id_ubicacion_inicial, int id_parada)
        {
            //Declaramos Mensaje Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validación de Disponibilidad de la Unidad
            resultado = ValidacionDisponibilidadUnidades(id_servicio, mitUnidades);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Validamos Disponibimidad de Operadores
                resultado = ValidacionDisponibilidadOperadores(id_servicio, mitOperadores);

                //Si la disponibilidad es correcta
                if (resultado.OperacionExitosa)
                {
                    //Validamos Ubicación del Recurso
                    resultado = ValidacionUbicacionUnidades(id_servicio, fecha_llegada, mitUnidades, id_ubicacion_inicial,
                                                                                        id_parada, tipo_actualizacion_estancia_inicio,
                                                                                        tipo_actualizacion_salida, id_usuario);
                }
            }
            //Devolvemos resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene la primer operador registrado al movimiento.
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static int ObtienePrimerOperadorRegistrado(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int id_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 18, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_recurso = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_recurso;
            }
        }

        /// <summary>
        /// Obtiene la primer unidad registrada al movimiento.
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static int ObtienePrimerUnidadRegistrada(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int id_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 19, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_recurso = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_recurso;
            }
        }

        /// <summary>
        /// Obtiene el servicio de la asignación iniciada de Tipo Operador.
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <returns></returns>
        public static int ObtieneServicioOperadorIniciado(int id_operador)
        {
            //Definiendo objeto de retorno
            int id_servicio = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 20, 0, 0, 0, 0, id_operador, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_servicio = (from DataRow r in ds.Tables[0].Rows
                                   select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_servicio;
            }
        }

        /// <summary>
        /// Carga los Operadores actuales ligado a la parada destino donde el movimiento se encuentre Iniciado
        /// </summary>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresActualesLLegada(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 21, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Operadores Iniciales del Servicio
        /// </summary>
        /// <param name="id_parada_origen">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresIniciales(int id_parada_origen)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 22, 0, 0, 0, 0, 0, 0, false, null, id_parada_origen, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Operadores actuales ligados a un movimiento
        /// </summary>
        /// <param name="id_parada_destino">Id Parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresSalida(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 23, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtiene Recurso Principal del(Tractor/Tercero) Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="tipo">Tipo de Asignación</param>
        /// <param name="motriz">"1" para indicar si se requiere recurso motriz (sólo para Tipo.Unidad), "0" para arrastre</param>
        /// <returns></returns>
        public static int ObtieneRecursoPrincipalServicio(int id_servicio, Tipo tipo, byte motriz)
        {
            //Definiendo objeto de retorno
            int id_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 24, 0, 0, 0, tipo, 0, 0, false, null, id_servicio, motriz };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_recurso = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["IdRecurso"])).FirstOrDefault();

                //Devolviendo resultado
                return id_recurso;
            }
        }

        /// <summary>
        ///  Obtiene las Unidades de Arratre principales del Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_arrastre_1">Obtiene la principal unidad de arratre</param>
        /// <param name="id_arrastre_2">Obtiene la secundaria unidad de arrastre</param>
        public static void ObtieneRecursoArrastrePrincipalServicio(int id_servicio, out int id_arrastre_1, out int id_arrastre_2)
        {
            //Definiendo objeto de retorno
            id_arrastre_1 = 0;
            id_arrastre_2 = 0;
            //Declaramos valor la 1 unidad de arrastre encontrada
            int arrastre_1 = 0;
            //Inicializando arreglo de parámetros
            object[] param = { 25, 0, 0, 0, 0, 0, 0, false, null, id_servicio, 0 };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Unidad de Arratre
                    arrastre_1 = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["IdRecurso"])).FirstOrDefault();
                    //Asignamos valor de Arrastre a la 1 Unidad
                    id_arrastre_1 = arrastre_1;
                    //Obtenemos 2 Unidad de Arrastre ignarando la 1 unidad de arrastre
                    id_arrastre_2 = (from DataRow r in ds.Tables[0].Rows
                                     where Convert.ToInt32(r["IdRecurso"]) != arrastre_1
                                     orderby Convert.ToInt32(r["Asignaciones"]) descending
                                     select Convert.ToInt32(r["IdRecurso"])).FirstOrDefault();
                }

            }
        }

        /// <summary>
        /// Cargamos Asignaciones Terminadas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesTerminadas(int id_movimiento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 26, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene todas las Asignaciones en estatus Terminada o Liquidada del movimiento indicado
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesTerminadasYLiquidadas(int id_movimiento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 39, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene todas las Asignaciones en estatus Liquidada del movimiento indicado
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesLiquidadas(int id_movimiento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 40, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtiene la asignación no cancelada del recurso dentro del movimiento especificado
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo">Tipo de Movimiento</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static MovimientoAsignacionRecurso ObtieneAsignacionRecursoMovimiento(int id_movimiento, Tipo tipo, int id_recurso)
        {
            //Definiendo objeto de retorno
            MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso();

            //Inicializando arreglo de parámetros
            object[] param = { 27, 0, id_movimiento, 0, tipo, id_recurso, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Parda cada recurso devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando a objeto de retorno
                        asignacion = new MovimientoAsignacionRecurso(Convert.ToInt32(r["Id"]));
                        break;
                    }
                }

                //Devolviendo resultado
                return asignacion;
            }
        }

        /// <summary>
        /// Método encargado de obtener el Id Movimiento de la Asignación Iniciada ligando un Tipo y un Recurso
        /// </summary>
        /// <param name="tipo">Tipo de Recurso (Unidad, Operador, Tercero)</param>
        /// <param name="id_recuro">Id Recurso</param>
        /// <returns></returns>
        public static int ObtieneMovimientoDeAsignacionIniciada(Tipo tipo, int id_recuro)
        {
            //Definiendo objeto de retorno
            int _id_movimiento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 28, 0, 0, 0, tipo, id_recuro, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    _id_movimiento = (from DataRow r in ds.Tables[0].Rows
                                      select Convert.ToInt32(r["IdMovimiento"])).FirstOrDefault();

                //Devolviendo resultado
                return _id_movimiento;
            }
        }


        /// <summary>
        /// Carga asignaciones terminadas ligando un Id Movimiento, Tipo Asignación, Id Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesTerminadas(int id_movimiento, Tipo tipo_asignacion, int id_recurso)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 30, 0, id_movimiento, 0, tipo_asignacion, id_recurso, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Deshabilita Asignación 
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoAsignacionRecurso(int id_servicio,int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //En caso de ser Asignaciones en Vacio
            if (id_servicio != 0)
            {
                //Validamos que no existan anticipos de acuerdo al tipo de asignaciòn
                switch ((Tipo)this._id_tipo_asignacion)
                {
                    case Tipo.Operador:
                        resultado = validaAnticipos(id_servicio, this._id_recurso_asignado, 0, 0, id_usuario);
                        break;
                    case Tipo.Unidad:
                        resultado = validaAnticipos(id_servicio, 0, this._id_recurso_asignado, 0, id_usuario);
                        break;
                    case Tipo.Tercero:
                        resultado = validaAnticipos(id_servicio, 0, 0, this._id_recurso_asignado, id_usuario);
                        break;
                }
            }
            else
            {
                //Declaramos objeto Resultado
                bool validacion = false;
                //Validamos que no existan anticipos de acuerdo al tipo de asignaciòn
                switch ((Tipo)this._id_tipo_asignacion)
                {
                    case Tipo.Operador:
                        validacion = DetalleLiquidacion.ValidaAnticiposMovimiento(this._id_movimiento, this._id_recurso_asignado, 0, 0);
                        break;
                    case Tipo.Unidad:
                        validacion = DetalleLiquidacion.ValidaAnticiposMovimiento(this._id_movimiento, 0, this._id_recurso_asignado, 0);
                        break;
                    case Tipo.Tercero:
                        validacion = DetalleLiquidacion.ValidaAnticiposMovimiento(this._id_movimiento, 0, 0, this._id_recurso_asignado);
                        break;

                }
                //Si el resultado no es exitos
                if(validacion)
                {
                    //Mostramos Error
                    resultado = new RetornoOperacion("Existen Anticipos Ligados al Movimiento.");
                }
            }

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Adeshabilitamos Asignación
                return editaMovimientoAsignacionRecurso(this._id_movimiento, (Estatus)this._id_estatus_asignacion,
                                                        (Tipo)this._id_tipo_asignacion, this._id_recurso_asignado, id_usuario, false);
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de obtener el Id de Asignación en estatus Iniciado ligando un Tipo y un Recurso
        /// </summary>
        /// <param name="tipo">Tipo de Recurso (Unidad, Operador, Tercero)</param>
        /// <param name="id_recuro">Id Recurso</param>
        /// <returns></returns>
        public static int ObtieneMovimientoAsignacionIniciada(Tipo tipo, int id_recuro)
        {
            //Definiendo objeto de retorno
            int id_movimiento_asignacion = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 31, 0, 0, 0, tipo, id_recuro, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_movimiento_asignacion = (from DataRow r in ds.Tables[0].Rows
                                                select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_movimiento_asignacion;
            }
        }

        /// <summary>
        /// Método encargado de obtener el Id de Asignación en estatus Iniciado ligando un Tipo y un Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo">Tipo de Recurso (Unidad, Operador, Tercero)</param>
        /// <param name="id_recuro">Id Recurso</param>
        /// <returns></returns>
        public static int ObtieneMovimientoAsignacionIniciada(int id_movimiento, Tipo tipo, int id_recuro)
        {
            //Definiendo objeto de retorno
            int id_movimiento_asignacion = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 32, 0, id_movimiento, 0, tipo, id_recuro, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_movimiento_asignacion = (from DataRow r in ds.Tables[0].Rows
                                                select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_movimiento_asignacion;
            }
        }

        /// <summary>
        /// Carga Operadores con asignaciones terminadas ligando un Id de Parada
        /// </summary>
        /// <param name="id_parada_destino">Id Parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaOperadoresConAsignacionesTerminadas(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 33, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Unidades con asignaciones terminadas ligando un Id de Parada
        /// </summary>
        /// <param name="id_parada_destino">Id Parada Destino</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesConAsignacionesTerminadas(int id_parada_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 34, 0, 0, 0, 0, 0, 0, false, null, id_parada_destino, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        ///  Carga Movimientos Asignación ligando un Id Movimiento 
        /// </summary>
        /// <param name="id_movimiento">Id de Movivimiento asignado a la Asignación</param>
        /// <returns></returns>
        public static DataTable CargaMovimientosAsignacion(int id_movimiento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 35, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtiene el primer recurso en estatus iniciado asignado al movimiento.
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="tipo_asignacion">Tipo de asignación (Unidad, Operador, Tercero).</param>
        /// <returns></returns>
        public static int ObtienePrimerRecursoAsignado(int id_movimiento, Tipo tipo_asignacion)
        {
            //Definiendo objeto de retorno
            int id_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 36, 0, id_movimiento, 0, tipo_asignacion, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_recurso = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_recurso;
            }
        }
        /// <summary>
        /// Obtiene las asignaciones pendientes del recurso solicitado
        /// </summary>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso">Id de recurso asignado</param>
        /// <returns></returns>
        public static DataTable ObtieneAsignacionesPendientesRecurso(Tipo tipo_asignacion, int id_recurso)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 41, 0, 0, 0, (byte)tipo_asignacion, id_recurso, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        ///Valida la existencia de una asignación activa ligando un recurso
        /// </summary>
        /// <param name="tipo_asignacion">Tipo Asignación</param>
        /// <param name="id_recurso">Id de recurso asignado</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static bool ValidaAsignacionActiva(Tipo tipo_asignacion, int id_recurso, int id_movimiento)
        {
            //Definiendo objeto de retorno
            bool asignacion = true;

            //Inicializando arreglo de parámetros
            object[] param = { 43, 0, id_movimiento, 0, (byte)tipo_asignacion, id_recurso, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (!Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    asignacion = false;

                //Devolviendo resultado
                return asignacion;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo_asignacion"></param>
        /// <param name="id_recurso"></param>
        /// <param name="id_movimiento"></param>
        /// <returns></returns>
        public static int ObtieneAsignacionActiva(Tipo tipo_asignacion, int id_recurso, int id_movimiento)
        {
            //Definiendo objeto de retorno
            int asignacion = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 43, 0, id_movimiento, 0, (byte)tipo_asignacion, id_recurso, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (!Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando a objeto de retorno
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                        asignacion = Convert.ToInt32(dr["Id"]);
                }

                //Devolviendo resultado
                return asignacion;
            }
        }
        /// <summary>
        /// Deshabilitamos las asignaciones ligando un Id Movimiento validadndo el estatus de la asignación
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaMovimientosAsignacionesRecursos(int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones ligadas a un Id Movimiento
            using (DataTable mitAsignaciones = CargaMovimientosAsignacion(id_movimiento))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                {
                    //Recorremos cada una de las Asignaciones
                    foreach (DataRow r in mitAsignaciones.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Asignación
                            using (MovimientoAsignacionRecurso objMovimientAsignacionRecurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                            {
                                //Deshabilitamos Asignación
                                resultado = objMovimientAsignacionRecurso.DeshabilitaMovimientoAsignacionRecursoDespacho(id_usuario);
                            }
                        }
                        else
                        //Salimos del ciclo
                        {
                            break;
                        }
                    }
                }
            }
            //Devolvemos resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilitamos las asignaciones Terminadas ligando un Id Movimiento 
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaMovimientosAsignacionesRecursosVacio(int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Asignaciones ligadas a un Id Movimiento
            using (DataTable mitAsignaciones = CargaMovimientosAsignacion(id_movimiento))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                {
                    //Recorremos cada una de las Asignaciones
                    foreach (DataRow r in mitAsignaciones.Rows)
                    {
                        //Instanciamos Asignación
                        using (MovimientoAsignacionRecurso objMovimientAsignacionRecurso = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                        {
                            //Deshabilitamos Asignación
                            resultado = objMovimientAsignacionRecurso.DeshabilitaMovimientoAsignacionRecurso(0,id_usuario);
                        }

                        //Validamos Resultado
                        if (!resultado.OperacionExitosa)
                        //Salimos del ciclo
                        {
                            break;
                        }
                    }
                }
            }
            //Devolvemos resultado
            return resultado;
        }

        /// <summary>
        /// Actualizamos los Detalle de liquidación generado por cada una de las Asignaciones
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaAsignacionesMovimientoVacioaServicio(int id_servicio, int id_cliente, int id_movimiento, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos las Asignaciones
            using (DataTable mitAsignaciones = CargaAsignacionesTerminadasYLiquidadas(id_movimiento))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                {
                    //Declaramos variables
                    int id_unidad = 0;
                    int id_operador = 0;
                    int id_tercero = 0;
                    //Recorremos cada una de las Asignaciones
                    foreach (DataRow r in mitAsignaciones.Rows)
                    {
                        //Validamos Tipo de Asignación Unidad
                        if (Convert.ToInt32(r["IdTipoAsignacion"]) == 1)
                        {
                            id_unidad = r.Field<int>("IdRecurso");
                            id_operador = 0;
                            id_tercero = 0;
                        }
                        //Validamos Tipo de Asignación Operador
                        else if (Convert.ToInt32(r["IdTipoAsignacion"]) == 2)
                        {
                            id_operador = r.Field<int>("IdRecurso");
                            id_unidad = 0;
                            id_tercero = 0;
                        }
                        //Validamos Tipo de Asignación Tercero
                        else if (Convert.ToInt32(r["IdTipoAsignacion"]) == 3)
                        {
                            id_tercero = r.Field<int>("IdRecurso");
                            id_unidad = 0;
                            id_operador = 0;
                        }
                        //Editamos Detalles de Liquidación por cada uno de los Anticipos
                        resultado = DetalleLiquidacion.ActualizaServicioDetalleLiquidacion(id_servicio, id_cliente, id_movimiento, id_operador, id_unidad, id_tercero, id_usuario);
                    }
                }
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método  encargado de Actualizar Movimiento Asignación Recurso
        /// </summary>
        /// <returns></returns>
        public bool ActualizaMovimientoAsignacionRecurso()
        {
            return this.cargaAtributosInstancia(this._id_movimiento_asignacion_recurso);
        }
        /// <summary>
        /// Valida que los recursos involucrados en el inicio del movimiento indicado se encuentren en la ubicación de origen y actualiza su estatus a ocupado
        /// </summary>
        /// <param name="id_parada_inicio">Id de Parada a donde inicia el servicio</param>
        /// <param name="fecha_llegada">Fecha de LLegada de la Parada</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion MueveAsignacionesParadaInicial(int id_parada_inicio, DateTime fecha_llegada, int id_servicio, int id_usuario)
        {
            //Declarando  objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No hay asignaciones Registradas para este movimiento.");

            //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando movimiento
                using (Movimiento mov = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(id_servicio, id_parada_inicio)))
                {
                    //Si el movimiento existe
                    if (mov.habilitar)
                    {
                        //Obteniendo laas asignaciones registradas del movimiento
                        using (DataTable mitAsignaciones = CargaAsignacionesMovimiento(mov.id_movimiento, Estatus.Registrado))
                        {
                            //Si existen asignaciones
                            if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                            {
                                //Para cada asignación
                                foreach (DataRow r in mitAsignaciones.Rows)
                                {
                                    //Instanciando asignación
                                    using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                                    {
                                        //Si la asignación se cargó correctamente
                                        if (asignacion.id_movimiento_asignacion_recurso > 0)
                                        {
                                            //Actualziando asignación a estatus iniciada
                                            resultado = asignacion.IniciaMovimientoAsignacionRecurso(id_usuario);

                                            //Si no hay error
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Determinando el tipo de asignación
                                                switch ((Tipo)r.Field<byte>("IdTipoAsignacion"))
                                                {
                                                    case Tipo.Unidad:
                                                        //Instanciando Unidad
                                                        using (Unidad u = new Unidad(r.Field<int>("IdRecurso")))
                                                        {
                                                            //Si la unidad se localizó
                                                            if (u.id_unidad > 0)
                                                            {
                                                                //Si el estatus de la unidad permite su uso
                                                                if (u.EstatusUnidad == Unidad.Estatus.ParadaDisponible)
                                                                {
                                                                    //Instanciando su estancia activa
                                                                    using (EstanciaUnidad est = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad)))
                                                                    {
                                                                        //Si la estancia existe
                                                                        if (est.id_estancia_unidad > 0)
                                                                        {
                                                                            //Instanciando su parada de inicio
                                                                            using (Parada paradaOrigen = new Parada(mov.id_parada_origen), paradaEstancia = new Parada(est.id_parada))
                                                                            {
                                                                                //Si la parada se localizó
                                                                                if (paradaOrigen.id_parada > 0 && paradaEstancia.id_parada > 0)
                                                                                {
                                                                                    //Validando que la estancia se encuentre en la misma ubicación que la parada inical del movimiento
                                                                                    if (paradaOrigen.id_ubicacion == paradaEstancia.id_ubicacion)
                                                                                    {
                                                                                        //Actualizando estancia a parada de origen del movimiento
                                                                                        resultado = est.CambiaParadaEstanciaUnidad(mov.id_parada_origen, id_usuario);
                                                                                        //Si no hay errores
                                                                                        if (resultado.OperacionExitosa)
                                                                                        {
                                                                                            //Actualizando estatus de unidad
                                                                                            resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.ParadaOcupado, id_usuario);

                                                                                        }
                                                                                    }
                                                                                    //Si no se encuentra en la misma ubicacion
                                                                                    else
                                                                                        resultado = new RetornoOperacion(string.Format("La unidad '{0}' no se encuentra en esta ubicación.", u.numero_unidad));
                                                                                }
                                                                                else
                                                                                    resultado = new RetornoOperacion(string.Format("Parada Origen o Parada de Estancia de unidad '{0}' no localizada.", u.numero_unidad));
                                                                            }

                                                                        }
                                                                        //Si no hay estancia iniciada
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("No fue posible encontrar la estancia actual de la unidad '{0}'.", u.numero_unidad));
                                                                    }
                                                                }
                                                                //Si no está disponible
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("La unidad '{0}' no está disponible.", u.numero_unidad));
                                                            }
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Error al recuperar unidad Id '{0}'", r.Field<int>("IdRecurso")));
                                                        }
                                                        break;
                                                    case Tipo.Operador:
                                                        //Instanciando al operador
                                                        using (Operador o = new Operador(r.Field<int>("IdRecurso")))
                                                        {
                                                            //Si el operador existe
                                                            if (o.id_operador > 0)
                                                            {
                                                                //Actualizando estatus de operador a ocupado
                                                                resultado = o.ActualizaEstatus(Operador.Estatus.Ocupado, id_usuario);

                                                                //Actualizamos atribuitos del Operador
                                                                if (o.ActualizaAtributosInstancia())
                                                                {
                                                                    //Sctualizamos Estancia y Movimiento
                                                                    resultado = o.ActualizaParadaYMovimiento(id_parada_inicio, 0, fecha_llegada, id_usuario);
                                                                }
                                                                else
                                                                    //Establecemos Mensaje Resultado
                                                                    resultado = new RetornoOperacion(string.Format("No se encontró datos complementarios del Operador '{0}'", o.nombre));
                                                            }
                                                        }
                                                        break;
                                                }
                                            }

                                            //Si hay errores
                                            if (!resultado.OperacionExitosa)
                                                break;
                                        }
                                        //Si no se localizó
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al recuperar asignación de recurso Id '{0}'.", r.Field<int>("Id")));
                                    }
                                }
                            }
                        }
                    }
                    //Si no se localizó el movimiento
                    else
                        resultado = new RetornoOperacion("Error al recuperar el movimiento involucrado.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Mueve los recursos involucrados en el primer movimiento del servicio hacia la parada genérica correspondiente
        /// </summary>
        /// <param name="id_parada_inicio">Id de Parada a donde se incia el servicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ReversaMueveAsignacionesParadaInicial(int id_parada_inicio, int id_usuario)
        {
            //Declarando  objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Parada indicada
                using (Parada parada = new Parada(id_parada_inicio))
                {
                    //Si la parada se localizó
                    if (parada.id_parada > 0)
                    {
                        //Obteniendo el movimiento correspondiente
                        using (Movimiento movimiento = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(parada.id_servicio, parada.id_parada)))
                        {
                            //Si el movimiento se localizó
                            if (movimiento.habilitar)
                            {
                                //Validamos Anticipos
                               resultado =  DetalleLiquidacion.ActualizaAnticiposPorMovimiento(movimiento.id_servicio, movimiento.id_movimiento, id_usuario);
                                //Validamos Anticipos
                               if (resultado.OperacionExitosa)
                               {
                                   //Obteniendo las asignaciones del movimiento
                                   using (DataTable mitAsignaciones = CargaAsignacionesMovimiento(movimiento.id_movimiento, Estatus.Iniciado))
                                   {
                                       //Si hay asignaciones
                                       if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                                       {
                                           //Obteniendo la parada comodín de la ubicación
                                           int idParadaComodin = Parada.ObtieneParadaComodinUbicacion(parada.id_ubicacion, true, id_usuario);

                                           //Si la parada fue localizada
                                           if (idParadaComodin > 0)
                                           {
                                               //Parada cada una de las asignaciones encontradas
                                               foreach (DataRow r in mitAsignaciones.Rows)
                                               {
                                                   //Instanciando asignación
                                                   using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                                                   {
                                                       //Si la asignación existe
                                                       if (asignacion.id_movimiento_asignacion_recurso > 0)
                                                       {
                                                           //Actualizando asignación a estatus registrado
                                                           resultado = asignacion.ActualizaEstatusMovimientoAsignacionRecurso(MovimientoAsignacionRecurso.Estatus.Registrado, id_usuario);

                                                           //Si se actualizó correctamente
                                                           if (resultado.OperacionExitosa)
                                                           {
                                                               //Determinando el tipo de asignación
                                                               switch (asignacion.TipoMovimientoAsignacion)
                                                               {
                                                                   case MovimientoAsignacionRecurso.Tipo.Unidad:
                                                                       //Instanciando la unidad en cuestion
                                                                       using (Unidad u = new Unidad(asignacion.id_recurso_asignado))
                                                                       {
                                                                           //SI la unidad se localizó
                                                                           if (u.id_unidad > 0)
                                                                           {
                                                                               //Instanciando estancia
                                                                               using (EstanciaUnidad estancia = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad)))
                                                                               {
                                                                                   //Si la estancia fue recuperada
                                                                                   if (estancia.id_estancia_unidad > 0)
                                                                                   {
                                                                                       //Actualizando Id de Parada a parada comodín de la ubicación
                                                                                       resultado = estancia.CambiaParadaEstanciaUnidad(idParadaComodin, id_usuario);

                                                                                       //Si no hay errores
                                                                                       if (resultado.OperacionExitosa)
                                                                                           //Actualizando estatus de unidad
                                                                                           resultado = u.ActualizaEstatusADisponible(id_usuario);
                                                                                       else
                                                                                           resultado = new RetornoOperacion(string.Format("Error al mover estancia de la unidad '{0}' a parada predeterminada.", u.numero_unidad));
                                                                                   }
                                                                                   else
                                                                                       resultado = new RetornoOperacion(string.Format("Estancia de la unidad '{0}' no recuperada.", u.numero_unidad));
                                                                               }
                                                                           }
                                                                           //Si no se encontró
                                                                           else
                                                                               resultado = new RetornoOperacion(string.Format("La unidad Id '{0}' no pudo ser recuperada.", r.Field<int>("IdRecurso")));
                                                                       }
                                                                       break;
                                                                   case MovimientoAsignacionRecurso.Tipo.Operador:
                                                                       //Instanciando operador
                                                                       using (Operador o = new Operador(r.Field<int>("IdRecurso")))
                                                                       {
                                                                           //Si e loperador existe
                                                                           if (o.id_operador > 0)
                                                                               //Actualizando estatus a disponible
                                                                               resultado = o.ActualizaEstatusADisponible(id_usuario);
                                                                           else
                                                                               resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no localizado.", r.Field<int>("IdRecurso")));
                                                                       }
                                                                       break;
                                                               }
                                                           }
                                                           else
                                                               resultado = new RetornoOperacion(string.Format("Error al actualizar asignación Id '{0}': {1}", r.Field<byte>("Id"), resultado.Mensaje));

                                                       }
                                                       //Si no se encotró la asignación
                                                       else
                                                           resultado = new RetornoOperacion(string.Format("Asignación de Recurso Id '{0}' no encontrada.", r.Field<byte>("IdTipoAsignacion")));
                                                   }

                                                   //Si hay errores
                                                   if (!resultado.OperacionExitosa)
                                                       break;
                                               }
                                           }
                                           else
                                               resultado = new RetornoOperacion("La parada predeterminada de retorno no fue localizada.");
                                       }
                                   }
                               }
                            }
                            else
                                resultado = new RetornoOperacion("El movimiento inicial no fue recuperado.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("Parada inicial no localizada.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Mueve los recursos involucrados en el movimiento cuyo destino es la parada indicada, liberando recursos y eliminando las asignaciones
        /// </summary>
        /// <param name="id_parada_inicio">Id de Parada a donde se incia el servicio</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ReversaMueveAsignacionesParada(int id_parada_inicio, int id_usuario)
        {
            //Declarando  objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_parada_inicio);

            //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Parada indicada
                using (Parada parada = new Parada(id_parada_inicio))
                {
                    //Si la parada se localizó
                    if (parada.id_parada > 0)
                    {
                        //Obteniendo el movimiento correspondiente
                        using (Movimiento movimiento = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(parada.id_servicio, parada.id_parada)))
                        {
                            //Si el movimiento se localizó
                            if (movimiento.habilitar)
                            {
                                //Obteniendo las asignaciones del movimiento
                                using (DataTable mitAsignaciones = CargaAsignacionesMovimiento(movimiento.id_movimiento, Estatus.Iniciado))
                                {
                                    //Si hay asignaciones
                                    if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                                    {
                                        //Obteniendo la parada comodín de la ubicación
                                        int idParadaComodin = Parada.ObtieneParadaComodinUbicacion(parada.id_ubicacion, true, id_usuario);

                                        //Si la parada fue localizada
                                        if (idParadaComodin > 0)
                                        {
                                            //Parada cada una de las asignaciones encontradas
                                            foreach (DataRow r in mitAsignaciones.Rows)
                                            {
                                                //Instanciando asignación
                                                using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(r.Field<int>("Id")))
                                                {
                                                    //Si la asignación existe
                                                    if (asignacion.id_movimiento_asignacion_recurso > 0)
                                                    {
                                                        //Deshabilitando asignación
                                                        resultado = asignacion.DeshabilitaMovimientoAsignacionRecurso(movimiento.id_servicio, id_usuario);

                                                        //Si se deshabilitó correctamente
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Determinando el tipo de asignación
                                                            switch (asignacion.TipoMovimientoAsignacion)
                                                            {
                                                                case MovimientoAsignacionRecurso.Tipo.Unidad:
                                                                    //Instanciando la unidad en cuestion
                                                                    using (Unidad u = new Unidad(asignacion.id_recurso_asignado))
                                                                    {
                                                                        //SI la unidad se localizó
                                                                        if (u.id_unidad > 0)
                                                                        {
                                                                            //Instanciando estancia
                                                                            using (EstanciaUnidad estancia = new EstanciaUnidad(EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad)))
                                                                            {
                                                                                //Si la estancia fue recuperada
                                                                                if (estancia.id_estancia_unidad > 0)
                                                                                {
                                                                                    //Actualizando Id de Parada a parada comodín de la ubicación
                                                                                    resultado = estancia.CambiaParadaEstanciaUnidad(idParadaComodin, id_usuario);

                                                                                    //Si no hay errores
                                                                                    if (resultado.OperacionExitosa)
                                                                                        //Actualizando estatus de unidad
                                                                                        resultado = u.ActualizaEstatusADisponible(id_usuario);
                                                                                    else
                                                                                        resultado = new RetornoOperacion(string.Format("Error al mover estancia de la unidad '{0}' a parada predeterminada.", u.numero_unidad));
                                                                                }
                                                                                else
                                                                                    resultado = new RetornoOperacion(string.Format("Estancia de la unidad '{0}' no recuperada.", u.numero_unidad));
                                                                            }
                                                                        }
                                                                        //Si no se encontró
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("La unidad Id '{0}' no pudo ser recuperada.", r.Field<int>("IdRecurso")));
                                                                    }
                                                                    break;
                                                                case MovimientoAsignacionRecurso.Tipo.Operador:
                                                                    //Instanciando operador
                                                                    using (Operador o = new Operador(r.Field<int>("IdRecurso")))
                                                                    {
                                                                        //Si e loperador existe
                                                                        if (o.id_operador > 0)
                                                                            //Actualizando estatus a disponible
                                                                            resultado = o.ActualizaEstatusADisponible(id_usuario);
                                                                        else
                                                                            resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no localizado.", r.Field<int>("IdRecurso")));
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        else
                                                            resultado = new RetornoOperacion(string.Format("Error al actualizar asignación Id '{0}': {1}", r.Field<int>("Id"), resultado.Mensaje));

                                                    }
                                                    //Si no se encotró la asignación
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("Asignación de Recurso Id '{0}' no encontrada.", r.Field<byte>("IdTipoAsignacion")));
                                                }

                                                //Si hay errores
                                                if (!resultado.OperacionExitosa)
                                                    break;
                                            }
                                        }
                                        else
                                            resultado = new RetornoOperacion("La parada predeterminada de retorno no fue localizada.");
                                    }
                                }
                            }
                            else
                                resultado = new RetornoOperacion("El movimiento inicial no fue recuperado.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("Parada inicial no localizada.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de obtener el tipo de asignación correspondiente.
        /// </summary>
        /// <param name="id_unidad">Obtiene el Id Unidad</param>
        /// <param name="id_operador">Obtiene el Id Operador</param>
        /// <param name="id_tercero">Obtiene el Id Tercero</param>
        /// <param name="id_movimiento_asignacion">Id Movimiento Asignación</param>
        /// <param name="id_recurso">Id Recurso</param>
        /// <returns></returns>
        public static RetornoOperacion ObtieneTipoAsignacion(out int id_unidad, out int id_operador, out int id_tercero, int id_movimiento_asignacion, int id_recurso)
        {
            //Declaramos Variables
            RetornoOperacion resultado = new RetornoOperacion(0);

            id_unidad = id_operador = id_tercero = 0;

            //Instanciamos la Asignacion
            using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
            {
                //Si el depósito es para la Unidad 
                if (objMovimientoAsignacionRecurso.TipoMovimientoAsignacion == MovimientoAsignacionRecurso.Tipo.Unidad)
                {
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(id_recurso))
                    {
                        //Validamos que sea de Permisionario
                        if (objUnidad.bit_no_propia == false)

                            //Establecemos Mensaje Error
                            resultado = new RetornoOperacion("No es posible asignar el depósito, ya que la Unidad es Propia.");
                        else
                        {
                            //Instanciamos Tipo de Unidad
                            using (UnidadTipo objUnidadTipo = new UnidadTipo(objUnidad.id_tipo_unidad))
                            {
                                //Validamos que sea  Unidad Motriz
                                if (objUnidadTipo.bit_motriz == true)

                                    //Asignamos valor
                                    id_unidad = id_recurso;
                                else
                                    //Establecemos Mensaje Error
                                    resultado = new RetornoOperacion("Imposible asignar anticipos a Unidades de arratre.");
                            }
                        }
                    }
                }
                else
                {
                    //Si el depósito es para Operador
                    if (objMovimientoAsignacionRecurso.TipoMovimientoAsignacion == MovimientoAsignacionRecurso.Tipo.Operador)

                        //Asignamos valor
                        id_operador = id_recurso;
                    else
                        //Asignamos valor
                        id_tercero = id_recurso;
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la actualización de 
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="kilometros">Cantidad de (+/-) kilometros que serán añadidos o restados del kilometraje actual de la unidad</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaKilometrajeUnidadesMovimiento(int id_movimiento, decimal kilometros, int id_usuario)
        {
            //Declarando objeto de resultado e inicializando sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_movimiento);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando todas las asignaciones de unidad terminadas del movimiento
                using (DataTable mitAsignaciones = CargaAsignacionesTerminadas(id_movimiento))
                {
                    //Si hay asignaciones coincidentes
                    if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                    {
                        //Filtrando sólo asignaciones del tipo unidad
                        List<int> unidades = (from DataRow r in mitAsignaciones.Rows
                                              where r.Field<byte>("TipoAsignacion") == 1
                                              select r.Field<int>("IdRecursoAsignado")).DefaultIfEmpty().ToList();
                        //Si hay unidades involucradas (excluyendo valor 0 de la lista)
                        if (unidades.Count(u => u > 0) > 0)
                        {
                            //Parada cada una de las asignaciones
                            foreach (int id_unidad in unidades)
                            {
                                //Instanciar unidad
                                using (Unidad u = new Unidad(id_unidad))
                                {
                                    //Si la unidad existe
                                    if (u.habilitar)
                                        //Realizando la actualización del kiometraje
                                        resultado = u.ActualizaKilometrosAsignados(kilometros, id_usuario);
                                    else
                                        resultado = new RetornoOperacion(string.Format("No se encontró la unidad con ID '{0}'", id_unidad));
                                }

                                //Si hay errores
                                if (!resultado.OperacionExitosa)
                                    break;
                            }
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene el conjunto de asignaciones solicitado de un recurso en concreto
        /// </summary>
        /// <param name="tipo_recurso">Tipo de Recurso asignado</param>
        /// <param name="id_recurso">Id de Recurso</param>
        /// <param name="numero_asignaciones">Total de asignaciones a devolver</param>
        /// <returns></returns>
        public static DataTable ObtieneUltimasAsignacionesRecurso(Tipo tipo_recurso, int id_recurso, int numero_asignaciones)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 42, 0, 0, 0, (byte)tipo_recurso, id_recurso, 0, false, null, numero_asignaciones, "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene las asignaciones registradas, iniciadas o terminadas de un servicio (sólo se excluyen canceladas)
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesServicio(int id_servicio)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo criterios de búsqueda
            object[] param = { 44, 0, 0, 0, 0, 0, 0, false, null, id_servicio, "" };

            //Cargando asignaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay tablas devueltas en origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando primer tabla a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Asignaciones Válidas para el Regitró de Anticipos
        /// </summary>
        /// <param name="id_movimiento">Id_Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAsignacionesParaRegistroAnticipos(int id_movimiento)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo criterios de búsqueda
            object[] param = { 45, 0, id_movimiento, 0, 0, 0, 0, false, null, 0, "" };

            //Cargando asignaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay tablas devueltas en origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando primer tabla a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método encargado de Obtener la Unidad Asignado al Operador
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <returns></returns>
        public static int ObtieneAsignacionUnidadPropia(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int id_asignacion_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 46, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos el movimiento
                    id_asignacion_recurso = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id_asignacion_recurso;
            }
        }

        /// <summary>
        /// Método encargado de Obtener la Asignación del Recurso en un Origen Especifico
        /// </summary>
        /// <param name="tipo">Tipo de Asignación (Unidad, Operador, Proveedor)</param>
        /// <param name="id_recurso">Recurso del Movimiento</param>
        /// <param name="id_origen">Origen de la Asignación</param>
        /// <returns></returns>
        public static int ObtieneAsignacionRegistrada(Tipo tipo, int id_recurso, int id_origen)
        {
            //Declarando Objeto de Retorno
            int id_asignacion_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 47, 0, 0, 0, (byte)tipo, id_recurso, 0, false, null, id_origen.ToString(), "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos la Asignación
                    id_asignacion_recurso = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).FirstOrDefault();
                }

            }

            //Devolviendo Resultado Obtenido
            return id_asignacion_recurso;
        }

        /// <summary>
        /// Método encargado de Obtener la Asignación del Recurso en estaus Iniciada y terminada diferente al Id de Movimiento
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Diferente al Movimiento actual</param>
        /// <param name="tipo">Tipo de Asignación (Unidad, Operador, Proveedor)</param>
        /// <param name="id_recurso">Recurso del Movimiento</param>
        /// <returns></returns>
        public static int ObtieneAsignacionIniciadaOTerminada(int id_servicio,int id_movimiento,int tipo, int id_recurso)
        {
            //Declarando Objeto de Retorno
            int id_asignacion_recurso = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 48, 0, id_movimiento, 0, tipo, id_recurso, 0, false, null, id_servicio, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos la Asignación
                    id_asignacion_recurso = (from DataRow r in ds.Tables[0].Rows
                                             select Convert.ToInt32(r["Id"])).DefaultIfEmpty().FirstOrDefault();
                }

            }

            //Devolviendo Resultado Obtenido
            return id_asignacion_recurso;
        }

        /// <summary>
        /// Obtiene el Total de ejes de las Asignaciones de Unidad menos en Estatus Canceladas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static int ObtieneTotalEjesUnidadNoCanceladas(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int ejes = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 50, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    ejes = (from DataRow r in ds.Tables[0].Rows
                            select Convert.ToInt32(r["TotalEjes"])).DefaultIfEmpty().FirstOrDefault();

                //Devolviendo resultado
                return ejes;
            }

        }
        /// <summary>
        /// Obtiene Asignación de la Unidad Motriz omitiendo las Canceldas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static int ObtieneUnidadMotriz(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 51, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }

        }
        /// <summary>
        /// Obtiene Asignación de la Unidad Motriz omitiendo las Canceldas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación(Unidad, Operador, Tercero)</param>
        /// <returns></returns>
        public static int ObtieneAsignacionIniciadaTerminada(int id_movimiento, Despacho.MovimientoAsignacionRecurso.Tipo id_tipo_asignacion)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 52, 0, id_movimiento, 0,(byte) id_tipo_asignacion, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }

        }

        /// <summary>
        /// Validamos Si el Servicio que se Desea quitar es el Activoa
        /// </summary>
        /// <param name="id_tipo">Id Tipo de la Asignación</param>
        /// <param name="id_recurso">Id del Recurso</param>
        ///<param name="id_movimiento">Id Movimiento de la Asignación</param>
        /// <returns></returns>
        public static bool ValidaViajeActivo(int id_tipo, int id_recurso, int id_movimiento)
        {
            //Declaramos Objeto resultado
            bool validaViaje = false;
            //Intsnaciamos Servicio
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                //Definiendo objeto de retorno
                int idServicio = 0;

                //Inicializando arreglo de parámetros
                object[] param = { 54, 0, 0, 0, id_tipo, id_recurso, 0, false, null, "", "" };

                //Realizando la consulta
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
                {
                    //Validando origen de datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos la Asignación
                        idServicio = (from DataRow r in ds.Tables[0].Rows
                                      select Convert.ToInt32(r["IdServicio"])).DefaultIfEmpty().FirstOrDefault();

                        //Si la Asignación es del Servicio Activo que se desea Eliminar
                        if (idServicio == objMovimiento.id_servicio)
                        {
                            //Asignamos Valor
                            validaViaje = true;
                        }
                    }

                }

            }
                            //Declarmos Objeto Retorno
                return validaViaje;
        }
        
        
        /// <summary>
        /// Obtiene Asignaciones Iniciadas y Terminadas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación(Unidad, Operador, Tercero)</param>
        /// <param name="id_recurso">Id Recursto</param>
        /// <returns></returns>
        public static int ObtieneAsignacionIniciadaTerminada(int id_movimiento, Despacho.MovimientoAsignacionRecurso.Tipo id_tipo_asignacion, int id_recurso)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 55, 0, id_movimiento, 0, (byte)id_tipo_asignacion, id_recurso
                                 , 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }

        }

        /// <summary>
        /// Cargamos la Lista de Unidades para Asignación de Diesel
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <returns></returns>
        public static DataTable CargaUnidadesRegistroDiesel(int id_movimiento)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Definiendo criterios de búsqueda
            object[] param = { 56, 0, id_movimiento, 0, 0, 0, 0, false, null, 0, "" };

            //Cargando asignaciones
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay tablas devueltas en origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando primer tabla a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtiene Asignación de la Unidad Motriz omitiendo las Canceldas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static int ObtieneUnidadArrasteDiesel(int id_movimiento)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 57, 0, id_movimiento, 0, 0, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }



        }

        /// <summary>
        /// Obtiene Asignaciones Iniciadas y Terminadas
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación(Unidad, Operador, Tercero)</param>
        /// <param name="id_recurso">Id Recursto</param>
        /// <returns></returns>
        public static int ObtieneAsignacionRecursoIniciadaTerminada(int id_movimiento, Despacho.MovimientoAsignacionRecurso.Tipo id_tipo_asignacion, int id_recurso)
        {
            //Definiendo objeto de retorno
            int id = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 58, 0, id_movimiento, 0, (byte)id_tipo_asignacion, id_recurso
                                 , 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Obtenemos la Asignación
                    id = (from DataRow r in ds.Tables[0].Rows
                          select Convert.ToInt32(r["Id"])).FirstOrDefault();

                //Devolviendo resultado
                return id;
            }

        }

        /// <summary>
        /// Actualizamos estatus de la Asignación
        /// </summary>
        /// <param name="id_recurso_asignado">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOperadorMovimientoAsignacionRecurso(int id_recurso, int id_usuario)
        {

            //Editamos estatus de la Asignación
            return editaMovimientoAsignacionRecurso(this._id_movimiento, (Estatus)this._id_estatus_asignacion, (Tipo)this._id_tipo_asignacion, id_recurso, id_usuario, this._habilitar);
        }
        #endregion
    }
}