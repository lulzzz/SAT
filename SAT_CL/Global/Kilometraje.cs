using System;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using Microsoft.SqlServer.Types;

namespace SAT_CL.Global
{
    /// <summary>
    /// Implementa los método para la administración Kilometraje.
    /// </summary>
    public class Kilometraje : Disposable
    {
        #region Atributos

        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_kilometraje_tk";


        private int _id_kilometraje;
        /// <summary>
        /// Describe el Id de   Kilometraje
        /// </summary>
        public int id_kilometraje
        {
            get { return _id_kilometraje; }
        }
        private int _id_ubicacion_origen;
        /// <summary>
        /// Describe la Ubicación Origen
        /// </summary>
        public int id_ubicacion_origen
        {
            get { return _id_ubicacion_origen; }
        }
        private int _id_ubicacion_destino;
        /// <summary>
        /// Describe la Ubicación Destino
        /// </summary>
        public int id_ubicacion_destino
        {
            get { return _id_ubicacion_destino; }
        }
        private SqlGeography _geo_ubicacion_origen;
        /// <summary>
        /// Describe la GeoUbicación Origen
        /// </summary>
        public SqlGeography geo_ubicacion_origen
        {
            get { return _geo_ubicacion_origen; }
        }
        private SqlGeography _geo_ubicacion_destino;
        /// <summary>
        /// Describe la GeoUbicación Destino
        /// </summary>
        public SqlGeography geo_ubicacion_destino
        {
            get { return _geo_ubicacion_destino; }
        }
        private decimal _kms_reales;
        /// <summary>
        /// Describe los Kms Reales
        /// </summary>
        public decimal kms_reales
        {
            get { return _kms_reales; }
        }
        private decimal _kms_maps;
        /// <summary>
        /// Describe  los Kms Maps
        /// </summary>
        public decimal kms_maps
        {
            get { return _kms_maps; }
        }
        private decimal _tiempo_real;
        /// <summary>
        /// Describe el Tiempo Real
        /// </summary>
        public decimal tiempo_real
        {
            get { return _tiempo_real; }
        }
        private decimal _tiempo_maps;
        /// <summary>
        /// Describe el Tiempo Maps
        /// </summary>
        public decimal tiempo_maps
        {
            get { return _tiempo_maps; }
        }
        private int _id_ruta;
        /// <summary>
        /// Describe la Ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private decimal _kms_pago;
        /// <summary>
        /// Describe los Kms de Pago
        /// </summary>
        public decimal kms_pago
        {
            get { return _kms_pago; }
        }
        private decimal _kms_cobro;
        /// <summary>
        /// Describe  los Kms Cobro
        /// </summary>
        public decimal kms_cobro
        {
            get { return _kms_cobro; }
        }
        private int _id_compania_emisor;
        /// <summary>
        /// Describe la Conpañia Emisor
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Kilometraje()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public Kilometraje()
        {
            //Asignando Valores
            this._id_kilometraje = 0;
            this._id_ubicacion_origen = 0;
            this._id_ubicacion_destino = 0;
            this._kms_reales = 0;
            this._kms_maps = 0;
            this._tiempo_real = 0;
            this._tiempo_maps = 0;
            this._id_ruta = 0;
            this._kms_pago = 0;
            this._kms_cobro = 0;
            this._id_compania_emisor = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Genera una Instancia de Kilometraje
        /// </summary>
        /// <param name="id_kilometraje"></param>
        public Kilometraje(int id_kilometraje)
        {
            cargaAtributosInstancia(id_kilometraje);
        }

        /// <summary>
        /// Genera una Instancia de Kilometraje ligando  un Id Ubicación Origen, Id Ubicacaión destino, Id ruta, Id Compañia emisor.
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicacion Origen</param>
        /// <param name="id_ubicacion_destino">Id Ubicacion Destino</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        public Kilometraje(int id_ubicacion_origen, int id_ubicacion_destino, int id_ruta, int id_compania_emisor)
        {
            cargaAtributosInstancia(id_ubicacion_origen, id_ubicacion_destino, id_ruta, id_compania_emisor);
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Genera una Instancia de Tipo Kilometraje
        /// </summary>
        /// <param name="id_kilometraje">Id Kilometraje</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_kilometraje)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 3, id_kilometraje, 0, 0, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_kilometraje = Convert.ToInt32(r["Id"]);
                        this._id_ubicacion_origen = Convert.ToInt32(r["IdUbicacionorigen"]);
                        this._id_ubicacion_destino = Convert.ToInt32(r["IdUbicacionDestino"]);
                        this._geo_ubicacion_origen = (SqlGeography)r["GeoUbicacionOrigen"];
                        this._geo_ubicacion_destino = (SqlGeography)r["GeoUbicacionDestino"];
                        this._kms_reales = Convert.ToDecimal(r["KmsReales"]);
                        this._kms_maps = Convert.ToDecimal(r["KmsMaps"]);
                        this._tiempo_real = Convert.ToDecimal(r["TiempoReal"]);
                        this._tiempo_maps = Convert.ToDecimal(r["TiempoMaps"]);
                        this._id_ruta = Convert.ToInt32(r["IdRuta"]);
                        this._kms_pago = Convert.ToDecimal(r["kmsPago"]);
                        this._kms_cobro = Convert.ToDecimal(r["kmsCobro"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignamos Resultado
                    resultado = true;
                }
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Genera una Instancia de Kilometraje ligando  un Id Ubicación Origen, Id Ubicacaión destino, Id ruta, Id Compañia emisor.
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicacion Origen</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_ubicacion_origen, int id_ubicacion_destino, int id_ruta, int id_compania_emisor)
        {
            //Declaramos objeto Resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros
            object[] param = { 6, 0, id_ubicacion_origen, id_ubicacion_destino, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, 0, 0, id_ruta, id_compania_emisor, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_kilometraje = Convert.ToInt32(r["Id"]);
                        this._id_ubicacion_origen = Convert.ToInt32(r["IdUbicacionorigen"]);
                        this._id_ubicacion_destino = Convert.ToInt32(r["IdUbicacionDestino"]);
                        this._geo_ubicacion_origen = (SqlGeography)r["GeoUbicacionOrigen"];
                        this._geo_ubicacion_destino = (SqlGeography)r["GeoUbicacionDestino"];
                        this._kms_reales = Convert.ToDecimal(r["KmsReales"]);
                        this._kms_maps = Convert.ToDecimal(r["KmsMaps"]);
                        this._tiempo_real = Convert.ToDecimal(r["TiempoReal"]);
                        this._tiempo_maps = Convert.ToDecimal(r["TiempoMaps"]);
                        this._id_ruta = Convert.ToInt32(r["IdRuta"]);
                        this._kms_pago = Convert.ToDecimal(r["kmsPago"]);
                        this._kms_cobro = Convert.ToDecimal(r["kmsCobro"]);
                        this._id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }

                    //Asignamos Resultado
                    resultado = true;
                }
            }
            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Editar un Kilometraje
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicación Origen para el kilometraje</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino para el kilometraje</param>
        /// <param name="geo_ubicacion_origen">Geo Ubicación Origen para el kilometraje</param>
        /// <param name="geo_ubicacion_destino">Geo Ubicación Destino para el kilometraje</param>
        /// <param name="kms_reales">Kms reales dado un Origen y Destido</param>
        /// <param name="kms_maps">Kms reales dado un Origen y Destido, dado el GIS</param>
        /// <param name="tiempo_real">Tiempo real dado un Origen y Destido</param>
        /// <param name="tiempo_maps">Tiempo maps dado un Origen y Destido, dado el GIS</param>
        /// <param name="id_ruta">Id Ruta correspondiente al kilometraje</param>
        /// <param name="kms_pago">Kms por Pagar</param>
        /// <param name="kms_cobro">Kms por Cobrar</param>
        /// <param name="id_compania_emisor">Id de Compañia  en caso de pertenecer</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaKilometraje(int id_ubicacion_origen, int id_ubicacion_destino, SqlGeography geo_ubicacion_origen, SqlGeography geo_ubicacion_destino,
                                                decimal kms_reales, decimal kms_maps, decimal tiempo_real, decimal tiempo_maps, int id_ruta, decimal kms_pago, decimal kms_cobro, 
                                                int id_compania_emisor, int id_usuario,
                                                bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = {2, this._id_kilometraje, id_ubicacion_origen, id_ubicacion_destino, geo_ubicacion_origen, geo_ubicacion_destino, kms_reales, kms_maps,
                                     tiempo_real, tiempo_maps, id_ruta, kms_pago, kms_cobro, id_compania_emisor, id_usuario, habilitar, "", "" };

            //Establecemos Resultado
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }


