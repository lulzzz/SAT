using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;
using FEv32 = SAT_CL.FacturacionElectronica;
using FEv33 = SAT_CL.FacturacionElectronica33;
namespace SAT_CL.Facturacion
{
    /// <summary>
    /// 
    /// </summary>
    public class FacturaGlobal : Disposable
    {
        #region Enumeración

        /// <summary>
        /// Enumera los estatus que puede tener una factura global
        /// </summary>
        public enum Estatus
        {
            /// <summary>
            /// Permite saber  el estado inicial de una factura 
            /// </summary>
            Registrada = 1,
            /// <summary>
            /// Estado final de una Factura
            /// </summary>
            Facturado = 2,
            /// <summary>
            /// Permite saber si se cancelo una factura
            /// </summary>
            Cancelada = 3,
            /// <summary>
            /// 
            /// </summary>
            NotaCredito = 4
        };

        #endregion

        #region Atributos

        /// <summary>
        /// Atributo que almacena el nombre del sp de la tabla 
        /// </summary>
        private static string nom_sp = "facturacion.sp_factura_global_tfg";

        private int _id_factura_global;
        /// <summary>
        /// ID que permite identificar el Registro de la Factura Global
        /// </summary>
        public int id_factura_global { get { return this._id_factura_global; } }

        private int _id_compania_cliente;
        /// <summary>
        /// ID que permite identificar la compania del cliente de las facturas
        /// </summary>
        public int id_compania_cliente { get { return _id_compania_cliente; } }

        private int _id_compania;
        /// <summary>
        /// ID que permite identificar la compania emisora de la factura
        /// </summary>
        public int id_compania { get { return _id_compania; } }

        private string _no_factura_global;
        /// <summary>
        /// ID que permite identificar el número consecutivo de la factura
        /// </summary>
        public string no_factura_global { get { return _no_factura_global; } }

        private byte _id_estatus;
        /// <summary>
        /// Permite identificar el estatus de una factura global
        /// </summary>
        public byte id_estatus { get { return _id_estatus; } }
        /// <summary>
        /// Permite acceder a los elementos de la enumeración Estatus.
        /// </summary>
        public Estatus estatus { get { return (Estatus)this._id_estatus; } }

        private int _id_factura_electronica;
        /// <summary>
        /// ID que permite identificar la factura electronica de la factura global
        /// </summary>
        public int id_factura_electronica { get { return _id_factura_electronica; } }

        private int _id_factura_electronica33;
        /// <summary>
        /// ID que permite identificar la factura electronica V3.3 de la factura global
        /// </summary>
        public int id_factura_electronica33 { get { return _id_factura_electronica33; } }

        private string _descripcion;
        /// <summary>
        /// Describe el concepto de la factura global
        /// </summary>
        public string descripcion { get { return _descripcion; } }

