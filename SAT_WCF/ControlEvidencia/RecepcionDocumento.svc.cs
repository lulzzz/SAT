using SAT_CL.ControlEvidencia;
using SAT_CL.Despacho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Transactions;
using TSDK.Base;
using TSDK.Datos;

namespace SAT_WCF.ControlEvidencia
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "RecepcionDocumento" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione RecepcionDocumento.svc o RecepcionDocumento.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class RecepcionDocumento : IRecepcionDocumento
    {
        /// <summary>
        /// Método encargado de Insertar el Servicio Control Evidencia de Todos los Segmentos
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string InsertaServicioControlEvidencia(int id_servicio, int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Validamos que exista un Servicio Control Evidencia
                        using (ServicioControlEvidencia objServicioControl = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, id_servicio))
                        {
                            //Validmos que exista el Servicio Control Evidencia
                            if (objServicioControl.id_servicio_control_evidencia <= 0)
                            {
                                //Insertamos Servicio Control Evidencia
                                result = ServicioControlEvidencia.InsertaServicioControlEvidencia(id_servicio, Fecha.ObtieneFechaEstandarMexicoCentro(), user_sesion.id_usuario);

                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Parada
                                    int idRegistro = result.IdRegistro;

                                    //Actualizando Ultima Actividad
                                    result = user_sesion.ActualizaUltimaActividad();

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Servicio
                                        result = new RetornoOperacion(idRegistro);

                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion(id_servicio, "", true);

                            //Personalisando resultado con número de servicio
                            result = new RetornoOperacion(result.IdRegistro, string.Format("Servicio {0}: {1}", id_servicio, result.Mensaje), result.OperacionExitosa);
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
        /// <summary>
        /// Método encargado de Insertar la Evidencia del Documento
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        /// <param name="id_servicio_ce">Servicio Control Evidencia</param>
        /// <param name="id_segmento">Segmento</param>
        /// <param name="id_segmento_ce">Segmento Control Evidencia</param>
        /// <param name="id_hoja_instruccion_doc">Documento de la Hoja de Instrucción</param>
        /// <param name="id_lugar_cobro">Lugar de Cobro</param>
        /// <param name="documento">Documento</param>
        /// <param name="id_sesion">Sesión del Usuario que Actualiza el Registro</param>
        /// <returns></returns>
        public string InsertaControlEvidenciaDocumento(int id_servicio, int id_servicio_ce, int id_segmento, int id_segmento_ce,
                                                       int id_hoja_instruccion_doc, int id_lugar_cobro, string documento,
                                                       int id_sesion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando la Sesión de Usuario
                using (SAT_CL.Seguridad.UsuarioSesion user_sesion = new SAT_CL.Seguridad.UsuarioSesion(id_sesion))
                {
                    //Validando que la Sesión este Activa
                    if (user_sesion.habilitar && user_sesion.EstatusSesion == SAT_CL.Seguridad.UsuarioSesion.Estatus.Activo)
                    {
                        //Instanciando Hoja de Instruccion Documento
                        using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(id_hoja_instruccion_doc))
                        {
                            //Validando que exista el Documento
                            if (hid.habilitar)
                            {
                                //Declarando Variables de Obtención de Formatos
                                //(Si es original, se niega valor de copia)
                                bool bit_copia = hid.id_copia_original == 1 ? false : true;
                                //(Si es copia, se nieva valor de original)
                                bool bit_original = hid.id_copia_original == 2 ? false : true;

                                //Realizando actualización
                                result = ControlEvidenciaDocumento.InsertaControlEvidenciaDocumento(id_servicio_ce, id_servicio, id_segmento_ce, id_segmento, (byte)hid.id_tipo_documento,
                                                                        ControlEvidenciaDocumento.EstatusDocumento.Imagen_Digitalizada, hid.id_hoja_instruccion_documento, 0, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                        id_lugar_cobro, bit_original, bit_copia, hid.bit_sello, 0, "", user_sesion.id_usuario);
                                //Validando la Operación
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Parada
                                    int idRegistro = result.IdRegistro;

                                    //Instanciando Servicio Control Evidencia
                                    using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, id_servicio))
                                    {
                                        //Actualizando estatus de Control de evidencia
                                        result = ce.ActualizaEstatusGeneralServicioControlEvidencia(user_sesion.id_usuario);
                                        
                                        //Validando la Operación
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciamos Segmento para obtenener información de las paradas
                                            using (SegmentoCarga objSegmento = new SegmentoCarga(id_segmento))
                                            {
                                                //Instanciamos Parada de Inicio y Parada Fin
                                                using (Parada objParadaInicio = new Parada(objSegmento.id_parada_inicio), objParadaFin = new Parada(objSegmento.id_parada_fin))
                                                {
                                                    //Actualizando Ultima Actividad
                                                    result = user_sesion.ActualizaUltimaActividad();

                                                    //Validando Operación Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Instanciando Servicio
                                                        result = new RetornoOperacion(idRegistro);

                                                        //Añadiendo resultado a mensaje final
                                                        result = new RetornoOperacion(result.IdRegistro, string.Format("{0}: {1}", documento + " [" + objParadaInicio.descripcion + "-" + objParadaFin.descripcion
                                                        + "] ", result.Mensaje), result.OperacionExitosa);

                                                        //Completando Transacción
                                                        trans.Complete();
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No Existe el Documento de la Hoja de Instrucción");
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("La Sesión no esta Activa");
                }
            }

            //Devolviendo Resultado Obtenido
            return result.ToXMLString();
        }
    }
}
