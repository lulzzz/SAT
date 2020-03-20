using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;

namespace TMS_CL.Global
{
    class AjusteUtc : Disposable
    {
        #region Enumeraciones
        /// <summary>
        /// Define los posibles estaus que puede adoptar la limpieza interior de la unidad.
        /// </summary>
        #endregion

        #region Atributos
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nom_sp = "global.sp_ajuste_utc_tau";

        /// <summary>
        /// Representa el identificador único del registro
        /// </summary>
        public int id_ajuste_utc { get { return _id_ajuste_utc; } }
        private int _id_ajuste_utc;

        /// <summary>
        /// Representa la fecha de inicio
        /// </summary>
        public DateTime fecha_inicio { get { return _fecha_inicio; } }
        private DateTime _fecha_inicio;

        /// <summary>
        /// Representa la fecha de fin
        /// </summary>
        public DateTime fecha_fin { get { return _fecha_fin; } }
        private DateTime _fecha_fin;

        /// <summary>
        /// Almacena el valor de utc
        /// </summary>
        public string valor_utc { get { return _valor_utc; } }
        private string _valor_utc;

        /// <summary>
        /// Representa si un registro puede ser usado por el usuario o no
        /// </summary>
        public bool habilitar { get { return _habilitar; } }
        private bool _habilitar;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public AjusteUtc()
        {
            _id_ajuste_utc = 0;
            _fecha_inicio = DateTime.MinValue;
            _fecha_fin = DateTime.MinValue;
            _valor_utc = "";
            _habilitar = false;

        }

        /// <summary>
        /// Constructor que inicializa la instancia en relacion al id especifico
        /// </summary>
        /// <param name="IdAjusteUtc"></param>
        public AjusteUtc(int id_ajuste_utc)
        {
            //Invoca al método cargaAtributos
            cargaAtributos(id_ajuste_utc);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AjusteUtc()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda de registros, y almacena el resultado en los atributos
        /// </summary>
        /// <param name="id_ajuste_utc">Identificador de reporte de unidades foraneas</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_ajuste_utc)
        {
            //Creacion del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para el store procedure
            object[] param = { 3, id_ajuste_utc, null, null, "", 0, false, "", "" };
            //Crea un dataset y almacena el resultado del método EjecutaProcAlmacenadoDataSet 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Valida que existan datos en el dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Recorre las filas del registro encontrado y almacena el resultado en cada atributo
                    foreach (DataRow r in DS.Tables["Table"].Rows)
                    {
                        this._id_ajuste_utc = id_ajuste_utc;
                        this._fecha_inicio = Convert.ToDateTime(r["FechaInicio"]);
                        this._fecha_fin = Convert.ToDateTime(r["FechaFin"]);
                        this._valor_utc = Convert.ToString(r["ValorUtc"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno
                    retorno = true;
                }
            }
            //Retorno el objeto retorno al método
            return retorno;
        }

        /// <summary>
        /// Metodo encargado de editar el registro reporte unidad foranea
        /// </summary>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="valor_utc"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAjusteUtc(DateTime fecha_inicio, DateTime fecha_fin, string valor_utc, int id_usuario, bool habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializando arreglo de parámetros
            object[] param = { 2, id_ajuste_utc, fecha_inicio, fecha_fin, valor_utc, id_usuario, habilitar, "", "" };
            //Realizando actualizacion
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado obtenido
            return resultado;
        }
        #endregion

        #region Metodos públicos
        /// <summary>
        /// Metodo encargado de insertar un registro orden actividad
        /// </summary>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="valor_utc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion InsertaAjusteUtc(DateTime fecha_inicio, DateTime fecha_fin, string valor_utc, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Creación del arreglo param
            object[] param = { 1, 0, fecha_inicio, fecha_fin, valor_utc, id_usuario, true, "", "" };
            //Asigna valores al objeto retorno
            resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Retorno el resultado al objeto retorno
            return resultado;
        }

        /// <summary>
        /// Método que actualiza los campos de un registro
        /// </summary>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <param name="valor_utc"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAjusteUtc(DateTime fecha_inicio, DateTime fecha_fin, string valor_utc, int id_usuario, bool habilitar)
        {
            //Objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Editar mediante el método privado
            resultado = this.editaAjusteUtc(fecha_inicio, fecha_fin, valor_utc, id_usuario, habilitar);
            //Devolver resultado
            return resultado;
        }
        /// <summary>
        /// Método que realiza una busqueda si un registro de actualizó correctamente
        /// </summary>
        /// <param name="id_ajuste_utc">Identificador principal para la busqueda de la actualizacion sobre los registros </param>
        /// <returns></returns>
        public bool ActualizaAjusteUtc()
        {
            return this.cargaAtributos(this._id_ajuste_utc);
        }

        /// <summary>
        /// Método encargado de editar el atributo "Habilitar" del registro, manteniendo el valor del resto de los atributos
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarAjusteUtc(int id_usuario)
        {
            //Objeto retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Editar mediante el método privado
            resultado = this.editaAjusteUtc(this._fecha_inicio, this._fecha_fin, this._valor_utc, id_usuario, false);
            //Devolver resultado
            return resultado;
        }
        #endregion
    }
}
