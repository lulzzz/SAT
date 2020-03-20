using System;
using System.Data;
using System.Linq;
using TSDK.Base;
using TSDK.Datos;
namespace SAT_CL.Nomina
{
    /// <summary>
    ///  Implementa los método para la administración  de la  Nómina del Empleado
    /// </summary> 
   public class Esquema : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "nomina.sp_esquema_te";

        private int _id_esquema;
        /// <summary>
        /// Atributo que almacena el  Id de Esquema
        /// </summary>
        public int id_esquema { get { return this._id_esquema; } }
        private string _version;
        /// <summary>
        /// Atributo que almacena la versión de la Nómina
        /// </summary>
        public string version { get { return this._version; } }
        private string _nombre;
        /// <summary>
        /// Atributo que almacena la versión del nombre
        /// </summary>
        public string nombre { get { return this._nombre; } }
        private byte _id_tipo;
        /// <summary>
        /// Atributo que almacena  el Tipo (Elemnto, Atributo)
        /// </summary>
        public byte id_tipo { get { return this._id_tipo; } }
        private byte _nivel;
        /// <summary>
        /// Atributo que almacena  el  Nivel 
        /// </summary>
        public byte nivel { get { return this._nivel; } }
        private int _id_agrupador;
        /// <summary>
        /// Atributo que almacena el Id Agrupador
        /// </summary>
        public int id_agrupador { get { return this._id_agrupador; } }
         private byte _id_tipo_dato;
        /// <summary>
        /// Atributo que almacena el Id Tipo de Datos
        /// </summary>
        public byte id_tipo_dato { get { return this._id_tipo_dato; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo que almacena el Estatus Habilitar
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }
        


        #endregion

       #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defectos
        /// </summary>
        public Esquema()
        {
            //Asignando Atributos
            this._id_esquema =  0;
            this._version =  "";
            this._nombre =  "";
            this._id_tipo =  0;
            this._id_agrupador =  0;
            this._id_tipo_dato =  0;
            this._nivel =  0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_nomina_empleado"></param>
        public Esquema(int id_esquema)
        {
            //Invocando Método de Carga
            cargaAtributosInstancia(id_esquema);
        }

       
        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Esquema()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Cargar los Atributos
        /// </summary>
        /// <param name="id_esquema">Id Esquema</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_esquema)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Armando Arreglo de Parametros
            object[] param = { 3, id_esquema, "", "", 0, 0, 0, 0, 0, false, "", ""};

            //Obteniendo resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Atributos
                        this._id_esquema = id_esquema;
                        this._version = dr["Version"].ToString();
                        this._nombre = dr["Nombre"].ToString(); 
                        this._id_tipo = Convert.ToByte(dr["IdTipo"]); 
                        this._id_agrupador = Convert.ToByte(dr["IdAgrupador"]); 
                        this._id_tipo_dato = Convert.ToByte(dr["IdTipoDato"]); 
                        this._nivel = Convert.ToByte(dr["Nivel"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }

                    //Asignando Retorno Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        
       /// <summary>
       /// Método encargado de Actualizar el Esquema
       /// </summary>
       /// <param name="version">Versión</param>
       /// <param name="nombre">Nombre</param>
       /// <param name="id_tipo">Tipo (Elemento, Atributo)</param>
       /// <param name="id_agrupador">Id Agrupador</param>
       /// <param name="id_tipo_dato">Tipo de Dato( INT, TINYINT )en Base de Datos</param>
       /// <param name="nivel">Nivel de acuerdo al Agrupador</param>
       /// <param name="id_usuario">Id Usuraio</param>
       ///<param name="habilitar">Habilitar</param>
       /// <returns></returns>
        private RetornoOperacion actualizaAtributosBD(string  version, string nombre,byte id_tipo,  int id_agrupador,  byte id_tipo_dato,byte nivel, int id_usuario, bool habilitar)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = {2, this._id_esquema, version, nombre, id_tipo, id_agrupador, id_tipo_dato, nivel, id_usuario, habilitar, "", "" };

            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Insertar el Esquema
        /// </summary>
        /// <param name="version">Versión</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="id_tipo">Tipo (Elemento, Atributo)</param>
        /// <param name="id_agrupador">Id Agrupador</param>
        /// <param name="id_tipo_dato">Tipo de Dato( INT, TINYINT )en Base de Datos</param>
        /// <param name="nivel">Nivel de acuerdo al Agrupador</param>
        /// <param name="id_usuario">Id Usuraio</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaEsquema(string version, string nombre, byte id_tipo, int id_agrupador, byte id_tipo_dato, byte nivel, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, version, nombre, id_tipo, id_agrupador, id_tipo_dato, nivel, id_usuario, true, "", "" };
            
            //Ejecutando SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
             
            //Devolviendo Resultado Obtenido
            return result;
        }
        
       /// <summary>
       /// Método encargado de Editar el Esquema
       /// </summary>
       /// <param name="version">Versión</param>
       /// <param name="nombre">Nombre</param>
       /// <param name="id_tipo">Tipo (Elemento, Atributo)</param>
       /// <param name="id_agrupador">Id Agrupador</param>
       /// <param name="id_tipo_dato">Tipo de Dato( INT, TINYINT )en Base de Datos</param>
       /// <param name="nivel">Nivel de acuerdo al Agrupador</param>
       /// <param name="id_usuario">Id Usuraio</param>
       /// <returns></returns>
        public RetornoOperacion EditaEsquema(string version, string nombre, byte id_tipo, int id_agrupador, byte id_tipo_dato, byte nivel, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();


                //Devolviendo Resultado Obtenido
                result = this.actualizaAtributosBD(version, nombre, id_tipo, id_agrupador, id_tipo_dato, nivel, id_usuario,this._habilitar);


            //Devolviendo Resultado Obtenido
            return result;
        }
        
        /// <summary>
        /// Método encargado de Actualizar los Valores
        /// </summary>
        /// <returns></returns>
        public bool ActualizaEsquema()
        {
            //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_esquema);
        }

        /// <summary>
        /// Método encargado de Deshabilitar un Esquema
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaEsquema( int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();


            //Devolviendo Resultado Obtenido
            result = this.actualizaAtributosBD(this._version, this._nombre, this._id_tipo, this._id_agrupador, this._id_tipo_dato, this._nivel, id_usuario, false);


            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Obtener un Id de Esquema
        /// </summary>
        /// <param name="version">Versión del esquema</param>
        /// <param name="nombre_atributo">Nombre del Atributo a Consultadr</param>
        /// <param name="nombre_sub_elemento">Elemnto donde se encuentra el atributo</param>
        /// <param name="nombre_elemento_principal">Elemento princiapal</param>
        /// <returns></returns>
        public static int ObtieneIdEsquema(string version, string nombre_atributo, string nombre_sub_elemento, string nombre_elemento_principal)
        {
            //Declarando Objeto de Retorno
            int id_esquema = 0;

            //Armando Arreglo de Parametros
            object[] param = { 4, 0, version, nombre_atributo, 0, 0, 0, 0, 0, true, nombre_elemento_principal, nombre_sub_elemento };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                     //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Obtenemos Validacion
                    id_esquema = (from DataRow r in ds.Tables[0].Rows
                                    select Convert.ToInt32(r["IdEsquema"])).DefaultIfEmpty().FirstOrDefault();

                }
            }
            

            //Devolviendo Resultado Obtenido
            return id_esquema;
        }

        /// <summary>
        /// Método encargado de Obtener un Id de Esquema
        /// </summary>
        /// <param name="version">Versión del esquema</param>
        /// <param name="nombre_atributo">Nombre del Atributo a Consultadr</param>
        /// <param name="nombre_elemento_principal">Elemento princiapal</param>
        /// <returns></returns>
        public static int ObtieneIdEsquema(string version, string nombre_atributo, string nombre_elemento_principal)
        {
            //Declarando Objeto de Retorno
            int id_esquema = 0;

            //Armando Arreglo de Parametros
            object[] param = { 5, 0, version, nombre_atributo, 0, 0, 0, 0, 0, true, nombre_elemento_principal, "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        id_esquema = (from DataRow r in ds.Tables[0].Rows
                                      select Convert.ToInt32(r["IdEsquema"])).DefaultIfEmpty().FirstOrDefault();

                    }
            }


