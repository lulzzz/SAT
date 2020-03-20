using System;
using System.Data;
using TSDK.Base;


using TSDK.Datos;
using System.Linq;
using System.Transactions;
using System.Configuration;

namespace SAT_CL.Soporte
{
    public class SoporteTecnicoDetalle : Disposable
    {

        #region Enumeraciones

        /// <summary>
        /// Enumeracion de estatus de una orden de compra
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
            Iniciado = 2,
            /// <summary>
            ///Terminado
            /// </summary>
            Terminado = 3,
            /// <summary>
            /// Cancelado
            /// </summary>
            Cancelado = 4,
     
        };


        #endregion

        #region Atributos
        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "soporte.sp_soporte_tecnico_detalle_tstd";
        private int _id_soporte_tecnico_detalle;
        /// <summary>
        /// Atributo encargado de almacenar el Id Soporte Tecnico Detalle
        /// </summary>
        public int id_soporte_tecnico_detalle { get { return this._id_soporte_tecnico_detalle; } }
        private int _id_soporte_tecnico;
        /// <summary>
        /// Atributo encargado de almacenar el Id Soporte Tecnico Tecnico
        /// </summary>
        public int id_soporte_tecnico { get { return this._id_soporte_tecnico; } }
        private byte _id_tipo_soporte;
        /// <summary>
        /// Atributo encargado de almacenar el Id Tipo Soporte
        /// </summary>
        public byte id_tipo_soporte { get { return this._id_tipo_soporte; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Id Estatus 
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de almacenar Fecha Inicio General
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_termino;
        /// <summary>
        /// Atributo encargado de almacenar Fecha Termino General
        /// </summary>
        public DateTime fecha_termino { get { return this._fecha_termino; } }
        private string _observacion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string observacion { get { return this._observacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        /// <summary>
        /// Atributo que permite acceder a los elementos de la enumeracion Estatus
        /// </summary>
        public Estatus estatus
        {
            get { return (Estatus)this.id_estatus; }
        }

     
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public SoporteTecnicoDetalle()
        {
            //Asignando Valores
            this._id_soporte_tecnico_detalle = 0;
            this._id_soporte_tecnico = 0;
            this._id_tipo_soporte = 0;
            this._id_estatus = 0;
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_termino = DateTime.MinValue;
            this._observacion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_soporte_tecnico_detalle">Id de Registro</param>
        public SoporteTecnicoDetalle(int id_soporte_tecnico_detalle)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_soporte_tecnico_detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~SoporteTecnicoDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_soporte_tecnico_detalle">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_soporte_tecnico_detalle)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de parametros
            object[] param = { 3, id_soporte_tecnico_detalle, 0, 0, 0, null, null, "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_soporte_tecnico_detalle = id_soporte_tecnico_detalle;
                        this._id_soporte_tecnico = Convert.ToInt32(dr["IdSoporteTecnico"]);
                        this._id_tipo_soporte = Convert.ToByte(dr["IdTipoSoporte"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out this._fecha_termino);
                        this._observacion = dr["Observacion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo resultado obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_soporte_tecnico, byte id_tipo_soporte, byte id_estatus, DateTime fecha_inicio,
                                                    DateTime fecha_termino, string observacion, int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_soporte_tecnico_detalle, id_soporte_tecnico, id_tipo_soporte, id_estatus, fecha_inicio,fecha_termino
                , observacion, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que realiza la busqueda de un registro a partir de un id_orden_compra_detalle
        /// </summary>
        /// <param name="id_soporte_tecnico_detalle">Id que permite la referencia de registros para su asignación a los atributos</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_soporte_tecnico_detalle)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el sp 
            object[] param = { 4, id_soporte_tecnico_detalle, 0, 0, 0, null, null, "", 0, false, "", "" };
            
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida los datos obtenidos de la transaccion, que existan y qe no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las filas del Dataset y asigna los valores a los atributos
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        this._id_soporte_tecnico_detalle = id_soporte_tecnico_detalle;
                        this._id_soporte_tecnico = Convert.ToInt32(dr["IdSoporteTecnico"]);
                        this._id_tipo_soporte = Convert.ToByte(dr["IdTipoSoporte"]);
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaTermino"].ToString(), out this._fecha_termino);
                        this._observacion = dr["Observacion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la sentencia
                    retorno = true;
                }
            }
            //Retorno al método el objeto retorno
            return retorno;
        }


       

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en Termino
        /// </summary>
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion EditaSoporteTecnicoDetalleModal(byte id_tipo_soporte, DateTime fecha_inicio, string observacion, int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 5, this._id_soporte_tecnico_detalle, id_soporte_tecnico, id_tipo_soporte, id_estatus, fecha_inicio, null, observacion, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar soporte tecnico detalle
        /// </summary>
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>


        public static RetornoOperacion InsertaSoporteTecnicoDetalle(int id_soporte_tecnico, byte id_tipo_soporte, byte id_estatus, DateTime fecha_inicio,
                                                       DateTime fecha_termino, string observacion, int id_usuario)
        {
            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1, 0, id_soporte_tecnico, id_tipo_soporte, id_estatus, fecha_inicio, null, observacion, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
     
        /// <summary>
        /// Método Público encargado de Editar los soporte tecnico detalle 
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion EditaSoporteTecnicoDetalle(int id_soporte_tecnico, byte id_tipo_soporte, byte id_estatus, DateTime fecha_inicio,
                                                       DateTime fecha_termino, string observacion, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(id_soporte_tecnico, id_tipo_soporte, id_estatus, fecha_inicio, fecha_termino, observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar los soporte tecnico detalle 
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion EditaSoporteTecnicoDetalle(DateTime fecha_termino,int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(this._id_soporte_tecnico, this._id_tipo_soporte, 3, this._fecha_inicio, fecha_termino, this._observacion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Editar los soporte tecnico detalle 
        /// </summary>
        /// <param name="id_compania_emisora">Id Compañia Emisora</param>
        /// <param name="no_consecutivo_compania">No Consecutivo Compañia</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="id_usuario_asistente">Id Usuario</param>
        /// <param name="usuario_solicitante">Usuario Solicitante</param>
        /// <param name="fecha_inicio_general">Fecha Inicio General</param>
        /// <param name="fecha_termino_general">Fecha Termino General</param>
        /// <param name="id_usuario">Id Usuario Soporte Tecnico</param>
        /// <returns></returns>
        public RetornoOperacion EditaSoporteTecnicoDetalle(DateTime fecha_inicio, string observacion, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(this._id_soporte_tecnico, this._id_tipo_soporte, this._id_estatus, fecha_inicio, this._fecha_termino, observacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar el Soporte Tecnico Detalle
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaSoporteTecnicoDetalle(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
             if ((Estatus)this._id_estatus == Estatus.Registrado)
            {
            //Invocando Método de Actualización
            return this.actualizaRegistro(this._id_soporte_tecnico, this._id_tipo_soporte, this._id_estatus, this._fecha_inicio,
                                           this._fecha_termino, this._observacion, id_usuario, false);
                    
                   
            }
             //Instanciando Excepción
             result = new RetornoOperacion("El detalle pertence a un Soporte Técnico");
             //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_soporte_tecnico, byte id_tipo_soporte, byte id_estatus, DateTime fecha_inicio,
                                                    DateTime fecha_termino, string observacion, int id_usuario, bool habilitar)
        {   //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_soporte_tecnico_detalle, id_soporte_tecnico, id_tipo_soporte, id_estatus, fecha_inicio,
                 Convert.ToDateTime( fecha_termino) == DateTime.MinValue ? null : fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"])
                , observacion, id_usuario, habilitar, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos del Soporte Tecnico Detalle
        /// </summary>
        /// <returns></returns>
        public bool ActualizaSoporteTecnicoDetalle()
        {
            //Invocando Método de Actualización
            return this.cargaAtributosInstancia(this._id_soporte_tecnico_detalle);
        }

        /// <summary>
        /// Método encargado Obtener soporte detalles
        /// </summary>
        /// <param name="id_soporte_tecnico_detalle">Detalle de Orden de Compra</param>
        /// <returns></returns>
        public static DataTable ObtieneDetalleSoporte(int id_soporte_tecnico_detalle)
        {
            //Declarando Objeto de Retorno
            DataTable dtSoporteTecnico = null;

            //Creación del arreglo necesario para la obtención de datos del sp de la tabla orden compra detalle
            object[] param = { 4, id_soporte_tecnico_detalle, 0, 0, 0, null, null, "", 0, false, "", "" };
            //Realiza la transaccion con la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))

                    //Asigna a la tabla los valores encontrados
                    dtSoporteTecnico = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtSoporteTecnico;
        }

        /// <summary>
        /// Método encargado Obtener soporte detalles
        /// </summary>
        /// <param name="id_soporte_tecnico">Detalle de Orden de Compra</param>
        /// <returns></returns>
        public static string CargaEstatusS(int id_soporte_tecnico)
        {
            //Definiendo objeto de retorno
            string valor = "";
            using (DataTable ds = ObtieneDetalleSoporte(id_soporte_tecnico))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds))
                    //Asignando a objeto de retorno
                    valor = (from DataRow r in ds.Rows
                             where r.Field<string>("Estatus") == "Registrado"
                             select r.Field<string>("Estatus")).FirstOrDefault();
                //Devolviendo resultado
                return valor;
            }
        }

        /// <summary>
        /// Método Público encargado de Editar los soporte tecnico detalle 
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        public RetornoOperacion EditaSoporteTecnicoDetalleVentanaModal(byte id_tipo_soporte, DateTime fecha_inicio,string observacion, int id_usuario)
        {
            //Invocando Método de Actualización
            return this.EditaSoporteTecnicoDetalleModal(id_tipo_soporte, fecha_inicio, observacion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método encargado Obtener soporte detalles
        /// </summary>
        /// <param name="id_soporte_tecnico">Detalle de Orden de Compra</param>
        /// <returns></returns>
        public static DataTable TerminarReportes(int id_soporte_tecnico)
        {
                //Declarando Objeto de Retorno
            DataTable dtSoporteTecnico = null;
            //Realiza la transaccion con la base de datos
            using (DataTable DS = ObtieneDetalleSoporte(id_soporte_tecnico))
            {
                //Valida que existan los datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS))
                    dtSoporteTecnico = (from DataRow r in DS.Rows
                                            where r.Field<string>("Estatus") == "Registrado"   
                                            select r).CopyToDataTable();
            }
            //Devolviendo Resultado Obtenido
            return dtSoporteTecnico;
          
            }



        /// <summary>
        /// Método Público encargado de Insertar soporte tecnico detalle
        /// </summary>
        /// <param name="id_soporte_tecnico">Id Soporte Tecnico</param>
        /// <param name="id_tipo_soporte"> Id Tipo Soporte</param>
        /// <param name="id_estatus">id Estatus</param>        
        /// <param name="fecha_inicio">Fecha Inicio</param>
        /// <param name="fecha_termino">Fecha Termino</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="id_usuario">Id Usuario del soporte tecnico detalle</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>

        public static RetornoOperacion InsertaSoporteTecnicoDetalle(byte id_tipo_soporte, byte id_estatus, DateTime fecha_inicio,
                                                       DateTime fecha_termino, string observacion, int id_usuario)
        {
            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 7, 0, 0, id_tipo_soporte, 3, fecha_inicio, fecha_termino, observacion, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        #endregion
    }
}
