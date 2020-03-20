using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;


namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Descripción breve de Descuento
    /// </summary>
    public class Descuento : Disposable
    {

        /* /// <summary>
         /// Enumeración que define los posibles resultados en un acceso a datos
         /// </summary>
         public enum ResultadoD { DatosRepetidos = -1, ErrorConexion = 0, Correcto = 1 };*/

        #region Atributos (R)

        private int _id_descuento;
        /// <summary>
        /// Obtiene el identificador de la instancia
        /// </summary>
        public int id_descuento
        {
            get
            {
                return _id_descuento;
            }
        }

        private int _id_motivo_descuento;
        /// <summary>
        /// Obtiene el id de motivo de descuento
        /// </summary>
        public int id_motivo_descuento
        {
            get
            {
                return _id_motivo_descuento;
            }
        }

        private int _id_comprobante;
        /// <summary>
        /// Obtiene el id de comprobante al que pertenece el descuento
        /// </summary>
        public int id_comprobante
        {
            get
            {
                return _id_comprobante;
            }
        }

        private decimal _porcentaje;
        /// <summary>
        /// Obtiene el porcentaje de descuento
        /// </summary>
        public decimal porcentaje
        {
            get
            {
                return _porcentaje;
            }
        }

        private decimal _cantidad_moneda_captura;
        /// <summary>
        /// Obtiene la cantidad en moneda de captura
        /// </summary>
        public decimal cantidad_moneda_captura
        {
            get
            {
                return _cantidad_moneda_captura;
            }
        }

        private decimal _cantidad_moneda_nacional;
        /// <summary>
        /// Obtine la cantidad en moneda nacional
        /// </summary>
        public decimal cantidad_moneda_nacional
        {
            get
            {
                return _cantidad_moneda_nacional;
            }
        }

        private bool _habilitar;
        /// <summary>
        /// Obtiene un valor que indica si esta habilitado el registro
        /// </summary>
        public bool habilitar
        {
            get
            {
                return _habilitar;
            }
        }

        /* //Puntero para recursos externos sin manejar
         private IntPtr handle = IntPtr.Zero;
         //Recursos manejados que utiliza la clase
         private Component Components = new Component();
         //Define si el objeto ya fue eliminado
         private bool disposed = false;*/

        private static string _nombre_stored_procedure = "fe.sp_descuento_tds";

        #endregion

        #region Constructores (R)

        /// <summary>
        /// Instancía un descuento con los valores por default
        /// </summary>
        public Descuento()
        {
            this._id_descuento = 0;
            this._id_motivo_descuento = 0;
            this._id_comprobante = 0;
            this._porcentaje = 0;
            this._cantidad_moneda_captura = 0;
            this._cantidad_moneda_nacional = 0;
            this._habilitar = false;

        }

        /// <summary>
        ///  Genera una nueva instancia del tipo Descuento
        /// </summary>
        /// <param name="id_descuento"></param>
        public Descuento(int id_descuento)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_descuento);
        }
        /// <summary>
        /// Genera una nueva instancia del tipo Descuento ligado a una transaccion
        /// </summary>
        /// <param name="id_descuento"></param>
        /// <param name="transaccion"></param>
        public Descuento(int id_descuento, SqlTransaction transaccion)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_descuento, transaccion);
        }

        /// <summary>
        /// Instancía un descuento por su id 
        /// </summary>
        /// <param name="id_descuento"></param>
        public bool cargaAtributosInstancia(int id_descuento)
        {
            //Declarando variable de retorno
            bool resultado = false;
            //Inicializando parametros
            object[] parametros = { 3, id_descuento, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando los datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo los datos
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando parametros
                        _id_descuento = Convert.ToInt32(r["Id"]);
                        _id_motivo_descuento = Convert.ToInt32(r["IdMotivoDescuento"]);
                        _id_comprobante = Convert.ToInt32(r["IdComprobante"]);
                        _porcentaje = Convert.ToDecimal(r["Porcentaje"]);
                        _cantidad_moneda_captura = Convert.ToDecimal(r["CantidadMonedaCaptura"]);
                        _cantidad_moneda_nacional = Convert.ToDecimal(r["CantidadMonedaNacional"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                        resultado = true;
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Carga un descuento ligado a una transaccion
        /// </summary>
        /// <param name="id_descuento"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_descuento, SqlTransaction transaccion)
        {
            //Declarando variable de retorno
            bool resultado = false;
            //Inicializando parametros
            object[] parametros = { 3, id_descuento, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros, transaccion))
            {
                //Validando los datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorriendo los datos
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando parametros
                        _id_descuento = Convert.ToInt32(r["Id"]);
                        _id_motivo_descuento = Convert.ToInt32(r["IdMotivoDescuento"]);
                        _id_comprobante = Convert.ToInt32(r["IdComprobante"]);
                        _porcentaje = Convert.ToDecimal(r["Porcentaje"]);
                        _cantidad_moneda_captura = Convert.ToDecimal(r["CantidadMonedaCaptura"]);
                        _cantidad_moneda_nacional = Convert.ToDecimal(r["CantidadMonedaNacional"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                        resultado = true;
                    }
                }
            }
            return resultado;
        }


        /// <summary>
        /// Destructor usado para finalizar el objeto
        /// </summary>
        ~Descuento()
        {
            Dispose(false);
        }


        #endregion

        #region Métodos Privados


        /// <summary>
        ///  Edita un Descuento
        /// </summary>
        /// <param name="id_motivo_descuento"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="porcentaje"></param>
        /// <param name="cantidad_moneda_captura"></param>
        /// <param name="cantidad_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaDescuento(int id_motivo_descuento, int id_comprobante,
                                                   decimal porcentaje, decimal cantidad_moneda_captura, decimal cantidad_moneda_nacional,
                                                   int id_usuario, bool habilitar)
        {

            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_descuento, id_motivo_descuento, id_comprobante, porcentaje, cantidad_moneda_captura,
                                  cantidad_moneda_nacional, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Edita un Descuento ligado a una transaccion
        /// </summary>
        /// <param name="id_motivo_descuento"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="porcentaje"></param>
        /// <param name="cantidad_moneda_captura"></param>
        /// <param name="cantidad_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        private RetornoOperacion editaDescuento(int id_motivo_descuento, int id_comprobante,
                                                   decimal porcentaje, decimal cantidad_moneda_captura, decimal cantidad_moneda_nacional,
                                                   int id_usuario, bool habilitar, SqlTransaction transaccion)
        {

            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_descuento, id_motivo_descuento, id_comprobante, porcentaje, cantidad_moneda_captura,
                                  cantidad_moneda_nacional, id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }
        #endregion

        #region Métodos publicos


        /// <summary>
        /// Inserta un Descuento
        /// </summary>
        /// <param name="id_motivo_descuento"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="porcentaje"></param>
        /// <param name="cantidad_moneda_captura"></param>
        /// <param name="cantidad_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaDescuento(int id_motivo_descuento, int id_comprobante,
                                                   decimal porcentaje, decimal cantidad_moneda_captura, decimal cantidad_moneda_nacional,
                                                   int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();


              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos Existencia de Coonceptos
                if (Validacion.ValidaOrigenDatos(Concepto.RecuperaConceptosComprobantes(id_comprobante)))
                {

                    //Inicializando arreglo de parámetros
                    object[] param = { 1, 0, id_motivo_descuento, id_comprobante, porcentaje, cantidad_moneda_captura,
                                   cantidad_moneda_nacional, id_usuario, true, "", "" };

                    //Realziando inserción del concepto
                    resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

                    //Editamos encabezado del Comprobante
                    if (resultado.OperacionExitosa)
                    {
                        //Guardando Id de Descuento afectado
                        int id_descuento = resultado.IdRegistro;

                        //Intsnaciamos Comprobante
                        using (Comprobante objcomprobante = new Comprobante(id_comprobante))
                        {
                            resultado = objcomprobante.ActualizaDescuento(cantidad_moneda_captura, cantidad_moneda_nacional, id_usuario);
                        }

                        //Actualizamos Impuestos
                        if (resultado.OperacionExitosa)
                        {
                            resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                        }
                        //Si  no hay errores 
                        if (resultado.OperacionExitosa)
                        {
                            resultado = new RetornoOperacion(id_descuento);
                            //Finalizamos Transacción
                            scope.Complete();
                        }
                    }

                }
                else
                {
                    resultado = new RetornoOperacion("No existen conceptos");
                }

            }

            return resultado;

        }

        /// <summary>
        /// Edita un Descuento
        /// </summary>
        /// <param name="id_motivo_descuento"></param>
        /// <param name="id_comprobante"></param>
        /// <param name="porcentaje"></param>
        /// <param name="cantidad_moneda_captura"></param>
        /// <param name="cantidad_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaDescuento(int id_motivo_descuento, int id_comprobante,
                                                   decimal porcentaje, decimal cantidad_moneda_captura, decimal cantidad_moneda_nacional,
                                                   int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Realizando actualizacion
                resultado = editaDescuento(id_motivo_descuento, id_comprobante, porcentaje, cantidad_moneda_captura,
                                     cantidad_moneda_nacional, id_usuario, this._habilitar);

                //Editamos Descuento del Comprobante
                if (resultado.OperacionExitosa)
                {
                    //Guardando Id de Descuento afectado
                    int id_descuento = resultado.IdRegistro;
                    //Instanciamos Comprobante
                    using (Comprobante objcomprobante = new Comprobante(id_comprobante))
                    {
                        resultado = objcomprobante.ActualizaDescuento(cantidad_moneda_captura, cantidad_moneda_nacional, id_usuario);
                    }
                    //Validamos edición del comprobante
                    if (resultado.OperacionExitosa)
                    {
                        resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                    }
                    //Si  no hay errores 
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(id_descuento);
                        //Finaliza transacción
                        scope.Complete();
                    }
                }
            }

            return resultado;
        }


        /// <summary>
        /// Deshabilita un Descuento
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabiltaDescuento(int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

              //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Realizando actualizacion
                resultado = editaDescuento(this._id_motivo_descuento, this._id_comprobante, this._porcentaje, this._cantidad_moneda_captura, this._cantidad_moneda_nacional,
                                     id_usuario, false);

                //Editamos Descuento del Comprobante
                if (resultado.OperacionExitosa)
                {
                    //Guardando Id de Descuento afectado
                    int id_descuento = resultado.IdRegistro;

                    //Instanciamos Comprobante
                    using (Comprobante objcomprobante = new Comprobante(id_comprobante))
                    {
                        resultado = objcomprobante.ActualizaDescuento(0, 0, id_usuario);
                    }
                    //Validamos edición del comprobante
                    if (resultado.OperacionExitosa)
                    {
                        resultado = Impuesto.RecalcularImpuestos(id_comprobante, id_usuario);
                    }
                    //Si  no hay errores 
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(id_descuento);
                        //Finalizamos transacción
                        scope.Complete();
                    }
                }
            }

            return resultado;

        }


        #endregion

    }
}