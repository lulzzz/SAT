using Microsoft.SqlServer.Types;
using SAT_CL.EgresoServicio;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Despacho
{
    /// <summary>
    /// Implementa los método para la administración de Movimientos
    /// </summary>
    public partial class Movimiento : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera el Estatus del Movimiento
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            ///  Registrado
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Iniciado
            /// </summary>
            Iniciado,
            /// <summary>
            /// Terminado
            /// </summary>
            Terminado

        }
        /// <summary>
        /// Enumera el Tipo de Movimiento
        /// </summary>
        public enum Tipo
        {
            /// <summary>
            ///  Cargado
            /// </summary>
            Cargado = 1,
            /// <summary>
            /// Vacio
            /// </summary>
            Vacio,
            /// <summary>
            /// En Tronco
            /// </summary>
            EnTronco

        }
        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "despacho.sp_movimiento_tm";


        private int _id_movimiento;
        /// <summary>
        /// Describe el Id de movimiento
        /// </summary>
        public int id_movimiento
        {
            get { return _id_movimiento; }
        }
        private int _id_servicio;
        /// <summary>
        /// Describe el Id de Sevicio
        /// </summary>
        public int id_servicio
        {
            get { return _id_servicio; }
        }
        private int _id_segmento_carga;
        /// <summary>
        /// Describe el Id de Segmento de Carga
        /// </summary>
        public int id_segmento_carga
        {
            get { return _id_segmento_carga; }
        }
        private decimal _secuencia_servicio;
        /// <summary>
        /// Describe la secuencia servicio
        /// </summary>
        public decimal secuencia_servicio
        {
            get { return _secuencia_servicio; }
        }
        private byte _id_estatus_movimiento;
        /// <summary>
        /// Describe el estatus
        /// </summary>
        public byte id_estatus_movimiento
        {
            get { return _id_estatus_movimiento; }
        }
        private byte _id_tipo_movimiento;
        /// <summary>
        /// Describe el tipo de movimiento
        /// </summary>
        public byte id_tipo_movimiento
        {
            get { return _id_tipo_movimiento; }
        }
        private decimal _kms;
        /// <summary>
        /// Describe el kms
        /// </summary>
        public decimal kms
        {
            get { return _kms; }
        }
        private decimal _kms_maps;
        /// <summary>
        /// Describe el kms maps
        /// </summary>
        public decimal kms_maps
        {
            get { return _kms_maps; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Describe el Id Compañia Emisor
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _id_parada_origen;
        /// <summary>
        /// Describe la parada Origen
        /// </summary>
        public int id_parada_origen
        {
            get { return _id_parada_origen; }
        }

        private int _id_parada_destino;
        /// <summary>
        /// Describe la parada Destino
        /// </summary>
        public int id_parada_destino
        {
            get { return _id_parada_destino; }
        }

        private string _descripcion;
        /// <summary>
        /// Describe el movimiento
        /// </summary>
        public string descripcion
        {
            get { return _descripcion; }
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
        /// Enumera el Estatus Movimiento
        /// </summary>
        public Estatus EstatusMovimiento
        {
            get { return (Estatus)_id_estatus_movimiento; }
        }

        /// <summary>
        /// Enumera el Tipo Movimiento
        /// </summary>
        public Tipo TipoMovimiento
        {
            get { return (Tipo)_id_tipo_movimiento; }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Movimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public Movimiento()
        {

        }

        /// <summary>
        /// Genera una Instancia de Tipo Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        public Movimiento(int id_movimiento)
        {
            cargaAtributosInstancia(id_movimiento);
        }

        /// <summary>
        /// Genera una Instancia Movimiento ligando un Id Parada Origen, Id Parada Destino, Id Servicio
        /// </summary>
        ///<param name="id_servicio">Id Servicio</param>
        ///<param name="id_secuencia">Id de Secuencia</param>
        public Movimiento(int id_servicio, int id_secuencia)
        {
            cargaAtributosInstancia(id_servicio, id_secuencia);
        }

        /// <summary>
        /// Genera una Instancia Movimiento ligando un Id Parada Origen, Id Parada Destino, Id Servicio
        /// </summary>
        ///<param name="id_parada_origen">Id Parada Origen</param>
        ///<param name="id_parada_destino">Id Parada Destino</param>
        ///<param name="id_servicio">Id Servicio</param>
        public Movimiento(int id_parada_origen, int id_parada_destino, int id_servicio)
        {
            cargaAtributosInstancia(id_parada_origen, id_parada_destino, id_servicio);
        }

        /// <summary>
        /// Genera una Instancia de Tipo Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_movimiento)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_movimiento, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _id_segmento_carga = Convert.ToInt32(r["IdSegmentoCarga"]);
                        _secuencia_servicio = Convert.ToDecimal(r["SecuenciaServicio"]);
                        _id_estatus_movimiento = Convert.ToByte(r["IdEstatus"]);
                        _id_tipo_movimiento = Convert.ToByte(r["IdTipoMovimeinto"]);
                        _kms = Convert.ToDecimal(r["KMS"]);
                        _kms_maps = Convert.ToDecimal(r["KMSMaps"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _id_parada_origen = Convert.ToInt32(r["IdParadaOrigen"]);
                        _descripcion = r["Descripcion"].ToString();
                        _id_parada_destino = Convert.ToInt32(r["IdParadDestino"]);
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
        /// Genera una Instancia de Tipo Movimiento
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_secuencia">Id de Secuencia</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio, int id_secuencia)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 23, 0, id_servicio, 0, id_secuencia, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _id_segmento_carga = Convert.ToInt32(r["IdSegmentoCarga"]);
                        _secuencia_servicio = Convert.ToDecimal(r["SecuenciaServicio"]);
                        _id_estatus_movimiento = Convert.ToByte(r["IdEstatus"]);
                        _id_tipo_movimiento = Convert.ToByte(r["IdTipoMovimeinto"]);
                        _kms = Convert.ToDecimal(r["KMS"]);
                        _kms_maps = Convert.ToDecimal(r["KMSMaps"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _id_parada_origen = Convert.ToInt32(r["IdParadaOrigen"]);
                        _id_parada_destino = Convert.ToInt32(r["IdParadDestino"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                        _descripcion = r["Descripcion"].ToString();
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
        /// Genera una Instancia de Tipo Movimiento
        /// </summary>
        /// <param name="id_parada_origen">Id Parada Origen</param>
        /// <param name="id_parada_destino">Id Parada Destino</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_parada_origen, int id_parada_destino, int id_servicio)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 7, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, id_parada_origen, id_parada_destino, 0, false, null, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {

                        _id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        _id_servicio = Convert.ToInt32(r["IdServicio"]);
                        _id_segmento_carga = Convert.ToInt32(r["IdSegmentoCarga"]);
                        _secuencia_servicio = Convert.ToDecimal(r["SecuenciaServicio"]);
                        _id_estatus_movimiento = Convert.ToByte(r["IdEstatus"]);
                        _id_tipo_movimiento = Convert.ToByte(r["IdTipoMovimeinto"]);
                        _kms = Convert.ToDecimal(r["KMS"]);
                        _kms_maps = Convert.ToDecimal(r["KMSMaps"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _id_parada_origen = Convert.ToInt32(r["IdParadaOrigen"]);
                        _id_parada_destino = Convert.ToInt32(r["IdParadDestino"]);
                        _descripcion = r["Descripcion"].ToString();
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
        /// Método encargado de Editar un Movimiento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece el Movimiento</param>
        /// <param name="id_segmento_carga">Id de Segmento de Carga al que pertencece el Movimiento</param>
        /// <param name="secuencia_servicio">Secuencia del Movimiento</param>
        /// <param name="estatus_movimiento">Estatus el cuel se encuentra el Movimiento</param>
        /// <param name="tipo_movimiento">Tipo de Movimiento que se esta realizando</param>
        /// <param name="kms">Almacena el kilometraje</param>
        /// <param name="kms_maps">Almacena el kilometraje proporcionado por el GIS</param>
        /// <param name="id_compania_emisor">Id de Compañia del movimiento en caso de que el Servicio sea 0</param>
        /// <param name="id_parada_origen">Establece la Parada Origen del Movimiento</param>
        /// <param name="id_parada_destino">Establece la Parada Destino del Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaMovimiento(int id_servicio, int id_segmento_carga, decimal secuencia_servicio, Estatus estatus_movimiento,
                                               Tipo tipo_movimiento, decimal kms, decimal kms_maps, int id_compania_emisor, int id_parada_origen,
                                              int id_parada_destino, int id_usuario, bool habilitar)
        {
            //Establecemos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Versión del Registro
            if (validaVersionRegistro())
            {
                //Inicializando arreglo de parámetros
                object[] param = {2, this._id_movimiento, id_servicio, id_segmento_carga, secuencia_servicio, estatus_movimiento, tipo_movimiento,
                                     kms, kms_maps, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario, habilitar, this._row_version, "", ""  };

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
            object[] param = { 4, this._id_movimiento, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, this._row_version, "", "" };

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
        /// Determina si los recursos proporcionados para un movimiento son los adecuados para su Inicio
        /// </summary>
        /// <param name="id_tercero">Id de Tercero</param>
        /// <param name="id_motriz">Id de Unidad Motriz</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <returns></returns>
        private static RetornoOperacion validaRecursosInicioMovimiento(int id_tercero, int id_motriz, int id_operador)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("Es necesario asignar una unidad motríz o un tercero para realizar el movimiento.");

            //Si la unidad motriz se especificó
            if (id_motriz > 0)
            {
                //Instanciando unidad
                using (Unidad unidadMotriz = new Unidad(id_motriz))
                {
                    //Si la unidad existe
                    if (unidadMotriz.id_unidad > 0)
                        //Si la unidad es propia, también se debe especificar un operador
                        if (!unidadMotriz.bit_no_propia && id_operador == 0)
                            resultado = new RetornoOperacion("Es necesario asignar un operador para este movimiento.");
                        //Si la unidad es de permisionario o el operador está indicado (junto con una unidad motriz propia)
                        else
                            //Validamos que el operador sea el asignado
                            if (unidadMotriz.id_operador == id_operador)
                                resultado = new RetornoOperacion(0);
                            else
                                resultado = new RetornoOperacion("El operador no se encuentra asignado a la Unidad.");

                }
            }
            //Si no hay tercero, pero hay unidad motriz
            else if (id_tercero > 0)
                resultado = new RetornoOperacion(0);

            //Si el Operador se Especifico
            if (id_operador > 0)
            {
                //Instanciamos Operador
                using (Operador objOperador = new Operador(id_operador))
                {
                    //Si el Operador existe
                    if (objOperador.id_operador > 0)
                    {
                        //Validamos Estatus de Operador
                        if (objOperador.estatus == Operador.Estatus.Disponible || objOperador.estatus == Operador.Estatus.Registrado)
                            resultado = new RetornoOperacion(0);
                        //De lo contrario
                        else
                            resultado = new RetornoOperacion(string.Format("El operador '{0}' no se encuentra disponible.", objOperador.nombre));
                    }
                    else
                    {
                        //Mostramos Error
                        resultado = new RetornoOperacion("No se pueden tener los datos complementarios del Operador");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene Ultima Secuencia del Movimiento
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        private static decimal obtieneSecuenciaMovimiento(int id_servicio)
        {
            //Declaramos Resultados
            decimal secuencia = 1;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Secuencia
                    decimal secuenciaBD = (from DataRow r in ds.Tables[0].Rows
                                           select Convert.ToDecimal(r["Secuencia"])).FirstOrDefault();

                    //Asignamos Secuencia Correspondiente
                    secuencia = secuenciaBD + 1;
                }
            }
            //Obtenemos Resultado
            return secuencia;
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar un Movimiento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece el Movimiento</param>
        /// <param name="id_segmento_carga">Id de Segmento de Carga al que pertencece el Movimiento</param>
        /// <param name="tipo_movimiento">Tipo de Movimiento que se esta realizando</param>
        /// <param name="kms">Almacena el kilometraje</param>
        /// <param name="kms_maps">Almacena el kilometraje proporcionado por el GIS</param>
        /// <param name="id_compania_emisor">Id de Compañia del movimiento en caso de que el Servicio sea 0</param>
        /// <param name="id_parada_origen">Establece la Parada Origen del Movimiento</param>
        /// <param name="id_parada_destino">Establece la Parada Destino del Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimiento(int id_servicio, int id_segmento_carga,
                                               Tipo tipo_movimiento, decimal kms, decimal kms_maps, int id_compania_emisor, int id_parada_origen,
                                              int id_parada_destino, int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1,0,id_servicio, id_segmento_carga, obtieneSecuenciaMovimiento(id_servicio), Estatus.Registrado, tipo_movimiento,
                                     kms, kms_maps, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario, true, null, "", ""  };

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        /// <summary>
        /// Método encargado de Insertar un Movimiento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece el Movimiento</param>
        /// <param name="id_segmento_carga">Id de Segmento de Carga al que pertencece el Movimiento</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_movimiento">Tipo de Movimiento que se esta realizando</param>
        /// <param name="kms">Almacena el kilometraje</param>
        /// <param name="kms_maps">Almacena el kilometraje proporcionado por el GIS</param>
        /// <param name="id_compania_emisor">Id de Compañia del movimiento en caso de que el Servicio sea 0</param>
        /// <param name="id_parada_origen">Establece la Parada Origen del Movimiento</param>
        /// <param name="id_parada_destino">Establece la Parada Destino del Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimiento(int id_servicio, int id_segmento_carga, decimal secuencia,
                                                Tipo tipo_movimiento, decimal kms, decimal kms_maps, int id_compania_emisor, int id_parada_origen,
                                               int id_parada_destino, int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1,0,id_servicio, id_segmento_carga, secuencia, Estatus.Registrado, tipo_movimiento,
                                     kms, kms_maps, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario, true, null, "", ""  };

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        /// <summary>
        /// Método encargado de Insertar un Movimiento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece el Movimiento</param>
        /// <param name="id_segmento_carga">Id de Segmento de Carga al que pertencece el Movimiento</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="tipo_movimiento">Tipo de Movimiento que se esta realizando</param>
        /// <param name="kms">Almacena el kilometraje</param>
        /// <param name="kms_maps">Almacena el kilometraje proporcionado por el GIS</param>
        /// <param name="id_compania_emisor">Id de Compañia del movimiento en caso de que el Servicio sea 0</param>
        /// <param name="id_parada_origen">Establece la Parada Origen del Movimiento</param>
        /// <param name="id_parada_destino">Establece la Parada Destino del Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimiento(int id_servicio, int id_segmento_carga, decimal secuencia, Estatus estatus,
                                         Tipo tipo_movimiento, decimal kms, decimal kms_maps, int id_compania_emisor, int id_parada_origen,
                                        int id_parada_destino, int id_usuario)
        {


            //Inicializando arreglo de parámetros
            object[] param = {1,0,id_servicio, id_segmento_carga, secuencia, estatus, tipo_movimiento,
                                     kms, kms_maps, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario, true, null, "", ""  };

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        /// <summary>
        /// Método encargado de Editar un Movimiento
        /// </summary>
        /// <param name="id_servicio">Id de Servicio al que pertenece el Movimiento</param>
        /// <param name="id_segmento_carga">Id de Segmento de Carga al que pertencece el Movimiento</param>
        /// <param name="secuencia_servicio">Secuencia del Movimiento</param>
        /// <param name="estatus_movimiento">Estatus el cuel se encuentra el Movimiento</param>
        /// <param name="tipo_movimiento">Tipo de Movimiento que se esta realizando</param>
        /// <param name="kms">Almacena el kilometraje</param>
        /// <param name="kms_maps">Almacena el kilometraje proporcionado por el GIS</param>
        /// <param name="id_compania_emisor">Id de Compañia del movimiento en caso de que el Servicio sea 0</param>
        /// <param name="id_parada_origen">Establece la Parada Origen del Movimiento</param>
        /// <param name="id_parada_destino">Establece la Parada Destino del Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaMovimiento(int id_servicio, int id_segmento_carga, decimal secuencia_servicio, Estatus estatus_movimiento,
                                                  Tipo tipo_movimiento, decimal kms, decimal kms_maps, int id_compania_emisor, int id_parada_origen,
                                                  int id_parada_destino, int id_usuario)
        {

            return this.editaMovimiento(id_servicio, id_segmento_carga, secuencia_servicio, estatus_movimiento, tipo_movimiento, kms,
                                       kms_maps, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Actualizamos estatus del movimiento
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaEstatusMovimiento(Estatus estatus, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que no existan pagos ligados al movimiento
            resultado = validaPagos();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Editamos Estatus del movimiento
                resultado = this.editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, estatus, (Tipo)this._id_tipo_movimiento,
                                            this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario, this._habilitar);
            }

            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Método encargado de Iniciar un Movimiento
        /// </summary>
        /// <param name="tipo_movimiento">Tipo Movimiento para inicio de paradas de Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaMovimiento(Tipo tipo_movimiento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Obtenemos el Tipo de Movimiento para su actualización
            Tipo _tipo_movimiento = ObtieneTipoMovimientoPorAsignar(tipo_movimiento);

            //Validamos Estatus
            if (Estatus.Iniciado != (Estatus)this._id_estatus_movimiento)
            {
                //Actualizamos estatus del Movimiento
                editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, Estatus.Iniciado, _tipo_movimiento,
                                     this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario,
                                     this._habilitar);
            }
            else
                //Asignamos etriueta Error
                resultado = new RetornoOperacion("El  movimiento ya se encuentra Iniciado.");

            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Terminamos Movimiento
        /// </summary>
        /// <param name="id_usuario">Id usuario</param>
        /// <returns></returns>
        private RetornoOperacion terminaMovimiento(int id_usuario)
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Terminamos Movimiento
            resultado = editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, Estatus.Terminado,
                                      (Tipo)this._id_tipo_movimiento, this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen,
                                      this._id_parada_destino, id_usuario, this._habilitar);
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Deshabilita un Movimiento   
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimiento(int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            // Validamos que el estatus este Registrado
            if ((Estatus)this._id_estatus_movimiento == Estatus.Registrado)
            {
                //Cargamos Asignaciones ligadas a un Id Movimiento
                using (DataTable mitAsignaciones = MovimientoAsignacionRecurso.CargaMovimientosAsignacion(this._id_movimiento))
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
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Validamos Deshabilitación de Movimiento
                        resultado = this.editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, (Estatus)this._id_estatus_movimiento, (Tipo)this._id_tipo_movimiento
                                                    , this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario, false);
                    }
                }
            }
            else
            {
                resultado = new RetornoOperacion("El estatus del movimiento no permite su edición.");
            }
            return resultado;
        }

        /// <summary>
        /// Deshabilitamos movimientos  y asignaciones ligando un id_servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaMovimientos(int id_servicio, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Movimientos
            using (DataTable mitMovimientos = CargaMovimientos(id_servicio))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitMovimientos))
                {
                    //Recorremos cada uno de los movimientos
                    foreach (DataRow r in mitMovimientos.Rows)
                    {
                        //Instanciamos Movimientos
                        using (Movimiento objMovimiento = new Movimiento(r.Field<int>("Id")))
                        {
                            //Deshabilitamos Asignaciones
                            resultado = MovimientoAsignacionRecurso.DeshabilitaMovimientosAsignacionesRecursos(objMovimiento.id_movimiento, id_usuario);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Deshablitamos Movimiento
                                resultado = objMovimiento.DeshabilitaMovimientoParaReversa(id_usuario);
                            }
                        }
                    }
                }
            }
            //Cargamos Movimientos 
            return resultado;
        }

        /// <summary>
        /// Deshabilita un Movimiento 
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoParaReversa(int id_usuario)
        {

            //deshabilitamos Movimiento Sin Importar el estatus
            return this.editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, (Estatus)this._id_estatus_movimiento, (Tipo)this._id_tipo_movimiento
                                              , this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario, false);
        }

        /// <summary>
        /// Carga Movimientos ligando un Id Servicio
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <returns></returns>
        public static DataTable CargaMovimientos(int id_servicio)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };


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
        /// Actualiza las secuencias de los Movimientos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaSecuenciaMovimientos(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, id_servicio, 0, secuencia, 0, 0, 0, 0, 0, 0, 0, id_usuario, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        ///  Obtenemos kilometraje Total de los Movimientos ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="total_kms_reales">Obtiene total de kms reales</param>
        public static void ObtieneKilometrajeTotal(int id_servicio, out decimal total_kms_reales)
        {
            //Declaramos Variables
            total_kms_reales = 0;

            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando arreglo de parámetros
            object[] param = { 9, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Obtenemos Totales Kms Reales
                    total_kms_reales = (from DataRow r in ds.Tables[0].Rows
                                        select Convert.ToDecimal(r["TotalKms"])).FirstOrDefault();
            }
        }


        /// <summary>
        /// Método encargado de cargar los Movimientos que se encuentren entre una Secuencia Origen y una Secuencia Destino (Operativas)
        /// </summary>
        ///<param name="secuencia_origen">Secuencia Origen</param>
        ///<param name="secuencia_destino">Secuencia Destino</param>
        ///<param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaMovimientos(int id_servicio, decimal secuencia_origen, decimal secuencia_destino)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 10, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, secuencia_origen, secuencia_destino };


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
        /// Método encargado de actualizar el segmento de los Movimientos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia_origen">Secuencia Origen</param>
        /// <param name="secuencia_destino">Secuencia Destino</param>
        /// <param name="id_segmento">Id Segmento Nuevo</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaSegmentoDeMovimientos(int id_servicio, decimal secuencia_origen, decimal secuencia_destino, int id_segmento, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Movimientos Sin Id de Segmento
            using (DataTable mit = CargaMovimientos(id_servicio, secuencia_origen, secuencia_destino))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos cada uno de los movimiento
                    foreach (DataRow r in mit.Rows)
                    {
                        //Validamos Actualización de los Movimientos
                        if (resultado.OperacionExitosa)
                        {
                            //Instanciamos Movimiento
                            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(r["Id"])))
                            {
                                //Editamos Movimiento
                                resultado = objMovimiento.EditaMovimiento(objMovimiento.id_servicio, id_segmento, objMovimiento.secuencia_servicio, (Estatus)objMovimiento.id_estatus_movimiento
                                            , (Tipo)objMovimiento.id_tipo_movimiento, objMovimiento.kms, objMovimiento.kms_maps, objMovimiento.id_compania_emisor,
                                            objMovimiento.id_parada_origen, objMovimiento.id_parada_destino, id_usuario);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Decrementa las secuencias de los Movimientos
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="secuencia">Secuencia</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DecrementaSecuenciaMovimientos(int id_servicio, decimal secuencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicializando arreglo de parámetros
            object[] param = { 11, 0, id_servicio, 0, secuencia, 0, 0, 0, 0, 0, 0, 0, id_usuario, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);

                //Devolviendo resultado
                return resultado;
            }
        }

        /// <summary>
        /// Método  encargado de Actualizar los atributos del Movimkento
        /// </summary>
        /// <returns></returns>
        public bool ActualizaMovimiento()
        {
            return this.cargaAtributosInstancia(this._id_movimiento);
        }

        /// <summary>
        /// Metodo encargado de actualizar el kilometraje del movimiento deseado en caso de encotrar cambios
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="id_usuario"></param>
        /// <returns>Regresa un objeto retorno operacion -50 - No hubo actualizacion; -100 - Kms No encontrados; Otro Valor - Actualizacion Realizada</returns>
        public static RetornoOperacion ActualizaKilometrajeMovimiento(int id_movimiento, int id_usuario)
        {

            //Declaramos la variable de retorno
            RetornoOperacion retorno = new RetornoOperacion(-50, "Sin Cambios", false);
            //Declaramos variables auxiliares
            decimal kilometraje = 0;
            int id_ruta = 0;
            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                if (objMovimiento.id_servicio > 0)
                    //Instanciamos el segmento para conocer la ruta 
                    using (SegmentoCarga objSegmento = new SegmentoCarga(objMovimiento.id_segmento_carga))
                    {
                        id_ruta = objSegmento.id_ruta;
                    }

                //Obtenemos el kilometraje del movimiento
                kilometraje = Kilometraje.BuscaKilometraje(id_movimiento, objMovimiento.id_compania_emisor, id_ruta);

                //Comparamos el valor del kilometraje con el kilometraje actual
                if (kilometraje != objMovimiento.kms)
                {
                    //En caso de tener kilometrajes diferentes procedemos a actualizar                    
                    retorno = objMovimiento.editaMovimiento(objMovimiento._id_servicio, objMovimiento._id_segmento_carga, objMovimiento._secuencia_servicio,
                                                             (Estatus)objMovimiento._id_estatus_movimiento, (Tipo)objMovimiento._id_tipo_movimiento, kilometraje,
                                                              kilometraje, objMovimiento._id_compania_emisor, objMovimiento._id_parada_origen,
                                                              objMovimiento._id_parada_destino, id_usuario, objMovimiento._habilitar);
                    //Si no hay errores
                    if (retorno.OperacionExitosa)
                    {
                        //ACTUALIZAR KM ASIGNADOS A UNIDADES MOTRICES
                        retorno = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(id_movimiento, kilometraje, id_usuario);
                    }
                }
                //En caso de no existir el kilometraje regresamos un codigo de error especifico
                if (kilometraje == 0)
                {
                    retorno = new RetornoOperacion(-100, "No Encontrado", false);
                }

            }
            //Regresamos el objeto de Retorno
            return retorno;
        }

        /// <summary>
        /// Método encargado de obtener el kilometraje de los Movimientos
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion CalculaKilometrajeMovimiento(int id_movimiento, int id_usuario)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            int IdKilometraje = 0;

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                //Instanciamos Parada Origen y Destino
                using (Parada objParadaOrigen = new Parada(objMovimiento._id_parada_origen), objParadaDestino = new Parada(objMovimiento._id_parada_destino))
                {
                    //Instanciamos Segmento ligado al movimiento
                    using (SegmentoCarga objSegmento = new SegmentoCarga(objMovimiento.id_segmento_carga))
                    {
                        //Buscamos Kilometraje del Movimiento
                        IdKilometraje = Kilometraje.BuscaKilometraje(objParadaOrigen.id_ubicacion, objParadaDestino.id_ubicacion,
                                        objMovimiento.id_compania_emisor, objSegmento.id_ruta);

                        //Declarando variables auxiliares para almacenamiento de kilometraje
                        decimal kms_maps = 0, kms_reales = 0;

                        //Si se Encontro el Kilometraje
                        if (IdKilometraje != 0)
                        {
                            //Instanciamos Kilometraje
                            using (Kilometraje objKilometraje = new Kilometraje(IdKilometraje))
                            {
                                //Asignando valores de kilometraje
                                kms_reales = objKilometraje.kms_reales;
                                kms_maps = objKilometraje.kms_maps;
                            }
                        }

                        //Actualizamos Kilometraje al movimiento
                        resultado = objMovimiento.editaMovimiento(objMovimiento._id_servicio, objMovimiento._id_segmento_carga, objMovimiento._secuencia_servicio,
                                                                 (Estatus)objMovimiento._id_estatus_movimiento, (Tipo)objMovimiento._id_tipo_movimiento, kms_reales,
                                                                  kms_maps, objMovimiento._id_compania_emisor, objMovimiento._id_parada_origen,
                                                                  objMovimiento._id_parada_destino, id_usuario, objMovimiento._habilitar);
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de obtener el kilometraje de los Movimientos
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="id_kilometraje">Id de Kilometraje</param>
        /// <returns></returns>
        public static RetornoOperacion CalculaKilometrajeMovimiento(int id_movimiento, int id_usuario, out int id_kilometraje)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Asignando valor Inicial
            id_kilometraje = 0;

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(id_movimiento))
            {
                //Instanciamos Parada Origen y Destino
                using (Parada objParadaOrigen = new Parada(objMovimiento._id_parada_origen), objParadaDestino = new Parada(objMovimiento._id_parada_destino))
                {
                    //Instanciamos Segmento ligado al movimiento
                    using (SegmentoCarga objSegmento = new SegmentoCarga(objMovimiento.id_segmento_carga))
                    {
                        //Buscamos Kilometraje del Movimiento
                        id_kilometraje = Kilometraje.BuscaKilometraje(objParadaOrigen.id_ubicacion, objParadaDestino.id_ubicacion,
                                        objMovimiento.id_compania_emisor, objSegmento.id_ruta);

                        //Declarando variables auxiliares para almacenamiento de kilometraje
                        decimal kms_maps = 0, kms_reales = 0;

                        //Si se Encontro el Kilometraje
                        if (id_kilometraje != 0)
                        {
                            //Instanciamos Kilometraje
                            using (Kilometraje objKilometraje = new Kilometraje(id_kilometraje))
                            {
                                //Asignando valores de kilometraje
                                kms_reales = objKilometraje.kms_reales;
                                kms_maps = objKilometraje.kms_maps;
                            }
                        }

                        //Actualizamos Kilometraje al movimiento
                        resultado = objMovimiento.editaMovimiento(objMovimiento._id_servicio, objMovimiento._id_segmento_carga, objMovimiento._secuencia_servicio,
                                                                 (Estatus)objMovimiento._id_estatus_movimiento, (Tipo)objMovimiento._id_tipo_movimiento, kms_reales,
                                                                  kms_maps, objMovimiento._id_compania_emisor, objMovimiento._id_parada_origen,
                                                                  objMovimiento._id_parada_destino, id_usuario, objMovimiento._habilitar);
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Obtenemos Kilometraje Recorrido de acuerdo al tipo de movimiento
        /// </summary>
        /// <param name="kms_recorridos">kms recorridos</param>
        /// <param name="kms_cargados_recorridos">kms cargados recorridos</param>
        /// <param name="kms_vacios_recorridos">kms vacios recorridos</param>
        /// <param name="kms_tronco_recorridos">kms tronco recorridos</param>
        public void obtenemosKilometrajeMovimiento(out decimal kms_recorridos, out decimal kms_cargados_recorridos,
                                                                     out decimal kms_vacios_recorridos, out decimal kms_tronco_recorridos)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            kms_recorridos = kms_cargados_recorridos = kms_vacios_recorridos = kms_tronco_recorridos = 0;

            //Asignamos km rrecorido
            kms_recorridos = this._kms;
            //Si el Tipo de Movimiento es Cargado
            if ((Tipo)this._id_tipo_movimiento == Tipo.Cargado)
            {
                //Establecemos kms cargados
                kms_cargados_recorridos = this._kms;

            }
            else
            {
                //Si el Tipo de Movimiento es Vacio
                if ((Tipo)this._id_tipo_movimiento == Tipo.Vacio)
                {
                    //Establecemos kms vacios
                    kms_vacios_recorridos = this._kms;
                }
                else
                {
                    //el tipo de movimiento es tronco
                    kms_tronco_recorridos = this._kms;
                }
            }
        }

        /// <summary>
        /// Buscamos movimiento en estatus iniciado ligando una parada Destino
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static int BuscamosMovimientoParadaDestino(int id_servicio, int id_parada_destino)
        {
            //Declaramos Variable movimiento
            int id_movimiento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 12, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, id_parada_destino, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    id_movimiento = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToInt32(r["IdMovimiento"])).FirstOrDefault();

                }

            }
            //Devolvemos Valor
            return id_movimiento;
        }

        /// <summary>
        /// Buscamos ultimo movimiento del servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static int BuscamosUltimoMovimiento(int id_servicio, int id_parada_destino)
        {
            //Declaramos Variable movimiento
            int id_movimiento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 16, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, id_parada_destino, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    id_movimiento = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToInt32(r["IdMovimiento"])).FirstOrDefault();

                }

            }
            //Devolvemos Valor
            return id_movimiento;
        }

        /// <summary>
        /// Carga Movimientos en vacio ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <returns></returns>
        public static DataTable CargaMovimientosEnVacio(int id_servicio)
        {
            //Declaramos Variable movimiento
            DataTable mit = null;

            //Inicializando arreglo de parámetros
            object[] param = { 15, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    mit = ds.Tables["Table"];

                }
            }
            //Devolvemos Valor
            return mit;
        }

        /// <summary>
        /// Carga estatus de los movimientos ligando un id de servicio
        /// </summary>
        /// <param name="id_servicio">Id servicio</param>
        /// <param name="registrado">Movimientios registrados</param>
        /// <param name="iniciado">Moimientos Iniciados</param>
        /// <param name="terminado">Movimientos Terminados</param>
        public static void CargaEstatusMovimiento(int id_servicio, out bool registrado, out bool iniciado,
            out bool terminado)
        {
            //Parametros de salida
            iniciado = terminado = false;
            registrado = true;

            //Inicializando arreglo de parámetros
            object[] param = { 14, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignando valores de estatus en parámetro de salida
                    registrado = (from DataRow r in ds.Tables["Table"].Rows
                                  where Convert.ToByte(r["IdEstatus"]) == 1
                                  select Convert.ToBoolean(r["Movimientos"])).FirstOrDefault();
                    iniciado = (from DataRow r in ds.Tables["Table"].Rows
                                where Convert.ToByte(r["IdEstatus"]) == 2
                                select Convert.ToBoolean(r["Movimientos"])).FirstOrDefault();
                    terminado = (from DataRow r in ds.Tables["Table"].Rows
                                 where Convert.ToByte(r["IdEstatus"]) == 3
                                 select Convert.ToBoolean(r["Movimientos"])).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Buscamos Movimiento en estatus registrado ligando una parada origen
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_origen">Id Parada Origen</param>
        /// <returns></returns>
        public static int BuscamosMovimientoParadaOrigen(int id_servicio, int id_parada_origen)
        {
            //Declaramos variable movimiento
            int id_movimiento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 13, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, id_parada_origen, 0, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    id_movimiento = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToInt32(r["IdMovimiento"])).FirstOrDefault();

                }

            }
            //Devolvemos Movimiento
            return id_movimiento;
        }

        /// <summary>
        /// Obtenemos el Tipo de Movimiento  a partir de los eventos de la parada origen.
        /// </summary>
        /// <param name="tipo_movimiento">Tipo de Movimiento para asignación de Paradas de Servicio</param>
        /// <returns></returns>
        public Tipo ObtieneTipoMovimientoPorAsignar(Tipo tipo_movimiento)
        {
            //Declaramos Variable Tipo de Eventos
            bool Carga, Descarga, DescargaParcial;

            DataTable mitEventos = null;

            //Declaramos tipo de movimiento
            Tipo tipo = Tipo.Vacio;

            //Cargamos los Tipos de Eventos 
            ParadaEvento.CargaTipoEventos(this._id_parada_origen, out Carga, out Descarga, out DescargaParcial);
            //Determinamos los tipos de eventos para establecer el tipo de movimiento
            //Si existe por lo menos una descarga Parcial
            if (DescargaParcial)
                //Establecemos Tipo de MOvimiento
                tipo = Tipo.Cargado;
            else
            {
                //Si existe solo una carga
                if (Carga)
                    //Establecemos Tipo de Movimiento
                    tipo = Tipo.Cargado;
                //Si existe sólo descarga
                else if (Descarga)
                    //Establecemos Movimiento a Vacio
                    tipo = Tipo.Vacio;
                //En caso de no existir eventos
                else if (!Validacion.ValidaOrigenDatos(mitEventos))
                    tipo = tipo_movimiento;
            }

            //Devolvemos Valor
            return tipo;
        }
        /// <summary>
        /// Obtenemos el Tipo de Movimiento  a partir de los eventos de la parada origen o bien a partir del tipo de movimiento previo en caso de no tener eventos Actualmente.
        /// </summary>
        /// <returns></returns>
        public Tipo ObtieneTipoMovimientoPorAsignar()
        {
            //Declaramos Variable Tipo de Eventos
            bool Carga, Descarga, DescargaParcial;

            DataTable mitEventos = null;

            //Declaramos tipo de movimiento
            Tipo tipo = Tipo.Vacio;

            //Cargamos los Tipos de Eventos 
            ParadaEvento.CargaTipoEventos(this._id_parada_origen, out Carga, out Descarga, out DescargaParcial);
            //Determinamos los tipos de eventos para establecer el tipo de movimiento
            //Si existe por lo menos una descarga Parcial
            if (DescargaParcial)
                //Establecemos Tipo de MOvimiento
                tipo = Tipo.Cargado;
            else
            {
                //Si existe solo una carga
                if (Carga)
                    //Establecemos Tipo de Movimiento
                    tipo = Tipo.Cargado;
                //Si existe sólo descarga
                else if (Descarga)
                    //Establecemos Movimiento a Vacio
                    tipo = Tipo.Vacio;
                //En caso de no existir eventos
                else if (!Validacion.ValidaOrigenDatos(mitEventos))
                {
                    //Si no es el primer movimiento del servicio
                    if (this._secuencia_servicio > 1)
                        tipo = Movimiento.BuscamosTipoMovimientoParadaDestino(this._id_servicio, this._id_parada_origen);
                    //Si es el primero
                    else
                        tipo = TipoMovimiento;
                }
            }

            //Devolvemos Valor
            return tipo;
        }

        /// <summary>
        /// Método Público encargado de Obtener los Estados de Origen y Destino del Movimiento
        /// </summary>
        /// <param name="id_movimiento">Movimiento</param>
        /// <returns></returns>
        public static DataTable ObtieneEstadosMovimiento(int id_movimiento)
        {   //Declarando Objeto de Retorno
            DataTable dtEstados = null;
            //Inicializando arreglo de parámetros
            object[] param = { 17, id_movimiento, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };
            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtEstados = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtEstados;
        }

        /// <summary>
        /// Obtenemos kilometraje Total de los Movimientos terminados ligando un Id de Servicio
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="total_kms_recorridos">Obtiene el Total de kilometros recorridos</param>
        /// <param name="total_kms_cargados_recorridos">Obtiene el Total de kilometros cargados Recorridos</param>
        /// <param name="total_kms_vacios_recorridos">Obtiene el Total de kilometros vacios recorridos</param>
        /// <param name="total_kms_troncos_recorridos">Obtiene el total de kilometros tronco recorridos</param>
        /// <returns></returns>
        public static void ObtieneKilometrajeTotalRecorrido(int id_servicio, out decimal total_kms_recorridos, out decimal total_kms_cargados_recorridos,
                                                          out decimal total_kms_vacios_recorridos, out decimal total_kms_troncos_recorridos)
        {
            //Declaramos Variables
            total_kms_recorridos = total_kms_cargados_recorridos = total_kms_vacios_recorridos = total_kms_troncos_recorridos = 0;

            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando arreglo de parámetros
            object[] param = { 18, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {

                    //Obtenemos Totales Kms Recorridos
                    total_kms_recorridos = (from DataRow r in ds.Tables[0].Rows
                                            select Convert.ToDecimal(r["TotalKmsRecorridos"])).FirstOrDefault();

                    //Obtenemos Totales Kms Cargados Recorridos
                    total_kms_cargados_recorridos = (from DataRow r in ds.Tables[0].Rows
                                                     select Convert.ToDecimal(r["TotalKmsCargadosRecorridos"])).FirstOrDefault();

                    //Obtenemos Totales Vacios Recorridos
                    total_kms_vacios_recorridos = (from DataRow r in ds.Tables[0].Rows
                                                   select Convert.ToDecimal(r["TotalKmsVaciosRecorridos"])).FirstOrDefault();
                    //Obtenemos Totales Troncos Recorridos
                    total_kms_troncos_recorridos = (from DataRow r in ds.Tables[0].Rows
                                                    select Convert.ToDecimal(r["TotalKmsTroncoRecorridos"])).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Buscamos el tipo de movimiento en estatus iniciado ligando una parada Destino
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada_destino">Id parada Destino</param>
        /// <returns></returns>
        public static Tipo BuscamosTipoMovimientoParadaDestino(int id_servicio, int id_parada_destino)
        {
            //Declaramos Variable movimiento
            Byte tipo_movimiento = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 22, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, id_parada_destino, 0, false, null, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos el movimiento
                    tipo_movimiento = (from DataRow r in ds.Tables[0].Rows
                                       select Convert.ToByte(r["TipoMovimiento"])).FirstOrDefault();
                }
            }
            //Devolvemos Valor
            return (Tipo)tipo_movimiento;
        }

        /// <summary>
        /// Validamos que no existan anticipos ligado al movimiento
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="objMovimientoInicio">Movimiento Inicial (parada como origen)</param>
        /// <param name="objMovimientoFinal">Movimiento Final (parada como destino)</param>
        /// <param name="objParadaAnterior">Parada anterior</param>
        /// <param name="objParadaPosterior">Parada Posterior</param>
        /// <param name="id_usuario">Id Uusraio</param>
        /// <returns></returns>
        public static RetornoOperacion ValidacionAnticiposPorMovimiento(int id_servicio, Movimiento objMovimientoInicio, Movimiento objMovimientoFinal, Parada objParadaAnterior, Parada objParadaPosterior, int id_usuario)
        {
            //Establecemos Variable Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //En caso de ser parada Al Inicio
            if (objParadaAnterior.id_parada == 0)
            {
                //Validamos existencia de Anticipos
                resultado = DetalleLiquidacion.ValidaAnticiposMovimiento(objMovimientoInicio.id_movimiento);

            }
            else
            {
                //En caso de ser parada al Final
                if (objParadaPosterior.id_parada == 0)
                {
                    //Actualizamos Movimiento de los Anticipos
                    resultado = DetalleLiquidacion.ActualizaAnticiposPorMovimiento(id_servicio, objMovimientoFinal.id_movimiento, id_usuario);

                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        #endregion

        #region Métodos Revisión General

        /// <summary>
        /// Inserta un movimiento en vacío o en tronco desde la parada comodín de la ubicación de origen hacia la parada comodín de la ubicación destino con las caracteristicas especificadas
        /// </summary>
        /// <param name="id_ubicacion_origen">Id de Ubicación de origen del movimiento</param>
        /// <param name="id_ubicacion_destino">Id de Unicación de destino del movimiento</param>
        /// <param name="id_compania_emisor">Id de Compañía que genera el movimiento</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_motriz">Id de Unidad Motriz</param>
        /// <param name="id_tercero">Id de Tercero que realiza el movimiento (proveedor)</param>
        /// <param name="arrastre">Conjunto de unidades de arrastre</param>
        /// <param name="fecha_salida_origen">Fecha de salida de la ubicación origen</param>
        /// <param name="tipo_actualizacion_fin_estancia">Tipo de actualización que tendrán las estancias que terminan por la salida de las unidades desde la ubicación de origen</param>
        /// <param name="id_usuario">Id de Usuario que realiza el movimiento<param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoVacio(int id_ubicacion_origen, int id_ubicacion_destino, int id_compania_emisor, int id_operador, int id_motriz, int id_tercero,
                                                                List<int> arrastre, DateTime fecha_salida_origen, EstanciaUnidad.TipoActualizacionFin tipo_actualizacion_fin_estancia,
                                                                string remolque_3ero_1, string remolque_3ero_2, int id_usuario)
        {
            //Declarando objeto de resutado
            RetornoOperacion resultado = new RetornoOperacion();

            //La ubicación origen y destino deben ser distintas entre si
            if (id_ubicacion_origen != id_ubicacion_destino)
            {
                //Inicializando transaccción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        //Creando parada de destino en la ubicación solicitada
                        resultado = Parada.InsertaParadaAlternativaEstancia(0, Parada.EstatusParada.Registrado, 0, Parada.TipoParada.Servicio, id_ubicacion_destino, DateTime.MinValue, SqlGeography.Null,
                                                        DateTime.MinValue, Parada.TipoActualizacionLlegada.SinActualizar, 0, DateTime.MinValue, Parada.TipoActualizacionSalida.SinActualizar, 0, id_usuario);
                    }
                    catch (Exception ex)
                    {
                        //Instanciando Error
                        resultado = new RetornoOperacion(string.Format("Source:{0} - Message:{1} - Inner:{2}", ex.Source, ex.Message, ex.InnerException));
                    }

                    //Si se insertó la parada de destino
                    if (resultado.OperacionExitosa)
                    {
                        //Obteniendo Id de nueva parada destino
                        int id_parada_destino = resultado.IdRegistro;

                        //Devolviendo resultado
                        resultado = InsertaMovimientoVacio(id_ubicacion_origen, 0, id_parada_destino, id_compania_emisor, id_operador, id_motriz, id_tercero, arrastre, fecha_salida_origen, tipo_actualizacion_fin_estancia, remolque_3ero_1, remolque_3ero_2, id_usuario);
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        scope.Complete();
                }
            }
            else
                resultado = new RetornoOperacion("Un reposicionamiento en vacío no puede tener como destino el mismo lugar de origen.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Obtiene el conjunto de movimientos de un servicio sin pago registrado, con asignación del recurso solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="tipo_asignacion">Tipo de asignación de recurso de movimiento</param>
        /// <param name="id_recurso">Id de Recurso asignado</param>
        /// <returns></returns>
        public static DataTable ObtieneMovimientosSinPagoServcioRecurso(int id_servicio, MovimientoAsignacionRecurso.Tipo tipo_asignacion, int id_recurso)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Creando arreglo de parámetros de consulta
            object[] param = { 24, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, (byte)tipo_asignacion, id_recurso };

            //Realizando consulta de movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Obtiene el conjunto de movimientos de un servicio sin pago registrado, con asignación del recurso solicitado
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="tipo_asignacion">Tipo de asignación de recurso de movimiento</param>
        /// <param name="id_recurso">Id de Recurso asignado</param>
        /// <returns></returns>
        public static DataTable ObtieneMovimientosServcioRecurso(int id_servicio, MovimientoAsignacionRecurso.Tipo tipo_asignacion, int id_recurso)
        {
            //Declarando objeto de resultado
            DataTable mit = null;

            //Creando arreglo de parámetros de consulta
            object[] param = { 25, 0, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null, (byte)tipo_asignacion, id_recurso };

            //Realizando consulta de movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Convertir un Movimiento en Vacio a Servicio
        /// </summary>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion HacerMovimientoVacioaServicio(int id_cliente, int id_ruta, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int id_servicio = 0;

            //Validamos que sea un Movimiento en Vacio
            if (this._id_servicio == 0)
            {
                //Validamos que el movimiento se encuntre Terminado
                if ((Estatus)this._id_estatus_movimiento == Estatus.Terminado)
                {
                    //Instanciamos Parada Origen y Destino
                    using (Parada objParadaOrigen = new Parada(this._id_parada_origen), objParadaDestino = new Parada(this._id_parada_destino))
                    {
                        //Inicializando transacción
                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Insertando nuevo servicio
                            resultado = SAT_CL.Documentacion.Servicio.InsertarServicio("", SAT_CL.Documentacion.Servicio.Estatus.Terminado, 1, 0, false, 0, this._id_compania_emisor, id_cliente, objParadaOrigen.id_ubicacion, objParadaOrigen.fecha_llegada,
                                                                                            objParadaDestino.id_ubicacion, objParadaDestino.fecha_llegada, "",
                                                                                            "", Fecha.ObtieneFechaEstandarMexicoCentro(), "", id_usuario);

                            //Si se insertó el Servicio
                            if (resultado.OperacionExitosa)
                            {
                                //Recuperando Id de servicio
                                id_servicio = resultado.IdRegistro;

                                //Editamos información de paradas, segmentos y movimientos ligando el sercvicio correspondiente
                                resultado = Parada.ActualizaParadasMovimientoVacioaServicio(id_servicio, id_cliente, objParadaOrigen, objParadaDestino, this._id_movimiento, id_ruta, id_usuario);

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Insertando Clasificacion predeterminada
                                    resultado = SAT_CL.Global.Clasificacion.InsertaClasificacion(1, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_usuario);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Realizando Insercción de Factura
                                        resultado = SAT_CL.Facturacion.Facturado.InsertaFactura(id_servicio, id_servicio);

                                        //Validamos Insercción de la Parada
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Insertamos Servicio Control Evidencia
                                            resultado = SAT_CL.ControlEvidencia.ServicioControlEvidencia.InsertaServicioControlEvidencia(id_servicio, objParadaDestino.fecha_llegada.AddMinutes(1), id_usuario);

                                            //Validamos Resultado
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Instacciamos Usuario
                                                using (SAT_CL.Seguridad.Usuario objUsuario = new Seguridad.Usuario(id_usuario))
                                                {
                                                    //Insertando Referencia
                                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(this._id_compania_emisor, 1, "Cambiado a Servicio Por", 0, "Referencia de Viaje"),
                                                               objUsuario.nombre, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Intsnaciamos Servicio 
                                using (Documentacion.Servicio objServicio = new Documentacion.Servicio(id_servicio))
                                {
                                    //Establecemos Mensaje 
                                    resultado = new RetornoOperacion(string.Format("El Servicio ha sido creado {0}.", objServicio.no_servicio.ToString()), true);
                                }
                                //Completeamos transacción 
                                scope.Complete();
                            }
                        }
                    }

                }
                //Mostramos mensaje Error
                else
                    resultado = new RetornoOperacion("El estatus del movimiento no permite su edición.");
            }
            else
                //Mostramos Mensaje Error
                resultado = new RetornoOperacion("El tipo de movimiento no es válido.");

            //Devolvemos Resultado
            return resultado;
        }


        /// <summary>
        /// Inserta un movimiento en vacío o en tronco desde la parada comodín de la ubicación de origen hacia la parada indicada con las caracteristicas especificadas
        /// </summary>
        /// <param name="id_ubicacion_origen">Id de Ubicación de origen del movimiento</param>
        /// <param name="id_servicio">Id de Servicio al que se asocia el movimiento</param>
        /// <param name="id_parada_destino">Id de parada de destino del movimiento</param>
        /// <param name="id_compania_emisor">Id de Compañía que genera el movimiento</param>
        /// <param name="id_operador">Id de Operador</param>
        /// <param name="id_motriz">Id de Unidad Motriz</param>
        /// <param name="id_tercero">Id de Tercero que realiza el movimiento (proveedor)</param>
        /// <param name="arrastre">Conjunto de unidades de arrastre</param>
        /// <param name="fecha_salida_origen">Fecha de salida de la ubicación origen</param>
        /// <param name="tipo_actualizacion_fin_estancia">Tipo de actualización que tendrán las estancias que terminan por la salida de las unidades desde la ubicación de origen</param>
        /// <param name="remolque_3ero_1">Remolque de Tercero 1</param>
        /// <param name="remolque_3ero_2">Remolque de Tercero 2</param>
        /// <param name="id_usuario">Id de Usuario que realiza el movimiento<param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoVacio(int id_ubicacion_origen, int id_servicio, int id_parada_destino, int id_compania_emisor, int id_operador, int id_motriz, int id_tercero,
                                                              List<int> arrastre, DateTime fecha_salida_origen, EstanciaUnidad.TipoActualizacionFin tipo_actualizacion_fin_estancia,
                                                              string remolque_3ero_1, string remolque_3ero_2, int id_usuario)
        {
            //Declarando objeto de resultado (validando recursos iniciales de movimiento)
            RetornoOperacion resultado = validaRecursosInicioMovimiento(id_tercero, id_motriz, id_operador);

            //Declarando variable de id de movimiento 
            int id_movimiento = -2;

            //Si no hay errores con la configuración del movimiento
            if (resultado.OperacionExitosa)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        //Creando parada de origen en la ubicación solicitada
                        resultado = Parada.InsertaParadaAlternativaEstancia(id_servicio, Parada.EstatusParada.Iniciado, 0, Parada.TipoParada.Servicio, id_ubicacion_origen, DateTime.MinValue, SqlGeography.Null,
                                                fecha_salida_origen.AddMinutes(-1), Parada.TipoActualizacionLlegada.Manual, 0, DateTime.MinValue, Parada.TipoActualizacionSalida.SinActualizar, 0, id_usuario);
                    }
                    catch (Exception ex)
                    {
                        //Instanciando Error
                        resultado = new RetornoOperacion(string.Format("Source:{0} - Message:{1} - Inner:{2}", ex.Source, ex.Message, ex.InnerException));
                    }

                    //Si no hay error
                    if (resultado.OperacionExitosa)
                    {
                        //Recuperando id de parada de origen creada
                        int id_parada_origen = resultado.IdRegistro;

                        //Creando arreglo de unidades por actualizar (tractor y unidades de arrastre)
                        List<int> unidades = new List<int>();
                        //Agregando unidad motriz, sólo si es mayor a 0
                        if (id_motriz > 0)
                            unidades.Add(id_motriz);
                        //Agregando Arrastre
                        unidades.AddRange((from int p in arrastre
                                           select p));

                        //Asegurando que todos los recursos a mover se encuentran en la misma ubicación (ubicación de origen) y moviendolos a la parada default de dicha ubicación
                        resultado = EstanciaUnidad.CambiaParadaEstanciaUnidadesUbicacion(unidades, id_parada_origen, id_ubicacion_origen, id_usuario);

                        //Si no hay errores en la actualización de parada
                        if (resultado.OperacionExitosa)
                        {
                            //Terminando estancias de unidades implicadas
                            resultado = EstanciaUnidad.TerminaEstanciaUnidades(unidades, fecha_salida_origen, tipo_actualizacion_fin_estancia, id_usuario);

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando parada de origen
                                using (Parada paradaOrigen = new Parada(id_parada_origen))
                                {
                                    //Si la parada se localizó
                                    if (paradaOrigen.id_parada > 0)
                                    {
                                        //Terminando la parada recien insertada como origen
                                        resultado = paradaOrigen.TerminaParada(fecha_salida_origen, Parada.TipoActualizacionSalida.Manual, 0, id_usuario);

                                        //Si no hay errores en la finalización de la parada
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Insertando nuevo movimiento desde la parada origen hacia la parada destino (es tronco si no existe ninguna unidad de arrastre, de lo contrario, es movimiento en vacío)
                                            resultado = InsertaMovimiento(id_servicio, 0, 0, Estatus.Iniciado, arrastre.Count > 0 ? Tipo.Vacio : Tipo.EnTronco,
                                                                            0, 0, id_compania_emisor, id_parada_origen, id_parada_destino, id_usuario);

                                            //Si se registró correctamente el movimiento vacío
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Recuperando id de movimiento insertado
                                                id_movimiento = resultado.IdRegistro;

                                                //Iniciando movimiento
                                                using (Movimiento movimientoVacio = new Movimiento(id_movimiento))
                                                    //Actualizando su estatus a iniciado
                                                    resultado = movimientoVacio.IniciaMovimiento(id_usuario);

                                                //Si se actualizó a iniciado correctamente
                                                if (resultado.OperacionExitosa)
                                                {
                                                    //Calculando su kilometraje
                                                    resultado = CalculaKilometrajeMovimiento(id_movimiento, id_usuario);

                                                    //Si no hay errores en calculo de kilometraje
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Armando conjunto de recursos por asignar al movimiento
                                                        List<KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>> recursos = new List<KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>>();
                                                        //Agregando Tercero, operador y unidad motriz
                                                        if (id_tercero > 0)
                                                            recursos.Add(new KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>(id_tercero, MovimientoAsignacionRecurso.Tipo.Tercero));
                                                        if (id_operador > 0)
                                                            recursos.Add(new KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>(id_operador, MovimientoAsignacionRecurso.Tipo.Operador));
                                                        if (id_motriz > 0)
                                                            recursos.Add(new KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>(id_motriz, MovimientoAsignacionRecurso.Tipo.Unidad));
                                                        //Agregando unidades de arrastre
                                                        recursos.AddRange((from int arr in arrastre
                                                                           select new KeyValuePair<int, MovimientoAsignacionRecurso.Tipo>(arr, MovimientoAsignacionRecurso.Tipo.Unidad)));

                                                        //Asignando recursos
                                                        resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursos(recursos, id_movimiento, MovimientoAsignacionRecurso.Estatus.Iniciado, id_usuario);
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("Error en cálculo de kilometraje del movimiento '{0}': {1}", id_movimiento, resultado.Mensaje));

                                                    //Si la asignación fue exitosa
                                                    if (resultado.OperacionExitosa)
                                                        //Actualizando estatus de recursos asignados
                                                        resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosAsignadosATransito(id_movimiento, fecha_salida_origen, id_usuario);
                                                    //Si no se pudieron asignar los recursos
                                                    else
                                                        resultado = new RetornoOperacion(string.Format("Error al asignar recursos al movimiento: {0}", resultado.Mensaje));
                                                }
                                                //Si no se pudo iniciar correctamente
                                                else
                                                    resultado = new RetornoOperacion(string.Format("Error al registrar Mov. '{0}': {1}", arrastre.Count > 0 ? Tipo.Vacio : Tipo.EnTronco, resultado.Mensaje));
                                            }
                                            //Si no pudo ser registrado el movimiento
                                            else
                                                resultado = new RetornoOperacion(string.Format("Error al registrar Mov. '{0}': {1}", arrastre.Count > 0 ? Tipo.Vacio : Tipo.EnTronco, resultado.Mensaje));
                                        }
                                        //Si no se pudo terminar la parada
                                        else
                                            resultado = new RetornoOperacion("No fue posible terminar la parada de origen.");
                                    }
                                    //Parada de origen no localizada
                                    else
                                        resultado = new RetornoOperacion("Parada de origen no encontrada.");
                                }
                            }
                        }
                    }
                    //Si no se insertó la parada de origen
                    else
                        resultado = new RetornoOperacion("Error al crear parada en ubicación de origen.");

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        //Creando Lista de Remolques
                        int cont = 1;
                        List<string> rem_3eros = new List<string>();
                        rem_3eros.Add(remolque_3ero_1);
                        rem_3eros.Add(remolque_3ero_2);

                        //Creando Ciclo
                        foreach (string rem_3ro in rem_3eros)
                        {
                            //Validando Contenido
                            if (!rem_3ro.Equals(""))
                            {
                                //Insertando Remolque
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(id_movimiento, 10, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 10,
                                                                                       string.Format("Remolque 3ero {0}", cont), 0, "Información de Carga"), rem_3ro,
                                                                                       Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);

                                //Validando Resultado
                                if (!resultado.OperacionExitosa)
                                    //Terminando Ciclo
                                    break;

                                //Incrementando Contador
                                cont++;
                            }
                        }

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Realizando envío de información a plataformas de terceros
                            using (Movimiento mov = new Movimiento(id_movimiento))
                            {
                                //Recopilando información y haciendo envío
                                mov.EnviaInformacionMovimientoVacioFTP(System.Text.Encoding.UTF8);
                            }

                            //Indicando resultado correcto con id de movimiento insertado
                            resultado = new RetornoOperacion(id_movimiento);
                            //Finalizando transacción
                            scope.Complete();
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Finaliza el movimiento, realizando las acciones correspondientes a un movimiento vacío
        /// </summary>
        /// <param name="fecha_llegada_destino">Fecha de llegada a su destino</param>
        /// <param name="tipo_actualizacion_inicio_estancia">Tipo de actualización que tendrán las estancias que inician por la llegada de las unidades a la parada de destino</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion TerminaMovimientoVacio(DateTime fecha_llegada_destino, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_inicio_estancia, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Iniciado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Terminando Asignaciones de Recurso del movimiento
                    resultado = MovimientoAsignacionRecurso.TerminaMovimientosAsignacionRecurso(this._id_movimiento, id_usuario);

                    //Si se deshabilitaron las asignaciones de recurso
                    if (resultado.OperacionExitosa)
                    {
                        //Terminando movimiento
                        resultado = terminaMovimiento(id_usuario);

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {

                            //Cargando asignaciones de recurso del movimiento recien terminadas
                            using (DataTable rec = MovimientoAsignacionRecurso.CargaAsignacionesMovimiento(this._id_movimiento, MovimientoAsignacionRecurso.Estatus.Terminado))
                            {
                                //Si hay recursos
                                if (Validacion.ValidaOrigenDatos(rec))
                                {
                                    //Recuperando todas las asignaciones de unidad y operador
                                    List<int> recursos = (from DataRow r in rec.Rows
                                                          where (MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion") != MovimientoAsignacionRecurso.Tipo.Tercero
                                                          select r.Field<int>("IdRecurso")).DefaultIfEmpty().ToList();
                                    List<int> unidades = (from DataRow r in rec.Rows
                                                          where (MovimientoAsignacionRecurso.Tipo)r.Field<byte>("IdTipoAsignacion") == MovimientoAsignacionRecurso.Tipo.Unidad
                                                          select r.Field<int>("IdRecurso")).DefaultIfEmpty().ToList();

                                    //Determinando si el movimiento está asignado a un servicio
                                    if (this._id_servicio > 0)
                                    {
                                        //Actualizando los recursos que serán asignados a la parada del servicio y liberando aquellos no requeridos
                                        resultado = EstanciaUnidad.CreaEstanciaRecursoMovimientoInicialServicio(recursos, this._id_servicio, fecha_llegada_destino, tipo_actualizacion_inicio_estancia, id_usuario);
                                        //Si se crearón correctamente las estancias de unidad
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciando la parada destino del movimiento
                                            using (Parada paradaDestino = new Parada(this._id_parada_destino))
                                                //Actualziando estatus de parada a iniciada
                                                resultado = paradaDestino.IniciaParada(fecha_llegada_destino, Parada.TipoActualizacionLlegada.Manual, 0, id_usuario);

                                            //Si no hay errores iniciando la parada
                                            if (resultado.OperacionExitosa)
                                            {
                                                decimal km_recorridos = 0, km_cargados = 0, km_vacios = 0, km_tronco = 0;
                                                //Recuperando información de kilometraje
                                                obtenemosKilometrajeMovimiento(out km_recorridos, out km_cargados, out km_vacios, out km_tronco);
                                                //Actualizando el kilometraje de las unidades que intervienen
                                                resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, km_recorridos, id_usuario);
                                                //Si no hay errores
                                                if (resultado.OperacionExitosa)
                                                    //Instanciando información de despacho del servicio del movimiento
                                                    using (ServicioDespacho despacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                                    {
                                                        //Si existe la información
                                                        if (despacho.id_servicio_despacho > 0)
                                                            //Actualizando kilometraje
                                                            resultado = despacho.ActualizaKimometrajeRecorridosServicioDespacho(km_recorridos, km_cargados, km_vacios, km_tronco, id_usuario);
                                                    }
                                                else
                                                    resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje recorrido por unidades asignadas: {0}", resultado.Mensaje));

                                            }
                                            //Si no se pudo iniciar la parada
                                            else
                                                resultado = new RetornoOperacion("Error al iniciar la parada inicial del servicio.");
                                        }
                                    }
                                    //Si no es un movimiento de servicio
                                    else
                                    {
                                        decimal km_recorridos = 0, km_cargados = 0, km_vacios = 0, km_tronco = 0;
                                        //Recuperando información de kilometraje
                                        obtenemosKilometrajeMovimiento(out km_recorridos, out km_cargados, out km_vacios, out km_tronco);
                                        //Actualizando el kilometraje de las unidades que intervienen
                                        resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, km_recorridos, id_usuario);
                                        //Si no hay errores
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Instanciando parada destino del movimiento
                                            using (Parada paradaDestino = new Parada(this._id_parada_destino))
                                            {
                                                //Si la parada destino fue localizada
                                                if (paradaDestino.id_parada > 0)
                                                {
                                                    //iniciamos Parada
                                                    resultado = paradaDestino.IniciaParada(fecha_llegada_destino, Parada.TipoActualizacionLlegada.Manual, 0, id_usuario);

                                                    //Validamos Resultado
                                                    if (resultado.OperacionExitosa && paradaDestino.ActualizaParada())
                                                    {
                                                        //Terminando la parada destino
                                                        resultado = paradaDestino.TerminaParada(id_usuario);

                                                        //Si se terminó la parada correctamente
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Obteniendo parada comodín de la misma ubicación
                                                            int id_parada_comodin = Parada.ObtieneParadaComodinUbicacion(paradaDestino.id_ubicacion, true, id_usuario);

                                                            //Si la parada comodín fue recuperada
                                                            if (id_parada_comodin > 0)
                                                            {
                                                                //Moviendo a todos los recursos unidad a la parada destino (comodín)
                                                                resultado = EstanciaUnidad.CreaEstanciaUnidades(unidades, id_parada_comodin, fecha_llegada_destino, tipo_actualizacion_inicio_estancia, id_usuario);

                                                                //Si no hay errores
                                                                if (resultado.OperacionExitosa && paradaDestino.ActualizaParada())
                                                                    //Actualizando estatus de unidades a disponible
                                                                    resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosTerminadosADisponible(this._id_movimiento, id_parada_comodin, fecha_llegada_destino, MovimientoAsignacionRecurso.Estatus.Terminado, id_usuario);
                                                                //Si no se crearon las estancias en la parada destino
                                                                else
                                                                    resultado = new RetornoOperacion(string.Format("Error al crear estancias de unidades en la parada '{0}'.", this._id_parada_destino));
                                                            }
                                                            //Si no hay parada comodín
                                                            else
                                                                resultado = new RetornoOperacion("No se pudo localizar la parada predeterminada de esta ubicación.");
                                                        }
                                                        else
                                                        {
                                                            //Si no se logró terminar la parada detino del movimiento
                                                            resultado = new RetornoOperacion("Error al terminar parada de destino del movimiento.");
                                                        }
                                                    }//Validamos Inicio de la Parada
                                                }
                                                //Si no se cargó la parada destino
                                                else
                                                    resultado = new RetornoOperacion("Error al recuperar la parada de destino del movimiento.");
                                            }
                                        }
                                        //Si no se actualizó el kilometraje de unidades
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje recorrido por unidades asignadas: {0}", resultado.Mensaje));
                                    }
                                }
                            }
                        }
                    }
                    //Si no se terminan las asignaciones
                    else
                        resultado = new RetornoOperacion(string.Format("Error al terminar asignación de recursos: {0}", resultado.Mensaje));

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(this._id_movimiento);
                        //Terminando transacción
                        scope.Complete();
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Iniciar un Movimiento
        /// </summary>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion IniciaMovimiento(int id_usuario)
        {
            //Obtenemos el Tipo de Movimiento para su actualización
            Tipo _tipo_movimiento = ObtieneTipoMovimientoPorAsignar();

            //Actualizamos estatus del Movimiento
            return editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, Estatus.Iniciado, _tipo_movimiento,
                                  this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario,
                                  this._habilitar);
        }
        /// <summary>
        /// Termina las estancias en la parada de origen, pone los recursos en tránsito, inicia el segmento (en caso de tenerlo) e inicia el movimiento
        /// </summary>
        /// <param name="fecha_salida_origen">Fecha de término de las estancias</param>
        /// <param name="tipo_actualizacion_fin_estancia">Tipo de actualización de fin de estancia</param>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion IniciaMovimiento(DateTime fecha_salida_origen, EstanciaUnidad.TipoActualizacionFin tipo_actualizacion_fin_estancia, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando su estatus
            if (this.EstatusMovimiento == Estatus.Registrado)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Terminando estancias de la parada origen
                    resultado = EstanciaUnidad.TerminaEstanciasIniciadasParada(this._id_parada_origen, fecha_salida_origen, tipo_actualizacion_fin_estancia, id_usuario);

                    //Si se terminaron correctamente las estancias de las unidades involucradas
                    if (resultado.OperacionExitosa)
                    {
                        //Actualizando el estatus de los recuersos asignados al movimiento que inicia, de ocupado a transito
                        resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosAsignadosATransito(this._id_movimiento, fecha_salida_origen, id_usuario);

                        //Si se han actualizado los recursos
                        if (resultado.OperacionExitosa)
                        {
                            //Obtenemos el Tipo de Movimiento para su actualización
                            Tipo tipoMovimiento = ObtieneTipoMovimientoPorAsignar();

                            //Actualizamos estatus del Movimiento a iniciado y actualizamos el tipo del mismo
                            resultado = editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, Estatus.Iniciado, tipoMovimiento,
                                                 this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario,
                                                 this._habilitar);
                        }
                        //Si no se actualizó el estatus de los recursos a transito
                        else
                            resultado = new RetornoOperacion("Error al actualizar a 'Tránsito' los recursos del movimiento.");
                    }
                    //Si no se han terminado las estancias de la parada de origen
                    else
                        resultado = new RetornoOperacion(string.Format("Error al terminar estancias de la parada '{0}': {1}", this._id_parada_origen, resultado.Mensaje));

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            //Si el estatus no es registrado
            else
                resultado = new RetornoOperacion(string.Format("El movimiento se encuentra en estatus '{0}'", this.EstatusMovimiento));

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza las acciones inversas al inicio de movimiento
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ReversaIniciaMovimiento(int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando su estatus
            if (this.EstatusMovimiento == Estatus.Iniciado)
            {
                //Inicializando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Iniciando estancias de la parada origen
                    resultado = EstanciaUnidad.IniciaEstanciasTerminadasParada(this._id_parada_origen, id_usuario);

                    //Si se iniciaron correctamente las estancias de las unidades involucradas
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando la parada de origen
                        using (Parada paradaOrigen = new Parada(this._id_parada_origen))
                        {
                            //Si la parada se localizó
                            if (paradaOrigen.habilitar)
                                //Actualizando el estatus de los recuersos asignados al movimiento que se reversa, transito a ocupado
                                resultado = MovimientoAsignacionRecurso.ActualizaEstatusRecursosTransitoAOcupado(this._id_movimiento, this._id_parada_origen, paradaOrigen.fecha_llegada, id_usuario);
                            else
                                resultado = new RetornoOperacion("No fue posible recuperar la información de la parada de origen del movimiento.");
                        }

                        //Si se han actualizado los recursos
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos estatus del Movimiento a registrado
                            resultado = editaMovimiento(this._id_servicio, this._id_segmento_carga, this._secuencia_servicio, Estatus.Registrado, this.TipoMovimiento,
                                                 this._kms, this._kms_maps, this._id_compania_emisor, this._id_parada_origen, this._id_parada_destino, id_usuario,
                                                 this._habilitar);
                        }
                        //Si no se actualizó el estatus de los recursos a transito
                        else
                            resultado = new RetornoOperacion("Error al actualizar a 'Ocupado' los recursos del movimiento.");
                    }
                    //Si no se han terminado las estancias de la parada de origen
                    else
                        resultado = new RetornoOperacion(string.Format("Error al iniciar estancias de la parada '{0}': {1}", this._id_parada_origen, resultado.Mensaje));

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            //Si el estatus no es registrado
            else
                resultado = new RetornoOperacion(string.Format("El movimiento se encuentra en estatus '{0}', el estatus para esta actualización debe ser 'Iniciado'.", this.EstatusMovimiento));

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Finaliza el movimiento, realizando las acciones correspondientes a un movimiento de servicio
        /// </summary>
        /// <param name="fecha_llegada_destino">Fecha de llegada a su destino</param>
        /// <param name="tipo_actualizacion_inicio_estancia">Tipo de actualización que tendrán las estancias que inician por la llegada de las unidades a la parada de destino</param>
        /// <param name="id_usuario">Id de usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion TerminaMovimiento(DateTime fecha_llegada_destino, EstanciaUnidad.TipoActualizacionInicio tipo_actualizacion_inicio_estancia, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Iniciado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Creando estancias en parada de detino del movimiento
                    resultado = EstanciaUnidad.CreaEstanciaUnidadesFinMovimientoServicio(this._id_movimiento, fecha_llegada_destino, tipo_actualizacion_inicio_estancia, id_usuario);

                    //Si se crearón correctamente las estancias de unidad
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando información de despacho del servicio del movimiento
                        using (ServicioDespacho despacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                        {
                            //Si existe la información
                            if (despacho.id_servicio_despacho > 0)
                            {
                                decimal km_recorridos = 0, km_cargados = 0, km_vacios = 0, km_tronco = 0;
                                //Recuperando información de kilometraje
                                obtenemosKilometrajeMovimiento(out km_recorridos, out km_cargados, out km_vacios, out km_tronco);

                                //Actualizando kilometrajes a unidades del movimiento
                                resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, km_recorridos, id_usuario);
                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                    //Actualizando kilometraje
                                    resultado = despacho.ActualizaKimometrajeRecorridosServicioDespacho(km_recorridos, km_cargados, km_vacios, km_tronco, id_usuario);
                                else
                                    resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje recorrido por unidades asignadas: {0}", resultado.Mensaje));
                            }
                        }
                    }

                    //Si no hubo errores
                    if (resultado.OperacionExitosa)
                        //Actualziando estatus de movimiento
                        resultado = terminaMovimiento(id_usuario);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            //Si el estatus no es registrado
            else
                resultado = new RetornoOperacion(string.Format("El movimiento se encuentra en estatus '{0}'", this.EstatusMovimiento));

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza las acciones necesarias para la reversa en la actualización de fin de movimiento: elimina estancias, inicia asignaciones del movimiento, elimina asignaciones copiadas al movimiento siguiente (en caso de existir), los recursos se colocan en estatus tránsito, se resta el kilometraje del movimiento en el despacho de su servicio y el movimiento se coloca como iniciado
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion ReversaTerminaMovimiento(int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando estatus actual del movimiento
            if (this.EstatusMovimiento == Estatus.Terminado)
            {
                //Iniciando transacción
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando que las unidades involucradas se encuentren:
                    //Asociadas aún al servicio y en estatus ocupado
                    //Disponibles en la ubicación de fin del movimiento y sin ningún movimiento asignado
                    resultado = MovimientoAsignacionRecurso.ValidaRecursosParaReversaTerminaMovimiento(this._id_movimiento);

                    //Si es válido realizar la reversa
                    if (resultado.OperacionExitosa)
                    {
                        decimal km_recorridos = 0, km_cargados = 0, km_vacios = 0, km_tronco = 0;
                        //Recuperando información de kilometraje
                        obtenemosKilometrajeMovimiento(out km_recorridos, out km_cargados, out km_vacios, out km_tronco);
                        //Actualizando kilometrajes a unidades del movimiento
                        resultado = MovimientoAsignacionRecurso.ActualizaKilometrajeUnidadesMovimiento(this._id_movimiento, -km_recorridos, id_usuario);
                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //instanciando parad de origen
                            using (Parada paradaOrigen = new Parada(this._id_parada_origen))
                            {
                                //Si la parada se encontró
                                if (paradaOrigen.habilitar)
                                    //Eliminando estancias creadas para dichos recursos
                                    resultado = EstanciaUnidad.DeshabilitaEstanciaUnidadesReversaFinMovimiento(this._id_movimiento, paradaOrigen.fecha_llegada, id_usuario);
                                else
                                    resultado = new RetornoOperacion("No fue localizada la información de la parada de origen.");
                            }

                            //Si se deshabilitaron correctamente las estancias de unidad
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando información de despacho del servicio del movimiento
                                using (ServicioDespacho despacho = new ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, this._id_servicio))
                                {
                                    //Si existe la información
                                    if (despacho.id_servicio_despacho > 0)
                                        //Restando kilometraje
                                        resultado = despacho.RestaKimometrajeRecorridosServicioDespacho(km_recorridos, km_cargados, km_vacios, km_tronco, id_usuario);
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion(string.Format("Error al actualizar kilometraje recorrido por unidades asignadas: {0}", resultado.Mensaje));

                        //Si no hubo errores
                        if (resultado.OperacionExitosa)
                            //Actualziando estatus de movimiento a iniciado
                            resultado = ActualizaEstatusMovimiento(Estatus.Iniciado, id_usuario);
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Terminando transacción
                        scope.Complete();
                }
            }
            //Si el estatus no es registrado
            else
                resultado = new RetornoOperacion(string.Format("El movimiento se encuentra en estatus '{0}'", this.EstatusMovimiento));

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}
