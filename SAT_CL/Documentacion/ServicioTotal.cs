using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;

namespace SAT_CL.Documentacion
{   
    /// <summary>
    /// Clase encargada de todas las Operaciones 
    /// </summary>
    public class ServicioTotal : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "documentacion.sp_servicio_total_tst";

        private int _id_total_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Id Total del Servicio
        /// </summary>
        public int id_total_servicio { get { return this._id_total_servicio; } }
        private int _id_servicio;
        /// <summary>
        /// Atributo encargado de Almacenar el Id del Servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }
        private decimal _kms_vacio;
        /// <summary>
        /// Atributo encargado de Almacenar los Kilometros en Vacio
        /// </summary>
        public decimal kms_vacio { get { return this._kms_vacio; } }
        private decimal _kms_cargado;
        /// <summary>
        /// Atributo encargado de Almacenar los Kilometros Cargados
        /// </summary>
        public decimal kms_cargado { get { return this._kms_cargado; } }
        private decimal _kms_totales;
        /// <summary>
        /// Atributo encargado de Almacenar los Kilometros Totales
        /// </summary>
        public decimal kms_totales { get { return this._kms_totales; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Valores por Defecto
        /// </summary>
        public ServicioTotal()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Valores dado un Id de Registro
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        public ServicioTotal(int id_servicio)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_servicio);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ServicioTotal()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_total_servicio = 0;
            this._id_servicio = 0;
            this._kms_vacio = 0;
            this._kms_cargado = 0;
            this._kms_totales = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_total_servicio = id_registro;
                        this._id_servicio = Convert.ToInt32(dr["IdServicio"]);
                        this._kms_vacio = Convert.ToDecimal(dr["KmsVacio"]);
                        this._kms_cargado = Convert.ToDecimal(dr["KmsCargado"]);
                        this._kms_totales = Convert.ToDecimal(dr["KmsTotales"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registro en la BD
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="kms_vacio">KMS en Vacio</param>
        /// <param name="kms_cargado">KMS Cargados</param>
        /// <param name="kms_totales">KMS Totales</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_servicio, decimal kms_vacio, decimal kms_cargado, decimal kms_totales, 
	                                                int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2,this._id_total_servicio, id_servicio, kms_vacio, kms_cargado, 
                                 kms_totales, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar Servicios Totales
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="kms_vacio">KMS en Vacio</param>
        /// <param name="kms_cargado">KMS Cargados</param>
        /// <param name="kms_totales">KMS Totales</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaServicioTotal(int id_servicio, decimal kms_vacio, decimal kms_cargado, 
                                                            decimal kms_totales, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_servicio, kms_vacio, kms_cargado, 
                                 kms_totales, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar Servicios Totales
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        /// <param name="kms_vacio">KMS en Vacio</param>
        /// <param name="kms_cargado">KMS Cargados</param>
        /// <param name="kms_totales">KMS Totales</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaServicioTotal(int id_servicio, decimal kms_vacio, decimal kms_cargado,
                                                   decimal kms_totales, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_servicio, kms_vacio, kms_cargado,
                                            kms_totales, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar Servicios Totales
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaServicioTotal(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._id_servicio, this._kms_vacio, this._kms_cargado,
                                            this._kms_totales, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar Servicios Totales
        /// </summary>
        /// <returns></returns>
        public bool ActualizaServicioTotal()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_total_servicio);
        }
        /// <summary>
        /// Devuelve la instancia del Total correspondiente al servicio
        /// </summary>
        /// <param name="id_servicio">Id de servicio</param>
        /// <returns></returns>
        public static ServicioTotal ObtieneTotalServicio(int id_servicio)
        { 
            //Definiendo objeto de retorno
            ServicioTotal total = new ServicioTotal();

            //Armando arreglo de Parametros
            object[] param = { 4, 0, id_servicio, 0, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   
                        //Instanciando registro
                        total = new ServicioTotal(Convert.ToInt32(dr["Id"]));
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return total;
        }

        #endregion
    }
}
