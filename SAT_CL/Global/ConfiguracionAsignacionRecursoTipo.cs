using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con la Configuración Tipo Unidad
    /// </summary>
   public class ConfiguracionAsignacionRecursoTipo :Disposable
    {

        /// <summary>
        /// Enumera el Tipo Recurso
        /// </summary>
        public enum TipoRecurso
        {
            /// <summary>
            /// Unidad
            /// </summary>
            Unidad = 1,
            /// <summary>
            /// Operador
            /// </summary>
            Operador,
            /// <summary>
            /// Tercero
            /// </summary>
            Tercero

        }

        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_configuracion_asignacion_recurso_tipo_tctu";

        private int _id_configuracion_asignacion_recurso_tipo;
        /// <summary>
        /// Atributo encargado de almacenar el Id Configuracion Asignacion Recurso Tipo
        /// </summary>
        public int id_configuracion_asignacion_recurso_tipo { get { return this._id_configuracion_asignacion_recurso_tipo; } }
        private int _id_configuracion_asignacion_recurso;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Configuracion Asignación Recurso
        /// </summary>
        public int id_configuracion_asignacion_recurso { get { return this._id_configuracion_asignacion_recurso; } }
        private byte _id_tipo_recurso;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tipo Recurso
        /// </summary>
        public byte id_tipo_recurso { get { return this._id_tipo_recurso; } } 
        private int _id_tipo_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Tipo Unidad
        /// </summary>
        public int id_tipo_unidad { get { return this._id_tipo_unidad; } }
         private int _id_sub_tipo_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Subtipo
        /// </summary>
        public int  id_sub_tipo_unidad{ get { return this._id_sub_tipo_unidad; } }
        private byte _cantidad;
        /// <summary>
        /// Atributo encargado de almacenar la cantidad
        /// </summary>
        public byte cantidad { get { return this._cantidad; } }
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
        public ConfiguracionAsignacionRecursoTipo()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso_tipo">Id Configuracion Tipo Unidad</param>
        public ConfiguracionAsignacionRecursoTipo(int id_configuracion_asignacion_recurso_tipo)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_configuracion_asignacion_recurso_tipo);
        }

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Configuracion y un Tipo Recurso
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso">Id Configuracion Tipo Unidad</param>
        /// <param name="tipo">Tipo Recurso </param>
        public ConfiguracionAsignacionRecursoTipo(int id_configuracion_asignacion_recurso, TipoRecurso tipo)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_configuracion_asignacion_recurso, tipo);
        }

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso">Id Configuracion</param>
        /// <param name="tipo">Tipo Recurso</param>
        /// <param name="id_tipo_unidad">Id Tipo Unidad</param>
        /// <param name="id_sub_tipo">Id Subtipo</param>
        public ConfiguracionAsignacionRecursoTipo(int id_configuracion_asignacion_recurso, TipoRecurso tipo, int id_tipo_unidad, int id_sub_tipo)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_configuracion_asignacion_recurso, tipo, id_tipo_unidad, id_sub_tipo);
        }
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ConfiguracionAsignacionRecursoTipo()
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
            this._id_configuracion_asignacion_recurso_tipo = 0;
            this._id_configuracion_asignacion_recurso= 0;
            this._id_tipo_recurso = 0;
            this._id_tipo_unidad = 0;
            this._id_sub_tipo_unidad = 0;
            this._cantidad = 0;
            this._habilitar = false;
           

        }
        /// <summary>
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso_tipo">Id Configuracion Tipo Unidad</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_configuracion_asignacion_recurso_tipo)
        {  
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3, id_configuracion_asignacion_recurso_tipo, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_configuracion_asignacion_recurso_tipo = Convert.ToInt32(dr["Id"]);
                        this._id_configuracion_asignacion_recurso = Convert.ToInt32(dr["IdConfiguracionRecurso"]);
                        this._id_tipo_recurso = Convert.ToByte(dr["IdTipoRecurso"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToInt32(dr["IdSubTipoUnidad"]);
                        this._cantidad = Convert.ToByte(dr["Cantidad"]);
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
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id Configuracion y Tipo Recurso
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso">Id Configuración Recurso</param>
        /// <param name="tipo">Tipo</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_configuracion_asignacion_recurso, TipoRecurso tipo)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 4, 0, id_configuracion_asignacion_recurso, tipo, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_configuracion_asignacion_recurso_tipo = Convert.ToInt32(dr["Id"]);
                        this._id_configuracion_asignacion_recurso = Convert.ToInt32(dr["IdConfiguracionRecurso"]);
                        this._id_tipo_recurso = Convert.ToByte(dr["IdTipoRecurso"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToInt32(dr["IdSubTipoUnidad"]);
                        this._cantidad = Convert.ToByte(dr["Cantidad"]);
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
        /// Método Priavdo encargado de Inicializar los Atributos dado un Id Configuración, Tipo Recurso,  Id Tipo Unidad, Id Sub Tipo
        /// </summary>
        /// <param name="id_configuracion_asignacion_recurso">Id Configuración Recurso</param>
        /// <param name="tipo">Tipo Recurso</param>
        /// <param name="id_tipo_unidad">Tipo Unidad</param>
        /// <param name="id_sub_tipo">Id Sub Tipo</param>
        /// <returns></returns>
        public bool cargaAtributosInstancia(int id_configuracion_asignacion_recurso, TipoRecurso tipo, int id_tipo_unidad,
                                           int id_sub_tipo)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 5, 0, id_configuracion_asignacion_recurso, tipo, id_tipo_unidad, id_sub_tipo, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_configuracion_asignacion_recurso_tipo = Convert.ToInt32(dr["Id"]);
                        this._id_configuracion_asignacion_recurso = Convert.ToInt32(dr["IdConfiguracionRecurso"]);
                        this._id_tipo_recurso = Convert.ToByte(dr["IdTipoRecurso"]);
                        this._id_tipo_unidad = Convert.ToInt32(dr["IdTipoUnidad"]);
                        this._id_sub_tipo_unidad = Convert.ToInt32(dr["IdSubTipoUnidad"]);
                        this._cantidad = Convert.ToByte(dr["Cantidad"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Resultado a Positivo
                    result = true;
                }
            }
            //Devolviendo Objeto de Retorno
            return result;
        }



        #endregion


        #region Métodos Públicos

        /// <summary>
        /// Cargos Unidades Motrices que permitan la asignación unidades de Arrastre
        /// </summary>
        /// <param name="id_configuraciones_asignacion_recurso">Id Configuración asignación recurso</param>
        /// <returns></returns>
        public static DataTable CargaUnidadesMotricesPermitanArrastre(int id_configuraciones_asignacion_recurso)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = { 6, 0, id_configuraciones_asignacion_recurso, 0, 0, 0, 0, 0, false, "", "" };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
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
