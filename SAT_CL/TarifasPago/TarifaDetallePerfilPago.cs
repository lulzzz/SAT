using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.TarifasPago
{
    /// <summary>
    /// Proporciona los medios para la gestión de Detalles de Perfil de Pago de Tarifa
    /// </summary>
    public class TarifaDetallePerfilPago:Disposable
    {
        #region Atributos

        private static string _nombre_stored_procedure = "tarifas_pago.sp_tarifa_detalle_perfil_pago_tdp";

        private int _id_tarifa_detalle_perfil_pago;
        /// <summary>
        /// Obtiene el Identificador único del detalle de perfil aplicado a tarifa
        /// </summary>
        public int id_tarifa_detalle_perfil_pago { get { return this._id_tarifa_detalle_perfil_pago; } }
        private int _id_tarifa;
        /// <summary>
        /// Obtiene el Id de Tarifa al que pertenece
        /// </summary>
        public int id_tarifa { get { return this._id_tarifa; } }
        private int _id_columna_filtro;
        /// <summary>
        /// Obtiene el Id de Columna Filtro asociado al detalle
        /// </summary>
        public int id_columna_filtro { get { return this._id_columna_filtro; } }
        private string _valor_columna_filtro;
        /// <summary>
        /// Obtiene el valor de la columna filtro asociada
        /// </summary>
        public string valor_columna_filtro { get { return this._valor_columna_filtro; } }
        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea una instancia del tipo TarifaDetallePerfilPago con los valores predeterminados
        /// </summary>
        public TarifaDetallePerfilPago()
        {
            this._id_tarifa_detalle_perfil_pago = 0;
            this._id_tarifa = 0;
            this._id_columna_filtro = 0;
            this._valor_columna_filtro = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Crea una instancia del tipo TarifaDetallePerfilPago, con los datos del registro solicitado
        /// </summary>
        /// <param name="id_tarifa_detalle_perfil_pago"></param>
        public TarifaDetallePerfilPago(int id_tarifa_detalle_perfil_pago)
        {
            cargaAtributosInstancia(id_tarifa_detalle_perfil_pago);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~TarifaDetallePerfilPago()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Carga los Atributos de un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   
            //Declarando Objeto de Retorno
            bool resultado = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, "", 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {   
                //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Asignando Valores
                        this._id_tarifa_detalle_perfil_pago = id_registro;
                        this._id_tarifa = Convert.ToInt32(dr["IdTarifa"]);
                        this._id_columna_filtro = Convert.ToInt32(dr["IdColumnaFiltro"]);
                        this._valor_columna_filtro = dr["ValorColumnaFiltro"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Valor Positivo
                    resultado = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return resultado;
        }
        /// <summary>
        /// Realiza la edición de datos en el registro
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_columna_filtro">Id de Columna Filtro</param>
        /// <param name="valor_columna_filtro">Valor de Columna Filtro</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaTarifaDetallePerfilPago(int id_tarifa, int id_columna_filtro, string valor_columna_filtro, int id_usuario, bool habilitar)
        {
            //Armando Arreglo de Parámetros
            object[] param = { 3, this._id_tarifa_detalle_perfil_pago, id_tarifa, id_columna_filtro, valor_columna_filtro, id_usuario, habilitar, "", "" };

            //Realizando edición de registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inserta un nuevo registro Detalle de Perfil de Pago para Tarifa
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_columna_filtro">Id de Columna Filtro</param>
        /// <param name="valor_columna_filtro">Valor Coluna FIltro</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTarifaDetallePerfilPago(int id_tarifa, int id_columna_filtro, string valor_columna_filtro, int id_usuario)
        {
            //Armando Arreglo de Parámetros
            object[] param = { 1, 0, id_tarifa, id_columna_filtro, valor_columna_filtro, id_usuario, true, "", "" };

            //Realizando edición de registro
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }
        /// <summary>
        /// Realiza la edición de datos en el registro
        /// </summary>
        /// <param name="id_tarifa">Id de Tarifa</param>
        /// <param name="id_columna_filtro">Id de Columna Filtro</param>
        /// <param name="valor_columna_filtro">Valor de Columna Filtro</param>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaTarifaDetallePerfilPago(int id_tarifa, int id_columna_filtro, string valor_columna_filtro, int id_usuario)
        {
            //Realizando edición de registro
            return editaTarifaDetallePerfilPago(id_tarifa, id_columna_filtro, valor_columna_filtro, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Realiza la deshabilitación del registro
        /// </summary>
        /// <param name="id_usuario">Id de usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaTarifaDetallePerfilPago(int id_usuario)
        {
            //Realizando edición de registro
            return editaTarifaDetallePerfilPago(this._id_tarifa, this._id_columna_filtro, this._valor_columna_filtro, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Obtener los perfiles de Pago
        /// </summary>
        /// <param name="id_tarifa">Tarifa de Pago</param>
        /// <returns></returns>
        public static DataTable ObtienePerfilesTarifa(int id_tarifa)
        {
            //Declarando Objeto de Retorno
            DataTable dtPerfiles = null;

            //Armando Arreglo de Parámetros
            object[] param = { 4, 0, id_tarifa, 0, "", 0, false, "", "" };

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Asignando Resultado Obtenido
                    dtPerfiles = ds.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dtPerfiles;
        }


        #endregion

    }
}
