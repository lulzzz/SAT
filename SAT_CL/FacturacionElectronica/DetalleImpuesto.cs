using System;
using System.Data;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Descripción breve de DetalleImpuesto
    /// </summary>
    public class DetalleImpuesto : Disposable
    {
        #region Atributos

        private int _id_detalle_impuesto;
        private int _id_impuesto;
        private int _id_tipo_detalle;
        private int _id_impuesto_retenido;
        private int _id_impuesto_trasladado;
        private decimal _tasa;
        private decimal _importe_moneda_captura;
        private decimal _importe_moneda_nacional;
        private bool _habilitar;
        private static string _nombre_stored_procedure = "fe.sp_impuesto_detalle_tdi";

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el identificador del objeto
        /// </summary>
        public int id_detalle_impuesto
        {
            get
            {
                return _id_detalle_impuesto;
            }
        }

        /// <summary>
        /// Obtiene el impuesto al que pertenece el detalle
        /// </summary>
        public int id_impuesto
        {
            get
            {
                return _id_impuesto;
            }
        }

        /// <summary>
        /// Obtiene el tipo al que pertenece el detalle
        /// </summary>
        public int id_tipo_detalle
        {
            get
            {
                return _id_tipo_detalle;
            }
        }

        /// <summary>
        /// Obtiene eñ id de impuesto retenido
        /// </summary>
        public int id_impuesto_retenido
        {
            get
            {
                return _id_impuesto_retenido;
            }
        }

        /// <summary>
        /// Obtiene el id de impuesto trasladado
        /// </summary>
        public int id_impuesto_trasladado
        {
            get
            {
                return _id_impuesto_trasladado;
            }
        }

        /// <summary>
        /// Obtiene la tasa del impuesto
        /// </summary>
        public decimal tasa
        {
            get
            {
                return _tasa;
            }
        }

        /// <summary>
        /// Obtiene el importe en monea de captura
        /// </summary>
        public decimal importe_moneda_captura
        {
            get
            {
                return _importe_moneda_captura;
            }
        }

        /// <summary>
        /// Obtiene el importe en moneda nacional
        /// </summary>
        public decimal importe_moneda_nacional
        {
            get
            {
                return _importe_moneda_nacional;
            }
        }

        /// <summary>
        /// Obtiene un avlor que indica si el regsitro esta habilitado
        /// </summary>
        public bool habilitar
        {
            get
            {
                return _habilitar;
            }
        }
        /// <summary>
        /// Tipo de Impuesto
        /// </summary>
        public TipoImpuestoDetalle TipoImpuestosDetalle { get { return (TipoImpuestoDetalle)_id_tipo_detalle; } }

        #endregion

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

        #region Constructores

        /// <summary>
        /// Instancía un detalle de impuesto con los valores por default
        /// </summary>
        public DetalleImpuesto()
        {
            this._id_detalle_impuesto = 0;
            this._id_impuesto = 0;
            this._id_tipo_detalle = 0;
            this._id_impuesto_retenido = 0;
            this._id_impuesto_trasladado = 0;
            this._tasa = 0;
            this._importe_moneda_captura = 0;
            this._importe_moneda_nacional = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Genera una nueva instancia del tipo Detalle Impuesto
        /// </summary>
        /// <param name="id_detalle_impuesto"></param>
        public DetalleImpuesto(int id_detalle_impuesto)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_detalle_impuesto);
        }

        /// <summary>
        /// Instancía un detalle de impuesto ligado a un Id
        /// </summary>
        /// <param name="detalle_impuesto"></param>
        public bool cargaAtributosInstancia(int id_detalle_impuesto)
        {
            //Declarando variable de retorno
            bool resultado = false;
            //Inicializando parametros
            object[] parametros = { 3, id_detalle_impuesto, 0, 0, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando datset con consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo als filas
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando valores                    
                        _id_detalle_impuesto = Convert.ToInt32(r["Id"]);
                        _id_impuesto = Convert.ToInt32(r["IdImpuesto"]);
                        _id_impuesto_retenido = Convert.ToInt32(r["IdImpuestoRetenido"]);
                        _id_impuesto_trasladado = Convert.ToInt32(r["IdImpuestoTrasladado"]);
                        _id_tipo_detalle = Convert.ToInt32(r["IdTipo"]);
                        _importe_moneda_captura = Convert.ToDecimal(r["ImporteMonedaCaptura"]);
                        _importe_moneda_nacional = Convert.ToDecimal(r["ImporteMonedaNacional"]);
                        _tasa = Convert.ToDecimal(r["Tasa"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                        resultado = true;
                    }
                }
            }
            return resultado;
        }

           #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~DetalleImpuesto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos privados

        /// <summary>
        /// Edito un  Impuesto Detalle
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="id_tipo_detalle"></param>
        /// <param name="id_impuesto_retenido"></param>
        /// <param name="id_impuesto_trasladado"></param>
        /// <param name="tasa"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaDetalleImpuesto(int id_impuesto, int id_tipo_detalle, int id_impuesto_retenido,
                                                          int id_impuesto_trasladado, decimal tasa, decimal importe_moneda_captura,
                                                          decimal importe_moneda_nacional, int id_usuario, bool habilitar)
        {


            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_detalle_impuesto, id_impuesto, id_tipo_detalle, id_impuesto_retenido, id_impuesto_trasladado, tasa,
                                 importe_moneda_captura, importe_moneda_nacional, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        #endregion

        #region Métodos publicos


        /// <summary>
        /// Inserta un Detalle Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="id_tipo_detalle"></param>
        /// <param name="id_impuesto_retenido"></param>
        /// <param name="id_impuesto_trasladado"></param>
        /// <param name="tasa"></param>
        /// <param name="id_usuario"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="id_conceptos"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleImpuesto(int id_impuesto, int id_tipo_detalle, int id_impuesto_retenido,
                                                                  int id_impuesto_trasladado, decimal tasa,
                                                                   int id_usuario, int id_comprobante, int[] id_conceptos)
        {
            //Declaramos Variable Impuesto 
            int id_impuesto_registro = id_impuesto;
            int id_impuesto_detalle_registro = 0;
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Comprobante
               using (Comprobante objComprobante = new Comprobante(id_comprobante))
                {

                    //Validamos Existencia de Impuesto
                    if (id_impuesto <= 0)
                    {
                        //Insertamos Impuesto
                        resultado = Impuesto.InsertaImpuesto(id_comprobante, 0, 0, 0, 0, id_usuario);

                        //Asignamod Id Impuesto
                        id_impuesto_registro = resultado.IdRegistro;


                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inserta Detalle de Impuesto
                        resultado = InsertaDetalleImpuesto(id_impuesto_registro, id_tipo_detalle, id_impuesto_retenido, id_impuesto_trasladado,
                                                          tasa, 0, 0, id_usuario);
                        //Asignamod Id Impuesto
                        id_impuesto_detalle_registro = resultado.IdRegistro;


                        //Si se inserta el Detalle Impuestos
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos Existencia de Conceptos
                            if (id_conceptos != null)
                            {
                                decimal impuesto_captura = 0, impuesto_nacional = 0,
                                        descuento_importe_moneda_nacional = 0,
                                        descuento_importe_moneda_captura = 0;


                                //Guardamos cada uno de los Conceptos
                                foreach (int id_concepto in id_conceptos)
                                {
                                    //Si el resultado es Exitoso
                                    if (resultado.OperacionExitosa)
                                    {

                                        //Instanciamos Concepto
                                        using (Concepto objconcepto = new Concepto(id_concepto))
                                        {
                                            //Obtenemos el Descuento Por Concepto
                                            objComprobante.ObtieneDescuentoPorConcepto(objconcepto.importe_moneda_captura, objconcepto.importe_moneda_nacional, out descuento_importe_moneda_captura, out descuento_importe_moneda_nacional);

                                            //Calculamos Impuesto Captura
                                            impuesto_captura = (objconcepto.importe_moneda_captura - descuento_importe_moneda_captura) * (tasa / 100);

                                            //Calculamos Impuesto Nacional
                                            impuesto_nacional = (objconcepto.importe_moneda_nacional - descuento_importe_moneda_nacional) * (tasa / 100);

                                            //Insertamos Concepto Detalle Impuesto
                                            resultado = ConceptoDetalleImpuesto.InsertarConceptoDetalleImpuesto(id_concepto, id_impuesto_detalle_registro,
                                                                                                                  impuesto_captura, impuesto_nacional, id_usuario);
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            //Validamos Inserccion de Concepto Detalle Impuesto
                            if (resultado.OperacionExitosa)
                            {
                                //Actualizamos Total Detalle Impuesto
                                using (DetalleImpuesto objDetalleImpuesto = new DetalleImpuesto(id_impuesto_detalle_registro))
                                {
                                    //Actualizamos Total Detalles
                                    resultado = objDetalleImpuesto.ActualizaTotalDetalleImpuesto(id_usuario);

                                }
                                //Si se actualizo correctamente Detalle Impuesto
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Impuesto
                                    using (Impuesto objImpuesto = new Impuesto(id_impuesto_registro))
                                    {
                                        resultado = objImpuesto.ActualizaTotalImpuesto(id_usuario);
                                    }
                                }
                                //Si se actualizo correctamente el  Impuesto
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualizamos Instnacia Comprobante
                                    resultado = objComprobante.ActualizaImpuestosComprobante(id_impuesto_registro, id_usuario);

                                }

                            }
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Asignando Resultado general con Id de Detalle insertado
                    resultado = new RetornoOperacion(id_impuesto_detalle_registro);
                    //Finalizamos transaccion
                    scope.Complete();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Inserta un Detalle Impuesto 
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="id_tipo_detalle"></param>
        /// <param name="id_impuesto_retenido"></param>
        /// <param name="id_impuesto_trasladado"></param>
        /// <param name="tasa"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDetalleImpuesto(int id_impuesto, int id_tipo_detalle, int id_impuesto_retenido,
                                                                  int id_impuesto_trasladado, decimal tasa, decimal importe_moneda_captura,
                                                                  decimal importe_moneda_nacional, int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();


            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_impuesto, id_tipo_detalle, id_impuesto_retenido, 
                                    id_impuesto_trasladado, tasa, importe_moneda_captura, importe_moneda_nacional, id_usuario, true, "", "" };

            //Realziando inserción del concepto
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            return resultado;

        }
        /// <summary>
        /// Edita un Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="id_tipo_detalle"></param>
        /// <param name="id_impuesto_retenido"></param>
        /// <param name="id_impuesto_trasladado"></param>
        /// <param name="tasa"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaDetalleImpuesto(int id_impuesto, int id_tipo_detalle, int id_impuesto_retenido,
                                                          int id_impuesto_trasladado, decimal tasa, decimal importe_moneda_captura,
                                                           decimal importe_moneda_nacional, int id_usuario)
        {
            //Realizando actualizacion
            return editaDetalleImpuesto(id_impuesto, id_tipo_detalle, id_impuesto_retenido, id_impuesto_trasladado, tasa, importe_moneda_captura,
                                        importe_moneda_nacional, id_usuario, this._habilitar);

        }

        /// <summary>
        /// Deshabilita Detalle Impuesto ligado a una transacción
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabiltaDetalleImpuesto(int id_usuario)
        {
            //Declaramos Variable Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Id de registro actualizado
            int id_detalle_impuesto = 0;
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Cargamos los Conceptos Detalles Impuesto ligado a un detalle de Impuesto
                using (DataTable mit = ConceptoDetalleImpuesto.RecuperaConceptosDetalles(this._id_detalle_impuesto))
                {
                    //Validamos Origen de Datos 
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //RRecorremos cada uno de los conceptos detalle Impuesto
                        foreach (DataRow r in mit.Rows)
                        {
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos  Concepto Detalle Impuesto
                                using (ConceptoDetalleImpuesto objConceptoDetalleImpuesto = new ConceptoDetalleImpuesto(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Registro
                                    resultado = objConceptoDetalleImpuesto.DeshabilitaConceptoDetalleImpuesto(id_usuario);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                    }
                }
                //Si el resultado es exitosos
                if (resultado.OperacionExitosa)
                {
                    //Realizando actualizacion
                    resultado = editaDetalleImpuesto(this._id_impuesto, this._id_tipo_detalle, this._id_impuesto_retenido, this._id_impuesto_trasladado, this._tasa,
                                                 this._importe_moneda_captura, this._importe_moneda_nacional, id_usuario, false);
                    //Si se Deshabilito el Detalle de Impuesto
                    if (resultado.OperacionExitosa)
                    {
                        //Asignando Id de resultado exitoso
                        id_detalle_impuesto = resultado.IdRegistro;

                        //Actualizamos Total Impuesto
                        using (Impuesto objImpuesto = new Impuesto(this._id_impuesto))
                        {
                            resultado = objImpuesto.ActualizaTotalImpuesto(id_usuario);

                            //Si se actualizo Impuesto
                            if (resultado.OperacionExitosa)
                            {
                                //Actualizo comprobante
                                using (Comprobante objcomprobante = new Comprobante(objImpuesto.id_comprobante))
                                {
                                    resultado = objcomprobante.ActualizaImpuestosComprobante(objImpuesto.id_impuesto, id_usuario);
                                }

                            }
                        }
                    }
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                //Reasignando resultado general
                {
                    resultado = new RetornoOperacion(id_detalle_impuesto);
                    //Finalizamos transacción
                    scope.Complete();
                }
            }

            return resultado;
        }


    
        /// <summary>
        /// Deshabilita un Detalle Impuesto sin referencias del Concepto Detalle Impuesto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabiltaSoloDetalleImpuesto(int id_usuario)
        {
            //Declaramos Variable Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualizacion
            resultado = editaDetalleImpuesto(this._id_impuesto, this._id_tipo_detalle, this._id_impuesto_retenido, this._id_impuesto_trasladado, this._tasa,
                                         this._importe_moneda_captura, this._importe_moneda_nacional, id_usuario, false);

            return resultado;
        }


        /// <summary>
        /// Carga detalles de impuesto ligado a un Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <returns></returns>
        public static DataTable CargaDetallesImpuesto(int id_impuesto)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Inicializando parametros
            object[] parametros = { 4, 0, id_impuesto, 0, 0, 0, 0, 0, 0, 0, false, "", "" };


            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Actualiza Subtotal de Comprobante
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalDetalleImpuesto(int id_usuario)
        {
            //Inicializamos Vaariables
            decimal ImporteMonedaCaptura = 0, ImporteMonedaNacional = 0;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtiene Total Conceptos
                using (DataTable mit = ConceptoDetalleImpuesto.RecuperaConceptosDetalles(this.id_detalle_impuesto))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {

                        //Obtenemos Total  Conceptos
                        ImporteMonedaCaptura = (from DataRow r in mit.Rows
                                                select Convert.ToDecimal(r["ImporteMonedaCaptura"])).Sum();


                        ImporteMonedaNacional = (from DataRow r in mit.Rows
                                                 select Convert.ToDecimal(r["ImporteMonedaNacional"])).Sum();
                    }
                }
                //Editamos Combrobante
                resultado = editaDetalleImpuesto(this.id_impuesto, this._id_tipo_detalle, this._id_impuesto_retenido,
                                                 this._id_impuesto_trasladado, this.tasa, ImporteMonedaCaptura, ImporteMonedaNacional,
                                                 id_usuario, this._habilitar);
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }

            }
            //Obtenemos Resultado
            return resultado;
        }
        #endregion

    }
}

