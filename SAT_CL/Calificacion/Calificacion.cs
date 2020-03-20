using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Linq;

namespace SAT_CL.Calificacion
{
    /// <summary>
    /// Clase que permite realizar Inserción, Edición y Consulta a la tabla Calificación
    /// </summary>
    public class Calificacion:Disposable
    {
        #region Atributos
        /// <summary>
        /// Atributo que almacena el nombre del sp
        /// </summary>
        private static string nom_sp = "calificacion.sp_calificacion_tca";

        private int _id_calificacion;
        /// <summary>
        /// Identifica una calificación en general de un Cliente,Operador o Transportista
        /// </summary>
        public int id_calificacion
        {
            get { return _id_calificacion; }
        }
        private int _id_tabla;
        /// <summary>
        /// Identificador de la entidad a calificar (Cliente,Operador, Transportista)
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }
        }
        private int _id_registro;
        /// <summary>
        /// Identificador del Cliente Operador o Transportista a calificar
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }
        private byte _calificacion_promedio;
        /// <summary>
        /// Número evaluativo asignado al Cliente Operador o Transportista
        /// </summary>
        public byte calificacion_promedio
        {
            get { return _calificacion_promedio; }
        }
        private string _comentario;
        /// <summary>
        /// Mensaje sobre la evaluación de un Cliente Operador o Transportista
        /// </summary>
        public string comentario
        {
            get { return _comentario; }
        }
        private string _email_calificante;
        /// <summary>
        /// Correo electrónico de la persona de evalua a un Cliente, Operador o Transportista
        /// </summary>
        public string email_calificante
        {
            get { return _email_calificante; }
        }
        private int _id_contacto;
        /// <summary>
        /// Identificador del contacto que realiza la calificación
        /// </summary>
        public int id_contacto
        {
            get { return _id_contacto; }
        }
        private bool _habilitar;
        /// <summary>
        /// Define la disponibilidad de uso de un registro (Habilitado - Disponible, Deshabilitado - No Disponible)
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor que inicializa los atributos en cero
        /// </summary>
        public Calificacion()
        {
            this._id_calificacion = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._calificacion_promedio = 0;
            this._comentario = "";
            this._email_calificante = "";
            this._id_contacto = 0;
            this._habilitar = false;
        }
        /// <summary>
        /// Constructor que inicializa los atributos a partir de un registro de calificación
        /// </summary>
        /// <param name="id_calificacion"></param>
        public Calificacion(int id_calificacion)
        {
            //Invoca al método que realiza la busqueda del registro
            cargaAtributos(id_calificacion);
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Destructor default de la clase
        /// </summary>
        ~Calificacion()
        {
            Dispose(false);
        }
        #endregion

        #region Métodos Privados
        /// <summary>
        /// Método que realiza la busqueda y asignación de un registro a los atributos de la clase
        /// </summary>
        /// <param name="id_calificacion">Id que sirve como referencia para la busuqeda de una calificación</param>
        /// <returns></returns>
        private bool cargaAtributos(int id_calificacion)
        {
            //Creación del objeto retorno
            bool retorno = false;
            //Creación del arreglo que almacena los datos necesarios para realizar la busqueda de un registro de calificación
            object[] param = { 3, id_calificacion, 0, 0, 0, "", "", 0, 0, false, "", "" };
            //Instancia al método que realiza la busqueda de una calificación 
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos alamcenados en el dataset (que existan)
                if (Validacion.ValidaOrigenDatos(DS.Tables[0]))
                {
                    //Recorre y asigna a los atributos los valores encontrados en las filas del DS
                    foreach (DataRow r in DS.Tables[0].Rows)
                    {
                        this._id_calificacion = id_calificacion;
                        this._id_tabla = Convert.ToInt32(r["IdTabla"]);
                        this._id_registro = Convert.ToInt32(r["IdRegistro"]);
                        this._calificacion_promedio = Convert.ToByte(r["CalificacionPromedio"]);
                        this._comentario = Convert.ToString(r["Comentario"]);
                        this._email_calificante = Convert.ToString(r["Email"]);
                        this._id_contacto = Convert.ToInt32(r["IdContacto"]);
                        this._habilitar = Convert.ToBoolean(r["Habilitar"]);
                    }
                    //Cambia el valor al objeto retorno
                    retorno = true;
                }
            }
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza una calificación
        /// </summary>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a calificar (Cliente, Operador o Transportista)</param>
        /// <param name="id_registro">Actualiza el identificador del Cliente, Operador o Transportista a calificar</param>
        /// <param name="calificacion_promedio">Actualiza el número de evaluación otorgado a un Cliente, Operador o Transportista</param>
        /// <param name="comentario">Actualiza el mensaje de evaluación sobre un Cliente, Operador o Transportista</param>
        /// <param name="email_calificante">Actualiza el correo electrónico de la persona que realizó la evaluación</param>
        /// <param name="id_contacto">Actualiza el identificador de la persona que realizó la evaluación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó acciones sobre el registro</param>
        /// <param name="habilitar">Actualiza el estado de uso de un registro (Habilitado-Disponible / Deshabilitado-NoDisponible)</param>
        /// <returns></returns>
        private RetornoOperacion editarCalificacion(int id_tabla, int id_registro, byte calificacion_promedio, string comentario, string email_calificante, int id_contacto, int id_usuario, bool habilitar)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para actualizar una calificación
            object[] param = { 2, this._id_calificacion, id_tabla, id_registro, calificacion_promedio, comentario, email_calificante, id_contacto, id_usuario, habilitar, "", "" };
            //Actualiza el registro de calificación
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        #endregion

        #region Método Públicos
        /// <summary>
        /// Método que inserta una calificación
        /// </summary>
        /// <param name="id_tabla">Inserta el identificador de la entidad a calificar (Cliente, Operador o Transportista)</param>
        /// <param name="id_registro">Inserta el identificador del Cliente, Operador o Transportista a calificar</param>
        /// <param name="calificacion_promedio">Inserta el número de evaluación otorgado a un Cliente, Operador o Transportista</param>
        /// <param name="comentario">Inserta el mensaje de evaluación sobre un Cliente, Operador o Transportista</param>
        /// <param name="email_calificante">Inserta el correo electronico de la persona que realizó la evaluación</param>
        /// <param name="id_contacto">Inserta el identificador de la persona que realizo la evaluación</param>
        /// <param name="id_usuario">Inserta el identificador del usuario que realizo acciones sobre el registro</param>
        /// <returns></returns>
        public static RetornoOperacion InsertarCalificacion(int id_tabla, int id_registro, byte calificacion_promedio, string comentario, string email_calificante, int id_contacto, int id_usuario)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Creación del arreglo que almacena los datos necesarios para insertar una calificación
            object[] param = { 1, 0, id_tabla, id_registro, calificacion_promedio, comentario, email_calificante, id_contacto, id_usuario, true, "", "" };
            //Inserta el registro de calificación
            retorno = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(nom_sp, param);
            //Retorna al método el objeto retorno
            return retorno;
        }
        /// <summary>
        /// Método que actualiza una calificación
        /// </summary>
        /// <param name="id_tabla">Actualiza el identificador de la entidad a calificar (Cliente, Operador o Transportista)</param>
        /// <param name="id_registro">Actualiza el identificador del Cliente, Operador o Transportista a calificar</param>
        /// <param name="calificacion_promedio">Actualiza el número de evaluación otorgado a un Cliente, Operador o Transportista</param>
        /// <param name="comentario">Actualiza el mensaje de evaluación sobre un Cliente, Operador o Transportista</param>
        /// <param name="email_calificante">Actualiza el correo electrónico de la persona que realizó la evaluación</param>
        /// <param name="id_contacto">Actualiza el identificador de la persona que realizó la evaluación</param>
        /// <param name="id_usuario">Actualiza el identificador del usuario que realizó acciones sobre el registro</param>
        /// <returns></returns>
        public RetornoOperacion EditarCalificacion(int id_tabla, int id_registro, byte calificacion_promedio, string comentario, string email_calificante, int id_contacto, int id_usuario)
        {
            //Retorna al método el método que realiza la actualización de una calificación
            return this.editarCalificacion(id_tabla, id_registro, calificacion_promedio, comentario, email_calificante, id_contacto, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que cambia el estado de uso de un registro
        /// </summary>
        /// <param name="id_usuario">Identifica al usuario que Deshabilito una calificación</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitarCalificación(int id_usuario,int id_contacto)
        {
            //Retorna al método el método que realiza la actualización de una calificación
            return this.editarCalificacion(this._id_tabla, this._id_registro, this._calificacion_promedio, this._comentario, this._email_calificante, id_contacto, id_usuario, false);
        }
        /// <summary>
        /// Método encargado de actualizar los valores de los atributos
        /// </summary>
        /// <returns></returns>
        public bool ActualizaCalificacion()
        {
            //Retorna el método que asigna valores a los atributos.
            return this.cargaAtributos(this._id_calificacion);
        }
        /// <summary>
        /// Método que obtiene el promedio asigndo a una entidad
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <returns></returns>
        public static byte ObtieneEntidad(int id_tabla, int id_registro)
        {
            //Creación de la tabla que almacenara la calificación
            byte Entidad = 0;
            //Creación del areglo que realiza la consulta de la calificación
            object[] param = { 4, 0, id_tabla, id_registro, 0, "", "", 0, 0, false, "", "" };
            
            //Almacena en un Dataset el resultado del método que realiza la busuqeda de calificación
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    Entidad = (from DataRow r in DS.Tables[0].Rows
                               select r.Field<byte>("Promedio")).FirstOrDefault();

            }
            //Devuelve el resultado al método
            return Entidad;
        }
        /// <summary>
        /// Método que obtiene el numero de comentarios calificaciones de una entidad
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <returns></returns>
        public static int ObtieneNumeroComentarios(int id_tabla, int id_registro)
        {
            //Creación de la tabla que almacenara el numero de comentarios
            int Comentarios = 0;
            //Creación del areglo que realiza la consulta el numero de comentarios
            object[] param = { 5, 0, id_tabla, id_registro, 0, "", "", 0, 0, false, "", "" };

            //Almacena en un Dataset el resultado del método que realiza la busuqeda de costos de caseta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    Comentarios = (from DataRow r in DS.Tables[0].Rows
                                   select r.Field<int>("Comentarios")).FirstOrDefault();

            }
            //Devuelve el resultado al método
            return Comentarios;
        }
        /// <summary>
        /// Método que obtiene el numero de comentarios calificaciones de una entidad
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <returns></returns>
        public static int ObtieneNumeroComentariosConcepto(int id_tabla, int id_registro)
        {
            //Creación de la tabla que almacenara el numero de comentarios
            int ComentariosConcepto = 0;
            //Creación del areglo que realiza la consulta el numero de comentarios
            object[] param = { 6, 0, id_tabla, id_registro, 0, "", "", 0, 0, false, "", "" };

            //Almacena en un Dataset el resultado del método que realiza la busuqeda de costos de caseta
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    ComentariosConcepto = (from DataRow r in DS.Tables[0].Rows
                                           select r.Field<int>("ComentariosConcepto")).FirstOrDefault();

            }
            //Devuelve el resultado al método
            return ComentariosConcepto;
        }
        /// <summary>
        /// Método que actualiza la calificación y los comentarios
        /// </summary>
        /// <param name="calificacion_promedio"></param>
        /// <param name="comentario"></param>
        /// <param name="id_contacto"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaComentario(string comentario, int id_contacto,int id_usuario)
        {

            return this.editarCalificacion(this._id_tabla, this._id_registro, this._calificacion_promedio, comentario, this._email_calificante, id_contacto, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que actualiza la calificación
        /// </summary>
        /// <param name="calificacion_promedio"></param>
        /// <param name="id_contacto"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ActualizaCalificacion(byte calificacion_promedio, int id_contacto, int id_usuario)
        {
            return this.editarCalificacion(this._id_tabla, this._id_registro, calificacion_promedio, this._comentario, this._email_calificante, id_contacto, id_usuario, this._habilitar);
        }
        /// <summary>
        /// Método que obtiene las calificaciones dadas a una entidad
        /// </summary>
        /// <param name="id_tabla">Identifica la entidad evaluada (operador,servicio,compañia)</param>
        /// <param name="id_registro">Identifica ol operador o al servicio o a la compañia evaluada</param>
        /// <returns></returns>
        public static DataTable ObtieneHistorialCalificacionEntidad(int id_tabla, int id_registro)
        {
            //Creación del datatable
            DataTable dtHistorial = null;
            //Creación del objeto que almacena los datos necesarios para realizar la busqueda de un registro
            object[] param = { 6, 0, id_tabla, id_registro, 0, "", "", 0, 0, false, "", "" };
            //Realiza la busqueda del registro
            using (DataSet DS = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(nom_sp, param))
            {
                //Valida los datos del dataset (que existan)
                if (Validacion.ValidaOrigenDatos(DS, "Table"))
                    //ASigna a la tabla los valores del dataset
                    dtHistorial = DS.Tables["Table"];
            }
            //Retorna al método el la tabla dtCalificacionDetalle
            return dtHistorial;
        }
        #endregion
    }
}
