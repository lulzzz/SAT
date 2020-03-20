using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Despacho;
using SAT_CL.Seguridad;
using System.Configuration;

namespace SAT_CL.Global
{
    public class NotificacionPush
    {
        #region Enumeraciones

        /// <summary>
        /// Tipos de notificacion para servicio
        /// </summary>
        public enum TipoNotificacionServicio
        {
            /// <summary>
            /// Nueva parada en el servicio actual
            /// </summary>
            NParadaServActual = 1,
            /// <summary>
            /// Eliminar parada del servicio actual
            /// </summary>
            EParadaServActual,
            /// <summary>
            /// Actualización de alguna parada del servicio actual
            /// </summary>
            AParadaServActual,
            /// <summary>
            /// Cualquier otra actualización en un servicio planeado
            /// </summary>
            ActualizacionServicio,
            /// <summary>
            /// Asignación de vale de diesel
            /// </summary>
            Diesel,
            /// <summary>
            /// Confirmación de depósito de anticipo
            /// </summary>
            AnticipoDepositado
        }

        #endregion

        #region Atributos

        /// <summary>
        /// URL del servidor Firebase Cloud Messaging a donde se enviarán las notificaciones push
        /// </summary>
        private string _url_servidor_fcm;
        /// <summary>
        /// Token Firebase Cloud Messaging habilitado para el servidor web que envía las notificaciones
        /// </summary>
        private string _server_token_fcm;


        #endregion

        #region Singleton

        /// <summary>
        /// Instancia única de la clase, la cual se inicializará hasta que se realice la primer petición de uso de la misma (no ocupará memoria hasta entonces)
        /// </summary>
        private static readonly Lazy<NotificacionPush> _instance = new Lazy<NotificacionPush>(() => new NotificacionPush());

