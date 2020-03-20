using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using RestSharp;
using SAT_CL.Global;
using SAT_CL.Maps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Monitoreo
{
    /// <summary>
    /// Clase que permite Insertar, Actualizar y consultar registro de proveedor de WS unidad
    /// </summary>
    public class ProveedorWSUnidad : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store prcedure de la tabla Proveedor WS Unidad 
        /// </summary>
        private static string nom_sp = "monitoreo.sp_proveedor_ws_unidad_tpwsu";

        private int _id_proveedor_ws_unidad;
        /// <summary>
        /// Identifica el registro de Proveedor  web service unidad
        /// </summary>
        public int id_proveedor_ws_unidad
        {
            get { return _id_proveedor_ws_unidad; }
        }
        private int _id_proveedor_ws;
        /// <summary>
        /// Identifica al proveedor de web service
        /// </summary>
        public int id_proveedor_ws
        {
            get { return _id_proveedor_ws; }
        }
        private int _id_unidad;
        /// <summary>
        /// Identifica a una unidad que contenga sistema de rastreo (Tractor, Remolque,etc.)
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
        private string _identificador_unidad;
        /// <summary>
        /// Nombre o UUID con el que se puede identificar a una unidad en el Web Service
        /// </summary>
        public string identificador_unidad
        {
            get { return _identificador_unidad; }
        }
        private bool _bit_antena_defecto;
        /// <summary>
        /// Indica si es la Antena por Defecto
        /// </summary>
        public bool bit_antena_defecto
        {
            get { return _bit_antena_defecto; }
        }
        private int _tiempo_encendido;
        /// <summary>
        /// Indica el Tiempo Limite de Encendido
        /// </summary>
        public int tiempo_encendido
        {
            get { return _tiempo_encendido; }
        }
        private int _tiempo_apagado;
        /// <summary>
        /// Indica el Tiempo Limite de Apagado
        /// </summary>
        public int tiempo_apagado
        {
            get { return _tiempo_apagado; }
        }
        private bool _habilitar;
        /// <summary>
        /// Habilita o Deshabilita el uso de una Lista de Contactos 
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public ProveedorWSUnidad()
        {
            this._id_proveedor_ws_unidad = 0;
            this._id_proveedor_ws = 0;
            this._id_unidad = 0;
            this._identificador_unidad = "";
            this._bit_antena_defecto = false;
            this._tiempo_encendido = 0;
            this._tiempo_apagado = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Contructor que inicializa los atributos a partir de un regsitro de Proveedor de WS Unidad
        /// </summary>
        /// <param name="id_proveedor_ws_unidad"></param>
        public ProveedorWSUnidad(int id_proveedor_ws_unidad)
        {
            //Invoca al método que inicializa los atributos de la clase
            cargaAtributos(id_proveedor_ws_unidad);
        }
        /// <summary>
        /// Constructor que inicializa los atributos apartir de un Proveedor de WS(Servicio Web) y una Unidad
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor de Servicio Web</param>
        /// <param name="id_unidad">Unidad Deseada</param>
        /// <param name="no_antena_gps">Antena GPS</param>
        public ProveedorWSUnidad(int id_proveedor_ws, int id_unidad, string no_antena_gps)
        {
            //Invoca al método que inicializa los atributos de la clase
            cargaAtributos(id_proveedor_ws, id_unidad, no_antena_gps);
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~ProveedorWSUnidad()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que busca y asigna a los atributos los valores de un registro de Proveedor de WS Unidad
        /// </summary>
        /// <param name="id_proveedor_ws_unidad"></param>
        /// <returns></returns>
        private bool cargaAtributos(int id_proveedor_ws_unidad)
        {
            //Creción de la variable parametro
            bool retorno = false;
            //Creación del arreglo que alamcena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 3, id_proveedor_ws_unidad, 0, 0, "", false, 0, 0, 0, false, "", "" };
            //Invoca al método que realiza la busqueda del registro y el resultado lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset tenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre el dataset y asigna el resultado a los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws_unidad = id_proveedor_ws_unidad;
                        this._id_proveedor_ws = Convert.ToInt32(r["IdProveedor"]);
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._identificador_unidad = Convert.ToString(r["IdentificadorUnidad"]);
                        this._bit_antena_defecto = Convert.ToBoolean(r["BitAntenaDefecto"]);
                        this._tiempo_encendido = Convert.ToInt32(r["TiempoEncendido"]);
                        this._tiempo_apagado = Convert.ToInt32(r["TiempoApagado"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que busca y asigna a los atributos los valores de un Proveedor de WS y una Unidad
        /// </summary>
        /// <param name="id_proveedor_ws">Proveedor de Servicio Web</param>
        /// <param name="id_unidad">Unidad Deseada</param>
        /// <param name="no_antena_gps">Antena GPS</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_proveedor_ws, int id_unidad, string no_antena_gps)
        {
            //Creción de la variable parametro
            bool retorno = false;
            //Creación del arreglo que alamcena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 4, 0, id_proveedor_ws, id_unidad, no_antena_gps, 0, 0, 0, false, "", "" };
            //Invoca al método que realiza la busqueda del registro y el resultado lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset tenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre el dataset y asigna el resultado a los atributos de la clase
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_proveedor_ws_unidad = id_proveedor_ws_unidad;
                        this._id_proveedor_ws = Convert.ToInt32(r["IdProveedor"]);
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._identificador_unidad = Convert.ToString(r["IdentificadorUnidad"]);
                        this._bit_antena_defecto = Convert.ToBoolean(r["BitAntenaDefecto"]);
                        this._tiempo_encendido = Convert.ToInt32(r["TiempoEncendido"]);
                        this._tiempo_apagado = Convert.ToInt32(r["TiempoApagado"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actuliza el registro de Proveedor de WS Unidad
        /// </summary>
        /// <param name="id_proveedor_ws">Actualiza el proveedor de web service</param>
        /// <param name="id_unidad">Actualiza la unidad registrada en el web service</param>
        /// <param name="identificador_unidad">Actualiza el UUID o nombre con el cual se identifica rapidamente a una unidad</param>
        /// <param name="bit_antena_defecto">Actualiza el Indicador de Antena por Defecto</param>
        /// <param name="tiempo_encendido">Actualiza el Tiempo Limite de Encendido</param>
        /// <param name="tiempo_apagado">Actualiza el Tiempo Limite de Apagado</param>
        /// <param name="id_usuario">Actualiza al usuario que realizo acciones sobr el registro</param>
        /// <param name="habilitar">Habilita o Deshabilita el uso de una Lista de Contactos </param>
        /// <returns></returns>
        private RetornoOperacion editarProveedorWSUnidad(int id_proveedor_ws, int id_unidad, string identificador_unidad, bool bit_antena_defecto,
                                                int tiempo_encendido, int tiempo_apagado, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar un registro
            object[] param = { 2, this._id_proveedor_ws_unidad, id_proveedor_ws, id_unidad, identificador_unidad, bit_antena_defecto, tiempo_encendido, tiempo_apagado, id_usuario, habilitar, "", "" };
            //Invoca al método que realiza la actualización de un registro y el resultado lo almacena en el objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que Inserta el registro de Proveedor de WS Unidad
        /// </summary>
        /// <param name="id_proveedor_ws">Inserta el proveedor de web service</param>
        /// <param name="id_unidad">Inserta la unidad registrada en el web service</param>
        /// <param name="identificador_unidad">Inserta el UUID o nombre con el cual se identifica rapidamente a una unidad</param>
        /// <param name="tiempo_encendido">Inserta el Tiempo Limite de Encendido</param>
        /// <param name="tiempo_apagado">Inserta el Tiempo Limite de Apagado</param>
        /// <param name="id_usuario">Inserta al usuario que realizo acciones sobr el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarProveedorWSUnidad(int id_proveedor_ws, int id_unidad, string identificador_unidad, bool bit_antena_defecto,
                                                                 int tiempo_encendido, int tiempo_apagado, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creación del arreglo que almacena los datos necesarios para Inserta un registro
            object[] param = { 1, 0, id_proveedor_ws, id_unidad, identificador_unidad, bit_antena_defecto, tiempo_encendido, tiempo_apagado, id_usuario, true, "", "" };

            //Inicializando Transacción
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando si sera la Antena Predeterminada
                if (bit_antena_defecto)
                {
                    //Obteniendo Antena por Defecto
                    ProveedorWSUnidad defecto = ProveedorWSUnidad.ObtieneAntenaPredeterminada(id_unidad);

                    //Validando si existe la Antena por Defecto
                    if (defecto.habilitar)

                        //Quitando Antena por Defecto
                        retorno = defecto.EditarProveedorWSUnidad(defecto._id_proveedor_ws, defecto.id_unidad, defecto.identificador_unidad,
                                                                  false, defecto._tiempo_encendido, defecto.tiempo_apagado, id_usuario);
                    else
                        //Instanciando Resultado Positivo
                        retorno = new RetornoOperacion(0);
                }
                else
                    //Instanciando Resultado Positivo
                    retorno = new RetornoOperacion(0);

                //Validando Operación Exitosa
                if (retorno.OperacionExitosa)
                {
                    //Invoca al método que realiza la actualización de un registro y el resultado lo almacena en el objeto retorno
                    retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

                    //Validando Operación Exitosa
                    if (retorno.OperacionExitosa)

                        //Completando Operación
                        trans.Complete();
                }
            }

            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actuliza el registro de Proveedor de WS Unidad
        /// </summary>
        /// <param name="id_proveedor_ws">Actualiza el proveedor de web service</param>
        /// <param name="id_unidad">Actualiza la unidad registrada en el web service</param>
        /// <param name="identificador_unidad">Actualiza el UUID o nombre con el cual se identifica rapidamente a una unidad</param>
        /// <param name="tiempo_encendido">Actualiza el Tiempo Limite de Encendido</param>
        /// <param name="tiempo_apagado">Actualiza el Tiempo Limite de Apagado</param>
        /// <param name="id_usuario">Actualiza al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarProveedorWSUnidad(int id_proveedor_ws, int id_unidad, string identificador_unidad, bool bit_antena_defecto, int tiempo_encendido, int tiempo_apagado, int id_usuario)
        {
            //Invoca al  método que actualiza los registro de Proveedor WS unidad
            return editarProveedorWSUnidad(id_proveedor_ws, id_unidad, identificador_unidad, bit_antena_defecto, tiempo_encendido, tiempo_apagado, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Marcar la Antena GPS por Defecto
        /// </summary>
        /// <param name="id_usuario">Actualiza al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion MarcaAntenaPorDefecto(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Transacción
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Antena por Defecto
                ProveedorWSUnidad defecto = ProveedorWSUnidad.ObtieneAntenaPredeterminada(this._id_unidad);

                //Validando si existe la Antena por Defecto
                if (defecto.habilitar)

                    //Quitando Antena por Defecto
                    result = defecto.EditarProveedorWSUnidad(defecto._id_proveedor_ws, defecto.id_unidad, defecto.identificador_unidad,
                                                             false, defecto._tiempo_encendido, defecto.tiempo_apagado, id_usuario);
                else
                    //Instanciando Resultado Positivo
                    result = new RetornoOperacion(this._id_proveedor_ws_unidad);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Invoca al método que realiza la actualización de un registro y el resultado lo almacena en el objeto retorno
                    result = editarProveedorWSUnidad(this._id_proveedor_ws, this._id_unidad, this._identificador_unidad, true,
                                                     this._tiempo_encendido, this._tiempo_apagado, id_usuario, this._habilitar);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)

                        //Completando Operación
                        trans.Complete();
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que cambia el estado de un regsitro de Proveedor WS unidad
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarProveedorWSUnidad(int id_usuario)
        {
            //Invoca al  método que actualiza los registro de Proveedor WS unidad
            return editarProveedorWSUnidad(this._id_proveedor_ws, this._id_unidad, this._identificador_unidad, this._bit_antena_defecto, this._tiempo_encendido, this._tiempo_apagado, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaProveedorWSUnidad()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_proveedor_ws_unidad);
        }
        /// <summary>
        /// Método encargado de Obtener las Antenas de una Unidad
        /// </summary>
        /// <param name="id_unidad"></param>
        /// <returns></returns>
        public static DataTable ObtieneProveedorUnidad(int id_unidad)
        {
            //Declarando Objeto de Retorno
            DataTable dtProveedorUnidad = null;

            //Creación del arreglo que almacena los datos necesarios para Inserta un registro
            object[] param = { 5, 0, 0, id_unidad, "", false, 0, 0, 0, false, "", "" };

            //Invoca al método que realiza la busqueda del registro y el resultado lo alamcena en un dataset
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset tenga datos
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))

                    //Asignando Valores
                    dtProveedorUnidad = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtProveedorUnidad;
        }
        /// <summary>
        /// Método encargado de Obtener la Antena por Defecto dado un Proveedor y una Unidad
        /// </summary>
        /// <param name="id_unidad">Unidad</param>
        /// <returns></returns>
        public static ProveedorWSUnidad ObtieneAntenaPredeterminada(int id_unidad)
        {
            //Declarando Objeto de Retorno
            ProveedorWSUnidad pro_unidad = new ProveedorWSUnidad();

            //Creación del arreglo que almacena los datos necesarios para Inserta un registro
            object[] param = { 6, 0, 0, id_unidad, "", false, 0, 0, 0, false, "", "" };

            //Invoca al método que realiza la busqueda del registro y el resultado lo alamcena en un dataset
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que el dataset tenga datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recoriendo Resultados
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Instanciando Proveedor Unidad Default
                        pro_unidad = new ProveedorWSUnidad(Convert.ToInt32(dr["Id"]));

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return pro_unidad;
        }
        /// <summary>
        /// Método encargado de Obtener la Posición Actual de la Unidad
        /// </summary>
        /// <param name="id_provedor_ws">Proveedor del Web Service</param>
        /// <param name="unidad_proveedor">Unidad Deseada</param>
        /// <param name="identificador">Identificador de la unidad en WS</param>
        /// <param name="latitud">Latitud</param>
        /// <param name="longitud">Longitud</param>
        /// <param name="fecha_gps">Fecha GPS</param>
        /// <param name="velocidad">Velocidad</param>
        /// <param name="bit_encendido">Indicador de Encendido</param>
        /// <returns></returns>
        public static RetornoOperacion ObtienePosicionActualUnidad(int id_provedor_ws, int unidad_proveedor, 
                                            out string ubicacion, out double latitud, out double longitud, out DateTime fecha_gps,
                                            out decimal velocidad, out bool bit_encendido, out decimal cantidad_combustible)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Inicializando Parametros de Salida
            latitud = longitud = 0;
            ubicacion = "";
            fecha_gps = DateTime.MinValue;
            velocidad = 0.00M;
            bit_encendido = false;
            cantidad_combustible = 0.00M;
            string BD = ConfigurationManager.ConnectionStrings["TECTOS_SAT_db"].ConnectionString, BdProduccion = "";

            //Validando BD
            if (BD.Contains("AZTLAN"))
                BdProduccion = "AZTLAN";
            else if (BD.Contains("NEXTIA"))
                BdProduccion = "NEXTIA";

            //Instanciando Servicio Web del Proveedor
            using (ProveedorWS pro = new ProveedorWS(id_provedor_ws))
            using (ProveedorWSUnidad unidad = new ProveedorWSUnidad(unidad_proveedor))
            {
                //Validando si existe el Registro
                if (pro.habilitar)
                {
                    //Validando si existe la Unidad
                    if (unidad.habilitar)
                    {
                        /** VALIDANDO BD **/
                        switch (BdProduccion)
                        {
                            case "AZTLAN":
                                {
                                    //Validando Compania
                                    switch (pro.id_compania)
                                    {
                                        //ARI TECTOS
                                        case 1:
                                        //TRANSBEAR
                                        case 2:
                                        //TECTOS TEST
                                        case 72:
                                        //TEM
                                        case 76:
                                        //ETV1
                                        case 1081:
                                        //JARUMI
                                        case 1292:
                                        //AXEJIT
                                        case 1353:
                                        //TRANSFRIO
                                        case 1440:
                                        //MELK
                                        case 1758:
                                            {
                                                retorno = EjecutaMonitoreoGPS(pro, unidad, out ubicacion, out latitud, out longitud, out fecha_gps,
                                                                out velocidad, out bit_encendido, out cantidad_combustible);
                                                break;
                                            }
                                        default:
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No existe la Configuración de GPS para esta Compania");
                                            break;
                                    }
                                    break;
                                }
                            case "NEXTIA":
                                {
                                    //Validando Compania
                                    switch (pro.id_compania)
                                    {
                                        //EMCI
                                        case 1:
                                        //TECTOS - DEV
                                        case 1124:
                                        //ROMIDA
                                        case 1126:
                                        //GROCHA
                                        case 1127:
                                        //TVH
                                        case 1149:
                                            {
                                                retorno = EjecutaMonitoreoGPS(pro, unidad, out ubicacion, out latitud, out longitud, out fecha_gps,
                                                                out velocidad, out bit_encendido, out cantidad_combustible);
                                                break;
                                            }
                                        default:
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No existe la Configuración de GPS para esta Compania");
                                            break;
                                    }
                                    break;
                                }
                            default:
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No existe la Configuración de GPS para esta Compania");
                                break;
                        }
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No existe Proveedor para esta Unidad");
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No existe el Web Service del Proveedor");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Segmentar el Consumo de Monitoreo por Unidad dado un Proveedor GPS
        /// </summary>
        /// <param name="proveedor"></param>
        /// <param name="unidad"></param>
        /// <param name="ubicacion"></param>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        /// <param name="fecha_gps"></param>
        /// <param name="velocidad"></param>
        /// <param name="bit_encendido"></param>
        /// <param name="cantidad_combustible"></param>
        /// <returns></returns>
        public static RetornoOperacion EjecutaMonitoreoGPS(ProveedorWS proveedor, ProveedorWSUnidad unidad, out string ubicacion,
                                                    out double latitud, out double longitud, out DateTime fecha_gps,
                                                    out decimal velocidad, out bool bit_encendido, out decimal cantidad_combustible)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            XDocument datosObtenidos = new XDocument();
            //Inicializando Variables de Retorno
            ubicacion = ""; latitud = longitud = 0; fecha_gps = DateTime.MinValue;
            velocidad = 0.00M; bit_encendido = false; cantidad_combustible = 0.00M;

            //Validando Proveedor GPS (Configuración)
            if (proveedor.habilitar)
            {
                //Validando Proveedor con Unidad
                if (proveedor.id_proveedor_ws == unidad.id_proveedor_ws)
                {
                    //Validando Punto Final
                    switch (proveedor.identificador)
                    {
                        case "TEM - IVehicleWebService":
                        case "TECTOS - IVehicleWebService":
                            {
                                //Validando Punto Final
                                switch (proveedor.accion)
                                {
                                    case "getVehiclesByClient":
                                        {
                                            //Objeto de Parametros
                                            XDocument parametros = new XDocument();

                                            //Obteniendo Parametros
                                            using (DataTable dtParams = ProveedorWSCargaParametros.ObtieneParametrosProveedorWS(proveedor.id_proveedor_ws))
                                            {
                                                //Existen Parametros
                                                if (Validacion.ValidaOrigenDatos(dtParams))
                                                {
                                                    //Copiando Tabla
                                                    DataTable dt = dtParams.Copy();
                                                    dt.TableName = "request";

                                                    //Creando flujo de memoria
                                                    using (System.IO.Stream s = new System.IO.MemoryStream())
                                                    {
                                                        //Leyendo flujo de datos XML
                                                        dt.WriteXml(s);

                                                        //Obteniendo DataSet en XML
                                                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                                        //Añadiendo Elemento al nodo de Publicaciones
                                                        parametros = new XDocument(dataTableElement.Element("request"));
                                                    }
                                                }
                                            }

                                            //Creando Ensobretado SOAP
                                            XDocument ensobretado = XDocument.Parse(
                                                        @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:sei='http://sei.rest.services.client.maya.omnitracs.mx/'>
                                                            <soapenv:Header/>
                                                            <soapenv:Body>
                                                                <sei:" + proveedor.accion + @">
                                                                    " + parametros.ToString() + @"
                                                                </sei:" + proveedor.accion + @">
                                                            </soapenv:Body>
                                                        </soapenv:Envelope>");//*/

                                            //Consumiendo Servicio Web de Proveedor
                                            retorno = proveedor.ConsumeProveedorWS(ensobretado, out datosObtenidos);

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Creando NameSpace's
                                                XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
                                                XNamespace ns1 = @"http://sei.rest.services.client.maya.omnitracs.mx/";

                                                //Obteniendo Elemento Deseado
                                                XDocument elemento = new XDocument(datosObtenidos.Root.Element(ns + "Body").Element(ns1 + proveedor.accion + "Response").Element("return"));

                                                //Convirtiendo XML a Tabla
                                                DataSet ds = new DataSet();
                                                ds.ReadXml(new XmlTextReader(new StringReader(elemento.ToString())));

                                                //Validando Origen de Datos
                                                if (Validacion.ValidaOrigenDatos(ds))
                                                {
                                                    //Declarando Variables Auxiliares
                                                    string[] serialNumbers = new string[1];
                                                    int count = 0;
                                                    DateTime fec_ws = DateTime.MinValue;

                                                    //Obteniendo Identificadores
                                                    using (DataTable dtIndentificadores = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtieneProveedorUnidad(unidad.id_unidad))
                                                    {
                                                        //Validando que existan
                                                        if (Validacion.ValidaOrigenDatos(dtIndentificadores))
                                                        {
                                                            //Creando Arreglo Dinamico
                                                            serialNumbers = new string[dtIndentificadores.Rows.Count];

                                                            //Recorriendo Filas
                                                            foreach (DataRow dr in dtIndentificadores.Rows)
                                                            {
                                                                //Asignando Identificador
                                                                serialNumbers[count] = dr["Identificador"].ToString();

                                                                //Incrementando Contador
                                                                count++;
                                                            }

                                                            //Obteniendo Fila
                                                            IEnumerable<DataRow> rows = (from DataRow r in ds.Tables["vehicleDevice"].Rows
                                                                                         where serialNumbers.Contains(r.Field<string>("serialnumber"))
                                                                                         select r);

                                                            //Asignando Valores
                                                            if (rows.Count() > 0)
                                                            {
                                                                //Instanciando Resultado Positivo
                                                                retorno = new RetornoOperacion(0);

                                                                //Recorriendo Ciclo
                                                                foreach (DataRow dr in rows)
                                                                {
                                                                    //Asignando valores
                                                                    latitud = Convert.ToDouble(dr["latitude"]);
                                                                    longitud = Convert.ToDouble(dr["longitude"]);
                                                                    ubicacion = dr["location"].ToString();
                                                                    DateTime.TryParse(dr["lastGPSDate"].ToString(), out fecha_gps);
                                                                    velocidad = Convert.ToDecimal(dr["speed"]);
                                                                    bit_encendido = Convert.ToBoolean(dr["ignition"].ToString().Equals("") ? "false" : dr["ignition"].ToString());
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                retorno = new RetornoOperacion("No se encontraron Unidades con la(s) Antena(s) proporcionada(s): '" + string.Join(",", serialNumbers) + "'");
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            retorno = new RetornoOperacion("No existen Identificadores por Filtrar");
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case "MELK - CSI":
                        case "HJV - CSI":
                            {
                                //Validando Punto Final
                                switch (proveedor.accion)
                                {
                                    case "localizarUnidad":
                                        {
                                            //Objeto de Parametros
                                            XDocument parametros = new XDocument();

                                            //Obteniendo Parametros
                                            using (DataTable dtParams = ProveedorWSCargaParametros.ObtieneParametrosProveedorWS(proveedor.id_proveedor_ws))
                                            {
                                                //Existen Parametros
                                                if (Validacion.ValidaOrigenDatos(dtParams))
                                                {
                                                    //Copiando Tabla
                                                    DataTable dt = dtParams.Copy();
                                                    dt.TableName = "request";

                                                    //Creando flujo de memoria
                                                    using (System.IO.Stream s = new System.IO.MemoryStream())
                                                    {
                                                        //Leyendo flujo de datos XML
                                                        dt.WriteXml(s);

                                                        //Obteniendo DataSet en XML
                                                        XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));

                                                        //Añadiendo Elemento al nodo de Publicaciones
                                                        XNamespace ci = "http://localhost/Demo/";
                                                        parametros = new XDocument(new XElement(ci + proveedor.accion, (from XElement xe in dataTableElement.Element("request").Elements() select new XElement(ci + xe.Name.LocalName, xe.Value))));
                                                    }
                                                }
                                            }

                                            //Creando Ensobretado SOAP
                                            XDocument ensobretado = XDocument.Parse(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'><soapenv:Header/><soapenv:Body>" + parametros.Root.ToString().Replace("NoUnidadDinamico", unidad.identificador_unidad) + @"</soapenv:Body></soapenv:Envelope>");

                                            //Consumiendo Servicio Web de Proveedor
                                            retorno = proveedor.ConsumeProveedorWS(ensobretado, out datosObtenidos);

                                            //Validando Operación
                                            if (retorno.OperacionExitosa)
                                            {
                                                XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
                                                XNamespace ns1 = "http://localhost/Demo/";
                                                XNamespace ns2 = "";

                                                //Obteniendo Elemento raíz de respuesta 
                                                XElement elemento = new XElement(datosObtenidos.Root.Element(ns + "Body").Element(ns1 + "localizarUnidadResponse").Element(ns1 + "localizarUnidadResult").Element(ns2 + "servicio"));

                                                //Validando estatus de consulta (OK)
                                                if (elemento.Element(ns2 + "status").Value.ToUpper() == "OK")
                                                {
                                                    //Recuperando información de equipo
                                                    XElement equipo = elemento.Element(ns2 + "equipo");

                                                    //Asignando valores de evento
                                                    latitud = Convert.ToDouble(Xml.DevuelveValorElementoCadena(equipo.Element("lat"), "0"));
                                                    longitud = Convert.ToDouble(Xml.DevuelveValorElementoCadena(equipo.Element("lng"), "0"));

                                                    //Recuperando geocodificación de lat/lng
                                                    RetornoOperacion geoInv = Maps.Geocoding.Instance.ObtenerGeocofificacionInversa(latitud, longitud, "* UBICACIÓN NO DISPONIBLE *");
                                                    ubicacion = geoInv.Mensaje;

                                                    DateTime.TryParse(equipo.Element("fechaHora").Value, out fecha_gps);
                                                    velocidad = Convert.ToDecimal(Xml.DevuelveValorElementoCadena(equipo.Element("velocidad"), "0").ToUpper().Replace(" KM/H", ""));
                                                    //AUTOREPORT OFF (PARA TRACTORES) / APAGADO (REMOLQUES)
                                                    bit_encendido = (equipo.Element("nomEvento").Value.ToUpper().Contains(" OFF") || equipo.Element("nomEvento").Value.ToUpper().Contains("APAGADO")) ? false : true;

                                                }
                                                else
                                                    retorno = new RetornoOperacion(elemento.Element("status").Value);
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case "TECTOS - ADS":
                        case "GROCHA - ADS":
                            {
                                //Validando Punto Final
                                switch (proveedor.accion)
                                {
                                    case "/data/getLastPosition":
                                        {
                                            //Declarando Objeto de Retorno
                                            retorno = new RetornoOperacion("El proceso sigue en desarrollo");
                                            string token_auth = @"", tipo_auth = @"", fecha_peticion = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyyy-MM-dd");

                                            //Obteniendo URL de Autenticación (TOKEN)
                                            string url_token = @"" + proveedor.endpoin + "/oauth/token", 
                                                   url_gps = @"" + proveedor.endpoin + proveedor.accion;
                                            RestClient clienteToken = new RestClient(url_token),
                                                       clienteGps = new RestClient(url_gps);

                                            //Configurando Datos del Cliente y de la Petición
                                            RestRequest peticionToken = new RestRequest(Method.POST),
                                                        peticionGps = new RestRequest(Method.POST);
                                            clienteToken.Timeout = -1;
                                            peticionToken.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(proveedor.usuario + ":" + proveedor.contraseña)));
                                            peticionToken.AddHeader("Content-Type", "multipart/form-data");
                                            peticionToken.AlwaysMultipartFormData = true;

                                            //Obteniendo y Asignando Parametros
                                            string grant_type = "", username = "", password = "";
                                            using (DataTable dtParams = ProveedorWSParametros.ObtieneParametrosProveedorWS(proveedor.id_proveedor_ws))
                                            {
                                                if (Validacion.ValidaOrigenDatos(dtParams))
                                                {
                                                    //Obteniendo GrantType
                                                    grant_type = (from DataRow p in dtParams.Rows
                                                                  where p["NombreParametro"].ToString().Equals("grant_type")
                                                                  select p["ValorParametro"].ToString()).FirstOrDefault();
                                                    //Obteniendo User Name
                                                    username = (from DataRow p in dtParams.Rows
                                                                where p["NombreParametro"].ToString().Equals("username")
                                                                select p["ValorParametro"].ToString()).FirstOrDefault();
                                                    //Obteniendo Password
                                                    password = (from DataRow p in dtParams.Rows
                                                                where p["NombreParametro"].ToString().Equals("password")
                                                                select p["ValorParametro"].ToString()).FirstOrDefault();
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(grant_type) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                                            {
                                                //Añadiendo Parametros
                                                peticionToken.AddParameter("grant_type", grant_type);
                                                peticionToken.AddParameter("username", username);
                                                peticionToken.AddParameter("password", password);
                                            }

                                            //Consumiendo Petición
                                            IRestResponse respuestaToken = clienteToken.Execute(peticionToken);
                                            JObject json_token = JObject.Parse(respuestaToken.Content);
                                            if (json_token != null)
                                            {
                                                //Obteniendo Token de Autorización
                                                token_auth = (string)json_token["access_token"];
                                                tipo_auth = (string)json_token["token_type"];

                                                //Configurando Petición GPS
                                                clienteGps.Timeout = -1;
                                                peticionGps.AddHeader("Content-Type", "application/json");
                                                peticionGps.AddHeader("Authorization", tipo_auth + " " + token_auth);

                                                //Parametros de Busqueda
                                                string gps_params = @"{ 'fecha' : '" +fecha_peticion + "', 'idUnit' : '"+unidad.identificador_unidad+"' }";
                                                JObject json_params = JObject.Parse(gps_params);
                                                peticionGps.AddParameter("application/json", json_params.ToString(Newtonsoft.Json.Formatting.None), ParameterType.RequestBody);

                                                //Consumiendo Petición
                                                IRestResponse respuestaGps = clienteGps.Execute(peticionGps);
                                                string posiciones_gps = @"{ 'posiciones' : " + respuestaGps.Content + " }";
                                                JObject json_gps = JObject.Parse(posiciones_gps);
                                                if (json_gps != null)
                                                {
                                                    JArray pos = (JArray)json_gps["posiciones"];
                                                    if (pos != null)
                                                    {
                                                        //Obteniendo Datos de los Elementos
                                                        List<Tuple<double, double, string, DateTime, decimal, bool>> points =
                                                              (from JToken p in pos
                                                               where p["idUnit"].ToString().Equals(unidad.identificador_unidad)
                                                               select new Tuple<double, double, string, DateTime, decimal, bool>
                                                                    (Convert.ToDouble(p["lat"]),
                                                                     Convert.ToDouble(p["lng"]),
                                                                     p["address"].ToString(),
                                                                     Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                                     Convert.ToDecimal(p["speed"]),
                                                                     p["speed"].ToString().Equals("0.0") ? false : true)
                                                               ).ToList();

                                                        //Validando Lista
                                                        if (points != null)
                                                        {
                                                            if (points.Count > 0)
                                                            {
                                                                //Obteniendo Fecha Máxima
                                                                DateTime fec_max_p = points.Max(f => f.Item4);
                                                                //Obteniendo Punto Máximo
                                                                Tuple<double, double, string, DateTime, decimal, bool> max =
                                                                    (from Tuple<double, double, string, DateTime, decimal, bool> m in points
                                                                     where m.Item4 == fec_max_p
                                                                     select m).FirstOrDefault();

                                                                //Validando Punto Máximo
                                                                if (max != null)
                                                                {
                                                                    if (max != null)
                                                                    {
                                                                        //Asignando Datos de Retorno
                                                                        latitud = max.Item1;
                                                                        longitud = max.Item2;
                                                                        ubicacion = max.Item3;
                                                                        fecha_gps = max.Item4;
                                                                        velocidad = max.Item5;
                                                                        bit_encendido = max.Item6;

                                                                        //Obteniendo Datos
                                                                        retorno = new RetornoOperacion(1, respuestaToken.StatusCode.ToString() + " : " + respuestaToken.StatusDescription, true);
                                                                    }
                                                                    else
                                                                        retorno = new RetornoOperacion("No se pudieron recuperar el último punto GPS de la Unidad");
                                                                }
                                                                else
                                                                    retorno = new RetornoOperacion("No se pudieron recuperar el último punto GPS de la Unidad");
                                                            }
                                                            else
                                                                retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS por unidad");
                                                        }
                                                        else
                                                            retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS por unidad");
                                                    }
                                                    else
                                                        retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS");
                                                }
                                                else
                                                    retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS");
                                            }

                                            break;
                                        }
                                }
                                break;
                            }
                        default:
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("No existe la Configuración de GPS para esta Compania");
                            break;
                    }
                }
                else
                    retorno = new RetornoOperacion("La unidad no coincide con el Proveedor especificado");
            }
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("No existe la Configuración de GPS para esta Compania");

            //Devolviendo Resultado Obubicaciontenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de enviar al WS MTC Viaje
        /// </summary>
        /// <param name="id_servicio">Id servicio</param>
        /// <returns></returns>x
        public static RetornoOperacion GenerarViajeMTC(int id_servicio, int id_parada)
        {
            //Instanciando los parametros 
            using (DataTable dtViaje = SAT_CL.Documentacion.Reportes.CreaciondeViaje(id_servicio))
            {
                using (DataTable dtDespacho = SAT_CL.Documentacion.Reportes.CargaEventosParaDespacho(id_servicio, id_parada))
                {            
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();
                XDocument datosObtenidos = new XDocument();
                //Objeto de Parametros
                XDocument parametros = new XDocument();
                //Existen Parametros
                if (Validacion.ValidaOrigenDatos(dtViaje))
                {
                    List<DataRow> EnvioViaje = (from DataRow p in dtViaje.AsEnumerable()
                                                select p).ToList();
                    if (EnvioViaje.Count > 0)
                    {
                        foreach (DataRow dr in EnvioViaje)
                        {
                            //Instanciando Proveedor de WB Para Consumo
                            using (ProveedorWS ws = new ProveedorWS(Convert.ToInt32(dr["IdProvedor"])))
                            {
                                //Validando si existe el Registro
                                if (ws.habilitar)
                                {
                                    //Existen Parametros
                                    if (Validacion.ValidaOrigenDatos(dtDespacho))
                                    {
                                        List<DataRow> Despacho = (from DataRow p in dtDespacho.AsEnumerable()
                                                                    select p).ToList();
                                        if (Despacho.Count > 0)
                                        {
                                            foreach (DataRow ds in Despacho)
                                            {
                                                //Instanciando "DiccionarioUnidad","DiccionarioCliente","Proveedor WS","Unidad"
                                                //using (ProveedorWSDiccionario dico = new ProveedorWSDiccionario(Convert.ToInt32(dr["IdDiccionarioOperador"])))
                                                using (ProveedorWSDiccionario dicu = new ProveedorWSDiccionario(Convert.ToInt32(ds["IdDiccionarioUnidad"])))
                                                using (ProveedorWSDiccionario dicc = new ProveedorWSDiccionario(Convert.ToInt32(ds["IdDiccionarioCliente"])))
                                                using (Unidad unidad = new Unidad(Convert.ToInt32(ds["Unidad"])))
                                                {
                                                    //Validando si existe el Registro
                                                    if (dicu.habilitar)
                                                    {
                                                        //Validando si existe el Registro
                                                        if (dicc.habilitar)
                                                        {
                                                            //Validando si existe el Registro
                                                            if (unidad.habilitar)
                                                            {
                                                                //Validando que sea  la configuracion establecida para la compañia 
                                                                switch (ws.identificador)
                                                                {
                                                                    case "MTC - Despacho":
                                                                        {
                                                                            ////Declarando variable de retorno
                                                                            string Visitas = "";
                                                                            //string DistanciasTiempos = "";
                                                                            //decimal Distancias = 0;
                                                                            //decimal Tiempos = 0;
                                                                            string DistanciaTiempos = "";
                                                                            //Instanciando los parametros 
                                                                            using (DataTable dtViajeArreglo = SAT_CL.Documentacion.Reportes.CreacionArregloViaje(id_servicio))
                                                                            {
                                                                                if (Validacion.ValidaOrigenDatos(dtViaje))
                                                                                {
                                                                                    List<DataRow> ArregloViaje = (from DataRow p in dtViajeArreglo.AsEnumerable()
                                                                                                                    select p).ToList();
                                                                                    if (ArregloViaje.Count > 0)
                                                                                    {
                                                                                        foreach (DataRow dl in ArregloViaje)
                                                                                        {
                                                                                            Visitas = Visitas + @"""" + Convert.ToString(dl["IdOrigen"]) + @""",""" + Convert.ToString(dl["IdDestino"]) + @"""" + ",";
                                                                                            DistanciaTiempos = DistanciaTiempos + "["+ Convert.ToString(dl["DKms"]) +"," + Convert.ToString(dl["DTiempos"]) + "],"; 
                                                                                            //Distancias = Distancias + Convert.ToDecimal(Convert.ToString(dl["DKms"]));
                                                                                            //Tiempos = Tiempos + Convert.ToDecimal(Convert.ToString(dl["DTiempos"]));
                                                                                        }
                                                                                        Visitas = @"[" + Visitas + "]";
                                                                                        Visitas = Cadena.SustituyePatronCadena(Visitas, ",]", "]");
                                                                                            //DistanciasTiempos = @"[[" + Distancias + "," + Tiempos + "]]";
                                                                                        DistanciaTiempos = @"[" + DistanciaTiempos + "]";
                                                                                        DistanciaTiempos = Cadena.SustituyePatronCadena(DistanciaTiempos, ",]", "]");
                                                                                        

                                                                                    }
                                                                                }
                                                                            }
                                                                            //Instanciando Diccionario WB para compañia
                                                                            using (ProveedorWSDiccionario Diccionario = new ProveedorWSDiccionario(Convert.ToInt32(dr["IdDiccionarioCliente"])))
                                                                            {
                                                                                //Validando si existe el Registro
                                                                                if (Diccionario.habilitar)
                                                                                {
                                                                                    //Enviamos peteción MTC "MetodoGeneraViaje"
                                                                                    resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(ws.endpoin),
                                                                                    CreateSoapEnvelopeGenerarViaje(Convert.ToString(dr["Nombre"]), Convert.ToString(dr["Cliente"]), Convert.ToString(dr["Descripion"]), Convert.ToString(dr["NombreGeocercaTraslado"]),
                                                                                    Convert.ToInt32(dr["GenerarGeocercaTraslado"]), Convert.ToString(dr["Amplitud"]), Convert.ToInt32(dr["VelocidadPromedio"]), Convert.ToInt32(dr["VelocidadMaxima"]), Convert.ToInt32(dr["TiempoVelocidadMaximo"]),
                                                                                    Convert.ToInt32(dr["TiempoMaximoVisita"]), Convert.ToInt32(dr["TiempoMinimoVisita"]), Visitas, DistanciaTiempos, Convert.ToString(dr["Usuario"]), Convert.ToString(dr["Password"])));
                                                                                    //Validamos Solicitud exitosa
                                                                                    if (resultado.OperacionExitosa)
                                                                                    {
                                                                                        ////Obtenemos Documento generado
                                                                                        XDocument xDoc = XDocument.Parse(resultado.Mensaje);
                                                                                        //Validamos que exista Respuesta
                                                                                        if (xDoc != null)
                                                                                        {
                                                                                            //Obteniendo Error con cadena separada
                                                                                            string CadenaSeparada = Convert.ToString(xDoc) == "" ? Convert.ToString(xDoc) : Cadena.RegresaCadenaSeparada(Convert.ToString(xDoc), @"{""estatus"":""", 1);
                                                                                            string ERROR = CadenaSeparada == "" ? CadenaSeparada : Cadena.RegresaCadenaSeparada(CadenaSeparada, @"""", 0);
                                                                                            resultado = obtieneResultadoMTC(ERROR.ToString().Trim());
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        //Establecmos Mensaje Resultado
                                                                                        resultado = new RetornoOperacion("No es posible obtener la respuesta MTC");

                                                                                }
                                                                                else
                                                                                    //Establecmos Mensaje Resultado
                                                                                    resultado = new RetornoOperacion("El cliente no se encuentra registrado en nuestro diccionario");
                                                                            }
                                                                            break;
                                                                        }
                                                                    default:
                                                                        //Instanciando Excepción
                                                                        resultado = new RetornoOperacion("No existe la Configuración de WS para este Proveedor");
                                                                        break;
                                                                }
                                                            }
                                                            else
                                                                //Instanciando Excepción
                                                                resultado = new RetornoOperacion("La unidad se encuentra en estatus baja");
                                                        }
                                                        else
                                                            //Instanciando Excepción
                                                            resultado = new RetornoOperacion("El cliente no se encuentra registrada en el diccionario MTC");
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion("La unidad no se encuentra registrada en el diccionario MTC");
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion(true);
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El viaje es menor a cero");
                }
                else
                    resultado = new RetornoOperacion("No existen parametros");
                //Devolviendo Resultado Obtenido
                return resultado;
                }
            }
        }
        /// <summary>
        /// Método encargado de Obtener la Posición Actual de la Unidad
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="evento">Evento</param>
        /// <returns></returns>
        public static RetornoOperacion GenerarDespachoMTC(int id_servicio, int id_parada)
        {         
            //Instanciando los parametros 
            using (DataTable dtDespacho = SAT_CL.Documentacion.Reportes.CargaEventosParaDespacho(id_servicio, id_parada))
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();
                XDocument datosObtenidos = new XDocument();
                //Objeto de Parametros
                XDocument parametros = new XDocument();
                //Existen Parametros
                if (Validacion.ValidaOrigenDatos(dtDespacho))
                {
                    List<DataRow> Despacho = (from DataRow p in dtDespacho.AsEnumerable()
                                              select p).ToList();
                    if (Despacho.Count > 0)
                    {
                        foreach (DataRow dr in Despacho)
                        {
                            //Instanciando "DiccionarioUnidad","DiccionarioCliente","Proveedor WS","Unidad"
                            //using (ProveedorWSDiccionario dico = new ProveedorWSDiccionario(Convert.ToInt32(dr["IdDiccionarioOperador"])))
                            using (ProveedorWSDiccionario dicu = new ProveedorWSDiccionario(Convert.ToInt32(dr["IdDiccionarioUnidad"])))
                            using (ProveedorWSDiccionario dicc = new ProveedorWSDiccionario(Convert.ToInt32(dr["IdDiccionarioCliente"])))
                            using (ProveedorWS pro = new ProveedorWS(Convert.ToInt32(dr["IdProvedor"])))
                            using (Unidad unidad = new Unidad(Convert.ToInt32(dr["Unidad"])))
                            {
                                //Validando si existe el Registro
                                if (pro.habilitar)
                                {
                                    //Validando si existe el Registro
                                    if (dicu.habilitar)
                                    {
                                        //Validando si existe el Registro
                                        if (dicc.habilitar)
                                        {
                                            //Validando si existe el Registro
                                            if (unidad.habilitar)
                                            {
                                                //Validando que sea la configuracion establecida para la compañia 
                                                switch (pro.identificador)
                                                {
                                                    case "MTC - Despacho":
                                                        {                                                         
                                                        //Enviamos peteción MTC "MetodoGenerarDespacho"
                                                        resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pro.endpoin),
                                                        CreateSoapEnvelopeDespacho(Convert.ToString(dr["FolioDespacho"]), Convert.ToString(dr["Fecha"]), Convert.ToString(dr["Alias"]), Convert.ToInt32(dr["IdOperador"]), Convert.ToInt32(dr["TipoInicio"]), Convert.ToInt32(dr["TiempoEspera"]), Convert.ToString(dr["NombreCliente"]),
                                                        Convert.ToInt32(dr["IdRuta"]), Convert.ToString(dr["IdRutaExterno"]), Convert.ToString(dr["FoliosExternos"]), Convert.ToString(dr["IDExternoOperador"]), Convert.ToString(dr["Usuario"]), Convert.ToString(dr["password"])));

                                                        //Validamos Solicitud exitosa
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            ////Obtenemos Documento generado
                                                            XDocument xDoc = XDocument.Parse(resultado.Mensaje);
                                                            //Validamos que exista Respuesta
                                                            if (xDoc != null)
                                                            {
                                                                //Obteniendo Error con cadena separada
                                                                string CadenaSeparada = Convert.ToString(xDoc) == "" ? Convert.ToString(xDoc) : Cadena.RegresaCadenaSeparada(Convert.ToString(xDoc), @"<estatus>", 1);
                                                                string ERROR = CadenaSeparada == "" ? CadenaSeparada : Cadena.RegresaCadenaSeparada(CadenaSeparada, @"</estatus>", 0);
                                                                resultado = obtieneResultadoMTC(ERROR.ToString().Trim());
                                                            }
                                                        }
                                                        else
                                                            //Establecmos Mensaje Resultado
                                                            resultado = new RetornoOperacion("No es posible obtener la respuesta MTC");
                                                    break;
                                                              
                                                }
                                                    default:
                                                    //Instanciando Excepción
                                                    resultado = new RetornoOperacion("No existe la Configuración de WS para este Proveedor");
                                                    break;

                                                }                                                   
                                            }                           
                                            else
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La unidad se encuentra en estatus baja");
                                        }
                                        else
                                            //Instanciando Excepción
                                            resultado = new RetornoOperacion("El cliente no se encuentra registrada en el diccionario MTC");
                                    }
                                    else
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La unidad no se encuentra registrada en el diccionario MTC");
                                }
                                else
                                    //Instanciando Excepción
                                    resultado = new RetornoOperacion(true);
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("El viaje es menor a cero");
                }
                else 
                resultado = new RetornoOperacion("No existen parametros");
                //Devolviendo Resultado Obtenido
                return resultado;
            }   
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para generar viaje
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="Cliente"></param>
        /// <param name="Descripion"></param>
        /// <param name="NombreGeocercaTraslado"></param>
        /// <param name="GenerarGeocercaTraslado"></param>
        /// <param name="Amplitud"></param>
        /// <param name="VelocidadPromedio"></param>
        /// <param name="velocidadMaxima"></param>
        /// <param name="TiempoVelocidadMaximo"></param>
        /// <param name="TiempoMaximoVisita"></param>
        /// <param name="TiempoMinimoVisita"></param>
        /// <param name="DistanciasTiempos"></param>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeGenerarViaje(string Nombre, string Cliente, string Descripion, string NombreGeocercaTraslado, int GenerarGeocercaTraslado,
            string Amplitud, int VelocidadPromedio, int VelocidadMaxima, int TiempoVelocidadMaximo, int TiempoMaximoVisita, int TiempoMinimoVisita, string Visitas, string DistanciasTiempos, string usuario, string contrasena)
        {
            ////Declaramos Variable para Armar Soap
            string xmlEnvioViaje = @"{""nombre"":""" + Nombre + @""",""cliente"":""" + Cliente + @""",""descripion"":""" + Descripion + @""",""nombreGeocercaTraslado"":""" + NombreGeocercaTraslado + @""",""generarGeocercaTraslado"":" + GenerarGeocercaTraslado +
                 @",""amplitud"":" + Amplitud + @",""velocidadPromedio"":" + VelocidadPromedio + @",""velocidadMaxima"":" + VelocidadMaxima + @",""tiempoVelocidadMaxima"":" + TiempoVelocidadMaximo + @",""tiempoMaximoVisita"":" + TiempoMaximoVisita +
                 @",""tiempoMinimoVisita"":" + TiempoMinimoVisita + @", ""visitas"":" + Visitas + @",""distanciasTiempos"":" + DistanciasTiempos + @"}";
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:des=""http://design.org"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <des:generarViaje>
                                             <!--Optional:-->
                                             <des:usuario></des:usuario>
                                             <!--Optional:-->
                                             <des:password></des:password>
                                            <!--Optional:-->
                                            <des:viaje></des:viaje>
                                          </des:generarViaje>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</des:usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</des:password>"), contrasena);
            //Insertamos Contenido
            sb.Insert(sb.ToString().IndexOf("</des:viaje"), @"<![CDATA[" + xmlEnvioViaje.ToString() + "]]>");
            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());
            ////Obteniendo bytes del archivo XML
            //byte[] bytes = Encoding.Default.GetBytes(soapEnvelopeXml.BaseUri);
            return soapEnvelopeXml;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para generar despacho
        /// </summary>
        /// <param name="folioDespacho">Folio despacho</param>
        /// <param name="FechaEnvio">Fecha Envio</param>
        /// <param name="Alias">Alias</param>
        /// <param name="IdOperador">Id operador</param>
        /// <param name="TipoInicio">Tipo Inicio</param>
        /// <param name="TiempoEspera">Tiempo Espera</param>
        /// <param name="NCliente">Nombre de Cliente</param>
        /// <param name="IdRuta">Id Ruta</param>
        /// <param name="IdRutaExterno">Id Ruta Externo</param>
        /// <param name="FoliosExternos">Folios Externos</param>
        /// <param name="IDExternoOperador">Id Externos Operador</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        protected static XDocument CreateSoapEnvelopeDespacho(string folioDespacho, string FechaEnvio,string Alias, int IdOperador, int TipoInicio, int TiempoEspera, string NCliente, int IdRuta, string IdRutaExterno, string FoliosExternos, string IDExternoOperador, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:des=""http://design.org"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <des:generarDespacho>                                          
                                             <des:usuario></des:usuario>
                                             <des:password></des:password>
                                           <des:folioDespacho>" + folioDespacho + @"</des:folioDespacho>
                                            <!--Optional:-->                                            <des:fechaHora>" + Convert.ToDateTime(FechaEnvio).ToString("HH:mm:ss dd/MM/yyyy") + @"</des:fechaHora>                                             <!--Optional:-->                                            <des:alias>" + Alias + @"</des:alias>                                             <!--Optional:-->                                            <des:IDOperador>" + IdOperador + @"</des:IDOperador>                                             <!--Optional:-->                                            <des:tipoInicio>" + TipoInicio + @"</des:tipoInicio>                                             <!--Optional:-->                                            <des:tiempoEspera>" + TiempoEspera + @"</des:tiempoEspera>                                             <!--Optional:-->                                            <des:nombreCliente>" + NCliente + @"</des:nombreCliente>                                             <!--Optional:-->                                            <des:IDRuta>" + IdRuta + @"</des:IDRuta>                                             <!--Optional:-->                                            <des:IDRutaExterno>" + IdRutaExterno + @"</des:IDRutaExterno>                                            <!--Optional:-->                                            <des:foliosExternos>" + FoliosExternos + @"</des:foliosExternos>                                           <!--Optional:-->                                           <des:IDExternoOperador>" + IDExternoOperador + @"</des:IDExternoOperador>
                                          </des:generarDespacho>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</des:usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</des:password>"), contrasena);
            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Metodo de Crear el Mensaje Soap para buscar despacho
        /// </summary>
        /// <param name="folioDespacho">Folio despacho</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        public static XDocument CreateSoapEnvelopeBuscarDespacho(string folioDespacho, string usuario, string contrasena)
        {
            //Declaramos Variable para Armar Soap
            string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:des=""http://design.org"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <des:buscarDespachoFolio>                                          
                                             <des:usuario></des:usuario>
                                             <des:password></des:password>
                                             <des:folioExterno>" + "" + @"</des:folioExterno>
                                           <des:folioDespacho>" + folioDespacho + @"</des:folioDespacho>
                                          </des:buscarDespachoFolio>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";
            //Declaramos String Buider
            StringBuilder sb = new StringBuilder(_soapEnvelope);
            //Insertamos Usuario
            sb.Insert(sb.ToString().IndexOf("</des:usuario>"), usuario);
            //Insertamos Contraseña
            sb.Insert(sb.ToString().IndexOf("</des:password>"), contrasena);
            //Creamos soap envelope en xml
            XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

            return soapEnvelopeXml;
        }
        /// <summary>
        /// Genera un resultado interpretable para el desarrollador a partir del resultado del Web Service de MTC
        /// </summary>
        /// <param name="codigo_resultado">Código de resultado de MTC</param>
        /// <returns></returns>
        protected static RetornoOperacion obtieneResultadoMTC(string codigo_resultado)
        {
            //Definiendo objeto de reultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Determinando el resultado a partir del código del SAT
            switch (codigo_resultado)
            {
                case "OK":
                    resultado = new RetornoOperacion("Operación finalizada correctamente.", true);
                    break;
                case "ERROR_WSMTC_100":
                    resultado = new RetornoOperacion("El usuario y la contraseña no coinciden con la plataforma MTC.");
                    break;
                case "ERROR_WSMTC:101":
                    resultado = new RetornoOperacion("El folio no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:102":
                    resultado = new RetornoOperacion("La fecha no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:103":
                    resultado = new RetornoOperacion("La serie no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:104":
                    resultado = new RetornoOperacion("El número no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:105":
                    resultado = new RetornoOperacion("El alias no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:106":
                    resultado = new RetornoOperacion("El operador no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:107":
                    resultado = new RetornoOperacion("El tipo de inicio no es válido.");
                    break;
                case "ERROR_WSMTC:108":
                    resultado = new RetornoOperacion("El tiempo de espera no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:109":
                    resultado = new RetornoOperacion("El cliente no tiene el formato correcto. ");
                    break;
                case "ERROR_WSMTC:110":
                    resultado = new RetornoOperacion("La ruta no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:111":
                    resultado = new RetornoOperacion("Fecha de inicio no válida.");
                    break;
                case "ERROR_WSMTC:112":
                    resultado = new RetornoOperacion("El viaje no existe.");
                    break;
                case "ERROR_WSMTC:113":
                    resultado = new RetornoOperacion("El cliente no existe.");
                    break;
                case "ERROR_WSMTC:114":
                    resultado = new RetornoOperacion("El operador no existe.");
                    break;
                case "ERROR_WSMTC:115":
                    resultado = new RetornoOperacion("El operador no corresponde al cliente ingresado");
                    break;
                case "ERROR_WSMTC:116":
                    resultado = new RetornoOperacion("El equipo no existe.");
                    break;
                case "ERROR_WSMTC:117":
                    resultado = new RetornoOperacion("El vehículo no existe.");
                    break;
                case "ERROR_WSMTC:118":
                    resultado = new RetornoOperacion("El número económico no corresponde al vehículo.");
                    break;
                case "ERROR_WSMTC:119":
                    resultado = new RetornoOperacion("El vehículo no está asignado a la cuenta.");
                    break;
                case "ERROR_WSMTC:120":
                    resultado = new RetornoOperacion("La unidad ya ha sido asignada a un despacho.");
                    break;
                case "ERROR_WSMTC:121":
                    resultado = new RetornoOperacion("El folio ya existe.");
                    break;
                case "ERROR_WSMTC:122":
                    resultado = new RetornoOperacion("El tiempo de espera no tiene el formato correcto.");
                    break;
                case "ERROR_WSMTC:123":
                    resultado = new RetornoOperacion("Ocurrió un error al insertar el despacho.");
                    break;
                case "ERROR_WSMTC:124":
                    resultado = new RetornoOperacion("Ocurrió un error en el proceso.");
                    break;
                case "ERROR_WSMTC:125":
                    resultado = new RetornoOperacion("El vehículo y el equipo no pertenecen a la misma unidad.");
                    break;
                case "ERROR_WSMTC:126":
                    resultado = new RetornoOperacion("No se proporcionó un identificador para el viaje.");
                    break;
                //ERRORES DOCUMENTO 2.0
                case "ERROR_WSMTC:001":
                    resultado = new RetornoOperacion("Usuario inválido.");
                    break;
                case "ERROR_WSMTC:002":
                    resultado = new RetornoOperacion("Password Inválido");
                    break;
                case "ERROR_WSMTC:003":
                    resultado = new RetornoOperacion("No se encontró la cuenta con base al usuario y password dado.");
                    break;
                case "ERROR_WSMTC:004":
                    resultado = new RetornoOperacion("La cuenta ha expirado. Contacte a su administrador.");
                    break;
                case "ERROR_WSMTC:005":
                    resultado = new RetornoOperacion("El perfil asociado a la cuenta, no tiene acceso a la interfaz Web.");
                    break;
                case "ERROR_WSMTC:006":
                    resultado = new RetornoOperacion("(Descripción de error general).");
                    break;
                case "ERROR_WSMTC:007":
                    resultado = new RetornoOperacion("Operación Inválida.");
                    break;
                case "ERROR_WSMTC:008":
                    resultado = new RetornoOperacion("Alias del vehículo inválido.");
                    break;
                case "ERROR_WSMTC:009":
                    resultado = new RetornoOperacion("Nombre del Cliente inválido.");
                    break;
                case "ERROR_WSMTC:010":
                    resultado = new RetornoOperacion("Número Económico inválido.");
                    break;
                case "ERROR_WSMTC:011":
                    resultado = new RetornoOperacion("Placa Inválida.");
                    break;
                case "ERROR_WSMTC:012":
                    resultado = new RetornoOperacion("Marca Inválida.");
                    break;
                case "ERROR_WSMTC:013":
                    resultado = new RetornoOperacion("Modelo Inválido.");
                    break;
                case "ERROR_WSMTC:014":
                    resultado = new RetornoOperacion("El alias del vehículo ya existe.");
                    break;
                case "ERROR_WSMTC:015":
                    resultado = new RetornoOperacion("El cliente no existe.");
                    break;
                case "ERROR_WSMTC:016":
                    resultado = new RetornoOperacion("El vehículo se encuentra asignado a una unidad activa.");
                    break;
                case "ERROR_WSMTC:017":
                    resultado = new RetornoOperacion("El vehículo correspondiente al alias no existe.");
                    break;
                case "ERROR_WSMTC:018":
                    resultado = new RetornoOperacion("Folio de despacho inválido.");
                    break;
                case "ERROR_WSMTC:019":
                    resultado = new RetornoOperacion("Fecha y hora inválida.");
                    break;
                case "ERROR_WSMTC:020":
                    resultado = new RetornoOperacion("Operador inválido.");
                    break;
                case "ERROR_WSMTC:021":
                    resultado = new RetornoOperacion("Tipo de inicio inválido.");
                    break;
                case "ERROR_WSMTC:022":
                    resultado = new RetornoOperacion("Tiempo de espera inválido.");
                    break;
                case "ERROR_WSMTC:023":
                    resultado = new RetornoOperacion("Cliente inválido.");
                    break;
                case "ERROR_WSMTC:024":
                    resultado = new RetornoOperacion("El identificador externo del viaje es inválido.");
                    break;
                case "ERROR_WSMTC:025":
                    resultado = new RetornoOperacion("Folios externos inválidos.");
                    break;
                case "ERROR_WSMTC:026":
                    resultado = new RetornoOperacion("No se proporcionó un identificador para el viaje.");
                    break;
                case "ERROR_WSMTC:027":
                    resultado = new RetornoOperacion("Viaje no encontrado.");
                    break;
                case "ERROR_WSMTC:028":
                    resultado = new RetornoOperacion("Vehículo no asignado a unidad.");
                    break;
                case "ERROR_WSMTC:029":
                    resultado = new RetornoOperacion("Unidad en despacho activo.");
                    break;
                case "ERROR_WSMTC:030":
                    resultado = new RetornoOperacion("El identificador externo del viaje ya existe.");
                    break;
                case "ERROR_WSMTC:031":
                    resultado = new RetornoOperacion("Uno de los folios externos ya existe: (folio repetido).");
                    break;
                case "ERROR_WSMTC:032":
                    resultado = new RetornoOperacion("El folio del despacho ya existe.");
                    break;
                case "ERROR_WSMTC:033":
                    resultado = new RetornoOperacion("Viaje inválido.");
                    break;
                case "ERROR_WSMTC:034":
                    resultado = new RetornoOperacion("Año invalido.");
                    break;
                case "ERROR_WSMTC:035":
                    resultado = new RetornoOperacion("El viaje tiene un despacho asociado.");
                    break;
                case "ERROR_WSMTC:036":
                    resultado = new RetornoOperacion("No se logró cargar la configuración del archivo Mastrace.ini.");
                    break;
                case "ERROR_WSMTC:037":
                    resultado = new RetornoOperacion("Fecha y hora de inicio inválida.");
                    break;
                case "ERROR_WSMTC:038":
                    resultado = new RetornoOperacion(" Fecha y hora de fin inválida.");
                    break;
                case "ERROR_WSMTC:039":
                    resultado = new RetornoOperacion("Nombre de cliente inválido..");
                    break;
                case "ERROR_WSMTC:040":
                    resultado = new RetornoOperacion("Folio externo inválido.");
                    break;
                case "ERROR_WSMTC:041":
                    resultado = new RetornoOperacion("La cuenta no tiene los permisos requeridos.");
                    break;
                case "ERROR_WSMTC:042":
                    resultado = new RetornoOperacion("Estatus del despacho inválido.");
                    break;
                case "ERROR_WSMTC:043":
                    resultado = new RetornoOperacion("Lista de unidades inválida.");
                    break;
                case "ERROR_WSMTC:044":
                    resultado = new RetornoOperacion("Folio despacho inválido.");
                    break;
                case "ERROR_WSMTC:045":
                    resultado = new RetornoOperacion("Folio despacho y Folio externo inválidos.");
                    break;
                case "ERROR_WSMTC:046":
                    resultado = new RetornoOperacion("La unidad no se encuentra habilitada.");
                    break;
                //Complemento ERRORES DOCUMENTO 2.0
                case "ERROR_WSMTC:047":
                    resultado = new RetornoOperacion("Folios del despacho inválidos.");
                    break;
                case "ERROR_WSMTC:048":
                    resultado = new RetornoOperacion("Parámetros de configuración inválidos.");
                    break;
                case "ERROR_WSMTC:049":
                    resultado = new RetornoOperacion("Tipo de vista inválido.");
                    break;
                case "ERROR_WSMTC:050":
                    resultado = new RetornoOperacion("Fuente inválida.");
                    break;
                case "ERROR_WSMTC:051":
                    resultado = new RetornoOperacion("Frecuencia inválida.");
                    break;
                case "ERROR_WSMTC:052":
                    resultado = new RetornoOperacion("Clasificación de geocerca 1 inválida.");
                    break;
                case "ERROR_WSMTC:053":
                    resultado = new RetornoOperacion("Clasificación de geocerca 2 inválida.");
                    break;
                case "ERROR_WSMTC:054":
                    resultado = new RetornoOperacion("Clasificación de geocerca 3 inválida.");
                    break;
                case "ERROR_WSMTC:055":
                    resultado = new RetornoOperacion("Clasificación de geocerca 4 inválida.");
                    break;
                case "ERROR_WSMTC:056":
                    resultado = new RetornoOperacion("La cuenta aún no está completamente configurada.");
                    break;
                case "ERROR_WSMTC:057":
                    resultado = new RetornoOperacion("Cuenta a modificar inválida.");
                    break;
                case "ERROR_WSMTC:058":
                    resultado = new RetornoOperacion("Nombre de geocerca inválido.");
                    break;
                case "ERROR_WSMTC:059":
                    resultado = new RetornoOperacion("Cliente inválido.");
                    break;
                case "ERROR_WSMTC:060":
                    resultado = new RetornoOperacion("Descripción inválida.");
                    break;
                case "ERROR_WSMTC:061":
                    resultado = new RetornoOperacion("Coordenadas inválidas.");
                    break;
                case "ERROR_WSMTC:062":
                    resultado = new RetornoOperacion("Radio inválido.");
                    break;
                case "ERROR_WSMTC:063":
                    resultado = new RetornoOperacion("Zona inválida.");
                    break;
                case "ERROR_WSMTC:064":
                    resultado = new RetornoOperacion("Categoría inválida.");
                    break;
                case "ERROR_WSMTC:065":
                    resultado = new RetornoOperacion("Clasificación inválida.");
                    break;
                case "ERROR_WSMTC:066":
                    resultado = new RetornoOperacion("Nombre de contacto inválido.");
                    break;
                case "ERROR_WSMTC:067":
                    resultado = new RetornoOperacion("Email de contacto inválido.");
                    break;
                case "ERROR_WSMTC:068":
                    resultado = new RetornoOperacion("Teléfono de contacto inválido.");
                    break;
                case "ERROR_WSMTC:069":
                    resultado = new RetornoOperacion("Ya existe una geocerca con el nombre especificado.");
                    break;
                case "ERROR_WSMTC:070":
                    resultado = new RetornoOperacion("Lista de geocercas inválida.");
                    break;
                case "ERROR_WSMTC:071":
                    resultado = new RetornoOperacion("Parámetro viaje inválido.");
                    break;
                case "ERROR_WSMTC:072":
                    resultado = new RetornoOperacion("Nombre de viaje inválido.");
                    break;
                case "ERROR_WSMTC:073":
                    resultado = new RetornoOperacion("Descripción inválida.");
                    break;
                case "ERROR_WSMTC:074":
                    resultado = new RetornoOperacion("Parámetro GenerarGeocercaTraslado inválido.");
                    break;
                case "ERROR_WSMTC:075":
                    resultado = new RetornoOperacion("Geocerca de Traslado inválida.");
                    break;
                case "ERROR_WSMTC:076":
                    resultado = new RetornoOperacion("Amplitud de geocerca de traslado inválido.");
                    break;
                case "ERROR_WSMTC:077":
                    resultado = new RetornoOperacion("Velocidad máxima inválida.");
                    break;
                case "ERROR_WSMTC:078":
                    resultado = new RetornoOperacion("Tiempo en velocidad máxima inválido.");
                    break;
                case "ERROR_WSMTC:079":
                    resultado = new RetornoOperacion("Tiempo máximo en visita inválido.");
                    break;
                case "ERROR_WSMTC:080":
                    resultado = new RetornoOperacion("Tiempo mínimo en visita inválido.");
                    break;
                case "ERROR_WSMTC:081":
                    resultado = new RetornoOperacion("Visitas inválidas.");
                    break;
                case "ERROR_WSMTC:082":
                    resultado = new RetornoOperacion("Distancias y Tiempos de visita inválidos.");
                    break;
                case "ERROR_WSMTC:083":
                    resultado = new RetornoOperacion("Se excedió el número de visitas permitidas.");
                    break;
                case "ERROR_WSMTC:084":
                    resultado = new RetornoOperacion("Se requiere un valor de key de Google Maps API.");
                    break;
                case "ERROR_WSMTC:085":
                    resultado = new RetornoOperacion("Ya existe un viaje con el nombre especificado.");
                    break;
                case "ERROR_WSMTC:086":
                    resultado = new RetornoOperacion("El tiempo máximo en visita debe ser mayor al tiempo mínimo en visita.");
                    break;          
                default:
                    resultado = new RetornoOperacion("Error no identificado.");
                    break;
            }
            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Evaluar las Unidades de GPS
        /// </summary>
        /// <param name="proveedor_ws"></param>
        /// <param name="unidades"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static List<RetornoOperacion> EvaluaUnidadesGPS(List<int> proveedor_ws, List<Tuple<int, int, int, int, int, int>> unidades, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            List<RetornoOperacion> listaRetornos = new List<RetornoOperacion>();
            /** Definición de "unidades" **/
            // Item1 - IdProveedorUnidadWS
            // Item2 - IdProveedorWS
            // Item3 - IdServicio
            // Item4 - IdParada
            // Item5 - IdMovimiento
            // Item6 - idParadaDestino

            //Inicializando Variables de Salida
            int id_evaluacion = 0;
            SqlGeography ubicacion_gps_unidad = SqlGeography.Null;

            if (proveedor_ws.Count > 0)
            {
                if (unidades.Count > 0)
                {
                    //Recorriendo Proveedores
                    foreach (int id_proveedor_ws in proveedor_ws)
                    {
                        using (ProveedorWS prov = new ProveedorWS(id_proveedor_ws))
                        {
                            if (prov.habilitar)
                            {
                                //Obteniendo Unidades del Proveedor WS
                                List<Tuple<int, int, int, int, int, int>> units = (from Tuple<int, int, int, int, int, int> unidad in unidades
                                                                                   where unidad.Item2 == id_proveedor_ws
                                                                                   select unidad).ToList();
                                //Inicializando Variables de Retorno
                                DateTime fecha_peticion = Fecha.ObtieneFechaEstandarMexicoCentro();
                                double latitud, longitud;
                                DateTime fecha_gps;
                                string ubicacion;
                                decimal velocidad, cantidad_combustible;
                                bool bit_encendido;
                                ubicacion = ""; latitud = longitud = 0; fecha_gps = DateTime.MinValue;
                                velocidad = 0.00M; bit_encendido = false; cantidad_combustible = 0.00M;
                                if (units.Count > 0)
                                {
                                    //Validando Atributo de Método General
                                    if (prov.obtienePosicionesGeneral)
                                    {
                                        switch (prov.identificador)
                                        {
                                            case "TEM - IVehicleWebService":
                                            case "TECTOS - IVehicleWebService":
                                                {
                                                    //Validando Punto Final
                                                    switch (prov.accion)
                                                    {
                                                        case "getVehiclesByClient":
                                                            {
                                                                //Objeto de Parametros
                                                                XDocument parametros = new XDocument();
                                                                XDocument datosObtenidos = new XDocument();
                                                                

                                                                //Obteniendo Parametros
                                                                using (DataTable dtParams = ProveedorWSCargaParametros.ObtieneParametrosProveedorWS(prov.id_proveedor_ws))
                                                                {
                                                                    //Existen Parametros
                                                                    if (Validacion.ValidaOrigenDatos(dtParams))
                                                                    {
                                                                        //Copiando Tabla
                                                                        DataTable dt = dtParams.Copy();
                                                                        dt.TableName = "request";
                                                                        //Creando flujo de memoria
                                                                        using (System.IO.Stream s = new System.IO.MemoryStream())
                                                                        {
                                                                            //Leyendo flujo de datos XML
                                                                            dt.WriteXml(s);
                                                                            //Obteniendo DataSet en XML
                                                                            XElement dataTableElement = XElement.Parse(System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s)));
                                                                            //Añadiendo Elemento al nodo de Publicaciones
                                                                            parametros = new XDocument(dataTableElement.Element("request"));
                                                                        }
                                                                    }
                                                                }

                                                                //Creando Ensobretado SOAP
                                                                XDocument ensobretado = XDocument.Parse(
                                                                            @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:sei='http://sei.rest.services.client.maya.omnitracs.mx/'>
                                                                                <soapenv:Header/>
                                                                                <soapenv:Body>
                                                                                    <sei:" + prov.accion + @">
                                                                                        " + parametros.ToString() + @"
                                                                                    </sei:" + prov.accion + @">
                                                                                </soapenv:Body>
                                                                            </soapenv:Envelope>");//*/

                                                                //Consumiendo Servicio Web de Proveedor
                                                                retorno = prov.ConsumeProveedorWS(ensobretado, out datosObtenidos);

                                                                //Validando Operación
                                                                if (retorno.OperacionExitosa)
                                                                {
                                                                    //Creando NameSpace's
                                                                    XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
                                                                    XNamespace ns1 = @"http://sei.rest.services.client.maya.omnitracs.mx/";

                                                                    //Obteniendo Elemento Deseado
                                                                    XDocument elemento = new XDocument(datosObtenidos.Root.Element(ns + "Body").Element(ns1 + prov.accion + "Response").Element("return"));

                                                                    //Convirtiendo XML a Tabla
                                                                    DataSet ds = new DataSet();
                                                                    ds.ReadXml(new XmlTextReader(new StringReader(elemento.ToString())));

                                                                    //Validando Origen de Datos
                                                                    if (Validacion.ValidaOrigenDatos(ds))
                                                                    {
                                                                        foreach (Tuple<int, int, int, int, int, int> idUnit in units)
                                                                        {
                                                                            using (ProveedorWSUnidad pu = new ProveedorWSUnidad(idUnit.Item1))
                                                                            using (Unidad u = new Unidad(pu.id_unidad))
                                                                            {
                                                                                //Declarando Variables Auxiliares
                                                                                string[] serialNumbers = new string[1];
                                                                                int count = 0;
                                                                                DateTime fec_ws = DateTime.MinValue;

                                                                                //Obteniendo Identificadores
                                                                                using (DataTable dtIndentificadores = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtieneProveedorUnidad(u.id_unidad))
                                                                                {
                                                                                    //Validando que existan
                                                                                    if (Validacion.ValidaOrigenDatos(dtIndentificadores))
                                                                                    {
                                                                                        //Creando Arreglo Dinamico
                                                                                        serialNumbers = new string[dtIndentificadores.Rows.Count];
                                                                                        //Recorriendo Filas
                                                                                        foreach (DataRow dr in dtIndentificadores.Rows)
                                                                                        {
                                                                                            //Asignando Identificador
                                                                                            serialNumbers[count] = dr["Identificador"].ToString();
                                                                                            //Incrementando Contador
                                                                                            count++;
                                                                                        }

                                                                                        //Obteniendo Fila
                                                                                        IEnumerable<DataRow> rows = (from DataRow r in ds.Tables["vehicleDevice"].Rows
                                                                                                                     where serialNumbers.Contains(r.Field<string>("serialnumber"))
                                                                                                                     select r);
                                                                                        //Asignando Valores
                                                                                        if (rows.Count() > 0)
                                                                                        {
                                                                                            //Instanciando Resultado Positivo
                                                                                            retorno = new RetornoOperacion(0);

                                                                                            //Recorriendo Ciclo
                                                                                            foreach (DataRow dr in rows)
                                                                                            {
                                                                                                //Asignando valores
                                                                                                latitud = Convert.ToDouble(dr["latitude"]);
                                                                                                longitud = Convert.ToDouble(dr["longitude"]);
                                                                                                ubicacion = dr["location"].ToString();
                                                                                                DateTime.TryParse(dr["lastGPSDate"].ToString(), out fecha_gps);
                                                                                                velocidad = Convert.ToDecimal(dr["speed"]);
                                                                                                bit_encendido = Convert.ToBoolean(dr["ignition"].ToString().Equals("") ? "false" : dr["ignition"].ToString());
                                                                                                int tiempo_excedido = 0;

                                                                                                //Validando que haya una Respuesta
                                                                                                if (retorno.OperacionExitosa)
                                                                                                {
                                                                                                    //Inicializando Bloque Transaccional
                                                                                                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                                                    {
                                                                                                        //Declarando Variables Auxiliares
                                                                                                        Monitoreo.EvaluacionBitacora.ResultadoBitacora result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok;
                                                                                                        int id_destino = 0;
                                                                                                        int distancia = 0, duracion = 0, id_bitacora = 0;
                                                                                                        //Posición Unidad Sistema
                                                                                                        ubicacion_gps_unidad = SqlGeography.Point(Convert.ToDouble(dr["latitude"]), Convert.ToDouble(dr["longitude"]), 4326);
                                                                                                        /** Definición de "unidades" **/
                                                                                                        // Item1 - IdProveedorUnidadWS
                                                                                                        // Item2 - IdProveedorWS
                                                                                                        // Item3 - IdServicio
                                                                                                        // Item4 - IdParada
                                                                                                        // Item5 - IdMovimiento
                                                                                                        // Item6 - idParadaDestino//Insertando Bitacora de Monitoreo
                                                                                                        retorno = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.AntenaGPS,
                                                                                                                    6, idUnit.Item3, idUnit.Item4, SAT_CL.Despacho.ParadaEvento.ObtienerPrimerEvento(idUnit.Item4), idUnit.Item5, 19, u.id_unidad,
                                                                                                                    ubicacion_gps_unidad, dr["location"].ToString(), "Petición de Ubicación GPS", fecha_gps, Convert.ToDecimal(dr["speed"]),
                                                                                                                    Convert.ToBoolean(dr["ignition"].ToString().Equals("") ? "false" : dr["ignition"].ToString()), id_usuario);

                                                                                                        //Validando que haya una Respuesta
                                                                                                        if (retorno.OperacionExitosa)
                                                                                                        {
                                                                                                            //Guardando Bitacora
                                                                                                            id_bitacora = retorno.IdRegistro;
                                                                                                            //Tiempo Tolerancia
                                                                                                            int tolerancia = bit_encendido ? pu.tiempo_encendido : pu.tiempo_apagado;
                                                                                                            //Calculando Hora Tolerada
                                                                                                            DateTime fecha_gps_tolerada = fecha_gps.AddMinutes(tolerancia);

                                                                                                            //Validando Comparación
                                                                                                            switch (fecha_gps_tolerada.CompareTo(fecha_peticion))
                                                                                                            {
                                                                                                                //Si es Mayor
                                                                                                                case 1:
                                                                                                                //Si es Igual
                                                                                                                case 0:
                                                                                                                    {
                                                                                                                        //Asignando Tiempo Excedido
                                                                                                                        tiempo_excedido = 0;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                //Si es Menor
                                                                                                                case -1:
                                                                                                                    //Obteniendo Tiempo Excedido
                                                                                                                    TimeSpan tiempo_ex = fecha_peticion - fecha_gps;
                                                                                                                    //Asignando Tiempo Excedido
                                                                                                                    tiempo_excedido = (int)(tiempo_ex.TotalMinutes < 0 ? tiempo_ex.TotalMinutes * -1 : tiempo_ex.TotalMinutes) - tolerancia;
                                                                                                                    break;
                                                                                                            }

                                                                                                            //Validando Estatus de la Unidad
                                                                                                            switch (u.EstatusUnidad)
                                                                                                            {
                                                                                                                case Unidad.Estatus.ParadaDisponible:
                                                                                                                case Unidad.Estatus.ParadaOcupado:
                                                                                                                    {
                                                                                                                        //Instanciando Estancia, Parada y Ubicación
                                                                                                                        using (SAT_CL.Despacho.EstanciaUnidad estancia = new SAT_CL.Despacho.EstanciaUnidad(u.id_estancia))
                                                                                                                        using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(estancia.id_parada))
                                                                                                                        using (Ubicacion ubi = new Ubicacion(stop.id_ubicacion))
                                                                                                                        {
                                                                                                                            //Validando que existan Registros
                                                                                                                            if (estancia.habilitar && stop.habilitar && ubi.habilitar)
                                                                                                                            {
                                                                                                                                //Validando que exista la Ubicación
                                                                                                                                if (ubi.geoubicacion != SqlGeography.Null)
                                                                                                                                {
                                                                                                                                    //Obtiene Distancia Permitida
                                                                                                                                    int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                                                                                    //Obteniendo Distancia por Ubicación
                                                                                                                                    using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubi.id_ubicacion, 15,
                                                                                                                                                                    SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                                                                                                            0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                                                                                    {
                                                                                                                                        //Validando que exista la Referencia
                                                                                                                                        if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                                                                                        {
                                                                                                                                            //Recorriendo Registro
                                                                                                                                            foreach (DataRow drs in dtDistancia.Rows)
                                                                                                                                                //Obteniendo Distancia Permitida
                                                                                                                                                distancia_permitida = Convert.ToInt32(drs["Valor"]);
                                                                                                                                        }
                                                                                                                                    }

                                                                                                                                    //Validando Tipo de Geometria
                                                                                                                                    switch (ubi.geoubicacion.STGeometryType().Value)
                                                                                                                                    {
                                                                                                                                        case "Point":
                                                                                                                                            {
                                                                                                                                                //Validando que el Punto no exceda mas de 10 metros
                                                                                                                                                if (!DatosEspaciales.ValidaDistanciaPermitida(ubi.geoubicacion, ubicacion_gps_unidad, distancia_permitida))
                                                                                                                                                    //Instanciando Resultado Positivo
                                                                                                                                                    result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                                                                                                //Instanciando Excepción
                                                                                                                                                retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case "LineString":
                                                                                                                                        case "CompoundCurve":
                                                                                                                                        case "Polygon":
                                                                                                                                        case "CurvePolygon":
                                                                                                                                            {
                                                                                                                                                //Obtiene Punto mas Cercano
                                                                                                                                                SqlGeography punto_cercano = ubi.geoubicacion.STBuffer(distancia_permitida);

                                                                                                                                                //Validando que exista el Punto mas Cercano
                                                                                                                                                if (punto_cercano != SqlGeography.Null)
                                                                                                                                                {
                                                                                                                                                    //Validando que haya Intersección en las columnas
                                                                                                                                                    if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))

                                                                                                                                                        //Instanciando Resultado Positivo
                                                                                                                                                        result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    retorno = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                    //Instanciando Excepción
                                                                                                                                    retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                            }
                                                                                                                            else
                                                                                                                                //Instanciando Excepción
                                                                                                                                retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                        }
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case Unidad.Estatus.Transito:
                                                                                                                    {
                                                                                                                        //Instanciando Movimiento y Parada Destino
                                                                                                                        using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(u.id_movimiento))
                                                                                                                        using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(mov.id_parada_destino))
                                                                                                                        using (Ubicacion ubi = new Ubicacion(stop.id_ubicacion))
                                                                                                                        {
                                                                                                                            //Validando que existan Registros
                                                                                                                            if (mov.habilitar && stop.habilitar && ubi.habilitar)
                                                                                                                            {
                                                                                                                                //Asignando Ubicación
                                                                                                                                id_destino = stop.id_parada;

                                                                                                                                //Validando si la Unidad esta en Movimiento
                                                                                                                                if (velocidad > 0)

                                                                                                                                    //Instanciando Resultado Correcto
                                                                                                                                    retorno = new RetornoOperacion(0, "", true);
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    //Validando que exista la Ubicación
                                                                                                                                    if (ubi.geoubicacion != SqlGeography.Null)
                                                                                                                                    {
                                                                                                                                        //Obtiene Distancia Permitida
                                                                                                                                        int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                                                                                        //Obteniendo Distancia por Ubicación
                                                                                                                                        using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubi.id_ubicacion, 15,
                                                                                                                                                                        SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                                                                                        {
                                                                                                                                            //Validando que exista la Referencia
                                                                                                                                            if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                                                                                            {
                                                                                                                                                //Recorriendo Registro
                                                                                                                                                foreach (DataRow drs in dtDistancia.Rows)
                                                                                                                                                    //Obteniendo Distancia Permitida
                                                                                                                                                    distancia_permitida = Convert.ToInt32(drs["Valor"]);
                                                                                                                                            }
                                                                                                                                        }

                                                                                                                                        //Validando Tipo de Geometria
                                                                                                                                        switch (ubi.geoubicacion.STGeometryType().Value)
                                                                                                                                        {
                                                                                                                                            case "Point":
                                                                                                                                                {
                                                                                                                                                    //Validando que el Punto no exceda mas de 10 metros
                                                                                                                                                    if (!DatosEspaciales.ValidaDistanciaPermitida(ubi.geoubicacion, ubicacion_gps_unidad, distancia_permitida))
                                                                                                                                                        //Instanciando Resultado Positivo
                                                                                                                                                        result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;
                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                    break;
                                                                                                                                                }
                                                                                                                                            case "LineString":
                                                                                                                                            case "CompoundCurve":
                                                                                                                                            case "Polygon":
                                                                                                                                            case "CurvePolygon":
                                                                                                                                                {
                                                                                                                                                    //Obtiene Punto mas Cercano
                                                                                                                                                    SqlGeography punto_cercano = ubi.geoubicacion.STBuffer(distancia_permitida);

                                                                                                                                                    //Validando que exista el Punto mas Cercano
                                                                                                                                                    if (punto_cercano != SqlGeography.Null)
                                                                                                                                                    {
                                                                                                                                                        //Validando que haya Intersección en las columnas
                                                                                                                                                        if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))
                                                                                                                                                            //Instanciando Resultado Positivo
                                                                                                                                                            result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        retorno = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                                                                                                    break;
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                        //Instanciando Excepción
                                                                                                                                        retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                        break;
                                                                                                                    }
                                                                                                            }

                                                                                                            //Validando Resultado Posicionamiento
                                                                                                            if (retorno.OperacionExitosa)
                                                                                                            {
                                                                                                                //Instanciando Parada de Destino
                                                                                                                using (SAT_CL.Despacho.Parada destino = new SAT_CL.Despacho.Parada(id_destino))
                                                                                                                using (Ubicacion dest = new Ubicacion(destino.id_ubicacion))
                                                                                                                {
                                                                                                                    //Validando que exista el Destino
                                                                                                                    if (destino.habilitar && dest.habilitar)
                                                                                                                    {
                                                                                                                        //Obtiene Distancia Permitida
                                                                                                                        SqlGeography d_point = SqlGeography.Null;
                                                                                                                        //Validando Tipo de Geometria
                                                                                                                        switch (dest.geoubicacion.STGeometryType().Value)
                                                                                                                        {
                                                                                                                            case "LineString":
                                                                                                                            case "CompoundCurve":
                                                                                                                            case "Polygon":
                                                                                                                            case "CurvePolygon":
                                                                                                                                {
                                                                                                                                    d_point = dest.geoubicacion.EnvelopeCenter();
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case "Point":
                                                                                                                                {
                                                                                                                                    d_point = dest.geoubicacion;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            default:
                                                                                                                                {
                                                                                                                                    d_point = SqlGeography.Null;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                        }

                                                                                                                        if (d_point != SqlGeography.Null && !(d_point.Lat == 0 && d_point.Long == 0))
                                                                                                                        {
                                                                                                                            //Obteniendo Datos API
                                                                                                                            retorno = DistanceMatrix.objDistanceMatrix.ObtieneDistanciaOrigenDestinoXML(ubicacion_gps_unidad, dest.geoubicacion,
                                                                                                                                                                                DistanceMatrix.Unidades.Metric, out distancia, out duracion);
                                                                                                                            //Validando Resultado Posicionamiento
                                                                                                                            if (retorno.OperacionExitosa)
                                                                                                                            {
                                                                                                                                //Validando si no hay errores
                                                                                                                                if (result == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && u.EstatusUnidad == SAT_CL.Global.Unidad.Estatus.Transito)
                                                                                                                                    //Obteniendo Resultado de Cercania
                                                                                                                                    result = Monitoreo.EvaluacionBitacora.ObtieneCercaniaEvaluacion(id_bitacora, distancia, fecha_peticion);
                                                                                                                            }
                                                                                                                        }
                                                                                                                        else
                                                                                                                            result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;
                                                                                                                    }

                                                                                                                    //Validando Resultado Posicionamiento
                                                                                                                    if (retorno.OperacionExitosa)
                                                                                                                    {
                                                                                                                        //Validando Estatus de Evaluación
                                                                                                                        if (result == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && tiempo_excedido > 0)

                                                                                                                            //Asignando Resultado
                                                                                                                            result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;

                                                                                                                        //Insertando Evaluación de la Bitacora
                                                                                                                        retorno = SAT_CL.Monitoreo.EvaluacionBitacora.InsertaEvaluacionBitacora(id_bitacora, fecha_peticion, result, tiempo_excedido, distancia,
                                                                                                                                duracion, dest.habilitar ? fecha_peticion.AddSeconds((double)duracion) : DateTime.MinValue, destino.cita_parada, id_usuario);

                                                                                                                        //Validando Resultado Posicionamiento
                                                                                                                        if (retorno.OperacionExitosa)
                                                                                                                        {
                                                                                                                            //Asignando Evaluación
                                                                                                                            id_evaluacion = retorno.IdRegistro;
                                                                                                                            //Instanciando Unidad
                                                                                                                            retorno = new RetornoOperacion(u.id_unidad);
                                                                                                                            //Completando Transacción
                                                                                                                            trans.Complete();
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                            //Instanciando Excepción
                                                                                            retorno = new RetornoOperacion("No se encontraron Unidades con la(s) Antena(s) proporcionada(s): '" + string.Join(",", serialNumbers) + "'");
                                                                                    }
                                                                                    else
                                                                                        //Instanciando Excepción
                                                                                        retorno = new RetornoOperacion("No existen Identificadores por Filtrar");
                                                                                }
                                                                            }

                                                                            //Añadiendo a Lista General
                                                                            listaRetornos.Add(retorno);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    listaRetornos.Add(retorno);
                                                                break;
                                                            }
                                                    }
                                                    break;
                                                }
                                            case "GROCHA - ADS":
                                                {
                                                    //Validando Punto Final
                                                    switch (prov.accion)
                                                    {
                                                        case "/data/getLastPosition":
                                                            {
                                                                //Declarando Objeto de Retorno
                                                                retorno = new RetornoOperacion(prov.id_proveedor_ws);
                                                                string token_auth = @"", tipo_auth = @"", fec_peticion = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("yyyy-MM-dd");

                                                                //Obteniendo URL de Autenticación (TOKEN)
                                                                string url_token = @"" + prov.endpoin + "/oauth/token",
                                                                       url_gps = @"" + prov.endpoin + prov.accion;
                                                                RestClient clienteToken = new RestClient(url_token),
                                                                           clienteGps = new RestClient(url_gps);

                                                                //Configurando Datos del Cliente y de la Petición
                                                                RestRequest peticionToken = new RestRequest(Method.POST),
                                                                            peticionGps = new RestRequest(Method.POST);
                                                                clienteToken.Timeout = -1;
                                                                peticionToken.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(prov.usuario + ":" + prov.contraseña)));
                                                                peticionToken.AddHeader("Content-Type", "multipart/form-data");
                                                                peticionToken.AlwaysMultipartFormData = true;

                                                                //Obteniendo y Asignando Parametros
                                                                string grant_type = "", username = "", password = "";
                                                                using (DataTable dtParams = ProveedorWSParametros.ObtieneParametrosProveedorWS(prov.id_proveedor_ws))
                                                                {
                                                                    if (Validacion.ValidaOrigenDatos(dtParams))
                                                                    {
                                                                        //Obteniendo GrantType
                                                                        grant_type = (from DataRow p in dtParams.Rows
                                                                                      where p["NombreParametro"].ToString().Equals("grant_type")
                                                                                      select p["ValorParametro"].ToString()).FirstOrDefault();
                                                                        //Obteniendo User Name
                                                                        username = (from DataRow p in dtParams.Rows
                                                                                    where p["NombreParametro"].ToString().Equals("username")
                                                                                    select p["ValorParametro"].ToString()).FirstOrDefault();
                                                                        //Obteniendo Password
                                                                        password = (from DataRow p in dtParams.Rows
                                                                                    where p["NombreParametro"].ToString().Equals("password")
                                                                                    select p["ValorParametro"].ToString()).FirstOrDefault();
                                                                    }
                                                                }

                                                                if (!string.IsNullOrEmpty(grant_type) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                                                                {
                                                                    //Añadiendo Parametros
                                                                    peticionToken.AddParameter("grant_type", grant_type);
                                                                    peticionToken.AddParameter("username", username);
                                                                    peticionToken.AddParameter("password", password);
                                                                }

                                                                //Consumiendo Petición
                                                                IRestResponse respuestaToken = clienteToken.Execute(peticionToken);
                                                                JObject json_token = JObject.Parse(respuestaToken.Content);
                                                                if (json_token != null)
                                                                {
                                                                    //Obteniendo Token de Autorización
                                                                    token_auth = (string)json_token["access_token"];
                                                                    tipo_auth = (string)json_token["token_type"];

                                                                    //Configurando Petición GPS
                                                                    clienteGps.Timeout = -1;
                                                                    peticionGps.AddHeader("Content-Type", "application/json");
                                                                    peticionGps.AddHeader("Authorization", tipo_auth + " " + token_auth);

                                                                    //Parametros de Busqueda
                                                                    string gps_params = @"{ 'fecha' : '" + fec_peticion + "', 'idUnit' : '0' }";
                                                                    JObject json_params = JObject.Parse(gps_params);
                                                                    peticionGps.AddParameter("application/json", json_params.ToString(Newtonsoft.Json.Formatting.None), ParameterType.RequestBody);

                                                                    //Consumiendo Petición
                                                                    IRestResponse respuestaGps = clienteGps.Execute(peticionGps);
                                                                    string posiciones_gps = @"{ 'posiciones' : " + respuestaGps.Content + " }";
                                                                    JObject json_gps = JObject.Parse(posiciones_gps);
                                                                    if (json_gps != null)
                                                                    {
                                                                        JArray pos = (JArray)json_gps["posiciones"];
                                                                        if (pos != null)
                                                                        {
                                                                            foreach (Tuple<int, int, int, int, int, int> idUnit in units)
                                                                            {
                                                                                using (ProveedorWSUnidad pu = new ProveedorWSUnidad(idUnit.Item1))
                                                                                using (Unidad u = new Unidad(pu.id_unidad))
                                                                                {
                                                                                    //Obteniendo Datos de los Elementos
                                                                                    List<Tuple<double, double, string, DateTime, decimal, bool>> points =
                                                                                      (from JToken p in pos
                                                                                       where p["idUnit"].ToString().Equals(pu.identificador_unidad)
                                                                                       select new Tuple<double, double, string, DateTime, decimal, bool>
                                                                                            (Convert.ToDouble(p["lat"]),
                                                                                             Convert.ToDouble(p["lng"]),
                                                                                             p["address"].ToString(),
                                                                                             Convert.ToDateTime(p["datePos"].ToString() + " " + p["hourPos"].ToString()),
                                                                                             Convert.ToDecimal(p["speed"]),
                                                                                             p["speed"].ToString().Equals("0.0") ? false : true)
                                                                                       ).ToList();
                                                                                    
                                                                                    //Validando Lista
                                                                                    if (points != null)
                                                                                    {
                                                                                        if (points.Count > 0)
                                                                                        {
                                                                                            //Obteniendo Fecha Máxima
                                                                                            DateTime fec_max_p = points.Max(f => f.Item4);
                                                                                            //Obteniendo Punto Máximo
                                                                                            Tuple<double, double, string, DateTime, decimal, bool> max =
                                                                                                (from Tuple<double, double, string, DateTime, decimal, bool> m in points
                                                                                                 where m.Item4 == fec_max_p
                                                                                                 select m).FirstOrDefault();

                                                                                            //Validando Punto Máximo
                                                                                            if (max != null)
                                                                                            {
                                                                                                if (max != null)
                                                                                                {
                                                                                                    //Asignando Datos de Retorno
                                                                                                    latitud = max.Item1;
                                                                                                    longitud = max.Item2;
                                                                                                    ubicacion = max.Item3;
                                                                                                    fecha_gps = max.Item4;
                                                                                                    velocidad = max.Item5;
                                                                                                    bit_encendido = max.Item6;
                                                                                                    int tiempo_excedido = 0;

                                                                                                    //Validando que haya una Respuesta
                                                                                                    if (retorno.OperacionExitosa)
                                                                                                    {
                                                                                                        //Inicializando Bloque Transaccional
                                                                                                        using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                                                        {
                                                                                                            //Declarando Variables Auxiliares
                                                                                                            Monitoreo.EvaluacionBitacora.ResultadoBitacora result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok;
                                                                                                            int id_destino = 0;
                                                                                                            int distancia = 0, duracion = 0, id_bitacora = 0;
                                                                                                            //Posición Unidad Sistema
                                                                                                            ubicacion_gps_unidad = SqlGeography.Point(latitud, longitud, 4326);
                                                                                                            /** Definición de "unidades" **/
                                                                                                            // Item1 - IdProveedorUnidadWS
                                                                                                            // Item2 - IdProveedorWS
                                                                                                            // Item3 - IdServicio
                                                                                                            // Item4 - IdParada
                                                                                                            // Item5 - IdMovimiento
                                                                                                            // Item6 - idParadaDestino//Insertando Bitacora de Monitoreo
                                                                                                            retorno = SAT_CL.Monitoreo.BitacoraMonitoreo.InsertaBitacoraMonitoreo(SAT_CL.Monitoreo.BitacoraMonitoreo.OrigenBitacoraMonitoreo.AntenaGPS,
                                                                                                                        6, idUnit.Item3, idUnit.Item4, SAT_CL.Despacho.ParadaEvento.ObtienerPrimerEvento(idUnit.Item4), idUnit.Item5, 19, u.id_unidad,
                                                                                                                        ubicacion_gps_unidad, ubicacion, "Petición de Ubicación GPS", fecha_gps, velocidad,
                                                                                                                        bit_encendido, id_usuario);

                                                                                                            //Validando que haya una Respuesta
                                                                                                            if (retorno.OperacionExitosa)
                                                                                                            {
                                                                                                                //Guardando Bitacora
                                                                                                                id_bitacora = retorno.IdRegistro;
                                                                                                                //Tiempo Tolerancia
                                                                                                                int tolerancia = bit_encendido ? pu.tiempo_encendido : pu.tiempo_apagado;
                                                                                                                //Calculando Hora Tolerada
                                                                                                                DateTime fecha_gps_tolerada = fecha_gps.AddMinutes(tolerancia);

                                                                                                                //Validando Comparación
                                                                                                                switch (fecha_gps_tolerada.CompareTo(fecha_peticion))
                                                                                                                {
                                                                                                                    //Si es Mayor
                                                                                                                    case 1:
                                                                                                                    //Si es Igual
                                                                                                                    case 0:
                                                                                                                        {
                                                                                                                            //Asignando Tiempo Excedido
                                                                                                                            tiempo_excedido = 0;
                                                                                                                            break;
                                                                                                                        }
                                                                                                                    //Si es Menor
                                                                                                                    case -1:
                                                                                                                        //Obteniendo Tiempo Excedido
                                                                                                                        TimeSpan tiempo_ex = fecha_peticion - fecha_gps;
                                                                                                                        //Asignando Tiempo Excedido
                                                                                                                        tiempo_excedido = (int)(tiempo_ex.TotalMinutes < 0 ? tiempo_ex.TotalMinutes * -1 : tiempo_ex.TotalMinutes) - tolerancia;
                                                                                                                        break;
                                                                                                                }

                                                                                                                //Validando Estatus de la Unidad
                                                                                                                switch (u.EstatusUnidad)
                                                                                                                {
                                                                                                                    case Unidad.Estatus.ParadaDisponible:
                                                                                                                    case Unidad.Estatus.ParadaOcupado:
                                                                                                                        {
                                                                                                                            //Instanciando Estancia, Parada y Ubicación
                                                                                                                            using (SAT_CL.Despacho.EstanciaUnidad estancia = new SAT_CL.Despacho.EstanciaUnidad(u.id_estancia))
                                                                                                                            using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(estancia.id_parada))
                                                                                                                            using (Ubicacion ubi = new Ubicacion(stop.id_ubicacion))
                                                                                                                            {
                                                                                                                                //Validando que existan Registros
                                                                                                                                if (estancia.habilitar && stop.habilitar && ubi.habilitar)
                                                                                                                                {
                                                                                                                                    //Validando que exista la Ubicación
                                                                                                                                    if (ubi.geoubicacion != SqlGeography.Null)
                                                                                                                                    {
                                                                                                                                        //Obtiene Distancia Permitida
                                                                                                                                        int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                                                                                        //Obteniendo Distancia por Ubicación
                                                                                                                                        using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubi.id_ubicacion, 15,
                                                                                                                                                                        SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(
                                                                                                                                                                                0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                                                                                        {
                                                                                                                                            //Validando que exista la Referencia
                                                                                                                                            if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                                                                                            {
                                                                                                                                                //Recorriendo Registro
                                                                                                                                                foreach (DataRow drs in dtDistancia.Rows)
                                                                                                                                                    //Obteniendo Distancia Permitida
                                                                                                                                                    distancia_permitida = Convert.ToInt32(drs["Valor"]);
                                                                                                                                            }
                                                                                                                                        }

                                                                                                                                        //Validando Tipo de Geometria
                                                                                                                                        switch (ubi.geoubicacion.STGeometryType().Value)
                                                                                                                                        {
                                                                                                                                            case "Point":
                                                                                                                                                {
                                                                                                                                                    //Validando que el Punto no exceda mas de 10 metros
                                                                                                                                                    if (!DatosEspaciales.ValidaDistanciaPermitida(ubi.geoubicacion, ubicacion_gps_unidad, distancia_permitida))
                                                                                                                                                        //Instanciando Resultado Positivo
                                                                                                                                                        result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                                                                                                    //Instanciando Excepción
                                                                                                                                                    retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                    break;
                                                                                                                                                }
                                                                                                                                            case "LineString":
                                                                                                                                            case "CompoundCurve":
                                                                                                                                            case "Polygon":
                                                                                                                                            case "CurvePolygon":
                                                                                                                                                {
                                                                                                                                                    //Obtiene Punto mas Cercano
                                                                                                                                                    SqlGeography punto_cercano = ubi.geoubicacion.STBuffer(distancia_permitida);

                                                                                                                                                    //Validando que exista el Punto mas Cercano
                                                                                                                                                    if (punto_cercano != SqlGeography.Null)
                                                                                                                                                    {
                                                                                                                                                        //Validando que haya Intersección en las columnas
                                                                                                                                                        if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))

                                                                                                                                                            //Instanciando Resultado Positivo
                                                                                                                                                            result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente;

                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        retorno = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                                                                                                    break;
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                        //Instanciando Excepción
                                                                                                                                        retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                                }
                                                                                                                                else
                                                                                                                                    //Instanciando Excepción
                                                                                                                                    retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                            }
                                                                                                                            break;
                                                                                                                        }
                                                                                                                    case Unidad.Estatus.Transito:
                                                                                                                        {
                                                                                                                            //Instanciando Movimiento y Parada Destino
                                                                                                                            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(u.id_movimiento))
                                                                                                                            using (SAT_CL.Despacho.Parada stop = new SAT_CL.Despacho.Parada(mov.id_parada_destino))
                                                                                                                            using (Ubicacion ubi = new Ubicacion(stop.id_ubicacion))
                                                                                                                            {
                                                                                                                                //Validando que existan Registros
                                                                                                                                if (mov.habilitar && stop.habilitar && ubi.habilitar)
                                                                                                                                {
                                                                                                                                    //Asignando Ubicación
                                                                                                                                    id_destino = stop.id_parada;

                                                                                                                                    //Validando si la Unidad esta en Movimiento
                                                                                                                                    if (velocidad > 0)

                                                                                                                                        //Instanciando Resultado Correcto
                                                                                                                                        retorno = new RetornoOperacion(0, "", true);
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        //Validando que exista la Ubicación
                                                                                                                                        if (ubi.geoubicacion != SqlGeography.Null)
                                                                                                                                        {
                                                                                                                                            //Obtiene Distancia Permitida
                                                                                                                                            int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                                                                                            //Obteniendo Distancia por Ubicación
                                                                                                                                            using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubi.id_ubicacion, 15,
                                                                                                                                                                            SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                                                                                            {
                                                                                                                                                //Validando que exista la Referencia
                                                                                                                                                if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                                                                                                {
                                                                                                                                                    //Recorriendo Registro
                                                                                                                                                    foreach (DataRow drs in dtDistancia.Rows)
                                                                                                                                                        //Obteniendo Distancia Permitida
                                                                                                                                                        distancia_permitida = Convert.ToInt32(drs["Valor"]);
                                                                                                                                                }
                                                                                                                                            }

                                                                                                                                            //Validando Tipo de Geometria
                                                                                                                                            switch (ubi.geoubicacion.STGeometryType().Value)
                                                                                                                                            {
                                                                                                                                                case "Point":
                                                                                                                                                    {
                                                                                                                                                        //Validando que el Punto no exceda mas de 10 metros
                                                                                                                                                        if (!DatosEspaciales.ValidaDistanciaPermitida(ubi.geoubicacion, ubicacion_gps_unidad, distancia_permitida))
                                                                                                                                                            //Instanciando Resultado Positivo
                                                                                                                                                            result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;
                                                                                                                                                        //Instanciando Excepción
                                                                                                                                                        retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                        break;
                                                                                                                                                    }
                                                                                                                                                case "LineString":
                                                                                                                                                case "CompoundCurve":
                                                                                                                                                case "Polygon":
                                                                                                                                                case "CurvePolygon":
                                                                                                                                                    {
                                                                                                                                                        //Obtiene Punto mas Cercano
                                                                                                                                                        SqlGeography punto_cercano = ubi.geoubicacion.STBuffer(distancia_permitida);

                                                                                                                                                        //Validando que exista el Punto mas Cercano
                                                                                                                                                        if (punto_cercano != SqlGeography.Null)
                                                                                                                                                        {
                                                                                                                                                            //Validando que haya Intersección en las columnas
                                                                                                                                                            if (!DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, ubicacion_gps_unidad))
                                                                                                                                                                //Instanciando Resultado Positivo
                                                                                                                                                                result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida;
                                                                                                                                                            //Instanciando Excepción
                                                                                                                                                            retorno = new RetornoOperacion(0, "La Unidad no se encuentra en la Ubicación", true);
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                            //Instanciando Excepción
                                                                                                                                                            retorno = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                                                                                                        break;
                                                                                                                                                    }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                            //Instanciando Excepción
                                                                                                                                            retorno = new RetornoOperacion("No se puede encontrar la Ubicación de la Unidad");
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                            break;
                                                                                                                        }
                                                                                                                }

                                                                                                                //Validando Resultado Posicionamiento
                                                                                                                if (retorno.OperacionExitosa)
                                                                                                                {
                                                                                                                    //Instanciando Parada de Destino
                                                                                                                    using (SAT_CL.Despacho.Parada destino = new SAT_CL.Despacho.Parada(id_destino))
                                                                                                                    using (Ubicacion dest = new Ubicacion(destino.id_ubicacion))
                                                                                                                    {
                                                                                                                        //Validando que exista el Destino
                                                                                                                        if (destino.habilitar && dest.habilitar)
                                                                                                                        {
                                                                                                                            //Obtiene Distancia Permitida
                                                                                                                            SqlGeography d_point = SqlGeography.Null;
                                                                                                                            //Validando Tipo de Geometria
                                                                                                                            switch (dest.geoubicacion.STGeometryType().Value)
                                                                                                                            {
                                                                                                                                case "LineString":
                                                                                                                                case "CompoundCurve":
                                                                                                                                case "Polygon":
                                                                                                                                case "CurvePolygon":
                                                                                                                                    {
                                                                                                                                        d_point = dest.geoubicacion.EnvelopeCenter();
                                                                                                                                        break;
                                                                                                                                    }
                                                                                                                                case "Point":
                                                                                                                                    {
                                                                                                                                        d_point = dest.geoubicacion;
                                                                                                                                        break;
                                                                                                                                    }
                                                                                                                                default:
                                                                                                                                    {
                                                                                                                                        d_point = SqlGeography.Null;
                                                                                                                                        break;
                                                                                                                                    }
                                                                                                                            }

                                                                                                                            if (d_point != SqlGeography.Null && !(d_point.Lat == 0 && d_point.Long == 0))
                                                                                                                            {
                                                                                                                                //Obteniendo Datos API
                                                                                                                                retorno = DistanceMatrix.objDistanceMatrix.ObtieneDistanciaOrigenDestinoXML(ubicacion_gps_unidad, dest.geoubicacion,
                                                                                                                                                                                    DistanceMatrix.Unidades.Metric, out distancia, out duracion);
                                                                                                                                //Validando Resultado Posicionamiento
                                                                                                                                if (retorno.OperacionExitosa)
                                                                                                                                {
                                                                                                                                    //Validando si no hay errores
                                                                                                                                    if (result == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && u.EstatusUnidad == SAT_CL.Global.Unidad.Estatus.Transito)
                                                                                                                                        //Obteniendo Resultado de Cercania
                                                                                                                                        result = Monitoreo.EvaluacionBitacora.ObtieneCercaniaEvaluacion(id_bitacora, distancia, fecha_peticion);
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                                result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;
                                                                                                                        }

                                                                                                                        //Validando Resultado Posicionamiento
                                                                                                                        if (retorno.OperacionExitosa)
                                                                                                                        {
                                                                                                                            //Validando Estatus de Evaluación
                                                                                                                            if (result == Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok && tiempo_excedido > 0)

                                                                                                                                //Asignando Resultado
                                                                                                                                result = Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido;

                                                                                                                            //Insertando Evaluación de la Bitacora
                                                                                                                            retorno = SAT_CL.Monitoreo.EvaluacionBitacora.InsertaEvaluacionBitacora(id_bitacora, fecha_peticion, result, tiempo_excedido, distancia,
                                                                                                                                    duracion, dest.habilitar ? fecha_peticion.AddSeconds((double)duracion) : DateTime.MinValue, destino.cita_parada, id_usuario);

                                                                                                                            //Validando Resultado Posicionamiento
                                                                                                                            if (retorno.OperacionExitosa)
                                                                                                                            {
                                                                                                                                //Asignando Evaluación
                                                                                                                                id_evaluacion = retorno.IdRegistro;
                                                                                                                                //Instanciando Unidad
                                                                                                                                retorno = new RetornoOperacion(u.id_unidad, string.Format("La Unidad '{0}' ha posicionado exitósamente.", u.numero_unidad), true);
                                                                                                                                //Completando Transacción
                                                                                                                                trans.Complete();
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                    retorno = new RetornoOperacion("No se pudieron recuperar el último punto GPS de la Unidad");
                                                                                            }
                                                                                            else
                                                                                                retorno = new RetornoOperacion("No se pudieron recuperar el último punto GPS de la Unidad");
                                                                                        }
                                                                                        else
                                                                                            retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS por unidad");
                                                                                    }
                                                                                    else
                                                                                        retorno = new RetornoOperacion("No se pudieron recuperar los puntos GPS por unidad");
                                                                                }
                                                                                //Añadiendo a Lista General
                                                                                listaRetornos.Add(retorno);
                                                                            }
                                                                        }
                                                                        else
                                                                            listaRetornos.Add(new RetornoOperacion("No se pudieron recuperar los puntos GPS"));
                                                                    }
                                                                    else
                                                                        listaRetornos.Add(new RetornoOperacion("No se pudieron recuperar los puntos GPS"));
                                                                }
                                                                break;
                                                            }
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        foreach (Tuple<int, int, int, int, int, int> idUnit in units)
                                        {
                                            /** Definición de "unidades" **/
                                            // Item1 - IdProveedorUnidadWS
                                            // Item2 - IdProveedorWS
                                            // Item3 - IdServicio
                                            // Item4 - IdParada
                                            // Item5 - IdMovimiento
                                            // Item6 - idParadaDestino
                                            using (ProveedorWSUnidad pu = new ProveedorWSUnidad(idUnit.Item1))
                                            using (Unidad u = new Unidad(pu.id_unidad))
                                            {
                                                retorno = u.EvaluaUnidadGPS(idUnit.Item1, idUnit.Item3, idUnit.Item4, idUnit.Item5,
                                                                idUnit.Item6, out ubicacion_gps_unidad, out id_evaluacion, id_usuario);
                                                //Añadiendo a Lista General
                                                listaRetornos.Add(retorno);
                                            }
                                        }
                                    }
                                }
                                else
                                    listaRetornos.Add(new RetornoOperacion("No hay unidades para ese Proveedor de GPS"));
                            }
                            else
                                listaRetornos.Add(new RetornoOperacion("El Proveedor así como clarita, es inválido"));
                        }
                    }
                }
                else
                    listaRetornos.Add(new RetornoOperacion("No existen unidades por evaluar"));
            }
            else
                listaRetornos.Add(new RetornoOperacion("No existen proveedores de GPS"));

            //Devolviendo Lista de Resultados
            return listaRetornos;
        }


        #endregion
    }
}
