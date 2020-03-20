using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;
using System.Configuration;

namespace SAT_CL.EgresoServicio
{
    public class AnticipoProgramado : Disposable
    {
        #region Atributos

        /// <summary>
        /// Nombre del Stored Procedure usado por la  clase
        /// </summary>
        private static string _nombre_stored_procedure = "egresos_servicio.sp_anticipo_programado_tap";

        private int _id_anticipo_programado;
        /// <summary>
        /// Obtiene el Id del anticipo programado
        /// </summary>
        public int id_anticipo_programado { get { return this._id_anticipo_programado; } }

        private int _id_anticipo;
        /// <summary>
        /// Obtiene el Id anticipo
        /// </summary>
        public int id_anticipo { get { return this._id_anticipo; } }

        private int _id_compania;
        /// <summary>
        /// Obtiene el Id compania
        /// </summary>
        public int id_compania { get { return this._id_compania; } }

        private int _id_servicio;
        /// <summary>
        /// Obtiene el Id de servicio
        /// </summary>
        public int id_servicio { get { return this._id_servicio; } }

        private int _id_concepto;
        /// <summary>
        /// Obtiene el Id de concepto
        /// </summary>
        public int id_concepto { get { return this._id_concepto; } }

        private decimal _monto;
        /// <summary>
        /// Obtiene el Monto
        /// </summary>
        public decimal monto { get { return this._monto; } }

        private DateTime _fecha_ejecucion;
        /// <summary>
        /// Obtiene la fecha en la que se ejecuto el anticipo programado
        /// </summary>
        public DateTime fecha_ejecucion { get { return this._fecha_ejecucion; } }

        private string _referencia;
        /// <summary>
        /// Obtiene el Id compania
        /// </summary>
        public string referencia { get { return this._referencia; } }

        private int _id_usuario;
        /// <summary>
        /// Obtiene el Id compania
        /// </summary>
        public int id_usuario { get { return this._id_usuario; } }

        private bool _habilitar;
        /// <summary>
        /// Obtiene el valor de habilitación del operador
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores
        public AnticipoProgramado()
        {
            cargaAtributosInstancia();
        }

        /// <summary>
        /// Crea una nueva instancia del tipo Anticipo Programado a partir del Id solicitado
        /// </summary>
        /// <param name="id_anticipo_programado">Id del anticipo programado</param>
        public AnticipoProgramado(int id_anticipo_programado)
        {
            cargaAtributosInstancia(id_anticipo_programado);
        }

