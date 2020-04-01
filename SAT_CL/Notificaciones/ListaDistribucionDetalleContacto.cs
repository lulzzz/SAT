using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Notificaciones
{
    public class ListaDistribucionDetalleContacto : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "notificaciones.sp_lista_distribucion_detalle_contacto_tlddc";

        private int _id_lista_distribucion_detalle_contacto;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion_detalle_contacto { get { return this._id_lista_distribucion_detalle_contacto; } }
        private int _id_contacto;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_contacto { get { return this._id_contacto; } }
        private int _id_lista_distribucion_detalle;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion_detalle { get { return this._id_lista_distribucion_detalle; } }
        private int _id_tipo_evento;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_tipo_evento { get { return this._id_tipo_evento; } }
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
        public ListaDistribucionDetalleContacto()
        {
            //Asignando Atributos
            this._id_lista_distribucion_detalle_contacto = 0;
            this._id_contacto = 0;
            this._id_lista_distribucion_detalle = 0;
            this._id_tipo_evento = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        public ListaDistribucionDetalleContacto(int id_lista_distribucion_detalle_contacto)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_lista_distribucion_detalle_contacto);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ListaDistribucionDetalleContacto()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos del Registro
        /// </summary>
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_lista_distribucion_detalle_contacto)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_lista_distribucion_detalle_contacto, 0, 0, 0, 0, false, "", "" };

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
                        this._id_lista_distribucion_detalle_contacto = id_lista_distribucion_detalle_contacto;
                        this._id_contacto = Convert.ToInt32(dr["IdContacto"]);
                        this._id_lista_distribucion_detalle = Convert.ToInt32(dr["IdListaDistDetalle"]);
                        this._id_tipo_evento = Convert.ToInt32(dr["IdTipoEvento"]);
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
        /// <param name="id_contacto"></param>
        /// <param name="id_lista_distribucion_detalle"></param>
        /// <param name="tipo_evento"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_contacto, int id_lista_distribucion_detalle, int tipo_evento, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_lista_distribucion_detalle_contacto, id_contacto, id_lista_distribucion_detalle, tipo_evento, id_usuario, habilitar, "", "" };

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
        /// <param name="id_contacto"></param>
        /// <param name="id_lista_distribucion_detalle"></param>
        /// <param name="tipo_evento"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaListaDistribucionDetalleContacto(int id_contacto, int id_lista_distribucion_detalle, int tipo_evento, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_contacto, id_lista_distribucion_detalle, tipo_evento, id_usuario, true, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar
        /// </summary>
        /// <param name="id_contacto"></param>
        /// <param name="id_lista_distribucion_detalle"></param>
        /// <param name="tipo_evento"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaListaDistribucionDetalleContacto(int id_contacto, int id_lista_distribucion_detalle, int tipo_evento, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(id_contacto, id_lista_distribucion_detalle, tipo_evento, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaListaDistribucionDetalleContacto(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(this._id_contacto, this._id_lista_distribucion_detalle, this._id_tipo_evento, id_usuario, false);
        }
        /// <summary>
		/// Método encargado de Obtener los Detalles 
		/// </summary>
		/// <param name="id_requisicion">Requisición</param>
		/// <returns></returns>
		public static DataTable ObtieneContactosLista(int id_lista_distribucion)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;
            //Inicializando parámetros de inserción           
            object[] parametros = { 4, 0, 0, id_lista_distribucion, 0, 0, false, "", "" };
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
        /// <summary>
		/// Método encargado de Obtener los Detalles 
		/// </summary>
		/// <param name="id_requisicion">Requisición</param>
		/// <returns></returns>
		public static DataTable ObtieneContactosDetalle(int id_lista_distribucion_detalle)
        {
            //Declarando Objeto de Retorno
            DataTable dtDetalles = null;
            //Inicializando parámetros de inserción           
            object[] parametros = { 5, 0, 0, id_lista_distribucion_detalle, 0, 0, false, "", "" };
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
