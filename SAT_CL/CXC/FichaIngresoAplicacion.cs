using System;
using System.Data;
using System.Transactions;
using TSDK.Base;

namespace SAT_CL.CXC
{
    /// <summary>
    /// Clase de la tabla ficha_ingreso_aplicacion que me permite realizar operaciones sobre la tabla (inserciones,actualizaciones,consultas,etc.).
    /// </summary>
    public class FichaIngresoAplicacion :Disposable
    {
        #region Atributos
        
        /// <summary>
        /// Atributo privado que almacena el nombre del store procedure de la tabla
        /// </summary>
        private static string nom_sp = "cxc.sp_ficha_ingreso_aplicacion_tfia";

        private int _id_ficha_ingreso_aplicacion;
        /// <summary>
        /// Id que permite identificar una ficha de ingreso aplicada
        /// </summary>
        public int id_ficha_ingreso_aplicacion
        {
            get { return _id_ficha_ingreso_aplicacion; }
        }

        private int _id_tabla;
        /// <summary>
        /// Id que permite identificar a una entidad que hace referencia a una ficha de ingreso aplicacion
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }

        private int _id_registro;
        /// <summary>
        /// Id que permite identificar un registros que hace referencia a una ficha de ingreso aplicacion
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private int _id_egreso_ingreso;
        /// <summary>
        /// Id que permite identificar el ingreso que se usara paraefectuar la aplicacion a una factura.
        /// </summary>
        public int id_egreso_ingreso
        {
            get { return _id_egreso_ingreso; }
        }

        private decimal _monto_aplicado;
        /// <summary>
        /// Permite saber el monto monetario aplicado a un ingreso.
        /// </summary>
        public decimal monto_aplicado
        {
            get { return _monto_aplicado; }
        }

        private DateTime _fecha_aplicacion;
        /// <summary>
        /// Permite saber la fecha en la que se aplico un ingreso a una factura
        /// </summary>
        public DateTime fecha_aplicacion
        {
            get { return _fecha_aplicacion; }
        }

        private bool _bit_transferido_nuevo;
        /// <summary>
        /// Permite identificar el estado de envio (1 - Enviado, 0 -NoEviado) de una nueva ficha de ingreso aplicación  al sistema contable.
        /// </summary>
        public bool bit_transferido_nuevo
        {
            get { return _bit_transferido_nuevo; }
        }

        private int _id_transferido_nuevo;
        /// <summary>
        /// Identificar bit transferido en la tabla de transferencias generado por el sistema contable.
        /// </summary>
        public int id_transferido_nuevo
        {
            get { return _id_transferido_nuevo; }
        }

        private bool _bit_transferido_cancelado;
        /// <summary>
        /// Permite identificar el estado de envio (1-Enviado, 0-NoEnviado) de una cancelación de una ficha de ingreso aplicacion sistema contable.
        /// </summary>
        public bool bit_transferido_cancelado
        {
            get { return _bit_transferido_cancelado; }
        }

        private int _id_transferido_cancelado;
        /// <summary>
        /// Identificar el bit transferido cancelado la tabla de transferencias generado por el sistema contable.
        /// </summary>
        public int id_transferido_cancelado
        {
            get { return _id_transferido_cancelado; }
        }

        private bool _habilitar;
        /// <summary>
        /// Permite identificar al usuario que realizo acciones sobre el registro.
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

