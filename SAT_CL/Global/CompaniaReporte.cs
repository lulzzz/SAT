using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    public class CompaniaReporte : Disposable
    {
        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure de la Entidad
        /// </summary>
        private static string _nombre_stored_procedure = "global.sp_compania_reporte_tcr";

        private int _id_compania_reporte;
        /// <summary>
        /// Obtiene el Id del Reporte
        /// </summary>
        public int id_compania_reporte { get { return this._id_compania_reporte; } }
        private int _id_compania;
        /// <summary>
        /// Obtiene la compañía a la que pertenece el reporte
        /// </summary>
        public int id_compania { get { return this._id_compania; } }
        private string _descripcion_reporte;
        /// <summary>
        /// Obtiene la descripción del reporte
        /// </summary>
        public string descripcion_reporte { get { return this._descripcion_reporte; } }
        private string _url_reporte_ssrs;
        /// <summary>
        /// Obtiene la URL Base del reporte a mostrar
        /// </summary>
        public string url_reporte_ssrs { get { return this._url_reporte_ssrs; } }

        private int _id_tipo_reporte;
        /// <summary>
        /// Obtiene la compañía a la que pertenece el reporte
        /// </summary>
        public int id_tipo_reporte { get { return this._id_tipo_reporte; } }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del reporte
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor predeterminado, sólo accesible mediante herencia
        /// </summary>
        protected CompaniaReporte()
        {
            this._id_compania_reporte =
            this._id_compania = 0;
            this._descripcion_reporte =
            this._url_reporte_ssrs = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Crea una instancia del tipo CompaniaReporte a partir de un Id de registro
        /// </summary>
        /// <param name="id_compania_reporte">Id de Reporte</param>
        public CompaniaReporte(int id_compania_reporte)
        {
            cargaAtributosInstancia(id_compania_reporte);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro a consultar</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, "", "", 0, 0, false, "", "" };

            //Buscando registro en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo cada Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_compania_reporte = Convert.ToInt32(dr["Id"]);
                        this._id_compania_reporte = Convert.ToInt32(dr["IdCompania"]);
                        this._descripcion_reporte = dr["Descripcion"].ToString();
                        this._url_reporte_ssrs = dr["Url"].ToString();
                        this._id_tipo_reporte = Convert.ToInt32(dr["TipoReporte"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        //Asignando Retorno Correcto
                        result = true;
                        break;
                    }                    
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        #endregion
    }
}
