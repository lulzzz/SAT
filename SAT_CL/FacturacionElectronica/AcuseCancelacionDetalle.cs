using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica
{
    /// <summary>
    /// Clase que Define los Comportamientos y Estados del Detalle de Acuse de Cancelación
    /// </summary>
    public class AcuseCancelacionDetalle : Disposable
    {
        #region Atributos

        private static string nom_sp = "fe.sp_acuse_cancelacion_detalle_tacd";

        private int _id_acuse_cancelacion_detalle_tacd;
        /// <summary>
        /// Atributo encargado de Obtener el Id del Detalle de Acuse de Cancelación
        /// </summary>
        public int id_acuse_cancelacion_detalle_tacd
        {
            get { return this._id_acuse_cancelacion_detalle_tacd; }
        }

        private int _id_acuse_cancelacion_tacd;
        /// <summary>
        /// Atributo encargado de Obtener el Id de Acuse de Cancelación
        /// </summary>
        public int id_acuse_cancelacion_tacd
        {
            get { return this._id_acuse_cancelacion_tacd; }
        }

        private int _id_timbre_fiscal_digital_tacd;
        /// <summary>
        /// Atributo encargado de Obtener el Id del Timbre Fiscal Digital
        /// </summary>
        public int id_timbre_fiscal_digital_tacd
        {
            get { return this._id_timbre_fiscal_digital_tacd; }
        }

        private int _id_estatus_tacd;
        /// <summary>
        /// Atributo encargado de Obtener el Id del Estatus
        /// </summary>
        public int id_estatus_tacd
        {
            get { return this._id_estatus_tacd; }
        }

        private bool _habilitar_tacd;
        /// <summary>
        /// Atributo encargado de Obtener el Estatus de Habilitar
        /// </summary>
        public bool habilitar_tacd
        {
            get { return this._habilitar_tacd; }
        }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los Valores por Defecto
        /// </summary>
        public AcuseCancelacionDetalle()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor que inicializa los Valores en base a un Id
        /// </summary>
        /// <param name="id">Id de Detalle de Acuse de Cancelación</param>
        public AcuseCancelacionDetalle(int id)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id);
        }

        #endregion

        #region Destructores
        /// <summary>
        /// Destructor de Objetos de la Clase
        /// </summary>
        ~AcuseCancelacionDetalle()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método Privado Encargado de Inicializar los Valores
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Inicializando Valores por Defecto
            this._id_acuse_cancelacion_detalle_tacd = 0;
            this._id_acuse_cancelacion_tacd = 0;
            this._id_timbre_fiscal_digital_tacd = 0;
            this._id_estatus_tacd = 0;
            this._habilitar_tacd = false;
        }
        /// <summary>
        /// Método Privado Encargado de Inicializar los Valores con un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Variable de retorno
            bool result = false;
            //Declarando Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, false, "", "" };
            //Instanciando DataSet con los Registros de la Tabla
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {   //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table")) 
                {   //Obteniendo cada fila de la Tabla
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores Obtenidos
                        this._id_acuse_cancelacion_detalle_tacd = id_registro;
                        this._id_acuse_cancelacion_tacd = Convert.ToInt32(dr["IdAcuseCancelacion"]);
                        this._id_timbre_fiscal_digital_tacd = Convert.ToInt32(dr["IdTimbreFiscalDigital"]);
                        this._id_estatus_tacd = Convert.ToInt32(dr["IdEstatus"]);
                        this._habilitar_tacd = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    result = true;
                }
            }
            //Regresando Resultado
            return result;
        }
      
        /// <summary>
        /// Método Privado encargado de Editar los Atributos
        /// </summary>
        /// <param name="id_acuse_cancelacion_tacd">Id de Acuse de Cancelación</param>
        /// <param name="id_timbre_fiscal_digital_tacd">Id de Timbre Fiscal Digital</param>
        /// <param name="id_estatus_tacd">Id de Estatus</param>
        /// <param name="id_usuario_tacd">Id de Usuario</param>
        /// <param name="habilitar_tacd">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editaAtributos(int id_acuse_cancelacion_tacd, int id_timbre_fiscal_digital_tacd,
                                                    int id_estatus_tacd, int id_usuario_tacd, bool habilitar_tacd)
        {   //Declarando Parametros de Entrada
            object[] param = { 2,this._id_acuse_cancelacion_detalle_tacd,id_acuse_cancelacion_tacd,
                                 id_timbre_fiscal_digital_tacd,id_estatus_tacd,id_usuario_tacd,habilitar_tacd,
                                 "","" };
            //Cargando Respuesta del SP
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Regresando Resultado
            return result;
        }

        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <param name="id">Id de Registro</param>
        /// <returns></returns>
        public bool ActualizaAcuseDetalleCancelacion(int id)
        {   //Regresando Variable de Retorno
            return this.cargaAtributosInstancia(id);
        }
        /// <summary>
        /// Método Público encargado de la Insercción de Registros
        /// </summary>
        /// <param name="id_acuse_cancelacion_tacd">Id de Acuse de Cancelación</param>
        /// <param name="id_timbre_fiscal_digital_tacd">Id de Timbre Fiscal Digital</param>
        /// <param name="id_estatus_tacd">Id de Estatus</param>
        /// <param name="id_usuario_tacd">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarAcuseDetalleCancelacion(int id_acuse_cancelacion_tacd,
                                                    int id_timbre_fiscal_digital_tacd, int id_estatus_tacd,
                                                    int id_usuario_tacd)
        {   //Declarando Parametros de Entrada
            object[] param = { 1,0,id_acuse_cancelacion_tacd,id_timbre_fiscal_digital_tacd,
                                 id_estatus_tacd,id_usuario_tacd,true,"","" };
            //Cargando Respuesta del SP
            RetornoOperacion result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Regresando Resultado
            return result;
        }
        /// <summary>
        /// Método Público Encargado de la Edición de Atributos
        /// </summary>
        /// <param name="id_acuse_cancelacion_tacd">Id de Acuse de Cancelación</param>
        /// <param name="id_timbre_fiscal_digital_tacd">Id de Timbre Fiscal Digital</param>
        /// <param name="id_estatus_tacd">Id de Estatus</param>
        /// <param name="id_usuario_tacd">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarAcuseDetalleCancelacion(int id_acuse_cancelacion_tacd,
                                                    int id_timbre_fiscal_digital_tacd, int id_estatus_tacd,
                                                    int id_usuario_tacd)
        {   //Regresando Resultado del SP
            return editaAtributos(id_acuse_cancelacion_tacd, id_timbre_fiscal_digital_tacd, id_estatus_tacd,
                                                    id_usuario_tacd, this._habilitar_tacd);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar Registros
        /// </summary>
        /// <param name="id_usuario_tacd">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarAcuseDetalleCancelacion(int id_usuario_tacd)
        {   //Regresando Resultado del SP
            return editaAtributos(this._id_acuse_cancelacion_tacd, this._id_timbre_fiscal_digital_tacd, this._id_estatus_tacd,
                                                    id_usuario_tacd, false);
        }

        #endregion
    }
}