        public AnticipoProgramado(int id_anticipo, int id_compania)
        {
            cargaAtributosInstancia(id_anticipo, id_compania);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Libera los recursos utilizados por la instancia
        /// </summary>
        ~AnticipoProgramado()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados
        /// <summary>
        /// Constructor que se encarga de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
            this._id_anticipo_programado = 0;
            this._id_anticipo = 0;
            this._id_compania = 0;
            this._id_servicio = 0;
            this._monto = 0;
            this._fecha_ejecucion = DateTime.MinValue;
            this._referencia = "";
            this._id_usuario = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Realiza la carga de los atributos de la instncia en base al Id de registro solicitado
        /// </summary>
        /// <param name="id_anticipo_programado">Id de usuario</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_anticipo_programado)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 3, id_anticipo_programado, 0, 0, 0, 0, 0, null, "", 0, false, "", "" };

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
                        this._id_anticipo_programado = Convert.ToInt32(r["Id"]);
                        this._id_anticipo = Convert.ToInt32(r["IdAnticipo"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_concepto = Convert.ToInt32(r["IdConcepto"]);
                        this._monto = Convert.ToDecimal(r["Monto"]);
                        DateTime.TryParse(r["FechaEjecucion"].ToString(), out this._fecha_ejecucion);
                        this._referencia = r["Referencia"].ToString();
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
        /// Realiza la carga de los atributos de la instncia en base al Id de anticipos y Id Compania
        /// </summary>
        /// <param name="id_anticipo">Id de usuario</param>
        /// <param name="id_compania">Id de compania</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_anticipo, int id_compania)
        {
            //Definiendo objeto de resultado
            bool resultado = false;

            //Inicializando arreglo de parámetros para consulta
            object[] param = { 5, 0, id_anticipo, id_compania, 0, 0, 0, null, "", 0, false, "", "" };
            
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
                        this._id_anticipo_programado = Convert.ToInt32(r["Id"]);
                        this._id_anticipo = Convert.ToInt32(r["IdAnticipo"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._id_servicio = Convert.ToInt32(r["IdServicio"]);
                        this._id_concepto = Convert.ToInt32(r["IdConcepto"]);
                        this._monto = Convert.ToDecimal(r["Monto"]);
                        DateTime.TryParse(r["FechaEjecucion"].ToString(), out this._fecha_ejecucion);
                        this._referencia = r["Referencia"].ToString();
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
        /// Edita un anticipo en la base de datos
        /// </summary>
        /// <param name="id_anticipo">Id Anticipo</param>
        /// <param name="id_compania">Id Compania</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="monto">Monto</param>
        /// <param name="fecha_ejecucion">fecha ejecucion</param>
        /// <param name="referencia">referencia</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <param name="habilitar">Valor de habilitación del amticipo</param>
        /// <returns></returns>
        private RetornoOperacion editaAnticipoProgramado(int id_anticipo, int id_compania, int id_servicio, int id_concepto, decimal monto, DateTime fecha_ejecucion, string referencia, int id_usuario, bool habilitar)
        {

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 2, this._id_anticipo_programado, id_anticipo, id_compania, id_servicio, id_concepto, monto, fecha_ejecucion, referencia, id_usuario, habilitar, "", "" };

            //Creando nuevo usuario en BD
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Registra un nuevo anticipo en la base de datos
        /// </summary>
        /// <param name="id_anticipo">Id Anticipo</param>
        /// <param name="id_compania">Id Compania</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="monto">Monto</param>
        /// <param name="fecha_ejecucion">fecha ejecucion</param>
        /// <param name="referencia">referencia</param>
        /// <param name="id_usuario">Id del usuario registra</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAnticipoProgramado(int id_anticipo, int id_compania, int id_servicio, int id_concepto, decimal monto, DateTime fecha_ejecucion, string referencia, int id_usuario)
        {
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando parámetros para registrar nuevo usuario
            object[] param = { 1, 0, id_anticipo, id_compania, id_servicio, id_concepto, monto, fecha_ejecucion, referencia, id_usuario, true, "", "" };

            //Creando nuevo usuario en BD
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Edita Anticipo Programado
        /// </summary>
        /// <param name="id_anticipo">Id Anticipo</param>
        /// <param name="id_compania">Id Compania</param>
        /// <param name="id_servicio">Id Servicio</param>
        /// <param name="id_concepto">Id Concepto</param>
        /// <param name="monto">Monto</param>
        /// <param name="fecha_ejecucion">fecha ejecucion</param>
        /// <param name="referencia">referencia</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <returns></returns>
        public RetornoOperacion EditarAnticipoProgramado(int id_anticipo, int id_compania, int id_servicio, int id_concepto, decimal monto, DateTime fecha_ejecucion, string referencia, int id_usuario)
        {
            return this.editaAnticipoProgramado(id_anticipo, id_compania, id_servicio, id_concepto, monto, fecha_ejecucion, referencia, id_usuario, this.habilitar);

        }
        
        /// <summary>
        /// Actualiza Referencia dependiendo del deposito registrado para un Anticipo Programado
        /// </summary>
        /// <param name="referencia">referencia</param>
        /// <param name="id_usuario">Id del usuario actualiza</param>
        /// <returns></returns>
        public RetornoOperacion AtualizaReferenciaAnticipoProgramado(string referencia, int id_usuario)
        {
            //Realizando actualización
            return this.editaAnticipoProgramado(this._id_anticipo, this._id_compania, this._id_servicio, this._id_concepto, this._monto, this._fecha_ejecucion, referencia, id_usuario, this._habilitar);

        }
        /// <summary>
        /// Deshabilita el registro de un anticipo programado
        /// </summary>
        /// <param name="id_usuario">Id de Usuario que actualiza</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAnticipoProgramado(int id_usuario)
        {

            //Realizando actualización
            return this.editaAnticipoProgramado(this._id_anticipo, this._id_compania, this._id_servicio, this._id_concepto, this._monto, this._fecha_ejecucion, this._referencia, id_usuario, false);

        }

        /// <summary>
        /// Realiza la actualización de los valores de atributos de la instancia volviendo a leer desde BD
        /// </summary>
        /// <returns></returns>
        public bool ActualizaAtributos()
        {
            return cargaAtributosInstancia(this._id_anticipo_programado);
        }

        /// <summary>
        /// Carga todos los anticipos de cada servicio en una ventana modal con GridView
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="no_servicio"></param>
        /// <returns></returns>
        public static DataTable CargaAnticiposProgramados(int id_compania, int no_servicio)
        {
            //Declarando objeto de retorno
            DataTable mit = null;

            //Inicializando los parámetros de consulta
            object[] param = { 4, 0, 0, id_compania, no_servicio, 0, 0, null, "", 0, false, "", "" };

            //Cargando los registros de interés
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Si hay registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables[0];

                //Devolviendo resultados
                return mit;
            }
        }
        #endregion

    }
}
