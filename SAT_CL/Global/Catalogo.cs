using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones correspondientes con la tabla Catalogo
    /// </summary>
    public  class Catalogo: Disposable
    {
        #region Stored Procedure
        /// <summary>
        /// Obtiene el nombre del Stored Procedure que administra la tabla
        /// </summary>
        public static string nombre_stored_procedure = "global.sp_catalogo_tc";

        #endregion

        #region Atributos
        /// <summary>
        /// Llave primaria de la tabla Catalogo_tc
        /// </summary>
        private int _idCatalogo;
        public int idCatalogo { get { return _idCatalogo; } }
        /// <summary>
        /// Indica el numero de tipo de catálogo 
        /// </summary>
        private int _idTipoCatalogo;
        public int idTipoCatalogo { get { return _idTipoCatalogo; } }
        /// <summary>
        /// Describe la opcion de catalogo, puede ser por ejemplo, una abreviatura o clave
        /// </summary>
        private string _idValorCadena;
        public string idValorCadena { get { return _idValorCadena; } }
        /// <summary>
        /// Indica el numero consecutivo de la opcion de catalogo
        /// </summary>
        private int _idValor;
        public int idValor { get { return _idValor; } }
        /// <summary>
        /// Indica si el tipo de catalogo deriva de otro tipo de catalogo
        /// </summary>
        private int _idValorSuperior;
        public int idValorSuperior { get { return _idValorSuperior; } }
        /// <summary>
        /// Describe la opcion del catalogo
        /// </summary>
        private string _descripcion;
        public string descripcion { get { return _descripcion; } }
        /// <summary>
        /// Define si el registro se puede usar
        /// </summary>
        private bool _habilitar;
        public bool habilitar { get { return _habilitar; } }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que crea un objeto dado una llave primaria de catalogo
        /// </summary>
        /// <param name="idCatalogo"></param>
        public Catalogo(int idCatalogo)
        {
            cargaAtributosInstania(idCatalogo);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Catalogo()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Crea un objeto dado una llave primaria de catalogo
        /// </summary>
        /// <param name="idCatalogo"></param>
        /// <returns></returns>
        private bool cargaAtributosInstania(int idCatalogo)
        {
            //Declarando objeto retorno
            bool retorno = false;
            //Crear arreglo de parámetros
            object[] parametros = { 3, idCatalogo, 0, "", 0, 0, "", 0, true, "", "" };
            //Obtener resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Validar registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorrer registros
                    foreach (DataRow Row in ds.Tables["Table"].Rows)
                    {
                        //Asignar valores
                        this._idCatalogo = Convert.ToInt32(Row["Id"]);
                        this._idTipoCatalogo = Convert.ToInt32(Row["TipoCatalogo"]);
                        this._idValorCadena = Convert.ToString(Row["ValorCadena"]);
                        this._idValor = Convert.ToInt32(Row["Valor"]);
                        this._idValorSuperior = Convert.ToInt32(Row["CatalogoSuperior"]);
                        this._descripcion = Convert.ToString(Row["Descripcion"]);
                        this._habilitar = Convert.ToBoolean(Row["Habilitar"]);
                    }
                    //Asignando retorno positivo
                    retorno = true;
                }
            }
            //Devolver objeto retonro
            return retorno;
        }
        /// <summary>
        /// Método encargado de editar todos los atributos del objeto
        /// </summary>
        /// <param name="idTipoCatalogo"></param>
        /// <param name="valorCadena"></param>
        /// <param name="idValor"></param>
        /// <param name="idValorSuperior"></param>
        /// <param name="descripcion"></param>
        /// <param name="idUsuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaCatalogo(int idTipoCatalogo, string valorCadena, int idValor, int idValorSuperior, string descripcion, int idUsuario, bool habilitar)
        {
            //Declarando objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creando arreglo de parámetros
            object[] parametros = { 2, this._idCatalogo, idTipoCatalogo, valorCadena, idValor, idValorSuperior, descripcion, idUsuario, habilitar, "", "" };
            //Ejecutar SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, parametros);
            //Devolviendo retorno
            return retorno;
        }
        #endregion

        #region Metodos publicos estaticos

        /// <summary>
        /// Método encargado de regresar la descripcion que pertenece a un catálogo
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string RegresaDescripcionCatalogo(int tipo, int valor)
        {
            object[] parametros = { 4, 0, tipo, "",valor, 0, "", 0, true, "", "" };

            string retorno = "";

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        retorno = r[0].ToString();
                }
            }

            return retorno;
        }

        /// <summary>
        /// Método encargado de obtener el Valor Cadena ligando un tipo y valor
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="valor_superior"></param>
        /// <returns></returns>
        public static string RegresaDescripcioValorCadena(int tipo, int valor)
        {
            object[] parametros = { 5, 0, tipo,"", valor, 0, "", 0, true, "", "" };

            string retorno = "";

            //Ejecutando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                        retorno = r[0].ToString();
                }
            }

            return retorno;
        }
        /// <summary>
        /// Metodo que permite obtener el valor superior de una cadena
        /// </summary>
        /// <param name="tipo">permite definir el tipo de catalogo a consultar en la base</param>
        /// <param name="valor_superior">permite definir un valor de catalogo para obtener el valor superior</param>
        /// <returns></returns>
        public static string RegresaDescripcionValorSuperior(int tipo, int valor)
        {
            //Creación del objeto parametros con los datos necesarios para el store procedure
            object[] parametros = { 6, 0, tipo, "", valor, 0, "", 0, true, "", "" };
            //Creación del objeto retorno
            string retorno = "";
            //Invoca al sp de la clase
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre cada fila del data set y almacena en la variable r las coincidencias con los parametros tipo y valor superior
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                        //Asigna a la variable retorno el resultado de las coincidencias.
                        retorno = r[0].ToString();
                }
            }
            //Retorna el resultado al método. 
            return retorno;
        }

        /// <summary>
        /// Metodo que permite obtener el valor 
        /// </summary>
        /// <param name="tipo">permite definir el tipo de catalogo a consultar en la base</param>
        /// <param name="valor_superior">permite definir un valor de catalogo para obtener el valor superior</param>
        /// <returns></returns>
        public static string RegresaDescripcionValor(int tipo, string descripcion)
        {
            //Creación del objeto parametros con los datos necesarios para el store procedure
            object[] parametros = { 7, 0, tipo, "", 0, 0, descripcion, 0, true, "", "" };
            //Creación del objeto retorno
            string retorno = "";
            //Invoca al sp de la clase
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre cada fila del data set y almacena en la variable r las coincidencias con los parametros tipo y valor superior
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                        //Asigna a la variable retorno el resultado de las coincidencias.
                        retorno = r[0].ToString();
                }
            }
            //Retorna el resultado al método. 
            return retorno;
        }

        /// <summary>
        /// Metodo que permite obtener el valor de acuerdo a un tipo y valor cadena
        /// </summary>
        /// <param name="tipo">permite definir el tipo de catalogo a consultar en la base</param>
        /// <param name="valor_cadena">permite definir un valor cadena para obtener el valor superior</param>
        /// <returns></returns>
        public static string RegresaValorCadenaValor(int tipo, string valor_cadena)
        {
            //Creación del objeto parametros con los datos necesarios para el store procedure
            object[] parametros = { 8, 0, tipo, valor_cadena, 0, 0, "", 0, true, "", "" };
            //Creación del objeto retorno
            string retorno = "";
            //Invoca al sp de la clase
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre cada fila del data set y almacena en la variable r las coincidencias con los parametros tipo y valor superior
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                        //Asigna a la variable retorno el resultado de las coincidencias.
                        retorno = r[0].ToString();
                }
            }
            //Retorna el resultado al método. 
            return retorno;
        }

        /// <summary>
        /// Metodo que permite obtener el valor 
        /// </summary>
        /// <param name="tipo">permite definir el tipo de catalogo a consultar en la base</param>
        /// <param name="id_valor_superior">permite definir el valor superior</param>
        /// <param name="descripcion">permite definir la descripcionr</param>
        /// <returns></returns>
        public static string RegresaDescripcionValor(int tipo, int id_valor_superior, string descripcion)
        {
            //Creación del objeto parametros con los datos necesarios para el store procedure
            object[] parametros = { 9, 0, tipo, "", 0, id_valor_superior, descripcion, 0, true, "", "" };
            //Creación del objeto retorno
            string retorno = "";
            //Invoca al sp de la clase
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre cada fila del data set y almacena en la variable r las coincidencias con los parametros tipo y valor superior
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                        //Asigna a la variable retorno el resultado de las coincidencias.
                        retorno = r[0].ToString();
                }
            }
            //Retorna el resultado al método. 
            return retorno;
        }
        /// <summary>
        /// Metodo que permite obtener la descripción de acuerdo a un tipo y valor cadena
        /// </summary>
        /// <param name="tipo">permite definir el tipo de catalogo a consultar en la base</param>
        /// <param name="valor_cadena">permite definir un valor cadena para obtener el valor superior</param>
        /// <returns></returns>
        public static string RegresaDescripcionCadenaValor(int tipo, string valor_cadena)
        {
            //Creación del objeto parametros con los datos necesarios para el store procedure
            object[] parametros = { 10, 0, tipo, valor_cadena, 0, 0, "", 0, true, "", "" };
            //Creación del objeto retorno
            string retorno = "";
            //Invoca al sp de la clase
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nombre_stored_procedure, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre cada fila del data set y almacena en la variable r las coincidencias con los parametros tipo y valor superior
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                        //Asigna a la variable retorno el resultado de las coincidencias.
                        retorno = r[0].ToString();
                }
            }
            //Retorna el resultado al método. 
            return retorno;
        }

        /// <summary>
        /// Este método devuelve una tabla con todas las columnas y registros (opciones/catalogos) de un tipo de catálogo
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="tipoCat"></param>
        /// <returns></returns>
        public static DataTable ObtieneCatalogos(int idTipoCatalogo)
        {
            //Declarando objeto retorno
            DataTable dtClavesSP = null;
            //Crear arreglo de parámetros
            object[] parametros = { 11, 0, idTipoCatalogo, "", 0, 0, "", 0, true, "", "" };
            //Instanciando
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet (nombre_stored_procedure,parametros))
            {
                //Validar que devuelva registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {
                    dtClavesSP = ds.Tables["Table"];
                }
            }
            //Devolver tabla
            return dtClavesSP;
        }
        /// <summary>
        /// Inserta una nueva opcion en de un tipo de catalogo, la consecutividad (idValor) del lado de base de datos
        /// </summary>
        /// <param name="idTipoCatalogo"></param>
        /// <param name="valorCadena"></param>
        /// <param name="idValorSuperior"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaCatalogoConsecutivo(int idTipoCatalogo, string valorCadena, string descripcion, int idUsuario)
        {
            //Declarando objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Armar arreglo de parámetros
            object[] parametros = { 12, //Tipo 
                                      0, //PK
                                      idTipoCatalogo, //TipoCatalogo
                                      valorCadena, //ValorCadena
                                      0, //IdValor - Se obtiene en base de datos
                                      0, //IdValorSuperior  - Se obtiene en base de datos
                                      descripcion, //Descripcion
                                      idUsuario, //IdUsuario
                                      true, //Habilitar
                                      "", "" };
            //Ejecutar SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, parametros);
            //Devolver retorno
            return retorno;
        }
        /// <summary>
        /// Método encargado de editar el registro sin deshabilitarlo
        /// </summary>
        /// <param name="idTipoCatalogo"></param>
        /// <param name="valorCadena"></param>
        /// <param name="idValor"></param>
        /// <param name="idValorSuperior"></param>
        /// <param name="descripcion"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditarCatalogo(int idTipoCatalogo, string valorCadena, int idValor, int idValorSuperior, string descripcion, int idUsuario)
        {
            //Llamar al metodo privado
            return this.editaCatalogo(idTipoCatalogo, valorCadena, idValor, idValorSuperior, descripcion, idUsuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de deshablitar un registro, manteniendo el resto de sus atributos
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCatalogo(int idUsuario)
        {
            //Llamar al metodo privado
            return this.editaCatalogo(this._idTipoCatalogo, this._idValorCadena, this._idValor, this._idValorSuperior, this._descripcion, idUsuario, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTipoCatalogo"></param>
        /// <param name="valorCadena"></param>
        /// <param name="idValor"></param>
        /// <param name="idValorSuperior"></param>
        /// <param name="descripcion"></param>
        /// <param name="idUsuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        public RetornoOperacion editaVCadenaDescripcionCatalogo(string valorCadena, string descripcion, int idUsuario)
        {
            //Declarando objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creando arreglo de parámetros
            object[] parametros = { 13, 
                                      this._idCatalogo, 
                                      0, 
                                      valorCadena, 
                                      0, 
                                      0, 
                                      descripcion, 
                                      idUsuario, true, "", "" };
            //Ejecutar SP
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nombre_stored_procedure, parametros);
            //Devolviendo retorno
            return retorno;
        }
        #endregion
    }
}
