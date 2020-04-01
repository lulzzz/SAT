using System;
using TSDK.Datos;
using TSDK.Base;
using System.Data;
using System.Collections.Generic;

namespace SAT_CL.EgresoServicio
{
    public class ServicioImportacionAnticipos : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP a invocar
        /// </summary>
        private static string _nom_sp = "egresos_servicio.sp_servicio_importacion_anticipos_tsia";

        /// <summary>
        /// 
        /// </summary>
        public int id_servicio_importacion_anticipos { get { return this._id_servicio_importacion_anticipos; } }
        private int _id_servicio_importacion_anticipos;

        /// <summary>
        /// 
        /// </summary>
        public int id_servicio_importacion_detalle { get { return this._id_servicio_importacion_detalle; } }
        private int _id_servicio_importacion_detalle;

        /// <summary>
        /// 
        /// </summary>
        public int id_anticipo_previo { get { return this._id_anticipo_previo; } }
        private int _id_anticipo_previo;

        /// <summary>
        /// 
        /// </summary>
        public byte id_tipo_scc { get { return this._id_tipo_scc; } }
        private byte _id_tipo_scc;

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
        public int id_anticipo_finiquito_cc { get { return this._id_anticipo_finiquito_cc; } }
        private int _id_anticipo_finiquito_cc;

        /// <summary>
        /// 
        /// </summary>
        public int id_anticipo_finiquito_sc { get { return this._id_anticipo_finiquito_sc; } }
        private int _id_anticipo_finiquito_sc;

        /// <summary>
        /// 
        /// </summary>
        public int id_factura_cxp { get { return this._id_factura_cxp; } }
        private int _id_factura_cxp;

