using System;
using System.Data;
using TSDK.Base;
using TSDK.Datos;
using System.Transactions;
namespace SAT_CL.Autorizacion
{
    /// <summary>
    /// Proprrciona los metodos para Administrar las autorizaciones realizadas.
    /// </summary>
    public class AutorizacionRealizada : Disposable
    {
        #region Propiedades
        /// <summary>
        /// Define el nombre del store procedure encargado de realizar las acciones en BD
        /// </summary>
        private static string _nombre_stored_procedure = "autorizacion.sp_autorizacion_realizada_tar";


        private int _id_autorizacion_realizada;
        /// <summary>
        /// Id Autorizacion Realizada
        /// </summary>
        public int id_autorizacion_realizada
        {
            get { return _id_autorizacion_realizada; }
        }

        private int _id_tabla;
        /// <summary>
        /// Id Tabla
        /// </summary>
        public int id_tabla
        {
            get { return _id_tabla; }

        }

        private int _id_registro;
        /// <summary>
        /// Id del registro
        /// </summary>
        public int id_registro
        {
            get { return _id_registro; }
        }

        private int _id_autorizacion_detalle_bloque_responsable;
        /// <summary>
        /// Id Autorizacion Responsable
        /// </summary>
        public int id_autorizacion_detalle_bloque_responsable
        {
            get { return _id_autorizacion_detalle_bloque_responsable; }
        }

        private bool? _bit_confirmacion;
        /// <summary>
        /// Estado de la autorizacion
        /// </summary>
        public bool? bit_confirmacion
        {
            get { return _bit_confirmacion; }
        }

        private DateTime _fecha_confirmacion;
        /// <summary>
        /// Fecha de la confirmacion
        /// </summary>
        public DateTime fecha_confirmacion
        {
            get { return _fecha_confirmacion; }
        }

        private bool _habilitar;
        /// <summary>
        /// Estado de una Autorizacion Realizada
        /// </summary>
        public bool habilitar
        {
            get { return _habilitar; }
        }

        #endregion

        #region Destructor
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~AutorizacionRealizada()
        {
            Dispose(false);
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Genera una instancia de tipo Autorizacion Realizada
        /// </summary>
        public AutorizacionRealizada()
        {
            _id_autorizacion_realizada = 0;
            _id_tabla = 0;
            _id_registro = 0;
            _id_autorizacion_detalle_bloque_responsable = 0;
            _bit_confirmacion = null;
            _fecha_confirmacion = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            _habilitar = false;
        }
        /// <summary>
        /// Genera una nueva instancia de tipo Autorización Realizada dado un id 
        /// </summary>
        /// <param name="id_autorizacion_realizada"></param>
        public AutorizacionRealizada(int id_autorizacion_realizada)
        {
            //inicializamos el arreglo de parametros
            object[] param = { 3, id_autorizacion_realizada, 0, 0, 0, false, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, false, "", "" };

            //Obtenemos el origen de datos
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validamos origen dedatos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    foreach (DataRow r in ds.Tables["Table"].Rows)
                    {
                        _id_autorizacion_realizada = Convert.ToInt32(r["IdAutorizacionRealizada"]);
                        _id_tabla = Convert.ToInt32(r["IdTabla"]);
                        _id_registro = Convert.ToInt32(r["IdRegistro"]);
                        _id_autorizacion_detalle_bloque_responsable = Convert.ToInt32(r["IdAutorizacionDetalleBloque"]);
                        if (r["bitConfirmacion"] == null || r["bitConfirmacion"] == DBNull.Value)
                        {
                            _bit_confirmacion = null; 
                        }
                        else
                        {
                            _bit_confirmacion = Convert.ToBoolean(r["bitConfirmacion"]);
                        }
                        DateTime.TryParse(r["fechaConfirmacion"].ToString(), out _fecha_confirmacion);
                        _habilitar = Convert.ToBoolean(r["Habilitado"]);
                    }
                }
            }
        }

        #endregion

        #region Métodos Privados

