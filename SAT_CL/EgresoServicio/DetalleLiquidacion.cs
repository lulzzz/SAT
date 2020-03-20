using SAT_CL.Despacho;
using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
namespace SAT_CL.EgresoServicio
{   
    /// <summary>
    /// Clase encargada de todas las operaciones de los Detalles de Liquidación
    /// </summary>
    public class DetalleLiquidacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumeración que muestra los Estatus del Detalle
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Estatus que indica que la Liquidación ha sido Registrada
            /// </summary>
            Registrado = 1,
            /// <summary>
            /// Estatus que indica que la Liquidación ha sido Liquidada
            /// </summary>
            Liquidado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encaragdo de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_detalle_liquidacion_tdl";
        
        private int _id_detalle_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar el Id de Detalle de Liquidación
        /// </summary>
        public int id_detalle_liquidacion { get { return this._id_detalle_liquidacion; } }
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
        private byte _id_estatus_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Liquidación
        /// </summary>
        public byte id_estatus_liquidacion { get { return this._id_estatus_liquidacion; } }
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de Liquidación (Enumeración)
        /// </summary>
        public Estatus estatus_liquidacion { get { return (Estatus)this._id_estatus_liquidacion; } }
        private int _id_unidad;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }
        private int _id_operador;
        /// <summary>
        /// Atributo encargado de almacenar el Operador
        /// </summary>
        public int id_operador { get { return this._id_operador; } }
        private int _id_proveedor_compania;
        /// <summary>
        /// Atributo encargado de almacenar el Proveedor
        /// </summary>
        public int id_proveedor_compania { get { return this._id_proveedor_compania; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de almacenar el Servicio al que se encuentra ligado
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private int _id_movimiento;
        /// <summary>
        /// Atributo encargado de almacenar el Movimiento al que se encuentra Ligado
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }
        private DateTime _fecha_liquidacion;
        /// <summary>
        /// Atributo encaragdo de almacenar la Fecha de Liquidación
        /// </summary>
        public DateTime fecha_liquidacion { get { return this._fecha_liquidacion; } }
        private int _id_liquidacion;
        /// <summary>
        /// Atributo encargado de almacenar la Liquidación a la que pertenece
        /// </summary>
        public int id_liquidacion { get { return this._id_liquidacion; } }
        private decimal _cantidad;
        /// <summary>
        /// Atributo encargado de almacenar la Cantidad
        /// </summary>
        public decimal cantidad { get { return this._cantidad; } }
        private int _id_unidad_medida;
        /// <summary>
        /// Atributo encargado de almacenar la Unidad de Medida
        /// </summary>
        public int id_unidad_medida { get { return this._id_unidad_medida; } }
        private decimal _valor_unitario;
        /// <summary>
        /// Atributo encargado de almacenar el Valor Unitario
        /// </summary>
        public decimal valor_unitario { get { return this._valor_unitario; } }
        private decimal _monto;
        /// <summary>
        /// Atributo encargado de almacenar el Monto
        /// </summary>
        public decimal monto { get { return this._monto; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public DetalleLiquidacion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="_id_detalle_liquidacion">Id de Registro a Instanciar</param>
        public DetalleLiquidacion(int _id_detalle_liquidacion)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(_id_detalle_liquidacion);
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        public DetalleLiquidacion(int id_registro, int id_tabla)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro, id_tabla);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~DetalleLiquidacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando valores
            this._id_detalle_liquidacion = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_estatus_liquidacion = 0;
            this._id_unidad = 0;
            this._id_operador = 0;
            this._id_proveedor_compania = 0;
            this._id_servicio = 0;
            this._id_movimiento = 0;
            this._fecha_liquidacion = DateTime.MinValue;
            this._id_liquidacion = 0;
            this._cantidad = 0;
            this._id_unidad_medida = 0;
            this._valor_unitario = 0;
            this._monto = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="_id_detalle_liquidacion">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int _id_detalle_liquidacion)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 3, _id_detalle_liquidacion, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada una de las Filas
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores
                        this._id_detalle_liquidacion = _id_detalle_liquidacion;
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_estatus_liquidacion = Convert.ToByte(dr["IdEstatusLiquidacion"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._id_proveedor_compania = Convert.ToInt32(dr["IdProveedorCompania"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaLiquidacion"].ToString(), out this._fecha_liquidacion);
                        this._id_liquidacion = Convert.ToInt32(dr["IdLiquidacion"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad_medida = Convert.ToInt32(dr["IdUnidadMedida"]);
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._monto = Convert.ToDecimal(dr["Monto"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dados un Registro y una Tabla
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro, int id_tabla)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 4, 0, id_tabla, id_registro, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada una de las Filas
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando valores
                        this._id_detalle_liquidacion = Convert.ToInt32(dr["Id"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_estatus_liquidacion = Convert.ToByte(dr["IdEstatusLiquidacion"]);
                        this._id_unidad = Convert.ToInt32(dr["IdUnidad"]);
                        this._id_operador = Convert.ToInt32(dr["IdOperador"]);
                        this._id_proveedor_compania = Convert.ToInt32(dr["IdProveedorCompania"]);
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._id_movimiento = Convert.ToInt32(dr["IdMovimiento"]);
                        DateTime.TryParse(dr["FechaLiquidacion"].ToString(), out this._fecha_liquidacion);
                        this._id_liquidacion = Convert.ToInt32(dr["IdLiquidacion"]);
                        this._cantidad = Convert.ToDecimal(dr["Cantidad"]);
                        this._id_unidad_medida = Convert.ToInt32(dr["IdUnidadMedida"]);
                        this._valor_unitario = Convert.ToDecimal(dr["ValorUnitario"]);
                        this._monto = Convert.ToDecimal(dr["Monto"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualziar los Registros en BD
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_estatus_liquidacion">Estatus de Liquidación</param>
        /// <param name="id_unidad">Referencia de la Unidad de Transporte</param>
        /// <param name="id_operador">Referencia del Operador</param>
        /// <param name="id_proveedor_compania">Referencia del Proveedor</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento Asignado</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="id_liquidacion">Liquidación Perteneciente</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_tabla, int id_registro, byte id_estatus_liquidacion, int id_unidad, int id_operador, 
                                            int id_proveedor_compania, int id_servicio, int id_movimiento, DateTime fecha_liquidacion, int id_liquidacion,
                                            decimal cantidad, int id_unidad_medida, decimal valor_unitario, int id_usuario, bool habilitar)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Armando arreglo de Parametros
            object[] param = { 2, this._id_detalle_liquidacion, id_tabla, id_registro, id_estatus_liquidacion, id_unidad, id_operador, 
                                 id_proveedor_compania, id_servicio, id_movimiento, TSDK.Base.Fecha.ConvierteDateTimeObjeto(fecha_liquidacion), id_liquidacion, 
                                 cantidad, id_unidad_medida, valor_unitario, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            
            /*/Validando que el estatus no este Liquidado
            if (this._id_estatus_liquidacion != (byte)Estatus.Liquidado)
                //Ejecutando SP
                result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            else//Instanciando Excepcion
                result = new RetornoOperacion("El Detalle se encuentra Liquidado, Imposible su Edición");*/
            
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Detalles de Liquidación
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_unidad">Referencia de la Unidad de Transporte</param>
        /// <param name="id_operador">Referencia del Operador</param>
        /// <param name="id_proveedor_compania">Referencia del Proveedor</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento Asignado</param>
        /// <param name="id_liquidacion">Liquidación Perteneciente</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleLiquidacion(int id_tabla, int id_registro, int id_unidad, int id_operador, int id_proveedor_compania, 
                                            int id_servicio, int id_movimiento, int id_liquidacion, decimal cantidad, 
                                            int id_unidad_medida, decimal valor_unitario, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_tabla, id_registro, (byte)Estatus.Registrado, id_unidad, id_operador, id_proveedor_compania, 
                                 id_servicio, id_movimiento, null, id_liquidacion, cantidad, 
                                 id_unidad_medida, valor_unitario, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Insertar los Detalles de Liquidación
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="estatus">Estatus del Detalle</param>
        /// <param name="id_unidad">Referencia de la Unidad de Transporte</param>
        /// <param name="id_operador">Referencia del Operador</param>
        /// <param name="id_proveedor_compania">Referencia del Proveedor</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento Asignado</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="id_liquidacion">Liquidación Perteneciente</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleLiquidacion(int id_tabla, int id_registro, Estatus estatus, int id_unidad, int id_operador, int id_proveedor_compania,
                                            int id_servicio, int id_movimiento, DateTime fecha_liquidacion, int id_liquidacion, decimal cantidad,
                                            int id_unidad_medida, decimal valor_unitario, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de Parametros
            object[] param = { 1, 0, id_tabla, id_registro, (byte)estatus, id_unidad, id_operador, id_proveedor_compania, 
                                 id_servicio, id_movimiento, fecha_liquidacion, id_liquidacion, cantidad, 
                                 id_unidad_medida, valor_unitario, id_usuario, true, "", "" };
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Detalles de Liquidación
        /// </summary>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_estatus_liquidacion">Estatus de Liquidación</param>
        /// <param name="id_unidad">Referencia de la Unidad de Transporte</param>
        /// <param name="id_operador">Referencia del Operador</param>
        /// <param name="id_proveedor_compania">Referencia del Proveedor</param>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_movimiento">Movimiento Asignado</param>
        /// <param name="fecha_liquidacion">Fecha de Liquidación</param>
        /// <param name="id_liquidacion">Liquidación Perteneciente</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="id_unidad_medida">Unidad de Medida</param>
        /// <param name="valor_unitario">Valor Unitario</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleLiquidacion(int id_tabla, int id_registro, byte id_estatus_liquidacion, int id_unidad, int id_operador,
                                            int id_proveedor_compania, int id_servicio, int id_movimiento, DateTime fecha_liquidacion, int id_liquidacion,
                                            decimal cantidad, int id_unidad_medida, decimal valor_unitario, int id_usuario)
        {   
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(id_tabla, id_registro, id_estatus_liquidacion, id_unidad, id_operador,
                                     id_proveedor_compania, id_servicio, id_movimiento, fecha_liquidacion, id_liquidacion, cantidad,
                                     id_unidad_medida, valor_unitario, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Edita un Detalle de Liquidación
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleLiquidacion(int id_servicio, int id_usuario)
        {
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(this._id_tabla, this._id_registro, this._id_estatus_liquidacion, this._id_unidad, this._id_operador,
                                     this._id_proveedor_compania, id_servicio, this._id_movimiento, this._fecha_liquidacion, this._id_liquidacion, this._cantidad,
                                     this._id_unidad_medida, this._valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Edita un Detalle de Liquidación
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleLiquidacion(int id_servicio, int id_movimiento, int id_usuario)
        {
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(this._id_tabla, this._id_registro, this._id_estatus_liquidacion, this._id_unidad, this._id_operador,
                                     this._id_proveedor_compania, id_servicio, id_movimiento, this._fecha_liquidacion, this._id_liquidacion, this._cantidad,
                                     this._id_unidad_medida, this._valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Liquidar el Detalle de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion">Referencia a la Liquidación</param>
        /// <param name="fecha_liquidacion">Fecha de Cierre de la Liquidación</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion LiquidaDetalle(int id_liquidacion, DateTime fecha_liquidacion, int id_usuario)
        {   
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(this._id_tabla, this._id_registro, (byte)Estatus.Liquidado, this._id_unidad, this._id_operador,
                                     this._id_proveedor_compania, this._id_servicio, this._id_movimiento, fecha_liquidacion, id_liquidacion, this._cantidad,
                                     this._id_unidad_medida, this._valor_unitario, id_usuario, this._habilitar); 
        }
        /// <summary>
        /// Método encargado de Liberar el Detalle al Eliminar la Liquidación 
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion LiberaDetalleLiquidacion(int id_usuario)
        {
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(this._id_tabla, this._id_registro, (byte)Estatus.Registrado, this._id_unidad, this._id_operador,
                                     this._id_proveedor_compania, this._id_servicio, this._id_movimiento, DateTime.MinValue, 0, this._cantidad,
                                     this._id_unidad_medida, this._valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Abrir la Liquidacion del Detalle de la Liquidación
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion AbreLiquidacionDetalle(int id_usuario)
        {   
            //Devolviendo resultado Obtenido
            return this.actualizaRegistros(this._id_tabla, this._id_registro, (byte)Estatus.Registrado, this._id_unidad, this._id_operador,
                                     this._id_proveedor_compania, this._id_servicio, this._id_movimiento, DateTime.MinValue, this._id_liquidacion, this._cantidad,
                                     this._id_unidad_medida, this._valor_unitario, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar los Detalles de Liquidación
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaDetalleLiquidacion(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_tabla, this._id_registro, this._id_estatus_liquidacion, this._id_unidad, this._id_operador,
                                 this._id_proveedor_compania, this._id_servicio, this._id_movimiento, this._fecha_liquidacion, this._id_liquidacion, this._cantidad,
                                 this._id_unidad_medida, this._valor_unitario, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos de los Detalles de Liquidación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaDetalleLiquidacion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_detalle_liquidacion);
        }
        /// <summary>
        /// Método Público encargado de Obtener los Movimientos por Liquidar
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Pendiente</param>
        /// <param name="estatus">Estatus Liquidación</param>
        /// <param name="id_entidad">Entidad</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <returns></returns>
        public static DataSet ObtieneMovimientosYDetallesPorLiquidacion(int id_liquidacion, Estatus estatus, int id_entidad, byte id_tipo_entidad)
        {
            //Armando arreglo de Parametros
            object[] param = { 5, 0, id_tipo_entidad, id_entidad, (byte)estatus, 0, 0, 0, 0, 0, null, id_liquidacion, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))

                //Devolviendo Objeto de Retorno
                return ds;
        }

        /// <summary>
        /// Mètodo encargado de validar que existan Anticipos (Diesel y Depósitos) Ligados a Un Movimiento
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static RetornoOperacion ValidaAnticiposMovimiento(int id_movimiento)
        {
            //Declarando variable 
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Armando arreglo de Parametros
            object[] param = { 6, 0, 0, 0, 0, 0, 0, 0, 0, id_movimiento, null, 0, 0, 0, 0, 0, false, "", "" };
            //Instanciando Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Instanciamos Movimiento
                    using(Movimiento objMovimiento = new Movimiento(id_movimiento))
                    {
                        //Instanciamos Parada Origen y Destino
                        using (Parada paradaOrigen = new Parada(objMovimiento.id_parada_origen), paradaDestino = new Parada(objMovimiento.id_parada_destino))
                        {
                            //Asignamos Valor
                            resultado = new RetornoOperacion("Existen anticipos registrados al movimiento " + paradaOrigen.descripcion + " - " + paradaDestino.descripcion + ".");
                        }
                    }

            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Mètodo encargado de validar que existan Anticipos (Diesel y Depósitos) Ligados a Un Movimiento y un Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        /// <returns></returns>
        public static bool ValidaAnticiposMovimiento(int id_movimiento, int id_operador, int id_unidad, int id_proveedor_compania)
        {
            //Declarando variable 
            bool validaAnticipos = false;
            //Instanciando Resultado del SP
            using (DataTable mit = CargaAnticipos(id_movimiento, id_operador, id_unidad, id_proveedor_compania))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    //Asignamos Valor
                    validaAnticipos = true;
            }
            //Devolviendo Resultado Obtenido
            return validaAnticipos;
        }

        /// Mètodo encargado de actualizar los Anticipos (Diesel y Depósitos) Ligados a Un Movimiento y un Recurso
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaAnticiposPorMovimiento(int id_servicio, int id_movimiento,int id_operador, int id_unidad, int id_proveedor_compania, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declaramos Tipo
            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
            int id_recurso = id_proveedor_compania;
            if(id_operador !=0)
            {
                //Establecemos Valores
                tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                id_recurso = id_operador;
            }
            else if(id_unidad !=0)
            {
                //Establecemos Valores
                tipo  = MovimientoAsignacionRecurso.Tipo.Unidad;
                id_recurso = id_unidad;
            }
            //Instanciando Resultado del SP
            using (DataTable mit = CargaAnticipos(id_movimiento, id_operador, id_unidad, id_proveedor_compania))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos los Detalles de la Liquidación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Obtenenemos Movimiento 
                        int id_movimiento_anterior = MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaOTerminada(id_servicio,id_movimiento,(byte)tipo, id_recurso);

                        //Validamos que exista Asignación
                        if (id_movimiento_anterior != 0)
                        {
                            //Instanciamos detalle Liqui.
                            using (DetalleLiquidacion obDetalleLiq = new DetalleLiquidacion(r.Field<int>("Id")))
                            {
                                //Validamos no se encuentre asignado a una Liquidacion
                                if (obDetalleLiq.id_liquidacion == 0)
                                {
                                    //Cambiamos Id de Movimineto
                                    resultado = obDetalleLiq.EditaDetalleLiquidacion(obDetalleLiq.id_servicio, id_movimiento_anterior, id_usuario);
                                }
                                else
                                {
                                    //Mostramos Error
                                    resultado = new RetornoOperacion("El Anticipo ya se encuentra ligado a una Liquidación.");
                                }

                            }
                        }
                        else
                        {
                            //Mostramos Mensaje Resultado
                            resultado = new RetornoOperacion("No existe Asignación disponible para los Anticipos registrados.");
                        }
                        //Si existe un error
                        if (!resultado.OperacionExitosa)
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }

        /// <summary>
        /// Mètodo encargado de actualizar los Anticipos (Diesel y Depósitos) Ligados a Un Movimiento y un Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaAnticiposPorMovimiento(int id_servicio, int id_movimiento, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Instanciando Resultado del SP
            using (DataTable mit = CargaAnticipos(id_movimiento))
            {
               //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Recorremos los Detalles de la Liquidación
                    foreach (DataRow r in mit.Rows)
                    {
                        //Obtenenemos Movimiento 
                        int id_movimiento_anterior = MovimientoAsignacionRecurso.ObtieneAsignacionIniciadaOTerminada(id_servicio,id_movimiento, r.Field<int>("IdTipo"), r.Field<int>("IdRecurso"));

                        //Validamos que exista Asignación
                        if (id_movimiento_anterior != 0)
                        {
                            //Instanciamos detalle Liqui.
                            using (DetalleLiquidacion obDetalleLiq = new DetalleLiquidacion(r.Field<int>("Id")))
                            {
                                //Validamos no se encuentre asignado a una Liquidacion
                                if (obDetalleLiq.id_liquidacion == 0)
                                {
                                    //Cambiamos Id de Movimineto
                                    resultado = obDetalleLiq.EditaDetalleLiquidacion(obDetalleLiq.id_servicio, id_movimiento_anterior, id_usuario);
                                }
                                else
                                {
                                    //Mostramos Error
                                    resultado = new RetornoOperacion("El Anticipo ya se encuentra ligado a una Liquidación.");
                                }

                            }
                        }
                        else
                        {
                            //Mostramos Mensaje Resultado
                            resultado = new RetornoOperacion("No existe Asignación disponible para los Anticipos registrados.");
                        }
                        //Si existe un error
                        if (!resultado.OperacionExitosa)
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Obtener el Resumen Total de la Liqudiación
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <returns></returns>
        public static DataTable ObtieneResumenTotalLiquidacion(int id_liquidacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtLiquidaciones = null;
            
            //Armando arreglo de Parametros
            object[] param = { 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, id_liquidacion, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {    //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtLiquidaciones = ds.Tables["Table"];
            }
                
            //Devolviendo Objeto de Retorno
            return dtLiquidaciones;
        }
        /// <summary>
        /// Método Público encargado de Validar que Existan Depositos Pendientes
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <param name="id_estatus">Estatus de la Liquidación</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <returns></returns>
        public static bool ValidaDepositosPendientesLiquidacion(int id_liquidacion, byte id_estatus, int id_tipo_asignacion, int id_entidad)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando arreglo de Parametros
            object[] param = { 9, 0, id_tipo_asignacion, id_entidad, id_estatus, 0, 0, 0, 0, 0, null, id_liquidacion, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Resultado Obtenido
                        result = Convert.ToBoolean(dr["Validacion"]);
                }
            }

            //Devoviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Validar si el Movimiento tiene Depositos Pendientes
        /// </summary>
        /// <param name="id_movimiento">Movimiento</param>
        /// <param name="id_estatus">Estatus de la Liquidación</param>
        /// <param name="id_tipo_asignacion">Tipo de Asignación</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <returns></returns>
        public static bool ValidaDepositosPendientesMovimiento(int id_movimiento, byte id_estatus, int id_tipo_asignacion, int id_entidad)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando arreglo de Parametros
            object[] param = { 13, 0, id_tipo_asignacion, id_entidad, id_estatus, 0, 0, 0, 0, id_movimiento, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Asignando Resultado Obtenido
                        result = Convert.ToBoolean(dr["Validacion"]);
                }
            }

            //Devoviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Realiza la búsqueda de detalles de liquidación del operador solicitado, cuyo pago de liquidación esté pendiente
        /// </summary>
        /// <param name="id_operador">Id de Operador</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesSinLiquidarOperador(int id_operador)
        { 
            //Declarando objeto de resultado
            DataTable mitDetalles = null;

            //Declarando parámetros de búsqueda
            object[] param = { 10, 0, 0, 0, 0, 0, id_operador, 0, 0, 0, null, 0, 0, 0, 0, 0, false, "", "" };

            //Búscando registros coincidentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            { 
                //Si hay registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mitDetalles = ds.Tables["Table"];

                //Devolviendo resultado
                return mitDetalles;
            }
        }
        /// <summary>
        /// Realiza la búsqueda de detalles de liquidación de la unidad solicitada, cuyo pago de liquidación esté pendiente
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesSinLiquidarUnidad(int id_unidad)
        {
            //Declarando objeto de resultado
            DataTable mitDetalles = null;

            //Declarando parámetros de búsqueda
            object[] param = { 11, 0, 0, 0, 0, id_unidad, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, false, "", "" };

            //Búscando registros coincidentes
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Si hay registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    mitDetalles = ds.Tables["Table"];

                //Devolviendo resultado
                return mitDetalles;
            }
        }

        /// <summary>
        /// Mètodo encargado cargar los Anticipos (Diesel y Depósitos) Ligados a Un Movimiento y un Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        /// <returns></returns>
        public static DataTable CargaAnticipos(int id_movimiento, int id_operador, int id_unidad, int id_proveedor_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Armando arreglo de Parametros
            object[] param = { 7, 0, 0, 0, 0, id_unidad, id_operador, id_proveedor_compania, 0, id_movimiento, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {    //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtAnticipos;
        }

        /// <summary>
        /// Mètodo encargado de cargar todos los Detalles liquidación Ligados a Un Movimiento y un Recurso
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        /// <returns></returns>
        public static DataTable CargaDetallesLiquidacion(int id_movimiento, int id_operador, int id_unidad, int id_proveedor_compania)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Armando arreglo de Parametros
            object[] param = { 12, 0, 0, 0, 0, id_unidad, id_operador, id_proveedor_compania, 0, id_movimiento, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {    //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtAnticipos;
        }
        /// <summary>
        /// Método encargado de actualizar el Servicio de los Depósitos y Vales de Diesel
        /// </summary>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_provedor_compania">Id proveedor Compania</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion ActualizaServicioDetalleLiquidacion(int id_servicio, int id_cliente, int id_movimiento, int id_operador, int id_unidad, int id_provedor_compania, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos los Detalles de Liquidación
            using (DataTable mitDetallesLiquidacion = DetalleLiquidacion.CargaDetallesLiquidacion(id_movimiento, id_operador, id_unidad, id_provedor_compania))
            {
                //Validamos Anticipos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mitDetallesLiquidacion))
                {
                    //Recorremos cada uno de los Detalles
                    foreach (DataRow r in mitDetallesLiquidacion.Rows)
                    {
                        //Instanciamos Detalle Lioquidacion
                        using (DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion(r.Field<int>("Id")))
                        {
                            //Validando Detalle
                            if (objDetalleLiquidacion.habilitar)
                            {
                                //Validando Estatus
                                switch (objDetalleLiquidacion.estatus_liquidacion)
                                {
                                    case Estatus.Liquidado:
                                        {
                                            //instanciando Excepción
                                            resultado = new RetornoOperacion("El Movimiento a sido Pagado, imposible su modificación");
                                            break;
                                        }
                                    case Estatus.Registrado:
                                        {
                                            //Validando Liquidación
                                            if (objDetalleLiquidacion.id_liquidacion > 0)

                                                //instanciando Excepción
                                                resultado = new RetornoOperacion("El Movimiento esta en una Liquidación, imposible su modificación");
                                            else
                                            {
                                                //Actualizamos Servicio
                                                resultado = objDetalleLiquidacion.EditaDetalleLiquidacion(id_servicio, id_usuario);

                                                //Si la Tabla es Depósito
                                                if (objDetalleLiquidacion.id_tabla == 51)
                                                {
                                                    //Instanciamos Depósito
                                                    using (Deposito objDeposito = new Deposito(objDetalleLiquidacion.id_registro))
                                                    {
                                                        //validando Deposito
                                                        if (objDeposito.habilitar)
                                                        
                                                            //Editamos cliente del Depósito
                                                            resultado = objDeposito.EditaDeposito(id_cliente, id_usuario);
                                                        else
                                                            //instanciando Excepción
                                                            resultado = new RetornoOperacion("No se puede recuperar el Deposito del Movimiento");
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            else
                                //instanciando Excepción
                                resultado = new RetornoOperacion("No se puede recuperar el Detalle");
                        }

                        //Si existe algun Error
                        if(!resultado.OperacionExitosa)
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolemos Resultado
            return resultado;
        }

        /// <summary>
        /// Mètodo encargado cargar los Anticipos (Diesel y Depósitos) Ligados a Un Movimiento 
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <returns></returns>
        public static DataTable CargaAnticipos(int id_movimiento)
        {
            //Declarando Objeto de Retorno
            DataTable dtAnticipos = null;

            //Armando arreglo de Parametros
            object[] param = { 14, 0, 0, 0, 0, 0, 0, 0, 0, id_movimiento, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {    //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtAnticipos = ds.Tables["Table"];
            }

            //Devolviendo Objeto de Retorno
            return dtAnticipos;
        }

        /// <summary>
        /// Obtiene Ultima Fecha de Carga Diesel
        /// </summary>
        /// <param name="id_unidad">Id de Unidad</param>
        /// <param name="id_vale_actual">Id de Vale Actual</param>
        /// <returns></returns>
        public static DateTime ObtieneUltimaFechaCargaDiesel(int id_unidad,int id_vale_actual)
        {
            //Declaramos variable fecha de última parada
            DateTime fechaUltima = DateTime.MinValue;

            //Armando arreglo de Parametros
            object[] param = { 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, 0, 0, 0, 0, 0, false, id_unidad, id_vale_actual };
            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos  Total
                    fechaUltima = (from DataRow r in ds.Tables[0].Rows
                                   select Convert.ToDateTime(r["FechaCarga"])).DefaultIfEmpty().FirstOrDefault();

                }
            }

            //Obtenemos Resultado
            return fechaUltima;
        }

        /// <summary>
        /// Actualizamos estatus de la Asignación
        /// </summary>
        /// <param name="id_">Estatus</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaOperadorDetalleLiquidacion(int id_operador, int id_usuario)
        {

            //Editamos estatus de la Asignación
            return EditaDetalleLiquidacion(this._id_tabla, this._id_registro,this._id_estatus_liquidacion, this._id_unidad, id_operador, this._id_proveedor_compania, this._id_servicio, 
                this._id_movimiento, this._fecha_liquidacion, this._id_liquidacion, this._cantidad, this._id_unidad_medida,this._valor_unitario, id_usuario );
        }    
        /// <summary>
        /// Obtenie detalles de liquidación por servicio ó movimiento
        /// </summary>
        /// <param name="id_servico"></param>
        /// <param name="id_movimiento"></param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesLiquidacionXServicioMovimiento(int id_servicio)
        {
            DataTable dtDetalles = null;

            //Armando arreglo de Parametros
            object[] param = { 16, 0, 0, 0, 0, 0, 0, 0, id_servicio, 0, null, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Movimientos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {    //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Valor Obtenido
                    dtDetalles = ds.Tables["Table"];
            }

            return dtDetalles;
        }
        #endregion
    }
}
