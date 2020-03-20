using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using System.Data;
namespace SAT_CL.Seguridad
{
   public class RegistroUsuario : Disposable
    {

        #region Atributos
        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
            private static string _nom_sp = "seguridad.sp_registro_usuario_tru";

        ///<summary>
        /// Atributo encargado de Almacenar la relacion entre el registro y el usuario
        /// </summary>
            private int _id_registro_usuario;
            public  int id_registro_usuario { get { return this._id_registro_usuario; } }

        ///<summary>
        ///Atributo encargado de Almacenar la relacion que existe entre el usuario y una entidad
        ///</summary>
            private int _id_usuario_relacion;
            public  int id_usuario_relacion { get {return this._id_usuario_relacion;}}

        ///<summary>
        ///Atributo encargado de almacenar el id de la compañia
        ///</summary>
            private int _id_compañia;
            public int id_compañia { get { return this._id_compañia; } }

        ///<summary>
        ///Atributo encargado de almacenar el id de la tabla
        ///</summary>
            private int _id_tabla;
            public int id_tabla { get { return this._id_tabla; } }

        ///<summary>
        ///Atributo encargado de almacenar el id del registro
        ///</summary>
            private int _id_registro;
            public int id_registro { get { return this._id_registro; } }

        ///<summary>
        ///Atributo encargado de almacenar el tipo de relacion
        ///</summary>
            private byte _id_tipo_relacion;
            public byte id_tipo_relacion {get {return this._id_tipo_relacion;}}

        ///<summary>
        ///Atributo encargado de almacenar el estado del registro
        ///</summary>
            private Boolean _habilitar;
            public Boolean habilitar { get { return this._habilitar; } }


        #endregion

        #region Constructor
            public RegistroUsuario()
            {
                this._id_registro_usuario = 0;
                this._id_usuario_relacion = 0;
                this._id_compañia = 0;
                this._id_tabla = 0;
                this._id_registro = 0;
                this._id_tipo_relacion = 0;
                this._habilitar = false;
            }

        public RegistroUsuario(int id_registro_usuario)
            {
                cargaAtributosInstancia(id_registro_usuario);
            }

        #endregion 

        #region Destructor

        ///<summary>
        ///Destructor de la clase
        ///</summary>

        ~RegistroUsuario()
        {
            Dispose(false);
        }

        #endregion 

        #region Metodos Privados

        /// <summary>
        /// Método encargado de Actualizar los Registros en BD
        /// </summary>
        private RetornoOperacion actualizaRegistroBD (int id_usuario_relacion, int id_compañia, int id_tabla, int id_registro,
                                                      byte id_tipo_relacion, int id_usuario, Boolean habilitar)
        {
            //Declarando objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando arreglo de parametros
            object[] param = { 2, this._id_registro_usuario , id_usuario_relacion, id_compañia, id_tabla, id_registro, id_tipo_relacion, id_usuario, habilitar};

            //Ejecuta SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Mostrar Registros de una tabla dado un ID especifico
        /// </summary>
        private bool cargaAtributosInstancia(int id_registro_usuario)
        {
            //Declarando objeto de renorno
            bool result = false;

            //Armando onjeto de parametros
            object[] param = { 3, id_registro_usuario, 0, 0, 0, 0, 0, 0, false };

            //Obteniendo el resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param)){

                //Validando que exista el registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds,"Table") ){
                     
                    //Recorriendo el registro
                     foreach (DataRow dr in ds.Tables["Table"].Rows){

                         //Asignando Valores 
                            this._id_registro_usuario = id_registro_usuario;
                            this._id_usuario_relacion = Convert.ToInt32(dr["IdUsuarioRelacion"]);
                            this._id_compañia = Convert.ToInt32(dr["IdCompañia"]);
                            this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                            this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                            this._id_tipo_relacion = Convert.ToByte(dr["IdTipoRelacion"]);
                            this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                     }

                    //Asignando resultado positivo 
                    result = true;
                }
            }
            return result;
        }
        

        #endregion

        #region Metodos Publicos 
           
        /// <summary>
        /// Método encargado de Insertar los Registros en BD
        /// </summary>
        /// 

        public static RetornoOperacion InsertaProcesoDias(int id_usuario_relacion, int id_compañia, int id_tabla, int id_registro, Byte id_tipo_relacion,
                                                   int id_usuario)
        {
            //Declarando objeto de retorno 
            RetornoOperacion result = new RetornoOperacion();

            //Armando arreglo de parametros 
            object[] param = {1,0,id_usuario_relacion,id_compañia,id_tabla,id_registro,id_tipo_relacion,id_usuario, true};

            //Ejecutando SP 
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            
            //Devolviendo resultado obtenido
            return result;

        }

        /// <summary>
        /// Método encargado de Deshabilitar un registro en BD
        /// </summary>
        /// 
        public RetornoOperacion deshabilitaRegistroUsuario(int id_usuario)
        {
            return actualizaRegistroBD(this.id_usuario_relacion, this._id_compañia, this._id_tabla, this._id_registro, this._id_tipo_relacion, id_usuario, false);
        }
       

        /// <summary>
        /// Método encargado de Actualizar un registro en BD
        /// </summary>
        /// 

        public RetornoOperacion ActualizaRegistroBD(int id_usuario_relacion, int id_compañia, int id_tabla, int id_registro,
                                                      byte id_tipo_relacion, int id_usuario)
        {
            return actualizaRegistroBD(id_usuario_relacion, id_compañia, id_tabla, id_registro, id_tipo_relacion, id_usuario, this._habilitar);
        }

        #endregion
    }
}