        private bool _habilitar;
        /// <summary>
        /// Permite identificar el estado de un registro (Habilitado/Deshabilitado)
        /// </summary>
        public bool habilitar { get { return _habilitar; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor por default que inicializa los atributos de la clase.
        public FacturaGlobal()
        {
            //Asignando Valores
            this._id_factura_global = 0;
            this._id_compania_cliente = 0;
            this._id_compania = 0;
            this._no_factura_global = "";
            this._id_estatus = 0;
            this._id_factura_electronica = 
            this._id_factura_electronica33 = 0;
            this._descripcion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_factura_global">Id de Factura Global</param>
        public FacturaGlobal(int id_factura_global)
        {
            //Invoca al método cargaInstanciaAtributos
            cargaAtributoInstancia(id_factura_global);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~FacturaGlobal()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que permite la carga de atributos a partir de un ID de busqueda en la tabla
        /// </summary>
        /// <param name="id_factura_global">Id que sirve de referencia para la búsqueda de registros</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_factura_global)
        {
            //Creación del objeto retorno
            bool retorno = false;
            
            //Creación y Asignación de valores al arreglo necesarios para el store procedure de la tabla
            object[] param = { 3, id_factura_global, 0, 0, "", 0, 0, 0, "", 0, false, "", "" };
            
            //Invoca al SP de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSetParametrosDefault(nom_sp, param))
            {
                //Valida los datos de la tabla, que existan y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y almacena en la variable r los valores de las filas.
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        //Asignando Valores
                        this._id_factura_global = id_factura_global;
                        this._id_compania_cliente = Convert.ToInt32(r["IdCompaniaCliente"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._no_factura_global = r["NoFacturaGlobal"].ToString();
                        this._id_estatus = Convert.ToByte(r["IdEstatus"]);
                        this._id_factura_electronica = Convert.ToInt32(r["IdFacturaElectronica"]);
                        this._id_factura_electronica33 = Convert.ToInt32(r["IdFacturaElectronica33"]);
                        this._descripcion = r["Descripcion"].ToString();
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia de valor al objeto retorno siempre y cuando cumpla la validación de los datos
                    retorno = true;
                }
            }
            //Retorna al método el objeto
            return retorno;
        }

        /// <summary>
        /// Método que permite actualizar los registros de FacturaGlobal
        /// </summary>
        /// <param name="id_compania_cliente">Compania del Cliente</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="no_factura_global">No. de Factura</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_factura_electronica">Factura Electronica</param>
        /// <param name="id_factura_electronica33">Factura Electronica 3.3</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarFacturaGlobal(int id_compania_cliente, int id_compania, string no_factura_global, byte id_estatus,
                                                     int id_factura_electronica, int id_factura_electronica33, string descripcion, 
                                                     int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Creación y Asignación de valores al arreglo necesarios para el SP de la tabla
            object[] param = { 2, this._id_factura_global, id_compania_cliente, id_compania, no_factura_global, id_estatus, 
                               id_factura_electronica, id_factura_electronica33, descripcion, id_usuario, habilitar, "", "" };

            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);

            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que Inserta registros en Factura Global
        /// </summary>
        /// <param name="id_compania_cliente">Compania del Cliente</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="no_factura_global">No. de Factura</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="id_factura_electronica">Factura Electronica</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarFacturaGlobal(int id_compania_cliente, int id_compania, string no_factura_global, Estatus estatus,
                                                     int id_factura_electronica, int id_factura_electronica33, string descripcion, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Creación y asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 1, 0, id_compania_cliente, id_compania, no_factura_global, (byte)estatus, 
                               id_factura_electronica, id_factura_electronica33, descripcion, id_usuario, true, "", "" };
            
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            
            //Retorno del resultado al método
            return retorno;
        }
        /// <summary>
        /// Método encargado de Reestablecer la Factura Global
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion ReestableceFacturaGlobal(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Registrada,
                                       0, 0, descripcion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método que permite actualizar los registros de FacturaGlobal
        /// </summary>
        /// <param name="id_compania_cliente">Permite actualizar el identificador de la factura</param>
        /// <param name="id_factura_electronica">Permite actualizar el identificador de la factura electronica</param>
        /// <param name="id_factura_electronica33"></param>
        /// <param name="descripcion">Permite actualizar la descripcion de la factura global</param>
        /// <param name="estatus">Permite actualizar el estatus de una factura global</param>
        /// <param name="id_usuario">Permite actualizar el identifiacdor del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarFacturaGlobal(int id_compania_cliente, int id_compania, Estatus estatus,
                                                    int id_factura_electronica, int id_factura_electronica33,
                                                    string descripcion, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("La Factura Global ha sido Timbrada.");

            //Validamos Estatus de la Factura Global
            if ((Estatus)this._id_estatus == Estatus.Registrada)
            {
                //Validando Cambio de Cliente
                if (this._id_compania_cliente != id_compania_cliente)
                {
                    //Obteniendo Facturas Ligadas
                    using (DataTable dtFacturas = SAT_CL.Facturacion.FacturadoFacturacion.ObtieneDetallesFacturacionFacturaGlobal(this._id_factura_global))
                    {
                        //Validando Existencia
                        if (Validacion.ValidaOrigenDatos(dtFacturas))

                            //Instanciando Excepción
                            resultado = new RetornoOperacion("La Factura Global tiene facturas agregadas");
                        else
                            //Instanciando Resultado Positivo
                            resultado = new RetornoOperacion(this._id_factura_global);
                    }
                }
                else
                    //Instanciando Resultado Positivo
                    resultado = new RetornoOperacion(this._id_factura_global);

                //Validando Resultado
                if (resultado.OperacionExitosa)

                    //Retorna e Invoca al método privado editarFacturaGlobal
                    resultado = this.editarFacturaGlobal(id_compania_cliente, id_compania, this._no_factura_global, (byte)estatus,
                                       id_factura_electronica, id_factura_electronica33, descripcion, id_usuario, this._habilitar);
            }

            //Devolvemos resultado
            return resultado;
        }

        /// <summary>
        /// Método que permite cambiar el estado de un registro(Habilitado/Deshabilitado)
        /// </summary>
        /// <param name="id_usuario">Permite Identificar al usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarFacturaGlobal(int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validamos que se encuentre enn estatus Registrada
            if ((Estatus)this._id_estatus == Estatus.Registrada && (this._id_factura_electronica == 0 && this._id_factura_electronica33 == 0))
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Retorna e Invoca el método editarFacturadoFacturación
                    result = this.editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, this._id_estatus,
                                   this._id_factura_electronica, this._id_factura_electronica33, this._descripcion, id_usuario, false);

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Detalles Activos
                        using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesSinFERegistradosFacturaGlobal(this._id_factura_global))
                        {
                            //Validando que existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtDetalles.Rows)
                                {
                                    //Instanciando Facturado Facturación
                                    using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista el Registro
                                        if (ff.habilitar)
                                        {
                                            //Deshabilitando el Registro
                                            result = ff.DeshabilitaFacturadoFacturacion(id_usuario);

                                            //Si no se actualizo
                                            if (!result.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }
                                        else
                                        {   //Instanciando Excepción
                                            result = new RetornoOperacion("No se puede acceder al Detalle");

                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Factura Global
                                result = new RetornoOperacion(this._id_factura_global);

                            //Validando que la Actualización fue Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Factura Global
                                result = new RetornoOperacion(this._id_factura_global);

                                //Completando Transacción
                                trans.Complete();
                            }
                        }
                    }
                }
            }
            else
                result = new RetornoOperacion("La factura ya ha sido regitrada a Factutación Electrónica.");
            
