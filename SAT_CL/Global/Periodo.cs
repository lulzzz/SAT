using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;

namespace SAT_CL.Global
{
    /// <summary>
    /// Clase encargada de todas las operaciones correspondientes con los Periodos
    /// </summary>
    public class Periodo : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_periodo_tpe";

        private int _id_periodo;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public int id_periodo { get { return this._id_periodo; } }
        private string _descripcion;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public string descripcion { get { return this._descripcion; } }
        private bool _bit_lunes;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_lunes { get { return this._bit_lunes; } }
        private bool _bit_martes;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_martes { get { return this._bit_martes; } }
        private bool _bit_miercoles;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_miercoles { get { return this._bit_miercoles; } }
        private bool _bit_jueves;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_jueves { get { return this._bit_jueves; } }
        private bool _bit_viernes;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_viernes { get { return this._bit_viernes; } }
        private bool _bit_sabado;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_sabado { get { return this._bit_sabado; } }
        private bool _bit_domingo;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool bit_domingo { get { return this.bit_domingo; } }
        private int _id_tipo_periodo;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public int id_tipo_periodo { get { return this._id_tipo_periodo; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar 
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Periodo()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_periodo">Registro</param>
        public Periodo(int id_periodo)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_periodo);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Periodo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Atributos
            this._id_periodo = 0;
            this._descripcion = "";
            this._bit_lunes = false;
            this._bit_martes = false;
            this._bit_miercoles = false;
            this._bit_jueves = false;
            this._bit_viernes = false;
            this._bit_sabado = false;
            this._bit_domingo = false;
            this._id_tipo_periodo = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_periodo">Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_periodo)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_periodo, "", false, false, false, false, false, false, false, 0, 0, false, "", "" };
            //Instanciando Resultado
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Atributos
                        this._id_periodo = id_periodo;
                        this._descripcion = dr["Descripcion"].ToString();
                        this._bit_lunes = Convert.ToBoolean(dr["BitLunes"]);
                        this._bit_martes = Convert.ToBoolean(dr["BitMartes"]);
                        this._bit_miercoles = Convert.ToBoolean(dr["BitMiercoles"]);
                        this._bit_jueves = Convert.ToBoolean(dr["BitJueves"]);
                        this._bit_viernes = Convert.ToBoolean(dr["BitViernes"]);
                        this._bit_sabado = Convert.ToBoolean(dr["BitSabado"]);
                        this._bit_domingo = Convert.ToBoolean(dr["BitDomingo"]);
                        this._id_tipo_periodo = Convert.ToInt32(dr["IdTipoPeriodo"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                    //Asignando Positivo el Resultado
                    result = true;
                }
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Registros en BD
        /// </summary>
        /// <param name="descripcion">Descripción del Periodo</param>
        /// <param name="bit_lunes">Indicador del Dia Lunes</param>
        /// <param name="bit_martes">Indicador del Dia Martes</param>
        /// <param name="bit_miercoles">Indicador del Dia Miercoles</param>
        /// <param name="bit_jueves">Indicador del Dia Jueves</param>
        /// <param name="bit_viernes">Indicador del Dia Viernes</param>
        /// <param name="bit_sabado">Indicador del Dia Sabado</param>
        /// <param name="bit_domingo">Indicador del Dia Domingo</param>
        /// <param name="id_tipo_periodo">Tipo del Periodo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion, bool bit_lunes, bool bit_martes, bool bit_miercoles, bool bit_jueves,
                                    bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_tipo_periodo, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_periodo, descripcion, bit_lunes, bit_martes, bit_miercoles, bit_jueves, bit_viernes, 
                               bit_sabado, bit_domingo, id_tipo_periodo, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Objeto de Retorno
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Periodos
        /// </summary>
        /// <param name="descripcion">Descripción del Periodo</param>
        /// <param name="bit_lunes">Indicador del Dia Lunes</param>
        /// <param name="bit_martes">Indicador del Dia Martes</param>
        /// <param name="bit_miercoles">Indicador del Dia Miercoles</param>
        /// <param name="bit_jueves">Indicador del Dia Jueves</param>
        /// <param name="bit_viernes">Indicador del Dia Viernes</param>
        /// <param name="bit_sabado">Indicador del Dia Sabado</param>
        /// <param name="bit_domingo">Indicador del Dia Domingo</param>
        /// <param name="id_tipo_periodo">Tipo del Periodo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaPeriodo(string descripcion, bool bit_lunes, bool bit_martes, bool bit_miercoles, bool bit_jueves,
                                    bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_tipo_periodo, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, descripcion, bit_lunes, bit_martes, bit_miercoles, bit_jueves, bit_viernes, 
                               bit_sabado, bit_domingo, id_tipo_periodo, id_usuario, true, "", "" };
            //Obteniendo Resultado de la Operación
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Periodos
        /// </summary>
        /// <param name="descripcion">Descripción del Periodo</param>
        /// <param name="bit_lunes">Indicador del Dia Lunes</param>
        /// <param name="bit_martes">Indicador del Dia Martes</param>
        /// <param name="bit_miercoles">Indicador del Dia Miercoles</param>
        /// <param name="bit_jueves">Indicador del Dia Jueves</param>
        /// <param name="bit_viernes">Indicador del Dia Viernes</param>
        /// <param name="bit_sabado">Indicador del Dia Sabado</param>
        /// <param name="bit_domingo">Indicador del Dia Domingo</param>
        /// <param name="id_tipo_periodo">Tipo del Periodo</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaPeriodo(string descripcion, bool bit_lunes, bool bit_martes, bool bit_miercoles, bool bit_jueves,
                                    bool bit_viernes, bool bit_sabado, bool bit_domingo, int id_tipo_periodo, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion, bit_lunes, bit_martes, bit_miercoles, bit_jueves, bit_viernes,
                               bit_sabado, bit_domingo, id_tipo_periodo, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Deshabilitar el Periodo
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaPeriodo(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion, this._bit_lunes, this._bit_martes, this._bit_miercoles, this._bit_jueves, this._bit_viernes,
                               this._bit_sabado, this._bit_domingo, this._id_tipo_periodo, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar el Periodo
        /// </summary>
        /// <returns></returns>
        public bool ActualizaPeriodo()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_periodo);
        }