            //Devolviendo Resultado Obtenido
            return id_esquema;
        }



        /// <summary>
        /// Método encargado de Obtener un Id de Esquema
        /// </summary>
        /// <param name="version">Versión de la Nómina</param>
        /// <param name="id_tipo">Tipo(Elemento/Atributo)</param>
        /// <param name="id_agrupador">Id Agrupador</param>
        /// <returns></returns>
        public static int  ObtieneIdEsquema(string version, int id_tipo, int id_agrupador)
        {
            //Declarando Objeto de Retorno
            int id_esquema = 0;

            //Armando Arreglo de Parametros
            object[] param = { 6, 0, version, 0, id_tipo, id_agrupador, 0, 0, 0, true, "", "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        id_esquema = (from DataRow r in ds.Tables[0].Rows
                                      select Convert.ToInt32(r["IdEsquema"])).DefaultIfEmpty().FirstOrDefault();

                    }
            }


            //Devolviendo Resultado Obtenido
            return id_esquema;
        }

        /// <summary>
        /// Obtiene los Atributos de acuerdo al un Elemneto Agrupador.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id_agrupador"></param>
        /// <param name="id_empleado"></param>
        /// <param name="id_grupo_superior"></param>
        /// <returns></returns>
        public static DataTable ObtieneAtributos(string version,int id_agrupador, int id_empleado, int id_grupo_superior)
        {
            //Declarando Objeto de Retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 7, 0, version, 0, 0, id_agrupador, 0, 0, 0, true, id_empleado, id_grupo_superior };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        mit = ds.Tables[0];

                    }
            }


            //Devolviendo Resultado Obtenido
            return mit;
        }

        /// <summary>
        /// Método encargado de Obtener un Id de Esquema
        /// </summary>
        /// <param name="version">Versión de la Nómina</param>
        /// <param name="id_agrupador">Id Agrupador</param>
        /// <param name="id_empleado"></param>
        /// <returns></returns>
        public static DataTable ObtieneElementos(string version, int id_agrupador, int id_empleado)
        {
            //Declarando Objeto de Retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 8, 0, version, 0, 0, id_agrupador, 0, 0, 0, true, id_empleado, "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        mit = ds.Tables[0];

                    }
            }


            //Devolviendo Resultado Obtenido
            return mit;
        }

   

        /// <summary>
        /// Obtenemos Ultimo Elemento
        /// </summary>
        /// <param name="version">Versión de Nómina</param>
        /// <param name="id_grupo">Id Grupo</param>
        /// <returns></returns>
        public static RetornoOperacion ObtieneUltimoElemento(string version, int id_grupo, int id_nomina_empleado, int id_esquema_superior)
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
           
            //Si Existen Atributos
            using(DataTable mit = ObtieneAtributosRegistrados(version,id_grupo,id_nomina_empleado, id_esquema_superior))
            {
                //Validamos Si No Existen Atributos
                if(!Validacion.ValidaOrigenDatos(mit))
                {
                    //Declaramos Ultimo Elemnto Grupo
                    int id_ultimo = id_grupo;
                    //Obtenemos Id de Esquema
                    int id_esquema = Esquema.ObtieneIdEsquema(version, 1, id_grupo);


                    //Validamos Exista Esquema
                    if (id_esquema != 0)
                    {
                          //Si existen Elemento
                        using (DataTable elementos = Esquema.ObtieneElementos(version, id_grupo, id_nomina_empleado))
                        {
                            //Validamos Origen de Datos
                            if (Validacion.ValidaOrigenDatos(elementos))
                            {
                                //Recorremos cada uno de los Elemntos
                                foreach (DataRow r in elementos.Rows)
                                {

                                    //Obtenemos Ultimo Elemento
                                    resultado = ObtieneUltimoElemento(version, id_esquema, id_nomina_empleado, r.Field<int>("IdEsquemaRegistro"));

                                    //Si el Resultado es Exitoso
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Salimos del ciclo
                                        break;
                                    }
                                }
                            }
                        }


                    }
                }
                else
                {
                    //Asignamos Valor Exitoso
                    resultado = new RetornoOperacion(0);
                }
            }                      
            //Devolvemos Valor
            return resultado;
        }

        /// <summary>
        /// Obtiene los Atributos Registrado a partir de un Elemento Superior
        /// </summary>
        /// <param name="version">Versión de la Nómina</param>
        /// <param name="id_agrupador">Id Agrupador</param>
        /// <param name="id_empleado">Id del Empleado</param>
        /// <param name="id_grupo_superior">Id Grupo Superior</param>
        /// <returns></returns>
        public static DataTable ObtieneAtributosRegistrados(string version,int id_agrupador, int id_empleado, int id_grupo_superior)
        {
            //Declarando Objeto de Retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 10, 0, version, 0, 0, id_agrupador, 0, 0, 0, true, id_empleado, id_grupo_superior };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        mit = ds.Tables[0];

                    }
            }


            //Devolviendo Resultado Obtenido
            return mit;
        }
        /// <summary>
        /// Obtiene Atributos Registrador de acuerdo a un Agrupador.
        /// </summary>
        /// <param name="version">Versión de la Nómina</param>
        /// <param name="id_agrupador">Id Agrupador</param>
        /// <param name="id_empleado">Id del Empleado</param>
        /// <param name="id_esquema_superior"> Id del Esquema Superior</param>
        /// <returns></returns>
        public static DataTable ObtieneAtributosRegistrados(string version, int id_agrupador, int id_empleado)
        {
            //Declarando Objeto de Retorno
            DataTable mit = null;

            //Armando Arreglo de Parametros
            object[] param = { 11, 0, version, 0, 0, id_agrupador, 0, 0, 0, true, id_empleado, "" };

            //Instanciando SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    {
                        //Obtenemos Validacion
                        mit = ds.Tables[0];

                    }
            }


            //Devolviendo Resultado Obtenido
            return mit;
        }
        #endregion
        
   }
}
