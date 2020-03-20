using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_WCF.Seguridad
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Usuario" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Usuario.svc o Usuario.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Usuario : IUsuario
    {
        #region Métodos Privados

        /// <summary>
        /// Realiza la conversión de un tipo de dispositivo en cadena a valor de enumeración
        /// </summary>
        /// <param name="tipo_dispositivo">Tipo de Dispositivo</param>
        /// <returns></returns>
        private TipoDispositivo convierteCadenaTipoDispositivo(string tipo_dispositivo)
        {
            //Declarando objeto de retorno
            TipoDispositivo t = TipoDispositivo.Desconocido;

            //Determinando el valor de entrada
            switch (tipo_dispositivo)
            {
                case "Escritorio":
                    t = TipoDispositivo.Escritorio;
                    break;
                case "Portatil":
                    t = TipoDispositivo.Portatil;
                    break;
                case "Android":
                    t = TipoDispositivo.Android;
                    break;
            }

            //Devolviendo resultado
            return t;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza las validaciones necesarias sobre la cuenta de usuario indicada y permite el acceso remoto a la plataforma.
        /// </summary>
        /// <param name="email">Email registrado en cuenta de usuario activa</param>
        /// <param name="contrasena">Contraseña asignada por el usuario para su inicio de sesión</param>
        /// <returns>TSDK.Base.RetornoOperacion en formato xml</returns>
        public string AutenticaUsuario(string email, string contrasena)
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado;
            int idTipoReferencia = 0;

            //Validando conjunto de datos requeridos
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contrasena))
            {
                //Instanciando usuario
                using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(email))
                {
                    //Realizando autenticación de usuario solicitado
                    resultado = u.AutenticaUsuario(contrasena);

                    //Validando Operación Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Obteniendo Tipo
                        idTipoReferencia = ReferenciaTipo.ObtieneIdReferenciaTipo(0, 30, "Codigo Acceso", 0, "Configuración");

                        //Obteniendo Referencia de Codigo
                        using (DataTable dtReferencias = Referencia.CargaReferencias(u.id_usuario, 30, idTipoReferencia))
                        {
                            //Validando que exista la Referencia
                            if (Validacion.ValidaOrigenDatos(dtReferencias))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtReferencias.Rows)
                                {
                                    //Editando Referencia
                                    resultado = Referencia.EditaReferencia(Convert.ToInt32(dr["Id"]), idTipoReferencia, Cadena.CadenaAleatoria(0, 0, 4), u.id_usuario);

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                            else
                                //Insertando Referencia de Usuario
                                resultado = Referencia.InsertaReferencia(u.id_usuario, 30, idTipoReferencia, Cadena.CadenaAleatoria(0, 0, 4), Fecha.ObtieneFechaEstandarMexicoCentro(), u.id_usuario);
                        }
                    }

                    //Validando Operación
                    if (resultado.OperacionExitosa)

                        //Instanciando Id de Usuario
                        resultado = new RetornoOperacion(u.id_usuario);
                }
            }
            else
                //Instanciando Excepción
                resultado = new RetornoOperacion(string.Format("{0} {1}", string.IsNullOrEmpty(email) ? "Falta email." : "", string.IsNullOrEmpty(contrasena) ? "Falta contraseña." : ""));

            //Devolvemos Resultado
            return resultado.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Validar la Existencia de un Usuario
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <param name="contrasena">Contraseña</param>
        /// <returns></returns>
        public string ValidaUsuarioContrasena(string email, string contrasena)
        {
            //Declarando objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando usuario
            using (SAT_CL.Seguridad.Usuario u = new SAT_CL.Seguridad.Usuario(email))
            {
                //Validando si existe
                if (u.habilitar)
                {
                    //Realizando autenticación de usuario solicitado
                    result = u.AutenticaUsuario(contrasena);

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)

                        //Instanciando Usuario
                        result = new RetornoOperacion(u.id_usuario);
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe el Usuario");
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Realiza el firmado del usuario sobre una compañía en particular
        /// </summary>
        /// <param name="id_usuario">Id de Usuario Autenticado</param>
        /// <param name="id_compania">Id de Compañía donde se firmará el usuario</param>
        /// <param name="tipo_dispositivo">Tipo de dispositivo desde donde se accesa (Consultar TipoDispositivo en contrato de servicio)</param>
        /// <param name="nombre_dispositivo">Nombre o alias del dispositivo</param>
        /// <param name="direccion_ip_mac">Dirección ipV6 o MAC del dispositivo</param>
        /// <param name="codigo_aut">Código de Autenticación</param>
        /// <param name="token_fcm">Token de dispositivo, registrado para Firebase Cloud Messaging</param>
        /// <returns></returns>
        public string IniciaSesion(int id_usuario, int id_compania, string tipo_dispositivo, string nombre_dispositivo, string direccion_ip_mac, string codigo_aut, string token_fcm)
        {
            //Declarando objeto de retorno
            RetornoOperacion result = new RetornoOperacion("No fueron proporcionados todos los valores de parámetros requeridos.");

            //Validando conjunto de datos requeridos
            if (id_usuario > 0 && id_compania > 0 && !string.IsNullOrEmpty(tipo_dispositivo) && !string.IsNullOrEmpty(nombre_dispositivo) && !string.IsNullOrEmpty(direccion_ip_mac))
            {
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Validando Compania
                    if (Operador.ObtieneOperadorUsuario(id_usuario).id_compania_emisor == id_compania)
                    {
                        //Obteniendo Referencia de Codigo
                        using (DataTable dtRefCodConfirmacion = Referencia.CargaReferencias(id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 30, "Codigo Acceso", 0, "Configuración")))
                        {
                            //Validando que exista la Referencia
                            if (Validacion.ValidaOrigenDatos(dtRefCodConfirmacion))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtRefCodConfirmacion.Rows)
                                {
                                    //Instanciando Referencia
                                    using (Referencia referencia = new Referencia(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista la Referencia
                                        if (referencia.habilitar)
                                        {
                                            //Validando que el Codigo no sea Vacio
                                            if (!referencia.valor.Equals(""))
                                            {
                                                //Validando que no Exceda el Tiempo Permitido
                                                if ((Fecha.ObtieneFechaEstandarMexicoCentro() - referencia.fecha).TotalMinutes <= 30)
                                                {
                                                    //Validando que el Codigo sea igual
                                                    if (referencia.valor.Equals(codigo_aut))
                                                    {
                                                        //Insertamos Sesión del Usuario
                                                        result = SAT_CL.Seguridad.UsuarioSesion.IniciaSesion(id_usuario, id_compania, (SAT_CL.Seguridad.UsuarioSesion.TipoDispositivo)((byte)convierteCadenaTipoDispositivo(tipo_dispositivo)), direccion_ip_mac, nombre_dispositivo, id_usuario);

                                                        //Validando Operación Exitosa
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Obteniendo Sesion generada
                                                            int idSession = result.IdRegistro;

                                                            //Editando Referencia
                                                            result = Referencia.EditaReferencia(referencia.id_referencia, "", id_usuario);

                                                            //Validando Operación Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualizando token de dispositivo
                                                                result = actualizaTokenFireBaseCloudMessaging(idSession, token_fcm);

                                                                //Si no hay errores de actualización de token
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Instanciando Sesión
                                                                    result = new RetornoOperacion(idSession);

                                                                    //Completando Transacción
                                                                    trans.Complete();
                                                                }
                                                                else
                                                                    result = new RetornoOperacion(-2, string.Format("Error al actualizar Token FCM: {0}.", result.Mensaje), false);
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion(-2, "El Código de Autenticación es Incorrecto.", false);
                                                }
                                                else
                                                {
                                                    //Limpiando código previo generado
                                                    result = Referencia.EditaReferencia(referencia.id_referencia, "", id_usuario);
                                                    trans.Complete();
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion(-2, "Se ha excedido el límite de tiempo para ingresar el Código de Autenticación.", false);
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion(-2, "No se especifico el Código de Autenticación.", false);
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(-2, "No existe el Código de Autenticación.", false);
                                    }

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion(-2, "No existe el Código de Autenticación.", false);
                        }
                    }
                    else
                        //
                        result = new RetornoOperacion("El usuario no corresponde a la Compania Asignada.");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion(string.Format("{0} {1} {2} {3} {4}.", id_usuario < 1 ? "Falta id_usuario." : "", id_compania < 1 ? "Falta id_compania." : "",
                                    string.IsNullOrEmpty(tipo_dispositivo) ? "Falta tipo_dispositivo." : "", string.IsNullOrEmpty(nombre_dispositivo) ? "Falta nombre_dispositivo." : "",
                                    string.IsNullOrEmpty(direccion_ip_mac) ? "Falta direccion_ip_mac." : ""));

            //Devolviendo resultado
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Finalizar la Sesión de un Usuario
        /// </summary>
        /// <param name="id_usuario_sesion">Sesión de un Usuario</param>
        /// <returns></returns>
        public string FinalizaSesion(int id_usuario_sesion)
        {
            //Declarando objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciamos la Sesion del Usuario
            using (UsuarioSesion objUsuarioSesion = new UsuarioSesion(id_usuario_sesion))
            {
                //Validamos que exista Sesion
                if (objUsuarioSesion.id_usuario_sesion > 0)

                    //Cerramos Sesion en BD
                    result = objUsuarioSesion.TerminarSesion();
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Obtiene las compañías a las que está adscrito el usuario
        /// </summary>
        /// <param name="id_usuario">Id de Usuario</param>
        /// <returns>System.Data.DataTable en formato xml</returns>
        public string ObtieneCompaniasUsuario(int id_usuario)
        {
            //Declarando objeto de resultado
            string resultado = @"";

            //Si el usuario fue proporcionado
            if (id_usuario > 0)
            {
                //Creando flujo de memoria
                using (System.IO.Stream s = new System.IO.MemoryStream())
                {
                    //Obteniendo conjunto de compañía
                    using (DataTable mit = SAT_CL.Seguridad.UsuarioCompania.ObtieneCompaniasUsuario(id_usuario))
                    {
                        //Realizando filtrado de columnas
                        using (DataTable mitCopia = OrigenDatos.CopiaDataTableFiltrandoColumnas(mit, "Table", false, "IdCompaniaEmisorReceptor", "NombreCorto"))
                        {
                            //Validando que existan registros
                            if (Validacion.ValidaOrigenDatos(mitCopia))
                            {
                                //Leyendo flujo de datos XML
                                mitCopia.WriteXml(s);
                                //Convirtiendo el flujo a una cadena de caracteres xml
                                resultado = System.Text.Encoding.UTF8.GetString(Flujo.ConvierteFlujoABytes(s));
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Método encargado de Actualizar la Ultima Actividad del Usuario
        /// </summary>
        /// <param name="id_usuario_sesion">Sesión Activa</param>
        /// <param name="direccion_mac">Dirección MAC</param>
        /// <param name="nombre_dispositivo">Nombre del Dispositivo</param>
        /// <returns></returns>
        public string ActualizaUltimaActividad(int id_usuario_sesion, string direccion_mac, string nombre_dispositivo)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Usuario Sesión
            using (UsuarioSesion us = new UsuarioSesion(id_usuario_sesion))
            {
                //Validando que exista
                if (us.habilitar && us.EstatusSesion == UsuarioSesion.Estatus.Activo)

                    //Actualizamos Ultima Actividad
                    result = us.ActualizaUltimaActividad(direccion_mac, nombre_dispositivo);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("La Sesión ha Expirado");
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Actualiza el token de dispositivo, asignado para servicios FCM
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Sesión de Usuario</param>
        /// <returns></returns>
        private RetornoOperacion actualizaTokenFireBaseCloudMessaging(int id_usuario_sesion, string token)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(id_usuario_sesion);
            
            //Inicializando bloque transaccional
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando sesión solicitada
                using (UsuarioSesion sesion = new UsuarioSesion(id_usuario_sesion))
                {
                    //Validando estatus de sesión
                    if (sesion.EstatusSesion == UsuarioSesion.Estatus.Activo)
                    {
                        //Obteniendo Referencia de token existente para el registro de usuario de la sesión
                        using (DataTable mitTokenActual = Referencia.CargaReferencias(sesion.id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 30, "Token FCM", 0, "Configuración")))
                        {
                            //Si existe alguna
                            if (mitTokenActual != null)
                            {
                                //Verificando existencia de esta misma referencia
                                using (Referencia tokenDuplicado = new Referencia(token, ReferenciaTipo.ObtieneIdReferenciaTipo(sesion.id_compania_emisor_receptor, 30, "Token FCM", 0, "Configuración")))
                                {
                                    //Si hay alguna existente que no sea la referencia del token actual a editar
                                    if (tokenDuplicado.id_referencia > 0 && tokenDuplicado.id_referencia != Convert.ToInt32(mitTokenActual.Rows[0]["Id"]))
                                    {
                                        //Verificando si está asignado a un usuario con sesiones móviles activas
                                        if (UsuarioSesion.ObtieneSesionesActivasUsuario(tokenDuplicado.id_registro, UsuarioSesion.TipoDispositivo.Android) != null)
                                            //Señalando error
                                            resultado = new RetornoOperacion(String.Format("El Token está asignado a una sesión activa de '{0}', cierre la sesión antes de continuar.", new SAT_CL.Seguridad.Usuario(tokenDuplicado.id_registro).nombre));
                                        //Si no hay asignación
                                        else
                                            //Borrando el token del usuario anterior
                                            resultado = Referencia.EditaReferencia(tokenDuplicado.id_referencia, "", sesion.id_usuario);
                                    }
                                }

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                    //Editando registro previo
                                    resultado = Referencia.EditaReferencia(Convert.ToInt32(mitTokenActual.Rows[0]["Id"]), token, sesion.id_usuario);
                                else
                                    resultado = new RetornoOperacion(String.Format("Error al borrar Token anterior: {0}", resultado.Mensaje));
                            }
                            //Si no existe alguna
                            else
                                //Insertando nuevo elemento
                                resultado = Referencia.InsertaReferencia(sesion.id_usuario, 30, ReferenciaTipo.ObtieneIdReferenciaTipo(sesion.id_compania_emisor_receptor, 30, "Token FCM", 0, "Configuración"), token, Fecha.ObtieneFechaEstandarMexicoCentro(), sesion.id_usuario);
                        }
                    }
                    //Si la sesión ya no se encuentra activa
                    else
                        resultado = new RetornoOperacion("La Sesión ha Expirado.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza el token de dispositivo, asignado para servicios FCM
        /// </summary>
        /// <param name="id_usuario_sesion">Id de Sesión de Usuario</param>
        /// <returns></returns>
        public string ActualizaTokenFireBaseCloudMessaging(int id_usuario_sesion, string token)
        {
            //Devolviendo resultado
            return actualizaTokenFireBaseCloudMessaging(id_usuario_sesion, token).ToXMLString();
        }

        #endregion

    }
}
