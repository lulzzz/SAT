using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Notificaciones
{
    public class ListaDistribucion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "notificaciones.sp_lista_distribucion_tld";

        private int _id_lista_distribucion;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_lista_distribucion { get { return this._id_lista_distribucion; } }
        private int _id_compania;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private string _nombre_lista;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public string nombre_lista { get { return this._nombre_lista; } }
        private int _id_tabla_origen;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public int id_tabla_origen { get { return this._id_tabla_origen; } }
        private bool _bit_telefono;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool bit_telefono { get { return this._bit_telefono; } }
        private bool _bit_email;
        /// <summary>
        /// Atributo que almacena 
        /// </summary>
        public bool bit_email { get { return this._bit_email; } }
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
        public ListaDistribucion()
        {
            //Asignando Atributos
            this._id_lista_distribucion = 0;
            this._id_compania = 0;
            this._nombre_lista = "";
            this._id_tabla_origen = 0;
            this._bit_telefono = false;
            this._bit_email = false;
            this._habilitar = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_lista_distribucion"></param>
        public ListaDistribucion(int id_lista_distribucion)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_lista_distribucion);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ListaDistribucion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos del Registro
        /// </summary>
        /// <param name="id_lista_distribucion"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_lista_distribucion)
        {
            //Declarando Objeto de Retorno
            bool retorno = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_lista_distribucion, 0, "", 0, false, false, 0, false, "", "" };

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
                        this._id_lista_distribucion = id_lista_distribucion;
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._nombre_lista = Convert.ToString(dr["NombreLista"]);
                        this._id_tabla_origen = Convert.ToInt32(dr["IdTabla"]);
                        this._bit_telefono = Convert.ToBoolean(dr["BitTelefono"]);
                        this._bit_email = Convert.ToBoolean(dr["BitEmail"]);
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
        /// <param name="id_compania"></param>
        /// <param name="nombre_lista"></param>
        /// <param name="tabla_origen"></param>
        /// <param name="bit_telefono"></param>
        /// <param name="bit_email"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistroBD(int id_compania, string nombre_lista, int tabla_origen,
                                                bool bit_telefono, bool bit_email, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_lista_distribucion, id_compania, nombre_lista, tabla_origen, bit_telefono, bit_email, id_usuario, habilitar, "", "" };

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
        /// <param name="id_lista_distribucion_detalle_contacto"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_estatus"></param>
        /// <param name="secuencia"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="id_estatus_respuesta"></param>
        /// <param name="fecha_termino"></param>
        /// <param name="id_usuario_respuesta"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaListaDistribucion(int id_compania, string nombre_lista, int tabla_origen,
                                                bool bit_telefono, bool bit_email, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_compania, nombre_lista, tabla_origen, bit_telefono, bit_email, id_usuario, true, "", "" };

            //Ejecutando SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Editar
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="nombre_lista"></param>
        /// <param name="tabla_origen"></param>
        /// <param name="bit_telefono"></param>
        /// <param name="bit_email"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaListaDistribucion(int id_compania, string nombre_lista, int tabla_origen,
                                                bool bit_telefono, bool bit_email, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(id_compania, nombre_lista, tabla_origen,
                               bit_telefono, bit_email, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaListaDistribucion(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return actualizaRegistroBD(this._id_compania, this._nombre_lista, this._id_tabla_origen,
                               this._bit_telefono, this._bit_email, id_usuario, false);
        }

        #endregion
    }
}