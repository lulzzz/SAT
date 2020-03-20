using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de las Operaciones del los Tipos de Comprobantes
    /// </summary>
    public class TipoComprobante : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_tipo_comprobante_ttc";

        private int _id_tipo_comprobante;
        /// <summary>
        /// Atributo que almacena el Identificador del Tipo de Comprobante
        /// </summary>
        public int id_tipo_comprobante { get { return this._id_tipo_comprobante; } }
        private string _clave;
        /// <summary>
        /// Atributo que almacena la Clave
        /// </summary>
        public string clave { get { return this._clave; } }
        private string _descripcion;
        /// <summary>
        /// Atributo que almacena la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private decimal _valor_maximo;
        /// <summary>
        /// Atributo que almacena el Valor Máximo
        /// </summary>
        public decimal valor_maximo { get { return this._valor_maximo; } }
        private decimal _valor_maximo_sueldos;
        /// <summary>
        /// Atributo que almacena el Valor Máximo (Sueldos)
        /// </summary>
        public decimal valor_maximo_sueldos { get { return this._valor_maximo_sueldos; } }
        private decimal _valor_maximo_otros;
        /// <summary>
        /// Atributo que almacena el Valor Máximo (Otros)
        /// </summary>
        public decimal valor_maximo_otros { get { return this._valor_maximo_otros; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que inicializa los Atributos por Defecto
        /// </summary>
        public TipoComprobante()
        {
            //Asignando Valores
            this._id_tipo_comprobante = 0;
            this._clave = 
            this._descripcion = "";
            this._valor_maximo =
            this._valor_maximo_sueldos =
            this._valor_maximo_otros = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
        public TipoComprobante(int id_tipo_comprobante)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_tipo_comprobante);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TipoComprobante()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tipo_comprobante)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_tipo_comprobante, "", "", 0.00M, 0.00M, 0.00M, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_tipo_comprobante = id_tipo_comprobante;
                        this._clave = dr["Clave"].ToString();
                        this._descripcion = dr["Descripcion"].ToString();
                        this._valor_maximo = Convert.ToDecimal(dr["ValorMaximo"]);
                        this._valor_maximo_sueldos = Convert.ToDecimal(dr["ValorMaximoSueldos"]);
                        this._valor_maximo_otros = Convert.ToDecimal(dr["ValorMaximoOtros"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros de la BD
        /// </summary>
        /// <param name="clave">Clave</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="valor_maximo">Valor Máximo</param>
        /// <param name="valor_maximo_sueldos">Valor Máximo (Sueldos)</param>
        /// <param name="valor_maximo_otros">Valor Máximo (Otros)</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(string clave, string descripcion, decimal valor_maximo, decimal valor_maximo_sueldos,
                                                      decimal valor_maximo_otros, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tipo_comprobante, clave, descripcion, valor_maximo, valor_maximo_sueldos, 
                               valor_maximo_otros, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Tipos de Comprobante
        /// </summary>
        /// <param name="clave">Clave</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="valor_maximo">Valor Máximo</param>
        /// <param name="valor_maximo_sueldos">Valor Máximo (Sueldos)</param>
        /// <param name="valor_maximo_otros">Valor Máximo (Otros)</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoComprobante(string clave, string descripcion, decimal valor_maximo, decimal valor_maximo_sueldos,
                                                       decimal valor_maximo_otros, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, clave, descripcion, valor_maximo, valor_maximo_sueldos, 
                               valor_maximo_otros, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Tipos de Comprobante
        /// </summary>
        /// <param name="clave">Clave</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="valor_maximo">Valor Máximo</param>
        /// <param name="valor_maximo_sueldos">Valor Máximo (Sueldos)</param>
        /// <param name="valor_maximo_otros">Valor Máximo (Otros)</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaTipoComprobante(string clave, string descripcion, decimal valor_maximo, decimal valor_maximo_sueldos,
                                                     decimal valor_maximo_otros, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(clave, descripcion, valor_maximo, valor_maximo_sueldos,
                               valor_maximo_otros, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Tipos de Comprobante
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoComprobante(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._clave, this._descripcion, this._valor_maximo, this._valor_maximo_sueldos,
                               this._valor_maximo_otros, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar el Tipo de Comprobante
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoComprobante()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tipo_comprobante);
        }

        #endregion
    }
}