            //Regresa Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Recalcula el Tipo de Cambio de los detalles de la Factura Global
        /// </summary>
        /// <param name="id_moneda">Id Moneda</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        private RetornoOperacion RecalculaDetallesTipoCambio(byte id_moneda, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesSinFERegistradosFacturaGlobal(this._id_factura_global))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Instanciamos Factura
                                    using (Facturado objFacturado = new Facturado(ff.id_factura))
                                    {
                                        //Recalcula Tipo de Cambio
                                        resultado = objFacturado.ActualizaMontosTipoCambio(this._id_compania, Fecha.ObtieneFechaEstandarMexicoCentro(), id_moneda, id_usuario);
                                    }
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                    }
                    //Validamos Resultado
                    if(resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
                //Devolvemos Objeto Resultado
                return resultado;
            }
        }
     
       /// <summary>
       ///  Actualiza Estatus a Facturado de la Factura Global
       /// </summary>
       /// <param name="id_usuario">Usuario</param>
       /// <returns></returns>
        public RetornoOperacion ActualizaAFacturadoEstatusFacturaGlobal(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    
                    
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobal(this._id_factura_electronica))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, ff.id_factura_electronica, ff.id_factura_electronica33, FacturadoFacturacion.Estatus.Facturada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Facturado, this._id_factura_electronica,
                                                            this._id_factura_electronica33, this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        ///  Actualiza Estatus a Facturado de la Factura Global
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaAFacturadoEstatusFacturaGlobalV3_3(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobalV3_3(this._id_factura_electronica33))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, ff.id_factura_electronica, ff.id_factura_electronica33, FacturadoFacturacion.Estatus.Facturada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Facturado, this._id_factura_electronica,
                                                            this._id_factura_electronica33, this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        /// Registra la Diferencia de Viaje
        /// </summary>
        /// <param name="monto_a_facturar">Monto que se desea Facturar</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion RegistraDiferenciaViaje(decimal monto_a_facturar, int id_usuario)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            
            //Declaramos Variables de Salida
            decimal subtotal, total_trasladado, total_retenido, total= 0;

            //Obtenemos lod datos de la factura global
            TotalFacturaGlobalPorRegistrar(this._id_factura_global, out subtotal, out total_trasladado, out total_retenido, out total);

            //Validamos que el Monto a Facturar y el actual no tengas diferencias
            if (monto_a_facturar != total)
            {
                decimal cantidad_correcta, diferencia = 0;
                //Declaramos Variables de Salida
                int id_factura , id_concepto_cobro = 0;
                decimal tasa_retenido, tasa_trasladado = 0;
                byte tipo_trasladado,id_unidad, tipo_retenido = 0;
                //Obtenemos Diferencia
                 diferencia = monto_a_facturar - total;

                //Obtenemos Datos del Viaje Principal
                ObtieneDatosDelViajePrincipal(this.id_factura_global, out id_factura, out tasa_trasladado, out tasa_retenido, out tipo_trasladado, out tipo_retenido, out id_unidad, out id_concepto_cobro);

                //Calculamos la Diferencia de Viaje
                resultado = CalculaDiferenciaViaje(out cantidad_correcta, monto_a_facturar, diferencia, tasa_trasladado, tasa_retenido, subtotal, total_trasladado, total_retenido, total);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Instanciamos Factura
                    using(SAT_CL.Facturacion.Facturado objFacturado = new Facturado(id_factura)
)                    {
                        //Insertamos Concepto
                        resultado = SAT_CL.Facturacion.FacturadoConcepto.InsertaFacturaConcepto(objFacturado.id_factura, (decimal)1, id_unidad, "", id_concepto_cobro, 1 * cantidad_correcta, tipo_retenido, tasa_retenido, tipo_trasladado, tasa_trasladado,0, id_usuario);
                    }
                }
            }
           
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Calculamos Diferencia de Viaje
        /// </summary>
        /// <param name="cantidad_correcta">cantidad correcta a Registrar en el concepto</param>
        /// <param name="monto_a_facturar">Monto que se desea facturar</param>
        /// <param name="diferencia">Diferencia de Monto a facturar - Total de la Factura</param>
        /// <param name="tasa_t">Tasa de Traladado de la factura actual</param>
        /// <param name="tasa_r">tasa de Retenido de la factura actual</param>
        /// <param name="subtotal_fg">Subtotal de la factura actual</param>
        /// <param name="trasladado_fg">total de trasladado de la factura actual</param>
        /// <param name="retenidos_fg">total de retenido de la factura actual</param>
        /// <param name="total_fg">total de la factura actual</param>
        /// <returns></returns>
        public RetornoOperacion CalculaDiferenciaViaje(out decimal cantidad_correcta,decimal monto_a_facturar, decimal diferencia, decimal tasa_t, decimal tasa_r, decimal subtotal_fg, 
                                                       decimal trasladado_fg, decimal retenidos_fg, decimal total_fg)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Obtiene Calculo de Total
            decimal tasa_a_calcular= 1  +(((tasa_t - tasa_r))/100);
            decimal cantidad =System.Math.Round(diferencia/tasa_a_calcular, 2,MidpointRounding.ToEven);
             cantidad_correcta = 0;

            //Calculamos valor de la factura global de acuerdo a la diferencia que se desea
            for (int i = 1; i <= Convert.ToInt32(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Maximo incrementa factura")); i++)
            {
                //Calculamos Subtotal por registrar
                decimal subtotal = 1 * cantidad;
                //Calculamos Trasladados por reggistrar
                decimal total_trasladados = subtotal * (tasa_t/100);
                //Calculamos retenidos por registrar
                decimal total_retenidos = subtotal * (tasa_r/100);
                //Calculamos total por registrar
                decimal total = (subtotal + total_trasladados) - total_retenidos;

                //Validamos que el monto sea igual al deseado
                if (monto_a_facturar.ToString() == Cadena.RegresaCadenaSeparada((total + total_fg).ToString(),'.',0) + "."+ Cadena.TruncaCadena(Cadena.RegresaCadenaSeparada((total + total_fg).ToString(),'.',1),2,""))
                {
                    //Asignamos cantidad por registrar
                    cantidad_correcta = cantidad;
                    //Salimos del ciclo
                    break;
                }
                //Añadimos decima a Cantidad
                cantidad = cantidad + (decimal).01;
            }
            //Devolvemos Valor
        return resultado;
        }
        
        /// <summary>
        ///  Validamos Monto de la Factura Global
        /// </summary>
        /// <param name="id_factura_global">Id de la Fcatura Global</param>
        /// <param name="subtotal">Subtotal de la Factura</param>
        /// <param name="total_trasladados">total trasladado de la factura</param>
        /// <param name="total_retenido">total retenido de la factura</param>
        /// <param name="total">total de la factura</param>
        public static void TotalFacturaGlobalPorRegistrar(int id_factura_global, out decimal subtotal, out decimal total_trasladados, out decimal total_retenido, out decimal total)
        {
            subtotal = 0;
            total_trasladados = 0;
            total_retenido = 0;
            total = 0;

            //Creación y Asignación de valores al arreglo necesarios para el store procedure de la tabla
            object[] param = { 5, id_factura_global, 0, 0, "", 0, 0, 0, "", 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Datos
                    subtotal = (from DataRow r in ds.Tables[0].Rows
                                select Convert.ToDecimal(r["subtotal"])).FirstOrDefault();
                    //Obtenemos Datos
                    total_trasladados = (from DataRow r in ds.Tables[0].Rows
                                         select Convert.ToDecimal(r["total_retenido"])).FirstOrDefault();
                    //Obtenemos Datos
                    total_retenido = (from DataRow r in ds.Tables[0].Rows
                                      select Convert.ToDecimal(r["total_trasladados"])).FirstOrDefault();
                    //Obtenemos Datos
                    total = (from DataRow r in ds.Tables[0].Rows
                             select Convert.ToDecimal(r["total"])).FirstOrDefault();

                }

            }
        }

       /// <summary>
        ///  Método encargado de Obtener los Datos del Viaje Principal
       /// </summary>
       /// <param name="id_factura_global">Id Factura Global</param>
       /// <param name="id_factura">Id Factura</param>
       /// <param name="tasa_trasladado">Tasa trasladado</param>
       /// <param name="tasa_retenido">Tasa retenido</param>
       /// <param name="tipo_trasladado">Tipo de Trasladado</param>
       /// <param name="tipo_retenido">Tipo Retenido</param>
       /// <param name="id_unidad">Id Unidad</param>
       /// <param name="id_concepto_cobro">Id Concepto de Cobro</param>
        public static void ObtieneDatosDelViajePrincipal(int id_factura_global, out int id_factura, out decimal tasa_trasladado, out decimal tasa_retenido,
                                                    out byte tipo_trasladado, out byte tipo_retenido, out byte id_unidad, out int id_concepto_cobro)
        {
            //Declaramos Resultados
            id_factura = 0;
            tasa_trasladado = 0;
            tasa_retenido = 0;
            tipo_trasladado = 0;
            tipo_retenido = 0;
            id_unidad = 0;
            id_concepto_cobro = 0;


            //Creación y Asignación de valores al arreglo necesarios para el store procedure de la tabla
            object[] param = { 6, id_factura_global, 0, 0, "", 0, 0, 0, "", 0, false, "", "" };

            //Obtenemos Resultados
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Id Facatura
                    id_factura = (from DataRow r in ds.Tables[0].Rows
                                  select Convert.ToInt32(r["IdFactura"])).FirstOrDefault();
                    //Obtenemos tasa trasladado
                    tasa_trasladado = (from DataRow r in ds.Tables[0].Rows
                                       select Convert.ToDecimal(r["TasaTrasladado"])).FirstOrDefault();
                    //Obtenemos tipo trasladado
                    tipo_trasladado = (from DataRow r in ds.Tables[0].Rows
                                       select Convert.ToByte(r["TipoTrasladado"])).FirstOrDefault();
                    //Obtenemos tasa retenido
                    tasa_retenido = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToDecimal(r["TasaRetenido"])).FirstOrDefault();
                    //Obtenemos tipo retenido
                    tipo_retenido = (from DataRow r in ds.Tables[0].Rows
                                     select Convert.ToByte(r["TipoRetenido"])).FirstOrDefault();
                    //Obtenemos Id Unidad
                    id_unidad = (from DataRow r in ds.Tables[0].Rows
                                 select Convert.ToByte(r["IdUnidad"])).FirstOrDefault();
                    //Obtenemos Concepto de Cobro
                    id_concepto_cobro = (from DataRow r in ds.Tables[0].Rows
                                         select Convert.ToInt32(r["IdConcepto"])).FirstOrDefault();

                }

            }
        }

        #region CFDI 3.2

        /// <summary>
        /// Importa Facturado a Facturación Electrónica  (Detalle de Conceptos)
        /// </summary>
        /// <param name="id_forma_pago">Id Forma de Pago</param>
        /// <param name="no_cuenta_pago">Num Cuenta Pago</param>
        /// <param name="id_metodo_pago">Id metodo de Pago</param>
        /// <param name="id_condiciones_pago">Id Condiciones de Pago</param>
        /// <param name="id_sucursal">Id Sucursal</param>
        /// <param name="id_compania">Id Compania</param>
        /// <param name="id_tipo_facturacion_electronica">Tipo facturación Global (1. Detalle desglosa cada concepto / 2. General agrupa los conceptos  / 3. Un Solo Concepto</param>
        ///<param name="id_referencias">Referencias de Servicio</param>
        ///<param name="no_identificacion">No Identicación del concepto</param>
        ///<param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ImportaFacturaGlobal_V3_2(decimal monto_a_facturar, byte id_forma_pago, int no_cuenta_pago, byte id_metodo_pago, byte id_condiciones_pago, int id_sucursal, int id_compania, byte id_tipo_facturacion_electronica, string id_referencias, string no_identificacion, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declaramos fecha para el tipo de cambio
            DateTime fecha_tc = DateTime.MinValue;
            //Declaramos Variable para almacenar los datos para la Importación de la Factura Global
            DataSet dsFactura = null;
            //Validamos que no esixta una Facturación Electrónica
            if (this._id_factura_electronica == 0)
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Asignamos valor a la Fecha de Tipo de Cambio
                    fecha_tc = Fecha.ObtieneFechaEstandarMexicoCentro();
                    //TODO :
                    //Recalcula los detalles de la Factura Global
                    // resultado = RecalculaDetallesTipoCambio(FacturadoFacturacion.ObtieneMonedaFacturaGlobal(this._id_factura_global), id_usuario);
                    resultado = Facturacion.FacturadoFacturacion.ValidaMonedaFacturaGlobal(this._id_factura_global);

                    //Validamos Recalculo de Tipo de Cambio
                    if (resultado.OperacionExitosa)
                    {
                        bool impresion_detalle = true;

                        //De acuerdo al Tipo de Importación de la Factura Global
                        //En caso de ser a detalles
                        if (id_tipo_facturacion_electronica == 1)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturaGlobalDetalleFacturaElectronica(this._id_factura_global, id_forma_pago, no_cuenta_pago, id_metodo_pago,
                                                                                                                          id_condiciones_pago, 1, fecha_tc, id_referencias);
                        }
                        //En caso de ser General (Agrupando por concepto, montos iguales de concepto)
                        else if (id_tipo_facturacion_electronica == 2)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturaGlobalGeneralFacturaElectronica(this._id_factura_global, id_forma_pago, no_cuenta_pago, id_metodo_pago,
                                                                                                                 id_condiciones_pago, 1, fecha_tc, id_referencias);
                            impresion_detalle = false;
                        }
                        //En caso de ser General (Agrupando por concepto, y tasas de impuestos)
                        else if (id_tipo_facturacion_electronica == 4)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturaGlobalGeneralConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, no_cuenta_pago, id_metodo_pago,
                                                                                                                 id_condiciones_pago, 1, fecha_tc, id_referencias);

                            //Extrayendo Tabla de conceptos
                            DataTable mConceptos = OrigenDatos.RecuperaDataTableDataSet(dsFactura, "Table1");
                            if (mConceptos != null)
                            {
                                //Agrupando elementos de concepto
                                List<KeyValuePair<int, int>> lista = (from DataRow r in mConceptos.Rows select r).GroupBy(concepto => concepto.Field<int>("Descripcion")).Select(group => new KeyValuePair<int, int>(group.Key, group.Count())).Where(c => c.Value > 1).ToList();

                                //Si no existe duplicidad de líneas de concepto
                                if (lista.Count == 0)
                                    impresion_detalle = false;
                                else
                                    resultado = new RetornoOperacion("Existen Conceptos iguales con tasas de impuesto distintas.");
                            }
                        }
                        else
                        {
                            //Un Solo Concepto
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, no_cuenta_pago, id_metodo_pago,
                                                                                                                 id_condiciones_pago, 1, fecha_tc, no_identificacion);


                            //Validamos un Soló Concepto
                            if (Validacion.ValidaOrigenDatos(dsFactura.Tables["Table5"]))
                            {
                                resultado = new RetornoOperacion("Revise los conceptos de los Servicios.");
                            }

                            //Validamos Impuestos
                            if (Validacion.ValidaOrigenDatos(dsFactura.Tables["Table4"]))
                            {
                                resultado = new RetornoOperacion("Revise los impuestos de los Conceptos");
                            }
                            impresion_detalle = false;

                            //Validamo Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Registramos Diferencia de Viaje en caso de Existir
                                resultado = RegistraDiferenciaViaje(monto_a_facturar, id_usuario);
                            }

                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.Facturacion.Reporte.ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, no_cuenta_pago, id_metodo_pago,
                                                                                                                 id_condiciones_pago, 1, fecha_tc, no_identificacion);
                        }

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos Registros
                            if (Validacion.ValidaOrigenDatos(dsFactura))
                            {
                                if (id_tipo_facturacion_electronica == 3)
                                {
                                    //Importamos Factura a Factura Electrónica
                                    resultado = FEv32.Comprobante.ImportaComprobante_V3_2(dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], dsFactura.Tables["Table3"], null, null, FEv32.Comprobante.OrigenDatos.FacturaGlobal, null, null, id_compania, id_sucursal, impresion_detalle, fecha_tc, id_usuario);
                                }
                                else
                                {
                                    //Importamos Factura a Factura Electrónica
                                    resultado = FEv32.Comprobante.ImportaComprobante_V3_2(dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], dsFactura.Tables["Table3"], dsFactura.Tables["Table4"], dsFactura.Tables["Table5"], FEv32.Comprobante.OrigenDatos.FacturaGlobal, dsFactura.Tables["Table6"], dsFactura.Tables["Table7"], id_compania, id_sucursal, impresion_detalle, fecha_tc, id_usuario);
                                }
                                //Validamos Resultaod de Timbrado
                                if (resultado.OperacionExitosa)
                                {
                                    //Editamo Relación de la Factura y Facturación Electrónica
                                    resultado = EditaSinFERegistradoFacturacionElectronica(resultado.IdRegistro, id_usuario);

                                    //Validamos Rsultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Devolvemos Id Factura
                                        resultado = new RetornoOperacion(this._id_factura_global);

                                        //Completamos Transacción
                                        scope.Complete();
                                    }
                                }
                            }
                            else
                            {
                                resultado = new RetornoOperacion("No se encontró Información para exportación de la Factura.");
                            }
                        }
                    }
                }
            }
            else
                resultado = new RetornoOperacion("La Factura global ya ha sido registrada.");
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Timbra  Facturación Electrónica
        /// </summary>
        /// <param name="serie">Serie para asignar la Factura</param>
        /// <param name="omitir_addenda">Si se desea Ommitir la Addenda</param>
        /// <param name="ruta_xslt_co">URI del archivo de transformación</param>
        /// <param name="ruta_xslt_co_local">URI del archivo de transformación a utilizar si el primero no puede ser utilizado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TimbraFacturaGlobal_V3_2(string serie, bool omitir_addenda, string ruta_xslt_co,
                                                                 string ruta_xslt_co_local, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {

                //Validamos que existan Relación
                if (this._id_factura_electronica > 0)
                {
                    //Instanciamos Comprobante
                    using (FEv32.Comprobante objComprobante = new FEv32.Comprobante(this._id_factura_electronica))
                    {
                        //Validamos existencia de comprobante
                        if (objComprobante.id_comprobante > 0)
                        {
                            //Timbramos Fctura
                            resultado = objComprobante.TimbraComprobante(serie, id_usuario, ruta_xslt_co, ruta_xslt_co_local, omitir_addenda);

                            //Validamos Resultaod de Timbrado
                            if (resultado.OperacionExitosa)
                            {
                                //Editamos Estatus a Timbrado
                                resultado = ActualizaAFacturadoEstatusFacturaGlobal(id_usuario);

                                //Validamos Rsultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualizamos atributos del Comprobante
                                    objComprobante.RefrescaAtributosInstancia();
                                    //Devolvemos Id Factura
                                    resultado = new RetornoOperacion(this._id_factura_global, "La factura Global ha sido Timbrado " + objComprobante.serie + objComprobante.folio + " .", true);

                                    //Completamos Transacción
                                    scope.Complete();
                                }
                            }
                        }
                        else
                        {
                            //Establecemos mensaje Resultado
                            resultado = new RetornoOperacion("No se encontró registró Factura Electrónica.");
                        }
                    }
                }
                else
                {
                    //Establecemos mensaje Resultado
                    resultado = new RetornoOperacion("No se encontró registró Factura Electrónica.");
                }
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Editamos Id Factura Electronica
        /// </summary>
        /// <param name="id_facturacion_electronica">Id Factura Electronica</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaSinFERegistradoFacturacionElectronica(int id_facturacion_e, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesSinFERegistradosFacturaGlobal(this._id_factura_global))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, id_facturacion_e, ff.id_factura_electronica33, (FacturadoFacturacion.Estatus)ff.id_estatus, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, this._id_estatus, id_facturacion_e,
                                                            this._id_factura_electronica33, this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        ///  Actualiza Estatus a Cancelado de la Factura Global
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaACanceladoEstatusFacturaGlobal(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobal(this._id_factura_electronica))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, ff.id_factura_electronica, ff.id_factura_electronica33, FacturadoFacturacion.Estatus.Cancelada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                            //Recorriendo Ciclo pa insercción de nuevos Registros
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = FacturadoFacturacion.InsertarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, 0, 0, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Registrada, 0, 0,
                                                           this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        /// Evento generado para dar Reversa a la Factura Global
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ReversaFacturaGlobal(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobal(this._id_factura_electronica))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, 0, 0, FacturadoFacturacion.Estatus.Registrada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Registrada, 0, 0,
                                                           this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }

        #endregion

        #region CFDI 3.3

        /// <summary>
        /// Importa Facturado a Facturación Electrónica  (Detalle de Conceptos)
        /// </summary>
        /// <param name="monto_a_facturar"></param>
        /// <param name="id_forma_pago">Id Forma de Pago</param>
        /// <param name="no_cuenta_pago">Num Cuenta Pago</param>
        /// <param name="id_metodo_pago">Id metodo de Pago</param>
        /// <param name="id_uso_cfdi"></param>
        /// <param name="condiciones_pago">Id Condiciones de Pago</param>
        /// <param name="id_sucursal">Id Sucursal</param>
        /// <param name="id_compania">Id Compania</param>
        /// <param name="id_tipo_facturacion_electronica">Tipo facturación Global (1. Detalle desglosa cada concepto / 2. General agrupa los conceptos  / 3. Un Solo Concepto</param>
        ///<param name="id_referencias">Referencias de Servicio</param>
        ///<param name="no_identificacion">No Identicación del concepto</param>
        ///<param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ImportaFacturaGlobal_V3_3(decimal monto_a_facturar, int id_forma_pago, int id_metodo_pago, int id_uso_cfdi, int id_sucursal, int id_compania, byte id_tipo_facturacion_electronica, string id_referencias, string no_identificacion, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declaramos fecha para el tipo de cambio
            DateTime fecha_tc = DateTime.MinValue;
            //Declaramos Variable para almacenar los datos para la Importación de la Factura Global
            DataSet dsFactura = null;
            //Validamos que no esixta una Facturación Electrónica
            if (this._id_factura_electronica33 == 0)
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Asignamos valor a la Fecha de Tipo de Cambio
                    fecha_tc = Fecha.ObtieneFechaEstandarMexicoCentro();
                    //TODO :
                    //Recalcula los detalles de la Factura Global
                    // resultado = RecalculaDetallesTipoCambio(FacturadoFacturacion.ObtieneMonedaFacturaGlobal(this._id_factura_global), id_usuario);
                    resultado = Facturacion.FacturadoFacturacion.ValidaMonedaFacturaGlobal(this._id_factura_global);

                    //Validamos Recalculo de Tipo de Cambio
                    if (resultado.OperacionExitosa)
                    {
                        bool impresion_detalle = true;

                        //De acuerdo al Tipo de Importación de la Factura Global
                        //En caso de ser a detalles
                        if (id_tipo_facturacion_electronica == 1)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturaGlobalDetalleFacturaElectronica(this._id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago,
                                                                                                                          "", 1, fecha_tc, id_referencias);
                        }
                        //En caso de ser General (Agrupando por concepto, montos iguales de concepto)
                        else if (id_tipo_facturacion_electronica == 2)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturaGlobalGeneralFacturaElectronica(this._id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago,
                                                                                                                 "", 1, fecha_tc, id_referencias);
                            impresion_detalle = false;
                        }
                        //En caso de ser General (Agrupando por concepto, y tasas de impuestos)
                        else if (id_tipo_facturacion_electronica == 4)
                        {
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturaGlobalGeneralConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago,
                                                                                                                 "", 1, fecha_tc, id_referencias);

                            //Extrayendo Tabla de conceptos
                            DataTable mConceptos = OrigenDatos.RecuperaDataTableDataSet(dsFactura, "Table1");
                            if (mConceptos != null)
                            {
                                //Agrupando elementos de concepto
                                List<KeyValuePair<int, int>> lista = (from DataRow r in mConceptos.Rows select r).GroupBy(concepto => concepto.Field<int>("Id_Tipo_Cargo")).Select(group => new KeyValuePair<int, int>(group.Key, group.Count())).Where(c => c.Value > 1).ToList();

                                //Si no existe duplicidad de líneas de concepto
                                if (lista.Count == 0)
                                    impresion_detalle = false;
                                else
                                    resultado = new RetornoOperacion("Existen Conceptos iguales con tasas de impuesto distintas.");
                            }
                        }
                        else
                        {
                            //Un Solo Concepto
                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago,
                                                                                                                 "", 1, fecha_tc, no_identificacion);

                            //Validamos un Soló Concepto
                            if (Validacion.ValidaOrigenDatos(dsFactura, "Table5"))
                            {
                                resultado = new RetornoOperacion("Revise los conceptos de los Servicios.");
                            }

                            //Validamos Impuestos
                            if (Validacion.ValidaOrigenDatos(dsFactura, "Table4"))
                            {
                                resultado = new RetornoOperacion("Revise los impuestos de los Conceptos");
                            }
                            impresion_detalle = false;

                            //Validamo Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Registramos Diferencia de Viaje en caso de Existir
                                resultado = RegistraDiferenciaViaje(monto_a_facturar, id_usuario);
                            }

                            //Obtenemos los Datos de Factura de acuerdo al esquema de Facturación Electrónica
                            dsFactura = SAT_CL.FacturacionElectronica33.Reporte.ObtienesDatosFacturaGlobalUnConceptoFacturaElectronica(this._id_factura_global, id_forma_pago, id_uso_cfdi, id_metodo_pago,
                                                                                                                 "", 1, fecha_tc, no_identificacion);
                        }

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Validamos Registros
                            if (Validacion.ValidaOrigenDatos(dsFactura))
                            {
                                if (id_tipo_facturacion_electronica == 3)
                                {
                                    //Importamos Factura a Factura Electrónica
                                    resultado = FEv33.Comprobante.ImportaComprobante_V3_3(this._id_compania_cliente, dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], null, null, FEv33.Comprobante.OrigenDatos.FacturaGlobal, null, null, id_compania, id_sucursal, impresion_detalle, fecha_tc, DateTime.MinValue, id_usuario);
                                }
                                else
                                {
                                    //Importamos Factura a Factura Electrónica
                                    resultado = FEv33.Comprobante.ImportaComprobante_V3_3(this._id_compania_cliente, dsFactura.Tables["Table"], dsFactura.Tables["Table1"], null, dsFactura.Tables["Table2"], dsFactura.Tables["Table3"], dsFactura.Tables["Table4"], FEv33.Comprobante.OrigenDatos.FacturaGlobal, dsFactura.Tables["Table5"], dsFactura.Tables["Table6"], id_compania, id_sucursal, impresion_detalle, fecha_tc, DateTime.MinValue, id_usuario);
                                }
                                //Validamos Resultaod de Timbrado
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Comprobante
                                    int idComprobante = resultado.IdRegistro;
                                    
                                    //Configurando Relaciones de Comprobantes Cancelados
                                    resultado = FEv33.ComprobanteRelacion.ConfiguraRelacionComprobanteCancelacionFacturaGlobal(this._id_factura_global, idComprobante, id_usuario);
                                    
                                    //Validamos Rsultado
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Editamo Relación de la Factura y Facturación Electrónica
                                        resultado = EditaSinFERegistradoFacturacionElectronica_V3_3(idComprobante, id_usuario);

                                        //Validamos Rsultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Devolvemos Id Factura
                                            resultado = new RetornoOperacion(this._id_factura_global);

                                            //Completamos Transacción
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                resultado = new RetornoOperacion("No se encontró Información para exportación de la Factura.");
                            }
                        }
                    }
                }
            }
            else
                resultado = new RetornoOperacion("La Factura global ya ha sido registrada.");//*/
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Timbra  Facturación Electrónica
        /// </summary>
        /// <param name="serie">Serie para asignar la Factura</param>
        /// <param name="omitir_addenda">Si se desea Ommitir la Addenda</param>
        /// <param name="ruta_xslt_co">URI del archivo de transformación</param>
        /// <param name="ruta_xslt_co_local">URI del archivo de transformación a utilizar si el primero no puede ser utilizado</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion TimbraFacturaGlobal_V3_3(string serie, bool omitir_addenda, string ruta_xslt_co, string ruta_xslt_co_local, int id_usuario)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Creamos la transacción 
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validamos que existan Relación
                if (this._id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobante
                    using (FEv33.Comprobante objComprobante = new FEv33.Comprobante(this._id_factura_electronica33))
                    {
                        //Validamos existencia de comprobante
                        if (objComprobante.id_comprobante33 > 0)
                        {
                            //Timbramos Fctura
                            resultado = objComprobante.TimbraComprobante("3.3", serie, id_usuario, ruta_xslt_co, ruta_xslt_co_local, omitir_addenda);

                            //Validamos Resultaod de Timbrado
                            if (resultado.OperacionExitosa)
                            {
                                //Editamos Estatus a Timbrado
                                resultado = ActualizaAFacturadoEstatusFacturaGlobalV3_3(id_usuario);

                                //Validamos Rsultado
                                if (resultado.OperacionExitosa)
                                {
                                    //Actualizamos atributos del Comprobante
                                    objComprobante.ActualizaComprobante();
                                    //Devolvemos Id Factura
                                    resultado = new RetornoOperacion(this._id_factura_global, "La factura Global ha sido Timbrado " + objComprobante.serie + objComprobante.folio + " .", true);

                                    //Completamos Transacción
                                    scope.Complete();
                                }
                            }
                        }
                        else
                        {
                            //Establecemos mensaje Resultado
                            resultado = new RetornoOperacion("No se encontró registró Factura Electrónica.");
                        }
                    }
                }
                else
                {
                    //Establecemos mensaje Resultado
                    resultado = new RetornoOperacion("No se encontró registró Factura Electrónica.");
                }
            }

            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Editamos Id Factura Electronica
        /// </summary>
        /// <param name="id_facturacion_e">Id Factura Electronica</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaSinFERegistradoFacturacionElectronica_V3_3(int id_facturacion_e, int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesSinFERegistradosFacturaGlobal(this._id_factura_global))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, ff.id_factura_electronica, id_facturacion_e, (FacturadoFacturacion.Estatus)ff.id_estatus, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, this._id_estatus, 
                                                            this._id_factura_electronica, id_facturacion_e, this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        /// Evento generado para dar Reversa a la Factura Global
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ReversaFacturaGlobal_V3_3(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobalV3_3(this._id_factura_electronica33))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, 0, 0, FacturadoFacturacion.Estatus.Registrada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                {
                                    //Salimos del ciclo
                                    break;
                                }
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Registrada, 0, 0,
                                                           this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }
        /// <summary>
        ///  Actualiza Estatus a Cancelado de la Factura Global
        /// </summary>
        /// <param name="id_usuario">Usuario</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaACanceladoEstatusFacturaGlobal_V3_3(int id_usuario)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            {
                //Creamos la transacción 
                using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Obteniendo Detalles Activos
                    using (DataTable dtDetalles = FacturadoFacturacion.ObtieneDetallesFacturaGlobalV3_3(this._id_factura_electronica33))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetalles))
                        {
                            //Recorriendo Ciclo
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = ff.EditarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, ff.id_factura_electronica, ff.id_factura_electronica33, FacturadoFacturacion.Estatus.Cancelada, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)
                                    
                                    //Salimos del ciclo
                                    break;
                            }
                            //Recorriendo Ciclo pa insercción de nuevos Registros
                            foreach (DataRow dr in dtDetalles.Rows)
                            {
                                //Instanciando Facturado Facturación
                                using (FacturadoFacturacion ff = new FacturadoFacturacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Editamos Factura Electrónica
                                    resultado = FacturadoFacturacion.InsertarFacturadoFacturacion(ff.id_factura, ff.id_factura_concepto, ff.id_factura_global, 0, 0, id_usuario);
                                }
                                //Validamos Resultado
                                if (!resultado.OperacionExitosa)

                                    //Salimos del ciclo
                                    break;
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Actualizamos Factura Electronica
                            resultado = editarFacturaGlobal(this._id_compania_cliente, this._id_compania, this._no_factura_global, (byte)Estatus.Registrada, 0, 0,
                                                           this._descripcion, id_usuario, this._habilitar);
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        scope.Complete();
                    }
                }
            }
            //Devolvemos Objeto Resultado
            return resultado;
        }

        #endregion

        #endregion
    }
}
