using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;
using System.Linq;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada todas las operaciones relacionadas con los Vencimientos
    /// </summary>
    public class Vencimiento : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que Indica el Estatus del Vencimiento
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Indica que el Vencimiento esta Activo
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Indica que el Vencimiento esta Completado
            /// </summary>
            Completado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_vencimiento_tv";

        private int _id_vencimiento;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Vencimiento
        /// </summary>
        public int id_vencimiento { get { return this._id_vencimiento; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public int id_estatus { get { return this._id_estatus; } }
        /// <summary>
        /// Atributo encargado de almacenar el Estatus
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }
        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de almacenar la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_registro;
        /// <summary>
        /// Atributo encargado de almacenar el Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private byte _id_prioridad;
        /// <summary>
        /// Atributo encargado de almacenar la Prioridad del Vencimiento
        /// </summary>
        public int id_prioridad { get { return this._id_prioridad; } }
        private int _id_tipo_vencimiento;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Vencimiento
        /// </summary>
        public int id_tipo_vencimiento { get { return this._id_tipo_vencimiento; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private DateTime _fecha_inicio;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Inicio
        /// </summary>
        public DateTime fecha_inicio { get { return this._fecha_inicio; } }
        private DateTime _fecha_fin;
        /// <summary>
        /// Atributo encargado de almacenar la Fecha de Fin
        /// </summary>
        public DateTime fecha_fin { get { return this._fecha_fin; } }
        private decimal _valor_km;
        /// <summary>
        /// Atributo encargado de almacenar el Valor del Kilometraje
        /// </summary>
        public decimal valor_km { get { return this._valor_km; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Valores por Defecto
        /// </summary>
        public Vencimiento()
        {
            //Asignando Valores
            this._id_vencimiento = 0;
            this._id_estatus = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_prioridad = 0;
            this._id_tipo_vencimiento = 0;
            this._descripcion = "";
            this._fecha_inicio = DateTime.MinValue;
            this._fecha_fin = DateTime.MinValue;
            this._valor_km = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_vencimiento"></param>
        public Vencimiento(int id_vencimiento)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_vencimiento);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Vencimiento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_vencimiento">Id de Vencimiento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_vencimiento)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_vencimiento, 0, 0, 0, 0, 0, "", null, null, 0, 0, false, "", "" };

            //Instanciando Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if(Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_vencimiento = id_vencimiento;
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_prioridad = Convert.ToByte(dr["IdPrioridad"]);
                        this._id_tipo_vencimiento = Convert.ToInt32(dr["IdTipoVencimiento"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        DateTime.TryParse(dr["FechaInicio"].ToString(), out this._fecha_inicio);
                        DateTime.TryParse(dr["FechaFin"].ToString(), out this._fecha_fin);
                        this._valor_km = Convert.ToDecimal(dr["ValorKm"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="id_prioridad">Prioridad</param>
        /// <param name="id_tipo_vencimiento">Tipo de Vencimiento</param>
        /// <param name="descripcion">Descripción del Vencimiento</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="valor_km">valor del Kilometraje</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(byte id_estatus, int id_tabla, int id_registro, int id_prioridad, 
                                        int id_tipo_vencimiento, string descripcion, DateTime fecha_inicio, DateTime fecha_fin, 
                                        decimal valor_km, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_vencimiento, id_estatus, id_tabla, id_registro, id_prioridad, id_tipo_vencimiento, descripcion, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               valor_km, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Vencimientos
        /// </summary>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="id_prioridad">Prioridad</param>
        /// <param name="id_tipo_vencimiento">Tipo de Vencimiento</param>
        /// <param name="descripcion">Descripción del Vencimiento</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="valor_km">valor del Kilometraje</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaVencimiento(int id_tabla, int id_registro, int id_prioridad,
                                        int id_tipo_vencimiento, string descripcion, DateTime fecha_inicio, DateTime fecha_fin,
                                        decimal valor_km, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, (byte)Estatus.Activo, id_tabla, id_registro, id_prioridad, id_tipo_vencimiento, descripcion, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               valor_km, id_usuario, true, "", "" };

            //Si no es de tipo de Taller
            if (id_tipo_vencimiento != 17)

                //Ejecutando SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else
                //Instanciando 
                result = new RetornoOperacion("Solo se puede Insertar Vencimientos 'En Taller' desde la Orden de Trabajo");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Insertar los Vencimientos de una Orden de Trabajo
        /// </summary>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="id_prioridad">Prioridad</param>
        /// <param name="descripcion">Descripción del Vencimiento</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="valor_km">valor del Kilometraje</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaVencimientoOrdenTrabajo(int id_tabla, int id_registro, byte id_prioridad,
                                        string descripcion, DateTime fecha_inicio, DateTime fecha_fin,
                                        decimal valor_km, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, (byte)Estatus.Activo, id_tabla, id_registro, id_prioridad, 17, descripcion, 
                               TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_inicio), TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_fin), 
                               valor_km, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Vencimientos
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="id_prioridad">Prioridad</param>
        /// <param name="id_tipo_vencimiento">Tipo de Vencimiento</param>
        /// <param name="descripcion">Descripción del Vencimiento</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="valor_km">valor del Kilometraje</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaVencimiento(Estatus estatus, int id_tabla, int id_registro, int id_prioridad,
                                        int id_tipo_vencimiento, string descripcion, DateTime fecha_inicio, DateTime fecha_fin,
                                        decimal valor_km, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Validando que no sea el Tipo de Taller
            if (id_tipo_vencimiento != 17)

                //Devolviendo Resultado Obtenido
                retorno = this.actualizaRegistros((byte)estatus, id_tabla, id_registro, id_prioridad, id_tipo_vencimiento, descripcion,
                                   fecha_inicio, fecha_fin, valor_km, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("El Vencimiento solo se puede Actualizar desde la Orden de Trabajo");

            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar los Vencimientos de la Orden de Trabajo
        /// </summary>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_tabla">Entidad</param>
        /// <param name="id_registro">Registro de la Entidad</param>
        /// <param name="id_prioridad">Prioridad</param>
        /// <param name="id_tipo_vencimiento">Tipo de Vencimiento</param>
        /// <param name="descripcion">Descripción del Vencimiento</param>
        /// <param name="fecha_inicio">Fecha de Inicio</param>
        /// <param name="fecha_fin">Fecha de Termino</param>
        /// <param name="valor_km">valor del Kilometraje</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaVencimientoOrdenTrabajo(Estatus estatus, int id_tabla, int id_registro, int id_prioridad,
                                        int id_tipo_vencimiento, string descripcion, DateTime fecha_inicio, DateTime fecha_fin,
                                        decimal valor_km, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando que no sea el Tipo de Taller
            if (id_tipo_vencimiento == 17)

                //Devolviendo Resultado Obtenido
                retorno = this.actualizaRegistros((byte)estatus, id_tabla, id_registro, id_prioridad, id_tipo_vencimiento, descripcion,
                                   fecha_inicio, fecha_fin, valor_km, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("No se puede Editar el Vencimiento.");

            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Vencimientos
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaVencimiento(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistros(this._id_estatus, this._id_tabla, this._id_registro, this._id_prioridad, this._id_tipo_vencimiento, this._descripcion,
                               this._fecha_inicio, this._fecha_fin, this._valor_km, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Vencimiento
        /// </summary>
        /// <returns></returns>
        public bool ActualizaVencimiento()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_vencimiento);
        }
        /// <summary>
        /// Método Público encargado de Terminar el Vencimiento
        /// </summary>
        /// <param name="fecha_fin">Fecha de Fin</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion TerminaVencimiento(DateTime fecha_fin, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando que no sea el Tipo de Taller
            if (id_tipo_vencimiento != 17)

                //Devolviendo Resultado Obtenido
                retorno = this.actualizaRegistros((byte)Estatus.Completado, this._id_tabla, this._id_registro, this._id_prioridad, this._id_tipo_vencimiento, this._descripcion,
                                   this._fecha_inicio, fecha_fin, this._valor_km, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("El Vencimiento solo se puede Terminar desde la Orden de Trabajo");

            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de Terminar los Vencimientos Provenientes de una Orden de Trabajo
        /// </summary>
        /// <param name="fecha_fin"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion TerminaVencimientoOrdenTrabajo(DateTime fecha_fin, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando que no sea el Tipo de Taller
            if (id_tipo_vencimiento == 17)

                //Devolviendo Resultado Obtenido
                retorno = this.actualizaRegistros((byte)Estatus.Completado, this._id_tabla, this._id_registro, this._id_prioridad, this._id_tipo_vencimiento, this._descripcion,
                                   this._fecha_inicio, fecha_fin, this._valor_km, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("El Vencimiento debe de ser de Tipo 'Taller'.");

            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Carga los vencimientos existentes para el recurso solicitado
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación del vencimiento (unidad, operador)</param>
        /// <param name="id_recurso">Id de Recurso a consultar</param>
        /// <param name="id_estatus">Estatus del vencimiento</param>
        /// <param name="id_tipo_evento">Tipo de Vencimiento</param>
        /// <param name="id_prioridad">Prioridad del Vencimiento</param>
        /// <param name="inicio_fecha_inicio">Inicio de Vencimiento Fecha Inicio</param>
        /// <param name="inicio_fecha_fin">Inicio de Vencimiento Fercha fin</param>
        /// <param name="fin_fecha_inicio">Fin de Vencimiento Fecha de Inicio</param>
        /// <param name="fin_fecha_fin">Fin de Vencimiento Fecha de Fin</param>
        /// <returns></returns>
        public static DataTable CargaVencimientosRecurso(TipoVencimiento.TipoAplicacion tipo_aplicacion, int id_recurso, byte id_estatus, int id_tipo_vencimiento, byte id_prioridad,
                                                     DateTime inicio_fecha_inicio, DateTime inicio_fecha_fin, DateTime fin_fecha_inicio, DateTime fin_fecha_fin)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Determinando la tabla correspondiente por tipo de aplicación
            int id_tabla = 0;
            switch (tipo_aplicacion)
            { 
                case TipoVencimiento.TipoAplicacion.Unidad:
                    id_tabla = 19;
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    id_tabla = 76;
                    break;
                case TipoVencimiento.TipoAplicacion.Transportista:
                    id_tabla = 25;
                    break;
                case TipoVencimiento.TipoAplicacion.Servicio:
                    id_tabla = 1;
                    break;
            }

            //Declarando arreglo de parámetros para consulta
            object[] param = { 4, 0,  id_estatus, id_tabla, id_recurso, id_prioridad, id_tipo_vencimiento, "", Fecha.ConvierteDateTimeObjeto(inicio_fecha_inicio),
                                 Fecha.ConvierteDateTimeObjeto(fin_fecha_inicio),
                                 0, 0, false,  inicio_fecha_fin == DateTime.MinValue ? "": inicio_fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fin_fecha_fin == DateTime.MinValue ?"": fin_fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };

            //Realizando consulta y devolviendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga los vencimientos activos para el recurso solicitado
        /// </summary>
        /// <param name="tipo_aplicacion">Tipo de Aplicación del vencimiento (unidad, operador)</param>
        /// <param name="id_recurso">Id de Recurso a consultar</param>
        /// <param name="fecha_referencia">Fecha mayor o igual a la Fecha Inicial de los Vencimientos a buscar</param>
        /// <returns></returns>
        public static DataTable CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion tipo_aplicacion, int id_recurso, DateTime fecha_referencia)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Determinando la tabla correspondiente por tipo de aplicación
            int id_tabla = 0;
            switch (tipo_aplicacion)
            {
                case TipoVencimiento.TipoAplicacion.Unidad:
                    id_tabla = 19;
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    id_tabla = 76;
                    break;
                case TipoVencimiento.TipoAplicacion.Transportista:
                    id_tabla = 25;
                    break;
                case TipoVencimiento.TipoAplicacion.Servicio:
                    id_tabla = 1;
                    break;
            }

            //Declarando arreglo de parámetros para consulta
            object[] param = { 5, 0, 0, id_tabla, id_recurso, 0, 0, "", fecha_referencia, null, 0, 0, false, "", "" };

            //Realizando consulta y devolviendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga los vencimientos activos para los recursos asignados al movimiento solicitado
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento a consultar</param>        
        /// <param name="fecha_referencia">Fecha mayor o igual a la Fecha Inicial de los Vencimientos a buscar</param>
        /// <returns></returns>
        public static DataTable CargaVencimientosActivosMovimiento(int id_movimiento, DateTime fecha_referencia)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Declarando arreglo de parámetros para consulta
            object[] param = { 6, 0, 0, 0, 0, 0, 0, "", fecha_referencia, null, 0, 0, false, id_movimiento, "" };

            //Realizando consulta y devolviendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Obtenemos estatus de la Licencia del Operador
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <returns></returns>
        public static bool ValidaLicenciaVigente(int id_operador)
        {
            //Declarando objeto de retorno
           bool vigente = false;

            //Declarando arreglo de parámetros para consulta
            object[] param = { 7, 0, 0, 0, id_operador, 0, 0, "", null, null, 0, 0, false, "", "" };

            //Realizando consulta y devolviendo resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    vigente = (from DataRow r in ds.Tables[0].Rows
                                   select Convert.ToBoolean(r["Vigente"])).FirstOrDefault();

                }
                //Devolviendo resultado
                return vigente;
            }
        }

        #endregion
    }
}
