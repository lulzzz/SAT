using System;
using System.Data;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Descripción breve de Concepto
    /// </summary>
    public class Concepto : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define el Tipo Concepto
        /// </summary>
        public enum TipoConcepto
        {
            /// <summary>
            /// Concepto
            /// </summary>
            Concepto = 1,
            /// <summary>
            /// ParteConcepto
            /// </summary>
            ParteConcepto
        }


        #endregion


        #region Atributos
        /// <summary>
        /// Define el nombre del stored procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string nombre_stored_procedure = "fe.sp_concepto_tco";

        private int _id_concepto;
        private int _id_comprobante;
        private int _id_concepto_padre;
        private int _id_tipo_concepto;
        private decimal _cantidad;
        private int _id_unidad;
        private int _id_tipo_cargo;
        private int _id_descripcion;
        private string _descripcion_parte;
        private string _numero_identificacion;
        private decimal _valor_unitario;
        private decimal _importe_moneda_captura;
        private decimal _importe_moneda_nacional;
        private bool _habilitar;




        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el id de concepto
        /// </summary>
        public int id_concepto
        {
            get
            {
                return _id_concepto;
            }
        }

        /// <summary>
        /// Obtiene el id de comrpobante al que prertenece el concepto
        /// </summary>
        public int id_comprobante
        {
            get
            {
                return _id_comprobante;
            }
        }

        /// <summary>
        /// Obtiene el id de concepto padre al que pertenece el concepto
        /// </summary>
        public int id_concepto_padre
        {
            get
            {
                return _id_concepto_padre;
            }
        }

        /// <summary>
        /// Obtiene el tipo de concepto
        /// </summary>
        public int id_tipo_concepto
        {
            get
            {
                return _id_tipo_concepto;
            }
        }

        /// <summary>
        /// Obtiene la cantidad del concepto
        /// </summary>
        public decimal cantidad
        {
            get
            {
                return _cantidad;
            }
        }

        /// <summary>
        /// Obtiene el id de unidad de medida 
        /// </summary>
        public int id_unidad
        {
            get
            {
                return _id_unidad;
            }
        }

        /// <summary>
        /// Obtiene la descripción del concepto
        /// </summary>
        public int id_descripcion
        {
            get
            {
                return _id_descripcion;
            }
        }

        /// <summary>
        /// Obtiene la descripción de la parte del concepto
        /// </summary>
        public string  descripcion_parte
        {
            get
            {
                return _descripcion_parte;
            }
        }

        /// <summary>
        /// Obtiene el número de identificación del concepto
        /// </summary>
        public string numero_identificacion
        {
            get
            {
                return _numero_identificacion;
            }
        }

        /// <summary>
        /// Obtiene el valor unitario del concepto
        /// </summary>
        public decimal valor_unitario
        {
            get
            {
                return _valor_unitario;
            }
        }

        /// <summary>
        /// Obtiene el importe en moneda de captura
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
        /// Obtiene un valor que indica si el regsitro esta habilitado
        /// </summary>
        public bool habilitar
        {
            get
            {
                return _habilitar;
            }
        }

        /// <summary>
        /// Tipo Concepto
        /// </summary>
        public TipoConcepto TipoConceptos { get { return (TipoConcepto)_id_tipo_concepto; } }

        #endregion


        #region Constructores

        /// <summary>
        /// Instancía un concepto con los valores por default
        /// </summary>
        public Concepto()
        {
            _id_concepto = 0;
            _id_comprobante = 0;
            _id_concepto_padre = 0;
            _id_tipo_concepto = 0;
            _cantidad = 0;
            _id_unidad = 0;
            _id_tipo_cargo = 0;
            _id_descripcion = 0;
            _descripcion_parte = "";
            _numero_identificacion = "";
            _valor_unitario = 0;
            _importe_moneda_captura = 0;
            _importe_moneda_nacional = 0;
            _habilitar = false;

        }

        /// <summary>
        /// Instancía un concepto por su id
        /// </summary>
        /// <param name="id_concepto"></param>
        public bool cargaAtributosInstancia(int id_concepto)
        {
            //Declarando variable de retorno
            bool resultado = false;
            //Inicialziando párametros
            object[] parametros = { 3, id_concepto, 0, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo los renglones
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando atributos
                        _id_concepto = Convert.ToInt32(r["Id"]);
                        _id_comprobante = Convert.ToInt32(r["IdComprobante"]);
                        _id_concepto_padre = Convert.ToInt32(r["IdConceptoPadre"]);
                        _id_tipo_concepto = Convert.ToInt32(r["IdTipoConcepto"]);
                        _cantidad = Convert.ToDecimal(r["Cantidad"]);
                        _id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        _id_tipo_cargo = Convert.ToInt32(r["IdTipoCargo"]);
                        _id_descripcion = Convert.ToInt32(r["IdDescripcion"]);
                        _descripcion_parte = r["DescripcionParte"].ToString();
                        _numero_identificacion = r["NoIdentificacion"].ToString();
                        _valor_unitario = Convert.ToDecimal(r["ValorUnitario"]);
                        _importe_moneda_captura = Convert.ToDecimal(r["ImporteMonedaCaptura"]);
                        _importe_moneda_nacional = Convert.ToDecimal(r["ImporteMonedaNacional"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                        resultado = true;
                    }
                }
            }
            //Devolviendo resultado
            return resultado;
        }

         /// <summary>
        /// Genera una nueva instancia del tipo Concepto con los datos del registro solicitado
        /// </summary>
        /// <param name="id_concepto"></param>
        public Concepto(int id_concepto)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_concepto);
        }
        
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~Concepto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos privados

        #endregion

        #region Métodos publicos


       

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Edita un concepto
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="id_concepto_padre"></param>
        /// <param name="id_tipo_concepto"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_descripcion"></param>
        /// <param name="descripcion_parte"></param>
        /// <param name="no_identificacion"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaConcepto(int id_comprobante, int id_concepto_padre, int id_tipo_concepto,
                                                   decimal cantidad, int id_unidad, int id_descripcion, string descripcion_parte, string no_identificacion,
                                                   decimal valor_unitario, decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario, bool habilitar)
        {

            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_concepto,id_comprobante, id_concepto_padre, id_tipo_concepto, cantidad, id_unidad,
                                 id_descripcion, descripcion_parte, no_identificacion, valor_unitario, importe_moneda_captura,
                                 importe_moneda_nacional, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);

        }



        #endregion

        #region Metodos Publicos

        /// <summary>
        ///  Método encargado de insertar un concepto 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_descripcion"></param>
        /// <param name="no_identificacion"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConcepto(int id_comprobante,
                                                   decimal cantidad, int id_unidad, int id_descripcion,
                                                  string no_identificacion, decimal valor_unitario, decimal importe_moneda_captura,
                                                  decimal importe_moneda_nacional, int id_usuario)
        {
            int id_concepto = 0;
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos comprobante
                using (Comprobante objComprobante = new Comprobante(id_comprobante))
                {
                    //validamos Existencia de comprobante
                    if (objComprobante.id_comprobante > 0)
                    {
                        //Validamos existencia de Descuento
                        if (objComprobante.descuento_moneda_captura == 0 || objComprobante.descuento_moneda_nacional == 0)
                        {
                            //Inicializando arreglo de parámetros
                            object[] param = { 1, 0,id_comprobante, 0, 1, cantidad, id_unidad,
                                                 id_descripcion, "", no_identificacion, valor_unitario, importe_moneda_captura,
                                                 importe_moneda_nacional, id_usuario, true, "", "" };

                            //Realziando inserción del concepto
                            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);


                            //Asignamos Id de Concopeto
                            id_concepto = resultado.IdRegistro;

                            //Recalucular Impuestos
                            if (resultado.OperacionExitosa)
                            {
                                resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                                //Validamos Resultado 
                                if (resultado.OperacionExitosa)
                                {
                                    //Declaramos Objeto Retorno
                                    resultado = new RetornoOperacion(id_concepto);
                                    //Finalizamos transacción
                                    scope.Complete();
                                }
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se puede agregar un concepto, ya que existe el decuento");
                        }
                    }
                    else
                    {
                        resultado = new RetornoOperacion("No se encontró datos complementarios Comprobante");
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Inserta Concepto Parte
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="id_concepto_padre"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad"></param>
        /// <param name="descripcion_parte"></param>
        /// <param name="no_identificacion"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConceptoParte(int id_comprobante, int id_concepto_padre,
                                                decimal cantidad, int id_unidad, string descripcion_parte, string no_identificacion,
                                                decimal valor_unitario, decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario)
        {

            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando arreglo de parámetros
            object[] param = { 1, 0,id_comprobante, id_concepto_padre, 2, cantidad, id_unidad,
                                 0, descripcion_parte, no_identificacion, valor_unitario, importe_moneda_captura,
                                 importe_moneda_nacional, id_usuario, true, "", "" };

            //Realziando inserción del concepto
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, param);
        }

        /// <summary>
        /// Editamos un Concepto
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_descripcion"></param>
        /// <param name="descripcion_parte"></param>
        /// <param name="no_identificacion"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaConcepto(int id_comprobante, decimal cantidad, int id_unidad, int id_descripcion,
                                                 int descripcion_parte, string no_identificacion, decimal valor_unitario,
                                                 decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Instanciamos comprobante
                using (Comprobante objComprobante = new Comprobante(id_comprobante))
                {
                    //validamos Existencia de comprobante
                    if (objComprobante.id_comprobante > 0)
                    {
                        //Validamos existencia de Descuento
                        if (objComprobante.descuento_moneda_captura == 0 || objComprobante.descuento_moneda_nacional == 0)
                        {
                            //Realizando actualizacion
                            resultado = editaConcepto(id_comprobante, 0, 1, cantidad, id_unidad, id_descripcion, "",
                                                   no_identificacion, valor_unitario, importe_moneda_captura, importe_moneda_nacional, id_usuario,
                                                   this._habilitar);



                            //Validamos Resultado
                            //Recalucular Impuestos
                            if (resultado.OperacionExitosa)
                            {
                                resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                                //Finalizamos transacción
                                scope.Complete();
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se puede editar un concepto, ya que existe el descuento");
                        }
                    }
                    else
                    {
                        resultado = new RetornoOperacion("No se encontró datos complementarios Comprobante");
                    }
                }

            }

            return resultado;
        }

        /// <summary>
        /// Edita un concepto Parte
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="id_concepto_padre"></param>
        /// <param name="cantidad"></param>
        /// <param name="id_unidad"></param>
        /// <param name="descripcion_parte"></param>
        /// <param name="no_identificacion"></param>
        /// <param name="valor_unitario"></param>
        /// <param name="importe_moneda_captura"></param>
        /// <param name="importe_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoParte(int id_comprobante, int id_concepto_padre,
                                                 decimal cantidad, int id_unidad, string descripcion_parte, string no_identificacion,
                                                 decimal valor_unitario, decimal importe_moneda_captura, decimal importe_moneda_nacional, int id_usuario)
        {
            return editaConcepto(id_comprobante, id_concepto_padre, 2, cantidad, id_unidad, 0, descripcion_parte, no_identificacion,
                                 valor_unitario, importe_moneda_captura, importe_moneda_nacional, id_usuario, this._habilitar);
        }



        /// <summary>
        /// Deshabilitamos un concepto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConcepto(int id_usuario)
        {

            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos tipo de Concepto no sea Padre 
                if ((TipoConcepto)this._id_tipo_concepto == TipoConcepto.ParteConcepto)
                {
                    //Realizando actualizacion
                    resultado = editaConcepto(this._id_comprobante, this._id_concepto_padre, this._id_tipo_concepto, this._cantidad, this._id_unidad, this._id_descripcion,
                                                        this._descripcion_parte, this._numero_identificacion, this._valor_unitario,
                                                        this._importe_moneda_captura, this._importe_moneda_nacional, id_usuario, false);
                }
                else
                {
                    //Instanciamos Comprobante
                    using (Comprobante objComprobante = new Comprobante(this._id_comprobante))
                    {
                        //Validamos existencia de descuento
                        if (objComprobante.descuento_moneda_nacional == 0 || objComprobante.descuento_moneda_captura == 0)
                        {
                            //Validamos Existencia de Conceptos Hijos
                            if (Validacion.ValidaOrigenDatos(CargaConceptosPartes(this._id_concepto)))
                            {

                                //Inicialziando párametros
                                object[] parametros = { 4, this._id_concepto, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, false, "", "" };

                                //Realizando actualización
                                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
                                {
                                    //Validamos Origen de Datos
                                    if (Validacion.ValidaOrigenDatos(ds))
                                    {
                                        //Validando actualziación múltiple
                                        resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);
                                    }
                                }
                            }
                            //Validamos Deshabilitación hijos
                            if (resultado.OperacionExitosa)
                            {
                                //Deshabilitamos Concepto Padre
                                resultado = editaConcepto(this._id_comprobante, this._id_concepto_padre, this._id_tipo_concepto, this._cantidad, this._id_unidad, this._id_descripcion,
                                                                    this._descripcion_parte, this._numero_identificacion, this._valor_unitario,
                                                                    this._importe_moneda_captura, this._importe_moneda_nacional, id_usuario, false);
                                //Recalcular Impuestos
                                if (resultado.OperacionExitosa)
                                {
                                    resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                                    //Finalziamos transacción
                                    scope.Complete();
                                }
                            }
                        }
                        else
                        {
                            resultado = new RetornoOperacion("No se puede deshabilitar un concepto, ya que existe un descuento");
                        }

                    }
                }
            }

            return resultado;
        }
        /// <summary>
        /// Recupera el Total de Conceptos ligado a un comprobante
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <returns></returns>
        public static DataTable RecuperaConceptosComprobantes(int id_comprobante)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Inicialziando párametros
            object[] parametros = { 5, 0, id_comprobante, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }


        /// <summary>
        /// Carga Conceptos Partes
        /// </summary>
        /// <param name="id_concepto"></param>
        /// <returns></returns>
        public static DataTable CargaConceptosPartes(int id_concepto)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Inicialziando párametros
            object[] parametros = { 8, id_concepto, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, false, "", "" };

            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Realziando recuperación de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Realziando consulta 
                return mit;
            }
        }

        /// <summary>
        /// Validamos Total Conceptos y Partes
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ValidaTotalConceptosHijos()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
             //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtiene Total Conceptos
                using (DataTable mit = Concepto.CargaConceptosPartes(this.id_concepto))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Obtenemos Total  Conceptos
                        double ImporteCaptura = (from DataRow r in mit.Rows
                                                 select r.Field<double>("ImporteCaptura")).Sum();


                        double ImportelNacional = (from DataRow r in mit.Rows
                                                   select r.Field<double>("ImporteNacional")).Sum();

                        //Validamos Totale del Concepto y Conceptos Hijos
                        if (ImporteCaptura == Convert.ToDouble(this._importe_moneda_captura) && ImportelNacional == Convert.ToDouble(this._importe_moneda_nacional))
                        {

                        }
                        else
                        {
                            resultado = new RetornoOperacion("Lo detalles no coincide con el Total del Concepto.");
                        }

                    }
                    else
                    {
                        resultado = new RetornoOperacion("Lo detalles no coincide con el Total del Concepto.");

                    }
                }
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


        /// <summary>
        /// Muestra el total de  de Impuestos ligadon a un concepto
        /// </summary>
        /// <param name="concepto_importe_moneda_captura"></param>
        /// <param name="concepto_importe_moneda_decimal"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="tasa"></param>
        /// <param name="impuesto_captura"></param>
        /// <param name="impuesto_nacional"></param>
        public void CalculaVistaImpuestoconcepto(decimal concepto_importe_moneda_captura, decimal concepto_importe_moneda_decimal, int id_comprobante, decimal tasa, out decimal impuesto_captura,
                                                out decimal impuesto_nacional)
        {
            impuesto_captura = 0; impuesto_nacional = 0;
            decimal descuento_importe_moneda_nacional = 0,
                       descuento_importe_moneda_captura = 0;

            //Instanciamos Comprobante
            using (Comprobante objComprobante = new Comprobante(id_comprobante))
            {

                //Obtenemos el Descuento Por Concepto
                objComprobante.ObtieneDescuentoPorConcepto(concepto_importe_moneda_captura, concepto_importe_moneda_decimal, out descuento_importe_moneda_captura, out descuento_importe_moneda_nacional);

                //Calculamos Impuesto Captura
                impuesto_captura = (this._importe_moneda_captura - descuento_importe_moneda_captura) * (tasa / 100);

                //Calculamos Impuesto Nacional
                impuesto_nacional = (this._importe_moneda_nacional - descuento_importe_moneda_nacional) * (tasa / 100);


            }
        }
        /// <summary>
        /// Método que carga los conceptos de un comprobante
        /// </summary>
        /// <param name="id_comprobante">Identficador que sirve como referencia para la busqueda de conceptos del comprobante</param>
        /// <returns></returns>
        public static DataTable CargaImpresionConceptos(int id_comprobante)
        {
            //Creación del arreglo retorno con los valores para realizar la consulta de registros
            object[] param = { 9, 0, id_comprobante, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, false, "", "" };
            //Realiza la busqueda de los conceptos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                DataTable mit = null;
                //Vaida que exista el registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    mit = DS.Tables["Table"];

                    return mit;
            }
        }

        /// <summary>
        /// Método que carga los conceptos de un comprobante
        /// </summary>
        /// <param name="id_comprobante">Identficador que sirve como referencia para la busqueda de conceptos del comprobante</param>
        /// <param name="id_concepto">Id de Concepto</param>
        /// <returns></returns>
        public static DataTable CargaImpresionReferencias(int id_comprobante, int id_concepto)
        {
            //Creación del arreglo retorno con los valores para realizar la consulta de registros
            object[] param = { 10, id_concepto, id_comprobante, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, false, "", "" };
            //Realiza la busqueda de los conceptos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, param))
            {
                DataTable mit = null;
                //Vaida que exista el registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    mit = DS.Tables["Table"];

                return mit;
            }
        }
      
        #endregion

    }
}