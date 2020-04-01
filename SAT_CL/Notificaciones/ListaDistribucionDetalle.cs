using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Notificaciones
{
    public class ListaDistribucionDetalle : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "notificaciones.sp_lista_distribucion_detalle_tldd";

        private int _id_lista_distribucion_detalle;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion_detalle { get { return this._id_lista_distribucion_detalle; } }
        private int _id_lista_distribucion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion { get { return this._id_lista_distribucion; } }
        private int _id_medio_distribucion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_medio_distribucion { get { return this._id_medio_distribucion; } }
        private bool _bit_reenvio;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool bit_reenvio { get { return this._bit_reenvio; } }
        private int _periodicidad_envio;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int periodicidad_envio { get { return this._periodicidad_envio; } }
        private int _unidad_conversion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int unidad_conversion { get { return this._unidad_conversion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public ListaDistribucionDetalle()
        {
            //Asignando Atributos
            this._id_lista_distribucion_detalle = 0;
            this._id_lista_distribucion = 0;
            this._id_medio_distribucion = 0;
            this._bit_reenvio = false;
            this._periodicidad_envio = 0;
            this._unidad_conversion = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_lista_distribucion_Detalle"></param>
        public ListaDistribucionDetalle(int id_lista_distribucion_Detalle)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_lista_distribucion_Detalle);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ListaDistribucionDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos del Registro
        /// </summary>
        /// <param name="id_lista_distribucion_detalle"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_lista_distribucion_detalle)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_lista_distribucion_detalle, 0, 0, false, 0, 0, 0, false, "", "" };

            //Obteniendo Datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Datos
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_lista_distribucion_detalle = id_lista_distribucion_detalle;
                        this._id_lista_distribucion = Convert.ToInt32(dr["IdLista"]);
                        this._id_medio_distribucion = Convert.ToInt32(dr["IdMedio"]);
                        this._bit_reenvio = Convert.ToBoolean(dr["BitReenvio"]);
                        this._periodicidad_envio = Convert.ToInt32(dr["Periodicidad"]);
                        this._unidad_conversion = Convert.ToInt32(dr["IdUnidadConversion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado
            return retorno;
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos del Registro
        /// </summary>
        /// <param name="id_lista_distribucion"></param>
        /// <param name="id_medio_distribucion"></param>
        /// <param name="bit_reenvio"></param>
        /// <param name="periodicidad"></param>
        /// <param name="id_unidad_conversion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_lista_distribucion, int id_medio_distribucion, bool bit_reenvio,
                                                int periodicidad, int id_unidad_conversion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_lista_distribucion_detalle, id_lista_distribucion, id_medio_distribucion, bit_reenvio, periodicidad, id_unidad_conversion, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar 
        /// </summary>
        /// <param name="id_lista_distribucion"></param>
        /// <param name="id_medio_distribucion"></param>
        /// <param name="bit_reenvio"></param>
        /// <param name="periodicidad"></param>
        /// <param name="id_unidad_conversion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaListaDistribucionDetalle(int id_lista_distribucion, int id_medio_distribucion, bool bit_reenvio,
                                                int periodicidad, int id_unidad_conversion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_lista_distribucion, id_medio_distribucion, bit_reenvio, periodicidad, id_unidad_conversion, id_usuario, true, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar
        /// </summary>
        /// <param name="id_lista_distribucion"></param>
        /// <param name="id_medio_distribucion"></param>
        /// <param name="bit_reenvio"></param>
        /// <param name="periodicidad"></param>
        /// <param name="id_unidad_conversion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaListaDistribucionDetalle(int id_lista_distribucion, int id_medio_distribucion, bool bit_reenvio,
                                                int periodicidad, int id_unidad_conversion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(id_lista_distribucion, id_medio_distribucion, bit_reenvio,
                               periodicidad, id_unidad_conversion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaListaDistribucionDetalle(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(this._id_lista_distribucion, this._id_medio_distribucion, this._bit_reenvio,
                               this._periodicidad_envio, this._unidad_conversion, id_usuario, false);
        }
        /// <summary>
		/// Método encargado de Obtener los Detalles 
		/// </summary>
		/// <param name="id_requisicion">Requisición</param>
		/// <returns></returns>
		public static DataTable ObtieneDetallesListaDistribucion(int id_lista_distribucion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;
            //Inicializando parámetros de inserción
            object[] parametros = { 4, 0, id_lista_distribucion, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Validando que Existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando Resultado Obtenido
                    dtDetalles = ds.Tables["Table"];
            }
            //Devovliendo Objeto de Retorno
            return dtDetalles;
        }
        #endregion
    }
}