        /*
        /// <summary>
        /// Genera un nuevo mensaje en formato HTML para envío vía e-mail de una autorización pendiente
        /// </summary>
        /// <param name="nombre_usuario_responsable">Nombre del usuario a quién se dirige el mensaje</param>
        /// <param name="tipo_autorizacion">Nombre del tipo de autorización</param>
        /// <param name="folio">No. de folio del registro por autorizar</param>
        /// <returns></returns>
        private static string creaMensajeEmail(string nombre_usuario_responsable, string tipo_autorizacion, string folio)
        { 
            //Instanciando administrador de estilos
            using (BibliotecaClasesBaseASP.clAdministradorCSS css = new BibliotecaClasesBaseASP.clAdministradorCSS(@CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Ruta Hoja Estilos e-mail")))
            {
                //Instanciando documento HTML
                using (clHTML documentoHTML = new clHTML(css.ObtieneDefinicionEstilo(".CuerpoDocumento")))
                {
                    //Contruyendo el mensaje a enviar
                    string mensaje = clHTML.CreaSpan("Solicitud de Autorización.", css.ObtieneDefinicionEstilo("h1")) + clHTML.CreaRetornoCarro(2) +
                                    clHTML.CreaSpan(nombre_usuario_responsable , css.ObtieneDefinicionEstilo("h2")) + clHTML.CreaRetornoCarro(2) +
                                    clHTML.CreaSpan("Tienes una autorización pendiente para ", css.ObtieneDefinicionEstilo(".Label")) + clHTML.CreaSpan("'"+tipo_autorizacion+"'", css.ObtieneDefinicionEstilo(".LabelNegrita")) +
                                    clHTML.CreaSpan(" con No. de Folio ", css.ObtieneDefinicionEstilo(".Label")) + clHTML.CreaSpan("'" + folio + "'", css.ObtieneDefinicionEstilo(".LabelNegrita")) + 
                                    clHTML.CreaSpan(". Haz clic ", css.ObtieneDefinicionEstilo(".Label")) + clHTML.CreaHiperVinculo(" <b>  aquí  </b> ", css.ObtieneDefinicionEstilo(".LinkButton"), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Dirección Aplicación Móvil")) + 
                                    clHTML.CreaSpan(" para dar tu visto bueno o rechazar la solicitud.", css.ObtieneDefinicionEstilo(".Label")) + clHTML.CreaRetornoCarro(2) +
                                    clHTML.CreaSpan("[ Este mensaje fue generado de manera automática mediante una herramienta de software propia del sistema, no es necesario enviar una respuesta al remitente ]", css.ObtieneDefinicionEstilo(".LabelNegrita"));

                    //Añadiendo el mensaje al documento 
                    documentoHTML.InsertaDivTag(mensaje, css.ObtieneDefinicionEstilo(".Mensaje"));

                    //Devolviendo documento HTML
                    return documentoHTML.ToString();
                }
            }
        }   */ 
         