        #endregion

        #region Metodos publicos

        /// <summary>
        /// Método encargado de Insertar Kilometraje
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicación Origen para el kilometraje</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino para el kilometraje</param>
        /// <param name="geo_ubicacion_origen">Geo Ubicación Origen para el kilometraje</param>
        /// <param name="geo_ubicacion_destino">Geo Ubicación Destino para el kilometraje</param>
        /// <param name="kms_reales">Kms reales dado un Origen y Destido</param>
        /// <param name="kms_maps">Kms reales dado un Origen y Destido, dado el GIS</param>
        /// <param name="tiempo_real">Tiempo real dado un Origen y Destido</param>
        /// <param name="tiempo_maps">Tiempo maps dado un Origen y Destido, dado el GIS</param>
        /// <param name="id_ruta">Id Ruta correspondiente al kilometraje</param>
        /// <param name="kms_pago">Kms por Pagar</param>
        /// <param name="kms_cobro">Kms por Cobrar</param>
        /// <param name="id_compania_emisor">Id de Compañia  en caso de pertenecer</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaKilometraje(int id_ubicacion_origen, int id_ubicacion_destino, SqlGeography geo_ubicacion_origen, SqlGeography geo_ubicacion_destino,
                                                decimal kms_reales, decimal kms_maps, decimal tiempo_real, decimal tiempo_maps, int id_ruta, decimal kms_pago, decimal kms_cobro,
                                                int id_compania_emisor, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = {1, 0, id_ubicacion_origen, id_ubicacion_destino, geo_ubicacion_origen, geo_ubicacion_destino, kms_reales, kms_maps,
                                     tiempo_real, tiempo_maps, id_ruta, kms_pago, kms_cobro, id_compania_emisor, id_usuario, true, "", "" };

