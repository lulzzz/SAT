using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Xml.Linq;
using System.Linq;
using System.Transactions;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con los Impuestos
    /// </summary>
    public class Impuesto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_impuesto_ti";

        private int _id_impuesto;
        /// <summary>
        /// Atributo encargado de Obtener el Identificador del Impuesto
        /// </summary>
        public int id_impuesto { get { return this._id_impuesto; } }
        private int _id_comprobante;
        /// <summary>
        /// Atributo encargado de Obtener el Comprobante del Impuesto
        /// </summary>
        public int id_comprobante { get { return this._id_comprobante; } }
        private decimal _total_trasladado_captura;
        /// <summary>
        /// Atributo encargado de Obtener el Total Trasladado en Moneda de Captura del Impuesto
        /// </summary>
        public decimal total_trasladado_captura { get { return this._total_trasladado_captura; } }
        private decimal _total_traslado_nacional;
        /// <summary>
        /// Atributo encargado de Obtener el Total Trasladado en Moneda Nacional del Impuesto
        /// </summary>
        public decimal total_traslado_nacional { get { return this._total_traslado_nacional; } }
        private decimal _total_retenido_captura;
        /// <summary>
        /// Atributo encargado de Obtener el Total Retenido en Moneda de Captura del Impuesto
        /// </summary>
        public decimal total_retenido_captura { get { return this._total_retenido_captura; } }
        private decimal _total_retenido_nacional;
        /// <summary>
        /// Atributo encargado de Obtener el Total Retenido en Moneda Nacional del Impuesto
        /// </summary>
        public decimal total_retenido_nacional { get { return this._total_retenido_nacional; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Obtener el Estatus Habilitar del Impuesto
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Impuesto()
        {
            //Asignando Valores
            this._id_impuesto = 
            this._id_comprobante = 0;
            this._total_trasladado_captura =
            this._total_traslado_nacional =
            this._total_retenido_captura =
            this._total_retenido_nacional = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_impuesto">Impuesto</param>
        public Impuesto(int id_impuesto)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_impuesto);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Impuesto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_impuesto)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_impuesto, 0, 0.00M, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Obteniendo Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_impuesto = id_impuesto;
                        this._id_comprobante = Convert.ToInt32(dr["IdComprobante"]);
                        this._total_trasladado_captura = Convert.ToDecimal(dr["TotalTrasladadoCaptura"]);
                        this._total_traslado_nacional = Convert.ToDecimal(dr["TotalTrasladadoNacional"]);
                        this._total_retenido_captura = Convert.ToDecimal(dr["TotalRetenidoCaptura"]);
                        this._total_retenido_nacional = Convert.ToDecimal(dr["TotalRetenidoNacional"]);
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
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_comprobante">Comprobante v3.3</param>
        /// <param name="total_trasladado_captura">Total de Impuestos Trasladados en Moneda de Captura</param>
        /// <param name="total_traslado_nacional">Total de Impuestos Trasladados en Moneda Nacional</param>
        /// <param name="total_retenido_captura">Total de Impuestos Retenidos en Moneda de Captura</param>
        /// <param name="total_retenido_nacional">Total de Impuestos Retenidos en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_comprobante, decimal total_trasladado_captura, decimal total_traslado_nacional, 
                                                      decimal total_retenido_captura, decimal total_retenido_nacional, int id_usuario, 
                                                      bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_impuesto, id_comprobante, total_trasladado_captura, total_traslado_nacional, total_retenido_captura, 
                               total_retenido_nacional, id_usuario, habilitar, "", "" };

            //Actualizando Registro
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Impuestos
        /// </summary>
        /// <param name="id_comprobante">Comprobante v3.3</param>
        /// <param name="total_trasladado_captura">Total de Impuestos Trasladados en Moneda de Captura</param>
        /// <param name="total_traslado_nacional">Total de Impuestos Trasladados en Moneda Nacional</param>
        /// <param name="total_retenido_captura">Total de Impuestos Retenidos en Moneda de Captura</param>
        /// <param name="total_retenido_nacional">Total de Impuestos Retenidos en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaImpuesto(int id_comprobante, decimal total_trasladado_captura, decimal total_traslado_nacional,
                                                decimal total_retenido_captura, decimal total_retenido_nacional, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobante, total_trasladado_captura, total_traslado_nacional, total_retenido_captura, 
                               total_retenido_nacional, id_usuario, true, "", "" };

            //Actualizando Registro
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Impuestos
        /// </summary>
        /// <param name="id_comprobante">Comprobante v3.3</param>
        /// <param name="total_trasladado_captura">Total de Impuestos Trasladados en Moneda de Captura</param>
        /// <param name="total_traslado_nacional">Total de Impuestos Trasladados en Moneda Nacional</param>
        /// <param name="total_retenido_captura">Total de Impuestos Retenidos en Moneda de Captura</param>
        /// <param name="total_retenido_nacional">Total de Impuestos Retenidos en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaImpuesto(int id_comprobante, decimal total_trasladado_captura, decimal total_traslado_nacional,
                                                decimal total_retenido_captura, decimal total_retenido_nacional, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_comprobante, total_trasladado_captura, total_traslado_nacional, total_retenido_captura, 
                               total_retenido_nacional, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Impuestos
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaImpuesto(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(0);

            //Obteniendo Detalles de Impuesto
            using (DataTable dtDetalle = ImpuestoDetalle.ObtieneDetallesImpuesto(this._id_impuesto))
            {
                //Validando Existencias
                if (Validacion.ValidaOrigenDatos(dtDetalle))
                {
                    //Inicializando Transacción
                    using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorriendo Detalles
                        foreach (DataRow dr in dtDetalle.Rows)
                        {
                            //Instanciando Detalle
                            using (ImpuestoDetalle id = new ImpuestoDetalle(Convert.ToInt32(dr["Id"])))
                            {
                                //Validando Existencia
                                if (id.habilitar)
                                
                                    //Deshabilitando Detalle
                                    result = id.DeshabilitaImpuestoDetalle(id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No se puede Recuperar el Detalle del Impuesto");

                                //Validando Resultado
                                if (!result.OperacionExitosa)

                                    //Terminando Ciclo
                                    break;
                            }
                        }

                        //Validando Resultado
                        if (result.OperacionExitosa)
                        {
                            //Devolviendo Resultado Obtenido
                            result = this.actualizaRegistrosBD(this._id_comprobante, this._total_trasladado_captura, this._total_traslado_nacional,
                                                             this._total_retenido_captura, this._total_retenido_nacional, id_usuario, false);

                            //Validando Resultado
                            if (result.OperacionExitosa)

                                //Completando Transacción
                                scope.Complete();
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de los Impuestos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaImpuesto()
        {
            //Devolviendo Resultado Obtenido
            return this.cargaAtributosInstancia(this._id_impuesto);
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales del Encabezado de Impuestos
        /// </summary>
        /// <param name="id_usuario">Usuario que actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalesImpuesto(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Totales del Comprobante
            using (DataTable dtTotales = Comprobante.ObtieneTotalImpuestos(this._id_comprobante))
            {
                //Validando Existencia
                if (Validacion.ValidaOrigenDatos(dtTotales))
                
                    //Editando Impuesto
                    result = this.EditaImpuesto(this._id_comprobante, (from DataRow r in dtTotales.Rows select Convert.ToDecimal(r["TotalTrasladoCaptura"])).Sum(),
                                                                      (from DataRow r in dtTotales.Rows select Convert.ToDecimal(r["TotalTrasladoNacional"])).Sum(),
                                                                      (from DataRow r in dtTotales.Rows select Convert.ToDecimal(r["TotalRetenidoCaptura"])).Sum(),
                                                                      (from DataRow r in dtTotales.Rows select Convert.ToDecimal(r["TotalRetenidoNacional"])).Sum(),
                                                                      id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se encontraron los Totales");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <returns></returns>
        public static Impuesto ObtieneImpuestoComprobante(int id_comprobante)
        {
            //Declarando Objeto de Retorno
            Impuesto impuesto = new Impuesto();

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_comprobante, 0.00M, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Actualizando Registro
            //Obteniendo Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Instanciando Impuesto
                        impuesto = new Impuesto(Convert.ToInt32(dr["Id"]));

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return impuesto;
        }

        #endregion
    }
}