        /// <summary>
        /// Metodo encargado de Editar una Autorizacion Realizada
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_autorizacion_detalle_bloque_responsable"></param>
        /// <param name="bit_confirmacion"></param>
        /// <param name="fecha_confirmacion"></param>
        /// <param name="id_usuario"></param>
        /// <param name="habilitar"></param>
        /// <returns></returns>
        private RetornoOperacion editaAutorizacionRealizada(int id_tabla, int id_registro, int id_autorizacion_detalle_bloque_responsable, bool? bit_confirmacion, DateTime fecha_confirmacion, int id_usuario, bool habilitar)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 2, this._id_autorizacion_realizada, id_tabla, id_registro, id_autorizacion_detalle_bloque_responsable, bit_confirmacion, Fecha.ConvierteDateTimeObjeto(fecha_confirmacion), id_usuario, habilitar, "", "" };

            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);

        }

        #endregion

        #region Metodos Publicos
        /// <summary>
        /// Metodo encargado de insertar un registro ligada a una transaccion
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_autorizacion_detalle_bloque_responsable"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public static RetornoOperacion InsertaAutorizacionRealizada(int id_tabla, int id_registro, int id_autorizacion_detalle_bloque_responsable, int id_usuario)
        {
            //Inicializando arreglo de parámetros
            object[] param = { 1, 0, id_tabla, id_registro, id_autorizacion_detalle_bloque_responsable, null, null, id_usuario, true, "", "" };
            //Realizando actualizacion
            return CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoObjeto(_nombre_stored_procedure, param);
        }

        /// <summary>
        /// Metodo encargado de editar una Autorizacion Realizada
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <param name="id_autorizacion_detalle_bloque_responsable"></param>
        /// <param name="bit_confirmacion"></param>
        /// <param name="fecha_confirmacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion EditaAutorizacionRealizada(int id_tabla, int id_registro, int id_autorizacion_detalle_bloque_responsable, bool? bit_confirmacion, DateTime fecha_confirmacion, int id_usuario)
        {
            return this.editaAutorizacionRealizada(id_tabla, id_registro, id_autorizacion_detalle_bloque_responsable, bit_confirmacion, fecha_confirmacion, id_usuario, this._habilitar);
        }

        /// <summary>
        /// Deshabilita una Autorizacion Realizada 
        /// </summary>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaAutorizacionRealizada(int id_usuario)
        {
            return this.editaAutorizacionRealizada(this._id_tabla, this._id_registro, this._id_autorizacion_detalle_bloque_responsable, this._bit_confirmacion, this._fecha_confirmacion, id_usuario, false);

        }

        /// <summary>
        /// Deshabilitamos Autorizaciones
        /// </summary>
        /// <param name="id_tabla">Id Tabla</param>
        /// <param name="id_registro">Id Registro</param>
        /// <param name="id_usuario">Id Usuario</param>
        /// <returns></returns>
        public static RetornoOperacion DeshabilitaAutorizacionRealizada(int id_tabla, int id_registro, int id_usuario)
        {
            //Declaramos Objeto Resutado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Cargamos Autorizaciones
            using (DataTable mitAutorizaiones = CargaAutorizacionesActivas(id_tabla, id_registro))
            {
                //Validamos Origen
                if (Validacion.ValidaOrigenDatos(mitAutorizaiones))
                {
                    //Creamos la transacción 
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Rrecorremos Autorizaciones
                        foreach (DataRow r in mitAutorizaiones.Rows)
                        {
                            //Instanciamos Autorizacion
                            using (AutorizacionRealizada obj = new AutorizacionRealizada(r.Field<int>("Id")))
                            {
                                //Deshabilitamos
                                resultado = obj.DeshabilitaAutorizacionRealizada(id_usuario);
                            }
                            //Validamos Resultado
                            if (!resultado.OperacionExitosa)
                            {
                                //Salimos del ciclo
                                break;
                            }
                        }
                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Finalizamos Transaccion
                            scope.Complete();
                        }
                    }

                }

            }
            //Devolvmemos Resultado
            return resultado;
        }
        

        /// <summary>
        /// Realiza la confirmación de la autorización, actualizando el valor de esta, así como la fecha
        /// </summary>
        /// <param name="confirmacion"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion ConfirmaAutorizacion(bool confirmacion, int id_usuario)
        { 
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que no se encuentre confirmada previamnete
            if (this._bit_confirmacion == null)
                //Actualizando autorización, indicando la fecha actual como fecha de confirmación
                resultado = editaAutorizacionRealizada(this._id_tabla, this._id_registro, this._id_autorizacion_detalle_bloque_responsable,
                                                        confirmacion, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), id_usuario, this._habilitar);
            //De lo contrario
            else
                resultado = new RetornoOperacion("La autorización ya ha sido confirmada previamente.");

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        ///Carga Autorizaciones Activas para su Deshabilitación
        /// </summary>
        /// <param name="id_tabla"></param>
        /// <param name="id_registro"></param>
        /// <returns></returns>
        public static DataTable CargaAutorizacionesActivas(int id_tabla, int id_registro)
        {
            //Definiendo objeto de retorno
            DataTable mit = null;

            //inicializamos el arreglo de parametros
            object[] param = { 4, 0, id_tabla, id_registro, 0, false, null, 0, false, "", "" };

            //Realizando la búsqueda en BD
            using (DataSet ds = CapaDatos.m_capaDeDatos.EjecutaProcAlmacenadoDataSet(_nombre_stored_procedure, param))
            {
                //Validando origen de datos
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                    //Asignando origen de datos
                    mit = ds.Tables["Table"];

                //Devolviendo resultado
                return mit;
            }
        }
        /*
        /// <summary>
        /// Realiza el armado y envío del mensaje de notificación de la solicitud de autorización
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EnviaNotificacionAutorizacion()
        { 
            //Definiendo objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion(this._id_autorizacion_realizada);

            //Instanciando la tabla a la que pertenece el registro por autorizar
            using (clTabla t = new clTabla(this._id_tabla))
            {
                //Si la tabla existe
                if (t.IdTabla > 0)
                {
                    //Instanciando el registro autorización bloque responsable al que pertenece
                    using (clAutorizacionDetalleBloqueResponsable r = new clAutorizacionDetalleBloqueResponsable(this._id_autorizacion_detalle_bloque_responsable))
                    {
                        //Instanciando al bloque correspondiente
                        using (clAutorizacionDetalleBloque b = new clAutorizacionDetalleBloque(r.id_autorizacion_detalle_bloque))
                        {
                            //Instanciando detalle de autorizacion
                            using (clAutorizacionDetalle d = new clAutorizacionDetalle(b.id_autorizacion_detalle))
                            {
                                //Instanciando autorización
                                using (clAutorizacion a = new clAutorizacion(d.id_autorizacion))
                                {
                                    //Instanciando el usuario al que se enviará la notificación
                                    using (Administracion.clUsuario u = new BibliotecaClasesCentralDB.Administracion.clUsuario(r.id_usuario_responsable))
                                    {
                                        //Definiendo variable de mensaje
                                        string mensaje = "";

                                        //En base al tipo de notificación definida al usuario
                                        //SMS
                                        if (r.bit_sms)
                                        {
                                            //Creando formato plano
                                        }
                                        //e-mail
                                        if (r.bit_email)
                                        {
                                            //Si el usuario solicitó que se envíe
                                            if (Convert.ToBoolean(u.Configuracion["E-mail Autorización"]))
                                            {
                                                //Creando formato HTML
                                                mensaje = creaMensajeEmail(u.nombre, a.descripcion, this._id_registro.ToString());

                                                //Instanciando correo electrónico
                                                using (clCorreo c = new clCorreo(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Remitente alertas e-mail"), u.email, "Solicitud de Autorización SIC", mensaje, true, System.Net.Mail.MailPriority.High))
                                                {
                                                    //Enviando el correo electrónico
                                                    bool envio = c.Enviar();
                                                    resultado = new RetornoOperacion(envio ? "El mensaje ha sido enviado correctamente." : "Se ha producido un error durante el envío.", envio);
                                                }
                                            }
                                            //De lo contrario, se considera exitoso por solicitud de no envío
                                            else
                                                resultado = new RetornoOperacion(this._id_autorizacion_realizada);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }            

            //Devolviendo resultado
            return resultado;
        }*/
        
        #endregion
    }
}