            //Realizando la actualización
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Método encargado de editar Kilometraje
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicación Origen para el kilometraje</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino para el kilometraje</param>
        /// <param name="geo_ubicacion_origen">Geo Ubicación Origen para el kilometraje</param>
        /// <param name="geo_ubicacion_destino">Geo Ubicación Destino para el kilometraje</param>
        /// <param name="kms_reales">Kms reales dado un Origen y Destido</param>
        /// <param name="kms_maps">Kms reales dado un Origen y Destido, dado el GIS</param>
        /// <param name="tiempo_real">Tiempo real dado un Origen y Destido</param>
        /// <param name="tiempo_maps">Tiempo maps dado un Origen y Destido, dado el GIS</param>
        /// <param name="id_ruta">Id Ruta correspondiente al kilometraje</param>
        /// <param name="kms_pago">Kms por Pagar</param>
        /// <param name="kms_cobro">Kms por Cobrar</param>
        /// <param name="id_compania_emisor">Id de Compañia  en caso de pertenecer</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaKilometraje(int id_ubicacion_origen, int id_ubicacion_destino, SqlGeography geo_ubicacion_origen, SqlGeography geo_ubicacion_destino,
                                                decimal kms_reales, decimal kms_maps, decimal tiempo_real, decimal tiempo_maps, int id_ruta, decimal kms_pago, decimal kms_cobro, 
                                                int id_compania_emisor, int id_usuario)
        {

            return this.editaKilometraje(id_ubicacion_origen, id_ubicacion_destino, geo_ubicacion_origen, geo_ubicacion_destino, kms_reales, kms_maps,
                                     tiempo_real, tiempo_maps, id_ruta, kms_pago, kms_cobro, id_compania_emisor, id_usuario, this._habilitar);
        }


