using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las funciones de la Información Aduanera de los Conceptos
    /// </summary>
    public class ConceptoInformacionAduanera : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que almacena el Nombre del SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_concepto_informacion_aduanera_tcia";

        private int _id_concepto_informacion_aduanera;
        /// <summary>
        /// Atributo que almacena el Identificador de la Información Aduanera del Concepto
        /// </summary>
        public int id_concepto_informacion_aduanera { get { return this._id_concepto_informacion_aduanera; } }
        private int _id_concepto;
        /// <summary>
        /// Atributo que almacena el Concepto de Interes
        /// </summary>
        public int id_concepto { get { return this._id_concepto; } }
        private string _no_pedimento;
        /// <summary>
        /// Atributo que almacena el Número de Pedimento de la Información Aduanera
        /// </summary>
        public string no_pedimento { get { return this._no_pedimento; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que Inicializa los Atributos por Defecto 
        /// </summary>
        public ConceptoInformacionAduanera()
        {
            //Asignando Valores
            this._id_concepto_informacion_aduanera = 
            this._id_concepto = 0;
            this._no_pedimento = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que Inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_concepto_informacion_aduanera"></param>
        public ConceptoInformacionAduanera(int id_concepto_informacion_aduanera)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_concepto_informacion_aduanera);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ConceptoInformacionAduanera()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_concepto_informacion_aduanera"></param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_concepto_informacion_aduanera)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_concepto_informacion_aduanera, 0, "", 0, false, "", "" };

            //Obteniendo Resultado de BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Resultados
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_concepto_informacion_aduanera = id_concepto_informacion_aduanera;
                        this._id_concepto = Convert.ToInt32(dr["IdConcepto"]);
                        this._no_pedimento = dr["NoPedimento"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Resultado Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_concepto">Concepto</param>
        /// <param name="no_pedimento">No. de Pedimento</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_concepto, string no_pedimento, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_concepto_informacion_aduanera, id_concepto, no_pedimento, id_usuario, habilitar, "", "" };

            //Obteniendo registro de la BD
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar los Conceptos de Información Aduanera
        /// </summary>
        /// <param name="id_concepto">Concepto</param>
        /// <param name="no_pedimento">No. de Pedimento</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConceptoInformacionAduanera(int id_concepto, string no_pedimento, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_concepto, no_pedimento, id_usuario, true, "", "" };

            //Obteniendo registro de la BD
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar los Conceptos de Información Aduanera
        /// </summary>
        /// <param name="id_concepto">Concepto</param>
        /// <param name="no_pedimento">No. de Pedimento</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoInformacionAduanera(int id_concepto, string no_pedimento, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_concepto, no_pedimento, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar los Conceptos de Información Aduanera
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConceptoInformacionAduanera(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_concepto, this._no_pedimento, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Conceptos de Información Aduanera
        /// </summary>
        /// <returns></returns>
        public bool ActualizaConceptoInformacionAduanera()
        {
            //Devolviendo resultado Obtenido
            return this.cargaAtributosInstancia(this._id_concepto_informacion_aduanera);
        }

        #endregion
    }
}
