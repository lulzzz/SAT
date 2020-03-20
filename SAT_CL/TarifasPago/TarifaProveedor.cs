using System;
using TSDK.Datos;
using TSDK.Base;
using System.Data;

namespace SAT_CL.TarifasPago
{
    public class TarifaProveedor : Disposable
    {
        #region Atributos

        /// <summary>
        /// 
        /// </summary>
        private static string _nom_sp = "tarifas_pago.sp_tarifa_proveedor_tp";

        /// <summary>
        /// 
        /// </summary>
        public int id_tarifa_proveedor { get { return this._id_tarifa_proveedor; } }
        private int _id_tarifa_proveedor;

        /// <summary>
        /// 
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private int _id_compania;

        /// <summary>
        /// 
        /// </summary>
        public int id_proveedor { get { return this._id_proveedor; } }
        private int _id_proveedor;

        /// <summary>
        /// 
        /// </summary>
        public int id_servicio_maestro { get { return this._id_servicio_maestro; } }
        private int _id_servicio_maestro;

        /// <summary>
        /// 
        /// </summary>
        public decimal monto_cc { get { return this._monto_cc; } }
        private decimal _monto_cc;

        /// <summary>
        /// 
        /// </summary>
        public decimal monto_sc { get { return this._monto_sc; } }
        private decimal _monto_sc;

        /// <summary>
        /// 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private bool _habilitar;

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public TarifaProveedor()
        {
            this._id_tarifa_proveedor =
            this._id_compania =
            this._id_proveedor =
            this._id_servicio_maestro = 0;
            this._monto_cc =
            this._monto_sc = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_tarifa_proveedor"></param>
        public TarifaProveedor(int id_tarifa_proveedor)
        {
            cargaAtributosInstancia(id_tarifa_proveedor);
        }

        #endregion

        #region Destructores


        ~TarifaProveedor()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_tarifa_proveedor"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tarifa_proveedor)
        {
            bool retorno = false;
            object[] param = { 3, id_tarifa_proveedor, 0, 0, 0, 0.00M, 0.00M, 0, false, "", "" };
            //Obteniendo Resultado de BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        this._id_tarifa_proveedor = id_tarifa_proveedor;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._id_proveedor = Convert.ToInt32(dr["IdProveedor"]);
                        this._id_servicio_maestro = Convert.ToInt32(dr["IdServicioMaestro"]);
                        this._monto_cc = Convert.ToDecimal(dr["MontoCC"]);
                        this._monto_sc = Convert.ToDecimal(dr["MontoSC"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    retorno = true;
                }
            }
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_compania, int id_proveedor, int id_servicio_maestro, decimal monto_cc, decimal monto_sc, int id_usuario, bool habilitar)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 2, this._id_tarifa_proveedor, id_compania, id_proveedor, id_servicio_maestro, monto_cc, monto_sc, id_usuario, habilitar, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifaProveedor(int id_compania, int id_proveedor, int id_servicio_maestro, decimal monto_cc, decimal monto_sc, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 1, 0, id_compania, id_proveedor, id_servicio_maestro, monto_cc, monto_sc, id_usuario, true, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_proveedor"></param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifaProveedor(int id_compania, int id_proveedor, int id_servicio_maestro, decimal monto_cc, decimal monto_sc, int id_usuario)
        {
            return this.actualizaRegistrosBD(id_compania, id_proveedor, id_servicio_maestro, monto_cc, monto_sc, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifaProveedor(int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_compania, this._id_proveedor, this._id_servicio_maestro, this._monto_cc, this._monto_sc, id_usuario, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaMontosTarifa(decimal monto_cc, decimal monto_sc, int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_compania, this._id_proveedor, this._id_servicio_maestro, monto_cc, monto_sc, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTarifaProveedor()
        {
            return this.cargaAtributosInstancia(this._id_tarifa_proveedor);
        }

        #endregion
    }
}