        /// <summary>
        /// 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        private bool _habilitar;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public ServicioImportacionAnticipos()
        {
            this._id_servicio_importacion_anticipos =
            this._id_servicio_importacion_detalle =
            this._id_anticipo_previo = 0;
            this._id_tipo_scc = 0;
            this._monto_cc =
            this._monto_sc = 0.00M;
            this._id_anticipo_finiquito_cc = 0;
            this._id_anticipo_finiquito_sc = 0;
            this._id_factura_cxp = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor dado un Registro
        /// </summary>
        /// <param name="id_servicio_importacion_anticipos"></param>
        public ServicioImportacionAnticipos(int id_servicio_importacion_anticipos)
        {
            cargaAtributosInstancia(id_servicio_importacion_anticipos);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioImportacionAnticipos()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion_anticipos"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_servicio_importacion_anticipos)
        {
            bool retorno = false;
            object[] param = { 3, id_servicio_importacion_anticipos, 0, 0, 0, 0.00M, 0.00M, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado de BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Inicializando Valores
                        this._id_servicio_importacion_anticipos = id_servicio_importacion_anticipos;
                        this._id_servicio_importacion_detalle = Convert.ToInt32(dr["IdServicioImportacionDetalle"]);
                        this._id_anticipo_previo = Convert.ToInt32(dr["IdAnticipoPrevio"]);
                        this._id_tipo_scc = Convert.ToByte(dr["IdTipoSCC"]);
                        this._monto_cc = Convert.ToDecimal(dr["MontoCC"]);
                        this._monto_sc = Convert.ToDecimal(dr["MontoSC"]);
                        this._id_anticipo_finiquito_cc = Convert.ToInt32(dr["IdAnticipoFiniquitoCC"]);
                        this._id_anticipo_finiquito_sc = Convert.ToInt32(dr["IdAnticipoFiniquitoSC"]);
                        this._id_factura_cxp = Convert.ToInt32(dr["IdFacturaCXP"]);
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
        /// <param name="id_servicio_importacion_detalle"></param>
        /// <param name="id_anticipo_previo"></param>
        /// <param name="id_tipo_scc"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_anticipo_finiquito_cc"></param>
        /// <param name="id_anticipo_finiquito_sc"></param>
        /// <param name="id_factura_cxp"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_servicio_importacion_detalle, int id_anticipo_previo, byte id_tipo_scc,
                                        decimal monto_cc, decimal monto_sc, int id_anticipo_finiquito_cc, int id_anticipo_finiquito_sc,
                                        int id_factura_cxp, int id_usuario, bool habilitar)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 2, this._id_servicio_importacion_anticipos, id_servicio_importacion_detalle, id_anticipo_previo,
                               id_tipo_scc, monto_cc, monto_sc, id_anticipo_finiquito_cc, id_anticipo_finiquito_sc, id_factura_cxp, 
                               id_usuario, habilitar, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion_detalle"></param>
        /// <param name="id_anticipo_previo"></param>
        /// <param name="id_tipo_scc"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_anticipo_finiquito_cc"></param>
        /// <param name="id_anticipo_finiquito_sc"></param>
        /// <param name="id_factura_cxp"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioImportacionAnticipos(int id_servicio_importacion_detalle, int id_anticipo_previo,
                                                                        byte id_tipo_scc, decimal monto_cc, decimal monto_sc,
                                                                        int id_anticipo_finiquito_cc, int id_anticipo_finiquito_sc,
                                                                        int id_factura_cxp, int id_usuario)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            object[] param = { 1, 0, id_servicio_importacion_detalle, id_anticipo_previo, id_tipo_scc, monto_cc, monto_sc,
                               id_anticipo_finiquito_cc, id_anticipo_finiquito_sc, id_factura_cxp, id_usuario, true, "", "" };
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion_detalle"></param>
        /// <param name="id_anticipo_previo"></param>
        /// <param name="id_tipo_scc"></param>
        /// <param name="monto_cc"></param>
        /// <param name="monto_sc"></param>
        /// <param name="id_anticipo_finiquito_cc"></param>
        /// <param name="id_anticipo_finiquito_sc"></param>
        /// <param name="id_factura_cxp"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioImportacionAnticipos(int id_servicio_importacion_detalle, int id_anticipo_previo,
                                                            byte id_tipo_scc, decimal monto_cc, decimal monto_sc,
                                                            int id_anticipo_finiquito_cc, int id_anticipo_finiquito_sc,
                                                            int id_factura_cxp, int id_usuario)
        {
            return this.actualizaRegistrosBD(id_servicio_importacion_detalle, id_anticipo_previo, id_tipo_scc, monto_cc, monto_sc,
                               id_anticipo_finiquito_cc, id_anticipo_finiquito_sc, id_factura_cxp, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioImportacionAnticipos(int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_servicio_importacion_detalle, this._id_anticipo_previo, this._id_tipo_scc, this._monto_cc, this._monto_sc,
                               this._id_anticipo_finiquito_cc, this._id_anticipo_finiquito_sc, this._id_factura_cxp, id_usuario, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_anticipo_finiquito_cc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaFiniquitoServicioImportacionAnticiposCC(int id_anticipo_finiquito_cc, int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_servicio_importacion_detalle, this._id_anticipo_previo, this._id_tipo_scc, this._monto_cc, this._monto_sc,
                               id_anticipo_finiquito_cc, this._id_anticipo_finiquito_sc, this._id_factura_cxp, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_anticipo_finiquito_sc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaFiniquitoServicioImportacionAnticiposSC(int id_anticipo_finiquito_sc, int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_servicio_importacion_detalle, this._id_anticipo_previo, this._id_tipo_scc, this._monto_cc, this._monto_sc,
                               this._id_anticipo_finiquito_cc, id_anticipo_finiquito_sc, this._id_factura_cxp, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_anticipo_previo"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaDepositoPrevioImportacionAnticipos(int id_anticipo_previo, int id_usuario)
        {
            return this.actualizaRegistrosBD(this._id_servicio_importacion_detalle, id_anticipo_previo, this._id_tipo_scc, this._monto_cc, this._monto_sc,
                               this._id_anticipo_finiquito_cc, this._id_anticipo_finiquito_sc, this._id_factura_cxp, id_usuario, this._habilitar);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioImportacionAnticipos()
        {
            return this.cargaAtributosInstancia(this._id_servicio_importacion_anticipos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_transportista"></param>
        /// <param name="id_servicio_maestro"></param>
        /// <param name="fecha_operacion"></param>
        /// <param name="depositos"></param>
        /// <returns></returns>
        public static DataTable ObtieneAnticiposImportaciones(int id_compania, int id_transportista, int id_servicio_maestro, DateTime fecha_operacion, string depositos)
        {
            DataTable dtDetallesImportaciones = new DataTable();
            object[] param = { 4, id_compania, id_transportista, id_servicio_maestro, 0, 0.00M, 0.00M,
                               0, 0, 0, 0, false, fecha_operacion.ToString("yyyyMMdd"), depositos };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    dtDetallesImportaciones = ds.Tables["Table"];
            }
            return dtDetallesImportaciones;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_anticipo_previo"></param>
        /// <returns></returns>
        public static List<ServicioImportacionAnticipos> ObtieneImportacionesAnticiposPrevios(int id_anticipo_previo)
        {
            List<ServicioImportacionAnticipos> anticipos = new List<ServicioImportacionAnticipos>();
            object[] param = { 5, 0, 0, id_anticipo_previo, 0, 0.00M, 0.00M, 0, 0, 0, 0, true, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        anticipos.Add(new ServicioImportacionAnticipos
                        {
                            //Inicializando Valores
                            _id_servicio_importacion_anticipos = Convert.ToInt32(dr["Id"]),
                            _id_servicio_importacion_detalle = Convert.ToInt32(dr["IdServicioImportacionDetalle"]),
                            _id_anticipo_previo = Convert.ToInt32(dr["IdAnticipoPrevio"]),
                            _id_tipo_scc = Convert.ToByte(dr["IdTipoSCC"]),
                            _monto_cc = Convert.ToDecimal(dr["MontoCC"]),
                            _monto_sc = Convert.ToDecimal(dr["MontoSC"]),
                            _id_anticipo_finiquito_cc = Convert.ToInt32(dr["IdAnticipoFiniquitoCC"]),
                            _id_anticipo_finiquito_sc = Convert.ToInt32(dr["IdAnticipoFiniquitoSC"]),
                            _id_factura_cxp = Convert.ToInt32(dr["IdFacturaCXP"]),
                            _habilitar = Convert.ToBoolean(dr["Habilitar"])
                        });
                    }
                }
            }
            return anticipos;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_anticipo_finiquito"></param>
        /// <returns></returns>
        public static List<ServicioImportacionAnticipos> ObtieneImportacionesFiniquito(int id_anticipo_finiquito)
        {
            List<ServicioImportacionAnticipos> anticipos = new List<ServicioImportacionAnticipos>();
            object[] param = { 6, 0, 0, 0, 0, 0.00M, 0.00M, id_anticipo_finiquito, 0, 0, 0, true, "", "" };
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        anticipos.Add(new ServicioImportacionAnticipos
                        {
                            //Inicializando Valores
                            _id_servicio_importacion_anticipos = Convert.ToInt32(dr["Id"]),
                            _id_servicio_importacion_detalle = Convert.ToInt32(dr["IdServicioImportacionDetalle"]),
                            _id_anticipo_previo = Convert.ToInt32(dr["IdAnticipoPrevio"]),
                            _id_tipo_scc = Convert.ToByte(dr["IdTipoSCC"]),
                            _monto_cc = Convert.ToDecimal(dr["MontoCC"]),
                            _monto_sc = Convert.ToDecimal(dr["MontoSC"]),
                            _id_anticipo_finiquito_cc = Convert.ToInt32(dr["IdAnticipoFiniquitoCC"]),
                            _id_anticipo_finiquito_sc = Convert.ToInt32(dr["IdAnticipoFiniquitoSC"]),
                            _id_factura_cxp = Convert.ToInt32(dr["IdFacturaCXP"]),
                            _habilitar = Convert.ToBoolean(dr["Habilitar"])
                        });
                    }
                }
            }
            return anticipos;
        }

        #endregion
    }
}
