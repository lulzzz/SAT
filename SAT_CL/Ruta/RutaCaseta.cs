using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Ruta
{
    /// <summary>
    /// Clase que realiza acciones sobre los registros de Ruta Caseta (Inserción, Edición, Consulta)
    /// </summary>
    public class RutaCaseta:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del store procedure de la tabla Ruta Caseta
        /// </summary>
        private static string nom_sp = "ruta.sp_ruta_casetas_trc";
        private int _id_ruta_caseta;
        /// <summary>
        /// Identifica el registro de una caseta asignada a una ruta
        /// </summary>
        public int id_ruta_caseta
        {
            get { return _id_ruta_caseta; }
        }
        private int _id_ruta;
        /// <summary>
        /// Identifica una ruta
        /// </summary>
        public int id_ruta
        {
            get { return _id_ruta; }
        }
        private int _id_caseta;
        /// <summary>
        /// Identifica a una caseta
        /// </summary>
        public int id_caseta
        {
            get { return _id_caseta; }
        }
        private int _id_tipo_deposito;
        /// <summary>
        /// Identifica la secuencia de una ruta
        /// </summary>
        public int id_tipo_deposito
        {
            get { return _id_tipo_deposito; }
        }
        private int _secuencia;
        /// <summary>
        /// Identifica la secuencia de una ruta
        /// </summary>
        public int secuencia
        {
            get { return _secuencia; }
        }
        private bool _habilitar;
        /// <summary>
        /// Actualiza el estado de uso de un registro (Habilitado-Disponible, Deshabilitado-No disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor default que inicializa los atributos a cero
        /// </summary>
        public RutaCaseta()
        {
            this._id_ruta_caseta = 0;
            this._id_ruta = 0;
            this._id_caseta = 0;
            this._id_tipo_deposito = 0;
            this._secuencia = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Cosntructor que inicializa los atributos de la clase a partir de la consulta de un registro
        /// </summary>
        /// <param name="id_ruta_caseta">Identficador que sirve como referencia para inicializar los atributos</param>
        public RutaCaseta(int id_ruta_caseta)
        {
            //Invoca al método encargado de consultar y asignar los valores a los atributos
            cargaAtributos(id_ruta_caseta);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~RutaCaseta()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de una RutaCaseta y el resultado lo almacena en los atributos de la clase
        /// </summary>
        /// <param name="id_ruta_caseta">Identifica al registro de una ruta caseta</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ruta_caseta)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de una RutaCaseta
            object[] param = { 3, id_ruta_caseta, 0, 0, 0, 0, 0, false, "", "" };
            //Realiza la busqueda de la Ruta Caseta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre el dataset y almacena los campos del registro en los atributos
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_ruta_caseta = id_ruta_caseta;
                        this._id_ruta = Convert.ToInt32(r["IRuta"]);
                        this._id_caseta = Convert.ToInt32(r["IdCaseta"]);
                        this._id_tipo_deposito = Convert.ToInt32(r["IdTipoDeposito"]);
                        this._secuencia = Convert.ToInt32(r["Secuencia"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza los campos de una RutaCaseta
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_caseta">Actualiza el identificador de una caseta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo actualizaciones sobre el registro</param>
        /// <param name="id_tipo_deposito">Actualiza la secuencia</param>
        /// <param name="habilitar">Actualiza el estado de uso del registro (Habilitado-Disponible, Deshabilitado-No Disponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarRutaCaseta(int id_ruta, int id_caseta, int id_usuario, int id_tipo_deposito, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del parametro que almacena los datos necesarios para realizar la actualización de la RutaCaseta
            object[] param = { 2, this._id_ruta_caseta, id_ruta, id_caseta, id_usuario, id_tipo_deposito, this._secuencia, habilitar, "", "" };
            //Realiza la actualización de la RutaCaseta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Publicos
        /// <summary>
        /// Método que actualiza los campos de una RutaCaseta
        /// </summary>
        /// <param name="id_ruta">Inserta el identificador de una ruta</param>
        /// <param name="id_caseta">Inserta el identificador de una caseta</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo la inserción del registro</param>
        /// <param name="id_tipo_deposito">Inserta el identificador de un deposito</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarRutaCaseta(int id_ruta, int id_caseta, int id_usuario, int id_tipo_deposito)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del parametro que almacena los datos necesarios para realizar la inserción de la RutaCaseta
            object[] param = { 1, 0, id_ruta, id_caseta, id_usuario, id_tipo_deposito, 0, true, "", "" };
            //Realiza la inserción de la RutaCaseta
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;

        }
        /// <summary>
        /// Método que actualiza los campos de una RutaCaseta
        /// </summary>
        /// <param name="id_ruta">Actualiza el identificador de una ruta</param>
        /// <param name="id_caseta">Actualiza el identificador de una caseta</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizo actualizaciones sobre el registro</param>
        /// <param name="secuencia">Actualiza la secuencia</param>
        /// <returns></returns>
        public RetornoOperacion EditarRutaCaseta(int id_ruta, int id_caseta, int secuencia, int id_usuario)
        {
            //Retorna al método el método que actualiza los registros
            return this.editarRutaCaseta(id_ruta, id_caseta, id_usuario, secuencia, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso del registro (Habilitado-Disponible, Deshabilitado-No Disponible)
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que deshabilito el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarRutaCaseta(int id_usuario)
        {
            //Retorna al método el método que actualiza los registros
            return this.editarRutaCaseta(this._id_ruta, this._id_caseta, id_usuario, this._secuencia, false);
        }

        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaRutaCaseta()
        {
            //Invoca al método que asigna valores a los atributos de la clase
            return this.cargaAtributos(this._id_ruta_caseta);
        }

        /// <summary>
        /// Carga Casetas ligando un Id de Ruta
        /// </summary>
        /// <param name="id_ruta"></param>
        /// <returns></returns>
        public static DataTable CargaCasetas(int id_ruta)
        {
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de una RutaCaseta
            object[] param = { 4, 0, id_ruta, 0, 0, 0, 0,false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Definiendo objeto de retorno
                DataTable mit = null;

                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Carga Monto Caseta Ruta
        /// </summary>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="ejes">Total de Ejes de las Unidades Asignadas</param>
        /// <returns></returns>
        public static DataTable CargaMontoCasetaRuta(int id_segmento, int id_ruta, int ejes)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 5, 0, id_ruta, 0, 0, 0, 0, false, id_segmento, ejes };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// [OBSOLETE]
        /// Carga Monto Caseta Ruta
        /// </summary>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="ejes">Total de Ejes de las Unidades Asignadas</param>
        /// <returns></returns>
        public static DataTable CargaMontoCasetaRutaIave(int id_segmento, int id_ruta, int ejes)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 6, 0, ejes, 0, 0, 0, 0, false, id_segmento, id_ruta };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Carga Monto Caseta Ruta
        /// </summary>
        /// <param name="id_segmento">Id Segmento</param>
        /// <param name="id_ruta">Id Ruta</param>
        /// <param name="ejes">Total de Ejes de las Unidades Asignadas</param>
        /// <returns></returns>
        public static DataTable CargaMontoCasetaRutaIave(int id_segmento, string id_ruta, int ejes)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo que almacena los datos necesarios para realizar la consulta del registro
            object[] param = { 6, 0, ejes, 0, 0, 0, 0, false, id_segmento, id_ruta };


            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        #endregion
    }
}
