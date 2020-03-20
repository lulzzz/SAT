using System;
using System.Data;
using TSDK.Base;

namespace SAT_CL.Bancos
{
    /// <summary>
    /// Clase de la tabla egreso_ingreso_concepto que permite crear operaciones sobre la tabla (inserciones,actualizaciones,consultas,etc.).
    /// </summary>
    public class EgresoIngresoConcepto : Disposable
    {
        #region Enumeracion

        /// <summary>
        /// Permite enumerar los tipos de operación (egreso,ingreso)
        /// </summary>
        public enum TipoOperacion
        {
            /// <summary>
            /// Permite saber si el registro es un egreso
            /// </summary>
            Egreso = 1,

            /// <summary>
            /// Permite saber si un registro es un ingreso
            /// </summary>
            Ingreso = 2
        };


        #endregion

        #region Atributos
        /// <summary>
        /// Atributo que alamcena el nombre del store procedure de la tabla. 
        /// </summary>
        private static string nom_sp = "bancos.sp_egreso_ingreso_concepto_teic";


        private int _id_egreso_ingreso_concepto;
        /// <summary>
        /// Id que corresponde al identificador del concepto 
        /// </summary>
        public int id_egreso_ingreso_concepto
        {
            get { return _id_egreso_ingreso_concepto; }

        }

        private byte _id_tipo_operacion;
        /// <summary>
        /// Id que permite saber el tipo de operacion registrada (Egreso - Ingreso)
        /// </summary>
        public byte id_tipo_operacion
        {
            get { return _id_tipo_operacion; }
        }

        /// <summary>
        /// Permite acceder a los elementos de la enumeración TipoOperacion
        /// </summary>
        public TipoOperacion tipo_operacion
        {
            get { return (TipoOperacion)this._id_tipo_operacion; }
        }


        private string _descripcion_concepto;
        /// <summary>
        /// Describe el concepto de un egreso o ingreso
        /// </summary>
        public string descripcion_concepto
        {
            get { return _descripcion_concepto; }
        }

        private int _id_compania;
        /// <summary>
        /// Id que identifica a una compañia 
        /// </summary>
        public int id_compania
        {
            get { return _id_compania; }
        }

        private int _id_departamento;
        /// <summary>
        /// Id que identifica un departamento
        /// </summary>
        public int id_departamento
        {
            get { return _id_departamento; }
        }
        private bool _habilitar;
        /// <summary>
        /// Permite Establecer el estado de habilitación de un registro de EgresoIngresoConcepto
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Cosntructor por default que inicializa los atributos
        /// </summary>
        public EgresoIngresoConcepto()
        {
            this._id_egreso_ingreso_concepto = 0;
            this._id_tipo_operacion = 0;
            this._descripcion_concepto = "";
            this._id_compania = 0;
            this._id_departamento = 0;
            this._habilitar = false;
        }

        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro
        /// </summary>
        /// <param name="id_egreso_ingreso_concepto">Id que sirve como referencia para inicializar los atributos</param>
        public EgresoIngresoConcepto(int id_egreso_ingreso_concepto)
        {
            //Invoca al método privado que carga atributos.
            cargaAtributoInstancia(id_egreso_ingreso_concepto);
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase.
        /// </summary>
        ~EgresoIngresoConcepto()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que permite la carga de registros a los atributos a través de un id de busqueda.
        /// </summary>
        /// <param name="id_egreso_ingreso_concepto">Id que permite la busqueda de registros de EgresoIngresoConcepto</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_egreso_ingreso_concepto)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación y Asignación de valores del arreglo necesarios para el Sp de la Tabla.
            object[] param = { 3, id_egreso_ingreso_concepto, 0, "", 0, 0, 0, false, "", "" };
            //Invoca al SP de la tabla
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación del los datos en la tabla (Que existan y que no sean nulos)
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y almacena registros en la variable r encontrados en el dataset
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_egreso_ingreso_concepto = id_egreso_ingreso_concepto;
                        this._id_tipo_operacion = Convert.ToByte(r["IdTipoOperacion"]);
                        this._descripcion_concepto = Convert.ToString(r["DescripcionConcepto"]);
                        this._id_compania = Convert.ToInt32(r["IdCompania"]);
                        this._id_departamento = Convert.ToInt32(r["IdDepartamento"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor a retorno siempre y cuando se cumpla la validación de datos
                    retorno = true;
                }
            }
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que permite actualizar registros de EgresoIngresoconcepto
        /// </summary>
        /// <param name="descripcion_concepto">Permite actualizar el campo descricion_concepto</param>
        /// <param name="id_compania">Permite actualizar el campo id_compania</param>
        /// <param name="id_departamento">Permite actualizar el campo id_departamento</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar</param>
        /// <returns></returns>
        private RetornoOperacion editarEgresoIngresoConcepto(TipoOperacion tipo_operacion, string descripcion_concepto, int id_compania, int id_departamento, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al arreglo necesarios para el sp de la tabla
            object[] param = { 2, this.id_egreso_ingreso_concepto, (byte)tipo_operacion, descripcion_concepto, id_compania, id_departamento, id_usuario, habilitar, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que permite insertar registros a EgresoIngresoConcepto
        /// </summary>
        /// <param name="descripcion_concepto">Permite insertar un concepto de un egreso o ingreso</param>
        /// <param name="id_compania">Permite insertar el identifiador de una compania en EgresoIngresoConcepto</param>
        /// <param name="id_departamento">Permite insertar el identifiador de un departamento en EgresoIngresoConcepto</param>
        /// <param name="id_usuario">Permite insertar al usuario que realizo acciónes sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarEgresoIngresoConcepto(TipoOperacion tipo_operacion, string descripcion_concepto, int id_compania, 
                                                                     int id_departamento, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y asignacion de valores del arreglo, necesarios para el sp de la tabla
            object[] param = { 1, 0, (byte)tipo_operacion, descripcion_concepto, id_compania,id_departamento, id_usuario, true, "", "" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorno del resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que Actualiza registros de ficha Conceto
        /// </summary>
        /// <param name="descripcion_concepto">Permite actualizar el campo descripcion_concepto </param>
        /// <param name="id_compania">Permite actualizar el campo id_compania</param>
        /// <param name="id_departamento">Permite actualizar el campo id_departamento</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarEgresoIngresoConcepto(TipoOperacion tipo_operacion, string descripcion_concepto, int id_compania, int id_departamento, 
                                                            int id_usuario)
        {
            //Retorna e invoca al método editarFichaConcepto
            return this.editarEgresoIngresoConcepto(this.tipo_operacion, descripcion_concepto, id_compania, id_departamento, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Metodó que permite cambiar el estado de un registro(Habilitado / Deshabilitado)
        /// </summary>
        /// <param name="id_usuario">Permite identificar el usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarEgresoIngresoConcepto(int id_usuario)
        {
            //Retorna e invoca al método editarFichaConcepto
            return this.editarEgresoIngresoConcepto(this.tipo_operacion, this.descripcion_concepto, this.id_compania, this.id_departamento, id_usuario, false);
        }
        #endregion

    }
}
