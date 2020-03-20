using System;
using TSDK.Base;
using System.Data;

namespace SAT_CL.EgresoServicio
{
    /// <summary>
    /// Clase encargada de todas las operaciones de relacionadas con 
    /// </summary>
    public class ConceptoDepartamento : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo privado nom_sp con el nombre del sp_de_la_tabla
        /// </summary>
        private static string nom_sp = "egresos_servicio.sp_concepto_departamento_tcd"; 

        private int _id_concepto_departamento;
        /// <summary>
        /// Id que corresponde al concepto departamento del objeto
        /// </summary>
        public int id_concepto_departamento
        {
            get { return _id_concepto_departamento; }
        }

        private int _id_concepto;
        /// <summary>
        /// Id que corresponde al concepto departamento del objeto
        /// </summary>
        public int id_concepto
        {
            get { return _id_concepto; }
        }

        private int _id_departamento;
        /// <summary>
        /// Id que corresponde al concepto departamento del objeto
        /// </summary>
        public int id_departamento
        {
            get { return _id_departamento; }
        }
       
        private bool _habilitar;
        /// <summary>
        /// Habilitar que corresponde al concepto departamento del objeto
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Constructores
        
        /// <summary>
        /// Constructor Default que inicializa los Atributos
        /// </summary>
        public ConceptoDepartamento()
        {
            this._id_concepto_departamento = 0;
            this._id_concepto = 0;
            this._id_departamento = 0;
            this._habilitar = false;
        }
        
        /// <summary>
        /// Constructor que inicializa a los atributos dado un Id de búsqueda de registros
        /// </summary>
        /// <param name="id_concepto_departamento">Id a partir del cual se inicializara la búsqueda de registros</param>
        public ConceptoDepartamento(int id_concepto_departamento)
        {
            //Invocación del método privado Carga Atributos
            cargaAtributoInstancia(id_concepto_departamento);
        }

        #endregion
        
        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~ConceptoDepartamento()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privado

        /// <summary>
        /// Método que permite realizar actualizaciones a los registros de Concepto Departamento
        /// </summary>
        /// <param name="id_concepto">Permite actualizar el campo id_concepto </param>
        /// <param name="id_departamento">Permite actualizar el campo id_departamento</param>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario</param>
        /// <param name="habilitar">Permite actualizar el campo habilitar </param>
        /// <returns></returns>
        private RetornoOperacion editarConceptoDepartamento(int id_concepto, int id_departamento, int id_usuario, bool habilitar) 
        { 
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación de valores al Arreglos necesarios para el SP
            object[] param = { 2, this._id_concepto_departamento, id_concepto,id_departamento,id_usuario,habilitar,"",""};
            //Asignación de valores al Objeto Retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Devuelvo el resultado del método
            return retorno;
        }

        /// <summary>
        /// Método privado  que Cargar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_concepto_departamento">Id con el que se inicializara la búsqueda de los valores del objeto</param>
        /// <returns></returns>
        private bool cargaAtributoInstancia(int id_concepto_departamento)
        {
            //Declarando al objeto de retorno
            bool retorno = false;
            //Creación y Asignación del un Arreglo con los valores necesarios para el SP_de_la_tabla
            object[] param = { 3, id_concepto_departamento, 0, 0, 0, false, "", "" };
            //Invocación del Store Procedure
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Validación de los datos de la tabla DataSet con los Datos del Store Procedure
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre las Filas de la tabla DataSet y las Asigna a r
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        _id_concepto_departamento = id_concepto_departamento;
                        _id_concepto = Convert.ToInt32(r["IdConcepto"]);
                        _id_departamento = Convert.ToInt32(r["IdDepartamento"]);
                        _habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambio de valor al objeto, siempre y cuando se cumpla la sentencia
                    retorno = true;
                }
                //Retorno del resultado al método
                return retorno;
            }
        }

        #endregion

        #region Método Publico

        /// <summary>
        /// Método que inserta registro a Concepto Departamento
        /// </summary>
        /// <param name="id_concepto">Valor que se inserta en el campo id_concepto</param>
        /// <param name="departamento">Valor que se inserta en el campo id_departamento</param>
        /// <param name="id_usuario">Valor que se inserta en el campo id_usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaConceptoDepartamento(int id_concepto, int departamento, int id_usuario)
        {
            //Creación del Objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación y Asignación del Arreglo con los valores necesarios para el SP
            object[] param = {1,0,id_concepto,departamento,id_usuario,true,"","" };
            //Asignación de valores al objeto retorno
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp,param);
            //Devuelvo un resultado al método
            return retorno;
        }

        /// <summary>
        /// Método que actualiza los registros de Concepto Departamento
        /// </summary>
        /// <param name="id_concepto">Permite actalizar el campo id_concepto del registro</param>
        /// <param name="id_departamento">Permite actalizar el campo id_departamento del registro</param>
        /// <param name="id_usuario">Permite actalizar el campo id_usuario del registro</param>
        /// <returns></returns>
        public RetornoOperacion EditaConceptoDepartamento(int id_concepto, int id_departamento, int id_usuario) 
        {
            // Invoca y Retorna el resultado del método privado edita Concepto Departamento
            return this.editarConceptoDepartamento(id_concepto, id_departamento, id_usuario, this._habilitar); 
        }

        /// <summary>
        /// Método que permite deshabilitar un registro de Concepto Departamento
        /// </summary>
        /// <param name="id_usuario">Permite actualizar el campo id_usuario del registro Concepto Departamento</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaConceptoDepartamento(int id_usuario)
        {
            // Invoca y Retorna el resultado del método privado edita Concepto Departamento
            return this.editarConceptoDepartamento(this.id_concepto, this.id_departamento, id_usuario, false);
        }

        #endregion
    }
}