       #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor por default que inicializa los atributos de la clase
        /// </summary>
        public FichaIngresoAplicacion()
        {
            this._id_ficha_ingreso_aplicacion = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._id_egreso_ingreso = 0;
            this._monto_aplicado = 0.0M;
            this._fecha_aplicacion = DateTime.MinValue;
            this._bit_transferido_nuevo = false;
            this._id_transferido_nuevo = 0;
            this._bit_transferido_cancelado = false;
            this._id_transferido_cancelado = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa atributos a partir de un registro de ficha ingreso aplicación
        /// </summary>
        /// <param name="id_ficha_ingreso_aplicacion">Id que sirve como referencia para la asignación de registros a los atributos</param>
        public FichaIngresoAplicacion(int id_ficha_ingreso_aplicacion)
        {
            //Invoca al método cargaAtributoInstancia
            cargaAtributoInstancia(id_ficha_ingreso_aplicacion);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~FichaIngresoAplicacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite asignar valores a los atributos, a partir de un registro a buscar de ficha ingreso aplicación.
        /// </summary>
        /// <param name="id_ficha_ingreso_aplicacion">Id que permite realizar la busqueda de registros.</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_ficha_ingreso_aplicacion) 
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 3, id_ficha_ingreso_aplicacion, 0, 0, 0, 0.0m, null, false, 0, false, 0, 0, false, "", "" };
            //Invoca al sp de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos, que existan y que no sean nulos.
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorrido de las filas, y alamcena en la variable r los registros enconatrados 
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ficha_ingreso_aplicacion = id_ficha_ingreso_aplicacion;
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._id_egreso_ingreso = Convert.ToInt32(r["IdEgresoIngreso"]);
                        this._monto_aplicado = Convert.ToDecimal(r["MontoAplicado"]);
                        DateTime.TryParse(r["FechaAplicacion"].ToString(), out this._fecha_aplicacion);
                        this._bit_transferido_nuevo = Convert.ToBoolean(r["BitTransferidoNuevo"]);
                        this._id_transferido_nuevo = Convert.ToInt32(r["IdTransferidoNuevo"]);
                        this._bit_transferido_cancelado = Convert.ToBoolean(r["BitTransferidoCancelado"]);
                        this._id_transferido_cancelado = Convert.ToInt32(r["IdTransferidoCancelado"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor a retorno siempre y cuando cumpla las sentencia de validación de datos
                    retorno = true;
                }
            }
            //Retorno del resultado al método 
            return retorno;
        }

        /// <summary>
        /// Método que pemite actualiar registros de ficha ingreso aplciación
        /// </summary>
        /// <param name="id_tabla">Permite actualizar el campo id_tabla</param>
        /// <param name="id_registro">Permite actualizar el campo id_registro</param>
        /// <param name="id_egreso_ingreso">Permite actualizar el campo id_egreso_ingreso</param>
        /// <param name="monto_aplicado">Permite actualizar el campo monto_aplicacion</param>
        /// <param name="fecha_aplicacion">Permite actualizar el campo fecha_aplicacion</param>
        /// <param name="bit_transferido_nuevo">Permite actualizar el campo bit_transferido_nuevo</param>
        /// <param name="id_transferido_nuevo">Permite actualizar el campo id_transferido_nuevo</param>
        /// <param name="bit_transferido_cancelado">Permite actualizar el campo bit_trasnferido_cancelado</param>
        /// <param name="id_transferido_cancelado">Permite actualizar el campo id_transferido_cancelado</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarFichaIngresoAplicacion(int id_tabla, int id_registro, int id_egreso_ingreso, decimal monto_aplicado, DateTime fecha_aplicacion, bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_cancelado, int id_transferido_cancelado, int id_usuario, bool habilitar)
        {
            //Validando que la aplicación no contenga relación con un CFDI de Recepción de Pagos Activo (No Cancelado o por Cancelar)
            RetornoOperacion resultado = FacturacionElectronica33.ComprobantePagoDocumentoRelacionado.ValidarAplicacionEnCFDIRecepcionPagoActivo(this._id_tabla, this._id_ficha_ingreso_aplicacion);

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
                object[] param = { 2, this.id_ficha_ingreso_aplicacion, id_tabla, id_registro, id_egreso_ingreso, monto_aplicado, Fecha.ConvierteDateTimeObjeto(fecha_aplicacion), bit_transferido_nuevo, id_transferido_nuevo, bit_transferido_cancelado, id_transferido_cancelado, id_usuario, habilitar, "", "" };
                //Asignación de valores al objeto retorno
                resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            }

