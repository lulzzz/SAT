using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;
using System.Transactions;

namespace SAT_CL.Global
{
    /// <summary>
    /// Proporciona métodos para la administración de registros referencia
    /// </summary>
    public class Referencia : Disposable
    {
        #region Enumeraciones

        /// <summary>
        /// Define la estructura del arbol referencia
        /// </summary>
        public enum EstructuraArbolReferencia
        {
            /// <summary>
            /// 
            /// </summary>
            Arbol = 6,
            /// <summary>
            /// 
            /// </summary>
            Raiz,
            /// <summary>
            /// 
            /// </summary>
            Hoja1,
            /// <summary>
            /// 
            /// </summary>
            Hoja2,
            /// <summary>
            /// 
            /// </summary>
            Valor,
            /// <summary>
            /// 
            /// </summary>
            ValorEdicion
        }
        /// <summary>
        /// Define los errores que pueden ser disparador por la tabla
        /// </summary>
        public enum ErrorReferencia
        {
            /// <summary>
            /// 
            /// </summary>
            NodoTipoNoValido = -3,
            /// <summary>
            /// 
            /// </summary>
            Editable = -2,
            /// <summary>
            /// 
            /// </summary>
            MaximoAlcanzado = -1,
            /// <summary>
            /// 
            /// </summary>
            SinError
        }
        /// <summary>
        /// Define las acciones que pueden ser realizadas sobre las referencias
        /// </summary>
        public enum AccionReferencia
        {
            /// <summary>
            /// 
            /// </summary>
            Registrar,
            /// <summary>
            /// 
            /// </summary>
            Editar,
            /// <summary>
            /// 
            /// </summary>
            Eliminar
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Define el nombre del SP a ejecutar
        /// </summary>
        private static string _nom_sp = "global.sp_referencia_tre";

        private int _id_referencia;
        /// <summary>
        /// Define el Id de la Referencia
        /// </summary>
        public int id_referencia { get { return this._id_referencia; } }
        private int _id_registro;
        /// <summary>
        /// Define el Id del Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private int _id_tabla;
        /// <summary>
        /// Define el Id de la Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }
        private int _id_referencia_tipo;
        /// <summary>
        /// Define el Id del Tipo de Referencia
        /// </summary>
        public int id_referencia_tipo { get { return this._id_referencia_tipo; } }
        private string _valor;
        /// <summary>
        /// Define el Valor de la Referencia
        /// </summary>
        public string valor { get { return this._valor; } }
        private DateTime _fecha;
        /// <summary>
        /// Define la Fecha de la Referencia
        /// </summary>
        public DateTime fecha { get { return this._fecha; } }
        private bool _habilitar;
        /// <summary>
        /// Define el Estatus Habilitar de la Referencia
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Valores por Defecto
        /// </summary>
        public Referencia()
        {   //Asignando Atributos
            this._id_referencia = 0;
            this._id_registro = 0;
            this._id_tabla = 0;
            this._id_referencia_tipo = 0;
            this._valor = "";
            this._fecha = DateTime.MinValue;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Valores dado un Registro
        /// </summary>
        /// <param name="id_registro"></param>
        public Referencia(int id_registro)
        {   //Armando Arreglo de Parametros
            object[] param = { 15, id_registro, 0, 0, 0, "", null, 0, false, "", "", "" };
            //Obteniendo Registro
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Atributos
                        this._id_referencia = id_registro;
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_referencia_tipo = Convert.ToInt32(dr["IdReferenciaTipo"]);
                        this._valor = dr["Valor"].ToString();
                        DateTime.TryParse(dr["Fecha"].ToString(), out this._fecha);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }
        }
        /// <summary>
        /// Obtiene una instancia Referencia a partir del valor y tipo solicitado
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="id_tipo_referencia"></param>
        public Referencia(string valor, int id_tipo_referencia)
        {
            //Armando Arreglo de Parametros
            object[] param = { 24, 0, 0, 0, id_tipo_referencia, valor, null, 0, false, "", "", "" };
            //Obteniendo Registro
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que exista el Registro
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo Registro
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {   //Asignando Atributos
                        this._id_referencia = Convert.ToInt32(dr["Id"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_referencia_tipo = Convert.ToInt32(dr["IdReferenciaTipo"]);
                        this._valor = dr["Valor"].ToString();
                        DateTime.TryParse(dr["Fecha"].ToString(), out this._fecha);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                    }
                }
            }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~Referencia()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Obtener el Registro por temperatura
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_tipo_temperatura">Tipo de Temperatura (Maxima, Media, Minima)</param>
        /// <returns></returns>
        private static Referencia obtieneTemperaturaServicio(int id_servicio, int id_tipo_temperatura)
        {
            //Declarando Objeto de Retorno
            Referencia result = new Referencia();

            //Armando Arreglo de Parametros
            object[] param = { 20, 0, id_servicio, 0, id_tipo_temperatura, "", null, 0, false, "", "", "" };

            //Instanciando 
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que Existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in ds.Tables["Table"].Rows)

                        //Instanciando Referencia
                        result = new Referencia(Convert.ToInt32(dr["Id"]));
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Insertar las Temperaturas de la 2da Unidad del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio Solicitante</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="temp_maxima">Temperatura Máxima (Congelado)</param>
        /// <param name="temp_media">Temperatura Media (Refrigerado)</param>
        /// <param name="temp_minima">Temperatura Minima (Fresco)</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        private static RetornoOperacion InsertaTemperaturasServicioFull(int id_servicio, int id_compania, string temp_maxima, string temp_media, string temp_minima, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha Actual
            DateTime fecha = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();

            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Fresco", 0, "Unidad 2"), temp_maxima, fecha, id_usuario, true, "", "", "" };

            //Insertando Temperatura Máxima
            //Insertando Temperatura Media
            result = temp_maxima.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

            //Validando que la Operación fuese Correcta
            if (result.OperacionExitosa)
            {
                //Armando Arreglo de Parametros
                object[] param1 = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Refrigerado", 0, "Unidad 2"), temp_media, fecha, id_usuario, true, "", "", "" };

                //Insertando Temperatura Media
                result = temp_media.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param1);

                //Validando que la Operación fuese Correcta
                if (result.OperacionExitosa)
                {
                    //Armando Arreglo de Parametros
                    object[] param2 = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Congelado", 0, "Unidad 2"), temp_minima, fecha, id_usuario, true, "", "", "" };

                    //Insertando Temperatura Máxima
                    result = temp_minima.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param2);
                }
            }

