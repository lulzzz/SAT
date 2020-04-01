using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_CL.FacturacionElectronica33
{
    public sealed class CancelacionCDFI : Disposable
    {
        #region Atributos

        /// <summary>
        /// Atributo que obtiene las Propiedades de la Clase de Cancelación
        /// </summary>
        private static CancelacionCDFI _objCancelacion = null;
        /// <summary>
        /// Atributo que utiliza las Propiedades de la Clase de Cancelación
        /// </summary>
        public static CancelacionCDFI objCancelacion
        {
            get
            {
                //Validando si el Atributo esta Vacio
                if (_objCancelacion == null)
                {
                    lock (new object())
                    {
                        //Validando si el Atributo esta Vacio
                        if (_objCancelacion == null)

                            //Instanciando Objeto
                            _objCancelacion = new CancelacionCDFI();
                    }
                }

                //Devolviendo Resultado Obtenido
                return _objCancelacion;
            }
        }

        #endregion

        #region Enumeraciones

        /// <summary>
        /// Enumeración que expresa el PAC que brindará los Servicios de Cancelación
        /// </summary>
        public enum PacCancelacion
        {
            /// <summary>
            /// 
            /// </summary>
            SinAsignar = 0,
            /// <summary>
            /// 
            /// </summary>
            Facturador = 1120,
            /// <summary>
            /// 
            /// </summary>
            FacturemosYa = 64,
        }
        /// <summary>
        /// 
        /// </summary>
        public enum TipoCancelacion
        {
            /// <summary>
            /// 
            /// </summary>
            SinAsignar = 0,
            /// <summary>
            /// 
            /// </summary>
            CancelacionSinAceptacion = 1,
            /// <summary>
            /// 
            /// </summary>
            CancelacionConAceptacion = 2
        }
        /// <summary>
        /// Enumeración que expresa el Estatus del UUID
        /// </summary>
        public enum EstatusUUID
        {
            /**** Facturador ****/
            /// <summary>
            /// Sin Obtención de Estatus
            /// </summary>
            SinEstatus = 0,
            /// <summary>
            /// UUID Cancelado
            /// </summary>
            Cancelado = 201,
            /// <summary>
            /// UUID Previamente Cancelado
            /// </summary>
            PreviamenteCancelado = 202,
            /// <summary>
            /// UUID No Encontrado o no corresponde al Emisor
            /// </summary>
            NoEncontrado = 203,
            /// <summary>
            /// UUID No Aplicable para Cancelación
            /// </summary>
            NoAplicable = 204,
            /// <summary>
            /// UUID No Existe
            /// </summary>
            NoExiste = 205,
            /// <summary>
            /// UUID no corresponde a un CFDI del Sector Primario
            /// </summary>
            NoCorresponde = 206,
            /**** Facturemos Ya! ****/
            /// <summary>
            /// UUID Cancelado
            /// </summary>
            Exito = 501,
            /// <summary>
            /// 
            /// </summary>
            EsperaCancelacion = 2001,
            /// <summary>
            /// 
            /// </summary>
            UUIDRelacionados = 102,
            /// <summary>
            /// 
            /// </summary>
            AceptacionCliente = 103,
            /// <summary>
            /// 
            /// </summary>
            RechazoCliente = 104,
            /// <summary>
            /// 
            /// </summary>
            DobleSolicitud = 105,
            /// <summary>
            /// 
            /// </summary>
            EnProceso = 106,
            /// <summary>
            /// 
            /// </summary>
            CancelacionPreviaAutomatica = 107,
            /// <summary>
            /// 
            /// </summary>
            ErrorSinClasificar = 999,
            /// <summary>
            /// 
            /// </summary>
            AccesoIncorrecto = 301,
            /// <summary>
            /// 
            /// </summary>
            UsuarioIncorrecto = 302,
            /// <summary>
            /// 
            /// </summary>
            ConstrasenaIncorrecta = 303,
            /// <summary>
            /// 
            /// </summary>
            UUIDMalFormado = 304
        }
        /// <summary>
        /// Enumeración que expresa el Estatus del UUID
        /// </summary>
        public enum EstatusPeticion
        {
            /// <summary>
            /// Sin Obtención de Estatus
            /// </summary>
            SinEstatus = 0,
            /// <summary>
            /// XML Mal Formado
            /// </summary>
            XMLMalFormado = 301,
            /// <summary>
            /// Sello mal formado o Invalido
            /// </summary>
            SelloInvalido = 302,
            /// <summary>
            /// Sello no corresponde al Emisor
            /// </summary>
            SelloNoCorresponde = 303,
            /// <summary>
            /// Certificado Revocado o Caduco
            /// </summary>
            CertificadoRevocado = 304,
            /// <summary>
            /// Certificado Invalido
            /// </summary>
            CertificadoInvalido = 305,
            /// <summary>
            /// Uso de Certificado de e.firma Invalido
            /// </summary>
            CertificadoEfirmaInvalido = 306
        }
        /// <summary>
        /// Enumeración que expresa el Mensaje de Respuesta de la Consulta del CFDI
        /// </summary>
        public enum RespuestaConsultaCFDI
        {
            /** Facturador **/
            /// <summary>
            /// Sin respuesta
            /// </summary>
            SinRespuesta = 0,
            /// <summary>
            /// La expresión impresa proporcionada no es válida
            /// </summary>
            ExpresionInvalida = 601,
            /// <summary>
            /// Comprobante no encontrado
            /// </summary>
            ComprobanteNoEncontrado = 602,
            /// <summary>
            /// Comprobante obtenido satisfactoriamente
            /// </summary>
            ComprobanteObtenido = 1,
            /** Facturemos Ya! **/
            /// <summary>
            /// 
            /// </summary>
            Activa = 200,
            /// <summary>
            /// 
            /// </summary>
            Cancelada = 201,
            /// <summary>
            /// 
            /// </summary>
            EsperaCancelacion = 202,
            /// <summary>
            /// 
            /// </summary>
            RechazoCliente = 203,
            /// <summary>
            /// 
            /// </summary>
            AceptacionCliente = 204,
            /// <summary>
            /// 
            /// </summary>
            AccesoIncorrecto = 301,
            /// <summary>
            /// 
            /// </summary>
            UsuarioIncorrecto = 302,
            /// <summary>
            /// 
            /// </summary>
            ConstrasenaIncorrecta = 303,
            /// <summary>
            /// 
            /// </summary>
            UUIDMalFormado = 304
        }
        /// <summary>
        /// Enumeración que expresa el Mensaje de Respuesta de la Consulta de los CFDI Relacionados
        /// </summary>
        public enum RespuestaConsultaRelacionados
        {
            /// <summary>
            /// Sin respuesta
            /// </summary>
            SinRespuesta = 0,
            /// <summary>
            /// Existen cfdi relacionados al folio fiscal
            /// </summary>
            ExistenCFDI = 2000,
            /// <summary>
            /// No existen cfdi relacionados al folio fiscal
            /// </summary>
            NoExistenCFDI = 2001,
            /// <summary>
            /// Comprobante obtenido satisfactoriamente
            /// </summary>
            FolioNoPertenece = 2002,
            /// <summary>
            /// 
            /// </summary>
            NoExistenPeticiones = 1101
        }
        /// <summary>
        /// Enumeración que expresa el Mensaje de Respuesta de la Obtención de peticiones Pendientes
        /// </summary>
        public enum RespuestaPeticionesPendientes
        {
            /// <summary>
            /// 
            /// </summary>
            SinRespuesta = 0,
            /// <summary>
            /// 
            /// </summary>
            UsuarioNoValido = 300,
            /// <summary>
            /// 
            /// </summary>
            XmlMalFormado = 301,
            /// <summary>
            /// 
            /// </summary>
            PeticionesExitosas = 1100,
            /// <summary>
            /// 
            /// </summary>
            NoExistenPeticiones = 1101
        }
        /// <summary>
        /// Enumeración que expresa el resultado de la Solicitud
        /// </summary>
        public enum RespuestaSolicitudCFDI
        {
            /// <summary>
            /// 
            /// </summary>
            SolicitudNoValida = -1,
            /// <summary>
            /// 
            /// </summary>
            SinRespuesta = 0,
            /// <summary>
            /// 
            /// </summary>
            UsuarioNoValido = 300,
            /// <summary>
            /// 
            /// </summary>
            XmlMalFormado = 301,
            /// <summary>
            /// 
            /// </summary>
            SelloMalFormado = 302,
            /// <summary>
            /// 
            /// </summary>
            CertificadoRevocado = 304,
            /// <summary>
            /// 
            /// </summary>
            CertificadoInvalido = 305,
            /// <summary>
            /// 
            /// </summary>
            PatronFolioInvalido = 309,
            /// <summary>
            /// 
            /// </summary>
            CsdInvalido = 310,
            /// <summary>
            /// 
            /// </summary>
            RespuestaExitosa = 1000,
            /// <summary>
            /// 
            /// </summary>
            PeticionesInexistentes = 1001,
            /// <summary>
            /// 
            /// </summary>
            RespuestaAnterior = 1002,
            /// <summary>
            /// 
            /// </summary>
            SelloNoCorresponde = 1003,
            /// <summary>
            /// 
            /// </summary>
            PeticionMultiple = 1004,
            /// <summary>
            /// 
            /// </summary>
            UuidFormatoIncorrecto = 1005,
            /// <summary>
            /// 
            /// </summary>
            MaximoSolicitudes = 1006
        }


        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        private CancelacionCDFI() { }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="estatusUUID"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneResultadoEstatusUUID(string valor, out EstatusUUID estatusUUID)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            estatusUUID = EstatusUUID.SinEstatus;

            //Obteniendo Estatus
            int val = 0;
            int.TryParse(valor, out val);

            //Validando Estatus
            switch (val)
            {
                case 201:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID Cancelado", true);
                        estatusUUID = EstatusUUID.Cancelado;
                        break;
                    }
                case 202:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID Previamente Cancelado", true);
                        estatusUUID = EstatusUUID.PreviamenteCancelado;
                        break;
                    }
                case 203:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID No Encontrado o no corresponde al Emisor", false);
                        estatusUUID = EstatusUUID.NoEncontrado;
                        break;
                    }
                case 204:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID No Aplicable para Cancelación", false);
                        estatusUUID = EstatusUUID.NoAplicable;
                        break;
                    }
                case 205:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID No Existe", false);
                        estatusUUID = EstatusUUID.NoExiste;
                        break;
                    }
                case 206:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("UUID no corresponde a un CFDI del Sector Primario", false);
                        estatusUUID = EstatusUUID.NoCorresponde;
                        break;
                    }
                case 501:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Exito", true);
                        estatusUUID = EstatusUUID.Cancelado;
                        break;
                    }
                case 2001:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Cancelación es espera de aceptación", true);
                        estatusUUID = EstatusUUID.EsperaCancelacion;
                        break;
                    }
                case 102:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI no se puede cancelar porque contiene comprobantes relacionados vigentes, para cancelarlo deberá cancelar previamente todos los comprobantes relacionados.", false);
                        estatusUUID = EstatusUUID.UUIDRelacionados;
                        break;
                    }
                case 103:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI ha sido cancelado previamente. Por aceptación del receptor", true);
                        estatusUUID = EstatusUUID.AceptacionCliente;
                        break;
                    }
                case 104:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI no se puede cancelar, por que fue rechazado previamente.", false);
                        estatusUUID = EstatusUUID.RechazoCliente;
                        break;
                    }
                case 105:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI no se puede cancelar porque tiene estatus de “En espera de aceptación”.", false);
                        estatusUUID = EstatusUUID.DobleSolicitud;
                        break;
                    }
                case 106:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI no se puede cancelar porque tiene estatus de “En proceso”", false);
                        estatusUUID = EstatusUUID.EnProceso;
                        break;
                    }
                case 107:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El CFDI ha sido cancelado previamente. Por plazo vencido", true);
                        estatusUUID = EstatusUUID.CancelacionPreviaAutomatica;
                        break;
                    }
                case 301:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Usuario o contraseña incorrectos", false);
                        estatusUUID = EstatusUUID.AccesoIncorrecto;
                        break;
                    }
                case 302:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El usuario es incorrecto", false);
                        estatusUUID = EstatusUUID.UsuarioIncorrecto;
                        break;
                    }
                case 303:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Error al validar la contraseña", false);
                        estatusUUID = EstatusUUID.ConstrasenaIncorrecta;
                        break;
                    }
                case 304:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El uuid es requerido o es erróneo, formato: (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) solo caracteres de la 'a-f' y del '0-9'", false);
                        estatusUUID = EstatusUUID.UUIDMalFormado;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("No se puede determinar el Estatus del UUID", false);
                        estatusUUID = EstatusUUID.SinEstatus;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="estatusPeticion"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneResultadoEstatusPeticion(string valor, out EstatusPeticion estatusPeticion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            estatusPeticion = EstatusPeticion.SinEstatus;

            //Obteniendo Estatus
            int val = 0;
            int.TryParse(valor, out val);

            //Validando Estatus
            switch (val)
            {
                case 301:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("XML Mal formado", false);
                        estatusPeticion = EstatusPeticion.XMLMalFormado;
                        break;
                    }
                case 302:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sello mal formado o inválido", false);
                        estatusPeticion = EstatusPeticion.SelloInvalido;
                        break;
                    }
                case 303:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sello no corresponde al emisor", false);
                        estatusPeticion = EstatusPeticion.SelloNoCorresponde;
                        break;
                    }
                case 304:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Certificado revocado o caduco", false);
                        estatusPeticion = EstatusPeticion.CertificadoRevocado;
                        break;
                    }
                case 305:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Certificado inválido", false);
                        estatusPeticion = EstatusPeticion.CertificadoInvalido;
                        break;
                    }
                case 306:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Uso de certificado de e.firma inválido", false);
                        estatusPeticion = EstatusPeticion.CertificadoEfirmaInvalido;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("", false);
                        estatusPeticion = EstatusPeticion.SinEstatus;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="mensaje_consulta"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneMensajeResultadoConsultaCFDI(string valor, out RespuestaConsultaCFDI mensaje_consulta)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            mensaje_consulta = RespuestaConsultaCFDI.SinRespuesta;

            //Validando Estatus
            switch (valor)
            {
                case "N 601 - La expresión impresa proporcionada no es válida.":
                case "N - 601: La expresión impresa proporcionada no es válida.":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion(valor, false);
                        mensaje_consulta = RespuestaConsultaCFDI.ExpresionInvalida;
                        break;
                    }
                case "N 602 - Comprobante no encontrado.":
                case "N - 602: Comprobante no encontrado.":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion(valor, false);
                        mensaje_consulta = RespuestaConsultaCFDI.ComprobanteNoEncontrado;
                        break;
                    }
                case "S - Comprobante obtenido satisfactoriamente.":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion(valor, true);
                        mensaje_consulta = RespuestaConsultaCFDI.ComprobanteObtenido;
                        break;
                    }
                case "200":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El Comprobante esta Activo.", true);
                        mensaje_consulta = RespuestaConsultaCFDI.Activa;
                        break;
                    }
                case "201":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El Comprobante esta Cancelado.", true);
                        mensaje_consulta = RespuestaConsultaCFDI.Cancelada;
                        break;
                    }
                case "202":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Cancelación en espera de aceptación.", true);
                        mensaje_consulta = RespuestaConsultaCFDI.EsperaCancelacion;
                        break;
                    }
                case "203":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Cancelación rechazada.", true);
                        mensaje_consulta = RespuestaConsultaCFDI.RechazoCliente;
                        break;
                    }
                case "204":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Cancelación aceptada.", true);
                        mensaje_consulta = RespuestaConsultaCFDI.AceptacionCliente;
                        break;
                    }
                case "301":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Usuario o contraseña incorrectos.", false);
                        mensaje_consulta = RespuestaConsultaCFDI.AccesoIncorrecto;
                        break;
                    }
                case "302":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El usuario es incorrecto.", false);
                        mensaje_consulta = RespuestaConsultaCFDI.UsuarioIncorrecto;
                        break;
                    }
                case "303":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Error al validar la contraseña.", false);
                        mensaje_consulta = RespuestaConsultaCFDI.ConstrasenaIncorrecta;
                        break;
                    }
                case "304":
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El uuid es requerido o es erróneo, formato: (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) solo caracteres de la 'a-f' y del '0-9'.", false);
                        mensaje_consulta = RespuestaConsultaCFDI.UUIDMalFormado;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sin respuesta", false);
                        mensaje_consulta = RespuestaConsultaCFDI.SinRespuesta;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="mensaje_consulta"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneRespuestaConsultaRelacionados(string valor, out RespuestaConsultaRelacionados mensaje_consulta)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            mensaje_consulta = RespuestaConsultaRelacionados.SinRespuesta;

            //Obteniendo Estatus
            int val = 0;
            int.TryParse(valor, out val);

            //Validando Estatus
            switch (val)
            {
                case 2000:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Existen cfdi relacionados al folio fiscal", false);
                        mensaje_consulta = RespuestaConsultaRelacionados.ExistenCFDI;
                        break;
                    }
                case 2001:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("No existen cfdi relacionados al folio fiscal", true);
                        mensaje_consulta = RespuestaConsultaRelacionados.NoExistenCFDI;
                        break;
                    }
                case 2002:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El folio fiscal no pertenece al receptor", false);
                        mensaje_consulta = RespuestaConsultaRelacionados.FolioNoPertenece;
                        break;
                    }
                case 1101:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("No existen peticiones para el RFC Receptor", false);
                        mensaje_consulta = RespuestaConsultaRelacionados.NoExistenPeticiones;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sin Respuesta", false);
                        mensaje_consulta = RespuestaConsultaRelacionados.SinRespuesta;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="mensaje_consulta"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneResultadoPeticionesPendientes(string valor, out RespuestaPeticionesPendientes mensaje_consulta)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            mensaje_consulta = RespuestaPeticionesPendientes.SinRespuesta;

            //Obteniendo Estatus
            int val = 0;
            int.TryParse(valor, out val);

            //Validando Estatus
            switch (val)
            {
                case 300:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Usuario No Válido", false);
                        mensaje_consulta = RespuestaPeticionesPendientes.UsuarioNoValido;
                        break;
                    }
                case 301:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("XML Mal Formado", false);
                        mensaje_consulta = RespuestaPeticionesPendientes.XmlMalFormado;
                        break;
                    }
                case 1100:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Se obtuvieron las peticiones del RFC Receptor de forma exitosa", true);
                        mensaje_consulta = RespuestaPeticionesPendientes.PeticionesExitosas;
                        break;
                    }
                case 1101:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("No existen peticiones para el RFC Receptor", true);
                        mensaje_consulta = RespuestaPeticionesPendientes.NoExistenPeticiones;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sin Respuesta", false);
                        mensaje_consulta = RespuestaPeticionesPendientes.SinRespuesta;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="mensaje_consulta"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneRespuestaSolicitudCancelacion(string valor, out RespuestaSolicitudCFDI mensaje_consulta)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignando Resultado
            mensaje_consulta = RespuestaSolicitudCFDI.SinRespuesta;

            //Obteniendo Estatus
            int val = 0;
            int.TryParse(valor, out val);

            //Validando Estatus
            switch (val)
            {
                case 300:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Usuario No Válido", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.UsuarioNoValido;
                        break;
                    }
                case 301:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("XML Mal Formado", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.XmlMalFormado;
                        break;
                    }
                case 302:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sello Mal Formado", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.SelloMalFormado;
                        break;
                    }
                case 304:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Certificado Revocado o Caduco", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.CertificadoRevocado;
                        break;
                    }
                case 305:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Certificado Inválido", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.CertificadoInvalido;
                        break;
                    }
                case 309:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Patrón de folio Inválido", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.PatronFolioInvalido;
                        break;
                    }
                case 310:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("CSD Inválido", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.CsdInvalido;
                        break;
                    }
                case 1000:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Se recibió la respuesta de la petición de forma exitosa", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.RespuestaExitosa;
                        break;
                    }
                case 1001:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("No existen peticiones de cancelación en espera de respuesta para el UUID", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.PeticionesInexistentes;
                        break;
                    }
                case 1002:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Ya se recibió una respuesta para la petición de cancelación del UUID", true);
                        mensaje_consulta = RespuestaSolicitudCFDI.RespuestaAnterior;
                        break;
                    }
                case 1003:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sello no corresponde al RFC Receptor", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.SelloNoCorresponde;
                        break;
                    }
                case 1004:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Existen más de una petición de cancelación para el mismo UUID", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.PeticionMultiple;
                        break;
                    }
                case 1005:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("El UUID es nulo no posee el formato correcto", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.UuidFormatoIncorrecto;
                        break;
                    }
                case 1006:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Se rebaso el número máximo de solicitudes permitidas", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.MaximoSolicitudes;
                        break;
                    }
                default:
                    {
                        //Instanciando Retorno personalizado
                        retorno = new RetornoOperacion("Sin Respuesta", false);
                        mensaje_consulta = RespuestaSolicitudCFDI.SinRespuesta;
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// Método encargado de Obtener los Datos del Certificado
        /// </summary>
        /// <param name="id_compania_emisora">Emisor del Certificado</param>
        /// <param name="id_certificado">identificador del Certificado (En caso de estar vacio, tomara el Certificado Actual del Emisor)</param>
        /// <param name="llaveCertificadoBase64">Llave del Certificado (Base 64)</param>
        /// <param name="certificadoBase64">Certificado del Emisor (Base 64)</param>
        /// <returns></returns>
        private RetornoOperacion obtieneDatosCertificado(int id_compania_emisora, out string llaveCertificadoBase64, out string certificadoBase64)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            int id_certificado;
            llaveCertificadoBase64 = certificadoBase64 = "";

            //Instanciando Emisor
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(id_compania_emisora))
            {
                //Validando Emisor
                if (emisor.habilitar)
                {
                    //Validando Certificado y reasignando
                    id_certificado = SAT_CL.Global.CertificadoDigital.RecuperaCertificadoEmisorSucursal(emisor.id_compania_emisor_receptor, 0, Global.CertificadoDigital.TipoCertificado.CSD).id_certificado_digital;

                    //Instanciando Certificado de Sello Digital con el que se emitio el Comprobante
                    using (SAT_CL.Global.CertificadoDigital cert = new SAT_CL.Global.CertificadoDigital(id_certificado))
                    using (TSDK.Base.CertificadoDigital.Certificado cer = new TSDK.Base.CertificadoDigital.Certificado(cert.ruta_llave_publica))
                    {
                        //Validando que exista el Comprobante
                        if (cert.Habilitar)
                        {
                            //Comprobando Carga del Certificado
                            if (cer.Subject != null)
                            {
                                //Validando RFC del Emisor contra el del Certificado
                                if (cer.Subject.RFCPropietario == emisor.rfc)
                                {
                                    //Obtenemos XML de la Llave privada
                                    System.Security.Cryptography.RSACryptoServiceProvider p = TSDK.Base.CertificadoDigital.CertificadosOpenSSLKey.DecodeEncryptedPrivateKeyInfo(System.IO.File.ReadAllBytes(cert.ruta_llave_privada), TSDK.Base.Cadena.CadenaSegura(cert.contrasena_desencriptada));

                                    //Obteniendo Certificado y Llave del Certificado
                                    llaveCertificadoBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(p.ToXmlString(true)));
                                    certificadoBase64 = cer.CertificadoBase64;

                                    //Validando Datos Obtenidos
                                    if (!llaveCertificadoBase64.Equals("") && !certificadoBase64.Equals(""))

                                        //Instanciando Retorno Positivo
                                        retorno = new RetornoOperacion(1, "Los Datos del Certificado se obtuvieron exitosamente", true);
                                }
                                else
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion(string.Format("El Certificado de Sello Digital (CSD) no pertenece al Emisor '{0}'", emisor.nombre));
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("Imposible recuperar los datos de Propietario del Certificado.");
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("No se puede recuperar el Certificado de Sello Digital");
                    }
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Emisor de Comprobante");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_compania_emisora"></param>
        /// <param name="pac"></param>
        /// <param name="id_pac_emisor"></param>
        /// <returns></returns>
        private RetornoOperacion obtienePacCompania(int id_compania_emisora, out PacCancelacion pac, out int id_pac_emisor)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("No se pudo recuperar el PAC del Emisor");
            pac = PacCancelacion.SinAsignar;
            id_pac_emisor = 0;

            //Obteniendo Pac's por Emisor
            using (DataTable mitPac = SAT_CL.FacturacionElectronica.PacCompaniaEmisor.CargaPACSCompaniaEmisor("3.3", id_compania_emisora, SAT_CL.FacturacionElectronica.PacCompaniaEmisor.Tipo.Cancelar))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mitPac))
                {
                    //Recorriendo Pac's
                    foreach (DataRow r in mitPac.Rows)
                    {
                        //Asignando Valores
                        pac = (PacCancelacion)r.Field<int>("IdCompaniaPac");
                        id_pac_emisor = r.Field<int>("Id");
                        retorno = new RetornoOperacion(id_pac_emisor);

                        //Terminando Ciclo
                        break;
                    }
                }
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="uuid"></param>
        /// <param name="pac"></param>
        /// <param name="consultaWS"></param>
        /// <param name="acuse"></param>
        /// <returns></returns>
        private RetornoOperacion devuelveResultadoWS(XDocument response, string uuid, PacCancelacion pac, string consultaWS, out XDocument acuse)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            acuse = new XDocument();

            //Validando PAC
            switch (pac)
            {
                case PacCancelacion.Facturador:
                    {
                        //Obteniendo Nodo sin Datos del Response SOAP
                        XDocument result = new XDocument(((XElement)((XElement)((XElement)response.Root.FirstNode).FirstNode).FirstNode));

                        //Validando que no sea un Error
                        if (result.Root.Element("Errores") == null)
                        {
                            //Validando Consulta
                            switch (consultaWS)
                            {
                                case "CancelarComprobante":
                                    {
                                        //Validando Errores
                                        if (result.Root.Element("Solicitud").Element("errores") == null)
                                        {
                                            //Recorriendo Profundidad de Nodos
                                            //1.- Root - EnvelopeSoap
                                            //2.- FirstNode - Body
                                            //3.- FirstNode - CancelarComprobanteResponse
                                            //4.- FirstNode - CancelarComprobanteResult

                                            //Validando que sea la Solicitud
                                            if (result.Root.Element("Solicitud") != null)
                                            {
                                                //Asignando valores Positivos
                                                acuse = new XDocument(result.Root.Element("Solicitud"));
                                                retorno = new RetornoOperacion(1, "", Convert.ToBoolean(result.Root.Element("Solicitud").Attribute("esValido").Value));
                                            }
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("No se puedo recuperar la Solicitud");
                                        }
                                        else
                                        {
                                            //Obteniendo Nodo de Errores
                                            XElement err = result.Root.Element("Solicitud").Element("errores");

                                            //Validando Errores
                                            if (err != null)
                                            {
                                                //Validando Codigo de Error
                                                switch (err.Element("Error").Attribute("codigo").Value.ToUpper())
                                                {
                                                    case "FET102":
                                                        {
                                                            //Creando Acuse del Error
                                                            acuse = XDocument.Parse(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                                                                                    <Solicitud xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns="""" esValido=""True"" tipoCancelacion=""Cancelable con aceptación"">
                                                                                                      <Folios>
                                                                                                        <Folio UUID=""{0}"" Estatus=""202"" />
                                                                                                      </Folios>
                                                                                                      <Acuse Fecha=""{1:yyyy-MM-dd}T{1:HH:mm:ss}"">
                                                                                                        <Folios xmlns=""http://cancelacfd.sat.gob.mx"">
                                                                                                          <UUID>{0}</UUID>
                                                                                                          <EstatusUUID>202</EstatusUUID>
                                                                                                        </Folios>
                                                                                                      </Acuse>
                                                                                                    </Solicitud>", uuid.ToUpper(), Fecha.ObtieneFechaEstandarMexicoCentro()));

                                                            //Instanciando Excepción Positiva
                                                            retorno = new RetornoOperacion(1, err.Element("Error").Attribute("mensaje").Value, true);
                                                            break;
                                                        }
                                                    case "FET101":
                                                    case "FET103":
                                                    case "FET104":
                                                    case "FET105":
                                                    case "205":
                                                        {
                                                            //Instanciando Excepción Positiva
                                                            retorno = new RetornoOperacion(string.Format("{0} - {1}", err.Element("Error").Attribute("codigo").Value.ToUpper(), err.Element("Error").Attribute("mensaje").Value));
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            //Instanciando Excepciónes
                                                            acuse = new XDocument(result.Root.Element("Errores"));
                                                            retorno = new RetornoOperacion(string.Format("{0} - {1}", acuse.Root.Element("Error").Attribute("codigo").Value, acuse.Root.Element("Error").Attribute("mensaje").Value), false);
                                                            break;
                                                        }
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("No se puede recuperar el Error");
                                        }

                                        break;
                                    }
                                case "ConsultaCfdi":
                                    {
                                        //Recorriendo Profundidad de Nodos
                                        //1.- Root - EnvelopeSoap
                                        //2.- FirstNode - Body
                                        //3.- FirstNode - ConsultaComprobanteResponse
                                        //4.- FirstNode - ConsultaComprobanteResult

                                        //Validando que no sea un Error
                                        if (result.Root.Element("Acuse") != null)
                                        {
                                            //Asignando valores Positivos
                                            acuse = new XDocument(result.Root.Element("Acuse"));
                                            retorno = new RetornoOperacion(acuse.Root.Element("CodigoEstatus").Value, true);
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se puede recuperar el Acuse", false);

                                        break;
                                    }
                                case "ConsultaRelacionados":
                                    {
                                        //Recorriendo Profundidad de Nodos
                                        //1.- Root - EnvelopeSoap
                                        //2.- FirstNode - Body
                                        //3.- FirstNode - ConsultaComprobanteRelacionadosResponse
                                        //4.- FirstNode - ConsultaComprobanteRelacionadosResult

                                        //Validando que no sea un Error
                                        if (result.Root.Element("ConsultaRelacionados") != null)
                                        {
                                            //Asignando valores Positivos
                                            acuse = new XDocument(result.Root.Element("ConsultaRelacionados"));

                                            //Obteniendo Valor de Resultado
                                            retorno = new RetornoOperacion(Cadena.RegresaCadenaSeparada(acuse.Element("Resultado").Value, "Clave:", 1), true);
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se puede recuperar el Acuse", false);

                                        break;
                                    }
                                case "ConsultaCancelacionPendientes":
                                    {
                                        //Recorriendo Profundidad de Nodos
                                        //1.- Root - EnvelopeSoap
                                        //2.- FirstNode - Body
                                        //3.- FirstNode - ConsultaComprobanteRelacionadosResponse
                                        //4.- FirstNode - ConsultaComprobanteRelacionadosResult

                                        //Validando que no sea un Error
                                        if (result.Root.Element("AcusePeticionesPendientes") != null)
                                        {
                                            //Asignando valores Positivos
                                            acuse = new XDocument(result.Root.Element("AcusePeticionesPendientes"));

                                            //Obteniendo Valor de Resultado
                                            retorno = new RetornoOperacion(acuse.Root.Attribute("CodEstatus").Value, true);
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se puede recuperar el Acuse", false);

                                        break;
                                    }
                            }
                        }
                        else
                        {
                            //Instanciando Excepciónes
                            acuse = new XDocument(result.Root.Element("Errores"));
                            retorno = new RetornoOperacion(string.Format("{0} - {1}", acuse.Root.Element("Error").Attribute("codigo").Value, acuse.Root.Element("Error").Attribute("mensaje").Value), false);
                        }
                        break;
                    }
                case PacCancelacion.FacturemosYa:
                    {
                        //Obteniendo Nodo sin Datos del Response SOAP
                        XDocument result = new XDocument(response.Descendants("respuesta").FirstOrDefault());

                        //Validando Consulta
                        switch (consultaWS)
                        {
                            case "CancelarComprobante":
                                {
                                    //Recorriendo Profundidad de Nodos
                                    //1.- Root - EnvelopeSoap
                                    //2.- FirstNode - Body
                                    //3.- FirstNode - comprobarStatusCancelacionResponse
                                    //4.- FirstNode - respuesta

                                    //Validando Existencia
                                    if (result != null)
                                    {
                                        //Asignando valores Positivos
                                        acuse = result;
                                        //Obteniendo Valor de Resultado
                                        retorno = new RetornoOperacion(acuse.Root.Element("codigo").Value, true);
                                    }
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("No se puede recuperar el Acuse", false);

                                    break;
                                }
                            case "ConsultaCfdi":
                                {
                                    //Recorriendo Profundidad de Nodos
                                    //1.- Root - EnvelopeSoap
                                    //2.- FirstNode - Body
                                    //3.- FirstNode - CancelarCFDIResponse
                                    //4.- FirstNode - respuesta

                                    //Validando Existencia
                                    if (result != null)
                                    {
                                        //Asignando valores Positivos
                                        acuse = result;
                                        //Obteniendo Valor de Resultado
                                        retorno = new RetornoOperacion(acuse.Root.Element("codigo").Value, true);
                                    }
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("No se puede recuperar el Acuse", false);

                                    break;
                                }
                        }

                        break;
                    }
            }



            //Devolviendo Resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="rfc_receptor"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneReceptorCFDI(Comprobante cfdi, out string rfc_receptor)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("");
            rfc_receptor = "";

            //Validando Tipo de Comprobante
            switch (cfdi.id_tipo_comprobante)
            {
                case 1: //Ingreso
                case 2: //Egreso
                case 3: //Traslado
                case 5: //Pago
                    {
                        //Instanciando Receptor
                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new Global.CompaniaEmisorReceptor(cfdi.id_compania_receptor))
                        {
                            //Validando Receptor
                            if (receptor.habilitar)
                            {
                                //Instanciando Excepción
                                retorno = new RetornoOperacion(cfdi.id_comprobante33);
                                //Asignando RFC
                                rfc_receptor = receptor.rfc;
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                        }
                        break;
                    }
                case 4: //Nómina
                    {
                        //Instanciando Receptor
                        using (SAT_CL.Nomina.NomEmpleado nom_emp = new SAT_CL.Nomina.NomEmpleado(SAT_CL.Nomina.NomEmpleado.ObtieneIdNomEmpleadoV3_3(cfdi.id_comprobante33)))
                        using (SAT_CL.Global.Operador emp = new Global.Operador(nom_emp.id_empleado))
                        {
                            //Validando Empleado
                            if (nom_emp.habilitar && emp.habilitar)
                            {
                                //Instanciando Excepción
                                retorno = new RetornoOperacion(cfdi.id_comprobante33);
                                //Asignando RFC
                                rfc_receptor = emp.rfc;
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                        }
                            break;
                    }
            }

            //Devolviendo resultado Obtenido
            return retorno;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_receptor"></param>
        /// <param name="id_tipo_comprobante"></param>
        /// <param name="rfc_receptor"></param>
        /// <returns></returns>
        private RetornoOperacion obtieneReceptorCFDI(int id_receptor, int id_tipo_comprobante, out string rfc_receptor)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("");
            rfc_receptor = "";

            //Validando Tipo de Comprobante
            switch (id_tipo_comprobante)
            {
                case 1: //Ingreso
                case 2: //Egreso
                case 3: //Traslado
                case 5: //Pago
                    {
                        //Instanciando Receptor
                        using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new Global.CompaniaEmisorReceptor(id_receptor))
                        {
                            //Validando Receptor
                            if (receptor.habilitar)
                            {
                                //Instanciando Excepción
                                retorno = new RetornoOperacion(id_receptor);
                                //Asignando RFC
                                rfc_receptor = receptor.rfc;
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                        }
                        break;
                    }
                case 4: //Nómina
                    {
                        //Instanciando Receptor
                        using (SAT_CL.Global.Operador emp = new Global.Operador(id_receptor))
                        {
                            //Validando Empleado
                            if (emp.habilitar)
                            {
                                //Instanciando Excepción
                                retorno = new RetornoOperacion(id_receptor);
                                //Asignando RFC
                                rfc_receptor = emp.rfc;
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                        }
                        break;
                    }
            }

            //Devolviendo resultado Obtenido
            return retorno;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_comprobante"></param>
        /// <param name="estatusUUID"></param>
        /// <param name="tipoCancelacion"></param>
        /// <param name="acuse"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        public RetornoOperacion CancelacionComprobanteCxC(int id_comprobante, out EstatusUUID estatusUUID, out TipoCancelacion tipoCancelacion, out XDocument acuse, int id_usuario)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("");
            int id_pac_emisor = 0;
            PacCancelacion pacCancelacion = PacCancelacion.SinAsignar;
            estatusUUID = EstatusUUID.SinEstatus;
            acuse = null;
            string llaveBase64 = "";
            string certificadoBase64 = "";
            Comprobante.OrigenDatos origen = Comprobante.OrigenDatos.Facturado; int id_registro = 0;

            //Declarando Variable de Tipo
            tipoCancelacion = TipoCancelacion.SinAsignar;

            //Instanciando Comprobante
            using (Comprobante cfdi = new Comprobante(id_comprobante))
            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
            {
                //Validando que exista el Comprobante
                if (cfdi.habilitar && tfd.habilitar)
                {
                    //Validando Vigencia
                    if ((Comprobante.EstatusVigencia)cfdi.id_estatus_vigencia == Comprobante.EstatusVigencia.Vigente || (Comprobante.EstatusVigencia)cfdi.id_estatus_vigencia == Comprobante.EstatusVigencia.PendienteCancelacion)
                    {
                        //Validando Emisor del Comprobante
                        if (emisor.habilitar)
                        {
                            //Validando Receptor
                            string rfc_receptor = "";
                            retorno = obtieneReceptorCFDI(cfdi, out rfc_receptor);

                            //Validando Receptor del Comprobante
                            if (retorno.OperacionExitosa)
                            {
                                //Obteniendo PAC por Emisor
                                retorno = obtienePacCompania(emisor.id_compania_emisor_receptor, out pacCancelacion, out id_pac_emisor);

                                //Validando Retorno
                                if (retorno.OperacionExitosa)
                                {
                                    //Instanciamos Pac
                                    using (SAT_CL.FacturacionElectronica.PacCompaniaEmisor pac = new SAT_CL.FacturacionElectronica.PacCompaniaEmisor(id_pac_emisor))
                                    {
                                        //Validando PAC's
                                        if (pac.hablitar)
                                        {
                                            //Validando PAC
                                            switch (pacCancelacion)
                                            {
                                                case PacCancelacion.Facturador:
                                                    {
                                                        //Obtención de Datos del Certificado
                                                        retorno = obtieneDatosCertificado(emisor.id_compania_emisor_receptor, out llaveBase64, out certificadoBase64);

                                                        //Validando Retorno
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Creando Petición de Envio
                                                            string peticion = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                                                                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tim=""http://facturadorelectronico.com/timbrado/"">
                                                                                    <soapenv:Header />
                                                                                    <soapenv:Body>
                                                                                        <tim:CancelarComprobante>
                                                                                            <tim:xml></tim:xml>
                                                                                            <tim:usuario></tim:usuario>
                                                                                            <tim:password></tim:password>
                                                                                        </tim:CancelarComprobante>
                                                                                    </soapenv:Body>
                                                                                </soapenv:Envelope>";

                                                            //Creando Solicitud de Consulta
                                                            string solicitud = string.Format(@"<Cancelacion llaveCertificado=""{0}"" certificado=""{1}"" rfcEmisor=""{2}"" ><Folios><Folio UUID=""{3}"" total=""{4:0.00}"" rfcReceptor=""{5}"" /></Folios></Cancelacion>",
                                                                                               llaveBase64, certificadoBase64, emisor.rfc, tfd.UUID, cfdi.total_captura, rfc_receptor);

                                                            //Declaramos String Buider
                                                            StringBuilder sb = new StringBuilder(peticion);

                                                            //Insertamos Usuario
                                                            sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), pac.usuario_web_servie);
                                                            //Insertamos Contraseña
                                                            sb.Insert(sb.ToString().IndexOf("</tim:password>"), pac.contrasena_web_service);
                                                            //Insertamos Solicitud
                                                            sb.Insert(sb.ToString().IndexOf("</tim:xml>"), @"<![CDATA[<?xml version=""1.0"" encoding=""utf-8""?>" + solicitud.ToString() + @"]]>");

                                                            //Obteniendo respuesta de la Consulta
                                                            retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), XDocument.Parse(sb.ToString()));

                                                            //Validando Retorno
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                //Obtenemos Documento generado
                                                                acuse = XDocument.Parse(retorno.Mensaje);

                                                                //Validando Documento
                                                                if (acuse != null)
                                                                {
                                                                    //Gestionando resultado de Consumo del WS
                                                                    retorno = devuelveResultadoWS(acuse, tfd.UUID, pacCancelacion, "CancelarComprobante", out acuse);

                                                                    //Validando Error de Cancelación
                                                                    if (!retorno.OperacionExitosa)
                                                                    {
                                                                        //Obteniendo Resultado
                                                                        string error = Cadena.RegresaCadenaSeparada(retorno.Mensaje, " - ", 0);
                                                                        string msj_error = Cadena.RegresaCadenaSeparada(retorno.Mensaje, " - ", 1);

                                                                        //Validando Error
                                                                        switch (error)
                                                                        {
                                                                            case "FET101":
                                                                                {
                                                                                    //Declarando auxiliares
                                                                                    string rfc_emisor, UUID;
                                                                                    decimal monto; DateTime fecha_expedicion;

                                                                                    //Obteniendo Archivo
                                                                                    byte[] comp = System.IO.File.ReadAllBytes(cfdi.ruta_xml);

                                                                                    //Declarando Documento XML
                                                                                    XmlDocument xmlDocument = new XmlDocument();
                                                                                    XDocument xDocument = new XDocument();

                                                                                    //Obteniendo XML en cadena
                                                                                    using (MemoryStream ms = new MemoryStream(comp))
                                                                                        //Cargando Documento XML
                                                                                        xmlDocument.Load(ms);

                                                                                    //Convirtiendo XML
                                                                                    xDocument = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(xmlDocument);

                                                                                    //Realizando validación de estatus en SAT
                                                                                    retorno = SAT_CL.FacturacionElectronica.Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);
                                                                                    if (!retorno.OperacionExitosa)
                                                                                    {
                                                                                        //Creando Acuse del Error
                                                                                        acuse = XDocument.Parse(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                                                                                    <Solicitud xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns="""" esValido=""True"" tipoCancelacion=""Cancelable con aceptación"">
                                                                                                      <Folios>
                                                                                                        <Folio UUID=""{0}"" Estatus=""202"" />
                                                                                                      </Folios>
                                                                                                      <Acuse Fecha=""{1:yyyy-MM-dd}T{1:HH:mm:ss}"">
                                                                                                        <Folios xmlns=""http://cancelacfd.sat.gob.mx"">
                                                                                                          <UUID>{0}</UUID>
                                                                                                          <EstatusUUID>202</EstatusUUID>
                                                                                                        </Folios>
                                                                                                      </Acuse>
                                                                                                    </Solicitud>", tfd.UUID.ToUpper(), Fecha.ObtieneFechaEstandarMexicoCentro()));
                                                                                        //Asignando Resultado Positivo
                                                                                        retorno = new RetornoOperacion(1, "Comprobante Cancelado Previamente", true);
                                                                                    }
                                                                                    else
                                                                                        retorno = new RetornoOperacion(msj_error);
                                                                                    break;
                                                                                }
                                                                            default:
                                                                                {
                                                                                    break;
                                                                                }
                                                                        }
                                                                    }
                                                                    
                                                                    //Validando Resultado
                                                                    if (retorno.OperacionExitosa)
                                                                    {
                                                                        //Obteniendo Tipo de Cancelación
                                                                        tipoCancelacion = acuse.Root.Attribute("tipoCancelacion").Value.Equals("Cancelable sin aceptación") ? TipoCancelacion.CancelacionSinAceptacion : TipoCancelacion.CancelacionConAceptacion;

                                                                        //Gestionando Resultado en Diccionario de Resultados
                                                                        retorno = obtieneResultadoEstatusUUID(acuse.Root.Element("Folios").Element("Folio").Attribute("Estatus").Value, out estatusUUID);

                                                                        //Validando Resultado
                                                                        if (retorno.OperacionExitosa)
                                                                        {
                                                                            //Inicializando Bloque Transaccional
                                                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                            {
                                                                                //Insertando Encabezado
                                                                                retorno = AcuseCancelacion.InsertaAcuseCancelacion(((int)estatusUUID).ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), acuse, "3.3", id_usuario);

                                                                                //Validando Resultado
                                                                                if (retorno.OperacionExitosa)
                                                                                {
                                                                                    //Obteniendo Acuse
                                                                                    int idAcuse = retorno.IdRegistro;

                                                                                    //Validando Tipo de Cancelación
                                                                                    if (tipoCancelacion == TipoCancelacion.CancelacionSinAceptacion)
                                                                                    {
                                                                                        //Insertando Detalle
                                                                                        retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.CanceladoSinAceptacion, id_usuario);

                                                                                        //Validando Resultado
                                                                                        if (retorno.OperacionExitosa)
                                                                                        {
                                                                                            //Obteniendo Origen del Comprobante
                                                                                            retorno = Comprobante.ObtieneOrigenDatosComprobante(cfdi.id_comprobante33, out origen, out id_registro);

                                                                                            //Validando Resultado
                                                                                            if (retorno.OperacionExitosa)
                                                                                            {
                                                                                                //Cancelando Comprobante
                                                                                                retorno = cfdi.CancelaComprobante(id_usuario);

                                                                                                //Validando Operación
                                                                                                if (retorno.OperacionExitosa)
                                                                                                {
                                                                                                    //Validando Origen de Datos
                                                                                                    switch (origen)
                                                                                                    {
                                                                                                        case Comprobante.OrigenDatos.Facturado:
                                                                                                            {
                                                                                                                //Deshabilitando Aplicaciones de Pago
                                                                                                                retorno = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(id_registro, cfdi.id_comprobante33, id_usuario);
                                                                                                                break;
                                                                                                            }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    //Si requiere Aceptación
                                                                                    else if (tipoCancelacion == TipoCancelacion.CancelacionConAceptacion)
                                                                                    {
                                                                                        //Consultando CFDI
                                                                                        RespuestaConsultaCFDI respuesta = RespuestaConsultaCFDI.SinRespuesta;
                                                                                        XDocument consultaCFDI = new XDocument();
                                                                                        retorno = ConsultaComprobanteCxC(cfdi.id_comprobante33, out respuesta, out consultaCFDI);

                                                                                        //Validando Resultado
                                                                                        if (retorno.OperacionExitosa && respuesta == RespuestaConsultaCFDI.ComprobanteObtenido)
                                                                                        {
                                                                                            //Obteniendo Estatus
                                                                                            string esCancelable = consultaCFDI.Root.Element("EsCancelable").Value,
                                                                                                   estado = consultaCFDI.Root.Element("Estado").Value,
                                                                                                   estatusCancelacion = consultaCFDI.Root.Element("EstatusCancelacion").Value;

                                                                                            //Validando Estado del Comprobante
                                                                                            if (estado.ToUpper().Equals("CANCELADO"))
                                                                                            {
                                                                                                //Insertando Detalle
                                                                                                retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.CanceladoSinAceptacion, id_usuario);

                                                                                                //Validando Resultado
                                                                                                if (retorno.OperacionExitosa)
                                                                                                {
                                                                                                    //Cancelando Comprobante
                                                                                                    retorno = cfdi.CancelaComprobante(id_usuario);

                                                                                                    //Validando Resultado
                                                                                                    if (retorno.OperacionExitosa)
                                                                                                    {
                                                                                                        //Validando Origen de Datos
                                                                                                        switch (origen)
                                                                                                        {
                                                                                                            case Comprobante.OrigenDatos.Facturado:
                                                                                                                {
                                                                                                                    //Deshabilitando Aplicaciones de Pago
                                                                                                                    retorno = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(id_registro, cfdi.id_comprobante33, id_usuario);
                                                                                                                    break;
                                                                                                                }
                                                                                                        }

                                                                                                        //Validando Resultado
                                                                                                        if (retorno.OperacionExitosa)

                                                                                                            //Instanciando Retorno Positivo
                                                                                                            retorno = new RetornoOperacion(cfdi.id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Cancelado Exitosamente", cfdi.serie, cfdi.folio), true);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                //Validando que el CFDI este en Proceso de Cancelación
                                                                                                if (estado.ToUpper().Equals("VIGENTE") && estatusCancelacion.ToUpper().Equals("EN PROCESO"))
                                                                                                {
                                                                                                    //Insertando Detalle
                                                                                                    retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.PendienteCancelacion, id_usuario);

                                                                                                    //Validando operación
                                                                                                    if (retorno.OperacionExitosa)

                                                                                                        //Instanciando Retorno Positivo
                                                                                                        retorno = new RetornoOperacion(cfdi.id_comprobante33, string.Format("El Comprobante '{0}{1}' fue sigue en Proceso de Cancelación", cfdi.serie, cfdi.folio), true);
                                                                                                }
                                                                                                //Validando que el CFDI siga vigente 
                                                                                                else if (estado.ToUpper().Equals("VIGENTE") && (estatusCancelacion.ToUpper().Equals("") || estatusCancelacion.ToUpper().Equals("PLAZO VENCIDO")))
                                                                                                {
                                                                                                    //Insertando Detalle
                                                                                                    retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.Rechazado, id_usuario);

                                                                                                    //Validando operación
                                                                                                    if (retorno.OperacionExitosa)
                                                                                                    {
                                                                                                        //Cancelando Comprobante
                                                                                                        retorno = cfdi.CancelacionRechazada(id_usuario);

                                                                                                        //Validando Operación
                                                                                                        if (retorno.OperacionExitosa)

                                                                                                            //Instanciando Retorno Positivo
                                                                                                            retorno = new RetornoOperacion(cfdi.id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Rechazado por el Cliente.", cfdi.serie, cfdi.folio), true);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    //Instanciando Retorno Positivo
                                                                                    if (retorno.OperacionExitosa)

                                                                                        //Completando Transacción
                                                                                        scope.Complete();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case PacCancelacion.FacturemosYa:
                                                    {
                                                        //Declaramos Variable para Armar Soap
                                                        string _soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                                                                <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                                                   <soapenv:Header/>
                                                                                   <soapenv:Body>
                                                                                      <CancelarCFDI soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                                                                                         <usuario xsi:type=""xsd:string""></usuario>
                                                                                         <contra xsi:type=""xsd:string""></contra>
                                                                                         <uuid xsi:type=""xsd:string""></uuid>
                                                                                      </CancelarCFDI>
                                                                                   </soapenv:Body>
                                                                                </soapenv:Envelope>";

                                                        //Declaramos String Buider
                                                        StringBuilder sb = new StringBuilder(_soapEnvelope);
                                                        //Insertamos Usuario
                                                        sb.Insert(sb.ToString().IndexOf("</usuario>"), pac.usuario_web_servie);
                                                        //Insertamos Contraseña
                                                        sb.Insert(sb.ToString().IndexOf("</contra>"), pac.contrasena_web_service);
                                                        //Insertamos Contenido
                                                        sb.Insert(sb.ToString().IndexOf("</uuid>"), tfd.UUID);

                                                        //Creamos soap envelope en xml
                                                        XDocument soapEnvelopeXml = XDocument.Parse(sb.ToString());

                                                        //Obteniendo respuesta de la Consulta
                                                        retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), soapEnvelopeXml);

                                                        //Validando Retorno
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Obtenemos Documento generado
                                                            acuse = XDocument.Parse(retorno.Mensaje);

                                                            //Validando Documento
                                                            if (acuse != null)
                                                            {
                                                                //Gestionando resultado de Consumo del WS
                                                                retorno = devuelveResultadoWS(acuse, tfd.UUID, pacCancelacion, "CancelarComprobante", out acuse);

                                                                //Validando Resultado
                                                                if (retorno.OperacionExitosa)
                                                                {
                                                                    //Gestionando Resultado en Diccionario de Resultados
                                                                    retorno = obtieneResultadoEstatusUUID(retorno.Mensaje, out estatusUUID);

                                                                    //Validando Resultado
                                                                    if (retorno.OperacionExitosa)
                                                                    {
                                                                        //Inicializando Bloque Transaccional
                                                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                                        {
                                                                            //Insertando Encabezado
                                                                            retorno = AcuseCancelacion.InsertaAcuseCancelacion(((int)estatusUUID).ToString(), Fecha.ObtieneFechaEstandarMexicoCentro(), acuse, "3.3", id_usuario);

                                                                            //Validando Resultado
                                                                            if (retorno.OperacionExitosa)
                                                                            {
                                                                                //Obteniendo Acuse
                                                                                int idAcuse = retorno.IdRegistro;

                                                                                //Validando Estatus
                                                                                switch (estatusUUID)
                                                                                {
                                                                                    case EstatusUUID.AceptacionCliente:
                                                                                    case EstatusUUID.CancelacionPreviaAutomatica:
                                                                                    case EstatusUUID.Cancelado:
                                                                                    case EstatusUUID.Exito:
                                                                                        {
                                                                                            //Insertando Detalle
                                                                                            retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.CanceladoSinAceptacion, id_usuario);

                                                                                            //Validando Resultado
                                                                                            if (retorno.OperacionExitosa)
                                                                                            {
                                                                                                //Cancelando Comprobante
                                                                                                retorno = cfdi.CancelaComprobante(id_usuario);

                                                                                                //Validando Resultado
                                                                                                if (retorno.OperacionExitosa)

                                                                                                    //Instanciando Retorno Positivo
                                                                                                    retorno = new RetornoOperacion(cfdi.id_comprobante33, string.Format("El Comprobante '{0}{1}' fue Cancelado Exitosamente", cfdi.serie, cfdi.folio), true);
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                    case EstatusUUID.EsperaCancelacion:
                                                                                        {
                                                                                            //Insertando Detalle
                                                                                            retorno = AcuseCancelacionDetalle.InsertarAcuseDetalleCancelacion(idAcuse, tfd.id_timbre_fiscal_digital, (byte)AcuseCancelacionDetalle.EstatusCancelacion.PendienteCancelacion, id_usuario);

                                                                                            //Validando operación
                                                                                            if (retorno.OperacionExitosa)

                                                                                                //Instanciando Retorno Positivo
                                                                                                retorno = new RetornoOperacion(cfdi.id_comprobante33, string.Format("El Comprobante '{0}{1}' fue sigue en Proceso de Cancelación", cfdi.serie, cfdi.folio), true);
                                                                                            break;
                                                                                        }
                                                                                    default:
                                                                                        {
                                                                                            //Instanciando Excepción
                                                                                            retorno = new RetornoOperacion("Error no Clasificado");
                                                                                            break;
                                                                                        }
                                                                                }

                                                                                //Validando Resultado
                                                                                if (retorno.OperacionExitosa)
                                                                                {
                                                                                    //Instanciando Comprobante
                                                                                    retorno = new RetornoOperacion(id_comprobante);
                                                                                    //Completando Transacción
                                                                                    scope.Complete();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No se pueden recuperar el PAC del Emisor");
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("No se puede recuperar el Emisor de Comprobante");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el Emisor de Comprobante");
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Comprobante o el Timbre Fiscal Digital");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public RetornoOperacion ConsultaComprobanteCxC(int id_comprobante, out RespuestaConsultaCFDI respuestaConsulta, out XDocument acuse)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("");
            int id_pac_emisor = 0;
            PacCancelacion pacCancelacion = PacCancelacion.SinAsignar;
            respuestaConsulta = RespuestaConsultaCFDI.SinRespuesta;
            acuse = null;
            string llaveBase64 = "";
            string certificadoBase64 = "";

            //Instanciando Comprobante
            using (Comprobante cfdi = new Comprobante(id_comprobante))
            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
            {
                //Validando que exista el Comprobante
                if (cfdi.habilitar && tfd.habilitar)
                {
                    //Validando Emisor del Comprobante
                    if (emisor.habilitar)
                    {
                        //Validando Receptor
                        string rfc_receptor = "";
                        retorno = obtieneReceptorCFDI(cfdi, out rfc_receptor);

                        //Validando Receptor del Comprobante
                        if (retorno.OperacionExitosa)
                        {
                            //Obteniendo PAC por Emisor
                            retorno = obtienePacCompania(emisor.id_compania_emisor_receptor, out pacCancelacion, out id_pac_emisor);

                            //Validando Retorno
                            if (retorno.OperacionExitosa)
                            {
                                //Validando PAC
                                switch (pacCancelacion)
                                {
                                    case PacCancelacion.Facturador:
                                        {
                                            //Instanciamos Pac
                                            using (SAT_CL.FacturacionElectronica.PacCompaniaEmisor pac = new SAT_CL.FacturacionElectronica.PacCompaniaEmisor(id_pac_emisor))
                                            {
                                                //Validando PAC's
                                                if (pac.hablitar)
                                                {
                                                    decimal total_cfdi = 0.00M;
                                                    if (!string.IsNullOrEmpty(cfdi.ruta_xml))
                                                    {
                                                        try
                                                        {
                                                            //Obteniendo Archivo
                                                            byte[] xml = System.IO.File.ReadAllBytes(cfdi.ruta_xml);

                                                            //Declarando Documento XML
                                                            XmlDocument xmlDocument = new XmlDocument();
                                                            
                                                            //Obteniendo XML en cadena
                                                            using (MemoryStream ms = new MemoryStream(xml))
                                                                //Cargando Documento XML
                                                                xmlDocument.Load(ms);

                                                            if (xmlDocument != null)
                                                                //Obteniendo Total
                                                                total_cfdi = Convert.ToDecimal(xmlDocument.DocumentElement.Attributes["Total"].Value);
                                                        }
                                                        catch { total_cfdi = Math.Truncate(100 * cfdi.total_captura) / 100; }
                                                    }

                                                    //Obtención de Datos del Certificado
                                                    retorno = obtieneDatosCertificado(emisor.id_compania_emisor_receptor, out llaveBase64, out certificadoBase64);

                                                    //Validando Retorno
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        //Creando Petición de Envio
                                                        string peticion = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' 
                                                                                xmlns:tim='http://facturadorelectronico.com/timbrado/'>
                                                                                <soapenv:Header />
                                                                                <soapenv:Body>
                                                                                    <tim:ConsultaComprobante>
                                                                                        <tim:xml></tim:xml>
                                                                                        <tim:usuario></tim:usuario>
                                                                                        <tim:password></tim:password>
                                                                                    </tim:ConsultaComprobante>
                                                                                </soapenv:Body>
                                                                            </soapenv:Envelope>";

                                                        //Creando Solicitud de Consulta
                                                        string solicitud = string.Format(@"<ConsultaCfdi rfcEmisor=""{0}"" rfcReceptor=""{1}"" UUID=""{2}"" total=""{3}"" certificado=""{4}"" llaveCertificado=""{5}"" />", emisor.rfc, rfc_receptor, tfd.UUID, total_cfdi, certificadoBase64, llaveBase64);
                                                        XDocument soapSolicitud = XDocument.Parse(solicitud);
                                                        soapSolicitud.Declaration = new XDeclaration("1.0", "utf-8", null);

                                                        //Declaramos String Buider
                                                        StringBuilder sb = new StringBuilder(peticion);

                                                        //Insertamos Usuario
                                                        sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), pac.usuario_web_servie);
                                                        //Insertamos Contraseña
                                                        sb.Insert(sb.ToString().IndexOf("</tim:password>"), pac.contrasena_web_service);
                                                        //Insertamos Solicitud
                                                        sb.Insert(sb.ToString().IndexOf("</tim:xml>"), @"<![CDATA[" + soapSolicitud.ToString() + @"]]>");

                                                        //Configurando Codificación del XML
                                                        XDocument soapEnvelope = XDocument.Parse(sb.ToString());
                                                        soapEnvelope.Declaration = new XDeclaration("1.0", "utf-8", null);

                                                        //Obteniendo respuesta de la Consulta
                                                        retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), soapEnvelope);

                                                        //Validando Retorno
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Obtenemos Documento generado
                                                            acuse = XDocument.Parse(retorno.Mensaje);

                                                            //Validando Documento
                                                            if (acuse != null)
                                                            {
                                                                //Gestionando resultado de Consumo del WS
                                                                retorno = devuelveResultadoWS(acuse, tfd.UUID, pacCancelacion, "ConsultaCfdi", out acuse);

                                                                //Validando Resultado
                                                                if (retorno.OperacionExitosa)

                                                                    //Gestionando Resultado en Diccionario de Resultados
                                                                    retorno = obtieneMensajeResultadoConsultaCFDI(retorno.Mensaje, out respuestaConsulta);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("No se pueden recuperar el PAC del Emisor");
                                            }
                                            break;
                                        }
                                    case PacCancelacion.FacturemosYa:
                                        {
                                            //Instanciamos Pac
                                            using (SAT_CL.FacturacionElectronica.PacCompaniaEmisor pac = new SAT_CL.FacturacionElectronica.PacCompaniaEmisor(id_pac_emisor))
                                            {
                                                //Validando PAC's
                                                if (pac.hablitar)
                                                {
                                                    //Creando Petición de Envio
                                                    string peticion = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                                            <soapenv:Header/>
                                                                            <soapenv:Body>
                                                                              <comprobarStatusCancelacion soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
                                                                                 <usuario xsi:type='xsd:string'></usuario>
                                                                                 <contra xsi:type='xsd:string'></contra>
                                                                                 <uuid xsi:type='xsd:string'></uuid>
                                                                              </comprobarStatusCancelacion>
                                                                            </soapenv:Body>
                                                                        </soapenv:Envelope>";

                                                    //Declaramos String Buider
                                                    StringBuilder sb = new StringBuilder(peticion);

                                                    //Insertamos Usuario
                                                    sb.Insert(sb.ToString().IndexOf("</usuario>"), pac.usuario_web_servie);
                                                    //Insertamos Contraseña
                                                    sb.Insert(sb.ToString().IndexOf("</contra>"), pac.contrasena_web_service);
                                                    //Insertamos Solicitud
                                                    sb.Insert(sb.ToString().IndexOf("</uuid>"), tfd.UUID);

                                                    //Configurando Codificación del XML
                                                    XDocument soapEnvelope = XDocument.Parse(sb.ToString());
                                                    soapEnvelope.Declaration = new XDeclaration("1.0", "utf-8", null);

                                                    //Obteniendo respuesta de la Consulta
                                                    retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), soapEnvelope);

                                                    //Validando Retorno
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        //Obtenemos Documento generado
                                                        acuse = XDocument.Parse(retorno.Mensaje);

                                                        //Validando Documento
                                                        if (acuse != null)
                                                        {
                                                            //Gestionando resultado de Consumo del WS
                                                            retorno = devuelveResultadoWS(acuse, tfd.UUID, pacCancelacion, "ConsultaCfdi", out acuse);

                                                            //Validando Resultado
                                                            if (retorno.OperacionExitosa)

                                                                //Gestionando Resultado en Diccionario de Resultados
                                                                retorno = obtieneMensajeResultadoConsultaCFDI(retorno.Mensaje, out respuestaConsulta);
                                                        }
                                                    }
                                                }
                                                else
                                                    //Instanciando Excepción
                                                    retorno = new RetornoOperacion("No se pueden recuperar el PAC del Emisor");
                                            }
                                            break;
                                        }
                                    default:
                                        {
                                            //Instanciando Excepción
                                            retorno = new RetornoOperacion("No tiene un PAC asignado, contacte a su provedor de Servicio de Facturación");
                                            break;
                                        }
                                }
                            }

                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el Emisor de Comprobante");
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Comprobante o el Timbre Fiscal Digital");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public RetornoOperacion ConsultaComprobanteRelacionadoCxC(int id_comprobante, out RespuestaConsultaRelacionados respuestaRelacionados, out XDocument consulta)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("");
            respuestaRelacionados = RespuestaConsultaRelacionados.SinRespuesta;
            PacCancelacion pacCancelacion = PacCancelacion.SinAsignar;
            int id_pac_emisor = 0;
            consulta = null;
            string llaveBase64 = "";
            string certificadoBase64 = "";

            //Instanciando Comprobante
            using (Comprobante cfdi = new Comprobante(id_comprobante))
            using (TimbreFiscalDigital tfd = TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
            {
                //Validando que exista el Comprobante
                if (cfdi.habilitar && tfd.habilitar)
                {
                    //Validando Emisor del Comprobante
                    if (emisor.habilitar)
                    {
                        //Validando Receptor
                        string rfc_receptor = "";
                        retorno = obtieneReceptorCFDI(cfdi, out rfc_receptor);

                        //Validando Receptor del Comprobante
                        if (retorno.OperacionExitosa)
                        {
                            //Obtención de Datos del Certificado
                            retorno = obtieneDatosCertificado(emisor.id_compania_emisor_receptor, out llaveBase64, out certificadoBase64);

                            //Validando Retorno
                            if (retorno.OperacionExitosa)
                            {
                                //Obteniendo PAC por Emisor
                                retorno = obtienePacCompania(emisor.id_compania_emisor_receptor, out pacCancelacion, out id_pac_emisor);

                                //Validando Retorno
                                if (retorno.OperacionExitosa)
                                {
                                    //Validando PAC
                                    switch (pacCancelacion)
                                    {
                                        case PacCancelacion.Facturador:
                                            {
                                                //Instanciamos Pac
                                                using (SAT_CL.FacturacionElectronica.PacCompaniaEmisor pac = new SAT_CL.FacturacionElectronica.PacCompaniaEmisor(id_pac_emisor))
                                                {
                                                    //Validando PAC's
                                                    if (pac.hablitar)
                                                    {
                                                        //Creando Petición de Envio
                                                        string peticion = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tim=""http://facturadorelectronico.com/timbrado/"">
                                                                   <soapenv:Header/>
                                                                   <soapenv:Body>
                                                                      <tim:ConsultaComprobanteRelacionados>
                                                                         <tim:xml></tim:xml>
                                                                         <tim:usuario></tim:usuario>
                                                                         <tim:password></tim:password>
                                                                      </tim:ConsultaComprobanteRelacionados>
                                                                   </soapenv:Body>
                                                                </soapenv:Envelope>";

                                                        //Creando Solicitud de Consulta
                                                        string solicitud = string.Format(@"<ConsultaRelacionado UUID=""{0}"" llaveCertificado=""{1}"" certificado=""{2}"" rfcReceptor=""{3}"" />", tfd.UUID, llaveBase64, certificadoBase64, rfc_receptor);
                                                        XDocument soapSolicitud = XDocument.Parse(solicitud);
                                                        soapSolicitud.Declaration = new XDeclaration("1.0", "utf-8", null);

                                                        //Declaramos String Buider
                                                        StringBuilder sb = new StringBuilder(peticion);

                                                        //Insertamos Usuario
                                                        sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), pac.usuario_web_servie);
                                                        //Insertamos Contraseña
                                                        sb.Insert(sb.ToString().IndexOf("</tim:password>"), pac.contrasena_web_service);
                                                        //Insertamos Solicitud
                                                        sb.Insert(sb.ToString().IndexOf("</tim:xml>"), @"<![CDATA[" + soapSolicitud.ToString() + @"]]>");

                                                        //Configurando Codificación del XML
                                                        XDocument soapEnvelope = XDocument.Parse(sb.ToString());
                                                        soapEnvelope.Declaration = new XDeclaration("1.0", "utf-8", null);

                                                        //Obteniendo respuesta de la Consulta
                                                        retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), soapEnvelope);

                                                        //Validando Retorno
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Obtenemos Documento generado
                                                            consulta = XDocument.Parse(retorno.Mensaje);

                                                            //Validando Documento
                                                            if (consulta != null)
                                                            {
                                                                //Gestionando resultado de Consumo del WS
                                                                retorno = devuelveResultadoWS(consulta, tfd.UUID, pacCancelacion, "ConsultaRelacionados", out consulta);

                                                                //Validando Resultado
                                                                if (retorno.OperacionExitosa)

                                                                    //Gestionando Resultado en Diccionario de Resultados
                                                                    retorno = obtieneRespuestaConsultaRelacionados(Cadena.RegresaCadenaSeparada(retorno.Mensaje.Trim(), "-", 0), out respuestaRelacionados);
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        case PacCancelacion.FacturemosYa:
                                            {

                                                break;
                                            }
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepción
                            retorno = new RetornoOperacion("No se puede recuperar el Receptor de Comprobante");
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el Emisor de Comprobante");
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Comprobante o el Timbre Fiscal Digital");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        public RetornoOperacion ConsultaCancelacionPendiente(int id_compania_emisora, int id_compania_receptora, out RespuestaPeticionesPendientes respuestaPeticiones, out XDocument pendientes)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion();
            int id_pac_emisor = 0;
            PacCancelacion pacCancelacion = PacCancelacion.SinAsignar;
            pendientes = new XDocument();
            respuestaPeticiones = RespuestaPeticionesPendientes.SinRespuesta;

            //Instanciando Datos de los Comprobantes
            using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new Global.CompaniaEmisorReceptor(id_compania_emisora))
            using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new Global.CompaniaEmisorReceptor(id_compania_receptora))
            {
                //Validando Emisor
                if (emisor.habilitar)
                {
                    //Validando Receptor
                    if (receptor.habilitar)
                    {
                        //Obteniendo PAC por Emisor
                        retorno = obtienePacCompania(emisor.id_compania_emisor_receptor, out pacCancelacion, out id_pac_emisor);

                        //Validando Retorno
                        if (retorno.OperacionExitosa)
                        {
                            //Validando PAC
                            switch (pacCancelacion)
                            {
                                case PacCancelacion.Facturador:
                                    {
                                        //Instanciamos Pac
                                        using (SAT_CL.FacturacionElectronica.PacCompaniaEmisor pac = new SAT_CL.FacturacionElectronica.PacCompaniaEmisor(id_pac_emisor))
                                        {
                                            //Validando PAC's
                                            if (pac.hablitar)
                                            {
                                                //Creando Petición de Envio
                                                string peticion = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                                        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tim=""http://facturadorelectronico.com/timbrado/"">
                                                           <soapenv:Header/>
                                                           <soapenv:Body>
                                                              <tim:ConsultaCancelacionPendientes>
                                                                 <tim:rfcReceptor></tim:rfcReceptor>
                                                                 <tim:usuario></tim:usuario>
                                                                 <tim:password></tim:password>
                                                              </tim:ConsultaCancelacionPendientes>
                                                           </soapenv:Body>
                                                        </soapenv:Envelope>";

                                                //Declaramos String Buider
                                                StringBuilder sb = new StringBuilder(peticion);

                                                //Insertamos Usuario
                                                sb.Insert(sb.ToString().IndexOf("</tim:usuario>"), pac.usuario_web_servie);
                                                //Insertamos Contraseña
                                                sb.Insert(sb.ToString().IndexOf("</tim:password>"), pac.contrasena_web_service);
                                                //Insertamos Solicitud
                                                sb.Insert(sb.ToString().IndexOf("</tim:rfcReceptor>"), receptor.rfc);

                                                //Obteniendo respuesta de la Consulta
                                                retorno = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(pac.ubicacion_web_service), XDocument.Parse(sb.ToString()));

                                                //Validando Retorno
                                                if (retorno.OperacionExitosa)
                                                {
                                                    //Obtenemos Documento generado
                                                    pendientes = XDocument.Parse(retorno.Mensaje);

                                                    //Gestionando resultado de Consumo del WS
                                                    retorno = devuelveResultadoWS(pendientes, "", pacCancelacion, "ConsultaCancelacionPendientes", out pendientes);

                                                    //Validando Resultado
                                                    if (retorno.OperacionExitosa)

                                                        //Gestionando Resultado en Diccionario de Resultados
                                                        retorno = obtieneResultadoPeticionesPendientes(retorno.Mensaje, out respuestaPeticiones);
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion("No se pueden recuperar el PAC del Emisor");
                                        }
                                        break;
                                    }
                                case PacCancelacion.FacturemosYa:
                                    {

                                        break;
                                    }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el Receptor");
                }
                else
                    //Instanciando Excepción
                    retorno = new RetornoOperacion("No se puede recuperar el Emisor");
            }

            //Devolviendo Resultado Obtenido
            return retorno;
        }

        #endregion
    }
}