        /// <summary>
        /// Validamos Si el Día se encuentra definido en el periodo.
        /// </summary>
        /// <param name="dia">Día a evaular</param>
        /// <returns></returns>
        public RetornoOperacion ValidaDiaEnPeriodo(DayOfWeek dia)
        {
            //Declaramos Objeto Retorno
            RetornoOperacion resultado = new RetornoOperacion("No existe el día en el periodo asignado al concepto");

            switch (dia)
            {
                //Si es Lunes
                case DayOfWeek.Monday:
                    //Validamos dia en Periodo
                    if (this._bit_lunes)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es Martes
                case DayOfWeek.Tuesday:
                    //Validamos dia en Periodo
                    if (this._bit_martes)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es Miercoles
                case DayOfWeek.Wednesday:
                    //Validamos dia en Periodo
                    if (this._bit_miercoles)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es jueves
                case DayOfWeek.Thursday:
                    //Validamos dia en Periodo
                    if (this._bit_jueves)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es Viernes
                case DayOfWeek.Friday:
                    //Validamos dia en Periodo
                    if (this._bit_viernes)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es Sabado
                case DayOfWeek.Saturday:
                    //Validamos dia en Periodo
                    if (this._bit_sabado)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
                //Si es Domingo
                case DayOfWeek.Sunday:
                    //Validamos dia en Periodo
                    if (this._bit_domingo)
                    {
                        resultado = new RetornoOperacion(0);
                    }
                    break;
            }
            //Devolvemos Resultado
            return resultado;
        }

        #endregion
    }
}