            //Validando que la Operación fuese Correcta
            if (result.OperacionExitosa)

                //Instanciando Valor Positivo
                result = new RetornoOperacion(id_servicio, "", true);

            //Devolviendo Resultado Obtenido
            return result;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Método encargado de Insertar una referencia
        /// </summary>
        /// <param name="registro">Id de Registro</param>
        /// <param name="tabla">Id de Tabla</param>
        /// <param name="tipo">Tipo de Referencia</param>
        /// <param name="valor">Valor de la Referencia</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaReferencia(int registro, int tabla, int tipo, string valor,
                            DateTime fecha, int usuario)
        {
            return InsertaReferencia(registro, tabla, tipo, valor, fecha, usuario, true);
        }
        /// <summary>
        /// Método encargado de Insertar una referencia
        /// </summary>
        /// <param name="registro">Id de Registro</param>
        /// <param name="tabla">Id de Tabla</param>
        /// <param name="tipo">Tipo de Referencia</param>
        /// <param name="valor">Valor de la Referencia</param>
        /// <param name="fecha">Fecha</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <param name="habilitar">Estatus Habilitar</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaReferencia(int registro, int tabla, int tipo, string valor,
                            DateTime fecha, int usuario, bool habilitar)
        {   //Inicializando parametros
            object[] param = {1, 0, registro, tabla, tipo, valor, fecha, usuario, habilitar, "", "", ""};
            //Ejecutando SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
        }

        /// <summary>
        /// Método encargado de Editar una referencia
        /// </summary>
        /// <param name="id_referencia">Id de Referencia</param>
        /// <param name="valor">Valor de la Referencia</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaReferencia(int id_referencia, string valor, int usuario)
        {
            //Dexclaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("El tipo de referencia no es editable.");
            //Instanciando Referencia
            using (Referencia ref1 = new Referencia(id_referencia))
            {
                //Instanciamos Tipo de Referencia
                using (ReferenciaTipo objreferenciaTipo = new ReferenciaTipo(ref1.id_referencia_tipo))
                {
                    //Validamos que sea editable el Tipo
                    if (objreferenciaTipo.bit_editable)
                    {
                        //Inicializando parametros
                        object[] param = { 2, id_referencia, 0, 0, ref1.id_referencia_tipo, valor, Fecha.ObtieneFechaEstandarMexicoCentro(), usuario, true, "", "", "" };
                        //Ejecutando SP
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Editar una referencia
        /// </summary>
        /// <param name="id_referencia">Id de Referencia</param>
        /// <param name="id_referencia_tipo">Tipo de la Referencia</param>
        /// <param name="valor">Valor de la Referencia</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EditaReferencia(int id_referencia, int id_referencia_tipo, string valor, int usuario)
        {   
            //Dexclaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("El tipo de referencia no es editable.");
            
            //Instanciando Referencia
            using (Referencia ref1 = new Referencia(id_referencia))
            {
                //Instanciamos Tipo de Referencia
                using (ReferenciaTipo objreferenciaTipo = new ReferenciaTipo(ref1.id_referencia_tipo))
                {
                    //Validamos que sea editable el Tipo
                    if (objreferenciaTipo.bit_editable)
                    {
                        //Inicializando parametros
                        object[] param = { 16, id_referencia, 0, 0, id_referencia_tipo, valor, Fecha.ObtieneFechaEstandarMexicoCentro(), usuario, true, "", "", "" };
                        //Ejecutando SP
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    }
                }
            }
            //Devolvemos valor
            return resultado;
        }
        /// <summary>
        /// Método encargado de Eliminar una referencia
        /// </summary>
        /// <param name="id_referencia">Id de Referencia</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EliminaReferencia(int id_referencia, int usuario)
        {
            //Declarando Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion("El tipo de referencia no es editable.");

            //Instanciando Referencia
            using (Referencia ref1 = new Referencia(id_referencia))
            {
                //Instanciamos Tipo de Referencia
                using (ReferenciaTipo objreferenciaTipo = new ReferenciaTipo(ref1.id_referencia_tipo))
                {
                    //Validamos que sea editable el Tipo
                    if (objreferenciaTipo.bit_editable)
                    {
                        //Inicializando parametros
                        object[] param = { 3, id_referencia, 0, 0, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), usuario, false, "", "", "" };
                        //Ejecutando SP
                        resultado = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
                    }
                }
            }
            //Devolvemos valor
            return resultado;
        }

        /// <summary>
        /// Método encargado de Eliminar una referencia aún cuando el tipo sea no editable
        /// </summary>
        /// <param name="id_referencia">Id de Referencia</param>
        /// <param name="usuario">Id de Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EliminaReferenciaNoEditable(int id_referencia, int usuario)
        {
            //Inicializando parametros
            object[] param = { 3, id_referencia, 0, 0, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), usuario, false, "", "", "" };
            //Ejecutando SP
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

        }

        /// <summary>
        /// Elimina las referencias del registro y tabla, aún y cuando estas son no editables
        /// </summary>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro<param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion EliminaReferenciasNoEditable(int id_tabla, int id_registro, int id_usuario)
        {
            //Declaramos  Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Cargamos Referencias
            using(DataTable mitReferencias = CargaReferenciasParaDeshabilitacion(id_tabla, id_registro))
            {
                //Validamos Origen
                if(Validacion.ValidaOrigenDatos(mitReferencias))
                {
                    //Recorremos las referencias
                    foreach(DataRow r in mitReferencias.Rows)
                    {
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Eliminamos Referencia
                            resultado = EliminaReferenciaNoEditable(r.Field<int>("Id"), id_usuario);
                        }
                        else
                        {
                            //Salimos del ciclo
                            break;
                        }
                    }
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método encargado de Obtener todas la referencia en detalle un valor
        /// </summary>
        /// <param name="mitReferencias"></param>
        /// <returns></returns>
        public static string ObtieneReferenciaDetalles(DataTable mitReferencias)
        {
            //Declaramos Objeto Resultado
            string Referencia = "";

            //Validamos que exista Origen de Datos
           if(TSDK.Datos.Validacion.ValidaOrigenDatos(mitReferencias))
            {
               
              //Agrupamos Referencias por Tipo
             var  tipos  =from DataRow t in mitReferencias.Rows 
                                group t by t["Tipo"].ToString() into grupo
                                    select grupo;

                //Recorremos cada uno de los tipos de Referencias
                foreach(var tipo in tipos)
                {
                    
                    //Asignamos Valor de Tipo de Referencia
                   string  Tipo = tipo.Key.ToString() + ": ";
                   //Recorremos cada Referencia de Acuerdo al Tipo
                    foreach (DataRow r in mitReferencias.Rows)
                    {
                        //Si es el mismo tipo
                        if (r["Tipo"].ToString() == tipo.Key)
                        {
                        //Asignamos Valor
                        Tipo =  Tipo + r["Valor"].ToString() + ",";
                        }
                    }
                    //Eliminamos Ultima Coma
                    Tipo = Tipo.TrimEnd(',') + ". ";

                    //Concatenamos la Refrencia al Tipo
                    Referencia = Referencia + " " + Tipo;
                }
            }
            //Devolvemos valor
            return Referencia =Referencia.TrimStart(' ');
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="mitReferencias"></param>
        /// <param name="mit_destino"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_tipo"></param>
        public static void ObtieneReferenciaGlobal(DataTable mitReferencias, DataTable mit_destino,int id_registro, int id_tabla, int id_tipo)
        {
            //Declaramos  Variable 
            DataRow[] re = null;

            //Validamos que exista Origen de Datos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mitReferencias))
            {
                //Agrupamos Referencias por Tipo
                var conceptos = from DataRow c in mitReferencias.Rows
                            group c by c["IdConcepto"].ToString() into grupo
                            select grupo;
               

                //Recorremos cada uno de los tipos de Clasificacion Servicio/Otros
                foreach (var concepto in conceptos)
                {
                    string clasificacion = (from DataRow r in mitReferencias.Rows
                                            where Convert.ToInt32(r["IdConcepto"]) == Convert.ToInt32(concepto.Key)
                                            select r["IdClasificacion"].ToString()).FirstOrDefault();
                    //Asignamos Valor de Tipo de Referencia
                    string Tipo = clasificacion;

                    //Obtenemos los Tipos de acuerdo a la clasificacion
                     re = (from DataRow r in mitReferencias.Rows
                                    where Convert.ToInt32(r["IdConcepto"]) == Convert.ToInt32(concepto.Key)
                                    select r).ToArray();

                    //Añadimos Tabl de Clasificacion
                    DataTable dtReferenciasResultado = TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(re);
                    
                    //Añadimos Registro
                    mit_destino.Rows.Add(id_registro, id_tabla, id_tipo, Tipo + ObtieneReferenciaDetalles(dtReferenciasResultado), Fecha.ObtieneFechaEstandarMexicoCentro()); 
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar la Estructura de las Referencia del Arbol
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_tipo"></param>
        /// <param name="id_grupo"></param>
        /// <returns></returns>
        public static DataSet CargaEstructuraReferencia(EstructuraArbolReferencia nivel, int id_tabla, int id_registro, int id_compania, int id_tipo, int id_grupo)
        {   //Declaramos la referencia al arreglo de parametros
            object[] param = null;
            //Validando 
            switch (nivel)
            {   //Carga toda la estructura del arbol
                case EstructuraArbolReferencia.Arbol:
                    {   //Inicializando parametros
                        object[] param1 = { (int)EstructuraArbolReferencia.Arbol, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", id_compania.ToString() };
                        param = param1;
                        break;
                    }
                case EstructuraArbolReferencia.Raiz:
                    {
                        break;
                    }
                case EstructuraArbolReferencia.Hoja1:
                    {   //Inicializando parametros
                        object[] param1 = { 13, id_compania, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", "" };
                        param = param1;
                        break;
                    }
                case EstructuraArbolReferencia.Hoja2:
                    {   //Inicializando parametros
                        object[] param1 = { 8, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, id_grupo.ToString(), "", id_compania.ToString() };
                        param = param1;
                        break;
                    }
                case EstructuraArbolReferencia.Valor:
                    {   //Inicializando parametros
                        object[] param1 = {9, 0, id_registro, id_tabla, id_tipo, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, id_grupo.ToString(), "", ""};
                        param = param1;
                        break;
                    }
            }
            try
            {
                return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param);
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Carga las referencias solicitadas acorde a los criterios definidos
        /// </summary>
        /// <param name="idCompania">Id de Compania</param>
        /// <param name="idRegistro">Id de registro a consultar</param>
        /// <param name="idTabla">Id de Tabla a ala que pertenece el registro</param>
        /// <param name="idGrupo">Grupo de Referencias</param>
        /// <param name="idTipo">Tipo de Referencias</param>
        /// <param name="idTipoAlterno">Tipo Alterno de Referencias</param>
        /// <param name="valor">Valor de la referencia</param>
        /// <param name="maxRegistros">Número máximo de registros a devolver, o bien '0' para devolver todos</param>
        /// <returns></returns>
        public static DataTable CargaReferenciasRegistro(int idCompania, int idRegistro, int idTabla, int idGrupo, int idTipo, string idTipoAlterno, string valor, int maxRegistros)
        {   //Inicialziando arreglo de parámetros
            object[] parametros = { 10, idCompania, idRegistro, idTabla, idTipo, valor, Fecha.ObtieneFechaEstandarMexicoCentro(), 0, true, idGrupo.ToString(), idTipoAlterno, maxRegistros.ToString() };
            //Cargando los datos solicitados
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    return DS.Tables["Table"];
                else
                    return null;
            }
        }
        /// <summary>
        /// Método Público encargado de Cargar las Referencias de Viaje dado un Registro y una Compania
        /// </summary>
        /// <param name="idCompania">Id de Compania</param>
        /// <param name="idRegistro">Id de Registro</param>
        /// <returns></returns>
        public static DataTable CargaReferenciasRegistroViaje(int idCompania, int idRegistro)
        {   //Declarando Objeto de Retorno
            DataTable dt = null;
            //Inicialziando arreglo de parámetros
            object[] parametros = { 14, idCompania, idRegistro, 0, 0, "", null, 0, false, "", "", "" };
            //Cargando los datos solicitados
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando resultado Obtenido
                    dt = DS.Tables["Table"];
            }
            //Devolviendo Resultado Obtenido
            return dt;
        }
        /// <summary>
        /// Método encargado de Insertar las Temperaturas del Servicio
        /// </summary>
        /// <param name="id_servicio">Servicio Solicitante</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="temp_maxima">Temperatura Máxima (Congelado)</param>
        /// <param name="temp_media">Temperatura Media (Refrigerado)</param>
        /// <param name="temp_minima">Temperatura Minima (Fresco)</param>
        /// <param name="full">Indicador que expresa si la Tempera</param>
        /// <param name="temp_maxima2">Temperatura Máxima (Congelado - Unidad 2)</param>
        /// <param name="temp_media2">Temperatura Media (Refrigerado - Unidad 2)</param>
        /// <param name="temp_minima2">Temperatura Minima (Fresco - Unidad 2)</param>
        /// <param name="id_usuario">Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaTemperaturasServicio(int id_servicio, int id_compania, string temp_maxima, string temp_media, string temp_minima, bool full, string temp_maxima2, string temp_media2, string temp_minima2, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Fecha Actual
            DateTime fecha = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();

            //Inicializando Bloque Transaccional
            using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Armando Arreglo de Parametros
                object[] param = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Fresco", 0, "Unidad 1"), temp_maxima, fecha, id_usuario, true, "", "", "" };

                //Insertando Temperatura Máxima
                result = temp_maxima.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);

                //Validando que la Operación fuese Correcta
                if(result.OperacionExitosa)
                {
                    //Armando Arreglo de Parametros
                    object[] param1 = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Refrigerado", 0, "Unidad 1"), temp_media, fecha, id_usuario, true, "", "", "" };

                    //Insertando Temperatura Media
                    result = temp_media.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param1);

                    //Validando que la Operación fuese Correcta
                    if (result.OperacionExitosa)
                    {
                        //Armando Arreglo de Parametros
                        object[] param2 = { 1, 0, id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Congelado", 0, "Unidad 1"), temp_minima, fecha, id_usuario, true, "", "", "" };

                        //Insertando Temperatura Minima
                        result = temp_minima.Equals("") ? new RetornoOperacion(id_servicio) : CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param2);
                    }
                }

                //Validando que la Operación fuese Correcta
                if (result.OperacionExitosa)
                {
                    //Validando si se solicito la 2da Unidad
                    if(full)
                    
                        //Insertando Temperatura de la 2da Unidad
                        result = Referencia.InsertaTemperaturasServicioFull(id_servicio, id_compania, temp_maxima2, temp_media2, temp_minima2, id_usuario);
                    else
                        //Instanciando Valor Positivo
                        result = new RetornoOperacion(id_servicio);

                    //Validando que la Operación fuese Correcta
                    if (result.OperacionExitosa)
                    {
                        //Instanciando Retorno con Servicio
                        result = new RetornoOperacion(id_servicio);

                        //Completando Transacción
                        trans.Complete();
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Editar las Temperaturas del Servicio
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="temp_maxima"></param>
        /// <param name="temp_media"></param>
        /// <param name="temp_minima"></param>
        /// <param name="full"></param>
        /// <param name="temp_maxima2"></param>
        /// <param name="temp_media2"></param>
        /// <param name="temp_minima2"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion EditaTemperaturasServicio(int id_servicio, int id_compania, string temp_maxima, string temp_media, string temp_minima, 
                                                    bool full, string temp_maxima2, string temp_media2, string temp_minima2, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            Referencia referencia = new Referencia();
            DateTime fecha = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            int contador = 0;

            //Instanciando Tipo de Temperatura
            int[] id_tipo_temp = { ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Fresco", 0, "Unidad 1"), 
                                     ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Refrigerado", 0, "Unidad 1"), 
                                        ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Congelado", 0, "Unidad 1"), 
                                   ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Fresco", 0, "Unidad 2"), 
                                     ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Refrigerado", 0, "Unidad 2"), 
                                        ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 1, "Temperatura Congelado", 0, "Unidad 2") };

            //Armando Arreglo de Temperaturas
            string[] temperaturas = { temp_maxima, temp_media, temp_minima, temp_maxima2, temp_media2, temp_minima2 };

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Inicializando Ciclo
                while (contador <= 5)
                {
                    //Temperatura Maxima
                    referencia = obtieneTemperaturaServicio(id_servicio, id_tipo_temp[contador]);

                    //Validando que Exista
                    if (referencia.id_referencia > 0)

                        //Editando Referencia
                        result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, new object[] { 2, referencia.id_referencia, id_servicio, referencia.id_tabla, referencia._id_referencia_tipo, temperaturas[contador], fecha, id_usuario, true, "", "", "" });
                    else
                        //Insertando Referencias
                        result = temperaturas[contador].Equals("") ? new RetornoOperacion(id_servicio) : Referencia.InsertaReferencia(id_servicio, 1, id_tipo_temp[contador], temperaturas[contador], fecha, id_usuario, true);

                    //Incrementando Contador
                    contador = result.OperacionExitosa ? contador + 1 : 6;
                }

                //Validando que las Operaciones Fuesen Exitosas
                if (result.OperacionExitosa)
                {
                    //Instanciando Servicio en Retorno
                    result = new RetornoOperacion(id_servicio);

                    //Completando Transacción
                    trans.Complete();
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de 
        /// </summary>
        /// <param name="id_servicio"></param>
        /// <param name="temp_maxima"></param>
        /// <param name="temp_media"></param>
        /// <param name="temp_minima"></param>
        /// <param name="full"></param>
        /// <param name="temp_maxima2"></param>
        /// <param name="temp_media2"></param>
        /// <param name="temp_minima2"></param>
        /// <returns></returns>
        public static bool ObtieneTemperaturasServicio(int id_servicio, int id_compania, out string temp_maxima, out string temp_media, out string temp_minima,
                                    out bool full, out string temp_maxima2, out string temp_media2, out string temp_minima2)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Asignando Variables de retorno
            temp_maxima = temp_media = temp_minima = temp_maxima2 = temp_media2 = temp_minima2 = "";
            full = false;

            //Armando Arreglo de Parametros
            object[] param = { 19, id_compania, id_servicio, 0, 0, "", null, 0, false, "", "", "" };

            //Instanciando SP
            using(DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Recorriendo Registro
                    foreach(DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        temp_maxima = dr["TempMax1"].ToString();
                        temp_media = dr["TempMed1"].ToString();
                        temp_minima = dr["TempMin1"].ToString();
                        temp_maxima2 = dr["TempMax2"].ToString();
                        temp_media2 = dr["TempMed2"].ToString();
                        temp_minima2 = dr["TempMin2"].ToString();
                        break;
                    }

                    //Validando que existan valores Full
                    if (!temp_maxima2.Equals("") || !temp_media2.Equals("") || !temp_minima2.Equals(""))

                        //Marcando Indicador
                        full = true;

                    //Asignando Valor Positivo
                    result = true;
                }
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Cargar las Referencias dado un Registro, una Tabla y una Compania
        /// </summary>
        /// <param name="idCompania">Id de Compania</param>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <returns></returns>
        public static DataTable CargaReferenciasRegistro(int idCompania, int idRegistro, int idTabla)
        {   
            //Declarando Objeto de Retorno
            DataTable dt = null;
            
            //Inicialziando arreglo de parámetros
            object[] parametros = { 22, idCompania, idRegistro, idTabla, 0, "", null, 0, false, "", "", "" };
            
            //Cargando los datos solicitados
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, parametros))
            {   
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //Asignando resultado Obtenido
                    dt = DS.Tables["Table"];
            }
            
            //Devolviendo Resultado Obtenido
            return dt;
        }

        /// <summary>
        /// Método encargado de Cargar las Referencias
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <returns></returns>
        public static DataTable CargaReferenciasUnicasServicio(int id_servicio)
        {
            //Declarando Objeto de Retorno
            DataTable dt = null;

            //Inicialziando arreglo de parámetros
            object[] param = { 23, 0, id_servicio, 0, 0, "", null, 0, false, "", "", "" };

            //Cargando los datos solicitados
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    
                    //Asignando resultado Obtenido
                    dt = DS.Tables["Table"];
            }

            //Devolviendo Resultado Obtenido
            return dt;
        }


        #endregion

        #region Logica de Forma Web

        /// <summary>
        /// Realiza las acciones correspondientes al seleccionar un nodo especifico
        /// </summary>
        public static void NodoSeleccionado(int profundidadNodo, int valorNodo, int id_tabla, int id_registro, int id_compania, GridView gv, bool edicion)
        {
            Referencia.EstructuraArbolReferencia vistaArbol = EstructuraArbolReferencia.Hoja1;
            string llave = "";
            int id_tipo = 0, id_grupo = 0;

            //De acuerdo al nivel en donde se haya dado el click 
            switch (profundidadNodo)
            {
                case 0:
                    {
                        vistaArbol = EstructuraArbolReferencia.Hoja1;
                        llave = "id_grupo";
                        break;
                    }
                case 1:
                    {
                        vistaArbol = EstructuraArbolReferencia.Hoja2;
                        llave = "id_tipo";
                        id_grupo = valorNodo;
                        break;
                    }
                case 2:
                    {
                        vistaArbol = EstructuraArbolReferencia.Valor;
                        llave = "id_referencia";
                        id_tipo = valorNodo;
                        break;
                    }

            }
            //Mostramos u ocultamos las columnas correspondientes
            if (edicion && vistaArbol == EstructuraArbolReferencia.Valor)
                gestionColumnasGridView(EstructuraArbolReferencia.ValorEdicion, gv);
            else
                gestionColumnasGridView(vistaArbol, gv);
            //Cargamos el grid view 
            using (DataSet DS = Referencia.CargaEstructuraReferencia(vistaArbol, id_tabla, id_registro, id_compania, id_tipo, id_grupo))
            {
                TSDK.ASP.Controles.CargaGridView(gv, DS, 0, llave);
            }
        }

        /// <summary>
        /// Metodo encargado de gestionar las columnas (ocultar/mostrar) del grid view de acuerdo al nivel seleccionado
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="gv"></param>
        private static void gestionColumnasGridView(Referencia.EstructuraArbolReferencia nivel, GridView gv)
        {   //Validando el Nivel
            switch (nivel)
            {   //Muestra la estructura de los grupos de referencias
                case Referencia.EstructuraArbolReferencia.Hoja1:
                    {   //grupo
                        gv.Columns[0].Visible = true;
                        //tipo
                        gv.Columns[1].Visible = true;
                        //valor
                        gv.Columns[2].Visible = true;
                        //fecha
                        gv.Columns[3].Visible = true;
                        //editar
                        gv.Columns[4].Visible = false;
                        break;
                    }
                //Muestra la estructura de los grupos de referencias
                case Referencia.EstructuraArbolReferencia.Hoja2:
                    {   //grupo
                        gv.Columns[0].Visible = false;
                        //tipo
                        gv.Columns[1].Visible = true;
                        //valor
                        gv.Columns[2].Visible = true;
                        //fecha
                        gv.Columns[3].Visible = false;
                        //editar
                        gv.Columns[4].Visible = false;

                        break;
                    }
                //Muestra la estructura de los grupos de referencias
                case Referencia.EstructuraArbolReferencia.Valor:
                    {   //grupo
                        gv.Columns[0].Visible = false;
                        //tipo
                        gv.Columns[1].Visible = false;
                        //valor
                        gv.Columns[2].Visible = true;
                        //fecha
                        gv.Columns[3].Visible = true;
                        //editar
                        gv.Columns[4].Visible = true;
                        break;
                    }
                //Muestra la estructura de los valores asignados a la referencia
                case Referencia.EstructuraArbolReferencia.ValorEdicion:
                    {   //grupo
                        gv.Columns[0].Visible = false;
                        //tipo
                        gv.Columns[1].Visible = false;
                        //valor
                        gv.Columns[2].Visible = true;
                        //fecha
                        gv.Columns[3].Visible = false;
                        //editar
                        gv.Columns[4].Visible = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método que guarda un registro referencia
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="tipo"></param>
        /// <param name="id_referencia"></param>
        /// <param name="referencia"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static ErrorReferencia Guarda(AccionReferencia accion, int tipo, int id_referencia, string referencia, int id_registro, int id_tabla, int id_usuario)
        {   //Declarando variable para almacenar registro insertado/editado
            int registro = 0;
            ErrorReferencia error = ErrorReferencia.SinError;
            //Determinando la forma de guardar
            switch (accion)
            {   //Nuevo registro
                case AccionReferencia.Registrar:
                    {   //Insertando la nueva rerencia
                        registro = Referencia.InsertaReferencia(id_registro, id_tabla, tipo, referencia, Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, true).IdRegistro;
                        //Si la insercion fue exitosa
                        if (registro > 0)
                            error = ErrorReferencia.SinError;
                        else//Asignando Error
                            error = (ErrorReferencia)registro;
                        break;
                    }
                //Edición de Registro
                case AccionReferencia.Editar:
                    {   //Realizando la actualización
                        registro = Referencia.EditaReferencia(id_referencia, referencia, id_usuario).IdRegistro;
                        //Si la actualización fue exitosa
                        if (registro > 0)
                            error = ErrorReferencia.SinError;
                        else//Asignando Error
                            error = (ErrorReferencia)registro;
                        break;
                    }
                //Eliminación de Registro
                case AccionReferencia.Eliminar:
                    {   //Realizando la actualización
                        registro = Referencia.EliminaReferencia(id_referencia, id_usuario).IdRegistro;
                        //Si la insercion fue exitosa
                        if (registro > 0)
                            error = ErrorReferencia.SinError;
                        else//Asignando Error
                            error = (ErrorReferencia)registro;
                        break;
                    }
            }


            //Devolviendo Resultado Obtenido
            return error;
        }
        /// <summary>
        /// Método que muestra el error de un registro referencia
        /// </summary>
        /// <param name="error"></param>
        /// <param name="etiqueta"></param>
        public static void Error(ErrorReferencia error, Label etiqueta)
        {   //Determinando que mensaje se mostrará
            switch (error)
            {   //Limpiar mensaje
                case ErrorReferencia.SinError:
                default:
                    etiqueta.Text = "";
                    break;
                //No existen elementos de catálogo.
                case ErrorReferencia.NodoTipoNoValido:
                    etiqueta.Text = "Para agregar una referencia, seleccione un nodo tipo de referencia.";
                    break;
                //Se ha alcanzado el límite de referencias definido para el tipo
                case ErrorReferencia.MaximoAlcanzado:
                    etiqueta.Text = "Se ha alcanzado el límite de referencias de este tipo.";
                    break;
                //Se ha alcanzado el límite de referencias definido para el tipo
                case ErrorReferencia.Editable:
                    etiqueta.Text = "No es posible la edición de referencias de este tipo.";
                    break;
            }
        }
        /// <summary>
        /// Carga Todas las Referencias 
        /// </summary>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <returns></returns>
        public static DataTable CargaReferenciasGeneral(int id_registro, int id_tabla)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros         
            object[] param =  {5, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", ""};
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        ///  Carga Referencias
        /// </summary>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_tipo"></param>
        /// <returns></returns>
        public static DataTable CargaReferencias(int id_registro, int id_tabla, int id_tipo)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros         
            object[] param =  {11, 0, id_registro, id_tabla, id_tipo, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", ""};
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        ///  Carga Referencias
        /// </summary>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <returns></returns>
        public static DataTable CargaReferencias(int id_registro, int id_tabla)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros         
            object[] param =  {12, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", ""};
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }
        /// <summary>
        /// Método encargado de Cargar todas las Referencias
        /// </summary>
        /// <param name="id_registro"></param>
        /// <param name="id_tabla"></param>
        /// <param name="id_compania"></param>
        /// <returns></returns>
        public static DataTable CargaReferenciasGeneral(int id_registro, int id_tabla, int id_compania)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros         
            object[] param = { 13, id_compania, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", "" };
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Método encargado de Cargar todas las Referencias
        /// </summary>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <returns></returns>
        public static DataTable CargaReferenciasParaDeshabilitacion(int id_tabla, int id_registro)
        {   //Definiendo objeto de retorno
            DataTable mit = null;
            //Inicializamos el arreglo de parametros         
            object[] param = { 17, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", "" };
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];
                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        ///  Carga Referencias para el reporte comprobante facturacion
        /// </summary>
        /// <param name="id_registro">Id que permite identifiacar el registro al cual se extraeran las referencias</param>
        /// <param name="id_tabla">Id que permite identificar la tabla a la cual pertenece el registro</param>
        /// <param name="id_tipo">Id que permite identificar el tipo de referencia.</param>
        /// <returns></returns>
        public static string CargaReferencia(int id_registro, int id_tabla, int id_tipo)
        {   
            //Definiendo objeto de retorno
            string valor = "";
            //Inicializamos el arreglo de parametros         
            object[] param = { 18, 0, id_registro, id_tabla, id_tipo, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "", "" };
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    valor = (from DataRow r in ds.Tables[0].Rows
                                     select r.Field<string>("Valor")).FirstOrDefault();
                //Devolviendo resultado
                return valor;
            }
        }

        /// <summary>
        /// Método que carga las referencias dada las descripciónes y compañia configuradas en base de datos (referencia grupo, referencia tipo y compañia)
        /// </summary>
        /// <param name="id_compania">Identificador que permite saber a que compañia pertenece</param>
        /// <param name="id_tabla">Identificador que permite saber a que tabla pertenece el registro</param>
        /// <param name="id_registro">Identificador que permite saber que registro es el que se va a consultar</param>
        /// <param name="descripcion_gpo">Descripción de la referencia grupo</param>
        /// <param name="descripcion_tipo">Descripción de la referencia tipo</param>
        /// <returns></returns>
        public static string CargaReferencia(string id_compania, int id_tabla, int id_registro, string descripcion_gpo, string descripcion_tipo)
        {
            //Definiendo objeto de retorno
            string valor = "";
            //Inicializamos el arreglo de parametros         
            object[] param = { 21, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, id_compania, descripcion_gpo, descripcion_tipo};
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    valor = (from DataRow r in ds.Tables[0].Rows
                             select r.Field<string>("Valor")).FirstOrDefault();
                //Devolviendo resultado
                return valor;
            }
        }
        /// <summary>
        /// Método que carga Id referencias dada las descripciónes y compañia configuradas en base de datos (referencia grupo, referencia tipo y compañia)
        /// </summary>
        /// <param name="id_compania">Identificador que permite saber a que compañia pertenece</param>
        /// <param name="id_tabla">Identificador que permite saber a que tabla pertenece el registro</param>
        /// <param name="id_registro">Identificador que permite saber que registro es el que se va a consultar</param>
        /// <param name="descripcion_gpo">Descripción de la referencia grupo</param>
        /// <param name="descripcion_tipo">Descripción de la referencia tipo</param>
        /// <returns></returns>
        public static int CargaRegistroReferencia(string id_compania, int id_tabla, int id_registro, string descripcion_gpo, string descripcion_tipo)
        {
            //Definiendo objeto de retorno
            int Id = 0;
            //Inicializamos el arreglo de parametros         
            object[] param = { 25, 0, id_registro, id_tabla, 0, "", Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, id_compania, descripcion_gpo, descripcion_tipo };
            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    Id = (from DataRow r in ds.Tables[0].Rows
                             select r.Field<int>("Id")).FirstOrDefault();
                //Devolviendo resultado
                return Id;
            }
        }
        #endregion
    }
}
