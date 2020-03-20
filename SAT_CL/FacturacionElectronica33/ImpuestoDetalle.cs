using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con los Detalles de los Impuestos
    /// </summary>
    public class ImpuestoDetalle : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Enumera los Tipos de Impuestos
        /// </summary>
        public enum TipoImpuestoDetalle
        {
            /// <summary>
            /// Retenido
            /// </summary>
            Retenido = 1,
            /// <summary>
            /// Trasladado
            /// </summary>
            Trasladado
        }

        #endregion
        
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_impuesto_detalle_tid";

        private int _id_impuesto_detalle;
        /// <summary>
        /// Atributo encargado de Obtener el Identificador del Detalle del Impuesto
        /// </summary>
        public int id_impuesto_detalle { get { return this._id_impuesto_detalle; } }
        private int _id_impuesto;
        /// <summary>
        /// Atributo encargado de Obtener el Impuesto al que Pertenece el Detalle
        /// </summary>
        public int id_impuesto { get { return this._id_impuesto; } }
        private int _id_tipo_detalle;
        /// <summary>
        /// Atributo encargado de Obtener el Tipo de Detalle (Retenido, Trasladado)
        /// </summary>
        public int id_tipo_detalle { get { return this._id_tipo_detalle; } }
        /// <summary>
        /// Atributo encargado de Obtener el Tipo de Detalle (Retenido, Trasladado) Enumeración
        /// </summary>
        public TipoImpuestoDetalle tipo_detalle { get { return (TipoImpuestoDetalle)this._id_tipo_detalle; } }
        private int _id_concepto;
        /// <summary>
        /// Atributo encargado de Obtener el Concepto
        /// </summary>
        public int id_concepto { get { return this._id_concepto; } }
        private byte _id_tipo_factor;
        /// <summary>
        /// Atributo encargado de Obtener el Tipo de Factor (SAT)
        /// </summary>
        public byte id_tipo_factor { get { return this._id_tipo_factor; } }
        private decimal _importe_base;
        /// <summary>
        /// Atributo encargado de Obtener el Importe Base del Concepto
        /// </summary>
        public decimal importe_base { get { return this._importe_base; } }
        private byte _id_impuesto_retenido;
        /// <summary>
        /// Atributo encargado de Obtener el Impuesto Retenido (IVA, IEPS, ISR)
        /// </summary>
        public byte id_impuesto_retenido { get { return this._id_impuesto_retenido; } }
        private byte _id_impuesto_trasladado;
        /// <summary>
        /// Atributo encargado de Obtener el Impuesto Retenido (IVA, IEPS)
        /// </summary>
        public byte id_impuesto_trasladado { get { return this._id_impuesto_trasladado; } }
        private decimal _tasa;
        /// <summary>
        /// Atributo encargado de Obtener la Tasa del Concepto
        /// </summary>
        public decimal tasa { get { return this._tasa; } }
        private decimal _importe_captura;
        /// <summary>
        /// Atributo encargado de Obtener el Importe de Captura del Concepto
        /// </summary>
        public decimal importe_captura { get { return this._importe_captura; } }
        private decimal _importe_nacional;
        /// <summary>
        /// Atributo encargado de Obtener el Importe Nacional del Concepto
        /// </summary>
        public decimal importe_nacional { get { return this._importe_nacional; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Obtener el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ImpuestoDetalle()
        {
            //Asignando Valores
            this._id_impuesto_detalle =
            this._id_impuesto =
            this._id_tipo_detalle =
            this._id_concepto = 0;
            this._id_tipo_factor = 0;
            this._importe_base = 0.00M;
            this._id_impuesto_retenido =
            this._id_impuesto_trasladado = 0;
            this._tasa = 
            this._importe_captura =
            this._importe_nacional = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_impuesto_detalle">Detalle del Impuesto</param>
        public ImpuestoDetalle(int id_impuesto_detalle)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_impuesto_detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ImpuestoDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_impuesto_detalle">Detalle del Impuesto</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_impuesto_detalle)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_impuesto_detalle, 0, 0, 0, 0, 0.00M, 0, 0, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Obteniendo Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_impuesto_detalle = id_impuesto_detalle;
                        this._id_impuesto = Convert.ToInt32(dr["IdImpuesto"]);
                        this._id_tipo_detalle = Convert.ToInt32(dr["IdTipoDetalle"]);
                        this._id_concepto = Convert.ToInt32(dr["IdConcepto"]);
                        this._id_tipo_factor = Convert.ToByte(dr["IdTipoFactor"]);
                        this._importe_base = Convert.ToDecimal(dr["Base"]);
                        this._id_impuesto_retenido = Convert.ToByte(dr["IdImpuestoRetenido"]);
                        this._id_impuesto_trasladado = Convert.ToByte(dr["IdImpuestoTrasladado"]);
                        this._tasa = Convert.ToDecimal(dr["Tasa"]);
                        this._importe_captura = Convert.ToDecimal(dr["ImporteCaptura"]);
                        this._importe_nacional = Convert.ToDecimal(dr["ImporteNacional"]);
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
        /// <param name="id_impuesto">Impuesto al que pertenece el Detalle</param>
        /// <param name="tipo_detalle">Tipo de Detalle (Traslado, Retención)</param>
        /// <param name="id_concepto">Concepto del Detalle de Impuesto</param>
        /// <param name="id_tipo_factor">Tipo de Factura (SAT)</param>
        /// <param name="importe_base">Base del Concepto</param>
        /// <param name="id_impuesto_retenido">Impuesto Retenido (IVA, IEPS, ISR)</param>
        /// <param name="id_impuesto_trasladado">Impuesto Trasladado (IVA, IEPS)</param>
        /// <param name="tasa">Tasa del Impuesto</param>
        /// <param name="importe_captura">Importe en Moneda de Captura</param>
        /// <param name="importe_nacional">Importe en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Detalle de Impuesto</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_impuesto, TipoImpuestoDetalle tipo_detalle, int id_concepto, int id_tipo_factor, decimal importe_base, 
                                                    byte id_impuesto_retenido, byte id_impuesto_trasladado, decimal tasa, decimal importe_captura, 
                                                    decimal importe_nacional, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_impuesto_detalle, id_impuesto, (int)tipo_detalle, id_concepto, id_tipo_factor, importe_base, 
                               id_impuesto_retenido, id_impuesto_trasladado, tasa, importe_captura, importe_nacional, id_usuario, 
                               habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Detalles de Impuestos
        /// </summary>
        /// <param name="id_impuesto">Impuesto al que pertenece el Detalle</param>
        /// <param name="tipo_detalle">Tipo de Detalle (Traslado, Retención)</param>
        /// <param name="id_concepto">Concepto del Detalle de Impuesto</param>
        /// <param name="id_tipo_factor">Tipo de Factura (SAT)</param>
        /// <param name="importe_base">Base del Concepto</param>
        /// <param name="id_impuesto_retenido">Impuesto Retenido (IVA, IEPS, ISR)</param>
        /// <param name="id_impuesto_trasladado">Impuesto Trasladado (IVA, IEPS)</param>
        /// <param name="tasa">Tasa del Impuesto</param>
        /// <param name="importe_captura">Importe en Moneda de Captura</param>
        /// <param name="importe_nacional">Importe en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaImpuestoDetalle(int id_impuesto, TipoImpuestoDetalle tipo_detalle, int id_concepto, int id_tipo_factor, 
                                                       decimal importe_base, byte id_impuesto_retenido, byte id_impuesto_trasladado, decimal tasa, 
                                                       decimal importe_captura, decimal importe_nacional, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_impuesto, (int)tipo_detalle, id_concepto, id_tipo_factor, importe_base, 
                               id_impuesto_retenido, id_impuesto_trasladado, tasa, importe_captura, importe_nacional, id_usuario, 
                               true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Insertar los Detalles de Impuestos
        /// </summary>
        /// <param name="id_impuesto">Impuesto al que pertenece el Detalle</param>
        /// <param name="tipo_detalle">Tipo de Detalle (Traslado, Retención)</param>
        /// <param name="id_concepto">Concepto del Detalle de Impuesto</param>
        /// <param name="id_tipo_factor">Tipo de Factura (SAT)</param>
        /// <param name="importe_base">Base del Concepto</param>
        /// <param name="id_impuesto_retenido">Impuesto Retenido (IVA, IEPS, ISR)</param>
        /// <param name="id_impuesto_trasladado">Impuesto Trasladado (IVA, IEPS)</param>
        /// <param name="tasa">Tasa del Impuesto</param>
        /// <param name="importe_captura">Importe en Moneda de Captura</param>
        /// <param name="importe_nacional">Importe en Moneda Nacional</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaImpuestoDetalle(int id_impuesto, TipoImpuestoDetalle tipo_detalle, int id_concepto, int id_tipo_factor,
                                                     decimal importe_base, byte id_impuesto_retenido, byte id_impuesto_trasladado, decimal tasa,
                                                     decimal importe_captura, decimal importe_nacional, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_impuesto, tipo_detalle, id_concepto, id_tipo_factor, importe_base, id_impuesto_retenido, 
                                             id_impuesto_trasladado, tasa, importe_captura, importe_nacional, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Detalles de Impuestos
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaImpuestoDetalle(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_impuesto, (TipoImpuestoDetalle)this._id_tipo_detalle, this._id_concepto, this._id_tipo_factor, 
                                             this._importe_base, this._id_impuesto_retenido, this._id_impuesto_trasladado, this._tasa, 
                                             this._importe_captura, this._importe_nacional, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de
        /// </summary>
        /// <returns></returns>
        public bool ActualizaImpuestoDetalle()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_impuesto_detalle);
        }
        /// <summary>
        /// Método encargado de Obtener los Detalles dado un Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <returns></returns>
        public static DataTable ObtieneDetallesImpuesto(int id_impuesto)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_impuesto, 0, 0, 0, 0.00M, 0, 0, 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Obteniendo Datos del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Valores
                    dtDetalles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtDetalles;
        }

        #endregion
    }
}