        /// <summary>
        /// Método encargado de editar Kilometraje
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicación Origen para el kilometraje</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino para el kilometraje</param>
        /// <param name="geo_ubicacion_origen">Geo Ubicación Origen para el kilometraje</param>
        /// <param name="geo_ubicacion_destino">Geo Ubicación Destino para el kilometraje</param>
        /// <param name="kms_reales">Kms reales dado un Origen y Destido</param>
        /// <param name="kms_maps">Kms reales dado un Origen y Destido, dado el GIS</param>
        /// <param name="tiempo_real">Tiempo real dado un Origen y Destido</param>
        /// <param name="tiempo_maps">Tiempo maps dado un Origen y Destido, dado el GIS</param>
        /// <param name="id_ruta">Id Ruta correspondiente al kilometraje</param>
        /// <param name="kms_pago">Kms por Pagar</param>
        /// <param name="kms_cobro">Kms por Cobrar</param>
        /// <param name="id_compania_emisor">Id de Compañia  en caso de pertenecer</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaKilometrajeImportacion(int id_ubicacion_origen, int id_ubicacion_destino,
                                                decimal kms_reales, decimal tiempo_real,decimal kms_pago, decimal kms_cobro,
                                                int id_compania_emisor, int id_usuario)
        {

            return this.editaKilometraje(id_ubicacion_origen, id_ubicacion_destino, this.geo_ubicacion_origen, this.geo_ubicacion_destino, kms_reales, this.kms_maps,
                                     tiempo_real, this.tiempo_maps, this.id_ruta, kms_pago, kms_cobro, id_compania_emisor, id_usuario, this._habilitar);
        }


        /// <summary>
        /// Deshabilita Kilometraje
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaKilometraje(int id_usuario)
        {
            return this.editaKilometraje(this._id_ubicacion_origen, this._id_ubicacion_destino, this._geo_ubicacion_origen, this._geo_ubicacion_destino, this._kms_reales, this._kms_maps,
                                     this._tiempo_real, this._tiempo_maps, this._id_ruta, this._kms_pago, this._kms_cobro, this._id_compania_emisor, id_usuario, false);
        }

        /// <summary>
        /// Carga Kilometraje ligadando una Ubicacion Origen, Ubicacion Destino y un Id de Compania
        /// </summary>
        /// <param name="id_ubicacion_origen">Id Ubicación Origen para el kilometraje</param>
        /// <param name="id_ubicacion_destino">Id Ubicación Destino para el kilometraje</param>
        /// <param name="id_ruta">Id Ruta correspondiente al kilometraje</param>
        /// <param name="id_compania_emisor">Id de Compañia  en caso de pertenecer</param>
        /// <returns></returns>
        public static int BuscaKilometraje(int id_ubicacion_origen, int id_ubicacion_destino, int id_compania_emisor, int id_ruta)
        {
            //Definiendo objeto de retorno
            int IdKilometraje = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 5, 0, id_ubicacion_origen, id_ubicacion_destino, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, id_ruta, 0, 0, id_compania_emisor, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  el Total de InicializaDataGridViews
                    IdKilometraje = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToInt32(r["Id"])).FirstOrDefault();
                }

            }
            return IdKilometraje;
        }

        /// <summary>
        /// Metodo encargado de buscar el kilometraje en base a un id de movimiento
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_ruta"></param>
        /// <returns>Regresa el valor de kilometraje</returns>
        public static decimal BuscaKilometraje(int id_movimiento, int id_compania_emisor, int id_ruta)
        {
            //Definiendo objeto de retorno
            decimal kilometraje = 0;

            //Inicializando arreglo de parámetros
            object[] param = { 7, 0, 0, 0, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, id_ruta, 0, 0, id_compania_emisor, 0, false, id_movimiento, "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach(DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Retornamos el valor de kilometraje encontrado
                        kilometraje = Convert.ToDecimal(r["kmsReales"]);
                    }
                }

            }
            return kilometraje;
        }
        /// <summary>
        /// Método  encargado de Actualizar los atributos de Kilometraje
        /// </summary>
        /// <returns></returns>
        public bool ActualizaKilometraje()
        {
            return this.cargaAtributosInstancia(this._id_kilometraje);
        }
        /// <summary>
        /// Método encargado de Obtener los Kilometrajes según los Siguientes Criterios
        /// </summary>
        /// <param name="id_origen">Ubicación de Origen</param>
        /// <param name="id_destino">Ubicación de Destino</param>
        /// <param name="id_cd_origen">Ciudad de Origen</param>
        /// <param name="id_cd_destino">Ciudad de Destino</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneKilometrajes(int id_origen, int id_destino, int id_cd_origen, int id_cd_destino, int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtKMS = null;

            //Inicializando arreglo de parámetros
            object[] param = { 4, 0, id_origen, id_destino, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, 0, 0, 0, id_compania, 0, false, 
                               id_cd_origen.ToString(), id_cd_destino.ToString() };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existe el Reporte
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtKMS = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtKMS;
        }
        /// <summary>
        /// Método encargado de Obtener el Kilometraje Inverso
        /// </summary>
        /// <param name="id_kilometraje">Kilometraje</param>
        /// <returns></returns>
        public static Kilometraje ObtieneKilometrajeInverso(int id_kilometraje)
        {
            //Declarando Objeto de Retorno
            Kilometraje kmsInv = new Kilometraje();

            //Instanciando Kilometraje
            using(Kilometraje kms = new Kilometraje(id_kilometraje))
            {
                //Validando que exista el Kilometraje
                if (kms.id_kilometraje > 0)

                    //Instanciando Kilometraje
                    kmsInv = new Kilometraje(kms.id_ubicacion_destino, kms.id_ubicacion_origen, kms.id_ruta, kms.id_compania_emisor);
            }

            //Devolviendo Objeto de Retorno
            return kmsInv;
        }
        /// <summary>
        /// Método encargado de Obtener los Kilometrajes pendientes por capturar o con movimientos no actualizados según
        /// </summary>
        /// <param name="id_origen">Ubicación de Origen</param>
        /// <param name="id_destino">Ubicación de Destino</param>
        /// <param name="id_cd_origen">Ciudad de Origen</param>
        /// <param name="id_cd_destino">Ciudad de Destino</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <returns></returns>
        public static DataTable ObtieneKilometrajeFaltante(int id_origen, int id_destino, int id_cd_origen, int id_cd_destino, int id_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtKMS = null;

            //Inicializando arreglo de parámetros
            object[] param = { 8, 0, id_origen, id_destino, SqlGeography.Null, SqlGeography.Null, 0, 0, 0, 0, 0, 0, 0, id_compania, 0, false, 
                               id_cd_origen.ToString(), id_cd_destino.ToString() };

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existe el Reporte
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtKMS = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtKMS;
        }

        #endregion
    }
}