            //Retrono del resultado al método
            return resultado;
        }

        #endregion 

        #region Métodos Públicos FI

        /// <summary>
        /// Método que permite insertar registros en ficha ingreso aplicación
        /// </summary>
        /// <param name="id_tabla"> Permite insertar la entidad que hace uso de FichaIngresoAplicación</param>
        /// <param name="id_registro">Permite insertar el registro que hace uso de FichaIngresoAplicación</param>
        /// <param name="id_egreso_ingreso">Permite insertar un id_egreso_ingreso en FichaIngresoAplicación</param>
        /// <param name="monto_aplicado">Permite insertar un monto_aplicacion aplicado en FichaIngresoAplicación</param>
        /// <param name="fecha_aplicacion">Permite insertar la fecha de aplicación de FichadeIngresoAplicada</param>
        /// <param name="bit_transferido_nuevo">Permite insertar un bit_transferido_nuevo(1-Enviado,0-No Enviado)</param>
        /// <param name="id_transferido_nuevo">Permite insertar el Id generado de la tabla de transferencias del bit_transferido_nuevo</param>
        /// <param name="bit_transferido_cancelado">Permite insertar un bit_transferido_cancelado (1-Enviado,0-No Enviado)</param>
        /// <param name="id_transferido_cancelado">Permite insertar el id generado de la tabla de transferencias del bit_transferido_cancelado</param>
        /// <param name="id_usuario">Permite insertar registros en el campo id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarFichaIngresoAplicacion(int id_tabla, int id_registro, int id_egreso_ingreso, decimal monto_aplicado, DateTime fecha_aplicacion,
                                                                      bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_cancelado,
                                                                      int id_transferido_cancelado, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando si la combinación Tabla-Registro-Egreso/Ingreso no se encuentra activa (registro previo existente)
            object[] param = { 10, 0, id_tabla, id_registro, id_egreso_ingreso, 0, null, 0, 0, 0, 0, 0, false, "", "" };

            //Inicializando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Buscando coincidencias
                using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
                {
                    //Si hay elementos coincidentes y la Tabla es Facturado(Facturas de Cliente)
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table") && id_tabla == 9)
                    {
                        //Instanciando aplicación de pagos
                        using (FichaIngresoAplicacion ap = new FichaIngresoAplicacion(Convert.ToInt32(ds.Tables["Table"].Rows[0]["Id"])))
                        {
                            //Si el registro se instanció correctamente
                            if (ap.habilitar)
                            {
                                //Actualizando monto aplicado
                                retorno = ap.EditarFichaIngresoAplicacion(ap.id_tabla, ap.id_registro, ap.id_egreso_ingreso, monto_aplicado + ap.monto_aplicado, ap.fecha_aplicacion, ap.bit_transferido_nuevo, ap.id_transferido_nuevo, ap._bit_transferido_cancelado, ap.id_transferido_cancelado, id_usuario);
                            }
                            else
                                retorno = new RetornoOperacion(string.Format("No se pudo localizar la aplicación previa ID: '{0}'", ds.Tables["Table"].Rows[0]["Id"]));
                        }
                    }
                    //Si no hay elementos previos o es Factutra de Proveedor
                    else
                    {
                        //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
                        param = new object[] { 1, 0, id_tabla, id_registro, id_egreso_ingreso, monto_aplicado, Fecha.ConvierteDateTimeObjeto(fecha_aplicacion), bit_transferido_nuevo, id_transferido_nuevo, bit_transferido_cancelado, id_transferido_cancelado, id_usuario, true, "", "" };

                        //Asignación de valores al objeto retorno.
                        retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
                    }
                }

                //SI no hay errores
                if (retorno.OperacionExitosa)
                    scope.Complete();
            }

            //Retrono del resultado al método.
            return retorno;
        }

        /// <summary>
        /// Método que permite  actualizar los campos de ficha ingreso aplicación
        /// </summary>
        /// <param name="id_tabla">Permite actualizar el campo id_tabla</param>
        /// <param name="id_registro">Permite actualizar el campo id_registro</param>
        /// <param name="id_egreso_ingreso">Permite actualizar el campo id_egreso_ingreso</param>
        /// <param name="monto_aplicado">Permite actualizar el campo monto_aplicado</param>
        /// <param name="fecha_aplicacion">Permite actualizar el campo fecha_aplicacion</param>
        /// <param name="bit_transferido_nuevo">Permite actualizar el campo bit_transferido_nuevo</param>
        /// <param name="id_transferido_nuevo">Permite actualizar el campo id_transferido_nuevo</param>
        /// <param name="bit_transferido_cancelado">Permite actualizar el campo bit_transferido_cancelado</param>
        /// <param name="id_transferido_cancelado">Permite actualizar el campo id_transferido_cancelado</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarFichaIngresoAplicacion(int id_tabla, int id_registro, int id_egreso_ingreso, decimal monto_aplicado, DateTime fecha_aplicacion, 
                                                             bool bit_transferido_nuevo, int id_transferido_nuevo, bool bit_transferido_cancelado, int id_transferido_cancelado, 
                                                             int id_usuario)
        {
            //Retona e Invoca al método editarFichaIngresoAplicacion
            return this.editarFichaIngresoAplicacion(id_tabla, id_registro, id_egreso_ingreso, monto_aplicado, fecha_aplicacion, bit_transferido_nuevo, 
                                                     id_transferido_nuevo, bit_transferido_cancelado, id_transferido_cancelado,id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite  actualizar los campos de ficha ingreso aplicación
        /// </summary>
        /// <param name="id_registro">Permite actualizar el campo id_registro</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarFichaIngresoAplicacion(int id_registro,  int id_usuario)
        {
            //Retona e Invoca al método editarFichaIngresoAplicacion
            return this.editarFichaIngresoAplicacion(this._id_tabla, id_registro, this._id_egreso_ingreso, this._monto_aplicado, this._fecha_aplicacion, this._bit_transferido_nuevo,
                                                     this._id_transferido_nuevo, this._bit_transferido_cancelado, this._id_transferido_cancelado, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Confirmar el Pago de la Liqudiación
        /// </summary>
        /// <param name="id_egreso_ingreso"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaPagoFacturaAplicacion(int id_egreso_ingreso, int id_usuario)
        {
            //Retona e Invoca al método editarFichaIngresoAplicacion
            return this.editarFichaIngresoAplicacion(this.id_tabla, this.id_registro, id_egreso_ingreso, this.monto_aplicado, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                     this.bit_transferido_nuevo, this.id_transferido_nuevo, this.bit_transferido_cancelado, this.id_transferido_cancelado,
                                                     id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite cambiar el estado de habilitación de un registro
        /// </summary>
        /// <param name="id_usuario"> Id que permite identificar al usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarFichaIngresoAplicacion(int id_usuario)
        {
            //Retorna e Invoca al método privado editarFichaIngresoAplicación
            RetornoOperacion resultado = this.editarFichaIngresoAplicacion(this.id_tabla, this.id_registro, this.id_egreso_ingreso, this.monto_aplicado, this.fecha_aplicacion, this.bit_transferido_nuevo, this.id_transferido_nuevo, this.bit_transferido_cancelado, this.id_transferido_cancelado, id_usuario, false);

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método Público encargado de Obtener el Monto Disponible por cada Ficha de Ingreso
        /// </summary>
        /// <param name="id_ficha_ingreso">Ficha de Ingreso</param>
        /// <returns></returns>
        public static decimal ObtieneMontoDisponiblePorFicha(int id_ficha_ingreso)
        {
            //Declarando Objeto de Retorno
            decimal monto_disponible = 0.00M;
            
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 4, 0, 0, 0, id_ficha_ingreso, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Instanciando la Ficha de Ingreso
                    using (Bancos.EgresoIngreso ficha = new Bancos.EgresoIngreso(id_ficha_ingreso))
                    {
                        //Validando que exista el Registro
                        if (ficha.id_egreso_ingreso > 0)
                        {
                            //Recorriendo Registros
                            foreach (DataRow dr in ds.Tables["Table"].Rows)

                                //Asignando Resultado Obtenido
                                monto_disponible = ficha.monto - Convert.ToDecimal(dr["MontoAplicado"]);
                        }
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return monto_disponible;
        }
        /// <summary>
        /// Método Público encargado de Obtener las Fichas o Facturas Ligadas a los Registros
        /// </summary>
        /// <param name="id_ficha_ingreso">Ficha de Ingeso</param>
        /// <param name="id_factura">Factura Aplicada</param>
        /// <returns></returns>
        public static DataTable ObtieneFichasFacturas(int id_ficha_ingreso, int id_factura)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasFacturas = null;
            
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 5, 0, 0, id_factura, id_ficha_ingreso, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFichasFacturas = ds.Tables["Table"];
            }

            //Devolviendo 
            return dtFichasFacturas;
        }
        /// <summary>
        /// Método encargado de Validar si una Factura esta Aplicada
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <returns></returns>
        public static bool ValidaFacturaAplicada(int id_factura)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 6, 0, 0, id_factura, 0, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iniciando Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Validando que Exista un Indicador
                        if (Convert.ToInt32(dr["Aplicaciones"]) > 0)

                            //Instanciando Resultado Negativo
                            result = true;
                        else
                            //Instanciando Resultado Positivo
                            result = false;
                    }
                }
            }

            //Devolviendo 
            return result;
        }
        /// <summary>
        /// Método que obtiene las facturas apicadas a una ficha de ingreso. 
        /// </summary>
        /// <param name="id_egreso_ingreso">Identificador de una ficha de ingreso y sus aplicaciones</param>
        /// <returns></returns>
        public static DataTable FacturasAplicadasFichaIngreso(int id_egreso_ingreso)
        {
            //Declara tabla dtFacturasAplicadas
            DataTable dtFacturasAplicadas = null;
            //Creación del objeto param
            object[] param = { 7, 0, 0, 0, id_egreso_ingreso, 0, null, false, 0, false, 0, 0, false, "", "" };
            //Crea dataset que almacena el resultado del método EjecutaProcAlmacenadoDataSet
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del obtenidos del métodos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asigna a la tabla dtFacturasAplicadas los datos del dataset DS
                    dtFacturasAplicadas = DS.Tables["Table"];
            }
            //Devuelve la tabla al método
            return dtFacturasAplicadas;
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Aplicaciones de un Facturado
        /// </summary>
        /// <param name="id_factura">Facturado</param>
        /// <param name="id_comprobante">Comprobante v3.3</param>
        /// <param name="id_usuario">Usuario que actualiza el registro</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaAplicacionesFacturado(int id_factura, int id_comprobante, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Instanciando Aplicaciones
            using (DataTable dtAplicaciones = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, id_factura))
            {
                //Validando que existan Aplicaciones
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicaciones))
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtAplicaciones.Rows)
                        {
                            //Instanciando Aplicacion de la Factura
                            using (SAT_CL.CXC.FichaIngresoAplicacion fia = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(dr["Id"])))
                            {
                                //Validando que exista la Aplicación
                                if (fia.id_ficha_ingreso_aplicacion > 0)
                                {
                                    //Deshabilitando Ficha de Ingreso
                                    retorno = fia.DeshabilitarFichaIngresoAplicacion(id_usuario);
                                    //Validando Operación Exitosa
                                    if (!retorno.OperacionExitosa)
                                        //Terminando Ciclo
                                        break;
                                    else
                                    {
                                        //Instanciando Ficha de Ingreso
                                        using (SAT_CL.Bancos.EgresoIngreso fi = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                        {
                                            //Validando que exista el Registro
                                            if (fi.habilitar)
                                            {
                                                //Obteniendo Facturas Aplicadas
                                                using (DataTable dtAplicacionesFicha = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(fi.id_egreso_ingreso, 0))
                                                {
                                                    //Si no existen Aplicaciones
                                                    if (!TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicacionesFicha))
                                                    {
                                                        //Actualizando Estatus de la Ficha
                                                        retorno = fi.ActualizaFichaIngresoEstatus(SAT_CL.Bancos.EgresoIngreso.Estatus.Capturada, id_usuario);
                                                        //Validando Operación Correcta
                                                        if (retorno.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Validando Desaplicación Exitosa
                        if (retorno.OperacionExitosa)
                        {
                            //Declaramos Variable
                            int facturado = 0;
                            //Obtenemos Facturado Fcaturacion
                            facturado = SAT_CL.Facturacion.FacturadoFacturacion.ObtieneRelacionFacturaElectronicav3_3(id_comprobante);
                            //Instanciamos FcaturadoFacturacion
                            using (SAT_CL.Facturacion.FacturadoFacturacion objfacturado = new SAT_CL.Facturacion.FacturadoFacturacion(facturado))
                            {
                                //Instanciamos Facturado
                                using (SAT_CL.Facturacion.Facturado objFacturado = new SAT_CL.Facturacion.Facturado(objfacturado.id_factura))
                                {
                                    //Actualizando Estatus de la Factura
                                    retorno = objFacturado.ActualizaEstatusFactura(SAT_CL.Facturacion.Facturado.EstatusFactura.Registrada, id_usuario);

                                    //Validando Operación Exitosa
                                    if (retorno.OperacionExitosa)
                                    {
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion(id_factura);
                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion(id_factura);
            }
            //Devolviendo Resultado Obtenido
            return retorno;
        }
        
        #endregion

        #region MÉTODOS FACTURA PROVEEDOR

        /// <summary>
        /// Método encargado de Obtener los Anticipos y/ó las Facturas de Proveedor
        /// </summary>
        /// <param name="id_anticipo_proveedor">Anticipo de Proveedor</param>
        /// <param name="id_factura_proveedor">Factura de Proveedor</param>
        /// <returns></returns>
        public static DataTable ObtieneAnticiposFacturasProveedor(int id_anticipo_proveedor, int id_factura_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasFacturas = null;

            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 8, 0, 0, id_factura_proveedor, id_anticipo_proveedor, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFichasFacturas = ds.Tables["Table"];
            }

            //Devolviendo 
            return dtFichasFacturas;
        }
        /// <summary>
        /// Realiza el rechazo de un pago programado a una factura de proveedor
        /// </summary>
        /// <param name="razon_rechazo">Razón del rechazo</param>
        /// <param name="id_usuario">Id de usuario que rechaza</param>
        /// <returns></returns>
        public RetornoOperacion RechazaPagoFacturaProveedor(string razon_rechazo, int id_usuario)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que sea una aplicación de factura de proveedor
            if (this._id_tabla == 72)
            {
                //Inicializando Bloque
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Deshabilitando Aplicación
                    resultado = DeshabilitarFichaIngresoAplicacion(id_usuario);

                    //Validando que la Operación fuese Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Factura de Proveedor
                        using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(this._id_registro))
                        {
                            //Validando que existe la Factura
                            if (fp.id_factura > 0)
                            {
                                //Actualizando la Factura
                                resultado = fp.ActualizaEstatusFacturadoProveedor(fp.ObtieneEstatusFacturaAplicada(), id_usuario);

                                //Validando que se haya Actualizado Exitosamente
                                if (resultado.OperacionExitosa)
                                {
                                    //Registrando razón de rechazo
                                    resultado = SAT_CL.Global.Referencia.InsertaReferencia(this._id_ficha_ingreso_aplicacion, 103, Global.ReferenciaTipo.ObtieneIdReferenciaTipo(fp.id_compania_receptor, 103, "Razón Rechazo", 0, "Datos Contables"), razon_rechazo, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario);

                                    //Si no hay errores en el registro de razón de rechazo
                                    if (resultado.OperacionExitosa)
                                        //Completando Transacción
                                        trans.Complete();
                                    else
                                        resultado = new RetornoOperacion("Error al actualizar razón de rechazo.");
                                }
                                else
                                    resultado = new RetornoOperacion("Error al actualizar estatus de Factura de Proveedor.");
                            }
                            else
                            resultado = new RetornoOperacion("Error al localizar factura de proveedor.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion(string.Format("Error al deshabilitar la aplicación de pago: {0}", resultado.Mensaje));
                }
            }
            else
                resultado = new RetornoOperacion("Esta aplicación de pago no corresponde a una Factura de Proveedor.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Obtener las Aplicaciones de una Factura de Proveedor
        /// </summary>
        /// <param name="id_factura_proveedor">Factura Deseada</param>
        /// <returns></returns>
        public static DataTable ObtieneAplicacionesFacturasProveedor(int id_factura_proveedor)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasFacturas = null;

            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 9, 0, 0, id_factura_proveedor, 0, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFichasFacturas = ds.Tables["Table"];
            }

            //Devolviendo 
            return dtFichasFacturas;
        }
        /// <summary>
        /// Método encargado de Obtener las Aplicaciones de una Factura contra un Egreso
        /// </summary>
        /// <param name="id_tabla">Entidad (72.- CXP, 9.- CXC)</param>
        /// <param name="id_registro">Registro de la Entidad (Factura)</param>
        /// <param name="id_egreso_ingreso">Egreso de la Aplicación</param>
        /// <returns></returns>
        public static DataTable ObtieneAplicacionesFacturas(int id_tabla, int id_registro, int id_egreso_ingreso)
        {
            //Declarando Objeto de Retorno
            DataTable dtFichasFacturas = null;

            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla.
            object[] param = { 10, 0, id_tabla, id_registro, id_egreso_ingreso, 0, null, false, 0, false, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtFichasFacturas = ds.Tables["Table"];
            }

            //Devolviendo 
            return dtFichasFacturas;
        }

        #endregion
    }
}