        /// <summary>
        /// Obtiene la instancia única de la clase
        /// </summary>
        public static NotificacionPush Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        private NotificacionPush()
        {
            //Inicializando atributos
            _server_token_fcm = ConfigurationManager.AppSettings["FirebaseCM_SAT_DriverWebKey"];
            _url_servidor_fcm = ConfigurationManager.AppSettings["FirebaseCM_URL"];
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Notificación de un nuevo servicio asignado (nueva asignación a movimiento)
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id de Asignación de recurso</param>
        /// <returns></returns>
        public RetornoOperacion NuevoServicioAsignado(int id_movimiento_asignacion_recurso)
        {
            //Inicializando variables auxiliares
            string tokenFCM = "";
            int idCompania = 0, idRecurso = 0;
            MovimientoAsignacionRecurso.Tipo tipoRecurso = MovimientoAsignacionRecurso.Tipo.Operador;

            //Validando que la asignación pertenezca a un servicio
            RetornoOperacion resultado = validaMovimientoServicio(id_movimiento_asignacion_recurso);

            //Si es de un servicio
            if (resultado.OperacionExitosa)
            {
                //Validando asignación del recurso y obtención de token al que se enviará la notificación
                resultado = validaUsuarioSesionActivaRecurso(id_movimiento_asignacion_recurso, out idCompania, out idRecurso, out tipoRecurso, out tokenFCM);

                //Si hay datos suficientes
                if (resultado.OperacionExitosa)
                {
                    //Validando configuración de notificaciones
                    resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(idCompania);

                    //Si hay datos suficientes
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializando asignaciones totales
                        int totalAsignaciones = 0;
                        DateTime cita_inicio = Fecha.ObtieneFechaEstandarMexicoCentro();

                        //Recuperando asignaciones totales de servicio al usuario
                        using (DataTable mit = MovimientoAsignacionRecurso.CargaServiciosAsignadosAlRecurso(tipoRecurso, idRecurso, false))
                        {
                            //Si hay asignaciones
                            if (mit != null)
                            {
                                totalAsignaciones = mit.Rows.Count;
                                cita_inicio = Convert.ToDateTime(mit.Rows[0]["CitaOrigen"]);
                            }
                        }

                        //Si hay asignaciones
                        if (totalAsignaciones > 0)
                        {
                            //Definiendo datos del mensaje de la notificación (si hay mas de una asignación, se considera como notificación de baja prioridad dado que existe otra en curso)
                            var datos = new
                            {
                                Titulo = "¡Nuevo viaje asignado!",
                                Mensaje = String.Format(totalAsignaciones > 1 ? "Se ha añadido un viaje a la lista de pendientes." : "Inicia el '{0:dd-MM-yyyy HH:mm}'", cita_inicio),
                                TipoNotificacion = totalAsignaciones > 1 ? "NAsignacion" : "NAsignacionActivo"
                            };

                            //Enviando mensaje vía FCM
                            resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos);
                        }
                        //Si no hay asignaciones
                        else
                            resultado = new RetornoOperacion("No se pudo obtener el total de asignaciones.");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Notificación de asignación eliminada
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id de Asignanción de Recurso</param>
        /// <param name="viaje_activo">True para indicar que la asignación correspondia al viaje inmediato siguiente, de lo contrario false</param>
        /// <returns></returns>
        public RetornoOperacion EliminaAsignacionServicio(int id_movimiento_asignacion_recurso, bool viaje_activo)
        {
            //Inicializando variables auxiliares
            string tokenFCM = "";
            int idCompania = 0, idRecurso = 0;
            MovimientoAsignacionRecurso.Tipo tipoRecurso = MovimientoAsignacionRecurso.Tipo.Operador;

            //Validando que la asignación pertenezca a un servicio
            RetornoOperacion resultado = validaMovimientoServicio(id_movimiento_asignacion_recurso);

            //Si es de un servicio
            if (resultado.OperacionExitosa)
            {
                //Obtención de token al que se enviará la notificación
                resultado = validaUsuarioSesionActivaRecurso(id_movimiento_asignacion_recurso, out idCompania, out idRecurso, out tipoRecurso, out tokenFCM);

                //Si hay datos suficientes
                if (resultado.OperacionExitosa)
                {
                    //Validando configuración de notificaciones
                    resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(idCompania);

                    //Si se configuró el uso de aplicación móvil
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializando asignaciones totales
                        int totalAsignaciones = 0;

                        //Recuperando asignaciones totales de servicio al usuario
                        using (DataTable mit = MovimientoAsignacionRecurso.CargaServiciosAsignadosAlRecurso(tipoRecurso, idRecurso, false))
                        {
                            //Si hay asignaciones
                            if (mit != null)
                            {
                                totalAsignaciones = mit.Rows.Count;
                            }
                        }

                        //Definiendo datos del mensaje de la notificación (si no hay asignaciones se informa que no tiene servicios asignados, si queda alguna otra pendiente se informa)
                        //Si hay asignaciones pendientes
                        if (totalAsignaciones > 0)
                        {
                            var datos1 = new
                            {
                                Titulo = "¡Viaje Reemplazado!",
                                Mensaje = "El viaje activo fue cambiado, consulte los detalles.",
                                TipoNotificacion = "NAsignacionActivo"
                            };

                            var datos2 = new
                            {
                                Titulo = "¡Viajes pendientes actualizados!",
                                Mensaje = "La lista de pendientes fue modificada, consulte los detalles.",
                                TipoNotificacion = "EAsignacion"
                            };

                            //Enviando mensaje vía FCM
                            resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, viaje_activo ? datos1 : datos2);
                        }
                        //Si ya no hay asignaciones
                        else
                        {
                            var datos = new
                            {
                                Titulo = "¡Sin viajes pendientes!",
                                Mensaje = "Por el momento no tiene viajes asignados.",
                                TipoNotificacion = "EAsignacionActivo"
                            };

                            //Enviando mensaje vía FCM
                            resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos);
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Notificación de cambios en alguna parada de servicio asignado (cambio de citas, nueva parada y eliminar parada)
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id de Asignanción de Recurso</param>
        /// <param name="tipo">Tipo de actualización</param>
        /// <returns></returns>
        public RetornoOperacion ActualizaInformacionServicio(int id_movimiento_asignacion_recurso, TipoNotificacionServicio tipo)
        {
            //Inicializando variables auxiliares
            string tokenFCM = "";
            int idCompania = 0, idRecurso = 0;
            MovimientoAsignacionRecurso.Tipo tipoRecurso = MovimientoAsignacionRecurso.Tipo.Operador;

            //Validando que la asignación pertenezca a un servicio
            RetornoOperacion resultado = validaMovimientoServicio(id_movimiento_asignacion_recurso);

            //Si es de un servicio
            if (resultado.OperacionExitosa)
            {
                //Obtención de token al que se enviará la notificación
                resultado = validaUsuarioSesionActivaRecurso(id_movimiento_asignacion_recurso, out idCompania, out idRecurso, out tipoRecurso, out tokenFCM);

                //Si hay datos suficientes
                if (resultado.OperacionExitosa)
                {
                    //Validando configuración de notificaciones
                    resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(idCompania);

                    //Si se configuró el uso de aplicación móvil
                    if (resultado.OperacionExitosa)
                    {
                        //Determinando el tipo de actualización realizada
                        switch (tipo)
                        {
                            case TipoNotificacionServicio.NParadaServActual:
                                var datos = new
                                    {
                                        Titulo = "¡Parada añadida al viaje actual!",
                                        Mensaje = "El viaje en curso fue modificado, consulte los detalles.",
                                        TipoNotificacion = "NParada"
                                    };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos);
                                break;
                            case TipoNotificacionServicio.EParadaServActual:
                                var datos1 = new
                                    {
                                        Titulo = "¡Parada eliminada del viaje actual!",
                                        Mensaje = "El viaje en curso fue modificado, consulte los detalles.",
                                        TipoNotificacion = "EParada"
                                    };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos1);
                                break;
                            case TipoNotificacionServicio.AParadaServActual:
                                var datos2 = new
                                    {
                                        Titulo = "¡Parada modificada en viaje actual!",
                                        Mensaje = "El viaje en curso fue modificado, consulte los detalles.",
                                        TipoNotificacion = "AParada"
                                    };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos2);
                                break;
                            case TipoNotificacionServicio.ActualizacionServicio:
                                var datos3 = new
                                    {
                                        Titulo = "¡Viaje pendiente actualizado!",
                                        Mensaje = "Un viaje asignado fue modificado, consulte los detalles.",
                                        TipoNotificacion = "AAsignacion"
                                    };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos3);
                                break;

                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Notificación de asignación de vale de diesel y depósito de anticipos
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id de Asignanción de Recurso</param>
        /// <param name="tipo">Tipo de actualización</param>
        ///<param name="fecha">Fecha Depósito o Fecha de Carga</param>
        ///<param name="cantidad">Cantidad Depositada o Litro</param>
        /// <returns></returns>
        public RetornoOperacion NotificacionDepositoAnticiposYDiesel(int id_movimiento_asignacion_recurso, TipoNotificacionServicio tipo, DateTime fecha, decimal cantidad)
        {
            //Inicializando variables auxiliares
            string tokenFCM = "";
            int idCompania = 0, idRecurso = 0, idServicio = 0;
            MovimientoAsignacionRecurso.Tipo tipoRecurso = MovimientoAsignacionRecurso.Tipo.Operador;

            //Validando que la asignación pertenezca a un servicio
            RetornoOperacion resultado = validaMovimientoServicio(id_movimiento_asignacion_recurso, out idServicio);

            //Si es de un servicio
            if (resultado.OperacionExitosa)
            {
                //Obtención de token al que se enviará la notificación
                resultado = validaUsuarioSesionActivaRecurso(id_movimiento_asignacion_recurso, out idCompania, out idRecurso, out tipoRecurso, out tokenFCM);

                //Si hay datos suficientes
                if (resultado.OperacionExitosa)
                {
                    //Validando configuración de notificaciones
                    resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(idCompania);

                    //Si se configuró el uso de aplicación móvil
                    if (resultado.OperacionExitosa)
                    {
                        //Determinando el tipo de actualización realizada
                        switch (tipo)
                        {
                            case TipoNotificacionServicio.Diesel:
                                var datos = new
                                {
                                    Titulo = "¡Vale de diesel asignado!",
                                    Mensaje = string.Format("{0:f2} litros, Carga el {1:dd/MM/yyyy HH:mm}",cantidad, fecha),
                                    IdServicio = idServicio,
                                    TipoNotificacion = "NDiesel"
                                };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos);
                                break;
                            case TipoNotificacionServicio.AnticipoDepositado:
                                var datos1 = new
                                {
                                    Titulo = "¡Depósito de anticipo confirmado!",
                                    Mensaje = string.Format("{0:c}, a las {1:dd/MM/yyyy HH:mm}", cantidad, fecha),
                                    IdServicio = idServicio,
                                    TipoNotificacion = "NDeposito"
                                };

                                //Enviando mensaje vía FCM
                                resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos1);
                                break;
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Notificación para pedir ubicación actual al dispositivo
        /// </summary>
        /// <param name="tipo_recurso">Tipo de Recurso</param>
        /// <param name="id_recurso">Id de Entidad acorde al tipo indicado</param>
        /// <returns></returns>
        public RetornoOperacion NotificacionPeticionUbicacion(MovimientoAsignacionRecurso.Tipo tipo_recurso, int id_recurso)
        {
            //Declrando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando variables auxiliares
            string tokenFCM = "";
            int idCompania = 0;
            Usuario usuario = null;

            //En base al tipo de recurso
            switch (tipo_recurso)
            {
                case MovimientoAsignacionRecurso.Tipo.Operador:
                    //Instanciando el recurso
                    using (Operador op = new Operador(id_recurso))
                    {
                        //Guardando id de compañía
                        idCompania = op.id_compania_emisor;
                        //Recuperando Usuario correspondiente
                        usuario = Usuario.ObtieneUsuarioAsignadoOperador(op.id_operador);
                    }
                    break;
                case MovimientoAsignacionRecurso.Tipo.Unidad:
                    //Instanciando Unidad
                    using (Unidad unidad = new Unidad(id_recurso))
                    {
                        //recuperando Id de Compañía
                        idCompania = unidad.id_compania_emisor;

                        //Instanciando tipo de unidad
                        using (UnidadTipo tipo = new UnidadTipo(unidad.id_tipo_unidad))
                        {
                            //Si la unidad es motriz
                            if (tipo.bit_motriz)
                            {
                                //Determiando la propiedad de unidad
                                //Si no es propia
                                if(unidad.bit_no_propia)
                                {

                                }
                                //Si lo es
                                else
                                {
                                    //Buscando asignación de operador activa
                                    int idOperador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(id_recurso);
                                    //Recuperando Usuario correspondiente
                                    usuario = Usuario.ObtieneUsuarioAsignadoOperador(idOperador);
                                }
                            }
                        }
                    }
                    break;
            }

            //Si hay compañía y usuario
            if (idCompania > 0 && usuario != null)
            {
                //Validando configuración de notificaciones
                resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(idCompania);

                //Si se configuró el uso de aplicación móvil
                if (resultado.OperacionExitosa)
                {
                    //Validando sesión activa en algún dispositivo
                    if (Validacion.ValidaOrigenDatos(UsuarioSesion.ObtieneSesionesActivasUsuario(usuario.id_usuario, UsuarioSesion.TipoDispositivo.Android)))
                    {
                        //Recuperando referencia del token de usuario
                        using (DataTable mit = Referencia.CargaReferencias(usuario.id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(idCompania, 30, "Token FCM", 0, "Configuración")))
                            tokenFCM = mit != null ? mit.Rows[0]["Valor"].ToString() : "";

                        //Si se obtuvo el token del usuario correspondiente
                        if (tokenFCM != "")
                        {
                            //Armando mensaje de petición
                            var datos = new
                            {
                                Titulo = "Comando de Ubicación",
                                Mensaje = "N/D",
                                TipoNotificacion = "Ubicacion"
                            };

                            //Enviando mensaje vía FCM
                            resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Alta, datos);

                        }
                        //Si no hay token
                        else
                            resultado = new RetornoOperacion("El token del dispositivo del usuario debe ser actualizado.");
                    }
                    //Si no hay sesión activa
                    else
                        resultado = new RetornoOperacion("El usuario no cuenta con una sesión activa en dispositivo móvil.");
                }
            }
            //Si no hay compañía o usuario
            else
                resultado = new RetornoOperacion("No se localizó la compañía o el usuario asignado al recurso.");

            //Si el resultado es correcto
            if (resultado.OperacionExitosa)
                resultado = new RetornoOperacion("Petición enviada correctamente, el tiempo de respuesta puede variar de un dispositivo a otro y verse afectado por la calidad de datos móviles.", true);

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Notificación de cierre de sesión
        /// </summary>
        /// <param name="id_sesion_cerrada">Id de Sesión de Usuario que fue terminada</param>
        /// <returns></returns>
        public RetornoOperacion NotificacionCierreSesion(int id_sesion_cerrada)
        {
            //Inicializando retorno
            RetornoOperacion resultado = new RetornoOperacion();
            //Token del usuario al que se reportará
            string tokenFCM = "";

            //Instanciando sesión
            using (UsuarioSesion sesion = new UsuarioSesion(id_sesion_cerrada))
            {
                //Validando si se tiene habilitado el uso de apliacación móvil
                resultado = CompaniaEmisorReceptor.ValidaConfiguracionUsoAplicacionMovil(sesion.id_compania_emisor_receptor);
                if (resultado.OperacionExitosa)
                {
                    //Recuperando referencias de usuario y compañía
                    using (DataTable mit = Referencia.CargaReferencias(sesion.id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(sesion.id_compania_emisor_receptor, 30, "Token FCM", 0, "Configuración")))
                        tokenFCM = mit != null ? mit.Rows[0]["Valor"].ToString() : "";

                    //Si se obtuvo el token del usuario correspondiente
                    if (tokenFCM != "")
                    {
                        //Construyendo mensaje
                        var datos = new
                        {
                            Titulo = "¡Sesión Terminada!",
                            Mensaje = "Vuelva a iniciar sesión en este dispositivo.",
                            TipoNotificacion = "CierreSesion"
                        };

                        //Enviando mensaje vía FCM
                        resultado = FirebaseCloudNotifications.EnviaMensajeFCM(_url_servidor_fcm, _server_token_fcm, tokenFCM, FirebaseCloudNotifications.Prioridad.Normal, datos);
                    }
                    else
                        resultado = new RetornoOperacion("El token del dispositivo del usuario debe ser actualizado.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Privados        

        /// <summary>
        /// Valida que la asignación de recurso esté asociada a un movimiento de servicio (NO Movimientos en Vacío)
        /// </summary>
        /// <param name="id_movimiento_asignacion">Id de Asignación del movimiento</param>
        /// <returns></returns>
        private RetornoOperacion validaMovimientoServicio(int id_movimiento_asignacion)
        {
            //Id de servicio
            int id_servicio;

            //Devolviendo resultado
            return validaMovimientoServicio(id_movimiento_asignacion, out id_servicio);
        }
        /// <summary>
        /// Valida que la asignación de recurso esté asociada a un movimiento de servicio (NO Movimientos en Vacío)
        /// </summary>
        /// <param name="id_movimiento_asignacion">Id de Asignación del movimiento</param>
        /// <param name="id_servicio">Id de servicio al que pertenece</param>
        /// <returns></returns>
        private RetornoOperacion validaMovimientoServicio(int id_movimiento_asignacion, out int id_servicio)
        {
            //Asignando parametros de salida
            id_servicio = 0;

            //Inicializando resultado sin errores
            RetornoOperacion resultado = new RetornoOperacion("Recurso asignado a servicio.", true);

            //Instanciando Asignación
            using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion))
            {
                //Cargando movimiento correspondiente
                using (Movimiento mov = new Movimiento(asignacion.id_movimiento))
                {
                    //Asignando id de servicio
                    id_servicio = mov.id_servicio;

                    //Si el movimiento no es de servicio
                    if (mov.id_servicio == 0)
                        resultado = new RetornoOperacion("El movimiento de la asignación no corresponde a un servicio.");
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        /// <summary>
        /// Realiza la validación de un usuario y asesión activa en dispositivo móvil
        /// </summary>
        /// <param name="id_movimiento_asignacion_recurso">Id de Asignación de recurso</param>
        /// <param name="id_compania">Id de Compañía donde se encuentra el recurso</param>
        /// <param name="token_fcm">Token de Firebase Cloud Messaging</param>
        /// <returns></returns>
        private RetornoOperacion validaUsuarioSesionActivaRecurso(int id_movimiento_asignacion_recurso, out int id_compania, out int id_recurso, out MovimientoAsignacionRecurso.Tipo tipo_recurso, out string token_fcm)
        {
            //Inicializando retorno
            RetornoOperacion resultado = new RetornoOperacion("Sesión Activa Encontrada.", true);
            //Inicializando parámetros de salida
            id_compania = id_recurso = 0;
            tipo_recurso = MovimientoAsignacionRecurso.Tipo.Operador;
            token_fcm = "";

            //Instanciando asignación
            using (MovimientoAsignacionRecurso asignacion = new MovimientoAsignacionRecurso(id_movimiento_asignacion_recurso))
            {
                //Definiendo objeto usuario sin asignar
                Usuario usuario = null;

                //Para el tipo de asignación correspondiente
                switch (asignacion.TipoMovimientoAsignacion)
                {
                    case MovimientoAsignacionRecurso.Tipo.Operador:
                        //Instanciando operador
                        using (Operador op = new Operador(asignacion.id_recurso_asignado))
                        {
                            //Obteniendo usuario en cuestión
                            usuario = Usuario.ObtieneUsuarioAsignadoOperador(op.id_operador);
                            id_compania = op.id_compania_emisor;
                            id_recurso = op.id_operador;
                        }

                        break;
                    case MovimientoAsignacionRecurso.Tipo.Tercero:

                        //TODO: Implementar obtención de token FCM de dispositivo asignado al tercero

                        break;
                    case MovimientoAsignacionRecurso.Tipo.Unidad:
                        //TODO: Implementar obtención de token FCM de dispositivo asignado a la unidad
                        break;
                }

                //Validando existencia de usuario
                if (usuario != null)
                {
                    //Validando sesión activa en algún dispositivo
                    if (Validacion.ValidaOrigenDatos(UsuarioSesion.ObtieneSesionesActivasUsuario(usuario.id_usuario, UsuarioSesion.TipoDispositivo.Android)))
                    {
                        //Recuperando referencia del token de usuario
                        using (DataTable mit = Referencia.CargaReferencias(usuario.id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(id_compania, 30, "Token FCM", 0, "Configuración")))
                            token_fcm = mit != null ? mit.Rows[0]["Valor"].ToString() : "";

                        //Si no obtuvo el token del usuario correspondiente
                        if (token_fcm == "")
                            resultado = new RetornoOperacion("El token del dispositivo del usuario debe ser actualizado.");

                    }
                    //Si no hay sesiones activas
                    else
                        resultado = new RetornoOperacion("El usuario no cuenta con una sesión activa en dispositivo móvil.");
                }
                //Si no hay un usuario asiciado
                else
                    resultado = new RetornoOperacion("No hay un usuario asociado al recurso.");

            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
    }
}