using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.EgresoServicio
{
    public class MovimientoAsignacionDiesel : Disposable
    {
        #region Atributos
        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "egresos_servicio.sp_movimiento_asignacion_diesel_tmad";

        private int _id_movimiento_asignacion_diesel;
        /// <summary>
        /// Obtiene el Id de movimiento asignacion diesel
        /// </summary>
        public int id_movimiento_asignacion_diesel { get { return this._id_movimiento_asignacion_diesel; } }

        private int _id_movimiento;
        /// <summary>
        /// Obtiene el Id de movimiento
        /// </summary>
        public int id_movimiento { get { return this._id_movimiento; } }

        private int _id_vale_diesel;
        /// <summary>
        /// Obtiene el Id de vale diesel
        /// </summary>
        public int id_vale_diesel { get { return this._id_vale_diesel; } }

        private int _secuencia;
        /// <summary>
        /// Obtiene la secuencia
        /// </summary>
        public int secuencia { get { return this._secuencia; } }

        private int _id_tipo_vale_diesel;
        /// <summary>
        /// Obtiene el Id de tipo vale diesel
        /// </summary>
        public int id_tipo_vale_diesel { get { return this._id_tipo_vale_diesel; } }

        private decimal _kms;
        /// <summary>
        /// Obtiene el kms
        /// </summary>
        public decimal kms { get { return this._kms; } }

        private int _id_usuario;
        /// <summary>
        /// Obtiene el Id de usuario
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del movimiento
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor que se encarga de Inicializar los Atributos por Defecto
        /// </summary>
        public MovimientoAsignacionDiesel()
        {
            this._id_movimiento_asignacion_diesel = 0;
            this._id_movimiento = 0;
            this._id_vale_diesel = 0;
            this._secuencia = 0;
            this._id_tipo_vale_diesel = 0;
            this._kms = 0;
            this._id_usuario = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Crea una nueva instancia del tipo movimiento a partir del Id solicitado
        /// </summary>
        /// <param name="id_movimiento_asignacion_diesel">Id de Usuario</param>
        public MovimientoAsignacionDiesel(int id_movimiento_asignacion_diesel)
        {
            cargaAtributosInstancia(id_movimiento_asignacion_diesel);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera los recursos utilizados por la instancia
        /// </summary>
        ~MovimientoAsignacionDiesel()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realiza la carga de los atributos de la instncia en base al Id de registro solicitado
        /// </summary>
        /// <param name="id_movimiento_asignacion_diesel">Id de Movimiento</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_movimiento_asignacion_diesel)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_movimiento_asignacion_diesel, 0, 0, 0, 0, 0, 0, false, "", "" };

            //Realizando consulta hacia la BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando el origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Iterando para cada registro devuelto
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        //Asignando valores sobre atributos
                        this._id_movimiento_asignacion_diesel = Convert.ToInt32(r["Id"]);
                        this._id_movimiento = Convert.ToInt32(r["IdMovimiento"]);
                        this._id_vale_diesel = Convert.ToInt32(r["IdValeDiesel"]);
                        this._secuencia = Convert.ToInt32(r["Secuencia"]);
                        this._id_tipo_vale_diesel = Convert.ToInt32(r["IdTipoValeDiesel"]);
                        this._kms = Convert.ToDecimal(r["Kms"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);

                        //Asignando Variables Positiva
                        resultado = true;
                        //Terminando iteraciones
                        break;
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Reguistra un nuevo movimiento en la base de datos
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_vale_diesel">Id del vale diesel vinculado</param>
        /// <param name="secuencia">Secuencia del Id vale diesel</param>
        /// <param name="id_tipo_vale_diesel">Id Tipo Vale Diesel</param>
        /// <param name="kms">Kilometraje</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <param name="habilitar">Valor de habilitación del movimiento</param>
        /// <returns></returns>
        private RetornoOperacion editaMovimientoAsignacionDiesel(int id_movimiento, int id_vale_diesel, int id_tipo_vale_diesel, decimal kms, int id_usuario, bool habilitar)
        {

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_movimiento_asignacion_diesel, id_movimiento, id_vale_diesel, this._secuencia, id_tipo_vale_diesel, kms, id_usuario, habilitar, "", "" };

            //Creando nuevo usuario en BD
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Registra un nuevo movimiento en la base de datos
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_vale_diesel">Id del vale diesel vinculado</param>
        /// <param name="secuencia">Secuencia del Id vale diesel</param>
        /// <param name="id_tipo_vale_diesel">Id Tipo Vale Diesel</param>
        /// <param name="kms">Kilometraje</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaMovimientoAsignacionDiesel(int id_movimiento, int id_vale_diesel, int id_tipo_vale_diesel, decimal kms, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, id_movimiento, id_vale_diesel, 0, id_tipo_vale_diesel, kms, id_usuario, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita Movimiento Asignacion Diesel
        /// </summary>
        /// <param name="id_movimiento">Id Movimiento</param>
        /// <param name="id_vale_diesel">Id del vale diesel vinculado</param>
        /// <param name="secuencia">Secuencia del Id vale diesel</param>
        /// <param name="id_tipo_vale_diesel">Id Tipo Vale Diesel</param>
        /// <param name="kms">Kilometraje</param>
        /// <param name="id_usuario"> Id Usuario Actualiza </param>
        /// <returns></returns>
        public RetornoOperacion EditarMovimientoAsignacionDiesel(int id_movimiento, int id_vale_diesel, int id_tipo_vale_diesel, decimal kms, int id_usuario)
        {
            return this.editaMovimientoAsignacionDiesel(id_movimiento, id_vale_diesel, id_tipo_vale_diesel, kms, id_usuario, this.habilitar);

        }

        /// <summary>
        /// Deshabilita el registro movimiento
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaMovimientoAsignacionDiesel(int id_usuario)
        {

            //Realizando actualización
            return this.editaMovimientoAsignacionDiesel(this._id_movimiento, this._id_vale_diesel, this._id_tipo_vale_diesel, this._kms, id_usuario, false);

        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_movimiento_asignacion_diesel);
        }


        #endregion
    }
}
