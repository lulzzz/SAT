using SAT_CL.Global;
using System;
using System.Data;
using System.IO;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;
using System.Data;
using System.Text;
namespace SAT_CL.Notificacion
{
    /// <summary>
    /// Clase encargada de todas las operaciones relacionadas con los Notificacions
    /// </summary>
    public class Notificacion : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo encargado de Almacenar el Nombre del SP
        /// </summary>
        private static string _nom_sp = "notificacion.sp_notificacion_tn";

        private int _id_notificacion;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Notificación
        /// </summary>
        public int id_notificacion { get { return this._id_notificacion; } }

        private int _id_contacto;
        /// <summary>
        /// Atributo encargado de Almacenar el Id  de Contacto
        /// </summary>
        public int id_contacto { get { return this._id_contacto; } }

        private int _id_compania_emisor;
        /// <summary>
        /// Atributo encargado de Almacenar la Compañia Emisor
        /// </summary>
        public int id_compania_emisor { get { return this._id_compania_emisor; } }

        private int _id_compania_cliente;
        /// <summary>
        /// Atributo encargado de Almacenar la Compañia Cliente
        /// </summary>
        public int id_compania_cliente { get { return this._id_compania_cliente; } }

        private int _id_tabla;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Tabla
        /// </summary>
        public int id_tabla { get { return this._id_tabla; } }

        private int _id_registro;
        /// <summary>
        /// Atributo encargado de Almacenar el Id de Registro
        /// </summary>
        public int id_registro { get { return this._id_registro; } }
        private bool _habilitar;
        /// <summary>
        /// Atributo encargado de Almacenar el Estado del Registro
        /// </summary>
        public bool habilitar { get { return this._habilitar; } }


        #endregion

        #region Constructores

        /// <summary>
        /// Constructor encargado de Inicializar los Atributos por Defecto
        /// </summary>
        public Notificacion()
        {   //Invocando Método de Carga
            cargaAtributosInstancia();
        }
        /// <summary>
        /// Constructor encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id Registro</param>
        public Notificacion(int id_registro)
        {   //Invocando Método de Carga
            cargaAtributosInstancia(id_registro);
        }

        #endregion

        #region Destructores

        /// <summary>
        /// Destructor de la Clase
        /// </summary>
        ~Notificacion()
        {
            Dispose(false);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos por Defecto
        /// </summary>
        private void cargaAtributosInstancia()
        {
            //Asignando Valores
            this._id_notificacion = 0;
            this._id_contacto = 0;
            this._id_compania_emisor = 0;
            this._id_compania_cliente = 0;
            this._id_tabla = 0;
            this._id_registro = 0;
            this._habilitar = false;

        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Atributos dado un Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <returns></returns>
        private bool cargaAtributosInstancia(int id_registro)
        {   //Declarando Objeto de Retorno
            bool result = false;
            //Armando Arreglo de Parametros
            object[] param = { 3, id_registro, 0, 0, 0, 0, 0, 0, false, "", "" };
            //Obteniendo Resultado del SP
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   //Recorriendo cada Fila
                    foreach (DataRow dr in ds.Tables["Table"].Rows)
                    {
                        //Asignando Valores
                        this._id_notificacion = Convert.ToInt32(dr["Id"]);
                        this._id_contacto = Convert.ToInt32(dr["IdContacto"]);
                        this._id_compania_emisor = Convert.ToInt32(dr["IdCompaniaEmisor"]);
                        this._id_compania_cliente = Convert.ToInt32(dr["IdCompaniaCliente"]);
                        this._id_tabla = Convert.ToInt32(dr["IdTabla"]);
                        this._id_registro = Convert.ToInt32(dr["IdRegistro"]);
                        this._habilitar = Convert.ToBoolean(dr["Habilitar"]);
                        //Asignando Resultado Positivo
                        result = true;
                    }
                }
                //Devolviendo resultado Obtenido
                return result;
            }
        }

