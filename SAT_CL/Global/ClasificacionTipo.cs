using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Data.SqlClient;

namespace SAT_CL.Global
{
    /// <summary>
    ///Implementa los método para la administración de ClasificacionTipo.
    /// </summary>
   public class ClasificacionTipo:Disposable
    {
        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_clasificacion_tipo_tct";


        private int _id_clasificacion;
        /// <summary>
        /// Describe el Id de la Clasificación
        /// </summary>
        public int id_clasificacion
        {
            get { return _id_clasificacion; }
        }
       private int _id_compania_emisor;
        /// <summary>
        /// Describe la Compañia Emisor
        /// </summary>
        public int id_compania_emisor
        {
            get { return _id_compania_emisor; }
        }
        private int _id_campo_clasificacion;
        /// <summary>
        /// Describe el Tipo de Clasificación
        /// </summary>
        public int id_campo_clasificacion
        {
            get { return _id_campo_clasificacion; }
        }
        private string _codigo_clasificacion;
        /// <summary>
        /// Describe el codigo de clasificacion
        /// </summary>
        public string codigo_clasificacion
        {
            get { return _codigo_clasificacion; }
        }
        private string _descripcion_clasificacion;
        /// <summary>
        /// Describe la descripcion de la Clasificación
        /// </summary>
        public string descripcion_clasificacion
        {
            get { return _descripcion_clasificacion; }
        }
        private byte _id_valor_clasificacion;
        /// <summary>
        /// Describe el valor de la clasificación
        /// </summary>
        public byte id_valor_clasificacion
        {
            get { return _id_valor_clasificacion; }
        }
        private bool _habilitar;
        /// <summary>
        /// Describe el Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ClasificacionTipo()
        {
            Dispose(false);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Destructor por defecto
        /// </summary>
        public ClasificacionTipo()
        {
           _id_clasificacion = 0;
           _id_compania_emisor = 0;
           _id_campo_clasificacion  = 0;
           _codigo_clasificacion = "";
           _descripcion_clasificacion = "";
           _id_valor_clasificacion =  0;
           _habilitar = false;
        }

        /// <summary>
        /// Genera una Instancia Clasificación Tipo dado un Id
        /// </summary>
        /// <param name="id_clasificacion"></param>
        public ClasificacionTipo (int id_clasificacion)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_clasificacion, 0, 0, "", "", 0,0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_clasificacion = Convert.ToInt32(r["Id"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _id_campo_clasificacion = Convert.ToInt32(r["IdCampoClasificacion"]);
                        _codigo_clasificacion = r["CodigoClasificacion"].ToString();
                        _descripcion_clasificacion = r["DescripcionClasificacion"].ToString();
                        _id_valor_clasificacion = Convert.ToByte(r["IdValorClasificacion"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }

        /// <summary>
        /// Genera una Instancia Clasificación Tipo dado un Id ligado a una transaccion
        /// </summary>
        /// <param name="id_clasificacion"></param>
        /// <param name="transaccion"></param>
        public ClasificacionTipo(int id_clasificacion, SqlTransaction transaccion)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_clasificacion, 0, 0, "", "", 0, 0,false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param, transaccion))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_clasificacion = Convert.ToInt32(r["Id"]);
                        _id_compania_emisor = Convert.ToInt32(r["IdCompaniaEmisor"]);
                        _id_campo_clasificacion = Convert.ToInt32(r["IdCampoClasificacion"]);
                        _codigo_clasificacion = r["CodigoClasificacion"].ToString();
                        _descripcion_clasificacion = r["DescripcionClasificacion"].ToString();
                        _id_valor_clasificacion = Convert.ToByte(r["IdValorClasificacion"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                }
            }
        }
        #endregion

        #region Metodos privados

        
       /// <summary>
       /// Método encargado de edytar un Tipo de Clasificación
       /// </summary>
       /// <param name="id_compania_emisor"></param>
       /// <param name="id_campo_clasificacion"></param>
       /// <param name="codigo_clasificacion"></param>
       /// <param name="descripcion_clasificacion"></param>
       /// <param name="id_usuario"></param>
       /// <param name="habilitar"></param>
       /// <returns></returns>
        private RetornoOperacion editaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                   string descripcion_clasificacion, int id_usuario, bool habilitar  )
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_clasificacion, id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                  this._id_valor_clasificacion, id_usuario, habilitar, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        /// <summary>
        /// Método encargado de editar un Tipo de Clasificación ligado a una transacción
        /// </summary>
        /// <param name="id_compania_emisor"></param>
        /// <param name="id_campo_clasificacion"></param>
        /// <param name="codigo_clasificacion"></param>
        /// <param name="descripcion_clasificacion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <param name="transaccion"></param>
        /// <returns></returns>
        private RetornoOperacion editaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                   string descripcion_clasificacion, int id_usuario, bool habilitar, SqlTransaction transaccion)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_clasificacion, id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                  this._id_valor_clasificacion, id_usuario, habilitar, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }
        #endregion

        # region Metodos Publicos


       /// <summary>
       /// Inserta un Tipo de Clasificación
       /// </summary>
       /// <param name="id_compania_emisor"></param>
       /// <param name="id_campo_clasificacion"></param>
       /// <param name="codigo_clasificacion"></param>
       /// <param name="descripcion_clasificacion"></param>
       /// <param name="id_usuario"></param>
       /// <returns></returns>
        public static RetornoOperacion InsertaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                   string descripcion_clasificacion,  int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0,  id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                      0, id_usuario, true, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

       /// <summary>
       /// Inserta un Tipo de Clasificación ligada a una transacción
       /// </summary>
       /// <param name="id_compania_emisor"></param>
       /// <param name="id_campo_clasificacion"></param>
       /// <param name="codigo_clasificacion"></param>
       /// <param name="id_usuario"></param>
       /// <param name="transaccion"></param>
       /// <returns></returns>
        public static RetornoOperacion InsertaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                   string descripcion_clasificacion, int id_usuario, SqlTransaction transaccion)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0,  id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                      0, id_usuario, true, "", ""};

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param, transaccion);

        }



       /// <summary>
       /// Edita un Tipo de Clasificación
       /// </summary>
       /// <param name="id_compania_emisor"></param>
       /// <param name="id_campo_clasificacion"></param>
       /// <param name="codigo_clasificacion"></param>
       /// <param name="descripcion_clasificacion"></param>
       /// <param name="id_usuario"></param>
       /// <returns></returns>
        public RetornoOperacion EditaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                   string descripcion_clasificacion, int id_usuario)
        {
            return this.editaClasificacionTipo(id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                               id_usuario, this._habilitar);
        }

       /// <summary>
        ///  Edita un Tipo de Clasificación ligado a una transacción
       /// </summary>
       /// <param name="id_compania_emisor"></param>
       /// <param name="id_campo_clasificacion"></param>
       /// <param name="codigo_clasificacion"></param>
       /// <param name="descripcion_clasificacion"></param>
       /// <param name="id_usuario"></param>
       /// <param name="transaccion"></param>
       /// <returns></returns>
        public RetornoOperacion EditaClasificacionTipo(int id_compania_emisor, int id_campo_clasificacion, string codigo_clasificacion,
                                                  string descripcion_clasificacion, int id_usuario, SqlTransaction transaccion)
        {
            return this.editaClasificacionTipo(id_compania_emisor, id_campo_clasificacion, codigo_clasificacion, descripcion_clasificacion,
                                               id_usuario, this._habilitar, transaccion );
        }

      
        /// <summary>
        /// Deshabilita un Tipo de Clasificación
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaClasificacionTipo(int id_usuario)
        {
            return this.editaClasificacionTipo(this.id_compania_emisor, this.id_campo_clasificacion, this.codigo_clasificacion, this.descripcion_clasificacion,
                                                id_usuario, false);
        }

       /// <summary>
        /// Deshabilita un Tipo de Clasificación ligado a una transacción
       /// </summary>
       /// <param name="id_usuario"></param>
       /// <param name="transaccion"></param>
       /// <returns></returns>
        public RetornoOperacion DeshabilitaClasificacionTipo(int id_usuario, SqlTransaction transaccion)
        {
            return this.editaClasificacionTipo(this.id_compania_emisor, this.id_campo_clasificacion, this.codigo_clasificacion, this.descripcion_clasificacion,
                                               id_usuario, false, transaccion);
        }

        #endregion
    }
}
