using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Descripción breve de Impuesto
    /// </summary>
    public class Impuesto : Disposable
    {
        #region Propiedades y Atributos (R)


        private int _id_impuesto;
        /// <summary>
        /// Obtiene el identificador de la instancia
        /// </summary>
        public int id_impuesto
        {
            get
            {
                return _id_impuesto;
            }
        }

        private int _id_comprobante;
        /// <summary>
        /// Obtiene el comprobante al que pertenece el impuesto
        /// </summary>
        public int id_comprobante
        {
            get
            {
                return _id_comprobante;
            }
        }

        private decimal _total_retenido_moneda_captura;
        /// <summary>
        /// Obtiene el total de impuestos retenidos en moneda de captura
        /// </summary>
        public decimal total_retenido_moneda_captura
        {
            get
            {
                return _total_retenido_moneda_captura;
            }
        }

        private decimal _total_retenido_moneda_nacional;
        /// <summary>
        /// Obtiene el total de impuestos retenidos en moneda nacional
        /// </summary>
        public decimal total_retenido_moneda_nacional
        {
            get
            {
                return _total_retenido_moneda_nacional;
            }
        }

        private decimal _total_trasladado_moneda_captura;
        /// <summary>
        /// Obtiene el total de impuestos trasladados en moneda de captura
        /// </summary>
        public decimal total_trasladado_moneda_captura
        {
            get
            {
                return _total_trasladado_moneda_captura;
            }
        }

        private decimal _total_trasladado_moneda_nacional;
        /// <summary>
        /// Obtiene el total de impuestos trasladados en moneda nacional
        /// </summary>
        public decimal total_trasladado_moneda_nacional
        {
            get
            {
                return _total_trasladado_moneda_nacional;
            }
        }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el Estatus Habilitar del Registro
        /// </summary>
        public bool habilitar
        {
            get
            {
                return _habilitar;
            }
        }


        private static string _nombre_stored_procedure = "fe.sp_impuesto_tim";
        #endregion

        #region Constructores (R)

        /// <summary>
        /// Instancía un impuesto con los valores por default
        /// </summary>
        public Impuesto()
        {
            this._id_impuesto = 0;
            this._id_comprobante = 0;
            this._total_retenido_moneda_captura = 0;
            this._total_retenido_moneda_nacional = 0;
            this._total_trasladado_moneda_captura = 0;
            this._total_trasladado_moneda_nacional = 0;
            this._habilitar = false;
        }


        /// <summary>
        /// Genera una nueva instancia del tipo Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        public Impuesto(int id_impuesto)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_impuesto);
        }
        /// <summary>
        /// Genera una nueva instancia del tipo Impuesto
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="transaccion"></param>
        public Impuesto(int id_impuesto, SqlTransaction transaccion)
        {
            //Incializando instancia
            cargaAtributosInstancia(id_impuesto, transaccion);
        }
        /// <summary>
        /// Instancía un impuesto por su id
        /// </summary>
        /// <param name="id_impuesto"></param>
        public bool cargaAtributosInstancia(int id_impuesto)
        {

            //Declarando variable de retorno
            bool resultado = false;
            //Inicializando parametros
            object[] parametros = { 3, id_impuesto, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table")) 
                {
                    //Recorriendo las filas
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando atributos
                        _id_impuesto = Convert.ToInt32(r["Id"]);
                        _id_comprobante = Convert.ToInt32(r["IdComprobante"]);
                        _total_retenido_moneda_captura = Convert.ToDecimal(r["TotalRetenidoMonedaCaptura"]);
                        _total_retenido_moneda_nacional = Convert.ToDecimal(r["TotalRetenidoMonedaNacional"]);
                        _total_trasladado_moneda_captura = Convert.ToDecimal(r["TotalTrasladadoMonedaCaptura"]);
                        _total_trasladado_moneda_nacional = Convert.ToDecimal(r["TotalTrasladadoMonedaNacional"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);

                        resultado = true;
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Carga una instancia Tipo Impuesto ligado a una transacción
        /// </summary>
        /// <param name="id_impuesto"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_impuesto, SqlTransaction transaccion)
        {

            //Declarando variable de retorno
            bool resultado = false;
            //Inicializando parametros
            object[] parametros = { 3, id_impuesto, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros, transaccion))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorriendo las filas
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        //Asignando atributos
                        _id_impuesto = Convert.ToInt32(r["Id"]);
                        _id_comprobante = Convert.ToInt32(r["IdComprobante"]);
                        _total_retenido_moneda_captura = Convert.ToDecimal(r["TotalRetenidoMonedaCaptura"]);
                        _total_retenido_moneda_nacional = Convert.ToDecimal(r["TotalRetenidoMonedaNacional"]);
                        _total_trasladado_moneda_captura = Convert.ToDecimal(r["TotalTrasladadoMonedaCaptura"]);
                        _total_trasladado_moneda_nacional = Convert.ToDecimal(r["TotalTrasladadoMonedaNacional"]);
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
        ~Impuesto()
        {
            Dispose(false);
        }


        #endregion

        #region Métodos privados


        /// <summary>
        /// Edita un Impuesto
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario, bool habilitar)
        {


            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_impuesto, id_comprobante, total_retenido_moneda_captura,
                                total_retenido_moneda_nacional, total_trasladado_moneda_captura, total_trasladado_moneda_nacional,
                                id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Edita un impuesto ligado a una transacción 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        private RetornoOperacion editaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario, bool habilitar, SqlTransaction transaccion)
        {

            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_impuesto, id_comprobante, total_retenido_moneda_captura,
                                total_retenido_moneda_nacional, total_trasladado_moneda_captura, total_trasladado_moneda_nacional,
                                id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }


         #endregion

        #region Métodos publicos

        /// <summary>
        /// Inserta  un Impuesto
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();


            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_comprobante, total_retenido_moneda_captura, total_retenido_moneda_nacional,
                                   total_trasladado_moneda_captura, total_trasladado_moneda_nacional, id_usuario, true, "", "" };

            //Realziando inserción del concepto
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            return resultado;

        }

        /// <summary>
        ///  Inserta un Impuesto ligado a una transacción
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario, SqlTransaction transaccion)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();


            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_comprobante, total_retenido_moneda_captura, total_retenido_moneda_nacional,
                                   total_trasladado_moneda_captura, total_trasladado_moneda_nacional, id_usuario, true, "", "" };

            //Realziando inserción del concepto
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

            return resultado;

        }

        /// <summary>
        /// Edita un Impuesto
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario)
        {
            //Realizando actualizacion
            return editaImpuesto(id_comprobante, total_retenido_moneda_captura, total_retenido_moneda_nacional,
                               total_trasladado_moneda_captura, total_trasladado_moneda_nacional, id_usuario, this._habilitar);

        }

        /// <summary>
        /// Edita un impuesto ligado a una transaccion
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="total_retenido_moneda_captura"></param>
        /// <param name="total_retenido_moneda_nacional"></param>
        /// <param name="total_trasladado_moneda_captura"></param>
        /// <param name="total_trasladado_moneda_nacional"></param>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public RetornoOperacion EditaImpuesto(int id_comprobante, decimal total_retenido_moneda_captura,
                                                   decimal total_retenido_moneda_nacional,
                                                   decimal total_trasladado_moneda_captura, decimal total_trasladado_moneda_nacional,
                                                   int id_usuario, SqlTransaction transaccion)
        {
            //Realizando actualizacion
            return editaImpuesto(id_comprobante, total_retenido_moneda_captura, total_retenido_moneda_nacional,
                                total_trasladado_moneda_captura, total_trasladado_moneda_nacional, id_usuario, this._habilitar, transaccion);

        }

        /// <summary>
        /// Deshabilita un Impuesto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabiltaImpuesto(int id_usuario)
        {
            //Inicializamos Transaccion
            SqlTransaction transaccion = CapaDatos.m_capaDeDatos.InicializaTransaccionSQL(IsolationLevel.ReadCommitted);

            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialziando párametros
            object[] parametros = { 4, this._id_impuesto, 0, 0, 0, 0, 0, 0, false, "", "" };


            //Realizando actualización
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros, transaccion))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds)) 
                {
                    //Validando actualziación múltiple
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);
                }
                //Validamos Deshabilitación hijos
                if (resultado.OperacionExitosa)
                {

                    //Realizando actualizacion
                    resultado = editaImpuesto(this._id_comprobante, this._total_retenido_moneda_captura, this._total_retenido_moneda_nacional,
                                         this._total_trasladado_moneda_captura, this._total_trasladado_moneda_nacional, id_usuario,
                                         false, transaccion);
                }
            }
            //Finalizando transacción
            CapaDatos.m_capaDeDatos.FinalizaTransaccionSQL(transaccion, resultado.OperacionExitosa);


            return resultado;

        }

        /// <summary>
        /// Deshabilita un Impuesto ligado a una transaccion
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabiltaImpuesto(int id_usuario, SqlTransaction transaccion)
        {
            //Establecemos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Inicialziando párametros
            object[] parametros = { 4, this._id_impuesto, 0, 0, 0, 0, 0, 0, false, "", "" };


            //Realizando actualización
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros, transaccion))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    //Validando actualziación múltiple
                    resultado = RetornoOperacion.ValidaResultadoOperacionMultiple(ds);
                }
                //Validamos Deshabilitación hijos
                if (resultado.OperacionExitosa)
                {

                    //Realizando actualizacion
                    resultado = editaImpuesto(this._id_comprobante, this._total_retenido_moneda_captura, this._total_retenido_moneda_nacional,
                                         this._total_trasladado_moneda_captura, this._total_trasladado_moneda_nacional, id_usuario,
                                         false, transaccion);
                }
            }

            return resultado;

        }

        /// <summary>
        /// Actualiza Total Impuesto
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaTotalImpuesto(int id_usuario)
        {
            //Inicializamos Variables
            decimal total_retenido_moneda_captura = 0,
             total_retenido_moneda_nacional = 0,
             total_trasladado_moneda_captura = 0,
             total_trasladado_moneda_nacional = 0;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtiene Total Conceptos
                using (DataTable mit = DetalleImpuesto.CargaDetallesImpuesto(this.id_impuesto))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {

                        //Obtenemos  Importe Captura Impuestos Retenidos
                        total_retenido_moneda_captura = (from DataRow r in mit.Rows
                                                         where r.Field<int>("IdTipo") == 1
                                                         select Convert.ToDecimal(r["ImporteMonedaCaptura"])
                                                          ).Sum();
                        //Obtenemos  Importe Nacional Impuesto Retenidos
                        total_retenido_moneda_nacional = (from DataRow r in mit.Rows
                                                          where r.Field<int>("IdTipo") == 1
                                                          select Convert.ToDecimal(r["ImporteMonedaNacional"])).Sum();
                        //Obtenemos  Importe Captura  Impuestos Retenidos
                        total_trasladado_moneda_captura = (from DataRow r in mit.Rows
                                                           where r.Field<int>("IdTipo") == 2
                                                           select Convert.ToDecimal(r["ImporteMonedaCaptura"])
                                                          ).Sum();
                        //Obtenemos Importe Nacional  Impuesto Trasladado
                        total_trasladado_moneda_nacional = (from DataRow r in mit.Rows
                                                            where r.Field<int>("IdTipo") == 2
                                                            select Convert.ToDecimal(r["ImporteMonedaNacional"])).Sum();
                    }
                }
                //Editamos Combrobante

                resultado = this.editaImpuesto(this._id_comprobante, total_retenido_moneda_captura, total_retenido_moneda_nacional,
                                               total_trasladado_moneda_captura, total_trasladado_moneda_nacional, id_usuario, this._habilitar
                                               );
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos Transacción
                    scope.Complete();
                }
            }

            //Obtenemos Resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la carga del registro Impuesto del comprobante solicitado, en el formato de tabla
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static DataTable CargaImpuestoComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parametros
            object[] parametros = { 5, 0, id_comprobante, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando resultado
                    mit = DS.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Realiza la carga del registro Impuesto del comprobante solicitado, en el formato de tabla
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public static DataTable CargaImpuestoComprobante(int id_comprobante, SqlTransaction transaccion)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Inicializando parametros
            object[] parametros = { 5, 0, id_comprobante, 0, 0, 0, 0, 0, false, "", "" };

            //Instanciando dataset con resultado de consulta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, parametros, transaccion))
            {
                //Validando datos
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando resultado
                    mit = DS.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Realiza la búsqueda del impuesto asignado al comprobante, devolviendo la instancia del mismo
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <returns></returns>
        public static Impuesto RecuperaImpuestoComprobante(int id_comprobante)
        {
            //Definiendo objeto de retorno
            Impuesto impuesto = new Impuesto();
            //Cargando impuesto
            DataTable mit = CargaImpuestoComprobante(id_comprobante);
            //Validando origen de datos
            if (mit != null)
            {
                //Para cada registro
                foreach (DataRow r in mit.Rows)

                    impuesto = new Impuesto(Convert.ToInt32(r["Id"]));
            }

            //Devolviendo impuesto resultante
            return impuesto;
        }
        /// <summary>
        /// Realiza la búsqueda del impuesto asignado al comprobante, devolviendo la instancia del mismo
        /// </summary>
        /// <param name="id_comprobante">Id de Comprobante</param>
        /// <param name="transaccion">Transacción</param>
        /// <returns></returns>
        public static Impuesto RecuperaImpuestoComprobante(int id_comprobante, SqlTransaction transaccion)
        {
            //Definiendo objeto de retorno
            Impuesto impuesto = new Impuesto();
            //Cargando impuesto
            DataTable mit = CargaImpuestoComprobante(id_comprobante, transaccion);
            //Validando origen de datos
            if (mit != null)
            {
                //Para cada registro
                foreach (DataRow r in mit.Rows)

                    impuesto = new Impuesto(Convert.ToInt32(r["Id"]), transaccion);
            }

            //Devolviendo impuesto resultante
            return impuesto;
        }


        /// <summary>
        /// Recalcula un Impuesto ligado 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion RecalcularImpuestos(int id_comprobante, int id_usuario)
        {
            //Declaramos Objeto resultado 
            RetornoOperacion resultado = new RetornoOperacion(0);
            int id_concepto = 0;
            decimal descuento_importe_moneda_nacional = 0,
            descuento_importe_moneda_captura = 0,
            impuesto_captura = 0, impuesto_nacional = 0;
               //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                using (Comprobante objComprobante = new Comprobante(id_comprobante))
                {

                    //Obtenemos instancia de impuesto
                    Impuesto impuesto = RecuperaImpuestoComprobante(id_comprobante);

                    //Validamos existencia de impuesto
                    if (impuesto.id_impuesto > 0)
                    {
                        //Cargamos Conceptos Existentes ligado al comprobante
                        using (DataTable mitConceptos = Concepto.RecuperaConceptosComprobantes(id_comprobante))
                        {
                            //Cargamos Detalles de Impuesto
                            using (DataTable mitDetalleImpuesto = DetalleImpuesto.CargaDetallesImpuesto(impuesto.id_impuesto))
                            {
                                //Validamos Origen de Datos
                                if (Validacion.ValidaOrigenDatos(mitDetalleImpuesto))
                                {
                                    //rrecorremos cada uno de los Detalles de Impuestos
                                    foreach (DataRow r in mitDetalleImpuesto.Rows)
                                    {
                                        //Instanciamos Detalle Impimpuesto
                                        using (DetalleImpuesto detalleImpuesto = new DetalleImpuesto(r.Field<int>("Id")))
                                        {
                                            //Valida Detalle Impuesto
                                            if (detalleImpuesto.id_detalle_impuesto > 0)
                                            {
                                                //Cargamos Concepto Detalle Impuesto
                                                using (DataTable mitConceptoDetalleImpuesto = ConceptoDetalleImpuesto.RecuperaConceptosDetalles(detalleImpuesto.id_detalle_impuesto))
                                                {
                                                    //Validamos Origen de Datos
                                                    if (Validacion.ValidaOrigenDatos(mitConceptoDetalleImpuesto))
                                                    {
                                                        //Recorremos cada uno de los Conceptos
                                                        foreach (DataRow rConceptoDetalleImpuesto in mitConceptoDetalleImpuesto.Rows)
                                                        {
                                                            //Validamos Existencia de Conceptos
                                                            if (Validacion.ValidaOrigenDatos(mitConceptos))
                                                            {
                                                                //Validamos Existencia del Concepto
                                                                id_concepto = (from DataRow rConcepto in mitConceptos.Rows
                                                                               where rConcepto.Field<int>("Id") == rConceptoDetalleImpuesto.Field<int>("IdConcepto")
                                                                               select rConcepto.Field<int>("Id")).FirstOrDefault();
                                                            }
                                                            //Validamos existencia de concepto
                                                            if (id_concepto == 0)
                                                            {
                                                                //Desahabilita Concepto Detalle Impuesto
                                                                using (ConceptoDetalleImpuesto objConceptoDetalleImpuesto = new ConceptoDetalleImpuesto
                                                                (rConceptoDetalleImpuesto.Field<int>("Id")))
                                                                {
                                                                    //Deshabilitamos liga
                                                                    resultado = objConceptoDetalleImpuesto.DeshabilitaConceptoDetalleImpuesto(id_usuario);

                                                                    //Validamos resultado
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        /*Validamos Existencia de concepto Detalle Impueston de lo contrario deshabilitamos el
                                                                       /detalle impuesto*/
                                                                        if (!Validacion.ValidaOrigenDatos(ConceptoDetalleImpuesto.RecuperaConceptosDetalles(detalleImpuesto.id_detalle_impuesto)))
                                                                        {
                                                                            //Actualizamos Total Detalles
                                                                            resultado = detalleImpuesto.ActualizaTotalDetalleImpuesto(id_usuario);
                                                                            //Validamos Actualizacion
                                                                            if (resultado.OperacionExitosa)
                                                                            {
                                                                                //Actualizamos Objeto 
                                                                                detalleImpuesto.cargaAtributosInstancia(detalleImpuesto.id_detalle_impuesto);
                                                                                //Deshabilita Detalle Impuesto
                                                                                resultado = detalleImpuesto.DeshabiltaSoloDetalleImpuesto(id_usuario);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }

                                                                }

                                                            }
                                                            else
                                                            {



                                                                //Instanciamos Concepto
                                                                using (Concepto objconcepto = new Concepto(id_concepto))
                                                                {
                                                                    //Validamos Concepto
                                                                    if (objconcepto.id_concepto > 0)
                                                                    {
                                                                        //Obtenemos el Descuento Por Concepto
                                                                        objComprobante.ObtieneDescuentoPorConcepto(objconcepto.importe_moneda_captura, objconcepto.importe_moneda_nacional,
                                                                        out descuento_importe_moneda_captura, out descuento_importe_moneda_nacional);


                                                                        //Calculamos Impuesto Captura
                                                                        impuesto_captura = (objconcepto.importe_moneda_captura - descuento_importe_moneda_captura) * (detalleImpuesto.tasa / 100);

                                                                        //Calculamos Impuesto Nacional
                                                                        impuesto_nacional = (objconcepto.importe_moneda_nacional - descuento_importe_moneda_nacional) * (detalleImpuesto.tasa / 100);

                                                                        //Instanciamos Concepto Detalle Impuesto
                                                                        using (ConceptoDetalleImpuesto objConceptoDetalleImpuesto = new ConceptoDetalleImpuesto
                                                                        (rConceptoDetalleImpuesto.Field<int>("Id")))
                                                                        {
                                                                            //Validamos Concepto Detalle Impuesto
                                                                            if (objConceptoDetalleImpuesto.id_concepto_detalle_impuesto > 0)
                                                                            {
                                                                                //Editamos Monto

                                                                                resultado = objConceptoDetalleImpuesto.EditaConceptoDetalleImpuesto(objConceptoDetalleImpuesto.id_concepto,
                                                                                                objConceptoDetalleImpuesto.id_detalle_impuesto, impuesto_captura,
                                                                                                impuesto_nacional, id_usuario);
                                                                            }
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        resultado = new RetornoOperacion("No se encontró datos complementario concepto");
                                                                    }

                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                //Actualiamos Detalle Impuesto
                                                detalleImpuesto.cargaAtributosInstancia(r.Field<int>("Id"));
                                                //Validamos existenci ade Detella
                                                if (detalleImpuesto.habilitar == true)
                                                {
                                                    //Validamos Inserccion de Concepto Detalle Impuesto
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizamos Total Detalles
                                                        resultado = detalleImpuesto.ActualizaTotalDetalleImpuesto(id_usuario);

                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                            }

                        }
                        //Si se actualizo correctamente Detalle Impuesto
                        if (resultado.OperacionExitosa)
                        {

                            resultado = impuesto.ActualizaTotalImpuesto(id_usuario);

                        }
                        //Si se actualizo correctamente el  Impuesto
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Instnacia Comprobante
                            resultado = objComprobante.ActualizaImpuestosComprobante(impuesto.id_impuesto, id_usuario);

                        }


                    }
                }
                //Validamos Resultado
                if(resultado.OperacionExitosa)
                {
                    //Finalizamos transacción
                    scope.Complete();
                }
            }
            return resultado;
        }

        /// <summary>
        /// Obtenemos Tasa
        /// </summary>
        /// <returns></returns>
        public static string TasaIvaRetenido(int id_comprobante)
        {

            string tasa = "";
            //Instanciamos Impuesto
            using (Impuesto impuesto = Impuesto.RecuperaImpuestoComprobante(id_comprobante))
            {
                //Cargamos Detalles de Impuesto
                using (DataTable mitDetalleImpuesto = DetalleImpuesto.CargaDetallesImpuesto(impuesto.id_impuesto))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitDetalleImpuesto))
                    {

                        //Obtenemos Total  Conceptos
                        tasa = (from DataRow r in mitDetalleImpuesto.Rows
                                where Convert.ToInt32(r["IdImpuestoRetenido"]) == 2
                                select r["tasa"].ToString()).FirstOrDefault();

                    }
                    //Validamos si la tasa es nulo
                    if (tasa == null)
                    {
                        tasa = "";
                    }
                }
            }
            return tasa;
        }

        #endregion

    }
}