        /// <summary>
        /// Método Privado que actualiza la Notificacion
        /// </summary>
        /// <param name="id_contacto">Id Contacto</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="id_compania_cliente">Id Compañia Cliente</param>
        /// <param name="id_tabla">Id tabla</param>
        /// <param name="id_registro">Id Resgistro</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <param name="habilitar">Habilitar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaRegistros(int id_contacto, int id_compania_emisor, int id_compania_cliente, int id_tabla, int id_registro, int id_usuario, bool habilitar)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 2, this._id_notificacion, id_contacto, id_compania_emisor, id_compania_cliente, id_tabla, id_registro, id_usuario, habilitar, "", "" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Envia un Correo de Tipo Notificación
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="remitente">Remitente</param>
        /// <param name="asunto">Asunto del Correo</param>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <param name="pie">Pie de la Plantilla</param>
        /// <param name="destinatarios">Destinatario</param>
        /// <param name="url_calificacion">URL Calificacion</param>
        ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        private static RetornoOperacion EnviaArchivosEmail(int id_compania_emisor, string remitente, string asunto, string encabezado, string titulo, string subtitulo,
                                               string tituloCuerpo, string cuerpo, DataTable mitCuerpo, string actualBase64, string realizadoBase64, string sinRealizarBase64, string pie, string[] destinatarios,
                                               string queryStringCalificar)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Creando mensaje personalizado para CFDI
            string mensajeCFDI = generaMensajeEmail(encabezado, titulo, subtitulo, tituloCuerpo, cuerpo, mitCuerpo, actualBase64, realizadoBase64, sinRealizarBase64, pie, queryStringCalificar);

