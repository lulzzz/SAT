using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Ruta
{
    public class CrucesAutorizadosIave : Disposable
    {
        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "[ruta].[sp_cruces_autorizados_iave_tcai]";

        private int _id_cruces_autorizados_iave;
        /// <summary>
        /// 
        /// </summary>
        public int id_cruces_autorizados_iave { get { return this._id_cruces_autorizados_iave; } }

        private int _id_caseta;
        /// <summary>
        /// 
        /// </summary>
        public int id_caseta { get { return this._id_caseta; } }

        private string _nombre_caseta;
        /// <summary>
        /// 
        /// </summary>
        public string nombre_caseta { get { return this._nombre_caseta; } }

        private string _no_tarjeta;
        /// <summary>
        /// 
        /// </summary>
        public string no_tarjeta { get { return this._no_tarjeta; } }

        private int _id_servicio;
        /// <summary>
        /// 
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }

        private int _id_unidad;
        /// <summary>
        /// 
        /// </summary>
        public int id_unidad { get { return this._id_unidad; } }

        private int _id_segmento;
        /// <summary>
        /// 
        /// </summary>
        public int id_segmento { get { return this._id_segmento; } }

        private int _id_factura;
        /// <summary>
        /// 
        /// </summary>
        public int id_factura { get { return this._id_factura; } }

        private DateTime _fecha_carga;
        /// <summary>
        /// 
        /// </summary>
        public DateTime fecha_carga { get { return this._fecha_carga; } }

        private DateTime _fecha_descarga;
        /// <summary>
        /// 
        /// </summary>
        public DateTime fecha_descarga { get { return this._fecha_descarga; } }

        private decimal _monto;
        /// <summary>
        /// 
        /// </summary>
        public decimal monto { get { return this._monto; } }

        private int _id_estatus;
        /// <summary>
        /// 
        /// </summary>
        public int id_estatus { get { return this._id_estatus; } }

        private int _id_usuario;
        /// <summary>
        /// 
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que se encarga de Inicializar los Atributos por Defecto
        /// </summary>
        public CrucesAutorizadosIave()
        {
            this._id_cruces_autorizados_iave = 0;
            this._id_caseta = 0;
            this._nombre_caseta = "";
            this._no_tarjeta = "";
            this._id_servicio = 0;
            this._id_unidad = 0;
            this._id_segmento = 0;
            this._id_factura = 0;
            this._fecha_carga = DateTime.MinValue;
            this._fecha_descarga = DateTime.MinValue;
            this._monto = 0;
            this._id_estatus = 0;
            this._id_usuario = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cruces_autorizados_iave">Id de Cruces Autorizados IAVE</param>
        public CrucesAutorizadosIave(int id_cruces_autorizados_iave)
        {
            cargaAtributosInstancia(id_cruces_autorizados_iave);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera los recursos utilizados por la instancia
        /// </summary>
        ~CrucesAutorizadosIave()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_cruces_autorizados_iave"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_cruces_autorizados_iave)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_cruces_autorizados_iave, 0, "", "", 0, 0, 0, 0, null, null, 0, 0, 0, false, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        this._id_cruces_autorizados_iave = Convert.ToInt32(r["Id"]);
                        this._id_caseta = Convert.ToInt32(r["IdCaseta"]);
                        this._nombre_caseta = r["NombreCaseta"].ToString();
                        this._no_tarjeta = r["NoTarjetaIAVE"].ToString();
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._id_segmento = Convert.ToInt32(r["IdSegmento"]);
                        this._id_factura = Convert.ToInt32(r["IdFactura"]);
                        DateTime.TryParse(r["FechaCarga"].ToString(), out this._fecha_carga);
                        DateTime.TryParse(r["FechaDescarga"].ToString(), out this._fecha_descarga);
                        this._monto = Convert.ToDecimal(r["Monto"]);
                        this._id_estatus = Convert.ToInt32(r["IdEstatus"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Asignando Variables Positiva
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private RetornoOperacion editaCrucesAutorizadosIAVE(int id_caseta, string nombre_caseta, string no_tarjeta, int id_servicio, int id_unidad, int id_segmento, int id_factura, DateTime fecha_carga, DateTime fecha_descarga, decimal monto, int id_estatus, int id_usuario, bool habilitar)
        {
            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_cruces_autorizados_iave, id_caseta, nombre_caseta, no_tarjeta, id_servicio, id_unidad, id_segmento, id_factura, fecha_carga, fecha_descarga, monto, id_estatus, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            //Devolviendo Resultado Obtenido
            return result;

        }

        /// <summary>
        /// Reguistra un nuevo usuario en la base de datos
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private RetornoOperacion asignaFacturaIave(int id_factura, int id_estatus, int id_usuario)
        {

            //Declarando Obejto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_cruces_autorizados_iave, this._id_caseta, this._nombre_caseta, this._no_tarjeta, this._id_servicio, this._id_unidad, this._id_segmento, id_factura, this._fecha_carga, this._fecha_descarga, this._monto, id_estatus, id_usuario, this._habilitar, "", "" };

            //Creando nuevo usuario en BD
          result =  CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
            //Devolviendo Resultado Obtenido
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCrucesAutorizadosIAVE(int id_caseta, string nombre_caseta, string no_tarjeta, int id_servicio, int id_unidad, int id_segmento, int id_factura, DateTime fecha_carga, DateTime fecha_descarga, decimal monto, int id_estatus, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, id_caseta, nombre_caseta, no_tarjeta, id_servicio, id_unidad, id_segmento, id_factura, fecha_carga, fecha_descarga, monto, id_estatus, id_usuario, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="">  </param>
        /// <returns></returns>
        public RetornoOperacion EditarCrucesAutorizadosIAVE(int id_caseta, string nombre_caseta, string no_tarjeta, int id_servicio, int id_unidad, int id_segmento, int id_factura, DateTime fecha_carga, DateTime fecha_descarga, decimal monto, int id_estatus, int id_usuario)
        {
            return this.editaCrucesAutorizadosIAVE(id_caseta, nombre_caseta, no_tarjeta, id_servicio, id_unidad, id_segmento, id_factura, fecha_carga, fecha_descarga, monto, id_estatus, id_usuario, this.habilitar);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="">  </param>
        /// <returns></returns>
        public RetornoOperacion AsignaFacturaIave(int id_factura, int id_estatus, int id_usuario)
        {
            return this.asignaFacturaIave(id_factura,id_estatus, id_usuario);

        }
        /// <summary>
        /// Deshabilita el registro operador
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaCrucesAutorizadosIAVE(int id_usuario)
        {

            //Realizando actualización
            return this.editaCrucesAutorizadosIAVE(this._id_caseta, this._nombre_caseta, this._no_tarjeta, this._id_servicio, this._id_unidad, this._id_segmento, this._id_factura, this._fecha_carga, this._fecha_descarga, this._monto,  this._id_estatus, id_usuario, false);

        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_cruces_autorizados_iave);
        }

        /// <summary>
        /// Método Público encargado de Obtener los Vales Ligados a una Factura
        /// </summary>
        /// <param name="id_factura">Factura</param>
        /// <returns></returns>
        public static DataTable ObtieneIAVEPorFactura(int id_factura)
        {   //Declarando Objeto de Retorno
            DataTable dtIAVE = null;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, 0, "", "", 0, 0, 0, id_factura, null, null, 0, 0, 0, false, "", "" };
            //Obteniendo Reporte
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {   //Validando que existan registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Reporte Obtenido
                    dtIAVE = ds.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dtIAVE;
        }
        #endregion

    }
}
