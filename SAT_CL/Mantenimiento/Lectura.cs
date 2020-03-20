using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;
using System.Data;
using System.Configuration;

namespace SAT_CL.Mantenimiento
{
    /// <summary>
    /// Clase que permite la inserción, actualización, y deshabilitar registros de la tabla Lectura
    /// </summary>
    public class Lectura : Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp para la realización de transacciones de la tabla Lectura
        /// </summary>
        private static string nom_sp = "mantenimiento.sp_lectura_tl";
        private int _id_lectura;
        /// <summary>
        /// Permite almacenar el identificador de una lectura a una unidad de transporte
        /// </summary>
        public int id_lectura
        {
            get { return _id_lectura; }
        }
        private DateTime _fecha_lectura;
        /// <summary>
        /// Permite almacenar la fecha en la que se realizo la lectura de una unidad de transporte
        /// </summary>
        public DateTime fecha_lectura
        {
            get { return _fecha_lectura; }
        }
        private int _id_unidad;
        /// <summary>
        /// Permite almacenar el identificador de una unidad de transporte
        /// </summary>
        public int id_unidad
        {
            get { return _id_unidad; }
        }
        private int _id_operador;
        /// <summary>
        /// Permite almacenar el identificador de un operador 
        /// </summary>
        public int id_operador
        {
            get { return _id_operador; }
        }
        private string _identificador_operador_unidad;
        /// <summary>
        /// Permite almacenar un identificador de un terciario(Cuando no exista registro del operador o la unidad)
        /// </summary>
        public string identificador_operador_unidad
        {
            get { return _identificador_operador_unidad; }
        }
        private decimal _kms_sistema;
        /// <summary>
        /// Permite almacenar los kilometros registrados en el sistema
        /// </summary>
        public decimal kms_sistema
        {
            get { return _kms_sistema; }
        }
        private decimal _kms_lectura;
        /// <summary>
        /// Permite almacenar los kilometros registrados por un odometro.
        /// </summary>
        public decimal kms_lectura
        {
            get { return _kms_lectura; }
        }
        private int _horas_lectura;
        /// <summary>
        /// Almacena las horas registradas por un odometro.
        /// </summary>
        public int horas_lectura
        {
            get { return _horas_lectura; }
        }
        private decimal _litros_lectura;
        /// <summary>
        /// Permite almacenar los litros registrados por un odometro.
        /// </summary>
        public decimal litros_lectura
        {
            get { return _litros_lectura; }
        }
        private string _referencia;
        /// <summary>
        /// Permite almacenar referencias sobre la lectura de la unidad de transporte
        /// </summary>
        public string referencia
        {
            get { return _referencia; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite almacenar el estado Habilitado/Deshabilitado de un regsitro.
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor default que inicializa los atributos a 0
        /// </summary>
        public Lectura()
        {
            this._id_lectura = 0;
            this._fecha_lectura = DateTime.MinValue;
            this._id_unidad = 0;
            this._id_operador = 0;
            this._identificador_operador_unidad = "";
            this._kms_sistema =0;
            this._litros_lectura = 0;
            this._horas_lectura = 0;
            this._litros_lectura = 0;
            this._referencia = "";
            this._habilitar = false;

        }
        /// <summary>
        /// Constructor que inicializa el valor de los atributos a partir de un id de referencia.
        /// </summary>
        /// <param name="id_lectura">Id que sirve como referencia al registro de busqueda.</param>
        public Lectura(int id_lectura)
        {
            //Invoca al método cargaAtributos().
            cargaAtributos(id_lectura);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase Lectura
        /// </summary>
        ~Lectura()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Prvados
        /// <summary>
        /// Método que realiza la busqueda de un regsitro y lo asigna a los atributos
        /// </summary>
        /// <param name="id_lectura">Id que sirve como referencia para la busqueda de registros Lectura</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_lectura)
        {
            //creación del onjeto retorno
            bool retorno = false;
            //Creación del arreglo de tipo objeto, con los parametros necesarios para consultar a la base de datos y estraer el registro requerido.
            object[] param = { 3, id_lectura, null, 0, 0, "", 0, 0, 0, 0, "", 0, false, "", "" };
            //Invoca y asigna los valores del arreglo y el atributo con el nombre del sp al metodo encargado de realizar las transacciones a la base de datos
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida que existan los datos almacenados en el dataset y que no sean nulos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre cada fila del dataset y asigna a los atributos el valor de variable r con los datos encontrados
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_lectura = id_lectura;
                        this._fecha_lectura = Convert.ToDateTime(r["FechaLectura"]);
                        this._id_unidad = Convert.ToInt32(r["IdUnidad"]);
                        this._id_operador = Convert.ToInt32(r["IdOperador"]);
                        this._identificador_operador_unidad = Convert.ToString(r["IdentificadorOperadorUnidad"]);
                        this._kms_sistema = Convert.ToDecimal(r["KmsSistema"]);
                        this._kms_lectura = Convert.ToDecimal(r["KmsLectura"]);
                        this._horas_lectura = Convert.ToInt32(r["HorasLectura"]);
                        this._litros_lectura = Convert.ToDecimal(r["LitrosLectura"]);
                        this._referencia = Convert.ToString(r["Referencia"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor del objeto retorno siempre y cuando se cumpla la validación de los datos.
                    retorno = true;
                }
            }
            //Retornal el objeto retorno al método
            return retorno;
        }
        /// <summary>
        /// Métoo que realiza la actualización de los campos de la tabla Lectura
        /// </summary>
        /// <param name="fecha_lectura">Permite la actualizar la fecha de lectura de unidad de transporte</param>
        /// <param name="id_unidad">Permite actualizar el identficador de una unidad de transporte</param>
        /// <param name="id_operador">Permite actualizar el identificador del operador</param>
        /// <param name="identificador_operador_unidad">Permite actualizar un identificador cuando no existen registro sobre una unidad u operador</param>
        /// <param name="kms_sistema">Permite actualizar los kms registrados en el sistema</param>
        /// <param name="kms_lectura">Permite actualizar los kms registrados por un odometro</param>
        /// <param name="horas_lectura">Permite actualizar las horas registradas por un odometro</param>
        /// <param name="litros_lectura">Permite actualizar los litros registrados por un odometro</param>
        /// <param name="referencia">Permite actualizar descripciones sobre una lectura de unidad de transporte</param>
        /// <param name="id_usuario">Permite actualizar el identificador del usuario que realizo acciones sobre el registro </param>
        /// <param name="habilitar">Permite actualizar el estado de habilitación de un registro</param>
        /// <returns></returns>
        private RetornoOperacion editarLectura(DateTime fecha_lectura, int id_unidad, int id_operador, string identificador_operador_unidad, decimal kms_sistema,
                                               decimal kms_lectura, int horas_lectura, decimal litros_lectura, string referencia, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo de tipo objeto que almacena los valores de los parametros del sp de la tabla Lectura
            object[] param ={2,this._id_lectura,fecha_lectura,id_unidad,id_operador,identificador_operador_unidad,kms_sistema,kms_lectura,horas_lectura,litros_lectura,
                                referencia, id_usuario, habilitar,"",""};
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Métodos Públicos
        /// <summary>
        /// Método que permite la inserción de registros a una Lectura de unidad de transporte
        /// </summary>
        /// <param name="fecha_lectura">Permite insertar la fecha en la que se realizo la lectura de una unidad de transporte</param>
        /// <param name="id_unidad">Permite insertar la unidad a la cual se realizo la lectura </param>
        /// <param name="id_operador">Permite insertar al operador de la unidad de transporte</param>
        /// <param name="identificador_operador_unidad">Permite insrtar un identificador de una unidad u operador si no existe registros de ellos</param>
        /// <param name="kms_sistema">Permite insertar kms asignados por el sistema</param>
        /// <param name="kms_lectura">Permite insertar kml obtenidos por un odometro</param>
        /// <param name="horas_lectura">Permite insertar las horas obtenidas por un odometro</param>
        /// <param name="litros_lectura">Permite insertar litros obtenidos por un odometro</param>
        /// <param name="referencia">Permite insertar referencias de una lectura </param>
        /// <param name="id_usuario">Permite insertar al usuario que realizo acciones sobre el registro.</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarLectura(DateTime fecha_lectura, int id_unidad, int id_operador, string identificador_operador_unidad, decimal kms_sistema,
                                                       decimal kms_lectura, int horas_lectura, decimal litros_lectura, string referencia, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo de tipo objeto que almacena los valores de los parametros del sp de la tabla Lectura
            object[] param ={1,0,fecha_lectura,id_unidad,id_operador,identificador_operador_unidad,kms_sistema,kms_lectura,horas_lectura,litros_lectura,
                                referencia, id_usuario, true,"",""};
            //Asigna al objeto retorno el arreglo y el atributo con el nombre del sp, necesarios para hacer la transacciones a la base de datos.
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Métoo que realiza la actualización de los campos de la tabla Lectura
        /// </summary>
        /// <param name="fecha_lectura">Permite la actualizar la fecha de lectura de unidad de transporte</param>
        /// <param name="id_unidad">Permite actualizar el identficador de una unidad de transporte</param>
        /// <param name="id_operador">Permite actualizar el identificador del operador</param>
        /// <param name="identificador_operador_unidad">Permite actualizar un identificador cuando no existen registro sobre una unidad u operador</param>
        /// <param name="kms_sistema">Permite actualizar los kms registrados en el sistema</param>
        /// <param name="kms_lectura">Permite actualizar los kms registrados por un odometro</param>
        /// <param name="horas_lectura">Permite actualizar las horas registradas por un odometro</param>
        /// <param name="litros_lectura">Permite actualizar los litros registrados por un odometro</param>
        /// <param name="referencia">Permite actualizar descripciones sobre una lectura de unidad de transporte</param>
        /// <param name="id_usuario">Permite actualizar el identificador del usuario que realizo acciones sobre el registro </param>
        public RetornoOperacion EditarLectura(DateTime fecha_lectura, int id_unidad, int id_operador, string identificador_operador_unidad, decimal kms_sistema,
                                                       decimal kms_lectura, int horas_lectura, decimal litros_lectura, string referencia, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();

            //Validando Importación del Archivo
            if (!this._referencia.Contains("Importado desde Archivo"))

                //Invoca y retorna el resultado al método editarLectura
                retorno = this.editarLectura(fecha_lectura, id_unidad, id_operador, identificador_operador_unidad, kms_sistema, kms_lectura, horas_lectura, litros_lectura, referencia, id_usuario, this._habilitar);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("La Lectura viene de un archivo de importación, imposible su modificación");
            
            //Devolviendo Resultado
            return retorno;
        }
        /// <summary>
        /// Método que permite cambiar el estado de un registro de habilitado a deshabilitado
        /// </summary>
        /// <param name="id_usuario">Id que permite identificar al usuario que realizo el cambio de estado del registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarLectura(int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            
            //Validando Importación del Archivo
            if (!this._referencia.Contains("Importado desde Archivo"))

                //Invoca y retorna el resultado al método editarLectura
                retorno = this.editarLectura(this.fecha_lectura, this.id_unidad, this.id_operador, this.identificador_operador_unidad, this.kms_sistema, this.kms_lectura, this.horas_lectura, this.litros_lectura, this.referencia, id_usuario, false);
            else
                //Instanciando Excepción
                retorno = new RetornoOperacion("La Lectura viene de un archivo de importación, imposible su eliminación");

            //Devolviendo Resultado
            return retorno;
        }

        /// <summary>
        /// Carga Historial de Lectura
        /// </summary>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="fecha_inicio">Fecha de Inicio de la Lectura</param>
        /// <param name="fecha_fin">Fecha de Fin de la Lectura</param>
        /// <returns></returns>
        public static DataTable CargaHistorialLectura(int id_unidad, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Creación del arreglo de tipo objeto, con los parametros necesarios para consultar a la base de datos y estraer el registro requerido.
            object[] param = { 4, 0, null, id_unidad, 0, "", 0, 0, 0, 0, "", 0, false, fecha_inicio.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]), fecha_fin.ToString(ConfigurationManager.AppSettings["FormatoFechaReportes"]) };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
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
