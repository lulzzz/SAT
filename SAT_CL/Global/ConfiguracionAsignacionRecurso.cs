using System;
using System.Collections.Generic;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con la Configuración  Unidad
    /// </summary>
   public  class ConfiguracionAsignacionRecurso :Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Enumera el TipoRegistro 
        /// </summary>
        public enum TipoRegistro
        {
            /// <summary>
            ///  Id Configuracion Asignación Recurso
            /// </summary>
            IdConfiguracionAsignacionRecurso = 1,
            /// <summary>
            /// IdCompania
            /// </summary>
            IdCompania
        }

        #endregion
       #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_configuracion_asignacion_recurso_tcu";

        private int _id_configuracion_asignacion_recurso;
        /// <summary>
        /// Atributo encargado de almacenar el Id Configuracion  Asignación Recurso
        /// </summary>
        public int id_configuracion_asignacion_recurso { get { return this._id_configuracion_asignacion_recurso; } }
        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de almacenar la Compañia Emisor
        /// </summary>
        public int id_compania_emisor  { get { return this._id_compania_emisor; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion { get { return this._descripcion; } }  
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public ConfiguracionAsignacionRecurso()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="tipo">tipo</param>
        /// <param name="id_registro">Id Registro</param>
        public ConfiguracionAsignacionRecurso(ConfiguracionAsignacionRecurso.TipoRegistro tipo, int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(tipo, id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ConfiguracionAsignacionRecurso()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   
            //Asignando Valores
            this._id_configuracion_asignacion_recurso = 0;
            this._id_compania_emisor = 0;
            this._descripcion = "";
            this._habilitar = false;
           

        }
        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="tipo">Tipo</param>
        /// <param name="id_registro">Id Registro</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(TipoRegistro tipo,  int id_registro)
        {
            int idConfiguracionUnidad = 0, idCompaniaEmisor = 0, idTipo = 0;
            
            //Validamos Tipo de Carga
            if(tipo == TipoRegistro.IdConfiguracionAsignacionRecurso)
            {
                idConfiguracionUnidad = id_registro;
                idTipo = 3;
            }
            else
            {
                idCompaniaEmisor = id_registro;
                idTipo = 4;
            }

            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = {idTipo, idConfiguracionUnidad, idCompaniaEmisor, "", 0, false, "", ""  };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_configuracion_asignacion_recurso = Convert.ToInt32(dr["Id"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._descripcion = dr["Descripcion"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="id_registro"></param>
        /// <returns></returns>
        public static List<ConfiguracionAsignacionRecurso> ObtieneConfiguracionAsignacionRecurso(TipoRegistro tipo, int id_registro)
        {
            int idConfiguracionUnidad = 0, idCompaniaEmisor = 0, idTipo = 0;

            //Validamos Tipo de Carga
            if (tipo == TipoRegistro.IdConfiguracionAsignacionRecurso)
            {
                idConfiguracionUnidad = id_registro;
                idTipo = 3;
            }
            else
            {
                idCompaniaEmisor = id_registro;
                idTipo = 4;
            }

            //Declarando Objeto de Retorno
            List<ConfiguracionAsignacionRecurso> retorno = new List<ConfiguracionAsignacionRecurso>();
            //Armando Objeto de Parametros
            object[] param = { idTipo, idConfiguracionUnidad, idCompaniaEmisor, "", 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        retorno.Add(new ConfiguracionAsignacionRecurso
                        {
                            _id_configuracion_asignacion_recurso = Convert.ToInt32(dr["Id"]),
                            _id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]),
                            _descripcion = dr["Descripcion"].ToString(),
                            _habilitar = Convert.ToBoolean(dr["Habilitar"])
                        });
                    }
                }
            }
            //Devolviendo Objeto de Retorno
            return retorno;
        }

        #endregion
    }
}
