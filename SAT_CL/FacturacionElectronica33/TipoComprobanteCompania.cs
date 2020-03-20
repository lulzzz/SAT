using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de las operaciones de los Tipos de Comprobantes de las Companias
    /// </summary>
    public class TipoComprobanteCompania : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_tipo_comprobante_compania_ttcc";

        private int _id_tipo_comprobante_compania;
        /// <summary>
        /// Atributo que almacena el Identificador del Tipo de Comprobante por Compania
        /// </summary>
        public int id_tipo_comprobante_compania { get { return this._id_tipo_comprobante_compania; } }
        private int _id_tipo_comprobante;
        /// <summary>
        /// Atributo que almacena el Identificador del Tipo de Comprobante
        /// </summary>
        public int id_tipo_comprobante { get { return this._id_tipo_comprobante; } }
        private int _id_compania;
        /// <summary>
        /// Atributo que almacena el Identificador de la Compania
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private decimal _valor_maximo_permitido;
        /// <summary>
        /// Atributo que almacena el Valor Máximo Permitido del Tipo de Comprobante
        /// </summary>
        public decimal valor_maximo_permitido { get { return this._valor_maximo_permitido; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar del Registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public TipoComprobanteCompania()
        {
            //Asignando Valores
            this._id_tipo_comprobante_compania = 
            this._id_tipo_comprobante = 
            this._id_compania = 0;
            this._valor_maximo_permitido = 0.00M;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_comprobante_compania">Tipo de Comprobante por Compania</param>
        public TipoComprobanteCompania(int id_tipo_comprobante_compania)
        {

        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TipoComprobanteCompania()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tipo_comprobante_compania">Tipo de Comprobante por Compania</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_tipo_comprobante_compania)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_tipo_comprobante_compania, 0, 0, 0.00M, 0, false, "", "" };

            //Obteniendo Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registro
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_tipo_comprobante_compania = id_tipo_comprobante_compania;
                        this._id_tipo_comprobante = Convert.ToInt32(dr["IdTipoComprobante"]);
                        this._id_compania = Convert.ToInt32(dr["IdCompania"]);
                        this._valor_maximo_permitido = Convert.ToDecimal(dr["ValorMaximoPermitido"]);
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
        /// Método encargado de Actualizar los Atributos en BD
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="valor_maximo_permitido">Valor Máximo Permitido</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_tipo_comprobante, int id_compania, decimal valor_maximo_permitido, 
                                                      int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tipo_comprobante_compania, id_tipo_comprobante, id_compania, valor_maximo_permitido, 
                               id_usuario, habilitar, "", "" };

            //Actualizando Atributos
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Tipos de Comprobante por Compania
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="valor_maximo_permitido">Valor Máximo Permitido</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTipoComprobanteCompania(int id_tipo_comprobante, int id_compania, 
                                                                      decimal valor_maximo_permitido, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tipo_comprobante, id_compania, valor_maximo_permitido, 
                               id_usuario, true, "", "" };

            //Actualizando Atributos
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Tipos de Comprobante por Compania
        /// </summary>
        /// <param name="id_tipo_comprobante">Tipo de Comprobante</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="valor_maximo_permitido">Valor Máximo Permitido</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaTipoComprobanteCompania(int id_tipo_comprobante, int id_compania,
                                                             decimal valor_maximo_permitido, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_tipo_comprobante, id_compania, valor_maximo_permitido, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Tipos de Comprobante por Compania
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTipoComprobanteCompania(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_tipo_comprobante, this._id_compania, this._valor_maximo_permitido, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizr los Tipos de Comprobante por Compania
        /// </summary>
        /// <returns></returns>
        public bool ActualizaTipoComprobanteCompania()
        {
            //Devolviendo Carga de Atributos
            return this.cargaAtributosInstancia(this._id_tipo_comprobante_compania);
        }

        #endregion
    }
}