            //Enviamos Email
            //Instanciando Correo Electronico
            using (Correo email = new Correo(remitente, destinatarios, asunto, mensajeCFDI, true))
            {

                //Enviando Correo Electronico
                bool enviar = email.Enviar();

                //Si no se envío el mensaje
                if (!enviar)
                {
                    string errores = "";
                    //Recorriendo los errores del envío
                    foreach (string error in email.Errores)
                        //Añadiendo errores a la lista
                        errores = errores + error + "<br />";
                    resultado = new RetornoOperacion(errores);
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Genera el mensaje del contenido de correo en base a la plantilla predefinida para esta opción de Notificación
        /// </summary>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo del Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <param name="pie">Pie de la Plantilla</param>
        ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        protected static string generaMensajeEmail(string encabezado, string titulo, string subtitulo, string tituloCuerpo, string cuerpo, DataTable mitCuerpo, string actualBase64, string realizadoBase64,
            string sinRealizarBase64, string pie, string queryStringCalificar)
        {
            //Declaramos Variable Retorno
            cuerpo = "";
            //Formato redeterminado
            string formato = File.ReadAllText(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + "Notificacion.html");           
            //Si se Desea Registrar una Tabla en el Cuerpo
            if (mitCuerpo != null)
            {
                //Obtenmos HTML de la Tabla
                cuerpo = DevuelveTablaAHTML(mitCuerpo, actualBase64, realizadoBase64, sinRealizarBase64);
            }
            //Obteniendo las imagenes a incluir en base64
            string estrellasCalificar = Convert.ToBase64String(File.ReadAllBytes(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Imagenes Generales SAT", 0) + ("Estrella5.png")));
      
            //Declarando objeto de retorno
            return formato.Replace("{0}", encabezado).Replace("{1}", titulo).Replace("{2}", subtitulo).Replace("{3}", tituloCuerpo).Replace("{4}", cuerpo).Replace("{5}", pie).Replace("{6}", queryStringCalificar).Replace("{7}", estrellasCalificar);

        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inserta una Notificación
        /// </summary>
        /// <param name="id_contacto">Id Contacto</param>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="id_compania_cliente">Id Compañia Cliente</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion InsertaNotificacion(int id_contacto, int id_compania_emisor, int id_compania_cliente, int id_tabla, int id_registro, int id_usuario)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Armando Arreglo de Parametros
            object[] param = { 1, 0, id_contacto, id_compania_emisor, id_compania_cliente, id_tabla, id_registro, id_usuario, true,"","" };
            //Obteniendo Resultado del SP
            result = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nom_sp, param);
            //Devolviendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método encargado de Editar una Notificación
        /// </summary>
        /// <param name="id_contacto">Id Contacto</param>
        /// <param name="id_compania_emisor">Id Compania Emisor</param>
        /// <param name="id_compania_cliente">Id Compania Cliente</param>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public RetornoOperacion EditaNotificacion(int id_contacto, int id_compania_emisor, int id_compania_cliente, int id_tabla, int id_registro, int id_usuario)
        {   //Invocando Método de Actualización
            return this.actualizaRegistros(id_contacto, id_compania_emisor, id_compania_cliente, id_tabla, id_registro, id_usuario, true);
        }

        /// <summary>
        /// Método Público encargado de Deshabilitar una Notificación
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaNotificacion(int id_usuario)
        {  
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Creamos la transacción 
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Obtenemos Detalles
                using (DataTable mitDetalles = DetalleNotificacion.CargaDetalleNotificacion(this._id_notificacion))
                {
                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mitDetalles))
                    {
                        //Recorremos cada uno de los Detalles
                        foreach (DataRow r in mitDetalles.Rows)
                        {
                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciamos Detalle
                                using (DetalleNotificacion objDetalle = new DetalleNotificacion(r.Field<int>("Id")))
                                {
                                    //Deshabilitamos Detalle
                                    resultado = objDetalle.DeshabilitarDetallleNotificacion(id_usuario);
                                }
                            }
                            else
                                break;
                        }

                    }
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Invocando Método de Actualización
                    resultado = this.actualizaRegistros(this._id_contacto, this._id_compania_emisor, this._id_compania_cliente, this._id_tabla, this._id_registro, id_usuario, false);
                }
                //Validamos resultado
                if(resultado.OperacionExitosa)
                {
                    scope.Complete();
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Método Público encargado de Actualizar la Notificación
        /// </summary>
        /// <returns></returns>
        public bool ActualizaNotificacion()
        {   //Invocando Método de Carga
            return this.cargaAtributosInstancia(this._id_notificacion);
        }


        /// <summary>
        /// Carga las Notificaciones
        /// </summary>
        /// <param name="id_compania_emisor">Compañia Emisora</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_tabla">Id Tabla (Ubicacion, Tipo Evento, </param>
        /// <param name="id_registro"></param>
        /// <param name="id_tipo_vento"></param>
        /// <returns></returns>
        public static DataTable CargaNotificaciones(int id_compania_emisor, int id_cliente, int id_tabla, int id_registro, int id_tipo_vento)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //Armando Objeto de Parametros
            object[] param = {4, 0, 0, id_compania_emisor, id_cliente, id_tabla, id_registro, 0, false, id_tipo_vento, ""  };

            //Realizando la consulta
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nom_sp, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando a objeto de retorno
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }

        /// <summary>
        /// Enviamos Correo de Notificaciones 
        /// </summary>
        /// <param name="id_compania_emisor">Id Compañia Emisor</param>
        /// <param name="asunto">Asunto del Mensaje</param>
        /// <param name="id_cliente">Id Cliente</param>
        /// <param name="id_tabla">Id Tabla (Ubicacion=15)</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_tipo_evento">Tipo de Evento a Realizar</param>
        /// <param name="encabezado">Encabezado de la Plantillas</param>
        /// <param name="titulo">Titulo de la Plantilla</param>
        /// <param name="subtitulo">Subtitulo de la Plantilla</param>
        /// <param name="tituloCuerpo">Titulo Cuerpo</param>
        /// <param name="cuerpo">Cuerpo de la Plantilla</param>
        /// <param name="mitCuerpo">Tabla del Cuerpo de la PLantilla</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
         ///<param name="queryStringCalificar">Enviamos por query string la URL de Calificar</param>
        /// <returns></returns>
        public static void EnviaCorreoNotificaciones(int id_compania_emisor, string asunto, int id_cliente, int id_tabla, int id_registro, int id_tipo_evento,
                                                     string encabezado, string titulo, string subtitulo, string tituloCuerpo, string cuerpo, DataTable mitCuerpo,
                                                     string actualBase64, string realizadoBase64, string sinRealizarBase64, string queryStringCalificar)
        {
            //Declaramos Variables 
            RetornoOperacion resultado = new RetornoOperacion();
            //Intsnamos Tipo de Evento
            using (TipoEventoNotificacion tipoEventoNotificacion = new TipoEventoNotificacion(id_tipo_evento))
            {
                //Cargamos Notificaciones de acuerdo a los criterios Obtenidos
                using (DataTable mitNotificaciones = CargaNotificaciones(id_compania_emisor, id_cliente, id_tabla, id_registro, id_tipo_evento))
                {
                    //Validamos que existan Notificaciones
                    if (Validacion.ValidaOrigenDatos(mitNotificaciones))
                    {
                        //Recorremos cada uno de los Correos
                        foreach (DataRow r in mitNotificaciones.Rows)
                        {
                            //Declaramos Variable Destinatario
                            string[] destinatario = new string []{r.Field<string>("Correo")};
                            //Enviamos E-mail
                            resultado =EnviaArchivosEmail( id_compania_emisor, destinatario[0], asunto, encabezado, titulo, subtitulo, tituloCuerpo,cuerpo, mitCuerpo, actualBase64,
                                      realizadoBase64, sinRealizarBase64, tipoEventoNotificacion.mensaje, destinatario, queryStringCalificar+ "&idC="+ r.Field<int>("Id"));
                        }                       
                    }
                }
            }
        }

   
        /// <summary>
        /// Método encargado de Devolver una Tabla HTML a Partir de un DataTable
        /// </summary>
        /// <param name="mit">Tabla a convertir</param>
        /// <param name="actualBase64">Imagen que actualmente el Evento, Parada, etc realizada, encuentra</param>
        /// <param name="realizadoBase64">Imagen que muestra los Eventos, Paradas realizados</param>
        /// <param name="sinRealizarBase64">Imagen que muestra los Eventos, Paaradas sin realizar</param>
        /// <returns></returns>
        public static string DevuelveTablaAHTML(DataTable mit, string actualBase64, string realizadoBase64, string sinRealizarBase64)
        {

            //Declaramos Variables
            StringBuilder html = new StringBuilder();

            //Validamos Origen de Datos
            if (Validacion.ValidaOrigenDatos(mit))
            {
                //Creamos el encabezado de la Tabla
                html.Append("<table>");

                //Creamos el encabezado de la fila
                html.Append("<tr>");
                foreach (DataColumn column in mit.Columns)
                {
                    html.Append("<th>");
                    html.Append(column.ColumnName);
                    html.Append("</th>");
                }
                html.Append("</tr>");

                //Construimos la información de cada una de la Filas
                foreach (DataRow row in mit.Rows)
                {
                    html.Append("<tr>");
                    foreach (DataColumn column in mit.Columns)
                    {
                        html.Append("<td>");
                        //Validamos si es Columna de Imagen
                        //Imagen de Eventos, Paradas, etc. Sin Realizar
                        if (row[column.ColumnName].ToString() == "SinRealizar")
                        {
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + sinRealizarBase64 + "/></DIR></DIR>");
                        }
                            //Imagen de Eventos, Paradas, etc Actual.
                        else if (row[column.ColumnName].ToString() == "Actual")
                        {
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + actualBase64 + "/></DIR></DIR>");
                        }
                        else if (row[column.ColumnName].ToString() == "Realizado")
                        {
                            //Imagen de Eventos, Paradas Realizadas.
                            html.Append("<DIR><DIR><img src=data:image/png;base64," + realizadoBase64 + "/></DIR></DIR>");
                        }
                        else
                        {
                            html.Append(row[column.ColumnName]);
                        }
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                }

                //Cerramo etiqueta de creación de Tabla
                html.Append("</table>");
            }
            //Devolvemos String HTML de la Tabla
            return html.ToString().Replace("src=", "src=\"").ToString().Replace("==/>", "==\">");
        }

        #endregion


    }
}

