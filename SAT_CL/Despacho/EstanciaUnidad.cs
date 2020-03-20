using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using SAT_CL.Global;
namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de Estancia Unidad.
    /// </summary>
    public class EstanciaUnidad : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus de la estancia Unidad
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Iniciada
            /// </summary>
            Iniciada = 1,
            /// <summary>
            /// Terminada
            /// </summary>
            Terminada
        }
        /// <summary>
        /// Enumera el Tipo de la estancia Unidad
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            /// Operativa
            /// </summary>
            Operativa = 1,
        }

        /// <summary>
        /// Enumera el Tipo de Actualización Inicio 
        /// </summary>
        public enum TipoActualizacionInicio
        {
            /// <summary>
            /// No Actualizado
            /// </summary>
            SinActualizar = 0,
            /// <summary>
            ///  Manual
            /// </summary>
            Manual = 1,
            /// <summary>
            /// GPS
            /// </summary>
            GPS,
            /// <summary>
            /// APP
            /// </summary>
            APP,

        }

        /// <summary>
        /// Enumera el Tipo de Actualización Fin 
        /// </summary>
        public enum TipoActualizacionFin
        {
            /// <summary>
            /// No Actualizado
            /// </summary>
            SinActualizar = 0,
            /// <summary>
            ///  Manual
            /// </summary>
            Manual = 1,
            /// <summary>
            /// GPS
            /// </summary>
            GPS,
            /// <summary>
            /// APP
            /// </summary>
            APP,

        }
        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_estancia_unidad_teu";


        private int _id_estancia_unidad;
        /// <summary>
        /// Describe el Id de la estancia Unidad
        /// </summary>
        public int id_estancia_unidad
        {
            get { return _id_estancia_unidad; }
        }
        private int _id_parada;
        /// <summary>
        /// Describe el Id de Parada
        /// </summary>
        public int id_parada
        {
            get { return _id_parada; }
        }
        private int _id_unidad;
        /// <summary>
        /// Describe el Id de la Unidad
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
        private byte _id_estatus_estancia;
        /// <summary>
        /// Describe el Id estatus estancia
        /// </summary>
        public byte id_estatus_estancia
        {
            get { return _id_estatus_estancia; }
        }
        private byte _id_tipo_estancia;
        /// <summary>
        /// Describe el Id de estancia
        /// </summary>
        public byte id_tipo_estancia
        {
            get { return _id_tipo_estancia; }
        }
        private DateTime _inicio_estancia;
        /// <summary>
        /// Describe el inicio de estancia
        /// </summary>
        public DateTime inicio_estancia
        {
            get { return _inicio_estancia; }
        }
        private byte _id_tipo_actualizacion_inicio;
        /// <summary>
        /// Describe el  tipo de actualización inicio
        /// </summary>
        public byte id_tipo_actualizacion_inicio
        {
            get { return _id_tipo_actualizacion_inicio; }
        }
        private DateTime _fin_estancia;
        /// <summary>
        /// Describe el fin de estancia
        /// </summary>
        public DateTime fin_estancia
        {
            get { return _fin_estancia; }
        }
        private byte _id_tipo_actualizacion_fin;
        /// <summary>
        /// Describe el tipo de actualización fin
        /// </summary>
        public byte id_tipo_actualizacion_fin
        {
            get { return _id_tipo_actualizacion_fin; }
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
        /// Enumera el Estatus Estancia
        /// </summary>
        public Estatus EstatusEstanciaUnidad
        {
            get { return (Estatus)_id_estatus_estancia; }
        }

        /// <summary>
        /// Enumera el Tipo Estancia
        /// </summary>
        public Tipo TipoEstanciaUnidad
        {
            get { return (Tipo)_id_tipo_estancia; }
        }

        /// <summary>
        /// Enumera el Tipo Actualizacion Inio
        /// </summary>
        public TipoActualizacionInicio TipoActualizacionInicio_
        {
            get { return (TipoActualizacionInicio)_id_tipo_actualizacion_inicio; }
        }


        /// <summary>
        /// Enumera el Tipo Actualizacion Fin
        /// </summary>
        public TipoActualizacionFin TipoActualizacionFin_
        {
            get { return (TipoActualizacionFin)_id_tipo_actualizacion_fin; }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~EstanciaUnidad()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public EstanciaUnidad()
        {

        }

        /// <summary>
        ///  Genera una Instancia de Tipo Estancia Unidad
        /// </summary>
        /// <param name="id_estancia_unidad">Id Estancia Unidad</param>
        public EstanciaUnidad(int id_estancia_unidad)
        {
            cargaAtributosInstancia(id_estancia_unidad);
        }

        /// <summary>
        /// Genera una Instancia  de Tipo Estancia Unidad
        /// </summary>
        /// <param name="id_estancia_unidad">Id Estancia Unidad</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_estancia_unidad)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_estancia_unidad, 0, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_estancia_unidad = Convert.ToInt32(r["Id"]);
                        _id_parada = Convert.ToInt32(r["IdParada"]);
                        _id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        _id_estatus_estancia = Convert.ToByte(r["IdEstatus"]);
                        _id_tipo_estancia = Convert.ToByte(r["IdTipoEstancia"]);
                        _inicio_estancia = Convert.ToDateTime(r["InicioEstancia"]);
                        _id_tipo_actualizacion_inicio = Convert.ToByte(r["IdTipoActualizacionInicio"]);
                        DateTime.TryParse(r["FinEstancia"].ToString(), out _fin_estancia);
                        _id_tipo_actualizacion_fin = Convert.ToByte(r["IdTipoActualizacionFin"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]); ;
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
        ///  Método encargado de Editar una Estancia Unidad
        /// </summary>
        /// <param name="id_parada">Id de la Parada al cual pertenece la Estancia de Unidad</param>
        /// <param name="id_unidad">Id de la Unidad</param>
        /// <param name="estatus_estancia">Estatus de la Estancia</param>
        /// <param name="tipo_estancia">Tipo de Estancia</param>
        /// <param name="inicio_estancia">Fecha de Inicio de la Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Medio  que se utilizó  para actualizar el Inicio de la Estancia</param>
        /// <param name="fin_estancia">Fecha de Inicio de la Estancia</param>
        /// <param name="tipo_actualizacion_fin">Medio  que se utilizó  para actualizar el Fin de la Estancia</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Id Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaEstanciaUnidad(int id_parada, int id_unidad, Estatus estatus_estancia, Tipo tipo_estancia,
                                            DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, DateTime fin_estancia,
                                            TipoActualizacionFin tipo_actualizacion_fin, int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = { 2, this._id_estancia_unidad, id_parada, id_unidad, estatus_estancia, tipo_estancia,
                                     inicio_estancia, tipo_actualizacion_inicio, Fecha.ConvierteDateTimeObjeto(fin_estancia), tipo_actualizacion_fin,
                                     id_usuario, habilitar, this._row_version, "", "" };

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

            //inicializamos el arreglo de parametros
            object[] param = { 4, this._id_estancia_unidad, 0, 0, 0, 0, null, 0, null, 0, 0, false, this._row_version, "", "" };

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


        #endregion

        #region Metodos publicos

        /// <summary>
        ///  Método encargado de Insertar una Estancia Unidad
        /// </summary>
        /// <param name="id_parada">Id de la Parada al cual pertenece la Estancia de Unidad</param>
        /// <param name="id_unidad">Id de la Unidad</param>
        /// <param name="tipo_estancia">Tipo de Estancia</param>
        /// <param name="inicio_estancia">Fecha de Inicio de la Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Medio  que se utilizó  para actualizar el Inicio de la Estancia</param>
        /// <param name="id_usuario">id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaEstanciaUnidad(int id_parada, int id_unidad, Tipo tipo_estancia,
                                            DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Obtenemo última fecha fin 
            DateTime ultima_fecha_fin = DateTime.MinValue;

            //Obteniendo última estancia
            using (EstanciaUnidad estancia = new EstanciaUnidad(EstanciaUnidad.ObtieneUltimaEstanciaUnidadTerminada(id_unidad)))
            {
                //Si hay estancia 
                if (estancia.id_estancia_unidad > 0)
                    ultima_fecha_fin = estancia.fin_estancia;

                //Validamos que la fecha de llegada sea mayor a la útima fecha fin
                if ((inicio_estancia > ultima_fecha_fin) || Fecha.EsFechaMinima(ultima_fecha_fin))
                {

                    //Inicializando arreglo de parámetros
                    object[] param = { 1, 0, id_parada, id_unidad, Estatus.Iniciada, tipo_estancia,
                                     inicio_estancia, tipo_actualizacion_inicio, null, 0,
                                     id_usuario, true, null, "", "" };

                    //Realizando la actualización
                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
                }
                else
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion("La fecha la estancia  de la unidad debe ser mayor a su última fecha " + ultima_fecha_fin.ToString("dd-MM-yyyy HH:mm") + ".");
                }
            }

            //Devolvemos valor
            return resultado;
        }

        /// <summary>
        ///  Método encargado de Editar una Estancia Unidad
        /// </summary>
        /// <param name="id_parada">Id de la Parada al cual pertenece la Estancia de Unidad</param>
        /// <param name="id_unidad">Id de la Unidad</param>
        /// <param name="tipo_estancia">Tipo de Estancia</param>
        /// <param name="inicio_estancia">Fecha de Inicio de la Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Medio  que se utilizó  para actualizar el Inicio de la Estancia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaEstanciaUnidad(int id_parada, int id_unidad, Tipo tipo_estancia,
                                            DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            // Validamos que el estatus de la esntancia no sea Terminado
            if ((Estatus)this._id_estatus_estancia != Estatus.Terminada)
            {

                return this.editaEstanciaUnidad(id_parada, id_unidad, (Estatus)this._id_estatus_estancia, tipo_estancia,
                                                inicio_estancia, tipo_actualizacion_inicio, this._fin_estancia, (TipoActualizacionFin)this._id_tipo_actualizacion_fin,
                                                id_usuario, this._habilitar);
            }
            else
            {
                //Mostramos Mensaje
                resultado = new RetornoOperacion("El estatus de la estancia no permite su edición ");
            }
            return resultado;
        }

        /// <summary>
        /// Iniciamos Estancia del Recurso
        /// </summary>
        /// <param name="inicio_estancia">Fecha Inicio Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Tipo actualización Inicio (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaEstanciaUnidad(DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {

            //Actualizamos Estatus de la Estancia
            return this.editaEstanciaUnidad(this._id_parada, this._id_unidad, Estatus.Iniciada, (Tipo)this._id_tipo_estancia, inicio_estancia, tipo_actualizacion_inicio,
                                            DateTime.MinValue, this.TipoActualizacionFin_, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Terminamos Estancia del Recurso
        /// </summary>
        /// <param name="fin_estancia">Fecha Fin Estancia</param>
        /// <param name="tipo_actualizacion_fin">Tipo Actualizacion fin (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TerminaEstanciaUnidad(DateTime fin_estancia, TipoActualizacionFin tipo_actualizacion_fin, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Unidad 
            using (SAT_CL.Global.Unidad objUnidad = new Global.Unidad(id_unidad))
            {
                // Validamos que el estatus de la esntancia no sea Terminado
                if ((Estatus)this._id_estatus_estancia != Estatus.Terminada)
                {
                    //Validamos Fecha de Salida de la Estancia
                    if (fin_estancia > this._inicio_estancia)
                    {
                        //Actualizamos Estatus
                        resultado = this.editaEstanciaUnidad(this._id_parada, this._id_unidad, Estatus.Terminada, (Tipo)this._id_tipo_estancia, this._inicio_estancia,
                                                      (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, fin_estancia, tipo_actualizacion_fin, id_usuario, this._habilitar);
                    }
                    else
                    {
                        //Establecemos Mensaje Error
                        resultado = new RetornoOperacion("No se puede terminar la estancia de la Unidad " + objUnidad.numero_unidad + " ya que la fecha de termino debe ser mayor al inicio de la estancia " +
                                                        this._inicio_estancia.ToString("dd/MM/yyyy HH:mm"));
                    }
                }
                else
                {

                    //Mostramos Mensaje
                    resultado = new RetornoOperacion("El estatus de la estancia de la Unidad: " + objUnidad.numero_unidad + "no permite su edición.");
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualizamos el Estatus de la Unidad a Iniciada
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion CambiaEstanciaUnidadEstatusAIniciada(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Actualizamos Estatus
            resultado = this.editaEstanciaUnidad(this._id_parada, this._id_unidad, Estatus.Iniciada, (Tipo)this._id_tipo_estancia, this._inicio_estancia,
                                               (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, DateTime.MinValue, 0, id_usuario, this._habilitar);

            return resultado;
        }

        /// <summary>
        /// Deshabilita un Registro Estancia Unidad     
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEstanciaUnidad(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Estatus de la Estancia
            if ((Estatus)this._id_estatus_estancia == Estatus.Iniciada)
            {
                //Deshabilitamos Estancia 
                resultado = this.editaEstanciaUnidad(this._id_parada, this._id_unidad, (Estatus)this._id_estatus_estancia, (Tipo)this._id_tipo_estancia,
                                                   this._inicio_estancia, (TipoActualizacionInicio)this._id_tipo_actualizacion_inicio, this._fin_estancia,
                                                   (TipoActualizacionFin)this._id_tipo_actualizacion_fin,
                                                   id_usuario, false);
            }
            else
            {
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("No es posible la eliminación de estancias terminadas.");
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Carga Estancia Iniciadas ligadas a un Id de Parada
        /// </summary>
        /// <param name="id_parada">Id de la Parada al cual pertenece la Estancia de Unidad</param>
        /// <returns></returns>
        public static DataTable CargaEstanciasIniciadas(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 5, 0, id_parada, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };


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
        /// Método  encargado de Actualizar  atributos Estancia Unidad
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEstanciaUnidad()
        {
            return this.cargaAtributosInstancia(this._id_estancia_unidad);
        }


        /// <summary>
        /// Obtenemos la Estancia actual donde se encuentra la Unidad
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static int ObtieneEstanciaUnidadIniciada(int id_unidad)
        {
            //Definiendo objeto de retorno
            int id_estancia = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 6, 0, 0, id_unidad, 0, 0, null, 0, null, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_estancia = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();

                //Devolviendo resultado
                return id_estancia;
            }
        }

        /// <summary>
        /// Editamos la parada de las estancias a partir de la parada actual
        /// </summary>
        /// <param name="id_parada_actual">Parada actual a la que pertenece las Estancias</param>
        /// <param name="id_parada_nueva">Parada para la reubicación de estancias</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaEstanciasUnidad(int id_parada_actual, int id_parada_nueva, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Estancias Iniciadas
            using (DataTable mitEstancias = CargaEstanciasIniciadas(id_parada_actual))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitEstancias))
                {
                    //Recorremos cada estancia
                    foreach (DataRow r in mitEstancias.Rows)
                    {
                        //Validamos Resultado de la edición
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos cada una de las estancias
                            using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(r.Field<int>("Id")))
                            {
                                //Editamos Parada de la estancia
                                resultado = objEstanciaUnidad.EditaEstanciaUnidad(id_parada_nueva, objEstanciaUnidad.id_unidad,
                                            (Tipo)objEstanciaUnidad.id_tipo_estancia, objEstanciaUnidad.inicio_estancia, (TipoActualizacionInicio)objEstanciaUnidad.id_tipo_actualizacion_inicio,
                                            id_usuario);

                            }
                        }
                        else
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos valor
            return resultado;
        }

        /// <summary>
        /// Método encargado de editar la Fecha de Llegada de las Estancias
        /// </summary>
        /// <param name="id_parada">Id parada de las estancias</param>
        /// <param name="inicio_estancia">fecha de Inicio de la Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Tipo Actualizacion Inicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaEstanciasUnidadFechaInicio(int id_parada, DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Estancias Iniciadas
            using (DataTable mitEstancias = CargaEstanciasTerminadasParada(id_parada))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitEstancias))
                {
                    //Recorremos cada estancia
                    foreach (DataRow r in mitEstancias.Rows)
                    {
                        //Instanciamos cada una de las estancias
                        using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(r.Field<int>("Id")))
                        {
                            //Editamos Parada de la estancia
                            resultado = objEstanciaUnidad.EditaEstanciaUnidad(objEstanciaUnidad.id_parada, objEstanciaUnidad.id_unidad,
                                        (Tipo)objEstanciaUnidad.id_tipo_estancia, inicio_estancia, tipo_actualizacion_inicio,
                                        id_usuario);

                        }
                        if (!resultado.OperacionExitosa)
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos valor
            return resultado;
        }
        
        /// <summary>
        /// Creamos Estancias de la Unidades
        /// </summary>
        /// <param name="mitRecursos">Tabla de recursos</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="inicio_estancia">Fecha Inicio Estancia</param>
        /// <param name="tipo_actualizacion_inicio">Tipo actualizacion Inicio (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CreaEstanciasUnidad(DataTable mitRecursos, int id_parada, DateTime inicio_estancia, 
                                        TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos origen de datos
            if (Validacion.ValidaOrigenDatos(mitRecursos))
            {
                //Instanciamos Parada
                using (Parada objParada = new Parada(id_parada))
                {
                    //Por cada Recurso
                    foreach (DataRow r in mitRecursos.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Obtenemos Estancia de la Unidad
                            int id_estancia = ObtieneEstanciaUnidadIniciada(r.Field<int>("Id"));
                            //Validamos que no exista la estancia de la Unidad
                            if (id_estancia == 0)
                            {
                                //Insertamos Estancias
                                resultado = InsertaEstanciaUnidad(id_parada, r.Field<int>("Id"), Tipo.Operativa, inicio_estancia, tipo_actualizacion_inicio, id_usuario);
                            }
                            else
                            {
                                //Instanciamos Estancia para validar la ubicación
                                using (EstanciaUnidad objEstanciaParada = new EstanciaUnidad(id_estancia))
                                {
                                    //Instanciamos Parada
                                    using (Parada objParadaEstancia = new Parada(objEstanciaParada.id_parada))
                                    {
                                        //Si es parada alternativa
                                        if (objParadaEstancia.secuencia_parada_servicio == 0)
                                        {
                                            //Validamos que no existan estancias ligados al Id de Parada
                                            if (EstanciaUnidad.ValidaEstanciaParada(objParadaEstancia.id_parada, mitRecursos, r.Field<int>("Id")) == 0)
                                            {
                                                //Deshabilitamos Parada alternativa
                                                resultado = objParadaEstancia.DeshabilitaParadaAlternativaEstancia(id_usuario);

                                                //Validamos Resultado
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Si la parada de la estancia fue un movimiento vacio ligado a una orden
                                                    if (objParadaEstancia.id_servicio != 0)
                                                    {
                                                        //Obtenemos Movimiento
                                                        using (Movimiento objMovimientoSec = new Movimiento(Movimiento.BuscamosMovimientoParadaDestino(objParadaEstancia.id_servicio, objParadaEstancia.id_parada)))
                                                        {
                                                            //Validamos que exista movimiento
                                                            if (objMovimientoSec.id_movimiento > 0)
                                                            {
                                                                //Editamos parada Movimiento
                                                                resultado = objMovimientoSec.EditaMovimiento(objMovimientoSec.id_servicio, objMovimientoSec.id_segmento_carga, objMovimientoSec.secuencia_servicio,
                                                                            objMovimientoSec.EstatusMovimiento, objMovimientoSec.TipoMovimiento, objMovimientoSec.kms, objMovimientoSec.kms_maps, objMovimientoSec.id_compania_emisor,
                                                                            objMovimientoSec.id_parada_origen, id_parada, id_usuario);

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //Validamos resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Actualizamos  el Id de parada alternativo con el actual
                                                resultado = objEstanciaParada.EditaEstanciaUnidad(objParadaEstancia.secuencia_parada_servicio == 0 ? id_parada : objEstanciaParada.id_parada, objEstanciaParada.id_unidad, (Tipo)objEstanciaParada.id_tipo_estancia,
                                                            objEstanciaParada.inicio_estancia, (TipoActualizacionInicio)objEstanciaParada.id_tipo_actualizacion_inicio, id_usuario);
                                            }
                                        }
                                    }
                                }
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
            else
            {
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("No se puden crear las estancias de las unidades ya que no se encontrarón recursos");
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Crea la estancia del conjunto de unidades indicado dentro de la parada especificada
        /// </summary>
        /// <param name="id_unidad">Id de unidad al que se le creará la estancia</param>
        /// <param name="id_parada">Id de parada</param>
        /// <param name="inicio_estancia">Fecha de inicio de la estancia creada</param>
        /// <param name="tipo_actualizacion_inicio">Tipo de actualización de inicio que tendra la estancia</param>
        /// <param name="id_usuario">Id de usuario que actualzia</param>
        /// <returns></returns>
        public static RetornoOperacion CreaEstanciaUnidades(List<int> id_unidad, int id_parada, DateTime inicio_estancia, TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("No se ha especificado ninguna unidad.");

            //Validamos origen de datos
            if (id_unidad != null)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Por cada Unidad
                    foreach (int idU in id_unidad)
                    {
                        //Insertando nueva estancia
                        resultado = InsertaEstanciaUnidad(id_parada, idU, Tipo.Operativa, inicio_estancia, tipo_actualizacion_inicio, id_usuario);

                        //Si hay errores de actualización
                        if (!resultado.OperacionExitosa)
                            break;
                    }

                    //Si no hay errores detectados
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Crea las estancias de la unidades requeridas para el inicio del servicio indicado, a partir de un conjunto de recursos especificados, actualizando el estatus de cada recurso implecado
        /// </summary>
        /// <param name="id_unidad">Conjunto de Id de Unidades que serán buscadas dentro de las unidades asignadas al movimiento incial del servicio</param>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="inicio_estancia">Fecha de inicio de la estancia de cada unidad</param>
        /// <param name="tipo_actualizacion_inicio">Tipo de actualización del inicio de estancia de cada unidad</param>
        /// <param name="id_usuario">Id de usuario que realiza la actualización</param>
        /// <returns></returns>
        public static RetornoOperacion CreaEstanciaRecursoMovimientoInicialServicio(List<int> id_unidad, int id_servicio, DateTime inicio_estancia, 
                                        TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        { 
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion("No hay unidades por actualizar.");

            //Validando que existan unidades que actualizar
            if (id_unidad != null)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Localizando el movimiento pendiente del servicio donde la parada actual es origen (primer movimiento del servicio).
                    using (Movimiento movimientoInicio = new Movimiento(id_servicio, 1))
                    {
                        //Validando que existe un movimiento incial para el servicio
                        if (movimientoInicio.id_movimiento > 0)
                        {
                            //Cargando asignaciones del movimiento pendiente del servicio
                            using (DataTable mit = MovimientoAsignacionRecurso.CargaAsignacionesMovimiento(movimientoInicio.id_movimiento, MovimientoAsignacionRecurso.Estatus.Registrado))
                            {
                                //Si hay asignaciones
                                if (Validacion.ValidaOrigenDatos(mit))
                                {
                                    //Localizando la parada comodín de la ubicación de inicio del servicio (ubicación actual donde ya se encuentran las unidades)
                                    //Instanciando parada origen del movimiento
                                    using (Parada paradaInicio = new Parada(movimientoInicio.id_parada_origen))
                                    {
                                        //Si la parada fue encontrada
                                        if (paradaInicio.id_parada > 0)
                                        {
                                            //Obteniendo la parada comodín de la misma ubicación de esta parada
                                            int idParadaComodin = Parada.ObtieneParadaComodinUbicacion(paradaInicio.id_ubicacion, true, id_usuario);
                                            //Para cada asignación de recurso del movimiento inicial del servicio
                                            foreach (DataRow r in mit.Rows)
                                            {
                                                //Creando variable para almacenar id de parada a utilizar para la estancia de cada unidad (comodín (si la unidad no es requerida en el servicio) o inicial del servicio (si ya se ha solicitado su uso para el servicio))
                                                int idParadaEstanciaUnidad = idParadaComodin;

                                                //Determinando si la unidad se encuentra dentro de las unidades que utilizará el servicio
                                                if ((from int idU in id_unidad
                                                     where idU == r.Field<int>("IdRecurso")
                                                     select idU).SingleOrDefault() != 0)
                                                    //De ser así se indica que la parada de la estancia debe ser la parada inicial del servicio
                                                    idParadaEstanciaUnidad = paradaInicio.id_parada;                                                

                                                
                                                    //Determinar el tipo de recurso que es
                                                    switch ((MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion"))
                                                    {
                                                        case MovimientoAsignacionRecurso.Tipo.Unidad:
                                                            //Instanciando unidad
                                                            using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                                            {
                                                                //Si la unidad existe
                                                                if (u.id_unidad > 0)
                                                                {
                                                                    //Se inserta la estancia correspondiente
                                                                    resultado = InsertaEstanciaUnidad(idParadaEstanciaUnidad, u.id_unidad, Tipo.Operativa, inicio_estancia, tipo_actualizacion_inicio, id_usuario);
                                                                    //SI se insertó la estancia de la unidad
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        //Actualizando su estatus
                                                                        resultado = u.ActualizaEstatusUnidad(idParadaEstanciaUnidad == idParadaComodin ? Unidad.Estatus.ParadaDisponible : Unidad.Estatus.ParadaOcupado, id_usuario);
                                                                        //Si se actualiza el estatus correctamente
                                                                        if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                                                        {
                                                                            //Buscando estancia actual
                                                                            int id_estancia = EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad);
                                                                            //Si la estancia se localizó
                                                                            if (id_estancia > 0)
                                                                                //Actualizando parada donde se mantendrá el recurso
                                                                                resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, inicio_estancia, id_usuario);
                                                                            else
                                                                                resultado = new RetornoOperacion(string.Format("Estancia actual no encontrada para la unidad '{0}'", u.numero_unidad));
                                                                        }
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion(string.Format("Error al crear estancia de unidad '{0}': {1}", u.numero_unidad, resultado.Mensaje));
                                                                }
                                                                //De lo contrario
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                                            }
                                                            break;
                                                        case MovimientoAsignacionRecurso.Tipo.Operador:
                                                            //Instanciando operador
                                                            using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                                            {
                                                                //Si la unidad existe
                                                                if (o.id_operador > 0)
                                                                {
                                                                    //Actualizando su estatus 
                                                                    resultado = o.ActualizaEstatus(idParadaEstanciaUnidad == idParadaComodin ? Operador.Estatus.Disponible : Operador.Estatus.Ocupado, id_usuario);
                                                                    //Si se actualizó el estatus
                                                                    if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                                        resultado = o.ActualizaParadaYMovimiento(idParadaEstanciaUnidad, 0, inicio_estancia, id_usuario);
                                                                }
                                                                //De lo contrario
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                                            }
                                                            break;
                                                    }


                                                //Si hay errores
                                                if (!resultado.OperacionExitosa)
                                                    break;
                                            }
                                        }
                                        //De lo contrario
                                        else
                                            resultado = new RetornoOperacion("La Parada Inicial del servicio no pudo ser recuperada.");
                                    }
                                }
                                //Si no hay asignaciones
                                else
                                    resultado = new RetornoOperacion(string.Format("No se encontraron asignaciones registradas para el movimiento '{0}'.", movimientoInicio.id_movimiento));
                            }

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                                //Iniciando asignaciones de movimiento inicial
                                resultado = MovimientoAsignacionRecurso.IniciaMovimientosAsignacionRecurso(movimientoInicio.id_movimiento, id_usuario);
                        }
                        //Si no se encuentra el movimiento de inicio del servicio
                        else
                            using (Documentacion.Servicio srv = new Documentacion.Servicio(id_servicio))
                                resultado = new RetornoOperacion(string.Format("No se pudo recuperar el movimiento inicial del servicio '{0}'.", srv.no_servicio));
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Finalizando transacción
                        scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Crea las estancias de la unidades involucradas en un movimiento dentro de la parada de destino de dicho movimiento, asignando los mismos recursos al movimiento siguiente del mismo servicio (en caso de existir). Los recursos quedan ocupados en el mismo servicio.
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento que finaliza</param>
        /// <param name="inicio_estancia">Fecha de inicio de la estancia de cada unidad</param>
        /// <param name="tipo_actualizacion_inicio">Tipo de actualización del inicio de estancia de cada unidad</param>
        /// <param name="id_usuario">Id de usuario que realiza la actualización</param>
        /// <returns></returns>
        public static RetornoOperacion CreaEstanciaUnidadesFinMovimientoServicio(int id_movimiento, DateTime inicio_estancia,
                                        TipoActualizacionInicio tipo_actualizacion_inicio, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Iniciando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando movimiento por afectar
                using (Movimiento movimientoActual = new Movimiento(id_movimiento))
                { 
                    //Si el movimiento fue encontrado
                    if (movimientoActual.id_movimiento > 0)
                    {
                        //Terminando las asignaciones del movimiento actual
                        resultado = MovimientoAsignacionRecurso.TerminaMovimientosAsignacionRecurso(movimientoActual.id_movimiento, id_usuario);

                        //Si las asignaciones del movimiento fueron terminadas correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Se localiza el movimiento siguiente (mismo servicio secuencia siguiente)
                            using (Movimiento movimientoSiguiente = new Movimiento(movimientoActual.id_servicio, (int)movimientoActual.secuencia_servicio + 1))
                            {
                                //Cargando asignaciones terminadas del movimiento actual
                                using (DataTable mit = MovimientoAsignacionRecurso.CargaAsignacionesMovimiento(movimientoActual.id_movimiento, MovimientoAsignacionRecurso.Estatus.Terminado))
                                {
                                    //Si hay asignaciones
                                    if (Validacion.ValidaOrigenDatos(mit))
                                    {
                                        //Para cada asignación de recurso del movimiento
                                        foreach (DataRow r in mit.Rows)
                                        {
                                            //Si el tipo de asignación es unidad
                                            if ((MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion") == MovimientoAsignacionRecurso.Tipo.Unidad)
                                                //Se inserta la estancia correspondiente asociando al siguiente movimiento
                                                resultado = InsertaEstanciaUnidad(movimientoActual.id_parada_destino, r.Field<int>("IdRecurso"), Tipo.Operativa, inicio_estancia, tipo_actualizacion_inicio, id_usuario);

                                            //Si se registró correctamente la estancia y existe un movimiento posterior al actual
                                            if (resultado.OperacionExitosa && movimientoSiguiente.id_movimiento > 0)
                                                //Realizando copia de asignacion
                                                resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(movimientoSiguiente.id_movimiento, MovimientoAsignacionRecurso.Estatus.Iniciado,
                                                                            (MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion"), r.Field<int>("IdRecurso"), id_usuario);

                                            //Si se creó la nueva asignación
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Determinar el tipo de recurso que es
                                                switch ((MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion"))
                                                {
                                                    case MovimientoAsignacionRecurso.Tipo.Unidad:
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
                                                                        resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, inicio_estancia, id_usuario);
                                                                    else
                                                                        resultado = new RetornoOperacion(string.Format("Estancia actual no encontrada para la unidad '{0}'", u.numero_unidad));
                                                                }
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                                        }
                                                        break;
                                                    case MovimientoAsignacionRecurso.Tipo.Operador:
                                                        //Instanciando operador
                                                        using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                                        {
                                                            //Si la unidad existe
                                                            if (o.id_operador > 0)
                                                            {
                                                                //Actualizando su estatus 
                                                                resultado = o.ActualizaEstatus(Operador.Estatus.Ocupado, id_usuario);
                                                                //Si se actualizó el estatus
                                                                if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                                    resultado = o.ActualizaParadaYMovimiento(movimientoActual.id_parada_destino, 0, inicio_estancia, id_usuario);
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", Convert.ToInt32(r.Field<int>("IdRecurso"))));
                                                        }
                                                        break;
                                                }
                                            }
                                            //else
                                            //    resultado = new RetornoOperacion("Error al crear asignación de recursos a nuevo movimieto.");

                                            //Si hay errores
                                            if (!resultado.OperacionExitosa)
                                                break;
                                        }

                                    }
                                    //Si no hay asignaciones
                                    else
                                        resultado = new RetornoOperacion(string.Format("No se encontraron asignaciones registradas para el movimiento '{0}'.", movimientoActual.id_movimiento));
                                }
                            }
                        }                        
                    }
                    //Si no se encontró el movimiento
                    else
                        resultado = new RetornoOperacion(string.Format("Movimiento '{0}' no encontrado.", id_movimiento));
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Deshabilita estancias de unidades cuya asignación a un movimiento sea terminada, siempre que la fecha de inicio de la estancia sea la misma a la fecha de llegada de la parada destino del movimiento
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="fecha_actualizacion">Fecha de actualización de parada y movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaEstanciaUnidadesReversaFinMovimiento(int id_movimiento, DateTime fecha_actualizacion, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("No hay asignaciones de recursos para este movimiento.");

            //Inicializando transaccion
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando el movimiento
                using (Movimiento movimiento = new Movimiento(id_movimiento))
                {
                    //Si el movimiento fue localizado
                    if (movimiento.id_movimiento > 0)
                    {
                        //Se localiza el movimiento siguiente (mismo servicio secuencia siguiente)
                        using (Movimiento movimientoSiguiente = new Movimiento(movimiento.id_servicio, (int)movimiento.secuencia_servicio + 1))
                        {
                            //Cargando las asignaciones terminadas o liquidadas del movimiento en cuestión
                            using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaAsignacionesTerminadasYLiquidadas(movimiento.id_movimiento))
                            {
                                //Si hay registros
                                if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                                {
                                    //Instanciando la parada destino del movimiento
                                    using (Parada paradaDestino = new Parada(movimiento.id_parada_destino))
                                    {
                                        //Si la parada fue recuperada
                                        if (paradaDestino.id_parada > 0)
                                        {
                                            //Para cada una de las asignaciones
                                            foreach (DataRow r in mitAsignaciones.Rows)
                                            {
                                                //Determinar el tipo de recurso que es
                                                switch ((MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion"))
                                                {
                                                    case MovimientoAsignacionRecurso.Tipo.Unidad:
                                                        //Instanciando unidad
                                                        using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                                        {
                                                            //Si la unidad existe
                                                            if (u.id_unidad > 0)
                                                            {
                                                                //Obteniendo la estancia de la unidad, correspondiente a la ubicación de la parada en la fecha de llegada a la parada
                                                                using (EstanciaUnidad estancia = new EstanciaUnidad(ObtieneEstanciaUnidadFechaInicio(u.id_unidad, paradaDestino.id_ubicacion, paradaDestino.fecha_llegada)))
                                                                {
                                                                    //Si la estancia existe
                                                                    if (estancia.id_estancia_unidad > 0)
                                                                        //Deshabilitando estancia
                                                                        resultado = estancia.DeshabilitaEstanciaUnidad(id_usuario);
                                                                    //Si no se encontró la estanci
                                                                    else
                                                                        resultado = new RetornoOperacion(string.Format("No es posible encontrar la estancia de la unidad '{0}' para su deshabilitación.", u.numero_unidad));
                                                                }

                                                                //Si se deshabilitó la estancia
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Actualizando su estatus
                                                                    resultado = u.ActualizaEstatusUnidad(Unidad.Estatus.Transito, id_usuario);
                                                                    //Si se actualiza el estatus correctamente
                                                                    if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                                                            //Actualizando parada donde se mantendrá el recurso
                                                                            resultado = u.ActualizaEstanciaYMovimiento(0, id_movimiento, fecha_actualizacion, id_usuario);
                                                                }
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Error al deshabilitar estancia de unidad '{0}'.", u.numero_unidad));

                                                                //Si no hay errores y hay un movimiento posterior al movimiento a reversar
                                                                if (resultado.OperacionExitosa && movimientoSiguiente.id_movimiento > 0)
                                                                    //Deshabilitando las asignaciones de la unidad en cualquier estatus siempre que estén asociadas al movimiento siguiente
                                                                    resultado = MovimientoAsignacionRecurso.DeshabilitaAsignacionesRecursoMovimiento(movimientoSiguiente.id_servicio, movimientoSiguiente.id_movimiento, MovimientoAsignacionRecurso.Tipo.Unidad, u.id_unidad, id_usuario);
                                                                else if (!resultado.OperacionExitosa)
                                                                    resultado = new RetornoOperacion(string.Format("Error al actualizar la unidad '{0}' a estatus 'Tránsito'.", u.numero_unidad));
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", r.Field<int>("IdRecurso")));
                                                        }
                                                        break;
                                                    case MovimientoAsignacionRecurso.Tipo.Operador:
                                                        //Instanciando operador
                                                        using (Operador o = new Operador(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                                        {
                                                            //Si la unidad existe
                                                            if (o.id_operador > 0)
                                                            {
                                                                //Actualizando su estatus 
                                                                resultado = o.ActualizaEstatus(Operador.Estatus.Transito, id_usuario);

                                                                //Si se actualizó el estatus correctamente
                                                                if(resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                                                                    //Actualizando parada y movimiento
                                                                    resultado = o.ActualizaParadaYMovimiento(0, id_movimiento, fecha_actualizacion, id_usuario);

                                                                //Si no hay errores y hay un movimiento posterior al movimiento a reversar
                                                                if (resultado.OperacionExitosa && movimientoSiguiente.id_movimiento > 0)
                                                                    //Deshabilitando las asignaciones de la unidad en cualquier estatus siempre que estén asociadas al movimiento siguiente
                                                                    resultado = MovimientoAsignacionRecurso.DeshabilitaAsignacionesRecursoMovimiento(movimientoSiguiente.id_servicio, movimientoSiguiente.id_movimiento, MovimientoAsignacionRecurso.Tipo.Operador, o.id_operador, id_usuario);
                                                                else if (!resultado.OperacionExitosa)
                                                                    resultado = new RetornoOperacion(string.Format("Error al actualizar la operador '{0}' a estatus 'Tránsito'.", o.nombre));
                                                            }
                                                            //De lo contrario
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Operador Id '{0}' no encontrado.", r.Field<int>("IdRecurso")));
                                                        }
                                                        break;
                                                    case MovimientoAsignacionRecurso.Tipo.Tercero:
                                                        //Instanciando tercero
                                                        using (Global.CompaniaEmisorReceptor tercero = new CompaniaEmisorReceptor(r.Field<int>("IdRecurso")))
                                                        {
                                                            //Si el tercero existe
                                                            if (tercero.id_compania_emisor_receptor > 0)
                                                                //Sólo se deshabilita la asignación del siguiente movimiento en caso de existir
                                                                resultado = MovimientoAsignacionRecurso.DeshabilitaAsignacionesRecursoMovimiento(movimientoSiguiente.id_servicio, movimientoSiguiente.id_movimiento, MovimientoAsignacionRecurso.Tipo.Tercero, r.Field<int>("IdRecurso"), id_usuario);
                                                            else
                                                                resultado = new RetornoOperacion(string.Format("Tercero Id '{0}' no encontrado.", r.Field<int>("IdRecurso")));
                                                        }
                                                        break;
                                                }

                                                //Si hay algún error
                                                if (!resultado.OperacionExitosa)
                                                    break;
                                            }

                                            //Si se deshabilitaron las asignaciones del movimiento a reversar y se tiene un movimiento posterior
                                            if (resultado.OperacionExitosa && movimientoSiguiente.id_movimiento > 0)
                                                //Moviendo estancias de unidades restantes del movimiento siguiente a la parada comodín y liberando recursos ocupados
                                                resultado = MovimientoAsignacionRecurso.ReversaMueveAsignacionesParada(movimiento.id_parada_destino, id_usuario);

                                                                                            

                                            //Si no hay errores en deshabilitación de estancias y asignaciones posteriores
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Iniciando asignaciones del movimiento que se encontraban terminadas
                                                resultado = MovimientoAsignacionRecurso.IniciaMovimientosAsignacionRecursoTerminados(movimiento.id_movimiento, id_usuario);

                                                //Si no se actualizaron las asignaciones previas
                                                if (!resultado.OperacionExitosa)
                                                    resultado = new RetornoOperacion(string.Format("Error al reiniciar las asignaciones del movimiento: {0}", movimiento.id_movimiento));
                                            }
                                        }
                                        //Si no se encontró la parada
                                        else
                                            resultado = new RetornoOperacion("La Parada Destino del Movimiento no pudo ser recuperada.");
                                    }
                                }
                            }
                        }
                    }
                    //SI no se localizó el movimiento
                    else
                        resultado = new RetornoOperacion("No es posible recuperar el movimiento solicitado.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando actualizaciones realizadas
                    scope.Complete();
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilita estancias de unidades cuya asignación a un movimiento sea terminada, siempre que la fecha de inicio de la estancia sea la misma a la fecha de llegada de la parada destino del movimiento
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaEstanciaUnidadesMovimientoVacio(int id_movimiento, int id_parada_destino, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("No hay asignaciones de recursos para este movimiento.");

            //Inicializando transaccion
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargando las asignaciones terminadas del movimiento en cuestion
                using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaAsignacionesMovimiento(id_movimiento, MovimientoAsignacionRecurso.Estatus.Terminado))
                {
                    //Si hay registros
                    if (Validacion.ValidaOrigenDatos(mitAsignaciones))
                    {
                        //Instanciando la parada destino del movimiento
                        using (Parada paradaDestino = new Parada(id_parada_destino))
                        {
                            //Si la parada fue recuperada
                            if (paradaDestino.id_parada > 0)
                            {
                                //Para cada una de las asignaciones
                                foreach (DataRow r in mitAsignaciones.Rows)
                                {
                                    switch((MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion"))
                                    {
                                        case MovimientoAsignacionRecurso.Tipo.Unidad:
                                            //Instanciando unidad
                                            using (Unidad u = new Unidad(Convert.ToInt32(r.Field<int>("IdRecurso"))))
                                            {
                                                //Si la unidad existe
                                                if (u.id_unidad > 0)
                                                {
                                                    //Obteniendo la estancia de la unidad, correspondiente a la ubicación de la parada en la fecha de llegada a la parada
                                                    using (EstanciaUnidad estancia = new EstanciaUnidad(ObtieneEstanciaUnidadFechaInicio(u.id_unidad, paradaDestino.id_ubicacion, paradaDestino.fecha_llegada)))
                                                    {
                                                        //Si la estancia existe
                                                        if (estancia.id_estancia_unidad > 0)
                                                            //Deshabilitando estancia
                                                            resultado = estancia.DeshabilitaEstanciaUnidad(id_usuario);
                                                        //Si no se encontró la estanci
                                                        else
                                                            resultado = new RetornoOperacion(string.Format("No es posible encontrar la estancia de la unidad '{0}' para su deshabilitación.", u.numero_unidad));
                                                    }

                                                }
                                                //De lo contrario
                                                else
                                                    resultado = new RetornoOperacion(string.Format("Unidad Id '{0}' no encontrada.", r.Field<int>("IdRecurso")));
                                            }
                                            break;
                                        default:
                                            resultado = new RetornoOperacion(0);
                                            break;
                                            
                                    }
                                    //Si hay algún error
                                    if (!resultado.OperacionExitosa)
                                        break;
                                }
                            }
                            //Si no se encontró la parada
                            else
                                resultado = new RetornoOperacion("La Parada Destino del Movimiento no pudo ser recuperada.");
                        }
                    }
                }
                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando actualizaciones realizadas
                    scope.Complete();
            }

            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene la estancia coincidente con  ubicación, unidad y fecha de inicio solicitadas
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <param name="inicio_estancia">Fecha de Inicio de Estancia</param>
        /// <returns></returns>
        public static int ObtieneEstanciaUnidadFechaInicio(int id_unidad, int id_ubicacion, DateTime inicio_estancia)
        {
            //Declarando objeto de resultado
            int id_estancia = 0;

            //Armando parametros para consulta de estancia
            object[] param = { 16, 0, 0, id_unidad, 0, 0, inicio_estancia, 0, null, 0, 0, false, null, id_ubicacion, "" };

            //Obteniendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Para cada resultado encontrado
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando resultado
                        id_estancia = r.Field<int>("Id");
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return id_estancia;
        }
        /// <summary>
        /// Realiza la actualización del id de parada actual de la estancia
        /// </summary>
        /// <param name="id_nueva_parada">Id de nueva parada por asignar</param>
        /// <param name="id_usuario">Id de Usuario que actualzia</param>
        /// <returns></returns>
        public RetornoOperacion CambiaParadaEstanciaUnidad(int id_nueva_parada, int id_usuario)
        {
            //Actualizando datos de estancia
            return editaEstanciaUnidad(id_nueva_parada, this._id_unidad, this.EstatusEstanciaUnidad, this.TipoEstanciaUnidad, this._inicio_estancia,
                                this.TipoActualizacionInicio_, this._fin_estancia, this.TipoActualizacionFin_, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la actualización del id de parada actual de la(s) estancia(s) de la(s) unidad(es) señalada(s), siempre y cuando se encuentren dentro de la misma ubicación.
        /// </summary>
        /// <param name="id_unidad">Conjunto de unidades a mover</param>
        /// <param name="id_parada">Id de Parada por asignar</param>
        /// <param name="id_ubicacion">Id de ubicación dónde debe estar la unidad</param>
        /// <param name="id_usuario">Id de usuario que solicita la actualización</param>
        /// <returns></returns>
        public static RetornoOperacion CambiaParadaEstanciaUnidadesUbicacion(List<int> id_unidad, int id_parada, int id_ubicacion, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No se ha especificado ninguna unidad.");

            //Validando la existencia de unidades que actualizar
            if (id_unidad != null)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Para cada una de las unidades
                    foreach (int idU in id_unidad)
                    {
                        //Instanciando unidad indicada
                        using (Unidad unidad = new Unidad(idU))
                        {
                            //Si la unidad se cargó correctamente
                            if (unidad.id_unidad > 0)
                            {
                                //Validando estatus de la unidad
                                if (unidad.EstatusUnidad == Unidad.Estatus.ParadaDisponible)
                                {
                                    //Obteniendo su estancia actual, validando que se encuentre en la misma ubicación
                                    using (EstanciaUnidad estanciaActualUnidad = new EstanciaUnidad(ObtieneEstanciaUnidadEnUbicacionIniciada(idU, id_ubicacion)))
                                    {
                                        //Si la estancia se cargó correctamente
                                        if (estanciaActualUnidad.id_estancia_unidad > 0)
                                            //Actualizando la parada de la estancia
                                            resultado = estanciaActualUnidad.CambiaParadaEstanciaUnidad(id_parada, id_usuario);
                                        //Si no hay una estancia iniciada en la ubicación
                                        else
                                            resultado = new RetornoOperacion(string.Format("La Unidad '{0}' ya no posee estancia activa en esta ubicación.", unidad.numero_unidad));
                                    }
                                }
                                //De lo contrario
                                else
                                    resultado = new RetornoOperacion(string.Format("La Unidad '{0}' no se encuentra disponible.", unidad.numero_unidad));
                            }
                            //Si no se encuentra la unidad
                            else
                                resultado = new RetornoOperacion(string.Format("ID {0}: Unidad no encontrada.", idU));
                        }

                        //Si existe algún error
                        if (!resultado.OperacionExitosa)
                            //Saliendo de ciclo
                            break;
                    }

                    //Si no hay errores detectados
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Terminamos todas las estancias de la unidades ligando un Id parada
        /// </summary>
        /// <param name="mitRecursos">Recursos para terminación de estancia</param>
        /// <param name="fin_estancia">Fecha fin de la estancia</param>
        /// <param name="tipo_actualizacion_fin">Tipo actualización fin  de la estancia(Manual, GPS)</param>
        /// <param name="tipo_actualizacion_salida">Tipo actualizacion Salida de la Parada (Manual, GPS)</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion TerminaEstanciasUnidad(DataTable mitRecursos, DateTime fin_estancia, TipoActualizacionFin tipo_actualizacion_fin,
                                                           Parada.TipoActualizacionSalida tipo_actualizacion_salida, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Validamos origen de datos
            if (Validacion.ValidaOrigenDatos(mitRecursos))
            {
                //Por cada Estancia
                foreach (DataRow r in mitRecursos.Rows)
                {
                    //Instanciamos Estancia
                    using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(ObtieneEstanciaUnidadIniciada(r.Field<int>("Id"))))
                    {
                        //Validamos que exista estancia
                        if (objEstanciaUnidad.id_estancia_unidad > 0)
                            //Terminamos Estancia
                            resultado = objEstanciaUnidad.TerminaEstanciaUnidad(fin_estancia, tipo_actualizacion_fin, id_usuario);
                        else
                        {
                            //Instanciamos Unidad
                            using (Global.Unidad objUnidad = new Global.Unidad(r.Field<int>("Id")))
                                //Establecemos Mensaje Error
                                resultado = new RetornoOperacion("No se encontró estancia de la Unidad: " + objUnidad.numero_unidad);
                        }
                    }

                    //Si hay errores de actualización
                    if (!resultado.OperacionExitosa)
                        break;
                }

            }
            else
                //Establecemos Mensaje Error
                resultado = new RetornoOperacion("No se ecnontró estancias de los Recursos");

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Termina la estancia activa de las unidades especificadas
        /// </summary>
        /// <param name="id_unidad">Conjunto de Id de Unidad por actualizar</param>
        /// <param name="fin_estancia">Fecha de Fin de su estancia</param>
        /// <param name="tipo_actualizacion_fin">Origen de la actualización</param>
        /// <param name="id_usuario">Id de Usuario que solicita actualización</param>
        /// <returns></returns>
        public static RetornoOperacion TerminaEstanciaUnidades(List<int> id_unidad, DateTime fin_estancia, TipoActualizacionFin tipo_actualizacion_fin, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("No se ha especificado ninguna unidad.");

            //Validamos origen de datos
            if (id_unidad != null)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Por cada Estancia
                    foreach (int idU in id_unidad)
                    {
                        //Instanciamos Estancia
                        using (EstanciaUnidad objEstanciaUnidad = new EstanciaUnidad(ObtieneEstanciaUnidadIniciada(idU)))
                        {
                            //Validamos que exista estancia
                            if (objEstanciaUnidad.id_estancia_unidad > 0)
                                //Terminamos Estancia
                                resultado = objEstanciaUnidad.TerminaEstanciaUnidad(fin_estancia, tipo_actualizacion_fin, id_usuario);
                            else
                            {
                                //Instanciamos Unidad
                                using (Global.Unidad unidad = new Global.Unidad(idU))
                                    //Establecemos Mensaje Error
                                    resultado = new RetornoOperacion(string.Format("La Unidad '{0}' no tiene ninguna estancia activa.", unidad.numero_unidad));
                            }
                        }

                        //Si hay errores de actualización
                        if (!resultado.OperacionExitosa)
                            break;
                    }

                    //Si no hay errores detectados
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza el estatus de las asignaciones de una parada, de iniciado a terminado
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="fin_estancia">Fecha de fin de las estancias</param>
        /// <param name="tipo_actualizacion_fin">Tipo de Actualización de fin de estancias</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion TerminaEstanciasIniciadasParada(int id_parada, DateTime fin_estancia, TipoActualizacionFin tipo_actualizacion_fin, int id_usuario)
        { 
            //Declarando objeto de resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(0, "No hay estancias iniciadas en esta parada.", true);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo las estancias en estatus iniciado de una parada
                using (DataTable mit = CargaEstanciasIniciadas(id_parada))
                {
                    //Si hay estancias activas
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las estancias encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciando estancia correspondiente
                            using (EstanciaUnidad estancia = new EstanciaUnidad(r.Field<int>("Id")))
                            {
                                //Realizando actualizanción de estatus
                                resultado = estancia.TerminaEstanciaUnidad(fin_estancia, tipo_actualizacion_fin, id_usuario);
                                //Si existe algún error
                                if (!resultado.OperacionExitosa)
                                {
                                    //Actualizando error
                                    resultado = new RetornoOperacion(string.Format("Error al Terminar la estancia Id '{0}': {1}.", estancia.id_estancia_unidad, resultado.Mensaje));
                                    //Saliendo del ciclo de actualización
                                    break;
                                }
                            }
                        }
                    }
                }

                //Si no hay errores se confirma la actualización general
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza el estatus de las asignaciones de una parada, de iniciado a terminado
        /// </summary>
        /// <param name="id_parada">Id de Parada</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion IniciaEstanciasTerminadasParada(int id_parada, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_parada);

            //Inicializando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo las estancias en estatus iniciado de una parada
                using (DataTable mit = CargaEstanciasTerminadasParada(id_parada))
                {
                    //Si hay estancias activas
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Para cada una de las estancias encontradas
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciando estancia correspondiente
                            using (EstanciaUnidad estancia = new EstanciaUnidad(r.Field<int>("Id")))
                            {
                                //Realizando actualizanción de estatus de la estancia a Iniciado
                                resultado = estancia.IniciaEstanciaUnidad(estancia.inicio_estancia, estancia.TipoActualizacionInicio_, id_usuario);
                                //Si existe algún error
                                if (!resultado.OperacionExitosa)
                                {
                                    //Actualizando error
                                    resultado = new RetornoOperacion(string.Format("Error al Terminar la estancia Id: '{0}'.", estancia.id_estancia_unidad));
                                    //Saliendo del ciclo de actualización
                                    break;
                                }
                            }
                        }
                    }
                }

                //Si no hay errores se confirma la actualización general
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene una Estancia Unidad iniciada liganda a un Id de Ubicación
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_ubicacion">Id de Ubicación</param>
        /// <returns></returns>
        public static int ObtieneEstanciaUnidadEnUbicacionIniciada(int id_unidad, int id_ubicacion)
        {
            //Declaramos variables
            int id_estancia = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 7, 0, 0, id_unidad, 0, 0, null, 0, null, 0, 0, false, null, id_ubicacion, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_estancia = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();

                //Devolviendo resultado
                return id_estancia;
            }
        }

        /// <summary>
        /// Validamos estancia disponibles ligando un id de parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="mitRecursos">Recursos</param>
        /// <param name="id_recurso_servicio">Id Recurso Servicio</param>
        /// <returns></returns>
        public static int ValidaEstanciaParada(int id_parada, DataTable mitRecursos, int id_recurso_servicio)
        {
            //Definiendo objeto de retorno
            int TotalUnidades = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 8, 0, id_parada, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //validamos existencia de Recursos
                    if (Validacion.ValidaOrigenDatos(mitRecursos))
                    {
                        //Recorremos cada uno de los recursos a mover
                        foreach (DataRow r in mitRecursos.Rows)
                        {
                            //Recorremos los recursos asignado al id de parada de la estancia
                            foreach (DataRow IdRecurso in ds.Tables["Table"].Rows)
                            {
                                //Validamos existencia de los recursos de las unidades vs recursos a mover
                                if (r.Field<int>("Id") == IdRecurso.Field<int>("Id") & (IdRecurso.Field<int>("Id") == id_recurso_servicio || id_recurso_servicio == 0))
                                {
                                    //Eliminamos Recurso
                                    ds.Tables["Table"].Rows.Remove(IdRecurso);
                                    break;
                                }
                            }
                        }
                        //Obtenemos total de unidades aún diponibles
                        TotalUnidades = ds.Tables["Table"].Rows.Count;

                    }
                }
            }

            //Devolviendo resultado
            return TotalUnidades;

        }
        
        /// <summary>
        /// Carga estancias activas ligando un Id Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaEstanciasActivasParada(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 9, 0, id_parada, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };

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
        /// Obtiene el Total de estancias que fuerón registradas a partir de un inicio estancia por diferentes servicios.
        /// </summary>
        /// <param name="id_servicio">Id Servicio actual</param>
        /// <param name="id_unidad">Id Unidad de las estancias a consultar</param>
        /// <param name="inicio_estancia">Fecha de inicio de la estancia a partir de la cual se recuperan las estancias</param>
        /// <returns></returns>
        public static int ObtieneEstanciasPosterioresPorDiferenteServicio(int id_servicio, int id_unidad, DateTime inicio_estancia)
        {
            //Definiendo objeto de retorno
            int id_total_estancias = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 10, 0, 0, id_unidad, 0, 0, inicio_estancia, 0, null, 0, 0, false, null, id_servicio, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando a objeto de retorno
                    id_total_estancias = (from DataRow r in ds.Tables["Table"].Rows
                                          select r.Field<int>("Total")).FirstOrDefault();
                }

                //Devolviendo resultado
                return id_total_estancias;
            }
        }

        /// <summary>
        /// Carga Estancias Terminadas ligadas a un Id de Parada
        /// </summary>
        /// <param name="id_parada">Id de la Parada al cual pertenece la Estancia de Unidad</param>
        /// <returns></returns>
        public static DataTable CargaEstanciasTerminadasParada(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 11, 0, id_parada, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };


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
        /// Obtiene una Estancia Unidad en cualquier estatus asociada a una Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static int ObtieneEstanciaUnidadEnParada(int id_parada, int id_unidad)
        {
            //Declaramos variables
            int id_estancia = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 12, 0, id_parada, id_unidad, 0, 0, null, 0, null, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_estancia = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();

                //Devolviendo resultado
                return id_estancia;
            }
        }        
        /// <summary>
        /// Obtenemos la última edtnacia de la unidad terminada
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <returns></returns>
        public static int ObtieneUltimaEstanciaUnidadTerminada(int id_unidad)
        {
            //Definiendo objeto de retorno
            int id_estancia = 0;

            //inicializamos el arreglo de parametros
            object[] param = { 13, 0, 0, id_unidad, 0, 0, null, 0, null, 0, 0, false, null, "", "" };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    id_estancia = (from DataRow r in ds.Tables["Table"].Rows
                                   select r.Field<int>("Id")).FirstOrDefault();

                //Devolviendo resultado
                return id_estancia;
            }
        }
        /// <summary>
        /// Carga estancias ligando un Id Parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static DataTable CargaEstancias(int id_parada)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 14, 0, id_parada, 0, 0, 0, null, 0, null, 0, 0, false, null, "", "" };

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
        /// Validamos si existen estancias ligadas a la parada
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaExistenciaEstanciasParada(int id_parada)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Estancias
            using (DataTable mitEstancias = EstanciaUnidad.CargaEstancias(id_parada))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitEstancias))
                {
                    //Recorremos la estancia
                    foreach (DataRow r in mitEstancias.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Estancia
                            using (EstanciaUnidad objEstancia = new EstanciaUnidad(r.Field<int>("Id")))
                            {
                                //Instanciamos Unidad 
                                using (Unidad objUnidad = new Unidad(objEstancia.id_unidad))
                                {
                                    //Establecemos Mensaje
                                    resultado = new RetornoOperacion("Imposible eliminar la parada ya que existe la estancia de la Unidad " + objUnidad.numero_unidad + ".");
                                    //Salimos del ciclo
                                    break;
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
        /// Carga estancias en cualquier estatus asignadas a una unidad determinada, cuya fecha de inicio de estancia sea mayor a una fecha dada
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="inicio_estancia">Fecha de Inicio de la estancia</param>
        /// <returns></returns>
        public static DataTable CargaEstanciasPosterioresFechaInicio(int id_unidad, DateTime inicio_estancia)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 15, 0, 0, id_unidad, 0, 0, inicio_estancia, 0, null, 0, 0, false, null, "", "" };

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

        #endregion
    }
}
