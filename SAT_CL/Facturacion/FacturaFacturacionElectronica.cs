using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.Facturacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones de las Facturas Electronicas
    /// </summary>
    public class FacturaFacturacionElectronica : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "facturacion.sp_factura_facturacion_electronica_tff";
        
        private int _id_factura_facturacion_electronica;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Facturación Electronica
        /// </summary>
        public int id_factura_facturacion_electronica { get { return this._id_factura_facturacion_electronica; } }
        private int _id_factura;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Factura
        /// </summary>
        public int id_factura { get { return this._id_factura; } }
        private int _id_factura_concepto;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Concepto de la Factura
        /// </summary>
        public int id_factura_concepto { get { return this._id_factura_concepto; } }
        private int _id_factura_electronica;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Factura Electronica
        /// </summary>
        public int id_factura_electronica { get { return this._id_factura_electronica; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private byte _id_estatus;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus de la Factura
        /// </summary>
        public byte id_estatus { get { return this._id_estatus; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public FacturaFacturacionElectronica()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public FacturaFacturacionElectronica(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~FacturaFacturacionElectronica()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_factura_facturacion_electronica = 0;
            this._id_factura = 0;
            this._id_factura_concepto = 0;
            this._id_factura_electronica = 0;
            this._descripcion = "";
            this._id_estatus = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encaragdo de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de parametros
            object[] param = { 3, id_registro, 0, 0, 0, "", 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada uno de los Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_factura_facturacion_electronica = id_registro;
                        this._id_factura = Convert.ToInt32(dr["IdFactura"]);
                        this._id_factura_concepto = Convert.ToInt32(dr["IdFacturaConcepto"]);
                        this._id_factura_electronica = Convert.ToInt32(dr["IdFacturaElectronica"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._id_estatus = Convert.ToByte(dr["IdEstatus"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado de la Operación
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="id_factura_concepto">Id del Concepto de la Factura</param>
        /// <param name="id_factura_electronica">id de la Factura Electronica</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_factura, int id_factura_concepto, int id_factura_electronica, 
                                                    string descripcion, byte id_estatus, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de parametros
            object[] param = { 2, this._id_factura_facturacion_electronica, id_factura, id_factura_concepto, 
                               id_factura_electronica, descripcion, id_estatus, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar las Facturas Electronicas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="id_factura_concepto">Id del Concepto de la Factura</param>
        /// <param name="id_factura_electronica">id de la Factura Electronica</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaFacturaFacturacionElectronica(int id_factura, int id_factura_concepto, int id_factura_electronica,
                                                    string descripcion, byte id_estatus, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando arreglo de parametros
            object[] param = { 1, 0, id_factura, id_factura_concepto, id_factura_electronica, 
                               descripcion, id_estatus, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar las Facturas Electronicas
        /// </summary>
        /// <param name="id_factura">Id de Factura</param>
        /// <param name="id_factura_concepto">Id del Concepto de la Factura</param>
        /// <param name="id_factura_electronica">id de la Factura Electronica</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="id_estatus">Estatus</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaFacturaFacturacionElectronica(int id_factura, int id_factura_concepto, int id_factura_electronica,
                                                    string descripcion, byte id_estatus, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_factura, id_factura_concepto, id_factura_electronica,
                               descripcion, id_estatus, id_usuario, this._habilitar);
        }
        /// <summary>
        ///  Método Público encargado de Deshabilitar las Facturas Electronicas
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarFacturaFacturacionElectronica(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_factura,this._id_factura_concepto, this._id_factura_electronica,
                               this._descripcion, this._id_estatus, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de las Facturas Electronicas
        /// </summary>
        /// <returns></returns>
        public bool ActualizaFacturaFacturacionElectronica()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_factura_facturacion_electronica);
        }

        #endregion
    }
}
