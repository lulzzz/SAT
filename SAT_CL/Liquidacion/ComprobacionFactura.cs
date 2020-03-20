using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Liquidacion
{
    /// <summary>
    /// Clase encargada de todas las Operaciones relacionadas con las Comprobaciones ligadas a Facturas
    /// </summary>
    public class ComprobacionFactura : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de alamcenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "liquidacion.sp_comprobacion_factura_tcf";

        private int _id_comprobacion_factura;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Comprobación de la Factura
        /// </summary>
        public int id_comprobacion_factura { get { return this._id_comprobacion_factura; } }
        private int _id_comprobacion;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Comprobación
        /// </summary>
        public int id_comprobacion { get { return this._id_comprobacion; } }
        private int _id_factura;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente a la Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de guardar el Atributo referente al Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos de la Clase por Defecto
        /// </summary>
        public ComprobacionFactura()
        {
            //Asignando Valores
            this._id_comprobacion_factura = 0;
            this._id_comprobacion = 0;
            this._id_factura = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos de la Clase dado un Registro
        /// </summary>
        /// <param name="id_comprobacion_factura">Comprobación de la Factura</param>
        public ComprobacionFactura(int id_comprobacion_factura)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_comprobacion_factura);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ComprobacionFactura()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_comprobacion_factura">Id de Comprobación de la Factura</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_comprobacion_factura)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_comprobacion_factura, 0, 0, 0, false, "", "" };

            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_comprobacion_factura = Convert.ToInt32(dr["Id"]);
                        this._id_comprobacion = Convert.ToInt32(dr["IdComprobacion"]);
                        this._id_factura = Convert.ToInt32(dr["IdFactura"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar el Registro
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        /// <param name="id_factura">Referencia de la Factura</param>
        /// <param name="id_usuario">usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_comprobacion, int id_factura, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_comprobacion_factura, id_comprobacion, id_factura, id_usuario, habilitar, "", "" };

            //Ejecutando resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        
        #endregion

        #region Métodos Públicos

        /// <summary>
        ///  Método Público encargado de Insertar la Relación entre los Comprobantes y las Facturas
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        /// <param name="id_factura">Referencia de la Factura</param>
        /// <param name="id_usuario">usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaComprobacionFactura(int id_comprobacion, int id_factura, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_comprobacion, id_factura, id_usuario, true, "", "" };

            //Ejecutando resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar la Relación entre los Comprobantes y las Facturas
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        /// <param name="id_factura">Referencia de la Factura</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaComprobacionFactura(int id_comprobacion, int id_factura, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(id_comprobacion, id_factura, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Editar la Relación entre los Comprobantes y las Facturas
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaComprobacionFactura(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(this._id_comprobacion, this._id_factura, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaComprobacionFactura()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_comprobacion_factura);
        }
        /// <summary>
        /// Método Público encargado de Obtener las Facturas Ligadas a una Comprobación dada.
        /// </summary>
        /// <param name="id_comprobacion">Comprobación</param>
        /// <returns></returns>
        public static DataTable ObtieneFacturasComprobacion(int id_comprobacion)
        {
            //Declarando Objeto de Retorno
            DataTable dtFacturas = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_comprobacion, 0, 0, false, "", "" };

            //Instanciando Reporte de Facturas
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                
                    //Asignando Tabla
                    dtFacturas = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtFacturas;
        }
        /// <summary>
        /// Método Público encargado de Obtener el Total de las Facturas Ligadas a una Comprobación dada.
        /// </summary>
        /// <param name="id_comprobacion">Comprobación</param>
        /// <returns></returns>
        public static decimal ObtieneTotalFacturasComprobacion(int id_comprobacion)
        {
            //Declarando Objeto de Retorno
            decimal totalFacturas = 0.00M;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, id_comprobacion, 0, 0, false, "", "" };

            //Instanciando Reporte de Facturas
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Tabla
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                        
                        //Asignando Resultado Obtenido
                        totalFacturas = Convert.ToDecimal(dr["TotalFacturas"]);
                    
                }
            }

            //Devolviendo Resultado Obtenido
            return totalFacturas;
        }


        #endregion
    }
}
