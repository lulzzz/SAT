using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas la operaciones de la Clasificación
    /// </summary>
    public class Clasificacion: Disposable
    {
        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_clasificacion_tc";


        private int _id_clasificacion;
        /// <summary>
        /// Describe el Id de la Clasificación
        /// </summary>
        public int id_clasificacion
        {
            get { return _id_clasificacion; }
        }
        private int _id_tabla;
        /// <summary>
        /// Describe el Id de la Tabla
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Describe el Id del Registro
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private int _id_tipo;
        /// <summary>
        /// Describe el  Tipo
        /// </summary>
        public int id_tipo
        {
            get { return _id_tipo; }
        }
        private int _id_flota;
        /// <summary>
        /// Describe la Flota
        /// </summary>
        public int id_flota
        {
            get { return _id_flota; }
        }
        private int _id_region;
        /// <summary>
        /// Describe el Id de la Region
        /// </summary>
        public int id_region
        {
            get { return _id_region; }
        }
        private int _id_ubicacion_terminal;
        /// <summary>
        /// Describe el Id de la Ubicación
        /// </summary>
        public int id_ubicacion_terminal
        {
            get { return _id_ubicacion_terminal; }
        }
        private int _id_tipo_servicio;
        /// <summary>
        /// Describe el Tipo de Servicio
        /// </summary>
        public int id_tipo_servicio
        {
            get { return _id_tipo_servicio; }
        }
        private int _id_alcance_servicio;
        /// <summary>
        /// Describe el Id Alcance de Servicio
        /// </summary>
        public int id_alcance_servicio
        {
            get { return _id_alcance_servicio; }
        }
        private int _id_detalle_negocio;
        /// <summary>
        /// Describe el Detalle de Negocio
        /// </summary>
        public int id_detalle_negocio
        {
            get { return _id_detalle_negocio; }
        }
        private int _id_clasificacion1;
        /// <summary>
        /// Describe la clasificación 1
        /// </summary>
        public int id_clasificacion1
        {
            get { return _id_clasificacion1; }
        }
        private int _id_clasificacion2;
        /// <summary>
        /// Describe la clasificación 2
        /// </summary>
        public int id_clasificacion2
        {
            get { return _id_clasificacion2; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe Habilitar
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
        ~Clasificacion()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public Clasificacion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }

        /// <summary>
        /// Genera una Instancia Clasificación dado un Id
        /// </summary>
        /// <param name="id_clasificacion"></param>
        public Clasificacion(int id_clasificacion)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_clasificacion);            
        }

        /// <summary>
        /// Genera una Instancia Clasificación dado un Id ligada a una transacción
        /// </summary>
        /// <param name="id_clasificacion"></param>
        /// <param name="transaccion"></param>
        public Clasificacion(int id_clasificacion, SqlTransaction transaccion)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_clasificacion, transaccion);
        }

        /// <summary>
        /// Genera una Instancia Clasificación dado un IdTabla, IdRegistro, IdTipo
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        public Clasificacion(int id_tabla, int id_registro, int id_tipo)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_tabla, id_registro, id_tipo);
        }

        #endregion

        #region Metodos privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            _id_clasificacion = 0;
            _id_tabla = 0;
            _id_registro = 0;
            _id_tipo = 0;
            _id_flota = 0;
            _id_region = 0;
            _id_ubicacion_terminal = 0;
            _id_tipo_servicio = 0;
            _id_alcance_servicio = 0;
            _id_detalle_negocio = 0;
            _id_clasificacion1 = 0;
            _id_clasificacion2 = 0;
            _habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_clasificacion">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_clasificacion)
        {
            bool result = false;

            //inicializamos el arreglo de parametros
            object[] param = { 3, id_clasificacion, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_clasificacion = Convert.ToInt32(r["Id"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _id_tipo = Convert.ToInt32(r["IdTipo"]);
                        _id_flota = Convert.ToInt32(r["IdFlota"]);
                        _id_region = Convert.ToInt32(r["IdRegion"]);
                        _id_ubicacion_terminal = Convert.ToInt32(r["IdUbicacionTerminal"]);
                        _id_tipo_servicio = Convert.ToByte(r["IdTipoSevicio"]);
                        _id_alcance_servicio = Convert.ToInt32(r["IdAlcanceServicio"]);
                        _id_detalle_negocio = Convert.ToInt32(r["IdDetalleNegocio"]);
                        _id_clasificacion1 = Convert.ToInt32(r["IdClasificacion1"]);
                        _id_clasificacion2 = Convert.ToInt32(r["IdClasificacion2"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro con Transacción SQL
        /// </summary>
        /// <param name="id_clasificacion">Id de Registro</param>
        /// <param name="transaccion">Transacción SQL</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_clasificacion, SqlTransaction transaccion)
        {
            bool result = false;

            //inicializamos el arreglo de parametros
            object[] param = { 3, id_clasificacion, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param, transaccion))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_clasificacion = Convert.ToInt32(r["Id"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _id_tipo = Convert.ToInt32(r["IdTipo"]);
                        _id_flota = Convert.ToInt32(r["IdFlota"]);
                        _id_region = Convert.ToInt32(r["IdRegion"]);
                        _id_ubicacion_terminal = Convert.ToInt32(r["IdUbicacionTerminal"]);
                        _id_tipo_servicio = Convert.ToByte(r["IdTipoSevicio"]);
                        _id_alcance_servicio = Convert.ToInt32(r["IdAlcanceServicio"]);
                        _id_detalle_negocio = Convert.ToInt32(r["IdDetalleNegocio"]);
                        _id_clasificacion1 = Convert.ToInt32(r["IdClasificacion1"]);
                        _id_clasificacion2 = Convert.ToInt32(r["IdClasificacion2"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método 
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tabla, int id_registro, int id_tipo)
        {
            bool result = false;

            //inicializamos el arreglo de parametros
            object[] param = { 4, 0, id_tabla, id_registro, id_tipo, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_clasificacion = Convert.ToInt32(r["Id"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _id_tipo = Convert.ToInt32(r["IdTipo"]);
                        _id_flota = Convert.ToInt32(r["IdFlota"]);
                        _id_region = Convert.ToInt32(r["IdRegion"]);
                        _id_ubicacion_terminal = Convert.ToInt32(r["IdUbicacionTerminal"]);
                        _id_tipo_servicio = Convert.ToByte(r["IdTipoSevicio"]);
                        _id_alcance_servicio = Convert.ToInt32(r["IdAlcanceServicio"]);
                        _id_detalle_negocio = Convert.ToInt32(r["IdDetalleNegocio"]);
                        _id_clasificacion1 = Convert.ToInt32(r["IdClasificacion1"]);
                        _id_clasificacion2 = Convert.ToInt32(r["IdClasificacion2"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        
        /// <summary>
        /// Editamos un Registro Clasificación
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                   int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_clasificacion, id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, habilitar, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Editamos un Registro Clasificación ligado a una transacción
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        private RetornoOperacion editaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                    int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario, bool habilitar, SqlTransaction transaccion)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_clasificacion, id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, habilitar, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }

        #endregion

        # region Metodos Publicos

        /// <summary>
        /// Metodo encargado de realizar la insercción de una Clasificación
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro al que se asigna la clasificación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaClasificacion(int id_tabla, int id_registro, int id_usuario)
        {
            //Insertando clasificación con valores default
            return InsertaClasificacion(id_tabla, id_registro, 0, 0, 0, 0, 0, 0, 0, 0, 0, id_usuario);
        }
        /// <summary>
        /// Metodo encargado de realizar la insercción de una Clasificación
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                         int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, true, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Metodo encargado de realizar la insercción de una Clasificación ligado a una transacción
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                         int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario, SqlTransaction transaccion)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, true, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }


        /// <summary>
        /// Método encargado de editar una Clasificación
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                         int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario)
        {
            return this.editaClasificacion(id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, this._habilitar);
        }

        /// <summary>
        ///  Método encargado de editar una Clasificación ligado a una transacción
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_flota"></param>
        /// <param name="id_region"></param>
        /// <param name="id_ubicacion_terminal"></param>
        /// <param name="id_tipo_servicio"></param>
        /// <param name="id_alcance_servicio"></param>
        /// <param name="id_detalle_negocio"></param>
        /// <param name="id_clasificacion1"></param>
        /// <param name="id_clasificacion2"></param>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public RetornoOperacion EditaClasificacion(int id_tabla, int id_registro, int id_tipo, int id_flota, int id_region, int id_ubicacion_terminal, int id_tipo_servicio,
                                                         int id_alcance_servicio, int id_detalle_negocio, int id_clasificacion1, int id_clasificacion2, int id_usuario, SqlTransaction transaccion)
        {
            return this.editaClasificacion(id_tabla, id_registro, id_tipo, id_flota, id_region, id_ubicacion_terminal,
                                 id_tipo_servicio, id_alcance_servicio, id_detalle_negocio, id_clasificacion1, id_clasificacion2, id_usuario, this._habilitar, transaccion);
        }

        /// <summary>
        /// Deshabilita una Clasificación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaClasificacion(int id_usuario)
        {
            return this.editaClasificacion(this.id_tabla, this.id_registro, this.id_tipo, id_flota, this.id_region, this.id_ubicacion_terminal,
                                 this.id_tipo_servicio, this.id_alcance_servicio, this.id_detalle_negocio, this.id_clasificacion1, this.id_clasificacion2, 
                                 id_usuario, false);
        }

        /// <summary>
        /// Deshabilita una Clasificación ligada a una transacción
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaClasificacion(int id_usuario, SqlTransaction transaccion)
        {
            return this.editaClasificacion(this.id_tabla, this.id_registro, this.id_tipo, id_flota, this.id_region, this.id_ubicacion_terminal,
                                 this.id_tipo_servicio, this.id_alcance_servicio, this.id_detalle_negocio, this.id_clasificacion1, this.id_clasificacion2,
                                 id_usuario, false, transaccion);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaClasificacion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_clasificacion);                   
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos con Transacción SQL
        /// </summary>
        /// <param name="trans">Transacción SQL</param>
        /// <returns></returns>
        public bool ActualizaClasificacion(SqlTransaction trans)
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_clasificacion, trans);
        }


    }
        #endregion
}
