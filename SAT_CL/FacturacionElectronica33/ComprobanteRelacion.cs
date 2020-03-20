using SAT_CL.Facturacion;
using System;
using System.Data;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de las Operaciones relacionadas con la Relación de Comprobante
    /// </summary>
    public class ComprobanteRelacion : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// 
        /// </summary>
        public enum TipoRelacion
        {
            /// <summary>
            /// Nota de crédito de los documentos relacionados
            /// </summary>
            NotaCredito = 1,
            /// <summary>
            /// Nota de débito de los documentos relacionados
            /// </summary>
            NotaDebito = 2,
            /// <summary>
            /// Devolución de mercancía sobre facturas o traslados previos
            /// </summary>
            Devolucion = 3,
            /// <summary>
            /// Sustitución de los CFDI previos
            /// </summary>
            Sustitucion = 4,
            /// <summary>
            /// Traslados de mercancias facturados previamente
            /// </summary>
            Traslados = 5,
            /// <summary>
            /// Factura generada por los traslados previos
            /// </summary>
            FacturaGenerada = 6
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_comprobante_relacion_tcr";

        private int _id_comprobante_relacion;
        /// <summary>
        /// Atributo encargado de obtener el Identificador de la Relación del Comprobante 
        /// </summary>
        public int id_comprobante_relacion { get { return this._id_comprobante_relacion; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de obtener el Comprobante
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private int _id_comprobante_relacionado;
        /// <summary>
        /// Atributo encargado de obtener el Comprobante Relacionado
        /// </summary>
        public int id_comprobante_relacionado { get { return this._id_comprobante_relacionado; } }
        private byte _id_tipo_relacion;
        /// <summary>
        /// Atributo encargado de obtener el Tipo de Relación del Comprobante (SAT)
        /// </summary>
        public byte id_tipo_relacion { get { return this._id_tipo_relacion; } }
        private int _secuencia_relacion;
        /// <summary>
        /// Atributo encargado de obtener la Secuencia de Relación del Comprobante
        /// </summary>
        public int secuencia_relacion { get { return this._secuencia_relacion; } }
        private int _id_aplicacion;
        /// <summary>
        /// Atributo encargado de obtener la Aplicación del Comprobante
        /// </summary>
        public int id_aplicacion { get { return this._id_aplicacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de obtener el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ComprobanteRelacion()
        {
            //Asignando Valores
            this._id_comprobante_relacion =
            this._id_comprobante =
            this._id_comprobante_relacionado = 0;
            this._id_tipo_relacion = 0;
            this._secuencia_relacion =
            this._id_aplicacion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobante_relacion">Relación del Comprobante</param>
        public ComprobanteRelacion(int id_comprobante_relacion)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_comprobante_relacion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ComprobanteRelacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobante_relacion">Relación del Comprobante</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_comprobante_relacion)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_comprobante_relacion, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_comprobante_relacion = id_comprobante_relacion;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._id_comprobante_relacionado = Convert.ToInt32(dr["IdComprobanteRelacionado"]);
                        this._id_tipo_relacion = Convert.ToByte(dr["IdTipoRelacion"]);
                        this._secuencia_relacion = Convert.ToInt32(dr["SecuenciaRelacion"]);
                        this._id_aplicacion = Convert.ToInt32(dr["IdAplicacion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_comprobante">Comprobante</param>
        /// <param name="id_comprobante_relacionado">Comprobante Relacionado</param>
        /// <param name="id_tipo_relacion">Tipo de Relación</param>
        /// <param name="secuencia_relacion">Secuencia de la Relación del Comprobante</param>
        /// <param name="id_aplicacion">Aplicación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_comprobante, int id_comprobante_relacionado, byte id_tipo_relacion, 
                                                      int secuencia_relacion, int id_aplicacion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_comprobante_relacion, id_comprobante, id_comprobante_relacionado, id_tipo_relacion, 
                               secuencia_relacion, id_aplicacion, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Relaciones de los Comprobantes
        /// </summary>
        /// <param name="id_comprobante">Comprobante</param>
        /// <param name="id_comprobante_relacionado">Comprobante Relacionado</param>
        /// <param name="id_tipo_relacion">Tipo de Relación</param>
        /// <param name="secuencia_relacion">Secuencia de la Relación del Comprobante</param>
        /// <param name="id_aplicacion">Aplicación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobanteRelacion(int id_comprobante, int id_comprobante_relacionado, byte id_tipo_relacion,
                                                            int secuencia_relacion, int id_aplicacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobante, id_comprobante_relacionado, id_tipo_relacion, 
                               secuencia_relacion, id_aplicacion, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Relaciones de los Comprobantes
        /// </summary>
        /// <param name="id_comprobante">Comprobante</param>
        /// <param name="id_comprobante_relacionado">Comprobante Relacionado</param>
        /// <param name="id_tipo_relacion">Tipo de Relación</param>
        /// <param name="secuencia_relacion">Secuencia de la Relación del Comprobante</param>
        /// <param name="id_aplicacion">Aplicación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaComprobanteRelacion(int id_comprobante, int id_comprobante_relacionado, byte id_tipo_relacion,
                                                         int secuencia_relacion, int id_aplicacion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_comprobante, id_comprobante_relacionado, id_tipo_relacion, 
                                             secuencia_relacion, id_aplicacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Relaciones de los Comprobantes
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshbilitaComprobanteRelacion(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_comprobante, this._id_comprobante_relacionado, this._id_tipo_relacion,
                                             this._secuencia_relacion, this._id_aplicacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar las Relaciones de los Comprobantes
        /// </summary>
        /// <returns></returns>
        public bool ActualizaComprobanteRelacion()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_comprobante_relacion);
        }
        /// <summary>
        /// Método encargado de Obtener las Relaciones del Comprobante
        /// </summary>
        /// <param name="id_comprobante33">Comprobante v3.3</param>
        /// <returns></returns>
        public static DataTable ObtieneRelacionesComprobante(int id_comprobante33)
        {
            //Declarando Objeto de Retorno
            DataTable dtCFDIRelacionados = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_comprobante33, 0, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtCFDIRelacionados = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtCFDIRelacionados;
        }
        /// <summary>
        /// Devuleve el primer Comprobante encontrado como sustituido por el CFDI proporcionado
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante a consultar (CFDI activo que sustituye a otro)</param>
        /// <returns></returns>
        public static Comprobante ObtenerCFDISustituido(int id_comprobante)
        {
            //Declarando objto de retorno
            Comprobante cfdiSustituido = new Comprobante();

            //Recuperando lista de relaciones
            using (DataTable mit = ObtieneRelacionesComprobante(id_comprobante))
            {
                //Si hay elementos
                if (mit != null)
                {
                    //Localizando el tipo de relación deseado
                    ComprobanteRelacion relacion = new ComprobanteRelacion((from DataRow r in mit.Rows
                                                      where r.Field<string>("TipoRelacion") == "04"
                                                      select r.Field<int>("Id")).DefaultIfEmpty(0).First());

                    //Si hay una relación
                    if( relacion.habilitar)
                        cfdiSustituido = new Comprobante(relacion.id_comprobante_relacionado);
                }
            }

            //Devolviendo resutado
            return cfdiSustituido;
        }
        /// <summary>
        /// Método encargado de Configurar las Relaciones de los Comprobantes en base al Comprobante Vigente Actual
        /// </summary>
        /// <param name="id_facturado">Facturado</param>
        /// <param name="id_cfdi_33">Comprobante Nuevo v3.3</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion ConfiguraRelacionComprobanteCancelacionFacturado(int id_facturado, int id_cfdi_33, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Factura Electronica Cancelada
                int idFE33Cancelada = FacturadoFacturacion.ObtieneFacturacionElectronicaCanceladaV3_3(id_facturado);

                //Validando existencia de una Cancelación Previa
                if (idFE33Cancelada > 0)
                {
                    //Obteniendo Relaciones del Comprobante Cancelado
                    using (DataTable dtRelaciones = ComprobanteRelacion.ObtieneRelacionesComprobante(idFE33Cancelada))
                    {
                        //Validando Relaciones
                        if (Validacion.ValidaOrigenDatos(dtRelaciones))
                        {
                            //Recorriendo Relaciones
                            foreach (DataRow dr in dtRelaciones.Rows)
                            {
                                //Instanciando Relación
                                using (ComprobanteRelacion cr = new ComprobanteRelacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando Relación
                                    if (cr.habilitar)

                                        //Actualizando Comprobante
                                        result = cr.EditaComprobanteRelacion(id_cfdi_33, cr.id_comprobante_relacionado, cr.id_tipo_relacion, cr.secuencia_relacion, cr.id_aplicacion, id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No se puede recuperar la Relación");

                                    //Validando Operación
                                    if (!result.OperacionExitosa)

                                        //Terminando Ciclo
                                        break;
                                }
                            }

                        }
                        else
                            //Instanciando Resultado Positivo
                            result = new RetornoOperacion(0, "No existe ninguna relación previa", true);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        
                            //Insertando Nueva Relación de Cancelación
                            result = ComprobanteRelacion.InsertaComprobanteRelacion(id_cfdi_33, idFE33Cancelada, 4, 0, 0, id_usuario);
                    }
                }
                else
                    //Instanciando Resultado Positivo
                    result = new RetornoOperacion(0, "No existe ninguna cancelación previa", true);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)

                    //Completando transacción
                    scope.Complete();
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Configurar las Relaciones de los Comprobantes en base al Comprobante Vigente Actual
        /// </summary>
        /// <param name="id_factura_global">Factura Global</param>
        /// <param name="id_cfdi_33">Comprobante Nuevo v3.3</param>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion ConfiguraRelacionComprobanteCancelacionFacturaGlobal(int id_factura_global, int id_cfdi_33, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obteniendo Factura Electronica Cancelada
                int idFE33Cancelada = FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobalCanceladaV3_3(id_factura_global);

                //Validando existencia de una Cancelación Previa
                if (idFE33Cancelada > 0)
                {
                    //Obteniendo Relaciones del Comprobante Cancelado
                    using (DataTable dtRelaciones = ComprobanteRelacion.ObtieneRelacionesComprobante(idFE33Cancelada))
                    {
                        //Validando Relaciones
                        if (Validacion.ValidaOrigenDatos(dtRelaciones))
                        {
                            //Recorriendo Relaciones
                            foreach (DataRow dr in dtRelaciones.Rows)
                            {
                                //Instanciando Relación
                                using (ComprobanteRelacion cr = new ComprobanteRelacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando Relación
                                    if (cr.habilitar)

                                        //Actualizando Comprobante
                                        result = cr.EditaComprobanteRelacion(id_cfdi_33, cr.id_comprobante_relacionado, cr.id_tipo_relacion, cr.secuencia_relacion, cr.id_aplicacion, id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No se puede recuperar la Relación");

                                    //Validando Operación
                                    if (!result.OperacionExitosa)

                                        //Terminando Ciclo
                                        break;
                                }
                            }

                        }
                        else
                            //Instanciando Resultado Positivo
                            result = new RetornoOperacion(0, "No existe ninguna relación previa", true);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Insertando Nueva Relación de Cancelación
                            result = ComprobanteRelacion.InsertaComprobanteRelacion(id_cfdi_33, idFE33Cancelada, 4, 0, 0, id_usuario);
                        }
                    }
                }
                else
                    //Instanciando Resultado Positivo
                    result = new RetornoOperacion(0, "No existe ninguna cancelación previa", true);

                //Validando Operación Exitosa
                if (result.OperacionExitosa)

                    //Completando transacción
                    scope.Complete();
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
