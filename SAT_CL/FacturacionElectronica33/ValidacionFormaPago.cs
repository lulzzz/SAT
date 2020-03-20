using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    /// <summary>
    /// Clase encargada de todas las funciones de las Validaciones de las Formas de Pago
    /// </summary>
    public class ValidacionFormaPago : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el SP
        /// </summary>
        private static string _nom_sp = "fe33.sp_validacion_forma_pago_tvfp";

        private int _id_validacion_forma_pago;
        /// <summary>
        /// Atributo encargado de Almacenar la Validación de Forma de Pago
        /// </summary>
        public int id_validacion_forma_pago { get { return this._id_validacion_forma_pago; } }
        private int _id_forma_pago;
        /// <summary>
        /// Atributo encargado de Almacenar la Forma de Pago
        /// </summary>
        public int id_forma_pago { get { return this._id_forma_pago; } }
        private int _id_tipo_validacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Tipo de Validación
        /// </summary>
        public int id_tipo_validacion { get { return this._id_tipo_validacion; } }
        private string _descripcion_validacion;
        public string descripcion_validacion { get { return this._descripcion_validacion; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que Inicializa los Atributos por Defecto
        /// </summary>
        public ValidacionFormaPago()
        {
            //Asignando Valores
            this._id_validacion_forma_pago =
            this._id_forma_pago =
            this._id_tipo_validacion = 0;
            this._habilitar = false;
        }
        
        /// <summary>
        /// Constructor que Inicializa los Atributos dado un Registro
        /// </summary>
        /// <param name="id_validacion_forma_pago">Validación de Forma de Pago</param>
        public ValidacionFormaPago(int id_validacion_forma_pago)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_validacion_forma_pago);
        }
        
        /// <summary>
        /// Constructor para llamar al StoreProcedure en su tipo 4
        /// </summary>
        /// <param name="id_forma_pago"></param>
        public ValidacionFormaPago(int id_forma_pago, string cadenainutil)
        {
            cargaAtributosInstancia(id_forma_pago,cadenainutil);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~ValidacionFormaPago()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_validacion_forma_pago">Validación de Forma de Pago</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_validacion_forma_pago)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_validacion_forma_pago, 0, 0, 0, false, "", "" };

            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_validacion_forma_pago = id_validacion_forma_pago;
                        this._id_forma_pago = Convert.ToInt32(dr["IdFormaPago"]);
                        this._id_tipo_validacion = Convert.ToInt32(dr["IdTipoValidacion"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Cargar los Atributos dado un Registro, la cadena solo se usa para crear una sobrecarda diferente
        /// </summary>
        /// <param name="id_validacion_forma_pago">Validación de Forma de Pago</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_forma_pago, string cadenainutil)
        {
            //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 4, 0, id_forma_pago, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_validacion_forma_pago = Convert.ToInt32(dr["Id"]);
                        this._id_forma_pago = id_forma_pago;
                        this._id_tipo_validacion = Convert.ToInt32(dr["IdTipoValidacion"]);
                        this._descripcion_validacion = Convert.ToString(dr["DescripcionValidacion"]);
                    }
                    //Asignando Retorno Positivo
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Actualizar los Registros en la BD
        /// </summary>
        /// <param name="id_forma_pago">Forma de Pago</param>
        /// <param name="id_tipo_validacion">Tipo de Validación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <param name="habilitar">Estatus Habilitar del Registro</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistrosBD(int id_forma_pago, int id_tipo_validacion, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_validacion_forma_pago, id_forma_pago, id_tipo_validacion, id_usuario, habilitar, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar las Validaciones de las Formas de Pago
        /// </summary>
        /// <param name="id_forma_pago">Forma de Pago</param>
        /// <param name="id_tipo_validacion">Tipo de Validación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaValidacionFormaPago(int id_forma_pago, int id_tipo_validacion, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_forma_pago, id_tipo_validacion, id_usuario, true, "", "" };

            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Validaciones de las Formas de Pago
        /// </summary>
        /// <param name="id_forma_pago">Forma de Pago</param>
        /// <param name="id_tipo_validacion">Tipo de Validación</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaValidacionFormaPago(int id_forma_pago, int id_tipo_validacion, int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(id_forma_pago, id_tipo_validacion, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método encargado de Deshabilitar las Validaciones de las Formas de Pago
        /// </summary>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaValidacionFormaPago(int id_usuario)
        {
            //Devolviendo Resultado Obtenido
            return this.actualizaRegistrosBD(this._id_forma_pago, this._id_tipo_validacion, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de Actualizar los Atributos de las Validaciones de las Formas de Pago
        /// </summary>
        /// <returns></returns>
        public bool ActualizaValidacionFormaPago()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_validacion_forma_pago);
        }

        public static DataTable ObtieneValidacionesFP(int id_forma_pago)
        {
            //Declarando objeto de retorno
            DataTable dtValidacionFormaPago = null;
            //Crear arreglo de parámetros
            object[] param = { 4, 0, id_forma_pago, 0, 0, false, "", "" };
            //Instanciando...
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validar que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Asignar el resultado obtenido al objeto retorno
                    dtValidacionFormaPago = ds.Tables["Table"];
                }
            }
            return dtValidacionFormaPago;
        }
        #endregion
    }
}
