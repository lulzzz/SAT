using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.TarifasPago
{
    /// <summary>
    /// Proporciona los medios para la adminsitración de recursos compuesta de las Tarifas
    /// </summary>
    public class TarifaCompuesta : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "tarifas_pago.sp_tarifa_compuesta_ttc";

        private int _id_tarifa_compuesta;
        /// <summary>
        /// Atributo que almacena el Id Primario de la Tarifa Compuesta
        /// </summary>
        public int id_tarifa_compuesta { get { return this._id_tarifa_compuesta; } }
        private int _id_tarifa_principal;
        /// <summary>
        /// Atributo que almacena la Tarifa Principal
        /// </summary>
        public int id_tarifa_principal { get { return this._id_tarifa_principal; } }
        private int _id_tarifa_secundaria;
        /// <summary>
        /// Atributo que almacena la Tarifa Secundaria
        /// </summary>
        public int id_tarifa_secundaria { get { return this._id_tarifa_secundaria; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public TarifaCompuesta()
        {
            //Asignando Valores
            this._id_tarifa_compuesta = 0;
            this._id_tarifa_principal = 0;
            this._id_tarifa_secundaria = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tarifa_compuesta">Tarifa Compuesta</param>
        public TarifaCompuesta(int id_tarifa_compuesta)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_tarifa_compuesta);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~TarifaCompuesta()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de iniciailzar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_tarifa_compuesta">Tarifa Compuesta</param>
        private bool cargaAtributosInstancia(int id_tarifa_compuesta)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_tarifa_compuesta, 0, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo Cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Valores
                        this._id_tarifa_compuesta = id_tarifa_compuesta;
                        this._id_tarifa_principal = Convert.ToInt32(dr["IdTarifaPrincipal"]);
                        this._id_tarifa_secundaria = Convert.ToInt32(dr["IdTarifaSecundaria"]);
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
        /// Método encargado de actualizar los Valores en BD
        /// </summary>
        /// <param name="id_tarifa_principal">Tarifa Principal</param>
        /// <param name="id_tarifa_secundaria">Tarifa Secundaria</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistro(int id_tarifa_principal, int id_tarifa_secundaria, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_tarifa_compuesta, id_tarifa_principal, id_tarifa_secundaria, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Tarifas Compuestas
        /// </summary>
        /// <param name="id_tarifa_principal">Tarifa Principal</param>
        /// <param name="id_tarifa_secundaria">Tarifa Secundaria</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifaCompuesta(int id_tarifa_principal, int id_tarifa_secundaria, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_tarifa_principal, id_tarifa_secundaria, id_usuario, true, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Tarifas Compuestas
        /// </summary>
        /// <param name="id_tarifa_principal">Tarifa Principal</param>
        /// <param name="id_tarifa_secundaria">Tarifa Secundaria</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifaCompuesta(int id_tarifa_principal, int id_tarifa_secundaria, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(id_tarifa_principal, id_tarifa_secundaria, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Tarifas Compuestas
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifaCompuesta(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistro(this._id_tarifa_principal, this._id_tarifa_secundaria, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Valores de la Tarifa Compuesta
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public bool ActualizaTarifaCompuesta(int id_usuario)
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tarifa_compuesta);
        }
        /// <summary>
        /// Método encargado de Obtener las Tarifas Secundarias dada una Tarifa Principal
        /// </summary>
        /// <param name="id_tarifa_principal">Tarifa Principal</param>
        /// <returns></returns>
        public static DataTable ObtieneTarifasSecundarias(int id_tarifa_principal)
        {
            //Declarando Objeto de Retorno
            DataTable dtTarifasSecundarias = null;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_tarifa_principal, 0, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Tarifa Secundaria
                    dtTarifasSecundarias = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtTarifasSecundarias;
        }
        /// <summary>
        /// Método encargado de Validar si la Tarifa es "Secundaria"
        /// </summary>
        /// <param name="id_tarifa">Tarifa</param>
        /// <returns></returns>
        public static bool ValidaTarifaSecundaria(int id_tarifa)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, 0, id_tarifa, 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Positivo
                    result = true;
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion
    }
}
