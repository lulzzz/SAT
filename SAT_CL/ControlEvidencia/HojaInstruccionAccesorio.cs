using System;
using TSDK.Base;
using TSDK.Datos;
using System.Data;

namespace SAT_CL.ControlEvidencia
{   
    /// <summary>
    /// Clase Encargada de las Operaciones de los Accesorios de las Hojas de Instrucción
    /// </summary>
    public class HojaInstruccionAccesorio : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de almacenar el nombre del SP
        /// </summary>
        private static string _nom_sp = "control_evidencia.sp_hoja_instruccion_accesorio_thia";
        
        private int _id_hoja_instruccion_accesorio;
        /// <summary>
        /// Atributo encargado de almacenar el Id de la Hoja de Instrucción del Accesorio
        /// </summary>
        public int id_hoja_instruccion_accesorio
        {
            get { return this._id_hoja_instruccion_accesorio; }
        }

        private int _id_hoja_instruccion;
        /// <summary>
        /// Obtiene el Id de HI
        /// </summary>
        public int id_hoja_instruccion
        {
            get { return this._id_hoja_instruccion; }
        }
        private int _id_accesorio;
        /// <summary>
        /// Atributo encargado de almacenar el Id del Accesorio
        /// </summary>
        public int id_accesorio
        {
            get { return this._id_accesorio; }
        }

        private int _id_tipo_evento;
        /// <summary>
        /// Atributo encargado de almacenar el Tipo del Evento
        /// </summary>
        public int id_tipo_evento
        {
            get { return this._id_tipo_evento; }
        }

        private string _observacion;
        /// <summary>
        /// Atributo encargado de almacenar las Observaciones
        /// </summary>
        public string observacion
        {
            get { return this._observacion; }
        }

        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de almacenar el Estatus Habilitar
        /// </summary>
        public bool habilitar
        {
            get { return this._habilitar; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos pro Default
        /// </summary>
        public HojaInstruccionAccesorio()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor de la Clase encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        public HojaInstruccionAccesorio(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~HojaInstruccionAccesorio()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {   //Inicializando Valores
            this._id_hoja_instruccion_accesorio = 0;
            this._id_hoja_instruccion = 0;
            this._id_accesorio = 0;
            this._id_tipo_evento = 0;
            this._observacion = "";
            this._habilitar = false;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Id
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando variable de Retorno
            bool result = false;
            //Armando Objeto de Parametros
            object[] param = { 3,id_registro, 0,0,0,"",0,false,"","" };
            //Obteniendo Tabla de Resultado del SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp,param))
            {   //Validando que la Tabla contenga Registros
                if(Validacion.ValidaOrigenDatos(ds,"Table"))
                {   //Recorriendo cada fila de la Tabla
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Inicializando Valores
                        this._id_hoja_instruccion_accesorio = id_registro;
                        this._id_hoja_instruccion = Convert.ToInt32(dr["IdHojaInstruccion"]);
                        this._id_accesorio = Convert.ToInt32(dr["IdAccesorio"]);
                        this._id_tipo_evento = Convert.ToInt32(dr["IdTipoEvento"]);
                        this._observacion = dr["Observaciones"].ToString();
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }//Asignando Positiva la variable de Retorno
                    result = true;
                }
            }//Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar los Valores en BD
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de Hoja de Instrucción</param>
        /// <param name="id_accesorio">Id del Accesorio</param>
        /// <param name="id_tipo_evento">Id del Tipo de Evento</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_hoja_instruccion, int id_accesorio, int id_tipo_evento, 
                                     string observacion, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 2, this._id_hoja_instruccion_accesorio, id_hoja_instruccion, id_accesorio, id_tipo_evento, 
                                 observacion, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Variable de Retorno
            return result;
        }


        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método Público encargado de Insertar registros
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de Hoja de Instrucción</param>
        /// <param name="id_accesorio">Id del Accesorio</param>
        /// <param name="id_tipo_evento">Id del Tipo de Evento</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarHojaInstruccionAccesorio(int id_hoja_instruccion, int id_accesorio, int id_tipo_evento,
                                     string observacion, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Objeto de Parametros
            object[] param = { 1,0, id_hoja_instruccion, id_accesorio, id_tipo_evento, 
                                 observacion, id_usuario, true, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Variable de Retorno
            return result;
        }

        /// <summary>
        /// Método Público encargado de Editar registros
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de Hoja de Instrucción</param>
        /// <param name="id_accesorio">Id del Accesorio</param>
        /// <param name="id_tipo_evento">Id del Tipo de Evento</param>
        /// <param name="observacion">Observación</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditarHojaInstruccionAccesorio(int id_hoja_instruccion, int id_accesorio, int id_tipo_evento,
                                     string observacion, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_hoja_instruccion, id_accesorio, id_tipo_evento,
                                 observacion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar los Registros
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaHojaInstruccionAccesorio(int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_hoja_instruccion, id_accesorio, id_tipo_evento,
                                 observacion, id_usuario, false);
        }

        /// <summary>
        /// Deshabilita todos los accesorios ligados a la HI indicada
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de Hoja de Instrucciones</param>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaHojaInstruccionAccesorios(int id_hoja_instruccion, int id_usuario)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_hoja_instruccion);

            //Cargando documentos
            using (DataTable mitDoc = ObtieneHojaInstruccionAccesorios(id_hoja_instruccion))
            {
                //Si existen resultados
                if (Validacion.ValidaOrigenDatos(mitDoc))
                {
                    //Para cada uno de los registros
                    foreach (DataRow d in mitDoc.Rows)
                    {
                        //instanciando registro
                        using (HojaInstruccionAccesorio hia = new HojaInstruccionAccesorio(Convert.ToInt32(d["Id"])))
                        {
                            //Si el registro existe
                            if (hia.id_hoja_instruccion_accesorio > 0)
                                //Deshabilitando Documento
                                resultado = hia.DeshabilitaHojaInstruccionAccesorio(id_usuario);
                            else
                                resultado = new RetornoOperacion(string.Format("Accesorio 'ID: {0}' no encontrado.", Convert.ToInt32(d["Id"])));
                        }

                        //Si existe error
                        if (!resultado.OperacionExitosa)
                            //Saliendo de ciclo
                            break;
                    }
                }
            }


            //Devolviendo resultado
            return resultado;
        }
    
        /// <summary>
        /// Realiza la carga de todos los accesorios ligados a un Id de Hoja de Instrucciones
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de HI</param>
        /// <returns></returns>
        public static DataTable ObtieneHojaInstruccionAccesorios(int id_hoja_instruccion)
        {
            //Definiendo objeto de resultado
            DataTable mit = null;

            //Declarando arreglo de parámetros de consulta
            object[] param = { 4, 0, id_hoja_instruccion, 0, 0, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }          
        }

        /// <summary>
        /// Realiza la carga de todos los accesorios ligados a un Id de Hoja de Instrucciones
        /// </summary>
        /// <param name="id_hoja_instruccion">Id de HI</param>
        /// <returns></returns>
        public static DataTable ObtieneHojaInstruccionAccesoriosParaImpresion(int id_hoja_instruccion)
        {
            //Declarando arreglo de parámetros de consulta
            object[] param = { 5, 0, id_hoja_instruccion, 0, 0, "", 0, false, "", "" };

            //Realizando carga de registros
            using (DataTable mit = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param).Tables[0])
            {
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método Público encargado de Actualizar los Atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaHojaInstruccionAccesorio()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_hoja_instruccion_accesorio);
        }


        #endregion

    }
}
