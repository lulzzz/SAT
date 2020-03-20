using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Facturacion
{
    public class FacturacionOtros : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_facturacion_otros_tfo";

        private int _id_facturacion_otros;
        /// <summary>
        /// Atributo encargado de Almacenar la Relación entre la Factura sin Servicio
        /// </summary>
        public int id_facturacion_otros { get { return this._id_facturacion_otros; } }
        private int _id_facturado;
        /// <summary>
        /// Atributo encargado de Almacenar la Factura
        /// </summary>
        public int id_facturado { get { return this._id_facturado; } }
        private int _id_compania_emisora;
        /// <summary>
        /// Atributo encargado de Almacenar la Compania Emisora
        /// </summary>
        public int id_compania_emisora { get { return this._id_compania_emisora; } }
        private int _id_cliente_receptor;
        /// <summary>
        /// Atributo encargado de Almacenar el Cliente Receptor
        /// </summary>
        public int id_cliente_receptor { get { return this._id_cliente_receptor; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public FacturacionOtros()
        {
            //Asignando Valores
            this._id_facturacion_otros = 0;
            this._id_facturado = 0;
            this._id_compania_emisora = 0;
            this._id_cliente_receptor = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro dada una Factura, Compania y Cliente
        /// </summary>
        /// <param name="id_facturacion_otros">Id Facturación Otros</param>
        public FacturacionOtros(int id_facturacion_otros)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_facturacion_otros);
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dada una Factura, Compania y Cliente
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <param name="id_compania">Compania</param>
        /// <param name="id_cliente">Cliente</param>
        public FacturacionOtros(int id_factura, int id_compania, int id_cliente)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_factura, id_compania, id_cliente);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturacionOtros()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_facturacion_otros">Facturación Otros</param>
        private bool cargaAtributosInstancia(int id_facturacion_otros)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_facturacion_otros, 0, 0, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_facturacion_otros = id_facturacion_otros;
                        this._id_facturado = Convert.ToInt32(dr["IdFacturado"]);
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
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
        /// Método encargado de Cargar los Atributos dada una Factura, Compania y Cliente
        /// </summary>
        /// <param name="id_factura"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_cliente"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_factura, int id_compania, int id_cliente)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_factura, id_compania, id_cliente, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_facturacion_otros = Convert.ToInt32(dr["Id"]);
                        this._id_facturado = Convert.ToInt32(dr["IdFacturado"]);
                        this._id_compania_emisora = Convert.ToInt32(dr["IdCompaniaEmisora"]);
                        this._id_cliente_receptor = Convert.ToInt32(dr["IdClienteReceptor"]);
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
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_facturado">Facturado (Factura del Cliente sin Servicio)</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_facturado, int id_compania_emisora, int id_cliente_receptor, 
                                                     int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_facturacion_otros, id_facturado, id_compania_emisora, id_cliente_receptor, id_usuario, habilitar, "", "" };
            
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar la Relación de las Facturas de Otros (No de Servicio)
        /// </summary>
        /// <param name="id_facturado">Facturado (Factura del Cliente sin Servicio)</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturacionOtros(int id_facturado, int id_compania_emisora, int id_cliente_receptor,
                                                               int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_facturado, id_compania_emisora, id_cliente_receptor, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar la Relación de las Facturas de Otros (Sin relación a un Servicio)
        /// </summary>
        /// <param name="id_facturado">Facturado (Factura del Cliente sin Servicio)</param>
        /// <param name="id_compania_emisora">Compania Emisora</param>
        /// <param name="id_cliente_receptor">Cliente Receptor</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturacionOtros(int id_facturado, int id_compania_emisora, int id_cliente_receptor,
                                                      int id_usuario)
        {
            //Devolviendo resultado Obtenido
            return this.actualizaRegistroBD(id_facturado, id_compania_emisora, id_cliente_receptor, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar la Relación de las Facturas de Otros (Sin relación a un Servicio)
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaFacturacionOtros(int id_usuario)
        {
            //Invocando Método de Actualización
            return this.actualizaRegistroBD(this._id_facturado, this._id_compania_emisora, this._id_cliente_receptor, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de la Relación de la Factura
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturacionOtros()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_facturacion_otros);
        }
        /// <summary>
        /// Método encargado de Obtener una Instancia dada el Id de Facturado
        /// </summary>
        /// <param name="id_facturado">Facturado</param>
        /// <returns></returns>
        public static FacturacionOtros ObtieneInstanciaFactura(int id_facturado)
        {
            //Declarando Objeto de Retorno
            FacturacionOtros fo = new FacturacionOtros();
            
            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_facturado, 0, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Instanciando Registro
                        fo = new FacturacionOtros(Convert.ToInt32(dr["Id"]));
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return fo;
        }

        #endregion
    }
}
