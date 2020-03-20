using System;
using System.Data;
using System.Data.SqlClient;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
namespace SAT_CL.Global
{   
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Tipos de Unidades
    /// </summary>
    public class UnidadTipo : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "global.sp_unidad_tipo_tut";

        private int _id_tipo_unidad;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad
        /// </summary>
        public int id_tipo_unidad { get { return this._id_tipo_unidad; } }
        private string _descripcion_unidad;
        /// <summary>
        /// Atributo encargado de almacenar la Descripción
        /// </summary>
        public string descripcion_unidad { get { return this._descripcion_unidad; } }
        private bool _bit_motriz;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Motriz
        /// </summary>
        public bool bit_motriz { get { return this._bit_motriz; } }
        private bool _bit_permite_arrastre;
        /// <summary>
        /// Atributo encargado de almacenar el Bit Permite Arrastre
        /// </summary>
        public bool bit_permite_arrastre { get { return this._bit_permite_arrastre; } }
        private int _id_tipo_combustible;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo de Unidad comsbutible
        /// </summary>
        public int id_tipo_combustible { get { return this._id_tipo_combustible; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public UnidadTipo()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public UnidadTipo(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~UnidadTipo()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Valores por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Asignando Valores
            this._id_tipo_unidad = 0;
            this._descripcion_unidad = "";
            this._bit_motriz = false;
            this._bit_permite_arrastre = false;
            this._id_tipo_combustible = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores dado un Id de Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Objeto de parametros
            object[] param = { 3, id_registro, "", false,false, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando Origen de Datos
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada Fila
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Valores
                        this._id_tipo_unidad = id_registro;
                        this._descripcion_unidad = dr["Descripcion"].ToString();
                        this._bit_motriz = Convert.ToBoolean(dr["BitMotriz"]);
                        this._bit_permite_arrastre = Convert.ToBoolean(dr["BitPermiteArrastre"]);
                        this._id_tipo_combustible = Convert.ToInt32(dr["TipoCombustible"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
                //Asignando Resultado Positivo
                result = true;
            }
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="descripcion_unidad">Descripcion del Tipo de Unidad</param>
        /// <param name="bit_motriz">Identificador de Unidad Motriz</param>
        /// <param name="bit_permite_arrastre">Bit permite Arrastre</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(string descripcion_unidad, bool bit_motriz, bool bit_permite_arrastre, int tipo_combustible, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 2, this._id_tipo_unidad, descripcion_unidad, bit_motriz, bit_permite_arrastre, tipo_combustible, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar los Tipos de Unidades
        /// </summary>
        /// <param name="descripcion_unidad">Descripcion del Tipo de Unidad</param>
        /// <param name="bit_motriz">Identificador de Unidad Motriz</param>
        /// <param name="bit_permite_arrastre">Bit permite Arrastre</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaUnidadTipo(string descripcion_unidad, bool bit_motriz,  bool bit_permite_arrastre, int tipo_combustible, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de parametros
            object[] param = { 1, 0, descripcion_unidad, bit_motriz, bit_permite_arrastre, tipo_combustible, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Editar los Tipos de Unidades
        /// </summary>
        /// <param name="descripcion_unidad">Descripcion del Tipo de Unidad</param>
        /// <param name="bit_motriz">Identificador de Unidad Motriz</param>
        /// <param name="bit_permite_arrastre">Bit Permite Arrastre</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaUnidadTipo(string descripcion_unidad, bool bit_motriz, bool bit_permite_arrastre, int tipo_combustible, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(descripcion_unidad, bit_motriz, bit_permite_arrastre, tipo_combustible, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método Público encargado de Eliminar los Tipos de Unidades
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaUnidadTipo(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(this._descripcion_unidad, this._bit_permite_arrastre, this._bit_motriz, this._id_tipo_combustible, id_usuario, false);
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaUnidadTipo()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_tipo_unidad);
        }

        /// <summary>
        /// Regresa el Id Unidad Tipo de Acuerdo a la descripción
        /// </summary>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        public static int RegresaIdUnidadTipo(string descripcion)
        {
            //Declaramos Variable
            int Id_Unidad_Tipo  = 0;
            //Armando Objeto de parametros
            object[] parametros = {4, 0, descripcion, false, false, 0, 0, false, "", "" };
           
            //Creación del objeto retorno
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                        //Asigna a la variable retorno el resultado de las coincidencias.
                    Id_Unidad_Tipo = (from DataRow r in DS.Tables["Table"].Rows
                                     select  Convert.ToInt32(r["Id"])).FirstOrDefault(); 
                }
            }
            //Retorna el resultado al método. 
            return Id_Unidad_Tipo;
        }

        /// <summary>
        /// Regresa la Descricpión de Acuerdo a Una Unidad Tipo
        /// </summary>
        /// <param name="id_unidad_tipo">Id Unidad Tipo</param>
        /// <returns></returns>
        public static string RegresaDescripcionUnidadTipo(int id_unidad_tipo)
        {
            //Declaramos Variable
            string descripcion = "";
            //Armando Objeto de parametros
            object[] parametros = { 5, id_unidad_tipo, "", false, false, 0, 0, false, "", "" };

            //Creación del objeto retorno
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {
                //Valida que los datos existan el la tabla catálogo
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                {
                    //Asigna a la variable retorno el resultado de las coincidencias.
                    descripcion = (from DataRow r in DS.Tables["Table"].Rows
                                      select r.Field<string>("Descripcion")).FirstOrDefault();
                }
            }
            //Retorna el resultado al método. 
            return descripcion;
        }
        #endregion
    }
}
